/*
 *  Programming: Eclipse Systems (P) Ltd., NOIDA, INDIA
 *  E-mail: support@eclsys.com
 *
 *  $Workfile:   Ledger.aspx.cs  $
 *  $Revision: 37852 $
 *  $Author: ssinghal $
 *  $Date: 2010-11-25 12:04:14 +0530 (Thu, 25 Nov 2010) $
 *  $Modtime:   Jul 28 2008 14:13:48  $
 *
 *  $Log:   S:/Projects/PHPA2/archives/Finance/Reports/Ledger.aspx.cs-arc  $
 * 
 *    Rev 1.30   Jul 28 2008 14:15:06   msharma
 * WIP
 * 
 */
using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Eclipse.PhpaLibrary.Reporting;
using Eclipse.PhpaLibrary.Web;
using EclipseLibrary.Web.JQuery;

namespace Finance.Reports
{
    public partial class Ledger : PageBase
    {

        /// <summary>
        /// You can pass one or more of the following query string variables:
        /// 
        /// ToDate - Only vouchers whose VoucherDate is less or equal to "ToDate" are selected.
        /// 
        /// FromDate - Only vouchers whose VoucherDate is more than or equal to "FromDate" are selected.
        /// 
        /// HeadOfAccount - Only Vouchers of this head are selected
        /// 
        /// If you pass both ToDate and FromDate then both are honored.
        /// 
        /// If only the FromDate is passed then ledger of one month is shown.
        /// 



        private decimal m_totalReceipts;
        private decimal m_totalPayments;

        protected override void OnLoad(EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["FromDate"] == null && Request.QueryString["ToDate"] == null)
                {
                    tbFromDate.Value = DateTime.Now.MonthStartDate().AddMonths(-1).ToShortDateString();
                    tbToDate.Value = DateTime.Now.ToShortDateString();
                }

                //if (Request.QueryString["HeadOfAccount"] != null)
                //{
                //    tbHeadOfAccount.Value = Request.QueryString["HeadOfAccount"];
                //}
            }
            else
            {
                gvVouchers.DataBind();
            }
            base.OnLoad(e);
        }

        protected void dsVouchers_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            if (!btnGo.IsPageValid())
            {
                e.Cancel = true;
                return;
            }
            DateTime? toDate = tbToDate.ValueAsDate;
            int stationId;
            if (!string.IsNullOrEmpty(ddlStation.Value))
            {
                stationId = Convert.ToInt32(ddlStation.Value);
            }
            else
            {
                stationId = 0;
            }

            // if only fromDate provided,then toDate will be consider as one month from fromDate.  
            if (tbFromDate.ValueAsDate != null && toDate == null)
            {
                toDate = tbFromDate.ValueAsDate.Value.AddMonths(1).AddDays(0);
            }

            ReportingDataContext db = (ReportingDataContext)dsVouchers.Database;
            var query = from vd in db.RoVoucherDetails
                        where
                        (vd.HeadOfAccountId == Convert.ToInt32(tbHeadOfAccount.Value))
                            && (vd.RoVoucher.VoucherDate >= (tbFromDate.ValueAsDate))
                            && (stationId==0|| vd.RoVoucher.StationId == stationId)
                            && (vd.RoVoucher.VoucherDate <= tbToDate.ValueAsDate)
                        group vd by vd.RoVoucher into grouping

                        select new
                        {
                            DivisionId = grouping.Key.DivisionId,
                            HeadOfAccountId = grouping.Key.RoVoucherDetails.Select(p => p.HeadOfAccountId),
                            VoucherId = grouping.Key.VoucherId,
                            VoucherRefrance = grouping.Key.VoucherReference,
                            VoucherCode = grouping.Key.VoucherCode,
                            VoucherType = grouping.Key.DisplayVoucherType,
                            VoucherDate = grouping.Key.VoucherDate,
                            VoucherTypeCode = grouping.Key.DisplayVoucherType,
                            Division = grouping.Key.RoDivision.DivisionName,
                            Name = grouping.Key.PayeeName,
                            Cheque = grouping.Key.CheckNumber,
                            PayeeName = grouping.Key.PayeeName,
                            EmployeeCode = grouping.Key.RoVoucherDetails.Max(p => p.RoEmployee.EmployeeCode),
                            JobCode = grouping.Key.RoVoucherDetails.Max(p => p.RoJob.JobCode),
                            Particulars = grouping.Key.Particulars,
                            DebitAmount = grouping.Sum(d => d.DebitAmount ?? 0),
                            CreditAmount = grouping.Sum(d => d.CreditAmount ?? 0),
                            Balance = grouping.Sum(p => p.DebitAmount ?? 0 - p.CreditAmount ?? 0)
                        };


            if (!string.IsNullOrEmpty(ddlDivision.Value))
            {
                query = query.Where(p => p.DivisionId == Convert.ToInt32(ddlDivision.Value));
            }
            e.Result = query;
       }

  
        private void ShowBalances()
        {
            //const string moneyFormat = "###,###,###,##0.00";

            ReportingDataContext db = (ReportingDataContext)dsVouchers.Database;
            decimal openingBalance = db.GetOpeningBalance(Convert.ToInt32(tbHeadOfAccount.Value), DateTime.Parse(tbFromDate.Text)) ?? 0;
            lblOpeningBalance.Text = openingBalance.ToString("C");

            decimal closingBalance = (m_totalReceipts + openingBalance) - m_totalPayments;
            lblClosingBalance.Text = closingBalance.ToString("C");
            if (!string.IsNullOrEmpty(tbHeadOfAccount.Value))
            {
                DateTime? toDate = tbToDate.ValueAsDate;

                // if only fromDate provided,then toDate will be consider as one month from fromDate.  

                if (tbFromDate.Text != null && toDate == DateTime.MinValue)
                {
                    //toDate = tbFromDate.TextAddMonths(1).AddDays(-1);
                    toDate = DateTime.Parse(tbFromDate.Text).AddMonths(1).AddDays(0);
                }
                string reportHeader = string.Format("{0}<br>{1:dd MMMM yyyy} To {2:dd MMMM yyyy}", tbHeadOfAccount.Text, tbFromDate.Text, toDate);
                lblHeadOfAccount.Text = reportHeader;
            }

        }

        protected void gvVouchers_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            GridViewEx gv = (GridViewEx)sender;
            switch (e.Row.RowType)
            {
                case DataControlRowType.DataRow:
                    // TODO: Think if any discrepent voucher should be shown with red color

                    //m_totalReceipts is always equalt to the total of debit side because
                    //whenever some amount is received for a headofaccount that headofaccount will always
                    //be credited and viceversa.
                    m_totalReceipts += System.Convert.ToDecimal((DataBinder.Eval(e.Row.DataItem, "DebitAmount")) ?? 0);
                    m_totalPayments += System.Convert.ToDecimal((DataBinder.Eval(e.Row.DataItem, "CreditAmount")) ?? 0);

                    break;
            }
        }

        protected void gvVouchers_DataBound(object sender, EventArgs e)
        {
            if (gvVouchers.Rows.Count > 0)
            {
                ShowBalances();
            }
        }

        /// <summary>
        /// Ritesh 6th June 2012
        /// Drop down list for station provided which is now manadatory.
        /// Drop down shows stations on which user has right
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dsStations_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {

            using (Eclipse.PhpaLibrary.Database.PIS.PISDataContext db = new Eclipse.PhpaLibrary.Database.PIS.PISDataContext(Eclipse.PhpaLibrary.Reporting.ReportingUtilities.DefaultConnectString))
            {
                var stations = this.GetUserStations();
                var query = (from station in db.Stations
                             where station.StationName != null && station.StationName != string.Empty
                             orderby station.StationName
                             select station).Distinct().ToList();
                if (stations != null)
                {
                    query = query.Where(p => stations.Contains(p.StationId)).ToList();
                }
                e.Result = query;
            }

        }
    }
}
