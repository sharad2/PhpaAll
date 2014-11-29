using System;
using System.Linq;
using System.Web.Services;
using Eclipse.PhpaLibrary.Database.Payroll;
using Eclipse.PhpaLibrary.Reporting;
using EclipseLibrary.Web.JQuery.Input;

namespace Finance.Services
{
    /// <summary>
    /// Summary description for Adjustments
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    [System.Web.Script.Services.ScriptService]
    public class Adjustments : System.Web.Services.WebService
    {

        [WebMethod]
        public AutoCompleteItem[] GetRecoveries(string term)
        {
            using (PayrollDataContext db = new PayrollDataContext(ReportingUtilities.DefaultConnectString))
            {
                return (from adj in db.Adjustments
                        let text = adj.AdjustmentCode + ": (" + adj.Description+ ")"
                        where (adj.AdjustmentCode == term || text.Contains(term) && adj.IsDeduction)
                        orderby adj.Description, adj.AdjustmentCode
                        select new AutoCompleteItem()
                        {
                            Text = text,
                            Value = adj.AdjustmentId.ToString()
                        }).Take(20).ToArray();
            }
        }

        [WebMethod]
        public AutoCompleteItem[] ValidateRecoveries(string term)
        {
            int adjustmentId;
            try
            {
                adjustmentId = string.IsNullOrEmpty(term) ? 0 : Convert.ToInt32(term);
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
            
            using (PayrollDataContext db = new PayrollDataContext(ReportingUtilities.DefaultConnectString))
            {
                return (from adj in db.Adjustments
                        let text = adj.AdjustmentCode + ": (" + adj.Description + ")"
                        where adj.AdjustmentId == adjustmentId
                        orderby adj.Description, adj.AdjustmentCode
                        select new AutoCompleteItem()
                        {
                            Text = text,
                            Value = adj.AdjustmentId.ToString()
                        }).Take(20).ToArray();
            }
        }

        /// <summary>
        /// This method is used in EmployeeAdjustment
        ///
        /// Its takes employeeid as parameter and fetches all adjustment 
        /// except those which employee has already availed
        /// </summary>
        /// <param name="term"></param>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        [WebMethod]
        public AutoCompleteItem[] GetAdjustmentCode(string term, int employeeId)
        {
            using (PayrollDataContext db = new PayrollDataContext(ReportingUtilities.DefaultConnectString))
            {
                var query = (from adj in db.Adjustments
                             where (adj.AdjustmentCode.StartsWith(term) || adj.Description.Contains(term))
                             select adj).Except
                                     (from empadj in db.EmployeeAdjustments
                                      where empadj.EmployeeId == employeeId
                                      select empadj.Adjustment
                                     ).OrderBy(p => p.AdjustmentCode);

                return (from q in query
                        select new AutoCompleteItem
                        {
                            Text = string.Format("{0}:{1}", q.AdjustmentCode, q.Description),
                            Value = q.AdjustmentId.ToString()
                        }).Take(20).ToArray();
            }

        }
        /// <summary>
        /// Its takes employeeperiodid as parameter and fetches all adjustment 
        /// except those which has already been availed in that particular SalaryPeriod
        /// </summary>
        /// <param name="term"></param>
        /// <param name="employeePeriodId"></param>
        /// <returns></returns>
        [WebMethod]
        public AutoCompleteItem[] GetAdjustmentWithoutPeriodID(string term, int employeePeriodId)
        {
            using (PayrollDataContext db = new PayrollDataContext(ReportingUtilities.DefaultConnectString))
            {
                var query = (from adj in db.Adjustments
                             where (adj.AdjustmentCode.StartsWith(term) || adj.Description.Contains(term))
                             select adj).Except
                             (from perempadj in db.PeriodEmployeeAdjustments
                              where perempadj.EmployeePeriodId == employeePeriodId
                              select perempadj.Adjustment
                             ).OrderBy(p => p.AdjustmentCode);

                return (from q in query
                        select new AutoCompleteItem
                        {
                            Text = string.Format("{0}:{1}", q.AdjustmentCode, q.Description),
                            Value = q.AdjustmentId.ToString()
                        }).Take(20).ToArray();
            }

        }

        [WebMethod]
        public AutoCompleteItem ValidateAdjustmentCode(string term)
        {
            int adjustmentId;
            try
            {
                adjustmentId = string.IsNullOrEmpty(term) ? 0 : Convert.ToInt32(term);
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

            using (PayrollDataContext db = new PayrollDataContext(ReportingUtilities.DefaultConnectString))
            {
               return (from adj in db.Adjustments
                        where adj.AdjustmentId == adjustmentId
                        select new AutoCompleteItem
                        {
                            Text = string.Format("{0}:{1}", adj.AdjustmentCode, adj.Description),
                            Value = adj.AdjustmentId.ToString()
                        }).FirstOrDefault();
            }
        }


    }
}
