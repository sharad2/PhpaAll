
/*
 *  Programming: Eclipse Systems (P) Ltd., NOIDA, INDIA
 *  E-mail: support@eclsys.com
 *
 *  $Workfile:   CashBook.aspx.cs  $
 *  $Revision: 37722 $
 *  $Author: ssinghal $
 *  $Date: 2010-11-23 20:00:52 +0530 (Tue, 23 Nov 2010) $
 *
 *  $Id: CashBook.aspx.cs 37722 2010-11-23 14:30:52Z ssinghal $
 * 
 */
using System;
using System.Web.UI;
using Eclipse.PhpaLibrary.Reporting;
using Eclipse.PhpaLibrary.Web;

//Code reviewed by Mayank Sharama on 12th May 2008 
namespace PhpaAll.Reports
{
    
    public partial class CashBook : PageBase
    {
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (!Page.IsPostBack)
            {
                tbDateFrom.Text = DateTime.Now.MonthStartDate().AddMonths(-1).ToShortDateString();
                tbDateTo.Text = DateTime.Today.ToShortDateString();
            }
            Page.Validate();
            if (Page.IsValid)
            {
                if (!string.IsNullOrEmpty(tbHeadOfAccount.Value))
                {
                    this.DataBind();
                }
            }
            
            tbHeadOfAccount.Focus();
        }

        public override void DataBind()
        {

            // Do not query if it is a asyncpostback

            //if (ScriptManager.GetCurrent(this.Page).IsInAsyncPostBack)
            //{
            //    return;
            //}
            ctlVoucherDetail.FocusHeadOfAccountId = Convert.ToInt32(tbHeadOfAccount.Value);
            // By defalt From Date is previous month's starting date.
            // if only fromDate provided,then toDate will be consider as one month after from fromDate. 
            ctlVoucherDetail.FromDate = Convert.ToDateTime(this.tbDateFrom.ValueAsDate);
            ctlVoucherDetail.ToDate = Convert.ToDateTime(this.tbDateTo.ValueAsDate);

            string reportHeader = string.Format("{0}<br>{1:dd MMMM yyyy} To {2:dd MMMM yyyy}", tbHeadOfAccount.Text,
                ctlVoucherDetail.FromDate, ctlVoucherDetail.ToDate);
            lblHeadOfAccount.Text = reportHeader;

            base.DataBind();
        } 
    }
}

