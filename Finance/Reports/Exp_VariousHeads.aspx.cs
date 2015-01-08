/*
 *  Programming: Eclipse Systems (P) Ltd., NOIDA, INDIA
 *  E-mail: support@eclsys.com
 *
 *  $Workfile:   Exp_VariousHeads.aspx.cs  $
 *  $Revision: 38443 $
 *  $Author: ssingh $
 *  $Date: 2010-12-03 10:58:07 +0530 (Fri, 03 Dec 2010) $
 *  $Modtime:   Jul 30 2008 12:44:32  $
 *
 *  $Log:   S:/Projects/PHPA2/archives/Finance/Reports/Exp_VariousHeads.aspx.cs-arc  $
 * 
 *    Rev 1.34   Jul 30 2008 12:44:44   yjuneja
 * WIP
 * 
 *    Rev 1.33   Jul 24 2008 12:27:30   yjuneja
 * WIP
 * 
 *    Rev 1.32   Jul 24 2008 11:50:22   yjuneja
 * WIP
 * 
 *    Rev 1.31   Jul 19 2008 17:11:52   yjuneja
 * WIP
 * 
 *    Rev 1.30   Jul 09 2008 17:40:56   vraturi
 * PVCS Template Added.
 */
using System;
using System.Linq;
using System.Web.UI.WebControls;
using Eclipse.PhpaLibrary.Reporting;
using Eclipse.PhpaLibrary.Web;

namespace Finance.Reports
{
    public partial class Exp_VariousHeads : PageBase
    {
        private const string HOA_EXPENDITURE = "EXPENDITURE";
        private const string HOA_TOUR_EXPENSEs = "TOUR_EXPENSES";
        //private ReportingDataContext m_db;
        //DateTime dt = new DateTime();

        //protected override void OnLoad(EventArgs e)
        //{
        //    m_db = (ReportingDataContext)dsexp.Database;
        //    dt = System.DateTime.Now.Date;

        //    this.Validate();

        //    if (this.IsValid && Page.IsPostBack)
        //    {
        //        var query = (from vd in m_db.RoVoucherDetails
        //                     where vd.RoJob.RoDivision.DivisionId == Convert.ToInt32(ddlDivisionCode.Value)
        //                     select new JobInfo()
        //                     {
        //                         JobId = vd.RoJob.JobId,
        //                         JobCode = vd.RoJob.JobCode,
        //                         JobDesc = vd.RoJob.Description
        //                     }).Distinct();

        //        lvdsexp.DataSource = query;
        //        lvdsexp.DataBind();
        //    }
        //    base.OnLoad(e);
        //}


        //class JobInfo
        //{
        //    public int JobId { get; set; }
        //    public string JobCode { get; set; }
        //    public string JobDesc { get; set; }
        //}

        /// <summary>
        /// Calculates sum of  Expenditur for current financial year and cumulative Expenditure Job wise
        /// and for each head of account.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        //protected void lvdsexp_ItemDataBound(object sender, ListViewItemEventArgs e)
        //{
        //    GridView gvtest = (GridView)e.Item.FindControl("gvlvdsexp");
        //    Label lbljbid = (Label)e.Item.FindControl("lblJobCode");
        //    Label lbljbdesc = (Label)e.Item.FindControl("lblJobDesc");
        //    ListViewDataItem lvtype = (ListViewDataItem)e.Item;
        //    JobInfo a = (JobInfo)lvtype.DataItem;
        //    switch (e.Item.ItemType)
        //    {
        //        case ListViewItemType.DataItem:
        //            lbljbid.Text = a.JobCode.ToString();
        //            lbljbdesc.Text = a.JobDesc;

        //            gvtest.DataSource = (from vd in m_db.RoVoucherDetails
        //                                 where (vd.RoHeadHierarchy.HeadOfAccountType == HOA_EXPENDITURE ||
        //                                 vd.RoHeadHierarchy.HeadOfAccountType == HOA_TOUR_EXPENSEs) &&
        //                                 vd.JobId == a.JobId
        //                                 group vd by vd.RoHeadHierarchy.HeadOfAccountId into grouping
        //                                 select new
        //                                 {
        //                                     HeadOfAccountId = grouping.Key,
        //                                     DisplayName = grouping.Max(p => p.RoHeadHierarchy.DisplayName),
        //                                     Description = grouping.Max(p => p.RoHeadHierarchy.Description),
        //                                     HeadType = grouping.Max(p => p.RoHeadHierarchy.HeadOfAccountType),
        //                                     Expenditure = (decimal?)(grouping.Where(p => p.RoVoucher.VoucherDate >= dt.FinancialYearStartDate()).Sum(p => p.DebitAmount ?? 0 - p.CreditAmount ?? 0)),
        //                                     ExpenditureOld = grouping.Sum(p => p.DebitAmount ?? 0 - p.CreditAmount ?? 0)
        //                                 });
        //            gvtest.DataBind();
        //            break;

        //        default:
        //            break;
        //    }

        //}

        protected void ds_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            DateTime? date = (DateTime?)tbDate.ValueAsDate;
            if (!btnGo.IsPageValid())
            {
                e.Cancel = true;
                return;
            }         
            //dt = System.DateTime.Now.Date;
            ReportingDataContext db = (ReportingDataContext)ds.Database;

            var query = (from vd in db.RoVoucherDetails
                         where (vd.RoHeadHierarchy.HeadOfAccountType == HOA_EXPENDITURE ||
                         vd.RoHeadHierarchy.HeadOfAccountType == HOA_TOUR_EXPENSEs) &&
                         vd.RoJob.DivisionId == Convert.ToInt32(ddlDivisionCode.Value)
                         && vd.RoVoucher.VoucherDate >= (tbDate.ValueAsDate ?? DateTime.Today.AddYears(-100)) && vd.RoVoucher.VoucherDate <= (txtToDate.ValueAsDate ?? DateTime.Today)
                         //&& (date.HasValue ? (vd.RoVoucher.VoucherDate >= date && vd.RoVoucher.VoucherDate <= date): date==null)

                         group vd by new
                         {
                             JobId = vd.RoJob.JobId,
                             JobCode = vd.RoJob.JobCode,
                             JobDescription = vd.RoJob.Description,
                             HeadOfAccountId = vd.HeadOfAccountId
                         } into grouping
                         orderby grouping.Key.JobId
                         select new
                         {
                             JobCode = grouping.Key.JobCode,
                             JobDescription = grouping.Key.JobDescription,
                             HeadOfAccountId = grouping.Key.HeadOfAccountId,
                             DisplayName = grouping.Max(p => p.RoHeadHierarchy.DisplayName),
                             Description = grouping.Max(p => p.RoHeadHierarchy.Description),
                             HeadType = grouping.Max(p => p.RoHeadHierarchy.HeadOfAccountType),
                             Expenditure = (decimal?)(grouping.Where(p => p.RoVoucher.VoucherDate >= DateTime.Today.FinancialYearStartDate()).Sum(p => p.DebitAmount ?? 0 - p.CreditAmount ?? 0)),
                             ExpenditureOld = grouping.Sum(p => p.DebitAmount ?? 0 - p.CreditAmount ?? 0)
                         });

            e.Result = query;
        }
    }
}

