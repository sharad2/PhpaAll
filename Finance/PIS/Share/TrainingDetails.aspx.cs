using System;
using System.Data.Linq;
using System.Linq;
using System.Web.Script.Serialization;
using System.Web.UI.WebControls;
using Eclipse.PhpaLibrary.Database.PIS;
using Eclipse.PhpaLibrary.Reporting;
using Eclipse.PhpaLibrary.Web;
using EclipseLibrary.Web.JQuery.Input;

namespace PhpaAll.PIS.Share
{
    public partial class TrainingDetails : PageBase
    {
        /// <summary>
        /// The EmployeeId being finally returned on successful insertion/updation
        /// </summary>
        private object _empId = null;

        /// <summary>
        /// The TrainingId being inserted/updated. This is set in the Inserting/Updating
        /// event.
        /// </summary>
        private Training _trn;

        #region Load

        /// <summary>
        /// Determine the FormView default mode from QueryString value
        /// </summary>
        /// <param name="e"></param>
        /// <remarks>
        /// QueryString:EmployeeId,TrainingId, NT-> New Training, ET-> Edit Training
        /// </remarks>
        protected override void OnLoad(EventArgs e)
        {
            switch (this.Request.QueryString["Key"])
            {
                case "NT":
                    fvTrainingDetails.DefaultMode = FormViewMode.Insert;
                    break;

                case "ET":
                    fvTrainingDetails.DefaultMode = FormViewMode.Edit;
                    break;

                default:
                    throw new NotImplementedException();
            }

            base.OnLoad(e);
        }

        #endregion

        #region Custom Validation

        /// <summary>
        /// Ensure TrainingStartFrom and TrainingEndTo is within the ServicePeriod
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //protected void custValTrainingPeriod_ServerValidate(object sender, EclipseLibrary.Web.JQuery.Input.ServerValidateEventArgs e)
        //{
        //    Custom ct = (Custom)sender;
        //    EclipseLibrary.Web.JQuery.Input.ValidationSummary valTrainingDetails = (EclipseLibrary.Web.JQuery.Input.ValidationSummary)fvTrainingDetails.FindControl("valTrainingDetails");
        //    DropDownListEx ddlServicePeriod = (DropDownListEx)fvTrainingDetails.FindControl("ddlServicePeriod");
        //    TextBoxEx tbTrainingStartFrom = (TextBoxEx)fvTrainingDetails.FindControl("tbTrainingStartFrom");
        //    TextBoxEx tbTrainingEndTo = (TextBoxEx)fvTrainingDetails.FindControl("tbTrainingEndTo");
        //    string errorMessage = string.Empty;

        //    using (PISDataContext db = new PISDataContext(ReportingUtilities.DefaultConnectString))
        //    {
        //        ServicePeriod sp = db.ServicePeriods
        //            .Where(p => p.ServicePeriodId == Convert.ToInt32(ddlServicePeriod.Value))
        //            .Single();

        //        switch (ct.ID)
        //        {
        //            case "custValTrainingStartFrom":
        //                //Ensure TrainingStartFrom is within the ServicePeriod
        //                errorMessage = string.Format("{0} must be within the Service Period", tbTrainingStartFrom.FriendlyName);
        //                if (sp.PeriodEndDate.HasValue)
        //                {
        //                    if ((tbTrainingStartFrom.ValueAsDate < sp.PeriodStartDate) || (tbTrainingStartFrom.ValueAsDate > sp.PeriodEndDate.Value))
        //                    {
        //                        ValidateLeave(e, valTrainingDetails, errorMessage);
        //                    }
        //                }
        //                else
        //                {
        //                    if (tbTrainingStartFrom.ValueAsDate < sp.PeriodStartDate)
        //                    {
        //                        ValidateLeave(e, valTrainingDetails, errorMessage);
        //                    }
        //                }

        //                break;

        //            case "custValTrainingEndTo":
        //                //Ensure TrainingEndTo is within the ServicePeriod
        //                errorMessage = string.Format("{0} must be within the Service Period", tbTrainingEndTo.FriendlyName);
        //                if (sp.PeriodEndDate.HasValue)
        //                {
        //                    if ((tbTrainingEndTo.ValueAsDate < sp.PeriodStartDate) || (tbTrainingEndTo.ValueAsDate > sp.PeriodEndDate.Value))
        //                    {
        //                        ValidateLeave(e, valTrainingDetails, errorMessage);
        //                    }
        //                }
        //                else
        //                {
        //                    if ((tbTrainingEndTo.ValueAsDate < sp.PeriodStartDate))
        //                    {
        //                        ValidateLeave(e, valTrainingDetails, errorMessage);
        //                    }
        //                }

        //                break;

        //            default:
        //                break;
        //        }


        //    }

        //}

        //private static void ValidateLeave(EclipseLibrary.Web.JQuery.Input.ServerValidateEventArgs e, EclipseLibrary.Web.JQuery.Input.ValidationSummary valLeaveDetails, string errorMessage)
        //{
        //    valLeaveDetails.ErrorMessages.Add(errorMessage);
        //    e.ControlToValidate.IsValid = false;
        //}

        #endregion

        #region Selecting

        /// <summary>
        /// Cancel the query if TrainingId is null while updating
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dsTrainingDetails_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            if (e.WhereParameters["TrainingId"] == null)
            {
                e.Cancel = true;
                return;
            }
        }

        #endregion

        #region Insert/Update button

        /// <summary>
        /// Insert/Update Training and send the EmployeeId on success
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnTraining_Click(object sender, EventArgs e)
        {
            ButtonEx btnTraining = (ButtonEx)sender;
            if (!btnTraining.IsPageValid())
            {
                return;
            }

            switch (fvTrainingDetails.CurrentMode)
            {
                case FormViewMode.Edit:
                    fvTrainingDetails.UpdateItem(false);
                    break;

                case FormViewMode.Insert:
                    fvTrainingDetails.InsertItem(false);
                    break;

                default:
                    throw new NotImplementedException();
            }

            if (!TrainingDetails_sp.HasErrorText)
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

        protected void dsTrainingDetails_ContextCreated(object sender, LinqDataSourceStatusEventArgs e)
        {
            DataContext db = (DataContext)e.Result;
            DataLoadOptions dlo = new DataLoadOptions();
            dlo.LoadWith<Training>(p => p.TrainingType);
            dlo.LoadWith<Training>(p => p.Country);
            db.LoadOptions = dlo;
        }

        #endregion

        #region Training Insertion

        /// <summary>
        /// Ensure that the Training periods does not overlap while inserting
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dsTrainingDetails_Inserting(object sender, LinqDataSourceInsertEventArgs e)
        {
            DropDownListEx ddlServicePeriod = (DropDownListEx)fvTrainingDetails.FindControl("ddlServicePeriod");
            TextBoxEx tbTrainingStartFrom = (TextBoxEx)fvTrainingDetails.FindControl("tbTrainingStartFrom");
            TextBoxEx tbTrainingEndTo = (TextBoxEx)fvTrainingDetails.FindControl("tbTrainingEndTo");
            using (PISDataContext db = new PISDataContext(ReportingUtilities.DefaultConnectString))
            {
                int trainingCount = (from trn in db.Trainings
                                     where trn.ServicePeriodId == Convert.ToInt32(ddlServicePeriod.Value)
                                     select trn).Count();

                if (trainingCount > 0)
                {
                    Training trnRec = db.Trainings
                                                .Where(p => p.ServicePeriodId == Convert.ToInt32(ddlServicePeriod.Value))
                                                .OrderByDescending(p => p.TrainingStartFrom)
                                                .Take(1)
                                                .Single();

                    if (tbTrainingStartFrom.ValueAsDate < trnRec.TrainingEndTo.Value)
                    {
                        throw new NotImplementedException(string.Format("Please enter {0} greater than previous {1}", tbTrainingStartFrom.FriendlyName, tbTrainingEndTo.FriendlyName));
                    }
                }
            }
        }

        /// <summary>
        /// Set the EmployeeId inserted value so that it can be sent as response
        /// after successful insertion
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dsTrainingDetails_Inserted(object sender, LinqDataSourceStatusEventArgs e)
        {
            if (e.Exception == null)
            {
                _trn = (Training)e.Result;
                _empId = _trn.ServicePeriod.EmployeeId;
            }
        }

        /// <summary>
        /// Show exception message and also keep the FormView in Insert mode when insertion fails
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void fvTrainingDetails_ItemInserted(object sender, FormViewInsertedEventArgs e)
        {
            if (e.Exception != null)
            {
                TrainingDetails_sp.AddErrorText(e.Exception.Message);
                e.ExceptionHandled = true;
                e.KeepInInsertMode = true;
            }
        }

        #endregion

        #region Training Updation

        /// <summary>
        /// Ensure that training period does not overlap while updating
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dsTrainingDetails_Updating(object sender, LinqDataSourceUpdateEventArgs e)
        {
            DropDownListEx ddlServicePeriod = (DropDownListEx)fvTrainingDetails.FindControl("ddlServicePeriod");
            TextBoxEx tbTrainingStartFrom = (TextBoxEx)fvTrainingDetails.FindControl("tbTrainingStartFrom");
            TextBoxEx tbTrainingEndTo = (TextBoxEx)fvTrainingDetails.FindControl("tbTrainingEndTo");
            using (PISDataContext db = new PISDataContext(ReportingUtilities.DefaultConnectString))
            {
                int trainingCount = (from trn in db.Trainings
                                     where trn.ServicePeriodId == Convert.ToInt32(ddlServicePeriod.Value)
                                     select trn).Count();

                if (trainingCount > 1)
                {
                    Training trnRec = db.Trainings
                                                .Where(p => p.ServicePeriodId == Convert.ToInt32(ddlServicePeriod.Value))
                                                .OrderByDescending(p => p.TrainingStartFrom)
                                                .Skip(1)
                                                .Take(1)
                                                .Single();


                    if (tbTrainingStartFrom.ValueAsDate < trnRec.TrainingEndTo.Value)
                    {
                        throw new NotImplementedException(string.Format("Please enter {0} greater than previous {1}", tbTrainingStartFrom.FriendlyName, tbTrainingEndTo.FriendlyName));
                    }

                }
            }
        }

        /// <summary>
        /// Set EmployeeId so that it can be sent as reponse on successful updation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dsTrainingDetails_Updated(object sender, LinqDataSourceStatusEventArgs e)
        {
            if (e.Exception == null)
            {
                _trn = (Training)e.Result;
                using (PISDataContext db = new PISDataContext(ReportingUtilities.DefaultConnectString))
                {
                    _empId = (from trnRec in db.Trainings
                              where trnRec.TrainingId == _trn.TrainingId
                              && trnRec.ServicePeriodId == _trn.ServicePeriodId
                              select trnRec).Max(p => p.ServicePeriod.EmployeeId);
                }
            }
        }

        /// <summary>
        /// Show exception message and keep the FormView EditMode when updation fails
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void fvTrainingDetails_ItemUpdated(object sender, FormViewUpdatedEventArgs e)
        {
            if (e.Exception != null)
            {
                TrainingDetails_sp.AddErrorText(e.Exception.Message);
                e.ExceptionHandled = true;
                e.KeepInEditMode = true;
            }
        }

        #endregion

    }
}
