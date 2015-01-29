<%@ Page Title="Monthly Construction Schedule" Language="C#" MasterPageFile="~/MIS/NestedMIS.master"
    AutoEventWireup="true" ValidateRequest="false" CodeBehind="MonthlyConstruction.aspx.cs"
    Inherits="PhpaAll.MIS.MonthlyConstructionProgress" %>

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

        function btnSave_Click(e) {
            var $gv = $('#gv');
            var $selectedRows = $gv.gridViewEx('selectedRows');
            if ($selectedRows.length == 0) { alert('Please select at least one row to Insert.'); return false; }
        }
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphSideNavigation" runat="server">
    <uc2:PrinterFriendlyButton ID="pfb" runat="server" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cph" runat="server">
    <jquery:StatusPanel ID="sp" Title="Status Message" runat="server" />
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
        <eclipse:LeftLabel runat="server" Text="Progress Month" />
        <i:DropDownSuggest runat="server" ID="ddlProgressDate" FriendlyName="Progress Month"
            QueryString="ProgressDate">
            <Cascadable CascadeParentId="ddlProgressformat" WebMethod="GetProgressDate" InitializeAtStartup="true" />
            <Items>
                <eclipse:DropDownItem Text="(Recent Months)" Persistent="Always" />
            </Items>
            <TextBox ID="tbProgressDate" runat="server" FriendlyName="Enter Month">
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
            ClientIDMode="Static" Icon="Refresh" OnClick="btnGo_Click" IsDefault="true" />
        <i:ValidationSummary runat="server" />
    </eclipse:TwoColumnPanel>
    <asp:HyperLink runat="server" ID="hlDataEntry" CssClass="noprint" Text="Data Entry"
        NavigateUrl="~/MIS/MonthlyConstruction.aspx" EnableViewState="false" />
    <jquery:JPanel runat="server" IsValidationContainer="true">
        <i:ButtonEx ID="btnSave1" runat="server" Text="Save" Icon="Refresh" CausesValidation="true"
            OnClientClick="btnSave_Click" OnClick="btnSave_Click" Action="Submit" ClientIDMode="Static" />
        <phpa:PhpaLinqDataSource ID="ds" runat="server" ContextTypeName="Eclipse.PhpaLibrary.Database.MIS.MISDataContext"
            RenderLogVisible="false" TableName="FormatActivityDetails" OnSelecting="ds_Selecting">
        </phpa:PhpaLinqDataSource>
        <jquery:GridViewEx ID="gv" runat="server" AutoGenerateColumns="false" DataSourceID="ds"
            DataKeyNames="ActivityId,FormatActivityDetailId" ClientIDMode="Static" OnDataBound="gv_DataBound"
            ShowFooter="true" EnableViewState="true" OnRowDataBound="gv_RowDataBound">
            <Columns>
                <eclipse:SequenceField />
                <jquery:SelectCheckBoxField AccessibleHeaderText="EditOnly" />
                <asp:TemplateField HeaderText="Item No.">
                    <ItemStyle Wrap="false" />
                    <ItemTemplate>
                        <a href="#" title="Click to edit activity">
                            <%# Eval("ItemNumber") %></a>
                    </ItemTemplate>
                </asp:TemplateField>
                <eclipse:MultiBoundField DataFields="ItemNumber,Description" HeaderText="Description"
                    DataFormatString="{0}&nbsp;{1}" HeaderToolTip="The activity whose progress is being reported">
                    <HeaderStyle Width="30em" />
                    <ItemStyle Width="30em" />
                </eclipse:MultiBoundField>
                <eclipse:MultiBoundField DataFields="PhysicalTarget,UOM" HeaderText="Quantity|Total Quantity"
                    DataFormatString="{0:#,###.####} {1}" HeaderToolTip="Target to accomplish at project completion">
                    <ItemStyle HorizontalAlign="Right" Wrap="false" Font-Bold="true" />
                </eclipse:MultiBoundField>
                <eclipse:MultiBoundField DataFields="PreMonth" HeaderText="Quantity|Executed Till Previous Month"
                    ItemStyle-HorizontalAlign="Right" DataFormatString="{0:#.####}" />
                <eclipse:MultiBoundField DataFields="ThisMonth" HeaderText="Quantity|Progress During This Month"
                    DataFormatString="{0:#,###.####}" ToolTipFields="ThisMonth" ToolTipFormatString="{0:#.####}"
                    HeaderToolTip="Work completed this month" AccessibleHeaderText="ReadOnly">
                    <ItemStyle HorizontalAlign="Right" />
                </eclipse:MultiBoundField>
                <asp:TemplateField HeaderText="Quantity|Progress During This Month" AccessibleHeaderText="EditOnly"
                    ItemStyle-HorizontalAlign="Right">
                    <ItemTemplate>
                        <i:TextBoxEx ID="tbProgressData" runat="server" Text='<%# Eval("ThisMonth") %>' FriendlyName='<%# "This month" + Eval("ItemNumber", " for Activity {0}") %>'>
                            <Validators>
                                <i:Value ValueType="Decimal" />
                            </Validators>
                        </i:TextBoxEx>
                    </ItemTemplate>
                </asp:TemplateField>
                <eclipse:MultiBoundField DataFields="TotalExpenditure" HeaderText="Quantity|Total quntity executed"
                    ItemStyle-HorizontalAlign="Right" DataFormatString="{0:#,###.####}" />
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
        <i:ButtonEx ID="btnSave" runat="server" Text="Save" Icon="Refresh" OnClientClick="function(e) {$('#btnSave1').click(); }" />
        <i:ValidationSummary runat="server" />
    </jquery:JPanel>
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
<asp:Content ID="Content5" ContentPlaceHolderID="cphNoForm" runat="server">
</asp:Content>
