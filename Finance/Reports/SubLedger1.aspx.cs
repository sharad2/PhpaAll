/*
 *  Programming: Eclipse Systems (P) Ltd., NOIDA, INDIA
 *  E-mail: support@eclsys.com
 *
 *  $Workfile:   SubLedger1.aspx.cs  $
 *  $Revision: 36569 $
 *  $Author: bkumar $
 *  $Date: 2010-10-25 14:32:19 +0530 (Mon, 25 Oct 2010) $
 *  $Modtime:   Jul 24 2008 14:37:50  $
 *
 *  $Log:   S:/Projects/PHPA2/archives/Finance/Reports/SubLedger1.aspx.cs-arc  $
 */
using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Eclipse.PhpaLibrary.Reporting;
using Eclipse.PhpaLibrary.Web;
using EclipseLibrary.Web.UI;

namespace Finance.Reports
{
    public partial class SubLedger1 : PageBase
    {
        /// <summary>
        /// Set default values for date
        /// </summary>
        /// <param name="e"></param>
        protected override void OnLoad(EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (string.IsNullOrEmpty(tbFromDate.Text))
                {
                    tbFromDate.Text = DateTime.Now.MonthStartDate().ToShortDateString();
                }
                if (string.IsNullOrEmpty(tbToDate.Text))
                {
                    tbToDate.Text = DateTime.Today.ToShortDateString();
                }
            }
            base.OnLoad(e);
        }

     

        protected void ds_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            if (!btnShowReport.IsPageValid())
            {
                e.Cancel = true;
                af.Visible = false;
                return;
            }
            ReportingDataContext db = (ReportingDataContext)ds.Database;

            IQueryable<RoVoucherDetail> allVoucherDetails = db.RoVoucherDetails
                .Where(p => p.RoHeadHierarchy.HeadOfAccountType == ddlLedgerAccountHead.Value &&
                p.RoVoucher.VoucherDate >= tbFromDate.ValueAsDate && p.RoVoucher.VoucherDate <= tbToDate.ValueAsDate);

            if (!tbContractor.FilterDisabled)
            {
                allVoucherDetails = allVoucherDetails.Where(p => p.ContractorId == Convert.ToInt32(tbContractor.Value));
            }
            else if (!tbEmployee.FilterDisabled)
            {
                allVoucherDetails = allVoucherDetails.Where(p => p.EmployeeId == Convert.ToInt32(tbEmployee.Value));
            }
            if (allVoucherDetails == null)
            {
                e.Cancel = true;
                af.Visible = false;
                return;
            }

            e.Result = from vd in allVoucherDetails
                       group vd by vd.RoVoucher into g
                       select new
                       {
                           Date = g.Key.VoucherDate,
                           VoucherReference = g.Key.VoucherReference,
                           VoucherId = g.Key.VoucherId,
                           VoucherCode = g.Key.VoucherCode,
                           VoucherType = g.Key.VoucherType,
                           Particulars = g.Key.Particulars,
                           CheckNumber = g.Key.CheckNumber,
                           DebitAmount = g.Sum(p => p.DebitAmount),
                           CreditAmount = g.Sum(p => p.CreditAmount)
                       };
        }
        protected void gv_DataBound(object sender, EventArgs e)
        {
            if (gv.Rows.Count > 0)
            {
                ReportingDataContext db = (ReportingDataContext)dsVouchers.Database;
                decimal? openingBalance = null;
                if (!tbContractor.FilterDisabled)
                {
                    openingBalance = db.GetContractorOpeningBalance(ddlLedgerAccountHead.Value,
                        tbFromDate.ValueAsDate.Value, Convert.ToInt32(tbContractor.Value));

                }
                else if (!tbEmployee.FilterDisabled)
                {
                    openingBalance = db.GetEmployeeOpeningBalance(ddlLedgerAccountHead.Value,
                        tbFromDate.ValueAsDate.Value, Convert.ToInt32(tbEmployee.Value)) ?? 0;
                }

                MultiBoundField mbDebit = (from col in gv.Columns.OfType<MultiBoundField>()
                                                .Where(p => p.AccessibleHeaderText == "DebitAmount")
                                           select col).First();
                MultiBoundField mbCredit = (from col in gv.Columns.OfType<MultiBoundField>()
                                                .Where(p => p.AccessibleHeaderText == "CreditAmount")
                                            select col).First();
                decimal? dNetBalance = mbDebit.SummaryValues[0] - mbCredit.SummaryValues[0];

                lblOpeningBalance.Text = string.Format(lblOpeningBalance.Text, openingBalance);
                lblClosingBalance.Text = string.Format(lblClosingBalance.Text,
                    openingBalance + dNetBalance);
                lblOpeningBalance.Visible = true;
                lblClosingBalance.Visible = true;
                lblNetBalance.Text = string.Format(lblNetBalance.Text, dNetBalance);
                lblNetBalance.Visible = true;
            }
        }

    }
}