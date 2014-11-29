<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" CodeBehind="PendingAdvance.aspx.cs"
    Inherits="Finance.Reports.PendingAdvance" Title="Divisional Expenses" %>

<asp:Content ID="Content3" ContentPlaceHolderID="cph" runat="server">
    <asp:Label ID="lblReportDescription" runat="server" />
    <br />
    <phpa:PhpaLinqDataSource ID="dsPendingAdv" runat="server" ContextTypeName="Eclipse.PhpaLibrary.Reporting.ReportingDataContext"
        TableName="" RenderLogVisible="false">
    </phpa:PhpaLinqDataSource>
    <br />
    <asp:ListView ID="lvPendingAdv" OnItemCreated="lvPendingAdv_ItemCreated" runat="server"
        OnDataBound="lvPendingAdv_DataBound">
        <LayoutTemplate>
            <div id="itemPlaceholderContainer" runat="server">
                <div id="itemPlaceholder" runat="server" />
                <eclipse:TwoColumnPanel ID="TwoColumnPanel1" runat="server">
                    <eclipse:LeftLabel runat="server" Text="Grand Total" />
                    <asp:Label ID="lblTotal" runat="server" />
                </eclipse:TwoColumnPanel>
                <%--<phpa:LabelLabel runat="server" ID="lblTotal" RightWidth="240px" LabelText="Grand Total   " />--%>
            </div>
        </LayoutTemplate>
        <EmptyDataTemplate>
            No Division exists.
        </EmptyDataTemplate>
        <ItemTemplate>
            <eclipse:TwoColumnPanel runat="server">
                <eclipse:LeftLabel runat="server" Text="<b>Division Group</b>" />
                <asp:Label runat="server" Text='<%#Eval("DivisionGroup") %>' Font-Bold="true" Font-Underline="true" />
            </eclipse:TwoColumnPanel>
            <%--<div style="float: left; min-width: 3in; width: 29%">
            <phpa:LabelLabel runat="server" ID="lblGroup" LeftLabelToolTip="Name of division group" RightLabelText='<%#Eval("DivisionGroup") %>'
                LabelText="Division Group">
            </phpa:LabelLabel>
            </div>--%>
            <div style="float: right; min-width: 5in; width: 70%">
                <jquery:GridViewEx ID="grdPendingAdv" OnRowDataBound="grdPendingAdv_RowDataBound"
                    AutoGenerateColumns="false" ShowFooter="true" runat="server" Width="40em">
                    <Columns>
                        <eclipse:MultiBoundField HeaderText="Divisions" FooterText="Total" DataFields="DivisionName"
                            ToolTipFormatString="Division Code :{0}" ToolTipFields="DivisionCode" />
                        <eclipse:MultiBoundField DataFields="advanceAmount" DataFormatString="{0:N2}" HeaderText="Pending Advance (Nu.)"
                            DataSummaryCalculation="ValueSummation" AccessibleHeaderText="AmountField">
                            <ItemStyle HorizontalAlign="Right" />
                            <FooterStyle HorizontalAlign="Right" />
                        </eclipse:MultiBoundField>
                    </Columns>
                </jquery:GridViewEx>
            </div>
        </ItemTemplate>
        <ItemSeparatorTemplate>
            <hr style="clear: both" />
        </ItemSeparatorTemplate>
    </asp:ListView>
</asp:Content>
