<%@ Page Title="HRD Training Report" Language="C#" MasterPageFile="~/MasterPage.master"
    CodeBehind="Trainings.aspx.cs" Inherits="PIS.Reports.Trainings" EnableViewState="false" %>

<%@ Register Src="~/Controls/PrinterFriendlyButton.ascx" TagPrefix="uc1" TagName="PrinterFriendlyButton" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" runat="server" ContentPlaceHolderID="cphSideNavigation">
    <uc1:PrinterFriendlyButton runat="server" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cph" runat="server">
    <eclipse:TwoColumnPanel runat="server" ID="tcp" IsValidationContainer="true">
        <eclipse:LeftLabel runat="server" Text="Institute Country" />
        <phpa:PhpaLinqDataSource runat="server" ID="dsCountry" ContextTypeName="Eclipse.PhpaLibrary.Database.PIS.PISDataContext"
            RenderLogVisible="false" OrderBy="CountryName" Select="new (CountryId, CountryName)"
            TableName="Countries" />
        <i:DropDownListEx runat="server" ID="ddlCountry" DataSourceID="dsCountry" DataValueField="CountryId"
            DataTextField="CountryName">
            <Items>
                <eclipse:DropDownItem Text="(All)" Value="" Persistent="Always" />
            </Items>
        </i:DropDownListEx>
        <eclipse:LeftLabel runat="server" Text="Training Type" />
        <i:DropDownListEx runat="server" ID="ddlTraining" DataSourceID="dsTrainingTypes"
            DataTextField="TrainingDescription" DataValueField="TrainingTypeId">
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
                <i:Date AssociatedControlID="dtPeriodFrom" />
            </Validators>
        </i:TextBoxEx>
        <br />
        <i:ButtonEx ID="btnApplyFilters" runat="server" Text="Apply Filters" Action="Submit" CausesValidation="true" IsDefault="true"/>
        <i:ButtonEx ID="btnClear" runat="server" Text="Clear Filters" CausesValidation="false"
            Action="Reset" />
        <i:ValidationSummary runat="server" />
    </eclipse:TwoColumnPanel>
    <eclipse:AppliedFilters runat="server" ID="af" ContainerId="tcp" ClientIDMode="Static" />
    <phpa:PhpaLinqDataSource runat="server" ID="dsTrainingTypes" ContextTypeName="Eclipse.PhpaLibrary.Database.PIS.PISDataContext"
        RenderLogVisible="False" Select="new (TrainingTypeId, TrainingDescription)" OrderBy="TrainingDescription"
        TableName="TrainingTypes" />
    <phpa:PhpaLinqDataSource runat="server" ID="dsTraining" ContextTypeName="Eclipse.PhpaLibrary.Database.PIS.PISDataContext"
        RenderLogVisible="False" TableName="Trainings" OnSelecting="dsTraining_Selecting">
        <WhereParameters>
            <asp:ControlParameter ControlID="dtPeriodFrom" Type="DateTime" Name="FromDate" />
            <asp:ControlParameter ControlID="dtPeriodTo" Type="DateTime" Name="ToDate" />
        </WhereParameters>
    </phpa:PhpaLinqDataSource>
    <jquery:GridViewEx runat="server" ID="gvTraining" DataSourceID="dsTraining" AutoGenerateColumns="false"
        AllowSorting="true" EmptyDataText="No employees found" AllowPaging="true" PageSize="50"
        Caption="Training Details Report">
        <Columns>
            <eclipse:SequenceField />
            <asp:HyperLinkField DataTextField="FullName" HeaderText="Employee|Name" DataNavigateUrlFields="EmployeeId"
                DataNavigateUrlFormatString="~/PIS/EmployeeDetails.aspx?EmployeeId={0}" SortExpression="FirstName" />
            <eclipse:MultiBoundField DataFields="Designation" HeaderText="Employee|Designation"
                SortExpression="Designation" />
            <eclipse:MultiBoundField DataFields="InstituteName" HeaderText="Institute|Name" SortExpression="InstituteName" />
            <eclipse:MultiBoundField DataFields="InstituteAddress" HeaderText="Institute|Address"
                SortExpression="InstituteAddress" />
            <eclipse:MultiBoundField DataFields="CountryName" HeaderText="Institute|Country"
                SortExpression="CountryName" AccessibleHeaderText="CountryName" />
            <eclipse:MultiBoundField DataFields="CourseLevel" HeaderText="Course" SortExpression="CourseLevel" />
            <eclipse:MultiBoundField DataFields="Subject" HeaderText="Subject of Study" SortExpression="Subject" />
            <eclipse:MultiBoundField DataFields="TrainingDescription" HeaderText="Training|Type"
                SortExpression="TrainingDescription" />
            <eclipse:MultiBoundField DataFields="TrainingStartFrom" HeaderText="Training|Started"
                ItemStyle-HorizontalAlign="Right" DataFormatString="{0:d}" SortExpression="TrainingStartFrom" />
            <eclipse:MultiBoundField DataFields="TrainingEndTo" HeaderText="Training|Ended" ItemStyle-HorizontalAlign="Right"
                DataFormatString="{0:d}" SortExpression="TrainingEndTo" />
            <eclipse:MultiBoundField DataFields="GovtApprovalNo" HeaderText="Reference No" SortExpression="GovtApprovalNo" />
            <eclipse:MultiBoundField DataFields="Remarks" HeaderText="Remarks" SortExpression="Remarks" />
        </Columns>
    </jquery:GridViewEx>
</asp:Content>
