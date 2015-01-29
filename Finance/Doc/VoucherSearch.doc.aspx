<%@ Page Title="Voucher Search Doc" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="VoucherSearch.doc.aspx.cs" Inherits="PhpaAll.Doc.VoucherSearch" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
 
<asp:Content ID="Content3" ContentPlaceHolderID="cphSideNavigation" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="contentRight" runat="server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cph" runat="server">
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="cphNoForm" runat="server">
 Voucher Search helps you to find the vouchers that you are looking for.
    <ol>
        <li>Voucher search have multiple parameters which allows you to search for a particular
            voucher and list of voucher according to the search parameter given. </li>
        <li>List displays vouchers for the last one month by default.</li>
        <li>Screen given with <b>Particulars</b>, <b>Voucher No</b>, <b>From Date/To Date</b>,
            <b>Employee Selector</b>, <b>Contractor Selector</b>, <b>Head Selector</b> and <b>several
                Account types</b> search parameters. </li>
        <li><b>Particulars</b> allow you to search vouchers based on their Particulars specified.</li>
        <li><b>Voucher No.</b> allows you to search a Voucher according to the Voucher no specified.
        </li>
        <li><b>Employee Selector</b> allows you to look for a Employee voucher same as Contractor
            selector. </li>
        <li><b>Head Selector</b> allows you to look for Voucher list based on Account Head.
        </li>
        <li>You have one Label given at the left top of the Voucher list, which gives the <b>
            Difference</b> between <b>Debit</b> and <b>Credit amount</b>.</li>
        <li><b>Voucher List</b> contains <b>Date</b>, <b>Voucher Reference</b>, <b>Division</b>,
            <b>Employee</b>, <b>Particulars</b>, <b>Voucher Type</b> and <b>No</b>, <b>Debit</b>
            and <b>Credit</b> amount. </li>
        <li>You can view the <b>Details</b> of any voucher by clicking of <b>Details button</b>
            which verify you the voucher Details of selected voucher. </li>
        <li>This screen server as <b>cross check medium</b> for few of the report like <b>Trial
            Balance</b>, <b>Expenditure Report</b>. You can drill to voucher search through
            these reports and cross check your vouchers. </li>
    </ol>
</asp:Content>
