<%@ Page Title="Receive GRN" Language="C#" MasterPageFile="~/MasterPage.master" CodeBehind="RecieveGRN.aspx.cs"
    EnableViewState="true" Inherits="Finance.Store.RecieveGRN" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            var $grid = $('#gvEditGRNItems');

            $(':text[name$=tbAccepted]', $grid).change(function (e) {
                var tr = $(this).closest('tr');
                var accepted = tr.find(':text[name$=tbAccepted]').val();
                var price = tr.find('span[id$=lblPrice]').html();
                var totalPrice = Number(price);
                var total;
                if (accepted == '' || price == '') {
                    total = accepted * price;
                }
                else {
                    total = accepted * totalPrice;
                }
                tr.find('span[id$=lblTotal]').html(total);
                UpdateGrandTotalNu();
            });

            $(':text', $grid).change(function (e) {
                $(this).closest('table').gridViewEx('selectRows', e, $(this).closest('tr'));
            });
            $.validator.addMethod('GreaterThanDelivered', function (value, element, params) {
                var $tbDelivered = $(':text[name$=tbDelivered]', $(element).closest('tr'));
                return value <= $tbDelivered.val();
            }, function (params, element) {
                return $.validator.format('Accepted quantity for Item {0} can not be more than delivered quantity', params);
            });
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

        function UpdateGrandTotalNu() {
            var total = 0;
            $('#gvEditGRNItems span[id$=lblTotal]').each(function () {
                var tr = $(this).closest('tr');
                var lblValue = tr.find('span[id$=lblTotal]').html();
                if (lblValue != '') {
                    total += parseFloat($(this).html().replace(',', ''));
                }
            });
            $('#lblGrandTotalNu').html(addCommas(total.toFixed(2)));
        }
       
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphSideNavigation" runat="server">
    <fieldset>
        <legend>Quick Links</legend>
        <ul>
            <li>
                <asp:HyperLink runat="server" NavigateUrl="~/Store/Reports/GRNReport.aspx" Text="View GRN Report" />
            </li>
            <li>
                <asp:HyperLink runat="server" NavigateUrl="~/Store/Reports/StockBalance.aspx" Text="See stock balances" />
            </li>
            <li>
                <asp:HyperLink runat="server" NavigateUrl="~/Store/GrnList.aspx" Text="See Received & UnReceived GRN" />
            </li>
        </ul>
    </fieldset>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cph" runat="server">
    <eclipse:TwoColumnPanel ID="TwoColumnPanel1" runat="server" WidthLeft="20%" IsValidationContainer="true">
        <eclipse:LeftLabel runat="server" Text="GRN No" />
        <i:TextBoxEx ID="tbGRN" runat="server" CaseConversion="UpperCase" FriendlyName="GRN">
            <Validators>
                <i:Required />
            </Validators>
        </i:TextBoxEx>
        <i:ButtonEx ID="btnShowReport" OnClick="btnShowReport_Click" runat="server" Text="Go"
            Icon="Refresh" CausesValidation="true" Action="Submit" IsDefault="true"/>
        <br />
        <a href="GrnList.aspx">Select from list</a>
        <i:ValidationSummary ID="valSummary" runat="server" />
    </eclipse:TwoColumnPanel>
    <phpa:PhpaLinqDataSource ID="dsReceive" runat="server" ContextTypeName="Eclipse.PhpaLibrary.Database.Store.StoreDataContext"
        TableName="GRNs" EnableUpdate="true" RenderLogVisible="false" OnSelecting="dsReceive_Selecting">
    </phpa:PhpaLinqDataSource>
    <asp:FormView ID="fvReceive" runat="server" DataSourceID="dsReceive" DataKeyNames="GRNId"
        OnItemCreated="fvReceive_ItemCreated" OnItemUpdated="fvReceive_ItemUpdated" DefaultMode="Edit">
        <EmptyDataTemplate>
            GRN Not found
        </EmptyDataTemplate>
        <HeaderTemplate>
            <phpa:FormViewContextHeader ID="fvHeader" runat="server" CurrentEntity='<%# Eval("GRNCode") %>'
                EntityName="Goods Receipt Note(GRN)" />
        </HeaderTemplate>
        <EditItemTemplate>
            <div style="float: left; width: 50%">
                <eclipse:TwoColumnPanel ID="TwoColumnPanel1" runat="server" WidthLeft="50%" WidthRight="50%">
                    <eclipse:LeftLabel runat="server" Text="GRN No." />
                    <asp:Label runat="server" Text='<%# Eval("GRNCode") %>' />
                    <eclipse:LeftLabel runat="server" Text="GRN Create Date" />
                    <asp:Label runat="server" Text='<%# Eval("GRNCreateDate","{0:d}") %>' />
                    <eclipse:LeftLabel runat="server" Text="Delivery Challan Number" />
                    <asp:Label runat="server" Text='<%# Eval("DeliveryChallanNumber") %>' />
                    <eclipse:LeftLabel runat="server" Text="Delivery Challan Date" />
                    <asp:Label runat="server" Text='<%# Eval("DeliveryChallanDate","{0:d}") %>' />
                    <eclipse:LeftLabel runat="server" Text="Invoice No" />
                    <asp:Label runat="server" Text='<%# Eval("InvoiceNo") %>' />
                    <eclipse:LeftLabel runat="server" Text="Invoice Date" />
                    <asp:Label runat="server" Text='<%# Eval("InvoiceDate","{0:d}") %>' />
                    <eclipse:LeftLabel runat="server" Text="Transportation Mode" />
                    <asp:Label runat="server" Text='<%# Eval("TransportationMode") %>' />
                </eclipse:TwoColumnPanel>
            </div>
            <div style="float: left; width: 50%">
                <eclipse:TwoColumnPanel ID="TwoColumnPanel2" runat="server" WidthLeft="50%" WidthRight="50%">
                    <eclipse:LeftLabel runat="server" Text="Conveyence Receipt No" />
                    <asp:Label runat="server" Text='<%# Eval("ConveyenceReceiptNo") %>' />
                    <eclipse:LeftLabel runat="server" Text="Other Reference Number" />
                    <asp:Label runat="server" Text='<%# Eval("OtherReferenceNumber","{0:d}") %>' />
                    <eclipse:LeftLabel runat="server" Text="Name of the Carrier" />
                    <asp:Label runat="server" Text='<%# Eval("NameofCarrier") %>' />
                    <eclipse:LeftLabel runat="server" Text="Address Of the Carrier" />
                    <asp:Label runat="server" Text='<%# Eval("AddressOfCarrier") %>' />
                    <eclipse:LeftLabel runat="server" Text="Purchase Order Number" />
                    <asp:Label runat="server" Text='<%# Eval("PONumber") %>' />
                    <eclipse:LeftLabel runat="server" Text="Purchase Order Date" />
                    <asp:Label runat="server" Text='<%# Eval("PODate","{0:d}") %>' />
                    <eclipse:LeftLabel ID="LeftLabel20" runat="server" Text="Amendment No" />
                    <asp:Label ID="Label18" runat="server" Text='<%# Eval("AmendmentNo") %>' />
                    <eclipse:LeftLabel ID="LeftLabel22" runat="server" Text="Amendment Date" />
                    <asp:Label ID="Label19" runat="server" Text='<%# Eval("AmendmentDate","{0:d}") %>' />
                    <eclipse:LeftLabel runat="server" Text="Order Placed" />
                    <asp:Label runat="server" Text='<%# Eval("OrderPlaced") %>' />
                    <eclipse:LeftLabel runat="server" Text="Contractor Name" />
                    <asp:Label runat="server" Text='<%# Eval("RoContractor.ContractorName") %>' />
                </eclipse:TwoColumnPanel>
            </div>
            <jquery:JPanel runat="server" IsValidationContainer="true" EnableViewState="true">
                <jquery:GridViewEx ID="gvEditGRNItems" runat="server" AutoGenerateColumns="false"
                    DataKeyNames="GRNItemId" ShowFooter="true" ClientIDMode="Static" Caption="List of GRN Item's"
                    EnableViewState="true" OnRowDataBound="gvEditGRNItems_RowDataBound">
                    <EmptyDataTemplate>
                        GRN Does not contain any items
                    </EmptyDataTemplate>
                    <Columns>
                        <eclipse:SequenceField />
                        <jquery:SelectCheckBoxField>
                        </jquery:SelectCheckBoxField>
                        <eclipse:MultiBoundField DataFields="ItemCode,Description,Brand,Identifier,Size"
                            HeaderText="Item" DataFormatString="{0}:{1} {2} {3}" />
                        <eclipse:MultiBoundField DataFields="QtyOrdered,Unit" DataFormatString="{0:N0} {1}"
                            HeaderText="Quantity|Ordered" HideEmptyColumn="true" />
                        <asp:TemplateField HeaderText="Quantity|Delivered">
                            <ItemStyle VerticalAlign="Top" Wrap="false" />
                            <ItemTemplate>
                                <i:TextBoxEx ID="tbDelivered" runat="server" Text='<%# Bind("QtyDelivered") %>' MaxLength="9"
                                    TabIndex="-1" FriendlyName='<%# Eval("ItemCode", "Delivered for Item {0}") %>'
                                    EnableViewState="true">
                                    <Validators>
                                        <i:Required />
                                        <i:Value Min="0" ValueType="Integer" />
                                    </Validators>
                                </i:TextBoxEx>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Quantity|Accepted">
                            <ItemStyle VerticalAlign="Top" Wrap="false" />
                            <ItemTemplate>
                                <i:TextBoxEx ID="tbAccepted" runat="server" Text='<%# Bind("AcceptedQty") %>' MaxLength="9"
                                    TabIndex="1" FriendlyName='<%# Eval("ItemCode", "Accepted for Item {0}") %>'
                                    EnableViewState="true">
                                    <Validators>
                                        <i:Required />
                                        <i:Value Min="0" ValueType="Integer" />
                                        <i:Custom Rule="GreaterThanDelivered" DependsOn="function(element) {
                                        return $(element).closest('tr').is('.ui-selected');
                                        }" DependsOnState="Custom" OnServerValidate="tbAccepted_ServerValidate" OnServerDependencyCheck="tbAccepted_ServerDependencyCheck" />
                                    </Validators>
                                </i:TextBoxEx>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Per Unit Price">
                            <ItemStyle HorizontalAlign="Right" />
                            <ItemTemplate>
                                <asp:Label ID="lblPrice" runat="server" Text='<%# string.Format("{0:N2}", Eval("Price")) %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <%-- <asp:TemplateField HeaderText="Landing Charges">
                            <ItemStyle HorizontalAlign="Right" />
                            <ItemTemplate>
                                <asp:Label ID="lblLandingCharges" runat="server" Text='<%# string.Format("{0:N2}", Eval("LandingCharges")) %>' />
                            </ItemTemplate>
                        </asp:TemplateField>--%>
                        <asp:TemplateField>
                            <ItemStyle VerticalAlign="Top" HorizontalAlign="Right" />
                            <FooterStyle VerticalAlign="Top" HorizontalAlign="Right" />
                            <HeaderTemplate>
                                <span title="Total = Accepted quantity * Price">Total (Nu)</span>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblTotal" runat="server" Text='<%# string.Format("{0:N2}",
                            (int?)Eval("AcceptedQty") * (decimal?)Eval("Price")) %>' />
                            </ItemTemplate>
                            <FooterTemplate>
                                <span id="lblGrandTotalNu"></span>
                            </FooterTemplate>
                        </asp:TemplateField>
                    </Columns>
                </jquery:GridViewEx>
                GRN Receive Date:
                <i:TextBoxEx ID="tbGRNReceiveDate" runat="server" FriendlyName="Receive Date" EnableViewState="true">
                    <Validators>
                        <i:Required />
                        <i:Date Min="-1000" Max="7" />
                    </Validators>
                </i:TextBoxEx>
                <i:ButtonEx ID="btnRecieve" runat="server" Text="Receive GRN" Icon="Custom" CustomIconName="ui-icon-disk"
                    Action="Submit" CausesValidation="true" OnClick="btnRecieve_Click" />
                <asp:HyperLink runat="server" Text="Cancel" NavigateUrl='<%# Eval("GRNId", "~/Store/CreateGRN.aspx?GRNId={0}") %>' />
                <i:ValidationSummary ID="ValidationSummary1" runat="server" />
            </jquery:JPanel>
        </EditItemTemplate>
        <FooterTemplate>
            <phpa:FormViewStatusMessage ID="statsuMessage" runat="server" />
            <asp:ValidationSummary runat="server" />
        </FooterTemplate>
    </asp:FormView>
</asp:Content>
