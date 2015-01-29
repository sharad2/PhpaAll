<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" CodeBehind="LearnMorePayroll.aspx.cs" Inherits="PhpaAll.Payroll.LearnMorePayroll" Title="Untitled Page" %>
<asp:Content ID="Content3" ContentPlaceHolderID="cphSideNavigation" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cph" runat="server">
<p>
If you are using Payroll program for the first time, this checklist can help you in getting started.
</p>
<h3>Adjustment Creation</h3>
<p>
If you intend to modify any data, you must ask your administrator to create a login for you. For reviewing the information, no
login is needed. If you are ready to login, click 
    <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="~/Login.aspx">here</asp:HyperLink>. If you are an administrator,
    you can create a new login account by clicking 
    <asp:HyperLink ID="HyperLink2" runat="server" NavigateUrl="~/ManageUsers.aspx">here</asp:HyperLink>.
</p>
<p>
View the currently existing <asp:HyperLink ID="HyperLink3" runat="server" NavigateUrl="~/Payroll/Adjustment.aspx">Adjustments</asp:HyperLink>.
At least some adjustments must be created before you are able to create paybill.
</p>
<p>
<asp:HyperLink ID="HyperLink4" runat="server" NavigateUrl="~/Payroll/EmployeeAdjustments.aspx">Employee Adjustment Manager</asp:HyperLink> to manage adjustments of an employee.
This serves as the template for creating the payroll for each new month.
</p>
<h3>Creating Paybill</h3>
<p>
To begin with you have to define Salary Period for which you want to pay salary.Use 
<asp:HyperLink ID="HyperLink5" runat="server" NavigateUrl="~/Payroll/SalaryPeriods.aspx">Salary Periods</asp:HyperLink> to create salary period.
</p>
<p>

</p>
<p>
After you have created a payroll period you will be able to create Salary bill for that period on the basis of 
adjustments applicable. 

</p>
<p>
View the <asp:HyperLink ID="HyperLink8" runat="server" NavigateUrl="~/Payroll/Reports/PayBill.aspx">Pay Bill</asp:HyperLink> to see the
created Paybills.You can choose a period and see paybill of that period.
</p>
<p>
View the <asp:HyperLink ID="HyperLink9" runat="server" NavigateUrl="~/Reports/BankBook.aspx">Bank Book</asp:HyperLink> to see the
bank transactions. In this report, you can select any head of account which you have designated as a bank account.
</p>
</asp:Content>
