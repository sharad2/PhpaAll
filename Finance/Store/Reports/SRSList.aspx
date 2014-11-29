<%@ Page Title="GIN List" Language="C#" MasterPageFile="~/MasterPage.master" CodeBehind="SRSList.aspx.cs"
    Inherits="Finance.Store.Reports.SRSList" EnableViewState="false" %>

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
                <asp:HyperLink runat="server" NavigateUrl="~/Store/Reports/IssueSRS.aspx" Text="Issue SRS" />
            </li>
            <li>
                <asp:HyperLink runat="server" NavigateUrl="~/Store/CreateSRS.aspx" Text="Create New SRS" />
            </li>
            <li>
                <asp:HyperLink runat="server" NavigateUrl="~/Store/Reports/StockBalance.aspx" Text="View Stock Balance" />
            </li>
        </ul>
    </fieldset>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cph" runat="server">
    <eclipse:TwoColumnPanel ID="panelGrid" runat="server">
        <eclipse:LeftLabel runat="server" Text="SRS Create Date From / To" />
        <i:TextBoxEx ID="tbFromDate" runat="server" FriendlyName="From Date" Text="-30">
            <Validators>
                <i:Date Max="0"/>
            </Validators>
        </i:TextBoxEx>
        <i:TextBoxEx ID="tbToDate" runat="server" FriendlyName="To Date" Text="0">
            <Validators>
                <i:Date DateType="ToDate" Max="0"/>
            </Validators>
        </i:TextBoxEx>
        <eclipse:LeftLabel ID="LeftLabel1" runat="server" Text="Issue Status" />
        <i:RadioButtonListEx runat="server" ID="rblIssueStatus" WidthItem="12em">
            <Items>
                <i:RadioItem Text="Not yet issued" Value="N" />
                <i:RadioItem Text="Partially issued" Value="P" />
                <i:RadioItem Text="Fully issued" Value="F" />
                <i:RadioItem Text="All" />
            </Items>
        </i:RadioButtonListEx>
        <eclipse:LeftLabel ID="LeftLabel2" runat="server" Text="Containing Item Code" />
        <i:TextBoxEx runat="server" ID="tbItemCode" CaseConversion="UpperCase" FriendlyName="Item Code" />
    </eclipse:TwoColumnPanel>
    <i:ValidationSummary ID="ValidationSummary1" runat="server" />
    <i:ButtonEx ID="btnShowSrs" runat="server" Text="Go" CausesValidation="true" Action="Submit" Icon="Refresh" IsDefault="true"/>
    <phpa:PhpaLinqDataSource ID="dsBriefSRS" runat="server" ContextTypeName="Eclipse.PhpaLibrary.Database.Store.StoreDataContext"
        TableName="SRS" RenderLogVisible="False" OnSelecting="dsBriefSRS_Selecting">
    </phpa:PhpaLinqDataSource>
    <jquery:GridViewEx ID="gvSrsBrief" runat="server" AllowSorting="True" AutoGenerateColumns="False"
        DataSourceID="dsBriefSRS" Caption="Display a list of pending SRS's" OnRowDataBound="gvSrsBrief_RowDataBound"
        EnableViewState="false" ShowFooter="true" AllowPaging="true" PageSize="500" DataKeyNames="SRSId">
        <Columns>
            <eclipse:SequenceField />
            <eclipse:HyperLinkFieldEx DataTextField="SRSId" HeaderText="GIN" DataNavigateUrlFields="SRSId"
                DataNavigateUrlFormatString="~/Store/CreateSRS.aspx?SRSId={0}">
            </eclipse:HyperLinkFieldEx>
            <eclipse:MultiBoundField DataFields="SRSCode" HeaderText="SRS No" SortExpression="SRSCode" />
               <eclipse:MultiBoundField DataFields="SRSCreateDate" HeaderText="Created" SortExpression="SRSCreateDate"
                DataFormatString="{0:d}" />
            <eclipse:MultiBoundField DataFields="SRSFrom" HeaderText="SRS From" SortExpression="SRSFrom" />
            <eclipse:MultiBoundField DataFields="SRSTo" HeaderText="SRS To" SortExpression="SRSTo" />
            <eclipse:MultiBoundField DataFields="IssuedTo" HeaderText="Material Issued To" SortExpression="IssuedTo"
                FooterText="Total" />
            <eclipse:MultiBoundField DataFields="ItemCount" DataFormatString="{0:N0}" HeaderText="Item Count|Required"
                SortExpression="ItemCount">
                <ItemStyle HorizontalAlign="Right" />
            </eclipse:MultiBoundField>
            <eclipse:HyperLinkFieldEx DataTextField="IssueCount" DataTextFormatString="{0:N0}"
                HeaderText="Item Count|Issued" SortExpression="IssueCount" DataNavigateUrlFormatString="~/Store/Reports/DivisionSRS.aspx?SRSId={0}"
                DataNavigateUrlFields="SRSId">
                <ItemStyle HorizontalAlign="Right" />
            </eclipse:HyperLinkFieldEx>
        </Columns>
        <EmptyDataTemplate>
            <b>No Data found for the given date range.</b>
        </EmptyDataTemplate>
    </jquery:GridViewEx>
</asp:Content>
