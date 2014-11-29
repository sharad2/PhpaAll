using System;
using System.Linq;
using System.Web.UI.WebControls;
using Eclipse.PhpaLibrary.Database.MIS;
using Eclipse.PhpaLibrary.Reporting;
using Eclipse.PhpaLibrary.Web;

namespace Finance.MIS
{
    //Query String :PackageId
    public partial class AbstractFinancialProgress : PageBase
    {
        protected new NestedMIS Master
        {
            get
            {
                return (NestedMIS)base.Master;
            }
        }
        /// <summary>
        /// This method use for following purpose
        /// 1.Bind the gridviewex.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ds_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            if (!btnSubmit.IsPageValid())
            {
                e.Cancel = true;
                return;
            }

            MISDataContext db = (MISDataContext)ds.Database;
            DateTime toDate;
            if (string.IsNullOrEmpty(tbMonth.Text))
            {
                toDate = DateTime.Today;
            }
            else
            {
                toDate = tbMonth.ValueAsDate.Value;
            }

            var query = (from ac in db.Activities
                         where ac.ProgressFormat.PackageId == this.Master.PackageId &&
                               ac.ProgressFormat.FormatCategoryId != null                                
                         group ac by ac.ProgressFormat.FormatCategory into g
                         select new
                         {
                             ToDate = toDate.MonthEndDate(),
                             Key = g.Key,
                             FormatCategoryId = (int?)g.Key.FormatCategoryId,
                             Packageid = this.Master.PackageId,
                             Description = g.Key.Description,
                             FinancialTarget = g.Sum(p => p.BoqRate * p.PhysicalTarget),
                             currentMonthData = g.SelectMany(p => p.FormatActivityDetails)
                                 .Where(p => p.FormatDetail.ProgressFormat.PackageId == this.Master.PackageId
                                        && p.FormatDetail.ProgressDate >= toDate.MonthStartDate()
                                        && p.FormatDetail.ProgressDate <= toDate.MonthEndDate())
                                 .Sum(p => p.FinancialProgressData),
                             TotalExpenditure = g.SelectMany(p => p.FormatActivityDetails)
                                 .Where(p => p.FormatDetail.ProgressDate < toDate.MonthEndDate()
                                      && p.FormatDetail.ProgressFormat.PackageId == this.Master.PackageId)
                                  .Sum(p => p.FinancialProgressData),
                             PreMonthData = g.SelectMany(p => p.FormatActivityDetails)
                                 .Where(q => q.FormatDetail.ProgressDate < toDate.MonthStartDate() && q.FormatDetail.ProgressFormat.PackageId == this.Master.PackageId)
                                 .Sum(p => p.FinancialProgressData),
                         });



            e.Result = query;
            this.Title += string.Format(" - {0:MMMM yyyy}", toDate);

        }

    }
}
