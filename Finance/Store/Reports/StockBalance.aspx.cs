using System;
using System.Data.Linq;
using System.Linq;
using System.Web.UI.WebControls;
using Eclipse.PhpaLibrary.Database.Store;
using Eclipse.PhpaLibrary.Web;

namespace Finance.Store.Reports
{
    public partial class StockBalance : PageBase
    {
        protected void dsStockBalance_Load(object sender, EventArgs e)
        {
            StoreDataContext db = (StoreDataContext)dsStockBalance.Database;
            DataLoadOptions dlo = new DataLoadOptions();
            dlo.LoadWith<GRN>(grn => grn.GRNItems);
            dlo.LoadWith<GRN>(grn => grn.RoContractor);
            dlo.LoadWith<GRNItem>(gd => gd.Item);
            dlo.LoadWith<Item>(item => item.ItemCategory);
            dlo.LoadWith<Item>(item => item.UOM);
            db.LoadOptions = dlo;
        }


        private class StockBalanceRow
        {
            public Item StockItem { get; set; }
            public int AcceptedQuantity { get; set; }
            public decimal? TransactionValue { get; set; }
            public int ReceivedQuantity { get; set; }
            public int IssuedQuantity { get; set; }

            public decimal? AcceptedValue { get; set; }

            public decimal? IssuedValue { get; set; }
        }

        /// <summary>
        /// Querying data for Grid. Creating Dynamic where clause.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dsStockBalance_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            StoreDataContext db = (StoreDataContext)this.dsStockBalance.Database;

            DateTime selectedDate = tbDate.ValueAsDate.Value;
            var allReceipts = from gi in db.GRNItems
                              where gi.GRN.GRNReceiveDate <= selectedDate && gi.AcceptedQty != null
                              select new StockBalanceRow
                              {
                                  StockItem = gi.Item,
                                  AcceptedQuantity = gi.AcceptedQty.Value,
                                  TransactionValue = ((gi.Price ?? 0) + (gi.LandedPrice ?? 0)) * (gi.AcceptedQty ?? 0),
                                  ReceivedQuantity = gi.ReceivedQty ?? 0,
                                  IssuedQuantity = 0,
                                  AcceptedValue = ((gi.Price ?? 0) + (gi.LandedPrice ?? 0)) * (gi.AcceptedQty ?? 0),
                                  IssuedValue = 0
                              };
            var allIssues = from ii in db.SRSIssueItems
                            where ii.IssueDate <= selectedDate
                            select new StockBalanceRow
                            {
                                StockItem = ii.SRSItem.Item,
                                AcceptedQuantity = 0,       //-srsIssueItem.QtyIssued.Value,
                                TransactionValue = 0 - ((ii.GRNItem.Price ?? 0) + (ii.GRNItem.LandedPrice ?? 0)) * ii.QtyIssued,
                                ReceivedQuantity = ii.QtyIssued.Value * 0,
                                IssuedQuantity = ii.QtyIssued ?? 0,
                                AcceptedValue = 0,
                                IssuedValue = ((ii.GRNItem.Price ?? 0) + (ii.GRNItem.LandedPrice ?? 0)) * ii.QtyIssued,
                            };

            int? categoryId;
            if (string.IsNullOrEmpty(ddlCategory.Value))
            {
                categoryId = null;
            }
            else
            {
                categoryId = Convert.ToInt32(ddlCategory.Value);
            }
            e.Result = (from row in allReceipts.Concat(allIssues)
                        where (categoryId == null || row.StockItem.ItemCategoryId == categoryId)
                        //&& (tbItem.SelectedId == null || row.StockItem.ItemId == tbItem.SelectedId)
                        group row by row.StockItem into g
                        orderby g.Key.ItemCategory.ItemCategoryCode, g.Key.ItemCategory.Description, g.Key.ItemCode
                        select new
                        {
                            StockItem = g.Key,
                            ItemId = g.Key.ItemId,
                            AcceptedQuantity = g.Sum(p => p.AcceptedQuantity),
                            TransactionValue = g.Sum(p => p.TransactionValue),
                            ReceivedQuantity = g.Sum(p => p.ReceivedQuantity),
                            IssuedQuantity = g.Sum(p => p.IssuedQuantity),
                            IntransitQuantity = g.Sum(p => p.ReceivedQuantity) - g.Sum(p => p.AcceptedQuantity),
                            StoreQuantity = g.Sum(p => p.AcceptedQuantity) - g.Sum(p => p.IssuedQuantity),
                            FromDate = selectedDate.AddMonths(-3),
                            ToDate = selectedDate,
                            AcceptedValue = g.Sum(p => p.AcceptedValue),
                            IssuedValue = g.Sum(p => p.IssuedValue)
                        }).Where(p => p.IntransitQuantity > 0 || p.StoreQuantity > 0);
            this.Title += string.Format(" As On {0:d MMMM yyyy}", tbDate.ValueAsDate);
        }

        /// <summary>
        /// Set the caption of the grid according to the Parameters passed or seleced.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvStockBalance_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            switch (e.Row.RowType)
            {
               case DataControlRowType.Footer:
                    // Prevents printing of the footer on each page
                    e.Row.TableSection = TableRowSection.TableBody;
                    break;
            }
        }
    }
}
