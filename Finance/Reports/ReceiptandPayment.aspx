<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" CodeBehind="ReceiptandPayment.aspx.cs"
    Inherits="Finance.Reports.ReceiptandPayment" Title="Receipts and Payments" EnableViewState="false" %>

<%@ Register Src="../Controls/PrinterFriendlyButton.ascx" TagName="PrinterFriendlyButton"
    TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server" EnableViewState="false">
    <style type="text/css">
        .MainTable
        {
            border-collapse: collapse;
            table-layout: auto;
            border: thin black solid;
        }
        .MainTable TD
        {
            border: thin 0.25mm black solid;
        }
        .MainTable TH
        {
            border: thin 0.5mm black solid;
        }
        .TableHeader
        {
            font-family: Arial;
            font-weight: bolder;
            padding-top: 3mm;
        }
        .RowHeader
        {
            font-size: 1.1em;
            font-family: Arial;
            text-indent: 5mm;
            font-weight: bolder;
            padding-top: 3mm;
            margin-top: 10mm;
            font-style: oblique;
            letter-spacing: 0.1mm;
        }
        .RowHeader td
        {
            border: solid 0.5mm black;
            border-collapse: collapse;
        }
        .AlternatingRow
        {
            background-color: #F7F7F7;
        }
    </style>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphSideNavigation" runat="server">
    <asp:HyperLink ID="HyperLink2" runat="server" Text="Help" NavigateUrl="~/Doc/ReceiptandPayment.doc.aspx" /><br/>
    <uc2:PrinterFriendlyButton ID="PrinterFriendlyButton1" runat="server" />
</asp:Content>
<asp:Content ID="c3" ContentPlaceHolderID="cph" runat="server">
    <phpa:PhpaLinqDataSource ID="dsQueries" runat="server" ContextTypeName="Eclipse.PhpaLibrary.Reporting.ReportingDataContext"
        TableName="ReportConfigurations" RenderLogVisible="false">
    </phpa:PhpaLinqDataSource>
    <eclipse:TwoColumnPanel runat="server">
        <eclipse:LeftLabel runat="server" Text="Include transactions until" />
        <i:TextBoxEx ID="dttbreceiptpayment" runat="server" FriendlyName="Date">
            <Validators>
                <i:Required />
                <i:Date />
            </Validators>
        </i:TextBoxEx>
        <br />
        <i:ButtonEx runat="server" ID="btnGo" Text="Recalculate" CausesValidation="true" Action="Submit" IsDefault="true"/>
    </eclipse:TwoColumnPanel>
    <i:ValidationSummary ID="ValidationSummary1" runat="server" />
    <table rules="all" cellpadding="4mm" class="MainTable">
        <col style="min-width: 20em" />
        <colgroup style="width: 8em; text-align: right" span="4" />
        <thead class="TableHeader">
            <tr>
                <th align="center" rowspan="2">
                    &nbsp;
                </th>
                <th align="center" rowspan="2">
                    Previous Years<br />
                    <span style="font-size: 0.8em; font-style: italic">Start -
                        <%# m_dtPreviousYear.AddDays(-1).ToString("MMM'&nbsp;'yyyy")%></span>
                </th>
                <th align="center" colspan="2">
                    Current Year
                    <%# m_dtPreviousYear.ToString("MMM'&nbsp;'yyyy")%>-<%# m_dtPreviousYear.AddMonths(11).ToString("MMM'&nbsp;'yyyy")%>
                </th>
                <th align="center" rowspan="2">
                    Cumulative<br />
                    <span style="font-size: 0.8em; font-style: italic">Start -
                        <%# m_dtMonthStart.ToString("MMM'&nbsp;'yyyy")%></span>
                </th>
            </tr>
            <tr>
                <th align="center" style="font-size: 0.8em; font-style: italic">
                    <%# m_dtMonthStart.ToString("MMM'&nbsp;'yyyy")%>
                </th>
                <th align="center" style="font-size: 0.8em; font-style: italic">
                    <%# m_dtPreviousYear.ToString("MMM") %>
                    -
                    <%# m_dtMonthStart.ToString("MMM'&nbsp;'yyyy")%>
                </th>
            </tr>
        </thead>
        <tbody>
            <tr class="RowHeader">
                <td colspan="5" class="RowHeader">
                    Receipts
                </td>
            </tr>
            <tr>
                <td colspan="5" style="font-style: italic; font-weight: bold">
                    Opening Balances
                </td>
            </tr>
            <tr class="AlternatingRow">
                <td style="text-indent: 4mm; font-weight: bold">
                    <%--Using this div to force a min column width. Report must print on a single A4 page--%>
                    <div style="white-space: nowrap">
                        Opening Balance Rs./Nu</div>
                </td>
                <td align="center" title="Not applicable">
                    &bull;
                </td>
                <td title="Opening balances as of <%# m_dtMonthStart.ToString("d MMM'&nbsp;'yyyy")%> for local currency Cash and Bank accounts. Click to see opening balances of individual accounts."
                    class="vd-amountcol">
                    <asp:HyperLink ID="hplnkOpBalForTheMonth" runat="server" EnableViewState="false"
                        NavigateUrl="~/Finance/VoucherSearch.aspx?AccountTypes=CASH,BANKNU,INVESTMENT&DateTo={4:d}" />
                </td>
                <td title="Opening local currency cash and bank balance as of <%# m_dtPreviousYear.ToString("d MMM'&nbsp;'yyyy")%>"
                    class="vd-amountcol">
                    <asp:HyperLink ID="hplnkOpBalUptoTheMonth" runat="server" EnableViewState="false"
                        NavigateUrl="~/Finance/VoucherSearch.aspx?AccountTypes=CASH,BANKNU,INVESTMENT&DateTo={3:d}" />
                </td>
                <td title="Not applicable" align="center">
                    &bull;
                </td>
            </tr>
            <tr>
                <td style="text-indent: 4mm">
                    Opening Balance FE
                </td>
                <td align="center" title="Not applicable">
                    &bull;
                </td>
                <td title="Opening balances as of <%# m_dtMonthStart.ToString("d MMM'&nbsp;'yyyy")%> for Foreign Currency Bank accounts. Click to see opening balances of individual accounts."
                    class="vd-amountcol">
                    <asp:HyperLink ID="hplnkOpBalForTheMonthFE" runat="server" EnableViewState="false"
                        NavigateUrl="~/Finance/VoucherSearch.aspx?AccountTypes=BANKFE&FromDate={0:d}&DateTo={1:d}" />
                </td>
                <td title="Opening local currency cash and bank balance as of <%# m_dtPreviousYear.ToString("d MMM'&nbsp;'yyyy")%>"
                    class="vd-amountcol">
                    <asp:HyperLink ID="hplnkOpBalUptoTheMonthFE" runat="server" EnableViewState="false"
                        NavigateUrl="~/Finance/VoucherSearch.aspx?AccountTypes=BANKFE&FromDate={0:d}&DateTo={1:d}" />
                </td>
                <td title="Not applicable" align="center">
                    &bull;
                </td>
            </tr>
            <tr class="AlternatingRow">
                <td>
                    <asp:HyperLink ID="hplnkgoinu" Text="Funds Received From GOI(Rs./NU.)" ToolTip="Click to get list of Heads involved"
                        NavigateUrl="~/Finance/VoucherSearch.aspx?Types=GRANT_RECEIVED_GOINU,LOAN_RECEIVED_GOINU"
                        runat="server"></asp:HyperLink>
                </td>
                <td class="vd-amountcol">
                    <asp:HyperLink ID="hplnkGOIAidPreviousYear" runat="server" ToolTip="Click to list voucher entries for Funds Received from Government of India in local Currency prior to Financial Year"
                        NavigateUrl="~/Finance/VoucherSearch.aspx?AccountTypes=GRANT_RECEIVED_GOINU,LOAN_RECEIVED_GOINU&DateTo={3:d}"
                        EnableViewState="false" />
                </td>
                <td class="vd-amountcol">
                    <asp:HyperLink ID="hplnkGOIAidForMonth" runat="server" ToolTip="Click to list voucher entries for Funds Received from Government of India in local Currency for input month"
                        NavigateUrl="~/Finance/VoucherSearch.aspx?AccountTypes=GRANT_RECEIVED_GOINU,LOAN_RECEIVED_GOINU&DateFrom={1:d}&DateTo={2:d}"
                        EnableViewState="false" />
                </td>
                <td class="vd-amountcol">
                    <asp:HyperLink ID="hplnkGOIAidUptoMonth" runat="server" ToolTip="Click to list voucher entries for Funds Received from Government of India in local Currency from financial year start till input month"
                        EnableViewState="false" NavigateUrl="~/Finance/VoucherSearch.aspx?AccountTypes=GRANT_RECEIVED_GOINU,LOAN_RECEIVED_GOINU&DateFrom={0:d}&DateTo={2:d}" />
                </td>
                <td class="vd-amountcol">
                    <asp:Label ID="lblGOIAidsum" runat="server" ToolTip="Funds Received from Government of India in local currency till date"
                        EnableViewState="false" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:HyperLink ID="hplnkgoife" Text="Funds Received From GOI (F.E.)" ToolTip="Click to get list of Heads involved"
                        NavigateUrl="~/Finance/VoucherSearch.aspx?Types=GRANT_RECEIVED_GOIFE,LOAN_RECEIVED_GOIFE"
                        runat="server"></asp:HyperLink>
                </td>
                <td class="vd-amountcol">
                    <asp:HyperLink ID="hplnkGOIFEPreviousYear" runat="server" ToolTip="Click to list voucher entries for Funds Received from Government of India in Foreign Exchange prior to Financial Year"
                        EnableViewState="false" NavigateUrl="~/Finance/VoucherSearch.aspx?AccountTypes=GRANT_RECEIVED_GOIFE,LOAN_RECEIVED_GOIFE&DateTo={3:d}" />
                </td>
                <td class="vd-amountcol">
                    <asp:HyperLink ID="hplnkGOIFEForMonth" runat="server" ToolTip="Click to list voucher entries for Funds Received from Government of India in Foreign Exchange for input month"
                        EnableViewState="false" NavigateUrl="~/Finance/VoucherSearch.aspx?AccountTypes=GRANT_RECEIVED_GOIFE,LOAN_RECEIVED_GOIFE&DateFrom={1:d}&DateTo={2:d}" />
                </td>
                <td class="vd-amountcol">
                    <asp:HyperLink ID="hplnkGOIFEUptoMonth" runat="server" ToolTip="Click to list voucher entries for Funds Received from Government of India in Foreign Exchange from financial year start till input month"
                        EnableViewState="false" NavigateUrl="~/Finance/VoucherSearch.aspx?AccountTypes=GRANT_RECEIVED_GOIFE,LOAN_RECEIVED_GOIFE&DateFrom={0:d}&DateTo={2:d}" />
                </td>
                <td class="vd-amountcol">
                    <asp:Label ID="lblGOIFEsum" runat="server" ToolTip="Funds received from Government of India in Foreign Exchange till date"
                        EnableViewState="false" />
                </td>
            </tr>
            <tr class="AlternatingRow">
                <td>
                    <asp:HyperLink ID="hplnkint" Text="Interest<b/>" ToolTip="Click to get list of Heads involved"
                        NavigateUrl="~/Finance/AccountHeads.aspx?Types=INTEREST" runat="server"></asp:HyperLink>
                </td>
                <td class="vd-amountcol">
                    <asp:HyperLink ID="hplnkInterestPreviousYear" runat="server" ToolTip="Click to list voucher entries for Interest received prior to Financial Year"
                        EnableViewState="false" NavigateUrl="~/Finance/VoucherSearch.aspx?AccountTypes=INTEREST&DateTo={3:d}" />
                </td>
                <td class="vd-amountcol">
                    <asp:HyperLink ID="hplnkInterestForMonth" runat="server" ToolTip="Click to list voucher entries for Interest received for input month"
                        EnableViewState="false" NavigateUrl="~/Finance/VoucherSearch.aspx?AccountTypes=INTEREST&DateFrom={1:d}&DateTo={2:d}" />
                </td>
                <td class="vd-amountcol">
                    <asp:HyperLink ID="hplnkInterestUptoMonth" runat="server" ToolTip="Click to list voucher entries for Interest received from financial year start till input month"
                        EnableViewState="false" NavigateUrl="~/Finance/VoucherSearch.aspx?AccountTypes=INTEREST&DateFrom={0:d}&DateTo={2:d}" />
                </td>
                <td class="vd-amountcol">
                    <asp:Label ID="lblInterestsum" runat="server" ToolTip="Interest received till date"
                        EnableViewState="false" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:HyperLink ID="hplnkrecovery" Text="Recovery Awaiting Remittances" ToolTip="Click to get list of Heads involved"
                        NavigateUrl="~/Finance/AccountHeads.aspx?Types=SALARY_REMITANCES" runat="server"></asp:HyperLink>
                </td>
                <td class="vd-amountcol">
                    <asp:HyperLink ID="hplnkRecoveryPreviousYear" runat="server" ToolTip="Click to list voucher entries for Salary Recoveries and remittances prior to Financial Year"
                        EnableViewState="false" NavigateUrl="~/Finance/VoucherSearch.aspx?AccountTypes=SALARY_REMITANCES&DateTo={3:d}" />
                </td>
                <td class="vd-amountcol">
                    <asp:HyperLink ID="hplnkRecoveryForMonth" runat="server" ToolTip="Click to list voucher entries for Salary Recoveries and remittances for input month"
                        EnableViewState="false" NavigateUrl="~/Finance/VoucherSearch.aspx?AccountTypes=SALARY_REMITANCES&DateFrom={1:d}&DateTo={2:d}" />
                </td>
                <td class="vd-amountcol">
                    <asp:HyperLink ID="hplnkRecoveryUptoMonth" runat="server" ToolTip="Click to list voucher entries for Salary Recoveries and remittances from financial year start till input month"
                        EnableViewState="false" NavigateUrl="~/Finance/VoucherSearch.aspx?AccountTypes=SALARY_REMITANCES&DateFrom={0:d}&DateTo={2:d}" />
                </td>
                <td class="vd-amountcol">
                    <asp:Label ID="lblRecoverysum" runat="server" ToolTip="Salary recoveries till date"
                        EnableViewState="false" />
                </td>
            </tr>
            <tr class="AlternatingRow">
                <td>
                    <asp:HyperLink ID="hplnktender" Text="Sale of Tender Documents" ToolTip="Click to get list of Heads involved"
                        NavigateUrl="~/Finance/AccountHeads.aspx?Types=TENDER_SALE" runat="server" />
                </td>
                <td class="vd-amountcol">
                    <asp:HyperLink ID="hplnkTenderSalePreviousYear" runat="server" ToolTip="Click to list voucher entries for sales from tender prior to Financial Year"
                        EnableViewState="false" NavigateUrl="~/Finance/VoucherSearch.aspx?AccountTypes=TENDER_SALE&DateTo={3:d}" />
                </td>
                <td class="vd-amountcol">
                    <asp:HyperLink ID="hplnkTenderSaleForMonth" runat="server" ToolTip="Click to list voucher entries for sales from tender for input month"
                        EnableViewState="false" NavigateUrl="~/Finance/VoucherSearch.aspx?AccountTypes=TENDER_SALE&DateFrom={1:d}&DateTo={2:d}" />
                </td>
                <td class="vd-amountcol">
                    <asp:HyperLink ID="hplnkTenderSaleUptoMonth" runat="server" ToolTip="Click to list voucher entries for sales from tender from financial year start till input month"
                        EnableViewState="false" NavigateUrl="~/Finance/VoucherSearch.aspx?AccountTypes=TENDER_SALE&DateFrom={0:d}&DateTo={2:d}" />
                </td>
                <td class="vd-amountcol">
                    <asp:Label ID="lblTenderSalesum" runat="server" ToolTip="Tender Sales till date"
                        EnableViewState="false" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:HyperLink ID="hplnkemd" Text="Earnest Money Deposit" ToolTip="Click to get list of Heads involved"
                        NavigateUrl="~/Finance/AccountHeads.aspx?Types=EMD" runat="server"></asp:HyperLink>
                </td>
                <td class="vd-amountcol">
                    <asp:HyperLink ID="hplnkEMDPreviousYear" runat="server" ToolTip="Click to list voucher entries for earned money deposit prior to Financial Year"
                        EnableViewState="false" NavigateUrl="~/Finance/VoucherSearch.aspx?AccountTypes=EMD&DateTo={3:d}" />
                </td>
                <td class="vd-amountcol">
                    <asp:HyperLink ID="hplnkEMDForMonth" runat="server" ToolTip="Click to list voucher entries for earned money deposit for input month"
                        EnableViewState="false" NavigateUrl="~/Finance/VoucherSearch.aspx?AccountTypes=EMD&DateFrom={1:d}&DateTo={2:d}" />
                </td>
                <td class="vd-amountcol">
                    <asp:HyperLink ID="hplnkEMDUptoMonth" runat="server" ToolTip="Click to list voucher entries for earned money deposit from financial year start till input month"
                        EnableViewState="false" NavigateUrl="~/Finance/VoucherSearch.aspx?AccountTypes=EMD&DateFrom={0:d}&DateTo={2:d}" />
                </td>
                <td class="vd-amountcol">
                    <asp:Label ID="lblEMDsum" runat="server" ToolTip="Earned Money Deposit till date"
                        EnableViewState="false" />
                </td>
            </tr>
            <tr class="AlternatingRow">
                <td>
                    <asp:HyperLink ID="hplnksd" Text="Security Deposit" ToolTip="Click to get list of Heads involved"
                        NavigateUrl="~/Finance/AccountHeads.aspx?Types=SD" runat="server"></asp:HyperLink>
                </td>
                <td class="vd-amountcol">
                    <asp:HyperLink ID="hplnkSecurityDepositPreviousYear" runat="server" Text="&nbsp;"
                        ToolTip="Click to list voucher entries for security deposit prior to Financial Year"
                        EnableViewState="false" NavigateUrl="~/Finance/VoucherSearch.aspx?AccountTypes=SD&DateTo={3:d}" />
                </td>
                <td class="vd-amountcol">
                    <asp:HyperLink ID="hplnkSecurityDepositForMonth" runat="server" ToolTip="Click to list voucher entries for security deposit for input month"
                        EnableViewState="false" NavigateUrl="~/Finance/VoucherSearch.aspx?AccountTypes=SD&DateFrom={1:d}&DateTo={2:d}" />
                </td>
                <td class="vd-amountcol">
                    <asp:HyperLink ID="hplnkSecurityDepositUptoMonth" runat="server" ToolTip="Click to list voucher entries for security deposit from financial year start till input month"
                        EnableViewState="false" NavigateUrl="~/Finance/VoucherSearch.aspx?AccountTypes=SD&DateFrom={0:d}&DateTo={2:d}" />
                </td>
                <td class="vd-amountcol">
                    <asp:Label ID="lblSecurityDepositsum" runat="server" ToolTip="Security Deposit till date"
                        EnableViewState="false" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:HyperLink ID="hplnkbit" Text="BIT / Contract Tax" ToolTip="Click to get list of Heads involved"
                        NavigateUrl="~/Finance/AccountHeads.aspx?Types=BIT" runat="server"></asp:HyperLink>
                </td>
                <td class="vd-amountcol">
                    <asp:HyperLink ID="hplnkBITPreviousYear" runat="server" ToolTip="Click to list voucher entries for contractor tax prior to Financial Year"
                        EnableViewState="false" NavigateUrl="~/Finance/VoucherSearch.aspx?AccountTypes=BIT&DateTo={3:d}" />
                </td>
                <td class="vd-amountcol">
                    <asp:HyperLink ID="hplnkBITForMonth" runat="server" ToolTip="Click to list voucher entries for contractor tax for input month"
                        EnableViewState="false" NavigateUrl="~/Finance/VoucherSearch.aspx?AccountTypes=BIT&DateFrom={1:d}&DateTo={2:d}" />
                </td>
                <td class="vd-amountcol">
                    <asp:HyperLink ID="hplnkBITUptoMonth" runat="server" ToolTip="Click to list voucher entries for contractor tax from financial year start till input month"
                        EnableViewState="false" NavigateUrl="~/Finance/VoucherSearch.aspx?AccountTypes=BIT&DateFrom={0:d}&DateTo={2:d}" />
                </td>
                <td class="vd-amountcol">
                    <asp:Label ID="lblBITsum" runat="server" ToolTip="Contractor Tax till date" EnableViewState="false" />
                </td>
            </tr>
            <tr class="AlternatingRow">
                <td>
                    <asp:HyperLink ID="hplnkothrec" ToolTip="Click to get list of Heads involved" Text="Other Receipts"
                        runat="server" NavigateUrl="~/Finance/AccountHeads.aspx?Types=ACCUMULATED_RECEIPTS,ASSETS,LIABILITY"></asp:HyperLink>
                </td>
                <td class="vd-amountcol">
                    <asp:HyperLink ID="hplnkOtherPreviousYear" runat="server" ToolTip="Click to list voucher entries for acculated_receipts,other assets and other liabilities prior to Financial Year"
                        NavigateUrl="~/Finance/VoucherSearch.aspx?AccountTypes=ACCUMULATED_RECEIPTS,ASSETS,LIABILITY&DateTo={3:d}"
                        EnableViewState="false" />
                </td>
                <td class="vd-amountcol">
                    <asp:HyperLink ID="hplnkOtherForMonth" runat="server" ToolTip="Click to list voucher entries for acculated_receipts,other assets and other liabilities for input month"
                        NavigateUrl="~/Finance/VoucherSearch.aspx?AccountTypes=ACCUMULATED_RECEIPTS,ASSETS,LIABILITY&DateFrom={1:d}&DateTo={2:d}"
                        EnableViewState="false" />
                </td>
                <td class="vd-amountcol">
                    <asp:HyperLink ID="hplnkOtheruptoMonth" runat="server" ToolTip="Click to list voucher entries for acculated_receipts,other assets and other liabilities from financial year start till input month"
                        NavigateUrl="~/Finance/VoucherSearch.aspx?AccountTypes=ACCUMULATED_RECEIPTS,ASSETS,LIABILITY&DateFrom={0:d}&DateTo={2:d}"
                        EnableViewState="false" />
                </td>
                <td class="vd-amountcol">
                    <asp:Label ID="lblOthersum" runat="server" ToolTip="Acculated_receipts,other assets and other liabilities till date"
                        EnableViewState="false" />
                </td>
            </tr>
            <tr class="RowHeader">
                <td align="right">
                    Total Receipts
                </td>
                <td class="vd-amountcol">
                    <asp:Label ID="lblReceiptsPreviousYear" runat="server" EnableViewState="false" OnPreRender="lbl_PreRenderShowSum"
                        SumType="ReceiptsPreviousYear" />
                </td>
                <td class="vd-amountcol">
                    <asp:Label ID="lblReceiptsForMonth" runat="server" EnableViewState="false" OnPreRender="lbl_PreRenderShowSum"
                        SumType="ReceiptsForMonth" />
                </td>
                <td class="vd-amountcol">
                    <asp:Label ID="lblReceiptsUptoMonth" runat="server" EnableViewState="false" OnPreRender="lbl_PreRenderShowSum"
                        SumType="ReceiptsUptoMonth" />
                </td>
                <td class="vd-amountcol">
                    <asp:Label ID="Label1" runat="server" EnableViewState="false" OnPreRender="lbl_PreRenderShowSum"
                        SumType="ReceiptsSum" />
                </td>
            </tr>
            <tr class="RowHeader">
                <td colspan="5" class="RowHeader">
                    Payments
                </td>
            </tr>
            <tr>
                <td>
                    <asp:HyperLink ID="hplnkexp" ToolTip="Click to get list of Heads involved" Text="Expenditure"
                        runat="server"></asp:HyperLink>
                </td>
                <td class="vd-amountcol">
                    <asp:HyperLink ID="hplnkExpenditurePreviousYear" runat="server" ToolTip="Click to list voucher entries for expenditure prior to Financial Year"
                        EnableViewState="false" NavigateUrl="~/Finance/VoucherSearch.aspx?AccountTypes=EXPENDITURE,TOUR_EXPENSES,MR&DateTo={3:d}" />
                </td>
                <td class="vd-amountcol">
                    <asp:HyperLink ID="hplnkExpenditureForMonth" runat="server" ToolTip="Click to list voucher entries for expenditure for input month"
                        EnableViewState="false" NavigateUrl="~/Finance/VoucherSearch.aspx?AccountTypes=EXPENDITURE,TOUR_EXPENSES,MR&DateFrom={1:d}&DateTo={2:d}" />
                </td>
                <td class="vd-amountcol">
                    <asp:HyperLink ID="hplnkExpenditureUptoMonth" runat="server" ToolTip="Click to list voucher entries for expenditure from financial year start till input month"
                        EnableViewState="false" NavigateUrl="~/Finance/VoucherSearch.aspx?AccountTypes=EXPENDITURE,TOUR_EXPENSES,MR&DateFrom={0:d}&DateTo={2:d}" />
                </td>
                <td class="vd-amountcol">
                    <asp:Label ID="lblExpendituresum" runat="server" ToolTip="Expenditure incured till date"
                        EnableViewState="false" />
                </td>
            </tr>
            <tr class="AlternatingRow">
                <td colspan="5" style="font-style: italic">
                    Advances
                </td>
            </tr>
            <tr>
                <td style="text-indent: 4mm">
                    a)
                    <asp:HyperLink ID="hplnkemp" ToolTip="Click to get list of Heads involved" Text="Employees"
                        NavigateUrl="~/Finance/AccountHeads.aspx?Types=EMPLOYEE_ADVANCE" runat="server"></asp:HyperLink>
                </td>
                <td class="vd-amountcol">
                    <asp:HyperLink ID="hplnkEmpAdvPreviousYear" runat="server" ToolTip="Click to list voucher entries for employee advances prior to Financial Year"
                        EnableViewState="false" NavigateUrl="~/Finance/VoucherSearch.aspx?AccountTypes=EMPLOYEE_ADVANCE&DateTo={3:d}" />
                </td>
                <td class="vd-amountcol">
                    <asp:HyperLink ID="hplnkEmpAdvForMonth" runat="server" ToolTip="Click to list voucher entries for employee advances for input month"
                        EnableViewState="false" NavigateUrl="~/Finance/VoucherSearch.aspx?AccountTypes=EMPLOYEE_ADVANCE&DateFrom={1:d}&DateTo={2:d}" />
                </td>
                <td class="vd-amountcol">
                    <asp:HyperLink ID="hplnkEmpAdvUptoMonth" runat="server" ToolTip="Click to list voucher entries for employee advances from financial year start till input month"
                        EnableViewState="false" NavigateUrl="~/Finance/VoucherSearch.aspx?AccountTypes=EMPLOYEE_ADVANCE&DateFrom={0:d}&DateTo={2:d}" />
                </td>
                <td class="vd-amountcol">
                    <asp:Label ID="lblEmpAdvsum" runat="server" ToolTip="Advances to Employees till date"
                        EnableViewState="false" />
                </td>
            </tr>
            <tr>
                <td style="text-indent: 4mm">
                    b)
                    <asp:HyperLink ID="hplnkparty" ToolTip="Click to get list of Heads involved" Text="Parties"
                        NavigateUrl="~/Finance/AccountHeads.aspx?Types=PARTY_ADVANCE" runat="server"></asp:HyperLink>
                </td>
                <td class="vd-amountcol">
                    <asp:HyperLink ID="hplnkContAdvPreviousYear" runat="server" ToolTip="Click to list voucher entries for party and material advances prior to Financial Year"
                        NavigateUrl="~/Finance/VoucherSearch.aspx?AccountTypes=PARTY_ADVANCE,MATERIAL_ADVANCE&DateTo={3:d}"
                        EnableViewState="false" />
                </td>
                <td class="vd-amountcol">
                    <asp:HyperLink ID="hplnkContAdvForMonth" runat="server" ToolTip="Click to list voucher entries for party and material advances for input month"
                        NavigateUrl="~/Finance/VoucherSearch.aspx?AccountTypes=PARTY_ADVANCE,MATERIAL_ADVANCE&DateFrom={1:d}&DateTo={2:d}"
                        EnableViewState="false" />
                </td>
                <td class="vd-amountcol">
                    <asp:HyperLink ID="hplnkContAdvUptoMonth" runat="server" ToolTip="Click to list voucher entries for party and material advances from financial year start till input month"
                        NavigateUrl="~/Finance/VoucherSearch.aspx?AccountTypes=PARTY_ADVANCE,MATERIAL_ADVANCE&DateFrom={0:d}&DateTo={2:d}"
                        EnableViewState="false" />
                </td>
                <td class="vd-amountcol">
                    <asp:Label ID="lblContAdvsum" runat="server" ToolTip="Advances to parties for material till date"
                        EnableViewState="false" />
                </td>
            </tr>
            <tr class="AlternatingRow">
                <td>
                    <asp:HyperLink ID="hplnkstock" ToolTip="Click to get list of Heads involved" Text="Stock Suspense"
                        NavigateUrl="~/Finance/AccountHeads.aspx?Types=STOCK_SUSPENSE" runat="server"></asp:HyperLink>
                </td>
                <td class="vd-amountcol">
                    <asp:HyperLink ID="hplnkStockSuspensePreviousYear" runat="server" ToolTip="Click to list voucher entries for stock suspense prior to Financial Year"
                        EnableViewState="false" NavigateUrl="~/Finance/VoucherSearch.aspx?AccountTypes=STOCK_SUSPENSE&DateTo={3:d}" />
                </td>
                <td class="vd-amountcol">
                    <asp:HyperLink ID="hplnkStockSuspenseForMonth" runat="server" ToolTip="Click to list voucher entries for stock suspense for input month"
                        EnableViewState="false" NavigateUrl="~/Finance/VoucherSearch.aspx?AccountTypes=STOCK_SUSPENSE&DateFrom={1:d}&DateTo={2:d}" />
                </td>
                <td class="vd-amountcol">
                    <asp:HyperLink ID="hplnkStockSuspenseUptoMonth" runat="server" ToolTip="Click to list voucher entries for stock suspense from financial year start till input month"
                        EnableViewState="false" NavigateUrl="~/Finance/VoucherSearch.aspx?AccountTypes=STOCK_SUSPENSE&DateFrom={0:d}&DateTo={2:d}" />
                </td>
                <td class="vd-amountcol">
                    <asp:Label ID="lblStockSuspensesum" runat="server" ToolTip="Stock suspense till date"
                        EnableViewState="false" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:HyperLink ID="hplnkbst" ToolTip="Click to get list of Heads involved" Text="BST"
                        NavigateUrl="~/Finance/AccountHeads.aspx?Types=BST" runat="server"></asp:HyperLink>
                </td>
                <td class="vd-amountcol">
                    <asp:HyperLink ID="hplnkBSTPreviousYear" runat="server" ToolTip="Click to list voucher entries for bhutan service tax prior to Financial Year"
                        EnableViewState="false" NavigateUrl="~/Finance/VoucherSearch.aspx?AccountTypes=BST&DateTo={3:d}" />
                </td>
                <td class="vd-amountcol">
                    <asp:HyperLink ID="hplnkBSTForMonth" runat="server" ToolTip="Click to list voucher entries for bhutan service tax for input month"
                        EnableViewState="false" NavigateUrl="~/Finance/VoucherSearch.aspx?AccountTypes=BST&DateFrom={1:d}&DateTo={2:d}" />
                </td>
                <td class="vd-amountcol">
                    <asp:HyperLink ID="hplnkBSTUptoMonth" runat="server" ToolTip="Click to list voucher entries for bhutan service tax from financial year start till input month"
                        EnableViewState="false" NavigateUrl="~/Finance/VoucherSearch.aspx?AccountTypes=BST&DateFrom={0:d}&DateTo={2:d}" />
                </td>
                <td class="vd-amountcol">
                    <asp:Label ID="lblBSTsum" runat="server" ToolTip="Service Tax paid till date" EnableViewState="false" />
                </td>
            </tr>
            <tr class="AlternatingRow">
                <td>
                    <asp:HyperLink ID="hplnksvctax" ToolTip="Click to get list of Heads involved" Text="Service Tax"
                        NavigateUrl="~/Finance/AccountHeads.aspx?Types=SVCTAX" runat="server"></asp:HyperLink>
                </td>
                <td class="vd-amountcol">
                    <asp:HyperLink ID="hplnksvctaxPreviousYear" runat="server" ToolTip="Click to list voucher entries for bhutan service tax prior to Financial Year"
                        EnableViewState="false" NavigateUrl="~/Finance/VoucherSearch.aspx?AccountTypes=SVCTAX&DateTo={3:d}" />
                </td>
                <td class="vd-amountcol">
                    <asp:HyperLink ID="hplnksvctaxForMonth" runat="server" ToolTip="Click to list voucher entries for bhutan service tax for input month"
                        EnableViewState="false" NavigateUrl="~/Finance/VoucherSearch.aspx?AccountTypes=SVCTAX&DateFrom={1:d}&DateTo={2:d}" />
                </td>
                <td class="vd-amountcol">
                    <asp:HyperLink ID="hplnksvctaxUptoMonth" runat="server" ToolTip="Click to list voucher entries for bhutan service tax from financial year start till input month"
                        EnableViewState="false" NavigateUrl="~/Finance/VoucherSearch.aspx?AccountTypes=SVCTAX&DateFrom={0:d}&DateTo={2:d}" />
                </td>
                <td class="vd-amountcol">
                    <asp:Label ID="lblsvctaxsum" runat="server" ToolTip="Service Tax paid till date"
                        EnableViewState="false" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:HyperLink ID="hplnkedGOI" ToolTip="Click to get list of Heads involved" Text="Excise Duty to G.O.I."
                        NavigateUrl="~/Finance/AccountHeads.aspx?Types=EDGOI" runat="server"></asp:HyperLink>
                </td>
                <td class="vd-amountcol">
                    <asp:HyperLink ID="hplnkExciseDutyGOIPreviousYear" runat="server" ToolTip="Click to list voucher entries for excise duty from government of india prior to Financial Year"
                        EnableViewState="false" NavigateUrl="~/Finance/VoucherSearch.aspx?AccountTypes=EDGOI&DateTo={3:d}" />
                </td>
                <td class="vd-amountcol">
                    <asp:HyperLink ID="hplnkExciseDutyGOIForMonth" runat="server" ToolTip="Click to list voucher entries for excise duty from government of for input month"
                        EnableViewState="false" NavigateUrl="~/Finance/VoucherSearch.aspx?AccountTypes=EDGOI&DateFrom={1:d}&DateTo={2:d}" />
                </td>
                <td class="vd-amountcol">
                    <asp:HyperLink ID="hplnkExciseDutyGOIUptoMonth" runat="server" ToolTip="Click to list voucher entries for excise duty from government of from financial year start till input month"
                        EnableViewState="false" NavigateUrl="~/Finance/VoucherSearch.aspx?AccountTypes=EDGOI&DateFrom={0:d}&DateTo={2:d}" />
                </td>
                <td class="vd-amountcol">
                    <asp:Label ID="lblExciseDutyGOIsum" runat="server" ToolTip="Excise duty paid to Government of India till date"
                        EnableViewState="false" />
                </td>
            </tr>
            <tr class="AlternatingRow">
                <td>
                    <asp:HyperLink ID="hplnkedRGOB" ToolTip="Click to get list of Heads involved" Text="Excise Duty to R.G.O.B."
                        NavigateUrl="~/Finance/AccountHeads.aspx?Types=EDRGOB" runat="server"></asp:HyperLink>
                </td>
                <td class="vd-amountcol">
                    <asp:HyperLink ID="hplnkExciseDutyRGOBPreviousYear" runat="server" ToolTip="Click to list voucher entries for excise duty from Royal Government of Bhutan prior to Financial Year"
                        EnableViewState="false" NavigateUrl="~/Finance/VoucherSearch.aspx?AccountTypes=EDRGOB&DateTo={3:d}" />
                </td>
                <td class="vd-amountcol">
                    <asp:HyperLink ID="hplnkExciseDutyRGOBForMonth" runat="server" ToolTip="Click to list voucher entries for excise duty from Royal Government of Bhutan for input month"
                        EnableViewState="false" NavigateUrl="~/Finance/VoucherSearch.aspx?AccountTypes=EDRGOB&DateFrom={1:d}&DateTo={2:d}" />
                </td>
                <td class="vd-amountcol">
                    <asp:HyperLink ID="hplnkExciseDutyRGOBUptoMonth" runat="server" ToolTip="Click to list voucher entries for excise duty from Royal Government of Bhutan from financial year start till input month"
                        EnableViewState="false" NavigateUrl="~/Finance/VoucherSearch.aspx?AccountTypes=EDRGOB&DateFrom={0:d}&DateTo={2:d}" />
                </td>
                <td class="vd-amountcol">
                    <asp:Label ID="lblExciseDutyRGOBsum" runat="server" ToolTip="Excise duty paid to Royal Government of Bhutan till date"
                        EnableViewState="false" />
                </td>
            </tr>
            <tr class="alternatingRow">
                <td><asp:HyperLink ID="hplnkGreenTax" ToolTip="Click to get list of Heads involved" Text="Green Tax"
                        NavigateUrl="~/Finance/AccountHeads.aspx?Types=GREEN_TAX" runat="server"></asp:HyperLink>
                </td>
                <td class="vd-amountcol"><asp:HyperLink ID="hplnkGreenTaxPreviousYear" runat="server" ToolTip="Click to list voucher entries for Green Tax from Royal Government of Bhutan prior to Financial Year"
                        EnableViewState="false" NavigateUrl="~/Finance/VoucherSearch.aspx?AccountTypes=GREEN_TAX&DateTo={3:d}" /></td>
                <td class="vd-amountcol"><asp:HyperLink ID="hplnkGreenTaxForMonth" runat="server" ToolTip="Click to list voucher entries for Green Tax from Royal Government of Bhutan for input month"
                        EnableViewState="false" NavigateUrl="~/Finance/VoucherSearch.aspx?AccountTypes=GREEN_TAX&DateFrom={1:d}&DateTo={2:d}" /></td>
                <td class="vd-amountcol"><asp:HyperLink ID="hplnkGreenTaxUptoMonth" runat="server" ToolTip="Click to list voucher entries for Green Tax from Royal Government of Bhutan from financial year start till input month"
                        EnableViewState="false" NavigateUrl="~/Finance/VoucherSearch.aspx?AccountTypes=GREEN_TAX&DateFrom={0:d}&DateTo={2:d}" /></td>
                <td class="vd-amountcol"><asp:Label ID="lblGreenTaxRGOBsum" runat="server" ToolTip="Green Tax paid to Royal Government of Bhutan till date"
                        EnableViewState="false" /></td>
            </tr>
            <tr class="RowHeader">
                <td align="right">
                    Total Payments
                </td>
                <td class="vd-amountcol">
                    <asp:Label ID="lblPaymentsPreviousYear" runat="server" EnableViewState="false" OnPreRender="lbl_PreRenderShowSum"
                        SumType="PaymentsPreviousYear" />
                </td>
                <td class="vd-amountcol">
                    <asp:Label ID="lblPaymentForMonth" runat="server" EnableViewState="false" SumType="PaymentsForMonth"
                        OnPreRender="lbl_PreRenderShowSum" />
                </td>
                <td class="vd-amountcol">
                    <asp:Label ID="lblPaymentsumUptoMonth" runat="server" EnableViewState="false" SumType="PaymentsUptoMonth"
                        OnPreRender="lbl_PreRenderShowSum" />
                </td>
                <td class="vd-amountcol">
                    <asp:Label ID="lblPaymentsum" runat="server" EnableViewState="false" SumType="PaymentsSum"
                        OnPreRender="lbl_PreRenderShowSum" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:HyperLink ID="hplnkfunds" ToolTip="Click to get list of Heads involved" Text="Funds in Transit"
                        NavigateUrl="~/Finance/AccountHeads.aspx?Types=FUNDS_TRANSIT" runat="server"></asp:HyperLink>
                </td>
                <td class="vd-amountcol">
                    <asp:HyperLink ID="hplnkFundTransitPreviousYear" runat="server" ToolTip="Click to list voucher entries for Funds Transfers prior to Financial Year"
                        EnableViewState="false"></asp:HyperLink>
                </td>
                <td class="vd-amountcol">
                    <asp:HyperLink ID="hplnkFundTransitForMonth" runat="server" ToolTip="Click to list voucher entries for Funds Transfers for input month"
                        EnableViewState="false"></asp:HyperLink>
                </td>
                <td class="vd-amountcol">
                    <asp:HyperLink ID="hplnkFundTransitUptoMonth" runat="server" ToolTip="Click to list voucher entries for Funds Transfers from financial year start till input month"
                        EnableViewState="false"></asp:HyperLink>
                </td>
                <td class="vd-amountcol">
                    <asp:Label ID="lblExciseDutyPreviousYear3" runat="server" ToolTip="Funds Transfers till date"
                        EnableViewState="false" />
                </td>
            </tr>
            <tr class="AlternatingRow">
                <td colspan="5" style="font-style: italic">
                    Closing Balances
                </td>
            </tr>
            <tr>
                <td style="text-indent: 4mm">
                    Closing Balance Rs./Nu
                </td>
                <td title="Closing balances as of <%# m_dtPreviousYear.AddDays(-1).ToString("d MMM'&nbsp;'yyyy")%> for local currency Cash & Bank accounts. Click to see closing balances of individual accounts."
                    class="vd-amountcol">
                    <asp:HyperLink ID="hplnkClBalPreviousYear" runat="server" EnableViewState="false" />
                </td>
                <td title="Closing balances as of <%# m_dtMonthEnd.ToString("d MMM'&nbsp;'yyyy")%> for local currency Cash & Bank accounts. Click to see opening balances of individual accounts."
                    class="vd-amountcol">
                    <asp:HyperLink ID="hplnkClBalForMonth" runat="server" EnableViewState="false" NavigateUrl="~/Finance/VoucherSearch.aspx?AccountTypes=CASH,BANKNU,INVESTMENT&DateTo={2:d}" />
                </td>
                <td title="Closing balances as of <%# m_dtMonthEnd.ToString("d MMM'&nbsp;'yyyy")%> for local currency Cash & Bank accounts. Click to see opening balances of individual accounts."
                    class="vd-amountcol">
                    <asp:HyperLink ID="hplnkClBalUptoMonth" runat="server" EnableViewState="false" />
                </td>
                <td class="vd-amountcol">
                    <asp:Label ID="lblClBalSum" runat="server" EnableViewState="false" />
                </td>
            </tr>
            <tr class="AlternatingRow">
                <td style="text-indent: 4mm">
                    Closing Balance FE
                </td>
                <td title="Closing balances as of <%# m_dtPreviousYear.AddDays(-1).ToString("d MMM'&nbsp;'yyyy")%> Foreign Currency Bank accounts. Click to see closing balances of individual accounts."
                    class="vd-amountcol">
                    <asp:HyperLink ID="hplnkClBalPreviousYearFE" runat="server" EnableViewState="false"
                        NavigateUrl="~/Finance/VoucherSearch.aspx?AccountTypes=BANKFE&FromDate={0:d}&ToDate={1:d}" />
                </td>
                <td title="Closing balances as of <%# m_dtMonthEnd.ToString("d MMM'&nbsp;'yyyy")%> for Foreign Currency Bank accounts. Click to see closing balances of individual accounts."
                    class="vd-amountcol">
                    <asp:HyperLink ID="hplnkClBalForMonthFE" runat="server" EnableViewState="false" NavigateUrl="~/Finance/VoucherSearch.aspx?AccountTypes=BANKFE&ToDate={0:d}" />
                </td>
                <td title="Closing balance as of <%# m_dtMonthEnd.ToString("d MMM'&nbsp;'yyyy")%> for Foreign Currency Bank accounts. Click to see closing balances of individual accounts."
                    class="vd-amountcol">
                    <asp:HyperLink ID="hplnkClBalUptoMonthFE" runat="server" EnableViewState="false"
                        NavigateUrl="~/Finance/VoucherSearch.aspx?AccountTypes=BANKFE&FromDate={0:d}&ToDate={1:d}" />
                </td>
                <td class="vd-amountcol">
                    <asp:Label ID="lblClBalFESum" runat="server" EnableViewState="false" />
                </td>
            </tr>
            <tr class="RowHeader">
                <td align="right">
                    Total
                </td>
                <td>
                    <asp:Label ID="lblClosingBalancePreviousYear" runat="server" ToolTip="Closing local currency balance prior to Financial year end"
                        EnableViewState="false" />
                </td>
                <td class="vd-amountcol">
                    <asp:Label ID="lblClosingBalanceForMonth" runat="server" ToolTip="Closing local currency balance for month"
                        EnableViewState="false" />
                </td>
                <td class="vd-amountcol">
                    <asp:Label ID="lblClosingBalanceUptoMonth" runat="server" ToolTip="Closing local currency balance upto month"
                        EnableViewState="false" />
                </td>
                <td class="vd-amountcol">
                    <asp:Label ID="lblCummulativeClosingBalance" runat="server" ToolTip="Closing local currency balance till date"
                        EnableViewState="false" />
                </td>
            </tr>
        </tbody>
    </table>
</asp:Content>
