<%@ Page Language="C#" Title="Banks" CodeBehind="Banks.aspx.cs" Inherits="Finance.PIS.Masters.Banks"
    MasterPageFile="~/MasterPage.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphSideNavigation" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cph" runat="server">
    <p>
        <i:ButtonEx ID="btnInsert" runat="server" Text="Add New Bank" OnClick="btnInsert_Click"
            Action="Submit" Icon="PlusThick" />
        <i:ValidationSummary ID="ValidationSummary1" runat="server" />
        <jquery:StatusPanel ID="Banks_sp" runat="server">
            <Ajax UseDialog="false" />
        </jquery:StatusPanel>
    </p>
    <phpa:PhpaLinqDataSource ID="dsBanks" runat="server" ContextTypeName="Eclipse.PhpaLibrary.Database.PIS.PISDataContext"
        TableName="Banks" RenderLogVisible="false" EnableInsert="true" OnInserted="dsBanks_Inserted"
        EnableUpdate="true" OnUpdated="dsBanks_Updated" OnDeleted="dsBanks_Deleted" EnableDelete="true">
        <InsertParameters>
            <asp:Parameter Name="BankName" Type="String" />
            <asp:Parameter Name="BranchName" Type="String" />
            <asp:Parameter Name="StationId" Type="Int32" ConvertEmptyStringToNull="true" />
        </InsertParameters>
        <UpdateParameters>
            <asp:Parameter Name="BankName" Type="String" />
            <asp:Parameter Name="BranchName" Type="String" />
            <asp:Parameter Name="StationId" Type="Int32" ConvertEmptyStringToNull="true" />
        </UpdateParameters>
        <DeleteParameters>
            <asp:Parameter Name="BankId" Type="Int32" />
        </DeleteParameters>
    </phpa:PhpaLinqDataSource>
    <jquery:GridViewExInsert ID="gvBanks" runat="server" AutoGenerateColumns="false"
        Caption="Banks" DataSourceID="dsBanks" DataKeyNames="BankId" OnRowDataBound="gvBanks_RowDataBound"
        AllowSorting="true">
        <RowMenuItems>
            <jquery:RowMenuPostBack PostBackType="Edit" Text="Edit" />
            <jquery:RowMenuPostBack PostBackType="Delete" Text="Delete" />
        </RowMenuItems>
        <Columns>
            <jquery:CommandFieldEx ShowEditButton="false" />
            <eclipse:SequenceField />
            <eclipse:MultiBoundField DataFields="BankName" HeaderText="Bank Name" SortExpression="BankName">
                <EditItemTemplate>
                    <i:TextBoxEx ID="tbBankName" runat="server" Text='<%#Bind("BankName") %>' FriendlyName="Bank Name">
                        <Validators>
                            <i:Required />
                            <i:Value ValueType="String" />
                        </Validators>
                    </i:TextBoxEx>
                </EditItemTemplate>
            </eclipse:MultiBoundField>
            <eclipse:MultiBoundField DataFields="BranchName" HeaderText="Branch Name" SortExpression="BranchName">
                <EditItemTemplate>
                    <i:TextBoxEx ID="tbBranchName" runat="server" Text='<%#Bind("BranchName") %>' FriendlyName="Branch Name">
                        <Validators>
                            <i:Value ValueType="String" />
                        </Validators>
                    </i:TextBoxEx>
                </EditItemTemplate>
            </eclipse:MultiBoundField>
            <eclipse:MultiBoundField DataFields="Station.StationName" HeaderText="Station Name" SortExpression="Station.StationName">
                <EditItemTemplate>
                    <phpa:PhpaLinqDataSource runat="server" ID="dsStation" ContextTypeName="Eclipse.PhpaLibrary.Database.PIS.PISDataContext"
                        Select="new (StationId, StationName)" TableName="Stations" RenderLogVisible="false">
                    </phpa:PhpaLinqDataSource>
                    <i:DropDownListEx runat="server" ID="ddlStation" DataSourceID="dsStation" DataTextField="StationName"
                        DataValueField="StationId" Value='<%# Bind("StationId") %>' FriendlyName="Station">
                        <Items>
                            <eclipse:DropDownItem Text="Not Set" Value="" Persistent="Always" />
                        </Items>
                        <Validators>
                            <i:Required />
                           <%-- <i:Value ValueType="String" />--%>
                        </Validators>
                    </i:DropDownListEx>
                </EditItemTemplate>
            </eclipse:MultiBoundField>
        </Columns>
    </jquery:GridViewExInsert>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cphNoForm" runat="server">
</asp:Content>
