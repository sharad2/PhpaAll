<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" CodeBehind="BankBook.aspx.cs"
    Inherits="Finance.BankBook" Title="Bank Book" EnableViewState="false" %>

<%@ Register Src="~/Controls/VoucherDetailControl.ascx" TagName="VoucherDetailControl"
    TagPrefix="uc1" %>
<%@ Register Src="~/Controls/PrinterFriendlyButton.ascx" TagName="PrinterFriendlyButton"
    TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphSideNavigation" runat="server">
    <asp:HyperLink ID="HyperLink2" runat="server" Text="Help" NavigateUrl="~/Doc/BankBook.doc.aspx" /><br />
    <uc2:PrinterFriendlyButton ID="PrinterFriendlyButton1" runat="server" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cph" runat="server">
    <eclipse:TwoColumnPanel runat="server">
        <eclipse:LeftLabel runat="server" Text="Date Range" />
        <i:TextBoxEx runat="server" ID="tbDateFrom" FriendlyName="From Date" Text="-30">
            <Validators>
                <i:Required />
                <i:Date />
            </Validators>
        </i:TextBoxEx>
        <i:TextBoxEx runat="server" ID="tbDateTo" FriendlyName="To Date" Text="0">
            <Validators>
                <i:Required />
                <i:Date DateType="ToDate" />
            </Validators>
        </i:TextBoxEx>
        <eclipse:LeftLabel runat="server" Text="Head of Account" />
        <phpa:PhpaLinqDataSource runat="server" ID="dsHeads" ContextTypeName="Eclipse.PhpaLibrary.Reporting.ReportingDataContext"
            OrderBy="SortableName" Select="new (DisplayDescription, RoAccountType, HeadOfAccountId)"
            RenderLogVisible="false" TableName="RoHeadHierarchies" Visible="True" Where="HeadOfAccountType == @HeadOfAccountType || HeadOfAccountType == @HeadOfAccountType2">
            <WhereParameters>
                <asp:Parameter DefaultValue="BANKNU" Name="HeadOfAccountType" Type="String" />
                <asp:Parameter DefaultValue="BANKFE" Name="HeadOfAccountType2" Type="String" />
            </WhereParameters>
        </phpa:PhpaLinqDataSource>
        <i:DropDownListEx ID="ddlHeads" runat="server" DataTextField="DisplayDescription"
            DataValueField="HeadOfAccountId" DataSourceID="dsHeads" DataOptionGroupField="RoAccountType.Description">
        </i:DropDownListEx>
        <br />
        <i:ButtonEx ID="btnShowReport" runat="server" Text="Go" CausesValidation="true" Action="Submit" />
    </eclipse:TwoColumnPanel>
    <i:ValidationSummary ID="ValidationSummary1" runat="server" />
    <div style="text-align: center">
        <asp:Label ID="lblHeadOfAccount" CssClass="headerStyle" runat="server" />
    </div>
    <hr />
    <uc1:VoucherDetailControl ID="ctlVoucherDetail" DebitHeader="Payments" CreditHeader="Receipts"
        runat="server" InformationLevel="Detail">
    </uc1:VoucherDetailControl>
</asp:Content>
