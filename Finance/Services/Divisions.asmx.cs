using System;
using System.Linq;
using System.Web.Services;
using Eclipse.PhpaLibrary.Database.Payroll;
using Eclipse.PhpaLibrary.Reporting;
using EclipseLibrary.Web.JQuery.Input;

namespace Finance.Services
{
    /// <summary>
    /// Summary description for Divisions
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    [System.Web.Script.Services.ScriptService]
    public class Divisions : System.Web.Services.WebService
    {

        /// <summary>
        /// Method is created for the InsertVoucher Page as User want auto-complete so that he can enter text 
        /// </summary>
        /// <param name="term"></param>
        /// <returns></returns>
        [WebMethod]
        public AutoCompleteItem[] GetDivisions(string term)
        {
            using (PayrollDataContext db = new PayrollDataContext(ReportingUtilities.DefaultConnectString))
            {
                return (from d in db.Divisions
                        where (d.DivisionCode.ToString().StartsWith(term) || d.DivisionName.ToUpper().Contains(term.ToUpper()))
                        orderby d.DivisionCode
                        select new AutoCompleteItem
                        {
                            Text = string.Format("{0}: {1}", d.DivisionCode, d.DivisionName),
                            Value = d.DivisionId.ToString()
                        }).Take(40).ToArray();
            }
        }

        [WebMethod]
        public AutoCompleteItem ValidateDivision(string term)
        {
            using (PayrollDataContext db = new PayrollDataContext(ReportingUtilities.DefaultConnectString))
            {
                return (from d in db.Divisions
                        where d.DivisionId.ToString().Equals(term) || d.DivisionCode.Equals(term)
                        select new AutoCompleteItem
                        {
                            Text = string.Format("{0}: {1}", d.DivisionCode, d.DivisionName),
                            Value = d.DivisionId.ToString()
                        }).FirstOrDefault();
            }
        }


        [WebMethod]
        public AutoCompleteItem[] GetVoucherPayeeList(string term)
        {
            using (ReportingDataContext db = new ReportingDataContext(ReportingUtilities.DefaultConnectString))
            {
                return (from v in db.RoVouchers
                        where v.PayeeName.Contains(term) &&
                             v.VoucherDate >= DateTime.Today.AddMonths(-6)
                             orderby v.PayeeName
                             select new AutoCompleteItem
                             {
                                 Text= v.PayeeName,
                                 Value = v.PayeeName
                             }).Distinct().Take(20).ToArray();
            }
        }
    }
}
