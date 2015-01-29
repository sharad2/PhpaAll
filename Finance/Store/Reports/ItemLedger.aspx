<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" CodeBehind="ItemLedger.aspx.cs"
    Inherits="PhpaAll.Store.Reports.ItemLedger" Title="Item Ledger" EnableViewState="false" %>

<%@ Register Src="~/Controls/PrinterFriendlyButton.ascx" TagName="PrinterFriendlyButton"
    TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphSideNavigation" runat="server">
    <asp:HyperLink ID="HyperLink2" runat="server" Text="Help" NavigateUrl="~/Doc/ItemLedger.doc.aspx" />
    <uc2:PrinterFriendlyButton ID="PrinterFriendlyButton1" runat="server" />
    <fieldset>
        <legend>Quick Links</legend>
        <ul>
            <li>
                <asp:HyperLink runat="server" NavigateUrl="~/Store/GrnList.aspx" Text="View GRN List" />
            </li>
            <li>
                <asp:HyperLink runat="server" NavigateUrl="~/Store/CreateGRN.aspx" Text="Create New GRN" />
            </li>
            <li>
                <asp:HyperLink runat="server" NavigateUrl="~/Store/CreateSRS.aspx" Text="Create New SRS" />
            </li>
            <li>
                <asp:HyperLink runat="server" NavigateUrl="~/Store/Reports/SRSList.aspx" Text="View SRS List" />
            </li>
            <li>
                <asp:HyperLink runat="server" NavigateUrl="~/Store/Reports/StockBalance.aspx" Text="Stock Balance" />
            </li>
        </ul>
    </fieldset>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cph" runat="server">
    <phpa:PhpaLinqDataSource ID="dsMaterialIssue" runat="server" ContextTypeName="Eclipse.PhpaLibrary.Database.Store.StoreDataContext"
        TableName="SRSIssues" OnSelecting="dsMaterialIssue_Selecting" RenderLogVisible="false">
    </phpa:PhpaLinqDataSource>
    <eclipse:TwoColumnPanel ID="TwoColumnPanel1" runat="server" IsValidationContainer="true">
        <eclipse:LeftLabel runat="server" Text="From / To Date" />
        <i:TextBoxEx ID="tbFromDate" runat="server" FriendlyName="From Date" QueryString="FromDate"
            Text="-60">
            <Validators>
                <i:Date Max="0" />
                <i:Required />
            </Validators>
        </i:TextBoxEx>
        <i:TextBoxEx ID="tbToDate" runat="server" FriendlyName="To Date" QueryString="ToDate"
            Text="0">
            <Validators>
                <i:Date DateType="ToDate" Max="0" />
                <i:Required />
            </Validators>
        </i:TextBoxEx>
        <eclipse:LeftLabel runat="server" Text="Item" />
        <i:AutoComplete ID="tbItem" runat="server" FriendlyName="Items" QueryString="ItemId"
            WebMethod="GetItems" WebServicePath="~/Services/Items.asmx" Width="20em" ValidateWebMethodName="ValidateItem">
            <Validators>
                <i:Required />
            </Validators>
        </i:AutoComplete>
        <%--  <i:TextBoxEx ID="tbItem" runat="server" FriendlyName="Item" MaxLength="40" QueryString="ItemCode"
            CaseConversion="UpperCase" Size="10">
            <Validators>
                <i:Required />
            </Validators>
        </i:TextBoxEx>--%>
        <br />
        Enter item code or <a href="../InsertItem.aspx">select from list</a><br />
        <i:ButtonEx ID="btnGo" runat="server" Text="Go" CausesValidation="true" Action="Submit"
            Icon="Refresh"  IsDefault="true"/>
        <i:ValidationSummary ID="valSummary" runat="server" />
    </eclipse:TwoColumnPanel>
    <div style="font-size: large; margin-bottom: 1mm">
        <asp:Label ID="lblOpeningBalance" ToolTip="Opening balance of selected Item" runat="server"
            Visible="false" />
    </div>
    <jquery:GridViewEx ID="gvMaterialIssue" runat="server" DataSourceID="dsMaterialIssue"
        AutoGenerateColumns="false" AllowSorting="true" ShowFooter="true" OnRowDataBound="gvMaterialIssue_RowDataBound"
        OnDataBound="gvMaterialIssue_DataBound">
        <Columns>
            <eclipse:SequenceField>
            </eclipse:SequenceField>
            <eclipse:MultiBoundField DataFields="TransactionDate" DataFormatString="{0:d}" HeaderText="Date"
                SortExpression="ReceiveDate" HeaderToolTip="Date Accepted or Issued" />
            <asp:HyperLinkField HeaderText="GRN No" DataTextField="GRNNo" DataNavigateUrlFields="GRNId"
                DataNavigateUrlFormatString="~/Store/Reports/GRNReport.aspx?GRNId={0}" />
            <asp:HyperLinkField HeaderText="SRS No" DataTextField="SRSNo" DataNavigateUrlFields="SRSId"
                DataNavigateUrlFormatString="~/Store/Reports/SRSReport.aspx?SRSId={0}" />
                 <asp:HyperLinkField HeaderText="GIN No" DataTextField="SRSId" DataNavigateUrlFields="SRSId"
                DataNavigateUrlFormatString="~/Store/Reports/SRSReport.aspx?SRSId={0}" />
            <eclipse:MultiBoundField DataFields="TransactionQuantity" SortExpression="QtyAccepted"
                HeaderText="Quantity" DataFormatString="{0:#,### 'Received'; #,### 'Issued';''}"
                DataSummaryCalculation="ValueSummation" DataFooterFormatString="{0:N0}" AccessibleHeaderText="TransactionQuantity">
                <ItemStyle HorizontalAlign="Right" />
                <FooterStyle HorizontalAlign="Right" />
            </eclipse:MultiBoundField>
            <eclipse:MultiBoundField DataFields="AcceptedRate" SortExpression="AcceptedRate"
                HeaderText="Rate" HeaderToolTip="Price per unit + Landing Charges" DataFormatString="{0:N2}">
                <ItemStyle HorizontalAlign="Right" />
            </eclipse:MultiBoundField>
            <eclipse:MultiBoundField DataFields="Amount" SortExpression="Amount" HeaderText="Amount"
                DataFormatString="{0:#,###.00;(#,###.00);''}" DataSummaryCalculation="ValueSummation">
                <ItemStyle HorizontalAlign="Right" />
            </eclipse:MultiBoundField>
        </Columns>
    </jquery:GridViewEx>
    <br />
    <div style="font-size: large; margin-bottom: 1mm">
        <asp:Label ID="lblClosingBalance" ToolTip="Closing balance of selecte Item" runat="server"
            Visible="true" />
    </div>
</asp:Content>
