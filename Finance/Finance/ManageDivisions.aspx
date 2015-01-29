<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" CodeBehind="ManageDivisions.aspx.cs"
    Inherits="PhpaAll.Finance.ManageDivisions" Title="Manage Divisions" %>

<%@ Register Src="~/Controls/PrinterFriendlyButton.ascx" TagName="PrinterFriendlyButton"
    TagPrefix="uc2" %>
<asp:Content ID="C1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="C3" ContentPlaceHolderID="cphSideNavigation" runat="server">
    <asp:HyperLink ID="HyperLink2" runat="server" Text="Help" NavigateUrl="~/Doc/ManageDivisions.doc.aspx" />
    <uc2:PrinterFriendlyButton ID="PrinterFriendlyButton1" runat="server" />
</asp:Content>
<asp:Content ID="C4" ContentPlaceHolderID="cph" runat="server">
    <br />
    <i:ButtonEx ID="btnNewDivsion" runat="server" RolesRequired="FinanceManager" Text="Create New Division"
        Action="Submit" Icon="PlusThick" OnClick="btnNew_Click" />
    <phpa:PhpaLinqDataSource ID="dsDivision" runat="server" ContextTypeName="Eclipse.PhpaLibrary.Database.FinanceDataContext"
        TableName="Divisions" RenderLogVisible="False" OrderBy="DivisionName, DivisionGroup"
        EnableDelete="true" EnableInsert="true" EnableUpdate="true">
        <UpdateParameters>
            <asp:Parameter Name="DivisionName" Type="String" />
            <asp:Parameter Name="DivisionCode" Type="String" />
            <asp:Parameter Name="DivisionGroup" Type="String" />
        </UpdateParameters>
        <InsertParameters>
            <asp:Parameter Name="DivisionId" Type="Int32" />
            <asp:Parameter Name="DivisionName" Type="String" />
            <asp:Parameter Name="DivisionCode" Type="String" />
            <asp:Parameter Name="DivisionGroup" Type="String" />
        </InsertParameters>
    </phpa:PhpaLinqDataSource>
    <i:ValidationSummary ID="valSummary" runat="server" />
    <jquery:GridViewExInsert ID="gvDivisions" runat="server" AutoGenerateColumns="False"
        OnRowInserting="gvDivision_RowInserting" OnRowUpdating="gvDivision_RowUpdating"
        OnRowDeleted="gvDivisions_RowDeleted" InsertRowsAtBottom="false" InsertRowsCount="0"
        DataKeyNames="DivisionId, DivisionName" DataSourceID="dsDivision" AllowSorting="True"
        Style="margin-top: 2px" Caption="List of Divisions">
        <Columns>
            <jquery:CommandFieldEx RolesRequired="FinanceManager" ShowDeleteButton="true" DeleteConfirmationText="Division {0} will be deleted. Deletetion will fail if division is in use. Are you sure you want to delete?"
                DataFields="DivisionName">
            </jquery:CommandFieldEx>
            <eclipse:SequenceField />
            <eclipse:MultiBoundField DataFields="DivisionName" HeaderText="Division Name" SortExpression="DivisionName"
                ItemStyle-HorizontalAlign="Left">
                <EditItemTemplate>
                    <i:TextBoxEx runat="server" ID="tbDivisionName" FriendlyName="Division Name" Size="50"
                        MaxLength="70" Text='<%# Bind("DivisionName") %>'>
                        <Validators>
                            <i:Required />
                        </Validators>
                    </i:TextBoxEx>
                </EditItemTemplate>
            </eclipse:MultiBoundField>
            <eclipse:MultiBoundField DataFields="DivisionCode" HeaderText="Division Code" SortExpression="DivisionCode">
                <ItemStyle HorizontalAlign="Left" />
                <EditItemTemplate>
                    <i:TextBoxEx runat="server" ID="tbDivisionCode" Size="10" MaxLength="25" Text='<%# Bind("DivisionCode") %>'>
                    </i:TextBoxEx>
                </EditItemTemplate>
            </eclipse:MultiBoundField>
            <eclipse:MultiBoundField HeaderText="Division Group" DataFields="DivisionGroup" SortExpression="DivisionGroup"
                ItemStyle-HorizontalAlign="Left">
                <EditItemTemplate>
                    <phpa:PhpaLinqDataSource runat="server" ID="dsDivisionGroup" ContextTypeName="Eclipse.PhpaLibrary.Reporting.ReportingDataContext"
                        Visible="True" RenderLogVisible="False" OnSelecting="dsDivisionGroup_Selecting"
                        OnLoad="dsDivisionGroup_Load">
                    </phpa:PhpaLinqDataSource>
                    <i:DropDownListEx runat="server" ID="ddlDivisionGroup" DataSourceID="dsDivisionGroup"
                        ClientIDMode="Static" FriendlyName="Division Group" DataTextField="DivisionGroup"
                        DataValueField="DivisionGroup" Value='<%# Bind("DivisionGroup") %>'>
                        <Items>
                            <eclipse:DropDownItem Text="(New Group)" Persistent="Always" />
                        </Items>
                    </i:DropDownListEx>
                    <i:TextBoxEx ID="tbDivisionGroup" runat="server" MaxLength="35" FriendlyName="New Division Group"
                        ClientIDMode="Static">
                        <Validators>
                            <i:Required DependsOn="ddlDivisionGroup" DependsOnState="Value" DependsOnValue="New" />
                        </Validators>
                    </i:TextBoxEx>
                </EditItemTemplate>
            </eclipse:MultiBoundField>
        </Columns>
        <EmptyDataTemplate>
            No Division exists.
        </EmptyDataTemplate>
    </jquery:GridViewExInsert>
</asp:Content>
