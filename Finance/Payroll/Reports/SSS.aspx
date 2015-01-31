<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" CodeBehind="SSS.aspx.cs"
    Inherits="PhpaAll.Payroll.Reports.SSS" Title="Schedule For Recovery Of Life Insurance Premium(SSS)"
    EnableViewState="false" %>

<%@ Register Src="~/Controls/PrinterFriendlyButton.ascx" TagName="PrinterFriendlyButton"
    TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphSideNavigation" runat="server">
    <asp:HyperLink ID="HyperLink2" runat="server" Text="Help" NavigateUrl="~/Doc/SSS.doc.aspx" />
    <uc2:PrinterFriendlyButton ID="PrinterFriendlyButton1" runat="server" EnableTheming="true" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cph" runat="server">
    <eclipse:TwoColumnPanel runat="server" IsValidationContainer="true">
        <eclipse:LeftLabel runat="server" Text="From Date / To Date" />
        <i:TextBoxEx ID="tbFromDate" runat="server" FriendlyName="From Date">
            <Validators>
                <i:Required />
                <i:Date Max="0" />
            </Validators>
        </i:TextBoxEx>
        <i:TextBoxEx ID="tbToDate" runat="server" FriendlyName="To Date">
            <Validators>
                <i:Date />
                <i:Required />
            </Validators>
        </i:TextBoxEx>
        <br />
        <eclipse:LeftLabel ID="LeftLabel1" runat="server" Text="Bank Name" />
        <phpa:PhpaLinqDataSource ID="dsBankName" runat="server" ContextTypeName="Eclipse.PhpaLibrary.Database.Payroll.PayrollDataContext"
            RenderLogVisible="false" TableName="Banks" Select="new (BankName,BankId)" />
        <i:DropDownListEx ID="ddlBankName" runat="server" EnableViewState="false" DataSourceID="dsBankName"
             DataTextField="BankName" DataValueField="BankId">
            <Items>
                <eclipse:DropDownItem Text="(All)" Persistent="Always" />
            </Items>
        </i:DropDownListEx>
        <br />
        <i:ButtonEx ID="btnGo" runat="server" Text="Go" Action="Submit" CausesValidation="true"
            Icon="Refresh" />
        <i:ValidationSummary runat="server" />
    </eclipse:TwoColumnPanel>
    <br />
    <br />
    <phpa:PhpaLinqDataSource ID="dsReportCat" runat="server" ContextTypeName="Eclipse.PhpaLibrary.Database.Payroll.PayrollDataContext"
        TableName="ReportCategories" RenderLogVisible="false">
    </phpa:PhpaLinqDataSource>
    <phpa:PhpaLinqDataSource ID="dsSSS" runat="server" ContextTypeName="Eclipse.PhpaLibrary.Database.Payroll.PayrollDataContext"
        OnSelecting="dsSSS_Selecting" RenderLogVisible="false" EnableViewState="false"
        TableName="Employee">
    </phpa:PhpaLinqDataSource>
    <asp:Label ID="lblRemittedTo" runat="server" Visible="false" /><br />
    <br />
    <jquery:GridViewEx ID="gvSSS" runat="server" AutoGenerateColumns="false" EnableViewState="false"
        DataSourceID="dsSSS" OnRowDataBound="gvSSS_RowDataBound" ShowFooter="true" AllowSorting="true">
        <EmptyDataTemplate>
            <b>No Recovery of Life Insurance Premium(SSS) found for the given Date Range.</b>
        </EmptyDataTemplate>
        <Columns>
            <eclipse:SequenceField />
            <eclipse:MultiBoundField DataFields="EmployeeCode" HeaderText="Employee|Code" SortExpression="EmployeeCode"
                ItemStyle-HorizontalAlign="Center" ItemStyle-Width="5em" />
                <eclipse:MultiBoundField DataFields="PolicyNo" HeaderText="Policy No" FooterText="Total" />
            <eclipse:MultiBoundField DataFields="EmployeeName" HeaderText="Employee|Name" SortExpression="FirstName"
                ItemStyle-Width="15em" />
            <eclipse:MultiBoundField DataFields="Designation" HeaderText="Employee|Designation"
                SortExpression="Designation" ItemStyle-Width="15em" />
            
            <eclipse:MultiBoundField DataFields="Amount" DataFormatString="{0:N0}" HeaderText="Amount"
                SortExpression="Amount" DataSummaryCalculation="ValueSummation">
                <ItemStyle HorizontalAlign="Right" />
                <FooterStyle HorizontalAlign="Right" />
            </eclipse:MultiBoundField>
            <eclipse:MultiBoundField DataFields="SalaryPeriodStartDate" HeaderText="For"
                DataFormatString="{0:MMM yyyy}"  Visible="false" ItemStyle-Wrap="false" AccessibleHeaderText="SalaryPeriodDate">
            </eclipse:MultiBoundField>
             <eclipse:MultiBoundField HeaderText="Remarks" DataFormatString="">
                <ItemStyle Width="2in" />
            </eclipse:MultiBoundField>
        </Columns>
    </jquery:GridViewEx>
</asp:Content>
