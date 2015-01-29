<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" CodeBehind="CashBook.aspx.cs"
    Inherits="PhpaAll.Reports.CashBook" Title="Cash Book" EnableViewState="false" %>

<%@ Register Src="../Controls/VoucherDetailControl.ascx" TagName="VoucherDetailControl"
    TagPrefix="uc1" %>
<%@ Register Src="../Controls/PrinterFriendlyButton.ascx" TagName="PrinterFriendlyButton"
    TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphSideNavigation" runat="server">
    <asp:HyperLink ID="HyperLink2" runat="server" Text="Help" NavigateUrl="~/Doc/CashBook.doc.aspx" /><br />
    <uc2:PrinterFriendlyButton ID="PrinterFriendlyButton1" runat="server" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cph" runat="server">
    <eclipse:TwoColumnPanel runat="server" IsValidationContainer="true">
        <eclipse:LeftLabel runat="server" Text="Date Range" />
        <i:TextBoxEx ID="tbDateFrom" runat="server" FriendlyName="Date From">
            <Validators>
                <i:Date />
                <i:Required />
            </Validators>
        </i:TextBoxEx>
        <i:TextBoxEx ID="tbDateTo" runat="server" FriendlyName="Date To">
            <Validators>
                <i:Date DateType="ToDate" />
                <i:Required />
            </Validators>
        </i:TextBoxEx>
        <eclipse:LeftLabel ID="LeftLabel1" runat="server" Text="Head of Account" />
        <i:AutoComplete ID="tbHeadOfAccount" runat="server" FriendlyName="HeadOfAccount"
            QueryString="HeadOfAccount" Width="20em" WebMethod="GetHeadOfAccount" WebServicePath="~/Services/HeadOfAccounts.asmx"
            ValidateWebMethodName="ValidateHeadOfAccount">
            <Validators>
                <i:Required />
            </Validators>
        </i:AutoComplete>
        <br />
        <i:ButtonEx ID="btnShowReport" runat="server" Text="Show Cash Book" CausesValidation="true"
            Action="Submit" Icon="Refresh" />
        <i:ValidationSummary ID="ValidationSummary1" runat="server" />
    </eclipse:TwoColumnPanel>
    <br />
    <div style="text-align: center">
        <asp:Label ID="lblHeadOfAccount" CssClass="headerStyle" runat="server" />
    </div>
    <br />
    <uc1:VoucherDetailControl ID="ctlVoucherDetail" DebitHeader="Payments" CreditHeader="Receipts"
        runat="server" InformationLevel="Detail">
    </uc1:VoucherDetailControl>
</asp:Content>
