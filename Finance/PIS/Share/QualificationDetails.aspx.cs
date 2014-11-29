using System;
using System.Data.Linq;
using System.Web.Script.Serialization;
using System.Web.UI.WebControls;
using Eclipse.PhpaLibrary.Database.PIS;
using Eclipse.PhpaLibrary.Reporting;
using Eclipse.PhpaLibrary.Web;
using EclipseLibrary.Web.JQuery.Input;
using EclipseLibrary.Web.UI;

namespace Finance.PIS.Share
{
    public partial class QualificationDetails : PageBase
    {
        /// <summary>
        /// The QualificationId being inserted/updated. This is set in the Inserting/Updating
        /// event.
        /// </summary>
        private Qualification _qual;

        #region Load

        /// <summary>      
        /// Determine the FormView DefaultMode from querystring value
        /// </summary>
        /// <param name="e"></param>
        /// <remarks>
        /// QueryString: EmployeeId,QualificationId, NQ->New Qualification, EQ-> Edit Qualification
        /// </remarks>
        protected override void OnLoad(EventArgs e)
        {
            switch (this.Request.QueryString["Key"])
            {
                case "NQ":
                    fvQualificationDetails.DefaultMode = FormViewMode.Insert;
                    break;

                case "EQ":
                    fvQualificationDetails.DefaultMode = FormViewMode.Edit;
                    break;

                default:
                    break;
            }

            base.OnLoad(e);
        }

        #endregion

        #region Selecting

        /// <summary>
        /// Cancel the query if QualificationId is null while updating
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dsQualifications_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            if (e.WhereParameters["QualificationId"] == null)
            {
                e.Cancel = true;
                return;
            }
        }

        #endregion

        #region Insert/Update Button

        /// <summary>
        /// Insert/Edit Qualification and send the inserted/updated EmployeeId
        /// on success
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnQualification_Click(object sender, EventArgs e)
        {
            if (!btnQualification.IsPageValid())
            {
                return;
            }
            switch (fvQualificationDetails.CurrentMode)
            {
                case FormViewMode.Edit:
                    fvQualificationDetails.UpdateItem(false);
                    break;

                case FormViewMode.Insert:
                    fvQualificationDetails.InsertItem(false);
                    break;

                default:
                    throw new NotImplementedException();
            }

            if (!QualificationDetails_sp.HasErrorText)
            {
                this.Response.ContentType = "application/json";
                JavaScriptSerializer ser = new JavaScriptSerializer();
                string respone = ser.Serialize(_qual.EmployeeId);
                this.Response.StatusCode = 205;
                this.Response.Write(respone);
                this.Response.End();
            }

        }

        #endregion

        #region Context Creation

        protected void dsQualifications_ContextCreated(object sender, LinqDataSourceStatusEventArgs e)
        {
            DataContext db = (DataContext)e.Result;
            DataLoadOptions dlo = new DataLoadOptions();
            dlo.LoadWith<Qualification>(p => p.QualificationDivision);
            dlo.LoadWith<Qualification>(p => p.Country);
            db.LoadOptions = dlo;
        }

        #endregion

        #region Qualification Insertion

        /// <summary>
        /// Set the QualificationDivisionId by calling SetQualificationDivisionId method
        /// Also set the EmployeeId so that it can be sent as response on successful insertion
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dsQualifications_Inserting(object sender, LinqDataSourceInsertEventArgs e)
        {
            _qual = (Qualification)e.NewObject;           
            SetQualificationDivisionId();
        }

        /// <summary>
        /// In case of error in insertion keep the FormView in insert mode
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void fvQualificationDetails_ItemInserted(object sender, FormViewInsertedEventArgs e)
        {
            if (e.Exception != null)
            {
                QualificationDetails_sp.AddErrorText(e.Exception.Message);
                e.ExceptionHandled = true;
                e.KeepInInsertMode = true;
            }
        }

        #endregion

        #region Qualification Updation

        /// <summary>
        /// Set the QualificationDivisionId by calling SetQualificationDivisionId method
        /// Also set the EmployeeId so that it can be sent on successful updation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dsQualifications_Updating(object sender, LinqDataSourceUpdateEventArgs e)
        {
            _qual = (Qualification)e.NewObject;           
            SetQualificationDivisionId();
        }

        /// <summary>
        /// In case of error in updation keep the FormView in update mode
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void fvQualificationDetails_ItemUpdated(object sender, FormViewUpdatedEventArgs e)
        {
            if (e.Exception != null)
            {
                QualificationDetails_sp.AddErrorText(e.Exception.Message);
                e.ExceptionHandled = true;
                e.KeepInEditMode = true;
            }
        }

        #endregion

        #region PreRender

        /// <summary>
        /// Populate the ddlCompletionYear with last 40 years 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlCompletionYear_PreRender(object sender, EventArgs e)
        {
            DropDownListEx ddlCompletionYear = (DropDownListEx)sender;
            int presentYear = DateTime.Now.Year;
            int startYear = presentYear - 40;
            for (int i = presentYear + 10; i >= startYear; i--)
            {
                ddlCompletionYear.Items.Add(new DropDownItem
                {
                    Text = i.ToString(),
                    Value = i.ToString()
                });
            }
        }

        #endregion

        #region Helpers

        /// <summary>
        /// Insert QualificationDivision in the master if new value is supplied 
        /// </summary>
        private void SetQualificationDivisionId()
        {
            DropDownSuggest ddlQualDivision = (DropDownSuggest)fvQualificationDetails.FindControl("ddlQualDivision");
            if (!string.IsNullOrEmpty(ddlQualDivision.TextBox.Text))
            {
                QualificationDivision qualDiv = new QualificationDivision()
                {
                    DivisionName = ddlQualDivision.TextBox.Text
                };
                using (PISDataContext db = new PISDataContext(ReportingUtilities.DefaultConnectString))
                {
                    db.QualificationDivisions.InsertOnSubmit(qualDiv);
                    db.SubmitChanges();
                }

                _qual.QualificationDivisionId = qualDiv.QualificationDivisionId;
            }
        }

        #endregion

    }
}
