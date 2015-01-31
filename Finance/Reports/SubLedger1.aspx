<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" CodeBehind="SubLedger1.aspx.cs"
    Inherits="PhpaAll.Reports.SubLedger1" Title="Sub Ledger Report" EnableViewState="false" %>

<%@ Register Src="../Controls/PrinterFriendlyButton.ascx" TagName="PrinterFriendlyButton"
    TagPrefix="uc2" %>
<%@ Register Src="../Controls/VoucherDetailControl.ascx" TagName="VoucherDetailControl"
    TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            manageControls('#ddlLedgerAccountHead');
        });
        function manageControls(control) {
            var val = $(control).val();
            if (val == '') {
                $('#tbEmployee').closest('tr').hide();
                $('#tbContractor').closest('tr').hide();
            }
            else {
                if (val == 'EMPLOYEE_ADVANCE' || val == 'TOUR_EXPENSES' || val == 'MR') {
                    $('#tbEmployee').closest('tr').show();
                    $('#tbContractor').closest('tr').hide();
                }
                else {
                    $('#tbContractor').closest('tr').show();
                    $('#tbEmployee').closest('tr').hide();
                }
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphSideNavigation" runat="server">
    <uc2:PrinterFriendlyButton ID="PrinterFriendlyButton1" runat="server" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cph" runat="server">
    <phpa:PhpaLinqDataSource ID="dsVouchers" runat="server" ContextTypeName="Eclipse.PhpaLibrary.Reporting.ReportingDataContext"
        TableName="RoVouchers" RenderLogVisible="false">
    </phpa:PhpaLinqDataSource>
    <eclipse:TwoColumnPanel runat="server" IsValidationContainer="true" ID="tcpFilters">
        <%--ddlLedgerAccountHead_SelectedIndexChanged--%>
        <eclipse:LeftLabel ID="LeftLabel1" runat="server" Text="Head Of Account" />
        <i:DropDownListEx ID="ddlLedgerAccountHead" runat="server" ClientIDMode="Static"
            OnClientChange="function(e) {
                manageControls($(this));
                }">
            <Validators>
                <i:Required />
            </Validators>
            <Items>
                <eclipse:DropDownItem Text="(Select Ledger Account)" />
                <eclipse:DropDownItem Value="EMD" Text="Earnest Money Deposit" />
                <eclipse:DropDownItem Value="SD" Text="Security Deposit" />
                <eclipse:DropDownItem Value="PARTY_ADVANCE" Text="Parties Advances" />
                <eclipse:DropDownItem Value="MATERIAL_ADVANCE" Text="Material Advances" />
                <eclipse:DropDownItem Value="EMPLOYEE_ADVANCE" Text="Employee Advances" />
                <eclipse:DropDownItem Value="TOUR_EXPENSES" Text="Expenses" />
                <eclipse:DropDownItem Value="MR" Text="Medical Reimbursement" />
            </Items>
        </i:DropDownListEx>
        <eclipse:LeftLabel runat="server" Text="From / To Date" />
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
        <eclipse:LeftLabel ID="lblEmployee" runat="server" Text="Employee" />
        <i:AutoComplete ID="tbEmployee" runat="server" ClientIDMode="Static" Width="25em"
            ValidateWebMethodName="ValidateEmployee" WebMethod="GetEmployees" WebServicePath="../Services/Employees.asmx"
            AutoValidate="true">
            <Validators>
                <i:Filter DependsOn="ddlLedgerAccountHead" DependsOnState="AnyValue" DependsOnValue="EMPLOYEE_ADVANCE,TOUR_EXPENSES,MR" />
                <i:Required />
            </Validators>
        </i:AutoComplete>
        <eclipse:LeftLabel ID="lblContractor" runat="server" Text="Contractor" />
        <i:AutoComplete ID="tbContractor" runat="server" ClientIDMode="Static" Width="20em"
            WebMethod="GetContractors" WebServicePath="~/Services/Contractors.asmx">
            <Validators>
                <i:Filter DependsOn="ddlLedgerAccountHead" DependsOnState="AnyValue" DependsOnValue="EMD,SD,PARTY_ADVANCE,MATERIAL_ADVANCE" />
                <i:Required />
            </Validators>
        </i:AutoComplete>
        <eclipse:LeftPanel runat="server" Span="true">
            <i:ButtonEx ID="btnShowReport" runat="server" Text="Go" CausesValidation="true" Action="Submit"
                Icon="Refresh" />
            <i:ValidationSummary runat="server" />
        </eclipse:LeftPanel>
    </eclipse:TwoColumnPanel>
    <eclipse:AppliedFilters runat="server" ID="af" ContainerId="tcpFilters" />
    <div style="font-size: large; margin-bottom: 1mm">
        <asp:Label ID="lblOpeningBalance" runat="server" Width="60%" Text="Opening Balance: {0:C}"
            Visible="false" />
        <asp:Label ID="lblClosingBalance" runat="server" Text="Closing Balance: {0:C}" Visible="false" />
    </div>
    <phpa:PhpaLinqDataSource runat="server" TableName="RoVouchers" ContextTypeName="Eclipse.PhpaLibrary.Reporting.ReportingDataContext"
        ID="ds" OnSelecting="ds_Selecting" RenderLogVisible="false">
    </phpa:PhpaLinqDataSource>
    <jquery:GridViewEx ID="gv" runat="server" DataSourceID="ds" AutoGenerateColumns="false"
        AllowSorting="true" ShowFooter="true" AllowPaging="true" PageSize="50" OnDataBound="gv_DataBound">
        <Columns>
            <eclipse:SequenceField />
            <eclipse:MultiBoundField DataFields="Date" HeaderText="Voucher|Date" SortExpression="Date"
                DataFormatString="{0:d}" />
            <eclipse:HyperLinkFieldEx DataTextField="VoucherReference" DataNavigateUrlFields="VoucherId"
                HeaderText="Voucher|Ref No." DataNavigateUrlFormatString="~/Finance/InsertVoucher.aspx?VoucherId={0}">
                <ItemStyle Wrap="false" />
            </eclipse:HyperLinkFieldEx>
            <eclipse:MultiBoundField DataFields="VoucherCode,VoucherType,CheckNumber" HeaderText="Type"
                DataFormatString="{0}|{1} <br />Cheque #:{2}">
                <ItemStyle Wrap="false" />
            </eclipse:MultiBoundField>
            <eclipse:MultiBoundField DataFields="Particulars" HeaderText="Particulars" />
            <eclipse:MultiBoundField DataFields="DebitAmount" AccessibleHeaderText="DebitAmount"
                HeaderText="Debit" DataSummaryCalculation="ValueSummation" DataFormatString="{0:#,###.##; (#,###.###);''}">
                <ItemStyle HorizontalAlign="Right" />
                <FooterStyle HorizontalAlign="Right" />
            </eclipse:MultiBoundField>
            <eclipse:MultiBoundField DataFields="CreditAmount" AccessibleHeaderText="CreditAmount"
                HeaderText="Credit" DataSummaryCalculation="ValueSummation" DataFormatString="{0:#,###.##; (#,###.###);''}">
                <ItemStyle HorizontalAlign="Right" />
                <FooterStyle HorizontalAlign="Right" />
            </eclipse:MultiBoundField>
        </Columns>
    </jquery:GridViewEx>
    <asp:Label runat="server" ID="lblNetBalance" Text="Net Balance: {0:C}" Font-Size="Large"
        Visible="false" />
</asp:Content>
