using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Web.Services;
using System.Web.UI.WebControls;
using Eclipse.PhpaLibrary.Database.PIS;
using Eclipse.PhpaLibrary.Reporting;
using Eclipse.PhpaLibrary.Web;
using EclipseLibrary.Web.JQuery;

namespace PhpaAll.PIS
{
    public partial class EmployeeDetails : PageBase
    {
        #region Load Events

        protected override void OnLoad(EventArgs e)
        {
            JQueryScriptManager.Current.RegisterScripts(ScriptTypes.WebMethods);
            base.OnLoad(e);
        }

        #endregion

        #region ServiceHistory Events

        protected void dsServiceHistory_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            if (e.WhereParameters["EmployeeId"] == null)
            {
                e.Cancel = true;
            }
        }

        /// <summary>
        /// On the Promotion dialog set the QueryStrings in the Url
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dlgIncrementPromotion_PreRender(object sender, EventArgs e)
        {
            Dialog dlgPromotion = (Dialog)sender;            
            int servicePeriodId = Convert.ToInt32(fvServiceHistory.DataKey.Values["ServicePeriodId"]);
            int employeeId = Convert.ToInt32(fvServiceHistory.DataKey.Values["EmployeeId"]);
            if (servicePeriodId != 0)
            {
                dlgPromotion.Ajax.Url += string.Format("?ServicePeriodId={0}&EmployeeId={1}", servicePeriodId,
                   employeeId);
            }
        }

        #endregion

        #region EmployeeDetails Selecting

        protected void dsEmployeeDetails_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            if (e.WhereParameters["EmployeeId"] == null)
            {
                e.Cancel = true;
            }
        }

        #endregion

        #region Employee Context Creation

        protected void dsEmployeeDetails_ContextCreated(object sender, LinqDataSourceStatusEventArgs e)
        {
            DataContext db = (DataContext)e.Result;
            DataLoadOptions dlo = new DataLoadOptions();
            dlo.LoadWith<Employee>(p => p.EmployeeType);
            dlo.LoadWith<Employee>(p => p.Division);
            //dlo.LoadWith<Employee>(p => p.Office);
            db.LoadOptions = dlo;
        }

        #endregion

        #region Title

        int? _employeeStatusId = null;
        DateTime? _dateOfRelieve = null;

        /// <summary>
        /// Remember EmployeeStatus and DateOfRelieve
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dsEmployeeDetails_Selected(object sender, LinqDataSourceStatusEventArgs e)
        {
            if (e.Exception == null)
            {
                _employeeStatusId = ((List<Employee>)e.Result).Max(p => p.EmployeeStatusId);
                _dateOfRelieve = ((List<Employee>)e.Result).Max(p => p.DateOfRelieve);
            }
        }

        /// <summary>
        /// Show the EmployeeName in the Page Title. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <remarks>
        /// Again if the employee is terminated append his status
        /// with the name
        /// Again also show Joining Date and termination status if terminated 
        /// in the sub title
        /// </remarks>        
        protected void fvEmployeeDetails_DataBound(object sender, EventArgs e)
        {
            Employee emp = (Employee)fvEmployeeDetails.DataItem;
            if (emp != null)
            {
                this.Page.Title += string.Format(" of {0}", emp.FullName);

                string subTitle = "Working since ";

                if (emp.JoiningDate.HasValue)
                {
                    subTitle += string.Format("{0:d}", emp.JoiningDate);
                }
                else
                {
                    subTitle += "(Joining date not filled)";
                }                

                //Add Termination Status in title when DateOfRelieve exists and is is <= current date
                if (_dateOfRelieve.HasValue && (DateTime.Now >= _dateOfRelieve.Value))
                {
                    this.Page.Title += string.Format(" ({0})", emp.EmployeeStatus.EmployeeStatusType);
                    subTitle += string.Format(" and {0} on {1:d}", emp.EmployeeStatus.EmployeeStatusType, emp.DateOfRelieve);
                }

                lblSubTitle.Text = subTitle;                
            }
        }

        #endregion

        #region Employee PreRender

        /// <summary>
        /// Appends EmployeeId to the Url of all remote dialogs
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dlg_PreRender(object sender, EventArgs e)
        {
            Dialog dlg = (Dialog)sender;
            if (!string.IsNullOrEmpty(Request.QueryString["EmployeeId"]))
            {
                dlg.Ajax.Url += string.Format("?EmployeeId={0}", Request.QueryString["EmployeeId"]);
            }

            //Add Terminated Status in querystring when DateOfRelieve exists and is is <= current date
            if (_dateOfRelieve.HasValue && (DateTime.Now >= _dateOfRelieve.Value))
            {
                dlg.Ajax.Url += "&Terminated=1";
            }
        }

        #endregion

        #region WebMethods

        /// <summary>
        /// Method to undo Termination
        /// </summary>
        /// <param name="employeeId"></param>
        [WebMethod]
        public static void UndoTermination(int employeeId)
        {
            using (PISDataContext db = new PISDataContext(ReportingUtilities.DefaultConnectString))
            {
                Employee emp = db.Employees.Single(p => p.EmployeeId == employeeId);
                emp.RelieveOrderDate = null;
                emp.RelieveOrderNo = null;
                emp.LeavingReason = null;
                emp.DateOfRelieve = null;
                emp.EmployeeStatusId = null;
                db.SubmitChanges();
            }
        }

        /// <summary>
        /// If a ServicePeriod is deleted correspondingly delete all its child relations
        /// tables
        /// </summary>
        /// <param name="servicePeriodId"></param>
        [WebMethod]
        public static void DeleteServicePeriod(int servicePeriodId)
        {
            using (PISDataContext db = new PISDataContext(ReportingUtilities.DefaultConnectString))
            {
                ServicePeriod sp = db.ServicePeriods.Single(p => p.ServicePeriodId == servicePeriodId);
                db.ServicePeriods.DeleteOnSubmit(sp);

                //Delete corresponding Training,Leave and MedicalRecord of the current ServicePeriod
                db.LeaveRecords.DeleteAllOnSubmit(sp.LeaveRecords);
                db.MedicalRecords.DeleteAllOnSubmit(sp.MedicalRecords);
                db.Trainings.DeleteAllOnSubmit(sp.Trainings);

                db.SubmitChanges();
            }
        }

        /// <summary>
        /// If an Employee is deleted correspondingly delete all its child relations
        /// tables
        /// </summary>
        /// <param name="employeeId"></param>
        [WebMethod]
        public static void DeleteEmployee(int employeeId)
        {
            using (PISDataContext db = new PISDataContext(ReportingUtilities.DefaultConnectString))
            {
                Employee emp = db.Employees.Single(p => p.EmployeeId == employeeId);

                db.ServicePeriods.DeleteAllOnSubmit(emp.ServicePeriods);
                db.Trainings.DeleteAllOnSubmit(emp.ServicePeriods.SelectMany(p => p.Trainings));
                db.LeaveRecords.DeleteAllOnSubmit(emp.ServicePeriods.SelectMany(p => p.LeaveRecords));
                db.MedicalRecords.DeleteAllOnSubmit(emp.ServicePeriods.SelectMany(p => p.MedicalRecords));
                db.Nominees.DeleteAllOnSubmit(emp.Nominees);
                db.FamilyMembers.DeleteAllOnSubmit(emp.FamilyMembers);
                db.Qualifications.DeleteAllOnSubmit(emp.Qualifications);
                db.EmployeeGrants.DeleteAllOnSubmit(emp.EmployeeGrants);

                db.Employees.DeleteOnSubmit(emp);

                db.SubmitChanges();
            }
        }

        #endregion
    }
}
