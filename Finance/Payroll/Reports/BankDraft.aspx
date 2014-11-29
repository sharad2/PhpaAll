<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" CodeBehind="BankDraft.aspx.cs"
    Inherits="Finance.Payroll.Reports.BankDraft" Title="Bank Draft Form" EnableViewState="false" %>

<%@ Register Src="~/Controls/PrinterFriendlyButton.ascx" TagName="PrinterFriendlyButton"
    TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphSideNavigation" runat="server">
    <uc2:PrinterFriendlyButton ID="PrinterFriendlyButton1" runat="server" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cph" runat="server">
    <eclipse:TwoColumnPanel ID="TwoColumnPanel1" runat="server">
        <eclipse:LeftLabel runat="server" Text="Date From/Date To" />
        <i:TextBoxEx ID="tbFromDate" runat="server" FriendlyName="From Date" Text="-30">
            <Validators>
                <i:Required />
                <i:Date Max="0" />
            </Validators>
        </i:TextBoxEx>
        <i:TextBoxEx ID="tbToDate" runat="server" FriendlyName="To Date" Text="0">
            <Validators>
                <i:Required />
                <i:Date DateType="ToDate" Max="0" />
            </Validators>
        </i:TextBoxEx>
        <eclipse:LeftLabel ID="LeftLabel1" runat="server" Text="Employee Type" />
        <phpa:PhpaLinqDataSource ID="dsEmpTypes" runat="server" ContextTypeName="Eclipse.PhpaLibrary.Database.Payroll.PayrollDataContext"
            OrderBy="EmployeeTypeCode" TableName="EmployeeTypes" RenderLogVisible="false" />
        <i:DropDownListEx ID="ddlEmpType" runat="server" DataSourceID="dsEmpTypes" DataTextField="Description"
            DataValueField="EmployeeTypeId">
            <Items>
                <eclipse:DropDownItem Text="(All types)" Persistent="Always" />
            </Items>
        </i:DropDownListEx>
    </eclipse:TwoColumnPanel>
    <i:ButtonEx ID="btnShow" runat="server" Text="Go" Action="Submit" Icon="Refresh"
        CausesValidation="true" IsDefault="true" />
    <i:ValidationSummary ID="vsRMTD" runat="server" />
    <phpa:PhpaLinqDataSource ID="ds" runat="server" ContextTypeName="Eclipse.PhpaLibrary.Database.Payroll.PayrollDataContext"
        RenderLogVisible="false" OnSelecting="ds_Selecting" />
    <jquery:GridViewEx runat="server" DataSourceID="ds" AutoGenerateColumns="false" ShowFooter="true">
        <Columns>
            <eclipse:MultiBoundField DataFields="ParentOrganization" HeaderText="In Favor of" />
            <%--<jquery:MatrixField DataMergeFields="ParentOrganization" DataHeaderFields="AdjustmentCategoryCode"
                DataValueFields="Amount" DisplayRowTotals="true" DisplayColumnTotals="true" HeaderText="Adjustments"
                DataValueFormatString="{0:#,###.##}">
            </jquery:MatrixField>--%>
            <m:MatrixField DataMergeFields="ParentOrganization" DataHeaderFields="AdjustmentCategoryCode"
                DataCellFields="Amount" HeaderText="Adjustments" RowTotalHeaderText="Adjustments">
                <MatrixColumns>
                    <m:MatrixColumn DisplayColumnTotal="true" DataCellFormatString="{0:#,###.##}" ColumnType="CellValue" />
                    <m:MatrixColumn ColumnType="RowTotal" ColumnTotalFormatString="{0:#,###.##}" DataCellFormatString="{0:#,###.##}"
                        DisplayColumnTotal="true" DataHeaderFormatString="Total">
                    </m:MatrixColumn>
                </MatrixColumns>
            </m:MatrixField>
        </Columns>
    </jquery:GridViewEx>
</asp:Content>
