/*
 *  Programming: Eclipse Systems (P) Ltd., NOIDA, INDIA
 *  E-mail: support@eclsys.com
 *
 *  $Workfile:   CumulativeYearlyExpenditure.aspx.cs  $
 *  $Revision: 34902 $
 *  $Author: ssinghal $
 *  $Date: 2010-09-04 17:37:28 +0530 (Sat, 04 Sep 2010) $
 *  $Modtime:   Jul 10 2008 17:12:34  $
 *
 *  $Log:   S:/Projects/PHPA2/archives/Finance/Reports/CumulativeYearlyExpenditure.aspx.cs-arc  $
 * 
 *    Rev 1.69   Jul 16 2008 16:05:12   pshishodia
 * WIP
 * 
 *    Rev 1.68   Jul 10 2008 17:13:16   pshishodia
 * WIP
 * 
 *    Rev 1.67   Jul 09 2008 17:40:56   vraturi
 * PVCS Template Added.
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using Eclipse.PhpaLibrary.Reporting;
using Eclipse.PhpaLibrary.Web;
using EclipseLibrary.Web.JQuery;

namespace Finance.Reports
{
    public partial class CumulativeYearlyExpenditure : PageBase
    {
        private IQueryable<SubTotalItem> m_query;
        protected void ds_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            ReportingDataContext db = (ReportingDataContext)this.ds.Database;

            //DataLoadOptions dlo = new DataLoadOptions();
            //dlo.LoadWith<RoVoucherDetail>(p => p.RoHeadHierarchy);
            //db.LoadOptions = dlo;

            m_query = (from vd in db.RoVoucherDetails
                       where vd.RoHeadHierarchy.HeadOfAccountType == HOA_EXPENDITURE
                             || vd.RoHeadHierarchy.HeadOfAccountType == HOA_TOUR_EXPENSES
                       let yearStart = new DateTime(vd.RoVoucher.VoucherDate.Year, 4, 1)
                       group vd by new
                       {
                           FinancialYearStart = vd.RoVoucher.VoucherDate > yearStart ? yearStart : yearStart.AddYears(-1),
                           RoHeadHierarchy = vd.RoHeadHierarchy
                       } into g
                       orderby g.Key.RoHeadHierarchy.SortableName, g.Key.FinancialYearStart
                       select new ResultItem(g.Key.RoHeadHierarchy, false)
                       {
                           ProjectCost = g.Key.RoHeadHierarchy.ProjectCost,
                           FinancialYearStart = g.Key.FinancialYearStart,
                           Expenditure = g.Sum(p => (p.DebitAmount ?? 0) - (p.CreditAmount ?? 0))
                       }).Cast<SubTotalItem>();
            e.Result = this.QueryIterator();
        }

        private const string HOA_EXPENDITURE = "EXPENDITURE";
        private const string HOA_TOUR_EXPENSES = "TOUR_EXPENSES";

        private LevelSubtotals m_levelSubtotals;
        protected IEnumerable<SubTotalItem> QueryIterator()
        {
            ReportingDataContext db = (ReportingDataContext)ds.Database;
            m_levelSubtotals = new LevelSubtotals(db);
            IEnumerable<SubTotalItem> subTotals;
            foreach (ResultItem item in m_query)
            {
                subTotals = m_levelSubtotals.ProcessItem(item);

                if (subTotals != null)
                {
                    foreach (var subtotalItem in subTotals)
                    {
                        yield return subtotalItem;
                    }
                }

                yield return item;
            }

            subTotals = m_levelSubtotals.GetFinalSubTotals();

            if (subTotals != null)
            {
                foreach (var subtotalItem in subTotals)
                {
                    yield return subtotalItem;
                }
            }
        }

        protected void matrix_RowDataBound(object sender, MatrixRowEventArgs e)
        {
            HeadSubTotals subtotals = e.Row.DataItem as HeadSubTotals;
            if (subtotals != null)
            {
                e.Row.CssClass += " ui-priority-primary";
                MatrixField mf = (MatrixField)sender;
                decimal rowtotal = 0;
                string text;
                foreach (var item in subtotals.SubTotals)
                {
                    MatrixColumn col = mf.MatrixColumns.Where(p => p.ColumnValues.ContainsKey("FinancialYearStart"))
                        .Single(p => (DateTime)p.ColumnValues["FinancialYearStart"] == item.Key);
                    text = string.Format(mf.DataValueFormatString, item.Value);
                    e.MatrixRow.SetCellText(col, text);
                    rowtotal += item.Value;
                }
                text = string.Format(mf.DataValueFormatString, rowtotal);
                e.MatrixRow.SetRowTotalText(0, text);
            }

        }

        private class ResultItem : SubTotalItem
        {
            public ResultItem(RoHeadHierarchy acc, bool isSubTotal)
                : base(acc, isSubTotal)
            {
                if (isSubTotal)
                {
                    this.ProjectCost = null;
                }
                else
                {
                    if (acc.RevisedProjectCost == null)
                    {
                        this.ProjectCost = acc.ProjectCost;
                    }
                    else
                    {
                        this.ProjectCost = acc.RevisedProjectCost.Value;
                    }
                }
            }


            public decimal? ProjectCost { get; set; }

            public override SubTotalItem CreateNew(RoHeadHierarchy head)
            {
                return new HeadSubTotals(head);
            }

            protected override void ExecuteUpdateSubtotals(SubTotalItem subtotalItem)
            {
                throw new NotImplementedException();
            }

            public DateTime FinancialYearStart { get; set; }

            public decimal Expenditure { get; set; }
        }

        private class HeadSubTotals : ResultItem
        {
            private readonly Dictionary<DateTime, decimal> _dict;
            public HeadSubTotals(RoHeadHierarchy head)
                : base(head, true)
            {
                _dict = new Dictionary<DateTime, decimal>();
            }
            public override SubTotalItem CreateNew(RoHeadHierarchy head)
            {
                throw new NotImplementedException();
            }

            /// <summary>
            /// This value has no relevance for us
            /// </summary>
            private new DateTime? FinancialYearStart { get; set; }

            public IDictionary<DateTime, decimal> SubTotals
            {
                get
                {
                    return _dict;
                }
            }

            protected override void ExecuteUpdateSubtotals(SubTotalItem subtotalItem)
            {
                ResultItem item = (ResultItem)subtotalItem;
                if (_dict.ContainsKey(item.FinancialYearStart))
                {
                    _dict[item.FinancialYearStart] += item.Expenditure;
                }
                else
                {
                    _dict.Add(item.FinancialYearStart, item.Expenditure);
                }
            }
        }
    }
}
