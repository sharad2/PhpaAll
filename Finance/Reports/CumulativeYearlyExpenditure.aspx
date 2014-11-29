<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" CodeBehind="CumulativeYearlyExpenditure.aspx.cs"
    EnableViewState="false" Inherits="Finance.Reports.CumulativeYearlyExpenditure"
    Title="Cumulative Yearly Expenditure Report" %>

<%@ Register Src="../Controls/PrinterFriendlyButton.ascx" TagName="PrinterFriendlyButton"
    TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphSideNavigation" runat="server">
    <asp:HyperLink ID="HyperLink2" runat="server" Text="Help" NavigateUrl="~/Doc/CumulativeYearlyExpenditure.doc.aspx" /><br/>
    <uc2:PrinterFriendlyButton ID="PrinterFriendlyButton1" runat="server" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cph" runat="server">
    <phpa:PhpaLinqDataSource ID="ds" runat="server" ContextTypeName="Eclipse.PhpaLibrary.Reporting.ReportingDataContext"
        OnSelecting="ds_Selecting" RenderLogVisible="False">
    </phpa:PhpaLinqDataSource>
    <jquery:GridViewEx runat="server" ID="gv" DataSourceID="ds" AutoGenerateColumns="false"
        ShowFooter="true">
        <Columns>
            <eclipse:MultiBoundField HeaderText="Head of Account" DataFields="DisplayName" FooterText="Grand Total" />
            <eclipse:MultiBoundField HeaderText="Project Cost" DataFields="ProjectCost" DataFormatString="{0:#,###}" />
            <jquery:MatrixField HeaderText="Expenditure (In Million Nu.)" DataMergeFields="HeadOfAccountId"
                DataHeaderFields="FinancialYearStart" DataValueFields="Expenditure" DataHeaderFormatString="{0:yyyy}"
                DataHeaderSortFields="FinancialYearStart" DataValueFormatString="{0:#,###,###,,.00;(#,###,###,,.00);''}"
                DisplayRowTotals="true" DisplayColumnTotals="true" RowTotalHeaderText="Till Date" OnMatrixRowDataBound="matrix_RowDataBound">
                <MatrixColumns>
                    <jquery:MatrixColumn Visible="false" />
                </MatrixColumns>
            </jquery:MatrixField>
        </Columns>
    </jquery:GridViewEx>
</asp:Content>
