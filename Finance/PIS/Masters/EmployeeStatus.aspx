<%@ Page Title="Employee Status" Language="C#" MasterPageFile="~/MasterPage.master"
    CodeBehind="EmployeeStatus.aspx.cs" Inherits="PhpaAll.PIS.Masters.EmployeeStatus" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphSideNavigation" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cph" runat="server">
<p>
    <i:ButtonEx ID="btnInsert" runat="server" Text="Add New Employee Status" OnClick="btnInsert_Click"
        Action="Submit" Icon="PlusThick"/>
    <i:ValidationSummary ID="ValidationSummary1" runat="server" />
    <jquery:StatusPanel ID="EmployeeStatus_sp" runat="server">
        <Ajax UseDialog="false" />
    </jquery:StatusPanel>
</p>
    <phpa:PhpaLinqDataSource runat="server" ID="dsEmployeeStatus" ContextTypeName="Eclipse.PhpaLibrary.Database.PIS.PISDataContext"
        TableName="EmployeeStatus" RenderLogVisible="false" EnableInsert="true" OnInserted="dsEmployeeStatus_Inserted"
        EnableUpdate="true" OnUpdated="dsEmployeeStatus_Updated" OnDeleted="dsEmployeeStatus_Deleted"
        EnableDelete="true">
        <InsertParameters>
            <asp:Parameter Name="EmployeeStatusType" Type="String" />
        </InsertParameters>
        <UpdateParameters>
            <asp:Parameter Name="EmployeeStatusType" Type="String" />
        </UpdateParameters>
        <DeleteParameters>
            <asp:Parameter Name="EmployeeStatusId" Type="Int32" />
        </DeleteParameters>
    </phpa:PhpaLinqDataSource>
    <jquery:GridViewExInsert ID="gvEmployeeStatus" runat="server" AutoGenerateColumns="false"
        Caption="Employee Status" DataSourceID="dsEmployeeStatus" DataKeyNames="EmployeeStatusId"
        OnRowDataBound="gvEmployeeStatus_RowDataBound" AllowSorting="true">
        <RowMenuItems>
            <jquery:RowMenuPostBack PostBackType="Edit" Text="Edit" />
            <jquery:RowMenuPostBack PostBackType="Delete" Text="Delete" />
        </RowMenuItems>
        <Columns>
            <jquery:CommandFieldEx ShowEditButton="false" />
            <eclipse:SequenceField />
            <eclipse:MultiBoundField DataFields="EmployeeStatusType" HeaderText="Employee Status"
                SortExpression="EmployeeStatusType">
                <EditItemTemplate>
                    <i:TextBoxEx ID="tbEmployeeStatusType" runat="server" Text='<%#Bind("EmployeeStatusType") %>'
                        FriendlyName="Employee Status">
                        <Validators>
                            <i:Required />
                            <i:Value ValueType="String" />
                        </Validators>
                    </i:TextBoxEx>
                </EditItemTemplate>
            </eclipse:MultiBoundField>
        </Columns>
    </jquery:GridViewExInsert>

</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cphNoForm" runat="server">
</asp:Content>
