using System;
using System.Data.Linq;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Eclipse.PhpaLibrary.Database.Store;
using Eclipse.PhpaLibrary.Reporting;
using Eclipse.PhpaLibrary.Web;
using EclipseLibrary.Web.JQuery.Input;

namespace Finance.Store.Reports
{
    public partial class IssueSRS : PageBase
    {

        private bool _setSrsCode;
        protected void dsSRSIssue_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            StoreDataContext db = (StoreDataContext) dsSRSIssue.Database;

            IQueryable<SRS> allSrs = null;

            if (this.IsPostBack)
            {
                if (!string.IsNullOrEmpty(tbSRSNo.Text))
                {
                   // allSrs = db.SRS.Where(p => p.SRSCode == tbSRSNo.Text);
                    allSrs = db.SRS.Where(p => p.SRSId == Convert.ToInt32(tbSRSNo.Text));
                }
            }
            else
            {
                string str = this.Request.QueryString["SRSId"];
                if (!string.IsNullOrEmpty(str))
                {
                    allSrs = db.SRS.Where(p => p.SRSId == Convert.ToInt32(str));
                    _setSrsCode = true;
                }
            }
            if (allSrs == null)
            {
                e.Cancel = true;
            }
            else
            {
                DataLoadOptions dlo = new DataLoadOptions();
                dlo.LoadWith<SRS>(srs => srs.RoDivision1);
                dlo.LoadWith<SRS>(srs => srs.RoDivision2);
                dlo.LoadWith<SRS>(srs => srs.RoEmployee);
                dlo.LoadWith<SRS>(srs => srs.RoEmployee1);
                dlo.LoadWith<SRS>(srs => srs.RoEmployee3);
                dlo.LoadWith<SRS>(srs => srs.RoEmployee4);
                dlo.LoadWith<SRS>(srs => srs.HeadOfAccount);
                db.LoadOptions = dlo;
                e.Result = from srs in allSrs
                           select srs;
            }
        }

        /// <summary>
        /// Querying the database for SRSItems to be issued.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void fvSRSIssue_ItemCreated(object sender, EventArgs e)
        {
            if (fvSRSIssue.DataItem == null)
            {
                return;
            }

            StoreDataContext db = (StoreDataContext)dsSRSIssue.Database;
            SRS srs = (SRS)fvSRSIssue.DataItem;
            if (_setSrsCode)
            {
                //tbSRSNo.Text = srs.SRSCode;
                tbSRSNo.Text = srs.SRSId.ToString();
            }
            GridView gvSRSItems = (GridView)fvSRSIssue.FindControl("gvSRSItems");

            var query = (from srsItem in db.SRSItems
                         where srsItem.SRSId == srs.SRSId
                         orderby srsItem.Item.ItemCode
                         select new
                         {
                             SRSItemId = srsItem.SRSItemId,
                             SRSId = srsItem.SRSId,
                             ItemCode = srsItem.Item.ItemCode,
                             ItemId = srsItem.ItemId,
                             ItemDescription = srsItem.Item.Description,
                             ItemUnit = srsItem.Item.UOM.UOMCode,
                             QtyReq = srsItem.QtyRequired ?? 0,
                             QtyIssued = ((srsItem.SRSIssueItems.Sum(p => p.QtyIssued)) ?? 0),
                             Remarks = srsItem.SRSIssueItems.OrderByDescending(p => p.IssueDate).Take(1).Min(p => p.Remarks),
                             QtyAvailable = (int?)(from gi in db.GRNItems
                                             where gi.ItemId == srsItem.ItemId
                                             group gi by 1 into grp
                                             select (grp.Sum(p => p.AcceptedQty) ?? 0) - (grp.SelectMany(p => p.SRSIssueItems).Sum(p => p.QtyIssued) ?? 0)
                                  ).SingleOrDefault()
                         }).ToList();
            gvSRSItems.DataSource = query;
        }

        /// <summary>
        /// Disable the Issue text box when Quanity required is equal to Quantity issued.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        protected void gvSRSItems_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            StoreDataContext db = (StoreDataContext)dsSRSIssue.Database;
            switch (e.Row.RowType)
            {
                case DataControlRowType.DataRow:
                    TextBoxEx tbIssue = (TextBoxEx)e.Row.FindControl("tbIssue");
                    tbIssue.FriendlyName = string.Format("Issue quantity for row {0}", e.Row.RowIndex);
                    //EnhancedTextBox tbAvailable = (EnhancedTextBox)e.Row.FindControl("tbAvailable");
                    int qtyReq = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "QtyReq"));
                    int qtyIssued = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "QtyIssued"));
                    int qtyAvailable=Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "QtyAvailable"));

                    Literal litUnissue = (Literal)e.Row.FindControl("litUnissue");
                    litUnissue.Visible = qtyIssued > 0;

                    Value val = tbIssue.Validators.OfType<Value>().Single();
                    val.Max = qtyReq - qtyIssued;
                    val.Min = -qtyIssued;// Negative value for un issuing
                    //Value val1 = tbIssue.Validators.OfType<Value>().Single();
                    //val1.Max = qtyAvailable;

                    break;
            }
        }

        protected void btnShowSRS_Click(object sender, EventArgs e)
        {
            fvSRSIssue.DataBind();
        }


        /// <summary>
        /// Issuing the items which user has entered in the issue text box. 
        /// Passing values to InsertSRSIssueItem method.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnIssuesSRS_Click(object sender, EventArgs e)
        {
            ButtonEx btnIssuesSRS = (ButtonEx)sender;
            if (!btnIssuesSRS.IsPageValid())
            {
                return;
            }
            GridView gv = (GridView)fvSRSIssue.FindControl("gvSRSItems");
            using (StoreDataContext db = new StoreDataContext(ReportingUtilities.DefaultConnectString))
            {
                foreach (GridViewRow gridRow in gv.Rows)
                {
                    TextBoxEx tbIssue = (TextBoxEx)gridRow.FindControl("tbIssue");

                    TextArea tbRemarks = (TextArea)gridRow.FindControl("tbRemarks");
                    if (!string.IsNullOrEmpty(tbIssue.Text))
                    {
                        int nQuantityToIssue = Convert.ToInt32(tbIssue.Text);
                        int srsItemId = Convert.ToInt32(gv.DataKeys[gridRow.RowIndex].Values["SRSItemId"]);
                        //int itemId = Convert.ToInt32(gv.DataKeys[gridRow.RowIndex].Values["ItemId"]);
                        string remarks = tbRemarks.Text;
                        if (nQuantityToIssue > 0)
                        {
                            SRS.IssueItem(db, nQuantityToIssue, srsItemId, remarks);
                        }
                        else if (nQuantityToIssue < 0)
                        {
                            SRS.UnissueItem(db, -nQuantityToIssue, srsItemId);
                        }
                    }
                }
                db.SubmitChanges();
            }
            fvSRSIssue.DataBind();
        }
    }
}

