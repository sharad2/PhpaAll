<%@ Page Title="Expenditure Doc" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="Expenditure.doc.aspx.cs" Inherits="PhpaAll.Doc.Expenditure" %>

<asp:Content ID="Content6" ContentPlaceHolderID="cphNoForm" runat="server">
    <br />
    <p>Display the expenditure for each Head of Account.</p>
    <ol>
    <li>
    Display expenditure incurred in each Head of Account. 
    </li>
    <li>Current date is selected in <b>For Date</b> text box by default.</li>
    <li>Report showing data for the selected date..</li>
    <li>In order to view the report, create voucher against any expenditure head. You can view that entry 
    in Expenditure report for the given date.</li>
    <li>
    Report showing <b>Head of Account</b>, <b>Project Cost</b>, <b>Upto Previous Year</b>, <b>Current Year During the Month</b>, 
    <b>Current Year Upto the Month</b>, <b>Cumulative</b> columns..
    </li>
    <li>
    <b>Project cost</b> shows the estimated project cost defined for Head of Account.
    </li>
    <li>
    <b>Upto Previous year</b> display expenditure up to previous financial year.
    </li>
    <li>
    <b>Current Year During the Month and Upto the month</b> display expenditure for Current month and upto the month for current financial year respectively.
    </li>
    <li>
    You can <b>verify During the month and Upto the month expenditure by clicking the cell text</b>. You will be redirected to the screen which will show 
    you the <b>details of expenditure occurred</b>.
    </li>
    <li>
    <b>Cumulative</b> display till date expenditure for each Head of Account.
    </li>
    <li>You can check all expenditure Head of Account by clicking <b>Click here to view the Expenditure Head of Accounts</b> given at the top of the report.</li>
    </ol>
</asp:Content>
