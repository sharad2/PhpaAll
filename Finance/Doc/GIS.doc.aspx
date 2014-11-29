<%@ Page Title="GIS Doc" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="GIS.doc.aspx.cs" Inherits="Finance.Doc.GIS" %>
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
        <li>Displays detail of GIS deduced during the given period</li>
        <li>For the report to display some data GIS must be deduced from employee during the
            given period, this can be verified from Manage Employee Period screen which contains
            detail information of all adjustment of each employee during the specified period</li>
        <li>For viewing the details of GIS during the given period provide the adjustment for
            GIS to employee for the given period through Manage Employee Period.</li>
        <li>You have given with the filter which allows you to view the <b>recovery for Deputationists
            and recovery except Deputationists</b>. Click the <b>check box</b> and select <b>Deputation</b>
            from the drop down list, it shows you the <b>recovery for Deputationists only</b>
            and if you <b>uncheck the check box</b> and keep the Deputation in the list, it
            shows you recovery except Deputationists. </li>
    </ol>
</asp:Content>
