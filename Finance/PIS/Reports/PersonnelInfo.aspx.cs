using System;
using System.Linq;
using System.Web.UI.WebControls;
using Eclipse.PhpaLibrary.Database.PIS;
using Eclipse.PhpaLibrary.Reporting;
using Eclipse.PhpaLibrary.Web;


namespace PIS.Reports
{
    public partial class PersonnelInfo : PageBase
    {
        protected void dsGrade_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            PISDataContext db = new PISDataContext(ReportingUtilities.DefaultConnectString);
            e.Result = (from emp in db.ServicePeriods
                        where emp.Grade != null
                        orderby emp.Grade
                        select emp.Grade).Distinct();
        }

        protected void dsEmployee_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            PISDataContext db = (PISDataContext)dsEmployee.Database;

            IQueryable<Employee> query = db.Employees;
            if (!string.IsNullOrEmpty(ddlEmployeeType.Value))
            {
                int empTypeId = Convert.ToInt32(ddlEmployeeType.Value);
                if (empTypeId == -1)
                {
                    query = query.Where(p => p.EmployeeTypeId == null);
                }
                else
                {
                    query = query.Where(p => p.EmployeeTypeId == empTypeId);
                }
            }

            if (!string.IsNullOrEmpty(ddlDivision.Value))
            {
                query = query.Where(p => p.DivisionId == Convert.ToInt32(ddlDivision.Value));
            }

            if (!string.IsNullOrEmpty(ddlEmployeeStatus.Value))
            {
                query = query.Where(p => p.EmployeeStatusId == Convert.ToInt32(ddlEmployeeStatus.Value));
            }

            if (!string.IsNullOrEmpty(ddlGrade.Value))
            {
                int grade = Convert.ToInt32(ddlGrade.Value);
                if (grade == -1)
                {
                    //query = query.Where(p => p.Grade == null);
                    query = query.Where(p => p.ServicePeriods.OrderByDescending(q => q.PeriodStartDate).FirstOrDefault().Grade == null);
                }
                else
                {
                    //query = query.Where(p => p.Grade == grade);
                    query = query.Where(p => p.ServicePeriods.OrderByDescending(q => q.PeriodStartDate).FirstOrDefault().Grade == grade);
                }
            }





            e.Result = from emp in query
                       join sp in db.ServicePeriods
                            on emp.EmployeeId equals sp.EmployeeId into sps
                       from sp in sps.DefaultIfEmpty()
                       .OrderByDescending(p => p.PeriodStartDate).Take(1)
                       orderby sp.Grade ?? 1000, emp.FirstName
                       select new
                       {
                           ParentOrganization = emp.ParentOrganization,
                           FirstName = emp.FirstName,
                           FullName = emp.FullName,
                           EmployeeType = emp.EmployeeType.Description,
                           //EmployeeStatus = emp.EmployeeStatus,
                           Designation = emp.Designation,
                           InitialTerm = sp.InitialTerm,
                           Employee = emp,
                           Description = emp.EmployeeType.Description,
                           JoiningDate = emp.JoiningDate,
                           DateOfIncrement = sp.DateOfIncrement,
                           DateOfRelieve = sp.PeriodEndDate,
                           ExpiryDate = sp.PeriodEndDate,
                           Division = emp.Division,
                           DivisionName = emp.Division.DivisionName,
                           PromotionDate = sp.PromotionDate,
                           Extension = sp.PromotionType.PromotionDescription,
                           ExtensionUpto = sp.ExtensionUpto,
                           Remarks = sp.Remarks,
                           DateOfBirth = emp.DateOfBirth,
                           Qualification = emp.Qualifications.OrderByDescending(p => p.CompletionYear).Take(1).Max(p => p.QualificationType),
                           Nationality = emp.IsBhutanese ? "Bhutanese" : "Foreigner",
                           ConsolidatedSalary = sp.IsConsolidated ? emp.BasicSalary : 0,
                           //.Where(p => (p.IsCurrent == true && p.IsConsolidated == true)).Max(p => p.BasicSalary),
                           Grade = sp.Grade,
                           EmployeeId = emp.EmployeeId,
                           EmployeeStatusId = emp.EmployeeStatusId
                       };



        }

    }
}
