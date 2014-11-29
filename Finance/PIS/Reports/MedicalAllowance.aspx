<%@ Page Title="Medical Allowances Report" Language="C#" MasterPageFile="~/MasterPage.master"
    CodeBehind="MedicalAllowance.aspx.cs" Inherits="PIS.Reports.MedicalAllowance"
    EnableViewState="false" %>

<%@ Register Src="~/Controls/PrinterFriendlyButton.ascx" TagPrefix="uc1" TagName="PrinterFriendlyButton" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" runat="server" ContentPlaceHolderID="cphSideNavigation">
    <uc1:PrinterFriendlyButton runat="server" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cph" runat="server">
    <eclipse:TwoColumnPanel runat="server" ID="tcp" IsValidationContainer="true">
        <eclipse:LeftLabel runat="server" Text="Hospital Country" />
        <phpa:PhpaLinqDataSource runat="server" ID="dsCountry" ContextTypeName="Eclipse.PhpaLibrary.Database.PIS.PISDataContext"
            RenderLogVisible="false" EnableViewState="false" OrderBy="CountryName" Select="new (CountryId, CountryName)"
            TableName="Countries" />
        <i:DropDownListEx runat="server" ID="ddlCountry" DataSourceID="dsCountry" DataTextField="CountryName"
            DataValueField="CountryId">
            <Items>
                <eclipse:DropDownItem Text="(All)" Value="" Persistent="Always" />
            </Items>
        </i:DropDownListEx>
        <eclipse:LeftLabel runat="server" Text="Period From / To" />
        <i:TextBoxEx runat="server" ID="dtFrom" FriendlyName="Period From" Text="-365">
            <Validators>
                <i:Date />
            </Validators>
        </i:TextBoxEx>
        <i:TextBoxEx runat="server" ID="dtTo" FriendlyName="Period To" Text="0">
            <Validators>
                <i:Date AssociatedControlID="dtFrom" />
            </Validators>
        </i:TextBoxEx>
        <br />
        <i:ButtonEx ID="btnApplyFilters" runat="server" Text="Apply Filters" Action="Submit"
            CausesValidation="true" />
        <i:ButtonEx ID="btnClear" runat="server" Text="Clear Filters" CausesValidation="false"
            Action="Reset" />
        <i:ValidationSummary runat="server" />
    </eclipse:TwoColumnPanel>
    <eclipse:AppliedFilters runat="server" ID="af" ContainerId="tcp" ClientIDMode="Static" />
    <phpa:PhpaLinqDataSource runat="server" ID="dsMedAllowance" ContextTypeName="Eclipse.PhpaLibrary.Database.PIS.PISDataContext"
        OnSelecting="dsMedAllowance_Selecting" TableName="MedicalRecords" RenderLogVisible="false">
        <WhereParameters>
            <asp:ControlParameter ControlID="dtFrom" Name="FromDate" Type="DateTime" />
            <asp:ControlParameter ControlID="dtTo" Name="ToDate" Type="DateTime" />
        </WhereParameters>
    </phpa:PhpaLinqDataSource>
    <jquery:GridViewEx runat="server" ID="gvMedAllowance" DataSourceID="dsMedAllowance"
        AutoGenerateColumns="false" EmptyDataText="No information found" Caption="Details of Medical Allowance granted"
        AllowSorting="true" AllowPaging="true" PageSize="50">
        <Columns>
            <eclipse:SequenceField />
            <asp:HyperLinkField DataTextField="FullName" HeaderText="Name" DataNavigateUrlFields="EmployeeId"
                DataNavigateUrlFormatString="~/PIS/EmployeeDetails.aspx?EmployeeId={0}" SortExpression="FirstName" />
            <eclipse:MultiBoundField DataFields="Designation" HeaderText="Designation" SortExpression="Designation" />
            <eclipse:MultiBoundField DataFields="Grade" HeaderText="Grade" SortExpression="Grade">
                <ItemStyle HorizontalAlign="Right" />
            </eclipse:MultiBoundField>
            <eclipse:MultiBoundField DataFields="EmpStatus" HeaderText="Service Status" SortExpression="EmpStatus" />
            <eclipse:MultiBoundField DataFields="PatientName" HeaderText="Patient" SortExpression="PatientName" />
            <eclipse:MultiBoundField DataFields="Relationship" HeaderText="Relationship" SortExpression="Relationship" />
            <eclipse:MultiBoundField DataFields="RefNo" HeaderText="Referral No" SortExpression="RefNo" />
            <eclipse:MultiBoundField DataFields="HospitalName" HeaderText="Hospital|Name" SortExpression="HospitalName" />
            <eclipse:MultiBoundField DataFields="HospitalAddr" HeaderText="Hospital|Address"
                SortExpression="HospitalAddr" />
            <eclipse:MultiBoundField DataFields="HospitalIn" HeaderText="Hospital|Country" SortExpression="HospitalIn"
                AccessibleHeaderText="HospitalIn" />
            <eclipse:MultiBoundField DataFields="Amount" HeaderText="Medical Advance|Amount"
                ItemStyle-HorizontalAlign="Right" DataFormatString="{0:N2}" SortExpression="Amount" />
            <eclipse:MultiBoundField DataFields="OrderDate" HeaderText="Office Order Date" DataFormatString="{0:d}"
                ItemStyle-HorizontalAlign="Right" SortExpression="OrderDate" HeaderStyle-Wrap="true" />
            <eclipse:MultiBoundField DataFields="AdvanceAdjusted" HeaderText="Adjusted|Advance"
                ItemStyle-HorizontalAlign="Right" DataFormatString="{0:N2}" SortExpression="AdvanceAdjusted" />
            <eclipse:MultiBoundField DataFields="PendingAdjustment" HeaderText="Adjusted|Pending"
                ItemStyle-HorizontalAlign="Right" DataFormatString="{0:N2}" SortExpression="PendingAdjustment" />
            <eclipse:MultiBoundField DataFields="OffOrderNo" HeaderText="Medical Advance|Vide Office Order No"
                SortExpression="OffOrderNo" HeaderStyle-Wrap="true" />
            <eclipse:MultiBoundField DataFields="Balance" HeaderText="Balance for Deduction/payable after adjustment"
                ItemStyle-HorizontalAlign="Right" DataFormatString="{0:N2}" SortExpression="Balance"
                HeaderStyle-Wrap="true" />
            <eclipse:MultiBoundField DataFields="Remarks" HeaderText="Remarks" SortExpression="Remarks" />
        </Columns>
    </jquery:GridViewEx>
</asp:Content>
