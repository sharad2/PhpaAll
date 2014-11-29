<%@ Page Title="DayBook Doc" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="DayBook.doc.aspx.cs" Inherits="Finance.Doc.DayBook" %>
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
 <br />
 Displays all the vouchers of a particular date and opening and closing balance of
    all bank and cash accounts of specify date. You may specify the voucher date.
    <ol>
        <li>All heads whose Account Type is set as a BANKNU or BANKFE are considered to be bank
            accounts and All heads whose Account Type is set as a CASH are considered to be
            cash accounts . </li>
        <li>By default, the date is set to the current date. This makes it convenient for you
            to see all the transactions which are placed in the current date. If you are interested
            in seeing the entries of an older statement, just change the date. </li>
        <li>You can determine the opening balance and closing balance of all bank and cash account
            heads at the end of date which you specify. </li>
    </ol>
</asp:Content>
