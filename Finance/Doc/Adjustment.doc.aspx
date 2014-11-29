<%@ Page Title="Adjustment Doc" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="Adjustment.doc.aspx.cs" Inherits="Finance.Doc.Adjustment" %>

<asp:Content ID="Content6" ContentPlaceHolderID="cphNoForm" runat="server">
<br />
<p>
        An employee's salary consists of basic pay along with the <i>allowances</i> and <i>deductions</i>.
        The term <i>adjustment</i> refers to allowances as well as deductions.
    </p>
    <p>
        When you define the salary of an employee, you can also define the allowances and
        deductions that must be applied to the basic salary. The adjustment can be:
    </p>
    <ul>
        <li>A flat amount, such as newspaper allowance. </li>
        <li>A percentage of basic salary, such as income tax. </li>
        <li>It is less common, though permissible to have both a flat amount and percentage
            of basic salary for an adjustment. </li>
        <li>Neither the flat amount or the percentage is required. This would be useful when
            the allowance amount varies widely from employee to employee, such as performance
            bonus. </li>
    </ul>
    <p>
        This screen allows you to define all allowances which are used by your payroll department.
        Remember that you can override the values you provide here on a per employee or
        even a per payroll period basis. To facilitate creating payroll for new employees,
        you can designate that the allowance should be applied automatically for all employees
        or for employees of a specific type. This way, when the payroll for a new employee
        is created, the automatic allowances show up by default. You still have the option
        of removing these automatic allowances for specific employees.
    </p>
</asp:Content>
