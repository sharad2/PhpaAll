using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Eclipse.PhpaLibrary.Database.Store;
using Eclipse.PhpaLibrary.Web;

namespace Finance.Store.Reports
{
    public partial class DivisionSRS : PageBase
    {
        [Flags]
        private enum QueryStringFlags
        {
            Default = 0x0,
            ItemId = 0x2,
            SRSId = 0x4,
            GrnId = 0x8
        }

        private QueryStringFlags _flags;

        protected void dsIssueItems_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            StoreDataContext db = (StoreDataContext)dsIssueItems.Database;

            IQueryable<SRSIssueItem> allIssues = db.SRSIssueItems;

            string str;

            if (!string.IsNullOrEmpty(tbSRSNo.Text))
            {
                allIssues = allIssues.Where(p => p.SRSItem.SRS.SRSCode == tbSRSNo.Text);
            }

            if (!this.IsPostBack)
            {
                str = this.Request.QueryString["ItemId"];
                if (!string.IsNullOrEmpty(str))
                {
                    allIssues = allIssues.Where(p => p.SRSItem.ItemId == Convert.ToInt32(str));
                    _flags |= QueryStringFlags.ItemId;
                }
                string str1 = this.Request.QueryString["SRSId"];
                if (!string.IsNullOrEmpty(str1))
                {
                    allIssues = allIssues.Where(p => p.SRSItem.SRSId == Convert.ToInt32(str1));
                    _flags |= QueryStringFlags.SRSId;
                }
                string str2 = this.Request.QueryString["GrnId"];
                if (!string.IsNullOrEmpty(str2))
                {
                    allIssues = allIssues.Where(p => p.GRNItem.GRNId == Convert.ToInt32(str2));
                    _flags |= QueryStringFlags.GrnId;
                }
            }

            if (!string.IsNullOrEmpty(tbItem.Text))
            {
                allIssues = allIssues.Where(p => p.SRSItem.Item.ItemCode == tbItem.Text);
            }

            if (!string.IsNullOrEmpty(ddlDivision.Value))
            {
                allIssues = allIssues.Where(p => p.SRSItem.SRS.SRSFrom == Convert.ToInt32(ddlDivision.Value));
                string divsionName=string.Empty;
                divsionName = (from q in db.RoDivisions
                              where q.DivisionId == Convert.ToInt32(ddlDivision.Value)
                              select q.DivisionName).SingleOrDefault();
                gvIssueItems.Caption = string.Format("Items issued for {0} from {1:dd MMMM yyyy} to {2:dd MMMM yyyy}",
                    divsionName, tbFromDate.ValueAsDate, tbToDate.ValueAsDate);
            }
            else
            {
                gvIssueItems.Caption = string.Format("List of Items issued from {0:dd MMMM yyyy} to {1:dd MMMM yyyy}",
                    tbFromDate.ValueAsDate, tbToDate.ValueAsDate);
            }

            if (tbFromDate.ValueAsDate != null)
            {
                allIssues = allIssues.Where(p => p.IssueDate >= tbFromDate.ValueAsDate);
            }

            if (tbToDate.ValueAsDate != null)
            {
                allIssues = allIssues.Where(p => p.IssueDate <= tbToDate.ValueAsDate);
            }

            if (!string.IsNullOrEmpty(tbGrnCode.Value))
            {
                allIssues = allIssues.Where(p => p.GRNItem.GRN.GRNCode.Equals(tbGrnCode.Value));
            }

            e.Result = from srsIssue in allIssues
                       orderby srsIssue.IssueDate descending, srsIssue.SRSItem.SRS.SRSCode
                       select new
                       {
                           SRSId = srsIssue.SRSItem.SRSId,
                           ItemId = srsIssue.SRSItem.ItemId,
                           DivisionId = srsIssue.SRSItem.SRS.SRSFrom,
                           SRSNo = srsIssue.SRSItem.SRS.SRSCode,
                           IssueDate = srsIssue.IssueDate,
                           IssuedTo = srsIssue.SRSItem.SRS.IssuedTo,
                           ItemCode = srsIssue.SRSItem.Item.ItemCode,
                           ItemDescription = srsIssue.SRSItem.Item.Description,
                           IssueQty = srsIssue.QtyIssued ?? 0,
                           SRSFrom = srsIssue.SRSItem.SRS.RoDivision1.DivisionName,
                           Price = (srsIssue.GRNItem.Price ?? 0) + (srsIssue.GRNItem.LandedPrice ?? 0),
                           TotalPrice = ((srsIssue.GRNItem.Price ?? 0) + (srsIssue.GRNItem.LandedPrice ?? 0)) * (srsIssue.QtyIssued ?? 0),
                           GRNCode = srsIssue.GRNItem.GRN.GRNCode,
                           GRNId = srsIssue.GRNItem.GRNId
                       };
            

        }

        protected void gvIssueItems_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            switch (e.Row.RowType)
            {
                case DataControlRowType.DataRow:
                    if (_flags != QueryStringFlags.Default)
                    {
                        if ((_flags & QueryStringFlags.ItemId) == QueryStringFlags.ItemId)
                        {
                            tbItem.Text = DataBinder.Eval(e.Row.DataItem, "ItemCode", "{0}");
                        }
                        if ((_flags & QueryStringFlags.SRSId) == QueryStringFlags.SRSId)
                        {
                            tbSRSNo.Text = DataBinder.Eval(e.Row.DataItem, "SRSNo", "{0}");
                        }
                        if ((_flags & QueryStringFlags.GrnId) == QueryStringFlags.GrnId)
                        {
                            tbGrnCode.Text = DataBinder.Eval(e.Row.DataItem, "GRNCode", "{0}");
                        }
                        _flags = QueryStringFlags.Default;
                    }
                    break;
            }
        }

        
    }
}
