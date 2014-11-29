<%@ Page Title="Monthly Financial Progress" Language="C#" MasterPageFile="~/MIS/NestedMIS.master"
    CodeBehind="MonthlyFinancial.aspx.cs" ValidateRequest="false" Inherits="Finance.MIS.FinancialProgress" %>

<%@ Register Src="../Controls/PrinterFriendlyButton.ascx" TagName="PrinterFriendlyButton"
    TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        var _$activityRow;
        // Automatically select the checkbox when user changes the text.
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


        function btnSave_ClientClick(e) {
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
    <jquery:StatusPanel ID="sp" runat="server" Title="Status Message" />
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
            <TextBox ID="tbProgressDate" runat="server" FriendlyName="Progress Month" >
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
        <i:ValidationSummary runat="server" />
    </eclipse:TwoColumnPanel>
    <asp:HyperLink runat="server" CssClass="noprint" ID="hlDataEntry" Text="Data Entry"
        NavigateUrl="~/MIS/MonthlyFinancial.aspx" EnableViewState="false" />
    <jquery:JPanel runat="server" IsValidationContainer="true">
        <i:ButtonEx ID="btnSave1" runat="server" Text="Save" Icon="Refresh" OnClientClick="btnSave_ClientClick"
            CausesValidation="true" Action="Submit" OnClick="btnSave_Click" ClientIDMode="Static" />
        <i:ValidationSummary ID="ValidationSummary1" runat="server" />
        <phpa:PhpaLinqDataSource ID="ds" runat="server" ContextTypeName="Eclipse.PhpaLibrary.Database.MIS.MISDataContext"
            RenderLogVisible="false" TableName="FormatActivityDetails" OnSelecting="ds_Selecting">
        </phpa:PhpaLinqDataSource>
        <jquery:GridViewEx ID="gv" runat="server" AutoGenerateColumns="false" DataSourceID="ds"
            DataKeyNames="ActivityId,FormatActivityDetailId" ClientIDMode="Static" OnDataBound="gv_DataBound"
            ShowFooter="true" EnableViewState="true" OnRowDataBound="gv_RowDataBound">
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
                <eclipse:MultiBoundField DataFields="FinancialTarget" HeaderText="Payment Made in (Rs/Nu)|As per Agreement"
                    DataSummaryCalculation="ValueSummation" FooterStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right"
                    DataFormatString="{0:#,###.00}">
                    <ItemStyle HorizontalAlign="Right" Wrap="false" Font-Bold="true" />
                </eclipse:MultiBoundField>
                <eclipse:MultiBoundField DataFields="FinancialPreMonth" HeaderText="Payment Made in (Rs/Nu)|Upto Previous Month"
                    ItemStyle-HorizontalAlign="Right" DataFormatString="{0:#,####.##}" DataSummaryCalculation="ValueSummation"
                    FooterStyle-HorizontalAlign="Right" />
                <eclipse:MultiBoundField DataFields="ProgressData" HeaderText="Payment Made in (Rs/Nu)|This Month"
                    DataFormatString="{0:#,###.00;(0.00);''}" ToolTipFields="ProgressData" ToolTipFormatString="{0:N2}"
                    HeaderToolTip="Work completed this month" AccessibleHeaderText="ReadOnly" DataSummaryCalculation="ValueSummation">
                    <ItemStyle HorizontalAlign="Right" />
                </eclipse:MultiBoundField>
                <asp:TemplateField HeaderText="Payment Made in (Rs/Nu)|This Month" AccessibleHeaderText="EditOnly"
                    ItemStyle-HorizontalAlign="Right">
                    <ItemTemplate>
                        <i:TextBoxEx ID="tbProgressData" runat="server" Text='<%# Eval("ProgressData", "{0:#.####}") %>'
                            FriendlyName='<%# "This month" + Eval("ItemNumber", " for Activity {0}") %>'
                            Size="10">
                            <Validators>
                                <i:Value ValueType="Decimal" MaxLength="15" />
                            </Validators>
                        </i:TextBoxEx>
                        <%# Eval("PhysicalProgressData", "{0:#,###.####}")%>
                        <%# Eval("PhysicalUnit")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <eclipse:MultiBoundField DataFields="TotalExpenditure" HeaderText="Payment Made in (Rs/Nu)|Total Expenditure"
                    ItemStyle-HorizontalAlign="Right" DataFormatString="{0:#,###.00;(0.00);''}" DataSummaryCalculation="ValueSummation"
                    FooterStyle-HorizontalAlign="Right" AccessibleHeaderText="ReadOnly" />
                <eclipse:MultiBoundField DataFields="BoqRate,ProvisionalRate,FinalRate" HeaderText="Rate BOQ/Provisonal/Final Rs/Nu"
                    ItemStyle-HorizontalAlign="Right" DataFormatString="{0:N2}/{1:N2}/{2:N2}" AccessibleHeaderText="EditOnly" />
                <%--                <eclipse:MultiBoundField DataFields="PhysicalProgressData" HeaderText="This month physical progress "
                    ItemStyle-HorizontalAlign="Right" DataFormatString="{0:#.##}" />
                --%>
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
