<%@ Page Language="C#" CodeBehind="FamilyMembers.aspx.cs" Inherits="PhpaAll.PIS.FamilyMembers"
    EnableViewState="true" Title="Family Members" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <jquery:JQueryScriptManager runat="server" />
    <script type="text/javascript" src="TerminatedEmployee.js">
    </script>
</head>
<body>
    <form id="formFamilyMembers" runat="server">
    <div>
        <phpa:PhpaLinqDataSource runat="server" ID="dsFamilyMembers" ContextTypeName="Eclipse.PhpaLibrary.Database.PIS.PISDataContext"
            TableName="FamilyMembers" Where="EmployeeId=@EmployeeId" RenderLogVisible="false"
            OrderBy="FullName" EnableInsert="true" EnableUpdate="true" OnSelecting="dsFamilyMembers_Selecting"
            OnContextCreated="dsFamilyMembers_ContextCreated" OnInserted="dsFamilyMembers_Inserted"
            OnUpdated="dsFamilyMembers_Updated" OnDeleted="dsFamilyMembers_Deleted" EnableDelete="true">
            <WhereParameters>
                <asp:QueryStringParameter Name="EmployeeId" Type="Int32" QueryStringField="EmployeeId" />
            </WhereParameters>
            <InsertParameters>
                <asp:Parameter Name="FullName" Type="String" />
                <asp:Parameter Name="Relationship" Type="String" />
                <asp:Parameter Name="Address" Type="String" />
                <asp:Parameter Name="CountryId" Type="Int32" />
                <asp:Parameter Name="Occupation" Type="String" />
                <asp:Parameter Name="Remarks" Type="String" />
                <asp:QueryStringParameter Name="EmployeeId" Type="Int32" QueryStringField="EmployeeId" />
            </InsertParameters>
            <UpdateParameters>
                <asp:Parameter Name="FamilyMemberId" Type="Int32" />
                <asp:Parameter Name="FullName" Type="String" />
                <asp:Parameter Name="Relationship" Type="String" />
                <asp:Parameter Name="Address" Type="String" />
                <asp:Parameter Name="CountryId" Type="Int32" />
                <asp:Parameter Name="Occupation" Type="String" />
                <asp:Parameter Name="Remarks" Type="String" />
            </UpdateParameters>
            <DeleteParameters>
                <asp:Parameter Name="FamilyMemberId" Type="Int32" />
            </DeleteParameters>
        </phpa:PhpaLinqDataSource>
        <jquery:GridViewExInsert runat="server" ID="gvFamilyMembers" DataSourceID="dsFamilyMembers"
            DataKeyNames="FamilyMemberId" AutoGenerateColumns="false" Caption="Family Members"
            EmptyDataText="No family member information found" EnableViewState="true" OnRowDataBound="gvFamilyMembers_RowDataBound">
            <RowMenuItems>
                <jquery:RowMenuPostBack PostBackType="Edit" Text="Edit" />
                <jquery:RowMenuPostBack PostBackType="Delete" Text="Delete" />
            </RowMenuItems>
            <Columns>
                <jquery:CommandFieldEx ShowEditButton="false" />
                <eclipse:MultiBoundField DataFields="FullName" HeaderText="Name">
                    <EditItemTemplate>
                        <i:TextBoxEx runat="server" ID="tbFullName" Text='<%# Bind("FullName") %>' Size="15"
                            FriendlyName="Name">
                            <Validators>
                                <i:Value ValueType="String" MaxLength="50" />
                                <i:Required />
                            </Validators>
                        </i:TextBoxEx>
                    </EditItemTemplate>
                </eclipse:MultiBoundField>
                <eclipse:MultiBoundField DataFields="Relationship" HeaderText="Relationship">
                    <EditItemTemplate>
                        <phpa:PhpaLinqDataSource runat="server" ID="dsRelationship" OnSelecting="dsRelationship_Selecting"
                            RenderLogVisible="false">
                        </phpa:PhpaLinqDataSource>
                        <i:DropDownSuggest ID="ddlRelationship" runat="server" DataSourceID="dsRelationship"
                            FriendlyName="Relationship" Value='<%# Bind("Relationship") %>'>
                            <Items>
                                <eclipse:DropDownItem Text="(New Relationship)" Value="" Persistent="Always" />
                            </Items>
                            <TextBox runat="server" MaxLength="50" Size="20" />
                            <Validators>
                                <i:Required />
                            </Validators>
                        </i:DropDownSuggest>
                    </EditItemTemplate>
                </eclipse:MultiBoundField>
                <eclipse:MultiBoundField DataFields="Address" HeaderText="Address">
                    <EditItemTemplate>
                        <i:TextArea runat="server" ID="tbAddress" Text='<%# Bind("Address") %>' FriendlyName="Address"
                            Cols="20" Rows="3" />
                    </EditItemTemplate>
                </eclipse:MultiBoundField>
                <eclipse:MultiBoundField DataFields="Country.CountryName" HeaderText="Country">
                    <EditItemTemplate>
                        <phpa:PhpaLinqDataSource runat="server" ID="dsCountry" ContextTypeName="Eclipse.PhpaLibrary.Database.PIS.PISDataContext"
                            Select="new (CountryId, CountryName)" TableName="Countries" RenderLogVisible="false">
                        </phpa:PhpaLinqDataSource>
                        <i:DropDownListEx runat="server" ID="ddlCountry" DataTextField="CountryName" FriendlyName="Country"
                            DataValueField="CountryId" Value='<%# Bind("CountryId") %>' DataSourceID="dsCountry">
                            <Validators>
                                <i:Required />
                            </Validators>
                        </i:DropDownListEx>
                    </EditItemTemplate>
                </eclipse:MultiBoundField>
                <eclipse:MultiBoundField DataFields="Occupation" HeaderText="Occupation">
                    <EditItemTemplate>
                        <i:TextBoxEx runat="server" ID="tbOccupation" Text='<%# Bind("Occupation") %>' Size="15"
                            FriendlyName="Occupation">
                            <Validators>
                                <i:Value ValueType="String" MaxLength="50" />
                            </Validators>
                        </i:TextBoxEx>
                    </EditItemTemplate>
                </eclipse:MultiBoundField>
                <eclipse:MultiBoundField DataFields="Remarks" HeaderText="Remarks">
                    <EditItemTemplate>
                        <i:TextBoxEx runat="server" ID="tbRemarks" Text='<%# Bind("Remarks") %>' Size="15"
                            FriendlyName="Remarks">
                            <Validators>
                                <i:Value ValueType="String" MaxLength="50" />
                            </Validators>
                        </i:TextBoxEx>
                    </EditItemTemplate>
                </eclipse:MultiBoundField>
            </Columns>
        </jquery:GridViewExInsert>
        <i:ButtonEx runat="server" ID="formFamilyMembers_btnInsert" Action="Submit" Text="Add New Family Member"
            OnClick="btnInsert_Click" EnableViewState="false" ClientIDMode="Static" />
        <i:ValidationSummary runat="server" />
        <span id="spTerminated" class="ui-helper-hidden">
            <%= this.Request.QueryString["Terminated"] %></span>
        <jquery:StatusPanel runat="server" ID="FamilyMembers_sp">
            <Ajax UseDialog="false" />
        </jquery:StatusPanel>
    </div>
    </form>
</body>
</html>
