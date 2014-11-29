using System;
using System.Collections.Specialized;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Eclipse.PhpaLibrary.Database.Payroll;
using Eclipse.PhpaLibrary.Reporting;
using Eclipse.PhpaLibrary.Web;
using EclipseLibrary.Web.JQuery;
using EclipseLibrary.Web.UI.Matrix;
using MatrixField = EclipseLibrary.Web.JQuery.MatrixField;

namespace Finance.Payroll.Reports
{
    public partial class Paybill1 : PageBase
    {

        protected override void OnLoad(EventArgs e)
        {
            // Explicit data binding to work around Printer Frindly crash
            ddlEmpType.DataBind();

            var mf = gvPaybill.Columns.OfType<EclipseLibrary.Web.UI.Matrix.MatrixField>().Single();
            mf.DisplayColumns.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(DisplayColumns_CollectionChanged);

            mf.DataHeaderValues.Add(new object[] { true, "X" });
            mf.DataHeaderValues.Add(new object[] { false, "X" });
            base.OnLoad(e);
        }

        void DisplayColumns_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (var dc in e.NewItems.Cast<EclipseLibrary.Web.UI.Matrix.MatrixDisplayColumn>().Where(p => p.ColumnHeaderText == "X"))
                {
                    dc.ColumnHeaderText = "Total";
                    //dc.MainSortText = "X";
                }
            }
        }

        protected void dsPaybill_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            DateTime? date = (DateTime?)tbDate.ValueAsDate;
            if (date == null)
            {
                e.Cancel = true;
                return;
            }

            this.Title += string.Format(" for {0:Y}", date);
            PayrollDataContext db = (PayrollDataContext)dsPaybill.Database;

            IQueryable<PeriodEmployeeAdjustment> listPea = db.PeriodEmployeeAdjustments
                .Where(pea => pea.EmployeePeriod.SalaryPeriod.PayableDate >= date.Value &&
                            pea.EmployeePeriod.SalaryPeriod.PayableDate <= date.Value &&
                            pea.Amount != null && pea.Amount != 0);
            if (!string.IsNullOrEmpty(ddlEmpType.Value))
            {
                listPea = listPea.Where(pea => pea.EmployeePeriod.Employee.EmployeeType.EmployeeTypeId ==
                    int.Parse(ddlEmpType.Value));
            }
             
            if (!string.IsNullOrEmpty(ddlBankName.Value))
            {
                listPea=listPea.Where(p=>p.EmployeePeriod.Employee.Bank.BankId== Convert.ToInt32(ddlBankName.Value));
             
            }
            gvPaybill.Caption += string.Format("{0} {1} Employees", rblNationality.SelectedItem.Text, ddlEmpType.SelectedItem.Text);

            if (!string.IsNullOrEmpty(rblNationality.Value))
            {
                listPea = listPea.Where(pea => pea.EmployeePeriod.Employee.IsBhutanese ==
                     bool.Parse(rblNationality.Value));
            }

            var query = from pea in listPea
                        group pea by new
                        {
                            EmployeePeriod = pea.EmployeePeriod,
                            Employee = pea.EmployeePeriod.Employee,
                            IsDeduction = pea.Adjustment.IsDeduction,
                            AdjustmentCategory = pea.Adjustment.AdjustmentCategory
                        } into g
                        orderby g.Key.Employee.FirstName, g.Key.Employee.LastName,
                               g.Key.EmployeePeriod.EmployeePeriodId,
                               g.Key.IsDeduction, g.Key.AdjustmentCategory.ShortDescription
                        select new
                        {
                            AdjustmentCategoryId = (int?)g.Key.AdjustmentCategory.AdjustmentCategoryId,
                            AdjustmentCategoryDescription = g.Key.AdjustmentCategory.ShortDescription ?? "No Category",
                            IsDeduction = g.Key.IsDeduction,
                            AmountRounded = Math.Round((decimal)(g.Sum(p => p.Amount)), MidpointRounding.AwayFromZero),
                            EmployeeId = g.Key.Employee.EmployeeId,
                            EmployeeCode = g.Key.Employee.EmployeeCode,
                            FullName = g.Key.Employee.FullName,
                            Designation = g.Key.Employee.Designation,
                            CitizenCardNo = g.Key.Employee.CitizenCardNo,
                            Grade = g.Key.Employee.Grade,
                            BasicPay = g.Key.EmployeePeriod.BasicPay,
                            EmployeePeriodId = g.Key.EmployeePeriod.EmployeePeriodId,
                            SalaryPeriodDescription = g.Key.EmployeePeriod.SalaryPeriod.Description
                        };
           
            e.Result = query;
        }
        private decimal _finalDeductions, _finalSanctions, _totalNetPay;
        protected void gvPaybill_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            EclipseLibrary.Web.UI.Matrix.MatrixRow mr;
            switch (e.Row.RowType)
            {
                case DataControlRowType.DataRow:
                    mr = e.Row.Controls.OfType<EclipseLibrary.Web.UI.Matrix.MatrixRow>().Single();
                    var deductionsTotal = mr.MatrixCells.Where(p => p.DisplayColumn.MatrixColumn.ColumnType == MatrixColumnType.CellValue && p.DataItem != null &&
                        (bool) DataBinder.Eval(p.DataItem, "IsDeduction")).Sum(p => (decimal?)DataBinder.Eval(p.DataItem, "AmountRounded"));
                    var deductionsTotalCell = mr.MatrixCells.First(p => (bool)DataBinder.Eval(p.DataItem, "IsDeduction") && DataBinder.Eval(p.DataItem, "AdjustmentCategoryDescription", "{0}") == "X");
                    deductionsTotalCell.Text = string.Format("{0:N0}", deductionsTotal);
                    _finalDeductions = _finalDeductions + Convert.ToDecimal(deductionsTotal);
                    decimal? basicPay = (decimal?)DataBinder.Eval(e.Row.DataItem, "BasicPay");
                    var earningsTotal = mr.MatrixCells.Where(p => p.DisplayColumn.MatrixColumn.ColumnType == MatrixColumnType.CellValue && p.DataItem != null &&
                        !(bool)DataBinder.Eval(p.DataItem, "IsDeduction")).Sum(p => (decimal?)DataBinder.Eval(p.DataItem, "AmountRounded"));
                         if (earningsTotal == null)
                        {
                            earningsTotal = basicPay ?? 0;
                        }
                        else
                        {
                            earningsTotal = (earningsTotal ?? 0) + (basicPay ?? 0);
                        }
                    var earningsTotalCell = mr.MatrixCells.First(p => !(bool)DataBinder.Eval(p.DataItem, "IsDeduction") && DataBinder.Eval(p.DataItem, "AdjustmentCategoryDescription", "{0}") == "X");
                    earningsTotalCell.Text = string.Format("{0:N0}", earningsTotal);
                    _finalSanctions = _finalSanctions + Convert.ToDecimal(earningsTotal);                    
                    decimal? netPay;
                    if (deductionsTotal != null)
                      {
                          netPay = earningsTotal - deductionsTotal;
                      }
                      else
                     {
                         netPay = earningsTotal;
                     }
                   int indexNetPay = gvPaybill.Columns.Cast<DataControlField>()
                   .Select((p, i) => p.AccessibleHeaderText == "netPay" ? i : -1).First(p => p >= 0);
                   e.Row.Cells[indexNetPay].Text = string.Format("{0:N0}", netPay);
                 _totalNetPay += Convert.ToDecimal(netPay);
                    break;
                case DataControlRowType.Footer:
                    int index = gvPaybill.Columns.Cast<DataControlField>()
                        .Select((p, i) => p.AccessibleHeaderText == "netPay" ? i : -1).First(p => p >= 0);
                    e.Row.Cells[index].Text = string.Format("{0:N0}", _totalNetPay);
                    mr = e.Row.Controls.OfType<EclipseLibrary.Web.UI.Matrix.MatrixRow>().Single();
                    var deductionCell = mr.MatrixCells.First(p => (bool)DataBinder.Eval(p.DataItem, "IsDeduction") && DataBinder.Eval(p.DataItem, "AdjustmentCategoryDescription", "{0}") == "X");
                    deductionCell.Text = string.Format("{0:N0}", _finalDeductions);
                    mr = e.Row.Controls.OfType<EclipseLibrary.Web.UI.Matrix.MatrixRow>().Single();
                    var earingCell = mr.MatrixCells.First(p => !(bool)DataBinder.Eval(p.DataItem, "IsDeduction") && DataBinder.Eval(p.DataItem, "AdjustmentCategoryDescription", "{0}") == "X");
                    earingCell.Text = string.Format("{0:N0}", _finalSanctions);                  
                    break;
            }
        }

        protected void dsPaybill1_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            DateTime? date = (DateTime?)tbDate.ValueAsDate;
            if (date == null)
            {
                e.Cancel = true;
                return;
            }

            this.Title += string.Format("");
            PayrollDataContext db = (PayrollDataContext)dsPaybill.Database;

            IQueryable<PeriodEmployeeAdjustment> listPea = db.PeriodEmployeeAdjustments
                .Where(pea => pea.EmployeePeriod.SalaryPeriod.PayableDate >= date.Value &&
                            pea.EmployeePeriod.SalaryPeriod.PayableDate <= date.Value &&
                            pea.Amount != null && pea.Amount != 0 && pea.Adjustment.IsDeduction==false);
            if (!string.IsNullOrEmpty(ddlEmpType.Value))
            {
                listPea = listPea.Where(pea => pea.EmployeePeriod.Employee.EmployeeType.EmployeeTypeId ==
                    int.Parse(ddlEmpType.Value));
            }

            if (!string.IsNullOrEmpty(ddlBankName.Value))
            {
                listPea = listPea.Where(p => p.EmployeePeriod.Employee.Bank.BankId == Convert.ToInt32(ddlBankName.Value));

            }
            gvPaybill.Caption += string.Format("");

            if (!string.IsNullOrEmpty(rblNationality.Value))
            {
                listPea = listPea.Where(pea => pea.EmployeePeriod.Employee.IsBhutanese ==
                     bool.Parse(rblNationality.Value));
            }      
            var query = from pea in listPea                     
                        group pea by new
                        {
                            Diviosn = pea.EmployeePeriod.Employee.Division
                           
                        } into g  
                        
                        select new
                        {
                            Amount = g.Sum(p => Math.Round(p.Amount ?? 0, MidpointRounding.AwayFromZero)) + g.Select(p => p.EmployeePeriod).Distinct().Sum(p => p.BasicPay) ?? 0,
                            //BasicSalary = Math.Round(g.Select(p=>p.EmployeePeriod).Distinct().Sum(p=>p.BasicPay) ?? 0, MidpointRounding.AwayFromZero),
                            DivisonName = g.Key.Diviosn.DivisionName,
                            
                        };

            e.Result = query;
        }


    }
}
