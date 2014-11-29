using System;
using System.Data.Linq;
using System.Linq;
using System.Web.Script.Serialization;
using System.Web.UI.WebControls;
using Eclipse.PhpaLibrary.Database.PIS;
using Eclipse.PhpaLibrary.Reporting;
using Eclipse.PhpaLibrary.Web;
using EclipseLibrary.Web.JQuery.Input;

namespace Finance.PIS.Share
{
    public partial class MedicalDetails : PageBase
    {
        /// <summary>
        /// The EmployeeId being finally returned on successful insertion/updation
        /// </summary>
        private object _empId = null;

        /// <summary>
        /// The MedicalRecordId being inserted/updated. This is set in the Inserting/Updating
        /// event.
        /// </summary>
        private MedicalRecord _medRecord;

        #region Load

        /// <summary>
        /// Determine the FormView default mode from QueryString value
        /// </summary>
        /// <param name="e"></param>
        /// <remarks>
        /// QueryString:EmployeeId,MedicalRecordId, NM-> New MedicalRecord, EM-> Edit MedicalRecord
        /// </remarks>
        protected override void OnLoad(EventArgs e)
        {
            switch (this.Request.QueryString["Key"])
            {
                case "NM":
                    fvMedicalDetails.DefaultMode = FormViewMode.Insert;
                    break;

                case "EM":
                    fvMedicalDetails.DefaultMode = FormViewMode.Edit;
                    break;

                default:
                    throw new NotImplementedException();
            }
            base.OnLoad(e);
        }

        #endregion

        #region Custom Validation

        /// <summary>
        /// Ensure that OrderDate is within the ServicePeriod
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //protected void custValOrderDate_ServerValidate(object sender, EclipseLibrary.Web.JQuery.Input.ServerValidateEventArgs e)
        //{
        //    EclipseLibrary.Web.JQuery.Input.ValidationSummary valOrderDate = (EclipseLibrary.Web.JQuery.Input.ValidationSummary)fvMedicalDetails.FindControl("valOrderDate");
        //    DropDownListEx ddlServicePeriod = (DropDownListEx)fvMedicalDetails.FindControl("ddlServicePeriod");
        //    TextBoxEx tbOrderDate = (TextBoxEx)fvMedicalDetails.FindControl("tbOrderDate");

        //    using (PISDataContext db = new PISDataContext(ReportingUtilities.DefaultConnectString))
        //    {
        //        ServicePeriod sp = db.ServicePeriods
        //            .Where(p => p.ServicePeriodId == Convert.ToInt32(ddlServicePeriod.Value))
        //            .Single();
        //        string errorMessage = string.Format("{0} must be within the Service Period", tbOrderDate.FriendlyName);

        //        switch (sp.PeriodEndDate.HasValue)
        //        {
        //            case true:
        //                if ((tbOrderDate.ValueAsDate < sp.PeriodStartDate) || (tbOrderDate.ValueAsDate > sp.PeriodEndDate.Value))
        //                {
        //                    valOrderDate.ErrorMessages.Add(errorMessage);
        //                    e.ControlToValidate.IsValid = false;

        //                }
        //                break;

        //            case false:
        //                if (tbOrderDate.ValueAsDate < sp.PeriodStartDate)
        //                {
        //                    valOrderDate.ErrorMessages.Add(errorMessage);
        //                    e.ControlToValidate.IsValid = false;
        //                }
        //                break;

        //            default:
        //                break;
        //        }
        //    }
        //}

        #endregion

        #region Selecting

        /// <summary>
        ///  Cancel the query if MedicalRecordId is null while updating
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dsMedicalDetails_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            if (e.WhereParameters["MedicalRecordId"] == null)
            {
                e.Cancel = true;
                return;
            }
        }

        #endregion

        #region Insert/Update Medical Details

        /// <summary>
        ///  Insert/Update MedicalRecord and send the EmployeeId on success
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnMedicalDetails_Click(object sender, EventArgs e)
        {
            
            if (!btnMedicalDetails.IsPageValid())
            {
                return;
            }

            switch (fvMedicalDetails.CurrentMode)
            {
                case FormViewMode.Edit:
                    fvMedicalDetails.UpdateItem(false);
                    break;

                case FormViewMode.Insert:
                    fvMedicalDetails.InsertItem(false);
                    break;

                default:
                    throw new NotImplementedException();
            }

            if (!MedicalDetails_sp.HasErrorText)
            {
                this.Response.ContentType = "application/json";
                JavaScriptSerializer ser = new JavaScriptSerializer();
                string response = ser.Serialize(_empId);
                this.Response.StatusCode = 205;
                this.Response.Write(response);
                this.Response.End();
            }
        }

        #endregion

        #region Context Creation

        protected void dsMedicalDetails_ContextCreated(object sender, LinqDataSourceStatusEventArgs e)
        {
            DataContext db = (DataContext)e.Result;
            DataLoadOptions dlo = new DataLoadOptions();
            dlo.LoadWith<MedicalRecord>(p => p.FamilyMember);
            dlo.LoadWith<MedicalRecord>(p => p.Country);
            db.LoadOptions = dlo;
        }

        #endregion

        #region Insertion

        /// <summary>
        /// Set the EmployeeId inserted value so that it can be sent as response
        /// after successful insertion
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dsMedicalDetails_Inserted(object sender, LinqDataSourceStatusEventArgs e)
        {
            _medRecord = (MedicalRecord)e.Result;
            _empId = _medRecord.ServicePeriod.EmployeeId;
        }

        /// <summary>
        /// Show exception message and also keep the FormView in Insert mode when insertion fails
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void fvMedicalDetails_ItemInserted(object sender, FormViewInsertedEventArgs e)
        {
            if (e.Exception != null)
            {
                MedicalDetails_sp.AddErrorText(e.Exception.Message);
                e.ExceptionHandled = true;
                e.KeepInInsertMode = true;
            }
        }

        #endregion

        #region Updation

        /// <summary>
        /// Set EmployeeId so that it can be sent as reponse on successful updation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dsMedicalDetails_Updated(object sender, LinqDataSourceStatusEventArgs e)
        {
            if (e.Exception == null)
            {
                _medRecord = (MedicalRecord)e.Result;

                using (PISDataContext db = new PISDataContext(ReportingUtilities.DefaultConnectString))
                {
                    _empId = (from medRec in db.MedicalRecords
                              where medRec.MedicalRecordId == _medRecord.MedicalRecordId
                              && medRec.ServicePeriodId == _medRecord.ServicePeriodId
                              select medRec).Max(p => p.ServicePeriod.EmployeeId);
                }
            }
        }

        /// <summary>
        /// Show exception message and keep the FormView EditMode when updation fails
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void fvMedicalDetails_ItemUpdated(object sender, FormViewUpdatedEventArgs e)
        {
            if (e.Exception != null)
            {
                MedicalDetails_sp.AddErrorText(e.Exception.Message);
                e.ExceptionHandled = true;
                e.KeepInEditMode = true;
            }
        }

        #endregion

    }
}
