<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" CodeBehind="MaterialReceipt.aspx.cs"
    Inherits="PhpaAll.Store.Reports.MaterialReceipt" Title="Material Receipt" EnableViewState="false" %>

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
                <asp:HyperLink runat="server" NavigateUrl="~/Store/Reports/GRNReport.aspx" Text="Receive & Edit GRN" />
            </li>
            <li>
                <asp:HyperLink runat="server" NavigateUrl="~/Store/CreateGRN.aspx" Text="Create New GRN" />
            </li>
            <li>
                <asp:HyperLink runat="server" NavigateUrl="~/Store/Reports/StockBalance.aspx" Text="See Stock Balance" />
            </li>
        </ul>
    </fieldset>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cph" runat="server">
    <phpa:PhpaLinqDataSource ID="dsGRNReceipt" runat="server" RenderLogVisible="False"
        ContextTypeName="Eclipse.PhpaLibrary.Database.Store.StoreDataContext" TableName="GRNItems"
        EnableUpdate="True" OnSelecting="dsGRNReceipt_Selecting">
        <%--<WhereParameters>
            <asp:ControlParameter ControlID="tbFromDate" Type="DateTime" Name="FromDate"  />
            <asp:ControlParameter ControlID="tbToDate" Type="DateTime" Name="ToDate" />
        </WhereParameters>--%>
    </phpa:PhpaLinqDataSource>
    <eclipse:TwoColumnPanel ID="TwoColumnPanel1" runat="server" IsValidationContainer="true">
        <eclipse:LeftLabel runat="server" Text="From Date / To Date" />
        <i:TextBoxEx ID="tbFromDate" runat="server" FriendlyName="From Date" Text="-7">
            <Validators>
                <i:Date />
                <i:Required />
            </Validators>
        </i:TextBoxEx>
        <i:TextBoxEx ID="tbToDate" runat="server" FriendlyName="To Date" QueryString="ToDate"
            Text="0">
            <Validators>
                <i:Date DateType="ToDate" />
                <i:Required />
            </Validators>
        </i:TextBoxEx>
        <eclipse:LeftLabel runat="server" Text="Item" />
        <i:AutoComplete ID="tbItem" runat="server" FriendlyName="Items" QueryString="ItemId" AutoValidate="true"
            Width="20em" WebMethod="GetItems" WebServicePath="~/Services/Items.asmx" ValidateWebMethodName="ValidateItem">
        </i:AutoComplete>
        <eclipse:LeftLabel runat="server" Text="Item Category" />
        <phpa:PhpaLinqDataSource runat="server" ID="dsCategory" ContextTypeName="Eclipse.PhpaLibrary.Database.Store.StoreDataContext"
            OrderBy="Description" TableName="ItemCategories" Visible="True" Select="new (ItemCategoryId, Description)"
            RenderLogVisible="false">
        </phpa:PhpaLinqDataSource>
        <i:DropDownListEx ID="ddlCategory" runat="server" DataSourceID="dsCategory" DataTextField="Description"
            DataValueField="ItemCategoryId" Value='<%# Bind("ItemCategoryId") %>'>
            <Items>
                <eclipse:DropDownItem Text="(All Categories)" Persistent="Always" />
            </Items>
        </i:DropDownListEx>
        <br />
        <br />
        <i:ButtonEx ID="btnGo" runat="server" Text="Go" Action="Submit" Icon="Refresh" CausesValidation="true" IsDefault="true"/>
        <i:ButtonEx ID="btnClear" runat="server" Text="Clear Filter" Action="Reset" Icon="Cancel" />
        <br />
        <i:ValidationSummary runat="server" />
    </eclipse:TwoColumnPanel>
    <br />
    <jquery:GridViewEx ID="gvReceipt" AllowSorting="true" runat="server" AutoGenerateColumns="false"
        DataSourceID="dsGRNReceipt" ShowFooter="true" OnRowDataBound="gvReceipt_RowDataBound"
        OnDataBinding="gvReceipt_DataBinding" AllowPaging="true" PageSize="500">
        <Columns>
            <eclipse:SequenceField />
            <eclipse:MultiBoundField DataFields="ItemCode, ItemDescription" HeaderText="Item | Code"
                DataFormatString="{0}:{1}" SortExpression="ItemCode" />
            <eclipse:MultiBoundField DataFields="Brand" HeaderText="Item | Brand" SortExpression="Brand"
                HideEmptyColumn="true" />
            <eclipse:MultiBoundField DataFields="Size" HeaderText="Item | Size" SortExpression="Size"
                HideEmptyColumn="true" />
            <eclipse:MultiBoundField DataFields="Identifier" HeaderText="Item | Identifier" SortExpression="Identifier"
                HideEmptyColumn="true" />
            <eclipse:MultiBoundField DataFields="ItemCategoryCode, CatDescription" HeaderText="Item | Category"
                SortExpression="ItemCode" DataFormatString="{0}:{1}" />
            <asp:HyperLinkField HeaderText="GRN | No" DataTextField="GRNNo" DataNavigateUrlFields="GRNId,ItemCode"
                DataNavigateUrlFormatString="~/Store/Reports/GRNReport.aspx?GRNId={0}&ItemCode={1}"
                SortExpression="GRNNo" ItemStyle-Width="5em" />
            <eclipse:MultiBoundField DataFields="RecieveDate" HeaderText="GRN | Receive Date"
                SortExpression="RecieveDate" DataFormatString="{0:d}" ItemStyle-HorizontalAlign="Right" />
            <eclipse:MultiBoundField DataFields="PONo" HeaderText="GRN | PO No." ItemStyle-Width="7em"
                SortExpression="PONo" />
            <eclipse:MultiBoundField DataFields="ReceivedQty" HeaderText="Received Quantity"
                SortExpression="ReceivedQty" AccessibleHeaderText="ReceivedQty" Visible="false" />
            <eclipse:MultiBoundField DataFields="AcceptedQty" HeaderText="Accepted Quantity"
                SortExpression="AcceptedQty" AccessibleHeaderText="AcceptedQty" DataFormatString="{0:N0}"
                DataSummaryCalculation="ValueSummation">
                <ItemStyle HorizontalAlign="Right" />
                <FooterStyle HorizontalAlign="Right" />
            </eclipse:MultiBoundField>
            <eclipse:MultiBoundField DataFields="RejectedQty" HeaderText="Rejected Quantity"
                SortExpression="RejectedQty" AccessibleHeaderText="RejectedQty" DataFormatString="{0:N0}"
                DataSummaryCalculation="ValueSummation">
                <ItemStyle HorizontalAlign="Right" />
                <FooterStyle HorizontalAlign="Right" />
            </eclipse:MultiBoundField>
            <eclipse:MultiBoundField DataFields="TotalPrice" HeaderText="Price" SortExpression="Price"
                HeaderToolTip="Price = Item Price + Landing Charges" DataFormatString="{0:C}">
                <ItemStyle HorizontalAlign="Right" />
            </eclipse:MultiBoundField>
            <eclipse:MultiBoundField DataFields="TotalAccepted" HeaderText="Total Accepted" SortExpression="TotalAccepted"
                DataFormatString="{0:C}" AccessibleHeaderText="TotalAccepted" DataSummaryCalculation="ValueSummation">
                <ItemStyle HorizontalAlign="Right" />
            </eclipse:MultiBoundField>
            <eclipse:MultiBoundField DataFields="TotalRejected" HeaderText="Total Rejected" SortExpression="TotalRejected"
                DataFormatString="{0:C}" AccessibleHeaderText="TotalRejected">
                <ItemStyle HorizontalAlign="Right" />
            </eclipse:MultiBoundField>
        </Columns>
        <EmptyDataTemplate>
            <b>Material not found for the given parameters.</b>
        </EmptyDataTemplate>
    </jquery:GridViewEx>
</asp:Content>
