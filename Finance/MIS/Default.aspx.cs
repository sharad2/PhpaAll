using System;
using System.Linq;
using System.Web.UI.WebControls;
using Eclipse.PhpaLibrary.Database.MIS;
using Eclipse.PhpaLibrary.Web;
using System.Web.UI;

namespace PhpaAll.MIS
{
    /// <summary>
    /// TODO: Expect PackageId to be passed in query string
    /// </summary>
    public partial class Default2 : PageBase
    {
        public new NestedMIS Master
        {
            get
            {
                return ((NestedMIS)(base.Master));
            }
        }

        protected void dsBuzz_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            MISDataContext db = (MISDataContext)dsFormats.Database;
            var query = (from actdet in db.FormatActivityDetails
                         group actdet by new
                         {
                             Date = (actdet.Modified ?? actdet.Created).Value.Date,
                             ProgressFormat = actdet.FormatDetail.ProgressFormat
                         } into g
                         orderby g.Key.ProgressFormat.Package.Description, g.Key.Date descending, g.Key.ProgressFormat.ProgressFormatName
                         select new
                         {
                             Date = g.Key.Date,
                             MinPhysicalProgressDate = (DateTime?)g.Where(p => p.PhysicalProgressData != null).Min(p => p.FormatDetail.ProgressDate.Date),
                             MaxPhysicalProgressDate = (DateTime?)g.Where(p => p.PhysicalProgressData != null).Max(p => p.FormatDetail.ProgressDate.Date),
                             CountPhysicalProgressDate = g.Where(p => p.PhysicalProgressData != null)
                                .Select(p => p.FormatDetail.ProgressDate.Date).Distinct().Count(),
                             MinFinancialProgressDate = (DateTime?)g.Where(p => p.FinancialProgressData != null).Min(p => p.FormatDetail.ProgressDate.Date),
                             MaxFinancialProgressDate = (DateTime?)g.Where(p => p.FinancialProgressData != null).Max(p => p.FormatDetail.ProgressDate.Date),
                             CountFinancialProgressDate = g.Where(p => p.FinancialProgressData != null)
                                .Select(p => p.FormatDetail.ProgressDate.Date).Distinct().Count(),
                             PackageId = g.Key.ProgressFormat.PackageId,
                             PackageDescription = g.Key.ProgressFormat.Package.Description,
                             ProgressFormatId = g.Key.ProgressFormat.ProgressFormatId,
                             PhysicalFormatCode = g.Key.ProgressFormat.FormatCode,
                             PhysicalFormatName = g.Key.ProgressFormat.ProgressFormatName,
                             PhysicalActivityCount = g.Where(p => p.PhysicalProgressData != null).Select(p => p.ActivityId).Distinct().Count(),
                             FinancialFormatName = g.Key.ProgressFormat.FinancialFormatName,
                             FinancialActivityCount = g.Where(p => p.FinancialProgressData != null).Select(p => p.ActivityId).Distinct().Count(),
                             FormatDescription = g.Key.ProgressFormat.Description
                         }).Take(20);
            e.Result = query;
        }

        protected void gvBuzz_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            switch (e.Row.RowType)
            {
                case DataControlRowType.DataRow:
                    HyperLink hlFormat = (HyperLink)e.Row.FindControl("hlFormat");
                    HyperLink hlFinancialFormat = (HyperLink)e.Row.FindControl("hlFinancialFormat");
                    int packageId = (int)DataBinder.Eval(e.Row.DataItem, "PackageId");
                    if (packageId == this.Master.PackageId)
                    {
                        // Show the link
                        hlFormat.NavigateUrl = string.Format(hlFormat.NavigateUrl,
                            DataBinder.Eval(e.Row.DataItem, "PhysicalFormatCode"),
                            DataBinder.Eval(e.Row.DataItem, "ProgressFormatId"),
                            DataBinder.Eval(e.Row.DataItem, "MinPhysicalProgressDate"));
                        hlFinancialFormat.NavigateUrl = string.Format(hlFormat.NavigateUrl,
                            DataBinder.Eval(e.Row.DataItem, "ProgressFormatId"),
                            DataBinder.Eval(e.Row.DataItem, "MinFinancialProgressDate"));
                    }
                    else
                    {
                        hlFormat.NavigateUrl = string.Empty;
                        hlFinancialFormat.NavigateUrl = string.Empty;
                    }
                    break;
                case DataControlRowType.EmptyDataRow:
                    break;
                case DataControlRowType.Footer:
                    break;
                case DataControlRowType.Header:
                    break;
                case DataControlRowType.Pager:
                    break;
                case DataControlRowType.Separator:
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// This method use for following purpose
        /// 1. Here bind the repeater control. Each row is an accordion header
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dsFormats_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            MISDataContext db = (MISDataContext)dsFormats.Database;

            // latestDate is the date on which the data was last entered. Time component is truncated.
            var query = from pf in db.ProgressFormats
                        where pf.PackageId == this.Master.PackageId
                        let target = pf.Activities.Sum(p => p.PhysicalTarget * (p.FinalRate ?? p.ProvisionalRate ?? p.BoqRate))
                        let actual = pf.FormatDetails.SelectMany(p => p.FormatActivityDetails)
                            .Sum(p => p.PhysicalProgressData * (p.Activity.FinalRate ?? p.Activity.ProvisionalRate ?? p.Activity.BoqRate))
                        let latestDate = (DateTime?)pf.FormatDetails.SelectMany(p => p.FormatActivityDetails)
                            .Where(p => p.PhysicalProgressData != null)
                            .Select(p => p.Modified ?? p.Created)
                            .Where(p => p != null)
                            .Select(p => p.Value.Date)
                            .Max()
                        select new
                        {
                            ProgressFormatType = (int)pf.ProgressFormatType,
                            ProgressFormatId = pf.ProgressFormatId,
                            FormatDisplayName = pf.FormatTypeDisplayName,
                            FormatCode = pf.FormatCode,
                            ProgressFormatName = pf.ProgressFormatName,
                            Description = pf.Description,
                            FinancialTarget = target,
                            FinancialActual = actual,
                            CompletionRatio = actual / target,
                            DaysAgo = (latestDate == null ? null : (double?)(DateTime.Today - latestDate.Value).TotalDays),
                            LastDataEntryDate = (DateTime?)latestDate
                        };

            var query2 = from pf in db.ProgressFormats
                         where pf.PackageId == this.Master.PackageId &&
                             pf.FinancialFormatName != null
                         let target = pf.Activities.Sum(p => p.PhysicalTarget * p.BoqRate)
                         let actual = pf.FormatDetails.SelectMany(p => p.FormatActivityDetails).Sum(p => p.FinancialProgressData)
                         let latestDate = (DateTime?)pf.FormatDetails.SelectMany(p => p.FormatActivityDetails)
                             .Where(p => p.FinancialProgressData != null)
                             .Select(p => p.Modified ?? p.Created)
                             .Where(p => p != null)
                             .Select(p => p.Value.Date)
                             .Max()
                         select new
                         {
                             ProgressFormatType = 5,
                             ProgressFormatId = pf.ProgressFormatId,
                             FormatDisplayName = "Monthly Financial",
                             FormatCode = "MonthlyFinancial",
                             ProgressFormatName = pf.FinancialFormatName,
                             Description = pf.Description,
                             FinancialTarget = target,
                             FinancialActual = actual,
                             CompletionRatio = actual / target,
                             DaysAgo = (latestDate == null ? null : (double?)(DateTime.Today - latestDate.Value).TotalDays),
                             LastDataEntryDate = (DateTime?)latestDate
                         };
            e.Result = query.ToList().Concat(query2.ToList()).OrderBy(p => p.ProgressFormatName);
        }

    }
}
