<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" CodeBehind="ItemsCategory.aspx.cs"
    Inherits="Finance.Store.ItemsCategory" Title="Item Category" %>

<%@ Register Src="../Controls/PrinterFriendlyButton.ascx" TagName="PrinterFriendlyButton"
    TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphSideNavigation" runat="server">
    <asp:HyperLink ID="HyperLink4" runat="server" Text="Help" NavigateUrl="~/Doc/ItemsCategory.doc.aspx" />
    <uc2:PrinterFriendlyButton ID="PrinterFriendlyButton1" runat="server" />
    <fieldset>
        <legend>Quick Links</legend>
        <ul>
            <li>
                <asp:HyperLink runat="server" NavigateUrl="~/Store/InsertItem.aspx" Text="Manage Item" />
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
    <p>
        To search, specify any of the search criteria in the text box provided below:</p>
    <eclipse:TwoColumnPanel ID="TwoColumnPanel" runat="server">
        <eclipse:LeftLabel ID="LeftLabel1" runat="server" Text="Item category" />
        <i:TextBoxEx ID="tbItemCategory" runat="server" MaxLength="50" Size="20">
        </i:TextBoxEx>
        <br />
        Will search within Item Category Code and Description.
    </eclipse:TwoColumnPanel>
    <i:ButtonEx ID="btnSearch" runat="server" Text="Go" OnClick="btnSearch_Click" Action="Submit"
        Icon="Search" />
    <i:ButtonEx ID="btnClearSearch" runat="server" Text="Clear" Action="Reset" />
    <br />
    <i:ButtonEx ID="btnNew" runat="server" Text="New Category" OnClick="btnNew_Click"
        Action="Submit" Icon="None" />
    <phpa:PhpaLinqDataSource ID="dsItemCategory" runat="server" ContextTypeName="Eclipse.PhpaLibrary.Database.Store.StoreDataContext"
        TableName="ItemCategories" RenderLogVisible="false" OrderBy="ItemCategoryCode,Description"
        OnSelecting="dsItemCategory_Selecting" EnableDelete="true" EnableInsert="true"
        EnableUpdate="true">
        <WhereParameters>
            <asp:ControlParameter ControlID="tbItemCategory" Name="ItemCategory" Type="String"
                PropertyName="Text" />
        </WhereParameters>
        <InsertParameters>
            <asp:Parameter Name="ItemCategoryId" Type="Int32" />
            <asp:Parameter Name="ItemCategoryCode" Type="String" />
            <asp:Parameter Name="Description" Type="String" />
        </InsertParameters>
        <UpdateParameters>
            <asp:Parameter Name="ItemCategoryCode" Type="String" />
            <asp:Parameter Name="Description" Type="String" />
        </UpdateParameters>
    </phpa:PhpaLinqDataSource>
    <i:ValidationSummary ID="valSummary" runat="server" />
    <jquery:GridViewExInsert ID="gvItemCategory" runat="server" DataKeyNames="ItemCategoryId, ItemCategoryCode"
        DataSourceID="dsItemCategory" AutoGenerateColumns="false" Caption="List of Item Categories" OnRowDeleted="gvItemCategory_RowDeleted"
        AllowSorting="true" AllowPaging="true" PageSize="50" EnableViewState="true" InsertRowsAtBottom="false"
        InsertRowsCount="0">
        <Columns>
            <jquery:CommandFieldEx ShowDeleteButton="true" DeleteConfirmationText="Item category {0} will be deleted. Are you sure?"
                DataFields="ItemCategoryCode">
                <HeaderStyle CssClass="noprint" />
                <ItemStyle CssClass="noprint" />
            </jquery:CommandFieldEx>
            <eclipse:SequenceField>
            </eclipse:SequenceField>
            <eclipse:MultiBoundField DataFields="ItemCategoryCode" HeaderText="Code" ItemStyle-Width="3in"
                SortExpression="ItemCategoryCode">
                <EditItemTemplate>
                    <i:TextBoxEx ID="tbItemCategoryCode" runat="server" CaseConversion="UpperCase" MaxLength="40"
                        Size="20" Text='<%# Bind("ItemCategoryCode") %>' FriendlyName="Item Category Code">
                        <Validators>
                            <i:Required />
                        </Validators>
                    </i:TextBoxEx>
                    <i:ValidationSummary ID="ValidationSummary1" runat="server" />
                </EditItemTemplate>
            </eclipse:MultiBoundField>
            <eclipse:MultiBoundField DataFields="Description" HeaderText="Description" ItemStyle-Width="3in"
                SortExpression="Description">
                <EditItemTemplate>
                    <i:TextBoxEx ID="tbDescription" runat="server" Text='<%# Bind("Description") %>'
                        MaxLength="100" Size="25" FriendlyName="Description">
                    </i:TextBoxEx>
                </EditItemTemplate>
            </eclipse:MultiBoundField>
        </Columns>
        <EmptyDataTemplate>
            Item category not found.
        </EmptyDataTemplate>
    </jquery:GridViewExInsert>
</asp:Content>
