<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" CodeBehind="BalanceProjectCost.aspx.cs"
    EnableViewState="false" Inherits="Finance.Reports.BalanceProjectCost" Title="Balance Project Cost" %>

<%@ Register Src="../Controls/PrinterFriendlyButton.ascx" TagName="PrinterFriendlyButton"
    TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphSideNavigation" runat="server">
    <asp:HyperLink runat="server" Text="Help" NavigateUrl="~/Doc/BalanceProjectCost.doc.aspx" />
    <br />
    <uc2:PrinterFriendlyButton ID="PrinterFriendlyButton1" runat="server" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cph" runat="server">
    <phpa:PhpaLinqDataSource ID="dsBalanceProjectCost" runat="server" ContextTypeName="Eclipse.PhpaLibrary.Reporting.ReportingDataContext"
        OnSelecting="dsBalanceProjectCost_Selecting" TableName="RoHeadHierarchies" RenderLogVisible="False">
    </phpa:PhpaLinqDataSource>
    <jquery:GridViewEx ID="gvBalanceProjectCost" runat="server" ShowFooter="true" AutoGenerateColumns="False"
        DataSourceID="dsBalanceProjectCost" OnRowDataBound="gvBalanceProjectCost_RowDataBound">
        <Columns>
            <asp:HyperLinkField DataNavigateUrlFormatString="~/Reports/Drill/BalanceProjectCostJobs.aspx?HeadID={0}"
                DataNavigateUrlFields="HeadId" Text="Details" ItemStyle-CssClass="noprint" HeaderStyle-CssClass="noprint"
                FooterStyle-CssClass="noprint" />
            <eclipse:MultiBoundField HeaderText="Head of Account" DataFields="DisplayName" FooterText="GRAND TOTAL">
                <HeaderStyle CssClass="HeadOfAccountColumn" />
                <ItemStyle CssClass="HeadOfAccountColumn" />
            </eclipse:MultiBoundField>
            <eclipse:MultiBoundField DataFields="ProjectCost" HeaderText="Project Cost" AccessibleHeaderText="ProjectCost"
                DataSummaryCalculation="None" DataFormatString="{0:C2}">
                <FooterStyle HorizontalAlign="Right"></FooterStyle>
                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                <ItemStyle HorizontalAlign="Right"></ItemStyle>
            </eclipse:MultiBoundField>
            <eclipse:MultiBoundField DataFields="SanctionIssued" HeaderToolTip="Amount Sanctioned for Jobs under given Head of Account."
                HeaderText="Sanction Issued" AccessibleHeaderText="SanctionIssued" DataSummaryCalculation="None"
                DataFormatString="{0:C2}">
                <FooterStyle HorizontalAlign="Right"></FooterStyle>
                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                <ItemStyle HorizontalAlign="Right"></ItemStyle>
            </eclipse:MultiBoundField>
            <eclipse:MultiBoundField DataFields="WorkAwarded" HeaderToolTip="Amount awarded for Jobs under given Head of Account."
                HeaderText="Work Awarded" AccessibleHeaderText="WorkAwarded" DataSummaryCalculation="None"
                DataFormatString="{0:C2}">
                <FooterStyle HorizontalAlign="Right"></FooterStyle>
                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                <ItemStyle HorizontalAlign="Right"></ItemStyle>
            </eclipse:MultiBoundField>
            <eclipse:MultiBoundField DataFields="Commitment" HeaderToolTip="Amount commited for Jobs under given Head of Account."
                HeaderText="Commitment" AccessibleHeaderText="Commitment" DataSummaryCalculation="None"
                DataFormatString="{0:C2}">
                <FooterStyle HorizontalAlign="Right"></FooterStyle>
                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                <ItemStyle HorizontalAlign="Right"></ItemStyle>
            </eclipse:MultiBoundField>
            <eclipse:MultiBoundField DataFields="BalanceProjectCost" HeaderText="Balance Project Cost"
                AccessibleHeaderText="BalanceProjectCost" DataSummaryCalculation="None"
                DataFormatString="{0:C2}">
                <FooterStyle HorizontalAlign="Right"></FooterStyle>
                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                <ItemStyle HorizontalAlign="Right"></ItemStyle>
            </eclipse:MultiBoundField>
        </Columns>
        <EmptyDataTemplate>
            <b>Balance Project not found.</b>
        </EmptyDataTemplate>
    </jquery:GridViewEx>
</asp:Content>
