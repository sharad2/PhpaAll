<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" CodeBehind="Store.aspx.cs"
    Inherits="PhpaAll.Store.Store" Title="Stores Home -- PHPA" EnableViewState="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphSideNavigation" runat="server">
    <br />
    <asp:HyperLink ID="HyperLink2" runat="server" Text="Help" NavigateUrl="~/Doc/Store.doc.aspx" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cph" runat="server">
    <phpa:PhpaLinqDataSource ID="dsGrnBrief" runat="server" ContextTypeName="Eclipse.PhpaLibrary.Database.Store.StoreDataContext"
        TableName="GRNs" RenderLogVisible="False" OnSelecting="dsGrnBrief_Selecting">
    </phpa:PhpaLinqDataSource>
    <div style="float: left; width: 50%; margin-right: 4mm">
        <jquery:GridViewEx runat="server" DataSourceID="dsGrnBrief" AutoGenerateColumns="false"
            Caption="<a href='GrnList.aspx'>GRN List</a>">
            <Columns>
                <eclipse:HyperLinkFieldEx DataTextField="GRNCode" HeaderText="Number" DataNavigateUrlFormatString="~/Store/CreateGRN.aspx?GRNId={0}"
                    DataNavigateUrlFields="GRNId">
                    <ItemStyle HorizontalAlign="Right" />
                </eclipse:HyperLinkFieldEx>
                <eclipse:MultiBoundField DataFields="LastUpdateDate,UpdatedBy" HeaderText="Updated"
                    DataFormatString="{0:d} by {1}" />
                <eclipse:MultiBoundField DataFields="GRNReceiveDate" HeaderText="Received" DataFormatString="{0:d}" />
                <eclipse:MultiBoundField DataFields="CountItems" HeaderText="# Items" DataFormatString="{0:N0}">
                    <ItemStyle HorizontalAlign="Right" />
                </eclipse:MultiBoundField>
            </Columns>
        </jquery:GridViewEx>
    </div>
    <phpa:PhpaLinqDataSource ID="dsGRNReceipt" runat="server" RenderLogVisible="False"
        ContextTypeName="Eclipse.PhpaLibrary.Database.Store.StoreDataContext" TableName="GRNItems"
        EnableUpdate="True" OnSelecting="dsGRNReceipt_Selecting">
    </phpa:PhpaLinqDataSource>
    <div style="float: left; width: 40%">
        <jquery:GridViewEx runat="server" DataSourceID="dsGRNReceipt" AutoGenerateColumns="false"
            Caption="<a href='Reports/MaterialReceipt.aspx'>Material Receipt</a>">
            <Columns>
                <eclipse:MultiBoundField DataFields="ItemCode,ItemDescription,Brand,Size, Identifier"
                    HeaderText="Item" DataFormatString="{0}: {1} {2} {3} {4}" />
                <eclipse:HyperLinkFieldEx DataTextField="GRNCode" HeaderText="GRN" DataNavigateUrlFormatString="~/Store/CreateGRN.aspx?GRNId={0}"
                    DataNavigateUrlFields="GRNId">
                    <ItemStyle HorizontalAlign="Right" />
                </eclipse:HyperLinkFieldEx>
                <eclipse:MultiBoundField DataFields="AcceptedQty" HeaderText="Accepted" DataFormatString="{0:N0}">
                    <ItemStyle HorizontalAlign="Right" />
                </eclipse:MultiBoundField>
                <eclipse:MultiBoundField DataFields="UpdatedDate,UpdatedBy" HeaderText="Updated"
                    DataFormatString="{0:d} by {1}">
                </eclipse:MultiBoundField>
            </Columns>
        </jquery:GridViewEx>
    </div>
</asp:Content>
