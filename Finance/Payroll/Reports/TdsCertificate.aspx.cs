/*
 *  Programming: Eclipse Systems (P) Ltd., NOIDA, INDIA
 *  E-mail: support@eclsys.com
 *
 *  $Workfile:   TdsCertificate.aspx.cs  $
 *  $Revision: 38430 $
 *  $Author: ssingh $
 *  $Date: 2010-12-03 10:40:31 +0530 (Fri, 03 Dec 2010) $
 *  $Modtime:   Jul 30 2008 20:12:52  $*
 *  $Log:   S:/Projects/PHPA2/archives/Finance/Payroll/Reports/TdsCertificate.aspx.cs-arc  $
 * PVCS Template Added.
 */

using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Eclipse.PhpaLibrary.Database.Payroll;
using Eclipse.PhpaLibrary.Reporting;
using Eclipse.PhpaLibrary.Web;
using EclipseLibrary.Web.JQuery;
using MatrixField = EclipseLibrary.Web.JQuery.MatrixField;
using System.Collections.Specialized;
using EclipseLibrary.Web.UI.Matrix;

namespace Finance.Payroll.Reports
{
    public partial class TdsCertificate : PageBase
    {
        protected void ds_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            if (!btnGo.IsPageValid())
            {
                e.Cancel = true;
                return;
            }
            PayrollDataContext db = (PayrollDataContext)ds.Database;
            e.Result = from pea in db.PeriodEmployeeAdjustments
                       where pea.EmployeePeriod.Employee.EmployeeId == Convert.ToInt32(tbEmployee.Value) &&
                            pea.EmployeePeriod.SalaryPeriod.SalaryPeriodStart >= tbFromDate.ValueAsDate &&
                            pea.EmployeePeriod.SalaryPeriod.SalaryPeriodEnd <= tbToDate.ValueAsDate
                       group pea by new
                       {
                           EmployeePeriod = pea.EmployeePeriod,
                           CategoryId = pea.Adjustment.AdjustmentCategory.ReportCategories.Any(p => p.ReportId == 100) ?
                                pea.Adjustment.AdjustmentCategoryId : 0,
                           IsDeduction = pea.Adjustment.IsDeduction
                       } into g
                       orderby g.Key.EmployeePeriod.SalaryPeriod.SalaryPeriodStart, g.Key.IsDeduction, g.Key.CategoryId
                       let catdesc = g.Where(p => p.Adjustment.AdjustmentCategoryId == g.Key.CategoryId)
                            .Select(p => p.Adjustment.AdjustmentCategory.ShortDescription).First()
                       select new
                       {
                           EmployeePeriodId = g.Key.EmployeePeriod.EmployeePeriodId,
                           Month = g.Key.EmployeePeriod.SalaryPeriod.SalaryPeriodCode,
                           CategoryId = g.Key.CategoryId,
                           CategoryShortDescription = catdesc,
                           BasicPay = g.Key.EmployeePeriod.BasicPay,
                           IsDeduction = g.Key.IsDeduction,
                           Amount = g.Sum(p => Math.Round(p.Amount ?? 0, MidpointRounding.AwayFromZero)),
                           Date = g.Key.EmployeePeriod.Created,
                           HeaderSortExpression = g.Key.CategoryId == 0 ? "Z" : catdesc,
                           MRNumber = g.Key.EmployeePeriod.SalaryPeriod.MRNumber
                       };
            this.Title += string.Format(" From {0:d MMMM yyyy} To {1:d MMMM yyyy}",tbFromDate.ValueAsDate,tbToDate.ValueAsDate);
        }
        protected void dsEmployee_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            if (!btnGo.IsPageValid())
            {
                e.Cancel = true;
                return;
            }

            PayrollDataContext db = (PayrollDataContext)ds.Database;
            e.Result = from emp in db.Employees
                       where emp.EmployeeId == Convert.ToInt32(tbEmployee.Value)
                       select new
                       {
                           EmployeeId = emp.EmployeeId,
                           FullName = emp.FullName,
                           EmployeeCode = emp.EmployeeCode,
                           CitizenCardNo = emp.CitizenCardNo,
                           Designation = emp.Designation,
                           Tpn = emp.Tpn
                       };
        }

        //decimal _dTotalNetPay = 0;
        //protected void gv_MatrixRowDataBound(object sender, MatrixRowEventArgs e)
        //{
        //    MatrixField mf = (MatrixField)sender;
        //    decimal dBasicPay = Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "BasicPay") ?? 0);
        //    decimal?[] rowTotals = e.MatrixRow.GetRowTotals(mf.MatrixColumns.Where(p => !(bool)p["IsDeduction"]));
        //    decimal? dTotalAllowances = rowTotals == null ? null : rowTotals[0];
        //    e.MatrixRow.SetRowTotalText(0, string.Format("{0:C}", dBasicPay + dTotalAllowances));

        //    rowTotals = e.MatrixRow.GetRowTotals(mf.MatrixColumns.Where(p => (bool)p["IsDeduction"]));
        //    decimal? dTotalDeduction = rowTotals == null ? null : rowTotals[0];

        //    var index = gv.Columns.Cast<DataControlField>()
        //        .Select((p, i) => p.AccessibleHeaderText == "NetPay" ? i : -1).Single(p => p >= 0);
        //    e.Row.Cells[index].Text = string.Format("{0:C2}", (dBasicPay + dTotalAllowances) - dTotalDeduction);
        //    _dTotalNetPay += ((dBasicPay + dTotalAllowances) - dTotalDeduction) ?? 0;

        //}
        private decimal _finalDeductions, _finalSanctions;
        decimal?_totalNetPay= 0.0M;
        protected void gv_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            EclipseLibrary.Web.UI.Matrix.MatrixRow mr;
            switch (e.Row.RowType)
            {
                case DataControlRowType.DataRow:
                    mr = e.Row.Controls.OfType<EclipseLibrary.Web.UI.Matrix.MatrixRow>().Single();
                    var deductionsTotal = mr.MatrixCells.Where(p => p.DisplayColumn.MatrixColumn.ColumnType == MatrixColumnType.CellValue && p.DataItem != null &&
                        (bool) DataBinder.Eval(p.DataItem, "IsDeduction")).Sum(p => (decimal?)DataBinder.Eval(p.DataItem, "Amount"));
                    var deductionsTotalCell = mr.MatrixCells.First(p => (bool)DataBinder.Eval(p.DataItem, "IsDeduction") && DataBinder.Eval(p.DataItem, "CategoryShortDescription", "{0}") == "X");
                    deductionsTotalCell.Text = string.Format("{0:N0}", deductionsTotal);
                    _finalDeductions = _finalDeductions + Convert.ToDecimal(deductionsTotal);
                    decimal? basicPay = (decimal?)DataBinder.Eval(e.Row.DataItem, "BasicPay");
                    var earningsTotal = mr.MatrixCells.Where(p => p.DisplayColumn.MatrixColumn.ColumnType == MatrixColumnType.CellValue && p.DataItem != null &&
                        !(bool)DataBinder.Eval(p.DataItem, "IsDeduction")).Sum(p => (decimal?)DataBinder.Eval(p.DataItem, "Amount"));
                         if (earningsTotal == null)
                        {
                            earningsTotal = basicPay ?? 0;
                        }
                        else
                        {
                            earningsTotal = (earningsTotal ?? 0) + (basicPay ?? 0);
                        }
                         var earningsTotalCell = mr.MatrixCells.First(p => !(bool)DataBinder.Eval(p.DataItem, "IsDeduction") && DataBinder.Eval(p.DataItem, "CategoryShortDescription", "{0}") == "Y");
                         earningsTotalCell.Text = string.Format("{0:N0}", earningsTotal);
                    _finalSanctions = _finalSanctions + Convert.ToDecimal(earningsTotal);                    
                    decimal? netPay;
                    if (deductionsTotal != null)
                      {
                          netPay = ReportingUtilities.SubtractNullable(earningsTotal, deductionsTotal);
                      }
                      else
                     {
                         netPay = earningsTotal;
                     }
                   int indexNetPay = gv.Columns.Cast<DataControlField>()
                   .Select((p, i) => p.AccessibleHeaderText == "NetPay" ? i : -1).First(p => p >= 0);
                   e.Row.Cells[indexNetPay].Text = string.Format("{0:N0}", netPay);
                 _totalNetPay += netPay ?? 0;
                 break;

                case DataControlRowType.Footer:
                    var index = gv.Columns.Cast<DataControlField>()
                        .Select((p, i) => p.AccessibleHeaderText == "NetPay" ? i : -1).Single(p => p >= 0);
                    e.Row.Cells[index].Text = string.Format("{0:N0}", _totalNetPay);
                      mr = e.Row.Controls.OfType<EclipseLibrary.Web.UI.Matrix.MatrixRow>().Single();
                      var deductionCell = mr.MatrixCells.First(p => (bool)DataBinder.Eval(p.DataItem, "IsDeduction") && DataBinder.Eval(p.DataItem, "CategoryShortDescription", "{0}") == "X");
                      deductionCell.Text = string.Format("{0:N0}", _finalDeductions);
                    mr = e.Row.Controls.OfType<EclipseLibrary.Web.UI.Matrix.MatrixRow>().Single();
                    var earingCell = mr.MatrixCells.First(p => !(bool)DataBinder.Eval(p.DataItem, "IsDeduction") && DataBinder.Eval(p.DataItem, "CategoryShortDescription", "{0}") == "Y");
                    earingCell.Text = string.Format("{0:N0}", _finalSanctions);    
                    break;

                default:
                    break;
            }
        }
        /// <summary>
        /// Setting default dates
        /// </summary>
        /// <param name="e"></param>
        protected override void OnLoad(EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                tbFromDate.Text = DateTime.Today.AddMonths(-1).MonthStartDate().ToShortDateString();
                tbToDate.Text = DateTime.Today.AddMonths(-1).MonthEndDate().ToShortDateString();
            }
            
            var mf = gv.Columns.OfType<EclipseLibrary.Web.UI.Matrix.MatrixField>().Single();
            mf.DisplayColumns.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(DisplayColumns_CollectionChanged);

            mf.DataHeaderValues.Add(new object[] { true, "X" });
            mf.DataHeaderValues.Add(new object[] { false, "Y" });
            base.OnLoad(e);
        }
        void DisplayColumns_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (var dc in e.NewItems.Cast<EclipseLibrary.Web.UI.Matrix.MatrixDisplayColumn>())
                {
                    if (dc.ColumnHeaderText == "X")
                    {
                        dc.ColumnHeaderText = "Total Deductions";
                    }
                    if(dc.ColumnHeaderText=="Y")
                    {
                        dc.ColumnHeaderText = "Total Salary";

                    }
                    if (dc.ColumnHeaderText == "")
                    {
                        dc.ColumnHeaderText = "Others";

                    }
                    //dc.MainSortText = "X";
                }
            }
        }

    }
}
