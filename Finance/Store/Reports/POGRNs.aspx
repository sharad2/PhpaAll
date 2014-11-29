<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" CodeBehind="POGRNs.aspx.cs"
    Inherits="Finance.Store.Reports.POGRNs" Title="GRN's per Purchase Order" %>

<%@ Register Src="~/Controls/PrinterFriendlyButton.ascx" TagName="PrinterFriendlyButton"
    TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphSideNavigation" runat="server">
    <uc2:PrinterFriendlyButton ID="PrinterFriendlyButton1" runat="server" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cph" runat="server">
    <eclipse:TwoColumnPanel ID="TwoColumnPanel1" runat="server">
        <%-- eclipse:LeftLabel runat="server" Text="From Date / To Date" />
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
        </i:TextBoxEx>--%>
        <eclipse:LeftLabel runat="server" Text="Purchase Order No" />
        <i:AutoComplete ID="tbPoNo" runat="server" FriendlyName="Purchase Order Number" WebMethod="GetPoNo"
            Width="30em" WebServicePath="~/Services/PONumbers.asmx">
            <Validators>
                <i:Required />
            </Validators>
        </i:AutoComplete>
        <br />
        Select Purchase Order no in order to view GRN's corresponding to selected Purchase
        Order No.
        <br />
        <br />
        <i:ButtonEx ID="btnShow" runat="server" Text="Go" OnClick="btnShow_Click" Action="Submit"
            Icon="Refresh" CausesValidation="true" IsDefault="true" />
        <i:ValidationSummary ID="valSummary" runat="server" />
    </eclipse:TwoColumnPanel>
    <br />
    <phpa:PhpaLinqDataSource ID="dsPOGRNs" runat="server" ContextTypeName="Eclipse.PhpaLibrary.Database.Store.StoreDataContext"
        TableName="GRNs" OnSelecting="dsPOGRNs_Selecting" RenderLogVisible="false" />
    <jquery:GridViewEx ID="gvPoGrns" runat="server" AutoGenerateColumns="false" DataKeyNames="GRNId"
        DataSourceID="dsPOGRNs" OnRowDataBound="gvPoGrns_RowDataBound" PageSize="200"
        AllowPaging="true" AllowSorting="true" ShowFooter="true">
        <Columns>
            <eclipse:SequenceField />
            <asp:HyperLinkField HeaderText="No" DataTextField="GRNNo" DataNavigateUrlFields="GRNId"
                DataNavigateUrlFormatString="~/Store/Reports/GRNReport.aspx?GRNId={0}" SortExpression="GRNNo" />
            <eclipse:MultiBoundField DataFields="GRNCreateDate" HeaderText="Create Date" SortExpression="GRNCreateDate"
                DataFormatString="{0:d}">
                <ItemStyle HorizontalAlign="Right" />
            </eclipse:MultiBoundField>
            <eclipse:MultiBoundField DataFields="GRNReceiveDate" HeaderText="Recieve Date" SortExpression="GRNReceiveDate"
                DataFormatString="{0:d}">
                <ItemStyle HorizontalAlign="Right" />
            </eclipse:MultiBoundField>
            <%--<eclipse:MultiBoundField DataFields="PONo" HeaderText="GRN | PO No." SortExpression="PONo" />--%>
            <eclipse:MultiBoundField DataFields="PoDate" HeaderText="PO Date" SortExpression="PoDate"
                DataFormatString="{0:d}">
                <ItemStyle HorizontalAlign="Right" />
            </eclipse:MultiBoundField>
            <eclipse:MultiBoundField DataFields="Supplier" HeaderText="Supplier" SortExpression="Supplier"
                FooterText="Grand Total" />
            <eclipse:MultiBoundField DataFields="OrderedItems" DataFormatString="{0:N0}" HeaderText="Items Ordered"
                SortExpression="OrderedItems" DataSummaryCalculation="ValueSummation"/>
            <eclipse:MultiBoundField DataFields="DeliveredItems" DataFormatString="{0:N0}" HeaderText="Items Delivered"
                SortExpression="DeliveredItems" DataSummaryCalculation="ValueSummation"/>
        </Columns>
        <EmptyDataTemplate>
            <b>Data not found.</b>
        </EmptyDataTemplate>
    </jquery:GridViewEx>
</asp:Content>
