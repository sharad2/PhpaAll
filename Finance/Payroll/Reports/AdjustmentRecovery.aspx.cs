/*
 *  Programming: Eclipse Systems (P) Ltd., NOIDA, INDIA
 *  E-mail: support@eclsys.com
 *
 *  $Workfile:   AdjustmentRecovery.aspx.cs  $
 *  $Revision: 35418 $
 *  $Author: ssinghal $
 *  $Date: 2010-09-22 09:48:39 +0530 (Wed, 22 Sep 2010) $
 *  $Modtime:   Jul 25 2008 16:55:24  $
 *
 *  $Log:   S:/Projects/PHPA2/archives/Finance/Payroll/Reports/AdjustmentRecovery.aspx.cs-arc  $
 */
using System;
using System.Linq;
using System.Configuration;
using System.Web.UI.WebControls;
using Eclipse.PhpaLibrary.Database.Payroll;
using Eclipse.PhpaLibrary.Reporting;
using Eclipse.PhpaLibrary.Web;

namespace PhpaAll.Payroll.Reports
{
    public partial class AdjustmentRecovery : PageBase
    {
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (!IsPostBack)
            {
                tbFromDate.Text = DateTime.Today.AddMonths(-1).MonthStartDate().ToShortDateString();
                tbToDate.Text = DateTime.Today.AddMonths(-1).MonthEndDate().ToShortDateString();
            }
        }
        protected void dsBLRecovery_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            if (!btnGo.IsPageValid())
            {
                e.Cancel = true;
                return;
            }
            PayrollDataContext db = (PayrollDataContext)dsBLRecovery.Database;
            var query = (from pea in db.PeriodEmployeeAdjustments
                         //let accountNumber = pea.EmployeePeriod.Employee.BankLoanAccountNo
                         where
                               pea.EmployeePeriod.SalaryPeriod.SalaryPeriodStart >= tbFromDate.ValueAsDate.Value &&
                               pea.EmployeePeriod.SalaryPeriod.SalaryPeriodEnd <= tbToDate.ValueAsDate.Value
                               && pea.Amount != 0 
                               && pea.Adjustment.AdjustmentCategory.ReportCategories.Any(p => p.ReportId == 104)
                              // && (string.IsNullOrEmpty(strAccountNo) || pea.EmployeePeriod.Employee.BankLoanAccountNo.StartsWith(strAccountNo))
                         orderby pea.EmployeePeriod.Employee.FirstName, pea.EmployeePeriod.Employee.LastName, pea.EmployeePeriod.SalaryPeriod.SalaryPeriodStart
                         select new
                         {
                             EmployeeCode = pea.EmployeePeriod.Employee.EmployeeCode,
                             EmployeeName = pea.EmployeePeriod.Employee.FullName,
                             Designation = pea.EmployeePeriod.Designation ?? pea.EmployeePeriod.Employee.Designation,
                             DivisionName = pea.EmployeePeriod.Employee.Division.DivisionName,
                             Amount = pea.Amount ?? 0,
                             AccountNumber = pea.EmployeePeriod.BankAccountNo ?? pea.EmployeePeriod.Employee.BankAccountNo,
                             HeadOfAccountId = pea.Adjustment.HeadOfAccountId,
                             BankId= pea.EmployeePeriod.BankId ?? pea.EmployeePeriod.Employee.BankId,
                             SalaryPeriodStartDate = pea.EmployeePeriod.SalaryPeriod.SalaryPeriodStart,
                             // Sharad 8 Jun 2015
                             // Loan account number is in the Comment field of EmployeeAdjustments
                             // We show the employee adjustment corresponding to this adjustment
                             Remarks = pea.Adjustment.EmployeeAdjustments.Where(p => p.AdjustmentId == pea.AdjustmentId).Max(p => p.Comment) ?? "Sharad"
                         });
            if (!string.IsNullOrEmpty(tbAccountNo.Text))
            {
                query = query.Where(p => p.AccountNumber.StartsWith(tbAccountNo.Text));
            }
            if (!string.IsNullOrEmpty(tbHeadOfAccount.Value))
            {
                query = query.Where(p => p.HeadOfAccountId == Convert.ToInt32(tbHeadOfAccount.Value));
            }
            if (!string.IsNullOrEmpty(ddlBankName.Value))
            {
                query = query.Where(p => p.BankId == Convert.ToInt32(ddlBankName.Value));

            }
            e.Result = query;
            this.Title = string.Format("Loan Recovery for {0: d MMMMM yyyy} to {1:d MMMMM yyyy}",
                tbFromDate.ValueAsDate, tbToDate.ValueAsDate);
        }
        protected void gvBLRecovery_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            char ch = ':';
            string[] bankLoanname = new string[2];
            if (!string.IsNullOrEmpty(tbHeadOfAccount.Text))
            {
                bankLoanname = tbHeadOfAccount.Text.Split(ch);
            }
            switch (e.Row.RowType)
            {
                case DataControlRowType.DataRow:
                    gvBLRecovery.Caption = string.Format("<b>{0}</b><br/><b>OFFICE OF THE SENIOR FINANCE OFFICER<br/><b>{3}::BHUTAN</b><br/>SCHEDULE OF RECOVERY OF LOAN {4}<br/>FOR THE PERIOD {1:dd MMMM yyyy} to {2:dd MMMM yyyy}", ConfigurationManager.AppSettings["PrintTitle"], tbFromDate.ValueAsDate, tbToDate.ValueAsDate, ConfigurationManager.AppSettings["Office"], (!string.IsNullOrEmpty(tbHeadOfAccount.Text)) ? bankLoanname.GetValue(1) : "");
                    break;
            }
            if (Convert.ToDateTime(tbFromDate.Value).ToString("yyyyMM") != Convert.ToDateTime(tbToDate.Value).ToString("yyyyMM"))
            {
                DataControlField column = (from DataControlField col in gvBLRecovery.Columns
                                           where col.AccessibleHeaderText == "SalaryPeriodDate"
                                           select col).Single();
                column.Visible = true;
            }
        }
     
    }
}
