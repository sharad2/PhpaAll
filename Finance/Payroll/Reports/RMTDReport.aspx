<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" CodeBehind="RMTDReport.aspx.cs"
    EnableViewState="false" Inherits="PhpaAll.Payroll.Reports.RMTDReport" Title="Remittances Recovered from the Salary of Deputationists" %>

<%@ Register Src="~/Controls/PrinterFriendlyButton.ascx" TagName="PrinterFriendlyButton"
    TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphSideNavigation" runat="server">
    <asp:HyperLink ID="HyperLink2" runat="server" Text="Help" NavigateUrl="~/Doc/RMTDReport.doc.aspx" />
    <uc2:PrinterFriendlyButton ID="PrinterFriendlyButton1" runat="server" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cph" runat="server">
    <eclipse:TwoColumnPanel runat="server">
        <eclipse:LeftLabel runat="server" Text="Date From/Date To" />
        <i:TextBoxEx ID="tbFromDate" runat="server" FriendlyName="From Date">
            <Validators>
                <i:Required />
                <i:Date Max="0" />
            </Validators>
        </i:TextBoxEx>
        <i:TextBoxEx ID="tbToDate" runat="server" FriendlyName="To Date">
            <Validators>
                <i:Required />
                <i:Date DateType="ToDate" />
            </Validators>
        </i:TextBoxEx>
        <phpa:PhpaLinqDataSource ID="dsEmpTypes" runat="server" ContextTypeName="Eclipse.PhpaLibrary.Database.Payroll.PayrollDataContext"
            OrderBy="EmployeeTypeCode" TableName="EmployeeTypes" RenderLogVisible="false" />
       <%-- <eclipse:LeftLabel runat="server" Text="Employee Type" />
        <i:DropDownListEx runat="server" ID="ddlEmpType" DataSourceID="dsEmpTypes" DataTextField="Description"
            DataValueField="EmployeeTypeId">
            <Items>
                <eclipse:DropDownItem Text="(All types)" Persistent="Always" />
            </Items>
        </i:DropDownListEx>
        <br />--%>
        <br />
        <eclipse:LeftLabel runat="server" Text="Bank Name" />
        <phpa:PhpaLinqDataSource ID="dsBankName" runat="server" ContextTypeName="Eclipse.PhpaLibrary.Database.Payroll.PayrollDataContext"
             TableName="Banks" Select="new(BankName,BankId)" RenderLogVisible="false" />
        <i:DropDownListEx ID="ddlBankName" runat="server" EnableViewState="false" DataSourceID="dsBankName"
             DataTextField="BankName" DataValueField="BankId">
            <Items>
                <eclipse:DropDownItem Text="(All)" Persistent="Always" />
            </Items>
        </i:DropDownListEx>
        <phpa:PhpaLinqDataSource ID="dsParentOffice" runat="server" ContextTypeName="Eclipse.PhpaLibrary.Database.Payroll.PayrollDataContext"
            OnSelecting="dsParentOffice_Selecting" RenderLogVisible="false" />
        <eclipse:LeftLabel ID="LeftLabel1" runat="server" Text="Parent Office" />
        <i:DropDownListEx runat="server" ID="ddlParentOffice" DataSourceID="dsParentOffice"
            Value='<%# Eval("ParentOrganization")%>'>
            <Items>
                <eclipse:DropDownItem Text="(All types)" Persistent="Always" />
            </Items>
        </i:DropDownListEx>
        <br />
        <i:ButtonEx ID="btnSearch" runat="server" Text="Show Report" Action="Submit" Icon="Search"
            CausesValidation="true" IsDefault="true" />
        <i:ValidationSummary runat="server" />
    </eclipse:TwoColumnPanel>
    <phpa:PhpaLinqDataSource ID="dsDeputationist" runat="server" ContextTypeName="Eclipse.PhpaLibrary.Database.Payroll.PayrollDataContext"
        RenderLogVisible="false" OnSelecting="dsDeputationist_Selecting" />
    <jquery:GridViewEx runat="server" DataSourceID="dsDeputationist" AutoGenerateColumns="false"
        ShowFooter="true">
        <Columns>
            <eclipse:SequenceField />
            <eclipse:MultiBoundField DataFields="SalaryPeriodStart,SalaryPeriodEnd" HeaderText="Salary Date"
                DataFormatString="{0:d} to {1:d}">
                <ItemStyle Wrap="false" />
            </eclipse:MultiBoundField>
            <eclipse:MultiBoundField DataFields="ParentOrg" HeaderText="In Favour Of">
                <ItemStyle Wrap="false" />
            </eclipse:MultiBoundField>
            <eclipse:MultiBoundField DataFields="EmployeeName" HeaderText="Employee Name">
                <ItemStyle Wrap="false" />
            </eclipse:MultiBoundField>
            <jquery:MatrixField DataMergeFields="ParentOrg,EmployeeID" DataValueFields="Amount"
                DataHeaderFields="AdjCatDescription" DisplayRowTotals="true" DataValueFormatString="{0:N2}"
                DisplayColumnTotals="true" DataHeaderSortFields="AdjCatDescription" />
        </Columns>
    </jquery:GridViewEx>
</asp:Content>
