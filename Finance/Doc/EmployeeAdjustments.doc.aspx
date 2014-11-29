<%@ Page Title="Employee Adjustments Doc" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="EmployeeAdjustments.doc.aspx.cs" Inherits="Finance.Doc.EmployeeAdjustments" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphSideNavigation" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="contentRight" runat="server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cph" runat="server">
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="cphNoForm" runat="server">
  Screen helps you to Manage Employee Adjustments and allows you to create new Adjustments
    for Employees.
    <ol>
        <li>Employee Payroll Template allows you to search for an Employee. </li>
        <li>On Search click, if Employee exists in the payroll template then template shows
            its Adjustment summary and all related information's. On <b>Employee name</b> click,
            user will be able to see the Adjustment details in Employee Adjustment Editor Panel
            floating at the top of the screen. Here you can Edit as well as add new Adjustments.
            <b>Edit</b> and <b>Add</b> features are reserved for Authorized users. <b>Login</b>
            is required to edit and add new adjustments for existing payroll templates.
        </li>
        <li>If employee does not exist in the payroll template, you will be provided with the
            option to <b>Create</b> it for the employee who is been selected in the Employee
            selector. On <b>Create click</b>, all default adjustments applicable to that selected
            employee will be added in the payroll template only if there are some <b>qualifying
                Adjustment</b> that have been already defined for that <b>Employee Type</b>.
            If no <b>default</b> adjustment is found for that Employee type, user can create
            adjustment using <b>Employee Adjustment Editor</b> floating at the top of the screen.
        </li>
        <li>The above process is valid for <b>single employee insertion</b>. Now if you want
            to add <b>multiple employees</b> at one time, you have Create Default Templates
            option given which adds <b>Employees per division</b>. The Division selector shows
            you the count of Employees who has some qualifying adjustments along with the name
            of the Division as well as defined basic amount. On <b>Create Default Template</b>
            click, all newly added employees in that Division will be added in the Payroll Template
            list.
            <p>
                <b>Note: Division selector only show employees who have at least one qualifying adjustment
                    and defined basic salary.</b></p>
        </li>
        <li>You can check newly added employees by simply verifying the list. All newly added
            employees contain <b style="color: Green">New!</b> Text along with the employee
            name.</li>
        <li>You can <b>Delete employee</b> using <b>Delete Button</b>. This will delete all
            the <b>Adjustments</b> of the selected employee. If you want to delete adjustment
            one by one you have same feature in <b>Employee Adjustment Editor</b> also.
        </li>
        <li>In editing mode you have <b>Adjustment</b>, <b>Amount</b>, <b>Percentage of Basic</b>,
            <b>Type</b> and <b>Remarks</b>. </li>
        <li>On click of Edit button in <b>Employee Adjustment Editor</b>, Amount and Percentage
            of Basic columns containing default values that have already been defined for that
            adjustment. We can modify these values by clicking <b>Override default value check box</b>
            and then set the new values. </li>
        <li>Whenever we edit the amount for an adjustment in the <b>Employee Adjustment Editor</b>,
            an image
            <img src="../Images/info.jpg" alt="info" />
            gets displayed signifying the change that occurred.</li>
    </ol>
</asp:Content>
