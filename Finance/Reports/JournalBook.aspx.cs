/*
 *  Programming: Eclipse Systems (P) Ltd., NOIDA, INDIA
 *  E-mail: support@eclsys.com
 *
 *  $Workfile:   JournalBook.aspx.cs  $
 *  $Revision: 37238 $
 *  $Author: ssingh $
 *  $Date: 2010-11-13 14:39:37 +0530 (Sat, 13 Nov 2010) $
 *  $Modtime:   Jul 22 2008 11:15:42  $
 *
 *  $Log:   S:/Projects/PHPA2/archives/Finance/Reports/JournalBook.aspx.cs-arc  $
 * 
 *    Rev 1.27   Jul 22 2008 11:21:10   yjuneja
 * WIP
 * 
 *    Rev 1.26   Jul 11 2008 16:29:32   msharma
 * WIP
 * 
 *    Rev 1.25   Jun 24 2008 15:12:36   yjuneja
 * wip
 * 
 *    Rev 1.24   Jun 24 2008 14:19:50   msharma
 * WIP
 * 
 *    Rev 1.23   Jun 22 2008 18:34:32   ssinghal
 * WIP
 * 
 *    Rev 1.22   Jun 22 2008 17:48:50   ssinghal
 * WIP
 * 
 *    Rev 1.21   Jun 19 2008 13:50:50   msharma
 * WIP
 * 
 *    Rev 1.20   Jun 19 2008 12:36:14   ssinghal
 * WIP
 * 
 *    Rev 1.19   May 31 2008 13:15:14   ssinghal
 * WIP
 * 
 *    Rev 1.18   May 22 2008 17:09:32   glal
 * WIP
 * 
 *    Rev 1.17   May 14 2008 16:50:52   msharma
 * WIP
 * 
 *    Rev 1.16   May 13 2008 15:21:54   msharma
 * WIP
 * 
 *    Rev 1.15   May 12 2008 15:01:32   msharma
 * WIP
 * 
 *    Rev 1.14   May 12 2008 14:58:14   msharma
 * WIP
 * 
 *    Rev 1.13   May 12 2008 14:49:04   msharma
 * WIP
 * 
 *    Rev 1.12   May 03 2008 17:22:24   yjuneja
 * WIP
 * 
 *    Rev 1.11   Apr 28 2008 21:36:34   msharma
 * WIP
 * 
 *    Rev 1.10   Apr 24 2008 15:13:48   yjuneja
 * WIP
 * 
 *    Rev 1.9   Apr 05 2008 12:30:42   msharma
 * WIP
 * 
 *    Rev 1.8   Mar 31 2008 19:48:18   msharma
 * WIP
 * 
 *    Rev 1.7   Mar 31 2008 18:38:32   msharma
 * WIP
 * 
 *    Rev 1.6   Mar 31 2008 18:23:32   msharma
 * WIP
 * 
 *    Rev 1.5   Mar 31 2008 18:07:36   msharma
 * WIP
 * 
 *    Rev 1.4   Mar 26 2008 16:58:32   msharma
 * WIP
 * 
 *    Rev 1.3   Mar 26 2008 14:55:18   msharma
 * WIP
 * 
 *    Rev 1.2   Mar 26 2008 14:48:14   msharma
 * WIP
 * 
 *    Rev 1.1   Mar 25 2008 18:04:36   msharma
 * WIP
 * 
 *    Rev 1.0   Mar 24 2008 12:58:30   ssinghal
 * Initial Revision

   Rev 1.25   Mar 14 2008 14:03:46   msharma
WIP

   Rev 1.24   Mar 14 2008 11:59:56   msharma
WIP

   Rev 1.23   Mar 14 2008 10:16:00   msharma
WIP
 * 
 */
//Code reviewed by Mayank Sharama on 12th May 2008 
using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using Eclipse.PhpaLibrary.Reporting;
using Eclipse.PhpaLibrary.Web;

namespace Finance.Reports
{
    public partial class JournalBook : PageBase
    {
        /// <summary>
        /// Set default values for date range and execute query for the report.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnLoad(EventArgs e)
        {
            // Set default date
            if (!Page.IsPostBack)
            {
                tbFromDate.Value = DateTime.Today.MonthStartDate().ToShortDateString();
                tbToDate.Value = DateTime.Today.ToShortDateString();
            }

            // if only fromDate provided,then toDate will be consider as one month after from fromDate.  
            if (tbToDate.Value == null)
            {
                //ctlDateRange.DateTo = Convert.ToDateTime(ctlDateRange.DateTo).AddMonths(1);
                tbToDate.Value = Convert.ToDateTime(tbToDate.Value).AddMonths(1).ToShortDateString();
            }
            
            //Force to validate date range so that page.isValid property can be used.


            if (btnShowReport.IsPageValid())
            {
                ctlVoucherDetail.FromDate = Convert.ToDateTime(tbFromDate.ValueAsDate);
                ctlVoucherDetail.ToDate = Convert.ToDateTime(tbToDate.ValueAsDate);
                ctlVoucherDetail.VoucherType = 'j';
                ctlVoucherDetail.DataBind();
            }
          
            base.OnLoad(e);
        }
    }
}
