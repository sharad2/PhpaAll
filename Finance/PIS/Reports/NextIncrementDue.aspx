<%@ Page Title="Increment Due" Language="C#" MasterPageFile="~/MasterPage.master"
    CodeBehind="NextIncrementDue.aspx.cs" Inherits="Finance.PIS.Reports.NextIncrementDue"
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
        <i:ButtonEx runat="server" Text="Clear Filters" Action="Reset" CausesValidation="false" />
        <i:ValidationSummary runat="server" />
    </eclipse:TwoColumnPanel>
    <eclipse:AppliedFilters ID="af" runat="server" ContainerId="tcp" ClientIDMode="Static" />
    <phpa:PhpaLinqDataSource runat="server" ID="ds" ContextTypeName="Eclipse.PhpaLibrary.Database.PIS.PISDataContext"
        TableName="Employees" OnSelecting="ds_Selecting" RenderLogVisible="false">
    </phpa:PhpaLinqDataSource>
    <jquery:GridViewEx runat="server" ID="gv" DataSourceID="ds" AutoGenerateColumns="false"
        PageSize="50" Caption="Next Increment Due">
        <Columns>
        <eclipse:SequenceField />
            <asp:HyperLinkField DataTextField="FullName" HeaderText="Name" DataNavigateUrlFields="EmployeeId"
                DataNavigateUrlFormatString="~/PIS/EmployeeDetails.aspx?EmployeeId={0}" SortExpression="FullName" />
            <eclipse:MultiBoundField DataFields="DateOfNextIncrement" HeaderText="Increment Date"
                ItemStyle-HorizontalAlign="Right" DataFormatString="{0:d}" />
            <eclipse:MultiBoundField DataFields="Designation" HeaderText="Designation" />
            <%--<eclipse:MultiBoundField DataFields="Description" HeaderText="Employee|Type" />--%>
            <eclipse:MultiBoundField DataFields="DivisionName" HeaderText="Division" >
                <ItemStyle Width="15em" />
                <HeaderStyle Width="15em" />
            </eclipse:MultiBoundField>
            <%--<eclipse:MultiBoundField DataFields="SubDivisionName" HeaderText="Sub Division" />--%>
            <eclipse:MultiBoundField DataFields="OfficeName" HeaderText="Office" />
            <eclipse:MultiBoundField DataFields="MinPayScaleAmount,IncrementAmount,MaxPayScaleAmount"
                HeaderText="Pay Scale" DataFormatString="{0:N0}-{1:N0}-{2:N0}">
                <ItemStyle Wrap="false" />
            </eclipse:MultiBoundField>
            <eclipse:MultiBoundField DataFields="BeforeIncrement"
                HeaderText="Salary|Before" DataFormatString="{0:N2}">
                <ItemStyle Wrap="false" />
                <ItemStyle HorizontalAlign="Right" />
            </eclipse:MultiBoundField>
            <eclipse:MultiBoundField DataFields="AfterIncrement"
                HeaderText="Salary|After" DataFormatString="{0:N2}">
                <ItemStyle Wrap="false" />
                <ItemStyle HorizontalAlign="Right" />
            </eclipse:MultiBoundField>
        </Columns>
    </jquery:GridViewEx>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cphNoForm" runat="server">
</asp:Content>
