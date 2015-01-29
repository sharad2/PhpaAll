using System;
using System.Data.Linq;
using System.Linq;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;
using Eclipse.PhpaLibrary.Database.PIS;
using Eclipse.PhpaLibrary.Reporting;
using Eclipse.PhpaLibrary.Web;
using EclipseLibrary.Web.JQuery.Input;

namespace PhpaAll.PIS.Share
{
    public partial class LeaveDetails : PageBase
    {
        /// <summary>
        /// The EmployeeId being finally returned on successful insertion/updation
        /// </summary>
        private object _empId = null;

        /// <summary>
        /// The LeaveRecordId being inserted/updated. This is set in the Inserting/Updating
        /// event.
        /// </summary>
        private LeaveRecord _leaveRecord;

        #region Load

        /// <summary>
        /// Determine the FormView default mode from querystring value
        /// </summary>
        /// <param name="e"></param>
        ///<remarks>
        /// QueryString:EmployeeId,LeaveRecordId, NL-> New Leave, EL-> Edit Leave
        /// </remarks>
        protected override void OnLoad(EventArgs e)
        {
            switch (this.Request.QueryString["Key"])
            {
                case "NL":
                    fvLeaveDetails.DefaultMode = FormViewMode.Insert;
                    break;

                case "EL":
                    fvLeaveDetails.DefaultMode = FormViewMode.Edit;
                    break;

                default:
                    throw new NotImplementedException();
            }
            base.OnLoad(e);
        }

        #endregion

        #region Custom Validation

        /// <summary>
        /// Ensure that LeaveStartDate and LeaveEndTo is within the ServicePeriod
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void custValLeavePeriod_ServerValidate(object sender, EclipseLibrary.Web.JQuery.Input.ServerValidateEventArgs e)
        {
            Custom ct = (Custom)sender;
            //EclipseLibrary.Web.JQuery.Input.ValidationSummary valLeaveDetails = (EclipseLibrary.Web.JQuery.Input.ValidationSummary)fvLeaveDetails.FindControl("valLeaveDetails");
            DropDownListEx ddlServicePeriod = (DropDownListEx)fvLeaveDetails.FindControl("ddlServicePeriod");
            TextBoxEx tbLeaveStartFrom = (TextBoxEx)fvLeaveDetails.FindControl("tbLeaveStartFrom");
            TextBoxEx tbLeaveEndTo = (TextBoxEx)fvLeaveDetails.FindControl("tbLeaveEndTo");
            string errorMessage = string.Empty;

            using (PISDataContext db = new PISDataContext(ReportingUtilities.DefaultConnectString))
            {
                ServicePeriod sp = db.ServicePeriods
                    .Where(p => p.ServicePeriodId == Convert.ToInt32(ddlServicePeriod.Value))
                    .Single();

                switch (ct.ID)
                {
                    case "custValLeaveStartFrom":
                        //Ensure that LeaveStartFrom is within ServicePeriod
                        errorMessage = string.Format("{0} must be within the Service Period", tbLeaveStartFrom.FriendlyName);
                        if (sp.PeriodEndDate.HasValue)
                        {
                            if ((tbLeaveStartFrom.ValueAsDate < sp.PeriodStartDate) || (tbLeaveStartFrom.ValueAsDate > sp.PeriodEndDate.Value))
                            {
                                ValidateLeave(e, errorMessage);
                            }
                        }
                        else
                        {
                            if (tbLeaveStartFrom.ValueAsDate < sp.PeriodStartDate)
                            {
                                ValidateLeave(e, errorMessage);
                            }
                        }

                        break;

                    case "custValLeaveEndTo":
                        //Ensure that LeaveEndTo is within ServicePeriod
                        errorMessage = string.Format("{0} must be within the Service Period", tbLeaveEndTo.FriendlyName);
                        if (sp.PeriodEndDate.HasValue)
                        {
                            if ((tbLeaveEndTo.ValueAsDate < sp.PeriodStartDate) || (tbLeaveEndTo.ValueAsDate > sp.PeriodEndDate.Value))
                            {
                                ValidateLeave(e, errorMessage);
                            }
                        }
                        else
                        {
                            if ((tbLeaveEndTo.ValueAsDate < sp.PeriodStartDate))
                            {
                                ValidateLeave(e, errorMessage);
                            }
                        }

                        break;

                    default:
                        break;
                }


            }

        }

        private static void ValidateLeave(EclipseLibrary.Web.JQuery.Input.ServerValidateEventArgs e, string errorMessage)
        {
            e.ControlToValidate.ErrorMessage = errorMessage;
            //valLeaveDetails.ErrorMessages.Add(errorMessage);
            e.ControlToValidate.IsValid = false;
        }

        #endregion

        #region Selecting

        /// <summary>
        /// Cancel the query if LeaveRecordId is null while updating
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dsLeaveDetails_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            if (e.WhereParameters["LeaveRecordId"] == null)
            {
                e.Cancel = true;
                return;
            }
        }

        #endregion



        #region Insert/Update Leave

        /// <summary>
        /// Insert/Update Leave and send the EmployeeId on success
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnLeaveDetails_Click(object sender, EventArgs e)
        {
            ButtonEx btnLeaveDetails = (ButtonEx)sender;
            if (!btnLeaveDetails.IsPageValid())
            {
                return;
            }

            switch (fvLeaveDetails.CurrentMode)
            {
                case FormViewMode.Edit:
                    fvLeaveDetails.UpdateItem(false);
                    break;

                case FormViewMode.Insert:
                    fvLeaveDetails.InsertItem(false);
                    break;

                default:
                    throw new NotImplementedException();
            }

            if (!LeaveDetails_sp.HasErrorText)
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

        protected void dsLeaveDetails_ContextCreated(object sender, LinqDataSourceStatusEventArgs e)
        {
            DataContext db = (DataContext)e.Result;
            DataLoadOptions dlo = new DataLoadOptions();
            dlo.LoadWith<LeaveRecord>(p => p.LeaveType);
            db.LoadOptions = dlo;
        }

        #endregion

        #region Leave Insertion

        /// <summary>
        /// Ensure that Leave Period entered is not overlapping
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dsLeaveDetails_Inserting(object sender, LinqDataSourceInsertEventArgs e)
        {
            DropDownListEx ddlServicePeriod = (DropDownListEx)fvLeaveDetails.FindControl("ddlServicePeriod");
            TextBoxEx tbLeaveStartFrom = (TextBoxEx)fvLeaveDetails.FindControl("tbLeaveStartFrom");
            TextBoxEx tbLeaveEndTo = (TextBoxEx)fvLeaveDetails.FindControl("tbLeaveEndTo");
            using (PISDataContext db = new PISDataContext(ReportingUtilities.DefaultConnectString))
            {
                int leaveCount = (from leave in db.LeaveRecords
                                  where leave.ServicePeriodId == Convert.ToInt32(ddlServicePeriod.Value)
                                  select leave).Count();

                if (leaveCount > 0)
                {
                    LeaveRecord leaveRec = db.LeaveRecords
                                                .Where(p => p.ServicePeriodId == Convert.ToInt32(ddlServicePeriod.Value))
                                                .OrderByDescending(p => p.LeaveStartFrom)
                                                .Take(1)
                                                .Single();
                    if (tbLeaveStartFrom.ValueAsDate < leaveRec.LeaveEndTo.Value)
                    {
                        throw new NotImplementedException(string.Format("Please enter {0} greater than previous {1}", tbLeaveStartFrom.FriendlyName, tbLeaveEndTo.FriendlyName));
                    }

                    //if (leaveRec.LeaveEndTo.HasValue)
                    //{
                    //    if (tbLeaveStartFrom.ValueAsDate < leaveRec.LeaveEndTo.Value)
                    //    {
                    //        throw new NotImplementedException(string.Format("Please enter {0} greater than previous {1}", tbLeaveStartFrom.FriendlyName, tbLeaveEndTo.FriendlyName));
                    //    }
                    //}
                    //else
                    //{
                    //    throw new NotImplementedException("Please enter end date for previous leave");
                    //}
                }
            }
        }

        /// <summary>
        /// Set the EmployeeId inserted value so that it can be sent as response
        /// after successful insertion
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dsLeaveDetails_Inserted(object sender, LinqDataSourceStatusEventArgs e)
        {
            if (e.Exception == null)
            {
                _leaveRecord = (LeaveRecord)e.Result;
                _empId = _leaveRecord.ServicePeriod.EmployeeId;
            }
        }

        /// <summary>
        /// Show exception message and also keep the FormView in Insert mode when insertion fails
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void fvLeaveDetails_ItemInserted(object sender, FormViewInsertedEventArgs e)
        {
            if (e.Exception != null)
            {
                LeaveDetails_sp.AddErrorText(e.Exception.Message);
                e.ExceptionHandled = true;
                e.KeepInInsertMode = true;
            }
        }

        #endregion

        #region Leave Updation

        protected void dsLeaveDetails_Updating(object sender, LinqDataSourceUpdateEventArgs e)
        {
            DropDownListEx ddlServicePeriod = (DropDownListEx)fvLeaveDetails.FindControl("ddlServicePeriod");
            TextBoxEx tbLeaveStartFrom = (TextBoxEx)fvLeaveDetails.FindControl("tbLeaveStartFrom");
            TextBoxEx tbLeaveEndTo = (TextBoxEx)fvLeaveDetails.FindControl("tbLeaveEndTo");
            using (PISDataContext db = new PISDataContext(ReportingUtilities.DefaultConnectString))
            {
                int leaveCount = (from leave in db.LeaveRecords
                                  where leave.ServicePeriodId == Convert.ToInt32(ddlServicePeriod.Value)
                                  select leave).Count();

                if (leaveCount > 1)
                {
                    LeaveRecord leaveRec = db.LeaveRecords
                                                .Where(p => p.ServicePeriodId == Convert.ToInt32(ddlServicePeriod.Value))
                                                .OrderByDescending(p => p.LeaveStartFrom)
                                                .Skip(1)
                                                .Take(1)
                                                .Single();

                    //if (leaveRec.LeaveEndTo.HasValue)
                    //{
                    if (tbLeaveStartFrom.ValueAsDate < leaveRec.LeaveEndTo.Value)
                    {
                        throw new NotImplementedException(string.Format("Please enter {0} greater than previous {1}", tbLeaveStartFrom.FriendlyName, tbLeaveEndTo.FriendlyName));
                    }
                    //}
                    //else
                    //{
                    //    throw new NotImplementedException("Please enter end date for previous leave");
                    //}
                }
            }
        }

        /// <summary>
        /// Set EmployeeId so that it can be sent as reponse on successful updation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dsLeaveDetails_Updated(object sender, LinqDataSourceStatusEventArgs e)
        {
            if (e.Exception == null)
            {
                _leaveRecord = (LeaveRecord)e.Result;
                using (PISDataContext db = new PISDataContext(ReportingUtilities.DefaultConnectString))
                {
                    _empId = (from l in db.LeaveRecords
                              where l.ServicePeriodId == _leaveRecord.ServicePeriodId
                              && l.LeaveRecordId == _leaveRecord.LeaveRecordId
                              select l).Max(p => p.ServicePeriod.EmployeeId);
                }
            }
        }

        /// <summary>
        /// Show exception message and keep the FormView Update Mode when updation fails
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void fvLeaveDetails_ItemUpdated(object sender, FormViewUpdatedEventArgs e)
        {
            if (e.Exception != null)
            {
                LeaveDetails_sp.AddErrorText(e.Exception.Message);
                e.ExceptionHandled = true;
                e.KeepInEditMode = true;
            }
        }

        #endregion

    }
}
