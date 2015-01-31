<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" CodeBehind="GPF.aspx.cs"
    Inherits="PhpaAll.Payroll.Reports.GPF" Title="National Pension and Provident Fund (NPPF) - Thimphu, Bhutan" %>

<%@ Register Src="~/Controls/PrinterFriendlyButton.ascx" TagName="PrinterFriendlyButton"
    TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphSideNavigation" runat="server">
    <asp:HyperLink ID="HyperLink2" runat="server" Text="Help" NavigateUrl="~/Doc/GPF.doc.aspx" />
    <uc2:PrinterFriendlyButton runat="server" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cph" runat="server">
    <eclipse:TwoColumnPanel ID="TwoColumnPanel1" runat="server">
        <eclipse:LeftLabel runat="server" Text="From Date/To Date" />
        <i:TextBoxEx ID="tbFromDate" runat="server" FriendlyName="From Date">
            <Validators>
                <i:Required />
                <i:Date Min="-183" Max="0" />
            </Validators>
        </i:TextBoxEx>
        <i:TextBoxEx ID="tbToDate" runat="server" FriendlyName="To Date">
            <Validators>
                <i:Required />
                <i:Date />
            </Validators>
        </i:TextBoxEx>
        <eclipse:LeftLabel runat="server" Text="NPPF Type" />
        <i:RadioButtonListEx ID="rblNPPFType" runat="server" Orientation="Horizontal" Value="true">
            <Items>
                <i:RadioItem Text="Tier-2" Value="Tier-2" />
                <i:RadioItem Text="Both" Value="Both" />
            </Items>
            <Validators>
                <i:Value ValueType="String" />
            </Validators>
        </i:RadioButtonListEx>
        <br />
        <eclipse:LeftLabel runat="server" Text="Bank Name" />
        <phpa:PhpaLinqDataSource ID="dsBankName" runat="server" ContextTypeName="Eclipse.PhpaLibrary.Database.Payroll.PayrollDataContext"
            RenderLogVisible="false" TableName="Banks" Select="new (BankId,BankName)"/>
        <i:DropDownListEx ID="ddlBankName" runat="server" EnableViewState="false" DataSourceID="dsBankName"
         DataTextField="BankName" DataValueField="BankId">
            <Items>
                <eclipse:DropDownItem Text="(All)" Persistent="Always" />
            </Items>
        </i:DropDownListEx>
        <br />

        <i:ButtonEx ID="btnShowReport" runat="server" Text="Go" OnClick="btnShowReport_Click"
            CausesValidation="true" Action="Submit" />
        <i:ValidationSummary ID="ValidationSummary1" runat="server" />
    </eclipse:TwoColumnPanel>
    <br />
    <br />
    <phpa:PhpaLinqDataSource ID="dsGPF" runat="server" ContextTypeName="Eclipse.PhpaLibrary.Database.Payroll.PayrollDataContext"
        OnSelecting="dsGPF_Selecting" RenderLogVisible="False" TableName="Employee">
    </phpa:PhpaLinqDataSource>
    <jquery:GridViewEx ID="gvGPF" runat="server" DataSourceID="dsGPF" ShowFooter="true"
        AutoGenerateColumns="false" EnableViewState="false" OnRowDataBound="gvGPF_RowDataBound">
        <Columns>
            <eclipse:SequenceField FooterText="">
            </eclipse:SequenceField>
            <eclipse:MultiBoundField HeaderText="NPPFP No." DataFields="NPPFPNo" />
            <eclipse:MultiBoundField HeaderText="Members Name" DataFields="MembersName" />
            <eclipse:MultiBoundField HeaderText="Designation" DataFields="Designation" />
            <eclipse:MultiBoundField HeaderText="Citizen ID" DataFields="CitizenId" FooterText="Total" />
            <eclipse:MultiBoundField HeaderText="Employee ID No." DataFields="EmployeeIDNo" />
            <eclipse:MultiBoundField HeaderText="Bank Account No" DataFields="BankAccountNo" />
            <eclipse:MultiBoundField HeaderText="Basic Pay" DataFields="BasicPay" DataFormatString="{0:N0}"
                DataSummaryCalculation="ValueSummation">
                <ItemStyle HorizontalAlign="Right" />
                <HeaderStyle HorizontalAlign="Center" />
                <FooterStyle HorizontalAlign="Right" />
            </eclipse:MultiBoundField>
            <eclipse:MultiBoundField DataFields="EmployeesContb" DataFormatString="{0:N0}" HeaderText="Contribution|Employee"
                DataSummaryCalculation="ValueSummation">
                <ItemStyle HorizontalAlign="Right" />
                <HeaderStyle HorizontalAlign="Center" />
                <FooterStyle HorizontalAlign="Right" />
            </eclipse:MultiBoundField>
            <eclipse:MultiBoundField DataFields="EmployersContb" DataFormatString="{0:N0}" HeaderText="Contribution|Employer"
                DataSummaryCalculation="ValueSummation">
                <ItemStyle HorizontalAlign="Right" />
                <HeaderStyle HorizontalAlign="Center" />
                <FooterStyle HorizontalAlign="Right" />
            </eclipse:MultiBoundField>
            <eclipse:MultiBoundField HeaderText="Total Nu" DataFields="TotalNu" DataFormatString="{0:N0}"
                DataSummaryCalculation="ValueSummation">
                <ItemStyle HorizontalAlign="Right" />
                <HeaderStyle HorizontalAlign="Center" />
                <FooterStyle HorizontalAlign="Right" />
            </eclipse:MultiBoundField>
            <eclipse:MultiBoundField HeaderText="Remarks" DataFormatString="">
                <ItemStyle Width="2in" />
            </eclipse:MultiBoundField>
        </Columns>
        <EmptyDataTemplate>
            <b>No Recovery of NPPF found for given date range.</b>
        </EmptyDataTemplate>
    </jquery:GridViewEx>
</asp:Content>
