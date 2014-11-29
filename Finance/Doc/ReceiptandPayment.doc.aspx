<%@ Page Title="Receipt and Payment Doc" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="ReceiptandPayment.doc.aspx.cs" Inherits="Finance.Doc.ReceiptandPayment" %>

<asp:Content ID="Content6" ContentPlaceHolderID="cphNoForm" runat="server">
 <br />
 <p>Displays Cumulative Receipts and Payments Details from Project start date.</p>
    <ol>
        <li>By default the date up to which Details are viewed is current date. </li>
        <li>To include postdated vouchers, you will need to enter a date in the future</li>
        <li>Opening balances for Up to the month must match with that of closing balances of
            previous year.</li>
        <li>To get the voucher details for any amount you just need to click the link i.e. amount</li>
        <li>To get to know the details of all the discrepant vouchers click the link
            <asp:HyperLink ID="hplnkdes" runat="server" Text="Discrepent Vouchers" NavigateUrl="~/Finance/VoucherDiscrepancies.aspx"></asp:HyperLink>
        </li>
        <li>To get the details of all the heads for Receipts & Payments click this link
            <asp:HyperLink ID="hplnkrecpay" runat="server" Text="Receipts and Payments Heads"></asp:HyperLink>
        </li>
    </ol>
</asp:Content>
