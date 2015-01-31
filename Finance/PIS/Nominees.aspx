<%@ Page Language="C#" CodeBehind="Nominees.aspx.cs" Inherits="PhpaAll.PIS.Nominees"
    EnableViewState="true" Title="Nominees" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <jquery:JQueryScriptManager runat="server" />
    <script type="text/javascript" src="TerminatedEmployee.js">
    </script>
</head>
<body>
    <form id="formNominees" runat="server">
    <div>
        <phpa:PhpaLinqDataSource runat="server" ID="dsNominees" ContextTypeName="Eclipse.PhpaLibrary.Database.PIS.PISDataContext"
            TableName="Nominees" Where="EmployeeId=@EmployeeId" RenderLogVisible="false"
            OrderBy="FamilyMember.FullName" EnableInsert="true" EnableUpdate="true" OnSelecting="dsNominees_Selecting"
            OnContextCreated="dsNominees_ContextCreated" OnInserted="dsNominees_Inserted"
            OnUpdated="dsNominees_Updated" OnDeleted="dsNominees_Deleted" EnableDelete="true">
            <WhereParameters>
                <asp:QueryStringParameter Name="EmployeeId" Type="Int32" QueryStringField="EmployeeId" />
            </WhereParameters>
            <InsertParameters>
                <asp:Parameter Name="FamilyMemberId" Type="Int32" />
                <asp:Parameter Name="EntitlementTypeId" Type="Int32" />
                <asp:Parameter Name="Description" Type="String" />
                <asp:QueryStringParameter Name="EmployeeId" Type="Int32" QueryStringField="EmployeeId" />
            </InsertParameters>
            <UpdateParameters>
                <asp:Parameter Name="EntitlementTypeId" Type="Int32" />
                <asp:Parameter Name="FamilyMemberId" Type="Int32" />
                <asp:Parameter Name="Description" Type="String" />
            </UpdateParameters>
        </phpa:PhpaLinqDataSource>
        <jquery:GridViewExInsert runat="server" ID="gvNominees" DataSourceID="dsNominees"
            DataKeyNames="NomineeId" AutoGenerateColumns="false" Caption="Nominees" EmptyDataText="No nominee information found"
            OnRowDataBound="gvNominees_RowDataBound" OnRowInserting="gvNominees_RowInserting">
            <RowMenuItems>
                <jquery:RowMenuPostBack PostBackType="Edit" Text="Edit" />
                <jquery:RowMenuPostBack PostBackType="Delete" Text="Delete" />
            </RowMenuItems>
            <Columns>
                <jquery:CommandFieldEx ShowEditButton="false" />
                <eclipse:MultiBoundField DataFields="FamilyMember.FullName" HeaderText="Name">
                    <EditItemTemplate>
                        <phpa:PhpaLinqDataSource runat="server" ID="dsFamilyMembers" ContextTypeName="Eclipse.PhpaLibrary.Database.PIS.PISDataContext"
                            Select="new (FamilyMemberId, FullName)" TableName="FamilyMembers" RenderLogVisible="false"
                            Where="EmployeeId=@EmployeeId">
                            <WhereParameters>
                                <asp:QueryStringParameter Name="EmployeeId" QueryStringField="EmployeeId" Type="Int32" />
                            </WhereParameters>
                        </phpa:PhpaLinqDataSource>
                        <i:DropDownListEx runat="server" ID="ddlFamilyMember" DataTextField="FullName" FriendlyName="Name"
                            DataValueField="FamilyMemberId" DataSourceID="dsFamilyMembers" Value='<%# Bind("FamilyMemberId") %>'>
                            <Validators>
                                <i:Required />
                            </Validators>
                            <Items>
                                <eclipse:DropDownItem Text="No Family Members Found" Persistent="WhenEmpty" />
                            </Items>
                        </i:DropDownListEx>
                    </EditItemTemplate>
                </eclipse:MultiBoundField>
                <eclipse:MultiBoundField DataFields="EntitlementType.EntitlementDescription" HeaderText="Entitlement|Type">
                    <EditItemTemplate>
                        <phpa:PhpaLinqDataSource runat="server" ID="dsEntitlementType" ContextTypeName="Eclipse.PhpaLibrary.Database.PIS.PISDataContext"
                            Select="new (EntitlementTypeId, EntitlementDescription)" TableName="EntitlementTypes"
                            RenderLogVisible="False" EnableInsert="true">
                            <InsertParameters>
                                <asp:Parameter Name="EntitlementTypeId" Type="Int32" />
                                <asp:Parameter Name="EntitlementDescription" Type="String" />
                            </InsertParameters>
                        </phpa:PhpaLinqDataSource>
                        <i:DropDownSuggest runat="server" ID="ddlEntitlementType" DataTextField="EntitlementDescription"
                            DataValueField="EntitlementTypeId" Value='<%# Bind("EntitlementTypeId") %>'
                            DataSourceID="dsEntitlementType" FriendlyName="Entitlement Type" AutoSelectedValue="false">
                            <Items>
                                <eclipse:DropDownItem Text="(New Entitlement Type)" Value="" Persistent="Always" />
                            </Items>
                            <TextBox runat="server" Size="20" MaxLength="50" />
                            <Validators>
                                <i:Required />
                            </Validators>
                        </i:DropDownSuggest>
                    </EditItemTemplate>
                </eclipse:MultiBoundField>
                <eclipse:MultiBoundField DataFields="Description" HeaderText="Entitlement|Description">
                    <EditItemTemplate>
                        <i:TextBoxEx runat="server" ID="tbDescription" Text='<%# Bind("Description") %>'
                            FriendlyName="Description" Size="30">
                            <Validators>
                                <i:Value ValueType="String" MaxLength="50" />
                            </Validators>
                        </i:TextBoxEx>
                    </EditItemTemplate>
                </eclipse:MultiBoundField>
            </Columns>
        </jquery:GridViewExInsert>
        <i:ValidationSummary runat="server" />
        <span id="spTerminated" class="ui-helper-hidden">
            <%= this.Request.QueryString["Terminated"] %></span>
        <i:ButtonEx runat="server" ID="formNominees_btnInsert" Text="Add New Nominee" Action="Submit"
            OnClick="btnInsert_Click" ClientIDMode="Static" />
        <jquery:StatusPanel runat="server" ID="Nominees_sp">
            <Ajax UseDialog="false" />
        </jquery:StatusPanel>
    </div>
    </form>
</body>
</html>
