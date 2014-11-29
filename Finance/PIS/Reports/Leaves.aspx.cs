using System;
using System.Linq;
using System.Web.UI.WebControls;
using Eclipse.PhpaLibrary.Database.PIS;
using Eclipse.PhpaLibrary.Web;

namespace PIS.Reports
{
    public partial class Leaves : PageBase
    {
        protected void dsEmpLeave_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            PISDataContext db = (PISDataContext)dsEmpLeave.Database;
            IQueryable<LeaveRecord> query = db.LeaveRecords;

            if (dtPeriodFrom.ValueAsDate != null)
            {
                query = query.Where(p => p.LeaveStartFrom >= dtPeriodFrom.ValueAsDate);
            }

            if (dtPeriodTo.ValueAsDate != null)
            {
                query = query.Where(p => p.LeaveEndTo <= dtPeriodTo.ValueAsDate);
            }

            if (!string.IsNullOrEmpty(ddlLeaveType.Value))
            {
                query = query.Where(p => p.LeaveTypeId == Convert.ToInt32(ddlLeaveType.Value));
            }

            e.Result = from leaveRec in query
                       orderby leaveRec.ServicePeriod.Employee.FirstName
                       select new
                       {
                           LeaveRecordId = leaveRec.LeaveRecordId,
                           EmpFirstName = leaveRec.ServicePeriod.Employee.FirstName,
                           FullName = leaveRec.ServicePeriod.Employee.FullName,
                           ServicePeriodId = leaveRec.ServicePeriodId,
                           Designation = leaveRec.ServicePeriod.Designation,
                           Grade = leaveRec.ServicePeriod.Grade,
                           LeaveType = leaveRec.LeaveType,
                           JoiningDate = leaveRec.ServicePeriod.Employee.JoiningDate,
                           LeaveDescription = leaveRec.LeaveType.LeaveDescription,
                           LeaveTypeId = leaveRec.LeaveTypeId,
                           LeaveStartFrom = leaveRec.LeaveStartFrom,
                           LeaveEndTo = leaveRec.LeaveEndTo,
                           NoOfLeaves = leaveRec.NoOfLeaves,
                           Remarks = leaveRec.Remarks,
                           EmployeeId = leaveRec.ServicePeriod.EmployeeId
                       };

        }

    }
}
