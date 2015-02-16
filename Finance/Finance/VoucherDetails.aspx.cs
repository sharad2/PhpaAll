using System;
using System.Data.Linq;
using System.Web.UI.WebControls;
using Eclipse.PhpaLibrary.Database;
using Eclipse.PhpaLibrary.Reporting;
using Eclipse.PhpaLibrary.Web;

namespace PhpaAll.Finance
{
    public partial class VoucherDetails : PageBase
    {

        /// <summary>
        /// Optimization to retrieve related information via joins
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dsVoucher_ContextCreated(object sender, LinqDataSourceStatusEventArgs e)
        {
            FinanceDataContext db = (FinanceDataContext)dsVoucher.Database;
            DataLoadOptions dlo = new DataLoadOptions();
            dlo.LoadWith<Voucher>(v => v.Division);
            dlo.LoadWith<Voucher>(v => v.Bill);
            dlo.LoadWith<Voucher>(v => v.VoucherDetails);
            db.LoadOptions = dlo;
        }       

        protected void lblVrRef_PreRender(object sender, EventArgs e)
        {
            Label lbl = (Label)sender;
            lbl.Text = ReportingUtilities.VoucherIdToVoucherReferenceNumber((int)fvVoucher.DataKey[0]);
        }

        protected void dsVoucher_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            if (e.WhereParameters["VoucherId"] == null)
            {
                e.Cancel = true;
            }
        }

    }
}