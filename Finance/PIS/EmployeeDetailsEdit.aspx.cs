using System;
using System.Linq;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using Eclipse.PhpaLibrary.Database.PIS;
using Eclipse.PhpaLibrary.Reporting;
using Eclipse.PhpaLibrary.Web;
using EclipseLibrary.Web.JQuery;
using EclipseLibrary.Web.JQuery.Input;
using EclipseLibrary.Web.UI;

namespace Finance.PIS
{
    /// <summary>
    /// Pass EmployeeId as query string
    /// Optionally pass ActiveTab in query string to determine which tab should be
    /// made active. PE -> Personal; FI -> Financial; PR -> professional; SP -> Service Period;TE -> Termination
    /// If EmployeeId is not passed, it allows insertion of a new employee.
    /// Click btnUpdate to perform the Update/Insert.
    /// After succesful insert/update returns response code 205 with employee id in data
    /// </summary>
    public partial class EmployeeDetailsEdit : PageBase
    {
        #region Load Events

        /// <summary>
        /// Switch to insert mode if queryString has no EmployeeId
        /// </summary>
        /// <param name="e"></param>
        protected override void OnLoad(EventArgs e)
        {
            if (!IsPostBack && string.IsNullOrEmpty(this.Request.QueryString["EmployeeId"]))
            {
                fvEmployeeDetailsEdit.ChangeMode(FormViewMode.Insert);
            }
            base.OnLoad(e);
        }

        /// <summary>
        /// Decide which tab should show up initiallyfrom QueryString value
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <remarks>
        /// QueryString: PR -> Professional, FI -> Financial, PE -> Personnel, SP -> Service Period
        /// </remarks>
        protected void tabs_Load(object sender, EventArgs e)
        {
            Tabs tabs = (Tabs)sender;
            switch (this.Request.QueryString["ActiveTab"])
            {
                case "PR":
                    tabs.Selected = 0;
                    break;

                case "FI":
                    tabs.Selected = 1;
                    break;

                case "PE":
                    tabs.Selected = 2;
                    break;

                case "SP":
                    tabs.Selected = 3;
                    break;

                default:
                    break;
            }
        }

        #endregion

        /// <summary>
        /// Find SubDivisionId, OfficeId and make them selected
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void fvEmployeeDetailsEdit_ItemCreated(object sender, EventArgs e)
        {
            DropDownListEx ddlSubDivision = (DropDownListEx)fvEmployeeDetailsEdit.FindControl("ddlSubDivision");
            DropDownListEx ddlOffices = (DropDownListEx)fvEmployeeDetailsEdit.FindControl("ddlOffices");

            if (fvEmployeeDetailsEdit.CurrentMode == FormViewMode.Edit)
            {
                using (PISDataContext db = new PISDataContext(ReportingUtilities.DefaultConnectString))
                {
                    Employee emp = db.Employees
                         .Where(p => p.EmployeeId == Convert.ToInt32(fvEmployeeDetailsEdit.DataKey.Value))
                         .Single();

                    if (emp.SubDivisionId.HasValue)
                    {
                        ddlSubDivision.Items.Add(new DropDownItem()
                        {
                            Text = emp.SubDivision.SubDivisionName,
                            Value = emp.SubDivisionId.ToString(),
                            Persistent = DropDownPersistenceType.WhenEmpty
                        });

                        ddlSubDivision.Value = emp.SubDivisionId.Value.ToString();
                    }

                    if (emp.OfficeId.HasValue)
                    {
                        ddlOffices.Items.Add(new DropDownItem()
                        {
                            Text = emp.Office.OfficeName,
                            Value = emp.OfficeId.Value.ToString(),
                            Persistent = DropDownPersistenceType.WhenEmpty
                        });

                        ddlOffices.Value = emp.OfficeId.Value.ToString();
                    }

                }
            }

        }

        #region Inserting Employee

        /// <summary>
        /// Take care of blood group and marital status
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dsEmployee_Inserting(object sender, LinqDataSourceInsertEventArgs e)
        {
            if (e.Exception == null)
            {
                _emp = (Employee)e.NewObject;
                FormView fvServiceHistory = (FormView)fvEmployeeDetailsEdit.FindControl("fvServiceHistory");
                HandleBloodGroup();
                HandleMaritalStatus();
                _okToInsertUpdateServicePeriod = false;
                UpdateInsertServicePeriod();
            }
        }

        /// <summary>
        /// Remember the id of the employee which has been inserted
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <remarks>
        /// Insert service period as well
        /// </remarks>
        protected void dsEmployee_Inserted(object sender, LinqDataSourceStatusEventArgs e)
        {
            if (e.Exception == null)
            {
                _emp = (Employee)e.Result;
               
                _okToInsertUpdateServicePeriod = true;
                UpdateInsertServicePeriod();
            }
        }

        /// <summary>
        /// In case of exception, stay in insert mode
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void fvEmployeeDetailsEdit_ItemInserted(object sender, FormViewInsertedEventArgs e)
        {
            if (e.Exception != null)
            {
                EmployeeDetailsEdit_spEmpEdit.AddErrorText(e.Exception.Message);
                e.ExceptionHandled = true;
                e.KeepInInsertMode = true;
            }
        }
        #endregion

        #region Updating Employee

        /// <summary>
        /// The employee being inserted/updated. This is set in the Inserting/Updating
        /// event.
        /// </summary>
        private Employee _emp;

        /// <summary>
        /// Service period inserting/updating events cancel when this is false
        /// </summary>
        private bool _okToInsertUpdateServicePeriod;

        /// <summary>
        /// Take care of blood group and marital status.
        /// Also set SubDivisionId and OfficeId
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dsEmployee_Updating(object sender, LinqDataSourceUpdateEventArgs e)
        {
            if (e.Exception == null)
            {
                
                _emp = (Employee)e.NewObject;
                FormView fvServiceHistory = (FormView)fvEmployeeDetailsEdit.FindControl("fvServiceHistory");
                DropDownListEx ddlSubDivision = (DropDownListEx)fvEmployeeDetailsEdit.FindControl("ddlSubDivision");
                if (!string.IsNullOrEmpty(ddlSubDivision.Value))
                {
                    _emp.SubDivisionId = Convert.ToInt32(ddlSubDivision.Value);
                }

                DropDownListEx ddlOffices = (DropDownListEx)fvEmployeeDetailsEdit.FindControl("ddlOffices");
                if (!string.IsNullOrEmpty(ddlOffices.Value))
                {
                    _emp.OfficeId = Convert.ToInt32(ddlOffices.Value);
                }

                HandleBloodGroup();
                HandleMaritalStatus();
                _okToInsertUpdateServicePeriod = false;
                UpdateInsertServicePeriod();
            }
        }

        /// <summary>
        /// Remember the employee id which has been updated
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <remarks>
        /// Update service period as well. If the current service period does not exist yet, insert it now
        /// </remarks>
        protected void dsEmployee_Updated(object sender, LinqDataSourceStatusEventArgs e)
        {
            if (e.Exception == null)
            {
                _okToInsertUpdateServicePeriod = true;
                UpdateInsertServicePeriod();
            }
            else
            {
                EmployeeDetailsEdit_spEmpEdit.AddErrorText(e.Exception.Message);
                e.ExceptionHandled = true;
            }
        }

        #endregion

        #region Insert/Update Helpers
        private void HandleMaritalStatus()
        {
            DropDownSuggest ddlMaritalStatus = (DropDownSuggest)fvEmployeeDetailsEdit.FindControl("ddlMaritalStatus");
            if (string.IsNullOrEmpty(ddlMaritalStatus.Value) && !string.IsNullOrEmpty(ddlMaritalStatus.TextBox.Text))
            {
                MaritalStatus ms = new MaritalStatus()
                {
                    MaritalStatusType = ddlMaritalStatus.TextBox.Text
                };

                using (PISDataContext db = new PISDataContext(ReportingUtilities.DefaultConnectString))
                {
                    db.MaritalStatus.InsertOnSubmit(ms);
                    db.SubmitChanges();
                }
                _emp.MaritalStatusId = ms.MaritalStatusId;
            }
        }

        private void HandleBloodGroup()
        {
            DropDownSuggest ddlBloodGroup = (DropDownSuggest)fvEmployeeDetailsEdit.FindControl("ddlBloodGroup");
            //PISDataContext db = (PISDataContext)dsEmployee.Database;
            if (string.IsNullOrEmpty(ddlBloodGroup.Value) && !string.IsNullOrEmpty(ddlBloodGroup.TextBox.Text))
            {
                // Need to insert
                BloodGroup bg = new BloodGroup()
                {
                    BloodGroupType = ddlBloodGroup.TextBox.Text
                };
                // Blood group gets inserted outside transaction
                using (PISDataContext db = new PISDataContext(ReportingUtilities.DefaultConnectString))
                {
                    db.BloodGroups.InsertOnSubmit(bg);
                    db.SubmitChanges();
                }
                _emp.BloodGroupId = bg.BloodGroupId;
            }
            return;
        }

        /// <summary>
        /// Calls UpdateItem or InsertItem
        /// </summary>
        private void UpdateInsertServicePeriod()
        {
            FormView fvServiceHistory = (FormView)fvEmployeeDetailsEdit.FindControl("fvServiceHistory");
            switch (fvServiceHistory.CurrentMode)
            {
                case FormViewMode.Edit:
                    fvServiceHistory.UpdateItem(false);
                    break;

                case FormViewMode.Insert:
                    fvServiceHistory.InsertItem(false);
                    break;

                default:
                    throw new NotImplementedException();
            }
        }
        #endregion

        #region Selecting

        /// <summary>
        /// Cancels the query when employee id not available
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <remarks>
        /// Called for service period data source as well because cancelling that query is important when
        /// a new employee is being added.
        /// </remarks>
        protected void ds_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            if (e.WhereParameters["EmployeeId"] == null)
            {
                e.Cancel = true;
            }
        }
        #endregion

        #region DropDownSuggestDataSource Events

        protected void dsReligion_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            PhpaLinqDataSource dsReligion = (PhpaLinqDataSource)fvEmployeeDetailsEdit.FindControl("dsReligion");
            PISDataContext db = (PISDataContext)dsReligion.Database;
            e.Result = (from rel in db.Employees
                        where rel.Religion != null
                        orderby rel.Religion
                        select rel.Religion).Distinct();
        }
        protected void dsGrade_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            FormView fvServiceHistory = (FormView)fvEmployeeDetailsEdit.FindControl("fvServiceHistory");
            PhpaLinqDataSource dsGrade = (PhpaLinqDataSource)fvServiceHistory.FindControl("dsGrade");
            PISDataContext db = (PISDataContext)dsGrade.Database;
            e.Result = (from sp in db.ServicePeriods
                        where sp.Grade != null
                        orderby sp.Grade
                        select sp.Grade).Distinct();
        }

        protected void dsPostedAt_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            FormView fvServiceHistory = (FormView)fvEmployeeDetailsEdit.FindControl("fvServiceHistory");
            PhpaLinqDataSource dsPostedAt = (PhpaLinqDataSource)fvServiceHistory.FindControl("dsPostedAt");
            PISDataContext db = (PISDataContext)dsPostedAt.Database;
            e.Result = (from sp in db.Employees
                        where sp.PostedAt != null && sp.PostedAt !=string.Empty
                        orderby sp.PostedAt
                        select sp.PostedAt).Distinct().ToArray();
        }

        protected void dsDesignation_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            FormView fvServiceHistory = (FormView)fvEmployeeDetailsEdit.FindControl("fvServiceHistory");
            PhpaLinqDataSource dsDesignation = (PhpaLinqDataSource)fvServiceHistory.FindControl("dsDesignation");
            PISDataContext db = (PISDataContext)dsDesignation.Database;
            e.Result = (from emp in db.Employees
                        where emp.Designation != null
                        orderby emp.Designation
                        select emp.Designation).Distinct();
        }

        #endregion

        #region Update button click

        /// <summary>
        /// Single data context used for all inserts and deletes
        /// </summary>
        private PISDataContext _db;

        /// <summary>
        /// Insert/Update Employee and return the inserted/updated EmployeeId
        /// on success. All operations are performed in a single tranaction
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            if (!btnUpdate.IsPageValid())
            {
                return;
            }
            
            using (_db = new PISDataContext(ReportingUtilities.DefaultConnectString))
            {
                _db.Connection.Open();
                _db.Transaction = _db.Connection.BeginTransaction();
                switch (fvEmployeeDetailsEdit.CurrentMode)
                {
                    case FormViewMode.Edit:
                        fvEmployeeDetailsEdit.UpdateItem(false);
                        break;

                    case FormViewMode.Insert:
                        fvEmployeeDetailsEdit.InsertItem(false);
                        break;

                    default:
                        throw new NotImplementedException();
                }

                if (EmployeeDetailsEdit_spEmpEdit.HasErrorText)
                {
                    _db.Transaction.Rollback();
                }
                else
                {
                    _db.Transaction.Commit();
                    if (JQueryScriptManager.IsAjaxCall)
                    {
                        this.Response.ContentType = "application/json";
                        JavaScriptSerializer ser = new JavaScriptSerializer();
                        string s = ser.Serialize(_emp.EmployeeId);
                        this.Response.StatusCode = 205;
                        this.Response.Write(s);
                        this.Response.End();
                    }
                }
            }
            _db = null;
        }

        /// <summary>
        /// Use the same data context for all insert/update/delete
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ds_ContextCreating(object sender, LinqDataSourceContextEventArgs e)
        {
            switch (e.Operation)
            {
                case DataSourceOperation.Delete:
                case DataSourceOperation.Insert:
                case DataSourceOperation.Update:
                    e.ObjectInstance = _db;
                    break;

                case DataSourceOperation.Select:
                case DataSourceOperation.SelectCount:
                default:
                    break;
            }
        }

        #endregion

        #region Service Period Insert/Update

        /// <summary>
        /// Switch to insert mode if there is no current service period
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <remarks>
        /// Set the default value initial term to 3 years
        /// </remarks>
        protected void fvServiceHistory_DataBound(object sender, EventArgs e)
        {
            FormView fvServiceHistory = (FormView)sender;
            if (fvServiceHistory.DataItem == null)
            {
                if (fvServiceHistory.CurrentMode == FormViewMode.Insert)
                {
                    TextBoxEx tbInitialTerm = (TextBoxEx)fvServiceHistory.FindControl("tbInitialTerm");
                    tbInitialTerm.Text = "3 years";

                    TextBoxEx tbPeriodStartDate = (TextBoxEx)fvServiceHistory.FindControl("tbPeriodStartDate");
                    tbPeriodStartDate.Text = string.Format("{0:d}", DateTime.Now);

                    // Service PeriodEndDate set to default (CurrentDate + 3 years date)
                    TextBoxEx tbPeriodEndDate = (TextBoxEx)fvServiceHistory.FindControl("tbPeriodEndDate");
                    tbPeriodEndDate.Text = string.Format("{0:d}", DateTime.Now.AddDays(1095));
                }
                else
                {
                    fvServiceHistory.ChangeMode(FormViewMode.Insert);
                }
            }
        }

        /// <summary>
        /// Set the EmployeeId in ServicePeriod at the time of inserting a
        /// new employee
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dsServiceHistory_Inserting(object sender, LinqDataSourceInsertEventArgs e)
        {
            ServicePeriod sp = (ServicePeriod)e.NewObject;
            if (_okToInsertUpdateServicePeriod)
            {
                sp.EmployeeId = _emp.EmployeeId;
            }
            else
            {
                UpdateEmployeeColumns(sp);
                e.Cancel = true;
            }

        }


        protected void dsServiceHistory_Updating(object sender, LinqDataSourceUpdateEventArgs e)
        {
            ServicePeriod sp = (ServicePeriod)e.NewObject;
            if (!_okToInsertUpdateServicePeriod)
            {
                UpdateEmployeeColumns(sp);
                e.Cancel = true;
            }
        }

        private void UpdateEmployeeColumns(ServicePeriod sp)
        {
            _emp.Designation = sp.Designation;
            _emp.Grade = sp.Grade;
        }

        protected void fvServiceHistory_ItemInserted(object sender, FormViewInsertedEventArgs e)
        {
            if (e.Exception != null)
            {
                EmployeeDetailsEdit_spEmpEdit.AddErrorText(e.Exception.Message);
                e.ExceptionHandled = true;
            }
            else if (!_okToInsertUpdateServicePeriod)
            {
                e.KeepInInsertMode = true;
            }
        }

        protected void fvServiceHistory_ItemUpdated(object sender, FormViewUpdatedEventArgs e)
        {
            if (e.Exception != null)
            {
                EmployeeDetailsEdit_spEmpEdit.AddErrorText(e.Exception.Message);
                e.ExceptionHandled = true;
            }
        }

        #endregion

        #region Web Methods

        [WebMethod]
        public static DropDownItem[] GetSubDivisions(string[] parentKeys)
        {
            using (PISDataContext db = new PISDataContext(ReportingUtilities.DefaultConnectString))
            {
                var query = (from subDivision in db.SubDivisions
                             where subDivision.DivisionId == int.Parse(parentKeys[0])
                             select new DropDownItem()
                             {
                                 Text = subDivision.SubDivisionName,
                                 Value = subDivision.SubDivisionId.ToString()
                             }).ToArray();
                return query;
            }
        }

        [WebMethod]
        public static DropDownItem[] GetOffices(string[] parentKeys)
        {
            using (PISDataContext db = new PISDataContext(ReportingUtilities.DefaultConnectString))
            {
                int? _subDivisionId = Convert.ToInt32(parentKeys[1]);

                var query = (from office in db.Offices
                             where office.SubDivisionId == _subDivisionId

                             select new DropDownItem()
                             {
                                 Text = office.OfficeName,
                                 Value = office.OfficeId.ToString()
                             }).ToArray();

                return query;
            }
        }

        #endregion
    }
}
