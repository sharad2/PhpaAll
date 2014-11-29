using System;
using System.Linq;
using System.Web.Script.Serialization;
using System.Web.UI.WebControls;
using Eclipse.PhpaLibrary.Database.PIS;
using Eclipse.PhpaLibrary.Reporting;
using Eclipse.PhpaLibrary.Web;
using EclipseLibrary.Web.JQuery.Input;

namespace Finance.PIS
{
    /// <summary>
    /// Promote an employee by creating a new ServicePeriod with
    /// PeriodStartDate=PromotionDate and PeriodEndDate=ExtensionUpto.
    /// The current ServicePeriod now becomes Service History
    /// </summary>
    /// <remarks>
    /// QueryString: ServicePeriodId,EmployeeId,Type-> I(ie:Insert),Type:E(ie:Edit)
    /// </remarks>
    public partial class GrantPromotion : PageBase
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
                    fvPromotion.DefaultMode = FormViewMode.Insert;
                    break;

                case "E":
                    fvPromotion.DefaultMode = FormViewMode.Insert;
                    break;

                default:
                    throw new NotImplementedException();
            }
            base.OnLoad(e);
        }

        #endregion

        #region Custom Validation

        protected void custValPromotion_ServerValidate(object sender, EclipseLibrary.Web.JQuery.Input.ServerValidateEventArgs e)
        {
            Custom ct = (Custom)sender;
            EclipseLibrary.Web.JQuery.Input.ValidationSummary valPromotion = (EclipseLibrary.Web.JQuery.Input.ValidationSummary)fvPromotion.FindControl("valPromotion");
            TextBoxEx tbPromotionDate = (TextBoxEx)fvPromotion.FindControl("tbPromotionDate");
            TextBoxEx tbNextPromotionDate = (TextBoxEx)fvPromotion.FindControl("tbNextPromotionDate");

            switch (ct.ID)
            {
                //case "custValPromotionDate":
                //    using (PISDataContext db = new PISDataContext(ReportingUtilities.DefaultConnectString))
                //    {
                //        ServicePeriod sp = db.ServicePeriods
                //   .Where(p => p.ServicePeriodId == Convert.ToInt32(this.Request.QueryString["ServicePeriodId"]))
                //   .Single();
                //        string errorMessage = string.Format("{0} must be greater than Start Date in Service", tbPromotionDate.FriendlyName);
                //        switch (sp.PeriodEndDate.HasValue)
                //        {
                //            case true:
                //                if (tbPromotionDate.ValueAsDate < sp.PeriodStartDate)
                //                {
                //                    valPromotion.ErrorMessages.Add(errorMessage);
                //                    e.ControlToValidate.IsValid = false;
                //                }
                //                break;

                //            case false:
                //                if (tbPromotionDate.ValueAsDate < sp.PeriodStartDate)
                //                {
                //                    valPromotion.ErrorMessages.Add(errorMessage);
                //                    e.ControlToValidate.IsValid = false;
                //                }
                //                break;

                //            default:
                //                break;
                //        }
                //    }
                //    break;

                case "custValNextPromotionDate":
                    if (tbNextPromotionDate.ValueAsDate.HasValue && (tbNextPromotionDate.ValueAsDate.Value < tbPromotionDate.ValueAsDate))
                    {
                        valPromotion.ErrorMessages.Add(string.Format("{0} must be greater than {1}", tbNextPromotionDate.FriendlyName, tbPromotionDate.FriendlyName));
                        e.ControlToValidate.IsValid = false;
                    }
                    break;

                default:
                    break;
            }



        }

        #endregion

        #region Promote/Edit Promotion button

        /// <summary>
        /// Insert/Update Promotion according to its CurrentMode and
        /// send the success StstusCode 205 if insertion/updation succeeds
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnPromote_click(object sender, EventArgs e)
        {
            ButtonEx btnPromote = (ButtonEx)sender;
            if (!btnPromote.IsPageValid())
            {
                return;
            }

            switch (fvPromotion.CurrentMode)
            {
                case FormViewMode.Edit:
                    fvPromotion.UpdateItem(false);
                    break;

                case FormViewMode.Insert:
                    fvPromotion.InsertItem(false);
                    break;

                default:
                    throw new NotImplementedException();
            }

            if (!Promotion_sp.HasErrorText)
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
        /// PeriodStartDate = PromotionDate,
        /// PeriodEndDate = ExtensionUpto,
        /// and all the other fields carry the same value as the old
        /// ServicePeriod
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dsPromotion_Inserting(object sender, LinqDataSourceInsertEventArgs e)
        {
            TextBoxEx tbPromotionDate = (TextBoxEx)fvPromotion.FindControl("tbPromotionDate");
            TextBoxEx tbExtensionUpto = (TextBoxEx)fvPromotion.FindControl("tbExtensionUpto");

            int servicePeriodId = Convert.ToInt32(this.Request.QueryString["ServicePeriodId"]);
            if (servicePeriodId != 0)
            {
                using (PISDataContext db = new PISDataContext(ReportingUtilities.DefaultConnectString))
                {
                    ServicePeriod sp = db.ServicePeriods.Single(p => p.ServicePeriodId == servicePeriodId);
                    //Update PeriodEndDate of old ServicePeriod to current (PromotionDate -1)                    
                    sp.PeriodEndDate = tbPromotionDate.ValueAsDate.Value.AddDays(-1);
                    db.SubmitChanges();

                    ServicePeriod spNew = (ServicePeriod)e.NewObject;
                    spNew.EmployeeId = sp.EmployeeId;
                    spNew.PeriodStartDate = tbPromotionDate.ValueAsDate.Value;
                    spNew.PeriodEndDate = tbExtensionUpto.ValueAsDate.HasValue ? tbExtensionUpto.ValueAsDate : null;
                    spNew.Designation = sp.Designation;
                    spNew.Grade = sp.Grade;
                    spNew.BasicSalary = sp.BasicSalary;
                    spNew.IsConsolidated = sp.IsConsolidated;
                    spNew.MinPayScaleAmount = sp.MinPayScaleAmount;
                    spNew.IncrementAmount = sp.IncrementAmount;
                    spNew.MaxPayScaleAmount = sp.MaxPayScaleAmount;
                    spNew.DateOfIncrement = sp.DateOfIncrement;
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
        protected void fvPromotion_ItemInserted(object sender, FormViewInsertedEventArgs e)
        {
            if (e.Exception != null)
            {
                Promotion_sp.AddErrorText(e.Exception.Message);
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
        protected void dsPromotion_Updating(object sender, LinqDataSourceUpdateEventArgs e)
        {
            DateTime? extensionUpto = null;
            int servicePeriodId = Convert.ToInt32(this.Request.QueryString["ServicePeriodId"]);
            int employeeId = Convert.ToInt32(this.Request.QueryString["EmployeeId"]);

            DateTime promotionDate = ((ServicePeriod)e.NewObject).PromotionDate.Value;

            if (((ServicePeriod)e.NewObject).ExtensionUpto.HasValue)
            {
                extensionUpto = ((ServicePeriod)e.NewObject).ExtensionUpto.Value;
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

                        sp.PeriodEndDate = promotionDate.AddDays(-1);
                        db.SubmitChanges();

                        ServicePeriod spOriginal = (ServicePeriod)e.OriginalObject;
                        ServicePeriod spNew = (ServicePeriod)e.NewObject;
                        spNew.EmployeeId = spOriginal.EmployeeId;
                        spNew.PeriodStartDate = promotionDate;
                        spNew.PeriodEndDate = extensionUpto != null ? extensionUpto : null;
                        spNew.Designation = spOriginal.Designation;
                        spNew.Grade = spOriginal.Grade;
                        spNew.BasicSalary = spOriginal.BasicSalary;
                        spNew.IsConsolidated = spOriginal.IsConsolidated;
                        spNew.MinPayScaleAmount = spOriginal.MinPayScaleAmount;
                        spNew.IncrementAmount = spOriginal.IncrementAmount;
                        spNew.MaxPayScaleAmount = spOriginal.MaxPayScaleAmount;
                        spNew.DateOfIncrement = spOriginal.DateOfIncrement;
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
        protected void fvPromotion_ItemUpdated(object sender, FormViewUpdatedEventArgs e)
        {
            if (e.Exception != null)
            {
                Promotion_sp.AddErrorText(e.Exception.Message);
                e.ExceptionHandled = true;
                e.KeepInEditMode = true;
            }
        }

        #endregion

    }
}
