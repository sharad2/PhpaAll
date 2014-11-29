<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" CodeBehind="Paybill1.aspx.cs"
    Inherits="Finance.Payroll.Reports.Paybill1" Title="Pay Bill" EnableViewState="false" %>

<%@ Register Src="~/Controls/PrinterFriendlyButton.ascx" TagName="PrinterFriendlyButton"
    TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphSideNavigation" runat="server">
    <asp:HyperLink ID="HyperLink2" runat="server" Text="Help" NavigateUrl="~/Doc/Paybill1.doc.aspx" />
    <br />
    <uc2:PrinterFriendlyButton ID="PrinterFriendlyButton1" runat="server" EnableTheming="true" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cph" runat="server">
    <eclipse:TwoColumnPanel runat="server">
        <eclipse:LeftLabel runat="server" Text="Paybill for the month of" />
        <i:TextBoxEx runat="server" ID="tbDate" FriendlyName="Month" Text="0">
            <Validators>
                <i:Date />
                <i:Required />
            </Validators>
        </i:TextBoxEx>
        <eclipse:LeftLabel runat="server" Text="Employee Type" />
        <phpa:PhpaLinqDataSource ID="dsEmpTypes" runat="server" ContextTypeName="Eclipse.PhpaLibrary.Database.Payroll.PayrollDataContext"
            OrderBy="EmployeeTypeCode" TableName="EmployeeTypes" RenderLogVisible="false" />
        <i:DropDownListEx ID="ddlEmpType" runat="server" EnableViewState="false" DataSourceID="dsEmpTypes"
            DataTextField="Description" DataValueField="EmployeeTypeId">
            <Items>
                <eclipse:DropDownItem Text="(All types)" Persistent="Always" />
            </Items>
        </i:DropDownListEx>
        <phpa:PhpaLinqDataSource ID="dsBankName" runat="server" ContextTypeName="Eclipse.PhpaLibrary.Database.Payroll.PayrollDataContext"
            RenderLogVisible="false" TableName="Banks" Select="new (BankId, BankName)" />
        <eclipse:LeftLabel ID="lblBankName" runat="server" Text="Bank Name" />
        <i:DropDownListEx ID="ddlBankName" runat="server" EnableViewState="false" DataSourceID="dsBankName" DataTextField="BankName" DataValueField="BankId">
             <Items>
                <eclipse:DropDownItem Text="(All)" Persistent="Always" />
            </Items>
        </i:DropDownListEx>
        <eclipse:LeftLabel ID="LeftLabel1" runat="server" Text="Nationality" />
        <i:RadioButtonListEx ID="rblNationality" runat="server" Orientation="Horizontal"
            Value="false" WidthItem="9.5em">
            <Items>
                <i:RadioItem Text="All" Value="" />
                <i:RadioItem Text="Bhutanese" Value="true" />
                <i:RadioItem Text="Non Bhutanese" Value="false" />
            </Items>
        </i:RadioButtonListEx>
        <br />
        <i:ButtonEx ID="btnShow" runat="server" Text="Go" CausesValidation="true" Icon="Refresh"
            Action="Submit" IsDefault="true" />
        <i:ButtonEx runat="server" Action="Reset" Text="Clear Filters" Icon="Cancel" />
        <i:ValidationSummary ID="ValidationSummary1" runat="server" />
    </eclipse:TwoColumnPanel>
    <br />
    <phpa:PhpaLinqDataSource ID="dsPaybill" runat="server" ContextTypeName="Eclipse.PhpaLibrary.Database.Payroll.PayrollDataContext"
        TableName="PeriodEmployeeAdjustments" RenderLogVisible="false" OnSelecting="dsPaybill_Selecting">
    </phpa:PhpaLinqDataSource>
    <jquery:GridViewEx ID="gvPaybill" runat="server" AutoGenerateColumns="false" ShowFooter="true"
        EnableViewState="false" CellPadding="2" OnRowDataBound="gvPaybill_RowDataBound"
        DataSourceID="dsPaybill">
        <HeaderStyle Font-Size="8pt" />
        <Columns>
            <eclipse:SequenceField>
            </eclipse:SequenceField>
            <eclipse:MultiBoundField DataFields="EmployeeCode" HeaderText="Employee|Code">
                <ItemStyle HorizontalAlign="Right" />
            </eclipse:MultiBoundField>
            <eclipse:MultiBoundField DataFields="FullName" HeaderText="Employee|Name" ToolTipFields="SalaryPeriodDescription"
                ToolTipFormatString="Salary Period: {0}">
                <ItemStyle Wrap="false" />
            </eclipse:MultiBoundField>
            <eclipse:MultiBoundField DataFields="Designation" HeaderText="Employee|Designation" />
            <eclipse:MultiBoundField DataFields="CitizenCardNo" HeaderText="Employee|Citizen ID" />
            <eclipse:MultiBoundField DataFields="Grade" HeaderText="Employee|Gr.">
                <ItemStyle HorizontalAlign="Right" />
            </eclipse:MultiBoundField>
            <eclipse:MultiBoundField DataFields="BasicPay" AccessibleHeaderText="BasicPay" DataFormatString="{0:N0}"
                HeaderText="Basic Salary" DataSummaryCalculation="ValueSummation">
                <ItemStyle HorizontalAlign="Right" />
                <FooterStyle HorizontalAlign="Right" />
            </eclipse:MultiBoundField>          
            <m:MatrixField DataMergeFields="EmployeeId,EmployeePeriodId" DataCellFields="AmountRounded"
                HeaderText="{0::$IsDeduction:B. Deductions:A. Earnings} " DataHeaderFields="IsDeduction,AdjustmentCategoryDescription">
                <MatrixColumns>
                    <m:MatrixColumn DataCellFormatString="{0:C0}" DataHeaderFormatString="{1}" ColumnType="CellValue"
                        DisplayColumnTotal="true" />
                </MatrixColumns>
            </m:MatrixField>
            <eclipse:MultiBoundField HeaderText="Net Pay" AccessibleHeaderText="netPay">
                <ItemStyle HorizontalAlign="Right" />
                <FooterStyle HorizontalAlign="Right" />
            </eclipse:MultiBoundField>
        </Columns>
        <EmptyDataTemplate>
            <b>Paybill not found for given month and year.</b>
        </EmptyDataTemplate>
    </jquery:GridViewEx>

    <br />
    <br />
    <br />
    <span style="font-weight:bold">Division wise Salary breakup</span>
     <phpa:PhpaLinqDataSource ID="dsPaybill1" runat="server" ContextTypeName="Eclipse.PhpaLibrary.Database.Payroll.PayrollDataContext"
        TableName="PeriodEmployeeAdjustments" RenderLogVisible="false" OnSelecting="dsPaybill1_Selecting">
    </phpa:PhpaLinqDataSource>
     <jquery:GridViewEx ID="GridViewEx1" runat="server" AutoGenerateColumns="false" ShowFooter="true"
        EnableViewState="false" CellPadding="2" 
        DataSourceID="dsPaybill1" DefaultSortExpression="">
        <HeaderStyle Font-Size="8pt" />
        <Columns>
            <eclipse:SequenceField>
            </eclipse:SequenceField>
            <eclipse:MultiBoundField DataFields="DivisonName" HeaderText="Division">
                <ItemStyle HorizontalAlign="Right" />
            </eclipse:MultiBoundField>  
           <%--    <eclipse:MultiBoundField DataFields="BasicSalary" HeaderText="Basic Salary" DataFormatString="{0:C0}" DataSummaryCalculation="ValueSummation">
                <ItemStyle HorizontalAlign="Right" />
                   <FooterStyle HorizontalAlign="Right"/>
            </eclipse:MultiBoundField>   --%>
             <eclipse:MultiBoundField DataFields="Amount" HeaderText="Amount" DataFormatString="{0:C2}" DataSummaryCalculation="ValueSummation">
                <ItemStyle HorizontalAlign="Right" />
                 <ItemStyle HorizontalAlign="Right" />
            </eclipse:MultiBoundField>   
                   
        </Columns>
        <EmptyDataTemplate>
            <b>Paybill not found for given month and year.</b>
        </EmptyDataTemplate>
    </jquery:GridViewEx>
</asp:Content>
