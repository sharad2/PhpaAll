<%@ Page Title="Employee Details Doc" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="EmployeeDetails.doc.aspx.cs" Inherits="PhpaAll.Doc.EmployeeDetails" %>
<asp:Content ID="Content6" ContentPlaceHolderID="cphNoForm" runat="server">
<br />
    <p>This page displays complete personnel information of an employee. Buttons have been
        provided to insert,updated and delete any information you want.
    </p>
    <ol>
        <li>An employee can have multipe service periods in his tenure. The most recent service
            period is shown at the top. </li>
        <li>An employee can be granted increment by filling in the details after clicking the
            Increment button.A new service period will be created starting on the increment
            date and the current service period will end. It will then be visible in 
            Service History. Likewise for promotion. </li>
        <li>The basic salary entered in the Service section is the one proposed by PIS staff
            while the basic salary in the Employee section is the one provided by Finance staff
            and is the actual basic being received by the employee </li>
        <li>You can also terminate an employee and record the reason for leaving and termination
            date. If the employee is no longer in service as on current date, then you can only
            view all his information but you cannot edit any more. </li>
        <li>You can also undo the termination by clicking on <strong>Undo Termination</strong>
            button.</li>
    </ol>

    <br />
</asp:Content>
