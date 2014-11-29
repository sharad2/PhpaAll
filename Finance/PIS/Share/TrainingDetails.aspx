<%@ Page Language="C#" CodeBehind="TrainingDetails.aspx.cs" Inherits="Finance.PIS.Share.TrainingDetails" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <jquery:JQueryScriptManager runat="server" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <phpa:PhpaLinqDataSource runat="server" ID="dsTrainingDetails" ContextTypeName="Eclipse.PhpaLibrary.Database.PIS.PISDataContext"
            TableName="Trainings" RenderLogVisible="false" OrderBy="TrainingStartFrom descending"
            Where="TrainingId==@TrainingId" OnSelecting="dsTrainingDetails_Selecting" EnableInsert="true"
            OnContextCreated="dsTrainingDetails_ContextCreated" OnInserting="dsTrainingDetails_Inserting"
            OnInserted="dsTrainingDetails_Inserted" EnableUpdate="true" OnUpdating="dsTrainingDetails_Updating"
            OnUpdated="dsTrainingDetails_Updated">
            <WhereParameters>
                <asp:QueryStringParameter QueryStringField="TrainingId" Name="TrainingId" Type="Int32" />
            </WhereParameters>
            <InsertParameters>
                <asp:Parameter Name="TrainingTypeId" Type="Int32" />
                <asp:Parameter Name="TrainingStartFrom" Type="DateTime" />
                <asp:Parameter Name="TrainingEndTo" Type="DateTime" />
                <asp:Parameter Name="ServicePeriodId" Type="Int32" />
                <asp:Parameter Name="InstituteName" Type="String" />
                <asp:Parameter Name="InstituteAddress" Type="String" />
                <asp:Parameter Name="CountryId" Type="Int32" />
                <asp:Parameter Name="StudyField" Type="String" />
                <asp:Parameter Name="Subject" Type="String" />
                <asp:Parameter Name="CourseLevel" Type="String" />
                <asp:Parameter Name="FundingAgency" Type="String" />
                <asp:Parameter Name="GovtApprovalNo" Type="String" />
                <asp:Parameter Name="Result" Type="String" />
                <asp:Parameter Name="Remarks" Type="String" />
                <asp:QueryStringParameter Name="EmployeeId" Type="Int32" QueryStringField="EmployeeId" />
            </InsertParameters>
            <UpdateParameters>
                <asp:Parameter Name="TrainingTypeId" Type="Int32" />
                <asp:Parameter Name="TrainingStartFrom" Type="DateTime" />
                <asp:Parameter Name="TrainingEndTo" Type="DateTime" />
                <asp:Parameter Name="ServicePeriodId" Type="Int32" />
                <asp:Parameter Name="InstituteName" Type="String" />
                <asp:Parameter Name="InstituteAddress" Type="String" />
                <asp:Parameter Name="CountryId" Type="Int32" />
                <asp:Parameter Name="StudyField" Type="String" />
                <asp:Parameter Name="Subject" Type="String" />
                <asp:Parameter Name="CourseLevel" Type="String" />
                <asp:Parameter Name="FundingAgency" Type="String" />
                <asp:Parameter Name="GovtApprovalNo" Type="String" />
                <asp:Parameter Name="Result" Type="String" />
                <asp:Parameter Name="Remarks" Type="String" />
            </UpdateParameters>
        </phpa:PhpaLinqDataSource>
        <asp:FormView runat="server" ID="fvTrainingDetails" DataSourceID="dsTrainingDetails"
            DataKeyNames="TrainingId,ServicePeriodId" OnItemInserted="fvTrainingDetails_ItemInserted"
            OnItemUpdated="fvTrainingDetails_ItemUpdated">
            <EditItemTemplate>
                <eclipse:TwoColumnPanel runat="server">
                    <eclipse:LeftLabel runat="server" Text="Service Period" />
                    <phpa:PhpaLinqDataSource runat="server" ID="dsServicePeriod" ContextTypeName="Eclipse.PhpaLibrary.Database.PIS.PISDataContext"
                        TableName="ServicePeriods" Where="EmployeeId == @EmployeeId" OrderBy="PeriodStartDate descending"
                        RenderLogVisible="false">
                        <WhereParameters>
                            <asp:QueryStringParameter Name="EmployeeId" QueryStringField="EmployeeId" Type="Int32" />
                        </WhereParameters>
                    </phpa:PhpaLinqDataSource>
                    <i:DropDownListEx runat="server" ID="ddlServicePeriod" DataSourceID="dsServicePeriod"
                        FriendlyName="Service Period" DataValueField="ServicePeriodId" DataTextField="DateRange"
                        Value='<%# Bind("ServicePeriodId") %>'>
                        <Validators>
                            <i:Required />
                        </Validators>
                    </i:DropDownListEx>
                    <eclipse:LeftLabel runat="server" Text="Training Type" />
                    <phpa:PhpaLinqDataSource runat="server" ID="dsTrainingType" ContextTypeName="Eclipse.PhpaLibrary.Database.PIS.PISDataContext"
                        Select="new (TrainingTypeId, TrainingDescription)" TableName="TrainingTypes"
                        RenderLogVisible="false">
                    </phpa:PhpaLinqDataSource>
                    <i:DropDownListEx runat="server" ID="ddlTrainingType" DataTextField="TrainingDescription"
                        DataValueField="TrainingTypeId" Value='<%# Bind("TrainingTypeId") %>'
                        DataSourceID="dsTrainingType" FriendlyName="Training Type" />
                    <eclipse:LeftLabel runat="server" Text="Training Period" />
                    <i:TextBoxEx runat="server" ID="tbTrainingStartFrom" Text='<%# Bind("TrainingStartFrom","{0:d}") %>'
                        FriendlyName="Training Period Start Date">
                        <Validators>
                            <i:Date />
                            <i:Required />
                        </Validators>
                    </i:TextBoxEx>
                    <i:TextBoxEx runat="server" ID="tbTrainingEndTo" Text='<%# Bind("TrainingEndTo","{0:d}") %>'
                        FriendlyName="Training Period End Date">
                        <Validators>
                            <i:Date DateType="ToDate" />
                            <i:Required />
                        </Validators>
                    </i:TextBoxEx>
                    <eclipse:LeftLabel runat="server" Text="Institute Name" />
                    <i:TextBoxEx runat="server" ID="tbInstituteName" Text='<%# Bind("InstituteName") %>'
                        Size="20" FriendlyName="Institute Name">
                        <Validators>
                            <i:Value ValueType="String" MaxLength="50" />
                        </Validators>
                    </i:TextBoxEx>
                    <eclipse:LeftLabel runat="server" Text="Institute Addres" />
                    <i:TextBoxEx runat="server" ID="tbInstituteAddress" Text='<%# Bind("InstituteAddress") %>'
                        Size="30" FriendlyName="Institute Address">
                        <Validators>
                            <i:Value ValueType="String" MaxLength="50" />
                        </Validators>
                    </i:TextBoxEx>
                    <eclipse:LeftLabel runat="server" Text="Institute Country" />
                    <phpa:PhpaLinqDataSource runat="server" ID="dsCountry" ContextTypeName="Eclipse.PhpaLibrary.Database.PIS.PISDataContext"
                        Select="new (CountryId, CountryName)" TableName="Countries" RenderLogVisible="false">
                    </phpa:PhpaLinqDataSource>
                    <i:DropDownListEx runat="server" ID="ddlCountry" DataTextField="CountryName" FriendlyName="Country"
                        DataValueField="CountryId" Value='<%# Bind("CountryId") %>' DataSourceID="dsCountry" />
                    <eclipse:LeftLabel runat="server" Text="Study Field" />
                    <i:TextBoxEx runat="server" ID="tbStudyField" Text='<%# Bind("StudyField") %>' Size="20"
                        FriendlyName="Study Field">
                        <Validators>
                            <i:Value ValueType="String" MaxLength="50" />
                        </Validators>
                    </i:TextBoxEx>
                    <eclipse:LeftLabel runat="server" Text="Subject" />
                    <i:TextBoxEx runat="server" ID="tbSubject" Text='<%# Bind("Subject") %>' Size="20"
                        FriendlyName="Subject">
                        <Validators>
                            <i:Value ValueType="String" MaxLength="50" />
                        </Validators>
                    </i:TextBoxEx>
                    <eclipse:LeftLabel runat="server" Text="Course Level" />
                    <i:TextBoxEx runat="server" ID="tbCourseLevel" Text='<%# Bind("CourseLevel") %>'
                        Size="20" FriendlyName="Course Level">
                        <Validators>
                            <i:Value ValueType="String" MaxLength="50" />
                        </Validators>
                    </i:TextBoxEx>
                    <eclipse:LeftLabel runat="server" Text="Funding Agency" />
                    <i:TextBoxEx runat="server" ID="tbFundingAgency" Text='<%# Bind("FundingAgency") %>'
                        Size="25" FriendlyName="Funding Agency">
                        <Validators>
                            <i:Value ValueType="String" MaxLength="50" />
                        </Validators>
                    </i:TextBoxEx>
                    <eclipse:LeftLabel runat="server" Text="Govt Approval No" />
                    <i:TextBoxEx runat="server" ID="tbGovtApprovalNo" Text='<%# Bind("GovtApprovalNo") %>'
                        Size="10" FriendlyName="Govt Approval No">
                        <Validators>
                            <i:Value ValueType="String" MaxLength="30" />
                        </Validators>
                    </i:TextBoxEx>
                    <eclipse:LeftLabel runat="server" Text="Result" />
                    <i:TextBoxEx runat="server" ID="tbResult" Text='<%# Bind("Result") %>' Size="10"
                        FriendlyName="Result">
                        <Validators>
                            <i:Value ValueType="String" MaxLength="30" />
                        </Validators>
                    </i:TextBoxEx>
                    <eclipse:LeftLabel runat="server" Text="Remarks" />
                    <i:TextBoxEx runat="server" ID="tbRemarks" Text='<%# Bind("Remarks") %>' Size="20"
                        FriendlyName="Remarks">
                        <Validators>
                            <i:Value ValueType="String" MaxLength="50" />
                        </Validators>
                    </i:TextBoxEx>
                </eclipse:TwoColumnPanel>
                <i:ValidationSummary ID="valTrainingDetails" runat="server" />
                <i:ButtonEx runat="server" ID="btnTraining" Action="Submit" Icon="Refresh" CausesValidation="true"
                    OnClick="btnTraining_Click" ClientIDMode="Static" />
            </EditItemTemplate>
        </asp:FormView>
        <jquery:StatusPanel runat="server" ID="TrainingDetails_sp">
            <Ajax UseDialog="false" />
        </jquery:StatusPanel>
    </div>
    </form>
</body>
</html>
