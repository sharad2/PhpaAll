<%@ Page Title="Contract Completion" Language="C#" MasterPageFile="~/MasterPage.master"
    CodeBehind="ContractEnding.aspx.cs" Inherits="PhpaAll.PIS.Reports.ContractEnding"
    EnableViewState="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphSideNavigation" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cph" runat="server">
    <eclipse:TwoColumnPanel runat="server" ID="tcp" IsValidationContainer="true">
        <eclipse:LeftLabel runat="server" Text="Employee Type" />
        <phpa:PhpaLinqDataSource runat="server" ID="dsEmployeeType" ContextTypeName="Eclipse.PhpaLibrary.Database.PIS.PISDataContext"
            Select="new (EmployeeTypeId, Description)" OrderBy="Description" TableName="EmployeeTypes"
            RenderLogVisible="false">
        </phpa:PhpaLinqDataSource>
        <i:DropDownListEx ID="ddlEmployeeType" runat="server" DataSourceID="dsEmployeeType"
            QueryString="EmployeeTypeId" DataTextField="Description" DataValueField="EmployeeTypeId"
            FriendlyName="Employee Type">
            <Items>
                <eclipse:DropDownItem Text="(All)" Value="" Persistent="Always" />
            </Items>
        </i:DropDownListEx>
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
    <phpa:PhpaLinqDataSource runat="server" ID="dsContractEnding" ContextTypeName="Eclipse.PhpaLibrary.Database.PIS.PISDataContext"
        TableName="Employees" OnSelecting="dsContractEnding_Selecting" RenderLogVisible="false"
        OrderBy="PeriodEndDate">
    </phpa:PhpaLinqDataSource>
    <jquery:GridViewEx runat="server" ID="gvContractEnding" DataSourceID="dsContractEnding"
        AutoGenerateColumns="false" PageSize="50" Caption="Contract completion of employees">
        <Columns>
            <asp:HyperLinkField DataTextField="FullName" HeaderText="Employee|Name" DataNavigateUrlFields="EmployeeId"
                DataNavigateUrlFormatString="~/PIS/EmployeeDetails.aspx?EmployeeId={0}" SortExpression="FullName" />
            <eclipse:MultiBoundField DataFields="PeriodEndDate" HeaderText="Contract End Date"
                ItemStyle-HorizontalAlign="Right" DataFormatString="{0:d}" />
            <eclipse:MultiBoundField DataFields="JoiningDate" HeaderText="Joining Date" ItemStyle-HorizontalAlign="Right"
                DataFormatString="{0:d}" />
            <eclipse:MultiBoundField DataFields="Designation" HeaderText="Employee|Designation" />
            <eclipse:MultiBoundField DataFields="Description" HeaderText="Employee|Type" />
            <eclipse:MultiBoundField DataFields="DivisionName" HeaderText="Division" />
            <eclipse:MultiBoundField DataFields="SubDivisionName" HeaderText="Sub Division" />
            <eclipse:MultiBoundField DataFields="OfficeName" HeaderText="Office" />
        </Columns>
    </jquery:GridViewEx>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cphNoForm" runat="server">
</asp:Content>
