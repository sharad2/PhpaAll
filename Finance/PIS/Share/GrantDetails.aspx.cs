using System;
using System.Web.Script.Serialization;
using System.Web.UI.WebControls;
using Eclipse.PhpaLibrary.Database.PIS;
using Eclipse.PhpaLibrary.Web;

namespace PhpaAll.PIS.Share
{
    public partial class GrantDetails : PageBase
    {
        /// <summary>
        /// The EmployeeGrantId being inserted/updated. This is set in the Inserting/Updating
        /// event.
        /// </summary>
        private EmployeeGrant _grant;

        #region Load

        /// <summary>
        /// Determine the FormView default mode from QueryString
        /// </summary>
        /// <param name="e"></param>
        /// <remarks>
        /// QueryString: EmployeeId, EG -> Edit Gratns,NG -> Insert Grant, keys: EmployeeGrantId
        /// </remarks>
        protected override void OnLoad(EventArgs e)
        {
            switch (this.Request.QueryString["Key"])
            {
                case "NG":
                    fvGrantDetails.DefaultMode = FormViewMode.Insert;
                    break;

                case "EG":
                    fvGrantDetails.DefaultMode = FormViewMode.Edit;
                    break;

                default:
                    throw new NotImplementedException();
            }
            base.OnLoad(e);
        }

        #endregion

        #region Selecting

        /// <summary>
        /// Cancel the query if the EmployeeGrantId is null during updating
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dsGrants_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            if (e.WhereParameters["EmployeeGrantId"] == null)
            {
                e.Cancel = true;
                return;
            }
        }
        #endregion

        #region Insert/Update Grant

        /// <summary>
        /// Insert/Update according to FormView DefaultMode
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <remarks>
        /// Also send success to json
        /// </remarks>
        protected void btnGrantDetails_Click(object sender, EventArgs e)
        {
            if (!btnGrantDetails.IsPageValid())
            {
                return;
            }

            switch (fvGrantDetails.CurrentMode)
            {
                case FormViewMode.Edit:
                    fvGrantDetails.UpdateItem(false);
                    break;

                case FormViewMode.Insert:
                    fvGrantDetails.InsertItem(false);
                    break;

                default:
                    throw new NotImplementedException();
            }

            if (!GrantDetails_sp.HasErrorText)
            {
                this.Response.ContentType = "application/json";
                JavaScriptSerializer ser = new JavaScriptSerializer();
                string response = ser.Serialize(_grant.EmployeeId);
                this.Response.StatusCode = 205;
                this.Response.Write(response);
                this.Response.End();
            }

        }

        #endregion

        #region Insertion

        /// <summary>
        /// Set the EmployeeGrant in order to remember the inserting employee
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dsGrants_Inserting(object sender, LinqDataSourceInsertEventArgs e)
        {
            if (e.Exception == null)
            {
                _grant = (EmployeeGrant)e.NewObject;
            }
        }

        /// <summary>
        /// If insertion has exception then keep the FormView in InsertMode
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void fvGrantDetails_ItemInserted(object sender, FormViewInsertedEventArgs e)
        {
            if (e.Exception != null)
            {
                GrantDetails_sp.AddErrorText(e.Exception.Message);
                e.ExceptionHandled = true;
                e.KeepInInsertMode = true;
            }
        }

        #endregion

        #region Updation

        /// <summary>
        ///  Set the EmployeeGrant in order to remember the updating employee
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dsGrants_Updating(object sender, LinqDataSourceUpdateEventArgs e)
        {
            _grant = (EmployeeGrant)e.NewObject;
        }

        /// <summary>
        /// If updation has exception then keep the FormView in EditMode
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void fvGrantDetails_ItemUpdated(object sender, FormViewUpdatedEventArgs e)
        {
            if (e.Exception != null)           
            {
                GrantDetails_sp.AddErrorText(e.Exception.Message);
                e.ExceptionHandled = true;
                e.KeepInEditMode = true;
            }
        }

        #endregion

    }
}
