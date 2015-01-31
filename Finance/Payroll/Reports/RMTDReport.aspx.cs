/*
 *  Programming: Eclipse Systems (P) Ltd., NOIDA, INDIA
 *  E-mail: support@eclsys.com
 *
 *  $Workfile:   RMTDReport.aspx.cs  $
 *  $Revision: 37450 $
 *  $Author: ssingh $
 *  $Date: 2010-11-19 12:46:06 +0530 (Fri, 19 Nov 2010) $
 *  $Modtime:   Jul 29 2008 15:10:38  $
 *
 *  $Log:   S:/Projects/PHPA2/archives/Finance/Payroll/Reports/RMTDReport.aspx.cs-arc  $
 */
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Eclipse.PhpaLibrary.Database.Payroll;
using Eclipse.PhpaLibrary.Reporting;
using Eclipse.PhpaLibrary.Web;

namespace PhpaAll.Payroll.Reports
{
    /// <summary>
    /// TODO: Need to remove the second grid
    /// </summary>
    public partial class RMTDReport : PageBase
    {
       
        protected void dsDeputationist_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            if (!btnSearch.IsPageValid())
            {
                e.Cancel = true;
                return;
            }
            PayrollDataContext db = (PayrollDataContext)dsDeputationist.Database;
            //Ritesh 16-12-2011 :Report Running for Deputationist only 
            int selectedType=67; 
           
            //if (string.IsNullOrEmpty(ddlEmpType.Value))
            //{
            //    selectedType = 0;
            //}
            //else
            //{
            //    selectedType = int.Parse(ddlEmpType.Value);
            //}
            var query = from pea in db.PeriodEmployeeAdjustments
                        where  pea.EmployeePeriod.Employee.EmployeeType.EmployeeTypeId == selectedType &&
                                pea.EmployeePeriod.SalaryPeriod.PayableDate >= tbFromDate.ValueAsDate &&
                                pea.EmployeePeriod.SalaryPeriod.PayableDate <= tbToDate.ValueAsDate
                                && db.ReportCategories.Where(p => p.ReportId == 103)
                                    .Any(p => p.AdjustmentCategoryId == pea.Adjustment.AdjustmentCategoryId)
                                    && pea.Amount.Value !=0
                               
                        group pea by new
                        {
                            ParentOrg = pea.EmployeePeriod.Employee.ParentOrganization,
                            AdjCat = pea.Adjustment.AdjustmentCategory,
                            Employee=pea.EmployeePeriod.Employee.EmployeeId
                        } into grp
                        orderby grp.Key.ParentOrg
                        select new
                        {
                            ParentOrg = grp.Key.ParentOrg,
                            AdjCatDescription = grp.Key.AdjCat.ShortDescription,
                            EmployeeID=grp.Key.Employee,
                            EmployeeName=grp.Max(p => p.EmployeePeriod.Employee.FirstName + " " + p.EmployeePeriod.Employee.LastName),
                            IsDeduction = grp.Key.AdjCat.IsDeduction,
                            SalaryPeriodStart = grp.Min(p => p.EmployeePeriod.SalaryPeriod.SalaryPeriodStart),
                            SalaryPeriodEnd = grp.Max(p => p.EmployeePeriod.SalaryPeriod.SalaryPeriodEnd),
                            Amount = grp.Sum(p => p.Amount),
                            BankId = grp.Max(p => p.EmployeePeriod.Bank.BankId) != null ? grp.Max(p => p.EmployeePeriod.Bank.BankId)  : grp.Max(p => p.EmployeePeriod.Employee.Bank.BankId)
                        };
            if (!string.IsNullOrEmpty(ddlParentOffice.Value))
            {
                query = query.Where(p => p.ParentOrg == ddlParentOffice.Value.ToString());
            }
            if (!string.IsNullOrEmpty(ddlBankName.Value))
            {
                query = query.Where(p => p.BankId == Convert.ToInt32(ddlBankName.Value));

            }
            e.Result = query;
        }

        protected override void OnLoad(EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                tbFromDate.Value = DateTime.Today.AddMonths(-1).MonthStartDate().ToShortDateString();
                tbToDate.Value = DateTime.Today.AddMonths(-1).MonthEndDate().ToShortDateString();
            }
            base.OnLoad(e);
        }
        protected void dsParentOffice_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            using (PayrollDataContext db = new PayrollDataContext(ReportingUtilities.DefaultConnectString))
            {
                var query = (from emp in db.Employees
                             where emp.ParentOrganization != null && emp.ParentOrganization != string.Empty
                             orderby emp.ParentOrganization
                             select emp.ParentOrganization).Distinct().ToList();
                e.Result = query;
            }
        }
    }
}
