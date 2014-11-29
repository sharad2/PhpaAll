using System;
using System.Data.Linq;
using System.Linq;
using System.Web.UI.WebControls;
using Eclipse.PhpaLibrary.Database.Store;
using Eclipse.PhpaLibrary.Web;

namespace Finance.Store.Reports
{
    public partial class SRSReport : PageBase
    {
        private bool _setSrsCode;
        protected void dsSRS_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            StoreDataContext db = (StoreDataContext)dsSRS.Database;

            IQueryable<SRS> allSrs = null;

            if (!string.IsNullOrEmpty(tbSRSNo.Text))
            {
                allSrs = db.SRS.Where(p => p.SRSId == Convert.ToInt32(tbSRSNo.Text));
            }

            if (!this.IsPostBack)
            {
                string str = this.Request.QueryString["SrsId"];
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
                dlo.LoadWith<SRS>(srs => srs.HeadOfAccount);
                dlo.LoadWith<SRS>(srs => srs.RoEmployee);
                dlo.LoadWith<SRS>(srs => srs.RoEmployee1);
                dlo.LoadWith<SRS>(srs => srs.RoEmployee3);
                dlo.LoadWith<SRS>(srs => srs.RoEmployee4);
                db.LoadOptions = dlo;
                e.Result = from srs in allSrs
                           select srs;
            }

        }

        protected void fvSRS_ItemCreated(object sender, EventArgs e)
        {
            if (fvSRS.DataItem == null)
            {
                return;
            }

            SRS srs = (SRS)fvSRS.DataItem;
            if (_setSrsCode)
            {
                tbSRSNo.Text = srs.SRSId.ToString();
            }
        }

        protected void dsIssueItems_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            PhpaLinqDataSource dsIssueItems = (PhpaLinqDataSource)fvSRS.FindControl("dsIssueItems");
            StoreDataContext db = (StoreDataContext)dsIssueItems.Database;

            SRS srs = (SRS)fvSRS.DataItem;

            var query = from srsItem in db.SRSItems
                        join issueItem in db.SRSIssueItems on srsItem.SRSItemId equals issueItem.SRSItemId into g
                        from issueItem in g.DefaultIfEmpty()
                        where srsItem.SRSId == srs.SRSId
                        orderby srsItem.Item.ItemCode, issueItem.IssueDate
                        select new
                        {
                            SRSId = srsItem.SRSId,
                            GRNId = (int?)issueItem.GRNItem.GRNId,
                            GRNCode = issueItem.GRNItem.GRN.GRNCode,
                            ItemCode = srsItem.Item.ItemCode,
                            IssueDate = issueItem.IssueDate,
                            Description = srsItem.Item.Description,
                            Brand = srsItem.Item.Brand,
                            Color = srsItem.Item.Color,
                            Identifier = srsItem.Item.Identifier,
                            Size = srsItem.Item.Size,
                            ItemUnit = srsItem.Item.UOM.UOMCode,
                            HeadOfAccount = srsItem.HeadOfAccount.DisplayName,
                            HOADescription = srsItem.HeadOfAccount.Description,
                            QtyRequired = srsItem.QtyRequired,
                            QtyIssued = issueItem.QtyIssued,
                            Rate = issueItem.GRNItem.Price,
                            Amount = issueItem.QtyIssued * issueItem.GRNItem.Price,
                            Remarks = issueItem.SRSItem.Remarks
                        };

           
            e.Result = query;
        }

        protected void btnRecalculate_Click(object sender, EventArgs e)
        {
            string srsId = tbSRSNo.Text;
            if (!string.IsNullOrEmpty(srsId))
            {
                SRS.RecalculateRates(Convert.ToInt32(srsId));
                dsSRS.DataBind();
            }
        }

        
    }
}
