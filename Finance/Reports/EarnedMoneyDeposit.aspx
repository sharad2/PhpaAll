<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" CodeBehind="EarnedMoneyDeposit.aspx.cs"
    Inherits="PhpaAll.Reports.MoneyDeposit" Title="Earnest Money Deposit" EnableViewState="false" %>

<%@ Register Src="~/Controls/PrinterFriendlyButton.ascx" TagName="PrinterFriendlyButton"
    TagPrefix="uc2" %>
<asp:Content ID="Content4" ContentPlaceHolderID="cphSideNavigation" runat="server">
    <asp:HyperLink ID="hlHelp" runat="server" Text="Help" NavigateUrl="~/Doc/EarnedMoneyDeposit.doc.aspx" /><br />
    <uc2:PrinterFriendlyButton ID="PrinterFriendlyButton1" runat="server" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cph" runat="server">
    <div style="float: left">
        <eclipse:TwoColumnPanel runat="server">
            <eclipse:LeftLabel ID="LeftLabel1" runat="server" Text="Report Type" />
            <i:DropDownListEx runat="server" QueryString="ReportName" ID="ddlReportType" Value="SD">
                <Items>
                    <eclipse:DropDownItem Text="Security Deposit" Value="SD" Persistent="Always" />
                    <eclipse:DropDownItem Text="Earnest Money Deposit" Value="EMD" Persistent="Always" />
                </Items>
            </i:DropDownListEx>
            <eclipse:LeftLabel runat="server" Text="Date" />
            <i:TextBoxEx ID="tbdate" runat="server" FriendlyName="Date" Text="0">
                <Validators>
                    <i:Required />
                    <i:Date />
                </Validators>
            </i:TextBoxEx>
            <i:ButtonEx ID="btnshowreport" runat="server" CausesValidation="true" Action="Submit"
                Text="Go" IsDefault="true" Icon="Search"/>
        </eclipse:TwoColumnPanel>
        <i:ValidationSummary ID="ValidationSummary1" runat="server" />
        <br />
        <phpa:PhpaLinqDataSource ID="dsEMD" runat="server" TableName="" ContextTypeName="Eclipse.PhpaLibrary.Reporting.ReportingDataContext"
            RenderLogVisible="false" EnableViewState="true" OnSelecting="dsEMD_Selecting">
        </phpa:PhpaLinqDataSource>
        <jquery:GridViewEx ID="gvdsEMD" runat="server" AutoGenerateColumns="false" ShowFooter="true"
            AllowSorting="true" DataSourceID="dsEMD" Visible="true" EnableViewState="false"
            OnRowDataBound="gvdsEMD_RowDataBound">
            <Columns>
                <eclipse:SequenceField AccessibleHeaderText="#" FooterText=" " ItemStyle-Width="25px" />
                <eclipse:MultiBoundField HeaderText="Contractor | Code" DataFields="partycode" />
                <eclipse:MultiBoundField HeaderText="Contractor | Name" DataFields="partyname"
                    FooterText="Total" FooterStyle-HorizontalAlign="Left" ItemStyle-Width="3in" />
                <eclipse:HyperLinkFieldEx DataNavigateUrlFields="ReportType,partyId,VoucherDate"
                    HeaderText="Amount" DataNavigateUrlFormatString="~/Finance/VoucherSearch.aspx?AccountTypes={0}&ContractorId={1}&DateTo={2}"
                    DataTextField="Amount" DataTextFormatString="{0:C}" ItemStyle-HorizontalAlign="Right"
                    DataSummaryCalculation="ValueSummation" />
            </Columns>
            <EmptyDataTemplate>
                <b>No Data found for the given date.</b>
            </EmptyDataTemplate>
            <FooterStyle HorizontalAlign="Right" />
        </jquery:GridViewEx>
    </div>
</asp:Content>
