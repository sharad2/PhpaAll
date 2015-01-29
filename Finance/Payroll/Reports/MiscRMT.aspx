<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" CodeBehind="MiscRMT.aspx.cs"
    Inherits="PhpaAll.Payroll.Reports.MiscRMT" Title="FFCLUB RMT" EnableViewState="false" %>

<%@ Register Src="~/Controls/PrinterFriendlyButton.ascx" TagName="PrinterFriendlyButton"
    TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphSideNavigation" runat="server">
    <uc2:PrinterFriendlyButton runat="server" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cph" runat="server" style="background-color: Red">
    <eclipse:TwoColumnPanel runat="server">
        <eclipse:LeftLabel runat="server" Text="From Date/To Date" />
        <i:TextBoxEx ID="tbFromDate" runat="server" FriendlyName="From Date">
            <Validators>
                <i:Required />
                <i:Date Max="0" />
            </Validators>
        </i:TextBoxEx>
        <i:TextBoxEx ID="tbToDate" runat="server" FriendlyName="To Date">
            <Validators>
                <i:Required />
                <i:Date DateType="ToDate" />
            </Validators>
        </i:TextBoxEx>
        <br />
        <eclipse:LeftLabel runat="server" Text="Bank Name" />
        <phpa:PhpaLinqDataSource ID="dsBankName" runat="server" ContextTypeName="Eclipse.PhpaLibrary.Database.Payroll.PayrollDataContext"
            RenderLogVisible="false" TableName="Banks" Select="new (BankName,BankId)"/>
        <i:DropDownListEx ID="ddlBankName" runat="server" EnableViewState="false" DataSourceID="dsBankName"
            DataTextField="BankName" DataValueField="BankId" >
            <Items>
                <eclipse:DropDownItem Text="(All)" Persistent="Always" />
            </Items>
        </i:DropDownListEx>
        <br />
        <i:ButtonEx ID="btnSearch" runat="server" Text="Show Report" Action="Submit" Icon="Search"
            CausesValidation="true" />
    </eclipse:TwoColumnPanel>
    <i:ValidationSummary runat="server" />
    <phpa:PhpaLinqDataSource ID="dsMiscRMT" runat="server" RenderLogVisible="False" ContextTypeName="Eclipse.PhpaLibrary.Database.Payroll.PayrollDataContext"
        OnSelecting="dsMiscRMT_Selecting">
    </phpa:PhpaLinqDataSource>
    <jquery:GridViewEx ID="gvAdjustment" runat="server" AutoGenerateColumns="false" ShowFooter="true"
        CellPadding="2" DataSourceID="dsMiscRMT" DefaultSortExpression="AdjustmentCode;$;" ShowExpandCollapseButtons="false" OnRowDataBound="gvAdjustment_RowDataBound">
        <Columns>
            <eclipse:SequenceField />
            <eclipse:MultiBoundField DataFields="AdjustmentCode" HeaderText="Recovery" SortExpression="AdjustmentCode" />
            <eclipse:MultiBoundField DataFields="FromDate,ToDate" HeaderText="Dates" DataFormatString="{0:d} to {1:d}" />
            <eclipse:MultiBoundField DataFields="EmpCode,Name,Designation" HeaderText="Employee"
                DataFormatString="{0}: {1} ({2})" />
            <eclipse:MultiBoundField DataFields="Amount" HeaderText="Amount" DataFormatString="{0:N0}" DataSummaryCalculation="ValueSummation">
                <ItemStyle HorizontalAlign="Right" />
                <FooterStyle HorizontalAlign="Right" />
            </eclipse:MultiBoundField>
             <eclipse:MultiBoundField HeaderText="Remarks" DataFormatString="">
              <ItemStyle Width="2in" />
            </eclipse:MultiBoundField>
        </Columns>
    </jquery:GridViewEx>
</asp:Content>
