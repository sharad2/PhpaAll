<%@ Page Language="C#" CodeBehind="Qualifications.aspx.cs" Inherits="Finance.PIS.Qualifications"
    Title="Qualifications" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <jquery:JQueryScriptManager runat="server" />
    <script type="text/javascript" src="TerminatedEmployee.js">
    </script>
</head>
<body>
    <form id="formQualifications" runat="server">
    <div>
        <phpa:PhpaLinqDataSource runat="server" ID="dsQualifications" ContextTypeName="Eclipse.PhpaLibrary.Database.PIS.PISDataContext"
            TableName="Qualifications" RenderLogVisible="false" Where="EmployeeId=@EmployeeId"
            OrderBy="CompletionYear descending" OnContextCreated="dsQualifications_ContextCreated"
            OnSelecting="dsQualifications_Selecting" OnDeleted="dsQualifications_Deleted"
            EnableDelete="true">
            <WhereParameters>
                <asp:QueryStringParameter Name="EmployeeId" Type="Int32" QueryStringField="EmployeeId" />
            </WhereParameters>
            <DeleteParameters>
                <asp:Parameter Name="QualificationId" Type="Int32" />
            </DeleteParameters>
        </phpa:PhpaLinqDataSource>
        <jquery:GridViewExInsert runat="server" ID="gvQualifications" DataSourceID="dsQualifications"
            DataKeyNames="QualificationId" AutoGenerateColumns="false" Caption="Qualifications"
            EmptyDataText="No qualification information found">
            <RowMenuItems>
                <jquery:RowMenuItem Text="Edit Qualification" OnClientClick="function(keys){
    $('#_dlgAddEditQualification')
    .ajaxDialog('option','data',{QualificationId:keys[0],Key:'EQ'})
    .ajaxDialog('load');
            }" />
                <jquery:RowMenuPostBack PostBackType="Delete" Text="Delete" />
            </RowMenuItems>
            <Columns>
                <jquery:CommandFieldEx ShowEditButton="false" />
                <eclipse:MultiBoundField DataFields="QualificationType" HeaderText="Qualification|Type" />
                <eclipse:MultiBoundField DataFields="QualificationDivision.DivisionName" HeaderText="Qualification|Division" />
                <eclipse:MultiBoundField DataFields="Result" HeaderText="Result" />
                <eclipse:MultiBoundField DataFields="CompletionYear" HeaderText="Completion Year"
                    ItemStyle-HorizontalAlign="Right" />
                <eclipse:MultiBoundField DataFields="Institute" HeaderText="Institute|Name" />
                <eclipse:MultiBoundField DataFields="InstituteAddress" HeaderText="Institute|Address" />
                <eclipse:MultiBoundField DataFields="Country.CountryName" HeaderText="Institute|Country" />
                <eclipse:MultiBoundField DataFields="StudyField" HeaderText="Field Of Study" />
                <eclipse:MultiBoundField DataFields="Subject" HeaderText="Subject" />
                <eclipse:MultiBoundField DataFields="Remarks" HeaderText="Remarks" />
            </Columns>
        </jquery:GridViewExInsert>
        <span id="spTerminated" class="ui-helper-hidden">
            <%= this.Request.QueryString["Terminated"] %></span>
        <i:ValidationSummary runat="server" />
        <jquery:StatusPanel ID="Qualifications_sp" runat="server">
            <Ajax UseDialog="false" />
        </jquery:StatusPanel>
        <i:ButtonEx ID="formQualifications_btnInsert" runat="server" Text="Add New Qualification..."
            OnClientClick="function(e) {
$('#_dlgAddEditQualification')
.ajaxDialog('option','data',{Key:'NQ'})
.ajaxDialog('load');}" />
        <jquery:Dialog runat="server" ID="_dlgAddEditQualification" Title="Qualification"
            ClientIDMode="Static" AutoOpen="false" OnPreRender="_dlgAddEditQualification_PreRender">
            <Ajax Url="Share/QualificationDetails.aspx" OnAjaxDialogClosing="function(event,ui){
        window.location='EmployeeDetails.aspx?EmployeeId=' + ui.data;
        }" />
            <Buttons>
                <jquery:RemoteSubmitButton RemoteButtonSelector="#btnQualification" IsDefault="true"
                    Text="Ok" />
                <jquery:CloseButton />
            </Buttons>
        </jquery:Dialog>
    </div>
    </form>
</body>
</html>
