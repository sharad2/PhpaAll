/*
 *  Programming: Eclipse Systems (P) Ltd., NOIDA, INDIA
 *  E-mail: support@eclsys.com
 *
 *  $Workfile:   PaybillRegister.aspx.cs  $
 *  $Revision: 19618 $
 *  $Author: pshishodia $
 *  $Date: 2009-06-09 14:36:21 +0530 (Tue, 09 Jun 2009) $
 *  $Modtime:   Jul 30 2008 17:30:40  $
 *
 *  $Log:   S:/Projects/PHPA2/archives/Finance/Payroll/Reports/PaybillRegister.aspx.cs-arc  $
 */
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Web.UI.WebControls;
using Eclipse.PhpaLibrary.Database.Payroll;
using Eclipse.PhpaLibrary.Reporting;
using Eclipse.PhpaLibrary.Web;
using EclipseLibrary.Web.UI;

namespace Finance.Payroll.Reports
{
    public partial class PaybillRegister : PageBase
    {
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (!IsPostBack)
            {
                tbFromDate.Text = DateTime.Today.AddMonths(-1).MonthStartDate().ToShortDateString();
                tbToDate.Text = DateTime.Today.AddMonths(-1).MonthEndDate().ToShortDateString();
            }
            else
            {
                if (btnGo.IsPageValid())
                {
                    EmployeePaybillGrid();
                }
            }
        }

        class EmpPayDtl
        {
            public EmployeePeriod EmployeePeriod { get; set; }
            public AdjustmentCategory AdjustmentCategory { get; set; }
            public Adjustment Adjustment { get; set; }
            public decimal? Amount { get; set; }
        }
        List<EmpPayDtl> m_query;

        private void EmployeePaybillGrid()
        {
            PayrollDataContext db = (PayrollDataContext)dsPayBillDtl.Database;
            DataLoadOptions dlo = new DataLoadOptions();
            dlo.LoadWith<PeriodEmployeeAdjustment>(pea => pea.Adjustment);
            dlo.LoadWith<PeriodEmployeeAdjustment>(pea => pea.EmployeePeriod);
            dlo.LoadWith<EmployeePeriod>(ep => ep.SalaryPeriod);

            m_query = (from pea in db.PeriodEmployeeAdjustments
                       where pea.EmployeePeriod.EmployeeId == Convert.ToInt32(tbEmployee.Value) &&

                              pea.EmployeePeriod.SalaryPeriod.PayableDate >= tbFromDate.ValueAsDate &&
                              pea.EmployeePeriod.SalaryPeriod.PayableDate <= tbToDate.ValueAsDate
                       group pea by new
                       {
                           EmployeePeriod = pea.EmployeePeriod,
                           Adjustment = pea.Adjustment,
                           AdjustmentCategory = pea.Adjustment.AdjustmentCategory
                       } into grp
                       select new EmpPayDtl()
                       {
                           EmployeePeriod = grp.Key.EmployeePeriod,
                           Adjustment = grp.Key.Adjustment,
                           AdjustmentCategory = grp.Key.AdjustmentCategory,
                           Amount = grp.Sum(p => p.Amount ?? 0)
                       }).Distinct().OrderBy(e => e.EmployeePeriod.EmployeePeriodId).ToList();

            //OrderBy(e => e.EmployeePeriod.EmployeePeriodId) added to solve problem reported as row was getting displayed repeatedly 


            MultiBoundField mf;
            var distinctAdjustments = (from pea in m_query
                                       select pea.Adjustment).Distinct();

            foreach (Adjustment adj in distinctAdjustments.Where(p => !p.IsDeduction))
            {
                mf = new MultiBoundField();
                mf.HeaderText = "Earnings|" + adj.ShortDescription;
                mf.HeaderToolTip = adj.Description;
                mf.AccessibleHeaderText = adj.AdjustmentCode;
                mf.HeaderStyle.Width =
                    mf.ItemStyle.Width = new Unit(4, UnitType.Em);
                mf.ItemStyle.HorizontalAlign = HorizontalAlign.Right;
                mf.DataFormatString = string.Empty;
                gvPayBillDtl.Columns.Add(mf);
            }

            mf = new MultiBoundField();
            mf.HeaderText = "Total Sanction";
            mf.AccessibleHeaderText = "TotalSanction";
            mf.ItemStyle.HorizontalAlign = HorizontalAlign.Right;
            mf.DataFormatString = string.Empty;
            gvPayBillDtl.Columns.Add(mf);

            foreach (Adjustment adj in distinctAdjustments.Where(p => p.IsDeduction))
            {
                mf = new MultiBoundField();
                mf.HeaderText = "Deductions|" + adj.ShortDescription;
                mf.HeaderToolTip = adj.Description;
                mf.AccessibleHeaderText = adj.AdjustmentCode;
                mf.ItemStyle.HorizontalAlign = HorizontalAlign.Right;
                mf.DataFormatString = string.Empty;
                gvPayBillDtl.Columns.Add(mf);
            }

            mf = new MultiBoundField();
            mf.HeaderText = "Total Deduction";
            mf.AccessibleHeaderText = "TotalDeduction";
            mf.ItemStyle.HorizontalAlign = HorizontalAlign.Right;
            mf.DataFormatString = string.Empty;
            gvPayBillDtl.Columns.Add(mf);

            mf = new MultiBoundField();
            mf.HeaderText = "Net Pay";
            mf.AccessibleHeaderText = "NetPay";
            mf.ItemStyle.HorizontalAlign = HorizontalAlign.Right;
            mf.DataFormatString = string.Empty;
            gvPayBillDtl.Columns.Add(mf);

            gvPayBillDtl.DataSource = MatrixIterator;
            gvPayBillDtl.DataBind();
        }

        public IEnumerable MatrixIterator
        {
            get
            {
                EmpPayDtl previousRow = null;
                foreach (var row in m_query)
                {
                    if (previousRow == null || (previousRow.EmployeePeriod.EmployeePeriodId != row.EmployeePeriod.EmployeePeriodId))
                    {
                        yield return row;
                    }
                    previousRow = row;
                }
            }
        }

        Dictionary<Adjustment, decimal?> m_dictAdjustmentsum = new Dictionary<Adjustment, decimal?>();

        decimal? _rsumallEarnings = 0.0M;
        decimal? _rsumallDeductions = 0.0M;
        decimal? _rsumallNetPay = 0.0M;
        decimal? _rsumBasic = 0.0M;
        protected void gvPayBillDtl_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            switch (e.Row.RowType)
            {
                case DataControlRowType.Header:
                    gvPayBillDtl.Caption = string.Format("<b>Paybill Register for {0} from {1:dd MMMM yyyy} to {2:dd MMMM yyyy}</b>", tbEmployee.Text, tbFromDate.ValueAsDate, tbToDate.ValueAsDate);
                    break;
                case DataControlRowType.DataRow:
                    EmpPayDtl peaCurrent = (EmpPayDtl)e.Row.DataItem;
                    var adjustmentSums = (from pea in m_query
                                          where pea.EmployeePeriod.EmployeeId == peaCurrent.EmployeePeriod.EmployeeId &&
                                                  pea.EmployeePeriod.SalaryPeriodId == peaCurrent.EmployeePeriod.SalaryPeriodId
                                          group pea by pea.Adjustment.IsDeduction into grp
                                          select new
                                          {
                                              IsDeduction = grp.Key,
                                              Sum = grp.Sum(p => Math.Round(p.Amount ?? 0, MidpointRounding.AwayFromZero))
                                          }).ToDictionary(p => p.IsDeduction, p => p.Sum);

                    decimal rsumEarnings = 0;
                    adjustmentSums.TryGetValue(false, out rsumEarnings);

                    decimal? basicSalary = peaCurrent.EmployeePeriod.BasicPay;
                    DataControlFieldCell cellBP = (from DataControlFieldCell c in e.Row.Cells
                                                   where c.ContainingField.AccessibleHeaderText == "BasicPay"
                                                   select c).Single();
                    cellBP.Text = basicSalary.HasValue ? string.Format("{0:N0}", basicSalary.Value) : string.Empty;
                    _rsumBasic += Math.Round(basicSalary ?? 0, MidpointRounding.AwayFromZero);

                    decimal? rsumSanction = ReportingUtilities.AddNullable(rsumEarnings,
                        Math.Round(peaCurrent.EmployeePeriod.BasicPay ?? 0, MidpointRounding.AwayFromZero));

                    DataControlFieldCell cellTS = (from DataControlFieldCell c in e.Row.Cells
                                                   where c.ContainingField.AccessibleHeaderText == "TotalSanction"
                                                   select c).Single();
                    cellTS.Text = rsumSanction.HasValue ? string.Format("{0:N0}", rsumSanction.Value) : string.Empty;
                    _rsumallEarnings += rsumEarnings;

                    decimal rsumDeductions = 0;
                    adjustmentSums.TryGetValue(true, out rsumDeductions);

                    DataControlFieldCell cellTD = (from DataControlFieldCell c in e.Row.Cells
                                                   where c.ContainingField.AccessibleHeaderText == "TotalDeduction"
                                                   select c).Single();
                    cellTD.Text = string.Format("{0:N0}", rsumDeductions);
                    _rsumallDeductions += rsumDeductions;

                    decimal? rsumNetPay = ReportingUtilities.SubtractNullable(rsumSanction, rsumDeductions);

                    DataControlFieldCell cellNP = (from DataControlFieldCell c in e.Row.Cells
                                                   where c.ContainingField.AccessibleHeaderText == "NetPay"
                                                   select c).Single();
                    cellNP.Text = rsumNetPay.HasValue ? string.Format("{0:N0}", rsumNetPay.Value) : string.Empty;
                    _rsumallNetPay += rsumNetPay ?? 0;

                    foreach (var pea in m_query.Where(p => p.EmployeePeriod.SalaryPeriodId == peaCurrent.EmployeePeriod.SalaryPeriodId && p.EmployeePeriod.EmployeeId == peaCurrent.EmployeePeriod.EmployeeId))
                    {
                        DataControlFieldCell cellP = (from DataControlFieldCell c in e.Row.Cells
                                                      where c.ContainingField.AccessibleHeaderText == pea.Adjustment.AdjustmentCode
                                                      select c).Single();
                        cellP.Text = string.Format("{0:N0}", pea.Amount);
                        UpdateSum(m_dictAdjustmentsum, Math.Round(pea.Amount ?? 0, MidpointRounding.AwayFromZero), pea.Adjustment);
                    }
                    break;

                case DataControlRowType.Footer:
                    foreach (KeyValuePair<Adjustment, decimal?> kvp in m_dictAdjustmentsum)
                    {
                        DataControlFieldCell cellDic = (from DataControlFieldCell c in e.Row.Cells
                                                        where c.ContainingField.AccessibleHeaderText == kvp.Key.AdjustmentCode
                                                        select c).Single();
                        cellDic.Text = string.Format("{0:N0}", kvp.Value);
                        cellDic.HorizontalAlign = HorizontalAlign.Right;
                    }

                    DataControlFieldCell cellBPFooter = (from DataControlFieldCell c in e.Row.Cells
                                                         where c.ContainingField.AccessibleHeaderText == "BasicPay"
                                                         select c).Single();
                    cellBPFooter.Text = _rsumBasic.HasValue ? string.Format("{0:N0}", _rsumBasic.Value) : string.Empty;
                    cellBPFooter.HorizontalAlign = HorizontalAlign.Right;

                    _rsumallEarnings += _rsumBasic;
                    DataControlFieldCell cellTSF = (from DataControlFieldCell c in e.Row.Cells
                                                    where c.ContainingField.AccessibleHeaderText == "TotalSanction"
                                                    select c).Single();
                    cellTSF.Text = _rsumallEarnings.HasValue ? string.Format("{0:N0}", _rsumallEarnings.Value) : string.Empty;
                    cellTSF.HorizontalAlign = HorizontalAlign.Right;

                    DataControlFieldCell cellTDF = (from DataControlFieldCell c in e.Row.Cells
                                                    where c.ContainingField.AccessibleHeaderText == "TotalDeduction"
                                                    select c).Single();
                    cellTDF.Text = _rsumallDeductions.HasValue ? string.Format("{0:N0}", _rsumallDeductions.Value) : string.Empty;
                    cellTDF.HorizontalAlign = HorizontalAlign.Right;

                    DataControlFieldCell cellNPF = (from DataControlFieldCell c in e.Row.Cells
                                                    where c.ContainingField.AccessibleHeaderText == "NetPay"
                                                    select c).Single();
                    cellNPF.Text = _rsumallNetPay.HasValue ? string.Format("{0:N0}", _rsumallNetPay.Value) : string.Empty;
                    cellNPF.HorizontalAlign = HorizontalAlign.Right;
                    break;
                default:
                    break;
            }
        }

        private void UpdateSum(Dictionary<Adjustment, decimal?> dictAdjCodesum,
          decimal? amount, Adjustment adj)
        {
            decimal? curValue;
            bool bFound = dictAdjCodesum.TryGetValue(adj, out curValue);
            if (bFound)
            {
                dictAdjCodesum[adj] = ReportingUtilities.AddNullable(amount, curValue);
            }
            else
            {
                dictAdjCodesum.Add(adj, amount);
            }
            return;
        }

        protected void lblGrossBCA_PreRender(object sender, EventArgs e)
        {
            Label lbl = (Label)sender;
            if (!btnGo.IsPageValid())
            {
                return;
            }
            else
            {
                using (PayrollDataContext db = new PayrollDataContext(ReportingUtilities.DefaultConnectString))
                {
                    var text = (from pea in db.PeriodEmployeeAdjustments
                                where pea.Adjustment.AdjustmentCategory.AdjustmentCategoryCode == "BCA" &&
                                      pea.EmployeePeriod.Employee.EmployeeId == Convert.ToInt32(tbEmployee.Value) &&
                                      pea.EmployeePeriod.SalaryPeriod.SalaryPeriodStart >= tbFromDate.ValueAsDate &&
                                      pea.EmployeePeriod.SalaryPeriod.SalaryPeriodEnd <= tbToDate.ValueAsDate
                                select new
                                {
                                    Amount = pea.Amount,
                                    Deduction = pea.Adjustment.IsDeduction,
                                }).ToList();
                    var bca = text.Where(p => p.Deduction == false).Sum(p => p.Amount.HasValue ? p.Amount.Value : 0);
                    lbl.Text = bca.ToString("N2");
                }
            }
        }


        protected void lblSlabDeduction_PreRender(object sender, EventArgs e)
        {
            Label lbl = (Label)sender;
            if (!btnGo.IsPageValid())
            {
                return;
            }
            else
            {
                using (PayrollDataContext db = new PayrollDataContext(ReportingUtilities.DefaultConnectString))
                {
                    var text = (from pea in db.PeriodEmployeeAdjustments
                                where pea.Adjustment.AdjustmentCategory.AdjustmentCategoryCode == "SLABDEDUCTION" &&
                                      pea.EmployeePeriod.Employee.EmployeeId == Convert.ToInt32(tbEmployee.Value) &&
                                      pea.EmployeePeriod.SalaryPeriod.SalaryPeriodStart >= tbFromDate.ValueAsDate &&
                                      pea.EmployeePeriod.SalaryPeriod.SalaryPeriodEnd <= tbToDate.ValueAsDate
                                select
                                    pea.Amount
                               ).ToList();

                    lbl.Text = text.Sum(p => p.HasValue ? p.Value : 0).ToString("N2");
                }
            }
        }

        protected void lblNetBCA_PreRender(object sender, EventArgs e)
        {
            Label lbl = (Label)sender;
            if (!btnGo.IsPageValid())
            {
                return;
            }
            else
            {
                using (PayrollDataContext db = new PayrollDataContext(ReportingUtilities.DefaultConnectString))
                {
                    var text = (from pea in db.PeriodEmployeeAdjustments
                                where (pea.Adjustment.AdjustmentCategory.AdjustmentCategoryCode == "SLABDEDUCTION" ||
                                      (pea.Adjustment.AdjustmentCategory.AdjustmentCategoryCode == "BCA")) &&
                                      pea.EmployeePeriod.Employee.EmployeeId == Convert.ToInt32(tbEmployee.Value) &&
                                      pea.EmployeePeriod.SalaryPeriod.SalaryPeriodStart >= tbFromDate.ValueAsDate &&
                                      pea.EmployeePeriod.SalaryPeriod.SalaryPeriodEnd <= tbToDate.ValueAsDate
                                select new
                                {
                                    GrossBCA = pea.Adjustment.AdjustmentCategory.AdjustmentCategoryCode == "BCA" ? pea.Amount ?? 0 : 0,
                                    SlabDeduction = pea.Adjustment.AdjustmentCategory.AdjustmentCategoryCode == "SLABDEDUCTION" ? pea.Amount ?? 0 : 0,
                                    Deduction = pea.Adjustment.IsDeduction
                                }).ToList();
                    decimal? sumLblText = 0.0M;
                    if (text.Count > 0)
                    {
                        var data = (from txt in text
                                    let bca = text.Where(p => p.Deduction == false).Sum(p => p.GrossBCA)
                                    let rec = text.Where(p => p.Deduction == true).Sum(p => p.GrossBCA)
                                    select new
                                    {

                                        NetBCA = (bca - rec) - txt.SlabDeduction
                                    }).Take(1);


                        foreach (var item in data)
                        {
                            sumLblText += item.NetBCA;
                        }
                    }
                        lbl.Text = (sumLblText.HasValue ? sumLblText.Value : 0).ToString("N2");
                    
                }
            }
        }

        protected void dsEmployee_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            if (!btnGo.IsPageValid())
            {
                e.Cancel = true;
                return;
            }
            PayrollDataContext db = (PayrollDataContext)dsEmployee.Database;
            ServicePeriod sp = db.ServicePeriods.Where(p => p.EmployeeId == Convert.ToInt32(tbEmployee.Value))
                            .OrderByDescending(p => p.PeriodStartDate).FirstOrDefault() ?? new ServicePeriod();
            e.Result = from emp in db.Employees
                       where emp.EmployeeId == Convert.ToInt32(tbEmployee.Value)
                       select new
                       {
                           FullName = emp.FullName,
                           EmployeeCode = emp.EmployeeCode,
                           Designation = emp.Designation,
                           Grade = sp.Grade,
                           EmpTypeDescription = emp.EmployeeType.Description,
                           PayScale = sp.PayScale,
                           JoiningDate = emp.JoiningDate,
                           DateOfIncrement = sp.DateOfIncrement,
                           IncrementAmount = sp.IncrementAmount,
                           Gender = emp.Gender,
                           DateOfBirth = emp.DateOfBirth,
                           MaritalStatusType = emp.MaritalStatus.MaritalStatusType,
                           PostedAt = emp.Station.StationName,
                           DivisionName = emp.Division.DivisionName,
                           BankAccountNo = emp.BankAccountNo,
                           BankName = emp.BankName,
                           GPFAccountNo = emp.GPFAccountNo,
                           GISAccountNumber = emp.GISAccountNumber,
                           CitizenCardNo = emp.CitizenCardNo,
                           Tpn = emp.Tpn,
                           IsBhutanese = emp.IsBhutanese ? "Bhutanese" : "Non Bhutanese",
                           ParentOrganization = emp.ParentOrganization,
                           EmployeeId = emp.EmployeeId
                       };
        }

    }
}
