using System;
using Eclipse.PhpaLibrary.Web;


namespace Finance
{
    public partial class BankBook : PageBase
    {
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            this.DataBind();
            return;
        }

        public override void DataBind()
        {
            ddlHeads.DataBind();
            ctlVoucherDetail.FocusHeadOfAccountId = Convert.ToInt32(ddlHeads.Value);
            ctlVoucherDetail.FromDate = this.tbDateFrom.ValueAsDate;
            ctlVoucherDetail.ToDate = this.tbDateTo.ValueAsDate;
            string reportHeader = string.Format("{0}<br />{1:dd MMMM yyyy} To {2:dd MMMM yyyy}", ddlHeads.SelectedItem.Text,
                ctlVoucherDetail.FromDate, ctlVoucherDetail.ToDate);
            lblHeadOfAccount.Text = reportHeader;
            base.DataBind();
        }

    }
}
