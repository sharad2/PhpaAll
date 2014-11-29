<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" CodeBehind="CreateGRN.aspx.cs"
    Inherits="Finance.Store.CreateGRN" Title=" Create GRN" ClientIDMode="Static" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            var $grid = $('#gvEditGRNItems');
            // If a new row is touched, mark it for insert one time only
            $('select[value=G]', $grid).closest('tr').one('keypress', function (e) {
                $('select', this).val('I');
            });
            $grid.change(function (e) {
                var tr = $(e.target).closest('tr');
                var delivered = tr.find('input:text[name$=tbReceived]').val();
                var price = tr.find('input:text[name$=tbPrice]').val();
                var totalPrice = Number(price);
                var total = delivered * totalPrice;
                tr.find('span[id$=lblTotal]').html(total);
                UpdateGrandTotalNu();
                $(e.target).is(':text[name$=tbOrdered]') && $('input:text[name$=tbReceived]', tr).val($(e.target).val());
            });
            UpdateGrandTotalNu(this);
        }).change(function (e) {
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

        function addCommas(nStr) {
            nStr += '';
            x = nStr.split('.');
            x1 = x[0];
            x2 = x.length > 1 ? '.' + x[1] : '';
            var rgx = /(\d+)(\d{3})/;
            while (rgx.test(x1)) {
                x1 = x1.replace(rgx, '$1' + ',' + '$2');
            }
            return x1 + x2;
        }

        function UpdateGrandTotalNu(grid) {
            var total = 0;
            $('span[id$=lblTotal]', grid).each(function () {
                var tr = $(this).closest('tr');
                var lblValue = tr.find('span[id$=lblTotal]').html();
                if (lblValue != '') {
                    total += parseFloat($(this).html().replace(',', ''));
                }
            });
            $('#lblGrandTotalNu').html(addCommas(total.toFixed(2)));
        }

        $.validator.addMethod('ReceivedLeOrdered', function (value, element, params) {
            var delivered = value;
            if (!delivered) {
                return true;
            }
            var ordered = $(element).closest('tr').find('input:text[name$=tbOrdered]').val();
            if (!ordered) {
                return false;
            }
            return Number(delivered) <= Number(ordered);
        });

    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphSideNavigation" runat="server">
    <asp:HyperLink ID="HyperLink2" runat="server" Text="Help" NavigateUrl="~/Doc/CreateGRN.doc.aspx" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cph" runat="server">
    <div>
        Create a GRN when you receive items from the supplier's carrier.
    </div>
    <asp:HiddenField ID="hfReferrer" runat="server" />
    <phpa:PhpaLinqDataSource ID="dsGRN" runat="server" RenderLogVisible="False" ContextTypeName="Eclipse.PhpaLibrary.Database.Store.StoreDataContext"
        TableName="GRNs" EnableInsert="True" EnableDelete="True" EnableUpdate="True"
        OnInserted="dsGRN_Inserted" AutoGenerateWhereClause="true" OnUpdated="dsGRN_Updated"
        OnContextCreating="ds_ContextCreating" OnDeleting="dsGRN_Deleting" OnSelecting="dsGRN_Selecting"
        OnContextCreated="dsGRN_ContextCreated">
        <WhereParameters>
            <asp:QueryStringParameter QueryStringField="GRNId" Name="GRNId" Type="Int32" />
        </WhereParameters>
        <UpdateParameters>
            <asp:Parameter Name="ContractorId" Type="Int32" />
            <asp:Parameter Name="GRNId" Type="Int32" />
            <asp:Parameter Name="GRNCode" Type="String" />
            <asp:Parameter Name="GRNCreateDate" Type="DateTime" />
            <asp:Parameter Name="PONumber" Type="String" />
            <asp:Parameter Name="PODate" Type="DateTime" />
            <asp:Parameter Name="DeliveryChallanNumber" Type="String" />
            <asp:Parameter Name="DeliveryChallanDate" Type="DateTime" />
            <asp:Parameter Name="InvoiceNo" Type="String" />
            <asp:Parameter Name="InvoiceDate" Type="DateTime" />
            <asp:Parameter Name="TransportationMode" Type="String" />
            <asp:Parameter Name="ConveyenceReceiptNo" Type="String" />
            <asp:Parameter Name="NameofCarrier" Type="String" />
            <asp:Parameter Name="AddressOfCarrier" Type="String" />
            <asp:Parameter Name="OtherReferenceNumber" Type="String" />
            <asp:Parameter Name="OrderPlaced" Type="String" />
            <asp:Parameter Name="Remarks" Type="String" />
            <asp:Parameter Name="AmendmentNo" Type="String" />
            <asp:Parameter Name="AmendmentDate" Type="DateTime" />
        </UpdateParameters>
        <InsertParameters>
            <asp:Parameter Name="ContractorId" Type="Int32" />
            <asp:Parameter Name="GRNId" Type="Int32" />
            <asp:Parameter Name="GRNCode" Type="String" />
            <asp:Parameter Name="GRNCreateDate" Type="DateTime" />
            <asp:Parameter Name="PONumber" Type="String" />
            <asp:Parameter Name="PODate" Type="DateTime" />
            <asp:Parameter Name="DeliveryChallanNumber" Type="String" />
            <asp:Parameter Name="DeliveryChallanDate" Type="DateTime" />
            <asp:Parameter Name="InvoiceNo" Type="String" />
            <asp:Parameter Name="InvoiceDate" Type="DateTime" />
            <asp:Parameter Name="TransportationMode" Type="String" />
            <asp:Parameter Name="ConveyenceReceiptNo" Type="String" />
            <asp:Parameter Name="NameofCarrier" Type="String" />
            <asp:Parameter Name="AddressOfCarrier" Type="String" />
            <asp:Parameter Name="OtherReferenceNumber" Type="String" />
            <asp:Parameter Name="OrderPlaced" Type="String" />
            <asp:Parameter Name="Remarks" Type="String" />
            <asp:Parameter Name="AmendmentNo" Type="String" />
            <asp:Parameter Name="AmendmentDate" Type="DateTime" />
        </InsertParameters>
    </phpa:PhpaLinqDataSource>
    <phpa:PhpaLinqDataSource ID="dsGRNItems" runat="server" RenderLogVisible="False"
        ContextTypeName="Eclipse.PhpaLibrary.Database.Store.StoreDataContext" TableName="GRNItems"
        OnContextCreating="ds_ContextCreating" EnableInsert="True" EnableDelete="True"
        EnableUpdate="True" AutoGenerateWhereClause="true" OnSelecting="dsGRN_Selecting"
        OnContextCreated="dsGRNItems_ContextCreated">
        <WhereParameters>
            <asp:QueryStringParameter QueryStringField="GRNId" Name="GRNId" Type="Int32" />
        </WhereParameters>
        <InsertParameters>
            <asp:Parameter Name="GRNId" Type="Int32" />
            <asp:Parameter Name="ItemId" Type="Int32" />
            <asp:Parameter Name="GRNItemId" Type="Int32" />
            <asp:Parameter Name="OrderedQty" Type="Int32" />
            <asp:Parameter Name="ReceivedQty" Type="Int32" />
            <asp:Parameter Name="Price" Type="Decimal" />
        </InsertParameters>
        <UpdateParameters>
            <asp:Parameter Name="GRNId" Type="Int32" />
            <asp:Parameter Name="ItemId" Type="Int32" />
            <asp:Parameter Name="GRNItemId" Type="Int32" />
            <asp:Parameter Name="OrderedQty" Type="Int32" />
            <asp:Parameter Name="ReceivedQty" Type="Int32" />
            <asp:Parameter Name="Price" Type="Decimal" />
        </UpdateParameters>
    </phpa:PhpaLinqDataSource>
    <asp:FormView ID="fvEdit" runat="server" DataSourceID="dsGRN" DataKeyNames="GRNId"
        OnItemCreated="fvEdit_ItemCreated" OnItemInserted="fvEdit_ItemInserted" RenderOuterTable="false">
        <HeaderTemplate>
            <h3>
                Goods Receipt Note(GRN):
                <%# Eval("GRNCode") ?? "New"%></h3>
            <%--<phpa:FormViewContextHeader ID="fvHeader" runat="server" CurrentEntity='<%# Eval("GRNCode") %>'
                EntityName="Goods Receipt Note(GRN)" />--%>
            <phpa:FormViewStatusMessage ID="FormViewStatusMessage1" runat="server" />
        </HeaderTemplate>
        <ItemTemplate>
            <jquery:Tabs ID="tabContainer" runat="server" Collapsible="false" Selected="1">
                <jquery:JPanel ID="JPanel1" runat="server" HeaderText="GRN">
                    <eclipse:TwoColumnPanel ID="TwoColumnPanel1" runat="server">
                        <eclipse:LeftLabel ID="LeftLabel1" runat="server" Text="GRN No." />
                        <asp:Label ID="Label1" runat="server" Text='<%# Eval("GRNCode") %>' />
                        <eclipse:LeftLabel ID="LeftLabel2" runat="server" Text="GRN Create Date" />
                        <asp:Label ID="Label2" runat="server" Text='<%# Eval("GRNCreateDate","{0:d}") %>' />
                        <eclipse:LeftLabel ID="LeftLabel3" runat="server" Text="GRN Receive Date" />
                        <asp:Label ID="Label3" runat="server" Text='<%# Eval("GRNReceiveDate","{0:d}") %>' />
                        <eclipse:LeftLabel ID="LeftLabel4" runat="server" Text="Delivery Challan Number" />
                        <asp:Label ID="Label4" runat="server" Text='<%# Eval("DeliveryChallanNumber") %>' />
                        <eclipse:LeftLabel ID="LeftLabel5" runat="server" Text="Delivery Challan Date" />
                        <asp:Label ID="Label5" runat="server" Text='<%# Eval("DeliveryChallanDate","{0:d}") %>' />
                        <eclipse:LeftLabel ID="LeftLabel6" runat="server" Text="Invoice No" />
                        <asp:Label ID="Label6" runat="server" Text='<%# Eval("InvoiceNo") %>' />
                        <eclipse:LeftLabel ID="LeftLabel7" runat="server" Text="Invoice Date" />
                        <asp:Label ID="Label7" runat="server" Text='<%# Eval("InvoiceDate","{0:d}") %>' />
                        <eclipse:LeftLabel ID="LeftLabel8" runat="server" Text="Transportation Mode" />
                        <asp:Label ID="Label8" runat="server" Text='<%# Eval("TransportationMode") %>' />
                        <eclipse:LeftLabel ID="LeftLabel9" runat="server" Text="Conveyence Receipt No" />
                        <asp:Label ID="Label9" runat="server" Text='<%# Eval("ConveyenceReceiptNo") %>' />
                        <eclipse:LeftLabel ID="LeftLabel10" runat="server" Text="Other Reference Number" />
                        <asp:Label ID="Label10" runat="server" Text='<%# Eval("OtherReferenceNumber","{0:d}") %>' />
                        <eclipse:LeftLabel ID="LeftLabel11" runat="server" Text="Name of the Carrier" />
                        <asp:Label ID="Label11" runat="server" Text='<%# Eval("NameofCarrier") %>' />
                        <eclipse:LeftLabel ID="LeftLabel12" runat="server" Text="Address Of the Carrier" />
                        <asp:Label ID="Label12" runat="server" Text='<%# Eval("AddressOfCarrier") %>' />
                        <eclipse:LeftLabel ID="LeftLabel13" runat="server" Text="Purchase Order Number" />
                        <asp:Label ID="Label13" runat="server" Text='<%# Eval("PONumber") %>' />
                        <eclipse:LeftLabel ID="LeftLabel14" runat="server" Text="Purchase Order Date" />
                        <asp:Label ID="Label14" runat="server" Text='<%# Eval("PODate","{0:d}") %>' />
                        <eclipse:LeftLabel ID="LeftLabel20" runat="server" Text="Amendment No" />
                        <asp:Label ID="Label18" runat="server" Text='<%# Eval("AmendmentNo") %>' />
                        <eclipse:LeftLabel ID="LeftLabel22" runat="server" Text="Amendment Date" />
                        <asp:Label ID="Label19" runat="server" Text='<%# Eval("AmendmentDate","{0:d}") %>' />
                        <eclipse:LeftLabel ID="LeftLabel15" runat="server" Text="Order Placed" />
                        <asp:Label ID="Label15" runat="server" Text='<%# Eval("OrderPlaced") %>' />
                        <eclipse:LeftLabel ID="LeftLabel16" runat="server" Text="Contractor Name" />
                        <asp:Label ID="Label16" runat="server" Text='<%# Eval("RoContractor.ContractorName") %>' />
                        <eclipse:LeftLabel ID="LeftLabel17" runat="server" Text="Remarks" />
                        <asp:Label ID="Label17" runat="server" Text='<%# Eval("Remarks") %>' />
                    </eclipse:TwoColumnPanel>
                </jquery:JPanel>
                <jquery:JPanel ID="tabPanelDetails" runat="server" HeaderText="Details">
                    <jquery:GridViewEx ID="gvGRNItems" runat="server" AutoGenerateColumns="false" ShowFooter="true"
                        OnRowDataBound="gvGRNItems_RowDataBound" DataSourceID="dsGRNItems">
                        <Columns>
                            <eclipse:SequenceField>
                            </eclipse:SequenceField>
                            <eclipse:MultiBoundField HeaderText="Item|Code" DataFields="Item.ItemCode" AccessibleHeaderText="ItemCode" />
                            <eclipse:MultiBoundField HeaderText="Item|Description" DataFields="Item.Description,Item.Color,Item.Size,Item.Dimension"
                                DataFormatString="{0} {1} {2} {3}" />
                            <eclipse:MultiBoundField HeaderText="Item|Unit" DataFields="Item.UOM.UOMCode" FooterText="Total" />
                            <eclipse:MultiBoundField DataFields="OrderedQty" DataFormatString="{0:N0}" HeaderText="Quantity|Ordered">
                                <ItemStyle HorizontalAlign="Right" />
                            </eclipse:MultiBoundField>
                            <eclipse:MultiBoundField DataFields="ReceivedQty" DataFormatString="{0:N0}" HeaderText="Quantity|Delivered">
                                <ItemStyle HorizontalAlign="Right" />
                            </eclipse:MultiBoundField>
                            <eclipse:MultiBoundField DataFields="Price" HeaderText="Price" AccessibleHeaderText="Price"
                                DataFormatString="{0:N2}">
                                <ItemStyle HorizontalAlign="Right" />
                            </eclipse:MultiBoundField>
                            <eclipse:MultiBoundField HeaderText="Total" AccessibleHeaderText="Total" ItemStyle-HorizontalAlign="Right"
                                FooterStyle-HorizontalAlign="Right" DataFormatString="{0:N2}" HeaderToolTip="(Price + Landing Charges) * Accepted Quantity" />
                        </Columns>
                        <EmptyDataTemplate>
                            <b>No Item found for this GRN.</b>
                        </EmptyDataTemplate>
                    </jquery:GridViewEx>
                </jquery:JPanel>
                <phpa:AuditTabPanel ID="tabPanelAudit" runat="server" />
            </jquery:Tabs>
            <asp:LoginView ID="lv" runat="server">
                <LoggedInTemplate>
                    <i:LinkButtonEx ID="btnEdit" runat="server" Text="Edit" CausesValidation="false"
                        Action="Submit" OnClick="btnEdit_Click" />
                    <i:LinkButtonEx ID="btnDelete" runat="server" Text="Delete" OnClick="btnDelete_Click"
                        Action="Submit" CausesValidation="false" />
                    <i:LinkButtonEx ID="btnNew" runat="server" Text="New" OnClick="btnNew_Click" Action="Submit"
                        CausesValidation="false" />
                </LoggedInTemplate>
                <AnonymousTemplate>
                    Please Login, in order to Create or Edit Goods Receipt Notes(GRN).
                </AnonymousTemplate>
            </asp:LoginView>
            <asp:HyperLink runat="server" ID="hlReport" Text="Report" NavigateUrl='<%# Eval("GRNId", "~/Store/Reports/GRNReport.aspx?GRNId={0}") %>'
                EnableViewState="false" />
            <asp:HyperLink ID="HyperLink1" runat="server" Text="Receive" NavigateUrl='<%# Eval("GRNId", "~/Store/RecieveGRN.aspx?GRNId={0}") %>'
                EnableViewState="false" />
        </ItemTemplate>
        <EditItemTemplate>
            <jquery:Tabs runat="server" ID="tabContainer1" Collapsible="false" EnableViewState="true">
                <jquery:JPanel ID="panelMain" runat="server" HeaderText="GRN Details" EnableViewState="true">
                    <eclipse:TwoColumnPanel ID="TwoColumnPanel2" runat="server" WidthLeft="30%" WidthRight="70%">
                        <eclipse:LeftLabel runat="server" Text="GRN No." />
                        <i:TextBoxEx ID="tbGrnCode" runat="server" Text='<%# Bind("GRNCode") %>' CaseConversion="UpperCase"
                            FriendlyName="GRN No." Size="36">
                            <Validators>
                                <i:Required />
                            </Validators>
                        </i:TextBoxEx>
                        <i:TextBoxEx ID="tbGRNCreateDate" runat="server" Value='<%# Bind("GRNCreateDate", "{0:d}") %>'
                            FriendlyName="GRN Create Date">
                            <Validators>
                                <i:Required />
                                <i:Date />
                            </Validators>
                        </i:TextBoxEx>
                        <eclipse:LeftLabel runat="server" Text="Purchase Order No." />
                        <i:TextBoxEx ID="tbPoNo" runat="server" Text='<%# Bind("PONumber") %>' MaxLength="50"
                            Size="36" CaseConversion="UpperCase" FriendlyName="Purchase Order No.">
                            <Validators>
                                <i:Required />
                            </Validators>
                        </i:TextBoxEx>
                        <i:TextBoxEx ID="tbPoDate" runat="server" Value='<%# Bind("PODate", "{0:d}") %>'>
                            <Validators>
                                <i:Date />
                            </Validators>
                        </i:TextBoxEx>
                        <eclipse:LeftLabel runat="server" Text="Amendment No." />
                        <i:TextBoxEx ID="tbAmendmentNo" runat="server" Text='<%# Bind("AmendmentNo") %>'
                            MaxLength="50" Size="36" CaseConversion="UpperCase" FriendlyName="Amendment Order No.">
                        </i:TextBoxEx>
                        <i:TextBoxEx ID="tbAmendmentDate" runat="server" Value='<%# Bind("AmendmentDate", "{0:d}") %>'>
                            <Validators>
                                <i:Date />
                            </Validators>
                        </i:TextBoxEx>
                        <eclipse:LeftLabel runat="server" Text="Delivery Challan No." />
                        <i:TextArea ID="tbDeliveryChallanNumber" Cols="50" Rows="4" runat="server" Text='<%# Bind("DeliveryChallanNumber") %>'>
                        </i:TextArea>
                        <br />
                        Max 200 charcters
                        <eclipse:LeftLabel runat="server" Text="Delivery Challan Date" />
                        <i:TextBoxEx ID="tbDeliveryChallanDate" runat="server" Value='<%# Bind("DeliveryChallanDate", "{0:d}") %>'>
                            <Validators>
                                <i:Date />
                            </Validators>
                        </i:TextBoxEx>
                        <eclipse:LeftLabel runat="server" Text="Bill/Invoice" />
                        <i:TextArea ID="tbInvoiceNo" Cols="50" Rows="4" runat="server" Text='<%# Bind("InvoiceNo") %>'>
                        </i:TextArea>
                        <br />
                        Max 200 charcters
                        <eclipse:LeftLabel runat="server" Text="Bill/Invoice Date" />
                        <i:TextBoxEx ID="tbInvoiceDate" runat="server" Value='<%# Bind("InvoiceDate", "{0:d}") %>'>
                            <Validators>
                                <i:Date />
                            </Validators>
                        </i:TextBoxEx>
                    </eclipse:TwoColumnPanel>
                </jquery:JPanel>
                <jquery:JPanel ID="supplierInfo" runat="server" HeaderText="Supplier">
                    <eclipse:TwoColumnPanel ID="eclPanel" runat="server" WidthLeft="30%" WidthRight="70%">
                        <eclipse:LeftLabel runat="server" Text="Supplier" />
                        <i:AutoComplete ID="tbContractor" runat="server" FriendlyName="Contractor" Value='<%# Bind("ContractorId") %>'
                            Text='<%# Eval("RoContractor.ContractorName") %>' WebMethod="GetContractors"
                            ValidateWebMethodName="ValidateContractor" WebServicePath="~/Services/Contractors.asmx">
                            <Validators>
                                <i:Custom OnServerValidate="tb_ServerValidate" />
                            </Validators>
                        </i:AutoComplete>
                        <eclipse:LeftLabel runat="server" Text="Transportation Mode" />
                        <i:TextBoxEx ID="tbTptMode" runat="server" Text='<%# Bind("TransportationMode") %>'
                            MaxLength="25" Size="15" />
                        <br />
                        Set the Transportation Mode. For eg. By Air, Road etc.
                        <eclipse:LeftLabel runat="server" Text="Lorry/Railway Receipt No." />
                        <i:TextBoxEx ID="tbConvRptNo" runat="server" Text='<%# Bind("ConveyenceReceiptNo") %>'
                            MaxLength="25" Size="15" />
                        <eclipse:LeftLabel runat="server" Text="Carrier Name" />
                        <i:TextBoxEx ID="tbNmCarr" runat="server" Text='<%# Bind("NameofCarrier") %>' MaxLength="25"
                            Size="15" />
                        <br />
                        Set the carrier Name. For eg. Fedex, DHL etc.
                        <eclipse:LeftLabel runat="server" Text="Carrier Address" />
                        <i:TextArea ID="tbAddCarr" runat="server" Text='<%# Bind("AddressOfCarrier") %>'
                            Cols="30" />
                        <eclipse:LeftLabel runat="server" Text="Other Reference No." />
                        <i:TextBoxEx ID="tbOtRefNo" runat="server" Text='<%# Bind("OtherReferenceNumber") %>'
                            MaxLength="25" Size="15" />
                        <eclipse:LeftLabel runat="server" Text="Order Placed" />
                        <i:DropDownListEx ID="ddlOrderPlaced" runat="server" Value='<%# Bind("OrderPlaced") %>'>
                            <Items>
                                <eclipse:DropDownItem Text="(Not Set)" Value="" Persistent="Always" />
                                <eclipse:DropDownItem Text="Verbal" Value="Verbal" Persistent="Always" />
                                <eclipse:DropDownItem Text="Telephonic" Value="Telephonic" Persistent="Always" />
                                <eclipse:DropDownItem Text="Written" Value="Written" Persistent="Always" />
                            </Items>
                        </i:DropDownListEx>
                        <br />
                        Set the Order Placed mode. For eg. Verbal, Telephonic etc.
                    </eclipse:TwoColumnPanel>
                </jquery:JPanel>
                <jquery:JPanel ID="remarks" runat="server" HeaderText="Remarks">
                    <i:TextArea ID="tbRemarks" runat="server" Text='<%# Bind("Remarks") %>' Cols="100"
                        Rows="12" />
                </jquery:JPanel>
            </jquery:Tabs>
            <br />
            <br />
            <i:ButtonEx runat="server" ID="btnSave" Text="Save" CausesValidation="true" Action="Submit"
                Icon="Disk" OnClick="btnSave_Click" DisableClientValidation="false" />
            <i:ValidationSummary ID="ValidationSummary2" runat="server" />
            <i:LinkButtonEx runat="server" Text="Cancel" CausesValidation="false" OnClick="btnCancel_Click" />
            <jquery:GridViewExInsert ID="gvEditGRNItems" runat="server" AutoGenerateColumns="false"
                DataKeyNames="GRNItemId" ShowFooter="true" InsertRowsCount="10" InsertRowsAtBottom="true"
                DataSourceID="dsGRNItems">
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
                    <asp:TemplateField HeaderText="Item" AccessibleHeaderText="Item">
                        <ItemStyle VerticalAlign="Top" Wrap="false" />
                        <ItemTemplate>
                            <i:AutoComplete ID="tbItem" runat="server" Value='<%# Bind("ItemId") %>' Text='<%# Eval("Item.Description") %>'
                                FriendlyName="Item" WebMethod="GetItems" WebServicePath="~/Services/Items.asmx"
                                Width="25em">
                                <Validators>
                                    <i:Required DependsOnState="AnyValue" DependsOnValue="I,U" DependsOn="ddlStatus" />
                                </Validators>
                            </i:AutoComplete>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Unit">
                        <ItemStyle VerticalAlign="Top" />
                        <ItemTemplate>
                            <asp:Label ID="lblUnit" runat="server" Text='<%# Eval("Item.UOM.UOMCode") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Quantity|Ordered">
                        <ItemStyle VerticalAlign="Top" Wrap="false" />
                        <ItemTemplate>
                            <i:TextBoxEx ID="tbOrdered" runat="server" Value='<%# Bind("OrderedQty") %>' MaxLength="6"
                                ToolTip="Quantity which is mentioned in Purchase Order" FriendlyName="Ordered Quantity">
                                <Validators>
                                    <i:Required DependsOnState="Checked" DependsOn="tbItem" />
                                    <i:Value Min="0" ValueType="Integer" />
                                </Validators>
                            </i:TextBoxEx>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Quantity|Delivered">
                        <ItemStyle VerticalAlign="Top" Wrap="false" />
                        <ItemTemplate>
                            <i:TextBoxEx ID="tbReceived" runat="server" Value='<%# Bind("ReceivedQty") %>' MaxLength="6"
                                ToolTip="Quantity which is delivered by the Supplier" FriendlyName="Delivered Quantity">
                                <Validators>
                                    <i:Required DependsOnState="Checked" DependsOn="tbOrdered" />
                                    <i:Value ValueType="Integer" />
                                    <i:Custom Rule="ReceivedLeOrdered" StringParams="#tbOrdered" ClientMessage="Delivered qty cannot be greater than Ordered qty 11222"
                                        OnServerValidate="tbOrdered_ServerValidate" />
                                </Validators>
                            </i:TextBoxEx>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Price">
                        <ItemStyle VerticalAlign="Top" Wrap="false" />
                        <ItemTemplate>
                            <i:TextBoxEx ID="tbPrice" runat="server" Text='<%# Bind("Price", "{0:N2}")%>' Size="10">
                                <Validators>
                                    <i:Value ValueType="Decimal" />
                                </Validators>
                            </i:TextBoxEx>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <ItemStyle VerticalAlign="Top" HorizontalAlign="Right" />
                        <FooterStyle VerticalAlign="Top" HorizontalAlign="Right" />
                        <HeaderTemplate>
                            <span title="Total = Accepted quantity * (Price + Landing Charges)">Total (Nu)</span>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblTotal" runat="server" Text='<%# string.Format("{0:N2}",
                            (int?)Eval("ReceivedQty") * ((decimal?)Eval("Price") + (decimal?)Eval("LandedPrice"))) %>' />
                        </ItemTemplate>
                        <FooterTemplate>
                            <span id="lblGrandTotalNu"></span>
                        </FooterTemplate>
                    </asp:TemplateField>
                </Columns>
            </jquery:GridViewExInsert>
            <i:LinkButtonEx runat="server" ID="btnAddrow" Text="Show more rows" CausesValidation="true"
                OnClick="btnAddrow_Click" ToolTip="If you need more detail lines for the GRN, click here"
                Action="Submit" />
            <i:LinkButtonEx ID="btnCancelBottom" runat="server" Text="Cancel" CausesValidation="false"
                OnClick="btnCancel_Click" />
        </EditItemTemplate>
        <EmptyDataTemplate>
            What would you like to do now ?
            <ul>
                <asp:LoginView ID="LoginView1" runat="server">
                    <RoleGroups>
                        <asp:RoleGroup Roles="Operator">
                            <ContentTemplate>
                                <li>You can
                                    <i:LinkButtonEx ID="btnInsert" runat="server" Text="Create New GRN" OnClick="btnNew_Click"
                                        Action="Submit" />
                                    . </li>
                                <li>To View GRN Details,
                                    <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="~/Store/Reports/GRNReport.aspx">Go to GRN Details</asp:HyperLink>
                                    and select the GRN to View Details. </li>
                                <li>
                            </ContentTemplate>
                        </asp:RoleGroup>
                    </RoleGroups>
                    <LoggedInTemplate>
                        <asp:Label runat="server" Text=" You can also edit them. Click on the link below to Create New GRN" />
                        <br />
                        <i:LinkButtonEx ID="btnInsert" runat="server" Text="Create New GRN" OnClick="btnNew_Click"
                            Action="Submit" />
                    </LoggedInTemplate>
                    <AnonymousTemplate>
                        <li>To create GRN, you must
                            <asp:HyperLink ID="HyperLink3" runat="server" NavigateUrl="~/Login.aspx">login</asp:HyperLink>.
                        </li>
                    </AnonymousTemplate>
                </asp:LoginView>
                <li><a href='<%= this.hfReferrer.Value %>'>Go back</a> to where you came from. </li>
                <phpa:FormViewStatusMessage runat="server" />
            </ul>
        </EmptyDataTemplate>
    </asp:FormView>
</asp:Content>
