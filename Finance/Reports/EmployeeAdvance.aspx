<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" CodeBehind="EmployeeAdvance.aspx.cs"
    Inherits="PhpaAll.Reports.EmployeeAdvance" Title="Employee Advance" EnableViewState="false" %>

<%@ Register Src="../Controls/PrinterFriendlyButton.ascx" TagName="PrinterFriendlyButton"
    TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphSideNavigation" runat="server">
    <uc2:PrinterFriendlyButton ID="PrinterFriendlyButton1" runat="server" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cph" runat="server">
    <eclipse:TwoColumnPanel runat="server">
        <eclipse:LeftLabel runat="server" Text="As of date" />
        <i:TextBoxEx ID="tbDate" runat="server" FriendlyName="As of date" Text="0">
            <Validators>
                <i:Required />
                <i:Date />
            </Validators>
        </i:TextBoxEx>
        <eclipse:LeftLabel runat="server" Text="Division" />
        <phpa:PhpaLinqDataSource runat="server" ID="dsDivisions" ContextTypeName="Eclipse.PhpaLibrary.Reporting.ReportingDataContext"
            OrderBy="DivisionName" Select="new (DivisionId, DivisionName, DivisionGroup)"
            TableName="RoDivisions" Visible="True" RenderLogVisible="false">
        </phpa:PhpaLinqDataSource>
        <i:DropDownListEx runat="server" ID="tbDivisionCode" DataSourceID="dsDivisions" DataTextField="DivisionName"
            DataValueField="DivisionId" DataOptionGroupField="DivisionGroup">
            <Items>
                <eclipse:DropDownItem Text="(All Divisions)" Persistent="Always" />
            </Items>
        </i:DropDownListEx>
        <br />
        <i:ButtonEx ID="btnShowEmpAdv" runat="server" Text="Go" Action="Submit" CausesValidation="true" Icon="Refresh" IsDefault="true"/>
    <i:ButtonEx ID="ExportBtn" runat="server" Text="ExportToExcel" OnClick="ExportBtn_Click" CausesValidation="true"
            Action="Submit" IsDefault="true"/>
    </eclipse:TwoColumnPanel>
    <i:ValidationSummary ID="ValidationSummary1" runat="server" />
    <phpa:PhpaLinqDataSource ID="dsEmpAdv" runat="server" ContextTypeName="Eclipse.PhpaLibrary.Reporting.ReportingDataContext"
        TableName="" RenderLogVisible="False" OnSelecting="dsEmpAdv_Selecting">
    </phpa:PhpaLinqDataSource>
    <jquery:GridViewEx ID="grdEmpAdv" AutoGenerateColumns="false" ShowFooter="True" runat="server"
        AllowSorting="false" DataSourceID="dsEmpAdv" OnRowDataBound="grdEmpAdv_RowDataBound">
        <Columns>
            <eclipse:SequenceField />
            <eclipse:MultiBoundField HeaderText="Code" DataFields="Employee.EmployeeCode" SortExpression="Employee.EmployeeCode" />
            <eclipse:MultiBoundField HeaderText="Name" DataFields="Employee.FirstName,Employee.LastName"
                SortExpression="Employee.FirstName" DataFormatString="{0}{1}" ItemStyle-Width="1.5in"
                ItemStyle-Wrap="true" />
            <eclipse:MultiBoundField HeaderText="Designation" DataFields="Employee.Designation" />
            <jquery:MatrixField DataMergeFields="Employee.EmployeeId" DataHeaderFields="HeadOfAccount.Description"
                DataValueFields="advanceAmount, EmployeeId, HeadOfAccountId" HeaderText="Amount" DisplayRowTotals="true" DataHeaderCustomFields="HeadOfAccount.HeadOfAccountId"
                DataTotalFormatString="{0:C}">
                <ItemTemplate>
                    <asp:HyperLink ID="hlEmployeeAdvances" runat="server" Text='<%# MatrixBinder.Eval("advanceAmount", "{0:C}")%>'
                        NavigateUrl='<%# string.Format("~/Finance/VoucherSearch.aspx?AccountTypes=EMPLOYEE_ADVANCE&EmployeeId={0}&HeadOfAccountId={1}", MatrixBinder.Eval("EmployeeId"),MatrixBinder.Eval("HeadOfAccountId"))%>'>
                    </asp:HyperLink>
                </ItemTemplate>
            </jquery:MatrixField>
        </Columns>
        <EmptyDataTemplate>
            No Employee Advances found for given date.
        </EmptyDataTemplate>
        <FooterStyle HorizontalAlign="Right" />
    </jquery:GridViewEx>
</asp:Content>
