/*
 *  Programming: Eclipse Systems (P) Ltd., NOIDA, INDIA
 *  E-mail: support@eclsys.com
 *
 *  $Workfile:   MiscRMT.aspx.cs  $
 *  $Revision: 37213 $
 *  $Author: ssinghal $
 *  $Date: 2010-11-12 18:29:32 +0530 (Fri, 12 Nov 2010) $
 *  $Modtime:   Jul 21 2008 18:32:04  $
 *
 *  $Log:   S:/Projects/PHPA2/archives/Finance/Payroll/Reports/MiscRMT.aspx.cs-arc  $
 */
using System;
using System.Linq;
using System.Web.UI;
using System.Configuration;
using System.Web.UI.WebControls;
using Eclipse.PhpaLibrary.Database.Payroll;
using Eclipse.PhpaLibrary.Reporting;
using Eclipse.PhpaLibrary.Web;


namespace Finance.Payroll.Reports
{
    public partial class MiscRMT : PageBase
    {

        protected override void OnLoad(EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                tbFromDate.Value = DateTime.Today.AddMonths(-1).MonthStartDate().ToShortDateString();
                tbToDate.Value = DateTime.Today.AddMonths(-1).MonthEndDate().ToShortDateString();
            }
            base.OnLoad(e);
        }
        protected void dsMiscRMT_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            if (!btnSearch.IsPageValid())
            {
                e.Cancel = true;
                return;
            }
            PayrollDataContext db = (PayrollDataContext)dsMiscRMT.Database;
            var query = from pea in db.PeriodEmployeeAdjustments
                        where 
                              pea.EmployeePeriod.SalaryPeriod.PayableDate >= tbFromDate.ValueAsDate &&
                              pea.EmployeePeriod.SalaryPeriod.PayableDate <= tbToDate.ValueAsDate &&
                              pea.Adjustment.AdjustmentCategory.ReportCategories.Any(p => p.ReportCategoryId == 13)
                        group pea by new
                        {
                            Adjustment = pea.Adjustment,
                            Employee = pea.EmployeePeriod.Employee
                        } into g
                        orderby g.Key.Adjustment.AdjustmentCode, g.Key.Employee.FirstName, g.Key.Employee.LastName
                        select new
                        {
                            AdjustmentCode = g.Key.Adjustment.AdjustmentCode,
                            EmpCode = g.Key.Employee.EmployeeCode,
                            Name = g.Key.Employee.FullName,
                            Designation = g.Key.Employee.Designation,
                            FromDate = g.Min(p => p.EmployeePeriod.SalaryPeriod.SalaryPeriodStart),
                            ToDate = g.Max(p => p.EmployeePeriod.SalaryPeriod.SalaryPeriodEnd),
                            Amount = g.Sum(p => p.Amount),
                            BankId = g.Max(p => p.EmployeePeriod.Employee.Bank.BankId)
                        };
            query = query.Where(p=>p.AdjustmentCode=="FFCLUB");
            if (!string.IsNullOrEmpty(ddlBankName.Value))
            {
                query = query.Where(p => p.BankId == Convert.ToInt32(ddlBankName.Value));

            }
            e.Result = query;
            
        }
        protected void gvAdjustment_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            switch (e.Row.RowType)
            {
                case DataControlRowType.DataRow:
                    gvAdjustment.Caption = string.Format("<b>{0}</b></br><b>{2}::BHUTAN<b/><br/><b>SCHEDULES FOR RECOVERY OF OFFICERS CLUB MEMEBERSHIP</b><br/><b>FOR THE MONTH OF {1:dd MMMM yyyy}</b><br/><b>REMITTED TO:SECRETARY,OFFICERS CLUB,PHPA,{3}</b>", ConfigurationManager.AppSettings["PrintTitle"], tbFromDate.ValueAsDate, ConfigurationManager.AppSettings["Office"], ConfigurationManager.AppSettings["Office"]);
                    break;
            }
        }
        
    }
}
