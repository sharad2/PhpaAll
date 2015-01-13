
/*
 *  Programming: Eclipse Systems (P) Ltd., NOIDA, INDIA
 *  E-mail: support@eclsys.com
 *
 *  $Workfile:   DayBook.aspx.cs  $
 *  $Revision: 37856 $
 *  $Author: skumar $
 *  $Date: 2010-11-25 12:40:26 +0530 (Thu, 25 Nov 2010) $
 *  $Modtime:   Jul 21 2008 10:52:26  $
 *
 *  $Log:   S:/Projects/PHPA2/archives/Finance/Finance/DayBook.aspx.cs-arc  $
 * 
 *    Rev 1.52   Jul 28 2008 14:57:52   msharma
 * WIP
 * 
 *    Rev 1.51   Jul 21 2008 10:55:32   ssinghal
 * WIP
 * 
 *    Rev 1.50   Jul 19 2008 14:09:02   ssinghal
 * WIP
 * 
 *    Rev 1.49   Jul 19 2008 12:39:36   ssinghal
 * WIP
 * 
 *    Rev 1.48   Jul 18 2008 13:42:52   ssinghal
 * WIP
 * 
 *    Rev 1.47   Jul 15 2008 10:22:14   ssinghal
 * WIP
 * 
 *    Rev 1.46   Jul 11 2008 19:51:18   ssinghal
 * WIP
 * 
 *    Rev 1.45   Jul 11 2008 14:01:42   ssinghal
 * WIP
 * 
 *    Rev 1.44   Jul 01 2008 16:43:26   ssinha
 * Closing balances is now displayed at the bottom of the report.
 * 
 *    Rev 1.43   Jun 24 2008 20:45:32   ssinghal
 * WIP
 * 
 *    Rev 1.42   Jun 24 2008 12:22:02   ssinghal
 * WIP
 * 
 *    Rev 1.41   Jun 23 2008 20:36:20   ssinghal
 * WIP
 * 
 *    Rev 1.40   Jun 23 2008 20:21:00   ssinghal
 * WIP
 * 
 */
using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Eclipse.PhpaLibrary.Reporting;
using Eclipse.PhpaLibrary.Web;

//Code reviewed by Mayank Sharama on 5th April 2008 
namespace Finance.Reports
{
    /// <summary>
    /// You can pass the date in query string VoucherDate
    /// </summary>
    public partial class DayBook : PageBase
    {
        protected override void OnLoad(EventArgs e)
        {

            this.Title = string.Format("Day Book for {0}", tbVoucherDate.Text);
            base.OnLoad(e);
        }

        protected override void OnLoadComplete(EventArgs e)
        {
            base.OnLoadComplete(e);
            ctlVoucherDetail.FromDate = DateTime.Parse(tbVoucherDate.Text);
            ctlVoucherDetail.ToDate = DateTime.Parse(tbVoucherDate.Text);
            ctlVoucherDetail.DataBind();

            ReportingDataContext db = (ReportingDataContext)dsOpeningBalance.Database;

            //Excute the  query for opening and closing balance of bank and cash accounts
            DateTime voucherDate = DateTime.Parse(tbVoucherDate.Text);
            gvClosingBalance.DataSource = from hoa in db.RoHeadHierarchies
                                          where HeadOfAccountHelpers.CashSubType.CashInBankNu.Concat(HeadOfAccountHelpers.CashSubType.CashInBankFe).Concat(HeadOfAccountHelpers.CashSubType.CashInHand).Contains(hoa.HeadOfAccountType)
                                          //hoa.HeadOfAccountType == "BANKNU" ||
                                          //hoa.HeadOfAccountType == "BANKFE" ||
                                          //hoa.HeadOfAccountType == "CASH"
                                          orderby hoa.DisplayName
                                          select new
                                          {
                                              HeadOfAccountId = hoa.HeadOfAccountId,
                                              displayName = hoa.DisplayName,
                                              Description = hoa.Description,
                                              receipts = (decimal?)hoa.RoVoucherDetails.Sum(vd => vd.RoVoucher.VoucherDate == voucherDate ? vd.DebitAmount ?? 0 : 0),
                                              payments = (decimal?)hoa.RoVoucherDetails.Sum(vd => vd.RoVoucher.VoucherDate == voucherDate ? vd.CreditAmount ?? 0 : 0),
                                              openingBalance = (decimal?)((hoa.RoVoucherDetails.Sum(vd => vd.RoVoucher.VoucherDate < voucherDate ? vd.DebitAmount ?? 0 : 0)) -
                                                               (hoa.RoVoucherDetails.Sum(vd => vd.RoVoucher.VoucherDate < voucherDate ? vd.CreditAmount ?? 0 : 0))),
                                              closingBalance = (decimal?)((hoa.RoVoucherDetails.Sum(vd => vd.RoVoucher.VoucherDate < voucherDate ? vd.DebitAmount ?? 0 : 0)) -
                                                               (hoa.RoVoucherDetails.Sum(vd => vd.RoVoucher.VoucherDate < voucherDate ? vd.CreditAmount ?? 0 : 0)))
                                                             + (decimal?)hoa.RoVoucherDetails.Sum(vd => vd.RoVoucher.VoucherDate == voucherDate ? vd.DebitAmount ?? 0 : 0)
                                                             - (decimal?)hoa.RoVoucherDetails.Sum(vd => vd.RoVoucher.VoucherDate == voucherDate ? vd.CreditAmount ?? 0 : 0)

                                          };

            gvClosingBalance.DataBind();
        }

        protected void lblMessage_PreRender(object sender, EventArgs e)
        {
            using (ReportingDataContext db = new ReportingDataContext(ReportingUtilities.DefaultConnectString))
            {
                var date = (from v in db.RoVouchers
                            select v).Max(v => (DateTime?)v.VoucherDate);
                if (date == null)
                {
                    lblMessage.Text = "[No vouchers found]";
                }
                else
                {
                    lblMessage.Text = "No vouchers found for the date you specified. Most recently, vouchers were entered on";
                    hlRecentDate.Text = date.Value.ToString("d");
                    hlRecentDate.NavigateUrl = string.Format("{0}?VoucherDate={1:d}",
                        this.Request.AppRelativeCurrentExecutionFilePath, date);
                }
            }
        }

        protected void gvClosingBalance_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            switch (e.Row.RowType)
            {
                case DataControlRowType.DataRow:
                    DateTime dt = DateTime.Parse(tbVoucherDate.Text);
                    HyperLink hlHeadofaccount = (HyperLink)e.Row.FindControl("hlHeadofAccount");
                    hlHeadofaccount.NavigateUrl += string.Format("?FromDate={0:d}&ToDate={1:d}&HeadOfAccount={2}",
                        dt.FinancialYearStartDate(), dt, DataBinder.Eval(e.Row.DataItem, "HeadOfAccountId"));
                    break;
            }
        }
        
    }
}

