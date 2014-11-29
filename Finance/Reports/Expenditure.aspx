<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" CodeBehind="Expenditure.aspx.cs"
    Inherits="Finance.Reports.Expenditure" EnableViewState="false" %>

<%@ Register Src="../Controls/PrinterFriendlyButton.ascx" TagName="PrinterFriendlyButton"
    TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server" EnableViewState="false">
    <style type="text/css">
        .HeadOfAccountColumn
        {
            width: 35em;
            min-width: 20em;
        }
    </style>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphSideNavigation" runat="server">
    <asp:HyperLink ID="HyperLink2" runat="server" Text="Help" NavigateUrl="~/Doc/Expenditure.doc.aspx" /><br />
    <uc2:PrinterFriendlyButton ID="PrinterFriendlyButton1" runat="server" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cph" runat="server">
    <phpa:PhpaLinqDataSource ID="dsExpenditure" runat="server" ContextTypeName="Eclipse.PhpaLibrary.Reporting.ReportingDataContext"
        OnSelecting="dsExpenditure_Selecting" RenderLogVisible="False">
        <WhereParameters>
            <asp:ControlParameter ControlID="tbMonth" Name="Date" Type="DateTime" />
        </WhereParameters>
    </phpa:PhpaLinqDataSource>
    <asp:Label runat="server" CssClass="noprint" Text="Monthly report showing expenditure for Head of Accounts" />
    <asp:HyperLink ID="hlHeadofAccount" runat="server" CssClass="noprint" NavigateUrl="~/Reports/Drill/ExpenditureHeadofAccount.aspx"
        Text="Click here to view the Expenditure Head of Accounts."></asp:HyperLink>
    <eclipse:TwoColumnPanel runat="server">
        <eclipse:LeftLabel runat="server" Text="For Date" />
        <i:TextBoxEx ID="tbMonth" runat="server" FriendlyName="Date" Text="0">
            <Validators>
                <i:Date Max="0" />
                <i:Required />
            </Validators>
        </i:TextBoxEx>
        <i:ButtonEx ID="btnShowReport" runat="server" Text="Show Report" Icon="Refresh" Action="Submit"
            OnClick="btnShowReport_Click" CausesValidation="true" IsDefault="true" />
    </eclipse:TwoColumnPanel>
    <i:ValidationSummary ID="vsExpenditure" runat="server" />
    <jquery:GridViewEx ID="gvExpenditure" runat="server" AutoGenerateColumns="False"
        ShowFooter="true" DataSourceID="dsExpenditure" OnRowDataBound="gvExpenditure_RowDataBound"
        DataKeyNames="HeadId">
        <Columns>
            <eclipse:SequenceField FooterText="">
            </eclipse:SequenceField>
            <eclipse:MultiBoundField HeaderText="Head of Account" DataFields="DisplayName" FooterText="GRAND TOTAL">
                <HeaderStyle CssClass="HeadOfAccountColumn" />
                <ItemStyle CssClass="HeadOfAccountColumn" />
            </eclipse:MultiBoundField>
            <eclipse:MultiBoundField DataFields="ProjectCost" HeaderText="Project Cost" AccessibleHeaderText="ProjectCost"
                DataFormatString="{0:C2}">
                <HeaderStyle HorizontalAlign="Center" />
                <ItemStyle HorizontalAlign="Right" />
                <FooterStyle HorizontalAlign="Right" />
            </eclipse:MultiBoundField>
            <eclipse:MultiBoundField DataFields="UptoPreviousYearsExpenses" HeaderToolTip="Expenditure upto Previous Financial year."
                HeaderText="Upto Previous Year" AccessibleHeaderText="UptoPreviousYearsExpenses"
                DataFormatString="{0:C2}">
                <HeaderStyle HorizontalAlign="Center" />
                <ItemStyle HorizontalAlign="Right" />
                <FooterStyle HorizontalAlign="Right" />
            </eclipse:MultiBoundField>
            <asp:TemplateField AccessibleHeaderText="CurrentYearDuringMonthExpenses" HeaderText="Current Year|During the Month">
                <HeaderTemplate>
                    <asp:Label ID="lblDuringMonth" runat="server" Text=""
                        ToolTip='<%# string.Format("Month to date total expenditure {0: d MMMM yyyy} to {1: d MMMM yyyy}", DateFrom, DateTo) %>'></asp:Label>
                </HeaderTemplate>
                <ItemTemplate>
                    <asp:HyperLink runat="server" ID="hlDuringMonth" Text='<%# Eval("CurrentYearDuringMonthExpenses","{0:C2}") %>'
                        NavigateUrl='<%# string.Format("~/Finance/VoucherSearch.aspx?&HeadofAccountId={0}&DateFrom={1:d}&DateTo={2:d}",
                        Eval("HeadOfAccountId"), DateFrom, DateTo) %>' ClientIDMode="Static" />
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Right" />
                <FooterStyle HorizontalAlign="Right" />
            </asp:TemplateField>
            <asp:TemplateField AccessibleHeaderText="CurrentYearUptoMonthExpenses" HeaderText="Current Year|Upto the Month">
                <HeaderTemplate>
                    <asp:Label ID="lblUptoMonth" runat="server" Text="" ToolTip='<%# string.Format("Month to date total expenditure {0: d MMMM yyyy} to {1: d MMMM yyyy}", DateFrom, DateTo) %>'></asp:Label>
                </HeaderTemplate>
                <ItemTemplate>
                    <asp:HyperLink runat="server" ID="hlUptoMonth" Text='<%# Eval("CurrentYearUptoMonthExpenses","{0:C2}") %>'
                        NavigateUrl='<%# string.Format("~/Finance/VoucherSearch.aspx?&HeadofAccountId={0}&DateFrom={1:d}&DateTo={2:d}",
                        Eval("HeadOfAccountId"), DateFrom, DateTo) %>' ClientIDMode="Static" />
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Right" />
                <FooterStyle HorizontalAlign="Right" />
            </asp:TemplateField>
            <eclipse:MultiBoundField DataFields="Cumulative" HeaderToolTip="Display the total expenditure till date including current month and previous financial year."
                HeaderText="Cumulative" AccessibleHeaderText="Cumulative" DataFormatString="{0:C2}">
                <ItemStyle HorizontalAlign="Right" />
                <HeaderStyle HorizontalAlign="Center" />
                <FooterStyle HorizontalAlign="Right" />
            </eclipse:MultiBoundField>
        </Columns>
        <EmptyDataTemplate>
            <b>Expenditure report not found for the given date.</b>
        </EmptyDataTemplate>
    </jquery:GridViewEx>
</asp:Content>
