<%@ Page Language="C#" CodeBehind="Grants.aspx.cs" Inherits="Finance.PIS.Grants"
    Title="Grants" MaintainScrollPositionOnPostback="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <jquery:JQueryScriptManager runat="server" />
    <script type="text/javascript" src="TerminatedEmployee.js">
    </script>
</head>
<body>
    <form id="formGrants" runat="server">
    <div>
        <phpa:PhpaLinqDataSource runat="server" ID="dsGrants" ContextTypeName="Eclipse.PhpaLibrary.Database.PIS.PISDataContext"
            TableName="EmployeeGrants" RenderLogVisible="false" Where="EmployeeId=@EmployeeId"
            OrderBy="GrantReceivedDate descending" OnSelecting="dsGrants_Selecting" EnableUpdate="true"
            EnableDelete="true" OnDeleted="dsGrants_Deleted">
            <WhereParameters>
                <asp:QueryStringParameter Name="EmployeeId" Type="Int32" QueryStringField="EmployeeId" />
            </WhereParameters>
            <DeleteParameters>
                <asp:Parameter Type="Int32" Name="EmployeeGrantId" />
            </DeleteParameters>
        </phpa:PhpaLinqDataSource>
        <jquery:GridViewEx runat="server" ID="gvGrants" DataSourceID="dsGrants" AutoGenerateColumns="false"
            Caption="Grants" DataKeyNames="EmployeeGrantId" EmptyDataText="No award or penalty information found"
            EnableViewState="true">
            <RowMenuItems>
                <jquery:RowMenuItem Text="Edit Grant" OnClientClick="function(keys){
    $('#_dlgAddEditGrantDetails')
    .ajaxDialog('option','data',{EmployeeGrantId:keys[0],Key:'EG'})
    .ajaxDialog('load');
            }" />
                <jquery:RowMenuPostBack PostBackType="Delete" Text="Delete" />
            </RowMenuItems>
            <Columns>
                <jquery:CommandFieldEx ShowEditButton="false" />
                <eclipse:MultiBoundField DataFields="GrantType" HeaderText="Grant|Type" />
                <eclipse:MultiBoundField DataFields="GrantName" HeaderText="Grant|Name" />
                <eclipse:MultiBoundField DataFields="GrantReceivedDate" HeaderText="Grant|Received Date"
                    DataFormatString="{0:d}" ItemStyle-HorizontalAlign="Right" />
                <eclipse:MultiBoundField DataFields="GrantingAuthority" HeaderText="Granting Authority" />
                <eclipse:MultiBoundField DataFields="MeritoriousService" HeaderText="Meritorious Service" />
                <eclipse:MultiBoundField DataFields="PenalityImposed" HeaderText="Penalty Imposed" />
                <eclipse:MultiBoundField DataFields="EnquiryDetails" HeaderText="Enquiry Details" />
                <eclipse:MultiBoundField DataFields="Remarks" HeaderText="Remarks" />
            </Columns>
        </jquery:GridViewEx>
        <span id="spTerminated" class="ui-helper-hidden">
            <%= this.Request.QueryString["Terminated"] %></span>
        <i:ButtonEx runat="server" ID="formGrants_btnInsert" Text="Add New Grant..." OnClientClick="function(e){
        $('#_dlgAddEditGrantDetails')
        .ajaxDialog('option','data',{Key:'NG'})
        .ajaxDialog('load');
        }" />
        <jquery:Dialog ID="_dlgAddEditGrantDetails" runat="server" AutoOpen="false" OnPreRender="_dlgAddEditGrantDetails_PreRender"
            ClientIDMode="Static" Title="Grant Details">
            <Ajax Url="Share/GrantDetails.aspx" OnAjaxDialogClosing="function(event,ui){
        window.location='EmployeeDetails.aspx?EmployeeId=' + ui.data;
        }" />
            <Buttons>
                <jquery:RemoteSubmitButton Text="Ok" RemoteButtonSelector="#btnGrantDetails" IsDefault="true" />
                <jquery:CloseButton />
            </Buttons>
        </jquery:Dialog>
        <i:ValidationSummary runat="server" />
        <jquery:StatusPanel runat="server" ID="Grants_sp">
            <Ajax UseDialog="false" />
        </jquery:StatusPanel>
    </div>
    </form>
</body>
</html>
