<%@ Page Title="Training Types" Language="C#" MasterPageFile="~/MasterPage.master"
    CodeBehind="TrainingTypes.aspx.cs" Inherits="PhpaAll.PIS.Masters.TrainingTypes" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphSideNavigation" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cph" runat="server">
    <p>
        <i:ButtonEx ID="btnInsert" runat="server" Text="Add New Training Type" OnClick="btnInsert_Click"
            Action="Submit" Icon="PlusThick" />
        <i:ValidationSummary ID="ValidationSummary1" runat="server" />
        <jquery:StatusPanel ID="TrainingType_sp" runat="server">
            <Ajax UseDialog="false" />
        </jquery:StatusPanel>
    </p>
    <phpa:PhpaLinqDataSource ID="dsTrainingType" runat="server" ContextTypeName="Eclipse.PhpaLibrary.Database.PIS.PISDataContext"
        TableName="TrainingTypes" RenderLogVisible="false" EnableInsert="true" OnInserted="dsTrainingType_Inserted"
        EnableUpdate="true" OnUpdated="dsTrainingType_Updated" OnDeleted="dsTrainingType_Deleted"
        EnableDelete="true">
        <InsertParameters>
            <asp:Parameter Name="TrainingDescription" Type="String" />
        </InsertParameters>
        <UpdateParameters>
            <asp:Parameter Name="TrainingDescription" Type="String" />
        </UpdateParameters>
        <DeleteParameters>
            <asp:Parameter Name="TrainingTypeId" Type="Int32" />
        </DeleteParameters>
    </phpa:PhpaLinqDataSource>
    <jquery:GridViewExInsert ID="gvTrainingType" runat="server" AutoGenerateColumns="false"
        Caption="Training Types" DataSourceID="dsTrainingType" DataKeyNames="TrainingTypeId"
        OnRowDataBound="gvTrainingType_RowDataBound" AllowSorting="true">
        <RowMenuItems>
            <jquery:RowMenuPostBack PostBackType="Edit" Text="Edit" />
            <jquery:RowMenuPostBack PostBackType="Delete" Text="Delete" />
        </RowMenuItems>
        <Columns>
            <jquery:CommandFieldEx ShowEditButton="false" />
            <eclipse:SequenceField />
            <eclipse:MultiBoundField DataFields="TrainingDescription" HeaderText="Training Description"
                SortExpression="TrainingDescription">
                <EditItemTemplate>
                    <i:TextBoxEx ID="tbTrainingDescription" runat="server" Text='<%#Bind("TrainingDescription") %>'
                        FriendlyName="Training Description">
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
