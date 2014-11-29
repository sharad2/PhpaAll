using System;
using System.Linq;
using System.Web.UI.WebControls;
using Eclipse.PhpaLibrary.Database.PIS;
using Eclipse.PhpaLibrary.Web;

namespace PIS.Reports
{
    public partial class ServiceStatus : PageBase
    {
        protected void dsEmploymentStatus_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            PISDataContext db = (PISDataContext)dsEmploymentStatus.Database;

            IQueryable<Employee> query = db.Employees;

            if (dtFrom.ValueAsDate != null)
            {
                query = query.Where(p => p.DateOfRelieve <= dtFrom.ValueAsDate);
            }

            if (!string.IsNullOrEmpty(ddlEmployeeStatus.Value))
            {
                query = query.Where(p => p.EmployeeStatusId == Convert.ToInt32(ddlEmployeeStatus.Value));
            }


            
            var result = from emp in query
                       orderby emp.DateOfRelieve descending
                       select new
                       {
                           FullName = emp.FullName,
                           Designation = emp.Designation,
                           JoiningDate = emp.JoiningDate,
                           EmployeeStatusType = emp.EmployeeStatus.EmployeeStatusType,
                           DateOfRelieve = emp.DateOfRelieve,
                           LeavingReason = emp.LeavingReason,
                           RelieveOrderNo = emp.RelieveOrderNo,
                           DivisionName = emp.Division.DivisionName,
                           Remarks = emp.Remarks,
                           EmployeeId = emp.EmployeeId
                       };

            var status = (from q in result
                          select q.EmployeeStatusType).FirstOrDefault();
            gvEmploymentStatus.Caption = string.Format("Details of {0} employees", status);

            e.Result = result;
        }
    }
}
