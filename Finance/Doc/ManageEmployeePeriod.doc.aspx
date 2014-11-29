<%@ Page Title="Manage Employee Period Doc" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="ManageEmployeePeriod.doc.aspx.cs" Inherits="Finance.Doc.ManageEmployeePeriod" %>
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
        <li>On <b>Employee Search/Add</b> click, if Employee exists in the specified period
            then its Adjustment summary and all related information's. will be displayed.</li>
        <li>If employee does not exist in the specified period, you will be provided with the
            option to <b>Add</b> it for the employee who is been selected in the Employee selector.
            On <b>Add click</b>, if you have already defined adjustments for this employee will
            automatically added else adjustment applicable to the <b>Employee Type</b> will be
            added automatically. </li>
        <li>You can <b>Delete employee</b> using <b>Delete Button</b>. This will delete all
            the <b>Adjustments</b> of the selected employee. If you want to delete adjustment
            one by one you have same feature in <b>Employee Adjustment Editor</b> also.
        </li>
        <li>On click of Edit button in <b>Employee Adjustment Editor</b>, Amount columns containing
            default values that have already been defined for that adjustment get displayed.
            We can modify these values by clicking <b>Override default value check box</b> and
            then set the new values. </li>
        <li>Whenever we edit the amount for an adjustment in the <b>Employee Adjustment Editor</b>,
            an image
            <img src="../Images/info.jpg" alt="info" />
            gets displayed signifying the change that occurred.</li>
        <li>You have given with the option <b>Remarks</b> field in <b>Employee Adjustment Editor</b>
            which allows you to enter your remarks against the changes you made in the salary.
            You can also use <b>Remarks</b> to enter Policy No of employees.</li>
        <li>You can navigate to this page through <b>Salary Periods</b> which allows you to
            remit the salary of employees for respective banks. </li>
    </ol>
</asp:Content>
