                                              /*
 *  Programming: Eclipse Systems (P) Ltd., NOIDA, INDIA
 *  E-mail: support@eclsys.com
 *
 *  $Workfile:   BalanceProjectCost.aspx.cs  $
 *  $Revision: 38204 $
 *  $Author: ssingh $
 *  $Date: 2010-12-01 12:13:59 +0530 (Wed, 01 Dec 2010) $
 *  $Modtime:   Jul 10 2008 15:30:14  $
 *
 *  $Log:   S:/Projects/PHPA2/archives/Finance/Reports/BalanceProjectCost.aspx.cs-arc  $
 * 
 *    Rev 1.66   Jul 16 2008 14:52:34   pshishodia
 * WIP
 * 
 *    Rev 1.65   Jul 10 2008 15:46:06   pshishodia
 * WIP
 * 
 *    Rev 1.64   Jul 10 2008 11:45:56   pshishodia
 * WIP
 * 
 *    Rev 1.63   Jul 09 2008 17:40:54   vraturi
 * PVCS Template Added.
 */
using System;
using System.Linq;
using System.Web.UI.WebControls;
using Eclipse.PhpaLibrary.Database;
using Eclipse.PhpaLibrary.Reporting;
using System.Web.UI;
using Eclipse.PhpaLibrary.Web;
using System.Collections.Generic;
using EclipseLibrary.Web.UI;
using EclipseLibrary.Web.Extensions;

namespace Finance.Reports
{
    public partial class BalanceProjectCost : PageBase
    {
       // private const string HOA_EXPENDITURE = "EXPENDITURE";
      //  private const string HOA_TOUR_EXPENSES = "TOUR_EXPENSES";

        /// <summary>
        /// Display Balance Project Cost fields from Head of Account and Job table.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private IQueryable<ResultItem> m_query;

        protected void dsBalanceProjectCost_Selecting(object sender, System.Web.UI.WebControls.LinqDataSourceSelectEventArgs e)
        {
            ReportingDataContext db = (ReportingDataContext)this.dsBalanceProjectCost.Database;
            m_query = from job in db.RoJobs
                      //where job.RoHeadHierarchy.HeadOfAccountType == HOA_EXPENDITURE
                      //      || job.RoHeadHierarchy.HeadOfAccountType == HOA_TOUR_EXPENSES
                      where HeadOfAccountHelpers.AllExpenditures.Contains(job.RoHeadHierarchy.HeadOfAccountType)
                      group job by job.RoHeadHierarchy into grouping
                      orderby grouping.Key.SortableName
                      select new ResultItem(grouping.Key, false)
                      {
                          HeadId = grouping.Key.HeadOfAccountId,
                          SanctionAmount = grouping.Sum(
                          job => job.SanctionedAmount),
                          SanctionIssued = grouping.Sum(
                              job => job.RevisedSanction ?? 0),
                          ContractAmount = grouping.Sum(
                          job => job.ContractAmount ?? 0),
                          WorkAwarded = grouping.Sum(
                          job => job.RevisedContract ?? 0),
                          Commitment = grouping.Sum(
                          job => job.RevisedContract >0 ? job.RevisedContract ?? 0 : job.ContractAmount ?? 0)
                      };
            e.Result = this.QueryIterator();

        }

        private LevelSubtotals m_levelSubtotals;

        public IEnumerable<ResultItem> QueryIterator()
        {
            ReportingDataContext db = (ReportingDataContext)dsBalanceProjectCost.Database;
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
        /// Sum of the data for the corresponding Parent Head.
        /// </summary>

        decimal sumProjectCost;
        decimal sumSanctionIssued;
        decimal sumWorkAwarded;
        decimal sumCommitment;
        decimal sumBalanceProjectCost;
        

        protected void gvBalanceProjectCost_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            DateTime date = DateTime.Now.Date;
            DateTime dateFinancialYearStart = date.FinancialYearStartDate();
            switch (e.Row.RowType)
            {
                case DataControlRowType.Header:
                    gvBalanceProjectCost.Caption = string.Format("<b>Statement Showing Project Cost, Sanctions Issued/work awarded and Balance Project Cost available as on {0:dd MMMM yyyy}</b>", date);
                    break;

                case DataControlRowType.DataRow:
                    ResultItem item = (ResultItem)e.Row.DataItem;

                    if (item.IsSubTotal)
                    {
                        //Subtotal Row
                        e.Row.Font.Bold = true;
                        e.Row.Font.Italic = true;
                        e.Row.Cells[0].Text = string.Empty;
                    }
                    else
                    {
                        sumProjectCost += item.ProjectCost;
                        sumSanctionIssued += item.SanctionIssued;
                        sumWorkAwarded += item.WorkAwarded;
                        sumCommitment += item.Commitment;
                        sumBalanceProjectCost += item.BalanceProjectCost;
                    }                    
                    break;

                case DataControlRowType.Footer:
                    DataControlFieldCell cellPC = (from DataControlFieldCell c in e.Row.Cells
                                                   where c.ContainingField.AccessibleHeaderText == "ProjectCost"
                                                   select c).Single();
                    cellPC.Text = sumProjectCost.ToString("N2");

                    DataControlFieldCell cellSI = (from DataControlFieldCell c in e.Row.Cells
                                                   where c.ContainingField.AccessibleHeaderText == "SanctionIssued"
                                                   select c).Single();
                    cellSI.Text = sumSanctionIssued.ToString("N2");

                    DataControlFieldCell cellWA = (from DataControlFieldCell c in e.Row.Cells
                                                   where c.ContainingField.AccessibleHeaderText == "WorkAwarded"
                                                   select c).Single();
                    cellWA.Text = sumWorkAwarded.ToString("N2");

                    DataControlFieldCell cellC = (from DataControlFieldCell c in e.Row.Cells
                                                  where c.ContainingField.AccessibleHeaderText == "Commitment"
                                                  select c).Single();
                    cellC.Text = sumCommitment.ToString("N2");

                    DataControlFieldCell cellBPC = (from DataControlFieldCell c in e.Row.Cells
                                                    where c.ContainingField.AccessibleHeaderText == "BalanceProjectCost"
                                                    select c).Single();
                    cellBPC.Text = sumBalanceProjectCost.ToString("N2");
                    break;
            }
        }

        /// <summary>
        /// This class represents the item which is retrieved by the query
        /// </summary>

        public class ResultItem:SubTotalItem
        {
            public ResultItem(RoHeadHierarchy acc, bool isSubTotal): base(acc, isSubTotal)
            {
                if (acc.RevisedProjectCost == null)
                {
                    this.ProjectCost = acc.ProjectCost == null ? 0 : acc.ProjectCost.Value;
                }
                else
                {
                    this.ProjectCost = acc.RevisedProjectCost.Value;
                }
               
                //SanctionIssued = 0;
                //WorkAwarded = 0;
                //Commitment = 0;
            }

            /// <summary>
            /// Display the Project Cost for each head of account id.
            /// </summary>

            public decimal ProjectCost
            {
                get;
              
                set;
               
            }


            private decimal _sanctionAmount;

            public decimal SanctionAmount
            {
                get
                {
                    return _sanctionAmount;
                }
                set
                {
                    _sanctionAmount = value;
                }
            }

            private decimal _contractAmount;

            public decimal ContractAmount
            {
                get
                {
                    return _contractAmount;
                }

                set
                {
                    _contractAmount = value;
                }
            }

            /// <summary>
            /// Display the amount sanctioned for job respective to head of account.
            /// </summary>

            private decimal _sanctionIssued;

            public decimal SanctionIssued
            {
                get
                {
                    return _sanctionIssued;
                }

                set
                {
                    if (value != 0)
                        _sanctionIssued = value;
                    else
                        _sanctionIssued = _sanctionAmount;
                }
            }

            private decimal _workAwarded;

            public decimal WorkAwarded
            {
                get
                {
                    return _workAwarded;
                }
                set
                {
                    if (value != 0)
                        _workAwarded = value;
                    else
                        _workAwarded = _contractAmount;
                }
            }

            private decimal _commitment;

            public decimal Commitment
            {
                get
                {
                    return _commitment;
                }
                set
                {
                    if (value != 0)
                        _commitment = value;
                    else
                        _commitment = _sanctionIssued;
                }
            }

            /// <summary>
            /// Display the difference between Project Cost and Commitment.
            /// </summary>

            public decimal BalanceProjectCost
            {
                get
                {
                    return this.ProjectCost - _commitment;
                }
            }
                        
            public int HeadId { get; set; }

            public override SubTotalItem CreateNew(RoHeadHierarchy head)
            {
                return new ResultItem(head, true);
            }

            protected override void ExecuteUpdateSubtotals(SubTotalItem subtotalItem)
            {
                ResultItem item = (ResultItem)subtotalItem;
                this.ProjectCost += item.ProjectCost;
                this.SanctionIssued += item.SanctionIssued;
                this.WorkAwarded += item.WorkAwarded;
                this.Commitment += item.Commitment;
            }
        }
    }
}
