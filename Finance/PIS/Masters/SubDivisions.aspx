<%@ Page Title="Sub Divisions" Language="C#" MasterPageFile="~/MasterPage.master"
    CodeBehind="SubDivisions.aspx.cs" Inherits="Finance.PIS.Masters.SubDivisions" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphSideNavigation" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cph" runat="server">
    <p>
        <i:ButtonEx ID="btnInsert" runat="server" Text="Add New Sub Division" OnClick="btnInsert_Click"
            Action="Submit" Icon="PlusThick" />
        <i:ValidationSummary ID="ValidationSummary1" runat="server" />
        <jquery:StatusPanel ID="SubDivisions_sp" runat="server">
            <Ajax UseDialog="false" />
        </jquery:StatusPanel>
    </p>
    <phpa:PhpaLinqDataSource ID="dsSubDivisions" runat="server" ContextTypeName="Eclipse.PhpaLibrary.Database.PIS.PISDataContext"
        TableName="SubDivisions" RenderLogVisible="false" EnableInsert="true" OnInserted="dsSubDivisions_Inserted"
        EnableUpdate="true" OnUpdated="dsSubDivisions_Updated" OnDeleted="dsSubDivisions_Deleted"
        EnableDelete="true" OnContextCreated="dsSubDivisions_ContextCreated">
        <InsertParameters>
            <asp:Parameter Name="SubDivisionName" Type="String" />
            <asp:Parameter Name="DivisionId" Type="Int32" />
        </InsertParameters>
        <UpdateParameters>
            <asp:Parameter Name="SubDivisionName" Type="String" />
            <asp:Parameter Name="DivisionId" Type="Int32" />
        </UpdateParameters>
        <DeleteParameters>
            <asp:Parameter Name="OfficeId" Type="Int32" />
        </DeleteParameters>
    </phpa:PhpaLinqDataSource>
    <jquery:GridViewExInsert ID="gvSubDivisions" runat="server" AutoGenerateColumns="false"
        Caption="Sub Divisions" DataSourceID="dsSubDivisions" DataKeyNames="SubDivisionId"
        OnRowDataBound="gvSubDivisions_RowDataBound" AllowSorting="true">
        <RowMenuItems>
            <jquery:RowMenuPostBack PostBackType="Edit" Text="Edit" />
            <jquery:RowMenuPostBack PostBackType="Delete" Text="Delete" />
        </RowMenuItems>
        <Columns>
            <jquery:CommandFieldEx ShowEditButton="false" />
            <eclipse:SequenceField />
            <eclipse:MultiBoundField DataFields="SubDivisionName" HeaderText="Sub Division Name"
                SortExpression="SubDivisionName">
                <EditItemTemplate>
                    <i:TextBoxEx ID="tbSubDivisionName" runat="server" Text='<%#Bind("SubDivisionName") %>'
                        FriendlyName="Sub Division Name">
                        <Validators>
                            <i:Required />
                            <i:Value ValueType="String" />
                        </Validators>
                    </i:TextBoxEx>
                </EditItemTemplate>
            </eclipse:MultiBoundField>
            <eclipse:MultiBoundField DataFields="Division.DivisionName" HeaderText="Division"
                SortExpression="Division.DivisionName">
                <EditItemTemplate>
                    <phpa:PhpaLinqDataSource runat="server" ID="dsDivision" ContextTypeName="Eclipse.PhpaLibrary.Database.PIS.PISDataContext"
                        Select="new (DivisionId, DivisionName, DivisionGroup)" TableName="Divisions"
                        RenderLogVisible="false">
                    </phpa:PhpaLinqDataSource>
                    <i:DropDownListEx runat="server" ID="ddlDivision" DataSourceID="dsDivision" DataTextField="DivisionName"
                        DataValueField="DivisionId" Value='<%# Bind("DivisionId") %>' FriendlyName="Division"
                        DataOptionGroupField="DivisionGroup">
                    </i:DropDownListEx>
                </EditItemTemplate>
            </eclipse:MultiBoundField>
        </Columns>
    </jquery:GridViewExInsert>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cphNoForm" runat="server">
</asp:Content>
