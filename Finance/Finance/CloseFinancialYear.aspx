<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" CodeBehind="CloseFinancialYear.aspx.cs"
    Inherits="Finance.Finance.CloseFinancialYear" EnableEventValidation="true" Title="Close Financial Year" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="C4" ContentPlaceHolderID="cph" runat="server">
    <br />
    <i:ButtonEx ID="btnNewFiscalYear" runat="server" Text="Enter New Fiscal Year" OnClick="btnNewFiscalYear_Click"
        Action="Submit" Icon="PlusThick" />
    <phpa:PhpaLinqDataSource ID="dsFiscalYear" runat="server" ContextTypeName="Eclipse.PhpaLibrary.Database.FiscalDataContext"
        TableName="FinancialYears" RenderLogVisible="False"
        EnableUpdate="true" EnableDelete="true" EnableInsert="true">
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
        InsertRowsAtBottom="false" InsertRowsCount="0"
        DataKeyNames="YearId" DataSourceID="dsFiscalYear" AllowSorting="True" EnableViewState="true"
        Style="margin-top: 2px" Caption="List of Fiscal Years">
        <Columns>
            <jquery:CommandFieldEx ShowDeleteButton="true" DeleteConfirmationText="Do you really want to delete this fiscal year?"
                DataFields="Freeze">
            </jquery:CommandFieldEx>
            <eclipse:SequenceField />
            <eclipse:MultiBoundField DataFields="StartDate" HeaderText="Starts On" SortExpression="StartDate" DataFormatString="{0:d}">
                <EditItemTemplate>
                    <i:TextBoxEx runat="server" ID="tbYearStartDate" Text='<%# Bind("StartDate") %>' FriendlyName="Fiscal year start date">
                        <Validators>
                            <i:Required />
                            <i:Date />
                        </Validators>
                    </i:TextBoxEx>
                </EditItemTemplate>
            </eclipse:MultiBoundField>
            <eclipse:MultiBoundField DataFields="EndDate" HeaderText="Ends On" SortExpression="EndDate" DataFormatString="{0:d}">
                <EditItemTemplate>
                    <i:TextBoxEx runat="server" ID="tbYearEndDate" Text='<%# Bind("EndDate") %>' FriendlyName="Fiscal year end date">
                        <Validators>
                            <i:Required />
                            <i:Date />
                        </Validators>
                    </i:TextBoxEx>
                </EditItemTemplate>
            </eclipse:MultiBoundField>
            <eclipse:MultiBoundField DataFields="Name" HeaderText="Year Name" SortExpression="Name"
                ItemStyle-HorizontalAlign="Left">
                <EditItemTemplate>
                    <i:TextBoxEx runat="server" ID="tbYearName" Text='<%# Bind("Name") %>'>
                        <Validators>
                            <i:Value DependsOn="tbYearStartDate" />
                            <i:Required />
                        </Validators>
                    </i:TextBoxEx>
                </EditItemTemplate>
            </eclipse:MultiBoundField>
            <eclipse:MultiBoundField HeaderText="Close Year" DataFields="Freeze" SortExpression="Freeze"
                ItemStyle-HorizontalAlign="Left">
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
