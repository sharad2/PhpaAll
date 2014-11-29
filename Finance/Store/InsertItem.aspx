<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" CodeBehind="InsertItem.aspx.cs"
    Inherits="Finance.Store.InsertItem" Title="Manage Item" %>

<%@ Register Src="../Controls/PrinterFriendlyButton.ascx" TagName="PrinterFriendlyButton"
    TagPrefix="uc2" %>
<asp:Content ID="Content3" ContentPlaceHolderID="cphSideNavigation" runat="server">
    <asp:HyperLink ID="HyperLink2" runat="server" Text="Help" NavigateUrl="~/Doc/InsertItem.doc.aspx" />
    <uc2:PrinterFriendlyButton ID="PrinterFriendlyButton1" runat="server" />
    <fieldset>
        <legend>Quick Links</legend>
        <ul>
            <li>
                <asp:HyperLink runat="server" NavigateUrl="~/Store/GrnList.aspx" Text="View GRN List" />
            </li>
            <li>
                <asp:HyperLink runat="server" NavigateUrl="~/Store/Reports/StockBalance.aspx" Text="See stock balances" />
            </li>
            <li>
                <asp:HyperLink runat="server" NavigateUrl="~/Store/CreateGRN.aspx" Text="Create New GRN" />
            </li>
        </ul>
    </fieldset>
    <br />
    <asp:HyperLink runat="server" NavigateUrl="~/Store/CreateGRN.aspx" Text="Create GRN" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cph" runat="server">
    <jquery:Dialog ID="panelHybrid" runat="server" Title="Item Editor" Position="RightTop"
        Width="400px" EnableViewState="true" EnablePostBack="true">
        <ContentTemplate>
            <phpa:PhpaLinqDataSource ID="dsEditItem" runat="server" ContextTypeName="Eclipse.PhpaLibrary.Database.Store.StoreDataContext"
                EnableDelete="True" EnableInsert="True" EnableUpdate="True" TableName="Items"
                AutoGenerateWhereClause="True" RenderLogVisible="false" OnInserted="dsEditItem_Inserted"
                OnContextCreated="dsItem_ContextCreated" OnSelecting="dsEditItem_Selecting">
                <WhereParameters>
                    <asp:ControlParameter ControlID="hfSelectedItem" Name="ItemId" Type="Int32" />
                </WhereParameters>
                <UpdateParameters>
                    <asp:Parameter Name="ItemId" Type="Int32" />
                    <asp:Parameter Name="ItemCode" Type="String" />
                    <asp:Parameter Name="Description" Type="String" />
                    <asp:Parameter Name="Brand" Type="String" />
                    <asp:Parameter Name="Color" Type="String" />
                    <asp:Parameter Name="Dimension" Type="String" />
                    <asp:Parameter Name="Size" Type="String" />
                    <asp:Parameter Name="Identifier" Type="String" />
                    <asp:Parameter Name="ItemCategoryId" Type="Int32" />
                    <asp:Parameter Name="UOMId" Type="Int32" />
                    <asp:Parameter Name="HeadOfAccountId" Type="Int32" />
                    <asp:Parameter Name="ReorderingLevel" Type="Int32" />
                </UpdateParameters>
                <InsertParameters>
                    <asp:Parameter Name="ItemId" Type="Int32" />
                    <asp:Parameter Name="ItemCode" Type="String" />
                    <asp:Parameter Name="Description" Type="String" />
                    <asp:Parameter Name="Brand" Type="String" />
                    <asp:Parameter Name="Color" Type="String" />
                    <asp:Parameter Name="Dimension" Type="String" />
                    <asp:Parameter Name="Size" Type="String" />
                    <asp:Parameter Name="Identifier" Type="String" />
                    <asp:Parameter Name="ItemCategoryId" Type="Int32" />
                    <asp:Parameter Name="UOMId" Type="Int32" />
                    <asp:Parameter Name="HeadOfAccountId" Type="Int32" />
                    <asp:Parameter Name="ReorderingLevel" Type="Int32" />
                </InsertParameters>
            </phpa:PhpaLinqDataSource>
            <asp:FormView ID="fvEditItem" runat="server" DataKeyNames="ItemId" DataSourceID="dsEditItem"
                OnItemDeleted="fvEditItem_ItemDeleted" OnItemInserted="fvEditItem_ItemInserted"
                OnItemUpdated="fvEditItem_ItemUpdated" OnItemCreated="fvEditItem_ItemCreated">
                <HeaderTemplate>
                    Item <em>
                        <%#Eval("ItemCode")?? "New" %></em>:<%#Eval("Description") %>
                </HeaderTemplate>
                <EmptyDataTemplate>
                    <asp:Label ID="lblEmptyMsg" runat="server" Text="Select the Item to view details." />
                    <br />
                    <asp:LoginView ID="restricUser1" runat="server">
                        <RoleGroups>
                            <asp:RoleGroup Roles="StoresManager">
                                <ContentTemplate>
                                    <asp:Label ID="lblEmptyMsg" runat="server" Text=" You can also edit Items. Click the link below to" />
                                    <br />
                                    <i:LinkButtonEx ID="btnNewItem" runat="server" CausesValidation="False" OnClick="btnNew_Click"
                                        Text="Insert New Item" ToolTip="Click to insert new Item" />
                                </ContentTemplate>
                            </asp:RoleGroup>
                        </RoleGroups>
                        <LoggedInTemplate>
                            <b>Only Store Manager can insert or edit Items.</b>
                        </LoggedInTemplate>
                    </asp:LoginView>
                    <br />
                    <phpa:FormViewStatusMessage ID="fvDeleteStatusMessage" runat="server" />
                </EmptyDataTemplate>
                <ItemTemplate>
                    <jquery:Tabs ID="TabContainer1" runat="server">
                        <jquery:JPanel runat="server" ID="Panel1" HeaderText="Details">
                            <eclipse:TwoColumnPanel ID="TwoColumnPanel" runat="server">
                                <eclipse:LeftLabel runat="server" Text="Item Code" />
                                <asp:Label runat="server" Text='<%#Eval("ItemCode") %>' />
                                <eclipse:LeftLabel runat="server" Text="Description" />
                                <asp:Label runat="server" Text='<%#Eval("Description") %>' />
                                <eclipse:LeftLabel runat="server" Text="Brand" />
                                <asp:Label runat="server" Text='<%#Eval("Brand") %>' />
                                <eclipse:LeftLabel runat="server" Text="Color" />
                                <asp:Label runat="server" Text='<%#Eval("Color") %>' />
                                <eclipse:LeftLabel runat="server" Text="Dimension" />
                                <asp:Label runat="server" Text='<%#Eval("Dimension") %>' />
                                <eclipse:LeftLabel runat="server" Text="Identifier" />
                                <asp:Label runat="server" Text='<%#Eval("Identifier") %>' />
                                <eclipse:LeftLabel runat="server" Text="Size" />
                                <asp:Label runat="server" Text='<%#Eval("Size") %>' />
                                <eclipse:LeftLabel runat="server" Text="Category" />
                                <asp:Label runat="server" Text='<%# Eval("ItemCategory.ItemCategoryCode")%>' />:
                                <asp:Label runat="server" Text='<%# Eval("ItemCategory.Description")%>' />
                                <eclipse:LeftLabel runat="server" Text="Unit Of Measurement" />
                                <asp:Label runat="server" Text='<%#Eval("UOM.UOMCode") %>' />
                                <eclipse:LeftLabel runat="server" Text="Head Of Account" />
                                <asp:Label runat="server" Text='<%#Eval("HeadOfAccount.DisplayName") %>' />:
                                <asp:Label ID="Label1" runat="server" Text='<%#Eval("HeadOfAccount.Description") %>' />
                                <eclipse:LeftLabel runat="server" Text="Reordering Level" />
                                <asp:Label runat="server" Text='<%#Eval("ReorderingLevel") %>' />
                                <eclipse:LeftLabel ID="LeftLabel4" runat="server" Text="Remarks" />
                                <asp:Label ID="Label2" runat="server" Text='<%#Eval("Remark") %>' />
                            </eclipse:TwoColumnPanel>
                        </jquery:JPanel>
                        <phpa:AuditTabPanel ID="panelAudit" runat="server" />
                    </jquery:Tabs>
                    <asp:LoginView ID="userRestriction" runat="server">
                        <RoleGroups>
                            <asp:RoleGroup Roles="StoresManager">
                                <ContentTemplate>
                                    <i:LinkButtonEx runat="server" ID="btnEdit" CausesValidation="False" OnClick="btnEdit_Click"
                                        Text="Edit" />
                                    <i:LinkButtonEx ID="btnDelete" OnClick="btnDelete_Click" runat="server" CausesValidation="False"
                                        Text="Delete" OnClientClick="
function(e) {
    return confirm('Deletion will succeed only if this item is not associated with any GRN. Are you sure you want to delete the Item?');
}" />
                                    <i:LinkButtonEx ID="btnNew" runat="server" OnClick="btnNew_Click" Text="New" />
                                </ContentTemplate>
                            </asp:RoleGroup>
                        </RoleGroups>
                        <LoggedInTemplate>
                            <b>Only Store Manager can insert or edit Items.</b>
                        </LoggedInTemplate>
                    </asp:LoginView>
                </ItemTemplate>
                <EditItemTemplate>
                    <eclipse:TwoColumnPanel ID="TwoColumnPanel1" runat="server">
                        <eclipse:LeftLabel runat="server" Text="Item Code" />
                        <i:TextBoxEx ID="tbItemCode" runat="server" CaseConversion="UpperCase" MaxLength="40"
                            Size="30" Text='<%# Bind("ItemCode") %>'>
                            <Validators>
                                <i:Required />
                            </Validators>
                        </i:TextBoxEx>
                        <eclipse:LeftLabel runat="server" Text="Description" />
                        <i:TextBoxEx ID="tbDescription" runat="server" Text='<%# Bind("Description") %>'
                            MaxLength="100" Size="25">
                        </i:TextBoxEx>
                        <eclipse:LeftLabel runat="server" Text="Brand" />
                        <i:TextBoxEx ID="tbBrand" runat="server" Text='<%# Bind("Brand") %>' MaxLength="40"
                            Size="20" CaseConversion="UpperCase">
                        </i:TextBoxEx>
                        <eclipse:LeftLabel runat="server" Text="Color" />
                        <i:TextBoxEx ID="tbColor" runat="server" Text='<%# Bind("Color") %>' MaxLength="20"
                            CaseConversion="UpperCase">
                        </i:TextBoxEx>
                        <eclipse:LeftLabel runat="server" Text="Dimension" />
                        <i:TextBoxEx ID="tbDimension" runat="server" Text='<%# Bind("Dimension") %>' MaxLength="40"
                            Size="20" CaseConversion="UpperCase">
                        </i:TextBoxEx>
                        <eclipse:LeftLabel runat="server" Text="size" />
                        <i:TextBoxEx ID="tbSize" runat="server" Text='<%# Bind("Size") %>' MaxLength="40"
                            Size="20" CaseConversion="UpperCase">
                        </i:TextBoxEx>
                        <eclipse:LeftLabel runat="server" Text="Identifier" />
                        <i:TextBoxEx ID="tbIdentifier" runat="server" Text='<%# Bind("Identifier") %>' MaxLength="50"
                            Size="20" CaseConversion="UpperCase">
                        </i:TextBoxEx>
                        <br />
                        Give any unique thing which help to identify the Item. eg. Part No, BTC Code.
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
                        <eclipse:LeftLabel runat="server" Text="Unit Of Measurement" />
                        <phpa:PhpaLinqDataSource runat="server" ID="dsUOM" ContextTypeName="Eclipse.PhpaLibrary.Database.Store.StoreDataContext"
                            OrderBy="Description" TableName="UOMs" Visible="True" Select="new (UOMId, Description, UOMCode)"
                            RenderLogVisible="False" EntityTypeName="">
                        </phpa:PhpaLinqDataSource>
                        <i:DropDownListEx ID="tbUOM" runat="server" DataSourceID="dsUOM" DataTextField="UOMCode"
                            DataValueField="UOMId" Value='<%# Bind("UOMId") %>' FriendlyName="Unit of Measurement">
                            <Validators>
                                <i:Required />
                            </Validators>
                            <Items>
                                <eclipse:DropDownItem Text="" Persistent="Always" />
                            </Items>
                        </i:DropDownListEx>
                        <eclipse:LeftLabel runat="server" Text="Head Of Account" />
                        <i:AutoComplete ID="tbHeadOfAccountDlg" runat="server" Value='<%# Bind("HeadOfAccountId") %>'
                            Text='<%# Eval("HeadOfAccount.DisplayName") %>' FriendlyName="Head of Account"
                            Width="200px" ValidateWebMethodName="ValidateHeadOfAccount" WebMethod="GetHeadOfAccount"
                            WebServicePath="~/Services/HeadOfAccounts.asmx">
                            <Validators>
                                <i:Required />
                                <i:Custom OnServerValidate="tb_ServerValidate" />
                            </Validators>
                        </i:AutoComplete>
                        <eclipse:LeftLabel runat="server" Text="Reordering Level" />
                        <i:TextBoxEx ID="tbReorderingLevel" runat="server" Text='<%# Bind("ReorderingLevel") %>'>
                            <Validators>
                                <i:Value ValueType="Integer" Min="0" />
                            </Validators>
                        </i:TextBoxEx>
                        <br />
                        Set any numeric value at which you want to order your Item again.
                        <eclipse:LeftLabel ID="LeftLabel3" runat="server" Text="Remarks" />
                        <i:TextArea ID="tbRemarks" runat="server" Rows="4" Cols="40" Text='<%# Bind("Remark") %>'>
                        </i:TextArea>
                    </eclipse:TwoColumnPanel>
                    <i:ButtonEx runat="server" ID="btnSave" Text="Save" CausesValidation="true" Action="Submit"
                        Icon="Disk" OnClick="btnSave_Click" />
                    <i:ValidationSummary ID="ValidationSummary2" runat="server" />
                    <i:LinkButtonEx ID="LinkButtonEx1" runat="server" Text="Cancel" CausesValidation="false"
                        OnClick="btnCancel_Click" />
                </EditItemTemplate>
            </asp:FormView>
        </ContentTemplate>
    </jquery:Dialog>
    <jquery:Tabs ID="tab1" runat="server" Selected="-1" Collapsible="true">
        <jquery:JPanel ID="pSearch" runat="server" HeaderText="Search">
            Specify anything you remember in the specified textbox.<br />
            <eclipse:TwoColumnPanel runat="server">
                <eclipse:LeftLabel runat="server" Text="Specify Keyword" />
                <i:TextBoxEx runat="server" ID="tbKeyword" />
                <br />
                e.g Item code, Description, Brand, Color, Size, Dimension or Identifier
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
            </eclipse:TwoColumnPanel>
            <br />
            <asp:ValidationSummary ID="ValidationSummary2" runat="server" />
            <i:ButtonEx ID="btnSearch" runat="server" Text="Go" OnClick="btnSearch_Click" Action="Submit"
                Icon="Search" />
            <i:ButtonEx ID="btnClearSearch" runat="server" Text="Clear" Action="Reset" />
        </jquery:JPanel>
        <jquery:JPanel ID="JPanel1" runat="server" HeaderText="Advanced">
            For more specific search, specify search criteria in the text boxes provided below:<br />
            <eclipse:TwoColumnPanel ID="TwoColumnPanel" runat="server">
                <eclipse:LeftLabel ID="LeftLabel1" runat="server" Text="Item Code" />
                <i:TextBoxEx ID="tbItem" runat="server" MaxLength="50" Size="15">
                </i:TextBoxEx>
                <br />
                <eclipse:LeftLabel runat="server" Text="Description" />
                <i:TextBoxEx runat="server" ID="tbDescription" />
                <eclipse:LeftLabel ID="LeftLabel3" runat="server" Text="Head of Account" />
                <i:AutoComplete ID="tbHeadOfAccount" runat="server" Value='<%# Bind("HeadOfAccountId") %>'
                    Text='<%# Eval("HeadOfAccount.DisplayName") %>' FriendlyName="Head of Account"
                    Width="200px" ValidateWebMethodName="ValidateHeadOfAccount" WebMethod="GetHeadOfAccount"
                    WebServicePath="~/Services/HeadOfAccounts.asmx">
                    <Validators>
                        <i:Custom OnServerValidate="tb_ServerValidate" />
                    </Validators>
                </i:AutoComplete>
                View Items within a Head of Account.
                <eclipse:LeftLabel runat="server" Text="Brand" />
                <i:TextBoxEx ID="tbBrand" runat="server" MaxLength="50" Size="15">
                </i:TextBoxEx>
                <eclipse:LeftLabel runat="server" Text="Color" />
                <i:TextBoxEx ID="tbColor" runat="server" MaxLength="50" Size="15">
                </i:TextBoxEx>
                <eclipse:LeftLabel runat="server" Text="Dimension" />
                <i:TextBoxEx ID="tbDimension" runat="server" MaxLength="50" Size="15">
                </i:TextBoxEx>
                <eclipse:LeftLabel runat="server" Text="Identifier" />
                <i:TextBoxEx ID="tbIdentifier" runat="server" MaxLength="50" Size="15">
                </i:TextBoxEx>
                <eclipse:LeftLabel runat="server" Text="Size" />
                <i:TextBoxEx ID="tbSize" runat="server" MaxLength="50" Size="15">
                </i:TextBoxEx>
            </eclipse:TwoColumnPanel>
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" />
            <i:ButtonEx ID="ButtonEx1" runat="server" Text="Go" OnClick="btnSearch_Click" Action="Submit"
                Icon="Search" />
            <i:ButtonEx ID="ButtonEx2" runat="server" Text="Clear" Action="Reset" />
        </jquery:JPanel>
    </jquery:Tabs>
    <phpa:PhpaLinqDataSource ID="dsItem" runat="server" ContextTypeName="Eclipse.PhpaLibrary.Database.Store.StoreDataContext"
        TableName="Items" RenderLogVisible="False" OrderBy="Description,ItemCode" OnSelecting="dsItem_Selecting"
        OnContextCreated="dsItem_ContextCreated">
        <WhereParameters>
            <asp:ControlParameter ControlID="tbItem" Name="ItemCode" PropertyName="Text" Type="String" />
            <asp:ControlParameter ControlID="tbDescription" Name="Description" PropertyName="Text"
                Type="String" />
            <asp:ControlParameter ControlID="ddlCategory" Name="Category" Type="Int32" />
            <asp:ControlParameter ControlID="tbBrand" Name="Brand" PropertyName="Text" Type="String" />
            <asp:ControlParameter ControlID="tbColor" Name="Color" PropertyName="Text" Type="String" />
            <asp:ControlParameter ControlID="tbDimension" Name="Dimension" PropertyName="Text"
                Type="String" />
            <asp:ControlParameter ControlID="tbIdentifier" Name="Identifier" PropertyName="Text"
                Type="String" />
            <asp:ControlParameter ControlID="tbSize" Name="Size" PropertyName="Text" Type="String" />
        </WhereParameters>
    </phpa:PhpaLinqDataSource>
    <asp:HiddenField ID="hfSelectedItem" runat="server" ClientIDMode="Static"></asp:HiddenField>
    <i:ButtonEx runat="server" ID="btnRowDetails" ClientVisible="Never" ClientIDMode="Static"
        Action="Submit" CausesValidation="false" OnClick="btnRowDetails_Click" />
    <jquery:GridViewEx ID="gvItem" DataSourceID="dsItem" AutoGenerateColumns="false"
        AllowSorting="true" runat="server" DataKeyNames="ItemId" EnableViewState="false"
        PreSorted="false" ShowExpandCollapseButtons="false" Caption="List of items" PageSize="200"
        AllowPaging="true" DefaultSortExpression="ItemCategory.Description,ItemCategory.ItemCategoryId;$;Description">
        <RowMenuItems>
            <jquery:RowMenuItem OnClientClick="function(keys) {
                       $('#hfSelectedItem').val(keys[0]);
                       $('#btnRowDetails').click();
        }" Text="Details" />
            <jquery:RowMenuNavigate NavigateUrl="~/Store/Reports/ItemLedger.aspx?ItemId={0}"
                Text="Item Ledger" />
        </RowMenuItems>
        <Columns>
            <eclipse:MultiBoundField HeaderText="Code" DataFields="ItemCode" SortExpression="ItemCode" />
            <eclipse:MultiBoundField HeaderText="Description" DataFields="Description" SortExpression="Description">
                <ItemStyle Width="2.5in" Wrap="true" />
            </eclipse:MultiBoundField>
            <eclipse:MultiBoundField HeaderText="Brand" DataFields="Brand" SortExpression="Brand"
                HideEmptyColumn="true" />
            <eclipse:MultiBoundField HeaderText="Color" DataFields="Color" SortExpression="Color"
                HideEmptyColumn="true" />
            <eclipse:MultiBoundField HeaderText="Identifier" DataFields="Identifier" SortExpression="Identifier"
                HideEmptyColumn="true" />
            <eclipse:MultiBoundField HeaderText="Size" DataFields="Size" SortExpression="Size"
                HideEmptyColumn="true" />
            <eclipse:MultiBoundField HeaderText="Dimension" DataFields="Dimension" SortExpression="Dimension"
                HideEmptyColumn="true" />
            <eclipse:MultiBoundField HeaderText="Category" DataFields="ItemCategory.Description"
                DataFormatString="{0}" SortExpression="ItemCategory.Description,ItemCategory.ItemCategoryId" />
            <eclipse:MultiBoundField HeaderText="UOM" DataFields="UOM.UOMCode" SortExpression="UOM.UOMCode"
                HeaderToolTip="Unit of Measurement" HeaderStyle-Wrap="true">
                <HeaderStyle Wrap="True"></HeaderStyle>
            </eclipse:MultiBoundField>
            <eclipse:HyperLinkFieldEx DataTextField="CountGrn" HeaderText="# GRN">
                <ItemStyle HorizontalAlign="Right" />
            </eclipse:HyperLinkFieldEx>
            <eclipse:HyperLinkFieldEx DataTextField="CountSrs" HeaderText="# SRS">
                <ItemStyle HorizontalAlign="Right" />
            </eclipse:HyperLinkFieldEx>
        </Columns>
        <PagerStyle VerticalAlign="Middle" />
        <EmptyDataTemplate>
            <b>Item not found.</b>
        </EmptyDataTemplate>
    </jquery:GridViewEx>
</asp:Content>
