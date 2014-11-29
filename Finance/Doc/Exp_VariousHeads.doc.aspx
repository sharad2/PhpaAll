<%@ Page Title="Exp_VariousHeads Doc" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="Exp_VariousHeads.doc.aspx.cs" Inherits="Finance.Doc.Exp_VariousHeads" %>

<asp:Content ID="Content6" ContentPlaceHolderID="cphNoForm" runat="server">
<br />
<p>Displays Expenditure Incurred on various Jobs</p>
    <ol>
        <li>Expenditure incurred on various jobs will be displayed in accordance to division i.e.
            only expenditure for those jobs will be displayed for which the division has been
            selected.</li>
        <li>All the expenditure for previous year will be displayed as consolidated amount under
            heading Expenditure b/d.</li>
        <li>For current year detailed expenditure is displayed in table for each Job respectively.</li>
        <li>By default no data will be displayed since no division has been selected</li>
        <li>The date parameter for the report is current date i.e. it will display all the expenditure
            for jobs up to current date</li>
    </ol>
    If no data found
    <ul>
        <li>Verify whether any job exist for the given division, this is possible through Job
            screen.</li>
        <li>If Job exists, then next verify whether there is any voucher against the given job,
            this can be verified through one of the report of your job type i.e Expenditure
            against Contracts, Expenditure against WorkOrder and Expenditure against departmentally
            executed Jobs.</li>
    </ul>
</asp:Content>
