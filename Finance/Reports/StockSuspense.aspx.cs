/*
 *  Programming: Eclipse Systems (P) Ltd., NOIDA, INDIA
 *  E-mail: support@eclsys.com
 *
 *  $Workfile:   StockSuspense.aspx.cs  $
 *  $Revision: 37145 $
 *  $Author: bkumar $
 *  $Date: 2010-11-10 17:49:04 +0530 (Wed, 10 Nov 2010) $
 *  $Modtime:   Jul 21 2008 15:33:38  $
 *
 *  $Log:   S:/Projects/PHPA2/archives/Finance/Reports/StockSuspense.aspx.cs-arc  $
 * 
 *    Rev 1.44   Jul 21 2008 15:34:20   yjuneja
 * WIP
 * 
 *    Rev 1.43   Jul 09 2008 17:41:00   vraturi
 * PVCS Template Added.
 */
using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Eclipse.PhpaLibrary.Reporting;
using Eclipse.PhpaLibrary.Web;
using EclipseLibrary.Web.JQuery.Input;

namespace Finance.Reports
{
    public partial class StockSuspense : PageBase
    {
        

        private ReportingDataContext m_db;

        protected override void OnLoad(EventArgs e)
        {
            DateTime? dt;
            if (!Page.IsPostBack)
            {
                tbdate.Value = DateTime.Now.ToShortDateString();
            }
            if (!Button1.IsPageValid())
            {
                return;
            }
            //Page.Validate();
            //if (Page.IsValid)
            else
            {
                dt = tbdate.ValueAsDate;
                DateTime dtMonthStart = dt.Value.MonthStartDate();
                DateTime dtNextMonthStart = dt.Value.MonthEndDate();
                DateTime dtPreviousYear = dt.Value.FinancialYearStartDate();

                m_db = (ReportingDataContext)dsStockSuspense.Database;

                var oldstock = (from vd in m_db.RoVoucherDetails
                                where HeadOfAccountHelpers.StockSuspense.Contains(vd.RoHeadHierarchy.HeadOfAccountType) &&
                                //vd.RoHeadHierarchy.HeadOfAccountType == "STOCK_SUSPENSE" &&
                                        vd.RoVoucher.VoucherDate < dt.Value.FinancialYearStartDate()
                                group vd by vd.RoHeadHierarchy.Description into grouping
                                select new
                                {
                                    Description = grouping.Key,
                                    Head = grouping.Max(p => p.RoHeadHierarchy.DisplayName),
                                    Receipt = grouping.Sum(p => p.DebitAmount ?? 0),
                                    Issue = grouping.Sum(p => p.CreditAmount ?? 0)
                                }).Distinct().ToList();

                var stock = (from v in m_db.RoVouchers
                             where v.VoucherDate >= dtPreviousYear &&
                                   v.VoucherDate < dtNextMonthStart &&
                                   v.RoVoucherDetails.Max(p => HeadOfAccountHelpers.StockSuspense.Contains(p.RoHeadHierarchy.HeadOfAccountType))
                                       //p.RoHeadHierarchy.HeadOfAccountType == "STOCK_SUSPENSE")
                             select new StockMonth()
                             {
                                 Month = v.VoucherDate.Date.Month,
                                 Year = v.VoucherDate.Date.Year
                             }
                             ).Distinct().ToList();

                if (oldstock.Count != 0)
                {
                    lblopbal.Visible = true;
                    gvopeningBalance.Visible = true;
                    lblnodata.Visible = false;
                    gvopeningBalance.DataSource = oldstock;
                    gvopeningBalance.DataBind();
                    repdsStockSuspense.Visible = false;
                    if (stock.Count != 0)
                    {
                        repdsStockSuspense.Visible = true;
                        repdsStockSuspense.DataSource = stock;
                        repdsStockSuspense.DataBind();
                    }
                }
                else if (oldstock.Count == 0 && stock.Count == 0)
                {
                    lblopbal.Visible = false;
                    lblnodata.Visible = true;
                    gvopeningBalance.Visible = false;
                    repdsStockSuspense.Visible = false;
                }
                base.OnLoad(e);
            }
        }

        /// <summary>
        /// Selects amount and head details for all vouchers in accordence to month for selected year
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void repdsStockSuspense_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            Repeater rep = (Repeater)sender;
            
            switch (e.Item.ItemType)
            {
                case ListItemType.Item:
                case ListItemType.AlternatingItem:
                    GridView gv = (GridView)e.Item.FindControl("gvrepdsStockSuspense");
                    StockMonth monthYear = (StockMonth)e.Item.DataItem;
                    Label lbl = (Label)e.Item.FindControl("Label1");
                    lbl.Text = string.Format("{0:MMMM}/{1}",getdate(monthYear.Month),monthYear.Year);
                    
                    gv.DataSource = (from vd in m_db.RoVoucherDetails
                                     where vd.RoHeadHierarchy.HeadOfAccountType == "STOCK_SUSPENSE" &&
                                           vd.RoVoucher.VoucherDate.Date.Month == monthYear.Month &&
                                           vd.RoVoucher.VoucherDate.Date.Year == monthYear.Year
                                     group vd by vd.RoHeadHierarchy.Description into grouping
                                     select new
                                     {
                                         Description = grouping.Key,
                                         Head = grouping.Max(p => p.RoHeadHierarchy.DisplayName),
                                         Receipt = grouping.Sum(p => p.DebitAmount),
                                         Issue = grouping.Sum(p => p.CreditAmount)
                                     }).Distinct();
                    gv.DataBind();
                    break;

                default:
                    break;
            }
        }

        public class StockMonth
        {
            public int Month { get; set; }
            public int Year { get; set; }
        }

        private string getdate(int month)
        {
            switch (month)
            {
                case 1:
                    return "January";

                case 2:
                    return "Feburary";

                case 3:
                    return "March";

                case 4:
                    return "April";

                case 5:
                    return "May";

                case 6:
                    return "June";

                case 7:
                    return "July";

                case 8:
                    return "August";

                case 9:
                    return "September";

                case 10:
                    return "October";

                case 11:
                    return "November";

                case 12:
                    return "December";

                default:
                    throw new ArgumentOutOfRangeException();
            }
         }
    }
}
