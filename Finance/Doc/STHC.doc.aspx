<%@ Page Title="STHC Doc" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="STHC.doc.aspx.cs" Inherits="PhpaAll.Doc.STHC" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphSideNavigation" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="contentRight" runat="server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cph" runat="server">
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="cphNoForm" runat="server">
    Report displays the Remittances of Personal Income Tax and Health Contribution for
    the given Period.
    <ol>
        <li><b>Default Date Range</b> is of 1 month, if period is not selected then report shows
            remittances for previous month.</li>
        <li>You have <b>Date Range Control</b> given at the top of the screen. You can select
            the date range either using the From Date and To Date <b>Calendar Icon</b> or <b>Date
                Range icon</b>. </li>
        <li>You have given with the filter which allows you to view the <b>recovery for Deputationists
            and recovery except Deputationists</b>. Click the <b>check box</b> and select <b>Deputation</b>
            from the drop down list, it shows you the <b>recovery for Deputationists only</b>
            and if you <b>uncheck the check box</b> and keep the Deputation in the list, it
            shows you recovery except Deputationists. </li>
        <li>On <b>Show Report</b> click, report shows the <b>Remittances</b> for the date range
            which is been selected in the <b>Date Range Control</b>. </li>
        <li>Report shows all the records for all the <b>Overlapping Salary Periods</b> which
            exists between the <b>Selected Date Range</b>. </li>
        <li>Report contains <b>Employee Information</b> along with <b>Taxable Amount</b>, <b>
            Salary Tax</b>, and <b>Health Contribution</b>. </li>
        <li><b>Taxable Amount</b> is the summation of <b>Basic Salary</b> and <b>Total Allowances</b>.
        </li>
        <li>In order to view the entry in Salary Tax column, <b>create adjustments having Adjustment
            Category as Salary Tax (ST)</b>. For the employees <b>(who has accommodation facility
                provided by the PHPA authority)</b>, create an extra adjustment having Adjustment
            Category as Salary Tax (ST), this deduction amount is automatically added in <b>Salary
                tax</b>. </li>
        <li>You have another Deduction type adjustment named <b>Health Contribution</b>, create
            an adjustment having adjustment category as Health Contribution (HC). The deduction
            amount in that adjustment will be shown in <b>Health Contribution</b>. </li>
    </ol>
</asp:Content>
