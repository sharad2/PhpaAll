<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" CodeBehind="PartyAdvances.aspx.cs"
    Inherits="Finance.Reports.PartyAdvances" EnableViewState="false" %>

<%@ Register Src="../Controls/PrinterFriendlyButton.ascx" TagName="PrinterFriendlyButton"
    TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server" EnableViewState="false">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphSideNavigation" runat="server">
    <asp:HyperLink ID="HyperLink2" runat="server" Text="Help" NavigateUrl="~/Doc/PartyAdvances.doc.aspx" /><br />
    <uc2:PrinterFriendlyButton ID="PrinterFriendlyButton1" runat="server" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cph" runat="server">
    <eclipse:TwoColumnPanel runat="server">
        <eclipse:LeftLabel runat="server" Text="Date" />
        <i:TextBoxEx ID="tbDate" runat="server" FriendlyName="From Date">
            <Validators>
                <i:Date />
                <i:Required />
            </Validators>
        </i:TextBoxEx>
        <br />
        <br />
        <i:ButtonEx ID="btnShowPartyAdvances" runat="server" Action="Submit" Icon="Search"
            IsDefault="true" Text="Show Party Advances" OnClick="btnShowPartyAdvances_Click"
            CausesValidation="true" />
    </eclipse:TwoColumnPanel>
    <i:ValidationSummary runat="server" />
    <phpa:PhpaLinqDataSource ID="dsPartyAdv" runat="server" ContextTypeName="Eclipse.PhpaLibrary.Reporting.ReportingDataContext"
        TableName="RoContractors" RenderLogVisible="false" OnSelecting="dsPartyAdv_Selecting">
    </phpa:PhpaLinqDataSource>
    <jquery:GridViewEx ID="grdPartyAdv" AutoGenerateColumns="False" ShowFooter="True"
        runat="server" AllowSorting="True" DataSourceID="dsPartyAdv" EnableViewState="false">
        <EmptyDataTemplate>
            No advances taken by any party in the specified date.
        </EmptyDataTemplate>
        <Columns>
            <eclipse:SequenceField>
            </eclipse:SequenceField>
            <eclipse:MultiBoundField HeaderText="Contractor|Code" DataFields="PartyCode" />
            <eclipse:MultiBoundField HeaderText="Contractor|Name" DataFields="PartyName" />

            <eclipse:HyperLinkFieldEx runat="server" DataTextField="Advance" DataTextFormatString="{0:C}" HeaderText="Advance Amount(Nu.)"
                DataNavigateUrlFields="AdvanceAccountTypes,PartyId" DataNavigateUrlFormatString="~/Finance/VoucherSearch.aspx?AccountTypes={0}&ContractorId={1}"
                DataSummaryCalculation="ValueSummation" DataFooterFormatString="{0:C}">
                <ItemStyle HorizontalAlign="Right" />
            </eclipse:HyperLinkFieldEx>
            
<%--            <asp:TemplateField HeaderText="Advance Amount(Nu.)_o" AccessibleHeaderText="advance">
                <ItemStyle HorizontalAlign="Right" />
                <ItemTemplate>
                    <asp:HyperLink ID="hlAdvanceAmount" runat="server" Text='<%# Eval("Advance","{0:C}") %>' />
                </ItemTemplate>
            </asp:TemplateField>--%>

            <eclipse:HyperLinkFieldEx runat="server" DataTextField="MaterialAdvance" DataTextFormatString="{0:C}" HeaderText="Material/ Tools & Plant Advance(Nu.)"
                DataNavigateUrlFields="MaterialAdvanceAccountType,PartyId" DataNavigateUrlFormatString="~/Finance/VoucherSearch.aspx?AccountTypes={0}&ContractorId={1}"
                DataSummaryCalculation="ValueSummation" DataFooterFormatString="{0:C}">
                <ItemStyle HorizontalAlign="Right" />
            </eclipse:HyperLinkFieldEx>

<%--            <asp:TemplateField HeaderText="Material/ Tools & Plant Advance(Nu.)_o" AccessibleHeaderText="MaterialAdvance">
                <ItemStyle HorizontalAlign="Right" />
                <ItemTemplate>
                    <asp:HyperLink ID="hlMaterialAdvance" runat="server" Text='<%# Eval("MaterialAdvance","{0:C}") %>' />
                </ItemTemplate>
            </asp:TemplateField>--%>
            <eclipse:MultiBoundField HeaderText="Total Advance (Nu.)" HeaderToolTip="Advance Amount+Material Advance"
                DataFields="TotalAdvance" DataSummaryCalculation="ValueSummation" DataFormatString="{0:C2}">
                <ItemStyle HorizontalAlign="Right" />
                <HeaderStyle HorizontalAlign="Center" />
                <FooterStyle HorizontalAlign="Right" />
            </eclipse:MultiBoundField>
        </Columns>
        <FooterStyle HorizontalAlign="Right" />
    </jquery:GridViewEx>
</asp:Content>
