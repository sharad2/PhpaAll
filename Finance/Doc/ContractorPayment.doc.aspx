<%@ Page Title="Contractor Payment Doc" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="ContractorPayment.doc.aspx.cs" Inherits="Finance.Doc.ContractorPayment" %>
<asp:Content ID="Content6" ContentPlaceHolderID="cphNoForm" runat="server">
<br />
<ol>
        <li>Displays payment register(expenditure and recoveries) for a job you specified.
        </li>
        <li>Total Recovery calculated as (Contract Tax + Security Deposit + Advance + Material
            Advance + Interest + Others). </li>
        <li>Net Payment calculated as (Amount + Advance - Total Recovery). </li>
        <li>Balance calculated as (Advance-Advance Adjustment)</li>
        <li>Report also displays Voucher even if contractor for the same does not exist, this
            provides the info that job provided for the given voucher dosen't contains the required
            contractor so contractor must be added to the job to get the condition right.</li>
    </ol>
</asp:Content>
