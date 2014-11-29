<%@ Page Title="Promotion Report" Language="C#" MasterPageFile="~/MasterPage.master"
    CodeBehind="Promotion.aspx.cs" Inherits="PIS.Reports.Promotion" EnableViewState="false" %>

<%@ Register Src="~/Controls/PrinterFriendlyButton.ascx" TagPrefix="uc1" TagName="PrinterFriendlyButton" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" runat="server" ContentPlaceHolderID="cphSideNavigation">
    <uc1:PrinterFriendlyButton runat="server" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cph" runat="server">
    <eclipse:TwoColumnPanel runat="server" ID="tcp" IsValidationContainer="true">
        <eclipse:LeftLabel runat="server" Text="Period Range" />
        <i:TextBoxEx runat="server" ID="dtPeriodFrom" FriendlyName="Period From" Text="-1095">
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
        <i:ButtonEx ID="btnApplyFilters" runat="server" Text="Apply Filters" Action="Submit"
            CausesValidation="true" />
        <i:ButtonEx ID="btnClear" runat="server" Text="Clear Filters" Action="Reset" CausesValidation="false" />
        <i:ValidationSummary runat="server" />
    </eclipse:TwoColumnPanel>
    <eclipse:AppliedFilters runat="server" ID="af" ContainerId="tcp" ClientIDMode="Static" />
    <phpa:PhpaLinqDataSource runat="server" ID="dsPromotion" ContextTypeName="Eclipse.PhpaLibrary.Database.PIS.PISDataContext"
        TableName="ServicePeriods" OnSelecting="dsPromotion_Selecting" RenderLogVisible="false">
        <WhereParameters>
            <asp:ControlParameter ControlID="dtPeriodFrom" Name="FromDate" Type="DateTime" />
            <asp:ControlParameter ControlID="dtPeriodTo" Name="ToDate" Type="DateTime" />
        </WhereParameters>
    </phpa:PhpaLinqDataSource>
    <jquery:GridViewEx runat="server" ID="gvPromotion" DataSourceID="dsPromotion" AllowPaging="true"
        PageSize="50" AutoGenerateColumns="false" EmptyDataText="No employee found" OnRowDataBound="gvPromotion_RowDataBound"
        Caption="Promotion Details Information" AllowSorting="true">
        <Columns>
            <eclipse:SequenceField />
            <asp:HyperLinkField DataTextField="FullName" HeaderText="Employee|Name" DataNavigateUrlFields="EmployeeId"
                DataNavigateUrlFormatString="~/PIS/EmployeeDetails.aspx?EmployeeId={0}" SortExpression="FirstName" />
            <eclipse:MultiBoundField DataFields="Designation" HeaderText="Employee|Designation"
                SortExpression="Designation" />
            <eclipse:MultiBoundField DataFields="Grade" HeaderText="Employee|Grade" SortExpression="Grade">
                <ItemStyle HorizontalAlign="Right" />
            </eclipse:MultiBoundField>
            <eclipse:MultiBoundField DataFields="JoiningDate" HeaderText="Date Of|Joining" ItemStyle-HorizontalAlign="Right"
                DataFormatString="{0:d}" SortExpression="JoiningDate" />
            <eclipse:MultiBoundField DataFields="PromotionDate" HeaderText="Date Of|Promotion"
                ItemStyle-HorizontalAlign="Right" DataFormatString="{0:d}" SortExpression="PromotionDate" />
            <eclipse:MultiBoundField DataFields="ServiceAfterPromotion" HeaderText="No of years of service from the date of|last promotion"
                ItemStyle-HorizontalAlign="Right" AccessibleHeaderText="ServiceAfterPromotion"
                SortExpression="ServiceAfterPromotion" />
            <eclipse:MultiBoundField DataFields="ContinuousService" HeaderText="No of years of service from the date of|appointment"
                ItemStyle-HorizontalAlign="Right" AccessibleHeaderText="ContinuousService" SortExpression="ContinuousService" />
            <eclipse:MultiBoundField DataFields="Remarks" HeaderText="Remarks" SortExpression="Remarks" />
        </Columns>
    </jquery:GridViewEx>
</asp:Content>
