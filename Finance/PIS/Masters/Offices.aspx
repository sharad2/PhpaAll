<%@ Page Title="Offices" Language="C#" MasterPageFile="~/MasterPage.master" CodeBehind="Offices.aspx.cs"
    Inherits="Finance.PIS.Masters.Offices" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphSideNavigation" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cph" runat="server">
    <p>
        <i:ButtonEx ID="btnInsert" runat="server" Text="Add New Office" OnClick="btnInsert_Click"
            Action="Submit" Icon="PlusThick" />
        <i:ValidationSummary ID="ValidationSummary1" runat="server" />
        <jquery:StatusPanel ID="Offices_sp" runat="server">
            <Ajax UseDialog="false" />
        </jquery:StatusPanel>
    </p>
    <phpa:PhpaLinqDataSource ID="dsOffices" runat="server" ContextTypeName="Eclipse.PhpaLibrary.Database.PIS.PISDataContext"
        TableName="Offices" RenderLogVisible="false" EnableInsert="true" OnInserted="dsOffices_Inserted"
        EnableUpdate="true" OnUpdated="dsOffices_Updated" OnDeleted="dsOffices_Deleted"
        EnableDelete="true" OnContextCreated="dsOffices_ContextCreated">
        <InsertParameters>
            <asp:Parameter Name="OfficeName" Type="String" />
            <asp:Parameter Name="SubDivisionId" Type="Int32" />
        </InsertParameters>
        <UpdateParameters>
            <asp:Parameter Name="OfficeName" Type="String" />
            <asp:Parameter Name="SubDivisionId" Type="Int32" />
        </UpdateParameters>
        <DeleteParameters>
            <asp:Parameter Name="OfficeId" Type="Int32" />
        </DeleteParameters>
    </phpa:PhpaLinqDataSource>
    <jquery:GridViewExInsert ID="gvOffices" runat="server" AutoGenerateColumns="false"
        Caption="Offices" DataSourceID="dsOffices" DataKeyNames="OfficeId" OnRowDataBound="gvOffices_RowDataBound"
        OnRowInserting="gvOffices_RowInserting" OnRowUpdating="gvOffices_RowUpdating"
        OnRowInserted="gvOffices_RowInserted" AllowSorting="true">
        <RowMenuItems>
            <jquery:RowMenuPostBack PostBackType="Edit" Text="Edit" />
            <jquery:RowMenuPostBack PostBackType="Delete" Text="Delete" />
        </RowMenuItems>
        <Columns>
            <jquery:CommandFieldEx ShowEditButton="false" />
            <eclipse:SequenceField />
            <eclipse:MultiBoundField DataFields="OfficeName" HeaderText="Office Name" SortExpression="OfficeName">
                <EditItemTemplate>
                    <i:TextBoxEx ID="tbOfficeName" runat="server" Text='<%#Bind("OfficeName") %>' FriendlyName="Office Name">
                        <Validators>
                            <i:Required />
                            <i:Value ValueType="String" />
                        </Validators>
                    </i:TextBoxEx>
                </EditItemTemplate>
            </eclipse:MultiBoundField>
            <eclipse:MultiBoundField DataFields="SubDivision.SubDivisionName" HeaderText="Sub Division"
                SortExpression="SubDivision.SubDivisionName">
                <EditItemTemplate>
                    <phpa:PhpaLinqDataSource runat="server" ID="dsDivision" ContextTypeName="Eclipse.PhpaLibrary.Database.PIS.PISDataContext"
                        Select="new (DivisionId, DivisionName, DivisionGroup)" TableName="Divisions"
                        RenderLogVisible="false">
                    </phpa:PhpaLinqDataSource>
                    Division:
                    <i:DropDownListEx runat="server" ID="ddlDivision" DataSourceID="dsDivision" DataTextField="DivisionName"
                        DataValueField="DivisionId" FriendlyName="Division" DataOptionGroupField="DivisionGroup">
                    </i:DropDownListEx>
                    <br />
                    Sub Division:
                    <i:DropDownListEx runat="server" ID="ddlSubDivision" FriendlyName="Sub Division"
                        QueryString="SubDivisionId" Value='<%# Bind("SubDivision.SubDivisionId") %>'>
                        <Cascadable CascadeParentId="ddlDivision" InitializeAtStartup="true" WebMethod="GetSubDivisions" />
                        <Items>
                            <eclipse:DropDownItem Text="(Not Set)" Value="" Persistent="WhenEmpty" />
                        </Items>
                    </i:DropDownListEx>
                </EditItemTemplate>
            </eclipse:MultiBoundField>
        </Columns>
    </jquery:GridViewExInsert>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cphNoForm" runat="server">
</asp:Content>
