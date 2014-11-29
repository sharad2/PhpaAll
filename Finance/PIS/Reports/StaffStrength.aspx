<%@ Page Title="Staff Strength Report" Language="C#" MasterPageFile="~/MasterPage.master"
    CodeBehind="StaffStrength.aspx.cs" Inherits="PIS.Reports.StaffStrength" EnableViewState="false" %>

<%@ Register Src="~/Controls/PrinterFriendlyButton.ascx" TagPrefix="uc1" TagName="PrinterFriendlyButton" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" runat="server" ContentPlaceHolderID="cphSideNavigation">
    <uc1:PrinterFriendlyButton runat="server" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cph" runat="server">
    <eclipse:TwoColumnPanel runat="server" ID="tcp" IsValidationContainer="true">
        <eclipse:LeftLabel runat="server" Text="Staff Strength uptill" />
        <i:TextBoxEx runat="server" ID="dtServiceTo" FriendlyName="Staff Strength uptill"
            TabIndex="2" Text="0">
            <Validators>
                <i:Required />
                <i:Date />
            </Validators>
        </i:TextBoxEx>
        <br />
        <i:ButtonEx runat="server" ID="btnApplyFilters" Text="Apply Filters" TabIndex="3"
            Action="Submit" IsDefault="true" CausesValidation="true" />
        <i:ButtonEx ID="btnClear" runat="server" Text="Clear Filters" Action="Reset" />
        <i:ValidationSummary runat="server" />
    </eclipse:TwoColumnPanel>
    <eclipse:AppliedFilters runat="server" ID="af" ContainerId="tcp" ClientIDMode="Static" />
    <phpa:PhpaLinqDataSource runat="server" ID="dsStaff" ContextTypeName="Eclipse.PhpaLibrary.Database.PIS.PISDataContext"
        TableName="Employees" RenderLogVisible="false" OnSelecting="dsStaff_Selecting"
        EnableViewState="false" />
    <jquery:GridViewEx runat="server" ID="gvStaffStrength" DataSourceID="dsStaff" AutoGenerateColumns="false"
        ShowFooter="true" AllowSorting="true" EmptyDataText="No Employees found">
        <Columns>
            <eclipse:MultiBoundField DataFields="EmpCategory" HeaderText="Category" FooterText="Total"
                SortExpression="EmpCategory" NullDisplayText="Not Set" />
            <jquery:MatrixField DataMergeFields="EmployeeTypeId" DataHeaderFields="IsBhutanese"
                DataValueFields="Total" HeaderText="Nationality" DisplayColumnTotals="true" DisplayRowTotals="true"
                DataHeaderFormatString="{0:'Bhutanese';'Not Expected';'Foreigner'}">
                <ItemTemplate>
                    <asp:HyperLink runat="server" NavigateUrl='<%# string.Format("~/PIS/Employees.aspx?IsBhutanese={0}&ActiveOnDate={1:d}&EmployeeTypeId={2}",
                      MatrixBinder.Eval("IsBhutanese"), dtServiceTo.ValueAsDate, MatrixBinder.Eval("EmployeeTypeId")) %>'>
                <%# MatrixBinder.Eval("Total", "{0:N0}")%>
                    </asp:HyperLink>
                </ItemTemplate>
            </jquery:MatrixField>
        </Columns>
    </jquery:GridViewEx>
</asp:Content>
