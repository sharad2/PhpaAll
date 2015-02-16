using System;
using System.Linq;
using PhpaAll.Bills;
using System.Web.Services;
using Eclipse.PhpaLibrary.Database.Payroll;
using Eclipse.PhpaLibrary.Reporting;
using EclipseLibrary.Web.JQuery.Input;

namespace PhpaAll.Services
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
            using (PhpaBillsDataContext db = new PhpaBillsDataContext(ReportingUtilities.DefaultConnectString))
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


        public AutoCompleteItem ValidateDivision(string term)
        {
            int divisionId;
            try
            {
                divisionId = string.IsNullOrEmpty(term) ? 0 : Convert.ToInt32(term);
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
            using (PhpaBillsDataContext db = new PhpaBillsDataContext(ReportingUtilities.DefaultConnectString))
            {
                return (from cnt in db.Divisions
                        let text = cnt.DivisionCode + ": " + cnt.DivisionName
                        where cnt.DivisionId == divisionId || cnt.DivisionCode.Equals(divisionId.ToString())
                        orderby cnt.DivisionName
                        select new AutoCompleteItem()
                        {
                            Text = text,
                            Value = cnt.DivisionId.ToString()
                        }).FirstOrDefault();
            }
        }
        //[WebMethod]
        //public AutoCompleteItem ValidateDivision(string term)
        //{
        //    using (PayrollDataContext db = new PayrollDataContext(ReportingUtilities.DefaultConnectString))
        //    {
        //        return (from d in db.Divisions
        //                where d.DivisionId.ToString().Equals(term) || d.DivisionCode.Equals(term)
        //                select new AutoCompleteItem
        //                {
        //                    Text = string.Format("{0}: {1}", d.DivisionCode, d.DivisionName),
        //                    Value = d.DivisionId.ToString()
        //                }).FirstOrDefault();
        //    }
        //}


        [WebMethod]
        [Obsolete]
        public object[] GetBillsForDivision(string term, int divisionId)
        {
            string BillId = string.Empty;
            int index = term.IndexOf(":");
            if (index >= 0)
            {
                BillId = term.Substring(0, index);
            }
            using (PhpaBillsDataContext db = new PhpaBillsDataContext(ReportingUtilities.DefaultConnectString))
            {
                var query = (from b in db.Bills
                             where b.DivisionId == divisionId && (b.Division.DivisionName.Contains(term) ||
                                 b.BillNumber == term || b.BillNumber.StartsWith(term) || b.BillNumber == BillId ||
                                 b.Division.DivisionName.Contains(term))
                             let relevance = (b.BillNumber == term ? -10 : b.BillNumber.StartsWith(term) ? -5 : 0)
                             orderby relevance, b.BillNumber
                             select new
                             {
                                 Text = b.BillNumber + ": " + b.Division.DivisionName,
                                 Value = b.Id.ToString(),
                                 Detail = b.Division.DivisionName,
                                 Relevance = relevance
                             }).Take(20).ToArray();

                return query;
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
