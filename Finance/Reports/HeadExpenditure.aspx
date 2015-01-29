<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" CodeBehind="HeadExpenditure.aspx.cs"
    Inherits="PhpaAll.Reports.HeadExpenditure" Title="Yearly Heads Expenditure" %>

<%@ Register Src="../Controls/PrinterFriendlyButton.ascx" TagName="PrinterFriendlyButton"
    TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphSideNavigation" runat="server">
    <uc2:PrinterFriendlyButton ID="PrinterFriendlyButton1" runat="server" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cph" runat="server">
    <eclipse:TwoColumnPanel runat="server">
        <eclipse:LeftLabel runat="server" Text="Year" />
        <i:TextBoxEx ID="tbDate" runat="server" FriendlyName="Date">
            <Validators>
                <i:Date Max="0" />
                <i:Required />
            </Validators>
        </i:TextBoxEx>
        <br />
        <br />
        <i:ButtonEx ID="btnShow" runat="server" Text="Show Report" Icon="Refresh" Action="Submit"
            OnClick="btnShow_Click" CausesValidation="true" IsDefault="true" />
        <br />
        <br />
    </eclipse:TwoColumnPanel>
    <i:ValidationSummary runat="server" />
    <phpa:PhpaLinqDataSource ID="dsHeadsExp" runat="server" RenderLogVisible="false"
        ContextTypeName="Eclipse.PhpaLibrary.Reporting.ReportingDataContext">
    </phpa:PhpaLinqDataSource>
    <jquery:GridViewEx ID="gvHeadsExp" runat="server" AutoGenerateColumns="false" ShowFooter="true"
        OnRowDataBound="gvHeadsExp_RowDataBound">
        <Columns>
            <eclipse:SequenceField FooterText="">
            </eclipse:SequenceField>
            <eclipse:MultiBoundField DataFields="Head" HeaderText="Head" FooterText="Total" ItemStyle-Width="2in" />
            <eclipse:MultiBoundField DataFields="Amount" DataFormatString="{0:N2}" HeaderText="Expenditure"
                DataSummaryCalculation="ValueSummation" HeaderStyle-Width="3in">
                <ItemStyle HorizontalAlign="Right" />
                <FooterStyle HorizontalAlign="Right" />
            </eclipse:MultiBoundField>
        </Columns>
        <EmptyDataTemplate>
            <br />
            <b>No data found for given year.</b>
        </EmptyDataTemplate>
    </jquery:GridViewEx>
     <jquery:GridViewEx ID="gvHeadsExpTillDate" runat="server" AutoGenerateColumns="false" ShowFooter="true"
        OnRowDataBound="gvHeadsExpTillDate_RowDataBound">
        <Columns>
            <eclipse:SequenceField FooterText="">
            </eclipse:SequenceField>
            <eclipse:MultiBoundField DataFields="Head" HeaderText="Head" FooterText="Total" ItemStyle-Width="2in" />
            <eclipse:MultiBoundField DataFields="Amount" DataFormatString="{0:N2}" HeaderText="Expenditure"
                DataSummaryCalculation="ValueSummation" HeaderStyle-Width="3in">
                <ItemStyle HorizontalAlign="Right" />
                <FooterStyle HorizontalAlign="Right" />
            </eclipse:MultiBoundField>
        </Columns>
        <EmptyDataTemplate>
            <br />
            <b>No data found for given year.</b>
        </EmptyDataTemplate>
    </jquery:GridViewEx>
</asp:Content>
