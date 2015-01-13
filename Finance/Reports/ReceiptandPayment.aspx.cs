/*
 *  Programming: Eclipse Systems (P) Ltd., NOIDA, INDIA
 *  E-mail: support@eclsys.com
 *
 *  $Workfile:   ReceiptandPayment.aspx.cs  $
 *  $Revision: 35263 $
 *  $Author: ssinghal $
 *  $Date: 2010-09-16 21:06:27 +0530 (Thu, 16 Sep 2010) $
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

namespace Finance.Reports
{
    public partial class ReceiptandPayment : PageBase
    {
        private ReportingDataContext m_db;
        protected DateTime m_dtPreviousYear;
        protected DateTime m_dtPreviousYearEnd;
        protected DateTime m_dtMonthStart;
        protected DateTime m_dtMonthEnd;

        protected override void OnLoad(EventArgs e)
        {
            DateTime? date;
            if (!Page.IsPostBack)
            {
                date = new DateTime();
                date = DateTime.Now;
                dttbreceiptpayment.Text = date.Value.ToShortDateString();
            }
            if (btnGo.IsPageValid())
            {
                date = (DateTime?)(dttbreceiptpayment.ValueAsDate);
                CalculateReceiptandPayment(date.Value);
                this.Title = string.Format("Receipts & Payments Statement for the month ending - {0:dd/MM/yyyy}", date.Value);
                this.DataBind();
            }
            base.OnLoad(e);
        }

        const string MONEY_FORMAT_SPECIFIER = "###,###,###,##0.00;(###,###,###,##0.00);'&nbsp;'";
        private enum SumType
        {
            PaymentsPreviousYear,
            PaymentsForMonth,
            PaymentsUptoMonth,
            ReceiptsPreviousYear,
            ReceiptsForMonth,
            ReceiptsUptoMonth,
            ReceiptsSum,
            PaymentsSum,
            test
        }

        /// <summary>
        /// Receipts and payments Calculation of various heads for whole time duration
        /// </summary>
        /// <param name="dt"></param>
        private void CalculateReceiptandPayment(DateTime dt)
        {
            // Input date 10 Jun 2008
            m_dtPreviousYear = dt.FinancialYearStartDate();         // {0}  1 Apr 2008
            m_dtPreviousYearEnd = dt.FinancialYearStartDate().AddDays(-1); //{3} 31 March 2008
            m_dtMonthStart = dt.MonthStartDate();                   // {1}  1 Jun 2008
            m_dtMonthEnd = dt.MonthEndDate();                       // {2}  30 Jun 2008

            m_db = (ReportingDataContext)dsQueries.Database;

            var query = (from vd in m_db.RoVoucherDetails
                         where vd.RoVoucher.VoucherId == vd.VoucherId
                                && vd.HeadOfAccount.HeadOfAccountType != null
                         group vd by new
                         {
                             vd.HeadOfAccount.RoAccountType
                         }
                             into grouping
                             select new
                             {
                                 grouping.Key,
                                 PreviousYearSum = grouping.Sum(hoa => hoa.RoVoucher.VoucherDate < m_dtPreviousYear ? (hoa.CreditAmount?? 0 - hoa.DebitAmount?? 0) : 0),
                                 ForMonthSum = grouping.Sum(hoa => hoa.RoVoucher.VoucherDate >= m_dtMonthStart && hoa.RoVoucher.VoucherDate <= m_dtMonthEnd ? hoa.CreditAmount?? 0 - hoa.DebitAmount?? 0 : 0),
                                 UptoMonthSum = grouping.Sum(hoa => hoa.RoVoucher.VoucherDate >= m_dtPreviousYear && hoa.RoVoucher.VoucherDate < m_dtMonthStart ? hoa.CreditAmount?? 0 - hoa.DebitAmount?? 0 : 0)
                             });

            foreach (var grp in query)
            {
                if (HeadOfAccountHelpers.CashSubType.CashInBankNu
                    .Concat(HeadOfAccountHelpers.CashSubType.CashInHand)
                    .Concat(HeadOfAccountHelpers.CashSubType.Investment)
                    .Contains(grp.Key.RoAccountType.HeadOfAccountType))
                {
                    SetAdditiveHyperLinkProperties(hplnkOpBalForTheMonth, -(grp.PreviousYearSum + grp.UptoMonthSum), SumType.ReceiptsForMonth);
                    SetAdditiveHyperLinkProperties(hplnkOpBalUptoTheMonth, -grp.PreviousYearSum, SumType.ReceiptsUptoMonth);
                    SetAdditiveHyperLinkProperties(hplnkClBalForMonth, -(grp.PreviousYearSum + grp.UptoMonthSum + grp.ForMonthSum), SumType.test);
                }
                else if (HeadOfAccountHelpers.CashSubType.CashInBankFe.Contains(grp.Key.RoAccountType.HeadOfAccountType))
                {
                    SetHyperLinkProperties(hplnkOpBalForTheMonthFE, -(grp.PreviousYearSum + grp.UptoMonthSum), SumType.ReceiptsForMonth);
                    SetHyperLinkProperties(hplnkOpBalUptoTheMonthFE, -grp.PreviousYearSum, SumType.ReceiptsUptoMonth);
                    SetHyperLinkProperties(hplnkClBalForMonthFE, -(grp.PreviousYearSum + grp.UptoMonthSum + grp.ForMonthSum), SumType.test);
                }
                else if (HeadOfAccountHelpers.GrantSubType.GrantNu
                    .Concat(HeadOfAccountHelpers.LoanSubType.LoanNu)
                    .Contains(grp.Key.RoAccountType.HeadOfAccountType))
                {
                    SetAdditiveHyperLinkProperties(hplnkGOIAidPreviousYear, grp.PreviousYearSum, SumType.ReceiptsPreviousYear);
                    SetAdditiveHyperLinkProperties(hplnkGOIAidForMonth, grp.ForMonthSum, SumType.ReceiptsForMonth);
                    SetAdditiveHyperLinkProperties(hplnkGOIAidUptoMonth, (grp.UptoMonthSum + grp.ForMonthSum), SumType.ReceiptsUptoMonth);
                    SetAdditiveLabelProperties(lblGOIAidsum, grp.PreviousYearSum + grp.ForMonthSum + grp.UptoMonthSum, SumType.ReceiptsSum);
                }
                else if (HeadOfAccountHelpers.GrantSubType.GrantFe
                    .Concat(HeadOfAccountHelpers.LoanSubType.LoanFe)
                    .Contains(grp.Key.RoAccountType.HeadOfAccountType))
                {
                    SetAdditiveHyperLinkProperties(hplnkGOIFEPreviousYear, grp.PreviousYearSum, SumType.ReceiptsPreviousYear);
                    SetAdditiveHyperLinkProperties(hplnkGOIFEForMonth, grp.ForMonthSum, SumType.ReceiptsForMonth);
                    SetAdditiveHyperLinkProperties(hplnkGOIFEUptoMonth, (grp.UptoMonthSum + grp.ForMonthSum), SumType.ReceiptsUptoMonth);
                    SetAdditiveLabelProperties(lblGOIFEsum, (grp.PreviousYearSum + grp.UptoMonthSum + grp.ForMonthSum), SumType.ReceiptsSum);
                }
                else if (HeadOfAccountHelpers.ReceiptSubType.InterestReceipts.Contains(grp.Key.RoAccountType.HeadOfAccountType))
                {
                    SetHyperLinkProperties(hplnkInterestPreviousYear, grp.PreviousYearSum, SumType.ReceiptsPreviousYear);
                    SetHyperLinkProperties(hplnkInterestForMonth, grp.ForMonthSum, SumType.ReceiptsForMonth);
                    SetHyperLinkProperties(hplnkInterestUptoMonth, (grp.UptoMonthSum + grp.ForMonthSum), SumType.ReceiptsUptoMonth);
                    SetLabelProperties(lblInterestsum, (grp.PreviousYearSum + grp.UptoMonthSum + grp.ForMonthSum), SumType.ReceiptsSum);
                    }
                else if (HeadOfAccountHelpers.SalaryRemitances.Contains(grp.Key.RoAccountType.HeadOfAccountType))
                {
                    SetHyperLinkProperties(hplnkRecoveryPreviousYear, grp.PreviousYearSum, SumType.ReceiptsPreviousYear);
                    SetHyperLinkProperties(hplnkRecoveryForMonth, grp.ForMonthSum, SumType.ReceiptsForMonth);
                    SetHyperLinkProperties(hplnkRecoveryUptoMonth, (grp.UptoMonthSum + grp.ForMonthSum), SumType.ReceiptsUptoMonth);
                    SetLabelProperties(lblRecoverysum, grp.PreviousYearSum + grp.ForMonthSum + grp.UptoMonthSum, SumType.ReceiptsSum);
                }
                else if (HeadOfAccountHelpers.ReceiptSubType.TenderSale.Contains(grp.Key.RoAccountType.HeadOfAccountType))
                {
                    SetHyperLinkProperties(hplnkTenderSalePreviousYear, grp.PreviousYearSum, SumType.ReceiptsPreviousYear);
                    SetHyperLinkProperties(hplnkTenderSaleForMonth, grp.ForMonthSum, SumType.ReceiptsForMonth);
                    SetHyperLinkProperties(hplnkTenderSaleUptoMonth, (grp.UptoMonthSum + grp.ForMonthSum), SumType.ReceiptsUptoMonth);
                    SetLabelProperties(lblTenderSalesum, grp.PreviousYearSum + grp.UptoMonthSum + grp.ForMonthSum, SumType.ReceiptsSum);
                }
                else if(HeadOfAccountHelpers.DepositSubTypes.EarnestMoneyDeposit.Contains(grp.Key.RoAccountType.HeadOfAccountType))
                {
                    SetHyperLinkProperties(hplnkEMDPreviousYear, grp.PreviousYearSum, SumType.ReceiptsPreviousYear);
                    SetHyperLinkProperties(hplnkEMDForMonth, grp.ForMonthSum, SumType.ReceiptsForMonth);
                    SetHyperLinkProperties(hplnkEMDUptoMonth, (grp.UptoMonthSum + grp.ForMonthSum), SumType.ReceiptsUptoMonth);
                    SetLabelProperties(lblEMDsum, grp.PreviousYearSum + grp.UptoMonthSum + grp.ForMonthSum, SumType.ReceiptsSum);
                }
                else if(HeadOfAccountHelpers.DepositSubTypes.SecurityDeposits.Contains(grp.Key.RoAccountType.HeadOfAccountType))
                {
                    SetHyperLinkProperties(hplnkSecurityDepositPreviousYear, grp.PreviousYearSum, SumType.ReceiptsPreviousYear);
                    SetHyperLinkProperties(hplnkSecurityDepositForMonth, grp.ForMonthSum, SumType.ReceiptsForMonth);
                    SetHyperLinkProperties(hplnkSecurityDepositUptoMonth, (grp.UptoMonthSum + grp.ForMonthSum), SumType.ReceiptsUptoMonth);
                    SetLabelProperties(lblSecurityDepositsum, (grp.PreviousYearSum + grp.UptoMonthSum + grp.ForMonthSum), SumType.ReceiptsSum);
                }
                else if(HeadOfAccountHelpers.TaxSubTypes.BhutanTax.Contains(grp.Key.RoAccountType.HeadOfAccountType))
                {
                    SetHyperLinkProperties(hplnkBITPreviousYear, grp.PreviousYearSum, SumType.ReceiptsPreviousYear);
                    SetHyperLinkProperties(hplnkBITForMonth, grp.ForMonthSum, SumType.ReceiptsForMonth);
                    SetHyperLinkProperties(hplnkBITUptoMonth, (grp.UptoMonthSum + grp.ForMonthSum), SumType.ReceiptsUptoMonth);
                    SetLabelProperties(lblBITsum, (grp.PreviousYearSum + grp.UptoMonthSum + grp.ForMonthSum), SumType.ReceiptsSum);
                }
                else if (HeadOfAccountHelpers.AdvanceSubTypes.EmployeeAdvance.Contains(grp.Key.RoAccountType.HeadOfAccountType))
                {
                    SetHyperLinkProperties(hplnkEmpAdvPreviousYear, -grp.PreviousYearSum, SumType.PaymentsPreviousYear);
                    SetHyperLinkProperties(hplnkEmpAdvForMonth, -grp.ForMonthSum, SumType.PaymentsForMonth);
                    SetHyperLinkProperties(hplnkEmpAdvUptoMonth, (-(grp.UptoMonthSum + grp.ForMonthSum)), SumType.PaymentsUptoMonth);
                    SetLabelProperties(lblEmpAdvsum, (-(grp.PreviousYearSum + grp.UptoMonthSum + grp.ForMonthSum)), SumType.PaymentsSum);
                }
                else if(HeadOfAccountHelpers.AdvanceSubTypes.PartyAdvance.Concat(HeadOfAccountHelpers.AdvanceSubTypes.MaterialAdvance).Contains(grp.Key.RoAccountType.HeadOfAccountType))
                {
                    SetAdditiveHyperLinkProperties(hplnkContAdvPreviousYear, -grp.PreviousYearSum, SumType.PaymentsPreviousYear);
                    SetAdditiveHyperLinkProperties(hplnkContAdvForMonth, -grp.ForMonthSum, SumType.PaymentsForMonth);
                    SetAdditiveHyperLinkProperties(hplnkContAdvUptoMonth, -(grp.UptoMonthSum + grp.ForMonthSum), SumType.PaymentsUptoMonth);
                    SetAdditiveLabelProperties(lblContAdvsum, (-(grp.PreviousYearSum + grp.UptoMonthSum + grp.ForMonthSum)), SumType.PaymentsSum);
                }
                else if (HeadOfAccountHelpers.StockSuspense.Contains(grp.Key.RoAccountType.HeadOfAccountType))
                {
                    SetHyperLinkProperties(hplnkStockSuspensePreviousYear, -grp.PreviousYearSum, SumType.PaymentsPreviousYear);
                    SetHyperLinkProperties(hplnkStockSuspenseForMonth, -grp.ForMonthSum, SumType.PaymentsForMonth);
                    SetHyperLinkProperties(hplnkStockSuspenseUptoMonth, (-(grp.UptoMonthSum + grp.ForMonthSum)), SumType.PaymentsUptoMonth);
                    SetLabelProperties(lblStockSuspensesum, (-(grp.UptoMonthSum + grp.ForMonthSum + grp.PreviousYearSum)), SumType.PaymentsSum);
                }
                else if (HeadOfAccountHelpers.TaxSubTypes.BhutanSalesTax.Contains(grp.Key.RoAccountType.HeadOfAccountType))
                {
                    SetHyperLinkProperties(hplnkBSTPreviousYear, -grp.PreviousYearSum, SumType.PaymentsPreviousYear);
                    SetHyperLinkProperties(hplnkBSTForMonth, -grp.ForMonthSum, SumType.PaymentsForMonth);
                    SetHyperLinkProperties(hplnkBSTUptoMonth, (-(grp.UptoMonthSum + grp.ForMonthSum)), SumType.PaymentsUptoMonth);
                    SetLabelProperties(lblBSTsum, (-(grp.UptoMonthSum + grp.ForMonthSum + grp.PreviousYearSum)), SumType.PaymentsSum);
                }
                else if(HeadOfAccountHelpers.TaxSubTypes.ServiceTax.Contains(grp.Key.RoAccountType.HeadOfAccountType))
                {
                    SetHyperLinkProperties(hplnksvctaxPreviousYear, -grp.PreviousYearSum, SumType.PaymentsPreviousYear);
                    SetHyperLinkProperties(hplnksvctaxForMonth, -grp.ForMonthSum, SumType.PaymentsForMonth);
                    SetHyperLinkProperties(hplnksvctaxUptoMonth, (-(grp.UptoMonthSum + grp.ForMonthSum)), SumType.PaymentsUptoMonth);
                    SetLabelProperties(lblsvctaxsum, (-(grp.UptoMonthSum + grp.ForMonthSum + grp.PreviousYearSum)), SumType.PaymentsSum);
                }
                else if (HeadOfAccountHelpers.DutySubType.ExciseDutiesGOI.Contains(grp.Key.RoAccountType.HeadOfAccountType))
                {
                    SetHyperLinkProperties(hplnkExciseDutyGOIPreviousYear, -grp.PreviousYearSum, SumType.PaymentsPreviousYear);
                    SetHyperLinkProperties(hplnkExciseDutyGOIForMonth, -grp.ForMonthSum, SumType.PaymentsForMonth);
                    SetHyperLinkProperties(hplnkExciseDutyGOIUptoMonth, (-(grp.UptoMonthSum + grp.ForMonthSum)), SumType.PaymentsUptoMonth);
                    SetLabelProperties(lblExciseDutyGOIsum, (-(grp.UptoMonthSum + grp.ForMonthSum + grp.PreviousYearSum)), SumType.PaymentsSum);
                }
                else if(HeadOfAccountHelpers.DutySubType.ExciseDutiesRGOB.Contains(grp.Key.RoAccountType.HeadOfAccountType))
                {
                    SetHyperLinkProperties(hplnkExciseDutyRGOBPreviousYear, -grp.PreviousYearSum, SumType.PaymentsPreviousYear);
                    SetHyperLinkProperties(hplnkExciseDutyRGOBForMonth, -grp.ForMonthSum, SumType.PaymentsForMonth);
                    SetHyperLinkProperties(hplnkExciseDutyRGOBUptoMonth, (-(grp.UptoMonthSum + grp.ForMonthSum)), SumType.PaymentsUptoMonth);
                    SetLabelProperties(lblExciseDutyRGOBsum, (-(grp.UptoMonthSum + grp.ForMonthSum + grp.PreviousYearSum)), SumType.PaymentsSum);
                }
                else if(HeadOfAccountHelpers.TaxSubTypes.GreenTax.Contains(grp.Key.RoAccountType.HeadOfAccountType))
                {
                    SetHyperLinkProperties(hplnkGreenTaxPreviousYear, -grp.PreviousYearSum, SumType.PaymentsPreviousYear);
                    SetHyperLinkProperties(hplnkGreenTaxForMonth, -grp.ForMonthSum, SumType.PaymentsForMonth);
                    SetHyperLinkProperties(hplnkGreenTaxUptoMonth, (-(grp.UptoMonthSum + grp.ForMonthSum)), SumType.PaymentsUptoMonth);
                    SetLabelProperties(lblGreenTaxRGOBsum, (-(grp.UptoMonthSum + grp.ForMonthSum + grp.PreviousYearSum)), SumType.PaymentsSum);
                }
                else if(HeadOfAccountHelpers.FundTransit.Contains(grp.Key.RoAccountType.HeadOfAccountType))
                {
                    hplnkFundTransitPreviousYear.Text = grp.PreviousYearSum.ToString(MONEY_FORMAT_SPECIFIER);
                    hplnkFundTransitForMonth.Text = grp.ForMonthSum.ToString(MONEY_FORMAT_SPECIFIER);
                    hplnkFundTransitUptoMonth.Text = (grp.UptoMonthSum + grp.ForMonthSum).ToString(MONEY_FORMAT_SPECIFIER);
                }
                else {
                if (grp.Key.RoAccountType.Category == "R" || grp.Key.RoAccountType.Category == "A" || grp.Key.RoAccountType.Category == "L")
                        {
                            SetAdditiveHyperLinkProperties(hplnkOtherPreviousYear, grp.PreviousYearSum, SumType.ReceiptsPreviousYear);
                            SetAdditiveHyperLinkProperties(hplnkOtherForMonth, grp.ForMonthSum, SumType.ReceiptsForMonth);
                            SetAdditiveHyperLinkProperties(hplnkOtheruptoMonth, grp.UptoMonthSum + grp.ForMonthSum, SumType.ReceiptsUptoMonth);

                            SetAdditiveLabelProperties(lblOthersum, grp.PreviousYearSum + grp.UptoMonthSum + grp.ForMonthSum, SumType.ReceiptsSum);
                        }
                        else if (grp.Key.RoAccountType.Category == "E")
                        {
                            SetAdditiveHyperLinkProperties(hplnkExpenditurePreviousYear, -grp.PreviousYearSum, SumType.PaymentsPreviousYear);
                            SetAdditiveHyperLinkProperties(hplnkExpenditureForMonth, -grp.ForMonthSum, SumType.PaymentsForMonth);
                            SetAdditiveHyperLinkProperties(hplnkExpenditureUptoMonth, -(grp.UptoMonthSum + grp.ForMonthSum), SumType.PaymentsUptoMonth);

                            SetAdditiveLabelProperties(lblExpendituresum, (-(grp.PreviousYearSum + grp.UptoMonthSum + grp.ForMonthSum)), SumType.PaymentsSum);
                        }
                }
                

                //switch (grp.Key.RoAccountType.HeadOfAccountType)
                //{
                //    //Opening Balances
                //    case "CASH":
                //    case "BANKNU":
                //    case "INVESTMENT":
                //        SetAdditiveHyperLinkProperties(hplnkOpBalForTheMonth, -(grp.PreviousYearSum + grp.UptoMonthSum), SumType.ReceiptsForMonth);
                //        SetAdditiveHyperLinkProperties(hplnkOpBalUptoTheMonth, -grp.PreviousYearSum,SumType.ReceiptsUptoMonth);
                //        SetAdditiveHyperLinkProperties(hplnkClBalForMonth, -(grp.PreviousYearSum + grp.UptoMonthSum + grp.ForMonthSum), SumType.test);
                //        break;

                //    case "BANKFE":
                //        SetHyperLinkProperties(hplnkOpBalForTheMonthFE, -(grp.PreviousYearSum + grp.UptoMonthSum), SumType.ReceiptsForMonth);
                //        SetHyperLinkProperties(hplnkOpBalUptoTheMonthFE, -grp.PreviousYearSum, SumType.ReceiptsUptoMonth);
                        
                //        SetHyperLinkProperties(hplnkClBalForMonthFE, -(grp.PreviousYearSum + grp.UptoMonthSum + grp.ForMonthSum), SumType.test);
                //        break;

                //    //Receipts Group Starts Here

                //    case "GRANT_RECEIVED_GOINU":
                //    case "LOAN_RECEIVED_GOINU":
                //        SetAdditiveHyperLinkProperties(hplnkGOIAidPreviousYear, grp.PreviousYearSum, SumType.ReceiptsPreviousYear);
                //        SetAdditiveHyperLinkProperties(hplnkGOIAidForMonth, grp.ForMonthSum, SumType.ReceiptsForMonth);
                //        SetAdditiveHyperLinkProperties(hplnkGOIAidUptoMonth, (grp.UptoMonthSum + grp.ForMonthSum), SumType.ReceiptsUptoMonth);

                //        SetAdditiveLabelProperties(lblGOIAidsum, grp.PreviousYearSum + grp.ForMonthSum + grp.UptoMonthSum, SumType.ReceiptsSum);
                //        break;

                //    case "GRANT_RECEIVED_GOIFE":
                //    case "LOAN_RECEIVED_GOIFE":
                //        SetAdditiveHyperLinkProperties(hplnkGOIFEPreviousYear, grp.PreviousYearSum, SumType.ReceiptsPreviousYear);
                //        SetAdditiveHyperLinkProperties(hplnkGOIFEForMonth, grp.ForMonthSum, SumType.ReceiptsForMonth);
                //        SetAdditiveHyperLinkProperties(hplnkGOIFEUptoMonth, (grp.UptoMonthSum + grp.ForMonthSum), SumType.ReceiptsUptoMonth);

                //        SetAdditiveLabelProperties(lblGOIFEsum, (grp.PreviousYearSum + grp.UptoMonthSum + grp.ForMonthSum), SumType.ReceiptsSum);
                //        break;

                //    case "INTEREST":
                //        SetHyperLinkProperties(hplnkInterestPreviousYear, grp.PreviousYearSum, SumType.ReceiptsPreviousYear);
                //        SetHyperLinkProperties(hplnkInterestForMonth, grp.ForMonthSum, SumType.ReceiptsForMonth);
                //        SetHyperLinkProperties(hplnkInterestUptoMonth, (grp.UptoMonthSum + grp.ForMonthSum), SumType.ReceiptsUptoMonth);

                //        SetLabelProperties(lblInterestsum,(grp.PreviousYearSum + grp.UptoMonthSum + grp.ForMonthSum),SumType.ReceiptsSum);
                //        break;

                //    case "SALARY_REMITANCES":
                //        SetHyperLinkProperties(hplnkRecoveryPreviousYear, grp.PreviousYearSum, SumType.ReceiptsPreviousYear);
                //        SetHyperLinkProperties(hplnkRecoveryForMonth, grp.ForMonthSum, SumType.ReceiptsForMonth);
                //        SetHyperLinkProperties(hplnkRecoveryUptoMonth, (grp.UptoMonthSum + grp.ForMonthSum), SumType.ReceiptsUptoMonth);
                        
                //        SetLabelProperties(lblRecoverysum, grp.PreviousYearSum + grp.ForMonthSum + grp.UptoMonthSum,SumType.ReceiptsSum);
                //        break;

                //    case "TENDER_SALE":
                //        SetHyperLinkProperties(hplnkTenderSalePreviousYear, grp.PreviousYearSum, SumType.ReceiptsPreviousYear);
                //        SetHyperLinkProperties(hplnkTenderSaleForMonth, grp.ForMonthSum, SumType.ReceiptsForMonth);
                //        SetHyperLinkProperties(hplnkTenderSaleUptoMonth, (grp.UptoMonthSum + grp.ForMonthSum), SumType.ReceiptsUptoMonth);
                        
                //        SetLabelProperties(lblTenderSalesum, grp.PreviousYearSum + grp.UptoMonthSum + grp.ForMonthSum, SumType.ReceiptsSum);
                //        break;

                //    case "EMD":
                //        SetHyperLinkProperties(hplnkEMDPreviousYear, grp.PreviousYearSum, SumType.ReceiptsPreviousYear);
                //        SetHyperLinkProperties(hplnkEMDForMonth, grp.ForMonthSum, SumType.ReceiptsForMonth);
                //        SetHyperLinkProperties(hplnkEMDUptoMonth, (grp.UptoMonthSum + grp.ForMonthSum), SumType.ReceiptsUptoMonth);
                        
                //        SetLabelProperties(lblEMDsum, grp.PreviousYearSum + grp.UptoMonthSum + grp.ForMonthSum, SumType.ReceiptsSum);
                //        break;

                //    case "SD":
                //        SetHyperLinkProperties(hplnkSecurityDepositPreviousYear, grp.PreviousYearSum, SumType.ReceiptsPreviousYear);
                //        SetHyperLinkProperties(hplnkSecurityDepositForMonth, grp.ForMonthSum, SumType.ReceiptsForMonth);
                //        SetHyperLinkProperties(hplnkSecurityDepositUptoMonth, (grp.UptoMonthSum + grp.ForMonthSum), SumType.ReceiptsUptoMonth);
                        
                //        SetLabelProperties(lblSecurityDepositsum, (grp.PreviousYearSum + grp.UptoMonthSum + grp.ForMonthSum), SumType.ReceiptsSum);
                //        break;

                //    case "BIT":
                //        SetHyperLinkProperties(hplnkBITPreviousYear, grp.PreviousYearSum, SumType.ReceiptsPreviousYear);
                //        SetHyperLinkProperties(hplnkBITForMonth, grp.ForMonthSum, SumType.ReceiptsForMonth);
                //        SetHyperLinkProperties(hplnkBITUptoMonth, (grp.UptoMonthSum + grp.ForMonthSum), SumType.ReceiptsUptoMonth);
                        
                //        SetLabelProperties(lblBITsum, (grp.PreviousYearSum + grp.UptoMonthSum + grp.ForMonthSum), SumType.ReceiptsSum);
                //        break;


                //    // Payments Groups Starts Here

                //    case "EMPLOYEE_ADVANCE":
                //        SetHyperLinkProperties(hplnkEmpAdvPreviousYear, -grp.PreviousYearSum, SumType.PaymentsPreviousYear);
                //        SetHyperLinkProperties(hplnkEmpAdvForMonth, -grp.ForMonthSum, SumType.PaymentsForMonth);
                //        SetHyperLinkProperties(hplnkEmpAdvUptoMonth, (-(grp.UptoMonthSum + grp.ForMonthSum)), SumType.PaymentsUptoMonth);

                //        SetLabelProperties(lblEmpAdvsum,(-(grp.PreviousYearSum + grp.UptoMonthSum + grp.ForMonthSum)),SumType.PaymentsSum);
                //        break;

                //    case "PARTY_ADVANCE":
                //    case "MATERIAL_ADVANCE":
                //        SetAdditiveHyperLinkProperties(hplnkContAdvPreviousYear, -grp.PreviousYearSum,SumType.PaymentsPreviousYear);
                //        SetAdditiveHyperLinkProperties(hplnkContAdvForMonth, -grp.ForMonthSum,SumType.PaymentsForMonth);
                //        SetAdditiveHyperLinkProperties(hplnkContAdvUptoMonth, -(grp.UptoMonthSum + grp.ForMonthSum),SumType.PaymentsUptoMonth);

                //        SetAdditiveLabelProperties(lblContAdvsum, (-(grp.PreviousYearSum + grp.UptoMonthSum + grp.ForMonthSum)), SumType.PaymentsSum);
                //        break;

                //    case "STOCK_SUSPENSE":
                //        SetHyperLinkProperties(hplnkStockSuspensePreviousYear, -grp.PreviousYearSum, SumType.PaymentsPreviousYear);
                //        SetHyperLinkProperties(hplnkStockSuspenseForMonth, -grp.ForMonthSum, SumType.PaymentsForMonth);
                //        SetHyperLinkProperties(hplnkStockSuspenseUptoMonth, (-(grp.UptoMonthSum + grp.ForMonthSum)), SumType.PaymentsUptoMonth);

                //        SetLabelProperties(lblStockSuspensesum, (-(grp.UptoMonthSum + grp.ForMonthSum + grp.PreviousYearSum)), SumType.PaymentsSum);
                //        break;

                //    case "BST":
                //        SetHyperLinkProperties(hplnkBSTPreviousYear, -grp.PreviousYearSum, SumType.PaymentsPreviousYear);
                //        SetHyperLinkProperties(hplnkBSTForMonth, -grp.ForMonthSum, SumType.PaymentsForMonth);
                //        SetHyperLinkProperties(hplnkBSTUptoMonth, (-(grp.UptoMonthSum + grp.ForMonthSum)), SumType.PaymentsUptoMonth);
                //        SetLabelProperties(lblBSTsum,(-(grp.UptoMonthSum + grp.ForMonthSum + grp.PreviousYearSum)),SumType.PaymentsSum);
                //        break;

                //    case "SVCTAX":
                //        SetHyperLinkProperties(hplnksvctaxPreviousYear, -grp.PreviousYearSum, SumType.PaymentsPreviousYear);
                //        SetHyperLinkProperties(hplnksvctaxForMonth, -grp.ForMonthSum, SumType.PaymentsForMonth);
                //        SetHyperLinkProperties(hplnksvctaxUptoMonth, (-(grp.UptoMonthSum + grp.ForMonthSum)), SumType.PaymentsUptoMonth);
                //        SetLabelProperties(lblsvctaxsum, (-(grp.UptoMonthSum + grp.ForMonthSum + grp.PreviousYearSum)), SumType.PaymentsSum);
                //        break;

                //    case "EDGOI":
                //        SetHyperLinkProperties(hplnkExciseDutyGOIPreviousYear, -grp.PreviousYearSum, SumType.PaymentsPreviousYear);
                //        SetHyperLinkProperties(hplnkExciseDutyGOIForMonth, -grp.ForMonthSum, SumType.PaymentsForMonth);
                //        SetHyperLinkProperties(hplnkExciseDutyGOIUptoMonth, (-(grp.UptoMonthSum + grp.ForMonthSum)), SumType.PaymentsUptoMonth);

                //        SetLabelProperties(lblExciseDutyGOIsum, (-(grp.UptoMonthSum + grp.ForMonthSum + grp.PreviousYearSum)), SumType.PaymentsSum);
                //        break;

                //    case "EDRGOB":
                //        SetHyperLinkProperties(hplnkExciseDutyRGOBPreviousYear, -grp.PreviousYearSum, SumType.PaymentsPreviousYear);
                //        SetHyperLinkProperties(hplnkExciseDutyRGOBForMonth, -grp.ForMonthSum, SumType.PaymentsForMonth);
                //        SetHyperLinkProperties(hplnkExciseDutyRGOBUptoMonth, (-(grp.UptoMonthSum + grp.ForMonthSum)), SumType.PaymentsUptoMonth);

                //        SetLabelProperties(lblExciseDutyRGOBsum, (-(grp.UptoMonthSum + grp.ForMonthSum + grp.PreviousYearSum)), SumType.PaymentsSum);
                //        break;

                //    case "GREEN_TAX":
                //        SetHyperLinkProperties(hplnkGreenTaxPreviousYear, -grp.PreviousYearSum, SumType.PaymentsPreviousYear);
                //        SetHyperLinkProperties(hplnkGreenTaxForMonth, -grp.ForMonthSum, SumType.PaymentsForMonth);
                //        SetHyperLinkProperties(hplnkGreenTaxUptoMonth, (-(grp.UptoMonthSum + grp.ForMonthSum)), SumType.PaymentsUptoMonth);
                //        SetLabelProperties(lblGreenTaxRGOBsum, (-(grp.UptoMonthSum + grp.ForMonthSum + grp.PreviousYearSum)), SumType.PaymentsSum);
                //        break;

                //    //Funds Transit Group

                //    case "FUNDS_TRANSIT":
                //        hplnkFundTransitPreviousYear.Text = grp.PreviousYearSum.ToString(MONEY_FORMAT_SPECIFIER);
                //        hplnkFundTransitForMonth.Text = grp.ForMonthSum.ToString(MONEY_FORMAT_SPECIFIER);
                //        hplnkFundTransitUptoMonth.Text = (grp.UptoMonthSum + grp.ForMonthSum).ToString(MONEY_FORMAT_SPECIFIER);
                //        break;

                //    default:

                //        if (grp.Key.RoAccountType.Category == "R" || grp.Key.RoAccountType.Category == "A" || grp.Key.RoAccountType.Category == "L")
                //        {
                //            SetAdditiveHyperLinkProperties(hplnkOtherPreviousYear, grp.PreviousYearSum, SumType.ReceiptsPreviousYear);
                //            SetAdditiveHyperLinkProperties(hplnkOtherForMonth, grp.ForMonthSum, SumType.ReceiptsForMonth);
                //            SetAdditiveHyperLinkProperties(hplnkOtheruptoMonth, grp.UptoMonthSum + grp.ForMonthSum, SumType.ReceiptsUptoMonth);

                //            SetAdditiveLabelProperties(lblOthersum, grp.PreviousYearSum + grp.UptoMonthSum + grp.ForMonthSum, SumType.ReceiptsSum);
                //        }
                //        else if (grp.Key.RoAccountType.Category == "E")
                //        {
                //            SetAdditiveHyperLinkProperties(hplnkExpenditurePreviousYear, -grp.PreviousYearSum, SumType.PaymentsPreviousYear);
                //            SetAdditiveHyperLinkProperties(hplnkExpenditureForMonth, -grp.ForMonthSum, SumType.PaymentsForMonth);
                //            SetAdditiveHyperLinkProperties(hplnkExpenditureUptoMonth, -(grp.UptoMonthSum + grp.ForMonthSum), SumType.PaymentsUptoMonth);

                //            SetAdditiveLabelProperties(lblExpendituresum, (-(grp.PreviousYearSum + grp.UptoMonthSum + grp.ForMonthSum)), SumType.PaymentsSum);
                //        }
                //        break;   
                //}
            }

            // Update the copy cat properties
            

            // Update hyperlink and label amounts
            foreach (KeyValuePair<Control, decimal> kvp in m_controlAmounts)
            {
                HyperLink hl = kvp.Key as HyperLink;
                if (hl == null)
                {
                    // Must be label
                    Label lbl = (Label)kvp.Key;
                    lbl.Text = kvp.Value.ToString(MONEY_FORMAT_SPECIFIER);
                }
                else
                {
                    hl.Text = kvp.Value.ToString(MONEY_FORMAT_SPECIFIER);
                }
            }

            hplnkClBalPreviousYear.Text = hplnkOpBalUptoTheMonth.Text;
            hplnkClBalPreviousYear.NavigateUrl = hplnkOpBalUptoTheMonth.NavigateUrl;

            hplnkClBalUptoMonth.Text = hplnkClBalForMonth.Text;
            hplnkClBalUptoMonth.NavigateUrl = hplnkClBalForMonth.NavigateUrl;

            hplnkClBalUptoMonthFE.Text = hplnkClBalForMonthFE.Text;
            hplnkClBalUptoMonthFE.NavigateUrl = hplnkClBalForMonthFE.NavigateUrl;

            hplnkClBalPreviousYearFE.Text = hplnkOpBalUptoTheMonthFE.Text;
            hplnkClBalPreviousYearFE.NavigateUrl = hplnkClBalForMonth.NavigateUrl;

            //hplnkOpBalUptoTheMonthFE.Text = hplnkClBalForMonthFE.Text;
            //hplnkOpBalUptoTheMonthFE.NavigateUrl = hplnkClBalForMonthFE.NavigateUrl;

            lblClBalSum.Text = hplnkClBalForMonth.Text;
            lblClBalFESum.Text = hplnkClBalForMonthFE.Text;

            return;
        }

        /// <summary>
        /// Make all NavigateUrls reasonable
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
  
            // Using an arbitrary hyperlink to determine the container of all hyperlinks
            foreach (Control ctl in hplnkClBalUptoMonth.NamingContainer.Controls)
            {
                HyperLink hl = ctl as HyperLink;
                if (hl != null)
                {
                    if (hl.Text.Trim().Length == 0)
                    {
                        hl.NavigateUrl = string.Empty;
                    }
                    else
                    {
                        hl.NavigateUrl = string.Format(hl.NavigateUrl, m_dtPreviousYear,
                            m_dtMonthStart, m_dtMonthEnd,m_dtPreviousYearEnd,m_dtMonthStart.AddDays(-1));
                    }
                }
            }

            GetTotal();
        }

        private Dictionary<SumType, decimal> m_totals = new Dictionary<SumType, decimal>();

        /// <summary>
        /// The amount to be set as the text of the control
        /// </summary>
        private Dictionary<Control, decimal> m_controlAmounts = new Dictionary<Control, decimal>();

        /// <summary>
        /// If amount is not 0, sets amount as the text of the hyperlink and url as the passed url. Also maintains the sum.
        /// The passed hyperlink already has text set, then the amount is added to its text.
        /// </summary>
        /// <param name="hl"></param>
        /// <param name="amount"></param>
        /// <param name="url"></param>
        /// <param name="sumType"></param>
        /// 
        private void SetHyperLinkProperties(HyperLink hl, decimal? amount, SumType sumType)
        {
            if (amount.HasValue && amount != 0)
            {
                SetControlAmount(hl, amount.Value, false);
                UpdateTotals(amount.Value, sumType);
            }
        }

        private void SetAdditiveHyperLinkProperties(HyperLink hl, decimal? amount, SumType sumType)
        {
            if (amount.HasValue && amount != 0)
            {
                SetControlAmount(hl, amount.Value, true);
                UpdateTotals(amount.Value, sumType);
            }
        }

        private void SetControlAmount(Control ctl, decimal amount, bool allowAddition)
        {
            if (m_controlAmounts.ContainsKey(ctl))
            {
                if (!allowAddition)
                {
                    throw new InvalidOperationException("The amount for this control cannot be set multiple times");
                }
                m_controlAmounts[ctl] += amount;
            }
            else
            {
                m_controlAmounts.Add(ctl, amount);
            }
        }

        private void UpdateTotals(decimal amount, SumType sumType)
        {
            if (m_totals.ContainsKey(sumType))
            {
                m_totals[sumType] += amount;
            }
            else
            {
                m_totals.Add(sumType, amount);
            }
        }

        private void SetLabelProperties(Label lbl, decimal? amount, SumType sumType)
        {
            if (amount.HasValue && amount != 0)
            {
                SetControlAmount(lbl, amount.Value, false);
                UpdateTotals(amount.Value, sumType);
            }
        }

        private void SetAdditiveLabelProperties(Label lbl, decimal? amount, SumType sumType)
        {
            if (amount.HasValue && amount != 0)
            {
                SetControlAmount(lbl, amount.Value, true);
                UpdateTotals(amount.Value, sumType);
            }
        }


        /// <summary>
        /// Displays the sum according to the SumType attribute associated with the label
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbl_PreRenderShowSum(object sender, EventArgs e)
        {
            Label lbl = (Label)sender;
            SumType totalType = (SumType)Enum.Parse(typeof(SumType), lbl.Attributes["SumType"]);
            if (m_totals.ContainsKey(totalType))
            {
                decimal d = m_totals[totalType];
                lbl.Text = d.ToString(MONEY_FORMAT_SPECIFIER);
            }
        }

        private enum ClSum
        {
            ClPreviousYear,
            ClForMonth,
            ClUptoMonth,
            ClCummulative
        }

        Dictionary<ClSum, decimal> dictClSum = new Dictionary<ClSum, decimal>();        
        private void UpdateClTotals(decimal amount, ClSum sumType)
        {
            if (dictClSum.ContainsKey(sumType))
            {
                dictClSum[sumType] += amount;
            }
            else
            {
                dictClSum.Add(sumType, amount);
            }
        }

        /// <summary>
        /// Displays closing balances for any period 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GetTotal()
        {
            foreach (KeyValuePair<Control, decimal> kvp in m_controlAmounts)
            {
                switch (kvp.Key.ID)
                {
                    case "hplnkClBalForMonth":
                    case "hplnkClBalForMonthFE":
                        UpdateClTotals(kvp.Value, ClSum.ClForMonth);
                        UpdateClTotals(kvp.Value, ClSum.ClUptoMonth);
                        UpdateClTotals(kvp.Value, ClSum.ClCummulative);
                        break;

                    case "hplnkOpBalUptoTheMonth":
                    case "hplnkOpBalUptoTheMonthFE":
                        UpdateClTotals(kvp.Value, ClSum.ClPreviousYear);
                        break;
                }
            }

            foreach (KeyValuePair<SumType, decimal> kvp in m_totals)
            {
                switch (kvp.Key)
                {
                    case SumType.PaymentsForMonth:
                        UpdateClTotals(kvp.Value, ClSum.ClForMonth);
                        break;

                    case SumType.PaymentsUptoMonth:
                        UpdateClTotals(kvp.Value, ClSum.ClUptoMonth);
                        break;

                    case SumType.PaymentsPreviousYear:
                        UpdateClTotals(kvp.Value, ClSum.ClPreviousYear);
                        break;

                    case SumType.PaymentsSum:
                        UpdateClTotals(kvp.Value, ClSum.ClCummulative);
                        break;
                }
            }

            foreach (KeyValuePair<ClSum, decimal> kvp in dictClSum)
            {
                switch (kvp.Key)
                {
                    case ClSum.ClForMonth:
                        lblClosingBalanceForMonth.Text = kvp.Value.ToString(MONEY_FORMAT_SPECIFIER);
                        break;

                    case ClSum.ClUptoMonth:
                        lblClosingBalanceUptoMonth.Text = kvp.Value.ToString(MONEY_FORMAT_SPECIFIER);
                        break;

                    case ClSum.ClPreviousYear:
                        lblClosingBalancePreviousYear.Text = kvp.Value.ToString(MONEY_FORMAT_SPECIFIER);
                        break;

                    case ClSum.ClCummulative:
                        lblCummulativeClosingBalance.Text = kvp.Value.ToString(MONEY_FORMAT_SPECIFIER);
                        break;
                }
            }
        }
    }
}
