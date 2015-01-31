<%@ Page Title="SSS Doc" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="SSS.doc.aspx.cs" Inherits="PhpaAll.Doc.SSS" %>
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
 Report Display the Recovery of Life Insurance Premium(SSS) for the given Date Range.
    <ol>
        <li>Report contains the Date Range Control which allow you to check the insurance premium
            recovered in the given date range. </li>
        <li><b>Default Date Range</b> is of 1 month, if range is not selected then report shows
            recovery for previous month.</li>
        <li>On Click of Show Report button, the summary of Life Insurance Premium(SSS) recovery
            will be displayed. </li>
        <li>In Order to view the report, create an adjustment having <b>Adjustment Category
            as SSS</b>. You can create multiple Adjustment as per as your need. </li>
        <li>Each adjustment contains the <b>Policy No</b> which can be added in <b>Employee
            Adjustment</b> or <b>Manage Employee Period</b> using <b>Remarks field</b> given.
        </li>
        <li>You must mention Policy No as a <b>Remark</b> in Employee Adjustment or Manage Employee
            Period screen in order to view the entry in SSS report. </li>
        <li>Report simply contains <b>Employee Name</b>, <b>Designation</b>, <b>Policy No</b>
            and <b>Amount</b> recovered corresponding to that policy. </li>
    </ol>
</asp:Content>
