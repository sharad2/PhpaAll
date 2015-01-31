using System;
using System.Linq;
using System.Web.Script.Serialization;
using System.Web.UI.WebControls;
using Eclipse.PhpaLibrary.Database.PIS;
using Eclipse.PhpaLibrary.Reporting;
using Eclipse.PhpaLibrary.Web;
using EclipseLibrary.Web.JQuery.Input;

namespace PhpaAll.PIS
{
    public partial class Termination : PageBase
    {
        //protected void custValDateOfRelieve_ServerValidate(object sender, EclipseLibrary.Web.JQuery.Input.ServerValidateEventArgs e)
        //{
        //    EclipseLibrary.Web.JQuery.Input.ValidationSummary valTermination = (EclipseLibrary.Web.JQuery.Input.ValidationSummary)fvTermination.FindControl("valTermination");
        //    TextBoxEx tbDateOfRelieve = (TextBoxEx)fvTermination.FindControl("tbDateOfRelieve");

        //    using (PISDataContext db = new PISDataContext(ReportingUtilities.DefaultConnectString))
        //    {
        //        ServicePeriod sp = db.ServicePeriods
        //            .Where(p => p.EmployeeId == Convert.ToInt32(this.Request.QueryString["EmployeeId"]))
        //            .OrderByDescending(p => p.PeriodStartDate)
        //            .Take(1)
        //            .Single();

        //        if (tbDateOfRelieve.ValueAsDate < sp.PeriodStartDate)
        //        {
        //            valTermination.ErrorMessages.Add(string.Format("{0} must be greater than Start Date in Service", tbDateOfRelieve.FriendlyName));
        //            e.ControlToValidate.IsValid = false;
        //        }

        //    }
        //}

        protected void btnTerminate_Click(object sender, EventArgs e)
        {
            if (!btnTerminate.IsPageValid())
            {
                return;
            }

            fvTermination.UpdateItem(false);

            if (!Termination_sp.HasErrorText)
            {
                this.Response.ContentType = "application/json";
                JavaScriptSerializer ser = new JavaScriptSerializer();
                this.Response.StatusCode = 205;
                this.Response.End();
            }
        }



        protected void dsTermination_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            if (e.WhereParameters["EmployeeId"] == null)
            {
                e.Cancel = true;
            }
        }

        /// <summary>
        /// Set the DateOfRelieve in the corresponding PeriodEndDate
        /// of PIS.ServicePeriod
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dsTermination_Updating(object sender, LinqDataSourceUpdateEventArgs e)
        {
            TextBoxEx tbDateOfRelieve = (TextBoxEx)fvTermination.FindControl("tbDateOfRelieve");
            int employeeId = Convert.ToInt32(fvTermination.DataKey["EmployeeId"]);
            if (employeeId != 0)
            {
                using (PISDataContext db = new PISDataContext(ReportingUtilities.DefaultConnectString))
                {
                    if (db.Employees.Single(p => p.EmployeeId == employeeId).ServicePeriods.Any())
                    {

                        int spId = db.Employees.Single(p => p.EmployeeId == employeeId)
                            .ServicePeriods.OrderByDescending(p => p.PeriodStartDate).Take(1)
                            .Max(p => p.ServicePeriodId);

                        ServicePeriod sp = db.ServicePeriods.Single(p => p.ServicePeriodId == spId);
                        sp.PeriodEndDate = tbDateOfRelieve.ValueAsDate.Value;
                        db.SubmitChanges();
                    }
                }
            }

        }

        protected void dsTermination_Updated(object sender, LinqDataSourceStatusEventArgs e)
        {
            if (e.Exception != null)
            {
                Termination_sp.AddErrorText(e.Exception.Message);
                e.ExceptionHandled = true;
            }
        }
    }
}
