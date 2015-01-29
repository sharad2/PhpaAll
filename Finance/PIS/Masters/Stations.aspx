<%@ Page Language="C#" Title="Stations" MasterPageFile="~/MasterPage.master" CodeBehind="Stations.aspx.cs" Inherits="PhpaAll.PIS.Masters.Stations" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphSideNavigation" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cph" runat="server">
    <p>
        <i:ButtonEx ID="btnInsert" runat="server" Text="Add New Station" OnClick="btnInsert_Click"
            Action="Submit" Icon="PlusThick" />
        <i:ValidationSummary ID="ValidationSummary1" runat="server" />
        <jquery:StatusPanel ID="Stations_sp" runat="server">
            <Ajax UseDialog="false" />
        </jquery:StatusPanel>
    </p>
    <phpa:PhpaLinqDataSource ID="dsStations" runat="server" ContextTypeName="Eclipse.PhpaLibrary.Database.PIS.PISDataContext"
        TableName="Stations" RenderLogVisible="false" EnableInsert="true" OnInserted="dsStations_Inserted"
        EnableUpdate="true" OnUpdated="dsStations_Updated" OnDeleted="dsStations_Deleted"
        EnableDelete="true">
        <InsertParameters>
            <asp:Parameter Name="StationName" Type="String" />
        </InsertParameters>
        <UpdateParameters>
            <asp:Parameter Name="StationName" Type="String" />
        </UpdateParameters>
        <DeleteParameters>
            <asp:Parameter Name="StationId" Type="Int32" />
        </DeleteParameters>
    </phpa:PhpaLinqDataSource>
    <jquery:GridViewExInsert ID="gvStations" runat="server" AutoGenerateColumns="false"
        AllowSorting="true" Caption="Stations" DataSourceID="dsStations" DataKeyNames="StationId"
        OnRowDataBound="gvStations_RowDataBound">
        <RowMenuItems>
            <jquery:RowMenuPostBack PostBackType="Edit" Text="Edit" />
            <jquery:RowMenuPostBack PostBackType="Delete" Text="Delete" />
        </RowMenuItems>
        <Columns>
            <jquery:CommandFieldEx ShowEditButton="false" />
            <eclipse:SequenceField />
            <eclipse:MultiBoundField DataFields="StationName" HeaderText="Station Name" SortExpression="StationName">
                <EditItemTemplate>
                    <i:TextBoxEx ID="tbStationName" runat="server" Text='<%#Bind("StationName") %>' FriendlyName="Station Name">
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

