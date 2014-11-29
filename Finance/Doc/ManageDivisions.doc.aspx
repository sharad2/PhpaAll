<%@ Page Title="ManageDivisions Doc" Language="C#" MasterPageFile="~/MasterPage.master"
    AutoEventWireup="true" CodeBehind="ManageDivisions.doc.aspx.cs" Inherits="Finance.Doc.ManageDivisions" %>

<asp:Content ID="Content6" ContentPlaceHolderID="cphNoForm" runat="server">
    <ol>
        <li>Display <b>Divisions</b> along with their <b>Division Name</b> and <b>Division Code</b>.</li>
        <li><b>Login is required to Create New Division and modify the existing Divisions.</b></li>
        <li>Allows you to <b>Add</b>, <b>Update</b> and <b>Delete</b> the Divisions.</li>
        <li>You can easily <b>Create New Divisions</b> by clicking <b>Create New Division</b>
            link. On Click of <b>Create New Divisions</b>, a new row with appear at the top
            of the grid which allows you to create new Division.</li>
        <li>In insert template you have <b>Division Name</b>, <b>Division Code</b> and <b>Division
            Group</b>.</li>
        <li><b>Division Name</b> and <b>Division Group</b> are mandatory fields.</li>
        <li><b>Division Group</b> is a large unit that can have multiple Division. <b>Division
            Group</b> can either be choosen from the List or you can create your own Division
            Group.</li>
        <li>To <b>Create New Division</b> by select <b>(New Group)</b> in DropDownList and
            Entering group <b>New Group Name</b> in TextBox.</li>
        <li>You can edit the details of any Division by clicking <b>Edit</b> button given in
            the Division List.</li>
        <li>You can <b>Update</b> and <b>Delete</b> the Division, while Deleting any Division
            make sure that this Division is not associated with any Job.</li>
    </ol>
</asp:Content>
