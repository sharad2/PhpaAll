using System;
using System.Linq;
using System.Web.Services;
using System.Web.UI.WebControls;
using Eclipse.PhpaLibrary.Database.MIS;
using Eclipse.PhpaLibrary.Reporting;
using Eclipse.PhpaLibrary.Web;
using EclipseLibrary.Web.JQuery;

namespace PhpaAll.MIS
{
    public partial class ProgressFormatManager : PageBase
    {
        protected override void OnLoad(EventArgs e)
        {
            JQueryScriptManager.Current.RegisterScripts(ScriptTypes.WebMethods);
        }
        protected void dsFormats_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            MISDataContext db = (MISDataContext)dsFormats.Database;
            var query = from fmt in db.ProgressFormats
                        orderby fmt.Package.PackageName, fmt.ProgressFormatType, fmt.ProgressFormatName
                        select new
                        {
                            ProgressFormatId = fmt.ProgressFormatId,
                            ProgressFormatName = fmt.ProgressFormatName,
                            FinancialFormatName = fmt.FinancialFormatName,
                            FormatDisplayName = fmt.Description,                      
                            PackageName = fmt.Package.PackageName,
                            PackageDescription = fmt.Package.Description,
                            ProgressFormatTypeAsEnum = fmt.FormatTypeDisplayName,
                            FormatCategoryDescription = fmt.FormatCategory.Description,
                            Activitycounts = fmt.Activities.Count(),
                            MinProgressDate = (DateTime?)fmt.FormatDetails.Min(p => p.ProgressDate),
                            MaxProgressDate = (DateTime?)fmt.FormatDetails.Max(p => p.ProgressDate),
                            CountProgressDate = fmt.FormatDetails.Count,
                            DataActivityCount = fmt.FormatDetails.SelectMany(p => p.FormatActivityDetails)
                                .Select(p => p.ActivityId).Distinct().Count()
                        };
            e.Result = query;
        }

        [WebMethod]
        public static void DeleteFormat(int ProgressFormatId)
        {
            using (MISDataContext db = new MISDataContext(ReportingUtilities.DefaultConnectString))
            {
                ProgressFormat toDelete = db.ProgressFormats.Single(p => p.ProgressFormatId == ProgressFormatId);
                db.ProgressFormats.DeleteOnSubmit(toDelete);
                db.SubmitChanges();

            }
        }
    }


}