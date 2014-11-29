using System;
using System.Web.UI.WebControls;
using Eclipse.PhpaLibrary.Web;

namespace Finance.PIS
{
    public partial class Grants : PageBase
    {
        #region Selecting

        /// <summary>
        /// Cancel query when EmployeeId is not found in QueryString
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dsGrants_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            if (e.WhereParameters["EmployeeId"] == null)
            {
                e.Cancel = true;
            }
        }

        #endregion

        #region Deletion

        /// <summary>
        /// Show status on deletion
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dsGrants_Deleted(object sender, LinqDataSourceStatusEventArgs e)
        {
            if (e.Exception == null)
            {
                Grants_sp.AddStatusText("Item deleted successfully");
            }
            else
            {
                Grants_sp.AddErrorText(e.Exception.Message);
                e.ExceptionHandled = true;
            }
        }

        #endregion

        #region PreRender

        /// <summary>
        /// Append EmployeeId in Url of Grant remote dialog
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void _dlgAddEditGrantDetails_PreRender(object sender, EventArgs e)
        {
            _dlgAddEditGrantDetails.Ajax.Url += string.Format("?EmployeeId={0}", this.Request.QueryString["EmployeeId"]);
        }

        #endregion

    }
}
