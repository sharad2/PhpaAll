<%@ Page Title="Issue SRS Doc" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="IssueSRS.doc.aspx.cs" Inherits="Finance.Doc.IssueSRS" %>
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
        <li>This page allows you to issue or unissue SRS for the selected SRS No. In case you
            have forgotten the SRS No, then you can select it from the SRS list by clicking
            on 'Select from List'</li>
    </ol>
    This table shows the following
    <ol>
        <li>The Required column shows the total no of Items required.</li>
        <li>Issued column shows the total no of Items issued so far</li>
        <li>Available column shows the total no of items available in the inventory.</li>
        <li>In issue text box, you can enter the number of items you want to issue. Then click
            on 'Issue SRS' button. If the text box is grey, then it means that the required
            quantity has been issued already.</li>
    </ol>
</asp:Content>
