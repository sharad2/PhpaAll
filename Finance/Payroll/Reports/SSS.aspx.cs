/*
 *  Programming: Eclipse Systems (P) Ltd., NOIDA, INDIA
 *  E-mail: support@eclsys.com
 *
 *  $Workfile:   SSS.aspx.cs  $
 *  $Revision: 35134 $
 *  $Author: glal $
 *  $Date: 2010-09-14 18:14:05 +0530 (Tue, 14 Sep 2010) $
 *  $Modtime:   Jul 09 2008 17:50:20  $
 *
 *  $Log:   S:/Projects/PHPA2/archives/Finance/Payroll/Reports/SSS.aspx.cs-arc  $
 * 
 *    Rev 1.2   Jul 09 2008 17:54:12   vraturi
 * PVCS Template Added.
 */
using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using Eclipse.PhpaLibrary.Database.Payroll;
using Eclipse.PhpaLibrary.Web;
using Eclipse.PhpaLibrary.Reporting;


namespace Finance.Payroll.Reports
{
    public partial class SSS : PageBase
    {
        protected override void OnLoad(EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                tbFromDate.Text =DateTime.Today.AddMonths(-1).MonthStartDate().ToShortDateString();
                tbToDate.Text = DateTime.Today.AddMonths(-1).MonthEndDate().ToShortDateString();
            }
        }
        private List<AdjustmentCategory> _category;
        private List<AdjustmentCategory> GetCategories()
        {
            if (_category == null)
            {
                PayrollDataContext db = (PayrollDataContext)this.dsReportCat.Database;
                DataLoadOptions dlo = new DataLoadOptions();
                dlo.LoadWith<ReportCategory>(rc => rc.AdjustmentCategory);
                db.LoadOptions = dlo;

                _category = (from rptCat in db.ReportCategories
                             where rptCat.ReportId == 108
                             select rptCat.AdjustmentCategory).ToList();
            }
            return _category;
        
        }
        protected void dsSSS_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            if (!btnGo.IsPageValid())
            {
                e.Cancel = true;
                return;
            }
            PayrollDataContext db = (PayrollDataContext)this.dsSSS.Database;
            var categories = GetCategories();
            List<string> strAdjCatList = new List<string>();
            foreach (var Category in categories.Where(cat => cat.IsDeduction))
            {
                strAdjCatList.Add(Category.AdjustmentCategoryCode);
            }

            List<string> periodList = new List<string>();
            var salPeriod = from sal in db.SalaryPeriods
                            where (
                            sal.PayableDate >= tbFromDate.ValueAsDate && sal.PayableDate <= tbToDate.ValueAsDate
                             )
                            select sal;

            foreach (SalaryPeriod salP in salPeriod)
            {
                periodList.Add(salP.SalaryPeriodId.ToString());
            }

            var query = from pea in db.PeriodEmployeeAdjustments
                       where periodList.Contains(pea.EmployeePeriod.SalaryPeriodId.ToString()) &&
                       strAdjCatList.Contains(pea.Adjustment.AdjustmentCategory.AdjustmentCategoryCode)
                       orderby pea.EmployeePeriod.Employee.FirstName
                       select new
                       {
                           EmployeeCode = pea.EmployeePeriod.Employee.EmployeeCode,
                           FirstName = pea.EmployeePeriod.Employee.FirstName,
                           EmployeeName = pea.EmployeePeriod.Employee.FullName,
                           Designation = pea.EmployeePeriod.Designation ?? pea.EmployeePeriod.Employee.Designation,
                           PolicyNo = pea.Comment,
                           Amount = pea.Amount ?? 0,
                           BankId = pea.EmployeePeriod.BankId ?? pea.EmployeePeriod.Employee.BankId
                       };
            if (!string.IsNullOrEmpty(ddlBankName.Value))
            {
                query = query.Where(p => p.BankId == Convert.ToInt32(ddlBankName.Value));

            }
            e.Result = query;
        }
        
        protected void gvSSS_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            switch (e.Row.RowType)
            { 
                case DataControlRowType.DataRow:
                    lblRemittedTo.Visible = true;
                    gvSSS.Caption = String.Format("<b>{0}<b><br/><b>OFFICE OF THE SENIOR FINANCE OFFICER<b><br /><B>BAJO ::BHUTAN<b><br/>",ConfigurationManager.AppSettings["PrintTitle"]);
                    gvSSS.Caption += string.Format("<b>Schedules for recovery of Life Insurance Premium/Salary Saving Scheme for {0:dd MMMM yyyy} to {1:dd MMMM yyyy}</b> <br />", tbFromDate.ValueAsDate, tbToDate.ValueAsDate);
                    gvSSS.Caption += string.Format("<b>REMITTED TO : ROYAL INSURANCE CORPORATION OF BHUTAN,BAJOTHANG</b>");
                    break;
                case DataControlRowType.EmptyDataRow:
                    lblRemittedTo.Visible = false;
                    break;
            }
        }
    }
}
