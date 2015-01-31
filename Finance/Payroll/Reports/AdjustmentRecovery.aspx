<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" CodeBehind="AdjustmentRecovery.aspx.cs"
    Inherits="PhpaAll.Payroll.Reports.AdjustmentRecovery" Title="Loan Recovery" EnableViewState="false" %>

<%@ Register Src="~/Controls/PrinterFriendlyButton.ascx" TagName="PrinterFriendlyButton"
    TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphSideNavigation" runat="server">
    <uc2:PrinterFriendlyButton ID="PrinterFriendlyButton1" runat="server" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cph" runat="server">
    <div class="ParamInstructions">
        <eclipse:TwoColumnPanel runat="server" IsValidationContainer="true">
            <eclipse:LeftLabel runat="server" Text="Date From / To" />
            <i:TextBoxEx ID="tbFromDate" runat="server" FriendlyName="From Date">
                <Validators>
                    <i:Required />
                    <i:Date />
                </Validators>
            </i:TextBoxEx>
            <i:TextBoxEx ID="tbToDate" runat="server" FriendlyName="To Date">
                <Validators>
                    <i:Required />
                    <i:Date DateType="ToDate" />
                </Validators>
            </i:TextBoxEx>
            <eclipse:LeftLabel runat="server" Text="Head of Account" />
            <i:AutoComplete ID="tbHeadOfAccount" runat="server" FriendlyName="HeadOfAccount"
                AutoValidate="true" ValidateWebMethodName="ValidateHeadOfAccount" QueryString="HeadOfAccountId"
                Width="20em" WebMethod="GetHeadOfAccount" WebServicePath="~/Services/HeadOfAccounts.asmx">
            </i:AutoComplete>
            <eclipse:LeftLabel runat="server" Text="Bank Name" />
            <phpa:PhpaLinqDataSource ID="dsBankName" runat="server" ContextTypeName="Eclipse.PhpaLibrary.Database.Payroll.PayrollDataContext"
                RenderLogVisible="false" Select="new (BankId,BankName)" TableName="Banks">
            </phpa:PhpaLinqDataSource>
            <i:DropDownListEx ID="ddlBankName" runat="server" EnableViewState="false" DataSourceID="dsBankName"
                DataTextField="BankName" DataValueField="BankId">
                <Items>
                    <eclipse:DropDownItem Text="(All)" Persistent="Always" />
                </Items>
            </i:DropDownListEx>
            <eclipse:LeftLabel runat="server" Text="A/c No. starts with" />
            <i:TextBoxEx runat="server" ID="tbAccountNo" CaseConversion="UpperCase" />
            <br />
            <i:ButtonEx ID="btnGo" runat="server" Text="Go" CausesValidation="true" Action="Submit"
                Icon="Refresh" />
            <i:ValidationSummary runat="server" />
        </eclipse:TwoColumnPanel>
    </div>
    <br />
    <phpa:PhpaLinqDataSource ID="dsBLRecovery" runat="server" ContextTypeName="Eclipse.PhpaLibrary.Database.Payroll.PayrollDataContext"
        OnSelecting="dsBLRecovery_Selecting" TableName="PeriodEmployeeAdjustments" RenderLogVisible="false">
    </phpa:PhpaLinqDataSource>
    <jquery:GridViewEx ID="gvBLRecovery" runat="server" AutoGenerateColumns="False" DataSourceID="dsBLRecovery"
        ShowFooter="true" OnRowDataBound="gvBLRecovery_RowDataBound">
        <Columns>
            <eclipse:SequenceField>
            </eclipse:SequenceField>
            <eclipse:MultiBoundField DataFields="EmployeeCode" SortExpression="EmployeeCode"
                HeaderText="Employee| Code" ItemStyle-Wrap="true" ItemStyle-HorizontalAlign="Center" />
            <eclipse:MultiBoundField DataFields="EmployeeName" SortExpression="FirstName" HeaderText="Employee| Name"
                ItemStyle-Wrap="true" />
            <eclipse:MultiBoundField DataFields="Designation" SortExpression="Designation" HeaderText="Employee|Designation">
                <ItemStyle Wrap="true" />
            </eclipse:MultiBoundField>
            <eclipse:MultiBoundField DataFields="Amount" DataFormatString="{0:N0}" HeaderText="Amount"
                DataSummaryCalculation="ValueSummation">
                <ItemStyle HorizontalAlign="Right" />
                <HeaderStyle HorizontalAlign="Center" />
                <FooterStyle HorizontalAlign="Right" />
            </eclipse:MultiBoundField>
            <eclipse:MultiBoundField DataFields="AccountNumber" HeaderText="Account No." />
             <eclipse:MultiBoundField DataFields="SalaryPeriodStartDate" HeaderText="For"
                DataFormatString="{0:MMM yyyy}"  Visible="false" ItemStyle-Wrap="false" AccessibleHeaderText="SalaryPeriodDate">
            </eclipse:MultiBoundField>
            <eclipse:MultiBoundField HeaderText="Remarks" DataFormatString="">
                <ItemStyle Width="2in" />
            </eclipse:MultiBoundField>
        </Columns>
        <EmptyDataTemplate>
            <strong>No Loan amount deduced during given period.</strong>
        </EmptyDataTemplate>
    </jquery:GridViewEx>
</asp:Content>

