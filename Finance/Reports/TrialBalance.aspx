<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" CodeBehind="TrialBalance.aspx.cs"
    Inherits="PhpaAll.Reports.TrialBalance" Title="Trial Balance Report" EnableViewState="false" %>

<%@ Register Src="../Controls/PrinterFriendlyButton.ascx" TagName="PrinterFriendlyButton"
    TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .HeadOfAccountColumn
        {
            width: 25em;
            min-width: 10em;
        }
    </style>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphSideNavigation" runat="server">
    <asp:HyperLink ID="HyperLink2" runat="server" Text="Help" NavigateUrl="~/Doc/TrialBalance.doc.aspx" /><br />
    <uc2:PrinterFriendlyButton ID="PrinterFriendlyButton1" runat="server" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cph" runat="server">
    <phpa:PhpaLinqDataSource ID="dsVoucherDetails" runat="server" ContextTypeName="Eclipse.PhpaLibrary.Reporting.ReportingDataContext"
        OnSelecting="dsVoucherDetails_Selecting" TableName="" RenderLogVisible="False" />
    <eclipse:TwoColumnPanel runat="server">
        <eclipse:LeftLabel runat="server" Text="For Date" />
        <i:TextBoxEx ID="tbMonth" runat="server" FriendlyName="Date" Text="0">
            <Validators>
                <i:Required />
                <i:Date />
            </Validators>
        </i:TextBoxEx>
        <br />
        <i:ButtonEx ID="btnShowReport" runat="server" Text="Go" OnClick="btnShowReport_Click"
            CausesValidation="true" Action="Submit" IsDefault="true"/>

        <i:ButtonEx ID="ExportBtn" runat="server" Text="ExportToExcel" OnClick="ExportBtn_Click" CausesValidation="true"
            Action="Submit" IsDefault="true"/>
        <i:ValidationSummary ID="vsMonth" runat="server" />
        
    </eclipse:TwoColumnPanel>
    <jquery:GridViewEx ID="gvTrialBalance" runat="server" ShowFooter="true" AutoGenerateColumns="false"
        DataSourceID="dsVoucherDetails" OnRowDataBound="gvTrialBalance_RowDataBound" 
        CellPadding="3">
        <FooterStyle CssClass="ui-priority-primary" Font-Size="Larger" />
        <Columns>
            <eclipse:MultiBoundField HeaderText="Head of Account" DataFields="DisplayName" FooterText="GRAND TOTAL">
                <HeaderStyle CssClass="HeadOfAccountColumn" />
            </eclipse:MultiBoundField>
            <asp:TemplateField AccessibleHeaderText="CurrentMonthGrossDebitSum" HeaderText="During the Month |Debit">
                <HeaderTemplate>
                    <asp:Label ID="lblDebit" runat="server" Text="During the Month Debit" ToolTip='<%# string.Format("Month to date total debits {0: d MMMM yyyy} to {1: d MMMM yyyy}", DateFrom, DateTo) %>'></asp:Label>
                </HeaderTemplate>
                <ItemTemplate>
                    <asp:HyperLink runat="server" ID="hlMonthDebit" Text='<%# Eval("CurrentMonthGrossDebitSum","{0:C2}") %>'
                        NavigateUrl='<%# string.Format("~/Finance/VoucherSearch.aspx?&HeadofAccountId={0}&DateFrom={1:d}&DateTo={2:d}",
                        Eval("HeadOfAccountId"), DateFrom, DateTo) %>' ClientIDMode="Static" />
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Right" />
                <FooterStyle HorizontalAlign="Right" />
            </asp:TemplateField>
            <asp:TemplateField AccessibleHeaderText="CurrentMonthGrossCreditSum" HeaderText="During the Month |Credit">
                <HeaderTemplate>
                    <asp:Label ID="lblCredit" runat="server" Text="During the Month Credit" ToolTip='<%# string.Format("Month to date total credits {0: d MMMM yyyy} to {1: d MMMM yyyy}", DateFrom, DateTo) %>'></asp:Label>
                </HeaderTemplate>
                <ItemTemplate>
                    <asp:HyperLink runat="server" ID="hlMonthCredit" Text='<%# Eval("CurrentMonthGrossCreditSum","{0:C2}") %>'
                        NavigateUrl='<%# string.Format("~/Finance/VoucherSearch.aspx?&HeadofAccountId={0}&DateFrom={1:d}&DateTo={2:d}",
                        Eval("HeadOfAccountId"), DateFrom, DateTo) %>' ClientIDMode="Static" />
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Right" />
                <FooterStyle HorizontalAlign="Right" />
            </asp:TemplateField>
            <asp:TemplateField AccessibleHeaderText="CurrentYearNetDebitSum" HeaderText="During the Year |Debit">
                <HeaderTemplate>
                    <asp:Label ID="lblDebitNet" runat="server" Text="During the Year Debit" ToolTip='<%# string.Format("Financial year to date total debits {0: d MMMM yyyy} to {1: d MMMM yyyy}", DateFrom, DateTo) %>'></asp:Label>
                </HeaderTemplate>
                <ItemTemplate>
                    <asp:HyperLink runat="server" ID="hlYearDebit" Text='<%# Eval("CurrentYearNetDebitSum","{0:C2}") %>'
                        NavigateUrl='<%# string.Format("~/Finance/VoucherSearch.aspx?&HeadofAccountId={0}&DateFrom={1:d}&DateTo={2:d}",
                        Eval("HeadOfAccountId"), DateFrom, DateTo) %>' ClientIDMode="Static" />
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Right" />
                <FooterStyle HorizontalAlign="Right" />
            </asp:TemplateField>
            <asp:TemplateField AccessibleHeaderText="CurrentYearNetCreditSum" HeaderText="During the Year |Credit">
                <HeaderTemplate>
                    <asp:Label ID="lblCreditNet" runat="server" Text="During the Year Credit" ToolTip='<%# string.Format("Financial year to date total credits {0: d MMMM yyyy} to {1: d MMMM yyyy}", DateFrom, DateTo) %>'></asp:Label>
                </HeaderTemplate>
                <ItemTemplate>
                    <asp:HyperLink runat="server" ID="hlYearCredit" Text='<%# Eval("CurrentYearNetCreditSum","{0:C2}") %>'
                        NavigateUrl='<%# string.Format("~/Finance/VoucherSearch.aspx?&HeadofAccountId={0}&DateFrom={1:d}&DateTo={2:d}",
                        Eval("HeadOfAccountId"), DateFrom, DateTo) %>' ClientIDMode="Static" />
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Right" />
                <FooterStyle HorizontalAlign="Right" />
            </asp:TemplateField>
            <eclipse:MultiBoundField DataFields="CumulativeDebitSum" HeaderText="Total Cumulative (Net)|Debit"
                HeaderToolTip="Display total Debit amount till current date." AccessibleHeaderText="CumulativeDebitSum"
                DataFormatString="{0:#,###.00;(#,###.00);''}">
                <ItemStyle HorizontalAlign="Right" />
            </eclipse:MultiBoundField>
            <eclipse:MultiBoundField DataFields="CumulativeCreditSum" HeaderText="Total Cumulative (Net)|Credit"
                HeaderToolTip="Display total Credit amount till current date." AccessibleHeaderText="CumulativeCreditSum"
                DataFormatString="{0:#,###.00;(#,###.00);''}">
                <ItemStyle HorizontalAlign="Right" />
            </eclipse:MultiBoundField>
        </Columns>
        <EmptyDataTemplate>
            Trial Balance not found for the given date.
        </EmptyDataTemplate>
    </jquery:GridViewEx>
</asp:Content>
