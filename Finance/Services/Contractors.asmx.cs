using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using EclipseLibrary.Web.UI;
using Eclipse.PhpaLibrary.Reporting;
using EclipseLibrary.Web.JQuery.Input;

namespace Finance.Services
{
    /// <summary>
    /// Summary description for Contractors
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class Contractors : WebService
    {
        private readonly ReportingDataContext _db;
        public Contractors()
        {
            _db = new ReportingDataContext(ReportingUtilities.DefaultConnectString);
            _db.ObjectTrackingEnabled = false;
        }

        [WebMethod]
        public AutoCompleteItem[] GetContractors(string term)
        {
            //int contractorId = string.IsNullOrEmpty(id) ? 0 : Convert.ToInt32(id);
            using (ReportingDataContext db = new ReportingDataContext(ReportingUtilities.DefaultConnectString))
            {
                return (from cnt in db.RoContractors
                        let text = cnt.ContractorCode + ": " + cnt.ContractorName
                        where (cnt.ContractorCode == term ||
                                text.Contains(term))
                        let relevance = cnt.ContractorCode == term ? -10 : 0
                        orderby relevance, cnt.ContractorName
                        select new AutoCompleteItem()
                        {
                            Relevance = relevance,
                            Text = text,
                            Value = cnt.ContractorId.ToString()
                        }).Take(20).ToArray();
           }
        }

        [WebMethod]
        public AutoCompleteItem ValidateContractor(string term)
        {
            int contractorId;
            try
            {
                contractorId = string.IsNullOrEmpty(term) ? 0 : Convert.ToInt32(term);
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
                return (from cnt in db.RoContractors
                        let text = cnt.ContractorCode + ": " + cnt.ContractorName
                        where cnt.ContractorId == contractorId || cnt.ContractorCode.Equals(contractorId.ToString())
                        orderby cnt.ContractorName
                        select new AutoCompleteItem()
                        {
                            Text = text,
                            Value = cnt.ContractorId.ToString()
                        }).FirstOrDefault();
            }
        }

        [WebMethod]
        public object[] GetJobsForDivision(string term, int divisionId)
        {
            string jobCode = string.Empty;
            int index = term.IndexOf(":");
            if (index >= 0)
            {
                jobCode = term.Substring(0, index);
            }
            var query = (from j in _db.RoJobs
                         where j.DivisionId == divisionId && (j.Description.Contains(term) ||
                             j.JobCode == term || j.JobCode.StartsWith(term) || j.JobCode == jobCode ||
                             j.RoContractor.ContractorName.Contains(term))
                         let relevance = (j.JobCode == term ? -10 : j.JobCode.StartsWith(term) ? -5 : 0)
                         orderby relevance, j.JobCode
                         select new 
                         {
                             Text = j.JobCode + ": " + j.Description,
                             Value = j.JobId.ToString(),
                             Detail = j.RoContractor.ContractorName,
                             Relevance = relevance,
                             
                             ContractorText = string.Format("{0}: {1}", j.RoContractor.ContractorCode,j.RoContractor.ContractorName),
                             ContractorValue = j.RoContractor.ContractorId.ToString(),
                             HeadOfAccountText = string.Format("{0}: {1}", j.RoHeadHierarchy.DisplayName,j.RoHeadHierarchy.Description),
                             HeadOfAccountValue = j.RoHeadHierarchy.HeadOfAccountId.ToString()
                         }).Take(20).ToArray();

            return query;
        }
        [WebMethod]
        public object[] GetJobsForContractor(string term, int contractorId)
        {
            string jobCode = string.Empty;
            int index = term.IndexOf(":");
            if (index >= 0)
            {
                jobCode = term.Substring(0, index);
            }
            var query = (from j in _db.RoJobs
                         where j.ContractorId == contractorId && (j.Description.Contains(term) ||
                             j.JobCode == term || j.JobCode.StartsWith(term) || j.JobCode == jobCode ||
                             j.RoContractor.ContractorName.Contains(term))
                         let relevance = (j.JobCode == term ? -10 : j.JobCode.StartsWith(term) ? -5 : 0)
                         orderby relevance, j.JobCode
                         select new
                         {
                             Text = j.JobCode + ": " + j.Description,
                             Value = j.JobId.ToString(),
                             Detail = j.RoContractor.ContractorName,
                             Relevance = relevance,

                             ContractorText = string.Format("{0}: {1}", j.RoContractor.ContractorCode, j.RoContractor.ContractorName),
                             ContractorValue = j.RoContractor.ContractorId.ToString(),
                             HeadOfAccountText = string.Format("{0}: {1}", j.RoHeadHierarchy.DisplayName, j.RoHeadHierarchy.Description),
                             HeadOfAccountValue = j.RoHeadHierarchy.HeadOfAccountId.ToString()
                         }).Take(20).ToArray();

            return query;
        }
        [WebMethod]
        public object ValidateJobForDivision(string term, int divisionId)
        {
            int jobId;
            try
            {
                jobId = string.IsNullOrEmpty(term) ? 0 : Convert.ToInt32(term);
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
            var query = (from j in _db.RoJobs
                         let text = j.JobCode + ": " + j.Description + " (" + j.RoContractor.ContractorName + ")"
                         where
                         j.DivisionId == divisionId && 
                         ( j.JobId == jobId || j.JobCode.Equals(jobId.ToString()) )
                         select new
                         {
                             Text = j.JobCode + ": " + j.Description,
                             Value = j.JobId.ToString(),

                             ContractorText = string.Format("{0}: {1}", j.RoContractor.ContractorCode, j.RoContractor.ContractorName),
                             ContractorValue = j.RoContractor.ContractorId.ToString(),
                             HeadOfAccountText = string.Format("{0}: {1}", j.RoHeadHierarchy.DisplayName, j.RoHeadHierarchy.Description),
                             HeadOfAccountValue = j.RoHeadHierarchy.HeadOfAccountId.ToString()
                         }
                         ).FirstOrDefault();

            return query;
        }

        [WebMethod]
        public object ValidateJobForContractor(string term, int contractorId)
        {
            int jobId;
            try
            {
                jobId = string.IsNullOrEmpty(term) ? 0 : Convert.ToInt32(term);
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
            var query = (from j in _db.RoJobs
                         let text = j.JobCode + ": " + j.Description + " (" + j.RoContractor.ContractorName + ")"
                         where
                         j.ContractorId == contractorId &&
                         (j.JobId == jobId || j.JobCode.Equals(jobId.ToString()))
                         select new
                         {
                             Text = j.JobCode + ": " + j.Description,
                             Value = j.JobId.ToString(),

                             ContractorText = string.Format("{0}: {1}", j.RoContractor.ContractorCode, j.RoContractor.ContractorName),
                             ContractorValue = j.RoContractor.ContractorId.ToString(),
                             HeadOfAccountText = string.Format("{0}: {1}", j.RoHeadHierarchy.DisplayName, j.RoHeadHierarchy.Description),
                             HeadOfAccountValue = j.RoHeadHierarchy.HeadOfAccountId.ToString()
                         }
                         ).FirstOrDefault();

            return query;
        }


        [WebMethod]
        public AutoCompleteItem[] GetJobList(string term)
        {
            //int jobId = string.IsNullOrEmpty(id) ? 0 : Convert.ToInt32(id);
            var query = (from j in _db.RoJobs
                         let text = j.JobCode + ": " + j.Description + " (" + j.RoContractor.ContractorName + ")"
                         where (text.Contains(term) ||
                             j.JobCode == term ||
                             j.RoContractor.ContractorName.Contains(term))
                         let relevance = (j.JobCode == term ? -10 : j.JobCode.StartsWith(term) ? -5 : 0)
                         orderby relevance,j.JobCode
                         select new AutoCompleteItem()
                         {
                             Relevance = relevance,
                             Text = text,
                             Value = j.JobId.ToString()
                         }
                         ).Take(20).ToArray();

            return query;
        }

        [WebMethod]
        public AutoCompleteItem ValidateJob(string term)
        {
            int jobId;
            try
            {
                jobId = string.IsNullOrEmpty(term) ? 0 : Convert.ToInt32(term);
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
            var query = (from j in _db.RoJobs
                         let text = j.JobCode + ": " + j.Description + " (" + j.RoContractor.ContractorName + ")"
                         where j.JobId == jobId || j.JobCode.Equals(jobId.ToString())
                         select new AutoCompleteItem
                         {
                             Text = text,
                             Value = j.JobId.ToString(),
                         }
                         ).FirstOrDefault();

            return query;
        }


        protected override void Dispose(bool disposing)
        {
            _db.Dispose();
            base.Dispose(disposing);
        }
    }
}
