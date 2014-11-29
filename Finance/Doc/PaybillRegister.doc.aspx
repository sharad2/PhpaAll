<%@ Page Title="Pay Bill Register Doc" Language="C#" MasterPageFile="~/MasterPage.master"
    AutoEventWireup="true" CodeBehind="PaybillRegister.doc.aspx.cs" Inherits="Finance.Doc.PaybillRegister" %>

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
    <ol>
        <li>Displays Employee's Complete detail as well as Salary detail for a given Employee
            and Year</li>
        <li>Total Dues is sum of all the Allowances for the employee during the given period</li>
        <li>Total Recoveries is sum of all the recoveries made by the employee during the given
            period</li>
        <li>Net Pay is the difference amount between net allowances and net recoveries</li>
        <li>For the report to display Salary Details data for the given employee the employee
            must be paid during the given year, which can be deduced from SalaryPeriod Screen,
            this screen contains a link against the period clicking on link results in providing
            info of salary paid or to be paid to the employee during the given period.</li>
        <li><b>Report can be best viewed in Landscape.</b></li>
    </ol>
</asp:Content>
