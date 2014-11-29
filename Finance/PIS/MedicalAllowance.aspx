<%@ Page Language="C#" CodeBehind="MedicalAllowance.aspx.cs" Inherits="Finance.PIS.MedicalAllowance"
    EnableViewState="true" Title="Medical Allowance" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <jquery:JQueryScriptManager runat="server" />
    <script type="text/javascript" src="TerminatedEmployee.js">
    </script>
</head>
<body>
    <form id="formMedicalAllowance" runat="server">
    <div>
        <phpa:PhpaLinqDataSource runat="server" ID="dsMedicalAllowance" RenderLogVisible="false"
            ContextTypeName="Eclipse.PhpaLibrary.Database.PIS.PISDataContext" TableName="MedicalRecords"
            Where="ServicePeriod.EmployeeId=@EmployeeId" OnSelecting="dsMedicalAllowance_Selecting"
            OnContextCreated="dsMedicalAllowance_ContextCreated" EnableDelete="true" EnableUpdate="true"
            OnDeleted="dsMedicalAllowance_Deleted">
            <WhereParameters>
                <asp:QueryStringParameter Name="EmployeeId" Type="Int32" QueryStringField="EmployeeId" />
            </WhereParameters>
            <DeleteParameters>
                <asp:Parameter Name="MedicalRecordId" Type="Int32" />
            </DeleteParameters>
        </phpa:PhpaLinqDataSource>
        <jquery:GridViewExInsert runat="server" ID="gvMedicalAllowance" DataSourceID="dsMedicalAllowance"
            DataKeyNames="MedicalRecordId" AutoGenerateColumns="false" Caption="Medical Allowance"
            EmptyDataText="No medical allowance information found">
            <RowMenuItems>
                <jquery:RowMenuItem Text="Edit Medical record" OnClientClick="function(keys){
            $('#_dlgAddEditMedicalDetails')
            .ajaxDialog('option','data',{MedicalRecordId:keys[0],Key:'EM'})
            .ajaxDialog('load');
            }" />
                <jquery:RowMenuPostBack Text="Delete" PostBackType="Delete" />
            </RowMenuItems>
            <Columns>
                <jquery:CommandFieldEx ShowEditButton="false" />
                <eclipse:MultiBoundField DataFields="ServicePeriod.DateRange" HeaderText="Service Period" />
                <eclipse:MultiBoundField DataFields="FamilyMember.FullName" HeaderText="Patient"
                    NullDisplayText="Self" />
                <eclipse:MultiBoundField DataFields="ReferalNo" HeaderText="Referal No" />
                <eclipse:MultiBoundField DataFields="CashMemoNo" HeaderText="Cash Memo No" />
                <eclipse:MultiBoundField DataFields="GrantedAmount" HeaderText="Amount|Granted" DataFormatString="{0:N2}"
                    ItemStyle-HorizontalAlign="Right" />
                <eclipse:MultiBoundField DataFields="AdvanceAdjusted" HeaderText="Amount|Advance Adjusted"
                    DataFormatString="{0:N2}" ItemStyle-HorizontalAlign="Right" />
                <eclipse:MultiBoundField DataFields="OfficeOrderNo" HeaderText="Order|No" />
                <eclipse:MultiBoundField DataFields="OrderDate" HeaderText="Order|Date" DataFormatString="{0:d}"
                    ItemStyle-HorizontalAlign="Right" />
                <eclipse:MultiBoundField DataFields="HospitalName" HeaderText="Hospital|Name" />
                <eclipse:MultiBoundField DataFields="HospitalAddress" HeaderText="Hospital|Address" />
                <eclipse:MultiBoundField DataFields="Country.CountryName" HeaderText="Hospital|Country" />
                <eclipse:MultiBoundField DataFields="Remarks" HeaderText="Remarks" />
            </Columns>
        </jquery:GridViewExInsert>
        <i:ValidationSummary runat="server" />
        <span id="spTerminated" class="ui-helper-hidden">
            <%= this.Request.QueryString["Terminated"] %></span>
        <i:ButtonEx ID="formMedicalAllowance_btnInsert" runat="server" Text="Add New Medical Details..."
            OnClientClick="function(e) {
$('#_dlgAddEditMedicalDetails')
.ajaxDialog('option','data',{Key:'NM'})
.ajaxDialog('load');}" />
        <jquery:Dialog runat="server" ID="_dlgAddEditMedicalDetails" ClientIDMode="Static"
            Title="Medical Details" AutoOpen="false" OnPreRender="_dlgAddEditMedicalDetails_PreRender">
            <Ajax Url="Share/MedicalDetails.aspx" OnAjaxDialogClosing="function(event,ui){
            window.location = 'EmployeeDetails.aspx?EmployeeId=' + ui.data;
            }" />
            <Buttons>
                <jquery:RemoteSubmitButton Text="Ok" IsDefault="true" RemoteButtonSelector="#btnMedicalDetails" />
                <jquery:CloseButton />
            </Buttons>
        </jquery:Dialog>
        <jquery:StatusPanel runat="server" ID="MedicalAllowance_sp">
            <Ajax UseDialog="false" />
        </jquery:StatusPanel>
    </div>
    </form>
</body>
</html>
