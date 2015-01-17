<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" CodeBehind="ManageFinancialYears.aspx.cs"
    Inherits="Finance.Finance.ManageFinancialYears" EnableEventValidation="true" Title="Manage Financial Year" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="C4" ContentPlaceHolderID="cph" runat="server">
    <br />
    <i:ButtonEx ID="btnNewFiscalYear" runat="server" Text="Create New Financial Year" OnClick="btnNewFiscalYear_Click"
        Action="Submit" Icon="PlusThick" />
    <phpa:PhpaLinqDataSource ID="dsFiscalYear" runat="server" ContextTypeName="Eclipse.PhpaLibrary.Database.FiscalDataContext"
        TableName="FinancialYears" RenderLogVisible="False" OrderBy="YearId desc"
        EnableUpdate="true" EnableInsert="true">
        <UpdateParameters>
            <asp:Parameter Name="Name" Type="String" />
            <asp:Parameter Name="StartDate" Type="DateTime" />
            <asp:Parameter Name="EndDate" Type="DateTime" />
            <asp:Parameter Name="Freeze" Type="String" />
        </UpdateParameters>
        <InsertParameters>
            <asp:Parameter Name="Name" Type="String" />
            <asp:Parameter Name="StartDate" Type="DateTime" />
            <asp:Parameter Name="EndDate" Type="DateTime" />
            <asp:Parameter Name="Freeze" Type="String" />
        </InsertParameters>
    </phpa:PhpaLinqDataSource>
    <i:ValidationSummary ID="valSummary" runat="server" />
    <jquery:GridViewExInsert ID="gvFiscalYear" runat="server" AutoGenerateColumns="False"
        InsertRowsAtBottom="false" InsertRowsCount="0" OnRowInserting="gvFiscalYear_RowInserting"
        DataKeyNames="YearId" DataSourceID="dsFiscalYear" AllowSorting="True" EnableViewState="true"
        Style="margin-top: 2px" Caption="List of Financial Years">
        <Columns>
            <jquery:CommandFieldEx></jquery:CommandFieldEx>
            <eclipse:SequenceField />
            <eclipse:MultiBoundField DataFields="Name" HeaderText="Financial Year" SortExpression="Name" ItemStyle-HorizontalAlign="Left">
            </eclipse:MultiBoundField>
            <eclipse:MultiBoundField DataFields="StartDate" HeaderText="Starts On" SortExpression="StartDate" DataFormatString="{0:d}">
                <EditItemTemplate>
                    <i:TextBoxEx runat="server" ID="tbFYStartDate" Text='<%# Bind("StartDate", "{0:d}") %>' FriendlyName="Fiscal Year Start Date">
                        <Validators>
                            <i:Required />
                            <i:Date />
                        </Validators>
                    </i:TextBoxEx>
                </EditItemTemplate>
            </eclipse:MultiBoundField>
            <eclipse:MultiBoundField DataFields="EndDate" HeaderText="Ends On" SortExpression="EndDate" DataFormatString="{0:d}">
                <EditItemTemplate>
                    <i:TextBoxEx runat="server" ID="tbFYEndDate" Text='<%# Bind("EndDate", "{0:d}") %>' FriendlyName="Fiscal Year End Date">
                        <Validators>
                            <i:Required />
                            <i:Date DateType="ToDate" AssociatedControlID="tbFYStartDate" MaxRange="365" />
                        </Validators>
                    </i:TextBoxEx>
                </EditItemTemplate>
            </eclipse:MultiBoundField>
            <eclipse:MultiBoundField HeaderText="Closed" DataFields="Freeze" SortExpression="Freeze" ItemStyle-HorizontalAlign="Left">
                <EditItemTemplate>
                    <i:TextBoxEx runat="server" ID="tbFreeze" FriendlyName="CloseYear" Size="1" CaseConversion="UpperCase"
                        MaxLength="1" Text='<%# Bind("Freeze") %>'>
                    </i:TextBoxEx>
                </EditItemTemplate>
            </eclipse:MultiBoundField>
        </Columns>
        <EmptyDataTemplate>
            No Fiscal Year exists.
        </EmptyDataTemplate>
    </jquery:GridViewExInsert>
</asp:Content>
