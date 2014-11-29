<%@ Page Language="C#" CodeBehind="QualificationDetails.aspx.cs" Inherits="Finance.PIS.Share.QualificationDetails"
    EnableViewState="true" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <jquery:JQueryScriptManager runat="server" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <phpa:PhpaLinqDataSource runat="server" ID="dsQualifications" ContextTypeName="Eclipse.PhpaLibrary.Database.PIS.PISDataContext"
            TableName="Qualifications" RenderLogVisible="False" Where="QualificationId == @QualificationId"
            OrderBy="CompletionYear descending" OnContextCreated="dsQualifications_ContextCreated"
            OnSelecting="dsQualifications_Selecting" EnableInsert="True" EnableUpdate="True"
            OnInserting="dsQualifications_Inserting" OnUpdating="dsQualifications_Updating">
            <WhereParameters>
                <asp:QueryStringParameter Name="QualificationId" Type="Int32" QueryStringField="QualificationId" />
            </WhereParameters>
            <InsertParameters>
                <asp:Parameter Name="QualificationType" Type="String" />
                <asp:Parameter Name="QualificationDivisionId" Type="Int32" />
                <asp:Parameter Name="Result" Type="String" />
                <asp:Parameter Name="CompletionYear" Type="Int32" />
                <asp:Parameter Name="Institute" Type="String" />
                <asp:Parameter Name="InstituteAddress" Type="String" />
                <asp:Parameter Name="CountryId" Type="Int32" />
                <asp:Parameter Name="StudyField" Type="String" />
                <asp:Parameter Name="Subject" Type="String" />
                <asp:Parameter Name="Remarks" Type="String" />
                <asp:QueryStringParameter Name="EmployeeId" Type="Int32" QueryStringField="EmployeeId" />
            </InsertParameters>
            <UpdateParameters>
                <asp:Parameter Name="QualificationType" Type="String" />
                <asp:Parameter Name="QualificationDivisionId" Type="Int32" />
                <asp:Parameter Name="Result" Type="String" />
                <asp:Parameter Name="CompletionYear" Type="Int32" />
                <asp:Parameter Name="Institute" Type="String" />
                <asp:Parameter Name="InstituteAddress" Type="String" />
                <asp:Parameter Name="CountryId" Type="Int32" />
                <asp:Parameter Name="StudyField" Type="String" />
                <asp:Parameter Name="Subject" Type="String" />
                <asp:Parameter Name="Remarks" Type="String" />
                <asp:QueryStringParameter Name="EmployeeId" Type="Int32" QueryStringField="EmployeeId" />
            </UpdateParameters>
        </phpa:PhpaLinqDataSource>
        <asp:FormView runat="server" ID="fvQualificationDetails" DataSourceID="dsQualifications"
            OnItemInserted="fvQualificationDetails_ItemInserted" OnItemUpdated="fvQualificationDetails_ItemUpdated"
            DataKeyNames="QualificationId,EmployeeId">
            <EditItemTemplate>
                <eclipse:TwoColumnPanel runat="server">
                    <eclipse:LeftLabel runat="server" Text="Qualification Type" />
                    <i:TextBoxEx runat="server" ID="tbEditQualificationType" Text='<%# Bind("QualificationType") %>'
                        Size="20" FriendlyName="Qualification Type">
                        <Validators>
                            <i:Value MaxLength="50" ValueType="String" />
                            <i:Required />
                        </Validators>
                    </i:TextBoxEx>
                    <eclipse:LeftLabel runat="server" Text="Division" />
                    <phpa:PhpaLinqDataSource runat="server" ID="dsQualficationDivision" ContextTypeName="Eclipse.PhpaLibrary.Database.PIS.PISDataContext"
                        Select="new (QualificationDivisionId, DivisionName)" TableName="QualificationDivisions"
                        RenderLogVisible="false" EnableInsert="true">
                        <InsertParameters>
                            <asp:Parameter Name="QualificationDivisionId" Type="Int32" />
                            <asp:Parameter Name="DivisionName" Type="String" />
                        </InsertParameters>
                    </phpa:PhpaLinqDataSource>
                    <i:DropDownSuggest runat="server" ID="ddlQualDivision" DataTextField="DivisionName"
                        DataValueField="QualificationDivisionId" Value='<%# Bind("QualificationDivisionId") %>'
                        DataSourceID="dsQualficationDivision" FriendlyName="Division" AutoSelectedValue="false">
                        <Items>
                            <eclipse:DropDownItem Text="Not Set" Value="" Persistent="Always" />
                        </Items>
                        <TextBox runat="server" Size="10" MaxLength="30" />
                    </i:DropDownSuggest>
                    <eclipse:LeftLabel runat="server" Text="Result" />
                    <i:TextBoxEx runat="server" ID="tbResult" Text='<%# Bind("Result") %>' Size="10"
                        FriendlyName="Result">
                        <Validators>
                            <i:Value ValueType="String" MaxLength="30" />
                        </Validators>
                    </i:TextBoxEx>
                    <eclipse:LeftLabel runat="server" Text="Completion Year" />
                    <i:DropDownListEx runat="server" ID="ddlCompletionYear" Value='<%# Bind("CompletionYear") %>'
                        FriendlyName="Year Of Completion" OnPreRender="ddlCompletionYear_PreRender" />
                    <eclipse:LeftLabel runat="server" Text="Institute Name" />
                    <i:TextBoxEx runat="server" ID="tbInstitute" Text='<%# Bind("Institute") %>' Size="20"
                        FriendlyName="Institute Name">
                        <Validators>
                            <i:Value MaxLength="50" ValueType="String" />
                            <i:Required />
                        </Validators>
                    </i:TextBoxEx>
                    <eclipse:LeftLabel runat="server" Text="Institute Address" />
                    <i:TextBoxEx runat="server" ID="tbInstituteAddress" Text='<%# Bind("InstituteAddress") %>'
                        Size="30" FriendlyName="Institute Address">
                        <Validators>
                            <i:Value MaxLength="50" ValueType="String" />
                        </Validators>
                    </i:TextBoxEx>
                    <eclipse:LeftLabel runat="server" Text="Institute Country" />
                    <phpa:PhpaLinqDataSource runat="server" ID="dsCountry" ContextTypeName="Eclipse.PhpaLibrary.Database.PIS.PISDataContext"
                        Select="new (CountryId, CountryName)" TableName="Countries" RenderLogVisible="false">
                    </phpa:PhpaLinqDataSource>
                    <i:DropDownListEx runat="server" ID="ddlCountry" DataTextField="CountryName" DataValueField="CountryId"
                        Value='<%# Bind("CountryId") %>' DataSourceID="dsCountry" FriendlyName="Country">
                        <Items>
                            <eclipse:DropDownItem Text="Not Set" Value="" Persistent="Always" />
                        </Items>
                    </i:DropDownListEx>
                    <eclipse:LeftLabel runat="server" Text="Field Of Study" />
                    <i:TextBoxEx runat="server" ID="tbStudyField" Text='<%# Bind("StudyField") %>' Size="20"
                        FriendlyName="Field Of Study">
                        <Validators>
                            <i:Value MaxLength="50" ValueType="String" />
                        </Validators>
                    </i:TextBoxEx>
                    <eclipse:LeftLabel runat="server" Text="Subject" />
                    <i:TextBoxEx runat="server" ID="tbSubject" Text='<%# Bind("Subject") %>' Size="20"
                        FriendlyName="Subject">
                        <Validators>
                            <i:Value MaxLength="50" ValueType="String" />
                        </Validators>
                    </i:TextBoxEx>
                    <eclipse:LeftLabel runat="server" Text="Remarks" />
                    <i:TextBoxEx runat="server" ID="tbRemarks" Text='<%# Bind("Remarks") %>' Size="20"
                        FriendlyName="Remarks">
                        <Validators>
                            <i:Value MaxLength="50" ValueType="String" />
                        </Validators>
                    </i:TextBoxEx>
                </eclipse:TwoColumnPanel>
            </EditItemTemplate>
        </asp:FormView>
        <i:ButtonEx runat="server" ID="btnQualification" Action="Submit" Icon="Refresh" CausesValidation="true"
            OnClick="btnQualification_Click" ClientIDMode="Static" />
        <i:ValidationSummary runat="server" />
        <jquery:StatusPanel ID="QualificationDetails_sp" runat="server">
            <Ajax UseDialog="false" />
        </jquery:StatusPanel>
    </div>
    </form>
</body>
</html>
