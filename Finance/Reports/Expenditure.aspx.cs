/*
 *  Programming: Eclipse Systems (P) Ltd., NOIDA, INDIA
 *  E-mail: support@eclsys.com
 *
 *  $Workfile:   Expenditure.aspx.cs  $
 *  $Revision: 38306 $
 *  $Author: ssingh $
 *  $Date: 2010-12-02 11:13:24 +0530 (Thu, 02 Dec 2010) $
 *  $Modtime:   Jul 21 2008 20:45:24  $
 *
 *  $Log:   S:/Projects/PHPA2/archives/Finance/Reports/Expenditure.aspx.cs-arc  $
 * 
 *    Rev 1.83   Jul 21 2008 20:54:16   pshishodia
 * WIP
 * 
 *    Rev 1.82   Jul 16 2008 15:26:16   pshishodia
 * WIP
 * 
 *    Rev 1.81   Jul 10 2008 17:37:04   pshishodia
 * WIP
 * 
 *    Rev 1.80   Jul 09 2008 17:40:58   vraturi
 * PVCS Template Added.
 */
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using Eclipse.PhpaLibrary.Reporting;
using Eclipse.PhpaLibrary.Web;
using EclipseLibrary.Web.JQuery.Input;

namespace Finance.Reports
{
    public partial class Expenditure : PageBase
    {

        /// <summary>
        /// Setting string for HeadOfAccountType.
        /// </summary>

        //private const string HOA_EXPENDITURE = "EXPENDITURE";
        //private const string HOA_TOUR_EXPENSES = "TOUR_EXPENSES";

        /// <summary>
        /// Display the Expenditure for each Head of Account.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private IQueryable<ResultItem> m_query;

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

        protected void dsExpenditure_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            if (!btnShowReport.IsPageValid())
            {
                e.Cancel = true;
            }
            else
            {
                _tbMonthValue = (DateTime)tbMonth.ValueAsDate;
                DateTime dateMonthStart = _tbMonthValue.MonthStartDate();
                DateTime dateFinancialYearStart = _tbMonthValue.FinancialYearStartDate();
                
                this.Title = string.Format("Expenditure Statement as on {0:dd/MM/yyyy}", tbMonth.Value);

               ReportingDataContext db = (ReportingDataContext)this.dsExpenditure.Database;
               m_query = from vd in db.RoVoucherDetails
                         where HeadOfAccountHelpers.JobExpenses.Contains(vd.RoHeadHierarchy.HeadOfAccountType) &&
                           //where (vd.RoHeadHierarchy.HeadOfAccountType == HOA_EXPENDITURE  ||
                           //       vd.RoHeadHierarchy.HeadOfAccountType == HOA_TOUR_EXPENSES) &&
                           vd.RoVoucher.VoucherDate <= _tbMonthValue
                           group vd by vd.RoHeadHierarchy
                           into grouping
                             orderby grouping.Key.SortableName
                               select new ResultItem(grouping.Key, false)
                               {
                                   HeadId=grouping.Key.HeadOfAccountId,
                                   CurrentYearDuringMonthExpenses = grouping.Sum(
                                   vd => vd.RoVoucher.VoucherDate >= dateMonthStart && vd.RoVoucher.VoucherDate <= _tbMonthValue ?
                                   vd.DebitAmount ?? 0 - vd.CreditAmount ?? 0 : 0),

                                   CurrentYearUptoMonthExpenses = grouping.Sum(
                                   vd => vd.RoVoucher.VoucherDate >= dateFinancialYearStart && vd.RoVoucher.VoucherDate <= _tbMonthValue ?
                                   vd.DebitAmount ?? 0 - vd.CreditAmount ?? 0 : 0),

                                   UptoPreviousYearsExpenses = grouping.Sum(
                                   vd => vd.RoVoucher.VoucherDate < dateFinancialYearStart ?
                                   vd.DebitAmount ?? 0 - vd.CreditAmount ?? 0 : 0),

                                   Cumulative = grouping.Sum(
                                   vd => vd.RoVoucher.VoucherDate < dateFinancialYearStart ?
                                   vd.DebitAmount ?? 0 - vd.CreditAmount ?? 0 : 0) +
                                   grouping.Sum(
                                   vd => vd.RoVoucher.VoucherDate >= dateFinancialYearStart && vd.RoVoucher.VoucherDate <= _tbMonthValue ?
                                   vd.DebitAmount ?? 0 - vd.CreditAmount ?? 0 : 0)
                               };
               e.Result = this.QueryIterator();
            }
        }

        /// <summary>
        /// The least recent entry contains level sub total for Level 0
        /// </summary>

        private LevelSubtotals m_levelSubtotals;

        public IEnumerable<ResultItem> QueryIterator()
        {
            ReportingDataContext db = (ReportingDataContext)dsExpenditure.Database;
            m_levelSubtotals = new LevelSubtotals(db);
            IEnumerable<SubTotalItem> subTotals;
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

        

        /// <summary>
        ///Sum of the data for the corresponding Parent Head.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        decimal sumProjectCost;
        decimal sumCYDME;
        decimal sumCYUME;
        decimal sumUPYE;
        decimal sumCumulative;

        /// <summary>
        /// Sum of the data for the corresponding Parent Head.
        /// </summary>
        protected void gvExpenditure_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            switch (e.Row.RowType)
            {
                case DataControlRowType.DataRow:
                    ResultItem item = (ResultItem)e.Row.DataItem;
                    if (item.IsSubTotal)
                    {
                        //Subtotal Row
                        e.Row.Font.Bold = true;
                        e.Row.Font.Italic = true;
                        HyperLink hlDuringMonth = (HyperLink)e.Row.FindControl("hlDuringMonth");
                        hlDuringMonth.NavigateUrl = null;

                        HyperLink hlUptoMonth = (HyperLink)e.Row.FindControl("hlUptoMonth");
                        hlUptoMonth.NavigateUrl = null;
                    }
                    else
                    {
                        //Total row of item.
                        sumProjectCost += item.ProjectCost;
                        sumCYDME += item.CurrentYearDuringMonthExpenses;
                        sumCYUME += item.CurrentYearUptoMonthExpenses;
                        sumUPYE += item.UptoPreviousYearsExpenses;
                        sumCumulative += item.Cumulative;
                    }
                   break;

                case DataControlRowType.Footer:
                   DataControlFieldCell cellPC = (from DataControlFieldCell c in e.Row.Cells
                                                   where c.ContainingField.AccessibleHeaderText == "ProjectCost"
                                                   select c).Single();
                   cellPC.Text = sumProjectCost.ToString("N2");

                   DataControlFieldCell cellCYDME = (from DataControlFieldCell c in e.Row.Cells
                                                     where c.ContainingField.AccessibleHeaderText == "CurrentYearDuringMonthExpenses"
                                                     select c).Single();
                   cellCYDME.Text = sumCYDME.ToString("N2");

                   DataControlFieldCell cellCYUME = (from DataControlFieldCell c in e.Row.Cells
                                                     where c.ContainingField.AccessibleHeaderText == "CurrentYearUptoMonthExpenses"
                                                     select c).Single();
                   cellCYUME.Text = sumCYUME.ToString("N2");

                   DataControlFieldCell cellUPYE = (from DataControlFieldCell c in e.Row.Cells
                                                     where c.ContainingField.AccessibleHeaderText == "UptoPreviousYearsExpenses"
                                                     select c).Single();
                   cellUPYE.Text = sumUPYE.ToString("N2");

                   DataControlFieldCell cellC = (from DataControlFieldCell c in e.Row.Cells
                                                    where c.ContainingField.AccessibleHeaderText == "Cumulative"
                                                    select c).Single();
                   cellC.Text = sumCumulative.ToString("N2");
                   break;
            }
        }

        protected void btnShowReport_Click(object sender, EventArgs e)
        {
            ButtonEx btn = (ButtonEx)sender;
            if (btn.IsPageValid())
            {
                gvExpenditure.DataBind();
            }
        }
    }

     /// <summary>
     /// This class represents the item which is retrieved by the query
     /// </summary>

    public class ResultItem:SubTotalItem
    {
        public ResultItem(RoHeadHierarchy acc, bool isSubTotal) : base(acc, isSubTotal)
        {
            if (isSubTotal)
            {
                ProjectCost = 0;
            }
            else
            {
                if (acc.RevisedProjectCost == null)
                {
                    this.ProjectCost = acc.ProjectCost == null ? 0 : acc.ProjectCost.Value;
                }
                else
                {
                    this.ProjectCost = acc.RevisedProjectCost.Value;
                }
            }

            UptoPreviousYearsExpenses = 0;
            CurrentYearDuringMonthExpenses = 0;
            CurrentYearUptoMonthExpenses = 0;
            Cumulative = 0;
        }

        /// <summary>
        /// Display the Project Cost for each head of account id.
        /// </summary>
        
        public int HeadId
        {
            get;
            set; 
        }

       public decimal ProjectCost
        {
            get;
            set;
        }

        /// <summary>
        /// Expenditure during the Month.
        /// </summary>

        public decimal CurrentYearDuringMonthExpenses
        {
            get;
            set; 
        }

        /// <summary>
        /// Expenditure during the Financial year.
        /// </summary>

        public decimal CurrentYearUptoMonthExpenses
        {
            get; 
            set;
        }

        /// <summary>
        /// Expenditure Upto Previous financial year.
        /// </summary>

        public decimal UptoPreviousYearsExpenses
        {
            get;
            set;
        }

        /// <summary>
        /// Till date expenditure.
        /// </summary>

        public decimal Cumulative
        {
            get;
            set;
        }

        public override SubTotalItem CreateNew(RoHeadHierarchy head)
        {
            return new ResultItem(head, true);
        }

        /// <summary>
        /// Adds the amounts of the passed item to the current amounts
        /// </summary>
        /// <param name="item"></param>

        protected override void ExecuteUpdateSubtotals(SubTotalItem subtotalItem)
        {
            ResultItem item = (ResultItem)subtotalItem;
            this.ProjectCost += item.ProjectCost;
            this.UptoPreviousYearsExpenses += item.UptoPreviousYearsExpenses;
            this.CurrentYearDuringMonthExpenses += item.CurrentYearDuringMonthExpenses;
            this.CurrentYearUptoMonthExpenses += item.CurrentYearUptoMonthExpenses;
            this.Cumulative += item.Cumulative;
        }
    }
}
