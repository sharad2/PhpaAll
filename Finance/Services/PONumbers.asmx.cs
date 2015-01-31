using System;
using System.Linq;
using System.Web.Services;
using Eclipse.PhpaLibrary.Database;
using Eclipse.PhpaLibrary.Database.Store;
using Eclipse.PhpaLibrary.Reporting;
using EclipseLibrary.Web.JQuery.Input;

namespace PhpaAll.Services
{
    /// <summary>
    /// Summary description for PONumbers
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class PONumbers : System.Web.Services.WebService
    {

        [WebMethod]
        public AutoCompleteItem[] GetPoNo(string term)
        {
            using (StoreDataContext db = new StoreDataContext(ReportingUtilities.DefaultConnectString))
            {
                return (from grn in db.GRNs
                            where grn.PONumber != null && grn.PONumber != string.Empty && grn.PONumber.Contains(term)
                            orderby grn.GRNCreateDate descending
                            select new AutoCompleteItem()
                            {
                                Text = string.Format("{0}:{1}:{2}", grn.PONumber, grn.RoContractor.ContractorName, string.IsNullOrEmpty(grn.PODate.ToString()) ? "" : grn.PODate.Value.ToShortDateString() ),
                                Value = grn.PONumber
                            }).Distinct().Take(100).ToArray();
            }
        }


        /// <summary>
        /// Function is used in the ManageEmployeePeriod.aspx page for displaying the Period
        /// </summary>
        /// <param name="term"></param>
        /// <returns></returns>
        [WebMethod]
        public AutoCompleteItem[] GetPeriod(string term)
        {
            using (ReportingDataContext db = new ReportingDataContext(ReportingUtilities.DefaultConnectString))
            {
                return (from j in db.RoSalaryPeriods
                             where
                             j.Description.ToLower().Contains(term) ||
                             j.SalaryperiodCode.StartsWith(term)
                             orderby j.SalaryperiodCode
                        select new AutoCompleteItem()
                        {
                            Text = string.Format("{0}: {1:d} to {2:d}", j.SalaryperiodCode, j.SalaryPeriodStart, j.SalaryPeriodEnd),
                            Value = j.SalaryPeriodId.ToString()
                        }).Distinct().Take(100).ToArray();
            }
        }

        [WebMethod]
        public AutoCompleteItem ValidatePeriod(string term)
        {
            int salaryPeriodId;
            try
            {
                salaryPeriodId = string.IsNullOrEmpty(term) ? 0 : Convert.ToInt32(term);
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
                return (from j in db.RoSalaryPeriods
                        where
                        j.Description.ToLower().Contains(term) ||
                        j.SalaryperiodCode.StartsWith(term)
                        orderby j.SalaryperiodCode
                        select new AutoCompleteItem()
                        {
                            Text = string.Format("{0}: {1:d} to {2:d}", j.SalaryperiodCode, j.SalaryPeriodStart, j.SalaryPeriodEnd),
                            Value = j.SalaryPeriodId.ToString()
                        }).FirstOrDefault();
            }
        }
    }
}
