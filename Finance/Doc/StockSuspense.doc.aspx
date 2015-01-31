<%@ Page Title="Stock Suspense" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="StockSuspense.doc.aspx.cs" Inherits="PhpaAll.Doc.StockSuspense" %>

<asp:Content ID="Content6" ContentPlaceHolderID="cphNoForm" runat="server">
<br />
    <p>Displays Stock Statement starting from Financial yaear upto the month for which date is input.</p>
    <ol>
        <li>By default the date upto which Details are viewed is current date.</li>
        <li>To get the details for any of the previous years stock you need to enter a date in future.</li>
        <li>Receipt: The amount of stock received during the given period in time</li>
        <li>Issue: The amount of stock sold or disposed or lost(due to any reason) during given period in time</li>
        <li>To get to know head for any stock item just place the cursor over the specefied item</li>
        <li>To get to know all the items included in the stock suspense just place the cursor over header item</li>
    </ol>
    <ul>If no data displayed
        <li>
            Verify whether during the given period is there any expenditure incured on Stock, this can be through 
            voucher search, do provide the argument StockSuspense and VoucherDate Range for which you are expecting 
            the result in Stock Suspense.
        </li>
        <li>
            If Voucher Search does not provide any info this states that voucher for the given account type have
            not been created or if it is then headofaccounttype for those headofaccountid isn't provided which 
            can be verified from Accounthead Screen.
        </li>
        <li>To get the details of all the heads included under stock supense account do click this link 
            <asp:HyperLink ID="hplnkstock" runat="server" Text="Stock Suspense" NavigateUrl="~/Finance/AccountHeads.aspx?Types=STOCK_SUSPENSE"></asp:HyperLink></li>
    </ul>
</asp:Content>
