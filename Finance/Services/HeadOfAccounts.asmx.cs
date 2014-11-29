using System;
using System.Linq;
using System.Web.Services;
using Eclipse.PhpaLibrary.Reporting;
using EclipseLibrary.Web.JQuery.Input;
using EclipseLibrary.Web.UI;
using System.Collections.Generic;

namespace Finance.Services
{
    /// <summary>
    /// Summary description for HeadOfAccounts
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class HeadOfAccounts : System.Web.Services.WebService
    { 
        /// <summary>
        /// Ritesh 13 Jan 2012
        /// Showing HeadOfAccounts belonging to station of logged in user
        /// In case of administrator not considering station
        /// </summary>
        /// <param name="term"></param>
        /// <returns></returns>
        [WebMethod]
        public AutoCompleteItem[] GetHeadOfAccountForStation(string term, int station)
        {
            
            using (ReportingDataContext db = new ReportingDataContext(ReportingUtilities.DefaultConnectString))
            {
               
                return (from head in db.RoHeadHierarchies
                        where
                        ((head.DisplayName.StartsWith(term) || head.Description.Contains(term)) && (head.StationId == null|| head.StationId==station))
                        let relevance = head.DisplayName == term ? -10 : 0
                        orderby relevance, head.SortableName
                        select new AutoCompleteItem()
                        {
                            Relevance = relevance,
                            Text = head.DisplayDescription,
                            Value = head.HeadOfAccountId.ToString()
                        }).Take(20).ToArray();
                
               
            }
        }
        [WebMethod]
        public  AutoCompleteItem[]  GetHeadOfAccount(string term)
        {

            using (ReportingDataContext db = new ReportingDataContext(ReportingUtilities.DefaultConnectString))
            {

                return (from head in db.RoHeadHierarchies
                        where
                        ((head.DisplayName.StartsWith(term) || head.Description.Contains(term)))
                        let relevance = head.DisplayName == term ? -10 : 0
                        orderby relevance, head.SortableName
                        select new AutoCompleteItem()
                        {
                            Relevance = relevance,
                            Text = head.DisplayDescription,
                            Value = head.HeadOfAccountId.ToString()
                        }).Take(20).ToArray();


            }
        }

        [WebMethod]
        public AutoCompleteItem[] GetLeafHeadOfAccountsForTypes(string term)
        {

            using (ReportingDataContext db = new ReportingDataContext(ReportingUtilities.DefaultConnectString))
            {
                //return db.GetLeafHeadOfAccountsForTypes(prefixText, count,contextKey);
                return (from head in db.RoHeadHierarchies
                        where (head.DisplayName.StartsWith(term) || head.Description.Contains(term))
                        && head.CountChildren == 0
                        let relevance = head.DisplayName == term ? -10 : 0
                        orderby relevance, head.SortableName
                        select new AutoCompleteItem()
                        {
                            Relevance = relevance,
                            Text = head.DisplayDescription,
                            Value = head.HeadOfAccountId.ToString()
                        }).Take(40).ToArray();
            }
        }


        /// <summary>
        /// term can be id or code
        /// </summary>
        /// <param name="term"></param>
        /// <returns></returns>
        [WebMethod]
        public AutoCompleteItem ValidateHeadOfAccount(string term)
        {
            using (ReportingDataContext db = new ReportingDataContext(ReportingUtilities.DefaultConnectString))
            {
                IQueryable<RoHeadHierarchy> query = db.RoHeadHierarchies;
                if (term.Contains("."))
                {
                    query = query.Where(p => p.DisplayName == term);
                }
                else
                {
                    int headOfAccountId;
                    try
                    {
                        headOfAccountId = string.IsNullOrEmpty(term) ? 0 : Convert.ToInt32(term);
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
                    query = query.Where(p => p.HeadOfAccountId == headOfAccountId);
                }
                //return db.GetLeafHeadOfAccountsForTypes(prefixText, count,contextKey);
                return (from head in query
                        select new AutoCompleteItem()
                        {
                            Text = head.DisplayDescription,
                            Value = head.HeadOfAccountId.ToString()
                        }).FirstOrDefault();
            }
        }


        /// <summary>
        /// This method is used to populate the DropDownList 
        /// based on the category passed as input.
        /// 
        /// It returns an array of Head of Account Type
        /// </summary>
        /// <param name="parentKeys"></param>
        /// <returns></returns>
        [WebMethod]
        public DropDownItem[] GetAccountTypes(string[] parentKeys)
        {
            // Get a dictionary of known category/value pairs
            using (ReportingDataContext db = new ReportingDataContext(ReportingUtilities.DefaultConnectString))
            {
                return (from acctype in db.RoAccountTypes
                        where acctype.Category.Equals(parentKeys[0])
                        orderby acctype.Description, acctype.HeadOfAccountType
                        select new DropDownItem()
                        {

                            Value = acctype.HeadOfAccountType,
                            Text =  acctype.Description
                        }).ToArray();
            }
        }

        
    }
}
