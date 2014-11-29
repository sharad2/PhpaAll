using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Eclipse.PhpaLibrary.Database.Store;
using Eclipse.PhpaLibrary.Web;
using EclipseLibrary.Web.UI;
using EclipseLibrary.Web.JQuery.Input;

namespace Finance.Store.Reports
{
    /// <summary>
    /// Pass ItemId
    /// </summary>
    public partial class ItemLedger : PageBase
    {

        private int? _openingBalance;
        protected void dsMaterialIssue_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            if (!btnGo.IsPageValid())
            {
                e.Cancel = true;
                return;
            }
            StoreDataContext db = (StoreDataContext)dsMaterialIssue.Database;

            int itemId = Convert.ToInt32(tbItem.Value);
            var receivedItems = db.GRNItems.Where(p => p.ItemId == itemId);
            var issuedItems = db.SRSIssueItems.Where(p => p.SRSItem.ItemId == itemId);
            DateTime? dateFrom = tbFromDate.ValueAsDate;
            if (dateFrom != null)
            {
                receivedItems = receivedItems.Where(p => p.GRN.GRNReceiveDate >= dateFrom.Value);
                issuedItems = issuedItems.Where(p => p.IssueDate >= dateFrom.Value);
            }

            var itemInfo = from item in db.Items
                           where item.ItemId == itemId
                           select new
                           {
                               Item = item,
                               OpeningBalance = (int?)0
                           };
            var oldReceipts = from grnitem in db.GRNItems
                              where grnitem.ItemId == itemId && grnitem.GRN.GRNReceiveDate < dateFrom
                              group grnitem by grnitem.Item into g
                              select new
                              {
                                  Item = g.Key,
                                  OpeningBalance = g.Sum(p => p.ReceivedQty)
                              };
            var oldIssues = from issueitem in db.SRSIssueItems
                            where issueitem.SRSItem.ItemId == itemId && issueitem.IssueDate < dateFrom
                            group issueitem by issueitem.SRSItem.Item into g
                            select new
                            {
                                Item = g.Key,
                                OpeningBalance = -g.Sum(p => p.QtyIssued)
                            };
            var openingInfo = (from item in oldReceipts.Concat(oldIssues).Concat(itemInfo)
                              group item by item into g
                              select new
                              {
                                  ItemCode = g.Key.Item.ItemCode,
                                  ItemDescription = g.Key.Item.Description,
                                  ItemUom = g.Key.Item.UOM.UOMCode,
                                  OpeningBalance = g.Sum(p => p.OpeningBalance)
                              });
            _openingBalance = openingInfo.Sum(p=>p.OpeningBalance);
            gvMaterialIssue.Caption = string.Format("{0}:{3} <br /> Item Ledger from {1:d MMMM yyyy} to {2:d MMMM yyyy}",
                openingInfo.Max(p=>p.ItemCode), tbFromDate.ValueAsDate, tbToDate.ValueAsDate, openingInfo.Max(p=>p.ItemDescription));
            gvMaterialIssue.Columns.OfType<MultiBoundField>()
                .Single(p => p.AccessibleHeaderText == "TransactionQuantity")
                .HeaderToolTip = openingInfo.Max(p=>p.ItemUom);
            // Now construct the query
            DateTime? dateTo = tbToDate.ValueAsDate;
            if (dateTo != null)
            {
                receivedItems = receivedItems.Where(p => p.GRN.GRNReceiveDate <= dateTo.Value);
                issuedItems = issuedItems.Where(p => p.IssueDate <= dateTo.Value);
            }
            var receipts = from gi in receivedItems
                           select new
                           {
                               TransactionDate = gi.GRN.GRNReceiveDate,
                               GRNNo = gi.GRN.GRNCode,
                               SRSNo = "",
                               GRNId = gi.GRNId,
                               SRSId = 0,
                               TransactionQuantity = gi.AcceptedQty,
                               AcceptedRate = (gi.Price ?? 0) + (gi.LandedPrice ?? 0),
                               Amount = gi.AcceptedQty * ((gi.Price ?? 0) + (gi.LandedPrice ?? 0)),
                           };

            var issues = from issueItem in issuedItems
                         select new
                         {
                             TransactionDate = issueItem.IssueDate,
                             GRNNo = issueItem.GRNItem.GRN.GRNCode,
                             SRSNo = issueItem.SRSItem.SRS.SRSCode,
                             GRNId = issueItem.GRNItem.GRNId,
                             SRSId = issueItem.SRSItem.SRSId,
                             TransactionQuantity = -issueItem.QtyIssued,
                             AcceptedRate = (issueItem.GRNItem.Price ?? 0) + (issueItem.GRNItem.LandedPrice ?? 0),
                             Amount = -issueItem.QtyIssued * ((issueItem.GRNItem.Price ?? 0) + (issueItem.GRNItem.LandedPrice ?? 0)),
                         };
            var query3 = receipts.Concat(issues).OrderBy(p => p.TransactionDate).ThenBy(p => p.GRNNo).ThenBy(p => p.SRSNo);
            e.Result = query3;

        }

        protected void gvMaterialIssue_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            switch (e.Row.RowType)
            {
                case DataControlRowType.DataRow:
                    string srs = DataBinder.Eval(e.Row.DataItem, "SRSNo", "{0}");
                    if (!string.IsNullOrEmpty(srs))
                    {
                        e.Row.Font.Italic = true;
                    }
                    break;
            }
        }

        protected void gvMaterialIssue_DataBound(object sender, EventArgs e)
        {
            if (_openingBalance == null)
            {
                return;
            }
            lblOpeningBalance.Text = string.Format("Opening Balance = {0:N0}", _openingBalance);
            lblOpeningBalance.Visible = true;

            var col = gvMaterialIssue.Columns.OfType<MultiBoundField>()
                .Single(p => p.AccessibleHeaderText == "TransactionQuantity");
            decimal totalQuantity;
            if (col.SummaryValues == null)
            {
                totalQuantity = 0;
            }
            else
            {
                totalQuantity = col.SummaryValues[0] ?? 0;
            }
            lblClosingBalance.Text = string.Format("Closing Balance = {0:N0}", _openingBalance.Value + totalQuantity);
            lblClosingBalance.Visible = true;
        }
    }
}
