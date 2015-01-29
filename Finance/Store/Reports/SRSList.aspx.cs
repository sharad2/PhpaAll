using System;
using System.Linq;
using System.Web.UI.WebControls;
using Eclipse.PhpaLibrary.Database.Store;
using Eclipse.PhpaLibrary.Web;

namespace PhpaAll.Store.Reports
{
    public partial class SRSList : PageBase
    {

        /// <summary>
        /// Removed required from date range filter and made it dynamic
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dsBriefSRS_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            StoreDataContext db = (StoreDataContext)dsBriefSRS.Database;
            IQueryable<SRS> allSrs = db.SRS;
            DateTime? dateFrom = tbFromDate.ValueAsDate;
            if (dateFrom.HasValue)
            {
                allSrs = allSrs.Where(p => p.SRSCreateDate >= dateFrom.Value);
            }

            DateTime? dateTo = tbToDate.ValueAsDate;
            if (dateTo.HasValue)
            {
                allSrs = allSrs.Where(p => p.SRSCreateDate <= dateTo.Value);
            }

            switch (rblIssueStatus.Value)
            {
                case "N":
                    // Not yet issued
                    allSrs = allSrs.Where(q => q.SRSItems.Sum(p => p.SRSIssueItems.Count) == 0);
                    break;

                case "P":
                    // Partially issued - Issued < Required
                    allSrs = allSrs.Where(q => q.SRSItems.SelectMany(p => p.SRSIssueItems)
                        .Sum(p => p.QtyIssued) < q.SRSItems.Sum(p => p.QtyRequired));
                    break;

                case "F":
                    // Fully Issued
                    allSrs = allSrs.Where(q => q.SRSItems.SelectMany(p => p.SRSIssueItems)
                        .Sum(p => p.QtyIssued) == q.SRSItems.Sum(p => p.QtyRequired));
                    break;

                case "":
                    break;

                default:
                    throw new NotImplementedException();
            }

            if (!string.IsNullOrEmpty(tbItemCode.Text))
            {
                allSrs = allSrs.Where(p => p.SRSItems.Any(q => q.Item.ItemCode == tbItemCode.Text));
            }

            e.Result = from srs in allSrs
                       orderby srs.Created descending
                       select new
                       {
                           SRSId = srs.SRSId,
                           SRSCode = srs.SRSCode,
                           SRSCreateDate = srs.SRSCreateDate,
                           SRSFrom = srs.RoDivision1.DivisionName,
                           SRSTo = srs.RoDivision2.DivisionName,
                           IssuedTo = srs.IssuedTo,
                           ItemCount = srs.SRSItems.Count,
                           IssueCount = srs.SRSItems.SelectMany(p => p.SRSIssueItems)
                                .Select(p => p.SRSItem.ItemId)
                                .Distinct().Count()
                       };

            //dsBriefSRS.Where = string.Join("&&", whereClause.ToArray());
        }

        protected void gvSrsBrief_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            switch (e.Row.RowType)
            {
                case DataControlRowType.Header:
                    gvSrsBrief.Caption = string.Format("List of SRS created from {0:d MMMM yyyy} to {1:d MMMM yyyy}",
                        tbFromDate.ValueAsDate, tbToDate.ValueAsDate);
                    break;
            }
        }
    }
}
