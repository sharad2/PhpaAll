/*
 *  Programming: Eclipse Systems (P) Ltd., NOIDA, INDIA
 *  E-mail: support@eclsys.com
 *
 *  $Workfile:   FundPositionReport.aspx.cs  $
 *  $Revision: 704 $
 *  $Author: hsingh $
 *  $Date: 2014-05-13 16:35:26 +0530 (Tue, 13 May 2014) $
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
    public partial class FundPositionReport : PageBase
    {
        private ReportingDataContext m_db;
        protected DateTime m_dtPreviousYear;
        protected DateTime m_dtPreviousYearEnd;
        protected DateTime m_dtMonthStart;
        protected DateTime m_dtMonthEnd;
        decimal _otherBankAmount = 0;
        protected string valdate = string.Empty;
        private IQueryable<ResultItem> m_query;
        string otherBankAmount = string.Empty;

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
                this.Title = string.Format("Fund Position as on - {0:dd/MM/yyyy}", date.Value);
                this.DataBind();
            }

            base.OnLoad(e);
        }

        const string MONEY_FORMAT_SPECIFIER = "#,###,,.000;(#,###,,.000);'&nbsp;'";
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
            test,
            FundReceivedPreviousYear,
            FundReceivedUptoMonth,
            FundSum,
            BalanceFund
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

            //Getting sum of amount ahainst headofaccount type
            var query = (from vd in m_db.RoVoucherDetails
                         where vd.RoVoucher.VoucherId == vd.VoucherId
                                && vd.HeadOfAccount.HeadOfAccountId != null
                                && vd.HeadOfAccount.RoAccountType != null
                         group vd by new
                         {
                             vd.HeadOfAccount.RoAccountType
                         }
                             into grouping
                             select new
                             {
                                 grouping.Key,
                                 PreviousYearSum = grouping.Sum(hoa => hoa.RoVoucher.VoucherDate < m_dtPreviousYear ? (hoa.CreditAmount ?? 0 - hoa.DebitAmount ?? 0) : 0),
                                 ForMonthSum = grouping.Sum(hoa => hoa.RoVoucher.VoucherDate >= m_dtMonthStart && hoa.RoVoucher.VoucherDate <= m_dtMonthEnd ? hoa.CreditAmount ?? 0 - hoa.DebitAmount ?? 0 : 0),
                                 UptoMonthSum = grouping.Sum(hoa => hoa.RoVoucher.VoucherDate >= m_dtPreviousYear && hoa.RoVoucher.VoucherDate < m_dtMonthStart ? hoa.CreditAmount ?? 0 - hoa.DebitAmount ?? 0 : 0)
                             });

            //Getting sum of amount against headofaccountId 
            var query1 = (from vd in m_db.RoVoucherDetails
                          group vd by vd.RoHeadHierarchy into grouping
                          select new
                          {
                              grouping.Key,
                              PreviousYearSum = grouping.Sum(hoa => hoa.RoVoucher.VoucherDate < m_dtPreviousYear ? (hoa.CreditAmount ?? 0 - hoa.DebitAmount ?? 0) : 0),
                              ForMonthSum = grouping.Sum(hoa => hoa.RoVoucher.VoucherDate >= m_dtMonthStart && hoa.RoVoucher.VoucherDate <= m_dtMonthEnd ? hoa.CreditAmount ?? 0 - hoa.DebitAmount ?? 0 : 0),
                              UptoMonthSum = grouping.Sum(hoa => hoa.RoVoucher.VoucherDate >= m_dtPreviousYear && hoa.RoVoucher.VoucherDate < m_dtMonthStart ? hoa.CreditAmount ?? 0 - hoa.DebitAmount ?? 0 : 0)
                          });
            foreach (var grp in query)
            {
                switch (grp.Key.RoAccountType.HeadOfAccountType)
                {
                    // Funds received from the Govt. of india. 
                    case "GRANT_RECEIVED_GOINU":
                    case "LOAN_RECEIVED_GOINU":
                        SetAdditiveHyperLinkProperties(hplnkGOIAidPreviousYear, grp.PreviousYearSum, SumType.ReceiptsPreviousYear);
                        SetAdditivePropertiesFund(grp.PreviousYearSum, SumType.FundReceivedPreviousYear);
                        SetAdditiveHyperLinkProperties(hplnkGOIAidUptoMonth, (grp.UptoMonthSum + grp.ForMonthSum), SumType.ReceiptsUptoMonth);
                        SetAdditivePropertiesFund((grp.UptoMonthSum + grp.ForMonthSum), SumType.FundReceivedUptoMonth);
                        SetAdditiveLabelProperties(lblGOIAidsum, grp.PreviousYearSum + grp.ForMonthSum + grp.UptoMonthSum, SumType.ReceiptsSum);
                        SetAdditivePropertiesFund(grp.PreviousYearSum + grp.ForMonthSum + grp.UptoMonthSum, SumType.FundSum);
                        break;
                    // Funds received from the Govt. of India in foreign exchange.
                    case "GRANT_RECEIVED_GOIFE":
                    case "LOAN_RECEIVED_GOIFE":
                        SetAdditiveHyperLinkProperties(hplnkGOIFEPreviousYear, grp.PreviousYearSum, SumType.ReceiptsPreviousYear);
                        SetAdditivePropertiesFund(grp.PreviousYearSum, SumType.FundReceivedPreviousYear);
                        SetAdditiveHyperLinkProperties(hplnkGOIFEUptoMonth, (grp.UptoMonthSum + grp.ForMonthSum), SumType.ReceiptsUptoMonth);
                        SetAdditivePropertiesFund((grp.UptoMonthSum + grp.ForMonthSum), SumType.FundReceivedUptoMonth);
                        SetAdditiveLabelProperties(lblGOIFEsum, (grp.PreviousYearSum + grp.UptoMonthSum + grp.ForMonthSum), SumType.ReceiptsSum);
                        SetAdditivePropertiesFund(grp.PreviousYearSum + grp.ForMonthSum + grp.UptoMonthSum, SumType.FundSum);
                        break;
                    //  Funds received from Other Receipts.
                    case "INTEREST":
                    case "SALARY_REMITANCES":
                    case "TENDER_SALE":
                    case "EMD":
                    case "SD":
                    case "BIT":
                        SetAdditiveHyperLinkProperties(hplnkReceiptsPreviousYear, grp.PreviousYearSum, SumType.ReceiptsPreviousYear);
                        SetAdditiveHyperLinkProperties(hplnkReceiptsUptoMonth, (grp.UptoMonthSum + grp.ForMonthSum), SumType.ReceiptsUptoMonth);
                        SetAdditiveLabelProperties(lblReceiptssum, (grp.PreviousYearSum + grp.UptoMonthSum + grp.ForMonthSum), SumType.ReceiptsSum);
                        break;
                    // Payments Groups Starts Here
                    // Need to handle these cases and do nothing otherwise amount of these accountypes is added in others repeipts(default case).
                    case "EMPLOYEE_ADVANCE":
                        break;

                    case "PARTY_ADVANCE":

                        break;
                    case "MATERIAL_ADVANCE":
                        break;

                    case "STOCK_SUSPENSE":
                        break;

                    case "BST":
                        break;

                    case "SVCTAX":
                        break;

                    case "EDGOI":
                        break;

                    case "EDRGOB":
                        break;
                    case "FUNDS_TRANSIT":
                        break;
                    case "GREEN_TAX":
                        break;
                    default:
                        if (grp.Key.RoAccountType.Category == "R" || grp.Key.RoAccountType.Category == "A" || grp.Key.RoAccountType.Category == "L")
                        {
                            SetAdditiveHyperLinkProperties(hplnkReceiptsPreviousYear, grp.PreviousYearSum, SumType.ReceiptsPreviousYear);
                            SetAdditiveHyperLinkProperties(hplnkReceiptsUptoMonth, (grp.UptoMonthSum + grp.ForMonthSum), SumType.ReceiptsUptoMonth);
                            SetAdditiveLabelProperties(lblReceiptssum, grp.PreviousYearSum + grp.UptoMonthSum + grp.ForMonthSum, SumType.ReceiptsSum);
                        }
                        break;
                }
            }
            foreach (var grp in query1)
            {
                switch (grp.Key.TopParentName)
                {
                    // Calculating expenditure for project establishment cost(Including WAPCOS).
                    case 100:
                        SetAdditiveHyperLinkProperties(hplnkEstablishExpenditurePreviousYear, -grp.PreviousYearSum, SumType.PaymentsPreviousYear);
                        SetAdditiveHyperLinkProperties(hplnkEstablishExpenditureUptoMonth, -(grp.UptoMonthSum + grp.ForMonthSum), SumType.PaymentsUptoMonth);
                        SetAdditiveLabelProperties(lblEstablishExpendituresum, (-(grp.PreviousYearSum + grp.UptoMonthSum + grp.ForMonthSum)), SumType.PaymentsSum);
                        SetAdditivePropertiesFund(-grp.ForMonthSum, SumType.PaymentsForMonth);
                        break;
                    // Calculating expenditure for Civil works.
                    case 200:
                        SetAdditiveHyperLinkProperties(hplnkCivilExpenditurePreviousYear, -grp.PreviousYearSum, SumType.PaymentsPreviousYear);
                        SetAdditiveHyperLinkProperties(hplnkCivilExpenditureUpToMonth, -(grp.UptoMonthSum + grp.ForMonthSum), SumType.PaymentsUptoMonth);
                        SetAdditiveLabelProperties(lblCivilExpendituresum, (-(grp.PreviousYearSum + grp.UptoMonthSum + grp.ForMonthSum)), SumType.PaymentsSum);
                        SetAdditivePropertiesFund(-grp.ForMonthSum, SumType.PaymentsForMonth);
                        break;
                    // Calculating expenditure for Electrical works.
                    case 300:
                        SetAdditiveHyperLinkProperties(hplnkElectricalExpenditurePreviousYear, -grp.PreviousYearSum, SumType.PaymentsPreviousYear);
                        SetAdditiveHyperLinkProperties(hplnkElectricalExpenditureUpToMonth, -(grp.UptoMonthSum + grp.ForMonthSum), SumType.PaymentsUptoMonth);
                        SetAdditiveLabelProperties(lblElectricalExpendituresum, (-(grp.PreviousYearSum + grp.UptoMonthSum + grp.ForMonthSum)), SumType.PaymentsSum);
                        SetAdditivePropertiesFund(-grp.ForMonthSum, SumType.PaymentsForMonth);
                        break;
                    // Calculating expenditure for Transmission works.
                    case 400:
                        SetAdditiveHyperLinkProperties(hplnkTransmissionExpenditurePreviousYear, -grp.PreviousYearSum, SumType.PaymentsPreviousYear);
                        SetAdditiveHyperLinkProperties(hplnkTransmissionExpenditureUpToMonth, -(grp.UptoMonthSum + grp.ForMonthSum), SumType.PaymentsUptoMonth);
                        SetAdditiveLabelProperties(lblTransmissionExpendituresum, (-(grp.PreviousYearSum + grp.UptoMonthSum + grp.ForMonthSum)), SumType.PaymentsSum);
                        SetAdditivePropertiesFund(-grp.ForMonthSum, SumType.PaymentsForMonth);
                        break;
                    // Calculating Non-Budgetary Heads.
                    case 800:
                        // Calculating on the basis of either mobilisation/secured Advance Estt and service tax and Employees Advance of establishment expenditure.
                        if (grp.Key.HeadOfAccountId == 1382 || grp.Key.HeadOfAccountType == "SVCTAX" || grp.Key.HeadOfAccountType == "EMPLOYEE_ADVANCE")
                        {
                            SetAdditiveHyperLinkProperties(hplnkEstablishExpenditurePreviousYear, -grp.PreviousYearSum, SumType.PaymentsPreviousYear);
                            SetAdditiveHyperLinkProperties(hplnkEstablishExpenditureUptoMonth, -(grp.UptoMonthSum + grp.ForMonthSum), SumType.PaymentsUptoMonth);
                            SetAdditiveLabelProperties(lblEstablishExpendituresum, (-(grp.PreviousYearSum + grp.UptoMonthSum + grp.ForMonthSum)), SumType.PaymentsSum);
                            SetAdditivePropertiesFund(-grp.ForMonthSum, SumType.PaymentsForMonth);
                        }
                        // Adding Mobilisation/Secured Advance (E&M Packages)  of Electrical expenditure.
                        if (grp.Key.HeadOfAccountId == 1593)
                        {
                            SetAdditiveHyperLinkProperties(hplnkElectricalExpenditurePreviousYear, -grp.PreviousYearSum, SumType.PaymentsPreviousYear);
                            SetAdditiveHyperLinkProperties(hplnkElectricalExpenditureUpToMonth, -(grp.UptoMonthSum + grp.ForMonthSum), SumType.PaymentsUptoMonth);
                            SetAdditiveLabelProperties(lblElectricalExpendituresum, (-(grp.PreviousYearSum + grp.UptoMonthSum + grp.ForMonthSum)), SumType.PaymentsSum);
                            SetAdditivePropertiesFund(-grp.ForMonthSum, SumType.PaymentsForMonth);
                        }
                        // Adding Mobilisation/Secured Advance of Transmission.
                        if (grp.Key.HeadOfAccountId == 1594)
                        {
                            SetAdditiveHyperLinkProperties(hplnkTransmissionExpenditurePreviousYear, -grp.PreviousYearSum, SumType.PaymentsPreviousYear);
                            SetAdditiveHyperLinkProperties(hplnkTransmissionExpenditureUpToMonth, -(grp.UptoMonthSum + grp.ForMonthSum), SumType.PaymentsUptoMonth);
                            SetAdditiveLabelProperties(lblTransmissionExpendituresum, (-(grp.PreviousYearSum + grp.UptoMonthSum + grp.ForMonthSum)), SumType.PaymentsSum);
                            SetAdditivePropertiesFund(-grp.ForMonthSum, SumType.PaymentsForMonth);
                        }
                        // Adding Mobilisation Advance Civil and Mobilisation Advance (HRT) and Green Tax (RGoB) and Mobilisation Advance (PH) and Mobilisation Advance (DAM) and Stock Suspense and BST and Excise Duty and for Material_Advance head of account type.
                        if (grp.Key.HeadOfAccountId == 1381 || grp.Key.HeadOfAccountId == 1686 || grp.Key.HeadOfAccountId == 1687 || grp.Key.HeadOfAccountId == 1691 || grp.Key.HeadOfAccountId == 1383 || grp.Key.HeadOfAccountId == 1395 || grp.Key.HeadOfAccountId == 1397 || grp.Key.HeadOfAccountType == "MATERIAL_ADVANCE" || grp.Key.HeadOfAccountId == 1384 || grp.Key.HeadOfAccountId == 1385 || grp.Key.HeadOfAccountId == 1386 || grp.Key.HeadOfAccountId == 1387 || grp.Key.HeadOfAccountId == 1388 || grp.Key.HeadOfAccountId == 1389 || grp.Key.HeadOfAccountId == 1769)
                        {
                            SetAdditiveHyperLinkProperties(hplnkCivilExpenditurePreviousYear, -grp.PreviousYearSum, SumType.PaymentsPreviousYear);
                            SetAdditiveHyperLinkProperties(hplnkCivilExpenditureUpToMonth, -(grp.UptoMonthSum + grp.ForMonthSum), SumType.PaymentsUptoMonth);
                            SetAdditiveLabelProperties(lblCivilExpendituresum, (-(grp.PreviousYearSum + grp.UptoMonthSum + grp.ForMonthSum)), SumType.PaymentsSum);
                            SetAdditivePropertiesFund(-grp.ForMonthSum, SumType.PaymentsForMonth);
                        }
                        break;
                }
            }

            // Update hyperlink and label amounts
            foreach (KeyValuePair<Control, decimal> kvp in m_controlAmounts)
            {
                HyperLink hl = kvp.Key as HyperLink;
                if (hl == null)
                {
                    // Must be label
                    Label lbl = (Label)kvp.Key;
                    lbl.Text = DisplayInMillion(kvp.Value);
                }
                else
                {
                    hl.Text = DisplayInMillion(kvp.Value);
                }
            }
            ltrlCurrentMonth.Text = string.Empty;
            DateTime dateval = Convert.ToDateTime(dttbreceiptpayment.Text);
            valdate = dateval.ToString("Y");

            ltrlCurrentMonth.Text = "Expenditure for the Month of " + valdate + " (till date)";

            //Sequence of queries does maater..as we want grid view to be rendered in the end.
            // Getting fund in banks as on date
            var query3 = (from vd in m_db.RoVoucherDetails
                          join hh in m_db.RoHeadHierarchies on vd.HeadOfAccountId equals hh.HeadOfAccountId
                          where hh.DisplayName.StartsWith("900.04.") && vd.HeadOfAccount.Created <= Convert.ToDateTime(dttbreceiptpayment.Text)
                          //Comment out this Code as now We are showing the BOB Thimphu 
                          //&& vd.HeadOfAccountId != 1415
                          group vd by vd.HeadOfAccountId into grouping
                          select new
                          {
                              BankName = grouping.Max(p => p.HeadOfAccount.Description),
                              Balance = (-(grouping.Sum(hoa => hoa.RoVoucher.VoucherDate < m_dtPreviousYear ? (hoa.CreditAmount ?? 0 - hoa.DebitAmount ?? 0) : 0) + grouping.Sum(hoa => hoa.RoVoucher.VoucherDate >= m_dtMonthStart && hoa.RoVoucher.VoucherDate <= m_dtMonthEnd ? hoa.CreditAmount ?? 0 - hoa.DebitAmount ?? 0 : 0) + grouping.Sum(hoa => hoa.RoVoucher.VoucherDate >= m_dtPreviousYear && hoa.RoVoucher.VoucherDate < m_dtMonthStart ? hoa.CreditAmount ?? 0 - hoa.DebitAmount ?? 0 : 0))),
                              BankHead = grouping.Max(p => p.HeadOfAccountId)
                          });
            // Get total of Balance fund in banks other than Bank at wangdue
            // We keep tottal of balance in million format.
            foreach (var bank in query3)
            {
                //if (bank.BankHead != 1415)
                //{
                    _otherBankAmount = _otherBankAmount + bank.Balance;
                    otherBankAmount = DisplayInMillion(_otherBankAmount);
               // }
            }
            grvBankAccount.DataSource = query3;
            grvBankAccount.DataBind();
            return;
        }

        /// <summary>
        /// Make all NavigateUrls reasonable
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            foreach (Control ctl in hplnkGOIAidUptoMonth.NamingContainer.Controls)
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
                            m_dtMonthStart, m_dtMonthEnd, m_dtPreviousYearEnd, m_dtMonthStart.AddDays(-1));
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

        private void SetAdditivePropertiesFund(decimal? amount, SumType sumType)
        {
            if (amount.HasValue && amount != 0)
            {
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
                lbl.Text = DisplayInMillion(d);
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

        /// <summary>
        /// Updates the Cl total.
        /// </summary>
        /// <param name="amount"></param>
        /// <param name="sumType"></param>
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
        private void GetTotal()
        {
            foreach (KeyValuePair<Control, decimal> kvp in m_controlAmounts)
            {
                switch (kvp.Key.ID)
                {
                    //case "hplnkClBalForMonth":
                    case "lblClBalForMonthFE":
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
        }

        protected void lblbalance_prerender(object sender, EventArgs e)
        {
            decimal receiptTotal = 0;
            decimal paymentTotal = 0;
            Label lbl = (Label)sender;
            SumType totalType1 = (SumType)Enum.Parse(typeof(SumType), "ReceiptsPreviousYear");
            SumType totalType2 = (SumType)Enum.Parse(typeof(SumType), "PaymentsPreviousYear");
            if (m_totals.ContainsKey(totalType1))
            {
                receiptTotal = m_totals[totalType1];
            }
            if (m_totals.ContainsKey(totalType2))
            {
                paymentTotal = m_totals[totalType2];
            }
            decimal d = receiptTotal - paymentTotal;
            lbl.Text = DisplayInMillion(d);
        }

        protected void lblbalancefundcumulative_prerender(object sender, EventArgs e)
        {

            Label lbl = (Label)sender;
            lbl.Text = DisplayInMillion(getBalancefund());
        }

        protected void lblExpenditureCurrentmonth_PreRenderShowSum(object sender, EventArgs e)
        {
            Label lbl = (Label)sender;
            SumType totalType = (SumType)Enum.Parse(typeof(SumType), lbl.Attributes["SumType"]);
            if (m_totals.ContainsKey(totalType))
            {
                decimal d = m_totals[totalType];
                lbl.Text = DisplayInMillion(d);
            }

        }

        protected void lblDifference_PreRender(object sender, EventArgs e)
        {
            Label lbl = (Label)sender;
            decimal d = m_totals[SumType.FundSum] - m_totals[SumType.PaymentsSum];
            lbl.Text = DisplayInMillion(d);
        }

        /// <summary>
        /// Returns balance fund.i.e difference between receipts and expenditure.
        /// </summary>
        /// <returns></returns>
        protected decimal getBalancefund()
        {
            decimal receiptTotal = 0;
            decimal paymentTotal = 0;
            SumType totalType1 = (SumType)Enum.Parse(typeof(SumType), "ReceiptsSum");
            SumType totalType2 = (SumType)Enum.Parse(typeof(SumType), "PaymentsSum");
            if (m_totals.ContainsKey(totalType1))
            {
                receiptTotal = m_totals[totalType1];
            }
            if (m_totals.ContainsKey(totalType2))
            {
                paymentTotal = m_totals[totalType2];
            }
            decimal d = receiptTotal - paymentTotal;
            return d;
        }

        /// <summary>
        /// Handling this event so as to show values in cell in million format.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grvBankAccount_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            switch (e.Row.RowType)
            {
                case DataControlRowType.DataRow:
                    var val = m_totals[0];
                    int indexAccountbanace = grvBankAccount.Columns.Cast<DataControlField>()
                  .Select((p, i) => p.AccessibleHeaderText == "AccountBalance" ? i : -1).First(p => p >= 0);
                    int indexAccountHead = grvBankAccount.Columns.Cast<DataControlField>()
                  .Select((p, i) => p.AccessibleHeaderText == "AccountHead" ? i : -1).First(p => p >= 0);
                    var valuehead = e.Row.Cells[indexAccountHead].Text;
                    var value = Convert.ToDecimal(e.Row.Cells[indexAccountbanace].Text);
                    //if (valuehead == "1415")
                    //{
                      //  // If bank is BOB wangdue then we get balance at bank by susbtracting total balace fund with balance in other banks
                        //var balancefund = Convert.ToDecimal(DisplayInMillion(getBalancefund())) - _otherBankAmount;
                        //e.Row.Cells[indexAccountbanace].Text = balancefund.ToString();
                    //}
                    //else
                    //{

                        e.Row.Cells[indexAccountbanace].Text = DisplayInMillion(value);
                    //}
                    break;
                case DataControlRowType.Footer:
                    int indexNetPay1 = grvBankAccount.Columns.Cast<DataControlField>()
                   .Select((p, i) => p.AccessibleHeaderText == "AccountBalance" ? i : -1).First(p => p >= 0);
                    e.Row.Cells[indexNetPay1].Text = DisplayInMillion(getBalancefund());
                    int indexNetPay2 = grvBankAccount.Columns.Cast<DataControlField>()
                  .Select((p, i) => p.AccessibleHeaderText == "AccountName" ? i : -1).First(p => p >= 0);
                    e.Row.Cells[indexNetPay2].Text = "TOTAL";
                    break;
            }
        }

        /// <summary>
        /// Display amount in millions..
        /// </summary>
        /// <param name="amount"></param>
        /// <returns></returns>
        private string DisplayInMillion(decimal? amount)
        {
            if (amount.HasValue)
            {
                return amount.Value.ToString(MONEY_FORMAT_SPECIFIER);
            }
            return string.Empty;
        }

    }
}
