<%@ Page Title="Cash Book Doc" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="CashBook.doc.aspx.cs" Inherits="Finance.Doc.CashBook" %>
<asp:Content ID="Content6" ContentPlaceHolderID="cphNoForm" runat="server">
 <br />
 <p>Displays all the vouchers associated with a particular cash account. You specify
    the date range and the cash account.</p>
    <ol>
        <li>All heads whose Account Type is set as a CASH are considered to be cash accounts.
        </li>
        <li>By default, the date range is set to the starting and ending of the previous calendar
            month. This makes it convenient for you to see all the transactions which are expected
            to be in the previous month's Cash statement. If you are interested in seeing the
            entries of an older statement, just change the date range. </li>
        <li>It is OK to specify just the From date. The To date which you do not specify is
            automatically deduced to be one month after the date you have specified. For example,
            if you specify 1st March 2007 as the From date, then the To date is assumed to be
            30 days after, i.e. 31st March 2007. </li>
        <li>You can easily determine the closing balance at the end of any date by specifying
            that date as the To date. </li>
    </ol>
</asp:Content>
