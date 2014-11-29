<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" CodeBehind="StockBalance.aspx.cs"
    Inherits="Finance.Store.Reports.StockBalance" Title="Stock Balance" EnableViewState="false" %>

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
                <asp:HyperLink runat="server" NavigateUrl="~/Store/GrnList.aspx" Text="View GRN List" />
            </li>
            <li>
                <asp:HyperLink runat="server" NavigateUrl="~/Store/CreateGRN.aspx" Text="Create New GRN" />
            </li>
            <li>
                <asp:HyperLink runat="server" NavigateUrl="~/Store/Reports/GRNReport.aspx" Text="View GRN Report" />
            </li>
        </ul>
    </fieldset>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cph" runat="server">
    <eclipse:TwoColumnPanel runat="server">
        <eclipse:LeftLabel runat="server" Text="Date" />
        <i:TextBoxEx ID="tbDate" runat="server" FriendlyName="Date" Text="0">
            <Validators>
                <i:Required />
                <i:Date Max="0" />
            </Validators>
        </i:TextBoxEx>
        <eclipse:LeftLabel runat="server" Text="Item Category" />
        <phpa:PhpaLinqDataSource runat="server" ID="dsCategory" ContextTypeName="Eclipse.PhpaLibrary.Database.Store.StoreDataContext"
            OrderBy="Description" TableName="ItemCategories" Visible="True" Select="new (ItemCategoryId, Description)"
            RenderLogVisible="false">
        </phpa:PhpaLinqDataSource>
        <i:DropDownListEx ID="ddlCategory" runat="server" DataSourceID="dsCategory" DataTextField="Description"
            DataValueField="ItemCategoryId">
            <Items>
                <eclipse:DropDownItem Text="(All Categories)" Persistent="Always" />
            </Items>
        </i:DropDownListEx>
        <br />
        <i:ValidationSummary ID="valStock" runat="server" />
        <br />
        <i:ButtonEx ID="btnShow" runat="server" Text="Go" CausesValidation="true" Action="Submit"
            Icon="Refresh"  IsDefault="true"/>
        <i:ButtonEx ID="btnClear" runat="server" Text="Clear Filter" CausesValidation="false"
            Action="Reset" Icon="Cancel" />
    </eclipse:TwoColumnPanel>
    <phpa:PhpaLinqDataSource ID="dsStockBalance" runat="server" ContextTypeName="Eclipse.PhpaLibrary.Database.Store.StoreDataContext"
        OnSelecting="dsStockBalance_Selecting" RenderLogVisible="false" OnLoad="dsStockBalance_Load">
    </phpa:PhpaLinqDataSource>
    <jquery:GridViewEx ID="gvStockBalance" runat="server" AutoGenerateColumns="False"
        ShowExpandCollapseButtons="false" DataSourceID="dsStockBalance" AllowSorting="true"
        OnRowDataBound="gvStockBalance_RowDataBound" ShowFooter="true" AllowPaging="true"
        PageSize="500" DefaultSortExpression="StockItem.ItemCategory.ItemCategoryCode,StockItem.ItemCategory.Description;$;">
        <Columns>
            <eclipse:SequenceField />
            <eclipse:MultiBoundField DataFields="StockItem.ItemCode" HeaderText="Code" SortExpression="StockItem.ItemCode" />
            <eclipse:MultiBoundField DataFields="StockItem.Description,StockItem.Brand,StockItem.Identifier,StockItem.Color,StockItem.Size"
                HeaderText="Item" SortExpression="StockItem.Description,StockItem.Brand,StockItem.Identifier,StockItem.Color,StockItem.Size"
                DataFormatString="{0} {1} {2} {3} {4}" />
            <%--            <eclipse:MultiBoundField DataFields="StockItem.Brand" HideEmptyColumn="true" HeaderText="Brand"
                SortExpression="StockItem.Brand" />
            <eclipse:MultiBoundField DataFields="StockItem.Identifier" HeaderText="Identifier"
                SortExpression="StockItem.Identifier" HideEmptyColumn="true">
            </eclipse:MultiBoundField>
            <eclipse:MultiBoundField DataFields="StockItem.Size" HideEmptyColumn="true" HeaderText="Size"
                SortExpression="StockItem.Size" />--%>
            <eclipse:MultiBoundField DataFields="StockItem.ItemCategory.ItemCategoryCode, StockItem.ItemCategory.Description"
                HeaderText="Category" SortExpression="StockItem.ItemCategory.ItemCategoryCode,StockItem.ItemCategory.Description"
                DataFormatString="{0}:{1}" FooterText="Total" />
            <%--            <eclipse:MultiBoundField DataFields="StockItem.UOM.UOMCode" HeaderText="Unit" SortExpression="StockItem.UOM.UOMCode" />--%>
            <eclipse:MultiBoundField DataFields="ReceivedQuantity,StockItem.UOM.UOMCode" HeaderText="Received"
                SortExpression="ReceivedQuantity" DataFormatString="{0:N0} {1}">
                <ItemStyle HorizontalAlign="Right" />
            </eclipse:MultiBoundField>
            <eclipse:MultiBoundField DataFields="AcceptedQuantity" HeaderText="Accepted|Quantity"
                SortExpression="AcceptedQuantity" DataFormatString="{0:#,###}">
                <ItemStyle HorizontalAlign="Right" />
            </eclipse:MultiBoundField>
            <eclipse:MultiBoundField DataFields="AcceptedValue" HeaderText="Accepted|Value" SortExpression="AcceptedValue"
                DataFormatString="{0:#,###}" DataSummaryCalculation="ValueSummation">
                <ItemStyle HorizontalAlign="Right" />
                <FooterStyle HorizontalAlign="Right" />
            </eclipse:MultiBoundField>
            <eclipse:MultiBoundField DataFields="IssuedQuantity" HeaderText="Issued|Quantity"
                SortExpression="IssuedQuantity" DataFormatString="{0:#,###}">
                <ItemStyle HorizontalAlign="Right" />
                <FooterStyle HorizontalAlign="Right" />
            </eclipse:MultiBoundField>
            <eclipse:MultiBoundField DataFields="IssuedValue" HeaderText="Issued|Value" SortExpression="IssuedValue"
                DataFormatString="{0:#,###}" DataSummaryCalculation="ValueSummation">
                <ItemStyle HorizontalAlign="Right" />
                <FooterStyle HorizontalAlign="Right" />
            </eclipse:MultiBoundField>
            <eclipse:MultiBoundField DataFields="IntransitQuantity" HeaderText="Balance|Transit"
                SortExpression="IntransitQuantity" DataFormatString="{0:#,###}" HeaderToolTip="Received - Accepted">
                <ItemStyle HorizontalAlign="Right" />
            </eclipse:MultiBoundField>
            <eclipse:HyperLinkFieldEx DataTextField="StoreQuantity" HeaderText="Balance|Store"
                SortExpression="StoreQuantity" DataTextFormatString="{0:#,###}" DataNavigateUrlFields="ItemId,FromDate,ToDate"
                DataNavigateUrlFormatString="~/Store/Reports/ItemLedger.aspx?ItemId={0}&FromDate={1:d}&ToDate={2:d}"
                HeaderToolTip="Accepted - Issued">
                <ItemStyle HorizontalAlign="Right" />
            </eclipse:HyperLinkFieldEx>
            <eclipse:MultiBoundField DataFields="TransactionValue" SortExpression="TransactionValue"
                HeaderText="Total Value" HeaderToolTip="(Average Price + Landing Charges) * (Store + Transit)"
                DataSummaryCalculation="ValueSummation" DataFormatString="{0:C0}">
                <ItemStyle HorizontalAlign="Right" />
                <FooterStyle HorizontalAlign="Right" />
            </eclipse:MultiBoundField>
            <%--            <eclipse:MultiBoundField DataFields="StockItem.ReorderingLevel" HeaderText="Reordering Level"
                SortExpression="StockItem.ReorderingLevel" HeaderToolTip="Shows Reordering Level which is set for the item at which item need to order again.">
                <ItemStyle HorizontalAlign="Right" />
            </eclipse:MultiBoundField>--%>
        </Columns>
        <EmptyDataTemplate>
            <b>No Items found.</b>
        </EmptyDataTemplate>
    </jquery:GridViewEx>
</asp:Content>
