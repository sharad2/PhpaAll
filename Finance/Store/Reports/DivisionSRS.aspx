<%@ Page Title="Issue Report" Language="C#" MasterPageFile="~/MasterPage.master"
    CodeBehind="DivisionSRS.aspx.cs" Inherits="Finance.Store.Reports.DivisionSRS"
    EnableViewState="false" %>

<%@ Register Src="~/Controls/PrinterFriendlyButton.ascx" TagName="PrinterFriendlyButton"
    TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphSideNavigation" runat="server">
    <uc2:PrinterFriendlyButton ID="PrinterFriendlyButton1" runat="server" />
    <fieldset>
        <legend>Quick Links</legend>
        <ul>
            <li>
                <asp:HyperLink runat="server" NavigateUrl="~/Store/Reports/SRSList.aspx" Text="SRS List" />
            </li>
            <li>
                <asp:HyperLink runat="server" NavigateUrl="~/Store/CreateSRS.aspx" Text="Create New SRS" />
            </li>
        </ul>
    </fieldset>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cph" runat="server">
    <eclipse:TwoColumnPanel ID="TwoColumnPanel" runat="server" IsValidationContainer="true">
        <eclipse:LeftLabel runat="server" Text="From / To Date" />
        <i:TextBoxEx ID="tbFromDate" runat="server">
            <Validators>
                <i:Date />
            </Validators>
        </i:TextBoxEx>
        <i:TextBoxEx ID="tbToDate" runat="server">
            <Validators>
                <i:Date DateType="ToDate" />
            </Validators>
        </i:TextBoxEx>
        <eclipse:LeftLabel runat="server" Text="Item" />
        <i:TextBoxEx ID="tbItem" runat="server" FriendlyName="Item" CaseConversion="UpperCase" />
        <eclipse:LeftLabel runat="server" Text="SRS No" />
        <i:TextBoxEx ID="tbSRSNo" runat="server" CaseConversion="UpperCase" />
        <eclipse:LeftLabel ID="LeftLabel1" runat="server" Text="Issued against GRN" />
        <i:TextBoxEx ID="tbGrnCode" runat="server" CaseConversion="UpperCase" />
        <eclipse:LeftLabel runat="server" Text="Division" />
        <phpa:PhpaLinqDataSource runat="server" ID="dsDivision" ContextTypeName="Eclipse.PhpaLibrary.Database.PIS.PISDataContext"
            Select="new (DivisionId, DivisionName, DivisionGroup)" TableName="Divisions" RenderLogVisible="false">
        </phpa:PhpaLinqDataSource>
        <i:DropDownListEx runat="server" ID="ddlDivision" DataSourceID="dsDivision" DataTextField="DivisionName"
            DataValueField="DivisionId" Value='<%# Bind("DivisionId") %>' FriendlyName="Division" DataOptionGroupField="DivisionGroup">
            <Items>
                <eclipse:DropDownItem Text="(All Divisions)" Value="" Persistent="Always" />
            </Items>
        </i:DropDownListEx>
        <br />
        <i:ButtonEx ID="btnShow" runat="server" Text="Go" CausesValidation="true" Icon="Refresh"
            Action="Submit" IsDefault="true" />
        <i:ButtonEx ID="btnClear" runat="server" Text="Clear" Action="Reset" />
        <i:ValidationSummary ID="valSummary" runat="server" />
    </eclipse:TwoColumnPanel>
    <phpa:PhpaLinqDataSource ID="dsIssueItems" runat="server" ContextTypeName="Eclipse.PhpaLibrary.Database.Store.StoreDataContext"
        TableName="SRSIssueItems" OnSelecting="dsIssueItems_Selecting" RenderLogVisible="false">
    </phpa:PhpaLinqDataSource>
    <jquery:GridViewEx ID="gvIssueItems" runat="server" DataSourceID="dsIssueItems" ShowFooter="true"
        AutoGenerateColumns="false" AllowSorting="true" OnRowDataBound="gvIssueItems_RowDataBound"
        AllowPaging="true" PageSize="100">
        <Columns>
            <eclipse:SequenceField />
            <eclipse:HyperLinkFieldEx DataTextField="SRSId" HeaderText="GIN" DataNavigateUrlFields="SRSId"
                DataNavigateUrlFormatString="~/Store/CreateSRS.aspx?SRSId={0}">
            </eclipse:HyperLinkFieldEx>
            <eclipse:MultiBoundField DataFields="SRSNo" HeaderText="SRS No" SortExpression="SRSCode" />
            <asp:HyperLinkField HeaderText="GRN No" DataTextField="GRNCode" DataNavigateUrlFields="GRNId"
                DataNavigateUrlFormatString="~/Store/Reports/GRNReport.aspx?GRNId={0}" />
            <eclipse:MultiBoundField DataFields="IssueDate" HeaderText="Issue Date" SortExpression="IssueDate"
                DataFormatString="{0:d}" />
            <eclipse:MultiBoundField DataFields="IssuedTo" HeaderText="Issued To" SortExpression="IssuedTo" />
            <eclipse:MultiBoundField DataFields="SRSFrom" HeaderText="Division" SortExpression="SRSFrom" />
            <eclipse:MultiBoundField DataFields="ItemCode,ItemDescription" HeaderText="Item"
                SortExpression="ItemCode" DataFormatString="{0}: {1}" />
            <eclipse:MultiBoundField DataFields="IssueQty" DataFormatString="{0:N0}" HeaderText="Issued Qty"
                SortExpression="IssueQty" DataSummaryCalculation="ValueSummation">
                <ItemStyle HorizontalAlign="Right" />
                <FooterStyle HorizontalAlign="Right" />
            </eclipse:MultiBoundField>
            <eclipse:MultiBoundField DataFields="Price" DataFormatString="{0:N2}" HeaderText="Rate"
                SortExpression="Price" HeaderToolTip="Price = Item Cost + Landing charges">
                <ItemStyle HorizontalAlign="Right" />
            </eclipse:MultiBoundField>
            <eclipse:MultiBoundField DataFields="TotalPrice" DataFormatString="{0:N2}" HeaderText="Total Price"
                SortExpression="TotalPrice" HeaderToolTip="Total Price = (Price * Issued Quantity)"
                DataSummaryCalculation="ValueSummation" ItemStyle-HorizontalAlign="Right">
                <ItemStyle HorizontalAlign="Right" />
            </eclipse:MultiBoundField>
        </Columns>
        <EmptyDataTemplate>
            <b>No data found for the given date range or parameters.</b>
        </EmptyDataTemplate>
    </jquery:GridViewEx>
</asp:Content>
