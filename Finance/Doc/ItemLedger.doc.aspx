<%@ Page Title="Item Ledger Doc" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="ItemLedger.doc.aspx.cs" Inherits="Finance.Doc.ItemLedger" %>
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
        <li>Report shows receipt and issue summary for an Item for the given date range.</li>
        <li>The report shows the details of receipt of that item as well as the item issued
            details. You can view the Balance as well. </li>
    </ol>
    The calculation of the columns under Balance are as follows:
    <ol>
        <li>Balance Quantity = Receipt Quantity - Issue Quantity </li>
        <li>Balance Amount = Balance Quantity * Balance Rate</li>
        <li>Opening Balance = The balance till the <strong>FROM</strong> date in the date range</li>
        <li>Closing Balance = (Opening Balance + Total Receipt Quantity) - Total Issued Quantity</li>
    </ol>
</asp:Content>
