/*
 *  Programming: Eclipse Systems (P) Ltd., NOIDA, INDIA
 *  E-mail: support@eclsys.com
 *
 *  $Workfile:   ExpenditureAgainstJobType.aspx.cs  $
 *  $Revision: 38224 $
 *  $Author: ssingh $
 *  $Date: 2010-12-01 12:59:37 +0530 (Wed, 01 Dec 2010) $
 *  $Modtime:   Jul 16 2008 16:17:52  $
 *
 *  $Log:   S:/Projects/PHPA2/archives/Finance/Reports/ExpenditureAgainstJobType.aspx.cs-arc  $
 * 
 *    Rev 1.57   Jul 16 2008 16:17:52   pshishodia
 * WIP
 * 
 *    Rev 1.56   Jul 15 2008 16:16:10   pshishodia
 * WIP
 * 
 *    Rev 1.55   Jul 15 2008 16:12:28   pshishodia
 * WIP
 * 
 *    Rev 1.54   Jul 15 2008 16:00:48   pshishodia
 * WIP
 * 
 *    Rev 1.53   Jul 09 2008 17:40:58   vraturi
 * PVCS Template Added.
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
    /// <summary>
    /// The query string variable ReportName should contain one of:
    ///     ExpenditureAgainstContracts, ExpenditureAgainstWorkOrder, ExpenditureAgainstDepartmentallyExecutedJobs.
    ///     
    /// If any other value is passed, an exception is thrown. If no value is passed, then ExpenditureAgainstContracts is assumed.
    /// 
    /// You can also pass HeadOfAccountId inthe query string. In this case, all jobs of the passed head of accounts are displayed.
    /// 
    /// </summary>
    /// <remarks>
    /// Passing both ReportName and HeadOfAccountId is an error cnondition.
    /// </remarks>
    public partial class ExpenditureAgainstJobType : PageBase
    {
        //DateTime date = DateTime.Now.Date;       
        private string _typeFlag = string.Empty;

        protected void ds_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            if (!btnShowReport.IsPageValid())
            {
                e.Cancel = true;
            }
            else
            {
                ReportingDataContext db = (ReportingDataContext)ds.Database;
                DateTime? m_tbTodate = tbToDate.ValueAsDate;
                DateTime dateMonthStart = m_tbTodate.Value.MonthStartDate();

                this.Title += string.Format(" From {0:d MMMM yyyy} to {1:d MMMM yyyy}", tbFromDate.ValueAsDate, tbToDate.ValueAsDate);

                e.Result = (from vd in db.RoVoucherDetails
                            where HeadOfAccountHelpers.JobExpenses.Concat(HeadOfAccountHelpers.JobAdvances).Contains(vd.RoHeadHierarchy.HeadOfAccountType) &&
                                //where (vd.RoHeadHierarchy.HeadOfAccountType == "EXPENDITURE" ||
                                //        vd.RoHeadHierarchy.HeadOfAccountType == "PARTY_ADVANCE" ||
                                //        vd.RoHeadHierarchy.HeadOfAccountType == "MATERIAL_ADVANCE" ||
                                //        vd.RoHeadHierarchy.HeadOfAccountType == "TOUR_EXPENSES") &&
                                   vd.RoJob.TypeFlag == _typeFlag &&
                                   vd.RoVoucher.VoucherDate >= tbFromDate.ValueAsDate &&
                                   vd.RoVoucher.VoucherDate <= tbToDate.ValueAsDate
                            group vd by vd.RoJob into grouping
                            select new
                            {
                                DivisionId = grouping.Key.DivisionId,
                                DivisionName = grouping.Key.RoDivision.DivisionName,
                                HeadOfAccountId = (int?)grouping.Key.RoHeadHierarchy.HeadOfAccountId,
                                HeadofAccount = grouping.Key.RoHeadHierarchy.DisplayName,
                                JobCode = grouping.Key.JobCode,
                                Description = grouping.Key.Description,
                                ContractorName = grouping.Key.RoContractor.ContractorName,
                                ContractorCode = grouping.Key.RoContractor.ContractorCode,
                                SanctionedAmount = grouping.Key.SanctionedAmount,
                                RevisedContract = grouping.Key.RevisedContract,
                                AwardAmount = grouping.Key.RevisedContract ?? grouping.Key.SanctionedAmount,
                                AmountMonth = (decimal?)grouping.Key.RoVoucherDetails
                                    .Where(p => HeadOfAccountHelpers.JobExpenses.Contains(p.RoHeadHierarchy.HeadOfAccountType) &&
                                        p.RoVoucher.VoucherDate >= dateMonthStart &&
                                        p.RoVoucher.VoucherDate <= tbToDate.ValueAsDate)
                                    .Sum(p => p.DebitAmount ?? 0 - p.CreditAmount ?? 0),
                                //AmountMonth = grouping.Key.RoVoucherDetails.Sum(p => ((p.RoHeadHierarchy.HeadOfAccountType == "EXPENDITURE" ||
                                //    p.RoHeadHierarchy.HeadOfAccountType == "TOUR_EXPENSES") && p.RoVoucher.VoucherDate >= dateMonthStart
                                //    && p.RoVoucher.VoucherDate <= tbToDate.ValueAsDate ? p.DebitAmount ?? 0 - p.CreditAmount ?? 0 : 0)),

                                AmountProgressive = (decimal?)grouping.Key.RoVoucherDetails
                                    .Where(p => HeadOfAccountHelpers.JobExpenses.Contains(p.RoHeadHierarchy.HeadOfAccountType) &&
                                        p.RoVoucher.VoucherDate >= tbFromDate.ValueAsDate && p.RoVoucher.VoucherDate <= tbToDate.ValueAsDate)
                                    .Sum(p => p.DebitAmount ?? 0 - p.CreditAmount ?? 0),

                                //AmountProgressive = grouping.Key.RoVoucherDetails.Sum(p => ((p.RoHeadHierarchy.HeadOfAccountType == "EXPENDITURE" ||
                                //    p.RoHeadHierarchy.HeadOfAccountType == "TOUR_EXPENSES") && p.RoVoucher.VoucherDate >= tbFromDate.ValueAsDate && p.RoVoucher.VoucherDate <= tbToDate.ValueAsDate
                                //    ? p.DebitAmount ?? 0 - p.CreditAmount ?? 0 : 0)),

                                AdvanceOutstanding = grouping.Key.RoVoucherDetails.Sum(p => ((p.RoHeadHierarchy.HeadOfAccountType == "PARTY_ADVANCE" ||
                                    p.RoHeadHierarchy.HeadOfAccountType == "MATERIAL_ADVANCE") && p.RoVoucher.VoucherDate >= tbFromDate.ValueAsDate && p.RoVoucher.VoucherDate <= tbToDate.ValueAsDate ? p.DebitAmount ?? 0 : 0)) -
                                    grouping.Key.RoVoucherDetails.Sum(p => p.RoHeadHierarchy.HeadOfAccountType == "PARTY_ADVANCE" && p.RoVoucher.VoucherDate >= tbFromDate.ValueAsDate && p.RoVoucher.VoucherDate <= tbToDate.ValueAsDate
                                    ? p.CreditAmount ?? 0 : 0),

                                    // Exp&TE(D - C)
                                Total = grouping.Key.RoVoucherDetails.Sum(p => ((p.RoHeadHierarchy.HeadOfAccountType == "EXPENDITURE" ||
                                    p.RoHeadHierarchy.HeadOfAccountType == "TOUR_EXPENSES") && p.RoVoucher.VoucherDate >= tbFromDate.ValueAsDate && p.RoVoucher.VoucherDate <= tbToDate.ValueAsDate
                                    ? p.DebitAmount ?? 0 - p.CreditAmount ?? 0 : 0))
                                    + grouping.Key.RoVoucherDetails.Sum(p => ((p.RoHeadHierarchy.HeadOfAccountType == "PARTY_ADVANCE" ||
                                    p.RoHeadHierarchy.HeadOfAccountType == "MATERIAL_ADVANCE") && p.RoVoucher.VoucherDate >= tbFromDate.ValueAsDate && p.RoVoucher.VoucherDate <= tbToDate.ValueAsDate ? p.DebitAmount ?? 0 : 0)) -
                                    grouping.Key.RoVoucherDetails.Sum(p => p.RoHeadHierarchy.HeadOfAccountType == "PARTY_ADVANCE" && p.RoVoucher.VoucherDate >= tbFromDate.ValueAsDate && p.RoVoucher.VoucherDate <= tbToDate.ValueAsDate
                                    ? p.CreditAmount ?? 0 : 0)
                            }).ToLookup(p => new { DivisionId = p.DivisionId, DivisionName = p.DivisionName });
                return;
            }
        }
        /// <summary>
        /// Select the Division name for List view Control.
        /// </summary>
        /// <param name="e"></param>

        protected override void OnLoad(EventArgs e)
        {
            string reportName = Request.QueryString["ReportName"];
            if (!Page.IsPostBack)
            {
                //tbDate.Value = DateTime.Today;
                tbFromDate.Value = DateTime.Today.AddMonths(-1).MonthStartDate().ToShortDateString();
                tbToDate.Value = DateTime.Today.ToShortDateString();
            }

            if (string.IsNullOrEmpty(reportName))
            {
                reportName = "C";
            }

            switch (reportName)
            {
                case "C":
                    _typeFlag = "C";
                    Page.Title = "Expenditure against Contracts";
                    break;

                case "W":
                    _typeFlag = "W";
                    Page.Title = "Expenditure against Work Orders";
                    break;

                case "D":
                    _typeFlag = "D";
                    Page.Title = "Expenditure against Departmentally Executed Jobs";
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }

            View view = (View)MultiView1.FindControl(reportName);
            MultiView1.SetActiveView(view);

            //lvExpenditureAgainstContract.DataSource = query;
            //lvExpenditureAgainstContract.DataBind();
            base.OnLoad(e);
        }

        /// <summary>
        /// Display the information on the basis of choosen parameter for corresponding Division Name.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lvExpenditureAgainstContract_ItemCreated(object sender, ListViewItemEventArgs e)
        {
            switch (e.Item.ItemType)
            {
                case ListViewItemType.DataItem:
                    ListViewDataItem lvdi = (ListViewDataItem)e.Item;
                    GridView gvExpenditureAgainstContract = (GridView)lvdi.FindControl("gvExpenditureAgainstContract");

                    gvExpenditureAgainstContract.DataSource = lvdi.DataItem;
                    break;
            }
        }

        /// <summary>
        /// Display the Grid Caption.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lblCaption_PreRender(object sender, EventArgs e)
        {
            Label lblCaption = (Label)sender;
            lblCaption.Visible = true;
            lblCaption.Text = string.Format("<br/><b>Statement Showing Expenditure and Sanctioned Amount from {0:dd MMMM yyyy} to {1:dd MMMM yyyy}</b>", tbFromDate.ValueAsDate, tbToDate.ValueAsDate);
        }

        protected void gvExpenditureAgainstContract_DataBinding(object sender, EventArgs e)
        {
            GridView gv = (GridView)sender;
            if (_typeFlag == "D")
            {
                DataControlField columnAO = (from DataControlField col in gv.Columns
                                             where col.AccessibleHeaderText == "AdvanceOutstanding"
                                             select col).Single();
                columnAO.Visible = false;
                DataControlField columnT = (from DataControlField col in gv.Columns
                                            where col.AccessibleHeaderText == "Total"
                                            select col).Single();
                columnT.Visible = false;
            }
        }

        protected void gvExpenditureAgainstContract_RowCreated(object sender, GridViewRowEventArgs e)
        {
            GridView gvExpenditureAgainstContract = (GridView)sender;
            DateTime? m_tbTodate = tbToDate.ValueAsDate;
            DateTime dateMonthStart = m_tbTodate.Value.MonthStartDate();
            switch (e.Row.RowType)
            {
                case DataControlRowType.Header:
                    MultiBoundField mfAM = (MultiBoundField)(from DataControlField col in gvExpenditureAgainstContract.Columns
                                                             where col.AccessibleHeaderText == "AmountMonth"
                                                             select col).Single();
                    mfAM.HeaderToolTip = string.Format("Expenditure incurred during the period from {0:dd MMMM yyyy} to {1:dd MMMM yyyy}", dateMonthStart, tbToDate.ValueAsDate);

                    MultiBoundField mfAP = (MultiBoundField)(from DataControlField col in gvExpenditureAgainstContract.Columns
                                                             where col.AccessibleHeaderText == "AmountProgressive"
                                                             select col).Single();
                    mfAP.HeaderToolTip = string.Format("Expenditure incurred from {0:dd MMMM yyyy} to {1:dd MMMM yyyy}", tbFromDate.ValueAsDate, tbToDate.ValueAsDate);
                    break;
            }
        }

        protected void gvExpenditureAgainstContract_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            switch (e.Row.RowType)
            {
                case DataControlRowType.Footer:
                    // Footer should not print on each page
                    e.Row.TableSection = TableRowSection.TableBody;
                    break;
            }
        }
    }
}
