<%@ Page Title="Jobs Doc" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="Jobs.doc.aspx.cs" Inherits="PhpaAll.Doc.Jobs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
 
<asp:Content ID="Content3" ContentPlaceHolderID="cphSideNavigation" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="contentRight" runat="server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cph" runat="server">
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="cphNoForm" runat="server">
  <ol>
        <li>This page is for managing <b>Job's</b>. You can view and update details of any selected
            Job. <b>Login</b> is required to update any Job. </li>
        <li>This interface allows you to insert <b>New Job and its Details.</b></li>
        <li>In order to view the Jobs in the list, <b>Create New Job</b> using the link provided
            in <b>Job Editor</b> floating at the top of the screen. </li>
        <li>The entry of New Job can be viewed in the list given in the screen. </li>
        <li>You can search a particular Job using the search feature given.<br />
            a) Job can be <b>searched</b> Job Code wise. Enter the <b>exact Job code</b> or
            <b>matching Job code</b> and click <b>Search button</b>.<br />
            b) Job can viewed <b>Division wise</b>. <b>Enter Division Name</b> in the given
            text box and click <b>Search button</b>. All the existing Job in the entered Division
            will be displayed in the list.<br />
            c) You can view the Job's according to their <b>Completion Date</b>. Enter no of
            days in the given text box and click <b>Search button</b>, Job's which are going
            to be completed in the no. of days entered in the text box will be displayed in
            the List. </li>
        <li>If <b>No Job found</b> in the List, either the <b>search information</b> given are
            <b>not correct</b> or no job is actually been created which suits the entered filters.
        </li>
    </ol>
</asp:Content>
