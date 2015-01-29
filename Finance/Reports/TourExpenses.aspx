<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" CodeBehind="TourExpenses.aspx.cs"
    Inherits="PhpaAll.Reports.TourExpenses" Title="Tour Expenses" EnableViewState="false" %>

<%@ Register Src="../Controls/PrinterFriendlyButton.ascx" TagName="PrinterFriendlyButton"
    TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphSideNavigation" runat="server">
    <uc2:PrinterFriendlyButton ID="PrinterFriendlyButton1" runat="server" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cph" runat="server">
    <eclipse:TwoColumnPanel runat="server">
        <eclipse:LeftLabel runat="server" Text="Date From/Date To" />
        <i:TextBoxEx ID="tbFromDate" runat="server" FriendlyName="From Date">
            <Validators>
                <i:Date />
            </Validators>
        </i:TextBoxEx>
        <i:TextBoxEx ID="tbToDate" runat="server" FriendlyName="To Date">
            <Validators>
                <i:Date DateType="ToDate" />
            </Validators>
        </i:TextBoxEx>
        <br />
        Leave both dates blank to summarize all existing data.
        <br />
        <i:ButtonEx ID="btnShowReport" runat="server" Text="Go" OnClick="btnShowReport_Click"
            Icon="Refresh" Action="Submit" CausesValidation="true" />
    </eclipse:TwoColumnPanel>
    <asp:Label ID="lblnodata" runat="server" Text="<b>No Data Available" Visible="false"></asp:Label>
    <i:ValidationSummary ID="ValidationSummary1" runat="server" />
    <phpa:PhpaLinqDataSource ID="dsTourExp" runat="server" ContextTypeName="Eclipse.PhpaLibrary.Reporting.ReportingDataContext"
        TableName="AccountTypes" RenderLogVisible="False" OnSelecting="dsTourExp_Selecting"
        OrderBy="EmpFirstName">
    </phpa:PhpaLinqDataSource>
    <jquery:GridViewEx ID="gvTourExpenses" runat="server" AutoGenerateColumns="false" ShowFooter="True"
        DataSourceID="dsTourExp" OnRowDataBound="gvTourExpenses_RowDataBound" AllowSorting="true">
        <Columns>
            <eclipse:SequenceField Visible="false" />
            <eclipse:MultiBoundField DataFields="EmpCode" HeaderText="Employee|Code" SortExpression="EmpCode" />
            <eclipse:MultiBoundField DataFields="EmpFirstName,EmpLastName" HeaderText="Employee|Name"
                DataFormatString="{0} {1}" SortExpression="EmpFirstName" />
            <eclipse:MultiBoundField DataFields="Designation" HeaderText="Employee|Designition"
                SortExpression="Designation" />
            <eclipse:MultiBoundField DataFields="division" HeaderText="Employee|Division" SortExpression="division"
                FooterText="Total" />
            <asp:TemplateField HeaderText="Amount (Nu.)" AccessibleHeaderText="TourExpense">
                <ItemStyle HorizontalAlign="Right" />
                <ItemTemplate>
                    <asp:HyperLink ID="hlTourExpense" runat="server" Text='<%# Eval("TourExpense", "{0:C}")%>'>
                    </asp:HyperLink>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
        <FooterStyle HorizontalAlign="Right" />
    </jquery:GridViewEx>
</asp:Content>
