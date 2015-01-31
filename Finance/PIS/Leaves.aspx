<%@ Page Language="C#" CodeBehind="Leaves.aspx.cs" Inherits="PhpaAll.PIS.Leaves"
    EnableViewState="true" Title="Leaves" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <jquery:JQueryScriptManager runat="server" />
    <script type="text/javascript" src="TerminatedEmployee.js">
    </script>
</head>
<body>
    <form id="formLeaves" runat="server">
    <div>
        <phpa:PhpaLinqDataSource runat="server" ID="dsLeaveInfo" ContextTypeName="Eclipse.PhpaLibrary.Database.PIS.PISDataContext"
            TableName="LeaveRecords" RenderLogVisible="false" OrderBy="LeaveStartFrom descending"
            Where="ServicePeriod.EmployeeId=@EmployeeId" OnSelecting="dsLeaveInfo_Selecting"
            OnContextCreated="dsLeaveInfo_ContextCreated" EnableDelete="true" OnDeleted="dsLeaveInfo_Deleted">
            <WhereParameters>
                <asp:QueryStringParameter QueryStringField="EmployeeId" Name="EmployeeId" Type="Int32" />
            </WhereParameters>
            <DeleteParameters>
                <asp:Parameter Name="LeaveRecordId" Type="Int32" />
            </DeleteParameters>
        </phpa:PhpaLinqDataSource>
        <jquery:GridViewExInsert runat="server" ID="gvLeaveInfo" DataSourceID="dsLeaveInfo"
            AutoGenerateColumns="false" EmptyDataText="No Leave information found" Caption="Leave Information"
            DataKeyNames="LeaveRecordId" ShowFooter="true">
            <RowMenuItems>
                <jquery:RowMenuItem Text="Edit Leave" OnClientClick="function(keys){
             $('#_dlgAddEditLeave')
    .ajaxDialog('option','data',{LeaveRecordId:keys[0],Key:'EL'})
    .ajaxDialog('load');
            }" />
                <jquery:RowMenuPostBack PostBackType="Delete" Text="Delete" />
            </RowMenuItems>
            <Columns>
                <jquery:CommandFieldEx ShowEditButton="false" />
                <eclipse:SequenceField />
                <eclipse:MultiBoundField DataFields="ServicePeriod.DateRange" HeaderText="Service Period" />
                <eclipse:MultiBoundField DataFields="LeaveType.LeaveDescription" HeaderText="Leave|Type" />
                <eclipse:MultiBoundField DataFields="LeaveStartFrom,LeaveEndTo" HeaderText="Leave|Period"
                    DataFormatString="{0:d}-{1:d}" ItemStyle-Wrap="false" />
                <eclipse:MultiBoundField DataFields="LeavePurpose" HeaderText="Leave|Purpose" />
                <eclipse:MultiBoundField DataFields="NoOfLeaves" HeaderText="Business|Days" ItemStyle-HorizontalAlign="Right"
                    DataSummaryCalculation="ValueSummation" FooterStyle-HorizontalAlign="Right" />
                <eclipse:MultiBoundField DataFields="LeaveOrderNo" HeaderText="Order No" />
                <eclipse:MultiBoundField DataFields="LeaveOrderDate" HeaderText="Order Date" DataFormatString="{0:d}"
                    ItemStyle-HorizontalAlign="Right" />
                <eclipse:MultiBoundField DataFields="MedicalCertificateNo" HeaderText="Medical|Certificate No" />
                <eclipse:MultiBoundField DataFields="CertificateIssuingAuthority" HeaderText="Medical|Issuing Authority" />
                <eclipse:MultiBoundField DataFields="Remarks" HeaderText="Remarks" />
            </Columns>
        </jquery:GridViewExInsert>
        <span id="spTerminated" class="ui-helper-hidden">
            <%= this.Request.QueryString["Terminated"] %></span>
        <i:ButtonEx ID="formLeaves_btnInsert" runat="server" Text="Add New Leave..." OnClientClick="function(e) {
$('#_dlgAddEditLeave')
.ajaxDialog('option','data',{Key:'NL'})
.ajaxDialog('load');}" />
        <jquery:Dialog runat="server" ID="_dlgAddEditLeave" Title="Leave Details" ClientIDMode="Static"
            AutoOpen="false" OnPreRender="_dlgAddEditLeave_PreRender" Width="350">
            <Ajax Url="Share/LeaveDetails.aspx" OnAjaxDialogClosing="function(event,ui){
        window.location='EmployeeDetails.aspx?EmployeeId=' + ui.data;
        }" />
            <Buttons>
                <jquery:RemoteSubmitButton RemoteButtonSelector="#btnLeaveDetails" IsDefault="true"
                    Text="Ok" />
                <jquery:CloseButton />
            </Buttons>
        </jquery:Dialog>
        <i:ValidationSummary runat="server" />
        <jquery:StatusPanel runat="server" ID="Leaves_sp">
            <Ajax UseDialog="false" />
        </jquery:StatusPanel>
    </div>
    </form>
</body>
</html>
