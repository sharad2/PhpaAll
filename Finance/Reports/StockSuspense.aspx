<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" CodeBehind="StockSuspense.aspx.cs"
    Inherits="Finance.Reports.StockSuspense" Title="Stock Suspense Statement" EnableViewState="false" %>

<%@ Register Src="../Controls/PrinterFriendlyButton.ascx" TagName="PrinterFriendlyButton"
    TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphSideNavigation" runat="server">
    <asp:HyperLink ID="HyperLink2" runat="server" Text="Help" NavigateUrl="~/Doc/StockSuspense.doc.aspx" /><br />
    <uc2:PrinterFriendlyButton ID="PrinterFriendlyButton1" runat="server" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cph" runat="server">
    <div style="float: left">
        Displays Stock details of all the stocks that are under suspense.
        <eclipse:TwoColumnPanel runat="server">
            <eclipse:LeftLabel runat="server" Text="Enter The Month for which Stock Details to be Shown:" />
            <i:TextBoxEx ID="tbdate" runat="server" FriendlyName="Date">
                <Validators>
                    <i:Date />
                    <i:Required />
                </Validators>
            </i:TextBoxEx>
        </eclipse:TwoColumnPanel>
        <i:ValidationSummary ID="ValidationSummary1" runat="server" />
        <i:ButtonEx ID="Button1" runat="server" Text="Stock Details" Icon="Refresh" Action="Submit"
            CausesValidation="true" IsDefault="true" />
        <br />
        <asp:Label ID="lblnodata" runat="server" Text="Stock Details upto selected Month and Year is not available"
            Visible="false"></asp:Label>
        <phpa:PhpaLinqDataSource ID="dsStockSuspense" runat="server" ContextTypeName="Eclipse.PhpaLibrary.Reporting.ReportingDataContext"
            TableName="" RenderLogVisible="false">
        </phpa:PhpaLinqDataSource>
        <br />
        <asp:Label ID="lblopbal" runat="server" Text="<b>Opening Balance of Stocks in Suspense</b>"></asp:Label>
        <jquery:GridViewEx ID="gvopeningBalance" runat="server" AutoGenerateColumns="false"
            ShowFooter="true" Width="40em">
            <Columns>
                <eclipse:MultiBoundField DataFields="Description" HeaderText="Item" HeaderToolTip="Items Included: Electrical, Civil, Machanical, Office Equipment, Rest House Furniture, Pool A/c, Explosive & Cement"
                    ToolTipFields="Head" ToolTipFormatString="Head:{0}">
                </eclipse:MultiBoundField>
                <eclipse:MultiBoundField DataFields="Receipt" HeaderText="Receipt" DataSummaryCalculation="ValueSummation"
                    DataFormatString="{0:N2}">
                    <ItemStyle HorizontalAlign="Right" />
                    <FooterStyle HorizontalAlign="Right" />
                </eclipse:MultiBoundField>
                <eclipse:MultiBoundField DataFields="Issue" HeaderText="Issue" DataSummaryCalculation="ValueSummation"
                    DataFormatString="{0:N2}">
                    <ItemStyle HorizontalAlign="Right" />
                    <FooterStyle HorizontalAlign="Right" />
                </eclipse:MultiBoundField>
            </Columns>
        </jquery:GridViewEx>
        <asp:Repeater ID="repdsStockSuspense" runat="server" Visible="true" OnItemDataBound="repdsStockSuspense_ItemDataBound">
            <ItemTemplate>
                <b>
                    <asp:Label ID="Label1" runat="server" Text="Label" ToolTip="Month/Year" /></b>
                <br />
                <%--Month:<%# Eval("Month") %>--%>
                <jquery:GridViewEx ID="gvrepdsStockSuspense" runat="server" AutoGenerateColumns="false"
                    ShowFooter="true" Width="40em">
                    <Columns>
                        <%--<asp:BoundField DataField="Month" HeaderText="Month" />--%>
                        <eclipse:MultiBoundField DataFields="Description" HeaderToolTip="Items Included: Electrical, Civil, Machanical, Office Equipment, Rest House Furniture, Pool A/c, Explosive & Cement"
                            HeaderText="Item" ToolTipFields="Head" ToolTipFormatString="Head:{0}" />
                        <eclipse:MultiBoundField DataFields="Receipt" HeaderText="Receipt" DataSummaryCalculation="ValueSummation"
                            DataFormatString="{0:N2}">
                            <ItemStyle HorizontalAlign="Right" />
                            <FooterStyle HorizontalAlign="Right" />
                        </eclipse:MultiBoundField>
                        <eclipse:MultiBoundField DataFields="Issue" HeaderText="Issue" DataSummaryCalculation="ValueSummation"
                            DataFormatString="{0:N2}">
                            <ItemStyle HorizontalAlign="Right" />
                            <FooterStyle HorizontalAlign="Right" />
                        </eclipse:MultiBoundField>
                    </Columns>
                </jquery:GridViewEx>
            </ItemTemplate>
            <SeparatorTemplate>
                <hr />
            </SeparatorTemplate>
        </asp:Repeater>
    </div>
</asp:Content>
