using System;
using System.Linq;
using System.Web.UI.WebControls;
using Eclipse.PhpaLibrary.Database.MIS;
using Eclipse.PhpaLibrary.Web;
using Eclipse.PhpaLibrary.Reporting;

namespace Finance.MIS
{
    public partial class AbstractFinancialDetail : PageBase
    {
        protected new NestedMIS Master
        {
            get
            {
                return (NestedMIS)base.Master;
            }
        }

        protected void ds_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            int financial = Convert.ToInt32(ProgressFormat.FormatType.MonthlyPhysicalFinancial);
            int packageid = this.Master.PackageId;
            DateTime? selectedDate = tbMonth.ValueAsDate;
            if (selectedDate == null)
            {
                e.Cancel = true;
                return;
            }
            DateTime monthstartDate = selectedDate.Value.MonthStartDate();      // DateTime.Now.AddDays(1 - DateTime.Now.Day);
            int currentmonth = selectedDate.Value.Month;
            int currentyear = selectedDate.Value.Year;
            MISDataContext db = (MISDataContext)ds.Database;
            var query = (from act in db.Activities
                         where act.ProgressFormat.PackageId == packageid
                         && act.ProgressFormat.FormatCategoryId == Convert.ToInt32(ddlActivityCategory.Value)
                         && act.ProgressFormat.ProgressFormatType == financial
                         group act by act.ProgressFormat into g

                         select new
                         {
                             SubPackageName = g.Max(p => p.ProgressFormat.FinancialFormatName),
                             Description = g.Max(p => p.ProgressFormat.Description),
                             FinancialTarget = g.Sum(p => p.PhysicalTarget * p.BoqRate),
                             preMonthData = g.Sum(p => p.FormatActivityDetails.Where(q => q.FormatDetail.ProgressDate < monthstartDate
                                              && q.FormatDetail.ProgressFormat.PackageId == packageid)
                                              .Sum(q => q.FinancialProgressData)),

                             currentMonthData = g.Sum(p => p.FormatActivityDetails.Where(q => q.FormatDetail.ProgressFormat.PackageId == packageid
                                                           && q.FormatDetail.ProgressDate.Month == currentmonth
                                                           && q.FormatDetail.ProgressDate.Year == currentyear)
                                                              .Sum(q => q.FinancialProgressData)),


                             TotalExpenditure = (((g.Sum(p => p.FormatActivityDetails.Where(q => q.FormatDetail.ProgressDate < monthstartDate
                                                              && q.FormatDetail.ProgressFormat.PackageId == packageid)
                                                             .Sum(q => q.FinancialProgressData)))) ?? 0) +

                                                 (((g.Sum(p => p.FormatActivityDetails.Where(q => q.FormatDetail.ProgressFormat.PackageId == packageid
                                                               && q.FormatDetail.ProgressDate.Month == currentmonth
                                                               && q.FormatDetail.ProgressDate.Year == currentyear)
                                                               .Sum(q => q.FinancialProgressData)))) ?? 0)

                         });

            e.Result = query;
            this.Title += string.Format(" - {0:MMMM yyyy}", tbMonth.ValueAsDate);


        }

    }
}
