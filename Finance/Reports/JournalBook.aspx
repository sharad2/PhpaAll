<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" CodeBehind="JournalBook.aspx.cs"
    Inherits="Finance.Reports.JournalBook" Title="Journal Book" EnableViewState="false" %>

<%@ Register Src="../Controls/PrinterFriendlyButton.ascx" TagName="PrinterFriendlyButton"
    TagPrefix="uc2" %>
<%@ Register Src="../Controls/VoucherDetailControl.ascx" TagName="VoucherDetailComtrol"
    TagPrefix="uc1" %>
<asp:Content ID="Content4" ContentPlaceHolderID="cphSideNavigation" runat="server">
    <asp:HyperLink ID="HyperLink2" runat="server" Text="Help" NavigateUrl="~/Doc/JournalBook.doc.aspx" /><br />
    <uc2:PrinterFriendlyButton ID="PrinterFriendlyButton1" runat="server" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cph" runat="server">
    Display all the journal vouchers.
    <br />
    <eclipse:TwoColumnPanel runat="server">
        <eclipse:LeftLabel runat="server" Text="From Date/To Date" />
        <i:TextBoxEx ID="tbFromDate" runat="server" FriendlyName="From Date">
            <Validators>
                <i:Required />
                <i:Date Max="0" />
            </Validators>
        </i:TextBoxEx>
        <i:TextBoxEx ID="tbToDate" runat="server" FriendlyName="To Date">
            <Validators>
                <i:Required />
                <i:Date DateType="ToDate" />
            </Validators>
        </i:TextBoxEx>
    </eclipse:TwoColumnPanel>
    <br />
    <i:ButtonEx ID="btnShowReport" runat="server" Action="Submit" Icon="None" Text="Show Journal" CausesValidation="true" />
    <i:ValidationSummary ID="ValidationSummary1" runat="server" />
    <uc1:VoucherDetailComtrol ID="ctlVoucherDetail" runat="server" InformationLevel="Detail" />
</asp:Content>
