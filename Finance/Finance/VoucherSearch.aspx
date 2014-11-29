<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" CodeBehind="VoucherSearch.aspx.cs"
    Inherits="Finance.Finance.VoucherSearch" Title="Search Voucher" EnableViewState="false" %>

<%@ Register Src="../Controls/PrinterFriendlyButton.ascx" TagName="PrinterFriendlyButton"
    TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphSideNavigation" runat="server">
    <asp:HyperLink ID="HyperLink2" runat="server" Text="Help" NavigateUrl="~/Doc/VoucherSearch.doc.aspx" />
    <uc2:PrinterFriendlyButton ID="PrinterFriendlyButton1" runat="server" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cph" runat="server">
    <jquery:JPanel runat="server" IsValidationContainer="true" ID="panelFilters">
        <jquery:Tabs runat="server" Selected="0" Collapsible="true">
            <jquery:JPanel runat="server" HeaderText="Filters">
                <eclipse:TwoColumnPanel ID="TwoColumnPanel1" runat="server" WidthLeft="21%" WidthRight="79%">
                    <eclipse:LeftLabel ID="LeftLabel1" runat="server" Text="Particulars" />
                    <i:TextBoxEx ID="tbParticulars" runat="server" MaxLength="40" />
                    <eclipse:LeftLabel ID="LeftLabel2" runat="server" Text="Payee Name" />
                    <i:TextBoxEx ID="tbPayeeName" runat="server" MaxLength="40" />
                    <eclipse:LeftLabel ID="LeftLabel3" runat="server" Text="Voucher No." />
                    <i:TextBoxEx ID="tbVoucherCode" runat="server" MaxLength="15" />
                    <eclipse:LeftLabel ID="LeftLabel4" runat="server" Text="Voucher Date From /To" />
                    <i:TextBoxEx ID="tbFromDate" runat="server" QueryString="DateFrom">
                        <Validators>
                            <i:Date Max="0" />
                        </Validators>
                    </i:TextBoxEx>
                    <i:TextBoxEx ID="tbToDate" runat="server" QueryString="DateTo">
                        <Validators>
                            <i:Date DateType="ToDate" Max="0" />
                        </Validators>
                    </i:TextBoxEx>
                    <eclipse:LeftLabel ID="LeftLabel5" runat="server" Text="Specific Employee" />
                    <i:AutoComplete ID="tbEmployee" runat="server" ClientIDMode="Static" Width="25em"
                        QueryString="EmployeeId" WebMethod="GetEmployees" WebServicePath="~/Services/Employees.asmx"
                        ValidateWebMethodName="ValidateEmployee" AutoValidate="true">
                    </i:AutoComplete>
                    <eclipse:LeftLabel ID="LeftLabel6" runat="server" Text="Specific Contractor" />
                    <i:AutoComplete ID="tbContractor" runat="server" ClientIDMode="Static" WebMethod="GetContractors"
                        ValidateWebMethodName="ValidateContractor" WebServicePath="~/Services/Contractors.asmx"
                        Width="25em" QueryString="ContractorId" AutoValidate="true">
                    </i:AutoComplete>
                    <br />
                    <i:CheckBoxEx ID="chkJobSpecified" runat="server" Text="Job has been specified" />
                    <eclipse:LeftLabel ID="LeftLabel7" runat="server" Text="Specific Head" />
                    <i:AutoComplete ID="tbHeadOfAccount" runat="server" FriendlyName="HeadOfAccount"
                        AutoValidate="true" QueryString="HeadOfAccountId" WebMethod="GetHeadOfAccount"
                        WebServicePath="~/Services/HeadOfAccounts.asmx" ValidateWebMethodName="ValidateHeadOfAccount"
                        Width="25em">
                    </i:AutoComplete>
                    <eclipse:LeftLabel ID="LeftLabel8" runat="server" Text="Created On" />
                    <i:TextBoxEx ID="tbDateCreated" runat="server" QueryString="Created">
                        <Validators>
                            <i:Date Max="0" />
                        </Validators>
                    </i:TextBoxEx>
                </eclipse:TwoColumnPanel>
            </jquery:JPanel>
            <jquery:JPanel runat="server" HeaderText="Head Types">
                <phpa:PhpaLinqDataSource ID="dsAccountTypes" runat="server" ContextTypeName="Eclipse.PhpaLibrary.Reporting.ReportingDataContext"
                    OrderBy="Category,Description" Select="new (HeadOfAccountType, Description, Category)"
                    TableName="RoAccountTypes" RenderLogVisible="false">
                </phpa:PhpaLinqDataSource>
                <i:CheckBoxListEx ID="cblAccountTypes" runat="server" DataSourceID="dsAccountTypes"
                    FriendlyName="Account Types" WidthItem="15em" DataTextField="Description" DataValueField="HeadOfAccountType"
                    QueryString="AccountTypes" />
            </jquery:JPanel>
        </jquery:Tabs>
        <i:ButtonEx runat="server" Text="Search" Action="Submit" AccessKey="S" CausesValidation="true"
            IsDefault="true" />
        <i:ButtonEx ID="btnClear" runat="server" Text="Clear Search" Action="Reset" />
        <i:ValidationSummary runat="server" />
    </jquery:JPanel>
    <b>Amount:</b>
    <asp:Label ID="lbldiffrence" runat="server" Text="Label" ToolTip="Difference in debit side and credit side" />
    <phpa:PhpaLinqDataSource ID="dsVoucherDetails" runat="server" ContextTypeName="Eclipse.PhpaLibrary.Reporting.ReportingDataContext"
        RenderLogVisible="false" OnSelecting="dsVoucherDetails_Selecting">
    </phpa:PhpaLinqDataSource>
    <eclipse:AppliedFilters runat="server" ContainerId="panelFilters" />
    <jquery:GridViewEx ID="gvSearchVoucher" runat="server" AutoGenerateColumns="False"
        DataSourceID="dsVoucherDetails" AllowPaging="true" PageSize="50" OnRowDataBound="gvSearchVoucher_RowDataBound"
        EnableViewState="false" AllowSorting="True" ShowFooter="true">
        <EmptyDataTemplate>
            <b>Vouchers not found for the given parameters.</b>
        </EmptyDataTemplate>
        <Columns>
            <asp:HyperLinkField DataNavigateUrlFields="VoucherId" DataNavigateUrlFormatString="InsertVoucher.aspx?VoucherId={0}"
                HeaderText="Details" Text="Details" />
            <eclipse:MultiBoundField HeaderText="Date" SortExpression="VoucherDate" DataFields="VoucherDate"
                DataFormatString="{0:d}" />
            <eclipse:MultiBoundField DataFields="VoucherReference" HeaderText="Voucher Reference"
                ToolTipFields="VoucherCode" ToolTipFormatString="Voucher No:{0}" />
            <eclipse:MultiBoundField DataFields="DivisionName" HeaderText="Division" SortExpression="DivisionName"
                ToolTipFields="DivisionCode" ToolTipFormatString="Division Code: {0}" />
            <eclipse:MultiBoundField DataFields="Particulars" HeaderText="Particulars" SortExpression="Particulars" />
            <eclipse:MultiBoundField DataFields="PayeeName" HeaderText="Payee" SortExpression="PayeeName" />
            <eclipse:MultiBoundField DataFields="VoucherType" HeaderText="Voucher|Type" ToolTipFields="VoucherTypeCode"
                ToolTipFormatString="Internal Voucher Type: {0}" />
            <eclipse:MultiBoundField DataFields="VoucherCode" HeaderText="Voucher|No" SortExpression="VoucherCode" />
            <eclipse:MultiBoundField DataFields="CountDetails" HeaderText="Voucher|# Details"
                SortExpression="CountDetails" FooterText="Total">
                <ItemStyle HorizontalAlign="Right" />
            </eclipse:MultiBoundField>
            <eclipse:MultiBoundField DataFields="DebitAmount" HeaderText="Debit" SortExpression="DebitAmount"
                AccessibleHeaderText="Debit" DataFormatString="{0:N2}" DataSummaryCalculation="ValueSummation">
                <FooterStyle HorizontalAlign="Right"></FooterStyle>
                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                <ItemStyle HorizontalAlign="Right"></ItemStyle>
            </eclipse:MultiBoundField>
            <eclipse:MultiBoundField DataFields="CreditAmount" HeaderText="Credit" SortExpression="CreditAmount"
                AccessibleHeaderText="Credit" DataFormatString="{0:N2}" DataSummaryCalculation="ValueSummation">
                <FooterStyle HorizontalAlign="Right"></FooterStyle>
                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                <ItemStyle HorizontalAlign="Right"></ItemStyle>
            </eclipse:MultiBoundField>
        </Columns>
    </jquery:GridViewEx>
</asp:Content>
