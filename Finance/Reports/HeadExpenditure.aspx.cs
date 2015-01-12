/*
 *  Programming: Eclipse Systems (P) Ltd., NOIDA, INDIA
 *  E-mail: support@eclsys.com
 *
 *  $Workfile:   HeadExpenditure.aspx.cs  $
 *  $Revision: 37146 $
 *  $Author: bkumar $
 *  $Date: 2010-11-10 17:49:45 +0530 (Wed, 10 Nov 2010) $
 *  $Modtime:   Jul 22 2008 12:24:34  $
 *
 *  $Log:   S:/Projects/PHPA2/archives/Finance/Reports/HeadExpenditure.aspx.cs-arc  $
 * 
 *    Rev 1.9   Jul 22 2008 12:26:56   yjuneja
 * WIP
 * 
 *    Rev 1.8   Jul 09 2008 17:40:58   vraturi
 * PVCS Template Added.
 */
using System;
using System.Linq;
using Eclipse.PhpaLibrary.Reporting;
using Eclipse.PhpaLibrary.Web;
using System.Web.UI.WebControls;
using EclipseLibrary.Web.JQuery.Input;

namespace Finance.Reports
{
    public partial class HeadExpenditure : PageBase
    {
        DateTime? dt;

        protected override void OnLoad(EventArgs e)
        {
            if (!IsPostBack)
            {
                dt = DateTime.Today;
                tbDate.Value = dt.Value.ToShortDateString();
            }
            if (!btnShow.IsPageValid())
            {
                return;
            }
            
            dt = Convert.ToDateTime(tbDate.Value).AddMonths(4);

            ReportingDataContext db = (ReportingDataContext)dsHeadsExp.Database;

            gvHeadsExp.DataSource = from vd in db.RoVoucherDetails
                                    where HeadOfAccountHelpers.JobExpenses.Contains(vd.RoHeadHierarchy.HeadOfAccountType) &&
                                    //where (vd.RoHeadHierarchy.HeadOfAccountType == "Expenditure" ||
                                             vd.RoHeadHierarchy.HeadOfAccountId == 1373 ||
                                             vd.RoHeadHierarchy.HeadOfAccountId == 1374 ||
                                             vd.RoHeadHierarchy.HeadOfAccountId == 1375 ||
                                             vd.RoHeadHierarchy.HeadOfAccountId == 1376 ||
                                             vd.RoHeadHierarchy.HeadOfAccountId == 1377 ||
                                             vd.RoHeadHierarchy.HeadOfAccountId == 1379 ||
                                             vd.RoHeadHierarchy.HeadOfAccountId == 1381 ||
                                             vd.RoHeadHierarchy.HeadOfAccountId == 1382 ||
                                             vd.RoHeadHierarchy.HeadOfAccountId == 1385 ||
                                             vd.RoHeadHierarchy.HeadOfAccountId == 1395 ||
                                             vd.RoHeadHierarchy.HeadOfAccountId == 1397 ||
                                             vd.RoHeadHierarchy.HeadOfAccountId == 1534 ||
                                             vd.RoHeadHierarchy.HeadOfAccountId == 1593 ||
                                             vd.RoHeadHierarchy.HeadOfAccountId == 1594 ||
                                             vd.RoHeadHierarchy.HeadOfAccountId == 1595 ||
                                             vd.RoHeadHierarchy.HeadOfAccountId == 1596 ||
                                             vd.RoHeadHierarchy.HeadOfAccountId == 1597 ||
                                             vd.RoHeadHierarchy.HeadOfAccountId == 1686 ||
                                             vd.RoHeadHierarchy.HeadOfAccountId == 1687 ||
                                             vd.RoHeadHierarchy.HeadOfAccountId == 1688 ||
                                             vd.RoHeadHierarchy.HeadOfAccountId == 1689 ||
                                             vd.RoHeadHierarchy.HeadOfAccountId == 1690 ||
                                             vd.RoHeadHierarchy.HeadOfAccountId == 1691 ||
                                             vd.RoHeadHierarchy.HeadOfAccountId == 1769 ||
                                             vd.RoHeadHierarchy.HeadOfAccountId == 1846 ||
                                             vd.RoHeadHierarchy.HeadOfAccountId == 1387) &&
                                            vd.RoVoucher.VoucherDate >= dt.Value.FinancialYearStartDate().AddYears(-1) &&
                                            vd.RoVoucher.VoucherDate <= dt.Value.FinancialYearEndDate().AddYears(-1)
                                    group vd by vd.RoHeadHierarchy.TopParentName into grouping
                                    orderby grouping.Key
                                    select new
                                    {
                                        Head = grouping.Key,
                                        Amount = grouping.Sum(p => p.DebitAmount ?? 0 - p.CreditAmount ?? 0)
                                        
                                    };
            gvHeadsExp.DataBind();
            gvHeadsExpTillDate.DataSource = from vd in db.RoVoucherDetails
                                      where HeadOfAccountHelpers.JobExpenses.Contains(vd.RoHeadHierarchy.HeadOfAccountType) &&
                                     //where (vd.RoHeadHierarchy.HeadOfAccountType == "Expenditure" ||
                                             vd.RoHeadHierarchy.HeadOfAccountId == 1373 || 
                                             vd.RoHeadHierarchy.HeadOfAccountId == 1374 || 
                                             vd.RoHeadHierarchy.HeadOfAccountId == 1375 || 
                                             vd.RoHeadHierarchy.HeadOfAccountId == 1376 || 
                                             vd.RoHeadHierarchy.HeadOfAccountId == 1377 || 
                                             vd.RoHeadHierarchy.HeadOfAccountId == 1379 || 
                                             vd.RoHeadHierarchy.HeadOfAccountId == 1381 || 
                                             vd.RoHeadHierarchy.HeadOfAccountId == 1382 || 
                                             vd.RoHeadHierarchy.HeadOfAccountId == 1385 || 
                                             vd.RoHeadHierarchy.HeadOfAccountId == 1395 ||
                                             vd.RoHeadHierarchy.HeadOfAccountId == 1397 ||
                                             vd.RoHeadHierarchy.HeadOfAccountId == 1534 ||
                                             vd.RoHeadHierarchy.HeadOfAccountId == 1593 ||
                                             vd.RoHeadHierarchy.HeadOfAccountId == 1594 ||
                                             vd.RoHeadHierarchy.HeadOfAccountId == 1595 ||
                                             vd.RoHeadHierarchy.HeadOfAccountId == 1596 ||
                                             vd.RoHeadHierarchy.HeadOfAccountId == 1597 ||
                                             vd.RoHeadHierarchy.HeadOfAccountId == 1686 ||
                                             vd.RoHeadHierarchy.HeadOfAccountId == 1687 ||
                                             vd.RoHeadHierarchy.HeadOfAccountId == 1688 ||
                                             vd.RoHeadHierarchy.HeadOfAccountId == 1689 ||
                                             vd.RoHeadHierarchy.HeadOfAccountId == 1690 ||
                                             vd.RoHeadHierarchy.HeadOfAccountId == 1691 ||
                                             vd.RoHeadHierarchy.HeadOfAccountId == 1769 ||
                                             vd.RoHeadHierarchy.HeadOfAccountId == 1846 ||
                                             vd.RoHeadHierarchy.HeadOfAccountId == 1387) &&
                                             vd.RoVoucher.VoucherDate >= dt.Value.FinancialYearStartDate() &&
                                             vd.RoVoucher.VoucherDate <= tbDate.ValueAsDate
                                     group vd by vd.RoHeadHierarchy.TopParentName into grouping
                                     orderby grouping.Key
                                     select new
                                     {
                                         Head = grouping.Key,
                                         Amount = grouping.Sum(p => p.DebitAmount ?? 0 - p.CreditAmount ?? 0)
                                     };
            gvHeadsExpTillDate.DataBind();
            base.OnLoad(e);
        }

        protected void gvHeadsExp_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            switch (e.Row.RowType)
            {
                case DataControlRowType.Header:
                    gvHeadsExp.Caption = string.Format("<b>Heads Expenditure for Financial Year {0:dd MMMM yyyy} to {1:dd MMMM yyyy}</b>", dt.Value.FinancialYearStartDate().AddYears(-1), dt.Value.FinancialYearEndDate().AddYears(-1));
                    break;
            }
        }

        protected void gvHeadsExpTillDate_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            switch (e.Row.RowType)
            {
                   
                case DataControlRowType.Header:
                    gvHeadsExpTillDate.Caption = string.Format("<b>Heads Expenditure from {0:dd MMMM yyyy}  til {1:dd MMMM yyyy}</b>", dt.Value.FinancialYearStartDate(), tbDate.ValueAsDate);
                    break;
            }
        }

        protected void btnShow_Click(object sender, EventArgs e)
        {
            //if (this.IsValid)
            //{
            ButtonEx btn = (ButtonEx)sender;
            if (btn.IsPageValid())
            {
                gvHeadsExp.DataBind();
            }            
        }
    }
}
