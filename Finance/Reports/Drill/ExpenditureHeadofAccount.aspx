<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" CodeBehind="ExpenditureHeadofAccount.aspx.cs" Inherits="PhpaAll.Reports.ExpenditureHeadofAccount" Title="Expenditure Head of Accounts" %>
<%@ Register src="~/Controls/PrinterFriendlyButton.ascx" tagname="PrinterFriendlyButton" tagprefix="uc2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphSideNavigation" runat="server">
    <uc2:PrinterFriendlyButton ID="PrinterFriendlyButton1" runat="server" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cph" runat="server">
    <phpa:PhpaLinqDataSource ID="dsExpenditureHeads" runat="server"
        ContextTypeName="Eclipse.PhpaLibrary.Reporting.ReportingDataContext" 
        TableName="RoHeadHierarchies" RenderLogVisible="false" Where="HeadOfAccountType == @HeadOfAccountType" OrderBy="SortableName">
        <WhereParameters>
            <asp:Parameter DefaultValue="EXPENDITURE" Name="HeadOfAccountType" 
                Type="String" />
        </WhereParameters>
    </phpa:PhpaLinqDataSource>
    <jquery:GridViewEx ID="gvExpenidtureHeads" runat="server" 
        DataSourceID="dsExpenditureHeads" AutoGenerateColumns="False">
        <Columns>
            <eclipse:MultiBoundField DataFields="TopParentName" HeaderText="Parent Head" />
            <eclipse:MultiBoundField DataFields="DisplayName" HeaderText="Head of Account" />
            <eclipse:MultiBoundField DataFields="Description" HeaderText="Description" />
            <eclipse:MultiBoundField DataFields="HeadOfAccountType" HeaderText="Head Of Account Type" />
        </Columns>
    </jquery:GridViewEx>
</asp:Content>
