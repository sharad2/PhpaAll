
/*
 *  Programming: Eclipse Systems (P) Ltd., NOIDA, INDIA
 *  E-mail: support@eclsys.com
 *
 *  $Workfile:   VoucherDiscrepancies.aspx.cs  $
 *  $Revision: 38232 $
 *  $Author: ssingh $
 *  $Date: 2010-12-01 13:34:53 +0530 (Wed, 01 Dec 2010) $
 *  $Modtime:   Jul 15 2008 20:01:12  $
 *
 *  $Log:   S:/Projects/PHPA2/archives/Finance/Finance/VoucherDiscrepancies.aspx.cs-arc  $
 * 
 *    Rev 1.37   Jul 15 2008 20:01:22   dbhatt
 * wip
 * 
 *    Rev 1.36   Jul 15 2008 20:00:40   dbhatt
 * wip
 * 
 *    Rev 1.35   Jul 15 2008 19:42:32   dbhatt
 * wip
 * 
 *    Rev 1.34   Jul 15 2008 18:50:52   dbhatt
 * wip
 * 
 *    Rev 1.33   Jul 15 2008 09:32:44   dbhatt
 * wip
 * 
 *    Rev 1.32   Jul 11 2008 20:45:28   ssinghal
 * WIP
 * 
 *    Rev 1.31   Jul 11 2008 20:31:40   ssinghal
 * WIP
 * 
 *    Rev 1.30   Jul 11 2008 20:29:02   ssinghal
 * WIP
 * 
 *    Rev 1.29   May 31 2008 13:15:00   ssinghal
 * WIP
 * 
 *    Rev 1.28   May 28 2008 17:32:26   dbhatt
 * wip
 * 
 *    Rev 1.27   May 28 2008 17:32:00   dbhatt
 * wip
 * 
 *    Rev 1.26   May 28 2008 17:26:40   dbhatt
 * wip
 * 
 *    Rev 1.25   May 28 2008 17:19:44   dbhatt
 * wip
 * 
 *    Rev 1.24   May 27 2008 19:38:20   yjuneja
 * WIP
 * 
 *    Rev 1.23   May 23 2008 18:07:58   dbhatt
 * wip
 * 
 *    Rev 1.22   May 23 2008 18:01:40   dbhatt
 * wip
 * 
 *    Rev 1.21   May 23 2008 17:51:26   dbhatt
 * wip
 * 
 *    Rev 1.20   May 23 2008 13:45:06   ssinghal
 * WIP
 * 
 *    Rev 1.19   May 22 2008 17:52:16   glal
 * WIP
 * 
 *    Rev 1.18   May 21 2008 17:54:42   dbhatt
 * WIP
 * 
 *    Rev 1.17   May 16 2008 18:51:28   dbhatt
 * wip
 * 
 *    Rev 1.16   May 16 2008 18:04:44   dbhatt
 * wip
 * 
 *    Rev 1.15   May 16 2008 16:34:54   dbhatt
 * wip
 * 
 *    Rev 1.14   May 16 2008 15:25:20   dbhatt
 * wip
 * 
 *    Rev 1.13   May 16 2008 14:35:22   ssinghal

 *    Rev 1.7   May 15 2008 13:33:22   dbhatt
 * wip
 * 
 *    Rev 1.6   May 14 2008 20:33:36   dbhatt
 * wip
 * 
 *    Rev 1.5   May 14 2008 19:13:54   dbhatt
 * wip
 * 
 *    Rev 1.4   May 14 2008 17:51:44   dbhatt
 * wip
 * 
 *    Rev 1.3   May 14 2008 17:10:28   dbhatt
 * wip
 * 
 *    Rev 1.2   May 07 2008 12:54:06   dbhatt
 * wip
 * 
 *    Rev 1.1   May 06 2008 18:19:18   dbhatt
 * wip
 * 
 *    Rev 1.0   May 06 2008 17:39:54   ssinghal
 * Initial Revision
 * 
 *    Rev 1.1   May 06 2008 17:31:26   ssinghal
 * WIP
 * 
 *    Rev 1.0   May 06 2008 14:20:50   ssinghal
 * Initial Revision
 */

using System;
using Eclipse.PhpaLibrary.Web;
using PhpaAll.Controls;

namespace PhpaAll.Finance
{
    /// <summary>
    /// Shows discrepant vouchers.
    /// </summary>
    public partial class Vouchers : PageBase
    {
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            ctlVoucherDetail.DataBind();
        }

        protected void lblMessage_PreRender(object sender, EventArgs e)
        {
            lblMessage.Text = "No discrepant vouchers found";
        }

        protected void ctlVoucherDetail_Selecting(object sender, VoucherDetailSelectingEventArgs e)
        {
            e.WhereClauses.Add("RoVoucherDetails.Sum(DebitAmount) != RoVoucherDetails.Sum(CreditAmount)");
        }
    }
}
