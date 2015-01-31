using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using Eclipse.PhpaLibrary.Database;
using Eclipse.PhpaLibrary.Database.Store;
using Eclipse.PhpaLibrary.Reporting;
using EclipseLibrary.Web.JQuery.Input;


namespace PhpaAll.Services
{
    /// <summary>
    /// Summary description for Packages
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
     [System.Web.Script.Services.ScriptService]
    public class Packages : System.Web.Services.WebService
    {

        [WebMethod]
        public AutoCompleteItem[] GetPackageNames(string term)
        {
            using (FinanceDataContext db = new FinanceDataContext(ReportingUtilities.DefaultConnectString))
            {
                var names = (from job in db.Jobs
                             where job.PackageName != null && job.PackageName != string.Empty && job.PackageName.Contains(term)
                             orderby job.PackageName
                             select new AutoCompleteItem()
                                {
                                    Value = job.PackageName,
                                    Text = job.PackageName
                                }).Distinct().Take(20).ToArray();

                return names;
            }
        }


        public AutoCompleteItem ValidatePackage(string term)
        {
            using (FinanceDataContext db = new FinanceDataContext(ReportingUtilities.DefaultConnectString))
            {
                var names = (from job in db.Jobs
                                where job.PackageName == term
                                orderby job.PackageName
                                select new AutoCompleteItem()
                                {
                                    Value=job.PackageName,
                                    Text=job.PackageName
                                }).Distinct().FirstOrDefault();

                return names;
            }
        }

    }
}
