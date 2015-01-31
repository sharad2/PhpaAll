<%@ Page Title="Journal Book Doc" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="JournalBook.doc.aspx.cs" Inherits="PhpaAll.Doc.JournalBook" %>

<asp:Content ID="Content6" ContentPlaceHolderID="cphNoForm" runat="server">
<br />
<p>Displays all the vouchers associated with other than bank and cash accounts. You
    specify the date range.</p>
    <ol>
        <li>By default, the date range is set from starting date of current month to current
            date. This makes it convenient for you to see all the transactions which are expected
            to be in the Current month's journal book. If you are interested in seeing the entries
            of an older statement, just change the date range. </li>
        <li>It is OK to specify just the From date. The To date which you do not specify is
            automatically deduced to be one month after the From date you have specified. For
            example, if you specify 1st March 2007 as the From date, then the To date is assumed
            to be 30 days after, i.e. 31st March 2007. </li>
    </ol>
</asp:Content>
