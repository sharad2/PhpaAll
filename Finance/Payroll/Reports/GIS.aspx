<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" EnableViewState="false"
    CodeBehind="GIS.aspx.cs" Inherits="Finance.Payroll.Reports.GIS" Title=" SCHEDULE FOR RECOVERY OF GIS" %>

<%@ Register Src="~/Controls/PrinterFriendlyButton.ascx" TagName="PrinterFriendlyButton"
    TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphSideNavigation" runat="server">
    <asp:HyperLink ID="hlHelp" runat="server" Text="Help" NavigateUrl="~/Doc/GIS.doc.aspx" />
    <uc2:PrinterFriendlyButton ID="PrinterFriendlyButton1" runat="server" EnableTheming="true" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cph" runat="server">
    <eclipse:TwoColumnPanel runat="server" WidthLeft="20%" WidthRight="80%">
        <eclipse:LeftLabel ID="LeftLabel2" runat="server" Text="From Date/To Date" />
        <i:TextBoxEx ID="tbFromDate" runat="server" FriendlyName="From Date">
            <Validators>
                <i:Required />
                <i:Date Min="-183" Max="0" />
            </Validators>
        </i:TextBoxEx>
        <i:TextBoxEx ID="tbToDate" runat="server" FriendlyName="To Date">
            <Validators>
                <i:Required />
                <i:Date />
            </Validators>
        </i:TextBoxEx>
        <br />
        <eclipse:LeftLabel ID="LeftLabel1" runat="server" Text="Bank Name" />
        <phpa:PhpaLinqDataSource ID="dsBankName" runat="server" ContextTypeName="Eclipse.PhpaLibrary.Database.Payroll.PayrollDataContext"
            RenderLogVisible="false" TableName="Banks" Select="new (BankId,BankName)"/>
        <i:DropDownListEx ID="ddlBankName" runat="server" EnableViewState="false" DataSourceID="dsBankName" DataTextField="BankName" DataValueField="BankId">
            <Items>
                <eclipse:DropDownItem Text="(All)" Persistent="Always" />
            </Items>
        </i:DropDownListEx>
        <br />
        <phpa:PhpaLinqDataSource runat="server" ID="dsEmployeeTypes" TableName="EmployeeTypes"
            ContextTypeName="Eclipse.PhpaLibrary.Database.Payroll.PayrollDataContext" OrderBy="EmployeeTypeCode"
            RenderLogVisible="false" />
        <i:CheckBoxListEx runat="server" ID="cblEmployeeTypes" DataSourceID="dsEmployeeTypes"
            DataTextField="Description" DataValueField="EmployeeTypeId" FriendlyName="Employee Types">
            <Validators>
                <i:Required />
            </Validators>
        </i:CheckBoxListEx>
        <br />
        <i:ButtonEx ID="btnShowReport" runat="server" Text="Show Report" CausesValidation="true"
            OnClick="btnShowReport_Click" Action="Submit" IsDefault="true" />
    </eclipse:TwoColumnPanel>
    <br />
    <i:ValidationSummary ID="ValidationSummary1" runat="server" />
    <phpa:PhpaLinqDataSource ID="dsGIS" runat="server" ContextTypeName="Eclipse.PhpaLibrary.Database.Payroll.PayrollDataContext"
        OnSelecting="dsGIS_Selecting" RenderLogVisible="false">
    </phpa:PhpaLinqDataSource>
    <asp:Label ID="lblRecovery" runat="server" Visible="false" /><br />
    <br />
    <jquery:GridViewEx ID="gvGIS" runat="server" AutoGenerateColumns="false" EnableViewState="false"
        DataSourceID="dsGIS" AllowSorting="true" OnRowDataBound="gvGIS_RowDataBound"
        ShowFooter="true">
        <Columns>
            <eclipse:SequenceField FooterText="" />
            <eclipse:MultiBoundField DataFields="GISNO" HeaderText="GIS A/C No." ItemStyle-HorizontalAlign="Center" />
            <eclipse:MultiBoundField DataFields="Name" HeaderText="Employee" SortExpression="FirstName"
                ItemStyle-Width="20em" />
            <eclipse:MultiBoundField DataFields="Designation" HeaderText="Designation" SortExpression="Designation" />
            <eclipse:MultiBoundField DataFields="Grade" HeaderText="Grade" SortExpression="Grade"
                ItemStyle-HorizontalAlign="Center" />
            <eclipse:MultiBoundField DataFields="GISGroup" HeaderText="GIS Grp." SortExpression="GISGroup"
                ItemStyle-HorizontalAlign="Center" />
            <eclipse:MultiBoundField DataFields="Amount" DataFormatString="{0:N0}" HeaderText="Subscription"
                DataSummaryCalculation="ValueSummation">
                <ItemStyle HorizontalAlign="Right" />
                <HeaderStyle HorizontalAlign="Center" />
                <FooterStyle HorizontalAlign="Right" />
            </eclipse:MultiBoundField>
            <eclipse:MultiBoundField DataFields="CitizenId" HeaderText="Citizen ID" FooterText="Total"
                ItemStyle-HorizontalAlign="Center" />
            <eclipse:MultiBoundField DataFields="DateOfBirth" HeaderText="Date of Birth" DataFormatString="{0:d}">
                <ItemStyle HorizontalAlign="Right" />
            </eclipse:MultiBoundField>
        </Columns>
        <EmptyDataTemplate>
            GIS not recovered from any employee during the given Period
        </EmptyDataTemplate>
    </jquery:GridViewEx>
</asp:Content>
