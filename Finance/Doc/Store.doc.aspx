<%@ Page Title="Store Doc" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="Store.doc.aspx.cs" Inherits="Finance.Doc.Store" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
 
<asp:Content ID="Content3" ContentPlaceHolderID="cphSideNavigation" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="contentRight" runat="server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cph" runat="server">
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="cphNoForm" runat="server">
  <br />
    <p>
        Welcome to the store section. From here you can manage stocks and the requirements
        issued by various departments (telephonically, verbally or through letters). After
        the requirement is processed and goods arrive in the stock, you need to receive
        them. The main highlights of <strong>Stores</strong> are as follows -
    </p>
    <dl>
        <dt>Adding Items to stock</dt>
        <dd>
            Adding or editing item to stock involves following steps -
            <dl>
                <dt><a href="../Store/Store.aspx">Creating a New GRN</a></dt>
                <dd>
                    You can create new GRN from <i>Create Goods Receipt Notes (GRN)</i> under <strong>Receiving</strong>
                    menu.
                </dd>
                <dt style="font-weight: bold"><a href="../Store/RecieveGRN.aspx">Updating stocks (Receive GRN)</a></dt>
                <dd>
                    Receive the GRN created, from <i>Receive GRN</i> under <strong>Receiving</strong>
                    menu. By receiving the GRN the stock will get updated with the items just recieved.
                </dd>
                <dt style="font-weight: bold"><a href="../Store/GrnList.aspx">Edit existing GRN</a></dt>
                <dd>
                    Items in a GRN can be edited from <i>GRN List</i> in the <strong>Recieving</strong>
                    menu.
                    <%--<ol>
                    <li>Go to <a href="GrnList.aspx">GRN List</a> in the <strong>Recieving</strong> menu.</li>
                    <li>Fetch the GRN need to be edited by specifying the known information in the filter section.</li>
                    <li>Click on the GRN No. you wish to edit and choose "Edit GRN".</li>
                </ol>--%>
                </dd>
            </dl>
        </dd>
        <dt style="font-weight: bold">Issuing Items from stock.</dt>
        <dd>
            The next step would be the fulfillment of requirement of various departments. You
            can issue the items by following the steps below -
            <dl>
                <dt style="font-weight: bold">Specify the requirement of departments. (Create SRS)</dt>
                <dd>
                    To specify the requirement of any department, go to <i><a href="../Store/CreateSRS.aspx">Create
                        SRS</a></i> under <strong>Issuing</strong> menu.
                </dd>
                <dt style="font-weight: bold">Deliver Items to the departments. (Issue SRS)</dt>
                <dd>
                    You need to issue the Items to the department whose SRS just created. You can issue
                    the items from <i><a href="../Store/CreateSRS.aspx">Goods Issue Notes</a></i> under <strong>Issuing</strong>
                    menu.
                </dd>
                <dt style="font-weight: bold"><a href="../Store/Reports/SRSReport.aspx">Edit existing SRS</a></dt>
                <dd>
                    Items in an existing SRS can be edited from <i>GIN Report</i> in the <strong>Issuing</strong>
                    menu.</dd>
            </dl>
        </dd>
    </dl>
    <h4>
        Following are the few more features avaialble in this section -</h4>
    <dl>
        <dt style="font-weight: bold">Manage Unit of Measurement:</dt>
        <dd>
            Go to <i>Manage Unit of Measurement</i> under <strong>Manage Items</strong> menu.
            You can use this to specify the unit of measurements. e.g. PIECES, KILOs, Liters,
            etc.
        </dd>
        <dt style="font-weight: bold">Stock Balance: </dt>
        <dd>
            Click on <i>Stock Balance</i> to see the stock balance till date. view
        </dd>
        <dt style="font-weight: bold">Item Ledger:</dt>
        <dd>
            This report shows you the receipt and issue summary for an item for the specified
            date range.</dd>
        <dt style="font-weight: bold">GRN List:</dt>
        <dd>
            Go to <i>GRN List</i> under <strong>Receiving</strong> menu. It displays the list
            of GRNs catgorically as "UnReceived" or "Received" or "All". By default it shows
            the list of "UnReceived" GRNs.</dd>
        <dt style="font-weight: bold">Material Receipt Per GRN:</dt>
        <dd>
            Go to <i>Material Receipt</i> under <strong>Receiving</strong> menu. It shows Material
            Receipt per GRN.</dd>
        <dt style="font-weight: bold">Material Rejected Per GRN:</dt>
        <dd>
            Go to <i>Material Rejected</i> under <strong>Receiving</strong> menu. It shows the
            items which were not fully received while receiving the GRN.</dd>
        <dt style="font-weight: bold">GRN Report:</dt>
        <dd>
            Go to <i>GRN Report</i> under <strong>Receiving</strong> menu. This reports shows
            GRN summary for the selected GRN.</dd>
        <dt style="font-weight: bold">GRN's per Purchase Order:</dt>
        <dd>
            Go to <i>GRN's per Purchase Order</i> under <strong>Receiving</strong> menu. It
            shows GRN list for any specfied Purchase Order.</dd>
        <dt style="font-weight: bold">SRS List:</dt>
        <dd>
            Go to <i>SRS List</i> under <strong>Issuing</strong> menu. The list of SRS Created
            for the given date range can be seen from here.</dd>
        <dt style="font-weight: bold">GIN Report:</dt>
        <dd>
            Go to <i>GIN Report</i> under <strong>Issuing</strong> menu. It shows SRS summary
            for the specified SRS No. You can also edit the following details -
            <ul>
                <li>Change SRS Recieve date</li>
                <li>Change SRS No.</li>
                <li>Change the Division from where the SRS has been cerated</li>
                <li>Change the Division to which the SRS will be delivered</li>
                <li>Change the designation to which the SRS will be issued</li>
                <li>Change the vehicle no.</li>
                <li>Change the Item code, Head Of Account and quantity required.</li>
            </ul>
        </dd>
        <dt style="font-weight: bold">Items Issued Per Division </dt>
        <dd>
            Go to <i>Items Issued Per Division</i> under <strong>Issuing</strong> menu. View
            items issued for the specified Division or SRS No or any specific item.
        </dd>
    </dl>

</asp:Content>
