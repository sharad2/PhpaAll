<%@ Page Title="Earned Money Deposit Doc" Language="C#" MasterPageFile="~/MasterPage.master"
    AutoEventWireup="true" CodeBehind="EarnedMoneyDeposit.doc.aspx.cs" Inherits="Finance.Doc.EarnedMoneyDeposit_doc" %>

<asp:Content ID="Content6" ContentPlaceHolderID="cph" runat="server">
    
    <h3>EarnedMoneyDeposit</h3>
            <p>Displays details of advance money received from various parties for job assigned.</p>
            <ol>
                <li>By default, the date is set to the current date. This makes it convenient for you
                    to see all the Earned Money Deposit up to current date. </li>
                <li>To include postdated amounts, you will need to enter a date in the future</li>
                <li>To get to know the contractor code for any particular contractor just place the
                    cursor over the specified contractor name</li>
                <li>To get the list of all the account heads under this report do click this link
                    <asp:HyperLink ID="hplnkemd" runat="server" Text="EMD" NavigateUrl="../Finance/AccountHeads.aspx?Types=EMD"></asp:HyperLink></li>
            </ol>
        <h3>SecurityDeposit</h3>
            <p>Displays details of money received for security from various parties for job assigned.</p>
            <ol>
                <li>By default, the date is set to the current date. This makes it convenient for you
                    to see all the Earned Money Deposit up to current date.</li>
                <li>To include postdated amounts, you will need to enter a date in the future</li>
                <li>To get to know the contractor code for any particular contractor just place the
                    cursor over the specified contractor name</li>
                <li>To get the list of all the account heads under this report do click this link
                    <asp:HyperLink ID="hplnksd" runat="server" Text="SD" NavigateUrl="../Finance/AccountHeads.aspx?Types=SD"></asp:HyperLink></li>
            </ol>
</asp:Content>
