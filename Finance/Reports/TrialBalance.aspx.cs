/*
 *  Programming: Eclipse Systems (P) Ltd., NOIDA, INDIA
 *  E-mail: support@eclsys.com
 *
 *  $Workfile:   TrialBalance.aspx.cs  $
 *  $Revision: 38319 $
 *  $Author: ssingh $
 *  $Date: 2010-12-02 11:57:35 +0530 (Thu, 02 Dec 2010) $
 *  $Modtime:   Jul 28 2008 14:50:48  $
 *
 * 
 */
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using Eclipse.PhpaLibrary.Reporting;
using Eclipse.PhpaLibrary.Web;
using EclipseLibrary.Web.JQuery.Input;
using OfficeOpenXml;
using System.IO;
using System.Drawing;
using System.Web.UI;

namespace PhpaAll.Reports
{
    public partial class TrialBalance : PageBase
    {
        private IQueryable<ResultItem> m_query;

        /// <summary>
        /// Numbers smaller than this are treated as 0
        /// </summary>
        private const decimal EPSILON = 0.005M;
        private DateTime _tbMonthValue;

        protected DateTime DateFrom
        {
            get
            {
                return _tbMonthValue.MonthStartDate();
            }
        }

        protected DateTime DateTo
        {
            get
            {
                return _tbMonthValue;
            }
        }

        protected void dsVoucherDetails_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            _tbMonthValue = (DateTime)tbMonth.ValueAsDate.Value;
            if (_tbMonthValue == null)
            {
                e.Cancel = true;
                return;
            }
            this.Title = string.Format("Trial Balance as of {0:dd MMMM yyyy}", _tbMonthValue);
            DateTime dateMonthStart = _tbMonthValue.MonthStartDate();
            DateTime dateFinancialYearStart = _tbMonthValue.FinancialYearStartDate();
            ReportingDataContext db = (ReportingDataContext)this.dsVoucherDetails.Database;
            m_query = (from vd in db.RoVoucherDetails
                       where vd.RoVoucher.VoucherDate <= _tbMonthValue
                       group vd by vd.RoHeadHierarchy into grouping
                       orderby grouping.Key.SortableName
                       select new ResultItem(grouping.Key, false)
                       {
                           CurrentMonthGrossDebitSum = grouping.Sum(
                           vd => vd.RoVoucher.VoucherDate >= dateMonthStart && vd.RoVoucher.VoucherDate <= _tbMonthValue ?
                                vd.DebitAmount : null),
                           CurrentMonthGrossCreditSum = grouping.Sum(
                            vd => vd.RoVoucher.VoucherDate >= dateMonthStart && vd.RoVoucher.VoucherDate <= _tbMonthValue ?
                                vd.CreditAmount : null),
                           CurrentYearNetDebitSum = grouping.Sum(
                           vd => vd.RoVoucher.VoucherDate >= dateFinancialYearStart && vd.RoVoucher.VoucherDate <= _tbMonthValue ?
                           vd.DebitAmount : null),
                           CurrentYearNetCreditSum = grouping.Sum(
                           vd => vd.RoVoucher.VoucherDate >= dateFinancialYearStart && vd.RoVoucher.VoucherDate <= _tbMonthValue ?
                           vd.CreditAmount : null),
                           CumulativeDebitSum = grouping.Sum(
                             vd => vd.RoVoucher.VoucherDate <= _tbMonthValue ? vd.DebitAmount ?? 0 : 0),
                           CumulativeCreditSum = grouping.Sum(
                            vd => vd.RoVoucher.VoucherDate <= _tbMonthValue ? vd.CreditAmount ?? 0 : 0)
                           //CumulativeBalance = grouping.Sum(vd => vd.RoVoucher.VoucherDate <= date ? vd.DebitAmount ?? 0 - vd.CreditAmount ?? 0 : 0)
                       }).Where(i => Math.Abs(i.CurrentMonthGrossCreditSum ?? 0) >= EPSILON ||
                            Math.Abs(i.CurrentMonthGrossCreditSum ?? 0) >= EPSILON ||
                            Math.Abs(i.CurrentYearNetDebitSum ?? 0) >= EPSILON ||
                            Math.Abs(i.CurrentYearNetCreditSum ?? 0) >= EPSILON ||
                            Math.Abs(i.CumulativeDebitSum ?? 0) >= EPSILON ||
                            Math.Abs(i.CumulativeCreditSum ?? 0) >= EPSILON);
            e.Result = this.QueryIterator();

        }

        /// <summary>
        /// The least recent entry contains level sub total for Level 0
        /// </summary>

        private LevelSubtotals m_levelSubtotals;

        public IEnumerable<ResultItem> QueryIterator()
        {
            ReportingDataContext db = (ReportingDataContext)dsVoucherDetails.Database;
            m_levelSubtotals = new LevelSubtotals(db);
            IEnumerable subTotals;
            foreach (ResultItem item in m_query)
            {
                subTotals = m_levelSubtotals.ProcessItem(item);

                if (subTotals != null)
                {
                    foreach (ResultItem subtotalItem in subTotals)
                    {

                        yield return subtotalItem;
                    }
                }

                yield return item;
            }

            subTotals = m_levelSubtotals.GetFinalSubTotals();

            if (subTotals != null)
            {
                foreach (ResultItem subtotalItem in subTotals)
                {
                    yield return subtotalItem;
                }
            }

        }

        decimal _sumCMGDS;
        decimal _sumCMGCS;
        decimal _sumCYNDS;
        decimal _sumCYNCS;
        decimal _sumCYUDD;
        decimal _sumCYUDC;

        protected void gvTrialBalance_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            switch (e.Row.RowType)
            {
                
                case DataControlRowType.DataRow:
                    ResultItem item = (ResultItem)e.Row.DataItem;
                    if (item.IsSubTotal)
                    {
                        // Subtotal row of item.
                        e.Row.Font.Bold = true;
                        e.Row.Font.Italic = true;
                        e.Row.Font.Size = new FontUnit(Unit.Point(12));

                        HyperLink hlNet = (HyperLink)e.Row.FindControl("hlMonthDebit");
                        hlNet.NavigateUrl = null;

                        hlNet = (HyperLink)e.Row.FindControl("hlMonthCredit");
                        hlNet.NavigateUrl = null;

                        hlNet = (HyperLink)e.Row.FindControl("hlYearDebit");
                        hlNet.NavigateUrl = null;

                        hlNet = (HyperLink)e.Row.FindControl("hlYearCredit");
                        hlNet.NavigateUrl = null;
                    }
                    else
                    {
                        //Total row of item.
                        if (item.CurrentMonthGrossDebitSum.HasValue)
                        {
                            _sumCMGDS += item.CurrentMonthGrossDebitSum.Value;
                        }
                        if (item.CurrentMonthGrossCreditSum.HasValue)
                        {
                            _sumCMGCS += item.CurrentMonthGrossCreditSum.Value;
                        }
                        if (item.CurrentYearNetDebitSum.HasValue)
                        {
                            _sumCYNDS += item.CurrentYearNetDebitSum.Value;
                        }
                        if (item.CurrentYearNetCreditSum.HasValue)
                        {
                            _sumCYNCS += item.CurrentYearNetCreditSum.Value;
                        }
                        if (item.CumulativeDebitSum.HasValue)
                        {
                            _sumCYUDD += item.CumulativeDebitSum.Value;
                        }
                        if (item.CumulativeCreditSum.HasValue)
                        {
                            _sumCYUDC += item.CumulativeCreditSum.Value;
                        }
                    }

                    break;

                case DataControlRowType.Footer:
                    // Prevents printing of the footer on each page
                    e.Row.TableSection = TableRowSection.TableBody;
                    DataControlFieldCell cellCMGDS = (from DataControlFieldCell c in e.Row.Cells
                                                      where c.ContainingField.AccessibleHeaderText == "CurrentMonthGrossDebitSum"
                                                      select c).Single();
                    cellCMGDS.Text = _sumCMGDS.ToString("N2");
                    DataControlFieldCell cellCMGCS = (from DataControlFieldCell c in e.Row.Cells
                                                      where c.ContainingField.AccessibleHeaderText == "CurrentMonthGrossCreditSum"
                                                      select c).Single();
                    cellCMGCS.Text = _sumCMGCS.ToString("N2");
                    DataControlFieldCell cellCYNDS = (from DataControlFieldCell c in e.Row.Cells
                                                      where c.ContainingField.AccessibleHeaderText == "CurrentYearNetDebitSum"
                                                      select c).Single();
                    cellCYNDS.Text = _sumCYNDS.ToString("N2");
                    DataControlFieldCell cellCYNCS = (from DataControlFieldCell c in e.Row.Cells
                                                      where c.ContainingField.AccessibleHeaderText == "CurrentYearNetCreditSum"
                                                      select c).Single();
                    cellCYNCS.Text = _sumCYNCS.ToString("N2");
                    DataControlFieldCell cellCBS = (from DataControlFieldCell c in e.Row.Cells
                                                    where c.ContainingField.AccessibleHeaderText == "CumulativeDebitSum"
                                                    select c).Single();
                    cellCBS.Text = _sumCYUDD.ToString("N2");
                    DataControlFieldCell cellCCS = (from DataControlFieldCell c in e.Row.Cells
                                                    where c.ContainingField.AccessibleHeaderText == "CumulativeCreditSum"
                                                    select c).Single();
                    cellCCS.Text = _sumCYUDC.ToString("N2");
                    break;
            }
            

        }

        /// <summary>
        /// Display the report for the given month.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnShowReport_Click(object sender, EventArgs e)
        {
            ButtonEx btn = (ButtonEx)sender; 
            if (btn.IsPageValid())
            {
                gvTrialBalance.DataBind();
            }
        }

        /// <summary>
        /// This class represents the item which is retrieved by the query
        /// </summary>
        public class ResultItem : SubTotalItem
        {
            public ResultItem(RoHeadHierarchy acc, bool isSubtotal)
                : base(acc, isSubtotal)
            {
            }

            /// <summary>
            /// Adds the amounts of the passed item to the current amounts
            /// </summary>
            /// <param name="item"></param>
            protected override void ExecuteUpdateSubtotals(SubTotalItem subtotalItem)
            {
                ResultItem item = (ResultItem)subtotalItem;
                this.CurrentMonthGrossDebitSum = ReportingUtilities.AddNullable(this.CurrentMonthGrossDebitSum, item.CurrentMonthGrossDebitSum);
                this.CurrentMonthGrossCreditSum = ReportingUtilities.AddNullable(this.CurrentMonthGrossCreditSum, item.CurrentMonthGrossCreditSum);
                this.CurrentYearNetDebitSum = ReportingUtilities.AddNullable(this.CurrentYearNetDebitSum, item.CurrentYearNetDebitSum);
                this.CurrentYearNetCreditSum = ReportingUtilities.AddNullable(this.CurrentYearNetCreditSum, item.CurrentYearNetCreditSum);
                this.CumulativeCreditSum = ReportingUtilities.AddNullable(this.CumulativeCreditSum, item.CumulativeCreditSum);
                this.CumulativeDebitSum = ReportingUtilities.AddNullable(this.CumulativeDebitSum, item.CumulativeDebitSum);
            }

            /// <summary>
            /// The sum of debits during the current month
            /// </summary>
            public decimal? CurrentMonthGrossDebitSum
            {
                get;
                set;
            }

            /// <summary>
            /// The sum of credits during the current month
            /// </summary>
            public decimal? CurrentMonthGrossCreditSum
            {
                get;
                set;
            }


            /// <summary>
            /// The sum of debits during the current financial year.
            /// </summary>
            public decimal? CurrentYearNetDebitSum
            {
                get;
                set;
            }

            /// <summary>
            /// The sum of credits during the current financial year.
            /// </summary>
            public decimal? CurrentYearNetCreditSum
            {
                get;
                set;
            }

            /// <summary>
            /// Sum of debits during current financial year prior to current month
            /// </summary>
            private decimal? _cumulativeDebitSum;

            /// <summary>
            /// Sharad 30 Aug 2010: For subtotals, we do not want to net debits and credits
            /// </summary>
            public decimal? CumulativeDebitSum
            {
                get
                {
                    if (this.IsSubTotal)
                    {
                        return _cumulativeDebitSum;
                    }
                    else
                    {
                        if (_cumulativeDebitSum >= _cumulativeCreditSum)
                        {
                            return (_cumulativeDebitSum - _cumulativeCreditSum);
                        }
                        else
                        {
                            return 0;
                        }
                    }
                }
                set
                {
                    _cumulativeDebitSum = value;
                }

            }

            /// <summary>
            /// Sum of credits during current financial year prior to current month
            /// </summary>
            private decimal? _cumulativeCreditSum;

            /// <summary>
            /// Sharad 30 Aug 2010: For subtotals, we do not want to net debits and credits
            /// </summary>
            public decimal? CumulativeCreditSum
            {
                get
                {
                    if (this.IsSubTotal)
                    {
                        return _cumulativeCreditSum;
                    }
                    else
                    {
                        if (_cumulativeDebitSum < _cumulativeCreditSum)
                        {
                            return (_cumulativeCreditSum - _cumulativeDebitSum);
                        }
                        else
                        {
                            return 0;
                        }
                    }
                }
                set
                {
                    _cumulativeCreditSum = value;
                }
            }

            public override SubTotalItem CreateNew(RoHeadHierarchy head)
            {
                return new ResultItem(head, true);
            }
        }
        protected void ExportBtn_Click(object sender, EventArgs e)
        {
            Response.Clear();
            Response.Buffer = true;
           // Response.AddHeader("content-disposition", "attachment;filename=TrialBalance.xls");
            Response.Charset = "";
           // Response.ContentType = "application/vnd.ms-excel";


            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("content-disposition", "attachment;filename=labtest.xls");

            using (StringWriter sw = new StringWriter())
            {
                HtmlTextWriter hw = new HtmlTextWriter(sw);
                gvTrialBalance.RenderControl(hw);
                Response.Output.Write(sw.ToString());
                Response.Flush();
                Response.End();
            }
        }  
    }

}
