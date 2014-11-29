<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" 
    CodeBehind="PrintAccountHeads.aspx.cs" Inherits="Finance.Reports.PrintAccountHeads"
    Title="Head of Accounts along with Cost" %>
    <%@ Register Src="~/Controls/PrinterFriendlyButton.ascx" TagName="PrinterFriendlyButton"
    TagPrefix="uc2" %>   

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphSideNavigation" runat="server">
<uc2:PrinterFriendlyButton ID="PrinterFriendlyButton1" runat="server" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cph" runat="server">
    <phpa:PhpaLinqDataSource ID="dsPrintAccountHeads" runat="server" ContextTypeName="Eclipse.PhpaLibrary.Reporting.ReportingDataContext"
      Select="new (ProjectCost, DisplayName, Description, GroupDescription)" 
        TableName="HeadOfAccounts" OrderBy="DisplayName" RenderLogVisible="false">
    </phpa:PhpaLinqDataSource>
    <jquery:GridViewEx ID="gvAccountHeads" runat="server" AutoGenerateColumns="False"
     DataSourceID="dsPrintAccountHeads" ShowFooter="true" AllowSorting="true">
        <Columns>
        <eclipse:SequenceField />
        <eclipse:MultiBoundField DataFields="DisplayName" HeaderText="Head of Account" 
                SortExpression="DisplayName" HeaderStyle-Wrap="true" />
                <eclipse:MultiBoundField DataFields="Description" HeaderText="Particulars" 
                SortExpression="Description" FooterText="Total" />
            <eclipse:MultiBoundField DataFields="ProjectCost" HeaderText="Project Cost(Amount/Nu million)" 
                SortExpression="ProjectCost" HeaderStyle-Wrap="true" DataSummaryCalculation="ValueSummation" DataFormatString="{0:N2}" >
                    <ItemStyle HorizontalAlign="Right" />
                <FooterStyle HorizontalAlign="Right" />
                </eclipse:MultiBoundField>
            <eclipse:MultiBoundField DataFields="GroupDescription" HeaderText="Remarks"
             SortExpression="GroupDescription" ItemStyle-Width="2in" />
        </Columns>
    </jquery:GridViewEx>
</asp:Content>
