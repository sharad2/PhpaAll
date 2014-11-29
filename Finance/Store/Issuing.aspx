<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" 
    CodeBehind="Issuing.aspx.cs" Inherits="Finance.Store.Issuing" Title="Issuing" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphSideNavigation" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cph" runat="server">
    <dl>
        <dt>Create SRS</dt>
        <dd>
            <asp:HyperLink runat="server" NavigateUrl="~/Store/CreateSRS.aspx"
                Text="Create SRS" />
            and specify the quantity required as per your choice for each item. If the items being received
            do not exist in the item master yet, you must <a href="InsertItem.aspx">Create new items</a>.
        </dd>
        <dt>View SRS</dt>
        <dd>
            View all created SRS for a given date range through <a href="Reports/SRSList.aspx">SRS
                List</a>
        </dd>
        <dt>Check SRS Details</dt>
        <dd>
            You can enter the SRS number in
            <asp:HyperLink runat="server" NavigateUrl="~/Store/Reports/SRSReport.aspx"
                Text="SRS Details" />
            and check SRS Details.
        </dd>
        <dt>Issue or Edit SRS</dt>
        <dd>
            Store manager enters the SRS number in
            <asp:HyperLink runat="server" NavigateUrl="~/Store/Reports/IssueSRS.aspx"
                Text="SRS Details" />
            and chooses to issue or Edit against it. While issuing he can specify the quantity
            being issued.
        </dd>
        <dt>View Ledger for an Item</dt>
        <dd>
            Store manager can check the till date Item Ledger through
            <asp:HyperLink runat="server" NavigateUrl="~/Store/Reports/ItemLedger.aspx"
                Text="Item Ledger" />
        </dd>
    </dl>
</asp:Content>
