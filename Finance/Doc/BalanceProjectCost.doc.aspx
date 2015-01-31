<%@ Page Title="Balance Project Cost Doc" Language="C#" MasterPageFile="~/MasterPage.master"
    CodeBehind="BalanceProjectCost.doc.aspx.cs" Inherits="PhpaAll.Doc.BalanceProjectCost" %>

<asp:Content ID="Content6" ContentPlaceHolderID="cphNoForm" runat="server">
    <p>
        Display the Project Cost corresponding to each Job.
    </p>
    <ol>
        <li>Project Cost show's the approximate cost that has to be spend on each Project.
        </li>
        <li>Sanction issued is the total amount issued for all the Jobs under one Head of Account.
        </li>
        <li>Work Awarded display the award amount issued for all the Jobs under one Head of
            Account. </li>
        <li>Commitment is equivalent to work awarded amount. if work awarded is null then commitment
            is equal to Sanction Issued. </li>
        <li>Balance Project Cost displays the Balance amount for corresponding Project. (Balance
            Project Cost= Project Cost - Commitment) </li>
        <li>On Details click, you can view all the Jobs associated with that Head of Account.
        </li>
        <li>Subtotal field display the summation of Project Cost, Sanction Issued, Work Awarded,
            Commitment and Balance Project Cost for each level of particular Head of Account
            (e.g. 100.01, 100.02, 100.02.03 etc.). </li>
        <li>Total field display the summation of Project Cost, Sanction Issued, Work Awarded,
            Commitment and Balance Project Cost for Parent Head of that Head of Account (e.g.
            100, 200, 300 etc.). </li>
        <li>In order to view the Balance Project Cost, create a Job and mention Santion issued,
            work awarded, Commitment amount for that job. </li>
    </ol>
</asp:Content>
