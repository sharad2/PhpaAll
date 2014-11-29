using System;
using System.Data.Linq;
using System.Linq;
using System.Web.UI.WebControls;
using Eclipse.PhpaLibrary.Database.PIS;
using Eclipse.PhpaLibrary.Web;

namespace Finance.PIS.Reports
{
    public partial class StaffDivisions : PageBase
    {
        protected void dsStaffDivisions_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            PISDataContext db = (PISDataContext)dsStaffDivisions.Database;
            DataLoadOptions dlo = new DataLoadOptions();
            dlo.LoadWith<Employee>(p => p.Division);
            dlo.LoadWith<Employee>(p => p.SubDivision);
            dlo.LoadWith<Employee>(p => p.Office);
            dlo.LoadWith<Employee>(p => p.EmployeeType);
            dlo.LoadWith<Employee>(p => p.BloodGroup);
            dlo.LoadWith<Employee>(p => p.MaritalStatus);
            dlo.LoadWith<Employee>(p => p.EmployeeStatus);
            db.LoadOptions = dlo;


            DateTime? dt = dtUptill.ValueAsDate;


            IQueryable<Employee> query = db.Employees
                .Where(p => !p.ServicePeriods.Any() ||(p.ServicePeriods
                .FirstOrDefault(q => q.PeriodStartDate.Date <= dt
                && (q.PeriodEndDate == null || q.PeriodEndDate.Value.Date >= dt)) != null));

            if (!string.IsNullOrEmpty(ddlDivision.Value))
            {
                query = query.Where(p => p.DivisionId == Convert.ToInt32(ddlDivision.Value));
            }

            e.Result = query;
        }
    }
}
