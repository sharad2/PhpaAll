<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" CodeBehind="BalanceProjectCostJobs.aspx.cs"
    Inherits="Finance.Reports.BalanceProjectCostJobs" Title="Jobs Associated with Account Head" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cph" runat="server">
    <div style="float: left">
        <phpa:PhpaLinqDataSource ID="dsBalanceProjectCostJobs" runat="server" ContextTypeName="Eclipse.PhpaLibrary.Reporting.ReportingDataContext"
            TableName="RoJobs" RenderLogVisible="false" Where="HeadOfAccountId == @HeadOfAccountId">
            <WhereParameters>
                <asp:QueryStringParameter Name="HeadOfAccountId" QueryStringField="HeadId" Type="Int32" />
            </WhereParameters>
        </phpa:PhpaLinqDataSource>
        <jquery:GridViewEx ID="gvBalanceProjectCostJobs" runat="server" AutoGenerateColumns="False"
            DataKeyNames="JobId" DataSourceID="dsBalanceProjectCostJobs" ShowFooter="true">
            <Columns>
                <eclipse:MultiBoundField DataFields="JobCode" HeaderText="Job Code" SortExpression="JobCode"
                    HeaderStyle-HorizontalAlign="Right" />
                <eclipse:MultiBoundField DataFields="Description" HeaderText="Description" FooterText="Total" />
                <eclipse:MultiBoundField DataFields="SanctionedAmount" DataFormatString="{0:C2}"
                    HeaderText="Sanctioned Issued" DataSummaryCalculation="ValueSummation">
                    <ItemStyle HorizontalAlign="Right" />
                    <HeaderStyle HorizontalAlign="Center" />
                    <FooterStyle HorizontalAlign="Right" />
                </eclipse:MultiBoundField>
                <eclipse:MultiBoundField DataFields="ContractAmount" DataFormatString="{0:C2}" HeaderText="Work Award Amount"
                    DataSummaryCalculation="ValueSummation">
                    <ItemStyle HorizontalAlign="Right" />
                    <HeaderStyle HorizontalAlign="Center" />
                    <FooterStyle HorizontalAlign="Right" />
                </eclipse:MultiBoundField>
                <eclipse:MultiBoundField DataFields="RevisedSanction" DataFormatString="{0:C2}" HeaderText="Revised Sanction Issued"
                    DataSummaryCalculation="ValueSummation">
                    <ItemStyle HorizontalAlign="Right" />
                    <HeaderStyle HorizontalAlign="Center" />
                    <FooterStyle HorizontalAlign="Right" />
                </eclipse:MultiBoundField>
                <eclipse:MultiBoundField DataFields="RevisedContract" DataFormatString="{0:C2}" HeaderText="Revised Work Award"
                    DataSummaryCalculation="ValueSummation" SortExpression="RevisedContract">
                    <ItemStyle HorizontalAlign="Right" />
                    <HeaderStyle HorizontalAlign="Center" />
                    <FooterStyle HorizontalAlign="Right" />
                </eclipse:MultiBoundField>
            </Columns>
        </jquery:GridViewEx>
    </div>
</asp:Content>
