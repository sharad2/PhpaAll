<%@ Page Title="Promotion Types" Language="C#" MasterPageFile="~/MasterPage.master"
    CodeBehind="PromotionTypes.aspx.cs" Inherits="PhpaAll.PIS.Masters.PromotionTypes" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphSideNavigation" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cph" runat="server">
    <p>
        <i:ButtonEx ID="btnInsert" runat="server" Text="Add New Promotion" OnClick="btnInsert_Click"
            Action="Submit" Icon="PlusThick" />
        <i:ValidationSummary ID="ValidationSummary1" runat="server" />
        <jquery:StatusPanel ID="PromotionTypes_sp" runat="server">
            <Ajax UseDialog="false" />
        </jquery:StatusPanel>
    </p>
    <phpa:PhpaLinqDataSource ID="dsPromotionTypes" runat="server" ContextTypeName="Eclipse.PhpaLibrary.Database.PIS.PISDataContext"
        TableName="PromotionTypes" RenderLogVisible="false" EnableInsert="true" OnInserted="dsPromotionTypes_Inserted"
        EnableUpdate="true" OnUpdated="dsPromotionTypes_Updated" OnDeleted="dsPromotionTypes_Deleted"
        EnableDelete="true">
        <InsertParameters>
            <asp:Parameter Name="PromotionDescription" Type="String" />
        </InsertParameters>
        <UpdateParameters>
            <asp:Parameter Name="PromotionDescription" Type="String" />
        </UpdateParameters>
        <DeleteParameters>
            <asp:Parameter Name="PromotionTypeId" Type="Int32" />
        </DeleteParameters>
    </phpa:PhpaLinqDataSource>
    <jquery:GridViewExInsert ID="gvPromotionTypes" runat="server" AutoGenerateColumns="false"
        Caption="Promotion Types" DataSourceID="dsPromotionTypes" DataKeyNames="PromotionTypeId"
        OnRowDataBound="gvPromotionTypes_RowDataBound" AllowSorting="true">
        <RowMenuItems>
            <jquery:RowMenuPostBack PostBackType="Edit" Text="Edit" />
            <jquery:RowMenuPostBack PostBackType="Delete" Text="Delete" />
        </RowMenuItems>
        <Columns>
            <jquery:CommandFieldEx ShowEditButton="false" />
            <eclipse:SequenceField />
            <eclipse:MultiBoundField DataFields="PromotionDescription" HeaderText="Promotion Description"
                SortExpression="PromotionDescription">
                <EditItemTemplate>
                    <i:TextBoxEx ID="tbPromotionDescription" runat="server" Text='<%#Bind("PromotionDescription") %>'
                        FriendlyName="Promotion Description">
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
