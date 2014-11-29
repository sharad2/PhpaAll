using System;
using System.Configuration;
using System.Linq;
using System.Web.Services;
using Eclipse.PhpaLibrary.Database.MIS;
using Eclipse.PhpaLibrary.Web;
using EclipseLibrary.Web.JQuery;
using System.Web.UI.WebControls;
using EclipseLibrary.Web.UI;

namespace Finance.MIS
{
    /// <summary>
    /// ActivityId can be passed in query string
    /// </summary>   
    public partial class ActivityList : PageBase
    {
        protected override void OnLoad(EventArgs e)
        {
            JQueryScriptManager.Current.RegisterScripts(ScriptTypes.WebMethods);
        }

        /// <summary>
        /// Delete the activity selected by user in the hower menu.       
        /// </summary>
        /// <param name="ActivityId"></param>
        [WebMethod]
        public static void DeleteActivity(int ActivityId)
        {
            string connectString = ConfigurationManager.ConnectionStrings["default"].ConnectionString;
            using (MISDataContext db = new MISDataContext(connectString))
            {
                Activity toDelete = db.Activities.Single(p => p.ActivityId == ActivityId);
                db.Activities.DeleteOnSubmit(toDelete);
                db.SubmitChanges();
            }

        }
        /// <summary>
        /// This method use for following purpose 
        /// 1.set querystring values in dialog url. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvActivityList_DataBound(object sender, EventArgs e)
        {
            dlgActivity.Ajax.Url += string.Format("?ProgressFormatId={0}", ddlProgressformat.Value);

        }
        decimal _grandTotal = 0;

        int _totalColumnIndex;
        protected void gvActivityList_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            switch (e.Row.RowType)
            {
                case DataControlRowType.Header:
                    _totalColumnIndex = gvActivityList.Columns.OfType<MultiBoundField>()
                        .Select((p,index)=> p.AccessibleHeaderText=="Amount"?index : -1).First(p=> p>=0);
                    break;

                case DataControlRowType.DataRow:
                    
                    Activity activity = (Activity)e.Row.DataItem;
                    decimal? total = activity.BoqRate * activity.PhysicalTarget;
                    e.Row.Cells[_totalColumnIndex].Text= string.Format("{0:N2}",total);
                    _grandTotal += total??0;
                    if (activity.IsTopLevel)
                    {
                        e.Row.CssClass += " ui-priority-primary";
                    }
                    break;
                case DataControlRowType.Footer:
                    e.Row.Cells[_totalColumnIndex].Text = string.Format("{0:N2}",_grandTotal);
                    break;
               default:
                    break;
            }

            
        }

    }
}
