<%@ Page Title="Packages" Language="C#" MasterPageFile="~/MasterPage.master" CodeBehind="Packages.aspx.cs"
    Inherits="PhpaAll.MIS.Packages" EnableViewState="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cph" runat="server">
    <phpa:PhpaLinqDataSource ID="dsPackage" runat="server" ContextTypeName="Eclipse.PhpaLibrary.Database.MIS.MISDataContext"
        TableName="Packages" RenderLogVisible="false" EnableInsert="true" OnInserted="dsPackage_Inserted"
        EnableUpdate="true" OnUpdated="dsPackage_Updated" OnDeleted="dsPackage_Deleted"
        EnableDelete="true" OrderBy="PackageName">
        <InsertParameters>
            <asp:Parameter Name="PackageId" Type="Int32" />
            <asp:Parameter Name="PackageName" Type="String" />
            <asp:Parameter Name="Description" Type="String" />
        </InsertParameters>
        <UpdateParameters>
            <asp:Parameter Name="PackageId" Type="Int32" />
            <asp:Parameter Name="PackageName" Type="String" />
            <asp:Parameter Name="Description" Type="String" />
        </UpdateParameters>
        <DeleteParameters>
            <asp:Parameter Name="PackageId" Type="Int32" />
        </DeleteParameters>
    </phpa:PhpaLinqDataSource>
    <i:ButtonEx ID="btnInsert" runat="server" Text="New package..." OnClick="btnInsert_Click"
        Action="Submit" Icon="PlusThick" Visible="false" />
    <jquery:GridViewExInsert ID="gvPackage" runat="server" AutoGenerateColumns="false"
        Caption="Packages" DataSourceID="dsPackage" DataKeyNames="PackageId, PackageName"
        OnRowDataBound="gvPackage_RowDataBound">
        <Columns>
            <jquery:CommandFieldEx ShowDeleteButton="true" DeleteConfirmationText="Package {0} will be deleted."
                DataFields="PackageName" />
            <eclipse:SequenceField />
            <eclipse:MultiBoundField DataFields="packagename" HeaderText="Package Name">
                <EditItemTemplate>
                    <i:TextBoxEx ID="tbPackageName" runat="server" Text='<%#Bind("PackageName") %>' FriendlyName="Package Name">
                        <Validators>
                            <i:Required />
                            <i:Value ValueType="String" MaxLength="25" />
                        </Validators>
                    </i:TextBoxEx>
                </EditItemTemplate>
            </eclipse:MultiBoundField>
            <eclipse:MultiBoundField DataFields="Description" HeaderText="Package Description">
                <EditItemTemplate>
                    <i:TextBoxEx ID="tbDescription" runat="server" Text='<%#Bind("Description") %>' FriendlyName="Package Description">
                        <Validators>
                            <i:Value ValueType="String" MaxLength="25" />
                        </Validators>
                    </i:TextBoxEx>
                </EditItemTemplate>
            </eclipse:MultiBoundField>
        </Columns>
    </jquery:GridViewExInsert>
    <i:ValidationSummary runat="server" />
    <jquery:StatusPanel ID="Package_status" runat="server">
        <Ajax UseDialog="false" />
    </jquery:StatusPanel>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cphNoForm" runat="server">
</asp:Content>
