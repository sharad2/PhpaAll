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
                var query = from empp in db.EmployeePeriods 
                            where empp.Employee.EmployeeId == Convert.ToInt32(tbEmployee.Value) && empp.SalaryPeriod.SalaryPeriodStart >= tbDate.ValueAsDate.Value.MonthStartDate() &&
                              empp.SalaryPeriod.SalaryPeriodEnd <= tbDate.ValueAsDate.Value.MonthEndDate()
                            group new { empp } by new { empp.Employee } into g
                           
                            select new
                            {
                                EmployeeId = g.Key.Employee.EmployeeId,
                                FullName = g.Key.Employee.FullName,
                                EmployeeCode = g.Key.Employee.EmployeeCode,
                                CitizenCardNo = g.Key.Employee.CitizenCardNo,
                                Designation = g.Max(p => p.empp.Designation)?? g.Key.Employee.Designation,
                                ParentOrganization = g.Key.Employee.ParentOrganization,
                                BankAccountNo = g.Max(p => p.empp.BankAccountNo) ?? g.Key.Employee.BankAccountNo,
                                BankName = g.Max(p => p.empp.Bank.BankName) ?? g.Key.Employee.Bank.BankName,
                                BankPlace = g.Key.Employee.BankPlace,
                                GPFAccountNumber = g.Key.Employee.GPFAccountNo,
                                NPPFNumber = g.Key.Employee.NPPFPNo,
                                GISAccountNumber = g.Key.Employee.GISAccountNumber
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

                var empId = (from empp in db.EmployeePeriods
                             where empp.SalaryPeriod.SalaryPeriodStart >= tbDate.ValueAsDate.Value.MonthStartDate()
                              && empp.SalaryPeriod.SalaryPeriodEnd <= tbDate.ValueAsDate.Value.MonthEndDate()
                              && (selectEmloyee == 0 || empp.EmployeeId == selectEmloyee)
                              && (selectDiviosn == 0 || empp.Employee.DivisionId == selectDiviosn)
                              && (selectBank == 0 || ((empp.BankId ?? empp.Employee.BankId) == selectBank))
                             group new { empp } by new { empp.Employee } into g
                             select
                                g.Key.Employee.EmployeeId
                         ).ToList();
                rpt1.DataSource = empId;
                rpt1.DataBind();
            }

        }
    }


}


