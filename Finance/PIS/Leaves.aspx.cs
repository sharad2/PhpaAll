using System;
using System.Data.Linq;
using System.Web.UI.WebControls;
using Eclipse.PhpaLibrary.Database.PIS;
using Eclipse.PhpaLibrary.Web;

namespace PhpaAll.PIS
{
    public partial class Leaves : PageBase
    {
        #region Selecting

        /// <summary>
        /// Cancel query if EmployeeId not found in QueryString
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dsLeaveInfo_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            if (e.WhereParameters["EmployeeId"] == null)
            {
                e.Cancel = true;
            }
        }

        #endregion

        #region Context Creation

        protected void dsLeaveInfo_ContextCreated(object sender, LinqDataSourceStatusEventArgs e)
        {
            DataContext db = (DataContext)e.Result;
            DataLoadOptions dlo = new DataLoadOptions();
            dlo.LoadWith<LeaveRecord>(p => p.LeaveType);
            db.LoadOptions = dlo;
        }

        #endregion
       

        #region Deletion

        /// <summary>
        /// Show status on Deletion
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dsLeaveInfo_Deleted(object sender, LinqDataSourceStatusEventArgs e)
        {
            if (e.Exception == null)
            {
                formLeaves_btnInsert.Visible = true;
                Leaves_sp.AddStatusText("Item deleted successfully");
            }
            else
            {
                Leaves_sp.AddErrorText(e.Exception.Message);
                e.ExceptionHandled = true;
            }
        }

        #endregion

        #region PreRender

        /// <summary>
        /// Append EmployeeId in Url of Leaves remote dialog
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void _dlgAddEditLeave_PreRender(object sender, EventArgs e)
        {
            _dlgAddEditLeave.Ajax.Url += string.Format("?EmployeeId={0}", this.Request.QueryString["EmployeeId"]);
        }

        #endregion

    }
}
