using System;
using System.Linq;
using System.Web.UI.WebControls;
using Eclipse.PhpaLibrary.Database.PIS;
using Eclipse.PhpaLibrary.Web;

namespace Finance.PIS.Reports
{
    public partial class ContractEnding : PageBase
    {
        protected void dsContractEnding_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            PISDataContext db = (PISDataContext)dsContractEnding.Database;
            IQueryable<Employee> query = db.Employees;

            if (!string.IsNullOrEmpty(ddlEmployeeType.Value))
            {
                query = query.Where(p => p.EmployeeTypeId == Convert.ToInt32(ddlEmployeeType.Value));
            }

            if (dtFrom.ValueAsDate.HasValue)
            {
                query = query.Where(p => p.ServicePeriods.Max(q => q.PeriodEndDate) >= Convert.ToDateTime(dtFrom.Text));
            }

            if (dtTo.ValueAsDate.HasValue)
            {
                query = query.Where(p => p.ServicePeriods.Max(q => q.PeriodEndDate) <= Convert.ToDateTime(dtTo.Text));
            }

            e.Result = from emp in query
                       where emp.ServicePeriods.Max(p => p.PeriodEndDate) != null
                       select new
                       {
                           FullName = emp.FullName,
                           JoiningDate = emp.JoiningDate,
                           PeriodEndDate = emp.ServicePeriods.Max(p => p.PeriodEndDate.Value),
                           Designation = emp.Designation,
                           DivisionName = emp.Division.DivisionName,
                           SubDivisionName = emp.SubDivision.SubDivisionName,
                           OfficeName = emp.Office.OfficeName,
                           Description = emp.EmployeeType.Description,
                           EmployeeId = emp.EmployeeId
                       };

        }
    }
}
