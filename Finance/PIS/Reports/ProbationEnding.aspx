<%@ Page Title="Probation Completion" Language="C#" MasterPageFile="~/MasterPage.master"
    CodeBehind="ProbationEnding.aspx.cs" Inherits="PhpaAll.PIS.Reports.ProbationEnding"
    EnableViewState="false" %>

<%@ Register Src="~/Controls/PrinterFriendlyButton.ascx" TagName="PrinterFriendlyButton"
    TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphSideNavigation" runat="server">
    <uc1:PrinterFriendlyButton runat="server" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cph" runat="server">
    <eclipse:TwoColumnPanel runat="server" ID="tcp" IsValidationContainer="true">
        <eclipse:LeftLabel runat="server" Text="Date Range" />
        <i:TextBoxEx runat="server" ID="dtFrom" FriendlyName="From Date" Text="0">
            <Validators>
                <i:Required />
                <i:Date />
            </Validators>
        </i:TextBoxEx>
        <i:TextBoxEx runat="server" ID="dtTo" FriendlyName="To Date" Text="90">
            <Validators>
                <i:Required />
                <i:Date DateType="ToDate" />
            </Validators>
        </i:TextBoxEx>
        <br />
        <i:ButtonEx ID="btnApplyFilters" runat="server" Text="Apply Filters" Action="Submit"
            CausesValidation="true" />
        <i:ButtonEx ID="btnClear" runat="server" Text="Clear Filters" Action="Reset" CausesValidation="false" />
        <i:ValidationSummary runat="server" />
    </eclipse:TwoColumnPanel>
    <eclipse:AppliedFilters runat="server" ID="af" ContainerId="tcp" ClientIDMode="Static" />
    <phpa:PhpaLinqDataSource runat="server" ID="dsProbation" ContextTypeName="Eclipse.PhpaLibrary.Database.PIS.PISDataContext"
        TableName="Employees" OrderBy="ProbationEndDate" OnSelecting="dsProbation_Selecting"
        RenderLogVisible="false">
    </phpa:PhpaLinqDataSource>
    <jquery:GridViewEx runat="server" ID="gvProbation" DataKeyNames="EmployeeId" DataSourceID="dsProbation"
        AutoGenerateColumns="false" PageSize="50" Caption="Probation Completion of employees">
        <Columns>
            <asp:HyperLinkField DataTextField="FullName" HeaderText="Employee|Name" DataNavigateUrlFields="EmployeeId"
                DataNavigateUrlFormatString="~/PIS/EmployeeDetails.aspx?EmployeeId={0}" SortExpression="FullName" />
            <eclipse:MultiBoundField DataFields="JoiningDate" HeaderText="Joining Date" ItemStyle-HorizontalAlign="Right"
                DataFormatString="{0:d}" />
            <eclipse:MultiBoundField DataFields="ProbationEndDate" HeaderText="Probation End Date"
                ItemStyle-HorizontalAlign="Right" DataFormatString="{0:d}" />
            <eclipse:MultiBoundField DataFields="Designation" HeaderText="Employee|Designation" />
            <eclipse:MultiBoundField DataFields="EmployeeType.Description" HeaderText="Employee|Type" />
            <eclipse:MultiBoundField DataFields="Division.DivisionName" HeaderText="Division" />
            <eclipse:MultiBoundField DataFields="SubDivision.SubDivisionName" HeaderText="Sub Division" />
            <eclipse:MultiBoundField DataFields="Office.OfficeName" HeaderText="Office" />
            <eclipse:MultiBoundField DataFields="Remarks" HeaderText="Remarks" />
        </Columns>
    </jquery:GridViewEx>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cphNoForm" runat="server">
</asp:Content>
