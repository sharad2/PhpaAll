<%@ Page Title="Progress Format Manager" Language="C#" MasterPageFile="~/MasterPage.master"
    CodeBehind="ProgressFormatManager.aspx.cs" Inherits="PhpaAll.MIS.ProgressFormatManager" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function Delete_format(key) {
            var $tr = $(this).gridViewEx('rows', key);
            CallPageMethod(
                        "DeleteFormat", { ProgressFormatId: key[0] }, function (result) {
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
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphSideNavigation" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cph" runat="server">
    <i:ButtonEx ID="btnAddNew" runat="server" Text="New format..." OnClientClick="function(e){                                         
 $('#dlgformat').ajaxDialog('option','data', {})
                .ajaxDialog('load');               
 }" />
    <jquery:Dialog ID="dlgformat" runat="server" Title="Format Detail" ClientIDMode="Static"
        AutoOpen="false" Width="500" Position="RightTop">
        <Ajax Url="ProgressFormat.aspx" OnAjaxDialogClosing="function(event, ui) {
$('form:first').submit();}" />
        <Buttons>
            <jquery:RemoteSubmitButton RemoteButtonSelector="#btnFormat" Text="Ok" IsDefault="true" />
            <jquery:CloseButton />
        </Buttons>
    </jquery:Dialog>
    <phpa:PhpaLinqDataSource ID="dsFormats" runat="server" ContextTypeName="Eclipse.PhpaLibrary.Database.MIS.MISDataContext"
        OrderBy="ProgressFormatName" TableName="ProgressFormats" OnSelecting="dsFormats_Selecting"
        RenderLogVisible="false">
    </phpa:PhpaLinqDataSource>
    <jquery:GridViewEx ID="gv" runat="server" DataSourceID="dsFormats" DataKeyNames="ProgressFormatId"
        AutoGenerateColumns="false" DefaultSortExpression="PackageName;$;" ShowExpandCollapseButtons="false"
        ClientIDMode="Static">
        <RowMenuItems>
            <jquery:RowMenuNavigate Text="Activity List" NavigateUrl="ActivityList.aspx?ProgressFormatId={0}" />
            <jquery:RowMenuItem Text="Edit format..." OnClientClick="function(keys){
$('#dlgformat').ajaxDialog('option','data',{ ProgressFormatId: keys[0]})
               .ajaxDialog('load');                        
                        }" />
            <jquery:RowMenuItem Text="Delete format" OnClientClick="Delete_format" />
        </RowMenuItems>
        <Columns>
            <eclipse:MultiBoundField DataFields="ProgressFormatName" HeaderText="Format|Physical" />
            <eclipse:MultiBoundField DataFields="FinancialFormatName" HeaderText="Format|Financial" />
            <eclipse:MultiBoundField DataFields="ProgressFormatTypeAsEnum" HeaderText="Type" />
            <eclipse:MultiBoundField DataFields="PackageName,PackageDescription" HeaderText="Package"
                DataFormatString="{0} - {1}" SortExpression="PackageName" />
            <eclipse:MultiBoundField DataFields="FormatDisplayName " HeaderText="Description" />
            <eclipse:MultiBoundField DataFields="FormatCategoryDescription" HeaderText="Category" />
            <eclipse:HyperLinkFieldEx HeaderText="# Activities" DataTextField="Activitycounts"
                DataNavigateUrlFields="ProgressFormatId" DataNavigateUrlFormatString="ActivityList.aspx?ProgressFormatId={0}"
                ItemStyle-HorizontalAlign="Right" />
            <asp:TemplateField>
                <HeaderTemplate>
                    <span title="How much data has been entered for this format">Data Entry</span>
                </HeaderTemplate>
                <ItemTemplate>
                    <%# Eval("DataActivityCount", "{0:# 'activities during';#;#}")%>
                    <eclipse:MultiValueLabel runat="server" DataFields="CountProgressDate,MinProgressDate,MaxProgressDate"
                        DataTwoValueFormatString="{1:d} to {2:d}" DataZeroValueFormatString="" />
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </jquery:GridViewEx>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cphNoForm" runat="server">
</asp:Content>
