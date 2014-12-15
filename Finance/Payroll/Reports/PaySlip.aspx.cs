using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Eclipse.PhpaLibrary.Database.Payroll;
using Eclipse.PhpaLibrary.Reporting;
using Eclipse.PhpaLibrary.Web;
using EclipseLibrary.Web.JQuery;
using System.Data;
using System.Configuration;
using System.Collections.Generic;

namespace Finance.Payroll.Reports
{
    public partial class PaySlip : PageBase
    {
        DateTime m_dt = new DateTime();
        GridView _gridView;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                m_dt = DateTime.Today;
                tbDate.Text = m_dt.MonthStartDate().ToShortDateString();
            }
            else
            {
                GetEmployees();
            }
        }
        protected void gv_MatrixRowDataBound(object sender, MatrixRowEventArgs e)
        {
            MatrixField mf = (MatrixField)sender;
            decimal dBasicPay = Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "BasicSalary") ?? 0);
            decimal? dTotalAllowances = e.MatrixRow.GetRowTotals(mf.MatrixColumns.Where(p => !(bool)p["IsDeduction"]))[0];
            e.MatrixRow.SetRowTotalText(0, string.Format("{0:C0}", dBasicPay + dTotalAllowances));
            decimal? dTotalDeduction = e.MatrixRow.GetRowTotals(mf.MatrixColumns.Where(p => (bool)p["IsDeduction"]))[0];
            GridView gv = new GridView();
            var index = _gridView.Columns.Cast<DataControlField>()
                .Select((p, i) => p.AccessibleHeaderText == "NetPay" ? i : -1).Single(p => p >= 0);
            e.Row.Cells[index].Text = string.Format("{0:C0}", (dBasicPay + dTotalAllowances) - dTotalDeduction);
        }
        protected void btnShowPaySlip_Click(object sender, EventArgs e)
        {

        }
        /// <summary>
        /// <remarks>
        /// Ritesh 20 Dec 2011
        /// Calling ItemDataBound event of Repeater to fill Grid View and Form View with Employee Detail and Salary detail for an employee id 
        /// </remarks>
        /// <summary>
        protected void rpt1_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            _gridView = new GridView();
            _gridView = ((GridView)(e.Item.FindControl("gv")));
            FormView formView2 = new FormView();
            formView2 = ((FormView)(e.Item.FindControl("frmView")));
            string empId = e.Item.DataItem.ToString();
            // = = = = = = = 
            if (!(formView2 == null))
            {
                //Calling method to fill FormView
                GetEmlpoyeeDetails(empId, formView2);

            }
            if (!(_gridView == null))
            {
                //Calling method to fill GriDview
                GetSalaryDetails(empId, _gridView);

            }


        }
        protected void GetEmlpoyeeDetails(string empId, FormView frmView)
        {
            DataSet ds = new DataSet();
            using (PayrollDataContext db = new PayrollDataContext(ReportingUtilities.DefaultConnectString))
            {
                var query = from emp in db.Employees
                            join empp in db.EmployeePeriods on emp.EmployeeId equals empp.EmployeeId
                            join empsal in db.SalaryPeriods on empp.SalaryPeriodId equals empsal.SalaryPeriodId
                            where emp.EmployeeId == Convert.ToInt32(empId) && empsal.SalaryPeriodStart >= tbDate.ValueAsDate.Value.MonthStartDate() &&
                                empsal.SalaryPeriodStart <= tbDate.ValueAsDate.Value.MonthEndDate()
                            select new
                            {
                                EmployeeId = emp.EmployeeId,
                                FullName = emp.FullName,
                                EmployeeCode = emp.EmployeeCode,
                                CitizenCardNo = emp.CitizenCardNo,
                                Designation = empp.Designation,
                                ParentOrganization = emp.ParentOrganization,
                                BankAccountNo = empp.BankAccountNo,
                                BankName = empp.Bank.BankName,
                                BankPlace = emp.BankPlace,
                                GPFAccountNumber = emp.GPFAccountNo,
                                NPPFNumber = emp.NPPFPNo
                            };
                frmView.DataSource = query;
                frmView.DataBind();
                if (rpt1.Items.Count >= 0)
                {
                    frmView.Caption = string.Format("<b>{1}<br/> Pay Slip for the month of {0: MMMM, yyyy} </b>", tbDate.ValueAsDate, ConfigurationManager.AppSettings["PrintTitle"]);
                }
            }

        }
        protected void GetSalaryDetails(string empId, GridView gv)
        {
            using (PayrollDataContext db = new PayrollDataContext(ReportingUtilities.DefaultConnectString))
            {
                var query = (from pea in db.PeriodEmployeeAdjustments
                             where (pea.EmployeePeriod.Employee.EmployeeId) == Convert.ToInt32(empId) &&
                                pea.EmployeePeriod.SalaryPeriod.SalaryPeriodStart >= tbDate.ValueAsDate.Value.MonthStartDate() &&
                                pea.EmployeePeriod.SalaryPeriod.SalaryPeriodEnd <= tbDate.ValueAsDate.Value.MonthEndDate()
                             group pea by new
                             {
                                 Adjustment = pea.Adjustment,

                             } into g
                             orderby g.Key.Adjustment.IsDeduction
                             select new
                             {
                                 EmployeeId = g.Max(p => p.EmployeePeriod.EmployeeId),
                                 BasicSalary = g.GroupBy(p => p.EmployeePeriod).Sum(p => p.Key.BasicPay),
                                 AdjustmentCode = g.Key.Adjustment.ShortDescription,
                                 IsDeduction = g.Key.Adjustment.IsDeduction,
                                 Amount = g.Where(p => p.Amount.HasValue)
                                      .Sum(p => Math.Round(p.Amount.Value, MidpointRounding.AwayFromZero))
                             }).Where(p => p.Amount > 0 && p.Amount != null);
                gv.DataSource = query;
                gv.DataBind();
            }
        }
        /// <summary>
        /// <remarks>
        /// Ritesh 20 Dec 2011
        /// This Method is used to fill repeater control with all Employee Id matching the parameters applied
        /// in query.
        /// </remarks>
        /// </summary>
        protected void GetEmployees()
        {


            int selectEmloyee, selectDiviosn;
            int selectBank;
            if (!string.IsNullOrEmpty(tbEmployee.Value))
            {
                selectEmloyee = Convert.ToInt32(tbEmployee.Value);
            }
            else
            {
                selectEmloyee = 0;
            }
            if (!string.IsNullOrEmpty(ddlDivision.Value))
            {
                selectDiviosn = Convert.ToInt32(ddlDivision.Value);
            }
            else
            {
                selectDiviosn = 0;
            }
            if (!string.IsNullOrEmpty(ddlBankName.Value))
            {
                selectBank = Convert.ToInt32(ddlBankName.Value);
            }
            else
            {
                selectBank = 0;
            }
            using (PayrollDataContext db = new PayrollDataContext(ReportingUtilities.DefaultConnectString))
            {

                var empId = (from emp in db.Employees
                             join empp in db.EmployeePeriods on emp.EmployeeId equals empp.EmployeeId
                             join empsal in db.SalaryPeriods on empp.SalaryPeriodId equals empsal.SalaryPeriodId
                             where empsal.SalaryPeriodStart >= tbDate.ValueAsDate.Value.MonthStartDate()
                              && empsal.SalaryPeriodEnd <= tbDate.ValueAsDate.Value.MonthEndDate()
                              && (selectEmloyee == 0 || empp.EmployeeId == selectEmloyee)
                              && (selectDiviosn == 0 || emp.DivisionId == selectDiviosn)
                              && (selectBank == 0 || empp.BankId == selectBank)
                             select
                                emp.EmployeeId
                         ).ToList();
                rpt1.DataSource = empId;
                rpt1.DataBind();
            }

        }
    }


}


