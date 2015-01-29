<%@ Page Title="Countries" Language="C#" MasterPageFile="~/MasterPage.master" CodeBehind="Countries.aspx.cs"
    Inherits="PhpaAll.PIS.Masters.Countries" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphSideNavigation" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cph" runat="server">
    <p>
        <i:ButtonEx ID="btnInsert" runat="server" Text="Add New Country" OnClick="btnInsert_Click"
            Action="Submit" Icon="PlusThick" />
        <i:ValidationSummary ID="ValidationSummary1" runat="server" />
        <jquery:StatusPanel ID="Countries_sp" runat="server">
            <Ajax UseDialog="false" />
        </jquery:StatusPanel>
    </p>
    <phpa:PhpaLinqDataSource ID="dsCountries" runat="server" ContextTypeName="Eclipse.PhpaLibrary.Database.PIS.PISDataContext"
        TableName="Countries" RenderLogVisible="false" EnableInsert="true" OnInserted="dsCountries_Inserted"
        EnableUpdate="true" OnUpdated="dsCountries_Updated" OnDeleted="dsCountries_Deleted"
        EnableDelete="true">
        <InsertParameters>
            <asp:Parameter Name="CountryName" Type="String" />
        </InsertParameters>
        <UpdateParameters>
            <asp:Parameter Name="CountryName" Type="String" />
        </UpdateParameters>
        <DeleteParameters>
            <asp:Parameter Name="CountryId" Type="Int32" />
        </DeleteParameters>
    </phpa:PhpaLinqDataSource>
    <jquery:GridViewExInsert ID="gvCountries" runat="server" AutoGenerateColumns="false"
        AllowSorting="true" Caption="Countries" DataSourceID="dsCountries" DataKeyNames="CountryId"
        OnRowDataBound="gvCountries_RowDataBound">
        <RowMenuItems>
            <jquery:RowMenuPostBack PostBackType="Edit" Text="Edit" />
            <jquery:RowMenuPostBack PostBackType="Delete" Text="Delete" />
        </RowMenuItems>
        <Columns>
            <jquery:CommandFieldEx ShowEditButton="false" />
            <eclipse:SequenceField />
            <eclipse:MultiBoundField DataFields="CountryName" HeaderText="Country Name" SortExpression="CountryName">
                <EditItemTemplate>
                    <i:TextBoxEx ID="tbCountryName" runat="server" Text='<%#Bind("CountryName") %>' FriendlyName="Country Name">
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
