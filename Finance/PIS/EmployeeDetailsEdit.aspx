<%@ Page Language="C#" CodeBehind="EmployeeDetailsEdit.aspx.cs" Inherits="PhpaAll.PIS.EmployeeDetailsEdit" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <jquery:JQueryScriptManager runat="server" />
</head>
<body>
    <form id="formEmployeeDetailsEdit" runat="server">
    <div>
        <phpa:PhpaLinqDataSource runat="server" ID="dsEmployee" ContextTypeName="Eclipse.PhpaLibrary.Database.PIS.PISDataContext"
            TableName="Employees" RenderLogVisible="False" Where="EmployeeId == @EmployeeId"
            EnableUpdate="True" OnSelecting="ds_Selecting" OnUpdated="dsEmployee_Updated"
            OnUpdating="dsEmployee_Updating" EnableInsert="true" OnInserted="dsEmployee_Inserted"
            OnInserting="dsEmployee_Inserting" OnContextCreating="ds_ContextCreating">
            <WhereParameters>
                <asp:QueryStringParameter Name="EmployeeId" QueryStringField="EmployeeId" Type="Int32" />
            </WhereParameters>
            <UpdateParameters>
                <asp:Parameter Name="FirstName" ConvertEmptyStringToNull="true" Type="String" />
                <asp:Parameter Name="LastName" ConvertEmptyStringToNull="true" Type="String" />
                <asp:Parameter Name="EmployeeCode" ConvertEmptyStringToNull="true" Type="String" />
                <asp:Parameter Name="EmployeeNumber" ConvertEmptyStringToNull="true" Type="String" />
                <asp:Parameter Name="Gender" ConvertEmptyStringToNull="true" Type="String" />
                <asp:Parameter Name="HomeTown" ConvertEmptyStringToNull="true" Type="String" />
                <asp:Parameter Name="IsBhutanese" Type="Boolean" />
                <asp:Parameter Name="DateOfBirth" Type="DateTime" />
                <asp:Parameter Name="BloodGroupId" Type="Int32" />
                <asp:Parameter Name="MaritalStatusId" Type="Int32" />
                <asp:Parameter Name="Religion" Type="String" ConvertEmptyStringToNull="true" />
                <asp:Parameter Name="IdentificationMark" Type="String" ConvertEmptyStringToNull="true" />
                <asp:Parameter Name="Height" Type="Int32" />
                <asp:Parameter Name="JoiningDate" Type="DateTime" ConvertEmptyStringToNull="true" />
                <asp:Parameter Name="EmployeeTypeId" Type="Int32" />
                <asp:Parameter Name="DivisionId" Type="Int32" />
                <asp:Parameter Name="FileindexNo" Type="String" ConvertEmptyStringToNull="true" />
                <asp:Parameter Name="CitizenCardNo" Type="String" ConvertEmptyStringToNull="true" />
                <asp:Parameter Name="ParentOrganization" Type="String" ConvertEmptyStringToNull="true" />
                <asp:Parameter Name="EmployeeStatusId" Type="Int32" />
                <asp:Parameter Name="BasicSalary" Type="Decimal" />
                <asp:Parameter Name="BankAccountNo" Type="String" />
                <asp:Parameter Name="BankName" Type="String" />
                <asp:Parameter Name="BankPlace" Type="String" />
                <asp:Parameter Name="BankLoanAccountNo" Type="String" />
                <asp:Parameter Name="GISAccountNumber" Type="String" />
                <asp:Parameter Name="GPFAccountNo" Type="String" />
                <asp:Parameter Name="BDFCAccountNo" Type="String" />
                <asp:Parameter Name="NPPFPNo" Type="String" />
                <asp:Parameter Name="Tpn" Type="String" />
                <asp:Parameter Name="GISGroup" Type="String" />
                <asp:Parameter Name="RelieveOrderNo" Type="String" />
                <asp:Parameter Name="RelieveOrderDate" Type="DateTime" />
                <asp:Parameter Name="LeavingReason" Type="String" />
                <asp:Parameter Name="DateOfRelieve" Type="DateTime" />
                <asp:Parameter Name="SubDivisionId" Type="Int32" />
                <asp:Parameter Name="OfficeId" Type="Int32" />
                <asp:Parameter Name="ProbationEndDate" Type="DateTime" />
                <asp:Parameter Name="NPPFType" Type="String" />
                <asp:Parameter Name="StationId" Type="Int32" ConvertEmptyStringToNull="true" />
                <asp:Parameter Name="BankId" Type="Int32" ConvertEmptyStringToNull="true" />
            </UpdateParameters>
            <InsertParameters>
                <asp:Parameter Name="FirstName" ConvertEmptyStringToNull="true" Type="String" />
                <asp:Parameter Name="LastName" ConvertEmptyStringToNull="true" Type="String" />
                <asp:Parameter Name="EmployeeCode" ConvertEmptyStringToNull="true" Type="String" />
                <asp:Parameter Name="EmployeeNumber" ConvertEmptyStringToNull="true" Type="String" />
                <asp:Parameter Name="Gender" ConvertEmptyStringToNull="true" Type="String" />
                <asp:Parameter Name="HomeTown" ConvertEmptyStringToNull="true" Type="String" />
                <asp:Parameter Name="IsBhutanese" Type="Boolean" />
                <asp:Parameter Name="DateOfBirth" Type="DateTime" />
                <asp:Parameter Name="BloodGroupId" Type="Int32" />
                <asp:Parameter Name="MaritalStatusId" Type="Int32" />
                <asp:Parameter Name="Religion" Type="String" ConvertEmptyStringToNull="true" />
                <asp:Parameter Name="IdentificationMark" Type="String" ConvertEmptyStringToNull="true" />
                <asp:Parameter Name="Height" Type="Int32" />
                <asp:Parameter Name="JoiningDate" Type="DateTime" ConvertEmptyStringToNull="true" />
                <asp:Parameter Name="EmployeeTypeId" Type="Int32" />
                <asp:Parameter Name="DivisionId" Type="Int32" />
                <asp:Parameter Name="FileindexNo" Type="String" ConvertEmptyStringToNull="true" />
                <asp:Parameter Name="CitizenCardNo" Type="String" ConvertEmptyStringToNull="true" />
                <asp:Parameter Name="ParentOrganization" Type="String" ConvertEmptyStringToNull="true" />
                <asp:Parameter Name="EmployeeStatusId" Type="Int32" />
                <asp:Parameter Name="BasicSalary" Type="Decimal" />
                <asp:Parameter Name="BankAccountNo" Type="String" />
                <asp:Parameter Name="BankName" Type="String" />
                <asp:Parameter Name="BankPlace" Type="String" />
                <asp:Parameter Name="BankLoanAccountNo" Type="String" />
                <asp:Parameter Name="GISAccountNumber" Type="String" />
                <asp:Parameter Name="GPFAccountNo" Type="String" />
                <asp:Parameter Name="BDFCAccountNo" Type="String" />
                <asp:Parameter Name="NPPFPNo" Type="String" />
                <asp:Parameter Name="Tpn" Type="String" />
                <asp:Parameter Name="GISGroup" Type="String" />
                <asp:Parameter Name="RelieveOrderNo" Type="String" />
                <asp:Parameter Name="RelieveOrderDate" Type="DateTime" />
                <asp:Parameter Name="LeavingReason" Type="String" />
                <asp:Parameter Name="DateOfRelieve" Type="DateTime" />
                <asp:Parameter Name="SubDivisionId" Type="Int32" />
                <asp:Parameter Name="OfficeId" Type="Int32" />
                <asp:Parameter Name="ProbationEndDate" Type="DateTime" />
                <asp:Parameter Name="NPPFType" Type="String" />
                <asp:Parameter Name="StationId" Type="Int32" ConvertEmptyStringToNull="true" />
                <asp:Parameter Name="BankId" Type="Int32" ConvertEmptyStringToNull="true" />
            </InsertParameters>
        </phpa:PhpaLinqDataSource>
        <asp:FormView runat="server" ID="fvEmployeeDetailsEdit" DataKeyNames="EmployeeId"
            OnItemCreated="fvEmployeeDetailsEdit_ItemCreated" DataSourceID="dsEmployee" DefaultMode="Edit"
            OnItemInserted="fvEmployeeDetailsEdit_ItemInserted">
            <EditItemTemplate>
                Name:
                <i:TextBoxEx runat="server" ID="tbFirstName" Text='<%# Bind("FirstName") %>' FriendlyName="First Name"
                    Size="20" MaxLength="50">
                    <Validators>
                        <i:Required />
                    </Validators>
                </i:TextBoxEx>
                <i:TextBoxEx runat="server" ID="tbLastName" Text='<%# Bind("LastName") %>' FriendlyName="Last Name"
                    Size="20">
                    <Validators>
                        <i:Value ValueType="String" MaxLength="50" />
                    </Validators>
                </i:TextBoxEx>
                <jquery:Tabs runat="server" Selected="0" Collapsible="false" OnLoad="tabs_Load" EnableViewState="true">
                    <jquery:JPanel runat="server" HeaderText="Professional">
                        <eclipse:TwoColumnPanel runat="server">
                            <eclipse:LeftLabel runat="server" Text="Joining Date" />
                            <i:TextBoxEx runat="server" ID="tbJoiningDate" Text='<%# Bind("JoiningDate","{0:d}") %>'
                                FriendlyName="Joining Date">
                                <Validators>
                                    <i:Date Min="-20075" />
                                </Validators>
                            </i:TextBoxEx>
                            <eclipse:LeftLabel runat="server" Text="Code" />
                            <i:TextBoxEx runat="server" ID="tbEmployeeCode" Text='<%# Bind("EmployeeCode") %>'
                                FriendlyName="Code" Size="10" CaseConversion="UpperCase" ReadOnly="true">
                                <Validators>
                                    <i:Value ValueType="String" MaxLength="50" />
                                </Validators>
                            </i:TextBoxEx>
                            <eclipse:LeftLabel ID="LeftLabel1" runat="server" Text="Employee Number" />
                            <i:TextBoxEx runat="server" ID="tbEmployeeNo" Text='<%# Bind("EmployeeNumber") %>'
                                FriendlyName="EmployeeNumber" Size="14">
                                <Validators>
                                    <i:Value ValueType="String" MaxLength="20" />
                                </Validators>
                            </i:TextBoxEx>
                            <eclipse:LeftLabel runat="server" Text="Employee Type" />
                            <phpa:PhpaLinqDataSource runat="server" ID="dsEmployeeType" ContextTypeName="Eclipse.PhpaLibrary.Database.PIS.PISDataContext"
                                Select="new (EmployeeTypeId, Description)" TableName="EmployeeTypes" RenderLogVisible="false">
                            </phpa:PhpaLinqDataSource>
                            <i:DropDownListEx runat="server" ID="ddlEmployeeType" DataSourceID="dsEmployeeType"
                                DataTextField="Description" DataValueField="EmployeeTypeId" Value='<%# Bind("EmployeeTypeId") %>'
                                FriendlyName="Employee Type">
                                <Items>
                                    <eclipse:DropDownItem Text="Not Set" Value="" Persistent="Always" />
                                </Items>
                                <Validators>
                                    <i:Required />
                                </Validators>
                            </i:DropDownListEx>
                            <eclipse:LeftLabel runat="server" Text="Division" />
                            <phpa:PhpaLinqDataSource runat="server" ID="dsDivision" ContextTypeName="Eclipse.PhpaLibrary.Database.PIS.PISDataContext"
                                Select="new (DivisionId, DivisionName, DivisionGroup)" TableName="Divisions"
                                RenderLogVisible="false">
                            </phpa:PhpaLinqDataSource>
                            <i:DropDownListEx runat="server" ID="ddlDivision" DataSourceID="dsDivision" DataTextField="DivisionName"
                                DataValueField="DivisionId" Value='<%# Bind("DivisionId") %>' FriendlyName="Division"
                                DataOptionGroupField="DivisionGroup">
                                <Items>
                                    <eclipse:DropDownItem Text="Not Set" Value="" Persistent="Always" />
                                </Items>
                            </i:DropDownListEx>
                            <eclipse:LeftLabel runat="server" Text="Sub Division" />
                            <i:DropDownListEx runat="server" ID="ddlSubDivision" FriendlyName="Sub Division"
                                QueryString="SubDivisionId" Value='<%# Bind("SubDivisionId") %>'>
                                <Cascadable CascadeParentId="ddlDivision" InitializeAtStartup="true" WebMethod="GetSubDivisions" />
                                <Items>
                                    <eclipse:DropDownItem Text="(Not Exists)" Value="" Persistent="WhenEmpty" />
                                </Items>
                            </i:DropDownListEx>
                            <eclipse:LeftLabel runat="server" Text="Office" />
                            <i:DropDownListEx runat="server" ID="ddlOffices" FriendlyName="Sub Division" QueryString="OfficeId"
                                Value='<%# Bind("OfficeId") %>'>
                                <Cascadable CascadeParentId="ddlSubDivision" InitializeAtStartup="true" WebMethod="GetOffices" />
                                <Items>
                                    <eclipse:DropDownItem Text="(Not Exists)" Value="" Persistent="WhenEmpty" />
                                </Items>
                            </i:DropDownListEx>
                            <phpa:PhpaLinqDataSource runat="server" ID="dsStation" ContextTypeName="Eclipse.PhpaLibrary.Database.PIS.PISDataContext"
                                Select="new (StationId, StationName)" TableName="Stations" RenderLogVisible="false"
                                EnableInsert="true">
                                <InsertParameters>
                                    <asp:Parameter Name="StationId" Type="Int32" />
                                    <asp:Parameter Name="StationName" Type="String" />
                                </InsertParameters>
                            </phpa:PhpaLinqDataSource>
                            <eclipse:LeftLabel ID="LeftLabel2" runat="server" Text="Posted At" />
                            <i:DropDownListEx runat="server" ID="ddlStation" FriendlyName="PostedAt" QueryString="StationId"
                                DataSourceID="dsStation" Value='<%# Bind("StationId") %>' DataTextField="StationName"
                                DataValueField="StationId">
                                <Items>
                                    <eclipse:DropDownItem Text="(Not Set)" Value="" Persistent="Always" />
                                </Items>
                                <Validators>
                                    <i:Required />
                                </Validators>
                            </i:DropDownListEx>
                            <eclipse:LeftLabel runat="server" Text="Probation End Date" />
                            <i:TextBoxEx runat="server" ID="tbProbationEndDate" Text='<%# Bind("ProbationEndDate","{0:d}") %>'
                                FriendlyName="Probation End Date">
                                <Validators>
                                    <i:Date />
                                </Validators>
                            </i:TextBoxEx>
                            <eclipse:LeftLabel runat="server" Text="Fileindex No" />
                            <i:TextBoxEx runat="server" ID="tbCadreNo" Text='<%# Bind("FileindexNo") %>' FriendlyName="Fileindex No"
                                Size="10">
                                <Validators>
                                    <i:Value ValueType="String" MaxLength="20" />
                                </Validators>
                            </i:TextBoxEx>
                            <eclipse:LeftLabel runat="server" Text="Citizen Card /Work Permit#" />
                            <i:TextBoxEx runat="server" ID="tbCitizenCardNo" Text='<%# Bind("CitizenCardNo") %>'
                                FriendlyName="Citizen Card /Work Permit#" Size="30">
                                <Validators>
                                    <i:Value ValueType="String" MaxLength="60" />
                                </Validators>
                            </i:TextBoxEx>
                            <eclipse:LeftLabel runat="server" Text="Parent Organization" />
                            <i:TextBoxEx runat="server" ID="tbParentOrganization" Text='<%# Bind("ParentOrganization") %>'
                                FriendlyName="Parent Organization" Size="30">
                                <Validators>
                                    <i:Value ValueType="String" MaxLength="50" />
                                </Validators>
                            </i:TextBoxEx>
                        </eclipse:TwoColumnPanel>
                    </jquery:JPanel>
                    <jquery:JPanel runat="server" HeaderText="Financial">
                        <eclipse:TwoColumnPanel runat="server">
                            <eclipse:LeftLabel runat="server" Text="Basic Salary" />
                            <i:TextBoxEx runat="server" ID="tbBasicSalary" Text='<%# Bind("BasicSalary","{0:N2}") %>'
                                FriendlyName="Basic Salary">
                                <Validators>
                                    <i:Value ValueType="Decimal" />
                                </Validators>
                            </i:TextBoxEx>
                            <eclipse:LeftLabel runat="server" Text="Bank Account No" />
                            <i:TextBoxEx runat="server" ID="tbBankAccountNo" Text='<%# Bind("BankAccountNo") %>'
                                FriendlyName="Bank Account No" Size="20">
                                <Validators>
                                    <i:Value ValueType="String" MaxLength="30" />
                                </Validators>
                            </i:TextBoxEx>
                            <phpa:PhpaLinqDataSource runat="server" ID="dsBankName" ContextTypeName="Eclipse.PhpaLibrary.Database.PIS.PISDataContext"
                                Select="new (BankId, BankName)" RenderLogVisible="false" TableName="Banks">
                            </phpa:PhpaLinqDataSource>
                            <eclipse:LeftLabel runat="server" Text="Bank Name" />
                            <i:DropDownListEx ID="ddlBankName" runat="server" DataTextField="BankName" DataValueField="BankId"
                                DataSourceID="dsBankName" FriendlyName="BankName" Value='<%# Bind("BankId") %>'>
                                <Items>
                                    <eclipse:DropDownItem Value="" Text="(Not Set)" Persistent="Always" />
                                </Items>
                                <Validators>
                                    <i:Required />
                                </Validators>
                                <%--<TextBox runat="server" Size="20" MaxLength="30" />--%>
                            </i:DropDownListEx>
                            <eclipse:LeftLabel runat="server" Text="Bank Address" />
                            <i:TextArea runat="server" ID="tbBankPlace" Text='<%# Bind("BankPlace") %>' FriendlyName="Bank Address"
                                Rows="3" Cols="35">
                                <Validators>
                                    <i:Value ValueType="String" MaxLength="50" />
                                </Validators>
                            </i:TextArea>
                            <eclipse:LeftLabel runat="server" Text="Bank Loan Account Number" />
                            <i:TextBoxEx runat="server" ID="tbBankLoanAccountNo" Text='<%# Bind("BankLoanAccountNo") %>'
                                FriendlyName="Bank Loan Account Number" Size="20">
                                <Validators>
                                    <i:Value ValueType="String" MaxLength="30" />
                                </Validators>
                            </i:TextBoxEx>
                            <eclipse:LeftLabel runat="server" Text="GIS Account Number" />
                            <i:TextBoxEx runat="server" ID="tbGISAccountNumber" Text='<%# Bind("GISAccountNumber") %>'
                                FriendlyName="GIS Account Number" Size="20">
                                <Validators>
                                    <i:Value ValueType="String" MaxLength="20" />
                                </Validators>
                            </i:TextBoxEx>
                            <eclipse:LeftLabel runat="server" Text="GPF Account Number" />
                            <i:TextBoxEx runat="server" ID="tbGPFAccountNo" Text='<%# Bind("GPFAccountNo") %>'
                                FriendlyName="GPF Account Number" Size="20">
                                <Validators>
                                    <i:Value ValueType="String" MaxLength="30" />
                                </Validators>
                            </i:TextBoxEx>
                            <eclipse:LeftLabel runat="server" Text="BDFC Account Number" />
                            <i:TextBoxEx runat="server" ID="tbBDFCAccountNo" Text='<%# Bind("BDFCAccountNo") %>'
                                FriendlyName="BDFC Account Number" Size="20">
                                <Validators>
                                    <i:Value ValueType="String" MaxLength="30" />
                                </Validators>
                            </i:TextBoxEx>
                            <eclipse:LeftLabel runat="server" Text="NPPFP Number" />
                            <i:TextBoxEx runat="server" ID="tbNPPFPNo" Text='<%# Bind("NPPFPNo") %>' FriendlyName="NPPFP Number"
                                Size="20">
                                <Validators>
                                    <i:Value ValueType="String" MaxLength="30" />
                                </Validators>
                            </i:TextBoxEx>
                            <eclipse:LeftLabel runat="server" Text="NPPFP Type" />
                            <i:RadioButtonListEx runat="server" ID="rblNPPFType" Value='<%# Bind("NPPFType") %>'
                                FriendlyName="Gender">
                                <Items>
                                    <i:RadioItem Text="Tier-2" Value="Tier-2" />
                                    <i:RadioItem Text="Both" Value="Both" />
                                </Items>
                            </i:RadioButtonListEx>
                            <eclipse:LeftLabel runat="server" Text="TPN" />
                            <i:TextBoxEx runat="server" ID="tbTpn" Text='<%# Bind("Tpn") %>' FriendlyName="TPN">
                                <Validators>
                                    <i:Value ValueType="String" MaxLength="15" />
                                </Validators>
                            </i:TextBoxEx>
                            <eclipse:LeftLabel runat="server" Text="GIS Group" />
                            <i:TextBoxEx runat="server" ID="tbGISGroup" Text='<%# Bind("GISGroup") %>' FriendlyName="GIS Group">
                                <Validators>
                                    <i:Value ValueType="String" MaxLength="5" />
                                </Validators>
                            </i:TextBoxEx>
                        </eclipse:TwoColumnPanel>
                    </jquery:JPanel>
                    <jquery:JPanel runat="server" HeaderText="Personal">
                        <eclipse:TwoColumnPanel runat="server">
                            <eclipse:LeftLabel runat="server" Text="Gender" />
                            <i:RadioButtonListEx runat="server" ID="rbGender" Value='<%# Bind("Gender") %>' FriendlyName="Gender">
                                <Items>
                                    <i:RadioItem Text="Not Set" Value="Not Set" />
                                    <i:RadioItem Text="Male" Value="Male" />
                                    <i:RadioItem Text="Female" Value="Female" />
                                </Items>
                            </i:RadioButtonListEx>
                            <eclipse:LeftLabel runat="server" Text="Permanent Address" />
                            <i:TextArea runat="server" ID="tbHomeTown" Text='<%# Bind("HomeTown") %>' FriendlyName="Permanent Address"
                                Rows="3" Cols="35" />
                            <eclipse:LeftLabel runat="server" Text="Nationality" />
                            <i:RadioButtonListEx runat="server" ID="cbNationality" Value='<%# Bind("IsBhutanese") %>'
                                FriendlyName="Nationality">
                                <Items>
                                    <i:RadioItem Text="Bhutan National" Value="True" />
                                    <i:RadioItem Text="Foreigner" Value="False" />
                                </Items>
                            </i:RadioButtonListEx>
                            <eclipse:LeftLabel runat="server" Text="Date Of Birth" />
                            <i:TextBoxEx runat="server" ID="tbDOB" Text='<%# Bind("DateOfBirth","{0:d}") %>'
                                FriendlyName="Date Of Birth">
                                <Validators>
                                    <i:Date Min="-32850" Max="-5840" />
                                </Validators>
                            </i:TextBoxEx>
                            <eclipse:LeftLabel runat="server" Text="Blood Group" />
                            <phpa:PhpaLinqDataSource runat="server" ID="dsBloodGroup" ContextTypeName="Eclipse.PhpaLibrary.Database.PIS.PISDataContext"
                                Select="new (BloodGroupId, BloodGroupType)" TableName="BloodGroups" RenderLogVisible="false"
                                EnableInsert="true">
                                <InsertParameters>
                                    <asp:Parameter Name="BloodGroupId" Type="Int32" />
                                    <asp:Parameter Name="BloodGroupType" Type="String" />
                                </InsertParameters>
                            </phpa:PhpaLinqDataSource>
                            <i:DropDownSuggest runat="server" ID="ddlBloodGroup" DataSourceID="dsBloodGroup"
                                DataTextField="BloodGroupType" DataValueField="BloodGroupId" Value='<%# Bind("BloodGroupId") %>'
                                FriendlyName="Blood Group" AutoSelectedValue="false" EnableViewState="true">
                                <Items>
                                    <eclipse:DropDownItem Text="(New Blood Group)" Value="" Persistent="Always" />
                                </Items>
                                <TextBox runat="server" Size="5" MaxLength="10" />
                            </i:DropDownSuggest>
                            <eclipse:LeftLabel runat="server" Text="Marital Status" />
                            <phpa:PhpaLinqDataSource runat="server" ID="dsMaritalStatus" ContextTypeName="Eclipse.PhpaLibrary.Database.PIS.PISDataContext"
                                Select="new (MaritalStatusId, MaritalStatusType)" TableName="MaritalStatus" EnableInsert="true"
                                RenderLogVisible="false">
                                <InsertParameters>
                                    <asp:Parameter Name="MaritalStatusId" Type="Int32" />
                                    <asp:Parameter Name="MaritalStatusType" Type="String" />
                                </InsertParameters>
                            </phpa:PhpaLinqDataSource>
                            <i:DropDownSuggest runat="server" ID="ddlMaritalStatus" DataSourceID="dsMaritalStatus"
                                DataTextField="MaritalStatusType" DataValueField="MaritalStatusId" Value='<%# Bind("MaritalStatusId") %>'
                                FriendlyName="Marital Status" AutoSelectedValue="false">
                                <Items>
                                    <eclipse:DropDownItem Text="(New Marital Status)" Value="" Persistent="Always" />
                                </Items>
                                <TextBox runat="server" Size="15" MaxLength="50" />
                            </i:DropDownSuggest>
                            <eclipse:LeftLabel runat="server" Text="Religion" />
                            <phpa:PhpaLinqDataSource runat="server" ID="dsReligion" ContextTypeName="Eclipse.PhpaLibrary.Database.PIS.PISDataContext"
                                OnSelecting="dsReligion_Selecting" RenderLogVisible="false" TableName="Employees">
                            </phpa:PhpaLinqDataSource>
                            <i:DropDownSuggest ID="ddlReligion" runat="server" DataSourceID="dsReligion" FriendlyName="Religion"
                                Value='<%# Bind("Religion") %>'>
                                <Items>
                                    <eclipse:DropDownItem Value="" Text="(New Religion)" Persistent="Always" />
                                </Items>
                                <TextBox runat="server" Size="20" MaxLength="30" />
                            </i:DropDownSuggest>
                            <eclipse:LeftLabel runat="server" Text="Identification Mark" />
                            <i:TextBoxEx runat="server" ID="tbIdentificationMark" Text='<%# Bind("IdentificationMark") %>'
                                FriendlyName="Identification Mark" Size="35">
                                <Validators>
                                    <i:Value ValueType="String" MaxLength="256" />
                                </Validators>
                            </i:TextBoxEx>
                            <eclipse:LeftLabel runat="server" Text="Height (in cms)" />
                            <i:TextBoxEx runat="server" ID="tbHeight" Text='<%# Bind("Height") %>' FriendlyName="Height">
                                <Validators>
                                    <i:Value ValueType="Integer" MaxLength="4" />
                                </Validators>
                            </i:TextBoxEx>
                        </eclipse:TwoColumnPanel>
                    </jquery:JPanel>
                    <jquery:JPanel runat="server" HeaderText="Service">
                        <phpa:PhpaLinqDataSource runat="server" ID="dsServiceHistory" ContextTypeName="Eclipse.PhpaLibrary.Database.PIS.PISDataContext"
                            TableName="ServicePeriods" Where="EmployeeId == @EmployeeId" OrderBy="PeriodStartDate desc"
                            RenderLogVisible="false" EnableInsert="true" EnableUpdate="true" OnSelecting="ds_Selecting"
                            OnInserting="dsServiceHistory_Inserting" OnUpdating="dsServiceHistory_Updating"
                            OnContextCreating="ds_ContextCreating">
                            <WhereParameters>
                                <asp:QueryStringParameter Name="EmployeeId" QueryStringField="EmployeeId" Type="Int32" />
                            </WhereParameters>
                            <InsertParameters>
                                <asp:Parameter Name="PeriodStartDate" Type="DateTime" />
                                <asp:Parameter Name="PeriodEndDate" Type="DateTime" />
                                <asp:Parameter Name="Designation" Type="String" />
                                <asp:Parameter Name="Grade" Type="Int32" />
                                <asp:Parameter Name="BasicSalary" Type="Decimal" />
                                <asp:Parameter Name="IsConsolidated" Type="Boolean" />
                                <asp:Parameter Name="MinPayScaleAmount" Type="Decimal" />
                                <asp:Parameter Name="MaxPayScaleAmount" Type="Decimal" />
                                <asp:Parameter Name="IncrementAmount" Type="Decimal" />
                                <asp:Parameter Name="DateOfIncrement" Type="DateTime" />
                                <asp:Parameter Name="DateOfNextIncrement" Type="DateTime" />
                                <asp:Parameter Name="PostedAt" Type="String" />
                                <asp:Parameter Name="GovtOrderNo" Type="String" />
                                <asp:Parameter Name="GovtOrderDate" Type="DateTime" />
                                <asp:Parameter Name="InitialTerm" Type="String" />
                                <asp:Parameter Name="Remarks" Type="String" />
                                <asp:QueryStringParameter Name="EmployeeId" Type="Int32" QueryStringField="EmployeeId" />
                            </InsertParameters>
                            <UpdateParameters>
                                <asp:Parameter Name="ServicePeriodId" Type="Int32" />
                                <asp:Parameter Name="PeriodStartDate" Type="DateTime" />
                                <asp:Parameter Name="PeriodEndDate" Type="DateTime" />
                                <asp:Parameter Name="Designation" Type="String" />
                                <asp:Parameter Name="Grade" Type="Int32" />
                                <asp:Parameter Name="BasicSalary" Type="Decimal" />
                                <asp:Parameter Name="IsConsolidated" Type="Boolean" />
                                <asp:Parameter Name="MinPayScaleAmount" Type="Decimal" />
                                <asp:Parameter Name="MaxPayScaleAmount" Type="Decimal" />
                                <asp:Parameter Name="IncrementAmount" Type="Decimal" />
                                <asp:Parameter Name="DateOfIncrement" Type="DateTime" />
                                <asp:Parameter Name="DateOfNextIncrement" Type="DateTime" />
                                <asp:Parameter Name="PostedAt" Type="String" />
                                <asp:Parameter Name="GovtOrderNo" Type="String" />
                                <asp:Parameter Name="GovtOrderDate" Type="DateTime" />
                                <asp:Parameter Name="InitialTerm" Type="String" />
                                <asp:Parameter Name="Remarks" Type="String" />
                            </UpdateParameters>
                        </phpa:PhpaLinqDataSource>
                        <asp:FormView runat="server" ID="fvServiceHistory" DataSourceID="dsServiceHistory"
                            DataKeyNames="ServicePeriodId" DefaultMode="Edit" OnDataBound="fvServiceHistory_DataBound"
                            OnItemUpdated="fvServiceHistory_ItemUpdated" OnItemInserted="fvServiceHistory_ItemInserted">
                            <HeaderTemplate>
                                Current Service Period
                            </HeaderTemplate>
                            <EmptyDataTemplate>
                                This employee is currently not in service.
                            </EmptyDataTemplate>
                            <EditItemTemplate>
                                <eclipse:TwoColumnPanel runat="server">
                                    <eclipse:LeftLabel runat="server" Text="Service Period" />
                                    <i:TextBoxEx runat="server" ID="tbPeriodStartDate" Text='<%# Bind("PeriodStartDate","{0:d}") %>'
                                        FriendlyName="Service Period Start Date">
                                        <Validators>
                                            <i:Required />
                                            <i:Date />
                                        </Validators>
                                    </i:TextBoxEx>
                                    <i:TextBoxEx runat="server" ID="tbPeriodEndDate" Text='<%# Bind("PeriodEndDate","{0:d}") %>'
                                        FriendlyName="Service Period End Date">
                                        <Validators>
                                            <i:Date DateType="ToDate" />
                                        </Validators>
                                    </i:TextBoxEx>
                                    <eclipse:LeftLabel runat="server" Text="Govt Order" />
                                    <i:TextBoxEx runat="server" ID="tbGovtOrderNo" Text='<%# Bind("GovtOrderNo") %>'
                                        FriendlyName="Govt Order No" Size="10" MaxLength="30" />
                                    <i:TextBoxEx runat="server" ID="tbGovtOrderDate" Text='<%# Bind("GovtOrderDate","{0:d}") %>'
                                        FriendlyName="Govt Order Date">
                                        <Validators>
                                            <i:Date />
                                        </Validators>
                                    </i:TextBoxEx>
                                    <eclipse:LeftLabel runat="server" Text="Designation" />
                                    <phpa:PhpaLinqDataSource runat="server" ID="dsDesignation" ContextTypeName="Eclipse.PhpaLibrary.Database.PIS.PISDataContext"
                                        OnSelecting="dsDesignation_Selecting" RenderLogVisible="false">
                                    </phpa:PhpaLinqDataSource>
                                    <i:DropDownSuggest runat="server" ID="ddlDesignation" Value='<%# Bind("Designation") %>'
                                        FriendlyName="Designation" DataSourceID="dsDesignation">
                                        <Items>
                                            <eclipse:DropDownItem Text="(New Designation)" Value="" Persistent="Always" />
                                        </Items>
                                        <TextBox runat="server" Size="25" MaxLength="50" />
                                    </i:DropDownSuggest>
                                    <eclipse:LeftLabel runat="server" Text="Grade" />
                                    <phpa:PhpaLinqDataSource runat="server" ID="dsGrade" ContextTypeName="Eclipse.PhpaLibrary.Database.PIS.PISDataContext"
                                        OnSelecting="dsGrade_Selecting" RenderLogVisible="false">
                                    </phpa:PhpaLinqDataSource>
                                    <i:DropDownSuggest runat="server" ID="ddlGrade" Value='<%# Bind("Grade") %>' FriendlyName="Grade"
                                        DataSourceID="dsGrade">
                                        <Items>
                                            <eclipse:DropDownItem Text="(New Grade)" Value="" Persistent="Always" />
                                        </Items>
                                        <TextBox runat="server" MaxLength="4" />
                                    </i:DropDownSuggest>
                                    <%-- <eclipse:LeftLabel runat="server" Text="Posted At" />
                                    <phpa:PhpaLinqDataSource runat="server" ID="dsPostedAt" ContextTypeName="Eclipse.PhpaLibrary.Database.PIS.PISDataContext"
                                        OnSelecting="dsPostedAt_Selecting" RenderLogVisible="false">
                                    </phpa:PhpaLinqDataSource>
                                    <i:DropDownSuggest ID="ddlPostedAt" runat="server" DataSourceID="dsPostedAt" FriendlyName="Posted At"
                                        Value='<%# Bind("PostedAt") %>'>
                                        <Items>
                                            <eclipse:DropDownItem Value="" Text="(New Posted At)" Persistent="Always" />
                                        </Items>
                                        <TextBox runat="server" Size="25" MaxLength="50" />
                                    </i:DropDownSuggest>--%>
                                    <eclipse:LeftLabel runat="server" Text="Initial Term" />
                                    <i:TextBoxEx runat="server" ID="tbInitialTerm" Text='<%# Bind("InitialTerm") %>'
                                        FriendlyName="Initial Term" Size="10" MaxLength="20" />
                                    <eclipse:LeftLabel runat="server" Text="Proposed Basic Salary" />
                                    <i:TextBoxEx runat="server" ID="tbBasicSalary" Text='<%# Bind("BasicSalary","{0:N2}") %>'
                                        FriendlyName="Basic Salary">
                                        <Validators>
                                            <i:Value ValueType="Decimal" />
                                        </Validators>
                                    </i:TextBoxEx>
                                    <i:CheckBoxEx runat="server" ID="cbConsolidated" Checked='<%# Bind("IsConsolidated") %>'
                                        FriendlyName="Consolidated" Text="Consolidated" CheckedValue="True" />
                                    <eclipse:LeftLabel runat="server" Text="PayScale" />
                                    <i:TextBoxEx runat="server" ID="tbMinPayScale" Text='<%# Bind("MinPayScaleAmount","{0:N2}") %>'
                                        FriendlyName="Min PayScale">
                                        <Validators>
                                            <i:Value ValueType="Decimal" />
                                        </Validators>
                                    </i:TextBoxEx>
                                    <i:TextBoxEx runat="server" ID="tbMaxPayScale" Text='<%# Bind("MaxPayScaleAmount","{0:N2}") %>'
                                        FriendlyName="Max PayScale">
                                        <Validators>
                                            <i:Value ValueType="Decimal" MinControlID="tbMinPayScale" />
                                        </Validators>
                                    </i:TextBoxEx>
                                    <eclipse:LeftLabel runat="server" Text="Increment" />
                                    <i:TextBoxEx runat="server" ID="tbIncrementAmount" Text='<%# Bind("IncrementAmount","{0:N2}") %>'
                                        FriendlyName="Increment Amount">
                                        <Validators>
                                            <i:Value ValueType="Decimal" />
                                        </Validators>
                                    </i:TextBoxEx>
                                    <eclipse:LeftLabel runat="server" Text="Remarks" />
                                    <i:TextBoxEx runat="server" ID="tbRemarks" Text='<%# Bind("Remarks") %>' FriendlyName="Remarks"
                                        Size="30" MaxLength="50" />
                                </eclipse:TwoColumnPanel>
                                <div class="ui-helper-clearfix">
                                </div>
                            </EditItemTemplate>
                        </asp:FormView>
                    </jquery:JPanel>
                    <%--<jquery:JPanel runat="server" HeaderText="Termination" ID="EmployeeDetailsEdit_Termination">
                        <eclipse:TwoColumnPanel runat="server">
                            <eclipse:LeftLabel runat="server" Text="Relieve Order No" />
                            <i:TextBoxEx runat="server" ID="tbRelieveOrderNo" Text='<%# Bind("RelieveOrderNo") %>'
                                FriendlyName="Relieve Order No" Size="10" MaxLength="30" />
                            <eclipse:LeftLabel runat="server" Text="Relieve Order Date" />
                            <i:TextBoxEx runat="server" ID="tbRelieveOrderDate" Text='<%# Bind("RelieveOrderDate","{0:d}") %>'
                                FriendlyName="Relieve Order Date">
                                <Validators>
                                    <i:Date />
                                </Validators>
                            </i:TextBoxEx>
                            <eclipse:LeftLabel runat="server" Text="Reason for Leaving" />
                            <i:TextBoxEx runat="server" ID="tbLeavingReason" Text='<%# Bind("LeavingReason") %>'
                                FriendlyName="Leaving Reason" Size="30" MaxLength="50" />
                            <eclipse:LeftLabel runat="server" Text="Relieve Date" />
                            <i:TextBoxEx runat="server" ID="tbDateOfRelieve" Text='<%# Bind("DateOfRelieve","{0:d}") %>'
                                FriendlyName="Relieve Date">
                                <Validators>
                                    <i:Date />
                                </Validators>
                            </i:TextBoxEx>
                            <eclipse:LeftLabel runat="server" ID="lblTerminationStatus" Text="Termination Status" />
                            <phpa:PhpaLinqDataSource runat="server" ID="dsEmployeeStatus" ContextTypeName="Eclipse.PhpaLibrary.Database.PIS.PISDataContext"
                                Select="new (EmployeeStatusId, EmployeeStatusType)" TableName="EmployeeStatus"
                                RenderLogVisible="false">
                            </phpa:PhpaLinqDataSource>
                            <i:DropDownListEx runat="server" ID="ddlEmployeeStatus" DataSourceID="dsEmployeeStatus"
                                DataTextField="EmployeeStatusType" DataValueField="EmployeeStatusId" Value='<%# Bind("EmployeeStatusId") %>'
                                FriendlyName="Termination Status" Enabled="false">
                                <Items>
                                    <eclipse:DropDownItem Text="Not Set" Value="" Persistent="Always" />
                                </Items>
                            </i:DropDownListEx>
                            <br />
                            Setting the termination status does not actually terminate the service. You must
                            also ensure that the end date of the service period is set correctly.
                        </eclipse:TwoColumnPanel>
                    </jquery:JPanel>--%>
                </jquery:Tabs>
            </EditItemTemplate>
        </asp:FormView>
        <i:ButtonEx runat="server" ID="btnUpdate" Action="Submit" Icon="Refresh" CausesValidation="true"
            OnClick="btnUpdate_Click" Text="Update" ClientIDMode="Static" />
        <i:ValidationSummary ID="ValidationSummary1" runat="server" />
        <jquery:StatusPanel runat="server" ID="EmployeeDetailsEdit_spEmpEdit">
            <Ajax UseDialog="false" />
        </jquery:StatusPanel>
    </div>
    </form>
</body>
</html>
