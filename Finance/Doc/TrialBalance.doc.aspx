<%@ Page Title="Trial Balance" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="TrialBalance.doc.aspx.cs" Inherits="PhpaAll.Doc.TrialBalance" %>
<asp:Content ID="Content6" ContentPlaceHolderID="cphNoForm" runat="server">
<br />
<ol>
        <li>Display<b> Trial Balance</b> for each Head of Account. </li>
        <li>Display<b> During the Month Gross Debit</b> and <b>Credit Amount</b> for selected
            month in date textbox.</li>
        <li><b>During the year Debit</b> and <b>Credit</b> amount shows the entry <b>upto yesterday</b>.
            e.g. If you are viewing the report for 21<sup>st</sup> of August, During the Year
            Debit and Credit shows entry upto 20<sup>th</sup> August.</li>
        <li>Display <b>till date</b> debit and credit amount.</li>
        <li><b>In order to view the Trial Balance Report</b>, create voucher(either cash, Bank
            or Journal). You can view the voucher amount entry in Trial Balance Report for the
            given Date.</li>
        <li>By default current date is selected in the For date textbox.</li>
        <li>You can varify the During the month Gross Debit amount by simply clicking the amount
            in the table cell.You will be redirected to <b>Voucher Search</b> and you can check
            your voucher details.Same you can do for During the Year Net Debit amount. </li>
    </ol>
</asp:Content>
