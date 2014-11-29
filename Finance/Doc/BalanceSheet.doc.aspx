<%@ Page Title="Balancesheet Doc" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="BalanceSheet.doc.aspx.cs" Inherits="Finance.Doc.BalanceSheet" %>

<asp:Content ID="Content6" ContentPlaceHolderID="cphNoForm" runat="server">
<p>The balance sheet is always displayed from the project start date.</p>
    <ol>
        <li>By default, up to the minute balance sheet is displayed. Postdated vouchers will
            not be included. </li>
        <li>You can enter a past date to see the balance sheet as it existed on that date</li>
        <li>To include postdated vouchers, you will need to enter a date in the future</li>
        <li>Total assets and liabilities may not match if you have some
            <asp:HyperLink ID="hldisc" runat="server" Text="discrepant vouchers" NavigateUrl="~/Finance/VoucherDiscrepancies.aspx"></asp:HyperLink>.</li>
    </ol>
    <h3>
        Troubleshooting</h3>
    <h4>
        Some of the amounts do not appear to be right?</h4>
    <ol>
        <li>Check that head of account types are correct. Each balance sheet item displays amounts
            belonging to specific head of accounts. For example, The EMD entry calculates its
            amount by summing up all voucher entries whose head of accounts have been designated
            as Earned Money Deposit. You can verify this by clicking on the text EMD. You will
            see a list of account heads with the EMD accounts checked. You can edit any of the
            accounts and update its type as necessary so that the EMD shows the correct value.
        </li>
        <li>Check that all vouchers exist. Click on the amount to see which voucher entries
            have been included. Look for vouchers which may be missing or which may have data
            entry errors. Each item in the balance sheet displace voucher entries for specific
            head of accounts. Click on the item name to see the list of heads which are included
            in the item. If a head which should included, but is not checked... </li>
    </ol>
    <h4>
        Total Assets do not match Total Liabilities?</h4>
    <ol>
        <li>Check whether discrepant vouchers exist. </li>
        <li>Are all head of account being used in the balance sheet. The balance sheet is calculated
            by summing up voucher entries in various head of accounts. Ideally, all head of
            accounts must reflect in the balance sheet. If some head of accounts exist which
            do not impact the balance sheet, then the potential for total assets and total liabilities
            mismatch is created. For the purpose of balance sheet, the following heads are considered
            to be liability heads: TODO: Show data bound bullet list. </li>
    </ol>
</asp:Content>
