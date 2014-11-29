<%@ Page Title="Personnel Information Report" Language="C#" MasterPageFile="~/MasterPage.master"
    CodeBehind="PersonnelInfo.aspx.cs" EnableViewState="false" Inherits="PIS.Reports.PersonnelInfo" %>

<%@ Register Src="~/Controls/PrinterFriendlyButton.ascx" TagPrefix="uc1" TagName="PrinterFriendlyButton" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" runat="server" ContentPlaceHolderID="cphSideNavigation">
    <uc1:PrinterFriendlyButton runat="server" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cph" runat="server">
    <eclipse:TwoColumnPanel runat="server" ID="tcp" IsValidationContainer="true">
        <eclipse:LeftLabel runat="server" Text="Employee Type" />
        <phpa:PhpaLinqDataSource runat="server" ID="dsEmployeeType" ContextTypeName="Eclipse.PhpaLibrary.Database.PIS.PISDataContext"
            OrderBy="Description" Select="new (EmployeeTypeId, Description)" TableName="EmployeeTypes"
            RenderLogVisible="False" />
        <i:DropDownListEx ID="ddlEmployeeType" runat="server" DataSourceID="dsEmployeeType"
            DataTextField="Description" DataValueField="EmployeeTypeId" FriendlyName="Employee Type"
            QueryString="EmployeeTypeId">
            <Items>
                <eclipse:DropDownItem Text="(All)" Value="" Persistent="Always" />
                <eclipse:DropDownItem Text="(No Type)" Value="-1" Persistent="Always" />
            </Items>
        </i:DropDownListEx>
        <eclipse:LeftLabel runat="server" Text="Termination Status" />
        <phpa:PhpaLinqDataSource runat="server" ID="dsEmployeeSatus" ContextTypeName="Eclipse.PhpaLibrary.Database.PIS.PISDataContext"
            OrderBy="EmployeeStatusType" TableName="EmployeeStatus" RenderLogVisible="false" />
        <i:DropDownListEx runat="server" ID="ddlEmployeeStatus" DataSourceID="dsEmployeeSatus"
            DataTextField="EmployeeStatusType" DataValueField="EmployeeStatusId">
            <Items>
                <eclipse:DropDownItem Text="(Not terminated)" Value="" Persistent="Always" />
            </Items>
        </i:DropDownListEx>
        <eclipse:LeftLabel runat="server" Text="Grade" />
        <phpa:PhpaLinqDataSource runat="server" ID="dsGrade" ContextTypeName="Eclipse.PhpaLibrary.Database.PIS.PISDataContext"
            OnSelecting="dsGrade_Selecting" RenderLogVisible="false">
        </phpa:PhpaLinqDataSource>
        <i:DropDownListEx runat="server" ID="ddlGrade" FriendlyName="Grade" DataSourceID="dsGrade"
            QueryString="Grade">
            <Items>
                <eclipse:DropDownItem Text="(All)" Value="" Persistent="Always" />
                <eclipse:DropDownItem Text="(No Grade)" Value="-1" Persistent="Always" />
            </Items>
        </i:DropDownListEx>
        <eclipse:LeftLabel runat="server" Text="Division" />
        <phpa:PhpaLinqDataSource runat="server" ID="dsDivision" ContextTypeName="Eclipse.PhpaLibrary.Database.PIS.PISDataContext"
            OrderBy="DivisionName" Select="new (DivisionId,DivisionName, DivisionGroup)" TableName="Divisions"
            RenderLogVisible="false" />
        <i:DropDownListEx runat="server" ID="ddlDivision" DataSourceID="dsDivision" DataTextField="DivisionName"
            DataValueField="DivisionId" QueryString="DivisionId" DataOptionGroupField="DivisionGroup">
            <Items>
                <eclipse:DropDownItem Text="(All)" Value="" Persistent="Always" />
            </Items>
        </i:DropDownListEx>
        <br />
        <i:ButtonEx ID="btnApplyFilters" runat="server" Text="Apply Filters" Action="Submit"
            CausesValidation="true" IsDefault="true" />
        <i:ButtonEx ID="btnClear" runat="server" Text="Clear Filters" Action="Reset" CausesValidation="false" />
        <i:ValidationSummary runat="server" />
    </eclipse:TwoColumnPanel>
    <eclipse:AppliedFilters runat="server" ID="af" ContainerId="tcp" ClientIDMode="Static" />
    <phpa:PhpaLinqDataSource runat="server" ID="dsEmployee" ContextTypeName="Eclipse.PhpaLibrary.Database.PIS.PISDataContext"
        TableName="ServicePeriods" RenderLogVisible="False" OnSelecting="dsEmployee_Selecting">
    </phpa:PhpaLinqDataSource>
    <jquery:GridViewEx runat="server" ID="gvEmpType" DataSourceID="dsEmployee" AutoGenerateColumns="False"
        AllowPaging="true" PageSize="50" EmptyDataText="No Employees found" AllowSorting="true"
        Caption="Personnel Information Details">
        <Columns>
            <eclipse:SequenceField />
            <asp:HyperLinkField DataTextField="FullName" HeaderText="Name" DataNavigateUrlFields="EmployeeId"
                DataNavigateUrlFormatString="~/PIS/EmployeeDetails.aspx?EmployeeId={0}" SortExpression="FirstName" />
            <eclipse:MultiBoundField DataFields="EmployeeType" HeaderText="Type" AccessibleHeaderText="EmployeeType"
                SortExpression="EmployeeType" />
            <eclipse:MultiBoundField DataFields="Designation" HeaderText="Designation" AccessibleHeaderText="Designation"
                SortExpression="Designation" />
            <eclipse:MultiBoundField DataFields="ParentOrganization" HeaderText="Parent Organization"
                AccessibleHeaderText="ParentOrganization" SortExpression="ParentOrganization" />
            <eclipse:MultiBoundField DataFields="Qualification" HeaderText="Qualification" AccessibleHeaderText="Qualification"
                SortExpression="Qualification" />
            <eclipse:MultiBoundField DataFields="Nationality" HeaderText="Nationality" AccessibleHeaderText="Nationality"
                SortExpression="Nationality" />
            <eclipse:MultiBoundField DataFields="ConsolidatedSalary" HeaderText="Consolidated Salary"
                SortExpression="ConsolidatedSalary" AccessibleHeaderText="ConsolidatedSalary"
                ItemStyle-HorizontalAlign="Right" DataFormatString="{0:N2}" HeaderToolTip="An amount is displayed only if the salary is marked as consolidated" />
            <eclipse:MultiBoundField DataFields="Grade" HeaderText="Grade" AccessibleHeaderText="Grade"
                SortExpression="Grade">
                <ItemStyle HorizontalAlign="Right" />
            </eclipse:MultiBoundField>
            <eclipse:MultiBoundField DataFields="InitialTerm" HeaderText="Initial Term" AccessibleHeaderText="InitialTerm"
                SortExpression="InitialTerm" />
            <eclipse:MultiBoundField DataFields="DivisionName" HeaderText="Office Circle Served"
                SortExpression="DivisionName" AccessibleHeaderText="DivisionName" />
            <eclipse:MultiBoundField DataFields="DateOfBirth" HeaderText="Birth Date" DataFormatString="{0:d}"
                ItemStyle-HorizontalAlign="Right" AccessibleHeaderText="DateOfBirth" SortExpression="DateOfBirth" />
            <eclipse:MultiBoundField DataFields="JoiningDate" HeaderText="Joined" DataFormatString="{0:d}"
                ItemStyle-HorizontalAlign="Right" AccessibleHeaderText="JoiningDate" SortExpression="JoiningDate" />
            <eclipse:MultiBoundField DataFields="DateOfIncrement" HeaderText="Increment" DataFormatString="{0:d}"
                ItemStyle-HorizontalAlign="Right" AccessibleHeaderText="DateOfIncrement" SortExpression="DateOfIncrement"
                HeaderToolTip="Date on which the next increment is due" />
            <eclipse:MultiBoundField DataFields="DateOfRelieve" HeaderText="Date of Releiving" DataFormatString="{0:d}"
                ItemStyle-HorizontalAlign="Right" AccessibleHeaderText="DateOfRelieve" SortExpression="DateOfRelieve" />
           <%-- <eclipse:MultiBoundField DataFields="ExpiryDate" HeaderText="Expired" DataFormatString="{0:d}"
                ItemStyle-HorizontalAlign="Right" AccessibleHeaderText="ExpiryDate" SortExpression="ExpiryDate"
                HeaderToolTip="The date on which service will expire unless it is extended" />--%>
            <eclipse:MultiBoundField DataFields="PromotionDate" HeaderText="Promotion Date" SortExpression="PromotionDate"
            DataFormatString="{0:d}" ItemStyle-HorizontalAlign="Right" />
            <eclipse:MultiBoundField DataFields="Extension" HeaderText="Extension" AccessibleHeaderText="Extension"
                SortExpression="Extension" HeaderToolTip="The type of promotion received" />
            <eclipse:MultiBoundField DataFields="ExtensionUpto" HeaderText="Extension Upto" DataFormatString="{0:d}"
                ItemStyle-HorizontalAlign="Right" AccessibleHeaderText="ExtensionUpto" SortExpression="ExtensionUpto" />
            <eclipse:MultiBoundField DataFields="Remarks" HeaderText="Remarks" AccessibleHeaderText="Remarks"
                SortExpression="Remarks" />
        </Columns>
    </jquery:GridViewEx>
</asp:Content>
