<%@ Page Title="Activity List" Language="C#" MasterPageFile="~/MasterPage.master"
    CodeBehind="ActivityList.aspx.cs" Inherits="PhpaAll.MIS.ActivityList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function Delete_activity(key) {
            if (confirm('The current activity will be deleted. Click OK to confirm.')) {
                var $tr = $(this).gridViewEx('rows', key);
                CallPageMethod(
                        "DeleteActivity", { ActivityId: key[0] }, function (result) {
                            //on success.                           
                            $tr.attr('disabled', 'true')
                                .css('text-decoration', 'line-through');
                        },
                        function (xhr, status, e) {
                            // On failure.
                            alert(xhr.responseText);
                        }

                     );
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cph" runat="server">
    <div style="margin-top: 5mm">
        <eclipse:TwoColumnPanel ID="tcp" runat="server">
            <eclipse:LeftLabel runat="server" Text="Progress Format" />
            <phpa:PhpaLinqDataSource ID="dsReporte" runat="server" ContextTypeName="Eclipse.PhpaLibrary.Database.MIS.MISDataContext"
                TableName="ProgressFormats" RenderLogVisible="False" OrderBy="ProgressFormatName">
            </phpa:PhpaLinqDataSource>
            <i:DropDownListEx ID="ddlProgressformat" runat="server" DataSourceID="dsReporte"
                DataTextField="FormatDisplayName" DataValueField="ProgressFormatId" FriendlyName="Progress Format"
                QueryString="ProgressFormatId" DataOptionGroupField="Package.PackageName">
                <Items>
                    <eclipse:DropDownItem Text="(No formats available)" Persistent="WhenEmpty" />
                </Items>
                <Validators>
                    <i:Required />
                </Validators>
            </i:DropDownListEx>
            <i:ButtonEx ID="btnProgressFormat" runat="server" Action="Submit" Icon="Refresh"
                Text="Go" CausesValidation="true" />
        </eclipse:TwoColumnPanel>
<%--        <div style="display: none">
            <eclipse:AppliedFilters ID="af" runat="server" ContainerId="tcp" ClientIDMode="Static" />
        </div>--%>
        <i:ValidationSummary runat="server" />
        <i:ButtonEx ID="btnAddActivity" runat="server" Action="None" Text="New activity..."
            Icon="Refresh" OnClientClick="function(e){
$('#dlgActivity').ajaxDialog('option','data',{})
                    .ajaxDialog('load');
}" Visible="true" />
        <phpa:PhpaLinqDataSource ID="dsActivityList" runat="server" RenderLogVisible="false"
            ContextTypeName="Eclipse.PhpaLibrary.Database.MIS.MISDataContext" TableName="Activities"
            EnableDelete="true" AutoGenerateWhereClause="true" OrderBy="ActivityCode">
            <DeleteParameters>
                <asp:Parameter Name="ActivityId" Type="Int32" />
            </DeleteParameters>
            <WhereParameters>
                <asp:ControlParameter ControlID="ddlProgressformat" Name="ProgressFormatId" Type="Int32" />
            </WhereParameters>
        </phpa:PhpaLinqDataSource>
        <jquery:GridViewEx ID="gvActivityList" runat="server" DataSourceID="dsActivityList"
            AutoGenerateColumns="False" EmptyDataText="No activities found" DataKeyNames="ActivityId"
            ClientIDMode="Static" OnDataBound="gvActivityList_DataBound" OnRowDataBound="gvActivityList_RowDataBound" ShowFooter="true">
            <RowMenuItems>
                <jquery:RowMenuItem Text="Edit Activity..." OnClientClick="function(keys){                             
$('#dlgActivity').ajaxDialog('option','data',{ ActivityId: keys[0]})
                    .ajaxDialog('load');            
            }" />
                <jquery:RowMenuItem Text="Delete activity..." OnClientClick="Delete_activity" />
            </RowMenuItems>
            <Columns>
                <eclipse:MultiBoundField DataFields="DisplayActivityCode" HeaderText="Item No.">
                    <ItemStyle Wrap="false" />
                </eclipse:MultiBoundField>
                <eclipse:MultiBoundField DataFields="Description" HeaderText="Description of Work" />
                <eclipse:MultiBoundField DataFields="PhysicalTarget,uom" HeaderText="As Per BOQ"
                    ItemStyle-HorizontalAlign="Right" DataFormatString="{0:N4} {1}">
                    <ItemStyle HorizontalAlign="Right" Wrap="false" />
                </eclipse:MultiBoundField>
                <eclipse:MultiBoundField DataFields="BoqRate" HeaderText="BOQ Rate" ItemStyle-HorizontalAlign="Right"
                    DataFormatString="{0:N2}" />
                <eclipse:MultiBoundField DataFields="BoqRate,PhysicalTarget" HeaderText="Amount" ItemStyle-HorizontalAlign="Right"
                     AccessibleHeaderText="Amount" />
            </Columns>
        </jquery:GridViewEx>
        <jquery:Dialog ID="dlgActivity" runat="server" AutoOpen="false" ClientIDMode="Static"
            Title="Activity" Width="400" Position="RightTop">
            <Ajax Url="ActivityDetail.aspx" OnAjaxDialogClosing="function(event, ui) {
$('form:first').submit();}" />
            <Buttons>
                <jquery:RemoteSubmitButton IsDefault="true" RemoteButtonSelector="#btn" Text="Ok" />
                <jquery:CloseButton />
            </Buttons>
        </jquery:Dialog>
    </div>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cphNoForm" runat="server">
</asp:Content>
 