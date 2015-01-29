<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MedicalDetails.aspx.cs"
    Inherits="PhpaAll.PIS.Share.MedicalDetails" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <jquery:JQueryScriptManager runat="server" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <phpa:PhpaLinqDataSource runat="server" ID="dsMedicalDetails" RenderLogVisible="false"
            ContextTypeName="Eclipse.PhpaLibrary.Database.PIS.PISDataContext" TableName="MedicalRecords"
            Where="MedicalRecordId==@MedicalRecordId" OnSelecting="dsMedicalDetails_Selecting"
            OnContextCreated="dsMedicalDetails_ContextCreated" OnInserted="dsMedicalDetails_Inserted"
            EnableInsert="true" EnableUpdate="true" OnUpdated="dsMedicalDetails_Updated">
            <WhereParameters>
                <asp:QueryStringParameter QueryStringField="MedicalRecordId" Name="MedicalRecordId"
                    Type="Int32" />
            </WhereParameters>
            <InsertParameters>
                <asp:Parameter Name="ServicePeriodId" Type="Int32" />
                <asp:Parameter Name="FamilyMemberId" Type="Int32" />
                <asp:Parameter Name="ReferalNo" Type="String" />
                <asp:Parameter Name="GrantedAmount" Type="Decimal" />
                <asp:Parameter Name="AdvanceAdjusted" Type="Decimal" />
                <asp:Parameter Name="OfficeOrderNo" Type="String" />
                <asp:Parameter Name="CashMemoNo" Type="String" />
                <asp:Parameter Name="OrderDate" Type="DateTime" />
                <asp:Parameter Name="HospitalName" Type="String" />
                <asp:Parameter Name="HospitalAddress" Type="String" />
                <asp:Parameter Name="CountryId" Type="Int32" />
                <asp:Parameter Name="Remarks" Type="String" />
            </InsertParameters>
            <UpdateParameters>
                <asp:Parameter Name="ServicePeriodId" Type="Int32" />
                <asp:Parameter Name="FamilyMemberId" Type="Int32" />
                <asp:Parameter Name="ReferalNo" Type="String" />
                <asp:Parameter Name="GrantedAmount" Type="Decimal" />
                <asp:Parameter Name="AdvanceAdjusted" Type="Decimal" />
                <asp:Parameter Name="OfficeOrderNo" Type="String" />
                <asp:Parameter Name="CashMemoNo" Type="String" />
                <asp:Parameter Name="OrderDate" Type="DateTime" />
                <asp:Parameter Name="HospitalName" Type="String" />
                <asp:Parameter Name="HospitalAddress" Type="String" />
                <asp:Parameter Name="CountryId" Type="Int32" />
                <asp:Parameter Name="Remarks" Type="String" />
            </UpdateParameters>
        </phpa:PhpaLinqDataSource>
        <asp:FormView runat="server" ID="fvMedicalDetails" DataSourceID="dsMedicalDetails"
            DataKeyNames="MedicalRecordId,ServicePeriodId" OnItemInserted="fvMedicalDetails_ItemInserted"
            OnItemUpdated="fvMedicalDetails_ItemUpdated">
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
                    <eclipse:LeftLabel runat="server" Text="Patient" />
                    <phpa:PhpaLinqDataSource runat="server" ID="dsFamilyMembers" ContextTypeName="Eclipse.PhpaLibrary.Database.PIS.PISDataContext"
                        Select="new (FamilyMemberId, FullName)" TableName="FamilyMembers" RenderLogVisible="false"
                        Where="EmployeeId=@EmployeeId">
                        <WhereParameters>
                            <asp:QueryStringParameter Name="EmployeeId" QueryStringField="EmployeeId" Type="Int32" />
                        </WhereParameters>
                    </phpa:PhpaLinqDataSource>
                    <i:DropDownListEx runat="server" ID="ddlFamilyMembers" DataTextField="FullName" DataValueField="FamilyMemberId"
                        Value='<%# Bind("FamilyMemberId") %>' DataSourceID="dsFamilyMembers"
                        FriendlyName="Patient">
                        <Items>
                            <eclipse:DropDownItem Text="(Self)" Value="" Persistent="Always" />
                        </Items>
                    </i:DropDownListEx>
                    <eclipse:LeftLabel runat="server" Text="Referal No" />
                    <i:TextBoxEx runat="server" ID="tbReferalNo" Text='<%# Bind("ReferalNo") %>' Size="10"
                        FriendlyName="Referal No">
                        <Validators>
                            <i:Required />
                            <i:Value ValueType="String" MaxLength="30" />
                        </Validators>
                    </i:TextBoxEx>
                    <eclipse:LeftLabel runat="server" Text="Cash Memo No" />
                    <i:TextBoxEx runat="server" ID="tbCashMemoNo" Text='<%# Bind("CashMemoNo") %>' Size="10"
                        FriendlyName="Cash Memo No">
                        <Validators>
                            <i:Value ValueType="String" MaxLength="30" />
                        </Validators>
                    </i:TextBoxEx>
                    <eclipse:LeftLabel runat="server" Text="Amount Granted" />
                    <i:TextBoxEx runat="server" ID="tbGrantedAmount" Text='<%# Bind("GrantedAmount","{0:N2}") %>'
                        Size="10" FriendlyName="Amount Granted">
                        <Validators>
                            <i:Value ValueType="Decimal" />
                        </Validators>
                    </i:TextBoxEx>
                    <eclipse:LeftLabel runat="server" Text="Advance adjusted" />
                    <i:TextBoxEx runat="server" ID="tbAdvanceAdjusted" Text='<%# Bind("AdvanceAdjusted","{0:N2}") %>'
                        Size="10" FriendlyName="Advance Adjusted">
                        <Validators>
                            <i:Value ValueType="Decimal" />
                        </Validators>
                    </i:TextBoxEx>
                    <eclipse:LeftLabel runat="server" Text="Order No" />
                    <i:TextBoxEx runat="server" ID="tbOfficeOrderNo" Text='<%# Bind("OfficeOrderNo") %>'
                        Size="10" FriendlyName="Order No">
                        <Validators>
                            <i:Value ValueType="String" MaxLength="30" />
                        </Validators>
                    </i:TextBoxEx>
                    <eclipse:LeftLabel runat="server" Text="Order Date" />
                    <i:TextBoxEx runat="server" ID="tbOrderDate" Text='<%# Bind("OrderDate","{0:d}") %>'
                        FriendlyName="Order Date">
                        <Validators>
                            <i:Date />
                        </Validators>
                    </i:TextBoxEx>
                    <eclipse:LeftLabel runat="server" Text="Hospital Name" />
                    <i:TextBoxEx runat="server" ID="tbHospitalName" Text='<%# Bind("HospitalName") %>'
                        Size="20" FriendlyName="Hospital Name">
                        <Validators>
                            <i:Value ValueType="String" MaxLength="50" />
                        </Validators>
                    </i:TextBoxEx>
                    <eclipse:LeftLabel runat="server" Text="Hospital Address" />
                    <i:TextBoxEx runat="server" ID="tbHospitalAddress" Text='<%# Bind("HospitalAddress") %>'
                        Size="30" FriendlyName="Hospital Address">
                        <Validators>
                            <i:Value ValueType="String" MaxLength="50" />
                        </Validators>
                    </i:TextBoxEx>
                    <eclipse:LeftLabel runat="server" Text="Hospital Country" />
                    <phpa:PhpaLinqDataSource runat="server" ID="dsCountry" ContextTypeName="Eclipse.PhpaLibrary.Database.PIS.PISDataContext"
                        Select="new (CountryId, CountryName)" TableName="Countries" RenderLogVisible="false">
                    </phpa:PhpaLinqDataSource>
                    <i:DropDownListEx runat="server" ID="ddlCountry" DataTextField="CountryName" FriendlyName="Country"
                        DataValueField="CountryId" Value='<%# Bind("CountryId") %>' DataSourceID="dsCountry">
                    </i:DropDownListEx>
                    <eclipse:LeftLabel runat="server" Text="Remarks" />
                    <i:TextBoxEx runat="server" ID="tbRemarks" Text='<%# Bind("Remarks") %>' Size="20"
                        FriendlyName="Remarks">
                        <Validators>
                            <i:Value ValueType="String" MaxLength="50" />
                        </Validators>
                    </i:TextBoxEx>
                </eclipse:TwoColumnPanel>
                <i:ValidationSummary ID="valOrderDate" runat="server" />
            </EditItemTemplate>
        </asp:FormView>
        <i:ButtonEx runat="server" ID="btnMedicalDetails" Action="Submit" Icon="Refresh"
            CausesValidation="true" OnClick="btnMedicalDetails_Click" ClientIDMode="Static" />
        <jquery:StatusPanel runat="server" ID="MedicalDetails_sp">
            <Ajax UseDialog="false" />
        </jquery:StatusPanel>
    </div>
    </form>
</body>
</html>
