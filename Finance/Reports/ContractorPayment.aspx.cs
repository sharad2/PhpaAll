using Eclipse.PhpaLibrary.Reporting;
using Eclipse.PhpaLibrary.Web;
using EclipseLibrary.Web.JQuery;
using EclipseLibrary.Web.UI;
/*
 *  Programming: Eclipse Systems (P) Ltd., NOIDA, INDIA
 *  E-mail: support@eclsys.com
 *
 *  $Workfile:   ContractorPayment.aspx.cs  $
 *  $Revision: 38341 $
 *  $Author: ssingh $
 *  $Date: 2010-12-02 14:40:44 +0530 (Thu, 02 Dec 2010) $
 *  $Modtime:   Jul 24 2008 09:17:14  $
 *
 *  $Log:   S:/Projects/PHPA2/archives/Finance/Reports/ContractorPayment.aspx.cs-arc  $
 */
using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PhpaAll.Reports
{
    public partial class ContractorPayment : PageBase
    {


        protected class GrandTotalData
        {
            public decimal? AdmittedAmount { get; set; }

            public decimal? AdvancePaid { get; set; }

            public decimal? ContractorAdvanceAdjusted { get; set; }

            public decimal? ContractorTax { get; set; }

            public decimal? SecurityDeposit { get; set; }

            public decimal? InterestRecoverd { get; set; }

            public decimal? OtherRecovery { get; set; }

            public decimal? TotalRecovery { get; set; }

            public decimal NetPayment { get; set; }
        }

        protected GrandTotalData GrandTotals { get; set; }

        //private IList<RoVoucher> m_query;
        protected override void OnLoad(EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                tbFromDate.Text = DateTime.Now.MonthStartDate().AddMonths(-1).ToShortDateString();
            }
            tbJob.Focus();

            //if (!string.IsNullOrEmpty(tbJob.Value))
            //{
            //    dsSpecificJob.WhereParameters["JobId"].DefaultValue = tbJob.Value;
            //}
            base.OnLoad(e);
        }

        private class JobComparer : IEqualityComparer<RoJob>
        {

            public bool Equals(RoJob x, RoJob y)
            {
                return x.JobId.Equals(y.JobId);
            }

            public int GetHashCode(RoJob obj)
            {
                return obj.JobId.GetHashCode();
            }
        }

        protected IDictionary<int, decimal?> _dictOpeningBalancePerJob;

        protected decimal GetOpeningBalance(int jobId)
        {
            decimal? val;
            if (_dictOpeningBalancePerJob != null && _dictOpeningBalancePerJob.TryGetValue(jobId, out val))
            {
                return val ?? 0;
            }
            return 0;
        }

        /// <summary>
        /// Execute the query for the report.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dsContractorPayment_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            if (!btnGo.IsPageValid() || (string.IsNullOrWhiteSpace(tbJob.Value) && string.IsNullOrWhiteSpace(tbContractors.Value)))
            {
                e.Cancel = true;
                return;
            }
            int? jobId = null;
            if (!string.IsNullOrWhiteSpace(tbJob.Value))
            {
                jobId = Convert.ToInt32(tbJob.Value);
            }

            int? contractorId = null;
            if (!string.IsNullOrWhiteSpace(tbContractors.Value))
            {
                contractorId = Convert.ToInt32(tbContractors.Value);
            }

            DateTime fromDate = Convert.ToDateTime(tbFromDate.ValueAsDate);
            DateTime toDate = Convert.ToDateTime(tbToDate.ValueAsDate);

            // if only fromDate provided,then toDate will be consider as one month after from fromDate.  
            if (fromDate != DateTime.MinValue && toDate == DateTime.MinValue)
            {
                toDate = fromDate.AddMonths(1);
                tbToDate.Text = toDate.ToShortDateString();
            }
            ReportingDataContext db = (ReportingDataContext)this.dsContractorPayment.Database;

            var dlo = new DataLoadOptions();
            dlo.LoadWith<RoJob>(p => p.RoContractor);
            db.LoadOptions = dlo;

            var otherRecoveryHeadExclusion = new[] { HeadOfAccountHelpers.TaxSubTypes.BhutanIncomeTax, HeadOfAccountHelpers.ReceiptSubType.InterestReceipt,
                HeadOfAccountHelpers.DepositSubTypes.SecurityDeposit}
                                                       .Concat(HeadOfAccountHelpers.JobAdvances)
                                                       .Concat(HeadOfAccountHelpers.AllExpenditures)
                                                       .Concat(HeadOfAccountHelpers.AllBanks);


            var query = from vd in db.RoVoucherDetails
                        where (jobId == null || vd.JobId == jobId) &&
                        !HeadOfAccountHelpers.AllExciseDuties.Contains(vd.HeadOfAccount.HeadOfAccountType) &&
                        vd.RoJob.ContractorId != null && (contractorId == null || vd.ContractorId == contractorId)
                        group vd by new
                        {
                            vd.RoJob,
                            vd.RoVoucher
                        } into grp
                        let contractorTax = (decimal?)(from vd in grp
                                                       where vd.HeadOfAccount.HeadOfAccountType == HeadOfAccountHelpers.TaxSubTypes.BhutanIncomeTax
                                                       select vd.CreditAmount ?? 0 - vd.DebitAmount ?? 0).Sum()
                        let securityDeposit = (decimal?)(from vd in grp
                                                         where vd.HeadOfAccount.HeadOfAccountType == HeadOfAccountHelpers.DepositSubTypes.SecurityDeposit
                                                         select vd.CreditAmount ?? 0 - vd.DebitAmount ?? 0).Sum()
                        let materialRecovered = (decimal?)(from vd in grp
                                                           where vd.HeadOfAccount.HeadOfAccountType == HeadOfAccountHelpers.AdvanceSubTypes.MaterialAdvance
                                                           select vd.CreditAmount).Sum()
                        let interestRecovered = (decimal?)(from vd in grp
                                                           where vd.HeadOfAccount.HeadOfAccountType == HeadOfAccountHelpers.ReceiptSubType.InterestReceipt
                                                           select vd.CreditAmount).Sum()
                        let advanceAdjusted = (decimal?)(from vd in grp
                                                         where HeadOfAccountHelpers.PartyAdvances.Contains(vd.HeadOfAccount.HeadOfAccountType)
                                                         select vd.CreditAmount).Sum()
                        let otherRecoveryDebit = (from vd in grp
                                                  where !otherRecoveryHeadExclusion.Contains(vd.HeadOfAccount.HeadOfAccountType)
                                                  select vd.DebitAmount).Sum()
                        let otherRecoveryCredit = (from vd in grp
                                                   where
                                                   (!otherRecoveryHeadExclusion.Contains(vd.HeadOfAccount.HeadOfAccountType) ||
                                                   (HeadOfAccountHelpers.AllExpenditures.Contains(vd.HeadOfAccount.HeadOfAccountType) &&
                                                   (vd.HeadOfAccount.HeadOfAccountType != HeadOfAccountHelpers.ExpenditureSubTypes.MainCivilExpenditure && vd.RoJob.TypeFlag == "S")))
                                                   select vd.CreditAmount).Sum()
                        let otherRecovery = otherRecoveryDebit.HasValue || otherRecoveryCredit.HasValue ? (otherRecoveryCredit ?? 0) - (otherRecoveryDebit ?? 0) : (decimal?)null
                        let anyRecovery = materialRecovered.HasValue || advanceAdjusted.HasValue || contractorTax.HasValue || securityDeposit.HasValue ||
                                            interestRecovered.HasValue || otherRecovery.HasValue
                        let totalRecovery1 = (materialRecovered ?? 0) + (advanceAdjusted ?? 0) + (contractorTax ?? 0) + (securityDeposit ?? 0) + (interestRecovered ?? 0)
                                 + (otherRecovery ?? 0)
                        let totalRecovery = anyRecovery ? totalRecovery1 : (decimal?)null

                        let admittedAmountDebit = (from vd in grp
                                                   where HeadOfAccountHelpers.AllExpenditures.Contains(vd.HeadOfAccount.HeadOfAccountType)
                                                   select vd.DebitAmount).Sum()
                        let admittedAmountCredit = (from vd in grp
                                                    where ((vd.HeadOfAccount.HeadOfAccountType == HeadOfAccountHelpers.ExpenditureSubTypes.MainCivilExpenditure && vd.RoJob.TypeFlag == "S") ||
                                                    (HeadOfAccountHelpers.AllExpenditures.Contains(vd.HeadOfAccount.HeadOfAccountType) && vd.RoJob.TypeFlag != "S"))
                                                    select vd.CreditAmount).Sum()
                        let admittedAmount = admittedAmountDebit.HasValue || admittedAmountCredit.HasValue ? (admittedAmountDebit ?? 0) - (admittedAmountCredit ?? 0) : (decimal?)null
                        let advancePaid = (decimal?)(from vd in grp
                                                     where HeadOfAccountHelpers.JobAdvances.Contains(vd.HeadOfAccount.HeadOfAccountType)
                                                     select vd.DebitAmount).Sum()
                        select new 
                        {
                            Job = grp.Key.RoJob,
                            Particulars = grp.Key.RoVoucher.Particulars,
                            VoucherDate = grp.Key.RoVoucher.VoucherDate,
                            VoucherCode = grp.Key.RoVoucher.VoucherCode,
                            VoucherId = grp.Key.RoVoucher.VoucherId,
                            AdmittedAmount = admittedAmount,  // Col 3
                            AdvancePaid = (from vd in grp
                                           where HeadOfAccountHelpers.JobAdvances.Contains(vd.HeadOfAccount.HeadOfAccountType)
                                           select vd.DebitAmount).Sum(),  // Col 5
                            ContractorAdvanceAdjusted = advanceAdjusted.HasValue || materialRecovered.HasValue ?
                                (advanceAdjusted ?? 0) + (materialRecovered ?? 0) : (decimal?)null,  // Col 6
                            ContractorTax = contractorTax,  // Col 7
                            SecurityDeposit = securityDeposit,  // Col 8
                            InterestRecoverd = interestRecovered,  // Col 9
                            OtherRecovery = otherRecovery,  // Col 10
                            TotalRecovery = totalRecovery,  // Col 11
                            NetPayment = (admittedAmount ?? 0) + (advancePaid ?? 0) - (totalRecovery ?? 0) // Col 12
                        };

            var results = query.Where(p => p.VoucherDate >= fromDate && p.VoucherDate <= toDate).ToLookup(p => p.Job, new JobComparer());

            var grandTotals = new GrandTotalData();
            foreach (var item in results.SelectMany(p => p))
            {
                grandTotals.AdmittedAmount = (new[] { grandTotals.AdmittedAmount, item.AdmittedAmount }).Sum();
                grandTotals.AdvancePaid = (new[] { grandTotals.AdvancePaid, item.AdvancePaid }).Sum();
                grandTotals.ContractorAdvanceAdjusted = (new[] { grandTotals.ContractorAdvanceAdjusted, item.ContractorAdvanceAdjusted }).Sum();
                grandTotals.ContractorTax = (new[] { grandTotals.ContractorTax, item.ContractorTax }).Sum();
                grandTotals.SecurityDeposit = (new[] { grandTotals.SecurityDeposit, item.SecurityDeposit }).Sum();
                grandTotals.InterestRecoverd = (new[] { grandTotals.InterestRecoverd, item.InterestRecoverd }).Sum();
                grandTotals.OtherRecovery = (new[] { grandTotals.OtherRecovery, item.OtherRecovery }).Sum();
                grandTotals.TotalRecovery = (new[] { grandTotals.TotalRecovery, item.TotalRecovery }).Sum();
                grandTotals.NetPayment = (new[] { grandTotals.NetPayment, item.NetPayment }).Sum();
            }
            GrandTotals = grandTotals;

            e.Result = results;

            //lblOpeningBalance.Text = string.Format("Opening Balance: {0:N2}", query.Where(p => p.VoucherDate < fromDate)
            //    .Sum(p => (decimal?)p.NetPayment));

            _dictOpeningBalancePerJob = (from item in query
                         where item.VoucherDate < fromDate
                         group item by item.Job.JobId into g
                         select new
                         {
                             JobId = g.Key,
                             OpeningBalance = (decimal?)g.Sum(p => p.NetPayment)
                         }).ToDictionary(p => p.JobId, p => p.OpeningBalance);
        }

        public decimal Balance { get; set; }


        /// <summary>
        /// Calaculate footer Text.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvContractorPayment_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            var gvContractorPayment = (GridViewEx)sender;
            switch (e.Row.RowType)
            {
                //case DataControlRowType.Header:
                //    gvContractorPayment.Caption = string.Format("<span style='font-size:medium; font-weight:bold'>Payment to Contractor from {0:dd MMMM yyyy} {1}</span>", tbFromDate.ValueAsDate, tbToDate.ValueAsDate.HasValue ? " to " + tbToDate.ValueAsDate.Value.ToString("dd MMMM yyyy") : null);
                //    //lblOpenBal.Visible = true;
                //    break;

                case DataControlRowType.Footer:
                    // Footer prints on last page only
                    e.Row.TableSection = TableRowSection.TableBody;
                    MultiBoundField advancePaid = (MultiBoundField)(from DataControlField col in gvContractorPayment.Columns
                                                                    where col.AccessibleHeaderText == "AdvancePaid"
                                                                    select col).Single();
                    MultiBoundField advanceAdjusted = (MultiBoundField)(from DataControlField col in gvContractorPayment.Columns
                                                                        where col.AccessibleHeaderText == "ContractorAdvanceAdjusted"
                                                                        select col).Single();
                    decimal? adv = Convert.ToDecimal(advancePaid.SummaryValues[0].Value);
                    decimal? adj = Convert.ToDecimal(advanceAdjusted.SummaryValues[0].Value);

                    this.Balance = (decimal)(adv - adj);
                    //var lbldiffrence = (Label)gvContractorPayment.NamingContainer.FindControl("lbldiffrence");
                    //lbldiffrence.Text += balance.ToString("C");
                    //lbldiffrence.Visible = true;
                    break;
            }
        }

        //protected string GetOpeningBalance(int jobId)
        //{
        //    return jobId.ToString();
        //}

    }
}
