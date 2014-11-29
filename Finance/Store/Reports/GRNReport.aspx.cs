using System;
using System.Data.Linq;
using System.Drawing;
using System.Linq;
using System.Web.UI.WebControls;
using Eclipse.PhpaLibrary.Database.Store;
using Eclipse.PhpaLibrary.Web;

namespace Finance.Store.Reports
{
    public partial class GRNReport : PageBase
    {
        /// <summary>
        /// Set the Default values of GRN for GRN code selected.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void fvMaterialReceipt_ItemCreated(object sender, EventArgs e)
        {
            if (fvMaterialReceipt.DataItem == null)
            {
                return;
            }

            StoreDataContext db = (StoreDataContext)dsMaterialReceipt.Database;
            GRN grn = (GRN)fvMaterialReceipt.DataItem;
            if (_setSrsCode)
            {
                tbGRN.Text = grn.GRNCode;
            }
            GridView gvMaterialReceipt = (GridView)fvMaterialReceipt.FindControl("gvMaterialReceipt");
            var query = (from gd in db.GRNItems
                         where gd.GRNId == grn.GRNId
                         select new
                         {
                             GRNItemId = gd.GRNItemId,
                             GrnId = gd.GRNId,
                             ItemCode = gd.Item.ItemCode,
                             ItemId = gd.ItemId,
                             Description = gd.Item.Description,
                             Brand = gd.Item.Brand,
                             Identifier = gd.Item.Identifier,
                             Size = gd.Item.Size,
                             Color = gd.Item.Color,
                             Dimension = gd.Item.Dimension,
                             ItemUnit = gd.Item.UOM.UOMCode,
                             OrderedQty = gd.OrderedQty,
                             AcceptedQty = gd.AcceptedQty,
                             //RejectedQty = (gd.OrderedQty ?? 0) - (gd.AcceptedQty ?? 0),
                             DeliveredQty = gd.ReceivedQty,
                             IssuedQty = db.SRSIssueItems.Where(p => p.GRNItemId == gd.GRNItemId).Sum(p => p.QtyIssued),
                             Price = gd.Price ?? 0,
                             LandedPrice = gd.LandedPrice ?? 0,
                             DeliveredTotal = (gd.ReceivedQty) * ((gd.Price ?? 0) + (gd.LandedPrice ?? 0)),
                             AcceptedTotal = (gd.AcceptedQty) * ((gd.Price ?? 0) + (gd.LandedPrice ?? 0))
                         }).OrderBy(p => p.ItemCode);
            gvMaterialReceipt.DataSource = query;
            if (User.Identity.IsAuthenticated)
            {
                mvEdit.ActiveViewIndex = 0;
                btnReceive.NavigateUrl = string.Format("~/Store/RecieveGRN.aspx?GRNId={0}", grn.GRNId);
                btnReceive.ToolTip = string.Format("You can not Edit GRN in Receiving Mode. You can only Accept items from Items Delivered for GRN No. {0}", grn.GRNCode);
                btnEdit.NavigateUrl = string.Format("~/Store/CreateGRN.aspx?GRNId={0}", grn.GRNId);
                btnEdit.ToolTip = string.Format("Edit GRN {0}", grn.GRNCode);
                lblPanel.Visible = true;
            }
            else
            {
                mvEdit.ActiveViewIndex = 1;
            }
        }

        protected void gvMaterialReceipt_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            GridView gv = (GridView)sender;
            switch (e.Row.RowType)
            {
                case DataControlRowType.DataRow:
                    DataControlFieldCell cell = (from DataControlFieldCell c in e.Row.Cells
                                                 where c.ContainingField.AccessibleHeaderText == "ItemCode"
                                                 select c).Single();
                    if (cell.Text == this.Request.QueryString["ItemCode"])
                    {
                        e.Row.BackColor = Color.Plum;
                    }
                    break;
            }
        }

        private bool _setSrsCode;
        protected void dsMaterialReceipt_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            StoreDataContext db = (StoreDataContext)dsMaterialReceipt.Database;

            IQueryable<GRN> allGrn = null;

            if (this.IsPostBack)
            {
                if (!string.IsNullOrEmpty(tbGRN.Text))
                {
                    allGrn = db.GRNs.Where(p => p.GRNCode == tbGRN.Text);
                }
            }
            else
            {
                string str = this.Request.QueryString["GRNId"];
                if (!string.IsNullOrEmpty(str))
                {
                    allGrn = db.GRNs.Where(p => p.GRNId == Convert.ToInt32(str));
                    _setSrsCode = true;
                }
            }
            if (allGrn == null)
            {
                e.Cancel = true;
            }
            else
            {
                DataLoadOptions dlo = new DataLoadOptions();
                dlo.LoadWith<GRN>(grn => grn.RoContractor);
                db.LoadOptions = dlo;
                e.Result = from grn in allGrn
                           select grn;
            }
        }
    }
}
