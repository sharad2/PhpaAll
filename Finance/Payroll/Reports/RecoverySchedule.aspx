<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" CodeBehind="RecoverySchedule.aspx.cs"
    Inherits="PhpaAll.Payroll.Reports.RecoverySchedule" Title="Recovery Schedule" %>

<%@ Register Src="~/Controls/PrinterFriendlyButton.ascx" TagName="PrinterFriendlyButton"
    TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphSideNavigation" runat="server">
    <uc2:PrinterFriendlyButton ID="PrinterFriendlyButton1" runat="server" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cph" runat="server">
    <phpa:PhpaLinqDataSource ID="dsREcoverySchedule" runat="server" ContextTypeName="Eclipse.PhpaLibrary.Database.Payroll.PayrollDataContext"
        RenderLogVisible="false" OnSelecting="dsREcoverySchedule_Selecting" TableName="PeriodEmployeeAdjustment">
    </phpa:PhpaLinqDataSource>
    <eclipse:TwoColumnPanel runat="server" IsValidationContainer="true">
        <eclipse:LeftLabel runat="server" Text="From Date/To Date" />
        <i:TextBoxEx ID="tbFromDate" runat="server" FriendlyName="From Date">
            <Validators>
                <i:Date />
                <i:Required />
            </Validators>
        </i:TextBoxEx>
        <i:TextBoxEx ID="tbToDate" runat="server" FriendlyName="To Date">
            <Validators>
                <i:Date DateType="ToDate" />
                <i:Required />
            </Validators>
        </i:TextBoxEx>
        <%--<p>
            You can select the Range Maximum of 31 days. Select From Date first then Select
            To Date. Default dates are previous month Start Date and End Date. In order to select
            different Date Range first clear the To Date then select From Date.
        </p>--%>
        <phpa:PhpaLinqDataSource ID="dsEmpTypes" runat="server" ContextTypeName="Eclipse.PhpaLibrary.Database.Payroll.PayrollDataContext"
            OrderBy="EmployeeTypeCode" TableName="EmployeeTypes" RenderLogVisible="false" />
        <eclipse:LeftLabel ID="LeftLabel2" runat="server" Text="Employee Type" />
        <i:DropDownListEx ID="ddlEmpType" runat="server" EnableViewState="false" DataSourceID="dsEmpTypes"
            DataTextField="Description" DataValueField="EmployeeTypeId">
            <Items>
                <eclipse:DropDownItem Text="(All types)" Value="" Persistent="Always" />
            </Items>
        </i:DropDownListEx>
        <eclipse:LeftLabel runat="server" Text="Recovery" />
        <i:AutoComplete ID="tbRecovery" runat="server" ClientIDMode="Static" Width="25em"
            ValidateWebMethodName="ValidateRecoveries" Value='<%# Bind("AdjustmentId") %>'
            WebMethod="GetRecoveries" WebServicePath="~/Services/Adjustments.asmx" Text='<%# Eval("Adjustment.ShortDescription") %>'
            Delay="2000">
            <Validators>
                <i:Required />
            </Validators>
        </i:AutoComplete>
         <eclipse:LeftLabel ID="LeftLabel3" runat="server" Text="Bank Name" />
            <phpa:PhpaLinqDataSource ID="dsBankName" runat="server" ContextTypeName="Eclipse.PhpaLibrary.Database.Payroll.PayrollDataContext"
                TableName="Banks" Select="new(BankName,BankId)" RenderLogVisible="false">
            </phpa:PhpaLinqDataSource>
            <i:DropDownListEx ID="ddlBankName" runat="server" EnableViewState="false" DataSourceID="dsBankName"
                 DataTextField="BankName" DataValueField="BankId">
                <Items>
                    <eclipse:DropDownItem Text="(All)" Persistent="Always" />
                </Items>
            </i:DropDownListEx>
        <eclipse:LeftLabel ID="LeftLabel1" runat="server" Text="Department contains" />
        <i:TextBoxEx runat="server" ID="tbDepartment" />
        <eclipse:LeftLabel runat="server" Text="Display Account No." />
        <i:DropDownListEx runat="server" ID="ddlAccountName">
            <Items>
                <eclipse:DropDownItem Text="(None)" Value="" />
                <eclipse:DropDownItem Text="CGEGIS" Value="CGEGIS" />
                <eclipse:DropDownItem Text="Bank Loan" Value="BL" />
                <eclipse:DropDownItem Text="BDFC" Value="BDFC" />
                <eclipse:DropDownItem Text="GPFSub" Value="GPFSub" />
            </Items>
        </i:DropDownListEx>
        <br />
        <i:ValidationSummary ID="ValidationSummary1" runat="server" />
        <br />
        <i:ButtonEx ID="btnGo" runat="server" Text="Go" CausesValidation="true" Action="Submit"
            Icon="Search" />
        <i:ButtonEx ID="btnClearSearch" runat="server" Text="Clear Filters" Action="Reset"
            Icon="Refresh" />
    </eclipse:TwoColumnPanel>
    <jquery:GridViewEx ID="gvRecoveries" runat="server" AutoGenerateColumns="false" DataSourceID="dsREcoverySchedule"
        ShowFooter="true" EnableViewState="false" OnDataBinding="gvRecoveries_DataBinding"
        OnDataBound="gvRecoveries_DataBound">
        <Columns>
            <eclipse:SequenceField />
            <eclipse:MultiBoundField DataFields="EmpName" HeaderText="Employee" HeaderStyle-Width="2in" />
            <eclipse:MultiBoundField DataFields="Department" HeaderText="Dept" FooterText="Total"
                HeaderStyle-Width="2in" AccessibleHeaderText="Department" />
            <eclipse:MultiBoundField DataFields="Amount" DataFormatString="{0:N0}" AccessibleHeaderText="Amount"
                DataSummaryCalculation="ValueSummation" HeaderStyle-Width="1in">
                <ItemStyle HorizontalAlign="Right" />
                <FooterStyle HorizontalAlign="Right" />
            </eclipse:MultiBoundField>
            <eclipse:MultiBoundField DataFields="GISAccNo" AccessibleHeaderText="CGEGIS" Visible="false"
                HeaderText="CGEGIS Acc No." HeaderStyle-Width="2in" />
            <eclipse:MultiBoundField DataFields="GPFAccNo" AccessibleHeaderText="GPFSub" Visible="false"
                HeaderText="GPFSub Acc No." HeaderStyle-Width="2in" />
            <eclipse:MultiBoundField DataFields="BDFCAccNo" AccessibleHeaderText="BDFC" Visible="false"
                HeaderText="BDFC Acc No." HeaderStyle-Width="2in" />
            <eclipse:MultiBoundField DataFields="BLAccNo" AccessibleHeaderText="BL" Visible="false"
                HeaderText="Bank Loan Acc No." HeaderStyle-Width="2in" />
            <eclipse:MultiBoundField DataFields="SalaryPeriodStartDate" HeaderText="For"
                DataFormatString="{0:MMM yyyy}"  Visible="false" ItemStyle-Wrap="false" AccessibleHeaderText="SalaryPeriodDate">
            </eclipse:MultiBoundField>
            <asp:BoundField HeaderText="Remarks" HeaderStyle-Width="2in" DataField="PolicyNumber"/>
        </Columns>
        <EmptyDataTemplate>
            <b>No Recoveries found for given period.</b>
        </EmptyDataTemplate>
    </jquery:GridViewEx>
</asp:Content>
