using System;
using System.Linq;
using System.Web.UI.WebControls;
using Eclipse.PhpaLibrary.Database.PIS;
using Eclipse.PhpaLibrary.Web;
using Eclipse.PhpaLibrary.Reporting;

namespace Finance.PIS.Reports
{
    public partial class NextPromotionDue : PageBase
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
                .Where(p => p.ServicePeriods.OrderByDescending(q => q.PeriodStartDate).FirstOrDefault().NextPromotionDate != null);

            if (dtFrom.ValueAsDate.HasValue)
            {
                query = query.Where(p => p.ServicePeriods.OrderByDescending(q => q.PeriodStartDate).FirstOrDefault().NextPromotionDate >= Convert.ToDateTime(dtFrom.Text));
            }

            if (dtTo.ValueAsDate.HasValue)
            {
                query = query.Where(p => p.ServicePeriods.OrderByDescending(q => q.PeriodStartDate).FirstOrDefault().NextPromotionDate <= Convert.ToDateTime(dtTo.Text));
            }

            e.Result = from emp in query
                       select new
                       {
                           FullName = emp.FullName,
                           Designation = emp.Designation,
                           DivisionName = emp.Division.DivisionName,
                           SubDivisionName = emp.SubDivision.SubDivisionName,
                           OfficeName = emp.Office.OfficeName,
                           Description = emp.EmployeeType.Description,
                           NextPromotionDate = emp.ServicePeriods.OrderByDescending(p => p.PeriodStartDate).FirstOrDefault().NextPromotionDate,
                           EmployeeId = emp.EmployeeId
                       };

        }

    }
}
