<%@ Page Title="Leave Report" Language="C#" MasterPageFile="~/MasterPage.master"
    CodeBehind="Leaves.aspx.cs" Inherits="PIS.Reports.Leaves" EnableViewState="false" %>

<%@ Register Src="~/Controls/PrinterFriendlyButton.ascx" TagName="PrinterFriendlyButton"
    TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphSideNavigation" runat="server">
    <uc1:PrinterFriendlyButton runat="server" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cph" runat="server">
    <eclipse:TwoColumnPanel runat="server" ID="tcp" IsValidationContainer="true">
        <eclipse:LeftLabel runat="server" Text="Leave Type" />
        <phpa:PhpaLinqDataSource runat="server" ID="dsLeaveType" ContextTypeName="Eclipse.PhpaLibrary.Database.PIS.PISDataContext"
            Select="new (LeaveTypeId, LeaveDescription)" TableName="LeaveTypes" OrderBy="LeaveDescription"
            RenderLogVisible="false" />
        <i:DropDownListEx runat="server" ID="ddlLeaveType" DataTextField="LeaveDescription"
            FriendlyName="Leave Type" DataValueField="LeaveTypeId" DataSourceID="dsLeaveType">
            <Items>
                <eclipse:DropDownItem Text="(All)" Value="" Persistent="Always" />
            </Items>
        </i:DropDownListEx>
        <eclipse:LeftLabel runat="server" Text="Period Range" />
        <i:TextBoxEx runat="server" ID="dtPeriodFrom" FriendlyName="Period From" Text="-365">
            <Validators>
                <i:Date />
            </Validators>
        </i:TextBoxEx>
        <i:TextBoxEx runat="server" ID="dtPeriodTo" FriendlyName="Period To" Text="0">
            <Validators>
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
    <phpa:PhpaLinqDataSource runat="server" ID="dsEmpLeave" ContextTypeName="Eclipse.PhpaLibrary.Database.PIS.PISDataContext"
        TableName="LeaveRecords" OnSelecting="dsEmpLeave_Selecting" RenderLogVisible="false">
        <WhereParameters>
            <asp:ControlParameter ControlID="dtPeriodFrom" Name="FromDate" Type="DateTime" />
            <asp:ControlParameter ControlID="dtPeriodTo" Name="ToDate" Type="DateTime" />
        </WhereParameters>
    </phpa:PhpaLinqDataSource>
    <jquery:GridViewEx runat="server" ID="gvEmpLeave" DataSourceID="dsEmpLeave" AutoGenerateColumns="false"
        AllowSorting="true" EmptyDataText="No leave information found" AllowPaging="true"
        PageSize="50" Caption="Leave Information Details">
        <Columns>
            <eclipse:SequenceField />
            <asp:HyperLinkField DataTextField="FullName" HeaderText="Employee|Name" DataNavigateUrlFields="EmployeeId"
                DataNavigateUrlFormatString="~/PIS/EmployeeDetails.aspx?EmployeeId={0}" SortExpression="FullName" />
            <eclipse:MultiBoundField DataFields="Designation" HeaderText="Employee|Designation"
                SortExpression="Designation" />
            <eclipse:MultiBoundField DataFields="Grade" HeaderText="Employee|Grade" SortExpression="Grade">
                <ItemStyle HorizontalAlign="Right" />
            </eclipse:MultiBoundField>
            <eclipse:MultiBoundField DataFields="JoiningDate" HeaderText="Joined" DataFormatString="{0:d}"
                ItemStyle-HorizontalAlign="Right" SortExpression="JoiningDate" />
            <eclipse:MultiBoundField DataFields="LeaveDescription" HeaderText="Leave|Type" SortExpression="LeaveDescription"
                AccessibleHeaderText="LeaveDescription" />
            <eclipse:MultiBoundField DataFields="LeaveStartFrom" HeaderText="Leave|Started" DataFormatString="{0:d}"
                ItemStyle-HorizontalAlign="Right" SortExpression="LeaveStartFrom" />
            <eclipse:MultiBoundField DataFields="LeaveEndTo" HeaderText="Leave|Ended" DataFormatString="{0:d}"
                ItemStyle-HorizontalAlign="Right" SortExpression="LeaveEndTo" />
            <eclipse:MultiBoundField DataFields="NoOfLeaves" HeaderText="Leave|No of Days" ItemStyle-HorizontalAlign="Right"
                SortExpression="NoOfLeaves" />
            <eclipse:MultiBoundField DataFields="Remarks" HeaderText="Leave|Remarks" SortExpression="Remarks" />
        </Columns>
    </jquery:GridViewEx>
</asp:Content>
