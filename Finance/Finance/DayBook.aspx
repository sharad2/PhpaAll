<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" CodeBehind="DayBook.aspx.cs"
    Inherits="PhpaAll.Finance.DayBook" Title="Day Book" EnableViewState="false" %>

<%@ Register Src="../Controls/VoucherDetailControl.ascx" TagName="VoucherDetailControl"
    TagPrefix="uc1" %>
<%@ Register Src="../Controls/PrinterFriendlyButton.ascx" TagName="PrinterFriendlyButton"
    TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphSideNavigation" runat="server">
    <asp:HyperLink ID="HyperLink2" runat="server" Text="Help" NavigateUrl="~/Doc/DayBook.doc.aspx" />
    <uc2:PrinterFriendlyButton ID="PrinterFriendlyButton1" runat="server" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cph" runat="server">
    <phpa:PhpaLinqDataSource ID="dsOpeningBalance" runat="server" ContextTypeName="Eclipse.PhpaLibrary.Reporting.ReportingDataContext"
        TableName="RoVouchers" RenderLogVisible="false">
    </phpa:PhpaLinqDataSource>
    <eclipse:TwoColumnPanel runat="server">
        <eclipse:LeftLabel runat="server" Text="Voucher Date" />
        <i:TextBoxEx ID="tbVoucherDate" runat="server" QueryString="VoucherDate" FriendlyName="Date"
            Text="0">
            <Validators>
                <i:Required />
                <i:Date />
            </Validators>
        </i:TextBoxEx>
        <br />
        All vouchers of this date are displayed below. You can edit any displayed voucher
        by clicking its voucher date. To see vouchers for another date, enter the date and
        press
        <i:ButtonEx ID="btnShowReport" runat="server" Text="Go" CausesValidation="true" Action="Submit"
            IsDefault="true" />
        <i:ValidationSummary runat="server" />
    </eclipse:TwoColumnPanel>
    <uc1:VoucherDetailControl ID="ctlVoucherDetail" runat="server" InformationLevel="Detail">
        <EmptyTemplate>
            <asp:Label ID="lblMessage" runat="server" OnPreRender="lblMessage_PreRender" />
            <asp:HyperLink ID="hlRecentDate" runat="server">Dynamic Date</asp:HyperLink>
        </EmptyTemplate>
    </uc1:VoucherDetailControl>
    <jquery:GridViewEx ID="gvClosingBalance" runat="server" AutoGenerateColumns="false"
        OnRowDataBound="gvClosingBalance_RowDataBound" Caption="Closing Balances of Bank & Cash Accounts"
        ShowFooter="true">
        <Columns>
            <asp:TemplateField HeaderText="Head Of Account">
                <ItemTemplate>
                    <asp:HyperLink ID="hlHeadofAccount" runat="server" Text='<%# Eval("DisplayName") %>'
                        NavigateUrl="~/Reports/Ledger.aspx" ToolTip="Click here to see the year to date ledger of this account">
                    </asp:HyperLink>
                </ItemTemplate>
            </asp:TemplateField>
            <eclipse:MultiBoundField HeaderText="Head Of Account" HeaderToolTip="The head of account for the voucher entry"
                DataFields="Description" FooterText="Total" />
            <eclipse:MultiBoundField HeaderText="Amounts in Nu|Opening Balance" DataFormatString="{0:N2}"
                DataSummaryCalculation="ValueSummation" DataFields="OpeningBalance" HeaderToolTip="Calculated from project start to just before the entered voucher date">
                <ItemStyle HorizontalAlign="Right" />
                <HeaderStyle HorizontalAlign="Center" />
                <FooterStyle HorizontalAlign="Right" />
            </eclipse:MultiBoundField>
            <eclipse:MultiBoundField HeaderText="Amounts in Nu|Receipts" DataFormatString="{0:N2}"
                DataSummaryCalculation="ValueSummation" DataFields="Receipts" HeaderToolTip="Receipts on the entered date. These vouchers are shown below">
                <ItemStyle HorizontalAlign="Right" />
                <HeaderStyle HorizontalAlign="Center" />
                <FooterStyle HorizontalAlign="Right" />
            </eclipse:MultiBoundField>
            <eclipse:MultiBoundField HeaderText="Amounts in Nu|Payments" DataFormatString="{0:N2}"
                DataSummaryCalculation="ValueSummation" DataFields="Payments" HeaderToolTip="Payments made on the entered date. These vouchers are shown below">
                <ItemStyle HorizontalAlign="Right" />
                <HeaderStyle HorizontalAlign="Center" />
                <FooterStyle HorizontalAlign="Right" />
            </eclipse:MultiBoundField>
            <eclipse:MultiBoundField HeaderText="Amounts in Nu|Closing Balance" DataFormatString="{0:N2}"
                DataSummaryCalculation="ValueSummation" DataFields="ClosingBalance" HeaderToolTip="Closing Balance = Opening Balance + Receipts">
                <ItemStyle HorizontalAlign="Right" />
                <HeaderStyle HorizontalAlign="Center" />
                <FooterStyle HorizontalAlign="Right" />
            </eclipse:MultiBoundField>
        </Columns>
        <EmptyDataTemplate>
            No cash or bank accounts exist.
        </EmptyDataTemplate>
    </jquery:GridViewEx>
    <br />
    <br />
    <br />
    <div>
        <div style="float: left">
            <asp:Label runat="server" Text="<b>Div. Accountant / Section Officer(A/Cs)</b>" />
        </div>
        <div style="float: right">
            <asp:Label runat="server" Text="<b>Finance Officer / Sr.Finance Officer</b>" />
        </div>
    </div>
</asp:Content>
