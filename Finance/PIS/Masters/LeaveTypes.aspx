<%@ Page Title="Leave Types" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeBehind="LeaveTypes.aspx.cs" Inherits="PIS.Masters.LeaveTypes" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphSideNavigation" runat="server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cph" runat="server">

<p>
    <i:ButtonEx ID="btnInsert" runat="server" Text="Add New Leave Type" OnClick="btnInsert_Click"
        Action="Submit" Icon="PlusThick"/>
    <i:ValidationSummary ID="ValidationSummary1" runat="server" />
    <jquery:StatusPanel ID="LeaveTypes_sp" runat="server">
        <Ajax UseDialog="false" />
    </jquery:StatusPanel>
</p>
    <phpa:PhpaLinqDataSource ID="dsLeaveTypes" runat="server" ContextTypeName="Eclipse.PhpaLibrary.Database.PIS.PISDataContext"
        TableName="LeaveTypes" RenderLogVisible="false" EnableInsert="true" OnInserted="dsLeaveTypes_Inserted"
        EnableUpdate="true" OnUpdated="dsLeaveTypes_Updated" OnDeleted="dsLeaveTypes_Deleted"
        EnableDelete="true">
        <InsertParameters>
            <asp:Parameter Name="LeaveDescription" Type="String" />
        </InsertParameters>
        <UpdateParameters>
            <asp:Parameter Name="LeaveDescription" Type="String" />
        </UpdateParameters>
        <DeleteParameters>
            <asp:Parameter Name="LeaveTypeId" Type="Int32" />
        </DeleteParameters>
    </phpa:PhpaLinqDataSource>
    <jquery:GridViewExInsert ID="gvLeaveTypes" runat="server" AutoGenerateColumns="false"
        Caption="Leave Types" DataSourceID="dsLeaveTypes" DataKeyNames="LeaveTypeId"
        OnRowDataBound="gvLeaveTypes_RowDataBound" AllowSorting="true">
        <RowMenuItems>
            <jquery:RowMenuPostBack PostBackType="Edit" Text="Edit" />
            <jquery:RowMenuPostBack PostBackType="Delete" Text="Delete" />
        </RowMenuItems>
        <Columns>
            <jquery:CommandFieldEx ShowEditButton="false" />
            <eclipse:SequenceField />
            <eclipse:MultiBoundField DataFields="LeaveDescription" HeaderText="Leave Type" SortExpression="LeaveDescription">
                <EditItemTemplate>
                    <i:TextBoxEx ID="tbLeaveDescription" runat="server" Text='<%#Bind("LeaveDescription") %>'
                        FriendlyName="Leave Type">
                        <Validators>
                            <i:Value ValueType="String" />
                            <i:Required />
                        </Validators>
                    </i:TextBoxEx>
                </EditItemTemplate>
            </eclipse:MultiBoundField>
        </Columns>
    </jquery:GridViewExInsert>

</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="cphNoForm" runat="server">
</asp:Content>
