<%@ Page Title="Expenditure Against Job Type" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="ExpenditureAgainstJobType.doc.aspx.cs" Inherits="Finance.Doc.ExpenditureAgainstJobType" %>

<asp:Content ID="Content6" ContentPlaceHolderID="cphNoForm" runat="server">
<br />
<p>Display Expenditure against various Job Types.</p>
    <ol>
        <li>Display <b>expenditure</b> per <b>Division</b>.</li>
        <li>On the basis of chosen report you will get the details of <b>particular Job Type</b>
            (Like Contracts, Work order and Departmental) for respective Division. </li>
        <li>Display Job code, its description and Contractor Name who has been assigned the
            Job. </li>
        <li><b>Award Amount</b> is the contract amount sanctioned by the power authority of
            <b>PHPA</b> to the contractors. </li>
        <li><b>Progressive amount</b> is the total amount spent till date on that <b>particular
            Job</b>. </li>
        <li>In order to view the report, <b>create a job</b> from Job screen and assigned that
            job to any contractor. </li>
        <li>Job should be in one of the <b>Division</b> and its type should be specified. E.g.
            Work Order, Contract or Departmentally Executed Job. </li>
        <li>In order to view <b>Expenditure against Contracts</b>, create a voucher against
            the Job whose Job Type is Contract and select Division corresponding to the Job.
            Same process is valid for Expenditure against Work Order and Expenditure against
            Departmentally executed jobs. While creating voucher please select Job against which
            you want to issue the voucher. Debit your Job head of account or any expenditure
            Head of account and Credit your non-expenditure head. </li>
        <li><b>While creating voucher, specify the corresponding Division associated with the
            Job in order to view the entry in correct Division in the report.</b></li>
    </ol>
</asp:Content>
