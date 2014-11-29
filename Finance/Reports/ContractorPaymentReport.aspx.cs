
/*
 *  Programming: Eclipse Systems (P) Ltd., NOIDA, INDIA
 *  E-mail: support@eclsys.com
 *
 *  $Workfile:   FundPositionReport.aspx.cs  $
 *  $Revision: 626 $
 *  $Author: hsingh $
 *  $Date: 2013-10-18 17:44:25 +0530 (Fri, 18 Oct 2013) $
 *  $Modtime:   Jul 21 2008 15:02:00  $
 *
 *  $Log:   S:/Projects/PHPA2/archives/Finance/Reports/ReceiptandPayment.aspx.cs-arc  $
 */
using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Eclipse.PhpaLibrary.Reporting;
using Eclipse.PhpaLibrary.Web;
using EclipseLibrary.Web.UI;

namespace Finance.Reports
{
    public partial class ContractorPaymentReport : PageBase
    {
        DateTime m_dt_yr = new DateTime();
        decimal? _amount = 0, _advance = 0, _adjustment = 0, _cTax = 0, _securityDeposit = 0, _interest = 0, _others = 0, _totalRecovery = 0, _netPayment = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                m_dt_yr = DateTime.Today;
                tbFromDate.Text = m_dt_yr.MonthStartDate().ToShortDateString();
                tbToDate.Text = m_dt_yr.MonthEndDate().ToShortDateString();
            }
            else
            {
                if (tbJobs.Text == "" && tbContractors.Text == "")
                {
                    lblMessage.Visible = true;
                    lblMessage.Text = "Either Contractor or Job is required **";
                }
                else
                    GetJobs();
            }
        }

        //private IQueryable<RoVoucher> m_query;
        protected override void OnLoad(EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                tbFromDate.Text = DateTime.Now.MonthStartDate().AddMonths(-1).ToShortDateString();
            }
            tbContractors.Focus();
            base.OnLoad(e);
        }

        /// <summary>
        /// Class for query.
        /// </summary>
        class ContractorRecoverdPayment
        {
            public int VoucherId { get; set; }
            public string VoucherCode { get; set; }
            public DateTime VoucherDate { get; set; }
            public String JobCode { get; set; }
            public string Particulars { get; set; }
            public RoJob Job { get; set; }
            public decimal? AdmittedAmount { get; set; }
            public decimal? ContractorTax { get; set; }
            public decimal? AdvancePaid { get; set; }
            public decimal? SecurityDeposit { get; set; }
            public decimal? AdvanceAdjusted { get; set; }
            public decimal? MaterialRecoverd { get; set; }
            public decimal? InterestRecoverd { get; set; }
            public decimal? OtherRecovery { get; set; }
            public decimal? ContractorAdvanceAdjusted
            {
                get
                {
                    return AdvanceAdjusted + MaterialRecoverd;
                }
            }
            public Decimal? TotalRecovery
            {
                get
                {
                    return ContractorTax + SecurityDeposit + MaterialRecoverd +
                            InterestRecoverd + AdvanceAdjusted + OtherRecovery;
                }
            }
            public Decimal? NetPayment
            {
                get
                {
                    return AdmittedAmount + AdvancePaid - TotalRecovery;
                }
            }
        }


        /// <summary>
        /// Sharad 17 Aug 2010: Admitted Amount = Debit - Credit. Earlier it was Debit only. This done at Lobeysa
        /// based on Rajesh Pant's feedback.
        /// </summary>
        /// <returns></returns>
        private IEnumerable<ContractorRecoverdPayment> QueryIterator(IQueryable<RoVoucher> query, int jobId)
        {
            foreach (RoVoucher v in query)
            {
                ContractorRecoverdPayment crp = new ContractorRecoverdPayment();
                crp.Particulars = v.Particulars;
                crp.VoucherDate = v.VoucherDate;
                crp.VoucherCode = v.VoucherCode;
                crp.VoucherId = v.VoucherId;
                crp.AdmittedAmount = (decimal?)v.RoVoucherDetails
                    .Where(p => (p.RoHeadHierarchy.HeadOfAccountType == "EXPENDITURE" ||
                                        p.RoHeadHierarchy.HeadOfAccountType == "TOUR_EXPENSES")
                                        && p.JobId == jobId)
                    .Sum(p => p.DebitAmount ?? 0 - p.CreditAmount ?? 0);
                crp.AdvancePaid = (decimal?)v.RoVoucherDetails.Sum(p => (((p.RoHeadHierarchy.HeadOfAccountType == "PARTY_ADVANCE" ||
                                        p.RoHeadHierarchy.HeadOfAccountType == "MATERIAL_ADVANCE") && p.JobId == jobId)
                                        ? p.DebitAmount ?? 0 : 0));
                crp.ContractorTax = (decimal?)v.RoVoucherDetails.Sum(p => ((p.RoHeadHierarchy.HeadOfAccountType == "BIT" && p.JobId == jobId)
                                        ? (p.CreditAmount ?? 0 - p.DebitAmount ?? 0) : 0));
                crp.SecurityDeposit = (decimal?)v.RoVoucherDetails.Sum(p => ((p.RoHeadHierarchy.HeadOfAccountType == "SD"
                                            && p.JobId == jobId) ? (p.CreditAmount ?? 0 - p.DebitAmount ?? 0) : 0));
                crp.AdvanceAdjusted = (decimal?)v.RoVoucherDetails.Sum(p => ((p.RoHeadHierarchy.HeadOfAccountType == "PARTY_ADVANCE"
                                            && p.JobId == jobId)
                                            ? p.CreditAmount ?? 0 : 0));
                crp.MaterialRecoverd = (decimal?)v.RoVoucherDetails.Sum(p => ((p.RoHeadHierarchy.HeadOfAccountType == "MATERIAL_ADVANCE"
                                            && p.JobId == jobId)
                                            ? p.CreditAmount ?? 0 : 0));
                crp.InterestRecoverd = (decimal?)v.RoVoucherDetails.Sum(p => ((p.RoHeadHierarchy.HeadOfAccountType == "INTEREST"
                                            && p.JobId == jobId)
                                            ? p.CreditAmount ?? 0 : 0));
                crp.OtherRecovery = (decimal?)v.RoVoucherDetails.Sum(p => (p.RoHeadHierarchy.HeadOfAccountType != "BIT" &&
                                            p.RoHeadHierarchy.HeadOfAccountType != "SD" &&
                                            p.RoHeadHierarchy.HeadOfAccountType != "PARTY_ADVANCE" &&
                                            p.RoHeadHierarchy.HeadOfAccountType != "MATERIAL_ADVANCE" &&
                                            p.RoHeadHierarchy.HeadOfAccountType != "INTEREST" &&
                                            p.RoHeadHierarchy.HeadOfAccountType != "BANKNU" &&
                                            p.RoHeadHierarchy.HeadOfAccountType != "BANKFE" &&
                                            p.RoHeadHierarchy.HeadOfAccountType != "EXPENDITURE" &&
                                            p.RoHeadHierarchy.HeadOfAccountType != "TOUR_EXPENSES" &&
                                            p.JobId == jobId)
                                            ?
                                            (p.CreditAmount ?? 0 - p.DebitAmount ?? 0)
                                            :
                                            0);
                yield return crp;
            }
        }

        /// <summary>
        /// Data source of formView & gridview is set for each item of repeater
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rptContarctorPayment_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            GridView gridView = new GridView();
            Label lblOpeiningBalance = new Label();
            Label lblClosingBalance = new Label();
            Label lblopenbalamt = new Label();
            gridView = ((GridView)(e.Item.FindControl("gvContractorPayment")));
            FormView formView2 = new FormView();
            formView2 = ((FormView)(e.Item.FindControl("fvContractorJob")));
            lblOpeiningBalance = ((Label)(e.Item.FindControl("lblOpeningBalance")));
            lblClosingBalance = ((Label)(e.Item.FindControl("lbldiffrence")));
            lblopenbalamt = ((Label)(e.Item.FindControl("lblbaldates")));
            //lblAmount = ((Label)(e.Item.FindControl("lblAmount")));
            switch (e.Item.ItemType)
            {
                case ListItemType.Item:
                case ListItemType.AlternatingItem:
                    string jobId = e.Item.DataItem.ToString();
                    // = = = = = = = 
                    if (!(formView2 == null))
                    {
                        //Calling method to fill FormView
                        FillFormView(Convert.ToInt32(jobId), formView2);

                    }
                    if (!(gridView == null))
                    {
                        //Calling method to fill GriDview
                        FillGridView(Convert.ToInt32(jobId), gridView, lblOpeiningBalance, lblClosingBalance, lblopenbalamt);
                    }
                    break;
            }
            dvGrand.Visible = true;
            lblAmount.Text = string.Format("{0:N2}", _amount);
            lblAdvance.Text = string.Format("{0:N2}", _advance);
            lblAdjustmenet.Text = string.Format("{0:N2}", _adjustment);
            lblConTax.Text = string.Format("{0:N2}", _cTax);
            lblSD.Text = string.Format("{0:N2}", _securityDeposit);
            lblInterest.Text = string.Format("{0:N2}", _interest);
            lblOthers.Text = string.Format("{0:N2}", _others);
            lblTotalRecovery.Text = string.Format("{0:N2}", _totalRecovery);
            lblNetPayment.Text = string.Format("{0:N2}", _netPayment);

        }
        /// <summary>
        /// Get jobs of passed contactor
        /// </summary>
        protected void GetJobs()
        {
            int selectJob, selectContractor;
            if (!string.IsNullOrEmpty(tbJobs.Value))
            {
                selectJob = Convert.ToInt32(tbJobs.Value);
            }
            else
            {
                selectJob = 0;
            }
            if (!string.IsNullOrEmpty(tbContractors.Value))
            {
                selectContractor = Convert.ToInt32(tbContractors.Value);
            }
            else
            {
                selectContractor = 0;
            }
            if ((!string.IsNullOrEmpty(tbJobs.Value)) && (!string.IsNullOrEmpty(tbContractors.Value)))
            {
                selectJob = Convert.ToInt32(tbJobs.Value);
                selectContractor = Convert.ToInt32(tbContractors.Value);
            }
            using (ReportingDataContext db = new ReportingDataContext(ReportingUtilities.DefaultConnectString))
            {

                var jobKey = (from job in db.RoJobs
                              where (selectJob == 0 || job.JobId == selectJob)
                              && (selectContractor == 0 || job.ContractorId == selectContractor)
                              select
                                 job.JobId
                         ).ToList();
                rptContarctorPayment.DataSource = jobKey;
                rptContarctorPayment.DataBind();
            }

        }


        /// <summary>
        /// Set datasource of formview
        /// </summary>
        /// <param name="jobId">JobId</param>
        /// <param name="frmView"></param>
        protected void FillFormView(int jobId, FormView frmView)
        {
            using (ReportingDataContext db = new ReportingDataContext(ReportingUtilities.DefaultConnectString))
            {
                var query = from rojob in db.RoJobs
                            where rojob.JobId == jobId
                            select new
                            {
                                JobCode = rojob.JobCode,
                                Description = rojob.Description,
                                SanctionNumber = rojob.SanctionNumber,
                                WorkOrderNumber = rojob.WorkOrderNumber,
                                CommencementDate = rojob.CommencementDate,
                                ContractorCode = rojob.RoContractor.ContractorCode,
                                ContractorName = rojob.RoContractor.ContractorName,
                                ContractAmount = rojob.ContractAmount,
                                SanctionDate = rojob.SanctionDate,
                                SanctionedAmount = rojob.SanctionedAmount,
                                RevisedSanction = rojob.RevisedSanction,
                                WorkOrderDate = rojob.WorkOrderDate,
                                CompletionDate = rojob.CompletionDate,
                                AwardDate = rojob.AwardDate,
                                RevisedContract = rojob.RevisedContract
                            };
                frmView.DataSource = query;
                frmView.DataBind();

            }
        }

        /// <summary>
        /// Set DataSource of GridView
        /// </summary>
        /// <param name="jobId"></param>
        /// <param name="gridView"></param>
        /// <param name="lblOpeningBalance"></param>
        /// <param name="lblDifference"></param>
        protected void FillGridView(int jobId, GridView gridView, Label lblOpeningBalance, Label lblDifference, Label lblbaldates)
        {
            DateTime fromDate = Convert.ToDateTime(tbFromDate.ValueAsDate);
            DateTime toDate = Convert.ToDateTime(tbToDate.ValueAsDate);
            if (fromDate != DateTime.MinValue && toDate == DateTime.MinValue)
            {
                toDate = fromDate.AddMonths(1);
                tbToDate.Text = toDate.ToShortDateString();
            }
            using (ReportingDataContext db = new ReportingDataContext(ReportingUtilities.DefaultConnectString))
            {
                DataLoadOptions dlo = new DataLoadOptions();
                dlo.LoadWith<RoVoucher>(vd => vd.RoVoucherDetails);
                dlo.LoadWith<RoVoucherDetail>(vd => vd.RoHeadHierarchy);
                dlo.LoadWith<RoJob>(job => job.RoContractor);
                db.LoadOptions = dlo;

                var query = (from vd in db.RoVoucherDetails
                             where vd.JobId == jobId &&
                             vd.RoHeadHierarchy.HeadOfAccountType != "EDGOI" &&
                             vd.RoHeadHierarchy.HeadOfAccountType != "EDRGOB" &&
                             vd.RoVoucher.VoucherDate < fromDate &&
                             vd.RoJob.ContractorId != null
                             group vd by vd.RoVoucher into grp
                             select grp.Key);
                lblOpeningBalance.Text = string.Format("Opening Balance : {0:N2}", this.QueryIterator(query, jobId).Sum(p => p.NetPayment));
                lblbaldates.Text = string.Format("<span style='font-size:medium; font-weight:bold'>Payment to Contractor from {0:dd MMMM yyyy} {1}</span>", tbFromDate.ValueAsDate, tbToDate.ValueAsDate.HasValue ? " to " + tbToDate.ValueAsDate.Value.ToString("dd MMMM yyyy") : null);
                query = (from vd in db.RoVoucherDetails
                         where vd.JobId == jobId &&
                         vd.RoHeadHierarchy.HeadOfAccountType != "EDGOI" &&
                         vd.RoHeadHierarchy.HeadOfAccountType != "EDRGOB" &&
                         (vd.RoVoucher.VoucherDate >= fromDate &&
                         vd.RoVoucher.VoucherDate <= toDate) &&
                         vd.RoJob.ContractorId != null
                         group vd by vd.RoVoucher into grp
                         select grp.Key);
                var list = this.QueryIterator(query, jobId).ToList();
                gridView.DataSource = list;

                _amount = _amount + list.Sum(p => p.AdmittedAmount);
                _advance = _advance + list.Sum(p => p.AdvancePaid);
                _adjustment = _adjustment + list.Sum(p => p.AdvanceAdjusted);
                _cTax = _cTax + list.Sum(p => p.ContractorTax);
                _securityDeposit = _securityDeposit + list.Sum(p => p.SecurityDeposit);
                _interest = _interest + list.Sum(p => p.InterestRecoverd);
                _others = _others + list.Sum(p => p.OtherRecovery);
                _totalRecovery = _totalRecovery + list.Sum(p => p.TotalRecovery);
                _netPayment = _netPayment + list.Sum(p => p.NetPayment);
                gridView.DataBind();
                if (gridView.Rows.Count == 0)
                { lblOpeningBalance.Text = "No Data Found"; }
                else
                {
                    MultiBoundField advancePaid = (MultiBoundField)(from DataControlField col in gridView.Columns
                                                                    where col.AccessibleHeaderText == "AdvancePaid"
                                                                    select col).Single();
                    MultiBoundField advanceAdjusted = (MultiBoundField)(from DataControlField col in gridView.Columns
                                                                        where col.AccessibleHeaderText == "ContractorAdvanceAdjusted"
                                                                        select col).Single();

                    decimal? adv = Convert.ToDecimal(advancePaid.SummaryValues[0].Value);
                    decimal? adj = Convert.ToDecimal(advanceAdjusted.SummaryValues[0].Value);
                    decimal balance;
                    if ((adv - adj) >= 0)
                    {
                        balance = Math.Abs((decimal)(adv - adj));
                    }
                    else
                    {
                        balance = (decimal)(adv - adj);
                    }
                    lblDifference.Text += balance.ToString("#,0.00;(-)#,#.00");
                    lblDifference.Visible = true;
                }
            }
        }
    }
}