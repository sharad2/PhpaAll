using System;
using System.Data.Linq;
using System.Linq;
using System.Web.Services;
using System.Web.UI.WebControls;
using Eclipse.PhpaLibrary.Reporting;
using Eclipse.PhpaLibrary.Web;
using EclipseLibrary.Web.UI;
using System.Collections.Generic;
using System.Web.Security;

namespace Finance.Finance
{
    /// <summary>
    /// You can pass one or more of the following query string variables:
    /// AccountTypes - comma seperated list of account types. Only those vouchers will be selected
    /// which contain at least one entry for the type. The Debit and credit sum will consider only the heads of the
    /// passed types.
    /// 
    /// DateTo - Only vouchers whose VoucherDate is less or equal to "DateTo" are selected.
    /// 
    /// DateFrom - Only vouchers whose VoucherDate is more than or equal to "DateFrom" are selected.

    /// If you pass both DateTo and DateFrom then both are honored.
    /// 
    /// EmployeeId - Voucher details of this employee id are selected. Pass 0 if you want to select
    /// only those voucher details where employee id is null.
    /// 
    /// HeadOfAccountId- Voucher details of passed HeadOfAccountId are shown.
    /// 
    ///  ContractorId - Voucher details of this contractor id are selected. Pass 0 if you want to select
    /// only those voucher details where contractor id is null.
    /// 
    /// </summary>
    public partial class VoucherSearch : PageBase
    {

        /// <summary>
        /// Ritesh: 18 Jan 2012
        /// Showing vouchers of logged in user's station only
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dsVoucherDetails_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            ReportingDataContext db = (ReportingDataContext)dsVoucherDetails.Database;
            var stations = this.GetUserStations();
            IQueryable<RoVoucherDetail> voucherDetails = db.RoVoucherDetails;
            if (tbFromDate.ValueAsDate.HasValue)
            {
                voucherDetails = voucherDetails.Where(p => p.RoVoucher.VoucherDate >= tbFromDate.ValueAsDate);
            }

            if (tbToDate.ValueAsDate.HasValue)
            {
                voucherDetails = voucherDetails.Where(p => p.RoVoucher.VoucherDate <= tbToDate.ValueAsDate);
            }

            if (cblAccountTypes.SelectedValues.Length > 0)
            {
                voucherDetails = voucherDetails.Where(p => cblAccountTypes.SelectedValues.Contains(p.HeadOfAccount.HeadOfAccountType));
            }

            if (!string.IsNullOrEmpty(tbParticulars.Text))
            {
                voucherDetails = voucherDetails.Where(p => p.RoVoucher.Particulars.Contains(tbParticulars.Text));
            }

            if (!string.IsNullOrEmpty(tbPayeeName.Text))
            {
                voucherDetails = voucherDetails.Where(p => p.RoVoucher.PayeeName.Contains(tbPayeeName.Text));
            }

            if (!string.IsNullOrEmpty(tbVoucherCode.Text))
            {
                voucherDetails = voucherDetails.Where(p => p.RoVoucher.VoucherCode == tbVoucherCode.Text);
            }

            if (!string.IsNullOrEmpty(tbContractor.Value))
            {
                voucherDetails = voucherDetails.Where(p => p.ContractorId == Convert.ToInt32(tbContractor.Value));

            }

            if (chkJobSpecified.Checked)
            {
                voucherDetails = voucherDetails.Where(p => p.JobId != null);
            }

            if (!string.IsNullOrEmpty(tbEmployee.Value))
            {
                voucherDetails = voucherDetails.Where(p => p.EmployeeId == Convert.ToInt32(tbEmployee.Value));
            }
            else if (!string.IsNullOrEmpty(tbEmployee.Text))
            {
                voucherDetails = voucherDetails.Where(p => p.RoEmployee.FirstName.Contains(tbEmployee.Text) ||
                    p.RoEmployee.LastName.Contains(tbEmployee.Text) || p.RoEmployee.EmployeeCode == tbEmployee.Text);
            }


            if (!string.IsNullOrEmpty(tbHeadOfAccount.Value))
            {
                voucherDetails = voucherDetails.Where(p => p.HeadOfAccountId == Convert.ToInt32(tbHeadOfAccount.Value));
            }



            if (tbDateCreated.ValueAsDate.HasValue)
            {
                voucherDetails = voucherDetails.Where(p => p.RoVoucher.Created.Value.Date == tbDateCreated.ValueAsDate.Value.Date);
            }
            if (stations != null)
            {
                voucherDetails = voucherDetails.Where(p => p.RoVoucher.StationId == null || stations.Contains(p.RoVoucher.StationId.Value));
            }
            e.Result = from vd in voucherDetails
                       group vd by vd.RoVoucher into g
                       orderby g.Key.VoucherDate descending
                       select new
                       {
                           //VoucherDetailId = vd.VoucherDetailId,
                           VoucherId = g.Key.VoucherId,
                           DivisionCode = g.Key.RoDivision.DivisionCode,
                           VoucherReference = g.Key.VoucherReference,
                           VoucherCode = g.Key.VoucherCode,
                           VoucherType = g.Key.DisplayVoucherType,
                           VoucherTypeCode = g.Key.VoucherType,
                           DivisionId = (int?)g.Key.DivisionId,
                           VoucherDate = g.Key.VoucherDate,
                           DivisionName = g.Key.RoDivision.DivisionName,
                           PayeeName = g.Key.PayeeName,
                           Particulars = g.Key.Particulars,
                           DebitAmount = g.Sum(p => p.DebitAmount),
                           CreditAmount = g.Sum(p => p.CreditAmount),
                           CountDetails = g.Count()
                       };
        }

        protected void gvSearchVoucher_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            switch (e.Row.RowType)
            {
                case DataControlRowType.Footer:
                    MultiBoundField fieldDebit = (MultiBoundField)(from DataControlField col in gvSearchVoucher.Columns
                                                         where col.AccessibleHeaderText == "Debit"
                                                         select col).Single();
                    MultiBoundField fieldCredit = (MultiBoundField)(from DataControlField col in gvSearchVoucher.Columns
                                                          where col.AccessibleHeaderText == "Credit"
                                                          select col).Single();
                    decimal? cr = Convert.ToDecimal(fieldCredit.SummaryValues[0].Value);
                    decimal? dr = Convert.ToDecimal(fieldDebit.SummaryValues[0].Value);
                    decimal Differance = Math.Abs((decimal)(cr - dr));
                    lbldiffrence.Text = Differance.ToString("C");
                    break;
            }
        }
    }
}
