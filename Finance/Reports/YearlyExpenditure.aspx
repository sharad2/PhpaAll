<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" CodeBehind="YearlyExpenditure.aspx.cs"
    Inherits="Finance.Reports.YearlyExpenditure" Title="Yearly Expenditure" %>

<%@ Register Src="../Controls/PrinterFriendlyButton.ascx" TagName="PrinterFriendlyButton"
    TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphSideNavigation" runat="server">
    <uc2:PrinterFriendlyButton ID="PrinterFriendlyButton1" runat="server" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cph" runat="server">
    <phpa:PhpaLinqDataSource ID="dsExpenditure" runat="server" ContextTypeName="Eclipse.PhpaLibrary.Reporting.ReportingDataContext"
        TableName="" RenderLogVisible="false">
    </phpa:PhpaLinqDataSource>
    This Report shows you Expenditure incurred each year.
    <br />
    <br />
    <jquery:GridViewEx ID="gvExpenditure" runat="server" AutoGenerateColumns="false"
        ShowFooter="true" OnRowDataBound="gvExpenditure_RowDataBound">
        <Columns>
            <eclipse:SequenceField />
            <eclipse:MultiBoundField DataFields="Year,NextYear" HeaderText="Year" FooterText="Total" ItemStyle-Width="2in" DataFormatString="{0}-{1}" />
            <eclipse:MultiBoundField DataFields="Amount" HeaderText="Amount" DataSummaryCalculation="ValueSummation"
                DataFormatString="{0:N2}">
                <ItemStyle HorizontalAlign="Right" Width="2in" />
                <FooterStyle HorizontalAlign="Right" />
            </eclipse:MultiBoundField>
        </Columns>
        <EmptyDataTemplate>
            <b>No data found.</b>
        </EmptyDataTemplate>
    </jquery:GridViewEx>
</asp:Content>
