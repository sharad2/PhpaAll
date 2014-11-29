<%@ Page Language="C#" CodeBehind="LeaveDetails.aspx.cs" Inherits="Finance.PIS.Share.LeaveDetails" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <jquery:JQueryScriptManager runat="server" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <phpa:PhpaLinqDataSource runat="server" ID="dsLeaveDetails" ContextTypeName="Eclipse.PhpaLibrary.Database.PIS.PISDataContext"
            TableName="LeaveRecords" RenderLogVisible="false" OrderBy="LeaveStartFrom descending"
            Where="LeaveRecordId==@LeaveRecordId" OnSelecting="dsLeaveDetails_Selecting"
            OnContextCreated="dsLeaveDetails_ContextCreated" OnInserting="dsLeaveDetails_Inserting"
            OnInserted="dsLeaveDetails_Inserted" EnableInsert="true" EnableUpdate="true"
            OnUpdating="dsLeaveDetails_Updating" OnUpdated="dsLeaveDetails_Updated">
            <WhereParameters>
                <asp:QueryStringParameter QueryStringField="LeaveRecordId" Name="LeaveRecordId" Type="Int32" />
            </WhereParameters>
            <InsertParameters>
                <asp:Parameter Name="ServicePeriodId" Type="Int32" />
                <asp:Parameter Name="LeaveTypeId" Type="Int32" />
                <asp:Parameter Name="LeaveStartFrom" Type="DateTime" />
                <asp:Parameter Name="LeaveEndTo" Type="DateTime" />
                <asp:Parameter Name="LeavePurpose" Type="String" />
                <asp:Parameter Name="NoOfLeaves" Type="Int32" />
                <asp:Parameter Name="LeaveOrderDate" Type="DateTime" />
                <asp:Parameter Name="LeaveOrderNo" Type="String" />
                <asp:Parameter Name="MedicalCertificateNo" Type="String" />
                <asp:Parameter Name="CertificateIssuingAuthority" Type="String" />
                <asp:Parameter Name="Remarks" Type="String" />
            </InsertParameters>
            <UpdateParameters>
                <asp:Parameter Name="ServicePeriodId" Type="Int32" />
                <asp:Parameter Name="LeaveTypeId" Type="Int32" />
                <asp:Parameter Name="LeaveStartFrom" Type="DateTime" />
                <asp:Parameter Name="LeaveEndTo" Type="DateTime" />
                <asp:Parameter Name="LeavePurpose" Type="String" />
                <asp:Parameter Name="NoOfLeaves" Type="Int32" />
                <asp:Parameter Name="LeaveOrderDate" Type="DateTime" />
                <asp:Parameter Name="LeaveOrderNo" Type="String" />
                <asp:Parameter Name="MedicalCertificateNo" Type="String" />
                <asp:Parameter Name="CertificateIssuingAuthority" Type="String" />
                <asp:Parameter Name="Remarks" Type="String" />
            </UpdateParameters>
        </phpa:PhpaLinqDataSource>
        <asp:FormView runat="server" ID="fvLeaveDetails" DataSourceID="dsLeaveDetails" DataKeyNames="LeaveRecordId,ServicePeriodId"
            OnItemInserted="fvLeaveDetails_ItemInserted" OnItemUpdated="fvLeaveDetails_ItemUpdated">
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
                    <eclipse:LeftLabel runat="server" Text="Leave Type" />
                    <phpa:PhpaLinqDataSource runat="server" ID="dsLeaveType" RenderLogVisible="false"
                        ContextTypeName="Eclipse.PhpaLibrary.Database.PIS.PISDataContext" Select="new (LeaveTypeId, LeaveDescription)"
                        TableName="LeaveTypes">
                    </phpa:PhpaLinqDataSource>
                    <i:DropDownListEx runat="server" ID="ddlLeaveType" DataTextField="LeaveDescription"
                        FriendlyName="Leave Type" DataValueField="LeaveTypeId" Value='<%# Bind("LeaveTypeId") %>'
                        DataSourceID="dsLeaveType" />
                    <eclipse:LeftLabel runat="server" Text="Leave Period" />
                    <i:TextBoxEx runat="server" ID="tbLeaveStartFrom" Text='<%# Bind("LeaveStartFrom","{0:d}") %>'
                        FriendlyName="Leave Start From">
                        <Validators>
                            <i:Date />
                            <i:Required />
                            <i:Custom ID="custValLeaveStartFrom" OnServerValidate="custValLeavePeriod_ServerValidate" />
                        </Validators>
                    </i:TextBoxEx>
                    <i:TextBoxEx runat="server" ID="tbLeaveEndTo" Text='<%# Bind("LeaveEndTo","{0:d}") %>'
                        FriendlyName="Leave End To">
                        <Validators>
                            <i:Date DateType="ToDate" />
                            <i:Required />
                            <i:Custom ID="custValLeaveEndTo" OnServerValidate="custValLeavePeriod_ServerValidate" />
                        </Validators>
                    </i:TextBoxEx>
                    <eclipse:LeftLabel runat="server" Text="Leave Purpose" />
                    <i:TextBoxEx runat="server" ID="tbLeavePurpose" Text='<%# Bind("LeavePurpose") %>'
                        Size="20" FriendlyName="Leave Purpose">
                        <Validators>
                            <i:Value ValueType="String" MaxLength="50" />
                        </Validators>
                    </i:TextBoxEx>
                    <eclipse:LeftLabel runat="server" Text="Business Days" />
                    <i:TextBoxEx runat="server" ID="tbNoOfLeaves" Text='<%# Bind("NoOfLeaves") %>' FriendlyName="Business Days">
                        <Validators>
                            <i:Value ValueType="Integer" MaxLength="4" />
                        </Validators>
                    </i:TextBoxEx>
                    <eclipse:LeftLabel runat="server" Text="Leave Order" />
                    <i:TextBoxEx runat="server" ID="tbLeaveOrderNo" Text='<%# Bind("LeaveOrderNo") %>'
                        Size="10" FriendlyName="Leave Order No">
                        <Validators>
                            <i:Value ValueType="String" MaxLength="30" />
                        </Validators>
                    </i:TextBoxEx>
                    <eclipse:LeftLabel runat="server" Text="Leave Order Date" />
                    <i:TextBoxEx runat="server" ID="tbLeaveOrderDate" Text='<%# Bind("LeaveOrderDate","{0:d}") %>'
                        FriendlyName="Leave Order Date">
                        <Validators>
                            <i:Date />
                        </Validators>
                    </i:TextBoxEx>
                    <eclipse:LeftLabel runat="server" Text="Medical Certiface No" />
                    <i:TextBoxEx runat="server" ID="tbMedicalCertificateNo" Text='<%# Bind("MedicalCertificateNo") %>'
                        Size="10" FriendlyName="Medical Certificate No">
                        <Validators>
                            <i:Value ValueType="String" MaxLength="30" />
                        </Validators>
                    </i:TextBoxEx>
                    <eclipse:LeftLabel runat="server" Text="Certificate Issuing Authority" />
                    <i:TextBoxEx runat="server" ID="tbCertificateIssuingAuthority" Text='<%# Bind("CertificateIssuingAuthority") %>'
                        Size="20" FriendlyName="Certificate Issuing Authority">
                        <Validators>
                            <i:Value ValueType="String" MaxLength="50" />
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
                <i:ButtonEx runat="server" ID="btnLeaveDetails" Action="Submit" Icon="Refresh" CausesValidation="true"
                    OnClick="btnLeaveDetails_Click" ClientIDMode="Static" />
                <i:ValidationSummary ID="valLeaveDetails" runat="server" />
            </EditItemTemplate>
        </asp:FormView>
        <jquery:StatusPanel runat="server" ID="LeaveDetails_sp">
            <Ajax UseDialog="false" />
        </jquery:StatusPanel>
    </div>
    </form>
</body>
</html>
