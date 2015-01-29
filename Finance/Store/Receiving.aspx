<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" CodeBehind="Receiving.aspx.cs"
    Inherits="PhpaAll.Store.Receiving" Title="Receiving" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphSideNavigation" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cph" runat="server">
    <dl>
        <dt>Truck carrying goods arrives at India - Bhutan border</dt>
        <dd>
            <asp:HyperLink runat="server" NavigateUrl="~/Store/CreateGRN.aspx" Text="Create GRN" />
            and specify quantity ordered and quantity delivered for each item. If the items
            being received do not exist in the item master yet, you must <a href="InsertItem.aspx">
                Create new items</a>.
        </dd>
        <dt>Truck reaches the store at project site</dt>
        <dd>
            Store manager enters the GRN number in
            <asp:HyperLink runat="server" NavigateUrl="~/Store/Reports/GRNReport.aspx" Text="GRN Details" />
            and chooses to receive against it. While receiving he specifies the quantity being
            accepted.
        </dd>
        <dt>Track inventory at any moment of time.</dt>
        <dd>
            Store manager can check the Stock Balance after issuing and receiving through
            <asp:HyperLink runat="server" NavigateUrl="~/Store/Reports/StockBalance.aspx" Text="Stock Balance" />
        </dd>
        <dt>View GRNs</dt>
        <dd>
            View unreceived and received GRN's for a given date range through <a href="GrnList.aspx">
                GRN List</a> or view GRNs against a <a href="Reports/POGRNs.aspx">Specific Purchase
                    Order</a>.</dd>
        <dt>View Receipts</dt>
        <dd>
            To check the Material Received and Rejected per GRN for a given date range, click
            on <a href="Reports/MaterialReceipt.aspx">Material Receipt</a> and <a href="Reports/MaterialReceipt.aspx?ReportName=Rejected">
                Material Rejected</a> respectively.</dd>
    </dl>
</asp:Content>
