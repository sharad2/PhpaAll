<%@ Page Title="Monthly Physical Progress" Language="C#" MasterPageFile="~/MIS/NestedMIS.master"
    EnableViewState="true" CodeBehind="MonthlyPhysical.aspx.cs" Inherits="Finance.MIS.MonthlyPhysicalFinancialProgress" %>

<%@ Register Src="../Controls/PrinterFriendlyButton.ascx" TagName="PrinterFriendlyButton"
    TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        var _$activityRow;
        $(document).ready(function () {
            $('#gv').click(function (e) {
                if ($(e.target).is('[href=#]')) {
                    _$activityRow = $(e.target).closest('tr');
                    var keys = $(this).gridViewEx('keys', _$activityRow);
                    $('#dlgActivity').ajaxDialog('option', 'data', { ActivityId: keys[0][0] })
                        .ajaxDialog('load');
                }
            }).find('input:text,textarea').bind('change', function (e) {
                $('#gv').gridViewEx('selectRows', e, $(this).closest('tr'));
            });
        });

        function btnSaveProgress_Click(e) {
            var $gv = $('#gv');
            var $selectedRows = $gv.gridViewEx('selectedRows');
            if ($selectedRows.length == 0) {
                alert('Please select at least one row to Insert.');
                return false;
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphSideNavigation" runat="server">
    <uc2:PrinterFriendlyButton ID="pfb" runat="server" EnableViewState="false" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cph" runat="server">
    <eclipse:TwoColumnPanel ID="tcp" runat="server" IsValidationContainer="true">
        <eclipse:LeftLabel runat="server" Text="Progress Format" />
        <phpa:PhpaLinqDataSource ID="dsProgressFormat" runat="server" ContextTypeName="Eclipse.PhpaLibrary.Database.MIS.MISDataContext"
            TableName="ProgressFormats" RenderLogVisible="False" OnSelecting="dsProgressFormat_Selecting">
        </phpa:PhpaLinqDataSource>
        <i:DropDownListEx ID="ddlProgressformat" runat="server" DataSourceID="dsProgressFormat"
            QueryString="ProgressFormatId" DataTextField="DisplayName" DataValueField="ProgressFormatId"
            FriendlyName="Progress Format">
            <Items>
                <eclipse:DropDownItem Text="(No formats available)" Persistent="WhenEmpty" />
            </Items>
            <Validators>
                <i:Required />
            </Validators>
        </i:DropDownListEx>
        <eclipse:LeftLabel runat="server" Text="Month" />
        <i:DropDownSuggest runat="server" ID="ddlProgressDate" FriendlyName="Progress Month"
            QueryString="ProgressDate">
            <Cascadable CascadeParentId="ddlProgressformat" WebMethod="GetProgressDate" InitializeAtStartup="true" />
            <Items>
                <eclipse:DropDownItem Text="(Select Month)" Persistent="Always" />
            </Items>
            <TextBox ID="tbProgressDate" runat="server" FriendlyName="Progress Month">
                <Validators>
                    <i:Date />
                </Validators>
            </TextBox>
            <Validators>
                <i:Required />
            </Validators>
        </i:DropDownSuggest>
        <eclipse:LeftLabel runat="server" ID="lblStartsWith" Text="Item No. starts with" />
        <i:TextBoxEx runat="server" ID="tbStartsWith" />
        <br />
        Leave blank to see first 50 items
        <eclipse:LeftLabel runat="server" />
        <i:ButtonEx ID="btnGo" runat="server" Text="Go" Action="Submit" CausesValidation="true"
            Icon="Refresh" IsDefault="true" OnClick="btnGo_Click" />
        <i:ValidationSummary ID="ValidationSummary1" runat="server" />
    </eclipse:TwoColumnPanel>
    <asp:HyperLink runat="server" CssClass="noprint" ID="hlDataEntry" Text="Data Entry"
        NavigateUrl="~/MIS/MonthlyPhysical.aspx" EnableViewState="false" />
    <jquery:JPanel runat="server" IsValidationContainer="true">
        <i:ButtonEx ID="btnSaveProgress" runat="server" Text="Save" Icon="Refresh" OnClientClick="btnSaveProgress_Click"
            CausesValidation="true" Action="Submit" OnClick="btnSaveProgress_Click" ClientIDMode="Static" />
        <phpa:PhpaLinqDataSource ID="dsPhysicalReport" runat="server" OnSelecting="dsPhysicalReport_Selecting"
            ContextTypeName="Eclipse.PhpaLibrary.Database.MIS.MISDataContext" RenderLogVisible="false"
            TableName="FormatActivityDetails" EnableViewState="false">
        </phpa:PhpaLinqDataSource>
        <jquery:GridViewEx runat="server" ID="gv" AutoGenerateColumns="false" DataSourceID="dsPhysicalReport"
            DataKeyNames="ActivityId,FormatActivityDetailId" EnableViewState="true" ClientIDMode="Static"
            OnDataBound="gv_DataBound" ShowFooter="true" CellPadding="4" OnRowDataBound="gv_RowDataBound">
            <Columns>
                <jquery:SelectCheckBoxField AccessibleHeaderText="EditOnly" />
                <asp:TemplateField HeaderText="Item No.">
                    <ItemStyle Wrap="false" />
                    <ItemTemplate>
                        <a href="#" title="Click to edit activity">
                            <%# Eval("ItemNumber") %></a>
                    </ItemTemplate>
                </asp:TemplateField>
                <eclipse:MultiBoundField DataFields="Description" HeaderText="Description" HeaderToolTip="The activity whose progress is being reported">
                    <HeaderStyle Width="30em" />
                    <ItemStyle Width="30em" />
                </eclipse:MultiBoundField>
                <eclipse:MultiBoundField DataFields="AsPerBOQ,Unit" HeaderText="Quantity|As Per BOQ"
                    DataFormatString="{0:#,###.####} {1}" ToolTipFields="AsPerBOQ,Unit" ToolTipFormatString="{0:#.####} {1}"
                    HeaderToolTip="Target to accomplish at project completion">
                    <ItemStyle HorizontalAlign="Right" Wrap="false" Font-Bold="true" />
                </eclipse:MultiBoundField>
                <eclipse:MultiBoundField DataFields="PreviousMonth" HeaderText="Quantity|Executed till Previous Month"
                    DataFormatString="{0:#,###.####}" ToolTipFields="PreviousMonth" ToolTipFormatString="{0:#.####}"
                    HeaderToolTip="Work completed so far excluding this month">
                    <ItemStyle HorizontalAlign="Right" />
                </eclipse:MultiBoundField>
                <eclipse:MultiBoundField DataFields="ThisMonth" HeaderText="Quantity|This Month"
                    DataFormatString="{0:#,###.####}" ToolTipFields="ThisMonth" ToolTipFormatString="{0:#.####}"
                    HeaderToolTip="Work completed this month" AccessibleHeaderText="ReadOnly">
                    <ItemStyle HorizontalAlign="Right" />
                </eclipse:MultiBoundField>
                <asp:TemplateField HeaderText="Quantity|This Month" AccessibleHeaderText="EditOnly">
                    <ItemStyle HorizontalAlign="Right" />
                    <ItemTemplate>
                        <i:TextBoxEx ID="tbProgressData" runat="server" Text='<%# Eval("ThisMonth") %>' FriendlyName='<%# "This month" + Eval("ItemNumber", " for Activity {0}") %>'
                            Size="10">
                            <Validators>
                                <i:Value ValueType="Decimal" MaxLength="15" />
                            </Validators>
                        </i:TextBoxEx>
                    </ItemTemplate>
                </asp:TemplateField>
                <eclipse:MultiBoundField DataFields="TotalQuantity" HeaderText="Quantity|Total Quantity Executed"
                    ItemStyle-HorizontalAlign="Right" DataFormatString="{0:#,###.####}" ToolTipFields="TotalQuantity"
                    ToolTipFormatString="{0:#.###}" AccessibleHeaderText="ReadOnly" HeaderToolTip="Total work executed including this month" />
                <eclipse:MultiBoundField DataFields="BoQ30" HeaderText="Quantity|BoQ+30%" ItemStyle-HorizontalAlign="Right"
                    DataFormatString="{0:#,###.####}" AccessibleHeaderText="ReadOnly" HeaderToolTip="This is blank if Total Quantity Executed is less than (As per BOQ + 30%)" />
                <eclipse:MultiBoundField DataFields="Deviation" HeaderText="Quantity|Deviation" ItemStyle-HorizontalAlign="Right"
                    DataFormatString="{0:#,###.####;'';''}" AccessibleHeaderText="ReadOnly" HeaderToolTip="Total quantity executed over and above BOQ + 30%" />
                <eclipse:MultiBoundField DataFields="ReasonForDeviation" HeaderText="Quantity|Reason For Deviation"
                    AccessibleHeaderText="ReadOnly" />
                <asp:TemplateField HeaderText="Quantity|Reason For Deviation" AccessibleHeaderText="EditOnly">
                    <ItemTemplate>
                        <i:TextArea ID="taReasonForDeviation" runat="server" Text='<%# Eval("ReasonForDeviation") %>'>
                            <Validators>
                                <i:Value ValueType="String" />
                            </Validators>
                        </i:TextArea>
                    </ItemTemplate>
                </asp:TemplateField>
                <eclipse:MultiBoundField DataFields="Remarks" HeaderText="Remarks" AccessibleHeaderText="ReadOnly" />
                <asp:TemplateField HeaderText="Remarks" AccessibleHeaderText="EditOnly">
                    <ItemTemplate>
                        <i:TextArea ID="taRemarks" runat="server" Text='<%# Eval("Remarks") %>'>
                            <Validators>
                                <i:Value ValueType="String" />
                            </Validators>
                        </i:TextArea>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </jquery:GridViewEx>
        <i:ButtonEx runat="server" ID="btnSaveProgress1" Text="Save" OnClientClick="function(e) {$('#btnSaveProgress').click(); }"
            Icon="Refresh" />
        <i:ValidationSummary runat="server" />
    </jquery:JPanel>
    <jquery:StatusPanel ID="sp" Title="Status Message" runat="server" />
    <asp:HiddenField ID="hfDate" runat="server" />
    <asp:HiddenField ID="hfProgressformat" runat="server" />
    <jquery:Dialog ID="dlgActivity" runat="server" AutoOpen="false" ClientIDMode="Static"
        Title="Activity" Width="600" Position="RightTop">
        <Ajax Url="ActivityDetail.aspx" OnAjaxDialogClosing="function(event, ui) {
_$activityRow.addClass('ui-state-highlight');
}" />
        <Buttons>
            <jquery:RemoteSubmitButton IsDefault="true" RemoteButtonSelector="#btn" Text="Ok" />
            <jquery:CloseButton />
        </Buttons>
        <ContentTemplate>
            <span class="ui-state-highlight">The changes you make here will be saved but will not
                be immediately visible on the main page.</span>
        </ContentTemplate>
    </jquery:Dialog>
</asp:Content>
