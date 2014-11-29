using System;
using System.Data.Linq;
using System.Web.UI.WebControls;
using Eclipse.PhpaLibrary.Database.PIS;
using Eclipse.PhpaLibrary.Web;

namespace Finance.PIS
{
    public partial class MedicalAllowance : PageBase
    {
        #region Selecting

        /// <summary>
        /// Cancel query when EmployeeId is null in QueryString
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dsMedicalAllowance_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            if (e.WhereParameters["EmployeeId"] == null)
            {
                e.Cancel = true;
            }
        }

        #endregion

        #region Context Creation

        protected void dsMedicalAllowance_ContextCreated(object sender, LinqDataSourceStatusEventArgs e)
        {
            DataContext db = (DataContext)e.Result;
            DataLoadOptions dlo = new DataLoadOptions();
            dlo.LoadWith<MedicalRecord>(p => p.FamilyMember);
            dlo.LoadWith<MedicalRecord>(p => p.Country);
            db.LoadOptions = dlo;
        }

        #endregion

        #region Deletion

        /// <summary>
        /// Show status on Deletion
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dsMedicalAllowance_Deleted(object sender, LinqDataSourceStatusEventArgs e)
        {
            if (e.Exception == null)
            {
                MedicalAllowance_sp.AddStatusText("Item deleted successfully");
            }
            else
            {
                MedicalAllowance_sp.AddErrorText(e.Exception.Message);
                e.ExceptionHandled = true;
            }
        }

        #endregion

        #region PreRender

        /// <summary>
        /// Append EmployeeId as QueryString in the Url of MedicalAllowance remote dialog
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void _dlgAddEditMedicalDetails_PreRender(object sender, EventArgs e)
        {
            _dlgAddEditMedicalDetails.Ajax.Url += string.Format("?EmployeeId={0}", this.Request.QueryString["EmployeeId"]);
        }

        #endregion

    }
}
