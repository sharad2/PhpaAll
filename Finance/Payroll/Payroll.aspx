<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" CodeBehind="Payroll.aspx.cs"
    Inherits="PhpaAll.Payroll.Payroll" Title="Payroll Home Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphNoForm" runat="server">
    <h3>
        Getting Started</h3>
    <p>
        Salary of an employee consists of Basic Salary, Allowances and Deductions. Allowances
        and Deductions together are termed as <em><b>Adjustments</b></em>. Following are
        the basic steps involved in creating the salary for an employee.
    </p>
    <ol>
        <li>
            <asp:HyperLink runat="server" NavigateUrl="~/PIS/Employees.aspx">Create Employee</asp:HyperLink>.
            Search for the employee you are interested in. If this employee has not yet been
            created, create him now. Once the employee exists and his basic salary has been
            defined then you are ready to define allowances and deductions. </li>
        <li>
            <asp:HyperLink runat="server" NavigateUrl="~/Payroll/Adjustment.aspx">Manage Adjustments</asp:HyperLink>.
            Ensure that allowances and deductions which will apply to this employee already
            exist, and create them if ncessary. Adjustments must be created before you are able
            to create paybill. </li>
        <li>
            <asp:HyperLink runat="server" NavigateUrl="~/Payroll/EmployeeAdjustments.aspx">Associate Adjustments With Employee</asp:HyperLink>.
            This serves as the template for creating the payroll for each new month. </li>
    </ol>
    <p>
        At this point, we have defined what salary the employee is entitled to, but we have
        not yet paid any salary to this employee. To pay salary, you must create salary
        period and decide which employees must be paid during that period. This is explained
        in more detail below.
    </p>
    <h3>
        Paybill Creation</h3>
    <ol>
        <li>
            <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="~/Payroll/SalaryPeriods.aspx">Define Salary Period</asp:HyperLink>.
            You have to define Salary Period for which you want to pay the salary. The most
            important information about the salary period is <b>Start Date</b> and <b>End Date</b>.
            If you are intending to pay salaries for June 2008, the start date would be 1 Jun
            2008 and the end date would be 31 Jun 2008. </li>
        <li>
            <asp:HyperLink ID="HyperLink5" runat="server" NavigateUrl="~/Payroll/SalaryPeriods.aspx">Add Employees to Salary Period</asp:HyperLink>.
            You must explicitly decide who to pay during the salary period. When you add an employee
            to the salary period, their basic salary and adjustments are copied from what you have defined
            for the employee. Make period specific changes to adjustments as needed.</li>
        <li><asp:HyperLink ID="HyperLink2" runat="server" NavigateUrl="~/Payroll/SalaryPeriods.aspx">Pay Salary</asp:HyperLink>.
        This simply means that you update the Paid Date of the Salary Period for which salaries have been paid. The
        system will prevent you from modifying this salary period after paid date has been entered.
        </li>
    </ol>
    <h3>
        View what you have entered</h3>
    <p>
        View the
        <asp:HyperLink ID="HyperLink8" runat="server" NavigateUrl="~/Payroll/Reports/PayBill1.aspx">Pay Bill</asp:HyperLink>
        to see the created Paybills.You can choose a period and see paybill for that period.
    </p>
    <p>
        View
        <asp:HyperLink ID="HyperLink7" runat="server" NavigateUrl="~/Payroll/Reports/GIS.aspx">GIS Recovery</asp:HyperLink>
        to see the GIS recovered from employees for a period.
    </p>
    <p>
        View
        <asp:HyperLink ID="HyperLink9" runat="server" NavigateUrl="~/Payroll/Reports/GPF.aspx">GPF Recovery</asp:HyperLink>
        to see the GPF recovered from employees for a period.
    </p>
    <p>
        View
        <asp:HyperLink ID="HyperLink10" runat="server" NavigateUrl="~/Payroll/Reports/PaybillRegister.aspx">PayBillRegister</asp:HyperLink>
        to see the PayBillRegister for a employee.
    </p>
    <p>
        View
        <asp:HyperLink ID="HyperLink11" runat="server" NavigateUrl="~/Payroll/Reports/STHC.aspx">STHC Report</asp:HyperLink>
        to see the Recovery of Personal income tax and health contribution.
    </p>
</asp:Content>
