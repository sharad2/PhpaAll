<%@ Page Title="Paybill Doc" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="Paybill1.doc.aspx.cs" Inherits="Finance.Doc.Paybill1" %>

<asp:Content ID="Content6" ContentPlaceHolderID="cphNoForm" runat="server">
<br />
 <p>Displays Pay Bill for the given month and for given parameters.</p>
    <ol>
        <li>The Salary period end date is used to decide which month the salary is for. Thus
            if you enter March 2009 as the month, all salary periods whose end date is between
            1 March 2009 and 31 March 2009 will be included in the pay bill. </li>
        <li>On click of <b>Show Pay Bill</b> button report shows paybill for all the employees
            for the given month. </li>
        <li>You have given with the filters which allows you to filter your paybill as per as
            your requirement. Select any filter and click on <b>Show Pay Bill button</b>, your
            desirable result will appear on the screen. If no Pay Bill found for the given month,
            a message <b>"Paybill not found for the given month"</b> will be displayed.
        </li>
        <li>You have given with a option to clear all the filters. On click of <b>Clear Filters
            button</b> all the applied filter set to default. </li>
    </ol>
</asp:Content>
