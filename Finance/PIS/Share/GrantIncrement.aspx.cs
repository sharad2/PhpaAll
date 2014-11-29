using System;
using System.Linq;
using System.Web.Script.Serialization;
using System.Web.UI.WebControls;
using Eclipse.PhpaLibrary.Database.PIS;
using Eclipse.PhpaLibrary.Reporting;
using Eclipse.PhpaLibrary.Web;
using EclipseLibrary.Web.JQuery.Input;

namespace Finance.PIS.Share
{
    public partial class GrantIncrement : PageBase
    {
        #region Load

        /// <summary>
        /// Set FormView DefaultMode according to the QueryString["Type"] value
        /// </summary>
        /// <param name="e"></param>
        protected override void OnLoad(EventArgs e)
        {
            switch (this.Request.QueryString["Type"])
            {
                case "I":
                    fvGrantIncrement.DefaultMode = FormViewMode.Insert;
                    break;

                case "E":
                    fvGrantIncrement.DefaultMode = FormViewMode.Insert;
                    break;

                default:
                    throw new NotSupportedException("QueryString Type must be I or E");
            }
            base.OnLoad(e);
        }

        #endregion

        #region Custom Validation

        /// <summary>
        /// Ensure that IncrementDate must be greater than current Service Period Date Range
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //protected void custValIncDate_ServerValidate(object sender, EclipseLibrary.Web.JQuery.Input.ServerValidateEventArgs e)
        //{
        //    EclipseLibrary.Web.JQuery.Input.ValidationSummary ValIncrement = (EclipseLibrary.Web.JQuery.Input.ValidationSummary)fvGrantIncrement.FindControl("ValIncrement");
        //    TextBoxEx tbIncrementDate = (TextBoxEx)fvGrantIncrement.FindControl("tbIncrementDate");
        //    TextBoxEx tbNextIncrementDate = (TextBoxEx)fvGrantIncrement.FindControl("tbNextIncrementDate");

        //    using (PISDataContext db = new PISDataContext(ReportingUtilities.DefaultConnectString))
        //    {
        //        ServicePeriod sp = db.ServicePeriods
        //            .Where(p => p.ServicePeriodId == Convert.ToInt32(this.Request.QueryString["ServicePeriodId"]))
        //            .Single();
        //        string errorMessage = string.Format("{0} must be greater than Start Date in Service", tbIncrementDate.FriendlyName);

        //        switch (sp.PeriodEndDate.HasValue)
        //        {
        //            case true:
        //                if (tbIncrementDate.ValueAsDate < sp.PeriodStartDate)
        //                {
        //                    ValIncrement.ErrorMessages.Add(errorMessage);
        //                    e.ControlToValidate.IsValid = false;
        //                }
        //                break;

        //            case false:
        //                if (tbIncrementDate.ValueAsDate < sp.PeriodStartDate)
        //                {
        //                    ValIncrement.ErrorMessages.Add(errorMessage);
        //                    e.ControlToValidate.IsValid = false;
        //                }
        //                break;

        //            default:
        //                break;
        //        }               
        //    }
        //}

        #endregion

        #region Promote/Edit Increment button

        /// <summary>
        /// Insert/Update Increment according to its CurrentMode and
        /// send the success StstusCode 205 if insertion/updation succeeds
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnIncrement_Click(object sender, EventArgs e)
        {
            ButtonEx btnIncrement = (ButtonEx)sender;
            if (!btnIncrement.IsPageValid())
            {
                return;
            }

            switch (fvGrantIncrement.CurrentMode)
            {
                case FormViewMode.Edit:
                    fvGrantIncrement.UpdateItem(false);
                    break;

                case FormViewMode.Insert:
                    fvGrantIncrement.InsertItem(false);
                    break;

                default:
                    throw new NotImplementedException();
            }

            if (!GrantIncrement_sp.HasErrorText)
            {
                this.Response.ContentType = "application/json";
                JavaScriptSerializer ser = new JavaScriptSerializer();
                this.Response.StatusCode = 205;
                this.Response.End();
            }

        }

        #endregion

        #region Insertion

        /// <summary>
        /// Current ServicePeriod becomes Service History and a new Service
        /// is inserted.        
        /// In the new Service Period
        /// PeriodStartDate = DateOfIncrement,
        /// PeriodEndDate = DateOfNextIncrement,
        /// and all the other fields carry the same value as the old
        /// ServicePeriod
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dsGrantIncrement_Inserting(object sender, LinqDataSourceInsertEventArgs e)
        {
            TextBoxEx tbIncrementDate = (TextBoxEx)fvGrantIncrement.FindControl("tbIncrementDate");
            TextBoxEx tbNextIncrementDate = (TextBoxEx)fvGrantIncrement.FindControl("tbNextIncrementDate");

            int servicePeriodId = Convert.ToInt32(this.Request.QueryString["ServicePeriodId"]);
            if (servicePeriodId != 0)
            {
                using (PISDataContext db = new PISDataContext(ReportingUtilities.DefaultConnectString))
                {
                    ServicePeriod sp = db.ServicePeriods.Single(p => p.ServicePeriodId == servicePeriodId);
                    //Update PeriodEndDate of old ServicePeriod to current (DateOfIncrement -1)                    
                    sp.PeriodEndDate = tbIncrementDate.ValueAsDate.Value.AddDays(-1);
                    db.SubmitChanges();

                    ServicePeriod spNew = (ServicePeriod)e.NewObject;
                    spNew.EmployeeId = sp.EmployeeId;
                    spNew.PeriodStartDate = tbIncrementDate.ValueAsDate.Value;
                    spNew.PeriodEndDate = tbNextIncrementDate.ValueAsDate.HasValue ? tbNextIncrementDate.ValueAsDate : null;
                    spNew.Designation = sp.Designation;
                    spNew.Grade = sp.Grade;
                    spNew.BasicSalary = sp.BasicSalary;
                    spNew.IsConsolidated = sp.IsConsolidated;
                    spNew.MinPayScaleAmount = sp.MinPayScaleAmount;
                    spNew.IncrementAmount = sp.IncrementAmount;
                    spNew.MaxPayScaleAmount = sp.MaxPayScaleAmount;
                    spNew.PromotionDate = sp.PromotionDate;
                    spNew.PromotionTypeId = sp.PromotionTypeId;
                    spNew.ExtensionUpto = sp.ExtensionUpto;
                    spNew.NextPromotionDate = sp.NextPromotionDate;
                    spNew.PostedAt = sp.PostedAt;
                    spNew.InitialTerm = sp.InitialTerm;
                }
            }
        }

        /// <summary>
        /// Show error message when exception occurs and
        /// also keep the FormView in InsertMode
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void fvGrantIncrement_ItemInserted(object sender, FormViewInsertedEventArgs e)
        {
            if (e.Exception != null)
            {
                GrantIncrement_sp.AddErrorText(e.Exception.Message);
                e.ExceptionHandled = true;
                e.KeepInInsertMode = true;
            }
        }

        #endregion

        #region Updation

        /// <summary>
        /// Changes are made in the current Service Period.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dsGrantIncrement_Updating(object sender, LinqDataSourceUpdateEventArgs e)
        {
            DateTime? nextIncrementDate = null;
            int servicePeriodId = Convert.ToInt32(this.Request.QueryString["ServicePeriodId"]);
            int employeeId = Convert.ToInt32(this.Request.QueryString["EmployeeId"]);

            DateTime dateOfIncrement = ((ServicePeriod)e.NewObject).DateOfIncrement.Value;

            if (((ServicePeriod)e.NewObject).DateOfNextIncrement.HasValue)
            {
                nextIncrementDate = ((ServicePeriod)e.NewObject).DateOfNextIncrement.Value;
            }

            // If there exists more than one Service Period
            // then in the second highest Service Period the
            // PeriodEndDate =  Current Service Period's (PeriodStartDate -1)
            if (servicePeriodId != 0)
            {
                using (PISDataContext db = new PISDataContext(ReportingUtilities.DefaultConnectString))
                {
                    int count = db.ServicePeriods
                        .Where(p => p.EmployeeId == employeeId)
                        .Count();

                    if (count > 1)
                    {
                        ServicePeriod sp = db.ServicePeriods
                             .Where(p => p.EmployeeId == employeeId)
                             .OrderByDescending(p => p.PeriodStartDate)
                             .Skip(1)
                             .Take(1)
                             .Single();

                        sp.PeriodEndDate = dateOfIncrement.AddDays(-1);
                        db.SubmitChanges();

                        ServicePeriod spOriginal = (ServicePeriod)e.OriginalObject;
                        ServicePeriod spNew = (ServicePeriod)e.NewObject;
                        spNew.EmployeeId = spOriginal.EmployeeId;
                        spNew.PeriodStartDate = dateOfIncrement;
                        //spNew.PeriodEndDate = nextIncrementDate != null ? nextIncrementDate : null;
                        spNew.Designation = spOriginal.Designation;
                        spNew.Grade = spOriginal.Grade;
                        spNew.BasicSalary = spOriginal.BasicSalary;
                        spNew.IsConsolidated = spOriginal.IsConsolidated;
                        spNew.MinPayScaleAmount = spOriginal.MinPayScaleAmount;
                        spNew.IncrementAmount = spOriginal.IncrementAmount;
                        spNew.MaxPayScaleAmount = spOriginal.MaxPayScaleAmount;
                        spNew.PromotionDate = spOriginal.PromotionDate;
                        spNew.PromotionTypeId = spOriginal.PromotionTypeId;
                        spNew.ExtensionUpto = spOriginal.ExtensionUpto;
                        spNew.NextPromotionDate = spOriginal.NextPromotionDate;
                        spNew.PostedAt = spOriginal.PostedAt;
                        spNew.InitialTerm = spOriginal.InitialTerm;

                    }
                }
            }

        }

        /// <summary>
        /// Show exception message on error and keep the 
        /// FormView in EditMode
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void fvGrantIncrement_ItemUpdated(object sender, FormViewUpdatedEventArgs e)
        {
            if (e.Exception != null)
            {
                GrantIncrement_sp.AddErrorText(e.Exception.Message);
                e.ExceptionHandled = true;
                e.KeepInEditMode = true;
            }
        }

        #endregion

    }
}
