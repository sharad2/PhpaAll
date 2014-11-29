using System;
using System.Linq;
using System.Web.UI.WebControls;
using Eclipse.PhpaLibrary.Database.PIS;
using Eclipse.PhpaLibrary.Web;

namespace PIS.Reports
{
    public partial class StaffStrength : PageBase
    {

        protected void dsStaff_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            DateTime? dt = dtServiceTo.ValueAsDate;
            if (dt == null)
            {
                e.Cancel = true;
                return;
            }
            PISDataContext db = (PISDataContext)dsStaff.Database;
            // An employee is active if a service period exists for him for the passed date.
            // PeriodEndDate null means that the service period lasts for ever.
            // If there are no service periods defined for an employee, then we assume he is active.
            var query = from emp in db.Employees
                        //Not Considering Service Period ,but considering only those employees with status null
                        where emp.EmployeeStatusId==null //&& !emp.ServicePeriods.Any() ||
                        ////(emp.ServicePeriods.FirstOrDefault(p => p.PeriodStartDate.Date <= dt &&
                        ////    (p.PeriodEndDate == null || p.PeriodEndDate.Value.Date >= dt)) != null ) 
                       
                        group emp by new
                        {
                            EmployeeType = emp.EmployeeType,
                            IsBhutanese = emp.IsBhutanese
                        } into grp
                        let count = grp.Count()
                        orderby count descending
                        select new
                       {
                           EmpCategory = grp.Key.EmployeeType.Description,
                           EmployeeTypeId = (int?)grp.Key.EmployeeType.EmployeeTypeId,
                           Total = count,
                           IsBhutanese = grp.Key.IsBhutanese ? 1 : 0
                       };
            e.Result = query;

        }

    }
}
