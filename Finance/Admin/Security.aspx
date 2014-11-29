<%@ Page Title="Security Help" Language="C#" MasterPageFile="~/MasterPage.master"
    CodeBehind="Security.aspx.cs" Inherits="Finance.Admin.Security" EnableViewState="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphSideNavigation" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cph" runat="server">
    <p>
        This page can help you troubleshoot security issues. Web.config defines security
        on the basis of the roles to which users belong. If the user should be able to access
        a page, but is unable to, check what roles he belongs to.
    </p>
    Show Roles for:
    <i:TextBoxEx runat="server" ID="tbUser">
    </i:TextBoxEx>
    <i:ButtonEx runat="server" Action="Submit" Text="Go" OnClick="btnGo_Click" />
    <i:ButtonEx ID="btnShowAll" runat="server" Action="Submit" Text="Show All Roles"
        OnClick="btnShowAll_Click" />
    <asp:BulletedList runat="server" ID="blRoles" />
    <h2>
        Welcome to the PHPA Application default Security Policy.
    </h2>
    <p>
        Administrator has highest privilege and can do anything in the application. Rest
        of the roles have controlled access and it is discussed per module. Visitors have
        lowest level of access and can only view reports. Similarly, mangers have highest
        level of access after administrators and can perform most of the jobs except some
        specialized ones like creating users which only administrators can do. Rights are
        hierarchical and administrator is at the highest level followed by manager, operator,
        executive and visitor. If you are a manager you automatically have rights of lower
        level like say an operator.
    </p>

    <h3>
        Finance Module
    </h3>
    <table>
        <tr>
            <td>
                Visitor</td>
            <td>
                Browse the site</td>
        </tr>
        <tr>
            <td>
                Executive</td>
            <td>
                Same as Visitor</td>
        </tr>
        <tr>
            <td>
                Operator</td>
            <td>
                Create vouchers</td>
        </tr>
        <tr>
            <td>
                Manager</td>
            <td>
                Edit Vouchers, Delete Vouchers. Modify/Create/Delete contractors, jobs, account 
                heads and divisions.</td>
        </tr>
    </table>
    
      <h3>
        Payroll Module
    </h3>
  <table>
        <tr>
            <td>
                Visitor</td>
            <td>
                Browse the site. </td>
        </tr>
        <tr>
            <td>
                Executive</td>
            <td>
                Browse the site.</td>
        </tr>
        <tr>
            <td>
                Operator</td>
            <td>
                Create Salary</td>
        </tr>
        <tr>
            <td>
                Manager</td>
            <td>
                Add new adjustments, edit existing adjustments,edit basic salary of an employee.</td>
        </tr>
    </table>
    
      <h3>
        Store Module
    </h3>
  <table>
        <tr>
            <td>
                Visitor </td>
            <td>
                Browse the site.</td>
        </tr>
        <tr>
            <td>
                Executive</td>
            <td>
                Browse the site.</td>
        </tr>
        <tr>
            <td>
                Operator</td>
            <td>
                Create GRN, Receive GRN, Create SRS, Issue SRS. </td>
        </tr>
        <tr>
            <td>
                Manager</td>
            <td>
                Add new item, edit existing item,add new category, edit existing category, add new UOM,edit UOM.</td>
        </tr>
    </table>

      <h3>
        MIS Module
    </h3>
    <table>
       <tr>
            <td>
                Executive</td>
            <td>
                Browse the site.</td>
        </tr>
        <tr>
            <td>
                Operator</td>
            <td>
                Update Physical Monthly Reports, Financial Monthly Reports,Construction Schedule, Daily Progress Report for respective packages.
                For example, operator for MC1 package can edit Daily Progress Report of MC1 only.
                </td>
                
        </tr>
        <tr>
            <td>
                Manager</td>
            <td>
                Add new activity, edit existing activity.</td>
        </tr>
    </table>

</asp:Content>

