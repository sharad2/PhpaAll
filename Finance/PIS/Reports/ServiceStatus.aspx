<%@ Page Title="Service Status Report" Language="C#" MasterPageFile="~/MasterPage.master"
    EnableViewState="false" CodeBehind="ServiceStatus.aspx.cs" Inherits="PIS.Reports.ServiceStatus" %>

<%@ Register Src="~/Controls/PrinterFriendlyButton.ascx" TagPrefix="uc1" TagName="PrinterFriendlyButton" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" runat="server" ContentPlaceHolderID="cphSideNavigation">
    <uc1:PrinterFriendlyButton runat="server" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cph" runat="server">
    <eclipse:TwoColumnPanel runat="server" ID="tcp" IsValidationContainer="true">
        <eclipse:LeftLabel runat="server" Text="Termination Reason" />
        <phpa:PhpaLinqDataSource runat="server" ID="dsEmployeeStatus" ContextTypeName="Eclipse.PhpaLibrary.Database.PIS.PISDataContext"
            Select="new (EmployeeStatusId, EmployeeStatusType)" TableName="EmployeeStatus"
            OrderBy="EmployeeStatusType" RenderLogVisible="false" />
        <i:DropDownListEx runat="server" ID="ddlEmployeeStatus" DataSourceID="dsEmployeeStatus"
            DataTextField="EmployeeStatusType" DataValueField="EmployeeStatusId" />
        <eclipse:LeftLabel runat="server" Text="Period Uptill" />
        <i:TextBoxEx runat="server" ID="dtFrom" FriendlyName="Period Uptill" Text="0">
            <Validators>
                <i:Date />
                <i:Required />
            </Validators>
        </i:TextBoxEx>
        <br />
        <i:ButtonEx ID="btnApplyFilters" runat="server" Text="Apply Filters" Action="Submit"
            CausesValidation="true" IsDefault="true"/>
        <i:ButtonEx ID="btnClear" runat="server" Text="Clear Filters" CausesValidation="false"
            Action="Reset" />
        <i:ValidationSummary runat="server" />
    </eclipse:TwoColumnPanel>
    <eclipse:AppliedFilters runat="server" ID="af" ContainerId="tcp" ClientIDMode="Static" />
    <phpa:PhpaLinqDataSource runat="server" ID="dsEmploymentStatus" ContextTypeName="Eclipse.PhpaLibrary.Database.PIS.PISDataContext"
        OnSelecting="dsEmploymentStatus_Selecting" RenderLogVisible="false" TableName="ServicePeriods">
        <WhereParameters>
            <asp:ControlParameter ControlID="dtFrom" Name="FromDate" Type="DateTime" />
            <asp:ControlParameter ControlID="ddlEmployeeStatus" Name="EmployeeStatusId" Type="Int32" />
        </WhereParameters>
    </phpa:PhpaLinqDataSource>
    <jquery:GridViewEx runat="server" ID="gvEmploymentStatus" DataSourceID="dsEmploymentStatus"
        AutoGenerateColumns="false" AllowSorting="true" EmptyDataText="No employees found"
        AllowPaging="true" PageSize="50">
        <Columns>
            <eclipse:SequenceField />
            <asp:HyperLinkField DataTextField="FullName" HeaderText="Employee|Name" DataNavigateUrlFields="EmployeeId"
                DataNavigateUrlFormatString="~/PIS/EmployeeDetails.aspx?EmployeeId={0}" SortExpression="FirstName" />
            <eclipse:MultiBoundField DataFields="Designation" HeaderText="Employee|Designation"
                SortExpression="Designation" />
            <eclipse:MultiBoundField DataFields="DivisionName" HeaderText="Employee|Division"
                SortExpression="DivisionName" />
            <eclipse:MultiBoundField DataFields="JoiningDate" HeaderText="Date Of |Joining" DataFormatString="{0:d}"
                ItemStyle-HorizontalAlign="Right" SortExpression="JoiningDate" />
            <eclipse:MultiBoundField DataFields="EmployeeStatusType" HeaderText="Termination Status"
                SortExpression="EmployeeStatusType" />
            <eclipse:MultiBoundField DataFields="DateOfRelieve" HeaderText="Date Of |Relieving"
                DataFormatString="{0:d}" ItemStyle-HorizontalAlign="Right" SortExpression="DateOfRelieve" />
            <eclipse:MultiBoundField DataFields="RelieveOrderNo" HeaderText="Relieving Order No"
                SortExpression="RelieveOrderNo" />
            <eclipse:MultiBoundField DataFields="LeavingReason" HeaderText="Reason for Leaving"
                AccessibleHeaderText="LeavingReason" SortExpression="LeavingReason" />
            <eclipse:MultiBoundField DataFields="Remarks" HeaderText="Remarks" SortExpression="Remarks" />
        </Columns>
    </jquery:GridViewEx>
</asp:Content>
