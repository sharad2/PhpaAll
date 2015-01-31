<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" CodeBehind="GRNReport.aspx.cs"
    Inherits="PhpaAll.Store.Reports.GRNReport" Title="GRN Report" EnableViewState="false" %>

<%@ Register Src="~/Controls/PrinterFriendlyButton.ascx" TagName="PrinterFriendlyButton"
    TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphSideNavigation" runat="server">
    <uc2:PrinterFriendlyButton ID="PrinterFriendlyButton1" runat="server" />
    <fieldset>
        <legend>Quick Links</legend>
        <ul>
            <li>
                <asp:HyperLink runat="server" NavigateUrl="~/Store/GrnList.aspx" Text="GRN List" />
            </li>
            <li>
                <asp:HyperLink runat="server" NavigateUrl="~/Store/CreateGRN.aspx" Text="Create New GRN" />
            </li>
        </ul>
    </fieldset>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cph" runat="server">
    <phpa:PhpaLinqDataSource ID="dsMaterialReceipt" runat="server" ContextTypeName="Eclipse.PhpaLibrary.Database.Store.StoreDataContext"
        TableName="GRNs" RenderLogVisible="False" OnSelecting="dsMaterialReceipt_Selecting"
        AutoGenerateWhereClause="false">
    </phpa:PhpaLinqDataSource>
    <eclipse:TwoColumnPanel ID="TwoColumnPanel1" runat="server" WidthLeft="20%" IsValidationContainer="true">
        <eclipse:LeftLabel runat="server" Text="GRN No" />
        <i:TextBoxEx ID="tbGRN" runat="server" CaseConversion="UpperCase" FriendlyName="GRN">
            <Validators>
                <i:Required />
            </Validators>
        </i:TextBoxEx>
        <br />
        Enter the GRN No to view its details or <a href="../GrnList.aspx">select from list</a>.
        <br />
        <i:ButtonEx ID="btnShowReport" runat="server" IsDefault="true" Text="Show GRN" Action="Submit"
            Icon="Refresh" CausesValidation="true" />
        <i:ValidationSummary ID="valSummaryCode" runat="server" />
    </eclipse:TwoColumnPanel>
    <asp:MultiView runat="server" ID="mvEdit" ActiveViewIndex="-1">
        <asp:View ID="View1" runat="server">
            <asp:HyperLink ID="btnReceive" Text="Receive" runat="server" CssClass="noprint" />&nbsp;&nbsp;
            <asp:HyperLink ID="btnEdit" runat="server" Text="Edit" CssClass="noprint" />
            <asp:ValidationSummary ID="valSummary" runat="server" />
        </asp:View>
        <asp:View ID="View2" runat="server">
            <asp:Label ID="lblError" runat="server" Text="Login to receive or edit this GRN."
                ForeColor="Red" />
        </asp:View>
    </asp:MultiView>
    <asp:FormView ID="fvMaterialReceipt" runat="server" OnItemCreated="fvMaterialReceipt_ItemCreated"
        DataSourceID="dsMaterialReceipt">
        <ItemTemplate>
            <div style="float: left; width: 40%">
                <eclipse:TwoColumnPanel ID="panelDetails" runat="server" WidthLeft="50%" WidthRight="50%"
                    SkinID="PrintVisible">
                    <eclipse:LeftLabel runat="server" Text="GRN No." />
                    <asp:Label runat="server" Text='<%# Eval("GRNCode") %>' />
                    <eclipse:LeftLabel runat="server" Text="Goods Create Date" />
                    <asp:Label runat="server" Text='<%# Eval("GRNCreateDate", "{0:d}") %>' />
                    <eclipse:LeftLabel runat="server" Text="Goods Receive Date" />
                    <asp:Label runat="server" Text='<%# Eval("GRNReceiveDate", "{0:d}") %>' />
                    <eclipse:LeftLabel runat="server" Text="Purchase Order No." />
                    <asp:Label runat="server" Text='<%# Eval("PONumber") %>' />
                    <eclipse:LeftLabel runat="server" Text="Purchase Order Date" />
                    <asp:Label runat="server" Text='<%# Eval("PODate", "{0:d}") %>' />
                    <eclipse:LeftLabel ID="LeftLabel20" runat="server" Text="Amendment No" />
                    <asp:Label ID="Label18" runat="server" Text='<%# Eval("AmendmentNo") %>' />
                    <eclipse:LeftLabel ID="LeftLabel22" runat="server" Text="Amendment Date" />
                    <asp:Label ID="Label19" runat="server" Text='<%# Eval("AmendmentDate","{0:d}") %>' />
                    <eclipse:LeftLabel runat="server" Text="Delivery Challan No." />
                    <asp:Label runat="server" Text='<%# Eval("DeliveryChallanNumber") %>' />
                    <eclipse:LeftLabel runat="server" Text="Delievery Challan Date" />
                    <asp:Label runat="server" Text='<%# Eval("DeliveryChallanDate", "{0:d}") %>' />
                    <eclipse:LeftLabel runat="server" Text="Invoice No." />
                    <asp:Label runat="server" Text='<%# Eval("InvoiceNo") %>' />
                    <eclipse:LeftLabel runat="server" Text="Invoice Date" />
                    <asp:Label runat="server" Text='<%# Eval("InvoiceDate", "{0:d}") %>' />
                </eclipse:TwoColumnPanel>
            </div>
            <div style="float: left; width: 50%">
                <eclipse:TwoColumnPanel ID="panelDetails1" runat="server" WidthLeft="50%" WidthRight="50%"
                    SkinID="PrintVisible">
                    <eclipse:LeftLabel runat="server" Text="Supplier's Name" />
                    <asp:Label runat="server" Text='<%# Eval("RoContractor.ContractorName") %>' />
                    <eclipse:LeftLabel ID="LeftLabel1" runat="server" Text="Supplier's Address" />
                    <asp:Label ID="Label1" runat="server" Text='<%# Eval("RoContractor.Address") %>' />
                    <eclipse:LeftLabel runat="server" Text="Mode Of Transport" />
                    <asp:Label runat="server" Text='<%# Eval("TransportationMode") %>' />
                    <eclipse:LeftLabel runat="server" Text="Lorry/Railway Receipt No." />
                    <asp:Label runat="server" Text='<%# Eval("ConveyenceReceiptNo") %>' />
                    <eclipse:LeftLabel runat="server" Text="Other Reference No." />
                    <asp:Label runat="server" Text='<%# Eval("OtherReferenceNumber") %>' />
                    <eclipse:LeftLabel runat="server" Text="Name of the Carrier" />
                    <asp:Label runat="server" Text='<%# Eval("NameofCarrier") %>' />
                    <eclipse:LeftLabel runat="server" Text="Address of the Carrier" />
                    <asp:Label runat="server" Text='<%# Eval("AddressOfCarrier") %>' />
                    <eclipse:LeftLabel runat="server" Text="Order Placed" />
                    <asp:Label runat="server" Text='<%# Eval("OrderPlaced") %>' />
                    <eclipse:LeftLabel ID="LeftLabel2" runat="server" Text="Remark" />
                    <%--<asp:TextBox runat="server" Text='<%# Eval("Remarks") %>' ReadOnly="true" TextMode="MultiLine" 
                        style="overflow:hidden"   BorderStyle="None" 
                        BorderWidth="0"  Rows="6" />--%>
                    <br />
                    <asp:Label ID="Label2" runat="server" Text='<%# Eval("Remarks") %>' />
                    <br />
                </eclipse:TwoColumnPanel>
                <br />
            </div>
            <div style="clear: both">
                <br />
                <br />
            </div>
            <jquery:GridViewEx ID="gvMaterialReceipt" runat="server" AutoGenerateColumns="false"
                ShowFooter="true" Caption="<b>GRN Details</b>" OnRowDataBound="gvMaterialReceipt_RowDataBound">
                <Columns>
                    <eclipse:SequenceField />
                    <eclipse:MultiBoundField DataFields="ItemCode" HeaderText="Item|Code" ItemStyle-Width="2in"
                        SortExpression="ItemCode" AccessibleHeaderText="ItemCode">
                        <ItemStyle Width="4em" />
                        <FooterStyle Width="4em" />
                    </eclipse:MultiBoundField>
                    <eclipse:MultiBoundField DataFields="Description,Brand,Color,Dimension" HeaderText="Item|Description"
                        ItemStyle-Width="4in" DataFormatString="{0} {1} {2} {3}">
                        <ItemStyle Width="11em" />
                        <FooterStyle Width="11em" />
                    </eclipse:MultiBoundField>
                    <eclipse:MultiBoundField DataFields="Size" HeaderText="Item|Size" SortExpression="Size"
                        HideEmptyColumn="true" />
                    <eclipse:MultiBoundField DataFields="Identifier" HeaderText="Item|Identifier" SortExpression="Identifier"
                        HideEmptyColumn="true" />
                    <eclipse:MultiBoundField DataFields="ItemUnit" HeaderText="Item|Unit" ItemStyle-Width="2in"
                        SortExpression="ItemUnit">
                        <ItemStyle Width="4em" />
                        <FooterStyle Width="4em" />
                    </eclipse:MultiBoundField>
                    <eclipse:MultiBoundField DataFields="OrderedQty" HeaderText="Quantity|Ordered" ItemStyle-Width="1in"
                        AccessibleHeaderText="OrderedQty" DataFormatString="{0:N0}" SortExpression="OrderedQty">
                        <ItemStyle Width="4em" HorizontalAlign="Right" />
                        <FooterStyle Width="4em" />
                    </eclipse:MultiBoundField>
                    <eclipse:MultiBoundField DataFields="DeliveredQty" HeaderText="Quantity|Delivered"
                        DataFormatString="{0:N0}" AccessibleHeaderText="DeliveredQty" SortExpression="DeliveredQty">
                        <ItemStyle Width="4em" HorizontalAlign="Right" />
                        <FooterStyle Width="4em" HorizontalAlign="Right" />
                    </eclipse:MultiBoundField>
                    <eclipse:MultiBoundField DataFields="AcceptedQty" HeaderText="Quantity|Accepted"
                        AccessibleHeaderText="AcceptedQty" DataFormatString="{0:N0}" SortExpression="AcceptedQty">
                        <ItemStyle Width="4em" HorizontalAlign="Right" />
                        <FooterStyle Width="4em" />
                    </eclipse:MultiBoundField>
                    <%--<eclipse:MultiBoundField DataFields="RejectedQty" HeaderText="Quantity|Rejected" ItemStyle-Width="1in"
                        AccessibleHeaderText="RejectedQty" DataFormatString="{0:N0}" SortExpression="RejectedQty" DataSummaryCalculation="ValueSummation" >
                        <ItemStyle Width="4em" />
                        <FooterStyle Width="4em" />
                    </eclipse:MultiBoundField>--%>
                    <eclipse:HyperLinkFieldEx DataTextField="IssuedQty" HeaderText="Quantity|Issued"
                        Visible="false" DataNavigateUrlFields="ItemId,GrnId" DataNavigateUrlFormatString="~/Store/Reports/DivisionSRS.aspx?ItemId={0}&GrnId={1}">
                        <ItemStyle HorizontalAlign="Right" />
                    </eclipse:HyperLinkFieldEx>
                    <eclipse:MultiBoundField DataFields="Price" HeaderText="Price" HeaderStyle-Wrap="false"
                        SortExpression="Price" DataFormatString="{0:N2}" AccessibleHeaderText="Price">
                        <ItemStyle Width="5em" HorizontalAlign="Right" />
                    </eclipse:MultiBoundField>
                    <eclipse:MultiBoundField DataFields="LandedPrice" HeaderText="Per Unit | Landing Charges"
                        ItemStyle-Width="1in" HeaderStyle-Wrap="false" SortExpression="LandedPrice" DataFormatString="{0:N2}"
                        Visible="false" AccessibleHeaderText="LandedPrice">
                        <ItemStyle HorizontalAlign="Right" />
                    </eclipse:MultiBoundField>
                    <eclipse:MultiBoundField DataFields="DeliveredTotal" HeaderText="Total Amount|Delivered"
                        ItemStyle-HorizontalAlign="Right" Visible="false" SortExpression="Total" HeaderToolTip="Total at the time of delivery of goods i.e.(Price + Landing Charges) * Delivered Quantity"
                        DataSummaryCalculation="ValueSummation" DataFormatString="{0:C2}">
                        <FooterStyle HorizontalAlign="Right" />
                    </eclipse:MultiBoundField>
                    <eclipse:MultiBoundField DataFields="AcceptedTotal" HeaderText="Total Amount" AccessibleHeaderText="Total"
                        ItemStyle-HorizontalAlign="Right" SortExpression="Total" HeaderToolTip="Total at the time of accepting goods i.e.(Price + Landing Charges) * Accepted Quantity"
                        DataSummaryCalculation="ValueSummation" DataFormatString="{0:C2}">
                        <ItemStyle HorizontalAlign="Right" />
                        <FooterStyle HorizontalAlign="Right" />
                    </eclipse:MultiBoundField>
                </Columns>
                <EmptyDataTemplate>
                    <b>Data not found.</b>
                </EmptyDataTemplate>
            </jquery:GridViewEx>
        </ItemTemplate>
        <EmptyDataTemplate>
            No data found.
        </EmptyDataTemplate>
    </asp:FormView>
    <br />
    <br />
    <br />
    <br />
    <asp:Panel ID="lblPanel" Visible="false" runat="server" Width="100%" Style="text-align: center;
        font-size: large">
        <div runat="server" style="width: 33%; float: left">
            <asp:Label ID="lblStoreIncharge" runat="server" Text="<b>Store Incharge</b>" />
        </div>
        <div runat="server" style="width: 33%; float: left">
            <asp:Label ID="lblPreparedBy" runat="server" Text="<b>Prepared By</b>" />
        </div>
        <div runat="server" style="width: 33%; float: right">
            <asp:Label ID="lblApprovedBy" runat="server" Text="<b>Approved By</b>" />
        </div>
    </asp:Panel>
</asp:Content>
