﻿/*
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
using Eclipse.PhpaLibrary.Reporting;
using Eclipse.PhpaLibrary.Web;
using EclipseLibrary.Web.UI;

namespace Finance.Reports
{
    public partial class ContractorPayment : PageBase
    {
        protected void dsSpecificJob_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            if (!btnGo.IsPageValid())
            {
                e.Cancel = true;
                return;
            }
            ReportingDataContext db = (ReportingDataContext)this.dsSpecificJob.Database;
            DataLoadOptions dlo = new DataLoadOptions();
            dlo.LoadWith<RoJob>(p => p.RoContractor);
            db.LoadOptions = dlo;
        }

        private IList<RoVoucher> m_query;
        protected override void OnLoad(EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                tbFromDate.Text = DateTime.Now.MonthStartDate().AddMonths(-1).ToShortDateString();
            }
            tbJob.Focus();

            if (!string.IsNullOrEmpty(tbJob.Value))
            {
                dsSpecificJob.WhereParameters["JobId"].DefaultValue = tbJob.Value;
            }
            base.OnLoad(e);
        }

        /// <summary>
        /// Class for query.
        /// </summary>
        class ContractorRecoverdPayment
        {
            public int VoucherId { get; set; }
            public string VoucherCode { get; set; }
            public DateTime VoucherDate { get; set; }
            public string Particulars { get; set; }
            public RoJob Job { get; set; }
            public decimal? AdmittedAmount { get; set; }
            public decimal? ContractorTax { get; set; }
            public decimal? AdvancePaid { get; set; }
            public decimal? SecurityDeposit { get; set; }
            public decimal? AdvanceAdjusted { get; set; }
            public decimal? MaterialRecoverd { get; set; }
            public decimal? InterestRecoverd { get; set; }
            public decimal? OtherRecovery { get; set; }
            public decimal? ContractorAdvanceAdjusted
            {
                get
                {
                    return AdvanceAdjusted + MaterialRecoverd;
                }
            }
            public Decimal? TotalRecovery
            {
                get
                {
                    return (ContractorTax ?? 0) + (SecurityDeposit ?? 0) + (MaterialRecoverd ?? 0) +
                            (InterestRecoverd ?? 0) + (AdvanceAdjusted ?? 0) + (OtherRecovery ?? 0);
                }
            }
            public Decimal? NetPayment
            {
                get
                {
                    return (AdmittedAmount ?? 0) + (AdvancePaid ?? 0) - (TotalRecovery ?? 0);
                }
            }
        }

        /// <summary>
        /// Execute the query for the report.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dsContractorPayment_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            if (!btnGo.IsPageValid())
            {
                e.Cancel = true;
                return;
            }
            int? jobId = Convert.ToInt32(tbJob.Value);

            DateTime fromDate = Convert.ToDateTime(tbFromDate.ValueAsDate);
            DateTime toDate = Convert.ToDateTime(tbToDate.ValueAsDate);

            // if only fromDate provided,then toDate will be consider as one month after from fromDate.  
            if (fromDate != DateTime.MinValue && toDate == DateTime.MinValue)
            {
                toDate = fromDate.AddMonths(1);
                tbToDate.Text = toDate.ToShortDateString();
            }
            ReportingDataContext db = (ReportingDataContext)this.dsContractorPayment.Database;
            // Seting load options is critical - otherwise no data is returned
            //DataLoadOptions dlo = new DataLoadOptions();
            //dlo.LoadWith<RoVoucher>(vd => vd.RoVoucherDetails);
            //dlo.LoadWith<RoVoucherDetail>(vd => vd.HeadOfAccount);
            //dlo.LoadWith<RoJob>(job => job.RoContractor);
            //db.LoadOptions = dlo;

            var query = from vd in db.RoVoucherDetails
                        where vd.JobId == jobId &&
                        !HeadOfAccountHelpers.ExciseDuties.Contains(vd.HeadOfAccount.HeadOfAccountType) &&
                        (vd.RoVoucher.VoucherDate >= fromDate &&
                        vd.RoVoucher.VoucherDate <= toDate) &&
                        vd.RoJob.ContractorId != null
                        group vd by vd.RoVoucher into grp
                        select new ContractorRecoverdPayment
                        {
                            Particulars = grp.Key.Particulars,
                            VoucherDate = grp.Key.VoucherDate,
                            VoucherCode = grp.Key.VoucherCode,
                            VoucherId = grp.Key.VoucherId,
                            AdmittedAmount = (from vd in grp
                                              where HeadOfAccountHelpers.JobExpenses.Contains(vd.HeadOfAccount.HeadOfAccountType)
                                              select vd.DebitAmount ?? 0 - vd.CreditAmount ?? 0).Sum(),
                            AdvancePaid = (from vd in grp
                                           where HeadOfAccountHelpers.JobAdvances.Contains(vd.HeadOfAccount.HeadOfAccountType)
                                           select vd.DebitAmount).Sum(),
                            ContractorTax = (from vd in grp
                                             where HeadOfAccountHelpers.TaxSubTypes.BhutanTax.Contains(vd.HeadOfAccount.HeadOfAccountType)
                                             select vd.CreditAmount ?? 0 - vd.DebitAmount ?? 0).Sum(),
                            SecurityDeposit = (from vd in grp
                                               where HeadOfAccountHelpers.SecurityDeposits.Contains(vd.HeadOfAccount.HeadOfAccountType)
                                               select vd.CreditAmount ?? 0 - vd.DebitAmount ?? 0).Sum(),
                            AdvanceAdjusted = (from vd in grp
                                               where HeadOfAccountHelpers.AdvanceSubTypes.PartyAdvance.Contains(vd.HeadOfAccount.HeadOfAccountType)
                                               select vd.CreditAmount).Sum(),
                            MaterialRecoverd = (from vd in grp
                                                where HeadOfAccountHelpers.AdvanceSubTypes.MaterialAdvance.Contains(vd.HeadOfAccount.HeadOfAccountType)
                                                select vd.CreditAmount).Sum(),

                            InterestRecoverd = (from vd in grp
                                                where HeadOfAccountHelpers.InterestReceipts.Contains(vd.HeadOfAccount.HeadOfAccountType)
                                                select vd.CreditAmount).Sum(),
                            OtherRecovery = (from vd in grp
                                             where !HeadOfAccountHelpers.SecurityDeposits
                                             .Concat(HeadOfAccountHelpers.TaxSubTypes.BhutanTax)
                                             .Concat(HeadOfAccountHelpers.JobAdvances)
                                             .Concat(HeadOfAccountHelpers.InterestReceipts)
                                             .Concat(HeadOfAccountHelpers.JobExpenses)
                                             .Concat(HeadOfAccountHelpers.CashInBank)
                                             .Contains(vd.HeadOfAccount.HeadOfAccountType)
                                             select vd.CreditAmount ?? 0 - vd.DebitAmount ?? 0).Sum(),
                        };


            e.Result = query;
        }


        protected void gvContractorPayment_DataBound(object sender, EventArgs e)
        {
            //if (gvContractorPayment.Rows.Count > 0)
            //{
            //    ReportingDataContext db = (ReportingDataContext)this.dsContractorPayment.Database;
            //    m_query = from vd in db.RoVoucherDetails
            //              where vd.JobId == Convert.ToInt32(tbJob.Value) &&
            //              vd.HeadOfAccount.HeadOfAccountType != "EDGOI" &&
            //              vd.HeadOfAccount.HeadOfAccountType != "EDRGOB" &&
            //              vd.RoVoucher.VoucherDate <= tbFromDate.ValueAsDate
            //              group vd by vd.RoVoucher into grp
            //              select grp.Key;

            //    lblOpeningBalance.Text = string.Format("{0:N2}", this.QueryIterator().Sum(p => p.NetPayment));
            //}
        }

        /// <summary>
        /// Calaculate footer Text.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvContractorPayment_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            switch (e.Row.RowType)
            {
                case DataControlRowType.Header:
                    gvContractorPayment.Caption = string.Format("<span style='font-size:medium; font-weight:bold'>Payment to Contractor from {0:dd MMMM yyyy} {1}</span>", tbFromDate.ValueAsDate, tbToDate.ValueAsDate.HasValue ? " to " + tbToDate.ValueAsDate.Value.ToString("dd MMMM yyyy") : null);
                    lblOpenBal.Visible = true;
                    break;

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

                    decimal balance = Math.Abs((decimal)(adv - adj));
                    lbldiffrence.Text += balance.ToString("C");
                    lbldiffrence.Visible = true;
                    break;
            }
        }

        /// <summary>
        /// Sharad 17 Aug 2010: Admitted Amount = Debit - Credit. Earlier it was Debit only. This done at Lobeysa
        /// based on Rajesh Pant's feedback.
        /// </summary>
        /// <returns></returns>
        private IEnumerable<ContractorRecoverdPayment> QueryIterator()
        {
            foreach (RoVoucher v in m_query)
            {
                ContractorRecoverdPayment crp = new ContractorRecoverdPayment();
                //crp.Particulars = v.Particulars;
                //crp.VoucherDate = v.VoucherDate;
                //crp.VoucherCode = v.VoucherCode;
                //crp.VoucherId = v.VoucherId;
                //crp.AdmittedAmount = (decimal?)v.RoVoucherDetails
                //    .Where(p => (p.HeadOfAccount.HeadOfAccountType == "EXPENDITURE" ||
                //                        p.HeadOfAccount.HeadOfAccountType == "TOUR_EXPENSES")
                //                        && p.JobId == Convert.ToInt32(tbJob.Value))
                //    .Sum(p => p.DebitAmount ?? 0 - p.CreditAmount ?? 0);
                //crp.AdvancePaid = (decimal?)v.RoVoucherDetails.Sum(p => (((p.HeadOfAccount.HeadOfAccountType == "PARTY_ADVANCE" ||
                //                        p.HeadOfAccount.HeadOfAccountType == "MATERIAL_ADVANCE") && p.JobId == Convert.ToInt32(tbJob.Value))
                //                        ? p.DebitAmount ?? 0 : 0));

                //crp.ContractorTax = (decimal?)v.RoVoucherDetails.Sum(p => ((p.HeadOfAccount.HeadOfAccountType == "BIT" && p.JobId == Convert.ToInt32(tbJob.Value))
                //                        ? (p.CreditAmount ?? 0 - p.DebitAmount ?? 0) : 0));
                //crp.SecurityDeposit = (decimal?)v.RoVoucherDetails.Sum(p => ((p.HeadOfAccount.HeadOfAccountType == "SD"
                //                            && p.JobId == Convert.ToInt32(tbJob.Value)) ? (p.CreditAmount ?? 0 - p.DebitAmount ?? 0) : 0));
                //crp.AdvanceAdjusted = (decimal?)v.RoVoucherDetails.Sum(p => ((p.HeadOfAccount.HeadOfAccountType == "PARTY_ADVANCE"
                //                            && p.JobId == Convert.ToInt32(tbJob.Value))
                //                            ? p.CreditAmount ?? 0 : 0));
                //crp.MaterialRecoverd = (decimal?)v.RoVoucherDetails.Sum(p => ((p.HeadOfAccount.HeadOfAccountType == "MATERIAL_ADVANCE"
                //                            && p.JobId == Convert.ToInt32(tbJob.Value))
                //                            ? p.CreditAmount ?? 0 : 0));
                //crp.InterestRecoverd = (decimal?)v.RoVoucherDetails.Sum(p => ((p.HeadOfAccount.HeadOfAccountType == "INTEREST"
                //                            && p.JobId == Convert.ToInt32(tbJob.Value))
                //                            ? p.CreditAmount ?? 0 : 0));
                //crp.OtherRecovery = (decimal?)v.RoVoucherDetails.Sum(p => (p.HeadOfAccount.HeadOfAccountType != "BIT" &&
                //                            p.HeadOfAccount.HeadOfAccountType != "SD" &&
                //                            p.HeadOfAccount.HeadOfAccountType != "PARTY_ADVANCE" &&
                //                            p.HeadOfAccount.HeadOfAccountType != "MATERIAL_ADVANCE" &&
                //                            p.HeadOfAccount.HeadOfAccountType != "INTEREST" &&
                //                            p.HeadOfAccount.HeadOfAccountType != "BANKNU" &&
                //                            p.HeadOfAccount.HeadOfAccountType != "BANKFE" &&
                //                            p.HeadOfAccount.HeadOfAccountType != "EXPENDITURE" &&
                //                            p.HeadOfAccount.HeadOfAccountType != "TOUR_EXPENSES" &&
                //                            p.JobId == Convert.ToInt32(tbJob.Value))
                //                            ?
                //                            (p.CreditAmount ?? 0 - p.DebitAmount ?? 0)
                //                            :
                //                            0);
                yield return crp;
            }
        }
    }
}
