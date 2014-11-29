<%@ Page Title="Insert Item Doc" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="InsertItem.doc.aspx.cs" Inherits="Finance.Doc.InsertItem" %>
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
 Screen helps you to Manage Items and allows you to create new Items
    <ol>
        <li>This page displays the list of items</li>
        <li>In order to create New Item, click Insert New Item given at the top right of the
            screen. </li>
        <li>In order to Edit and Delete existing Item, Click Select given in the first column.
            You will then see the selected item in the Item Editor pane placed at top right
            which allows you to Edit and Delete the item. </li>
        <li>You have being given a Search Panel to search items on the basis of Item code or
            Description, Category and Head of Account. </li>
        <li>In order to search for an Item, expand the search Panel by clicking arrow given
            in a circle placed at extreme left of the search panel and fill in your search criteria.
        </li>
    </ol>
</asp:Content>
