<%@ Page Title="Insert Voucher Doc" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="InsertVoucher.doc.aspx.cs" Inherits="PhpaAll.Doc.InsertVoucher" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
 
<asp:Content ID="Content3" ContentPlaceHolderID="cphSideNavigation" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="contentRight" runat="server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cph" runat="server">
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="cphNoForm" runat="server">
    <p>
        To create a new Voucher, click on the Add new Voucher link you see when the page
        first loads. Once you have created a voucher, it displays in the second half of
        the screen and the top half clears to allow you to create another voucher.
    </p>
    <p>
        To modify a voucher you can click edit in the second half of the screen and the
        voucher becomes editable in the top half.
    </p>
    <p>
        The screen also displays a list of recently created and modified vouchers. In case
        you want to get back to a voucher which you created or modified a short while ago,
        look in this list. The voucher should be visible. Once you find that voucher, you
        can click on it to view all its details.
    </p>
    <h3>
        Employee Vouchers</h3>
    <p>
        If you would like to create a voucher for one or more employees, click Show Employee
        while creating the voucher. In this situation, an additional employee column becomes
        available where you can enter the employee code. The Payee column becomes read only
        since the Payee will be automatically set to Employee Name - Designation. If you
        enter multiple employees, the Payee will indicate <i>Multiple Employees</i>.
    </p>
    <p>
        When you update this voucher, you will have the option to update the payee as well.
        Note that if you update the employee, it is your responsibility to update the payee
        as well. While updating, the payee is not automatically set.
    </p>
    <h3>
        Job Vouchers</h3>
    <p>
        Create Voucher screen contains 3 Columns for inserting. If you would like to insert
        voucher against a Job, select show Job from the More Columns(Drop down List) given
        at the top of the inserting table. On selecting of show Job, a new column will be
        added in the table which allows you to choose Job against which you would like to
        create the voucher.</p>
    <b>Follow the steps while creating voucher against Jobs.</b>
    <ol>
        <li>Select the associated Division for the Job against which you are going to create
            the voucher.</li>
        <li>No need to give payee/sender's name, it will be set automatically corresponding
            to the job selected.</li>
        <li>While creating voucher against Job, select the job in the Job column and select
            the Head of Account corresponding to the Job selected. </li>
        <li>If you are updating the Job Voucher, update the Payee and sender name as well.</li>
    </ol>
</asp:Content>
