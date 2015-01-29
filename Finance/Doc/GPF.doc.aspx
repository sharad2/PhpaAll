<%@ Page Title="GPF Doc" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="GPF.doc.aspx.cs" Inherits="PhpaAll.Doc.GPF" %>
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
        <li>This screen displays NPPF details for the given salary period</li>
        <li>Report display recovery for Bhutanese employees for previous month.</li>
        <li>You have given with the filter for Bhutanese and Non-Bhutanese which allows you to
            view the report for Bhutanese and Non-Bhutanese employees separately.</li>
        <li>Report contains NPPF No, Members Name, Designation, Citizen Id, Basic Pay, Contribution
            and Total Nu column.</li>
        <li>Contribution is a Dynamic column which changes automatically based on filter choosed.</li>
        <li>Report shows data for one month only.</li>
    </ol>
</asp:Content>
