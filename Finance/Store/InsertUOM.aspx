<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" CodeBehind="InsertUOM.aspx.cs"
    Inherits="Finance.Store.InsertUOM" Title="Unit of Measurement" %>

<%@ Register Src="../Controls/PrinterFriendlyButton.ascx" TagName="PrinterFriendlyButton"
    TagPrefix="uc2" %>
<asp:Content ID="Content3" ContentPlaceHolderID="cphSideNavigation" runat="server">
    <asp:HyperLink ID="HyperLink4" runat="server" Text="Help" NavigateUrl="~/Doc/InsertUOM.doc.aspx" />
    <uc2:PrinterFriendlyButton ID="PrinterFriendlyButton1" runat="server" />
    <fieldset>
        <legend>Quick Links</legend>
        <ul>
            <li>
                <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="~/Store/InsertItem.aspx"
                    Text="Manage Item" />
            </li>
            <li>
                <asp:HyperLink ID="HyperLink2" runat="server" NavigateUrl="~/Store/CreateGRN.aspx"
                    Text="Create New GRN" />
            </li>
            <li>
                <asp:HyperLink ID="HyperLink3" runat="server" NavigateUrl="~/Store/Reports/StockBalance.aspx"
                    Text="See Stock Balance" />
            </li>
        </ul>
    </fieldset>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cph" runat="server">
    <p>
        To search, specify any of the search criteria in the text box provided below:
    </p>
    <eclipse:TwoColumnPanel ID="TwoColumnPanel" runat="server">
        <eclipse:LeftLabel runat="server" Text="Unit of Measurement" />
        <i:TextBoxEx ID="tbUOM" runat="server" MaxLength="50" Size="20">
        </i:TextBoxEx>
        <br />
        Will search within Unit Code and Description.
    </eclipse:TwoColumnPanel>
    <i:ButtonEx ID="btnSearch" runat="server" Text="Go" OnClick="btnSearch_Click" Action="Submit"
        Icon="Search" />
    <i:ButtonEx ID="btnClearSearch" runat="server" Text="Clear" Action="Reset" Icon="Refresh" />
    <br /><br />
    <i:ButtonEx ID="btnNew" runat="server" Text="New UOM" OnClick="btnNew_Click" Action="Submit"
        Icon="PlusThick" />
    <phpa:PhpaLinqDataSource ID="dsUOM" runat="server" ContextTypeName="Eclipse.PhpaLibrary.Database.Store.StoreDataContext"
        TableName="UOMs" RenderLogVisible="false" OnSelecting="dsUOM_Selecting" OrderBy="UOMCode,Description"
        EnableUpdate="true" EnableInsert="true" EnableDelete="true">
        <WhereParameters>
            <asp:ControlParameter ControlID="tbUOM" Name="UOM" PropertyName="Text" Type="String" />
        </WhereParameters>
        <UpdateParameters>
            <asp:Parameter Name="UOMCode" Type="String" />
            <asp:Parameter Name="Description" Type="String" />
        </UpdateParameters>
        <InsertParameters>
            <asp:Parameter Name="UOMId" Type="Int32" />
            <asp:Parameter Name="UOMCode" Type="String" />
            <asp:Parameter Name="Description" Type="String" />
        </InsertParameters>
    </phpa:PhpaLinqDataSource>
    <i:ValidationSummary ID="valSummary" runat="server" />
    <jquery:GridViewExInsert ID="gvUOM" runat="server" DataKeyNames="UOMId,UOMCode" DataSourceID="dsUOM"
        PageSize="50" DefaultSortExpression="UOMCode,Description" AllowSorting="true" OnRowDeleted="gvUOM_RowDeleted"
        Caption="List of Units" AutoGenerateColumns="false" EnableViewState="true" InsertRowsAtBottom="false"
        InsertRowsCount="0">
        <Columns>
            <jquery:CommandFieldEx ShowDeleteButton="true" DeleteConfirmationText="Unit of Measure {0} will be deleted. Are you sure?"
                DataFields="UOMCode">
                <HeaderStyle CssClass="noprint" />
                <ItemStyle CssClass="noprint" />
            </jquery:CommandFieldEx>
            <eclipse:SequenceField>
            </eclipse:SequenceField>
            <eclipse:MultiBoundField DataFields="UOMCode" HeaderText="Code" AccessibleHeaderText="Code"
                ItemStyle-Width="2in" SortExpression="UOMCode">
                <EditItemTemplate>
                    <i:TextBoxEx ID="tbUOMCode" runat="server" CaseConversion="UpperCase" MaxLength="40"
                        Size="20" Text='<%# Bind("UOMCode") %>' FriendlyName="UOM Code">
                        <Validators>
                            <i:Required />
                        </Validators>
                    </i:TextBoxEx>
                    <i:ValidationSummary ID="ValidationSummary1" runat="server" />
                </EditItemTemplate>
            </eclipse:MultiBoundField>
            <eclipse:MultiBoundField DataFields="Description" HeaderText="Description" AccessibleHeaderText="Description"
                ItemStyle-Width="3in" SortExpression="Description">
                <EditItemTemplate>
                    <i:TextBoxEx ID="tbDescription" runat="server" Text='<%# Bind("Description") %>'
                        MaxLength="100" Size="25" FriendlyName="Description">
                    </i:TextBoxEx>
                </EditItemTemplate>
            </eclipse:MultiBoundField>
        </Columns>
        <EmptyDataTemplate>
            Unit of Measurement not found.
        </EmptyDataTemplate>
    </jquery:GridViewExInsert>
</asp:Content>
