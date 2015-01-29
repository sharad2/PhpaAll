<%@ Page Title="Staff Divisions" Language="C#" MasterPageFile="~/MasterPage.master"
    CodeBehind="StaffDivisions.aspx.cs" Inherits="PhpaAll.PIS.Reports.StaffDivisions"
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
        <eclipse:LeftLabel runat="server" Text="Division" />
        <phpa:PhpaLinqDataSource runat="server" ID="dsDivision" ContextTypeName="Eclipse.PhpaLibrary.Database.PIS.PISDataContext"
            Select="new (DivisionId, DivisionName, DivisionGroup)" TableName="Divisions"
            RenderLogVisible="false" />
        <i:DropDownListEx runat="server" ID="ddlDivision" DataSourceID="dsDivision" DataTextField="DivisionName"
            DataValueField="DivisionId" FriendlyName="Division" DataOptionGroupField="DivisionGroup">
            <Items>
                <eclipse:DropDownItem Text="(All)" Value="" Persistent="Always" />
            </Items>
        </i:DropDownListEx>
        <eclipse:LeftLabel runat="server" Text="Uptill" />
        <i:TextBoxEx runat="server" ID="dtUptill" FriendlyName="Uptill Date" Text="0">
            <Validators>
                <i:Required />
                <i:Date />
            </Validators>
        </i:TextBoxEx>
        <br />
        <i:ButtonEx ID="btnApplyFilters" runat="server" Text="Apply Filters" Action="Submit"
            CausesValidation="true" IsDefault="true" />
        <i:ButtonEx ID="btnClear" runat="server" Text="Clear Filters" Action="Reset" CausesValidation="false" />
        <i:ValidationSummary runat="server" />
    </eclipse:TwoColumnPanel>
    <eclipse:AppliedFilters runat="server" ID="af" ContainerId="tcp" ClientIDMode="Static" />
    <phpa:PhpaLinqDataSource runat="server" ID="dsStaffDivisions" ContextTypeName="Eclipse.PhpaLibrary.Database.PIS.PISDataContext"
        TableName="Employees" OnSelecting="dsStaffDivisions_Selecting" RenderLogVisible="false">
    </phpa:PhpaLinqDataSource>
    <jquery:GridViewEx runat="server" ID="gvStaffDivisions" DataSourceID="dsStaffDivisions"
        AutoGenerateColumns="false" AllowPaging="true" PageSize="50" DefaultSortExpression="Division.DivisionName;
        SubDivision.SubDivisionName;Office.OfficeName;$;FirstName">
        <Columns>
            <eclipse:MultiBoundField DataFields="Division.DivisionName" HeaderText="Division"
                ShowHeader="true" SortExpression="Division.DivisionName" HeaderToolTip="Division associated with employee." />
            <eclipse:MultiBoundField DataFields="SubDivision.SubDivisionName" HeaderText="Sub Division"
                ShowHeader="true" SortExpression="SubDivision.SubDivisionName" HeaderToolTip="Sub Division associated with employee." />
            <eclipse:MultiBoundField DataFields="Office.OfficeName" HeaderText="Office" ShowHeader="true"
                SortExpression="Office.OfficeName" HeaderToolTip="Office associated with employee." />
            <eclipse:MultiBoundField DataFields="FullName" HeaderText="Employee|FullName" />
            <eclipse:MultiBoundField DataFields="Designation" HeaderText="Employee|Designation" />
            <eclipse:MultiBoundField DataFields="EmployeeType.Description" HeaderText="Employee|Type" />
            <eclipse:MultiBoundField DataFields="JoiningDate" HeaderText="Joining Date" ItemStyle-HorizontalAlign="Right" />
            <eclipse:MultiBoundField DataFields="Remarks" HeaderText="Remarks" />
        </Columns>
    </jquery:GridViewEx>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cphNoForm" runat="server">
</asp:Content>
