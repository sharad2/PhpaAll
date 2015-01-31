<%@ Page Title="Create GRN Doc" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeBehind="CreateGRN.doc.aspx.cs" Inherits="PhpaAll.Doc.CreateGRN" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
 
<asp:Content ID="Content4" ContentPlaceHolderID="contentRight" runat="server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cph" runat="server">
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="cphNoForm" runat="server">
    <ol>
        <li>To create a new GRN enter the necessary information and then click on Insert button.</li>
        <li>If you want to create GRN for a single item only, then select Ignore option under
            Status column for the second item. If you want to create GRN for more than six items,
            then select the option 'Show more rows' from the insert button at the bottom</li>
        <li>Likewise you can also edit the created GRN. On clicking the delete button you will
            be prompted whether you want to delete or not. If you choose ok then it will get
            deleted </li>
    </ol>
</asp:Content>
