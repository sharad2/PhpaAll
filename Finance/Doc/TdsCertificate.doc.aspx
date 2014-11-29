<%@ Page Title="Tds Certificate Doc" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="TdsCertificate.doc.aspx.cs" Inherits="Finance.Doc.TdsCertificate" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphSideNavigation" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="contentRight" runat="server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cph" runat="server">
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="cphNoForm" runat="server">
<br />
 Report showing the TDS certificate for selected employee.
    <ol>
        <li>You can view <b>TDS Certificate</b> by selecting employee in employee selector.
        </li>
        <li>On Show TDS Certificate click, a table will be displayed, containing the entire
            <b>deductions</b> and <b>allowances</b> summary applicable to that <b>particular employee</b>
            which is been selected in the employee selector. </li>
        <li>Table contains <b>month column</b> which shows the <b>salary period code</b> for
            which salary is paid to the employee. </li>
        <li>You have <b>BCA</b> and <b>Other Allowances columns</b>, if any Allowance is added
            except BCA, the entry of that Allowance will be shown in <b>Other Allowances column</b>.
        </li>
        <li><b>Gross salary</b> is the sum of <b>Basic salary</b>, <b>BCA</b> and <b>Other Allowances.</b></li>
        <li>Rest columns shows the deduction summary applicable for selected employee. If any
            other Deduction is added whose Adjustment category not exists in <b>TDS</b>, <b>GPF</b>,
            <b>GIS</b>, <b>HBA</b>, <b>FF Club</b>, <b>Telephone Recovery</b>, the entry of
            that Deduction will be shown in <b>Other Deductions column</b>. </li>
        <li><b>Net pay</b> is the difference between <b>Gross salary</b> and <b>Total deductions</b>.
        </li>
        <li>You have given with a Read only Column name as <b>MR No./Date</b>. You can manually
            mention that no.</li>
        <li>Table contains 1 row for each <b>salary period</b> between the <b>range chosen</b>.
            So, in order to view the report, choose the date range which includes all the <b>Salary
                Period Start</b> and <b>Salary Period End</b> date. </li>
    </ol>
</asp:Content>
