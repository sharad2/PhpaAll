using System.Linq;
using System.Web.Services;
using Eclipse.PhpaLibrary.Reporting;
using EclipseLibrary.Web.UI;
using EclipseLibrary.Web.JQuery.Input;
using System;

namespace PhpaAll.Services
{
    /// <summary>
    /// Summary description for Employees
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class Employees : WebService
    {
        [WebMethod]
        public AutoCompleteItem[] GetEmployees(string term)
        {
            //int employeeId = string.IsNullOrEmpty(id) ? 0 : Convert.ToInt32(id);
            using (ReportingDataContext db = new ReportingDataContext(ReportingUtilities.DefaultConnectString))
            {
                return (from emp in db.RoEmployees
                        let text = emp.EmployeeCode + ": " + emp.FirstName + " " + emp.LastName
                        where ((emp.EmployeeCode.Equals(term) || text.Contains(term)))
                        let relevance = emp.EmployeeCode == term ? -10 : 0
                        orderby relevance,emp.EmployeeCode, emp.FirstName, emp.LastName
                        select new AutoCompleteItem()
                        {
                            Relevance = relevance,
                            Text = string.Format("{0}: {1} : {2}", emp.EmployeeCode, emp.FullName, emp.Designation),
                            Value = emp.EmployeeId.ToString()
                        }).Take(20).ToArray();
            }
        }


        [WebMethod]
        public AutoCompleteItem ValidateEmployee(string term)
        {
            int employeeId;
            try
            {
                employeeId = string.IsNullOrEmpty(term) ? 0 : Convert.ToInt32(term);
            }
            catch (OverflowException)
            {
                // Id is either too large or too small for an Int32. Ignore it.
                return null;
            }
            catch (FormatException)
            {
                // Id is not numeric. Ignore it.
                return null;
            }
            using (ReportingDataContext db = new ReportingDataContext(ReportingUtilities.DefaultConnectString))
            {
                return (from emp in db.RoEmployees
                        let text = emp.EmployeeCode + ": " + emp.FirstName + " " + emp.LastName
                        where emp.EmployeeId == employeeId || emp.EmployeeCode.Equals(employeeId.ToString())
                        select new AutoCompleteItem()
                        {
                            Text = string.Format("{0}: {1} : {2}", emp.EmployeeCode, emp.FullName, emp.Designation),
                            Value = emp.EmployeeId.ToString()
                        }).FirstOrDefault();
            }
        }
    }
}
