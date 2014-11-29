<%@ Page Language="C#" CodeBehind="Trainings.aspx.cs" Inherits="Finance.PIS.Trainings"
    Title="Trainings" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <jquery:JQueryScriptManager runat="server" />
    <script type="text/javascript" src="TerminatedEmployee.js">
    </script>
</head>
<body>
    <form id="formTrainings" runat="server">
    <div>
        <phpa:PhpaLinqDataSource runat="server" ID="dsTraining" ContextTypeName="Eclipse.PhpaLibrary.Database.PIS.PISDataContext"
            TableName="Trainings" RenderLogVisible="false" OrderBy="TrainingStartFrom descending"
            Where="ServicePeriod.EmployeeId=@EmployeeId" OnSelecting="dsTraining_Selecting"
            OnContextCreated="dsTraining_ContextCreated" EnableDelete="true" OnDeleted="dsTraining_Deleted">
            <WhereParameters>
                <asp:QueryStringParameter QueryStringField="EmployeeId" Name="EmployeeId" Type="Int32" />
            </WhereParameters>
            <DeleteParameters>
                <asp:Parameter Name="TrainingId" Type="Int32" />
            </DeleteParameters>
        </phpa:PhpaLinqDataSource>
        <jquery:GridViewExInsert runat="server" ID="gvTrainings" DataSourceID="dsTraining"
            DataKeyNames="TrainingId" AutoGenerateColumns="false" Caption="Recent Training Records"
            EmptyDataText="No Training information found">
            <RowMenuItems>
                <jquery:RowMenuItem Text="Edit Training" OnClientClick="function(keys){
    $('#_dlgAddEditTraining')
    .ajaxDialog('option','data',{TrainingId:keys[0],Key:'ET'})
    .ajaxDialog('load');
            }" />
                <jquery:RowMenuPostBack PostBackType="Delete" Text="Delete" />
            </RowMenuItems>
            <Columns>
                <jquery:CommandFieldEx ShowEditButton="false" />
                <eclipse:MultiBoundField DataFields="ServicePeriod.DateRange" HeaderText="Service Period" />
                <eclipse:MultiBoundField DataFields="TrainingType.TrainingDescription" HeaderText="Training|Type" />
                <eclipse:MultiBoundField DataFields="TrainingStartFrom,TrainingEndTo" HeaderText="Training|Period"
                    DataFormatString="{0:d}-{1:d}" ItemStyle-Wrap="false" />
                <eclipse:MultiBoundField DataFields="InstituteName" HeaderText="Institute|Name" />
                <eclipse:MultiBoundField DataFields="InstituteAddress" HeaderText="Institute|Address" />
                <eclipse:MultiBoundField DataFields="Country.CountryName" HeaderText="Institute|Country" />
                <eclipse:MultiBoundField DataFields="StudyField" HeaderText="Study Field" />
                <eclipse:MultiBoundField DataFields="Subject" HeaderText="Subject" />
                <eclipse:MultiBoundField DataFields="CourseLevel" HeaderText="Course Level" />
                <eclipse:MultiBoundField DataFields="FundingAgency" HeaderText="Funding Agency" />
                <eclipse:MultiBoundField DataFields="GovtApprovalNo" HeaderText="Govt Approval No" />
                <eclipse:MultiBoundField DataFields="Result" HeaderText="Result" />
                <eclipse:MultiBoundField DataFields="Remarks" HeaderText="Remarks" />
            </Columns>
        </jquery:GridViewExInsert>
        <span id="spTerminated" class="ui-helper-hidden">
            <%= this.Request.QueryString["Terminated"] %></span>
        <i:ValidationSummary runat="server" />
        <jquery:StatusPanel runat="server" ID="Trainings_sp">
            <Ajax UseDialog="false" />
        </jquery:StatusPanel>
        <i:ButtonEx ID="formTrainings_btnInsert" runat="server" Text="Add New Training..." OnClientClick="function(e) {
$('#_dlgAddEditTraining')
.ajaxDialog('option','data',{Key:'NT'})
.ajaxDialog('load');}" />
        <jquery:Dialog runat="server" ID="_dlgAddEditTraining" Title="Training Details" ClientIDMode="Static"
            AutoOpen="false" OnPreRender="_dlgAddEditTraining_PreRender" Width="350">
            <Ajax Url="Share/TrainingDetails.aspx" OnAjaxDialogClosing="function(event,ui){
        window.location='EmployeeDetails.aspx?EmployeeId=' + ui.data;
        }" />
            <Buttons>
                <jquery:RemoteSubmitButton RemoteButtonSelector="#btnTraining" IsDefault="true" Text="Ok" />
                <jquery:CloseButton />
            </Buttons>
        </jquery:Dialog>
    </div>
    </form>
</body>
</html>
