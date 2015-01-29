<%@ Page Title="Cumulative Yearly Expenditure Doc" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="CumulativeYearlyExpenditure.doc.aspx.cs" Inherits="PhpaAll.Doc.CumulativeYearlyExpenditure" %>
<asp:Content ID="Content6" ContentPlaceHolderID="cphNoForm" runat="server">
 <br />
 <p>Display the Expenditure against Expenditure Head's for each financial year.</p>
    <ol>
        <li>Report shows the Expenditure incurred in last 10 years of the project. </li>
        <li>Report contains <b>Head of Account</b>, <b>Project Cost</b> and <b>previous 10 year</b>
            expenditure data along with the current financial year. </li>
        <li>In order to view the report, create voucher for any date of current financial year
            against any expenditure head of account. Amount of the voucher is added along with
            the current expenditure data available for that expenditure Head of Account.</li>
        <li>Report shows till date expenditure for the current financial year.</li>
        <li><b>Report can be best viewed in Landscape</b>.</li>
    </ol>
</asp:Content>
