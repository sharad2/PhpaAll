using System;
using System.Linq;
/*
 *  Programming: Eclipse Systems (P) Ltd., NOIDA, INDIA
 *  E-mail: support@eclsys.com
 *
 *  $Workfile:   Finance.aspx.cs  $
 *  $Revision: 31173 $
 *  $Author: ssinghal $
 *  $Date: 2010-02-11 14:08:36 +0530 (Thu, 11 Feb 2010) $
 *  $Modtime:   Jul 09 2008 17:31:34  $
 *
 *  $Log:   S:/Projects/PHPA2/archives/Finance/Finance/Finance.aspx.cs-arc  $
 * 
 *    Rev 1.22   Jul 09 2008 17:31:36   vraturi
 * PVCS Template Added.
 */
using Eclipse.PhpaLibrary.Database;
using Eclipse.PhpaLibrary.Web;

namespace Finance.Finance
{
    public partial class Finance : PageBase
    {

        protected void dsVouchers_Selecting(object sender, System.Web.UI.WebControls.LinqDataSourceSelectEventArgs e)
        {
            FinanceDataContext db = (FinanceDataContext)dsVouchers.Database;

            object query = this.Context.Cache["Finance.aspx"];

            if (query == null)
            {
                query = (from vd in db.VoucherDetails
                         where vd.Voucher.Created != null
                         group vd by vd.Voucher.Created.Value.Date into g
                         orderby g.Key descending
                         select new
                         {
                             Created = g.Key,
                             CountVouchers = g.Select(p => p.Voucher.VoucherId).Distinct().Count(),
                             CountCreatedBy = g.Select(p => p.Voucher.CreatedBy).Distinct().Count(),
                             MinCreatedBy = g.Min(p => p.Voucher.CreatedBy),
                             MaxCreatedBy = g.Max(p => p.Voucher.CreatedBy),
                             Debits = g.Sum(p => p.DebitAmount),
                             Credits = g.Sum(p => p.CreditAmount),
                             MinVoucherDate = g.Min(p => p.Voucher.VoucherDate),
                             MaxVoucherDate = g.Max(p => p.Voucher.VoucherDate),
                             MaxDebitInfo = g.Where(p => p.DebitAmount == g.Max(q => q.DebitAmount))
                                 .Select(p => new
                                 {
                                     VoucherId = p.Voucher.VoucherId,
                                     Head = p.HeadOfAccount.DisplayName,
                                     HeadId = p.HeadOfAccountId,
                                     HeadDescription = p.HeadOfAccount.Description,
                                     Amount = p.DebitAmount
                                 }).First()
                         }).Take(10).ToList();

                // Save result for three hours
                this.Context.Cache.Insert("Finance.aspx", query, null, DateTime.UtcNow + TimeSpan.FromHours(3),
                    System.Web.Caching.Cache.NoSlidingExpiration);
            }
            e.Result = query;

        }
    }
}
