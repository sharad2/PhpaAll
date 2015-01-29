<%@ Page Title="Party Advances Doc" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="PartyAdvances.doc.aspx.cs" Inherits="PhpaAll.Doc.PartyAdvances" %>

<asp:Content ID="Content6" ContentPlaceHolderID="cphNoForm" runat="server">
<br />
<p>Displays all the Contractors and Advance Amount drawn by him in financial year of
    selected date. You may specify the date.</p>
    <ol>
        <li>By default,this report displays advance drawn by contractors up to current date
            in current financial year.If you are interested in seeing the entries of an older
            statement, just change the date. </li>
        <li>Total Advance computed as Advance + Material Advance. </li>
        <li>Negative amount displayed as (amount). </li>
    </ol>
</asp:Content>
