<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" CodeBehind="Contractor.aspx.cs"
    Inherits="PhpaAll.Finance.ManageContractor" Title="Manage Contractors" EnableEventValidation="true" %>

<%@ Register Src="~/Controls/PrinterFriendlyButton.ascx" TagName="PrinterFriendlyButton"
    TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function DeleteConfirmation(e) {
            return confirm("Deletion will only succeed if this Contractor not associated anywhere else.Are you sure you want to delete the Contractor?");
        }
    </script>
</asp:Content>
<asp:Content ID="c4" ContentPlaceHolderID="cphSideNavigation" runat="server">
    <uc2:PrinterFriendlyButton ID="PrinterFriendlyButton1" runat="server" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cph" runat="server">
    <p>
        <i:ButtonEx ID="btnNewContractor" runat="server" CausesValidation="False" Action="Submit"
            OnClick="btnNew_Click" Text="Create New Contractor" ToolTip="Click to enter new Contractor's details"
            RolesRequired="FinanceManager,StoresManager" Icon="PlusThick" />
    </p>
    <p>
        Manage contractor will help you to keep track of your contractors by allowing you
        to add new contractor and editing details of existing contractors.
    </p>
    <jquery:Accordion runat="server" Collapsible="true" SelectedIndex="-1">
        <jquery:JPanel runat="server" HeaderText="Search">
            <p>
                To search contractors, specify as much as you know
            </p>
            <eclipse:TwoColumnPanel ID="TwoColumnPanel2" runat="server">
                <eclipse:LeftLabel runat="server" Text="Contractor Code" />
                <i:TextBoxEx ID="txtContractorCode" runat="server">
                </i:TextBoxEx>
                <eclipse:LeftLabel runat="server" Text="Contractor Name" />
                <i:TextBoxEx ID="txtContratorName" runat="server">
                </i:TextBoxEx>
                <eclipse:LeftLabel runat="server" Text="Contact Person" />
                <i:TextBoxEx ID="txtContactPerson" runat="server">
                </i:TextBoxEx>
                <eclipse:LeftLabel runat="server" Text="Address" />
                <i:TextBoxEx ID="txtAddress" runat="server">
                </i:TextBoxEx>
                <eclipse:LeftLabel runat="server" Text="City" />
                <i:TextBoxEx ID="txtCity" runat="server">
                </i:TextBoxEx>
                <eclipse:LeftLabel runat="server" Text="Country" />
                <i:TextBoxEx ID="txtCountry" runat="server">
                </i:TextBoxEx>
            </eclipse:TwoColumnPanel>
            <i:ButtonEx ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click"
                Action="Submit" Icon="Search" />
            <i:ButtonEx ID="btnClearSearch" runat="server" Text="Clear Search" Action="Reset"
                Icon="Refresh" />
        </jquery:JPanel>
    </jquery:Accordion>
    <phpa:PhpaLinqDataSource ID="dsContractors" runat="server" ContextTypeName="Eclipse.PhpaLibrary.Database.FinanceDataContext"
        TableName="Contractors" RenderLogVisible="false" OnSelecting="dsContractors_Selecting"
        OrderBy="ContractorCode">
        <WhereParameters>
            <asp:ControlParameter ControlID="txtContractorCode" Name="ContractorCode" PropertyName="Value" />
            <asp:ControlParameter ControlID="txtContratorName" Name="ContractorName" PropertyName="Value" />
            <asp:ControlParameter ControlID="txtContactPerson" Name="Contact_Person" PropertyName="Value" />
            <asp:ControlParameter ControlID="txtAddress" Name="Address" PropertyName="Value" />
            <asp:ControlParameter ControlID="txtCity" Name="City" PropertyName="Value" />
            <asp:ControlParameter ControlID="txtCountry" Name="Country" PropertyName="Value" />
        </WhereParameters>
    </phpa:PhpaLinqDataSource>
    <jquery:GridViewEx ID="gvContractors" runat="server" AutoGenerateColumns="False"
        DataKeyNames="ContractorId" DataSourceID="dsContractors" AllowSorting="True"
        AllowPaging="true" PageSize="100" Caption="<b>List of Contractors/Parties</b>."
        OnRowDataBound="gvContractors_RowDataBound" OnSelectedIndexChanged="gvContractors_SelectedIndexChanged">
        <EmptyDataTemplate>
            <b>No Contractors exists.</b>
        </EmptyDataTemplate>
        <PagerSettings FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast"
            NextPageText="Next" PreviousPageText="Previous" />
        <Columns>
            <asp:TemplateField ItemStyle-CssClass="noprint" HeaderStyle-CssClass="noprint">
                <ItemTemplate>
                    <asp:LinkButton ID="lnkSelect" runat="server" CommandName="Select" Text="Select">
                    </asp:LinkButton>
                </ItemTemplate>
            </asp:TemplateField>
            <eclipse:SequenceField />
            <eclipse:MultiBoundField DataFields="ContractorCode" HeaderText="Contractor|Code"
                SortExpression="ContractorCode" />
            <eclipse:MultiBoundField DataFields="ContractorName" HeaderText="Contractor|Name"
                SortExpression="ContractorName" />
            <eclipse:MultiBoundField DataFields="Contact_Person" HeaderText="Contact Person"
                SortExpression="Contact_Person" />
            <eclipse:MultiBoundField DataFields="Address" HeaderText="Address" SortExpression="Address" />
            <eclipse:MultiBoundField DataFields="City" HeaderText="City" SortExpression="City" />
            <eclipse:MultiBoundField DataFields="Country" HeaderText="Country" SortExpression="Country" />
        </Columns>
    </jquery:GridViewEx>
    <jquery:Dialog ID="dlgContractorEditor" runat="server" Title="Contractor Editor"
        Position="RightTop" EnableViewState="true" EnablePostBack="true" Width="400"
        Visible="false">
        <ContentTemplate>
            <phpa:PhpaLinqDataSource ID="dsSpecificContratctor" runat="server" ContextTypeName="Eclipse.PhpaLibrary.Database.FinanceDataContext"
                EnableDelete="True" EnableInsert="True" EnableUpdate="True" TableName="Contractors"
                Where="ContractorId == @ContractorId" OnSelecting="dsSpecificContratctor_Selecting"
                RenderLogVisible="false" OnInserted="dsSpecificContratctor_Inserted">
                <WhereParameters>
                    <asp:Parameter Name="ContractorId" Type="Int32" />
                </WhereParameters>
                <UpdateParameters>
                    <asp:Parameter Name="ContractorName" Type="String" />
                    <asp:Parameter Name="Contact_person" Type="String" />
                    <asp:Parameter Name="Nationality" Type="String" />
                    <asp:Parameter Name="Address" Type="String" />
                    <asp:Parameter Name="City" Type="String" />
                    <asp:Parameter Name="State" Type="String" />
                    <asp:Parameter Name="Country" Type="String" />
                    <asp:Parameter Name="Postal_Code" Type="String" />
                    <asp:Parameter Name="Phone1" Type="String" />
                    <asp:Parameter Name="Phone2" Type="String" />
                    <asp:Parameter Name="Fax" Type="String" />
                </UpdateParameters>
                <InsertParameters>
                    <asp:Parameter Name="ContractorId" Type="Int32" />
                    <asp:Parameter Name="ContractorName" Type="String" />
                    <asp:Parameter Name="Contact_person" Type="String" />
                    <asp:Parameter Name="Nationality" Type="String" />
                    <asp:Parameter Name="Address" Type="String" />
                    <asp:Parameter Name="City" Type="String" />
                    <asp:Parameter Name="State" Type="String" />
                    <asp:Parameter Name="Country" Type="String" />
                    <asp:Parameter Name="Postal_Code" Type="String" />
                    <asp:Parameter Name="Phone1" Type="String" />
                    <asp:Parameter Name="Phone2" Type="String" />
                    <asp:Parameter Name="Fax" Type="String" />
                </InsertParameters>
            </phpa:PhpaLinqDataSource>
            <asp:FormView ID="frmContractor" runat="server" DataKeyNames="ContractorId" DataSourceID="dsSpecificContratctor"
                OnItemInserted="frmContractor_ItemInserted" OnItemUpdated="frmContractor_ItemUpdated"
                OnItemDeleted="frmContractor_ItemDeleted">
                <EmptyDataTemplate>
                    <asp:Label ID="lblEmptyMsg" runat="server" Text="Select the Contractors to view details." />
                    <br />
                    <asp:LoginView ID="restricUser1" runat="server">
                        <RoleGroups>
                            <asp:RoleGroup Roles="FinanceManager,StoresManager">
                                <ContentTemplate>
                                    <asp:Label ID="lblEmptyMsg" runat="server" Text=" You can also edit them. Click on the link below to Create New Contractor" />
                                    <br />
                                    <i:LinkButtonEx ID="btnNewUser" runat="server" CausesValidation="False" Action="Submit"
                                        OnClick="btnNew_Click" Text="New Contractor" ToolTip="Click to enter new Contractor's details" />
                                </ContentTemplate>
                            </asp:RoleGroup>
                        </RoleGroups>
                        <LoggedInTemplate>
                            Only Finance Manager and Stores Manager can create new contractors.
                        </LoggedInTemplate>
                    </asp:LoginView>
                    <br />
                    <phpa:FormViewStatusMessage ID="fvDeleteStatusMessage" runat="server" />
                </EmptyDataTemplate>
                <HeaderTemplate>
                    Contractor
                    <%# Eval("ContractorCode") ?? "New"%>:
                    <%# Eval("ContractorName") %>
                </HeaderTemplate>
                <EditItemTemplate>
                    <eclipse:TwoColumnPanel ID="TwoColumnPanel1" runat="server">
                        <eclipse:LeftLabel ID="LeftLabel8" runat="server" Text="Contractor Name" />
                        <i:TextBoxEx ID="tbContractorName" runat="server" Value='<%# Bind("ContractorName") %>'
                            MaxLength="100" Size="40">
                            <Validators>
                                <i:Required />
                            </Validators>
                        </i:TextBoxEx>
                        <eclipse:LeftLabel ID="LeftLabel9" runat="server" Text="Contact Person" />
                        <i:TextBoxEx ID="tbContactPerson" runat="server" Value='<%# Bind("Contact_person") %>'
                            MaxLength="40">
                        </i:TextBoxEx>
                        <eclipse:LeftLabel ID="LeftLabel10" runat="server" Text="Nationality" />
                        <i:RadioButtonListEx ID="rblNationality" runat="server" Value='<%# Bind("Nationality") %>'
                            Orientation="Horizontal">
                            <Items>
                                <i:RadioItem Text="Bhutan National" Value="BN" Enabled="true" />
                                <i:RadioItem Text="Indian National" Value="IN" />
                                <i:RadioItem Text="Third Country" Value="TC" />
                            </Items>
                        </i:RadioButtonListEx>
                        <eclipse:LeftLabel ID="LeftLabel11" runat="server" Text="Address" />
                        <i:TextArea ID="tbParticulars" runat="server" Value='<%# Bind("Address") %>' Rows="3">
                            <Validators>
                                <i:Value MaxLength="250" />
                            </Validators>
                        </i:TextArea>
                        <eclipse:LeftLabel ID="LeftLabel12" runat="server" Text="City" />
                        <i:TextBoxEx ID="tbCity" runat="server" Value='<%# Bind("City") %>' MaxLength="25">
                        </i:TextBoxEx>
                        <eclipse:LeftLabel ID="LeftLabel13" runat="server" Text="State" />
                        <i:TextBoxEx ID="tbState" runat="server" Value='<%# Bind("State") %>' MaxLength="25">
                        </i:TextBoxEx>
                        <eclipse:LeftLabel ID="LeftLabel14" runat="server" Text="Country" />
                        <i:TextBoxEx ID="tbCountry" runat="server" Value='<%# Bind("Country") %>' MaxLength="25">
                        </i:TextBoxEx>
                        <eclipse:LeftLabel ID="LeftLabel15" runat="server" Text="Postal Code" />
                        <i:TextBoxEx ID="tbPostalCode" runat="server" Value='<%# Bind("Postal_Code") %>'
                            MaxLength="10">
                        </i:TextBoxEx>
                        <eclipse:LeftLabel ID="LeftLabel16" runat="server" Text="Primary Phone" />
                        <i:TextBoxEx ID="tbPhone1" runat="server" Value='<%# Bind("Phone1") %>' MaxLength="15">
                        </i:TextBoxEx>
                        <eclipse:LeftLabel ID="LeftLabel17" runat="server" Text="Secondary Phone" />
                        <i:TextBoxEx ID="tbPhone2" runat="server" Value='<%# Bind("Phone2") %>' MaxLength="15">
                        </i:TextBoxEx>
                        <eclipse:LeftLabel ID="LeftLabel18" runat="server" Text="Fax" />
                        <i:TextBoxEx ID="tbFax" runat="server" Value='<%# Bind("Fax") %>' MaxLength="15">
                        </i:TextBoxEx>
                    </eclipse:TwoColumnPanel>
                    <i:ButtonEx runat="server" ID="btnSave" Text="Save" CausesValidation="true" Action="Submit"
                        Icon="Disk" OnClick="btnSave_Click" />
                    <i:LinkButtonEx ID="btnCan" runat="server" Text="Cancel" CausesValidation="false"
                        OnClick="btnCancel_Click" />
                    <i:ValidationSummary runat="server" />
                </EditItemTemplate>
                <ItemTemplate>
                    <jquery:Tabs ID="TabContainer1" runat="server">
                        <jquery:JPanel runat="server" HeaderText="Details" ID="panelDetails">
                            <eclipse:TwoColumnPanel ID="TwoColumnPanel1" runat="server">
                                <eclipse:LeftLabel ID="LeftLabel19" runat="server" Text="Contractor Code" />
                                <asp:Label ID="Label1" runat="server" Text='<%# Eval("ContractorCode") %>' />
                                <eclipse:LeftLabel ID="LeftLabel20" runat="server" Text="Contractor" />
                                <asp:Label ID="Label2" runat="server" Text='<%# Eval("ContractorName") %>' />
                                <br />
                                <asp:Label ID="Label3" runat="server" Text='<%# Eval("DisplayNationality") %>' />
                                <eclipse:LeftLabel ID="LeftLabel21" runat="server" Text="Contact Person" />
                                <asp:Label ID="Label4" runat="server" Text='<%# Eval("Contact_person") %>' />
                                <eclipse:LeftLabel ID="LeftLabel22" runat="server" Text="Address" />
                                <asp:Label ID="Label5" runat="server" Text='<%# Eval("Address") %>' />
                                <eclipse:LeftLabel ID="LeftLabel23" runat="server" Text="City" />
                                <asp:Label ID="Label6" runat="server" Text='<%# Eval("City") %>' />
                                <eclipse:LeftLabel ID="LeftLabel24" runat="server" Text="State" />
                                <asp:Label ID="Label7" runat="server" Text='<%# Eval("State") %>' />
                                <eclipse:LeftLabel ID="LeftLabel25" runat="server" Text="Country" />
                                <asp:Label ID="Label8" runat="server" Text='<%# Eval("Country") %>' />
                                <eclipse:LeftLabel ID="LeftLabel26" runat="server" Text="Postal Code" />
                                <asp:Label ID="Label9" runat="server" Text='<%# Eval("Postal_Code") %>' />
                                <eclipse:LeftLabel ID="LeftLabel27" runat="server" Text="Primary Phone" />
                                <asp:Label ID="Label10" runat="server" Text='<%# Eval("Phone1") %>' />
                                <eclipse:LeftLabel ID="LeftLabel28" runat="server" Text="Secondary Phone" />
                                <asp:Label ID="Label11" runat="server" Text='<%# Eval("Phone2") %>' />
                                <eclipse:LeftLabel ID="LeftLabel29" runat="server" Text="Fax" />
                                <asp:Label ID="Label12" runat="server" Text='<%# Eval("Fax") %>' />
                            </eclipse:TwoColumnPanel>
                        </jquery:JPanel>
                        <phpa:AuditTabPanel ID="panelAudit" runat="server" CssClasses="PanelContainer" LeftCssClass="PanelLeftField"
                            RightCssClass="PanelRightField">
                            <asp:Table ID="Table1" runat="server">
                            </asp:Table>
                        </phpa:AuditTabPanel>
                    </jquery:Tabs>
                    <asp:LoginView ID="userRestriction" runat="server">
                        <RoleGroups>
                            <asp:RoleGroup Roles="FinanceManager,StoresManager">
                                <ContentTemplate>
                                    <i:LinkButtonEx ID="btnEdit" runat="server" Text="Edit" CausesValidation="false"
                                        OnClick="btnEdit_Click" />
                                    <i:LinkButtonEx ID="LinkButtonEx1" runat="server" Text="Delete" OnClick="btnDelete_Click"
                                        Action="Submit" CausesValidation="false" OnClientClick="DeleteConfirmation" />
                                    <i:LinkButtonEx ID="btnNew" runat="server" Text="New" OnClick="btnNew_Click" Action="Submit"
                                        CausesValidation="false" />
                                </ContentTemplate>
                            </asp:RoleGroup>
                        </RoleGroups>
                        <LoggedInTemplate>
                            <b>Only Finance Manager and Stores Manager can Modify Contractors. </b>
                        </LoggedInTemplate>
                        <AnonymousTemplate>
                            <b>Please login, if you want to create or manage contractor details.</b>
                        </AnonymousTemplate>
                    </asp:LoginView>
                </ItemTemplate>
                <FooterTemplate>
                    <phpa:FormViewStatusMessage ID="frmStatusMsg" runat="server" />
                </FooterTemplate>
            </asp:FormView>
        </ContentTemplate>
    </jquery:Dialog>
</asp:Content>
