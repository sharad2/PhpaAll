<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" CodeBehind="ManageFinancialYears.aspx.cs"
    Inherits="Finance.Finance.ManageFinancialYears" EnableEventValidation="true" Title="Manage Financial Years" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="C4" ContentPlaceHolderID="cph" runat="server">
    <br />
    <i:ButtonEx ID="btnNewFiscalYear" RolesRequired="FinanceManager" runat="server" Text="Create New Financial Year" OnClick="btnNewFiscalYear_Click"
        Action="Submit" Icon="PlusThick" />
    <phpa:PhpaLinqDataSource ID="dsFiscalYear" runat="server" ContextTypeName="Eclipse.PhpaLibrary.Database.FiscalDataContext"
        TableName="FinancialYears" RenderLogVisible="False" OrderBy="Name desc"
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
        Style="margin-top: 2px" Caption="List of Financial Years" ItemType="Eclipse.PhpaLibrary.Database.FinancialYear">
        <Columns>
            <jquery:CommandFieldEx RolesRequired="FinanceManager"></jquery:CommandFieldEx>
            <eclipse:SequenceField />
            <eclipse:MultiBoundField DataFields="Name" HeaderText="Financial Year" SortExpression="Name" ItemStyle-HorizontalAlign="Left">
            </eclipse:MultiBoundField>
            <eclipse:MultiBoundField DataFields="StartDate" HeaderText="Starts On" SortExpression="StartDate" DataFormatString="{0:d}">
                <EditItemTemplate>
                    <i:TextBoxEx runat="server" ID="tbFYStartDate" Text='<%# Bind("StartDate", "{0:d}") %>' FriendlyName="Financial Year Start Date">
                        <Validators>
                            <i:Required />
                            <i:Date />
                        </Validators>
                    </i:TextBoxEx>
                </EditItemTemplate>
            </eclipse:MultiBoundField>
            <eclipse:MultiBoundField DataFields="EndDate" HeaderText="Ends On" SortExpression="EndDate" DataFormatString="{0:d}">
                <EditItemTemplate>
                    <i:TextBoxEx runat="server" ID="tbFYEndDate" Text='<%# Bind("EndDate", "{0:d}") %>' FriendlyName="Financial Year End Date">
                        <Validators>
                            <i:Required />
                            <i:Date DateType="ToDate" AssociatedControlID="tbFYStartDate" MaxRange="364" />
                        </Validators>
                    </i:TextBoxEx>
                </EditItemTemplate>
            </eclipse:MultiBoundField>
            <eclipse:MultiBoundField HeaderText="Closed" DataFields="Freeze" SortExpression="Freeze" ItemStyle-HorizontalAlign="Left">
                <EditItemTemplate>
                    <i:DropDownListEx2 runat="server" ID="ddlFreeze" 
                        ClientIDMode="Static" FriendlyName="Freeze" Value='<%# Bind("Freeze") %>'>
                        <Items>
                            <eclipse:DropDownItem Text="No" Value="N" Persistent="Always" />
                            <eclipse:DropDownItem Text="Yes" Value="Y" Persistent="Always" />
                        </Items>
                    </i:DropDownListEx2>
                </EditItemTemplate>
            </eclipse:MultiBoundField>
        </Columns>
        <EmptyDataTemplate>
            There is no active financial year defined in the system yet. Nobody will be able to Create, Edit or Delete any Vouchers. Please define an active financial year so that Vouchers can be Created, Edited or Deleted in the active financial year.
        </EmptyDataTemplate>
    </jquery:GridViewExInsert>
</asp:Content>
