<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" CodeBehind="STHC.aspx.cs"
    Inherits="Finance.Payroll.Reports.STHC" Title="Remittances Of PIT & HC" EnableViewState="false" %>

<%@ Register Src="~/Controls/PrinterFriendlyButton.ascx" TagName="PrinterFriendlyButton"
    TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphSideNavigation" runat="server">
    <asp:HyperLink ID="HyperLink2" runat="server" Text="Help" NavigateUrl="~/Doc/STHC.doc.aspx" />
    <uc2:PrinterFriendlyButton ID="PrinterFriendlyButton1" runat="server" EnableTheming="true" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cph" runat="server">
    <eclipse:TwoColumnPanel runat="server" IsValidationContainer="true">
        <eclipse:LeftLabel runat="server" Text="From Date / To Date" />
        <i:TextBoxEx ID="tbFromDate" runat="server" FriendlyName="From Date">
            <Validators>
                <i:Required />
                <i:Date />
            </Validators>
        </i:TextBoxEx>
        <i:TextBoxEx ID="tbToDate" runat="server" FriendlyName="To Date">
            <Validators>
                <i:Date DateType="ToDate" />
                <i:Required />
            </Validators>
        </i:TextBoxEx>
        <br />
        <eclipse:LeftLabel runat="server" Text="Bank Name" />
        <phpa:PhpaLinqDataSource ID="dsBankName" runat="server" ContextTypeName="Eclipse.PhpaLibrary.Database.Payroll.PayrollDataContext"
            RenderLogVisible="false" TableName="Banks" Select="new (BankName,BankId)" />
        <i:DropDownListEx ID="ddlBankName" runat="server" EnableViewState="false" DataSourceID="dsBankName"
             DataTextField="BankName" DataValueField="BankId" >
            <Items>
                <eclipse:DropDownItem Text="(All)" Persistent="Always" />
            </Items>
        </i:DropDownListEx>
        <br />
        <phpa:PhpaLinqDataSource runat="server" ID="dsEmployeeTypes" TableName="EmployeeTypes"
            ContextTypeName="Eclipse.PhpaLibrary.Database.Payroll.PayrollDataContext" OrderBy="EmployeeTypeCode"
            RenderLogVisible="false" />
        <i:CheckBoxListEx runat="server" ID="cblEmployeeTypes" DataSourceID="dsEmployeeTypes"
            WidthItem="10em" DataTextField="Description" DataValueField="EmployeeTypeId"
            FriendlyName="Employee Types">
            <Validators>
                <i:Required />
            </Validators>
        </i:CheckBoxListEx>
        <br />
        <i:ButtonEx ID="btnGo" runat="server" Text="Go" CausesValidation="true" Action="Submit"
            Icon="Refresh" />
        <i:ValidationSummary runat="server" />
    </eclipse:TwoColumnPanel>
    <phpa:PhpaLinqDataSource ID="ds" runat="server" ContextTypeName="Eclipse.PhpaLibrary.Database.Payroll.PayrollDataContext"
        RenderLogVisible="false" OnSelecting="ds_Selecting">
    </phpa:PhpaLinqDataSource>
    <jquery:GridViewEx ID="gv" runat="server" DataSourceID="ds" AutoGenerateColumns="false"
        OnDataBound="gv_DataBound" PreSorted="true" ShowExpandCollapseButtons="false"
        OnRowDataBound="gv_RowDataBound" ShowFooter="true" AllowPaging="true" PageSize="200">
        <Columns>
            <eclipse:SequenceField />
            <eclipse:MultiBoundField DataFields="EmployeeCode" HeaderText="Employee|Code" />
            <eclipse:MultiBoundField DataFields="FullName" HeaderText="Employee|Name" />
            <eclipse:MultiBoundField DataFields="Designation" HeaderText="Employee|Desig." />
            <eclipse:MultiBoundField DataFields="CitizenCardNo" HeaderText="Employee|Citizen Card No." />
            <eclipse:MultiBoundField DataFields="TpnNo" HeaderText="Employee|Tpn No." />
            <eclipse:MultiBoundField DataFields="ServiceStatus" HeaderText="Service Status" SortExpression="ServiceStatus" />
            <eclipse:MultiBoundField DataFields="BasicSalary" HeaderText="Basic Salary" DataFormatString="{0:C0}"
                DataSummaryCalculation="ValueSummation">
                <ItemStyle HorizontalAlign="Right" />
                <FooterStyle HorizontalAlign="Right" />
            </eclipse:MultiBoundField>
            <eclipse:MultiBoundField DataFields="TotalAllowance" HeaderText="Total Benifits/Allow."
                DataFormatString="{0:C0}">
                <ItemStyle HorizontalAlign="Right" />
                <FooterStyle HorizontalAlign="Right" />
            </eclipse:MultiBoundField>
            <eclipse:MultiBoundField HeaderText="Taxable Amt." DataFormatString="{0:C0}" AccessibleHeaderText="TableAmount">
                <ItemStyle HorizontalAlign="Right" />
                <FooterStyle HorizontalAlign="Right" />
            </eclipse:MultiBoundField>
            <jquery:MatrixField DataHeaderFields="CategoryId" DataValueFields="DeductionAmount"
                HeaderText="Remittances" DataHeaderCustomFields="DeductionCategoryCode" DisplayRowTotals="true"
                DisplayColumnTotals="true" DataValueFormatString="{0:C0}" DataMergeFields="EmployeeId"
                DataHeaderFormatString="{1}">
                <ItemStyle HorizontalAlign="Right" />
                <FooterStyle HorizontalAlign="Right" />
            </jquery:MatrixField>
            <eclipse:MultiBoundField HeaderText="Remarks" DataFormatString="">
                <ItemStyle Width="2in" />
            </eclipse:MultiBoundField>
        </Columns>
    </jquery:GridViewEx>
</asp:Content>
