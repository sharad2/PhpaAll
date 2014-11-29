using Eclipse.PhpaLibrary.Database.Payroll;
using Eclipse.PhpaLibrary.Reporting;
using Eclipse.PhpaLibrary.Web;
using EclipseLibrary.Web.JQuery.Input;
using EclipseLibrary.Web.UI;
/*
 *  Programming: Eclipse Systems (P) Ltd., NOIDA, INDIA
 *  E-mail: support@eclsys.com
 *
 *  $Revision: 38557 $
 *  $Author: ssingh $
 *  $Date: 2010-12-06 12:44:57 +0530 (Mon, 06 Dec 2010) $
 *  $Id: SalaryPeriods.aspx.cs 38557 2010-12-06 07:14:57Z ssingh $
 */
using System;
using System.Data.Linq;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Finance.Payroll
{
    public partial class SalaryPeriods : PageBase
    {
        //private int _salaryPeriod; 

        /// <summary>
        /// The default filter for tbToDate had to be increased by three months ahead else
        /// the entered salary period is not visible by default.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnLoad(EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                tbFromDate.Value = DateTime.Now.AddMonths(-6).MonthStartDate().ToShortDateString();
                tbToDate.Value = DateTime.Now.AddMonths(3).MonthStartDate().ToShortDateString();
            }
            base.OnLoad(e);
        }
        /// <summary>
        /// Form view displays nothing if nothing selected in grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dsSpecificSalaryPeriod_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            if (e.WhereParameters["SalaryPeriodId"] == null)
            {
                e.Cancel = true;
            }
        }


        /// <summary>
        /// Making Hybrid panel always visible, even in case it is closed manually.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvSalaryPeriod_SelectedIndexChanged(object sender, EventArgs e)
        {
            dsSpecificSalaryPeriod.WhereParameters["SalaryPeriodId"].DefaultValue = gvSalaryPeriod.SelectedDataKey["SalaryPeriodId"].ToString();
            if (frmSalaryPeriod.CurrentMode == FormViewMode.Insert)
                frmSalaryPeriod.ChangeMode(FormViewMode.ReadOnly);
            dlgEditor.Visible = true;
        }

        /// <summary>
        /// After insertion grid is getting refreshed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void frmSalaryPeriod_ItemInserted(object sender, FormViewInsertedEventArgs e)
        {
            if (e.Exception == null)
            {
                frmSalaryPeriod.ChangeMode(FormViewMode.ReadOnly);
                gvSalaryPeriod.DataBind();
            }
        }

        protected void frmSalaryPeriod_ItemUpdated(object sender, FormViewUpdatedEventArgs e)
        {
            gvSalaryPeriod.DataBind();
        }

        /// <summary>
        /// Binding Gridview when an item is deleted to refresh it.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void frmSalaryPeriod_ItemDeleted(object sender, FormViewDeletedEventArgs e)
        {
            gvSalaryPeriod.DataBind();
            gvSalaryPeriod.SelectedIndex = 0;
        }

        /// <summary>
        /// Set the default values for salary period object such as start date,end date, code..
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void frmSalaryPeriod_ItemCreated(object sender, EventArgs e)
        {
            switch (frmSalaryPeriod.CurrentMode)
            {
                case FormViewMode.Insert:
                    TextBoxEx tbFromDateEdit = (TextBoxEx)frmSalaryPeriod.FindControl("tbFromDateEdit");
                    TextBoxEx tbToDateEdit = (TextBoxEx)frmSalaryPeriod.FindControl("tbToDateEdit");
                    tbFromDateEdit.Value = ReportingUtilities.MonthStartDate(DateTime.Now).ToShortDateString();
                    tbToDateEdit.Value = ReportingUtilities.MonthEndDate(DateTime.Now).ToShortDateString();

                    TextBoxEx tbCode = (TextBoxEx)frmSalaryPeriod.FindControl("tbsalaryPeriodCode");
                    tbCode.Text = string.Format("{0:MMMM'.'yyyy}", tbFromDateEdit.ValueAsDate).ToUpper();

                    TextBoxEx tbPeriodDescription = (TextBoxEx)frmSalaryPeriod.FindControl("tbPeriodDescription");
                    tbPeriodDescription.Text = string.Format("Salary for the month of {0:MMMM yyyy}", tbFromDateEdit.ValueAsDate);
                    break;

                case FormViewMode.Edit:
                    CheckBoxEx chkAllEmp = (CheckBoxEx)frmSalaryPeriod.FindControl("chkAllEmp");
                    chkAllEmp.Visible = false;
                    DropDownListEx ddlBankName = (DropDownListEx)frmSalaryPeriod.FindControl("ddlBankName");
                    ddlBankName.Visible = false;
                    LeftLabel lblEmpType = (LeftLabel)frmSalaryPeriod.FindControl("lblEmpType");
                    lblEmpType.Visible = false;
                    DropDownListEx ddlEmpType = (DropDownListEx)frmSalaryPeriod.FindControl("ddlEmpType");
                    ddlEmpType.Visible = false;
                    break;
            }
        }

        protected void dsSpecificSalaryPeriod_Deleting(object sender, LinqDataSourceDeleteEventArgs e)
        {
            PayrollDataContext db = (PayrollDataContext)dsSpecificSalaryPeriod.Database;
            SalaryPeriod sp = (SalaryPeriod)e.OriginalObject;
            var query = from pea in db.PeriodEmployeeAdjustments
                        where pea.EmployeePeriod.SalaryPeriodId == sp.SalaryPeriodId
                        select pea;

            foreach (PeriodEmployeeAdjustment pea in query)
            {
                db.EmployeePeriods.DeleteOnSubmit(pea.EmployeePeriod);
                db.PeriodEmployeeAdjustments.DeleteOnSubmit(pea);
            }

        }
        
        //Perform bulk operation.
        protected void dsSpecificSalaryPeriod_Inserting(object sender, LinqDataSourceInsertEventArgs e)
        {
            if (e.Exception != null)
            {
                return;
            }
            //get inserted salary period.
            SalaryPeriod sp = (SalaryPeriod)e.NewObject;
            using (PayrollDataContext db = new PayrollDataContext(dsSpecificSalaryPeriod.Database.Connection.ConnectionString))
            {
                DataLoadOptions dlo = new DataLoadOptions();
                dlo.LoadWith<Employee>(emp => emp.EmployeeAdjustments);
                db.LoadOptions = dlo;                
                CheckBoxEx chkAllEmp = (CheckBoxEx)frmSalaryPeriod.FindControl("chkAllEmp");
                DropDownListEx ddlEmpType = (DropDownListEx)frmSalaryPeriod.FindControl("ddlEmpType");
                DropDownListEx ddlBankName = (DropDownListEx)frmSalaryPeriod.FindControl("ddlBankName");
                if (chkAllEmp.Checked)
                {
                    if (ddlEmpType.Value != string.Empty)
                    {
                        //Get all employees whose  adjustment already defined and for a bank selected
                        var employees = from emp in db.Employees
                                        where emp.EmployeeAdjustments.Any() &&
                                        (emp.Bank.BankId == Convert.ToInt32(ddlBankName.Value)
                                         && emp.EmployeeType.EmployeeTypeCode == ddlEmpType.Value)
                                         && emp.DateOfRelieve == null
                                        select emp;

                        foreach (Employee emp in employees)
                        {
                            EmployeePeriod ep = new EmployeePeriod();
                            ep.AddDefaultAdjustments(emp);
                            sp.EmployeePeriods.Add(ep);
                        }
                    }
                    else
                    {
                        var employees = from emp in db.Employees
                                        where emp.EmployeeAdjustments.Any()
                                        && emp.Bank.BankId == Convert.ToInt32(ddlBankName.Value)
                                        && emp.DateOfRelieve == null
                                        select emp;

                        foreach (Employee emp in employees)
                        {
                            EmployeePeriod ep = new EmployeePeriod();
                            ep.AddDefaultAdjustments(emp);
                            sp.EmployeePeriods.Add(ep);
                        }
                    }
                }
            }
        }

        protected void dsSpecificSalaryPeriod_Inserted(object sender, LinqDataSourceStatusEventArgs e)
        {
            if (e.Exception == null)
            {
                SalaryPeriod sal = (SalaryPeriod)e.Result;
               
                this.dsSpecificSalaryPeriod.WhereParameters["SalaryPeriodId"].DefaultValue = sal.SalaryPeriodId.ToString();
                gvSalaryPeriod.DataBind();
            }
        }

        //Select the query for report.
        /// <summary>
        /// Ritesh:18 Jan 2012
        /// Showing salary periods belogning to logged in user's station 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dsSalaryPeriod_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            PayrollDataContext db = (PayrollDataContext)dsSalaryPeriod.Database;
            var stations = this.GetUserStations();
            var query = db.SalaryPeriods.Where(p =>
                (tbFromDate.ValueAsDate == null || p.SalaryPeriodStart >= tbFromDate.ValueAsDate) &&
                                (tbToDate.ValueAsDate == null || p.SalaryPeriodEnd <= tbToDate.ValueAsDate));
            if (stations != null)
            {
                query = query.Where(p => p.StationId == null || stations.Contains(p.StationId.Value));
            }
            var query2 = from sp in query
                         orderby sp.SalaryPeriodStart descending
                         select new
                         {
                             SalaryPeriodCode = sp.SalaryPeriodCode,
                             description = sp.Description,
                             SalaryPeriodId = sp.SalaryPeriodId,
                             SalaryPeriodStart = sp.SalaryPeriodStart,
                             PayableDate = sp.PayableDate,
                             PaidDate = sp.PaidDate,
                             SalaryPeriodEnd = sp.SalaryPeriodEnd,
                             TotalEmployees = sp.EmployeePeriods.Count(),
                             NetPay = sp.EmployeePeriods.Sum(ep =>
                                 ep.BasicPay +
                                 ep.PeriodEmployeeAdjustments.Sum(pea => pea.Adjustment.IsDeduction ? -pea.Amount : pea.Amount)
                                ),
                               StationName=sp.Station.StationName
                         };

            e.Result = query2.ToList();
        }

        protected void btnLookUp_Click(object sender, EventArgs e)
        {
            ButtonEx btn = (ButtonEx)sender;
            if (btn.IsPageValid())
            {
                gvSalaryPeriod.PageIndex = 0;
                gvSalaryPeriod.DataBind();
            }
        }

        protected void btnDelete_PreRender(object sender, EventArgs e)
        {
            SalaryPeriod sp = (SalaryPeriod)frmSalaryPeriod.DataItem;
            LinkButtonEx lbl = (LinkButtonEx)sender;
            if (sp != null)
            {
                lbl.Enabled = string.IsNullOrEmpty(sp.PaidDate.ToString());
                if (!lbl.Enabled)
                {
                    lbl.ToolTip = "This period is paid, You can not " + lbl.Text;
                }
                else
                {
                    object EmpCount = DataBinder.Eval(frmSalaryPeriod.DataItem, "EmployeePeriods.Count");
                    hfDeleteConfirmMessage.Value = string.Format("Period contains {0} employee's. Do you want to continue anyways?", EmpCount);
                }
            }
        }
        /// <summary>
        /// Ritesh 01 Dec 2011
        /// Bank filter provided
        /// Bank name is provided to create salary period of employees belonging to particular bank.
        /// Showing banks of user's station only
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dsBankName_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            using (PayrollDataContext db = new PayrollDataContext(ReportingUtilities.DefaultConnectString))
            {
                var stations = this.GetUserStations();

                var query = (from bnk in db.Banks
                             where bnk.BankName != null && bnk.BankName != string.Empty
                             orderby bnk.BankName
                             select bnk).Distinct().ToList();
                if (stations != null)
                {
                    query=query.Where(p => p.StationId == null || stations.Contains(p.StationId.Value)).ToList();
                }
                e.Result = query;
            }
        }
        /// <summary>
        /// Ritesh 6th Feb 2011
        /// User has to provide station as well while creating salary period
        /// Drop down shows station on which user has rights
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dsStation_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            using (PayrollDataContext db = new PayrollDataContext(ReportingUtilities.DefaultConnectString))
            {
                var stations = this.GetUserStations();
                var query = (from station in db.Stations
                             where station.StationName != null && station.StationName != string.Empty
                             orderby station.StationName
                             select station).ToList();
                if (stations != null)
                {
                    query = query.Where(p => p.StationName == null || stations.Contains(p.StationId)).ToList();
                }
                e.Result = query;
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            frmSalaryPeriod.DeleteItem();
        }

        protected void btnEdit_Click(object sender, EventArgs e)
        {
            frmSalaryPeriod.ChangeMode(FormViewMode.Edit);
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            frmSalaryPeriod.ChangeMode(FormViewMode.ReadOnly);
        }

        protected void btnNew_Click(object sender, EventArgs e)
        {
            frmSalaryPeriod.ChangeMode(FormViewMode.Insert);
            dlgEditor.Visible = true;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            switch (frmSalaryPeriod.CurrentMode)
            {
                case FormViewMode.Insert:
                    frmSalaryPeriod.InsertItem(false);
                    break;
                case FormViewMode.Edit:
                    frmSalaryPeriod.UpdateItem(false);
                    break;
            }
        }

        protected void gvSalaryPeriod_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //switch (e.Row.RowType)
            //{
            //    case DataControlRowType.DataRow:
            //        if (_salaryPeriod != -1)
            //        {
            //            SalaryPeriod sal = (SalaryPeriod)e.Row.DataItem;
            //            if (sal.SalaryPeriodId == _salaryPeriod)
            //            {
            //                e.Row.RowState |= DataControlRowState.Selected;
            //            }
            //        }
            //        break;
            //}
        }
    }
}
