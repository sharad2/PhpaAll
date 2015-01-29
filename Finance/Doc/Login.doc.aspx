<%@ Page Title="Login Doc" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="Login.doc.aspx.cs" Inherits="PhpaAll.Doc.Login" %>

<asp:Content ID="Content6" ContentPlaceHolderID="cphNoForm" runat="server">
    <h2>Troubleshooting</h2>
    <p>
        If no error message displays after you login, it means that your login succeeded
        but you do not have the role required to perform the operation which requires the
        login.
    </p>
    <p>
        As an example, let us say that you bookmark the URL for the Manage Users page. Later
        when you attempt to use the bookmark, the login dialog will show up since the page
        requires administrator privileges. If you provide an invalid user name and password,
        an error message will show as expected. However, if you provide the user name and
        password for a valid user who is <em>not</em> an administrator, no error message
        will be displayed and the login page will continue to show.
    </p>
    <p>
        This scenario is extremely rare and should not happen during normal course of operation.
    </p>
    
</asp:Content>
