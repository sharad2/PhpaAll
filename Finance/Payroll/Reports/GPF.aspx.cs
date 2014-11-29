using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Eclipse.PhpaLibrary.Database.Payroll;
using Eclipse.PhpaLibrary.Reporting;
using Eclipse.PhpaLibrary.Web;
using EclipseLibrary.Web.JQuery.Input;

namespace Finance.Payroll.Reports
{
    public partial class GPF : PageBase
    {
        protected override void OnLoad(EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                tbFromDate.Value = DateTime.Today.AddMonths(-1).MonthStartDate().ToShortDateString();
                tbToDate.Value = DateTime.Today.AddMonths(-1).MonthEndDate().ToShortDateString();
            }
        }

        protected void dsGPF_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            PayrollDataContext db = (PayrollDataContext)dsGPF.Database;
            var query = from pea in db.PeriodEmployeeAdjustments
                       where pea.Adjustment.AdjustmentCategory.ReportCategories.Any(p => p.ReportId == 106)
                       && pea.EmployeePeriod.SalaryPeriod.PayableDate >= tbFromDate.ValueAsDate
                       && pea.EmployeePeriod.SalaryPeriod.PayableDate <= tbToDate.ValueAsDate
                       && pea.EmployeePeriod.Employee.NPPFType == Convert.ToString(rblNPPFType.Value)
                       orderby pea.EmployeePeriod.Employee.NPPFPNo
                       select new
                       {                           
                           NPPFPNo = pea.EmployeePeriod.Employee.NPPFPNo,
                           MembersName = pea.EmployeePeriod.Employee.FullName,
                           Designation = pea.EmployeePeriod.Employee.Designation,
                           CitizenId = pea.EmployeePeriod.Employee.CitizenCardNo,
                           EmployeeIDNo=pea.EmployeePeriod.Employee.EmployeeId,
                           BasicPay = pea.EmployeePeriod.Employee.BasicSalary,
                           EmployeesContb = Math.Round(pea.Amount ?? 0, MidpointRounding.AwayFromZero),
                           EmployersContb = Math.Round(pea.Amount ?? 0, MidpointRounding.AwayFromZero),
                           TotalNu = Math.Round(pea.Amount ?? 0, MidpointRounding.AwayFromZero) * 2,
                           GPFAccountNo = pea.EmployeePeriod.Employee.GPFAccountNo,
                           Date = pea.EmployeePeriod.SalaryPeriod.SalaryPeriodStart,
                           BankAccountNo = pea.EmployeePeriod.Employee.BankAccountNo,
                           BankId = pea.EmployeePeriod.Employee.Bank.BankId
                       };
            if (!string.IsNullOrEmpty(ddlBankName.Value))
            {
                query = query.Where(p => p.BankId == Convert.ToInt32(ddlBankName.Value));

            }
            e.Result = query;
        }
        
        
        protected void gvGPF_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            switch (e.Row.RowType)
            {
                case DataControlRowType.DataRow:
                    //Boolean val=Convert.ToBoolean(rblNationality.Value);
                    gvGPF.Caption = string.Format("<b>NATIONAL PENSION & PROVIDENT FUND</b><br/><b>THIMPU:BHUTAN<br/>Category Name : Government Institution<br/>Organization Code: GI 0011</b></br> ");
                    if (rblNPPFType.Value == "Tier-2")
                    {
                        gvGPF.Caption += string.Format("<b>Monthly Recovery Schedule of NPPF for Tier-2 Members for the period from {0:dd MMMM yyyy} to {1:dd MMMM yyyy}</b>",
                            tbFromDate.ValueAsDate, tbToDate.ValueAsDate);
                    }
                    else
                    {
                        gvGPF.Caption += string.Format("<b>Monthly Recovery Schedule of NPPF for Non Bhutanese Members for the period from {0:dd MMMM yyyy} to {1:dd MMMM yyyy}</b>",
                            tbFromDate.ValueAsDate, tbToDate.ValueAsDate);
                    }
                    break;
            }
        }

        protected void btnShowReport_Click(object sender, EventArgs e)
        {
            ButtonEx btn = (ButtonEx)sender;
            if (!btn.IsPageValid())
            {
                gvGPF.DataBind();
            }
        }
    }
}