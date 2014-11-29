<%@ Page Title="Daily Progress Report" Language="C#" MasterPageFile="~/MIS/NestedMIS.master"
    EnableViewState="true" CodeBehind="DailyPhysical.aspx.cs" Inherits="Finance.MIS.DailyPhysical" %>

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
            if ($selectedRows.length == 0) { alert('Please select at least one row to Insert.'); return false; }
        }
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphSideNavigation" runat="server">
    <uc2:PrinterFriendlyButton ID="pfb" runat="server" />
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
                <eclipse:DropDownItem Text="No Daily Progress Format Defined" Persistent="WhenEmpty" />
            </Items>
            <Validators>
                <i:Required />
            </Validators>
        </i:DropDownListEx>
        <eclipse:LeftLabel runat="server" Text="Progress Date" />
        <i:TextBoxEx ID="tbProgressDate" runat="server" QueryString="ProgressDate" ClientIDMode="Static"
            Text="0">
            <Validators>
                <i:Date Max="0" />
                <i:Required />
            </Validators>
        </i:TextBoxEx>
        <i:ButtonEx ID="btnGo" runat="server" Text="Go" Action="Submit" CausesValidation="true"
            ClientIDMode="Static" Icon="Refresh" OnClick="btn_Click" IsDefault="true"/>
        <i:ValidationSummary runat="server" />
    </eclipse:TwoColumnPanel>
    <asp:HyperLink runat="server" CssClass="noprint" ID="hlDataEntry" Text="Data Entry"
        NavigateUrl="~/MIS/DailyPhysical.aspx" EnableViewState="false" />
    <jquery:JPanel ID="JPanel1" runat="server" IsValidationContainer="true">
        <i:ButtonEx runat="server" Text="Save" ID="btnSaveProgress" CausesValidation="true"
            OnClientClick="btnSaveProgress_Click"  Action="Submit" OnClick="btnSaveProgress_Click"
            ClientIDMode="Static" />
        <phpa:PhpaLinqDataSource ID="dsDPR" runat="server" OnSelecting="dsPhysicalReport_Selecting"
            ContextTypeName="Eclipse.PhpaLibrary.Database.MIS.MISDataContext" RenderLogVisible="false"
            TableName="FormatActivityDetails">
        </phpa:PhpaLinqDataSource>
        <jquery:GridViewEx runat="server" ID="gv" AutoGenerateColumns="false" DataSourceID="dsDPR"
            DataKeyNames="ActivityId,FormatActivityDetailId" EnableViewState="true" ClientIDMode="Static"
            OnDataBound="gv_DataBound" ShowFooter="true" eOnRowDataBound="gv_RowDataBound">
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
                <eclipse:MultiBoundField DataFields="AsPerBOQ,Unit" HeaderText="Total Length" DataFormatString="{0:#.####} {1}"
                    ToolTipFields="AsPerBOQ,Unit" ToolTipFormatString="{0:#.####} {1}" HeaderToolTip="Target to accomplish at project completion">
                    <ItemStyle HorizontalAlign="Right" Wrap="false" Font-Bold="true" />
                </eclipse:MultiBoundField>
                <eclipse:MultiBoundField DataFields="PreviousMonth" HeaderText="Upto Previous Month"
                    ItemStyle-HorizontalAlign="Right" DataFormatString="{0:#.####}" />
                <eclipse:MultiBoundField DataFields="UptoPreviousDay" HeaderText="During the Month|Upto Previous Day"
                    ItemStyle-HorizontalAlign="Right" DataFormatString="{0:#.####}" />
                <eclipse:MultiBoundField DataFields="Today" HeaderText="During the Month|During the day"
                    DataFormatString="{0:N2}" ToolTipFields="Today" ToolTipFormatString="{0:#.####}"
                    HeaderToolTip="Work completed today" AccessibleHeaderText="ReadOnly">
                    <ItemStyle HorizontalAlign="Right" />
                </eclipse:MultiBoundField>
                <asp:TemplateField HeaderText="During the Month|During the day" AccessibleHeaderText="EditOnly"
                    ItemStyle-HorizontalAlign="Right">
                    <ItemTemplate>
                        <i:TextBoxEx ID="tbProgressData" runat="server" MaxLength="9" Text='<%# Eval("Today", "{0:#.####}") %>'
                            FriendlyName='<%# "During the day" + Eval("ItemNumber", " for Activity {0}") %>'>
                            <Validators>
                                <i:Value ValueType="Decimal" Min="0" />
                            </Validators>
                        </i:TextBoxEx>
                    </ItemTemplate>
                </asp:TemplateField>
                <eclipse:MultiBoundField DataFields="ThisMonth" HeaderText="During the Month|During the month"
                    ItemStyle-HorizontalAlign="Right" DataFormatString="{0:#.####}" />
                <eclipse:MultiBoundField DataFields="TotalQuantity" HeaderText="Upto date progress"
                    ItemStyle-HorizontalAlign="Right" DataFormatString="{0:#.####}" />
                <eclipse:MultiBoundField DataFields="Balance" HeaderText="Balance" ItemStyle-HorizontalAlign="Right"
                    DataFormatString="{0:#.####}" />
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
       <%-- <i:ButtonEx runat="server" Text="Save" ID="btnSaveProgress" OnClientClick="function(e) {$('#btnSaveProgress').click(); }"
            Icon="Refresh" />--%>
        <i:ValidationSummary runat="server" />
    </jquery:JPanel>
    <jquery:StatusPanel ID="sp" Title="Status Message" runat="server" />
    <asp:HiddenField ID="hfDate" runat="server" />
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
