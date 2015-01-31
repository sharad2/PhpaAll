/*
 *  Programming: Eclipse Systems (P) Ltd., NOIDA, INDIA
 *  E-mail: support@eclsys.com
 *
 *  $Workfile:   VoucherDetailControl.ascx.cs  $
 *  $Revision: 38184 $
 *  $Author: ssinghal $
 *  $Date: 2010-11-30 18:40:08 +0530 (Tue, 30 Nov 2010) $
 *  $Modtime:   Jul 22 2008 12:28:04  $
 *
 *  $Log:   S:/Projects/PHPA2/archives/Finance/Controls/VoucherDetailControl.ascx.cs-arc  $
 * 
 *    Rev 1.111   Jul 24 2008 13:26:30   ssinghal
 * WIP
 */
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Linq;
using System.Globalization;
using System.Linq;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Eclipse.PhpaLibrary.Reporting;
using System.Web.Security;

namespace PhpaAll.Controls
{
    public class VoucherDetailSelectingEventArgs : EventArgs
    {
        private readonly List<string> _clauses;
        public VoucherDetailSelectingEventArgs()
        {
            _clauses = new List<string>();
            _dlo = new DataLoadOptions();

            _dlo.LoadWith<RoVoucher>(v => v.RoDivision);
            _dlo.LoadWith<RoVoucher>(v => v.RoVoucherDetails);
            _dlo.LoadWith<RoVoucherDetail>(vd => vd.HeadOfAccount);
            _dlo.LoadWith<RoVoucherDetail>(vd => vd.RoEmployee);
            _dlo.LoadWith<RoVoucherDetail>(vd => vd.RoJob);
            _dlo.LoadWith<RoJob>(j => j.RoContractor);
        }

        public List<string> WhereClauses
        {
            get { return _clauses; }
        }

        private readonly DataLoadOptions _dlo;
        public DataLoadOptions LoadOptions
        {
            get
            {
                return _dlo;
            }
        }
    }

    public enum VoucherDetailInformationLevel
    {
        Detail,
        Summary
    }
    /// <summary>
    /// This contol displays the voucher and its voucher details.
    /// You must call DataBind() to cause this control to display data
    /// There is a limit of 2000 vouchers.
    /// </summary>
    /// <remarks>
    /// You specify the parameters required to filter the vouchers retrieved. 
    /// Then you handle the VoucherDetailSelecting event to dynamically construct your where clause.
    /// If your query is very complex, you can set the query in e.Result parameter passed to VoucherDetailSelecting
    /// event.
    /// You can also define an empty template which will be displayed if no vouchers are retrieved.
    /// Sample markup:
    /// <code>
    ///<uc1:VoucherDetailControl ID="ctlVoucherDetail" runat="server" 
    ///    OnVoucherDetailSelecting="ctlVoucherDetail_Selecting">
    ///    <WhereParameters>
    ///        <asp:Parameter Name="VoucherDate" Type="DateTime" />
    ///    </WhereParameters>
    ///    <EmptyTemplate>
    ///        <asp:Label ID="lblMessage" runat="server" OnPreRender="lblMessage_PreRender" />
    ///        <asp:HyperLink ID="hlRecentDate" runat="server">Dynamic Date</asp:HyperLink>
    ///    </EmptyTemplate>
    ///</uc1:VoucherDetailControl>
    /// </code>
    /// 
    /// Sample Selecting Handler. Notice that the sender is the PhpaLinqDataSource.
    /// <code>
    /// protected void ctlVoucherDetail_Selecting(object sender, VoucherDetailSelectingEventArgs e)
    /// {
    ///     VoucherDetailControl ctlVoucherDetail = (VoucherDetailControl)sender;
    ///     e.WhereClauses.Add("VoucherDate == @VoucherDate");
    ///     ctlVoucherDetail.WhereParameters["VoucherDate"] = tbVoucherDate.Date;
    /// }
    /// </code>
    /// 
    /// Use the DataBound event to detect when the query has completely executed.
    /// 
    /// SS 8 Jul 2008: Handle the CreateLoadOptions event to specify your own load options
    /// 
    /// Specify ExcludeHeadOfAccountId to exclude all details of that id
    /// 
    /// Customize the headers by setting DebitHeader and CreditHeader property
    /// 
    /// FocusHeadOfAccountId - The information will be focused for this head. You can either show
    /// entries only for this head or entries for all except this head.
    /// 
    /// ReportType - Summary: Only entries of the focus head are displayed. 
    /// ReportType - Detail: Vouchers containing the focus head are selected. All details are shown except the
    /// one corresponding to the focus head.
    /// 
    /// SS 11 Jul 2008: Added FromDate property. If specified, the where clause VoucherDate &lt;= FromDate is automatically added.
    /// Opening and closing balances are automtically displayed if FocusHeadOfAccountId and FromDate have been specified.
    /// Same for ToDate
    /// 
    /// Added OrderBy and MaxVoucher properties
    /// 
    /// SubTotal is also shown when ReportType is Detail. When ReportType is summary then only total is shown.
    /// </remarks>
    [ParseChildren(true)]
    [PersistChildren(false)]
    public partial class VoucherDetailControl : UserControl
    {
        #region Properties

        /// <summary>
        /// Comma seperated list of voucher columns on which to sort
        /// </summary>
        [Browsable(true)]
        public string OrderBy
        {
            get
            {
                return this.dsVouchers.OrderBy;
            }
            set
            {
                this.dsVouchers.OrderBy = value;
            }
        }

        /// <summary>
        /// If specified, the where clause VoucherDate <= FromDate is automatically added
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public DateTime? FromDate { get; set; }

        /// <summary>
        /// If specified, the where clause VoucherDate >= ToDate is automatically added
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public DateTime? ToDate { get; set; }

        /// <summary>
        /// If this is set, voucher details of this head of account will not be displayed
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int? FocusHeadOfAccountId
        {
            get;
            set;
        }

        /// <summary>
        /// If this is set, voucher of  passed voucher type will  be displayed
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public char? VoucherType
        {
            get;
            set;
        }

        [Browsable(true)]
        [DefaultValue(VoucherDetailInformationLevel.Detail)]
        public VoucherDetailInformationLevel InformationLevel { get; set; }

        private ITemplate _emptyTemplate;
        [
        Browsable(false),
        PersistenceMode(PersistenceMode.InnerProperty),
        DefaultValue(typeof(ITemplate), ""),
        Description("Empty template"),
        TemplateInstance(TemplateInstance.Single)
        ]
        public ITemplate EmptyTemplate
        {
            get
            {
                return _emptyTemplate;
            }
            set
            {
                _emptyTemplate = value;
            }
        }

        [Browsable(false)]
        public decimal SumofDebit
        {
            get;
            private set;
        }

        [Browsable(false)]
        public decimal SumofCredit
        {
            get;
            private set;
        }

        [Browsable(false)]
        public decimal TotalDebit
        {
            get;
            private set;
        }
        [Browsable(false)]
        public decimal TotalCredit
        {
            get;
            private set;
        }

        [Browsable(false)]
        public decimal ClosingBalance
        {
            get;
            private set;
        }

        // If set ,contains stations on which logged in user has rights
        [Browsable(false)]
        public int[] Station
        {
            get;
            set;
        }

        [Browsable(true)]
        [DefaultValue("Debit")]
        public string DebitHeader { get; set; }

        [Browsable(true)]
        [DefaultValue("Credit")]
        public string CreditHeader { get; set; }


        /// <summary>
        /// If set to true, the name column is not displayed
        /// </summary>
        [Browsable(true)]
        public bool HideNameColumn { get; set; }

        #endregion

        protected override void OnInit(EventArgs e)
        {
            if (_emptyTemplate != null)
            {
                lvDayBook.EmptyDataTemplate = _emptyTemplate;
            }
            base.OnInit(e);
        }

        VoucherDetailSelectingEventArgs m_selectingArgs;
        public override void DataBind()
        {
            // To prevent retrieval of all vouchers, we limit to past one month
            //dsVouchers.WhereParameters["DefaultFromDate"].DefaultValue = DateTime.Today.AddDays(-2).ToString("d");

            m_selectingArgs = new VoucherDetailSelectingEventArgs();
            if (this.FromDate.HasValue)
            {
                m_selectingArgs.WhereClauses.Add("VoucherDate >= @FromDate");
                dsVouchers.WhereParameters["FromDate"].DefaultValue = this.FromDate.Value.ToString("d");
            }
            if (this.ToDate.HasValue)
            {
                m_selectingArgs.WhereClauses.Add("VoucherDate <= @ToDate");
                dsVouchers.WhereParameters["ToDate"].DefaultValue = this.ToDate.Value.ToString("d");
            }

            if (this.VoucherType.HasValue)
            {
                m_selectingArgs.WhereClauses.Add("VoucherType == @VoucherType");
                dsVouchers.WhereParameters["VoucherType"].DefaultValue = this.VoucherType.Value.ToString();
            }
            lvDayBook.DataSource = this.dsVouchers;
            lvDayBook.DataBind();
        }

        private int _count;


        /// <summary>
        /// Make check no visible for Bank voucher only.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lvDayBook_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            switch (e.Item.ItemType)
            {
                case ListViewItemType.DataItem:
                    ListViewDataItem lvdi = (ListViewDataItem)e.Item;
                    RoVoucher v = (RoVoucher)lvdi.DataItem;

                    HtmlTableCell td = (HtmlTableCell)lvdi.FindControl("tdVoucherDetails");
                    Label lblCheckNo = (Label)td.FindControl("lblCheckNo");
                    Label lblVoucherType = (Label)td.FindControl("lblVoucherType");
                    if (lblVoucherType.Text == "B" && v.CheckNumber != null)
                    {
                        lblCheckNo.Visible = true;
                    }
                    break;
            }
        }

        /// <summary>
        /// Bind the grid to display voucher details of a voucher
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lvDayBook_ItemCreated(object sender, ListViewItemEventArgs e)
        {
            switch (e.Item.ItemType)
            {
                case ListViewItemType.DataItem:
                    ++_count;
                    ListViewDataItem lvdi = (ListViewDataItem)e.Item;
                    RoVoucher v = (RoVoucher)lvdi.DataItem;

                    Label lblSequence = (Label)lvdi.FindControl("lblSequence");
                    lblSequence.Text = _count.ToString();

                    Repeater lvVoucherDetails = (Repeater)lvdi.FindControl("lvVoucherDetails");
                    int nCount;
                    IEnumerable<RoVoucherDetail> details;
                    switch (this.InformationLevel)
                    {
                        case VoucherDetailInformationLevel.Summary:
                            if (this.FocusHeadOfAccountId == null)
                            {
                                details = from vd in v.RoVoucherDetails
                                          group vd by vd.VoucherId into grouping
                                          select new RoVoucherDetail(grouping.Sum(vd => vd.DebitAmount),
                                              grouping.Sum(vd => vd.CreditAmount));
                            }
                            else
                            {
                                details = v.RoVoucherDetails;
                            }
                            break;

                        case VoucherDetailInformationLevel.Detail:
                            if (this.FocusHeadOfAccountId == null)
                            {
                                details = v.RoVoucherDetails;
                            }
                            else
                            {
                                details = v.RoVoucherDetails.Where(vd => vd.HeadOfAccountId != this.FocusHeadOfAccountId.Value);
                            }
                            break;

                        default:
                            throw new NotImplementedException();
                    }
                    lvVoucherDetails.DataSource = details;
                    nCount = details.Count();
                    SumofDebit += details.Sum(vd => vd.DebitAmount) ?? 0;
                    SumofCredit += details.Sum(vd => vd.CreditAmount) ?? 0;

                    if (nCount == 0)
                    {
                        MultiView mvEmpty = (MultiView)lvdi.FindControl("mvEmpty");
                        mvEmpty.ActiveViewIndex = 0;
                    }
                    else if (nCount > 1)
                    {
                        HtmlTableCell td = (HtmlTableCell)lvdi.FindControl("tdVoucherDetails");
                        td.RowSpan = nCount;
                        td = (HtmlTableCell)lvdi.FindControl("tdParticulars");
                        td.RowSpan = nCount;
                        td = (HtmlTableCell)lvdi.FindControl("tdDate");
                        td.RowSpan = nCount;
                        td = (HtmlTableCell)lvdi.FindControl("tdPayee");
                        td.RowSpan = nCount;
                    }
                    break;
            }
        }


        //private bool _bDefaultWhereClauseUsed;
        /// <summary>
        /// The page must handle this event to customize the where clause
        /// </summary>
        ///
        public event EventHandler<VoucherDetailSelectingEventArgs> VoucherDetailSelecting;
        /// <summary>
        /// Ritesh 18th Jan 2012
        /// Showing Vouchers of logged in users station only
        /// In case of administrator ignoring station
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dsVouchers_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            
            ReportingDataContext db = (ReportingDataContext)dsVouchers.Database;
            if (VoucherDetailSelecting != null)
            {
                VoucherDetailSelecting(this, m_selectingArgs);
            }
            if (e.Result != null)
            {
                throw new InvalidOperationException("Please do not set the Result Property");
            }

            
            if (this.FocusHeadOfAccountId != null)
            {
                if (this.InformationLevel == VoucherDetailInformationLevel.Summary)
                {
                    m_selectingArgs.LoadOptions.AssociateWith<RoVoucher>(v => v.RoVoucherDetails.Where(vd => vd.HeadOfAccountId == this.FocusHeadOfAccountId));
                }
                e.Result = from v in db.RoVouchers
                           where v.RoVoucherDetails.Any(p => p.HeadOfAccountId == this.FocusHeadOfAccountId.Value)
                           select v;
            }
            if (m_selectingArgs.WhereClauses.Count == 0)
            {
                // SS 22 Jan 2010: If no where clause, set to a small page size. This case occurs when
                // insert voucher shows recent vouchers.
                //_bDefaultWhereClauseUsed = true;
                //m_selectingArgs.WhereClauses.Add("Created >= @DefaultFromDate");
                ctlPager.PageSize = 20;
            }
           
            if (Station != null)
            {
                e.Result = from v in db.RoVouchers
                           where v.StationId == null || Station.Contains(v.StationId.Value)
                           select v;
            }
           
            dsVouchers.Where = string.Join(" && ", m_selectingArgs.WhereClauses.ToArray());

            // If you call DataBind() twice, you will get an error here. We expect to be data bound only once
            db.LoadOptions = m_selectingArgs.LoadOptions;

        }

        public event EventHandler<EventArgs> DataBound;
        protected void lvDayBook_DataBound(object sender, EventArgs e)
        {
            if (DataBound != null)
            {
                DataBound(this, EventArgs.Empty);
            }
            HtmlTableRow rowClosingBalance = (HtmlTableRow)lvDayBook.FindControl("rowClosingBalance");
            if (rowClosingBalance != null && rowClosingBalance.Visible)
            {
                HtmlTableCell cellOpeningBalance = (HtmlTableCell)lvDayBook.FindControl("cellOpeningBalance");
                ReportingDataContext db = (ReportingDataContext)dsVouchers.Database;
                decimal sumOpeningBalance = db.GetOpeningBalance(this.FocusHeadOfAccountId.Value, this.FromDate.Value) ?? 0;
                cellOpeningBalance.InnerText = string.Format(CultureInfo.CurrentUICulture, "{0:C}", sumOpeningBalance);
                HtmlTableCell ctlClosingBalance = (HtmlTableCell)lvDayBook.FindControl("ctlClosingBalance");
                decimal closingBalance;

                switch (this.InformationLevel)
                {
                    case VoucherDetailInformationLevel.Detail:
                        // All heads except focus head are being shown
                        closingBalance = sumOpeningBalance - SumofDebit + SumofCredit;
                        ClosingBalance = closingBalance;

                        TotalDebit = SumofDebit + closingBalance;
                        TotalCredit = SumofCredit + sumOpeningBalance;
                        break;

                    case VoucherDetailInformationLevel.Summary:
                        // Only focus head is being shown
                        closingBalance = sumOpeningBalance + SumofDebit - SumofCredit;

                        break;

                    default:
                        throw new NotImplementedException();
                }

                ctlClosingBalance.InnerText = string.Format(CultureInfo.CurrentUICulture, "{0:C}", closingBalance);

            }
        }

        protected void lblCount_PreRender(object sender, EventArgs e)
        {
            Label lbl = (Label)sender;
            //if (_bDefaultWhereClauseUsed)
            //{
            //    lbl.Text = "Recent vouchers based on voucher date";
            //}
            //else
            //{
            lbl.Text = string.Format("{0:N0} of {1:N0} voucher{2}",
                _count, ctlPager.TotalRowCount,
                _count == 1 ? "" : "s");
            //}
        }

        /// <summary>
        /// Set sum of debit in footer.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void tdDebitFooter_prerender(object sender, EventArgs e)
        {
            HtmlTableCell td = (HtmlTableCell)sender;
            td.InnerText = string.Format("{0:C}", TotalDebit);
        }


        /// <summary>
        /// set sum of credit in footer.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void tdCreditFooter_prerender(object sender, EventArgs e)
        {
            HtmlTableCell td = (HtmlTableCell)sender;
            td.InnerText = string.Format("{0:C}", TotalCredit);
        }

        protected void tdSubTotalDebit_prerender(object sender, EventArgs e)
        {
            HtmlTableCell td = (HtmlTableCell)sender;
            td.InnerText = string.Format("{0:C}", SumofDebit);
        }

        protected void tdSubTotalCredit_prerender(object sender, EventArgs e)
        {
            HtmlTableCell td = (HtmlTableCell)sender;
            td.InnerText = string.Format("{0:C}", SumofCredit);
        }

        protected void tdNetBalanceDebit_prerender(object sender, EventArgs e)
        {
            HtmlTableCell td = (HtmlTableCell)sender;
            if (this.SumofDebit - this.SumofCredit >= 0)
            {

                td.InnerText = (this.SumofDebit - this.SumofCredit).ToString("C");
            }
        }

        protected void tdNetBalanceCredit_prerender(object sender, EventArgs e)
        {
            HtmlTableCell td = (HtmlTableCell)sender;
            if (this.SumofDebit - this.SumofCredit <= 0)
            {
                td.InnerText = (this.SumofCredit - this.SumofDebit).ToString("C");
            }
        }




        /// <summary>
        /// Change header text of debit column if this control use for bank or cash book.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void tdDebitHeader_prerender(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(this.DebitHeader))
            {
                HtmlTableCell td = (HtmlTableCell)sender;
                td.InnerText = this.DebitHeader;
            }
        }

        /// <summary>
        /// Change header text of Credit column if this control use for bank or cash book.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void tdCreditHeader_prerender(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(this.CreditHeader))
            {
                HtmlTableCell td = (HtmlTableCell)sender;
                td.InnerText = this.CreditHeader;
            }
        }

        protected void colHeadOfAccount_Load(object sender, EventArgs e)
        {
            if (this.InformationLevel == VoucherDetailInformationLevel.Summary)
            {
                HtmlTableCell colHeadOfAccount = (HtmlTableCell)sender;
                //colHeadOfAccount.Style.Add(HtmlTextWriterStyle.Display, "none");
                colHeadOfAccount.Visible = false;
            }
        }

        protected void colName_Load(object sender, EventArgs e)
        {
            if (this.HideNameColumn)
            {
                HtmlTableCell colName = (HtmlTableCell)sender;
                colName.Visible = false;
            }
        }

        protected void colEmpContractor_Load(object sender, EventArgs e)
        {
            if (this.InformationLevel == VoucherDetailInformationLevel.Summary && this.FocusHeadOfAccountId == null)
            {
                HtmlTableCell colEmpContractor = (HtmlTableCell)sender;
                colEmpContractor.Visible = false;
            }
        }

        protected void lvDayBook_LayoutCreated(object sender, EventArgs e)
        {
            HtmlTableRow rowOpeningBalance = (HtmlTableRow)lvDayBook.FindControl("rowOpeningBalance");
            HtmlTableRow rowClosingBalance = (HtmlTableRow)lvDayBook.FindControl("rowClosingBalance");
            HtmlTableRow rowTotal = (HtmlTableRow)lvDayBook.FindControl("rowTotal");
            HtmlTableCell tdSubTotal = (HtmlTableCell)lvDayBook.FindControl("tdSubTotal");



            if (this.FocusHeadOfAccountId == null || this.FromDate == null)
            {
                rowOpeningBalance.Visible = false;
                rowClosingBalance.Visible = false;
                rowTotal.Visible = false;
                tdSubTotal.InnerText = "Total";


            }
            else
            {

                // Calculate opening balance
                rowOpeningBalance.Visible = true;
                rowClosingBalance.Visible = true;


            }
        }
    }
}