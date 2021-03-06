﻿/*
 *  Programming: Eclipse Systems (P) Ltd., NOIDA, INDIA
 *  E-mail: support@eclsys.com
 *
 *  $Workfile:   FundPositionReport.aspx.cs  $
 *  $Revision: 727 $
 *  $Author: ssinghal $
 *  $Date: 2014-11-28 11:07:19 +0530 (Fri, 28 Nov 2014) $
 *  $Modtime:   Jul 21 2008 15:02:00  $
 *
 *  $Log:   S:/Projects/PHPA2/archives/Finance/Reports/ReceiptandPayment.aspx.cs-arc  $
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Eclipse.PhpaLibrary.Reporting;
using Eclipse.PhpaLibrary.Web;
using System.Collections;

namespace PhpaAll.Reports
{
    public class FundPositionReportData
    {
        public DateTime DateTo { get; set; }

        public DateTime DateFrom { get; set; }

        public DateTime PreviousYearEndDate { get; set; }

        /// <summary>
        /// I - a
        /// </summary>
        public decimal? FundsReceivedGOIGrantNuUpToPrev { get; set; }

        public decimal? FundsReceivedGOIGrantNuCurr { get; set; }

        public decimal? FundsReceivedGOIGrantNuCum { get; set; }

        public string FundsReceivedGOIGrantNuHeads { get; set; }

        /// <summary>
        /// I - b
        /// </summary>
        public decimal? FundsReceivedGOIGrantFeUpToPrev { get; set; }

        public decimal? FundsReceivedGOIGrantFeCurr { get; set; }

        public decimal? FundsReceivedGOIGrantFeCum { get; set; }

        public string FundsReceivedGOIGrantFeHeads { get; set; }

        /// <summary>
        /// I - c
        /// </summary>
        public decimal? FundsReceivedGOILoanNuUpToPrev { get; set; }

        public decimal? FundsReceivedGOILoanNuCurr { get; set; }

        public decimal? FundsReceivedGOILoanNuCum { get; set; }

        public string FundsReceivedGOILoanNuHeads { get; set; }


        /// <summary>
        /// I - d
        /// </summary>
        public decimal? FundsReceivedGOILoanFeUpToPrev { get; set; }

        public decimal? FundsReceivedGOILoanFeCurr { get; set; }

        public decimal? FundsReceivedGOILoanFeCum { get; set; }

        public string FundsReceivedGOILoanFeHeads { get; set; }

        /// <summary>
        /// II Total of All Funds Received from GOI
        /// </summary>
        public decimal? FundReceivedGOITotalUpToPrev { get; set; }

        public decimal? FundReceivedGOITotalCurr { get; set; }

        public decimal? FundReceivedGOITotalCum { get; set; }

        /// <summary>
        /// III Othere receipts
        /// </summary>
        public decimal? FundReceivedOtherUpToPrev { get; set; }

        public decimal? FundReceivedOtherCurr { get; set; }

        public decimal? FundReceivedOtherCum { get; set; }

        public string FundReceivedOtherHeads { get; set; }

        /// <summary>
        /// IV
        /// </summary>
        public decimal? FundReceivedOtherAndGOITotalUpToPrev { get; set; }

        public decimal? FundReceivedOtherAndGOITotalCurr { get; set; }

        public decimal? FundReceivedOtherAndGOITotalCum { get; set; }


        /// <summary>
        /// V
        /// </summary>
        public decimal? ExpendituresUpToPrev { get; set; }

        public decimal? ExpendituresCurr { get; set; }

        public decimal? ExpendituresCum { get; set; }

        //public string ExpendituresHeads { get; set; }


        /// <summary>
        /// VI
        /// </summary>
        public decimal? BalanceFundUpToPrev { get; set; }

        //public decimal? BalanceFundCurr { get; set; }

        public decimal? BalanceFundCum { get; set; }



        /// <summary>
        /// VII Detail of Expenditure
        /// VII - a
        /// </summary>
        public decimal? EstablishmentExpendituresUpToPrev { get; set; }

        public decimal? EstablishmentExpendituresCurr { get; set; }

        public decimal? EstablishmentExpendituresCum { get; set; }

        public string EstablishmentExpendituresHead { get; set; }

        /// <summary>
        /// VII - a1
        /// </summary>
        public decimal? WapcosExpendituresUpToPrev { get; set; }

        public decimal? WapcosExpendituresCurr { get; set; }

        public decimal? WapcosExpendituresCum { get; set; }

        public string WapcosExpendituresHead { get; set; }


        /// <summary>
        /// VII - b
        /// </summary>
        public decimal? CivilWorkExpendituresUpToPrev { get; set; }

        public decimal? CivilWorkExpendituresCurr { get; set; }

        public decimal? CivilWorkExpendituresCum { get; set; }

        public string CivilWorkExpendituresHead { get; set; }


        /// <summary>
        /// VII - c
        /// </summary>
        public decimal? ElectricalExpendituresUpToPrev { get; set; }

        public decimal? ElectricalExpendituresCurr { get; set; }

        public decimal? ElectricalExpendituresCum { get; set; }

        public string ElectricalExpendituresHead { get; set; }


        /// <summary>
        /// VII - d
        /// </summary>
        public decimal? TransmissionExpendituresUpToPrev { get; set; }

        public decimal? TransmissionExpendituresCurr { get; set; }

        public decimal? TransmissionExpendituresCum { get; set; }

        public string TransmissionExpendituresHead { get; set; }

        public IEnumerable AllBanks { get; set; }

        /// <summary>
        /// Expenditure for Current Month
        /// </summary>
        public decimal? CurrentMonthExpenditure { get; set; }


        /// <summary>
        /// Balance fund
        /// </summary>

        public decimal? BalanceFund { get; set; }

        /// <summary>
        /// Outstanding bills amount
        /// </summary>
        public decimal? OutstandingBillsAmount { get; set; }
    }

    public partial class FundPositionReport : PageBase
    {
        ////private ReportingDataContext m_db;
        //protected DateTime m_dtPreviousYear;
        //protected DateTime m_dtPreviousYearEnd;
        //protected DateTime m_dtMonthStart;
        //protected DateTime m_dtPassed;
        ////decimal _otherBankAmount = 0;
        //protected string valdate = string.Empty;
        ////private IQueryable<ResultItem> m_query;
        //string otherBankAmount = string.Empty;

        protected override void OnLoad(EventArgs e)
        {
            //DateTime? date;
            if (!Page.IsPostBack)
            {
                dttbreceiptpayment.Text = DateTime.Today.ToShortDateString();

            }
            if (btnGo.IsPageValid())
            {
                var date = dttbreceiptpayment.ValueAsDate;
                //CalculateReceiptandPayment(date.Value);
                this.Title = string.Format("Fund Position as on - {0:dd/MM/yyyy}", date);
                this.DataBind();
            }

            base.OnLoad(e);
        }

        #region Old Code


        //const string MONEY_FORMAT_SPECIFIER = "#,###,,.000;(#,###,,.000);'&nbsp;'";
        //private enum SumType
        //{
        //    PaymentsPreviousYear,
        //    PaymentsForMonth,
        //    PaymentsUptoMonth,
        //    ReceiptsPreviousYear,
        //    ReceiptsForMonth,
        //    ReceiptsUptoMonth,
        //    ReceiptsSum,
        //    PaymentsSum,
        //    test,
        //    FundReceivedPreviousYear,
        //    FundReceivedUptoMonth,
        //    FundSum,
        //    BalanceFund
        //}

//        /// <summary>
//        /// Receipts and payments Calculation of various heads for whole time duration
//        /// </summary>
//        /// <param name="dt"></param>
//        private void CalculateReceiptandPayment(DateTime dt)
//        {
//            // Input date 10 Jun 2008
//            m_dtPreviousYear = dt.FinancialYearStartDate();         // {0}  1 Apr 2008
//            m_dtPreviousYearEnd = dt.FinancialYearStartDate().AddDays(-1); //{3} 31 March 2008
//            m_dtMonthStart = dt.MonthStartDate();                   // {1}  1 Jun 2008
//            m_dtPassed = dt;

//            var db = (ReportingDataContext)dsQueries.Database;

//            //Getting sum of amount ahainst headofaccount type
//            var query = (from vd in db.RoVoucherDetails
//                         where vd.HeadOfAccount.HeadOfAccountId != null
//                                && vd.HeadOfAccount.RoAccountType != null
//                                && !HeadOfAccountHelpers.FundTransit.Concat(HeadOfAccountHelpers.AllBanks)
//                                 .Concat(new[] { HeadOfAccountHelpers.ExciseDutySubTypes.ExciseDutyRGOB, HeadOfAccountHelpers.CashSubTypes.CashInHand })
//                                 .Contains(vd.HeadOfAccount.RoAccountType.HeadOfAccountType)
//                         group vd by vd.HeadOfAccount.RoAccountType into g
//                         select new
//                         {
//                             RoAccountType = g.Key,
//                             PreviousYearSum = g.Sum(hoa => hoa.RoVoucher.VoucherDate < m_dtPreviousYear ? (hoa.CreditAmount ?? 0 - hoa.DebitAmount ?? 0) : 0),
//                             ForMonthSum = g.Sum(hoa => hoa.RoVoucher.VoucherDate >= m_dtMonthStart && hoa.RoVoucher.VoucherDate <= m_dtPassed ? hoa.CreditAmount ?? 0 - hoa.DebitAmount ?? 0 : 0),
//                             UptoMonthSum = g.Sum(hoa => hoa.RoVoucher.VoucherDate >= m_dtPreviousYear && hoa.RoVoucher.VoucherDate < m_dtMonthStart ? hoa.CreditAmount ?? 0 - hoa.DebitAmount ?? 0 : 0)
//                         });

//            //foreach (var grp in query)
//            //{
//            //    // I - a: Grant received from the Govt. of India in Rs./Nu.
//            //    if (grp.RoAccountType.HeadOfAccountType == HeadOfAccountHelpers.GrantSubType.GrantNu)
//            //    {
//            //        SetAdditiveHyperLinkProperties(hplnkGOIAidPreviousYear, grp.PreviousYearSum, SumType.ReceiptsPreviousYear);
//            //        SetAdditivePropertiesFund(grp.PreviousYearSum, SumType.FundReceivedPreviousYear);
//            //        SetAdditiveHyperLinkProperties(hplnkGOIAidUptoMonth, (grp.UptoMonthSum + grp.ForMonthSum), SumType.ReceiptsUptoMonth);
//            //        SetAdditivePropertiesFund((grp.UptoMonthSum + grp.ForMonthSum), SumType.FundReceivedUptoMonth);
//            //        SetAdditiveLabelProperties(lblGOIAidsum, grp.PreviousYearSum + grp.ForMonthSum + grp.UptoMonthSum, SumType.ReceiptsSum);
//            //        SetAdditivePropertiesFund(grp.PreviousYearSum + grp.ForMonthSum + grp.UptoMonthSum, SumType.FundSum);
//            //    }
//            //    // I - b: Grant received from the Govt. of India in foreign exchange.
//            //    else if (grp.RoAccountType.HeadOfAccountType == HeadOfAccountHelpers.GrantSubType.GrantFe)
//            //    {
//            //        SetAdditiveHyperLinkProperties(hplnkGOIAidFEPreviousYear, grp.PreviousYearSum, SumType.ReceiptsPreviousYear);
//            //        SetAdditivePropertiesFund(grp.PreviousYearSum, SumType.FundReceivedPreviousYear);
//            //        SetAdditiveHyperLinkProperties(hplnkGOIAidFEUptoMonth, (grp.UptoMonthSum + grp.ForMonthSum), SumType.ReceiptsUptoMonth);
//            //        SetAdditivePropertiesFund((grp.UptoMonthSum + grp.ForMonthSum), SumType.FundReceivedUptoMonth);
//            //        SetAdditiveLabelProperties(lblGOIAidFEsum, (grp.PreviousYearSum + grp.UptoMonthSum + grp.ForMonthSum), SumType.ReceiptsSum);
//            //        SetAdditivePropertiesFund(grp.PreviousYearSum + grp.ForMonthSum + grp.UptoMonthSum, SumType.FundSum);
//            //    }
//            //    // I - c: Loan received from the Govt. of India in Rs./Nu.
//            //    else if (grp.RoAccountType.HeadOfAccountType == HeadOfAccountHelpers.LoanSubType.LoanNu)
//            //    {
//            //        SetAdditiveHyperLinkProperties(hplnkGOILoanPreviousYear, grp.PreviousYearSum, SumType.ReceiptsPreviousYear);
//            //        SetAdditivePropertiesFund(grp.PreviousYearSum, SumType.FundReceivedPreviousYear);
//            //        SetAdditiveHyperLinkProperties(hplnkGOILoanUptoMonth, (grp.UptoMonthSum + grp.ForMonthSum), SumType.ReceiptsUptoMonth);
//            //        SetAdditivePropertiesFund((grp.UptoMonthSum + grp.ForMonthSum), SumType.FundReceivedUptoMonth);
//            //        SetAdditiveLabelProperties(lblGOILoansum, grp.PreviousYearSum + grp.ForMonthSum + grp.UptoMonthSum, SumType.ReceiptsSum);
//            //        SetAdditivePropertiesFund(grp.PreviousYearSum + grp.ForMonthSum + grp.UptoMonthSum, SumType.FundSum);
//            //    }
//            //    // I - d: Loan received from the Govt. of India in foreign exchange.
//            //    else if (grp.RoAccountType.HeadOfAccountType == HeadOfAccountHelpers.LoanSubType.LoanFe)
//            //    {
//            //        SetAdditiveHyperLinkProperties(hplnkGOILoanFEPreviousYear, grp.PreviousYearSum, SumType.ReceiptsPreviousYear);
//            //        SetAdditivePropertiesFund(grp.PreviousYearSum, SumType.FundReceivedPreviousYear);
//            //        SetAdditiveHyperLinkProperties(hplnkGOILoanFEUptoMonth, (grp.UptoMonthSum + grp.ForMonthSum), SumType.ReceiptsUptoMonth);
//            //        SetAdditivePropertiesFund((grp.UptoMonthSum + grp.ForMonthSum), SumType.FundReceivedUptoMonth);
//            //        SetAdditiveLabelProperties(lblGOILoanFEsum, (grp.PreviousYearSum + grp.UptoMonthSum + grp.ForMonthSum), SumType.ReceiptsSum);
//            //        SetAdditivePropertiesFund(grp.PreviousYearSum + grp.ForMonthSum + grp.UptoMonthSum, SumType.FundSum);
//            //    }
//            //    // III Funds received from Other Receipts.
//            //    else if (HeadOfAccountHelpers.OtherFundReceipts.Contains(grp.RoAccountType.HeadOfAccountType))
//            //    {
//            //        SetAdditiveHyperLinkProperties(hplnkReceiptsPreviousYear, grp.PreviousYearSum, SumType.ReceiptsPreviousYear);
//            //        SetAdditiveHyperLinkProperties(hplnkReceiptsUptoMonth, (grp.UptoMonthSum + grp.ForMonthSum), SumType.ReceiptsUptoMonth);
//            //        SetAdditiveLabelProperties(lblReceiptssum, (grp.PreviousYearSum + grp.UptoMonthSum + grp.ForMonthSum), SumType.ReceiptsSum);
//            //    }
//                // VII a)
//                //else if (HeadOfAccountHelpers.EstablishmentExpenditures.Contains(grp.RoAccountType.HeadOfAccountType))
//                //{
//                //    SetAdditiveHyperLinkProperties(hplnkEstablishExpenditurePreviousYear, -grp.PreviousYearSum, SumType.PaymentsPreviousYear);
//                //    SetAdditiveHyperLinkProperties(hplnkEstablishExpenditureUptoMonth, -(grp.UptoMonthSum + grp.ForMonthSum), SumType.PaymentsUptoMonth);
//                //    SetAdditiveLabelProperties(lblEstablishExpendituresum, (-(grp.PreviousYearSum + grp.UptoMonthSum + grp.ForMonthSum)), SumType.PaymentsSum);
//                //    SetAdditivePropertiesFund(-grp.ForMonthSum, SumType.PaymentsForMonth);
//                //}
//                ////else if (HeadOfAccountHelpers.CivilExpenditures
//                ////            .Concat(HeadOfAccountHelpers.StockSuspense)
//                ////            .Concat(new[] { HeadOfAccountHelpers.ExciseDutySubTypes.ExciseDutyGOI })
//                ////            .Contains(grp.RoAccountType.HeadOfAccountType) || grp.RoAccountType.HeadOfAccountType == HeadOfAccountHelpers.AdvanceSubTypes.MaterialAdvance ||
//                ////    grp.RoAccountType.HeadOfAccountType == HeadOfAccountHelpers.AdvanceSubTypes.CivilPartyAdance || grp.RoAccountType.HeadOfAccountType == HeadOfAccountHelpers.TaxSubTypes.GreenTax ||
//                ////    grp.RoAccountType.HeadOfAccountType == HeadOfAccountHelpers.TaxSubTypes.BhutanSalesTax)
//                //// VII b) Calculating expenditure for Civil works (200 Heads).
//                //else if (HeadOfAccountHelpers.CivilExpenditures.Contains(grp.RoAccountType.HeadOfAccountType))
//                //{
//                //    SetAdditiveHyperLinkProperties(hplnkCivilExpenditurePreviousYear, -grp.PreviousYearSum, SumType.PaymentsPreviousYear);
//                //    SetAdditiveHyperLinkProperties(hplnkCivilExpenditureUpToMonth, -(grp.UptoMonthSum + grp.ForMonthSum), SumType.PaymentsUptoMonth);
//                //    SetAdditiveLabelProperties(lblCivilExpendituresum, (-(grp.PreviousYearSum + grp.UptoMonthSum + grp.ForMonthSum)), SumType.PaymentsSum);
//                //    SetAdditivePropertiesFund(-grp.ForMonthSum, SumType.PaymentsForMonth);
//                //}
//                //// VII c) Calculating expenditure for Electrical works (300 Heads)
//                //else if (HeadOfAccountHelpers.ElectricalExpenditures.Contains(grp.RoAccountType.HeadOfAccountType))
//                //{
//                //    SetAdditiveHyperLinkProperties(hplnkElectricalExpenditurePreviousYear, -grp.PreviousYearSum, SumType.PaymentsPreviousYear);
//                //    SetAdditiveHyperLinkProperties(hplnkElectricalExpenditureUpToMonth, -(grp.UptoMonthSum + grp.ForMonthSum), SumType.PaymentsUptoMonth);
//                //    SetAdditiveLabelProperties(lblElectricalExpendituresum, (-(grp.PreviousYearSum + grp.UptoMonthSum + grp.ForMonthSum)), SumType.PaymentsSum);
//                //    SetAdditivePropertiesFund(-grp.ForMonthSum, SumType.PaymentsForMonth);
//                //}
//                //// VII d) Calculating expenditure for Transmission works (400 heads)
//                //else if (HeadOfAccountHelpers.TransmissionExpenditures.Contains(grp.RoAccountType.HeadOfAccountType))
//                //{
//                //    SetAdditiveHyperLinkProperties(hplnkTransmissionExpenditurePreviousYear, -grp.PreviousYearSum, SumType.PaymentsPreviousYear);
//                //    SetAdditiveHyperLinkProperties(hplnkTransmissionExpenditureUpToMonth, -(grp.UptoMonthSum + grp.ForMonthSum), SumType.PaymentsUptoMonth);
//                //    SetAdditiveLabelProperties(lblTransmissionExpendituresum, (-(grp.PreviousYearSum + grp.UptoMonthSum + grp.ForMonthSum)), SumType.PaymentsSum);
//                //    SetAdditivePropertiesFund(-grp.ForMonthSum, SumType.PaymentsForMonth);
//                //}
////                else
////                {
////#if DEBUG
////                    throw new NotImplementedException(string.Format("Optimize the query and exclude head type {0}", grp.RoAccountType.HeadOfAccountType));
////#endif
////                }

//           // }

//            // Update hyperlink and label amounts
//            foreach (KeyValuePair<Control, decimal> kvp in m_controlAmounts)
//            {
//                HyperLink hl = kvp.Key as HyperLink;
//                if (hl == null)
//                {
//                    // Must be label
//                    Label lbl = (Label)kvp.Key;
//                    lbl.Text = DisplayInMillion(kvp.Value);
//                }
//                else
//                {
//                    hl.Text = DisplayInMillion(kvp.Value);
//                }
//            }
//            //ltrlCurrentMonth.Text = string.Empty;
//            //DateTime dateval = Convert.ToDateTime(dttbreceiptpayment.Text);
//            //valdate = dateval.ToString("Y");

//            //ltrlCurrentMonth.Text = "Expenditure for the Month of " + valdate + " (till date)";

//            //Sequence of queries does maater..as we want grid view to be rendered in the end.
//            // Getting fund in banks as on date
//            //var query3 = (from vd in db.RoVoucherDetails
//            //              join hh in db.RoHeadHierarchies on vd.HeadOfAccountId equals hh.HeadOfAccountId
//            //              where HeadOfAccountHelpers.AllBanks.Contains(hh.HeadOfAccountType)
//            //              group vd by vd.HeadOfAccountId into grouping
//            //              select new
//            //              {
//            //                  BankName = grouping.Max(p => p.HeadOfAccount.Description),
//            //                  Balance = (-(grouping.Sum(hoa => hoa.RoVoucher.VoucherDate < m_dtPreviousYear ? (hoa.CreditAmount ?? 0 - hoa.DebitAmount ?? 0) : 0) + grouping.Sum(hoa => hoa.RoVoucher.VoucherDate >= m_dtMonthStart && hoa.RoVoucher.VoucherDate <= m_dtPassed ? hoa.CreditAmount ?? 0 - hoa.DebitAmount ?? 0 : 0) + grouping.Sum(hoa => hoa.RoVoucher.VoucherDate >= m_dtPreviousYear && hoa.RoVoucher.VoucherDate < m_dtMonthStart ? hoa.CreditAmount ?? 0 - hoa.DebitAmount ?? 0 : 0))),
//            //                  BankHead = grouping.Max(p => p.HeadOfAccountId)
//            //              });
//            //// Get total of Balance fund in banks other than Bank at wangdue
//            //// We keep tottal of balance in million format.
//            //foreach (var bank in query3)
//            //{
//            //    _otherBankAmount = _otherBankAmount + bank.Balance;
//            //    otherBankAmount = DisplayInMillion(_otherBankAmount);
//            //}
//            //grvBankAccount.DataSource = query3;
//            //grvBankAccount.DataBind();
//            return;
//        }

        ///// <summary>
        ///// Make all NavigateUrls reasonable
        ///// </summary>
        ///// <param name="e"></param>
        //protected override void OnPreRender(EventArgs e)
        //{
        ////    base.OnPreRender(e);
        ////    foreach (Control ctl in hplnkGOIAidUptoMonth.NamingContainer.Controls)
        ////    {
        ////        HyperLink hl = ctl as HyperLink;
        ////        if (hl != null)
        ////        {
        ////            if (hl.Text.Trim().Length == 0)
        ////            {
        ////                hl.NavigateUrl = string.Empty;
        ////            }
        ////            else
        ////            {
        ////                hl.NavigateUrl = string.Format(hl.NavigateUrl, m_dtPreviousYear,
        ////                    m_dtMonthStart, m_dtPassed, m_dtPreviousYearEnd, m_dtMonthStart.AddDays(-1));
        ////            }
        ////        }
        ////    }

        ////    GetTotal();
        //}

        //private Dictionary<SumType, decimal> m_totals = new Dictionary<SumType, decimal>();

        ///// <summary>
        ///// The amount to be set as the text of the control
        ///// </summary>
        //private Dictionary<Control, decimal> m_controlAmounts = new Dictionary<Control, decimal>();

        ///// <summary>
        ///// If amount is not 0, sets amount as the text of the hyperlink and url as the passed url. Also maintains the sum.
        ///// The passed hyperlink already has text set, then the amount is added to its text.
        ///// </summary>
        ///// <param name="hl"></param>
        ///// <param name="amount"></param>
        ///// <param name="sumType"></param>
        ///// 
        //private void SetHyperLinkProperties(HyperLink hl, decimal? amount, SumType sumType)
        //{
        //    if (amount.HasValue && amount != 0)
        //    {
        //        SetControlAmount(hl, amount.Value, false);
        //        UpdateTotals(amount.Value, sumType);
        //    }
        //}

        //private void SetAdditiveHyperLinkProperties(HyperLink hl, decimal? amount, SumType sumType)
        //{
        //    if (amount.HasValue && amount != 0)
        //    {
        //        SetControlAmount(hl, amount.Value, true);
        //        UpdateTotals(amount.Value, sumType);
        //    }
        //}

        //private void SetControlAmount(Control ctl, decimal amount, bool allowAddition)
        //{
        //    if (m_controlAmounts.ContainsKey(ctl))
        //    {
        //        if (!allowAddition)
        //        {
        //            throw new InvalidOperationException("The amount for this control cannot be set multiple times");
        //        }
        //        m_controlAmounts[ctl] += amount;
        //    }
        //    else
        //    {
        //        m_controlAmounts.Add(ctl, amount);
        //    }
        //}

        //private void UpdateTotals(decimal amount, SumType sumType)
        //{
        //    if (m_totals.ContainsKey(sumType))
        //    {
        //        m_totals[sumType] += amount;
        //    }
        //    else
        //    {
        //        m_totals.Add(sumType, amount);
        //    }
        //}

        //private void SetLabelProperties(Label lbl, decimal? amount, SumType sumType)
        //{
        //    if (amount.HasValue && amount != 0)
        //    {
        //        SetControlAmount(lbl, amount.Value, false);
        //        UpdateTotals(amount.Value, sumType);
        //    }
        //}

        //private void SetAdditiveLabelProperties(Label lbl, decimal? amount, SumType sumType)
        //{
        //    if (amount.HasValue && amount != 0)
        //    {
        //        SetControlAmount(lbl, amount.Value, true);
        //        UpdateTotals(amount.Value, sumType);
        //    }
        //}

        //private void SetAdditivePropertiesFund(decimal? amount, SumType sumType)
        //{
        //    if (amount.HasValue && amount != 0)
        //    {
        //        UpdateTotals(amount.Value, sumType);
        //    }
        //}
        ///// <summary>
        ///// Displays the sum according to the SumType attribute associated with the label
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //protected void lbl_PreRenderShowSum(object sender, EventArgs e)
        //{
        //    Label lbl = (Label)sender;
        //    SumType totalType = (SumType)Enum.Parse(typeof(SumType), lbl.Attributes["SumType"]);
        //    if (m_totals.ContainsKey(totalType))
        //    {
        //        decimal d = m_totals[totalType];
        //        lbl.Text = DisplayInMillion(d);
        //    }
        //}

        //private enum ClSum
        //{
        //    ClPreviousYear,
        //    ClForMonth,
        //    ClUptoMonth,
        //    ClCummulative
        //}

        //Dictionary<ClSum, decimal> dictClSum = new Dictionary<ClSum, decimal>();

        ///// <summary>
        ///// Updates the Cl total.
        ///// </summary>
        ///// <param name="amount"></param>
        ///// <param name="sumType"></param>
        //private void UpdateClTotals(decimal amount, ClSum sumType)
        //{
        //    if (dictClSum.ContainsKey(sumType))
        //    {
        //        dictClSum[sumType] += amount;
        //    }
        //    else
        //    {
        //        dictClSum.Add(sumType, amount);
        //    }
        //}


        ///// <summary>
        ///// Displays closing balances for any period 
        ///// </summary>
        //private void GetTotal()
        //{
        //    foreach (KeyValuePair<Control, decimal> kvp in m_controlAmounts)
        //    {
        //        switch (kvp.Key.ID)
        //        {
        //            //case "hplnkClBalForMonth":
        //            case "lblClBalForMonthFE":
        //                UpdateClTotals(kvp.Value, ClSum.ClForMonth);
        //                UpdateClTotals(kvp.Value, ClSum.ClUptoMonth);
        //                UpdateClTotals(kvp.Value, ClSum.ClCummulative);
        //                break;

        //            case "hplnkOpBalUptoTheMonth":
        //            case "hplnkOpBalUptoTheMonthFE":
        //                UpdateClTotals(kvp.Value, ClSum.ClPreviousYear);
        //                break;
        //        }
        //    }

        //    foreach (KeyValuePair<SumType, decimal> kvp in m_totals)
        //    {
        //        switch (kvp.Key)
        //        {
        //            case SumType.PaymentsForMonth:
        //                UpdateClTotals(kvp.Value, ClSum.ClForMonth);
        //                break;

        //            case SumType.PaymentsUptoMonth:
        //                UpdateClTotals(kvp.Value, ClSum.ClUptoMonth);
        //                break;

        //            case SumType.PaymentsPreviousYear:
        //                UpdateClTotals(kvp.Value, ClSum.ClPreviousYear);
        //                break;

        //            case SumType.PaymentsSum:
        //                UpdateClTotals(kvp.Value, ClSum.ClCummulative);
        //                break;
        //        }
        //    }
        //}

        //protected void lblbalance_prerender(object sender, EventArgs e)
        //{
        //    decimal receiptTotal = 0;
        //    decimal paymentTotal = 0;
        //    Label lbl = (Label)sender;
        //    SumType totalType1 = (SumType)Enum.Parse(typeof(SumType), "ReceiptsPreviousYear");
        //    SumType totalType2 = (SumType)Enum.Parse(typeof(SumType), "PaymentsPreviousYear");
        //    if (m_totals.ContainsKey(totalType1))
        //    {
        //        receiptTotal = m_totals[totalType1];
        //    }
        //    if (m_totals.ContainsKey(totalType2))
        //    {
        //        paymentTotal = m_totals[totalType2];
        //    }
        //    decimal d = receiptTotal - paymentTotal;
        //    lbl.Text = DisplayInMillion(d);
        //}

        //protected void lblbalancefundcumulative_prerender(object sender, EventArgs e)
        //{

        //    Label lbl = (Label)sender;
        //    lbl.Text = DisplayInMillion(getBalancefund());
        //}

        //protected void lblExpenditureCurrentmonth_PreRenderShowSum(object sender, EventArgs e)
        //{
        //    Label lbl = (Label)sender;
        //    SumType totalType = (SumType)Enum.Parse(typeof(SumType), lbl.Attributes["SumType"]);
        //    if (m_totals.ContainsKey(totalType))
        //    {
        //        decimal d = m_totals[totalType];
        //        lbl.Text = DisplayInMillion(d);
        //    }

        //}

        //protected void lblDifference_PreRender(object sender, EventArgs e)
        //{
        //    Label lbl = (Label)sender;
        //    decimal d = m_totals[SumType.FundSum] - m_totals[SumType.PaymentsSum];
        //    lbl.Text = DisplayInMillion(d);
        //}

        ///// <summary>
        ///// Returns balance fund.i.e difference between receipts and expenditure.
        ///// </summary>
        ///// <returns></returns>
        //protected decimal getBalancefund()
        //{
        //    decimal receiptTotal = 0;
        //    decimal paymentTotal = 0;
        //    SumType totalType1 = (SumType)Enum.Parse(typeof(SumType), "ReceiptsSum");
        //    SumType totalType2 = (SumType)Enum.Parse(typeof(SumType), "PaymentsSum");
        //    if (m_totals.ContainsKey(totalType1))
        //    {
        //        receiptTotal = m_totals[totalType1];
        //    }
        //    if (m_totals.ContainsKey(totalType2))
        //    {
        //        paymentTotal = m_totals[totalType2];
        //    }
        //    decimal d = receiptTotal - paymentTotal;
        //    return d;
        //}

        /// <summary>
        /// Handling this event so as to show values in cell in million format.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //protected void grvBankAccount_RowDataBound(object sender, GridViewRowEventArgs e)
        //{
        //    switch (e.Row.RowType)
        //    {
        //        case DataControlRowType.DataRow:
        //            var val = m_totals[0];
        //            int indexAccountbanace = grvBankAccount.Columns.Cast<DataControlField>()
        //          .Select((p, i) => p.AccessibleHeaderText == "AccountBalance" ? i : -1).First(p => p >= 0);
        //            int indexAccountHead = grvBankAccount.Columns.Cast<DataControlField>()
        //          .Select((p, i) => p.AccessibleHeaderText == "AccountHead" ? i : -1).First(p => p >= 0);
        //            var valuehead = e.Row.Cells[indexAccountHead].Text;
        //            var value = Convert.ToDecimal(e.Row.Cells[indexAccountbanace].Text);
        //            //if (valuehead == "1415")
        //            //{
        //            //  // If bank is BOB wangdue then we get balance at bank by susbtracting total balace fund with balance in other banks
        //            //var balancefund = Convert.ToDecimal(DisplayInMillion(getBalancefund())) - _otherBankAmount;
        //            //e.Row.Cells[indexAccountbanace].Text = balancefund.ToString();
        //            //}
        //            //else
        //            //{

        //            e.Row.Cells[indexAccountbanace].Text = DisplayInMillion(value);
        //            //}
        //            break;
        //        case DataControlRowType.Footer:
        //            int indexNetPay1 = grvBankAccount.Columns.Cast<DataControlField>()
        //           .Select((p, i) => p.AccessibleHeaderText == "AccountBalance" ? i : -1).First(p => p >= 0);
        //            e.Row.Cells[indexNetPay1].Text = DisplayInMillion(getBalancefund());
        //            int indexNetPay2 = grvBankAccount.Columns.Cast<DataControlField>()
        //          .Select((p, i) => p.AccessibleHeaderText == "AccountName" ? i : -1).First(p => p >= 0);
        //            e.Row.Cells[indexNetPay2].Text = "TOTAL";
        //            break;
        //    }
        //}

        ///// <summary>
        ///// Display amount in millions..
        ///// </summary>
        ///// <param name="amount"></param>
        ///// <returns></returns>
        //private string DisplayInMillion(decimal? amount)
        //{
        //    if (amount.HasValue)
        //    {
        //        return amount.Value.ToString(MONEY_FORMAT_SPECIFIER);
        //    }
        //    return string.Empty;
        //}

        #endregion

        // The id parameter should match the DataKeyNames value set on the control
        // or be decorated with a value provider attribute, e.g. [QueryString]int id
        public FundPositionReportData GetFundData(int? id)
        {
            var dtPassed = dttbreceiptpayment.ValueAsDate.Value;
            // Input date 10 Jun 2008
            var dtPreviousYear = dtPassed.FinancialYearStartDate();         // {0}  1 Apr 2008
            var dtPreviousYearEnd = dtPassed.FinancialYearStartDate().AddDays(-1); //{3} 31 March 2008
            var dtMonthStart = dtPassed.MonthStartDate();                   // {1}  1 Jun 2008
            //var m_dtPassed = dtPassed;

            var db = (ReportingDataContext)dsQueries.Database;

            // The array contains heads in the order in which they are displayed on the report
            var allGoiHeadTypes = new[] { HeadOfAccountHelpers.GrantSubType.GrantNu, HeadOfAccountHelpers.GrantSubType.GrantFe,
                HeadOfAccountHelpers.LoanSubType.LoanNu, HeadOfAccountHelpers.LoanSubType.LoanFe };

            var allExpenditureHeadTypes = HeadOfAccountHelpers.EstablishmentExpenditures
                                .Concat(HeadOfAccountHelpers.WapcosExpenditures)
                                 .Concat(HeadOfAccountHelpers.CivilExpenditures).Concat(HeadOfAccountHelpers.ElectricalExpenditures)
                                 .Concat(HeadOfAccountHelpers.TransmissionExpenditures);

            //var query3 = (from vd in db.RoVoucherDetails
            //              join hh in db.RoHeadHierarchies on vd.HeadOfAccountId equals hh.HeadOfAccountId
            //              where HeadOfAccountHelpers.AllBanks.Contains(hh.HeadOfAccountType)
            //              group vd by vd.HeadOfAccountId into grouping
            //              select new
            //              {
            //                  BankName = grouping.Max(p => p.HeadOfAccount.Description),
            //                  Balance = (-(grouping.Sum(hoa => hoa.RoVoucher.VoucherDate < m_dtPreviousYear ? (hoa.CreditAmount ?? 0 - hoa.DebitAmount ?? 0) : 0)
            //                  + grouping.Sum(hoa => hoa.RoVoucher.VoucherDate >= m_dtMonthStart && hoa.RoVoucher.VoucherDate <= m_dtPassed ? hoa.CreditAmount ?? 0 - hoa.DebitAmount ?? 0 : 0)
            //                  + grouping.Sum(hoa => hoa.RoVoucher.VoucherDate >= m_dtPreviousYear && hoa.RoVoucher.VoucherDate < m_dtMonthStart ? hoa.CreditAmount ?? 0 - hoa.DebitAmount ?? 0 : 0))),
            //                  BankHead = grouping.Max(p => p.HeadOfAccountId)
            //              });
            var query5 = (from q in db.RoBills
                          where q.RoVouchers == null
                          group q by 1 into grouping
                          select new
                          {
                              outstandingBillsAmount = grouping.Sum(p => p.Amount) ?? 0
                          }).FirstOrDefault();
            //Getting sum of amount ahainst headofaccount type
            var query = (from vd in db.RoVoucherDetails
                         where vd.HeadOfAccount.HeadOfAccountId != null
                                && vd.HeadOfAccount.RoAccountType != null
                                && (vd.DebitAmount.HasValue || vd.CreditAmount.HasValue)
                                && vd.RoVoucher.VoucherDate <= dtPassed
                                //&& !HeadOfAccountHelpers.FundTransit
                                // .Concat(new[] { HeadOfAccountHelpers.ExciseDutySubTypes.ExciseDutyRGOB, HeadOfAccountHelpers.CashSubTypes.CashInHand })
                                // .Contains(vd.HeadOfAccount.RoAccountType.HeadOfAccountType)
                         group vd by 1 into g

                         // let prevYearVouchers = g.Where(p => p.RoVoucher.VoucherDate <= m_dtPreviousYear) // TODO: I think we should be using this
                         let prevYearVouchers = g.Where(p => p.RoVoucher.VoucherDate < dtPreviousYear)
                         let currYearVouchers = g.Where(p => p.RoVoucher.VoucherDate >= dtPreviousYear)
                         //II
                         let fundReceivedGOITotalUpToPrev = (decimal?)prevYearVouchers.Where(p => allGoiHeadTypes.Contains(p.HeadOfAccount.HeadOfAccountType))
                                            .Sum(p => p.CreditAmount ?? 0 - p.DebitAmount ?? 0)

                         let fundReceivedGOITotalCurr = (decimal?)currYearVouchers.Where(p => allGoiHeadTypes.Contains(p.HeadOfAccount.HeadOfAccountType))
                                            .Sum(p => p.CreditAmount ?? 0 - p.DebitAmount ?? 0)

                         let fundReceivedGOITotalCum = (decimal?)g.Where(p => allGoiHeadTypes.Contains(p.HeadOfAccount.HeadOfAccountType))
                                            .Sum(p => p.CreditAmount ?? 0 - p.DebitAmount ?? 0)

                         // III
                         let fundReceivedOtherUpToPrev = (decimal?)prevYearVouchers.Where(p => HeadOfAccountHelpers.OtherFundReceipts.Contains(p.HeadOfAccount.HeadOfAccountType))
                                                         .Sum(p => p.CreditAmount ?? 0 - p.DebitAmount ?? 0)

                         let fundReceivedOtherCurr = (decimal?)currYearVouchers.Where(p => HeadOfAccountHelpers.OtherFundReceipts.Contains(p.HeadOfAccount.HeadOfAccountType))
                                                        .Sum(p => p.CreditAmount ?? 0 - p.DebitAmount ?? 0)
                         let fundReceivedOtherCum = (decimal?)g.Where(p => HeadOfAccountHelpers.OtherFundReceipts.Contains(p.HeadOfAccount.HeadOfAccountType))
                                .Sum(p => p.CreditAmount ?? 0 - p.DebitAmount ?? 0)

                         // IV = II + III
                         let fundReceivedOtherAndGOITotalUpToPrev = fundReceivedGOITotalUpToPrev == null && fundReceivedOtherUpToPrev == null ? (decimal?)null :
                            (fundReceivedGOITotalUpToPrev ?? 0) + (fundReceivedOtherUpToPrev ?? 0)

                         let fundReceivedOtherAndGOITotalCurr = fundReceivedGOITotalCurr == null && fundReceivedOtherCurr == null ? (decimal?)null :
                                                                (fundReceivedGOITotalCurr ?? 0) + (fundReceivedOtherCurr ?? 0)

                         let fundReceivedOtherAndGOITotalCum = fundReceivedGOITotalCum == null && fundReceivedOtherCum == null ? (decimal?)null :
                                                       (fundReceivedGOITotalCum ?? 0) + (fundReceivedOtherCum ?? 0)


                         let expendituresUpToPrev = (decimal?)prevYearVouchers.Where(p => allExpenditureHeadTypes.Contains(p.HeadOfAccount.HeadOfAccountType))
                               .Sum(p => p.DebitAmount ?? 0 - p.CreditAmount ?? 0)

                         let expendituresCurr = (decimal?)currYearVouchers.Where(p => allExpenditureHeadTypes.Contains(p.HeadOfAccount.HeadOfAccountType))
                                .Sum(p => p.DebitAmount ?? 0 - p.CreditAmount ?? 0)

                         let expendituresCum = (decimal?)g.Where(p => allExpenditureHeadTypes.Contains(p.HeadOfAccount.HeadOfAccountType))
                                .Sum(p => p.DebitAmount ?? 0 - p.CreditAmount ?? 0)

                         select new FundPositionReportData
                         {
                             DateFrom = dtPreviousYear,
                             DateTo = dtPassed,
                             PreviousYearEndDate = dtPreviousYearEnd,
                             // I - a
                             FundsReceivedGOIGrantNuUpToPrev = prevYearVouchers.Where(p => p.HeadOfAccount.HeadOfAccountType == allGoiHeadTypes[0])
                                                        .Sum(p => p.CreditAmount ?? 0 - p.DebitAmount ?? 0),
                             FundsReceivedGOIGrantNuCurr = currYearVouchers.Where(p => p.HeadOfAccount.HeadOfAccountType == allGoiHeadTypes[0])
                                                        .Sum(p => p.CreditAmount ?? 0 - p.DebitAmount ?? 0),
                             FundsReceivedGOIGrantNuCum = g.Where(p => p.HeadOfAccount.HeadOfAccountType == allGoiHeadTypes[0]).Sum(p => p.CreditAmount ?? 0 - p.DebitAmount ?? 0),
                             FundsReceivedGOIGrantNuHeads = allGoiHeadTypes[0],

                             //I - b
                             FundsReceivedGOIGrantFeUpToPrev = prevYearVouchers.Where(p => p.HeadOfAccount.HeadOfAccountType == allGoiHeadTypes[1])
                                                        .Sum(p => p.CreditAmount ?? 0 - p.DebitAmount ?? 0),
                             FundsReceivedGOIGrantFeCurr = currYearVouchers.Where(p => p.HeadOfAccount.HeadOfAccountType == allGoiHeadTypes[1])
                                                        .Sum(p => p.CreditAmount ?? 0 - p.DebitAmount ?? 0),
                             FundsReceivedGOIGrantFeCum = g.Where(p => p.HeadOfAccount.HeadOfAccountType == allGoiHeadTypes[1])
                                .Sum(p => p.CreditAmount ?? 0 - p.DebitAmount ?? 0),
                             FundsReceivedGOIGrantFeHeads = allGoiHeadTypes[1],

                             // I - c
                             FundsReceivedGOILoanNuUpToPrev = prevYearVouchers.Where(p => p.HeadOfAccount.HeadOfAccountType == allGoiHeadTypes[2])
                                                        .Sum(p => p.CreditAmount ?? 0 - p.DebitAmount ?? 0),
                             FundsReceivedGOILoanNuCurr = currYearVouchers.Where(p => p.HeadOfAccount.HeadOfAccountType == allGoiHeadTypes[2])
                                                        .Sum(p => p.CreditAmount ?? 0 - p.DebitAmount ?? 0),
                             FundsReceivedGOILoanNuCum = g.Where(p => p.HeadOfAccount.HeadOfAccountType == allGoiHeadTypes[2])
                                .Sum(p => p.CreditAmount ?? 0 - p.DebitAmount ?? 0),

                             FundsReceivedGOILoanNuHeads = allGoiHeadTypes[2],

                             //I - d
                             FundsReceivedGOILoanFeUpToPrev = prevYearVouchers.Where(p => p.HeadOfAccount.HeadOfAccountType == allGoiHeadTypes[3])
                                                        .Sum(p => p.CreditAmount ?? 0 - p.DebitAmount ?? 0),
                             FundsReceivedGOILoanFeCurr = currYearVouchers.Where(p => p.HeadOfAccount.HeadOfAccountType == allGoiHeadTypes[3])
                                                        .Sum(p => p.CreditAmount ?? 0 - p.DebitAmount ?? 0),
                             FundsReceivedGOILoanFeCum = g.Where(p => p.HeadOfAccount.HeadOfAccountType == allGoiHeadTypes[3])
                                .Sum(p => p.CreditAmount ?? 0 - p.DebitAmount ?? 0),
                             FundsReceivedGOILoanFeHeads = allGoiHeadTypes[3],

                             //II
                             FundReceivedGOITotalUpToPrev = fundReceivedGOITotalUpToPrev,
                             FundReceivedGOITotalCurr = fundReceivedGOITotalCurr,
                             FundReceivedGOITotalCum = fundReceivedGOITotalCum,
                             //III
                             FundReceivedOtherUpToPrev = fundReceivedOtherUpToPrev,
                             FundReceivedOtherCurr = fundReceivedOtherCurr,
                             FundReceivedOtherCum = fundReceivedOtherCum,
                             FundReceivedOtherHeads = string.Join(",", HeadOfAccountHelpers.OtherFundReceipts),

                             //IV = II + III
                             FundReceivedOtherAndGOITotalUpToPrev = fundReceivedOtherAndGOITotalUpToPrev,

                             FundReceivedOtherAndGOITotalCurr = fundReceivedOtherAndGOITotalCurr,
                             FundReceivedOtherAndGOITotalCum = fundReceivedOtherAndGOITotalCum,

                             // V
                             ExpendituresUpToPrev = expendituresUpToPrev,
                             ExpendituresCurr = expendituresCurr,
                             ExpendituresCum = expendituresCum,

                             // VI = IV - V
                             BalanceFundUpToPrev = fundReceivedOtherAndGOITotalUpToPrev == null && expendituresUpToPrev == null ? (decimal?)null :
                                (fundReceivedOtherAndGOITotalUpToPrev ?? 0) - (expendituresUpToPrev ?? 0),

                             //BalanceFundCurr = fundReceivedOtherAndGOITotalUpToPrev == null && expendituresUpToPrev == null ? (decimal?)null :
                             //   (fundReceivedOtherAndGOITotalUpToPrev ?? 0) - (expendituresUpToPrev ?? 0),

                             BalanceFundCum = fundReceivedOtherAndGOITotalCum == null && expendituresCum == null ? (decimal?)null :
                                (fundReceivedOtherAndGOITotalCum ?? 0) - (expendituresCum ?? 0),

                             //VII -a
                             EstablishmentExpendituresUpToPrev = prevYearVouchers.Where(p => HeadOfAccountHelpers.EstablishmentExpenditures.Contains(p.HeadOfAccount.HeadOfAccountType))
                                                         .Sum(p => p.DebitAmount ?? 0 - p.CreditAmount ?? 0),

                             EstablishmentExpendituresCurr = currYearVouchers.Where(p => HeadOfAccountHelpers.EstablishmentExpenditures.Contains(p.HeadOfAccount.HeadOfAccountType))
                                             .Sum(p => p.DebitAmount ?? 0 - p.CreditAmount ?? 0),
                             EstablishmentExpendituresCum = g.Where(p => HeadOfAccountHelpers.EstablishmentExpenditures.Contains(p.HeadOfAccount.HeadOfAccountType))
                                         .Sum(p => p.DebitAmount ?? 0 - p.CreditAmount ?? 0),

                             EstablishmentExpendituresHead = string.Join(",", HeadOfAccountHelpers.EstablishmentExpenditures),

                             //VII -a1
                             WapcosExpendituresUpToPrev = prevYearVouchers.Where(p => HeadOfAccountHelpers.WapcosExpenditures.Contains(p.HeadOfAccount.HeadOfAccountType))
                                                         .Sum(p => p.DebitAmount ?? 0 - p.CreditAmount ?? 0),

                             WapcosExpendituresCurr = currYearVouchers.Where(p => HeadOfAccountHelpers.WapcosExpenditures.Contains(p.HeadOfAccount.HeadOfAccountType))
                                             .Sum(p => p.DebitAmount ?? 0 - p.CreditAmount ?? 0),
                             WapcosExpendituresCum = g.Where(p => HeadOfAccountHelpers.WapcosExpenditures.Contains(p.HeadOfAccount.HeadOfAccountType))
                                         .Sum(p => p.DebitAmount ?? 0 - p.CreditAmount ?? 0),

                             WapcosExpendituresHead = string.Join(",", HeadOfAccountHelpers.WapcosExpenditures),


                             //VII -b

                             CivilWorkExpendituresUpToPrev = prevYearVouchers.Where(p => HeadOfAccountHelpers.CivilExpenditures.Contains(p.HeadOfAccount.HeadOfAccountType))
                                                         .Sum(p => p.DebitAmount ?? 0 - p.CreditAmount ?? 0),

                             CivilWorkExpendituresCurr = currYearVouchers.Where(p => HeadOfAccountHelpers.CivilExpenditures.Contains(p.HeadOfAccount.HeadOfAccountType))
                                             .Sum(p => p.DebitAmount ?? 0 - p.CreditAmount ?? 0),
                             CivilWorkExpendituresCum = g.Where(p => HeadOfAccountHelpers.CivilExpenditures.Contains(p.HeadOfAccount.HeadOfAccountType))
                                         .Sum(p => p.DebitAmount ?? 0 - p.CreditAmount ?? 0),

                             CivilWorkExpendituresHead = string.Join(",", HeadOfAccountHelpers.CivilExpenditures),

                             //VII -c

                             ElectricalExpendituresUpToPrev = prevYearVouchers.Where(p => HeadOfAccountHelpers.ElectricalExpenditures.Contains(p.HeadOfAccount.HeadOfAccountType))
                                                         .Sum(p => p.DebitAmount ?? 0 - p.CreditAmount ?? 0),

                             ElectricalExpendituresCurr = currYearVouchers.Where(p => HeadOfAccountHelpers.ElectricalExpenditures.Contains(p.HeadOfAccount.HeadOfAccountType))
                                             .Sum(p => p.DebitAmount ?? 0 - p.CreditAmount ?? 0),
                             ElectricalExpendituresCum = g.Where(p => HeadOfAccountHelpers.ElectricalExpenditures.Contains(p.HeadOfAccount.HeadOfAccountType))
                                         .Sum(p => p.DebitAmount ?? 0 - p.CreditAmount ?? 0),

                             ElectricalExpendituresHead = string.Join(",", HeadOfAccountHelpers.ElectricalExpenditures),


                             //VII -d

                             TransmissionExpendituresUpToPrev = prevYearVouchers.Where(p => HeadOfAccountHelpers.TransmissionExpenditures.Contains(p.HeadOfAccount.HeadOfAccountType))
                                                         .Sum(p => p.DebitAmount ?? 0 - p.CreditAmount ?? 0),

                             TransmissionExpendituresCurr = currYearVouchers.Where(p => HeadOfAccountHelpers.TransmissionExpenditures.Contains(p.HeadOfAccount.HeadOfAccountType))
                                             .Sum(p => p.DebitAmount ?? 0 - p.CreditAmount ?? 0),
                             TransmissionExpendituresCum = g.Where(p => HeadOfAccountHelpers.TransmissionExpenditures.Contains(p.HeadOfAccount.HeadOfAccountType))
                                         .Sum(p => p.DebitAmount ?? 0 - p.CreditAmount ?? 0),

                             TransmissionExpendituresHead = string.Join(",", HeadOfAccountHelpers.TransmissionExpenditures),

                             // VIII
                             AllBanks = from item in g
                                 where HeadOfAccountHelpers.AllBanks.Contains(item.HeadOfAccount.HeadOfAccountType)
                                 group item by item.HeadOfAccount into g2
                                 select new
                                 {
                                     BankName = g2.Key.Description,
                                     Balance = -g2.Sum(p => p.CreditAmount ?? 0 - p.DebitAmount ?? 0),
                                     BankHead = g2.Key.HeadOfAccountId
                                 },

                             BalanceFund = fundReceivedGOITotalCum == null && expendituresCum == null ? (decimal?)null :
                                           (fundReceivedGOITotalCum ?? 0) - (expendituresCum ?? 0),

                             CurrentMonthExpenditure = g.Where(p => allExpenditureHeadTypes.Contains(p.HeadOfAccount.HeadOfAccountType) &&
                                 p.RoVoucher.VoucherDate >= dtMonthStart && p.RoVoucher.VoucherDate <= dtPassed)
                                .Sum(p => p.DebitAmount ?? 0 - p.CreditAmount ?? 0),

                                // query5 will be null if there are no outstanding bills
                             OutstandingBillsAmount = query5 == null ? (decimal?)null : query5.outstandingBillsAmount
                         });
            //throw new NotImplementedException();
            return query.FirstOrDefault();
        }

    }
}
