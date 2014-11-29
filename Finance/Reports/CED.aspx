<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" CodeBehind="CED.aspx.cs"
    Inherits="Finance.Reports.CED" Title="Central Excise Duty Report" EnableViewState="false" %>

<%@ Register Src="../Controls/PrinterFriendlyButton.ascx" TagName="PrinterFriendlyButton"
    TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphSideNavigation" runat="server">
    <asp:HyperLink ID="hlHelp" runat="server" Text="Help" NavigateUrl="~/Doc/CED.doc.aspx" /><br />
    <uc2:PrinterFriendlyButton ID="PrinterFriendlyButton1" runat="server" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cph" runat="server">
    <phpa:PhpaLinqDataSource ID="dsCED" runat="server" ContextTypeName="Eclipse.PhpaLibrary.Reporting.ReportingDataContext"
        TableName="RoJobs" RenderLogVisible="false">
    </phpa:PhpaLinqDataSource>
    <eclipse:TwoColumnPanel runat="server">
        <eclipse:LeftLabel runat="server" Text="For Date" />
        <i:TextBoxEx ID="tbMonth" runat="server" FriendlyName="Date" Text="0">
            <Validators>
                <i:Date />
                <i:Required />
            </Validators>
        </i:TextBoxEx>
        <br />
        <eclipse:LeftLabel runat="server" Text="Nationality" />
        <i:DropDownListEx ID="ddlNationality" runat="server" DataTextField="DisplayNationality"
            Value='<%# Bind("Nationality") %>' DataValueField="Nationality">
            <Items>
                <eclipse:DropDownItem Text="Bhutan National" Value="BN" />
                <eclipse:DropDownItem Text="Indian National" Value="IN" />
                <eclipse:DropDownItem Text="Third Country" Value="TC" />
            </Items>
        </i:DropDownListEx>
        <br />
        <br />
        <i:ButtonEx ID="btnGo" runat="server" Text="Go" Icon="Refresh" CausesValidation="true"
            Action="Submit" IsDefault="true" />
    </eclipse:TwoColumnPanel>
    <i:ValidationSummary ID="vsMonth" runat="server" />
    <b>
        <asp:Label ID="lblCaption" runat="server" Visible="false" /></b>
    <asp:ListView ID="lvCED" runat="server" OnItemCreated="lvCED_ItemCreated">
        <LayoutTemplate>
            <div id="itemPlaceholderContainer" runat="server">
                <div id="itemPlaceholder" runat="server" />
            </div>
        </LayoutTemplate>
        <ItemTemplate>
            <div>
                <asp:Label ID="lblDivisionName" runat="server" Text='<%# Eval("PackageName")%>' Font-Bold="true"
                    Font-Size="Large" Font-Underline="true" />
            </div>
            <br />
            <jquery:GridViewEx ID="gvCED" runat="server" AutoGenerateColumns="false" GridLines="Both"
                ShowFooter="true" OnRowDataBound="gvCED_RowDataBound">
                <Columns>
                    <eclipse:SequenceField FooterText="">
                    </eclipse:SequenceField>
                    <eclipse:MultiBoundField DataFields="HeadOfAccount" HeaderText="Head of Account"
                        FooterText="Total" />
                    <eclipse:HyperLinkFieldEx DataTextField="DuringMonth" HeaderText="During the Month"
                        AccessibleHeaderText="DuringMonth" DataTextFormatString="{0:C2}" DataSummaryCalculation="ValueSummation"
                        DataNavigateUrlFormatString="~/Finance/VoucherSearch.aspx?&HeadofAccountId={0}"
                        DataNavigateUrlFields="HeadOfAccountId">
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle HorizontalAlign="Right" />
                        <FooterStyle HorizontalAlign="Right" />
                    </eclipse:HyperLinkFieldEx>
                    <eclipse:HyperLinkFieldEx DataTextField="DuringYear" HeaderText="During the Year"
                        AccessibleHeaderText="DuringYear" DataTextFormatString="{0:C2}" DataNavigateUrlFormatString="~/Finance/VoucherSearch.aspx?&HeadofAccountId={0}"
                        DataNavigateUrlFields="HeadOfAccountId" DataSummaryCalculation="ValueSummation">
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle HorizontalAlign="Right" />
                        <FooterStyle HorizontalAlign="Right" />
                    </eclipse:HyperLinkFieldEx>
                    <eclipse:MultiBoundField DataFields="Cumulative" HeaderText="Total Cumulative" AccessibleHeaderText="Cumulative"
                        HeaderToolTip="Till Date Central Excise Duty" DataFormatString="{0:C2}" DataSummaryCalculation="ValueSummation">
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle HorizontalAlign="Right" />
                        <FooterStyle HorizontalAlign="Right" />
                    </eclipse:MultiBoundField>
                </Columns>
                <EmptyDataTemplate>
                    <asp:Label ID="lblEmpty" runat="server" Font-Bold="true" Font-Size="Larger">
                    No Data found in this Package.
                    </asp:Label>
                </EmptyDataTemplate>
            </jquery:GridViewEx>
        </ItemTemplate>
        <ItemSeparatorTemplate>
            <hr />
        </ItemSeparatorTemplate>
    </asp:ListView>
</asp:Content>
