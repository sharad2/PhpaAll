using System;
using System.Linq;
using System.Web.UI.WebControls;
using System.Configuration;
using Eclipse.PhpaLibrary.Database.Payroll;
using Eclipse.PhpaLibrary.Reporting;
using Eclipse.PhpaLibrary.Web;
using EclipseLibrary.Web.JQuery.Input;
using System.Collections.Generic;

namespace Finance.Payroll.Reports
{
    public partial class GIS : PageBase
    {
        protected DateTime m_dt = new DateTime();
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (!Page.IsPostBack)
            {
                tbFromDate.Value = DateTime.Today.AddMonths(-1).MonthStartDate().ToShortDateString();
                tbToDate.Value = DateTime.Today.AddMonths(-1).MonthEndDate().ToShortDateString();
            }
        }
        protected void dsGIS_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            PayrollDataContext db = (PayrollDataContext)dsGIS.Database;
            int[] selectedTypes =cblEmployeeTypes.Value.Split(',')
                .Where(p => !string.IsNullOrEmpty(p))
                .Select(p => int.Parse(p)).ToArray();
            var query = from pea in db.PeriodEmployeeAdjustments
                       where pea.EmployeePeriod.SalaryPeriod.PayableDate >= tbFromDate.ValueAsDate &&
                             pea.EmployeePeriod.SalaryPeriod.PayableDate <= tbToDate.ValueAsDate &&
                             pea.EmployeePeriod.Employee.EmployeeTypeId.HasValue &&
                             selectedTypes.Contains(pea.EmployeePeriod.Employee.EmployeeTypeId.Value)
                             && pea.Adjustment.AdjustmentCategory.ReportCategories.Any(p => p.ReportId == 109)
                        orderby pea.EmployeePeriod.Employee.GISAccountNumber
                       select new
                       {
                           GISNo = pea.EmployeePeriod.Employee.GISAccountNumber,
                           Name = pea.EmployeePeriod.Employee.FullName,
                           FirstName = pea.EmployeePeriod.Employee.FirstName,
                           Grade = pea.EmployeePeriod.Employee.ServicePeriods.OrderByDescending(p => p.PeriodStartDate).Take(1).Max(p => p.Grade),
                           Designation = pea.EmployeePeriod.Designation ?? pea.EmployeePeriod.Employee.Designation,                           
                           CitizenId = pea.EmployeePeriod.Employee.CitizenCardNo,
                           Amount = string.Format("{0:N0}", pea.Amount),
                           GISGroup = pea.EmployeePeriod.Employee.GISGroup,
                           DateOfBirth = pea.EmployeePeriod.Employee.DateOfBirth,
                           BankId = pea.EmployeePeriod.BankId ?? pea.EmployeePeriod.Employee.BankId
                       };
            if (!string.IsNullOrEmpty(ddlBankName.Value))
            {
                query = query.Where(p => p.BankId == Convert.ToInt32(ddlBankName.Value));

            }
            e.Result = query;
        }

        protected void gvGIS_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            switch (e.Row.RowType)
            {
                case DataControlRowType.DataRow:
                    lblRecovery.Visible = true;
                    gvGIS.Caption = string.Format("<b>{0}</b><br/><b>{1}::BHUTAN</b><br/>", ConfigurationManager.AppSettings["PrintTitle"], ConfigurationManager.AppSettings["Office"]);
                    gvGIS.Caption += string.Format("<b>SCHEDULES OF RECOVERY OF GROUP INSURANCE SCHEME</b><br/><b>FOR THE MONTH OF {0:MMMM, yyyy}</b><br/><b>REMITTED TO:-ROYAL INSURANCE CORPORATION OF BHUTAN,{1}</b>", tbFromDate.Value, ConfigurationManager.AppSettings["Office"]);
                    break;

                case DataControlRowType.EmptyDataRow:
                    lblRecovery.Visible = false;
                    break;
            }
        }

        protected void btnShowReport_Click(object sender, EventArgs e)
        {
            ButtonEx btn = (ButtonEx)sender;
            if (!btn.IsPageValid())
            {
                gvGIS.DataBind();
            }
        }
      
    }
}
