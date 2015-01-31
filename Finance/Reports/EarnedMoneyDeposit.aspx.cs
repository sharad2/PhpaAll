/*
 *  Programming: Eclipse Systems (P) Ltd., NOIDA, INDIA
 *  E-mail: support@eclsys.com
 *
 *  $Workfile:   EarnedMoneyDeposit.aspx.cs  $
 *  $Revision: 37067 $
 *  $Author: ssinghal $
 *  $Date: 2010-11-08 16:14:09 +0530 (Mon, 08 Nov 2010) $
 *  $Modtime:   Jul 19 2008 17:34:36  $
 *
 *  $Log:   S:/Projects/PHPA2/archives/Finance/Reports/EarnedMoneyDeposit.aspx.cs-arc  $
 * 
 *    Rev 1.45   Jul 19 2008 17:35:34   yjuneja
 * WIP
 * 
 *    Rev 1.44   Jul 14 2008 15:36:16   yjuneja
 * WIP
 * 
 *    Rev 1.43   Jul 09 2008 17:40:56   vraturi
 * PVCS Template Added.
 */

using System.Linq;
using System.Web.UI.WebControls;
using Eclipse.PhpaLibrary.Reporting;
using Eclipse.PhpaLibrary.Web;

namespace PhpaAll.Reports
{
    public partial class MoneyDeposit : PageBase
    {
        protected void dsEMD_Selecting(object sender, System.Web.UI.WebControls.LinqDataSourceSelectEventArgs e)
        {
            if (!btnshowreport.IsPageValid())
            {
                e.Cancel = true;
                return;
            }
            this.Title = string.Format("{0} as of {1:d}", ddlReportType.SelectedItem.Text, tbdate.ValueAsDate);

            ReportingDataContext db = (ReportingDataContext)dsEMD.Database;

            e.Result = (from vd in db.RoVoucherDetails
                        where vd.RoVoucher.VoucherDate <= tbdate.ValueAsDate &&
                              vd.HeadOfAccount.HeadOfAccountType == ddlReportType.Value &&
                              (vd.ContractorId.HasValue || vd.JobId.HasValue)
                        group vd by vd.ContractorId ?? vd.RoJob.ContractorId into grouping
                        select new
                        {
                            VoucherDate = tbdate.QueryStringValue,
                            ReportType = ddlReportType.Value,
                            partyname = grouping.Min(p => p.RoJob.RoContractor.ContractorName ?? p.RoContractor.ContractorName),
                            partycode = grouping.Min(p => p.RoJob.RoContractor.ContractorCode ?? p.RoContractor.ContractorCode),
                            partyid = (int?)grouping.Key,
                            Amount = grouping.Sum(p => p.CreditAmount ?? 0 - p.DebitAmount ?? 0)
                        }).Where(p => p.Amount != 0).OrderBy(p => p.partyname);
        }

        protected void gvdsEMD_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            switch (e.Row.RowType)
            {
                case DataControlRowType.Footer:
                    // Prevents footer from printing on each page
                    e.Row.TableSection = TableRowSection.TableBody;
                    break;
            }
        }

    }
}
