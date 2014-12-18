using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Eclipse.PhpaLibrary.Database.Payroll;
using Eclipse.PhpaLibrary.Reporting;
using Eclipse.PhpaLibrary.Web;
using EclipseLibrary.Web.UI;

namespace Finance.Payroll.Reports
{
    public partial class RecoverySchedule : PageBase
    {

        protected override void OnLoad(EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                tbFromDate.Text = DateTime.Today.AddMonths(-1).MonthStartDate().ToShortDateString();
                tbToDate.Text = DateTime.Today.AddMonths(-1).MonthEndDate().ToShortDateString();
            }

            if (!string.IsNullOrEmpty(ddlAccountName.Value))
            {
                (from DataControlField col in gvRecoveries.Columns
                 where col.AccessibleHeaderText == ddlAccountName.Value
                 select col).Single().Visible = true;
            }
            base.OnLoad(e);
        }


        protected void dsREcoverySchedule_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            if (!btnGo.IsPageValid())
            {
                e.Cancel = true;
                return;
            }

            PayrollDataContext db = (PayrollDataContext)dsREcoverySchedule.Database;
            int selectedType;
            string selectedDepartment;
            if (string.IsNullOrEmpty(ddlEmpType.Value))
            {
                selectedType = 0;
            }
            else
            {
                selectedType = int.Parse(ddlEmpType.Value);
            }
            if (string.IsNullOrEmpty(tbDepartment.Text))
            {
                selectedDepartment = string.Empty;
            }
            else
            {
                selectedDepartment = tbDepartment.Text;
            }
            var query = from pea in db.PeriodEmployeeAdjustments
                       where pea.EmployeePeriod.SalaryPeriod.PayableDate >= tbFromDate.ValueAsDate
                       && pea.EmployeePeriod.SalaryPeriod.PayableDate <= tbToDate.ValueAsDate
                        && (selectedType == 0 || pea.EmployeePeriod.Employee.EmployeeType.EmployeeTypeId == selectedType)
                       && pea.Adjustment.AdjustmentId == Convert.ToInt32(tbRecovery.Value)
                       && (selectedDepartment==string.Empty || pea.EmployeePeriod.Employee.ParentOrganization.Contains(tbDepartment.Text))
                       orderby pea.EmployeePeriod.Employee.FirstName, pea.EmployeePeriod.Employee.LastName
                       select new
                       {
                           EmpName = pea.EmployeePeriod.Employee.FullName,
                           GISAccNo = pea.EmployeePeriod.Employee.GISAccountNumber,
                           GPFAccNo = pea.EmployeePeriod.Employee.GPFAccountNo,
                           BDFCAccNo = pea.EmployeePeriod.Employee.BDFCAccountNo,
                           BLAccNo = pea.EmployeePeriod.Employee.BankLoanAccountNo,
                           Amount = string.Format("{0:N0}", pea.Amount ?? 0),
                           Department = pea.EmployeePeriod.Employee.ParentOrganization,
                           BankId = pea.EmployeePeriod.BankId ?? pea.EmployeePeriod.Employee.BankId,
                           PolicyNumber=pea.Comment
                       };
            if (!string.IsNullOrEmpty(ddlBankName.Value))
            {
                query = query.Where(p => p.BankId == Convert.ToInt32(ddlBankName.Value));
            }
            e.Result = query;
        }

        protected void gvRecoveries_DataBinding(object sender, EventArgs e)
        {
            DataControlField column = (from DataControlField col in gvRecoveries.Columns
                                      where col.AccessibleHeaderText == "Amount"
                                      select col).Single();
            column.HeaderText = tbRecovery.Text;
        }

        protected void gvRecoveries_DataBound(object sender, EventArgs e)
        {
            MultiBoundField dept = (MultiBoundField)(from DataControlField col in gvRecoveries.Columns
                                       where col.AccessibleHeaderText == "Department"
                                       select col).Single();

            if (dept.CommonCellText != null)
            {
                dept.Visible = false;
                gvRecoveries.Caption = string.Format("<b>Schedule of Recovery of {0} for the period from {1: dd MMMM yyyy} to {2:dd MMMM yyyy} for the department {3}</b>", tbRecovery.Text, tbFromDate.ValueAsDate, tbToDate.ValueAsDate, dept.CommonCellText);
            }
            else
            {
                gvRecoveries.Caption = string.Format("<b>Schedule of Recovery of {0} for the period from {1: dd MMMM yyyy} to {2:dd MMMM yyyy}</b>", tbRecovery.Text, tbFromDate.ValueAsDate, tbToDate.ValueAsDate);
            }
        }
    }
}
