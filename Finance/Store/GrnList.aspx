<%@ Page Title="GRN List" Language="C#" MasterPageFile="~/MasterPage.master" CodeBehind="GrnList.aspx.cs"
    Inherits="PhpaAll.Store.GrnList" EnableViewState="false" %>

<%@ Register Src="../Controls/PrinterFriendlyButton.ascx" TagName="PrinterFriendlyButton"
    TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
 
<asp:Content ID="Content3" ContentPlaceHolderID="cphSideNavigation" runat="server">
    <uc2:PrinterFriendlyButton ID="PrinterFriendlyButton1" runat="server" />
    <fieldset>
        <legend>Quick Links</legend>
        <ul>
            <li>
                <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="~/Store/Reports/GRNReport.aspx"
                    Text="GRN Report" />
            </li>
            <li>
                <asp:HyperLink ID="HyperLink2" runat="server" NavigateUrl="~/Store/CreateGRN.aspx"
                    Text="Create New GRN" />
            </li>
        </ul>
    </fieldset>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cph" runat="server">
    <phpa:PhpaLinqDataSource ID="dsGrnBrief" runat="server" ContextTypeName="Eclipse.PhpaLibrary.Database.Store.StoreDataContext"
        TableName="GRNs" RenderLogVisible="False" OnSelecting="dsGrnBrief_Selecting"
        OnContextCreated="dsGrnBrief_ContextCreated">
    </phpa:PhpaLinqDataSource>
    <eclipse:TwoColumnPanel ID="panelGrid" runat="server">
        <eclipse:LeftLabel ID="LeftLabel1" runat="server" Text="From Date / To Date" />
        <i:TextBoxEx ID="tbFromDate" runat="server" FriendlyName="From Date"
            Text="-60">
            <Validators>
                <i:Required />
                <i:Date Max="0" />
            </Validators>
        </i:TextBoxEx>
        <i:TextBoxEx ID="tbToDate" runat="server" FriendlyName="To Date"
            Text="0">
            <Validators>
                <i:Required />
                <i:Date DateType="ToDate" Max="0" />
            </Validators>
        </i:TextBoxEx>
        <eclipse:LeftLabel ID="LeftLabel2" runat="server" Text="Receive Status" />
        <i:RadioButtonListEx ID="rblShowGrn" runat="server" Orientation="Horizontal" Value="UR">
            <Items>
                <i:RadioItem Text="Unreceived" Value="UR" />
                <i:RadioItem Text="Received" Value="R" />
                <i:RadioItem Text="All" Value="All" />
            </Items>
        </i:RadioButtonListEx>
        <eclipse:LeftLabel runat="server" Text="PO" />
        <i:TextBoxEx runat="server" ID="tbPo" CaseConversion="UpperCase" />
        <eclipse:LeftLabel runat="server" Text="Supplier" />
        <i:TextBoxEx runat="server" ID="tbSupplier" />
        <asp:HyperLink runat="server" NavigateUrl="~/Finance/Contractor.aspx" Text="View List" />
        <br />
        Specify supplier code or partial name.<br />
        <i:ButtonEx ID="btnShowGRN" runat="server" Text="Go" CausesValidation="true" 
            Action="Submit" Icon="Refresh" IsDefault="true" />
        <i:ButtonEx ID="btnClear" runat="server" Text="Clear Filter" CausesValidation="false"
            Action="Reset" Icon="Cancel" />
        <i:ValidationSummary ID="ValidationSummary1" runat="server" />
    </eclipse:TwoColumnPanel>
    <br />
    <jquery:GridViewEx ID="gvGrnBrief" runat="server" AutoGenerateColumns="False" AllowSorting="True"
        DataSourceID="dsGrnBrief" DataKeyNames="GRNId" AllowPaging="True" EnableViewState="false"
        PageSize="100" ShowFooter="true" DefaultSortExpression="GRNCreateDate {0:I}">
        <Columns>
            <eclipse:SequenceField />
            <asp:HyperLinkField  DataTextField="GRNCode" HeaderText="GRN|No" DataNavigateUrlFields="GRNId" 
             DataNavigateUrlFormatString="~/Store/CreateGRN.aspx?GRNId={0}"/>
            <eclipse:MultiBoundField DataFields="GRNCreateDate" HeaderText="GRN|Created" SortExpression="GRNCreateDate"
                DataFormatString="{0:d}" />
            <eclipse:MultiBoundField DataFields="GRNReceiveDate" HeaderText="GRN|Received" SortExpression="GRNReceiveDate"
                DataFormatString="{0:d}" />
            <eclipse:MultiBoundField DataFields="PONumber" HeaderText="Purchase Order|No." SortExpression="PONumber" />
            <eclipse:MultiBoundField DataFields="PODate" HeaderText="Purchase Order|Date" SortExpression="PODate"
                DataFormatString="{0:d}" />
            <eclipse:MultiBoundField DataFields="NameOfCarrier" HeaderText="Carrier Name" SortExpression="NameOfCarrier" />
            <eclipse:MultiBoundField DataFields="Supplier" HeaderText="Supplier" SortExpression="Supplier"
                FooterText="Grand Total" />
            <eclipse:MultiBoundField DataFields="CountItems" HeaderText="# Items" SortExpression="CountItems"
                DataFormatString="{0:N0}">
                <ItemStyle HorizontalAlign="Right" />
            </eclipse:MultiBoundField>
        </Columns>
        <EmptyDataTemplate>
            <b>GRN not found for the given date range.</b>
        </EmptyDataTemplate>
    </jquery:GridViewEx>
</asp:Content>
