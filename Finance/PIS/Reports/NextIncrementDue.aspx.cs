using System;
using System.Linq;
using System.Web.UI.WebControls;
using Eclipse.PhpaLibrary.Database.PIS;
using Eclipse.PhpaLibrary.Web;
using Eclipse.PhpaLibrary.Reporting;

namespace Finance.PIS.Reports
{
    public partial class NextIncrementDue : PageBase
    {
        protected override void OnLoad(EventArgs e)
        {
            if (!IsPostBack)
            {
                dtFrom.Text = DateTime.Now.MonthStartDate().AddMonths(1).ToShortDateString();
                dtTo.Text = DateTime.Now.MonthEndDate().AddMonths(1).ToShortDateString();
            }
            base.OnLoad(e);
        }

        protected void ds_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            PISDataContext db = (PISDataContext)ds.Database;
            IQueryable<Employee> query = db.Employees
                .Where(p => p.ServicePeriods.OrderByDescending(q => q.PeriodStartDate).FirstOrDefault().DateOfNextIncrement != null);

            if (dtFrom.ValueAsDate.HasValue)
            {
                query = query.Where(p => p.ServicePeriods.OrderByDescending(q => q.PeriodStartDate).FirstOrDefault().DateOfNextIncrement >= Convert.ToDateTime(dtFrom.Text));
            }

            if (dtTo.ValueAsDate.HasValue)
            {
                query = query.Where(p => p.ServicePeriods.OrderByDescending(q => q.PeriodStartDate).FirstOrDefault().DateOfNextIncrement <= Convert.ToDateTime(dtTo.Text));
            }

            e.Result = from emp in query
                       let sp = emp.ServicePeriods.OrderByDescending(p => p.PeriodStartDate)
                                .FirstOrDefault()
                       orderby sp.DateOfNextIncrement
                       select new
                       {
                           FullName = emp.FullName,
                           Designation = emp.Designation,
                           DivisionName = emp.Division.DivisionName,
                           SubDivisionName = emp.SubDivision.SubDivisionName,
                           OfficeName = emp.Office.OfficeName,
                           Description = emp.EmployeeType.Description,
                           //DateOfNextIncrement = emp.ServicePeriods.OrderByDescending(p => p.PeriodStartDate)
                           //     .FirstOrDefault().DateOfNextIncrement,
                           EmployeeId = emp.EmployeeId,
                           DateOfNextIncrement = sp.DateOfNextIncrement,
                           MinPayScaleAmount = sp.MinPayScaleAmount,
                           IncrementAmount = sp.IncrementAmount,
                           MaxPayScaleAmount = sp.MaxPayScaleAmount,
                           BeforeIncrement = sp.BasicSalary,
                           AfterIncrement = sp.BasicSalary + sp.IncrementAmount ?? 0,
                       };
            this.Title += string.Format(" - {0:d MMM yyyy} to {1:d MMM yyyy}",dtFrom.ValueAsDate,dtTo.ValueAsDate);

        }

    }
}
