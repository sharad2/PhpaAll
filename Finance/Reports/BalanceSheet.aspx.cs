/*
 *  Programming: Eclipse Systems (P) Ltd., NOIDA, INDIA
 *  E-mail: support@eclsys.com
 *
 *  $Workfile:   BalanceSheet.aspx.cs  $
 *  $Revision: 34727 $
 *  $Author: ssinghal $
 *  $Date: 2010-08-27 12:24:34 +0530 (Fri, 27 Aug 2010) $
 *  $Modtime:   Jul 19 2008 17:40:50  $
 *
 *  $Log:   S:/Projects/PHPA2/archives/Finance/Reports/BalanceSheet.aspx.cs-arc  $
 * 
 *    Rev 1.57   Jul 19 2008 17:45:26   yjuneja
 * WIP
 * 
 *    Rev 1.56   Jul 16 2008 11:50:12   ssinghal
 * Using format specified "C" for displaying money values
 * 
 *    Rev 1.55   Jul 09 2008 17:40:54   vraturi
 * PVCS Template Added.
 */
using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Eclipse.PhpaLibrary.Reporting;
using Eclipse.PhpaLibrary.Web;


namespace Finance.Reports
{
    public partial class BalanceSheet : PageBase
    {
        private ReportingDataContext m_db;

        protected override void OnLoad(EventArgs e)
        {
            m_db = (ReportingDataContext)dsQueries.Database;

            if (!Page.IsPostBack)
            {
                DateTime date = new DateTime();
                date = System.DateTime.Now;
                tbdate.Text = date.ToShortDateString();
            }
            Page.Validate();
            if (Page.IsValid)
            {
                CalculateBalaceSheet(DateTime.Parse(tbdate.Text));
                this.Title = string.Format("Balance Sheet as on {0:dd MMMM yyyy}", tbdate.ValueAsDate);
            }
            base.OnLoad(e);
        }

        private void CalculateBalaceSheet(DateTime dt)
        {
            var query = from vd in m_db.RoVoucherDetails
                        where vd.RoVoucher.VoucherId == vd.VoucherId &&
                        vd.RoVoucher.VoucherDate <= dt &&
                        vd.RoHeadHierarchy.HeadOfAccountType != null
                        group vd by new
                        {
                            vd.HeadOfAccount.RoAccountType
                        }
                            into grouping
                            select new
                            {
                                grouping.Key,
                                Amount = grouping.Sum(p => p.CreditAmount ?? 0 - p.DebitAmount ?? 0),
                            };

            double greenTaxSum = 0;
            double sumAssets = 0;
            double sumLiability = 0;
            double accsum = 0;
            //double banksum = 0;
            //double wipsum = 0;
            double grantsSum = 0;
            double loansSum = 0;
            double advanceSum = 0;
            double expsum = 0;
            double otherAssets = 0;
            double otherLiability = 0;
            //double servicetaxsum = 0;

            string fmt = "C";
            foreach (var grp in query)
            {
                //string fmt = "##.##;(##.##)";
                switch (grp.Key.RoAccountType.HeadOfAccountType)
                {
                    //case "BANKNU":    
                    //case "BANKFE":
                    //    banksum += Convert.ToDouble(-grp.Amount);
                    //    hplnkcashatbank.Text = banksum.ToString(fmt);
                    //    sumAssets += Convert.ToDouble(-grp.Amount);
                    //    hplnkcashatbank.NavigateUrl = string.Format("~/Finance/VoucherSearch.aspx?AccountTypes=BANKNU,BANKFE&DateTo={0}", dt.ToShortDateString());
                    //    break;
                    //case "Service_Tax":
                    //    servicetaxsum += Convert.ToDouble(grp.Amount);
                    //    SERVICETAX.Text = servicetaxsum.ToString(fmt);
                    //    sumLiability += Convert.ToDouble(grp.Amount);
                    //    SERVICETAX.NavigateUrl = string.Format("~/Finance/VoucherSearch.aspx?AccountTypes=Service_Tax&DateTo={0}", dt.ToShortDateString());
                    //    break;
                    case "GREEN_TAX":
                        greenTaxSum += Convert.ToDouble(-grp.Amount);
                        hplnkgtax.Text = greenTaxSum.ToString(fmt);
                        sumAssets += Convert.ToDouble(-grp.Amount);
                        hplnkgtax.NavigateUrl = (string.Format("~/Finance/VoucherSearch.aspx?AccountTypes=GREEN_TAX&DateTo={0}", dt.ToShortDateString()));
                        break;
                    case "ACCUMULATED_RECEIPTS":
                    //case "INTEREST":
                    case "TENDER_SALE":
                        accsum += Convert.ToDouble(grp.Amount);
                        hplnkAcc_Rec.Text = accsum.ToString(fmt);
                        sumLiability += Convert.ToDouble(grp.Amount);
                        hplnkAcc_Rec.NavigateUrl = (string.Format("~/Finance/VoucherSearch.aspx?AccountTypes=ACCUMULATED_RECEIPTS,TENDER_SALE&DateTo={0}", dt.ToShortDateString()));
                        break;

                    case "GRANT_RECEIVED_GOINU":
                    case "GRANT_RECEIVED_GOIFE":
                        grantsSum += Convert.ToDouble(grp.Amount);
                        hplnkgrantreceived.Text = grantsSum.ToString(fmt);
                        sumLiability += Convert.ToDouble(grp.Amount);
                        hplnkgrantreceived.NavigateUrl = string.Format("~/Finance/VoucherSearch.aspx?AccountTypes=GRANT_RECEIVED_GOINU,GRANT_RECEIVED_GOIFE&DateTo={0}", dt.ToShortDateString());
                        break;

                    case "LOAN_RECEIVED_GOINU":
                    case "LOAN_RECEIVED_GOIFE":
                        loansSum += Convert.ToDouble(grp.Amount);
                        hplnkloanreceived.Text = loansSum.ToString(fmt);
                        sumLiability += Convert.ToDouble(grp.Amount);
                        hplnkloanreceived.NavigateUrl = string.Format("~/Finance/VoucherSearch.aspx?AccountTypes=LOAN_RECEIVED_GOINU,LOAN_RECEIVED_GOIFE&DateTo={0}", dt.ToShortDateString());
                        break;

                    case "MATERIAL_ADVANCE":
                    case "PARTY_ADVANCE":
                        advanceSum += Convert.ToDouble(-grp.Amount);
                        hlContractorAdvance.Text = advanceSum.ToString(fmt);
                        sumAssets += Convert.ToDouble(-grp.Amount);
                        hlContractorAdvance.NavigateUrl = string.Format("~/Finance/VoucherSearch.aspx?AccountTypes=MATERIAL_ADVANCE,PARTY_ADVANCE&DateTo={0}", dt.ToShortDateString());
                        break;

                    case "EXPENDITURE":
                    case "TOUR_EXPENSES":
                        expsum += Convert.ToDouble(-grp.Amount);
                        hlExpenditure.Text = expsum.ToString(fmt);
                        sumAssets += Convert.ToDouble(-grp.Amount);
                        hlExpenditure.NavigateUrl = string.Format("~/Finance/VoucherSearch.aspx?AccountTypes=EXPENDITURE,TOUR_EXPENSES&DateTo={0}", dt.ToShortDateString());
                        break;

                    default:
                        HyperLink hplnk = (HyperLink)this.plhTable.FindControl(grp.Key.RoAccountType.HeadOfAccountType);
                        if (grp.Key.RoAccountType.Category == "A" || grp.Key.RoAccountType.Category == "B" || grp.Key.RoAccountType.Category == "E" || grp.Key.RoAccountType.Category == "C")
                        {
                            if (hplnk == null || hplnk.ID == "ASSETS")
                            {
                                hplnk = ASSETS;
                                otherAssets += Convert.ToDouble(-grp.Amount);
                                hplnk.Text = otherAssets.ToString(fmt);
                                sumAssets += Convert.ToDouble(-grp.Amount);
                                hplnk.NavigateUrl = string.Format("~/Finance/VoucherSearch.aspx?AccountTypes={0}&DateTo={1}", hplnk.ID, dt.ToShortDateString());
                            }
                            else
                            {
                                hplnk.Text = (-grp.Amount).ToString(fmt);
                                sumAssets += Convert.ToDouble(-grp.Amount);
                                hplnk.NavigateUrl = string.Format("~/Finance/VoucherSearch.aspx?AccountTypes={0}&DateTo={1}", hplnk.ID, dt.ToShortDateString());
                            }
                        }
                        else if (grp.Key.RoAccountType.Category == "L" || grp.Key.RoAccountType.Category == "R")
                        {
                            if (hplnk == null || hplnk.ID == "LIABILITY")
                            {
                                hplnk = LIABILITY;
                                otherLiability += Convert.ToDouble(grp.Amount);
                                hplnk.Text = otherLiability.ToString(fmt);
                                sumLiability += Convert.ToDouble(grp.Amount);
                                hplnk.NavigateUrl = string.Format("~/Finance/VoucherSearch.aspx?AccountTypes={0}&DateTo={1}", hplnk.ID, dt.ToShortDateString());
                            }
                            else
                            {
                                hplnk.Text = (grp.Amount).ToString(fmt);
                                sumLiability += Convert.ToDouble(grp.Amount);
                                hplnk.NavigateUrl = string.Format("~/Finance/VoucherSearch.aspx?AccountTypes={0}&DateTo={1}", hplnk.ID, dt.ToShortDateString());
                            }
                        }
                        else
                        {
                            throw new NotSupportedException("Unexpected head of account type");
                        }
                        break;
                }
            }

            lblSumLiabilities.Text = sumLiability.ToString("N2");
            lblSumAssets.Text = sumAssets.ToString("N2");
        }
    }
}
