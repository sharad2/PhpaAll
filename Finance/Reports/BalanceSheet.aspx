<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" CodeBehind="BalanceSheet.aspx.cs"
    Inherits="PhpaAll.Reports.BalanceSheet" Title="Balance Sheet" %>

<%@ Register Src="../Controls/PrinterFriendlyButton.ascx" TagName="PrinterFriendlyButton"
    TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server" EnableViewState="false">
    <style type="text/css">
        .bs_SectionHeader
        {
            font-size: large;
            padding-left: 2mm;
            padding-top: 2mm;
        }
        .bs_table
        {
            float: left;
            width: 26em;
            table-layout: fixed;
            border: solid .25mm black;
            height: 100%;
        }
        .bs_HeaderCell
        {
            font-size: 14pt;
            border-bottom: solid 0.5mm black;
        }
        .bs_FooterCell
        {
            font-size: 14pt;
            border-top: solid 0.5mm black;
            text-align: right;
        }
        .bs_LeftColumn
        {
            width: 15em;
            padding-left: 6mm;
            padding-bottom: 1mm;
            padding-top: 1mm;
            font-size: 11pt;
        }
        .bs_RightColumn
        {
            width: 10em;
            padding-right: 2mm;
            text-align: right;
            font-size: 12pt;
        }
    </style>
    <script type="text/javascript">
        function ShowDisclaimer() {
            return confirm('The screen you are about to see is meant for diagnostic purposes only.it may take a long time to run. The information displayed may not be readily comprehensible.');
        }
    </script>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphSideNavigation" runat="server">
    <asp:HyperLink ID="HyperLink2" runat="server" Text="Help" NavigateUrl="~/Doc/BalanceSheet.doc.aspx" /><br />
    <uc2:PrinterFriendlyButton ID="PrinterFriendlyButton1" runat="server" />
</asp:Content>
<asp:Content ID="c3" ContentPlaceHolderID="cph" runat="server">
    <phpa:PhpaLinqDataSource ID="dsQueries" runat="server" ContextTypeName="Eclipse.PhpaLibrary.Reporting.ReportingDataContext"
        TableName="ReportConfigurations" RenderLogVisible="false">
    </phpa:PhpaLinqDataSource>
    <eclipse:TwoColumnPanel runat="server">
        <eclipse:LeftLabel Text="Date" runat="server" />
        <i:TextBoxEx runat="server" ID="tbdate" FriendlyName="Date">
            <Validators>
                <i:Required />
                <i:Date />
            </Validators>
        </i:TextBoxEx>
        <br />
        <i:ButtonEx runat="server" Text="Recalculate" CausesValidation="true" Action="Submit" IsDefault="true"/>
           <i:ButtonEx ID="btnExport" runat="server" Text="Export" OnClick="btnExport_Click"
            Action="Submit" Icon="None" CausesValidation="true" />
    </eclipse:TwoColumnPanel>
    <br />
    <i:ValidationSummary ID="ValidationSummary1" runat="server" />
    <asp:PlaceHolder runat="server" ID="plhTable">
        <div style="min-width: 60em; width: 100%; height: 5.5in">
            <table cellspacing="0" class="bs_table" rules="cols" >
                <thead>
                    <tr>
                        <th align="center" class="bs_LeftColumn bs_HeaderCell">
                            Liabilities
                        </th>
                        <th align="center" class="bs_RightColumn bs_HeaderCell">
                            Amount(Nu.)
                        </th>
                    </tr>
                </thead>
                <tbody>
                    <tr>
                        <td class="bs_SectionHeader">
                            Capital Funds/Contributions
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr runat="server" onprerender="liability_PreRender">
                        <td class="bs_LeftColumn">
                            <asp:HyperLink ID="hplnkGrant" runat="server" NavigateUrl="~/Finance/AccountHeads.aspx?Types=GRANT_RECEIVED_GOINU,GRANT_RECEIVED_GOIFE">Grant Received from GOI</asp:HyperLink>
                        </td>
                        <td class="bs_RightColumn">
                            <asp:HyperLink ID="hplnkgrantreceived" runat="server" Text="0.00" EnableViewState="false"
                                ToolTip="Click to list voucher entries for Grant Received from Government of India"
                                onclick="return ShowDisclaimer();"></asp:HyperLink>
                        </td>
                    </tr>
                    <tr runat="server" onprerender="liability_PreRender">
                        <td class="bs_LeftColumn">
                            <asp:HyperLink ID="hplnkLoan" runat="server" NavigateUrl="~/Finance/AccountHeads.aspx?Types=LOAN_RECEIVED_GOINU,LOAN_RECEIVED_GOIFE">Loan Received from GOI</asp:HyperLink>
                        </td>
                        <td class="bs_RightColumn">
                            <asp:HyperLink ID="hplnkloanreceived" runat="server" Text="0.00" EnableViewState="false"
                                ToolTip="Click to list voucher entries for Loan Received from Government of India"
                                onclick="return ShowDisclaimer();"></asp:HyperLink>
                        </td>
                    </tr>
                    <tr >
                        <td class="bs_SectionHeader">
                            Accumulated Receipts
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr runat="server" onprerender="liability_PreRender">
                        <td class="bs_LeftColumn">
                            <asp:HyperLink ID="hplInterest" runat="server" Text="Interest" NavigateUrl="~/Finance/AccountHeads.aspx?Types=INTEREST"></asp:HyperLink>
                        </td>
                        <td class="bs_RightColumn">
                            <asp:HyperLink ID="INTEREST" runat="server" Text="0.00" EnableViewState="false" ToolTip="Click to list voucher entries for Interest"
                                onclick="return ShowDisclaimer();"></asp:HyperLink>
                        </td>
                    </tr>
                    <tr runat="server" onprerender="liability_PreRender">
                        <td class="bs_LeftColumn">
                            <asp:HyperLink ID="hplnkAccRec" runat="server" Text="Other Earnings" NavigateUrl="~/Finance/AccountHeads.aspx?Types=ACCUMULATED_RECEIPTS,TENDER_SALE"></asp:HyperLink>
                        </td>
                        <td class="bs_RightColumn">
                            <asp:HyperLink ID="hplnkAcc_Rec" runat="server" Text="0.00" EnableViewState="false"
                                ToolTip="Click to list voucher entries for Accumulated_Receipts & Tender_Sale"
                                onclick="return ShowDisclaimer();"></asp:HyperLink>
                        </td>
                    </tr>
                    <tr>
                        <td class="bs_SectionHeader">
                            Creditors
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr runat="server" onprerender="liability_PreRender">
                        <td class="bs_LeftColumn">
                            <asp:HyperLink ID="hplnkemd" runat="server" Text="Earnest Money Deposit" NavigateUrl="~/Finance/AccountHeads.aspx?Types=EMD"></asp:HyperLink>
                        </td>
                        <td class="bs_RightColumn">
                            <asp:HyperLink ID="EMD" runat="server" Text="0.00" EnableViewState="false" ToolTip="Click to list voucher entries for Earned Money Deposit"
                                onclick="return ShowDisclaimer();"></asp:HyperLink>
                        </td>
                    </tr>
                    <tr runat="server" onprerender="liability_PreRender">
                        <td class="bs_LeftColumn">
                            <asp:HyperLink ID="hplnksd" runat="server" Text="Security Deposit" NavigateUrl="~/Finance/AccountHeads.aspx?Types=SD"></asp:HyperLink>
                        </td>
                        <td class="bs_RightColumn">
                            <asp:HyperLink ID="SD" runat="server" Text="0.00" EnableViewState="false" ToolTip="Click to list voucher enteries for Security Deposit"
                                onclick="return ShowDisclaimer();"></asp:HyperLink>
                        </td>
                    </tr>
                    <tr>
                        <td class="bs_SectionHeader">
                            Other Payables
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr runat="server" onprerender="liability_PreRender">
                        <td class="bs_LeftColumn">
                            <asp:HyperLink ID="hplnkbit" runat="server" Text="BIT" NavigateUrl="~/Finance/AccountHeads.aspx?Types=BIT"></asp:HyperLink>
                        </td>
                        <td class="bs_RightColumn">
                            <asp:HyperLink ID="BIT" runat="server" Text="0.00" EnableViewState="false" ToolTip="Click to list voucher entries for Contractor Tax"
                                onclick="return ShowDisclaimer();"></asp:HyperLink>
                        </td>
                    </tr>
                    <tr runat="server" onprerender="liability_PreRender">
                        <td class="bs_LeftColumn">
                            <asp:HyperLink ID="hplnksalrem" runat="server" Text="Salary Remittances" NavigateUrl="~/Finance/AccountHeads.aspx?Types=SALARY_REMITANCES"></asp:HyperLink>
                        </td>
                        <td class="bs_RightColumn">
                            <asp:HyperLink ID="SALARY_REMITANCES" runat="server" Text="0.00" EnableViewState="false"
                                ToolTip="Click to list voucher entries for Salary Allownces" onclick="return ShowDisclaimer();"></asp:HyperLink>
                        </td>
                    </tr>
                    <tr runat="server" onprerender="liability_PreRender">
                        <td class="bs_LeftColumn">
                            <asp:HyperLink ID="hplnkotlib" runat="server" Text="Other Liablities" NavigateUrl="~/Finance/AccountHeads.aspx?Types=LIABILITY"></asp:HyperLink>
                        </td>
                        <td class="bs_RightColumn">
                            <asp:HyperLink ID="LIABILITY" runat="server" Text="0.00" EnableViewState="false"
                                ToolTip="Click to list voucher entries for Other Liability" onclick="return ShowDisclaimer();"></asp:HyperLink>
                        </td>
                    </tr>
                </tbody>
                <tfoot>
                    <tr>
                        <td class="bs_FooterCell" colspan="2">
                            <asp:Label ID="lblSumLiabilities" runat="server" Text="0.00" EnableViewState="false"></asp:Label>
                        </td>
                    </tr>
                </tfoot>
            </table>
            <table cellspacing="0" class="bs_table" rules="cols">
                <thead>
                    <tr>
                        <th align="center" class="bs_LeftColumn bs_HeaderCell">
                            Assets
                        </th>
                        <th align="center" class="bs_RightColumn bs_HeaderCell">
                            Amount(Nu.)
                        </th>
                    </tr>
                </thead>
                <tbody>
                    <tr>
                        <td class="bs_LeftColumn">
                            <asp:HyperLink ID="hplnkwip" runat="server" Text="Work In Progress" NavigateUrl="~/Finance/AccountHeads.aspx?Types=EXPENDITURE"></asp:HyperLink>
                        </td>
                        <td class="bs_RightColumn">
                            <asp:HyperLink ID="hlExpenditure" runat="server" Text="0.00" EnableViewState="false"
                                ToolTip="Click to list voucher entries for Project,Civil,Electrical & Transmission Works"
                                onclick="return ShowDisclaimer();"></asp:HyperLink>
                        </td>
                    </tr>
                    <tr>
                        <td class="bs_LeftColumn">
                            <asp:HyperLink ID="hplnkStockSuspense" runat="server" Text="Stock Suspense" NavigateUrl="~/Finance/AccountHeads.aspx?Types=STOCK_SUSPENSE"></asp:HyperLink>
                        </td>
                        <td class="bs_RightColumn">
                            <asp:HyperLink ID="STOCK_SUSPENSE" runat="server" Text="0.00" EnableViewState="false"
                                ToolTip="Click to list voucher entries for Suspense Stock" onclick="return ShowDisclaimer();"></asp:HyperLink>
                        </td>
                    </tr>
                    <tr>
                        <td class="bs_SectionHeader">
                            Advances
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td class="bs_LeftColumn">
                            <asp:HyperLink ID="hplnkEmployee" runat="server" Text="Employee" NavigateUrl="~/Finance/AccountHeads.aspx?Types=EMPLOYEE_ADVANCE"></asp:HyperLink>
                        </td>
                        <td class="bs_RightColumn">
                            <asp:HyperLink ID="EMPLOYEE_ADVANCE" runat="server" Text="0.00" EnableViewState="false"
                                ToolTip="Click to list voucher entries for Employee Advances" onclick="return ShowDisclaimer();"></asp:HyperLink>
                        </td>
                    </tr>
                    <tr>
                        <td class="bs_LeftColumn">
                            <asp:HyperLink ID="hplnkparties" runat="server" Text="Parties" NavigateUrl="~/Finance/AccountHeads.aspx?Types=PARTY_ADVANCE"></asp:HyperLink>
                        </td>
                        <td class="bs_RightColumn">
                            <asp:HyperLink ID="hlContractorAdvance" runat="server" Text="0.00" EnableViewState="false"
                                ToolTip="Click to list voucher entries for Party Advances" onclick="return ShowDisclaimer();"></asp:HyperLink>
                        </td>
                    </tr>
                    <tr>
                        <td class="bs_SectionHeader">
                            Receivables
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td class="bs_LeftColumn">
                            <asp:HyperLink ID="hplnkbst" runat="server" Text="BST" NavigateUrl="~/Finance/AccountHeads.aspx?Types=BST"></asp:HyperLink>
                        </td>
                        <td class="bs_RightColumn">
                            <asp:HyperLink ID="BST" runat="server" Text="0.00" EnableViewState="false" ToolTip="Click to list voucher entries for Bhutan Service Tax"
                                onclick="return ShowDisclaimer();"></asp:HyperLink>
                        </td>
                    </tr>
                    <tr>
                        <td class="bs_LeftColumn">
                            <asp:HyperLink ID="HyperLink1" runat="server" Text="Service Tax" NavigateUrl="~/Finance/AccountHeads.aspx?Types=Service_Tax"></asp:HyperLink>
                        </td>
                        <td class="bs_RightColumn">
                            <asp:HyperLink ID="SVCTAX" runat="server" Text="0.00" EnableViewState="false" onclick="return ShowDisclaimer();"></asp:HyperLink>
                        </td>
                    </tr>
                    <tr>
                        <td class="bs_LeftColumn">
                            <asp:HyperLink ID="hplnkExciseGOI" runat="server" Text="Excise Duty(GOI)" NavigateUrl="~/Finance/AccountHeads.aspx?Types=EDGOI"></asp:HyperLink>
                        </td>
                        <td class="bs_RightColumn">
                            <asp:HyperLink ID="EDGOI" runat="server" Text="0.00" EnableViewState="false" ToolTip="Click to list voucher entries for Excise Duty(GOI)"
                                onclick="return ShowDisclaimer();"></asp:HyperLink>
                        </td>
                    </tr>
                    <tr>
                        <td class="bs_LeftColumn">
                            <asp:HyperLink ID="hplnkExciseRGOB" runat="server" Text="Excise Duty(RGOB)" NavigateUrl="~/Finance/AccountHeads.aspx?Types=EDRGOB"></asp:HyperLink>
                        </td>
                        <td class="bs_RightColumn">
                            <asp:HyperLink ID="EDRGOB" runat="server" Text="0.00" EnableViewState="false" ToolTip="Click to list voucher entries for Excise Duty(RGOB)"
                                onclick="return ShowDisclaimer();"></asp:HyperLink>
                        </td>
                    </tr>
                    <tr>
                        <td class="bs_LeftColumn">
                            <asp:HyperLink ID="hpllnkGreenTax" runat="server" Text="Green Tax(RGOB)" NavigateUrl="~/Finance/AccountHeads.aspx?Types=GREEN_TAX"></asp:HyperLink>
                        </td>
                        <td class="bs_RightColumn"><asp:HyperLink ID="hplnkgtax" runat="server" Text="0.00" EnableViewState="false" ToolTip="Click to list the voucher entries for Green tax(RGOB)" onclick="return ShowDisclaimer();"></asp:HyperLink>
                            </td>
                    </tr>
                    <tr>
                        <td class="bs_LeftColumn">
                            <asp:HyperLink ID="hplnkotassets" runat="server" Text="Other Assets" NavigateUrl="~/Finance/AccountHeads.aspx?Types=ASSETS"></asp:HyperLink>
                        </td>
                        <td class="bs_RightColumn">
                            <asp:HyperLink ID="ASSETS" runat="server" Text="0.00" EnableViewState="false" ToolTip="Click to list voucher entries for Other Assets"
                                onclick="return ShowDisclaimer();"></asp:HyperLink>
                        </td>
                    </tr>
                    <tr>
                        <td class="bs_SectionHeader">
                            Closing Balances
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td class="bs_LeftColumn">
                            <asp:HyperLink ID="hplnkfundintransit" runat="server" Text="Funds in Transit" NavigateUrl="~/Finance/AccountHeads.aspx?Types=FUNDS_TRANSIT"></asp:HyperLink>
                        </td>
                        <td class="bs_RightColumn">
                            <asp:HyperLink ID="FUNDS_TRANSIT" runat="server" Text="0.00" EnableViewState="false"
                                ToolTip="Click to list voucher entries for Funds Transfer" onclick="return ShowDisclaimer();"></asp:HyperLink>
                        </td>
                    </tr>
                    <tr>
                        <td class="bs_LeftColumn">
                            <asp:HyperLink ID="hplnkinv" runat="server" Text="Investment" NavigateUrl="~/Finance/AccountHeads.aspx?Types=INVESTMENT"></asp:HyperLink>
                        </td>
                        <td class="bs_RightColumn">
                            <asp:HyperLink ID="INVESTMENT" runat="server" Text="0.00" EnableViewState="false"
                                ToolTip="Click to list voucher entries for Investment" onclick="return ShowDisclaimer();"></asp:HyperLink>
                        </td>
                    </tr>
                    <tr>
                        <td class="bs_LeftColumn">
                            <asp:HyperLink ID="hplnkcashinhand" runat="server" Text="Cash in Hand" NavigateUrl="~/Finance/AccountHeads.aspx?Types=CASH"></asp:HyperLink>
                        </td>
                        <td class="bs_RightColumn">
                            <asp:HyperLink ID="CASH" runat="server" Text="0.00" EnableViewState="false" ToolTip="Click to list voucher entries for Cash"
                                onclick="return ShowDisclaimer();"></asp:HyperLink>
                        </td>
                    </tr>
                    <tr>
                        <td class="bs_LeftColumn">
                            <asp:HyperLink ID="hplnkcashatbnkFE" runat="server" Text="Cash at Bank(FE)" NavigateUrl="~/Finance/AccountHeads.aspx?Types=BANKFE"></asp:HyperLink>
                        </td>
                        <td class="bs_RightColumn">
                            <asp:HyperLink ID="BANKFE" runat="server" Text="0.00" EnableViewState="false" ToolTip="Click to list voucher entries for Bank"
                                onclick="return ShowDisclaimer();"></asp:HyperLink>
                        </td>
                    </tr>
                    <tr>
                        <td class="bs_LeftColumn">
                            <asp:HyperLink ID="hplnkcashatbnkNU" runat="server" Text="Cash at Bank(NU)" NavigateUrl="~/Finance/AccountHeads.aspx?Types=BANKNU"></asp:HyperLink>
                        </td>
                        <td class="bs_RightColumn">
                            <asp:HyperLink ID="BANKNU" runat="server" Text="0.00" EnableViewState="false" ToolTip="Click to list voucher entries for Bank"
                                onclick="return ShowDisclaimer();"></asp:HyperLink>
                        </td>
                    </tr>
                </tbody>
                <tfoot>
                    <tr>
                        <td class="bs_FooterCell" colspan="2">
                            <asp:Label ID="lblSumAssets" runat="server" Text="0.00" EnableViewState="false"></asp:Label>
                        </td>
                    </tr>
                </tfoot>
            </table>
        </div>
    </asp:PlaceHolder>
</asp:Content>
