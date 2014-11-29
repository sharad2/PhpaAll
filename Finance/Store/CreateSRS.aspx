<%@ Page Title="Create SRS" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeBehind="CreateSRS.aspx.cs" Inherits="Finance.Store.CreateSRS" ClientIDMode="Static" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            var $grid = $('#gvEditSRSItems').change(function (e) {
                var $tr = $(e.target).closest('tr');
                var $ddlStatus = $('select', $tr);
                if (e.target != $ddlStatus[0]) {
                    // Mark the row as modified
                    $ddlStatus.val('I')
                }
                switch ($ddlStatus.val()) {
                    case 'U':
                        // New or unchanged
                        $(this).gridViewEx('unselectRows', e, $tr);
                        break;

                    case 'I':
                        // Modified row, or needs to be inserted
                        $(this).gridViewEx('selectRows', e, $tr);
                        break;

                    case 'D':
                        // Row to be deleted
                        $(this).gridViewEx('selectRows', e, $tr);
                        break;
                }
            });
            $('select[value=G]', $grid).closest('tr').one('keypress', function (e) {
                $('select', this).val('I');
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphSideNavigation" runat="server">
    <fieldset>
        <legend>Quick Links</legend>
        <ul>
            <li>
                <asp:HyperLink ID="HyperLink3" runat="server" NavigateUrl="~/Store/Reports/SRSList.aspx"
                    Text="View GIN List" />
            </li>
            <li>
                <asp:HyperLink ID="HyperLink4" runat="server" NavigateUrl="~/Store/Reports/IssueSRS.aspx"
                    Text="Issue GIN" />
            </li>
            <li>
                <asp:HyperLink ID="HyperLink5" runat="server" NavigateUrl="~/Store/Reports/StockBalance.aspx"
                    Text="View Stock Balance" />
            </li>
        </ul>
    </fieldset>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cph" runat="server">
    <asp:HiddenField ID="hfReferrer" runat="server" />
    <phpa:PhpaLinqDataSource ID="dsSRS" runat="server" ContextTypeName="Eclipse.PhpaLibrary.Database.Store.StoreDataContext"
        TableName="SRS" OnInserted="dsSRS_Inserted" OnUpdated="dsSRS_Updated" OnDeleting="dsSRS_Deleting"
        OnSelecting="dsSRS_Selecting" OnContextCreated="dsSRS_ContextCreated" OnContextCreating="dsSRS_ContextCreating"
        EnableDelete="True" EnableInsert="True" EnableUpdate="True" RenderLogVisible="False"
        AutoGenerateWhereClause="True" EntityTypeName="" Visible="True">
        <WhereParameters>
            <asp:QueryStringParameter QueryStringField="SRSId" Name="SRSId" Type="Int32" />
        </WhereParameters>
        <InsertParameters>
            <asp:Parameter Name="SRSCode" Type="String" />
            <asp:Parameter Name="SRSId" Type="Int32" />
            <asp:Parameter Name="SRSCreateDate" Type="DateTime" />
            <asp:Parameter Name="SRSFrom" Type="Int32" />
            <asp:Parameter Name="SRSTo" Type="Int32" />
            <asp:Parameter Name="IssuedTo" Type="String" />
            <asp:Parameter Name="VehicleNumber" Type="String" />
            <asp:Parameter Name="ChargeableTo" Type="Int32" />
            <asp:Parameter Name="ApplyingOfficer" Type="Int32" />
            <asp:Parameter Name="IntendingOfficer" Type="Int32" />
            <asp:Parameter Name="IssuingOfficer" Type="Int32" />
            <asp:Parameter Name="ReceivingOfficer" Type="Int32" />
        </InsertParameters>
        <UpdateParameters>
            <asp:Parameter Name="SRSCode" Type="String" />
            <asp:Parameter Name="SRSId" Type="Int32" />
            <asp:Parameter Name="SRSCreateDate" Type="DateTime" />
            <asp:Parameter Name="SRSFrom" Type="Int32" />
            <asp:Parameter Name="SRSTo" Type="Int32" />
            <asp:Parameter Name="IssuedTo" Type="String" />
            <asp:Parameter Name="VehicleNumber" Type="String" />
            <asp:Parameter Name="ChargeableTo" Type="Int32" />
            <asp:Parameter Name="ApplyingOfficer" Type="Int32" />
            <asp:Parameter Name="IntendingOfficer" Type="Int32" />
            <asp:Parameter Name="IssuingOfficer" Type="Int32" />
            <asp:Parameter Name="ReceivingOfficer" Type="Int32" />
        </UpdateParameters>
    </phpa:PhpaLinqDataSource>
    <phpa:PhpaLinqDataSource ID="dsSRSItems" runat="server" ContextTypeName="Eclipse.PhpaLibrary.Database.Store.StoreDataContext"
        RenderLogVisible="False" EnableDelete="True" EnableInsert="True" EnableUpdate="True"
        OnContextCreated="dsSRSItems_ContextCreated" TableName="SRSItems" AutoGenerateWhereClause="True"
        Visible="True" OnSelecting="dsSRS_Selecting">
        <WhereParameters>
            <asp:QueryStringParameter QueryStringField="SRSId" Name="SRSId" Type="Int32" />
        </WhereParameters>
        <InsertParameters>
            <asp:Parameter Name="SRSId" Type="Int32" />
            <asp:Parameter Name="ItemId" Type="Int32" />
            <asp:Parameter Name="HeadOfAccountId" Type="Int32" />
            <asp:Parameter Name="QtyRequired" Type="Int32" />
        </InsertParameters>
        <UpdateParameters>
            <asp:Parameter Name="SRSId" Type="Int32" />
            <asp:Parameter Name="ItemId" Type="Int32" />
            <asp:Parameter Name="HeadOfAccountId" Type="Int32" />
            <asp:Parameter Name="QtyRequired" Type="Int32" />
        </UpdateParameters>
    </phpa:PhpaLinqDataSource>
    <asp:FormView ID="fvSrs" runat="server" DataKeyNames="SRSId" DataSourceID="dsSRS"
        RenderOuterTable="False" OnItemCreated="fvSrs_ItemCreated" OnItemInserted="fvSrs_ItemInserted">
        <HeaderTemplate>
            <h3>Goods Issue Note(GIN):
                <%# Eval("SRSId") ?? "New"%>
            </h3>
            <%-- <phpa:FormViewContextHeader ID="fvHeader" runat="server" CurrentEntity='<%# Eval("SRSId") %>'
                EntityName="Goods Issue Note(GIN)" />--%>
        </HeaderTemplate>
        <ItemTemplate>
            <jquery:Tabs runat="server" Collapsible="false" Selected="1">
                <jquery:JPanel runat="server" HeaderText="Basic">
                    <eclipse:TwoColumnPanel ID="TwoColumnPanel1" runat="server">
                        <eclipse:LeftLabel runat="server" Text="SRS No." />
                        <asp:Label runat="server" Text='<%# Eval("SRSCode") %>' />
                        <eclipse:LeftLabel runat="server" Text="SRS Received Date" />
                        <asp:Label runat="server" Text='<%# Eval("SRSCreateDate", "{0:d}") %>' />
                        <eclipse:LeftLabel runat="server" Text="SRS From" />
                        <asp:Label runat="server" Text='<%# Eval("RoDivision1.DivisionName") %>' />
                        <eclipse:LeftLabel runat="server" Text="SRS To" />
                        <asp:Label runat="server" Text='<%# Eval("RoDivision2.DivisionName") %>' />
                        <eclipse:LeftLabel runat="server" Text="Material Issued To" />
                        <asp:Label runat="server" Text='<%# Eval("IssuedTo") %>' />
                        <eclipse:LeftLabel runat="server" Text="Vehicle Number" />
                        <asp:Label runat="server" Text='<%# Eval("VehicleNumber") %>' />
                        <eclipse:LeftLabel runat="server" Text="Chargeable To" />
                        <asp:Label runat="server" Text='<%# Eval("HeadOfAccount.DisplayName") %>' />
                        <eclipse:LeftLabel runat="server" Text="Approving Officer" />
                        <asp:Label runat="server" Text='<%# Eval("RoEmployee.FullName") %>' />
                        <eclipse:LeftLabel runat="server" Text="Receiving Officer" />
                        <asp:Label runat="server" Text='<%# Eval("RoEmployee4.FullName") %>' />
                        <eclipse:LeftLabel runat="server" Text="Indenting Officer" />
                        <asp:Label runat="server" Text='<%# Eval("RoEmployee1.FullName") %>' />
                        <eclipse:LeftLabel runat="server" Text="Issuing Officer" />
                        <asp:Label runat="server" Text='<%# Eval("RoEmployee3.FullName") %>' />
                    </eclipse:TwoColumnPanel>
                </jquery:JPanel>
                <jquery:JPanel runat="server" HeaderText="Details">
                    <jquery:GridViewEx ID="gvSRSItem" runat="server" AutoGenerateColumns="false" DataSource='<%# Eval("SRSItems")%>'
                        ShowFooter="true">
                        <Columns>
                            <eclipse:SequenceField>
                            </eclipse:SequenceField>
                            <eclipse:MultiBoundField HeaderText="Item|Code" DataFields="Item.ItemCode" AccessibleHeaderText="ItemCode" />
                            <eclipse:MultiBoundField HeaderText="Item|Description" DataFields="Item.Description" />
                            <eclipse:MultiBoundField HeaderText="Item|Unit" DataFields="Item.UOM.UOMCode" />
                            <eclipse:MultiBoundField HeaderText="Item|Head Of Account" DataFields="HeadOfAccount.DisplayName, HeadOfAccount.Description"
                                DataFormatString="{0}:{1}" />
                            <eclipse:MultiBoundField HeaderText="Quantity Required" DataFields="QtyRequired"
                                DataFormatString="{0:N0}" AccessibleHeaderText="QtyRequired" DataSummaryCalculation="ValueSummation">
                                <ItemStyle HorizontalAlign="Right" />
                                <FooterStyle HorizontalAlign="Right" />
                            </eclipse:MultiBoundField>
                        </Columns>
                        <EmptyDataTemplate>
                            <b>Items not found for the SRS</b>
                        </EmptyDataTemplate>
                    </jquery:GridViewEx>
                </jquery:JPanel>
                <phpa:AuditTabPanel runat="server" />
            </jquery:Tabs>
            <asp:LoginView ID="LoginView1" runat="server">
                <LoggedInTemplate>
                    <i:LinkButtonEx ID="btnEdit" runat="server" Text="Edit" CausesValidation="false"
                        Action="Submit" OnClick="btnEdit_Click" />
                    <i:LinkButtonEx ID="btnDelete" runat="server" Text="Delete" OnClick="btnDelete_Click"
                        Action="Submit" CausesValidation="false" OnClientClick="
function(e) {
    return confirm('Deletion will succeed only if any item is not issued against this GIN. Are you sure you want to delete the GIN?');
}" />
                    <i:LinkButtonEx ID="btnNew" runat="server" Text="New" OnClick="btnNew_Click" Action="Submit"
                        CausesValidation="false" />
                </LoggedInTemplate>
                <AnonymousTemplate>
                    Please Login, in order to Create and Edit Store Requisition Slip(SRS).
                </AnonymousTemplate>
            </asp:LoginView>
            <asp:HyperLink ID="HyperLink1" runat="server" Text="Report" NavigateUrl='<%# Eval("SRSId", "~/Store/Reports/SRSReport.aspx?SRSId={0}") %>' />
            <asp:HyperLink ID="HyperLink2" runat="server" Text="Issue" NavigateUrl='<%# Eval("SRSId", "~/Store/Reports/IssueSRS.aspx?SRSId={0}") %>' />
        </ItemTemplate>
        <EmptyDataTemplate>
            <asp:LoginView ID="LoginView1" runat="server">
                <LoggedInTemplate>
                    <asp:Label ID="Label1" runat="server" Text="You can also edit them. Click on the link below to Create New SRS." />
                    <br />
                    <i:LinkButtonEx ID="btnNewSRS" runat="server" Text="New" OnClick="btnNew_Click" Action="Submit"
                        ToolTip="Click to Create New SRS" CausesValidation="false" />
                </LoggedInTemplate>
                <AnonymousTemplate>
                    Please Login, in order to Insert or Manage Store Requisition Slip.
                </AnonymousTemplate>
            </asp:LoginView>
            <phpa:FormViewStatusMessage runat="server" />
        </EmptyDataTemplate>
        <EditItemTemplate>
            <jquery:Tabs ID="TabContainer1" runat="server" Collapsible="false" EnableViewState="true">
                <jquery:JPanel ID="panelBasic" runat="server" HeaderText="Basic">
                    <eclipse:TwoColumnPanel runat="server" WidthLeft="25%" WidthRight="75%">
                        <eclipse:LeftLabel runat="server" Text="SRS No." />
                        <i:TextBoxEx ID="tbSrsCode" runat="server" Text='<%# Bind("SRSCode") %>' CaseConversion="UpperCase"
                            FriendlyName="SRS No" Size="30">
                            <Validators>
                                <i:Required />
                            </Validators>
                        </i:TextBoxEx>
                        <i:TextBoxEx ID="tbSrsDate" runat="server" Text='<%# Bind("SRSCreateDate","{0:d}") %>'
                            FriendlyName="SRS Create Date" Size="20">
                            <Validators>
                                <i:Required />
                                <i:Date />
                            </Validators>
                        </i:TextBoxEx>
                        <phpa:PhpaLinqDataSource runat="server" ID="dsDivisions" ContextTypeName="Eclipse.PhpaLibrary.Reporting.ReportingDataContext"
                            OrderBy="DivisionName" Select="new (DivisionId, DivisionName, DivisionGroup)"
                            TableName="RoDivisions" Visible="True" RenderLogVisible="false">
                        </phpa:PhpaLinqDataSource>
                        <eclipse:LeftLabel ID="LeftLabel1" runat="server" Text="SRS From" />
                        <i:DropDownListEx runat="server" ID="ddlSrsFrom" DataSourceID="dsDivisions" DataTextField="DivisionName"
                            DataValueField="DivisionId" DataOptionGroupField="DivisionGroup" ClientIDMode="Static"
                            Value='<%# Bind("SRSFrom") %>'>
                            <Items>
                                <eclipse:DropDownItem Text="(Not Set)" Persistent="Always" />
                            </Items>
                        </i:DropDownListEx>
                        <br />
                        Select Division Name from where the SRS is being created.
                        <eclipse:LeftLabel runat="server" Text="SRS To" />
                        <i:DropDownListEx runat="server" ID="ddlSrsTo" DataSourceID="dsDivisions" DataTextField="DivisionName"
                            DataValueField="DivisionId" DataOptionGroupField="DivisionGroup" ClientIDMode="Static"
                            Value='<%# Bind("SRSTo") %>'>
                            <Items>
                                <eclipse:DropDownItem Text="(Not Set)" Persistent="Always" />
                            </Items>
                        </i:DropDownListEx>
                        <br />
                        Select Division Name to whom the SRS will be delivered.
                        <eclipse:LeftLabel runat="server" Text="Material Issued To" />
                        <i:TextBoxEx ID="tbIssuedTo" runat="server" Text='<%# Bind("IssuedTo") %>' MaxLength="60"
                            Size="30">
                        </i:TextBoxEx>
                        <br />
                        Designation to whom SRS is being Issued.
                        <eclipse:LeftLabel runat="server" Text="Vehicle No" />
                        <i:TextBoxEx ID="tbVechicleNumber" runat="server" Text='<%# Bind("VehicleNumber") %>'
                            MaxLength="20" Size="20">
                        </i:TextBoxEx>
                    </eclipse:TwoColumnPanel>
                </jquery:JPanel>
                <jquery:JPanel ID="panelDetails" runat="server" HeaderText="Details">
                    <eclipse:TwoColumnPanel runat="server" WidthLeft="30%" WidthRight="70%">
                        <eclipse:LeftLabel runat="server" Text="Chargeable To" />
                        <i:AutoComplete ID="tbHeadOfAccount" runat="server" Value='<%# Bind("ChargeableTo") %>'
                            FriendlyName="Chargeable To" Text='<%# Eval("HeadOfAccount.DisplayName") %>'
                            WebMethod="GetHeadOfAccount" ValidateWebMethodName="ValidateHeadOfAccount" WebServicePath="~/Services/HeadOfAccounts.asmx"
                            Width="20em">
                            <Validators>
                                <i:Custom OnServerValidate="tb_ServerValidate" />
                            </Validators>
                        </i:AutoComplete>
                        <br />
                        Select Head of Account to which the amount should be credit.
                        <eclipse:LeftLabel runat="server" Text="Approving Officer" />
                        <i:AutoComplete ID="tbApprovingOfficer" runat="server" Value='<%# Bind("ApplyingOfficer") %>'
                            FriendlyName="Approving Officer" Text='<%# Eval("RoEmployee.EmployeeCode") %>'
                            WebMethod="GetEmployees" ValidateWebMethodName="ValidateEmployee" WebServicePath="~/Services/Employees.asmx"
                            Width="20em" AutoValidate="true">
                            <Validators>
                                <i:Custom OnServerValidate="tb_ServerValidate" />
                            </Validators>
                        </i:AutoComplete>
                        <eclipse:LeftLabel runat="server" Text="Receiving Officer" />
                        <i:AutoComplete ID="tbReceivingOfficer" runat="server" Value='<%# Bind("ReceivingOfficer") %>'
                            FriendlyName="Receiving Officer" Text='<%# Eval("RoEmployee4.EmployeeCode") %>'
                            WebMethod="GetEmployees" ValidateWebMethodName="ValidateEmployee" WebServicePath="~/Services/Employees.asmx"
                            Width="20em" AutoValidate="true">
                            <Validators>
                                <i:Custom OnServerValidate="tb_ServerValidate" />
                            </Validators>
                        </i:AutoComplete>
                        <eclipse:LeftLabel runat="server" Text="Indenting Officer" />
                        <i:AutoComplete ID="tbIntendingOfficer" runat="server" Value='<%# Bind("IntendingOfficer") %>'
                            FriendlyName="Indenting Officer" Text='<%# Eval("RoEmployee1.EmployeeCode") %>'
                            WebMethod="GetEmployees" ValidateWebMethodName="ValidateEmployee" WebServicePath="~/Services/Employees.asmx"
                            Width="20em" AutoValidate="true">
                            <Validators>
                                <i:Custom OnServerValidate="tb_ServerValidate" />
                            </Validators>
                        </i:AutoComplete>
                        <eclipse:LeftLabel runat="server" Text="Issuing Officer" />
                        <i:AutoComplete ID="tbIssuingOfficer" runat="server" Value='<%# Bind("IssuingOfficer") %>'
                            FriendlyName="Issuing Officer" Text='<%# Eval("RoEmployee3.EmployeeCode") %>'
                            WebMethod="GetEmployees" ValidateWebMethodName="ValidateEmployee" WebServicePath="~/Services/Employees.asmx"
                            Width="20em" AutoValidate="true">
                            <Validators>
                                <i:Custom OnServerValidate="tb_ServerValidate" />
                            </Validators>
                        </i:AutoComplete>
                    </eclipse:TwoColumnPanel>
                </jquery:JPanel>
            </jquery:Tabs>
            <br />
            <br />
            <i:ButtonEx runat="server" ID="btnSave" Text="Save" CausesValidation="true" Action="Submit"
                Icon="Disk" OnClick="btnSave_Click" />
            <i:ValidationSummary ID="ValidationSummary2" runat="server" />
            <i:LinkButtonEx runat="server" Text="Cancel" CausesValidation="false" OnClick="btnCancel_Click" />
            <br />
            <jquery:GridViewExInsert ID="gvEditSRSItems" runat="server" AutoGenerateColumns="false"
                DataSourceID="dsSRSItems" ShowFooter="true" InsertRowsCount="10" InsertRowsAtBottom="true"
                DataKeyNames="SRSItemId">
                <Columns>
                    <eclipse:SequenceField>
                    </eclipse:SequenceField>
                    <asp:TemplateField HeaderText="Status">
                        <ItemStyle VerticalAlign="Top" />
                        <ItemTemplate>
                            <i:DropDownListEx ID="ddlStatus" runat="server" TabIndex="-1" Value="U">
                                <Items>
                                    <eclipse:DropDownItem Text="Update" Value="U" Persistent="Always" CssClass="ui-selecting" />
                                    <eclipse:DropDownItem Text="Delete" Value="D" Persistent="Always" CssClass="ui-state-error" />
                                </Items>
                            </i:DropDownListEx>
                        </ItemTemplate>
                        <InsertItemTemplate>
                            <i:DropDownListEx ID="ddlStatus" runat="server" TabIndex="-1" Value="G">
                                <Items>
                                    <eclipse:DropDownItem Text="Insert" Value="I" Persistent="Always" CssClass="ui-selecting" />
                                    <eclipse:DropDownItem Text="Ignore" Value="G" Persistent="Always" CssClass="ui-priority-secondary" />
                                </Items>
                            </i:DropDownListEx>
                        </InsertItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Item|Code" AccessibleHeaderText="Item">
                        <ItemStyle VerticalAlign="Top" Wrap="false" />
                        <ItemTemplate>
                            <i:AutoComplete ID="tbItem" runat="server" Value='<%# Bind("ItemId") %>' Text='<%# Eval("Item.Description") %>'
                                Width="25em" FriendlyName="Item Code" WebMethod="GetItems" WebServicePath="~/Services/Items.asmx"
                                ValidateWebMethodName="ValidateItem">
                                <Validators>
                                    <i:Required DependsOnState="AnyValue" DependsOnValue="I,U" DependsOn="ddlStatus" />
                                </Validators>
                            </i:AutoComplete>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Item|Unit" AccessibleHeaderText="Unit">
                        <ItemStyle VerticalAlign="Top" />
                        <ItemTemplate>
                            <asp:Label ID="lblUnit" runat="server" Text='<%# Eval("Item.UOM.UOMCode") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Item|Head of Account" AccessibleHeaderText="Description">
                        <ItemStyle VerticalAlign="Top" />
                        <ItemTemplate>
                            <i:AutoComplete ID="tbHeadOfAccount" runat="server" Value='<%# Bind("HeadOfAccountId") %>'
                                Text='<%# string.Format("{0}:{1}",Eval("HeadOfAccount.DisplayName"),Eval("HeadOfAccount.Description")) %>'
                                FriendlyName="Head of Account" Width="25em" ValidateWebMethodName="ValidateHeadOfAccount"
                                WebMethod="GetHeadOfAccount" WebServicePath="~/Services/HeadOfAccounts.asmx">
                                <Validators>
                                    <i:Required DependsOnState="AnyValue" DependsOnValue="I,U" DependsOn="ddlStatus" />
                                </Validators>
                            </i:AutoComplete>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Qty. Req.">
                        <ItemStyle VerticalAlign="Top" Wrap="false" />
                        <ItemTemplate>
                            <i:TextBoxEx ID="tbRequired" runat="server" Value='<%# Bind("QtyRequired") %>' MaxLength="9"
                                FriendlyName="Required Quantity">
                                <Validators>
                                    <i:Required DependsOnState="AnyValue" DependsOnValue="I,U" DependsOn="ddlStatus" />
                                    <i:Value Min="0" ValueType="Integer" />
                                </Validators>
                            </i:TextBoxEx>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </jquery:GridViewExInsert>
            <i:LinkButtonEx runat="server" ID="btnAddrow" Text="Show more rows" CausesValidation="true"
                OnClick="btnAddrow_Click" ToolTip="If you need more detail lines for the GIN, click here"
                Action="Submit" />
            <i:LinkButtonEx ID="btnCancelBottom" runat="server" Text="Cancel" CausesValidation="false"
                OnClick="btnCancel_Click" />
        </EditItemTemplate>
    </asp:FormView>
</asp:Content>
