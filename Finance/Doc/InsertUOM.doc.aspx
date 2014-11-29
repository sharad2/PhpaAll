<%@ Page Title="Insert UOM Doc" Language="C#" MasterPageFile="~/MasterPage.master"
    AutoEventWireup="true" CodeBehind="InsertUOM.doc.aspx.cs" Inherits="Finance.Doc.InsertUOM" %>

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
    Screen helps you to Manage Unit of Measurement and allows you to create new Units.
    <ol>
        <li>This page displays the list of Unit of Measurement</li>
        <li>In order to create New Unit, click <b>New UOM</b> given above at the top of the
            list of Unit of Measurement. </li>
        <li>When you click on <b>New UOM</b>, Textbox's will appear in the first row of the list. After giving input click on <b>Insert</b> to save.</li>
        <li>In order to Edit and Delete existing Unit, Click <b>Edit</b> and <b>Delete</b> given in the first
            column. </li>
        <li>A Search Textbox has been given to search Unit on the basis of Unit of Measurement
            Code or Description.</li>
    </ol>
</asp:Content>
