using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Eclipse.PhpaLibrary.Database.Payroll;
using Eclipse.PhpaLibrary.Web;
using EclipseLibrary.Web.JQuery;
using EclipseLibrary.Web.JQuery.Input;
using EclipseLibrary.Web.UI;
using Num2Wrd;


namespace Finance.Payroll
{
    // Take querystring variable "PeriodId".

    public partial class ManageEmployeePeriod : PageBase
    {

        int? m_PeriodId;
        int? m_EmployeeId;
        public const string strFormat = "###,###,###,##0.00;(###,###,###,##0.00);#";
        SalaryPeriod m_sp;

        /// <summary>
        /// save the PeriodId from querystring varaiable
        /// and set the period in header.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (btnPeriod.IsPageValid())
            {
                m_PeriodId = Convert.ToInt32(tbPeriod.Value);
                PayrollDataContext dbEmployeePeriod = (PayrollDataContext)dsEmployeePeriod.Database;
                DataLoadOptions dlo = new DataLoadOptions();
                dlo.LoadWith<EmployeePeriod>(ep => ep.Employee);
                dlo.LoadWith<EmployeePeriod>(ep => ep.PeriodEmployeeAdjustments);
                dlo.LoadWith<PeriodEmployeeAdjustment>(pea => pea.Adjustment);

                dbEmployeePeriod.LoadOptions = dlo;

                m_sp = (from sp in dbEmployeePeriod.SalaryPeriods
                        where sp.SalaryPeriodId == m_PeriodId
                        select sp).SingleOrDefault();
                tbPeriod.Text = m_sp.SalaryPeriodCode.ToString();
            }
        }

        /// <summary>
        /// Make the Hybrid Drag Panel always visible.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnLookUp_Click(object sender, EventArgs e)
        {
            ButtonEx btn = (ButtonEx)sender;
            if (btn.IsPageValid())
            {
                m_EmployeeId = Convert.ToInt32(tbEmployee.Value);
                gvEmployeesForperiod.DataBind();
            }
        }

        /// <summary>
        /// Show All employee of Particular salary period 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnShowAllemployee_Click(object sender, EventArgs e)
        {
            tbEmployee.Text = null;
            tbEmployee.Value = null;
            m_EmployeeId = null;
            gvEmployeesForperiod.DataBind();
        }

        /// <summary>
        /// Show the results based on SalaryPeriodId
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnPeriod_Click(object sender, EventArgs e)
        {
            ButtonEx btn = (ButtonEx)sender;
            if (btn.IsPageValid())
            {
                gvEmployeesForperiod.DataBind();
                gvEmployeesForperiod.SelectedIndex = -1;
                dlgEditor.Visible = false;
            }
        }

        /// <summary>
        /// This button is to validate that the users must have selected the Period and Emplyoee
        /// then only insert operation can be performed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAddNewEmployeeForPeriod_Click(object sender, EventArgs e)
        {
            ButtonEx btn = (ButtonEx)sender;
            EclipseLibrary.Web.JQuery.Input.ValidationSummary valSummary = (EclipseLibrary.Web.JQuery.Input.ValidationSummary)btn.FindControl("valSummary");
            bool _ins = false;

            if (btnPeriod.IsPageValid() == false )
            {
                valSummary.ErrorMessages.Add(string.Format("{0} is required", tbPeriod.FriendlyName));
                _ins = false;
            }
            else
            {
                _ins = true;
            }

            if (btnLookUp.IsPageValid() == false)
            {
                valSummary.ErrorMessages.Add(string.Format("{0} is required", tbEmployee.FriendlyName));
                _ins = false;
            }
            else
            {
                if(_ins == true)
                    _ins = true;
            }

            if(_ins == true)
            {
                fvNewEmplyeeforperiod.InsertItem(false);
            }
        }

        /// <summary>
        /// Insert new employee for passed period. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void fvNewEmplyeeforperiod_ItemInserting(object sender, FormViewInsertEventArgs e)
        {
            m_EmployeeId = Convert.ToInt32(tbEmployee.Value);
            m_PeriodId = Convert.ToInt32(tbPeriod.Value);
            PayrollDataContext dbEmployeePeriod = (PayrollDataContext)dsEmployeePeriod.Database;
            FormView fv = (FormView)sender;

            e.Values["SalaryPeriodId"] = m_PeriodId;

            Employee emp = (from em in dbEmployeePeriod.Employees
                            where em.EmployeeId == (int)m_EmployeeId
                            select em).SingleOrDefault();

            e.Values["EmployeeId"] = emp.EmployeeId;
            e.Values["BasicPay"] = emp.BasicSalary;
            e.Values["Designation"] = emp.Designation;
        }

        /// <summary>
        ///  This data source only use for inserting new employee for passed
        ///  period and thus cancel the selecting event in every case.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dsNewEmployeeForPeriod_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            e.Cancel = true;
        }

        /// <summary>
        /// refresh gridview after insertion
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void fvNewEmplyeeforperiod_ItemInserted(object sender, FormViewInsertedEventArgs e)
        {
            if (e.Exception == null)
            {
                gvEmployeesForperiod.DataBind();
            }
        }
        
        protected void dsEditPeriodEmpAdjustments_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            if (e.WhereParameters["EmployeePeriodId"] == null)
            {
                e.Cancel = true;
            }
        }


        /// <summary>
        /// Update the adjustment.for Passed period.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvEditPeriodEmpAdjustments_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            int adjId = Convert.ToInt32(e.Keys["PeriodEmployeeAdjustmentId"]);
            decimal defaultAmount = 0;

            var flatAmt = e.NewValues["Amount"];
            if (flatAmt == null)
            {
                using (PayrollDataContext db = new PayrollDataContext(dsEditPeriodEmpAdjustments.Database.Connection))
                {
                    PeriodEmployeeAdjustment periodEmpAdj = (from pea in db.PeriodEmployeeAdjustments
                                                             where pea.PeriodEmployeeAdjustmentId == adjId
                                                             select pea).SingleOrDefault();
                    EmployeeAdjustment empadj = (from ea in db.EmployeeAdjustments
                                                 where ea.AdjustmentId == periodEmpAdj.AdjustmentId &&
                                                 ea.EmployeeId == periodEmpAdj.EmployeePeriod.EmployeeId
                                                 select ea).SingleOrDefault();
                    if (empadj == null)
                    {
                        if (periodEmpAdj.Adjustment.FractionOfBasic.HasValue && periodEmpAdj.EmployeePeriod.BasicPay.HasValue)
                        {
                            defaultAmount += Convert.ToDecimal(periodEmpAdj.Adjustment.FractionOfBasic.Value) * periodEmpAdj.EmployeePeriod.BasicPay.Value;
                        }
                        if (periodEmpAdj.Adjustment.FlatAmount.HasValue)
                        {
                            defaultAmount += periodEmpAdj.Adjustment.FlatAmount.Value;
                        }
                    }
                    else
                    {
                        if (empadj.FractionOfBasic.HasValue && periodEmpAdj.EmployeePeriod.BasicPay.HasValue)
                        {
                            defaultAmount += Convert.ToDecimal(empadj.FractionOfBasic.Value) * periodEmpAdj.EmployeePeriod.BasicPay.Value;
                        }
                        if (empadj.FlatAmount.HasValue)
                        {
                            defaultAmount += empadj.FlatAmount.Value;
                        }
                    }
                }
                e.NewValues["Amount"] = defaultAmount;
            }
        }

        protected void gvEditPeriodEmpAdjustments_RowInserted(object sender, GridViewInsertedEventArgs e)
        {
            if (e.Exception == null)
            {
                gvEditPeriodEmpAdjustments.DataBind();
            }
            else
            {
                GridViewExInsert gv = (GridViewExInsert)sender;
                EclipseLibrary.Web.JQuery.Input.ValidationSummary valSummaryEditor = (EclipseLibrary.Web.JQuery.Input.ValidationSummary)gv.NamingContainer.FindControl("valSummaryEditor");
                valSummaryEditor.ErrorMessages.Add(e.Exception.Message);
            }
        }

        /// <summary>
        /// Update the adjustment.for Passed period.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvEditPeriodEmpAdjustments_RowInserting(object sender, EclipseLibrary.Web.JQuery.GridViewInsertingEventArgs e)
        {
            if (!btnNew.IsPageValid())
            {
                return;
            }
            try
            {
                decimal amt = 0;
                e.Values["EmployeePeriodId"] = hdEmployeePeriodId.Value.ToString();
                using (PayrollDataContext db = new PayrollDataContext(this.dsEditPeriodEmpAdjustments.Database.Connection.ConnectionString))
                {

                    //Select employee period for employeeid and and basic pay.
                    EmployeePeriod empPeriod = (from ePeriod in db.EmployeePeriods
                                                where ePeriod.EmployeePeriodId == Convert.ToInt32(hdEmployeePeriodId.Value.ToString())
                                                select ePeriod).SingleOrDefault();


                    EmployeeAdjustment empAdj = (from eAdj in db.EmployeeAdjustments
                                                 where eAdj.AdjustmentId == Convert.ToInt32(e.Values["AdjustmentId"]) && eAdj.EmployeeId == empPeriod.EmployeeId
                                                 select eAdj).SingleOrDefault();
                    
                    //If selected adjustment not define for employee in employeeAdjustment
                    if (empAdj == null)
                    {
                        Adjustment adj = (from em in db.Adjustments
                                          where em.AdjustmentId == Convert.ToInt32(e.Values["AdjustmentId"])
                                          select em).SingleOrDefault();
                        var _fractionOfBasic = string.IsNullOrEmpty(adj.FractionOfBasic.ToString()) ? 0 : adj.FractionOfBasic;
                        var _basicPay = string.IsNullOrEmpty(empPeriod.BasicPay.ToString()) ? 0 : empPeriod.BasicPay;
                        var _flatAmount = string.IsNullOrEmpty(adj.FlatAmount.ToString()) ? 0 : adj.FlatAmount;

                        amt = (decimal)(Convert.ToDecimal(_fractionOfBasic) * _basicPay + _flatAmount);
                    }
                    //If selected adjustment  define for employee in employeeAdjustment
                    else
                    {
                        amt = (decimal)(Convert.ToDecimal(empAdj.FractionOfBasic) * empPeriod.BasicPay + empAdj.FlatAmount);
                        e.Values["EmployeeAdjustmentId"] = empAdj.EmployeeAdjustmentId;
                    }
                }

                var flatAmount = e.Values["Amount"];
                if (flatAmount == null)
                {
                    e.Values["Amount"] = amt;
                }
            }
            catch (Exception ex)
            {
                GridViewExInsert gv = (GridViewExInsert)sender;
                EclipseLibrary.Web.JQuery.Input.ValidationSummary valSummaryEditor = (EclipseLibrary.Web.JQuery.Input.ValidationSummary)gv.NamingContainer.FindControl("valSummaryEditor");
                valSummaryEditor.ErrorMessages.Add(ex.Message);
            }
        }


        /// <summary>
        /// This becomes non null when we calcuate the default basic salary amount ourselves.
        /// First contains the key of the row updated, Second contains the amount
        /// </summary>
        private Pair m_updatedAmount;

        /// <summary>
        /// Update Basic Pay of employees for passed period.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvEmployeesForperiod_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            bool bOldIsBasicPayOverridden = (bool)e.OldValues["IsBasicPayOverridden"];
            bool bIsBasicPayOverridden = (bool)e.NewValues["IsBasicPayOverridden"];
            if (bOldIsBasicPayOverridden && !bIsBasicPayOverridden)
            {
                // Was overridden earlier, but not now. So we need to recalculate the default amount
                int employeeAdjustmentId = (int)gvEmployeesForperiod.DataKeys[e.RowIndex].Value;
                PayrollDataContext db = (PayrollDataContext)dsEditPeriodEmpAdjustments.Database;
                e.NewValues["BasicPay"] = (from empadj in db.EmployeePeriods
                                           where empadj.EmployeePeriodId == employeeAdjustmentId
                                           select empadj.Employee.BasicSalary).Single();
                m_updatedAmount = new Pair(e.Keys[0], e.NewValues["BasicPay"]);
            }
        }

        decimal? m_Allowancessum=0.0M;
        decimal? m_Deductionssum=0.0M;
        decimal? m_Netpay = 0.0M;
        Label lbl;
        protected void gvEmployeesForperiod_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            switch (e.Row.RowType)
            {
                case DataControlRowType.Header:
                    e.Row.Cells[1].Text = "";
                    break;
                case DataControlRowType.DataRow:
                    EmployeePeriod ep = (EmployeePeriod)e.Row.DataItem;

                    decimal? deductionSum = ep.PeriodEmployeeAdjustments.Sum(pea => pea.Adjustment.IsDeduction ? Math.Round(pea.Amount??0, MidpointRounding.AwayFromZero) : 0);
                    m_Deductionssum += deductionSum;
                    decimal? allowanceSum = ep.PeriodEmployeeAdjustments.Sum(pea => pea.Adjustment.IsDeduction ? 0 : Math.Round(pea.Amount??0, MidpointRounding.AwayFromZero));
                    m_Allowancessum += allowanceSum;

                    //var adj = ep.PeriodEmployeeAdjustments.Sum(pea => pea.Adjustment.IsDeduction ? pea.Amount : null);
                    LinkButton lnkEarning = (LinkButton)e.Row.FindControl("lnkAllowance");
                    lnkEarning.Text = string.Format("{0:N0}", allowanceSum);

                    LinkButton lnkdeduction = (LinkButton)e.Row.FindControl("lnkDeduction");
                    lnkdeduction.Text = string.Format("{0:N0}", deductionSum);

                    Label lblNetPay = (Label)e.Row.FindControl("lblNetPay");
                    decimal totalEarning = (allowanceSum ?? 0) - (deductionSum ?? 0);
                    decimal netPay = totalEarning + (ep.BasicPay ?? 0);
                    m_Netpay += netPay;

                    lblNetPay.Text = string.Format("{0:N0}", netPay);
                    //lblNetPay.PostBackUrl += string.Format("?PeriodID={0}&EmployeeId={1}", ep.SalaryPeriodId, ep.EmployeeId);

                    if (m_sp != null)
                    {
                        if (!string.IsNullOrEmpty(m_sp.PaidDate.ToString()))
                        {
                            Label lblDel = new Label();
                            lblDel.Text = "Delete";

                            lblDel.ToolTip = "This period is paid, You can not Delete";
                            e.Row.Cells[0].Controls.Clear();
                            e.Row.Cells[0].Controls.Add(lblDel);
                        }
                    }
                    break;

                case DataControlRowType.Footer:
                    lbl = new Label();
                    lbl = (Label)e.Row.FindControl("lblAllowance");
                    lbl.Text = string.Format("{0:N0}", m_Allowancessum);
                    lbl = new Label();
                    lbl = (Label)e.Row.FindControl("lblDeduction");
                    lbl.Text = string.Format("{0:N0}", m_Deductionssum);
                    lbl = new Label();
                    lbl = (Label)e.Row.FindControl("lblNetPay");
                    lbl.Text = string.Format("{0:N0}", m_Netpay);
                    break;
            }
        }

        protected void gvEmployeesForperiod_DataBound(object sender, EventArgs e)
        {
            if (tbPeriod.Value != null && m_sp != null)
            {
                MultiBoundField bank = (MultiBoundField)(from DataControlField col in gvEmployeesForperiod.Columns
                                        where col.AccessibleHeaderText == "Bank"
                                        select col).Single();
                if (string.IsNullOrEmpty(bank.CommonCellText))
                {

                    gvEmployeesForperiod.Caption = string.Format(@"<br/><b>This is a list of employees  who will be paid a salary during the period from
                {0:d} To {1:d}", m_sp.SalaryPeriodStart, m_sp.SalaryPeriodEnd);
                    }
                    else
                    {
                        //bank.Visible = false;
                        string caption = string.Empty;
                        if (m_sp.StationId == null)
                        {
                            caption = string.Format(@"<br/><b>This is a list of employees  who will be paid a salary during the period from
                    {0:d} To {1:d} through {2}</b>", m_sp.SalaryPeriodStart, m_sp.SalaryPeriodEnd, bank.CommonCellText);

                        }
                        else {
                            caption = string.Format(@"<br/><b>This is a list of employees of {3} who will be paid a salary during the period from
                    {0:d} To {1:d} through {2}</b>", m_sp.SalaryPeriodStart, m_sp.SalaryPeriodEnd, bank.CommonCellText, m_sp.Station.StationName);
                        }
                        gvEmployeesForperiod.Caption = caption;
                    }
                pnlMessage.Visible = true;
                lblamount.Text = string.Format("{0:N0}", m_Netpay);
                NumberToEnglish number = new NumberToEnglish();
               string currencyinwords= number.changeCurrencyToWords(Convert.ToDouble(m_Netpay));
               lblCurrencyInWords.Text = currencyinwords;
            }
            
        }

        protected void dsEmployeePeriod_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            //Select EmployeeCode to filter the grid.
            //If no employee found on go Click, New Form view pop up which allows to insert new adjustment for that employee.
            List<string> whereClauses = new List<string>();
            if (m_PeriodId == null)
            {
                e.Cancel = true;
            }
            e.WhereParameters["EmployeeId"] = m_EmployeeId;

            e.WhereParameters["SalaryPeriodId"] = m_PeriodId;
            if (e.WhereParameters["EmployeeId"] != null)
            {
                whereClauses.Add(string.Format("EmployeeId==@EmployeeId"));
            }
            whereClauses.Add(string.Format("SalaryPeriodId==@SalaryPeriodId"));
            dsEmployeePeriod.Where = string.Join("&&", whereClauses.ToArray());
        }

        protected void dsNewEmployeeForPeriod_Inserting(object sender, LinqDataSourceInsertEventArgs e)
        {
            if (e.Exception != null)
            {
                return;
            }
            EmployeePeriod ep = (EmployeePeriod)e.NewObject;
            using (PayrollDataContext db = new PayrollDataContext(dsEmployeePeriod.Database.Connection.ConnectionString))
            {
                var emp = (from employee in db.Employees
                          where employee.EmployeeId == ep.EmployeeId
                          select employee).Single();
                ep.AddDefaultAdjustments(emp);
            }
        }
        
        protected void btnEdit_PreRender(object sender, EventArgs e)
        {            
            LinkButtonEx btn = (LinkButtonEx)sender;
            if (m_sp != null)
            {
                if (!string.IsNullOrEmpty(m_sp.PaidDate.ToString()))
                {
                    btn.Action = ButtonAction.None;
                    btn.ToolTip = "This period is paid, You can not " + btn.Text;
                }
            }
        }

        /// <summary>
        /// Delete all adjustments for this period for this employee
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dsEmployeePeriod_Deleting(object sender, LinqDataSourceDeleteEventArgs e)
        {
            if (e.Exception == null)
            {
                PayrollDataContext db = (PayrollDataContext)dsEmployeePeriod.Database;

                EmployeePeriod ep = (EmployeePeriod)e.OriginalObject;

                var query = from pea in db.PeriodEmployeeAdjustments
                            where pea.EmployeePeriodId==ep.EmployeePeriodId
                            select pea;

                db.PeriodEmployeeAdjustments.DeleteAllOnSubmit(query);
            }
        }       

        protected void gvEmployeesForperiod_RowUpdated(object sender, GridViewUpdatedEventArgs e)
        {
            if (e.Exception != null)
            {
                GridViewExInsert gv = (GridViewExInsert)sender;
                EclipseLibrary.Web.JQuery.Input.ValidationSummary valSummaryEditor = (EclipseLibrary.Web.JQuery.Input.ValidationSummary)gv.NamingContainer.FindControl("valSummaryEditor");
                valSummaryEditor.ErrorMessages.Add(e.Exception.Message);
                m_updatedAmount = null;
            }

        }

        protected void dsEditPeriodEmpAdjustments_ContextCreated(object sender, LinqDataSourceStatusEventArgs e)
        {
            PayrollDataContext db = (PayrollDataContext)e.Result;
            if (db.LoadOptions == null)
            {
                // For some reason, we are getting this event twice!!
                DataLoadOptions dlo = new DataLoadOptions();
                dlo.LoadWith<EmployeePeriod>(p => p.PeriodEmployeeAdjustments);
                dlo.LoadWith<PeriodEmployeeAdjustment>(p => p.Adjustment);
                db.LoadOptions = dlo;
            }
        }

        protected void gvEditPeriodEmpAdjustments_RowDeleted(object sender, GridViewDeletedEventArgs e)
        {
            if (e.Exception == null)
            {
                gvEmployeesForperiod.DataBind();
            }

        }

        protected void gvEmployeesForperiod_SelectedIndexChanged(object sender, EventArgs e)
        {
            dsEditPeriodEmpAdjustments.WhereParameters["EmployeePeriodId"].DefaultValue = gvEmployeesForperiod.SelectedDataKey["EmployeePeriodId"].ToString();
            hdEmployeePeriodId.Value = gvEmployeesForperiod.SelectedDataKey["EmployeePeriodId"].ToString();
            int index = gvEmployeesForperiod.Columns.Cast<DataControlField>().Select((p, i) => p.AccessibleHeaderText == "Employee" ? i : -1).First(p => p >= 0);
            dlgEditor.Title = string.Format("Adjustmet for {0}", gvEmployeesForperiod.SelectedRow.Cells[index].Text);
            dlgEditor.Visible = true;
            gvEmployeesForperiod.DataBind();
        }

        protected void btnNew_Click(object sender, EventArgs e)
        {
            gvEditPeriodEmpAdjustments.InsertRowsCount = 1;
        }

        protected void gvEditPeriodEmpAdjustments_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            switch (e.Row.RowState)
            {
                case DataControlRowState.Insert:
                    TextBoxEx tbFlatAmount = (TextBoxEx)e.Row.FindControl("tbFlatAmount");
                    tbFlatAmount.ReadOnly = true;
                    break;
            }

            switch (e.Row.RowType)
            {
                case DataControlRowType.Header:
                    e.Row.Cells[0].Text = "";
                    break;
                case DataControlRowType.DataRow:
                    if (m_sp != null)
                    {
                        if (!string.IsNullOrEmpty(m_sp.PaidDate.ToString()))
                        {
                            Label lbl = new Label();
                            lbl.Text = "Edit Delete";

                            lbl.ToolTip = "This period is paid, You can not Edit or Delete";
                            e.Row.Cells[0].Controls.Clear();
                            e.Row.Cells[0].Controls.Add(lbl);
                        }
                    }
                    break;
            }
        }

    }       
}
