using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Eclipse.PhpaLibrary.Database.Store;
using Eclipse.PhpaLibrary.Reporting;
using Eclipse.PhpaLibrary.Web;
using EclipseLibrary.Web.JQuery;
using EclipseLibrary.Web.JQuery.Input;


namespace Finance.Store
{
    public partial class RecieveGRN : PageBase
    {
        protected void dsReceive_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            StoreDataContext db = (StoreDataContext)dsReceive.Database;
            IQueryable<GRN> allGrn = null;
            if (!string.IsNullOrEmpty(tbGRN.Text))
            {
                allGrn = db.GRNs.Where(p => p.GRNCode == tbGRN.Text);
            }
            if (!this.IsPostBack)
            {
                string strGrnId = this.Request.QueryString["GRNId"];
                if (!string.IsNullOrEmpty(strGrnId))
                {
                    allGrn = db.GRNs.Where(p => p.GRNId == Convert.ToInt32(strGrnId));
                }
            }

            if (allGrn == null)
            {
                e.Cancel = true;
            }
            else
            {
                e.Result = from grn in allGrn
                           select grn;
            }
        }

        protected void fvReceive_ItemUpdated(object sender, FormViewUpdatedEventArgs e)
        {
            if (e.Exception == null)
            {
                string url = string.Format("CreateGRN.aspx?GRNId={0}", e.Keys["GRNId"]);
                Response.Redirect(url, true);
            }
        }

        protected void fvReceive_ItemCreated(object sender, EventArgs e)
        {
            if (fvReceive.DataItem == null)
            {
                return;
            }
            GridView gvEditGRNItems = (GridView)fvReceive.FindControl("gvEditGRNItems");
            GRN grn = (GRN)fvReceive.DataItem;
            tbGRN.Text = grn.GRNCode;
            switch (fvReceive.CurrentMode)
            {
                case FormViewMode.Edit:
                    StoreDataContext db = (StoreDataContext)dsReceive.Database;
                    var query = (from grnItems in db.GRNItems
                                 where grnItems.GRNId == grn.GRNId
                                 select new
                                 {
                                     GRNId = grnItems.GRNId,
                                     GRNItemId = grnItems.GRNItemId,
                                     ItemId = grnItems.ItemId,
                                     ItemCode = grnItems.Item.ItemCode,
                                     Description = grnItems.Item.Description,
                                     Brand = grnItems.Item.Brand,
                                     Identifier = grnItems.Item.Identifier,
                                     Size = grnItems.Item.Size,
                                     Unit = grnItems.Item.UOM.UOMCode,
                                     QtyOrdered = grnItems.OrderedQty,
                                     QtyDelivered = grnItems.ReceivedQty,
                                     AcceptedQty = grnItems.AcceptedQty,
                                     Version = grnItems.Version,
                                     Price = grnItems.Price
                                     //LandingCharges = grnItems.LandedPrice ?? 0,
                                     //TotalPrice = grnItems.Price ?? 0 + grnItems.LandedPrice ?? 0
                                 }).OrderBy(p => p.ItemCode).ThenBy(p => p.Description);
                    gvEditGRNItems.DataSource = query;
                    break;
            }
            DateTime dtCreated = Convert.ToDateTime(DataBinder.Eval(fvReceive.DataItem, "GRNCreateDate", "{0}"));

            TextBoxEx tbGRNReceiveDate = (TextBoxEx)fvReceive.FindControl("tbGRNReceiveDate");
            tbGRNReceiveDate.Validators.OfType<Date>().Single().Min =  Convert.ToInt32((dtCreated -DateTime.Today).TotalDays);
        }

        protected void btnShowReport_Click(object sender, EventArgs e)
        {
            fvReceive.DataBind();
        }

        protected void btnRecieve_Click(object sender, EventArgs e)
        {
            ButtonEx btnRecieve = (ButtonEx)sender;
            if (!btnRecieve.IsPageValid())
            {
                return;
            }
            int grnId = (int)fvReceive.DataKey["GRNId"];
            GridViewEx gvEditGRNItems = (GridViewEx)fvReceive.FindControl("gvEditGRNItems");
            if (gvEditGRNItems.SelectedIndexes != null)
            {
                using (StoreDataContext db = new StoreDataContext(ReportingUtilities.DefaultConnectString))
                {
                    var selectedKeys = (from index in gvEditGRNItems.SelectedIndexes
                                        select new
                                        {
                                            Index = index,
                                            GRNItemId = (int)gvEditGRNItems.DataKeys[index]["GRNItemId"]
                                        }).ToDictionary(p => p.GRNItemId, q => q.Index);
                    var changedItems = from grnItem in db.GRNItems
                                       where selectedKeys.Keys.Contains(grnItem.GRNItemId)
                                       select grnItem;

                    foreach (var changedItem in changedItems)
                    {
                        int rowIndex = selectedKeys[changedItem.GRNItemId];
                        GridViewRow row = gvEditGRNItems.Rows[rowIndex];
                        TextBoxEx tbAccepted = (TextBoxEx)row.FindControl("tbAccepted");
                        TextBoxEx tbDelivered = (TextBoxEx)row.FindControl("tbDelivered");
                        changedItem.AcceptedQty = Convert.ToInt32(tbAccepted.Text);
                        changedItem.ReceivedQty = Convert.ToInt32(tbDelivered.Text);    // Delivered
                    }
                    var grn = db.GRNs.Single(p => p.GRNId == grnId);
                    TextBoxEx tbGRNReceiveDate = (TextBoxEx)fvReceive.FindControl("tbGRNReceiveDate");
                    grn.GRNReceiveDate = tbGRNReceiveDate.ValueAsDate;
                    db.SubmitChanges();
                }
            }
            string url = string.Format("CreateGRN.aspx?GRNId={0}", grnId);
            Response.Redirect(url, true);
        }

        protected void gvEditGRNItems_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            switch (e.Row.RowType)
            {
                case DataControlRowType.DataRow:
                    TextBoxEx tbAccepted = (TextBoxEx)e.Row.FindControl("tbAccepted");
                    tbAccepted.Validators.OfType<Custom>().Single().Params = DataBinder.Eval(e.Row.DataItem, "ItemCode", "{0}");
                    TextBoxEx tbDelivered = (TextBoxEx)e.Row.FindControl("tbDelivered");
                    tbDelivered.Validators.OfType<Value>().Single().Max = (int?)DataBinder.Eval(e.Row.DataItem, "QtyOrdered");
                    break;
            }
        }


        protected void tbAccepted_ServerDependencyCheck(object sender, DependencyCheckEventArgs e)
        {
            GridViewEx gv = (GridViewEx)fvReceive.FindControl("gvEditGRNItems");
            GridViewRow row = (GridViewRow)e.ControlToValidate.NamingContainer;
            e.NeedsToBeValdiated = gv.SelectedIndexes.Contains(row.RowIndex);
        }

        protected void tbAccepted_ServerValidate(object sender, EclipseLibrary.Web.JQuery.Input.ServerValidateEventArgs e)
        {
            TextBoxEx tbDelivered = (TextBoxEx)e.ControlToValidate.NamingContainer.FindControl("tbDelivered");
            TextBoxEx tbAccepted = (TextBoxEx)e.ControlToValidate;
            int nDeliveredQty = Convert.ToInt32(tbDelivered.Text);
            int nAccepted = Convert.ToInt32(tbAccepted.Text);
            e.ControlToValidate.IsValid = nAccepted <= nDeliveredQty;
        }

      
    }
}
