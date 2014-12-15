/*
 *  Programming: Eclipse Systems (P) Ltd., NOIDA, INDIA
 *  E-mail: support@eclsys.com
 *
 *  $Workfile:   STHC.aspx.cs  $
 *  $Revision: 35133 $
 *  $Author: glal $
 *  $Date: 2010-09-14 17:52:13 +0530 (Tue, 14 Sep 2010) $
 *  $Modtime:   Jul 30 2008 16:36:38  $
 *
 *  $Log:   S:/Projects/PHPA2/archives/Finance/Payroll/Reports/STHC.aspx.cs-arc  $  
 *    Rev 1.46   Jul 09 2008 17:54:12   vraturi
 * PVCS Template Added.
 */
using System;
using System.Linq;
using System.Web.UI.WebControls;
using Eclipse.PhpaLibrary.Database.Payroll;
using Eclipse.PhpaLibrary.Reporting;
using Eclipse.PhpaLibrary.Web;
using EclipseLibrary.Web.JQuery;
using System.Web.UI;
using System.Configuration;


namespace Finance.Payroll.Reports
{
    public partial class STHC : PageBase
    {
        /// <summary>
        /// Set label text for period selected and when ont selected in the Period Selector.
        /// </summary>
        /// <param name="e"></param>

        protected override void OnLoad(EventArgs e)
        {
            if (!IsPostBack)
            {
                tbFromDate.Text = DateTime.Today.AddMonths(-1).MonthStartDate().ToShortDateString();
                tbToDate.Text = DateTime.Today.AddMonths(-1).MonthEndDate().ToShortDateString();
            }
        }
       protected void ds_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            int selectBank;
            if (!btnGo.IsPageValid())
            {
                e.Cancel = true;
                return;
            }
            if (!string.IsNullOrEmpty(ddlBankName.Value))
            {
                selectBank = Convert.ToInt32(ddlBankName.Value);
            }
            else
            {
                selectBank = 0;
            }
            PayrollDataContext db = (PayrollDataContext)ds.Database;
            var query = from pea in db.PeriodEmployeeAdjustments
                        where cblEmployeeTypes.SelectedValues.Contains(pea.EmployeePeriod.Employee.EmployeeTypeId.ToString())
                             && pea.EmployeePeriod.SalaryPeriod.PayableDate >= tbFromDate.ValueAsDate &&
                             pea.EmployeePeriod.SalaryPeriod.PayableDate <= tbToDate.ValueAsDate
                             && (pea.Amount !=0
                             || db.ReportCategories.Where(p => p.ReportId == 101)
                                     .Any(p => p.AdjustmentCategoryId == pea.Adjustment.AdjustmentCategoryId))
                             && (selectBank == 0 || pea.EmployeePeriod.BankId == selectBank)
                        group pea by new
                        {
                            CategoryId = pea.Adjustment.IsDeduction &&
                                pea.Adjustment.AdjustmentCategory.ReportCategories.Any(p => p.ReportId == 101) ?
                                    pea.Adjustment.AdjustmentCategoryId : 0,
                            Employee = pea.EmployeePeriod.Employee

                        } into g
                        orderby
                             g.Key.Employee.FirstName, g.Key.Employee.LastName, g.Key.Employee.EmployeeId, g.Key.CategoryId
                        select new
                       {
                           EmployeeId = g.Key.Employee.EmployeeId,
                           EmployeeCode = g.Key.Employee.EmployeeCode,
                           FullName = g.Key.Employee.FullName,
                           Designation = g.Key.Employee.Designation,
                           CitizenCardNo = g.Key.Employee.CitizenCardNo,
                           TpnNo = g.Key.Employee.Tpn,
                           ServiceStatus = g.Key.Employee.EmployeeType.Description,
                           BasicSalary = g.GroupBy(p => p.EmployeePeriod).Sum(p => p.Key.BasicPay),
                           TotalAllowance = g.Where(p => !p.Adjustment.IsDeduction).Sum(p => p.Amount),
                           DeductionAmount = g.Where(p => p.Adjustment.IsDeduction).Sum(p => p.Amount),
                           DeductionCategoryCode = db.AdjustmentCategories
                                .SingleOrDefault(p => p.AdjustmentCategoryId == g.Key.CategoryId).AdjustmentCategoryCode,
                           CategoryId = g.Key.CategoryId,
                           BankId = g.Key.Employee.Bank.BankId
                       };

            e.Result = query;
        }


        int _rowIndex;
        protected void lblSequence_PreRender(object sender, EventArgs e)
        {
            _rowIndex++;
            ((Label)sender).Text = _rowIndex.ToString();
        }

        protected void gv_DataBound(object sender, EventArgs e)
        {
            MatrixField mf = gv.Columns.OfType<MatrixField>().Single();
            MatrixColumn col = mf.MatrixColumns.SingleOrDefault(p => (int)p["CategoryId"] == 0);
            if (col != null)
            {
                col.Visible = false;
            }
        }

        int _index = -1;
        protected void gv_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            gv.Caption = string.Format("<b>{1}</b></br><b>SCHEDULE FOR RECOVERY OF SALARY TAX AND HEALTH CONTRIBUTION</b><br/><b> FOR THE MONTH OF {0:dd MMMM yyyy}</b>", tbFromDate.ValueAsDate, ConfigurationManager.AppSettings["PrintTitle"]);
            switch (e.Row.RowType)
            {
                case DataControlRowType.Header:
                    _index = gv.Columns.Cast<DataControlField>()
                           .Select((p, i) => p.AccessibleHeaderText == "TableAmount" ? i : -1).Single(p => p >= 0);
                    break;
                case DataControlRowType.DataRow:
                    decimal dBasicSalary = Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "BasicSalary") ?? 0);
                    decimal dTotalAllowance = Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "TotalAllowance") ?? 0);
                    decimal dTotalDeduction = Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "DeductionAmount") ?? 0);
                    string name = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "FullName") ?? 0);
                    string category = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "DeductionCategoryCode") ?? 0);
                    string categoryId = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "CategoryId") ?? 0);
                    if (cblEmployeeTypes.SelectedValue == "67")
                    {
                        if (dTotalAllowance != 0 && dTotalDeduction != 0 && category == "0")
                        {
                            e.Row.Visible = false;
                        }
                        else
                        {
                            e.Row.Cells[_index].Text = string.Format("{0:C0}", dBasicSalary + dTotalAllowance);
                        }
                    }
                    else
                    {
                        e.Row.Cells[_index].Text = string.Format("{0:C0}", dBasicSalary + dTotalAllowance);
                    }
                    break;
                default:
                    break;
            }
        }
    }
}
