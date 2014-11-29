<%@ Page Title="Next Promotion Due" Language="C#" MasterPageFile="~/MasterPage.master"
    CodeBehind="NextPromotionDue.aspx.cs" Inherits="Finance.PIS.Reports.NextPromotionDue"
    EnableViewState="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cph" runat="server">
    <eclipse:TwoColumnPanel runat="server" ID="tcp" IsValidationContainer="true">
        <eclipse:LeftLabel ID="LeftLabel1" runat="server" Text="Date Range" />
        <i:TextBoxEx runat="server" ID="dtFrom" FriendlyName="From Date">
            <Validators>
                <i:Required />
                <i:Date />
            </Validators>
        </i:TextBoxEx>
        <i:TextBoxEx runat="server" ID="dtTo" FriendlyName="To Date">
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
    <eclipse:AppliedFilters ID="af" runat="server" ContainerId="tcp" ClientIDMode="Static" />
    <phpa:PhpaLinqDataSource runat="server" ID="ds" ContextTypeName="Eclipse.PhpaLibrary.Database.PIS.PISDataContext"
        TableName="Employees" OrderBy="NextPromotionDate" OnSelecting="ds_Selecting"
        RenderLogVisible="false">
    </phpa:PhpaLinqDataSource>
    <jquery:GridViewEx runat="server" ID="gv" DataSourceID="ds" AutoGenerateColumns="false"
        PageSize="50" Caption="Next Promotion Due">
        <Columns>
            <asp:HyperLinkField DataTextField="FullName" HeaderText="Employee|Name" DataNavigateUrlFields="EmployeeId"
                DataNavigateUrlFormatString="~/PIS/EmployeeDetails.aspx?EmployeeId={0}" SortExpression="FullName" />
            <eclipse:MultiBoundField DataFields="NextPromotionDate" HeaderText="Next Promotion Date"
                ItemStyle-HorizontalAlign="Right" DataFormatString="{0:d}" />
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
