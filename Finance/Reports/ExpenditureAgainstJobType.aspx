<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" CodeBehind="ExpenditureAgainstJobType.aspx.cs"
    Inherits="Finance.Reports.ExpenditureAgainstJobType" Title="Job Type Expenditure"
    EnableViewState="false" %>

<%@ Register Src="../Controls/PrinterFriendlyButton.ascx" TagName="PrinterFriendlyButton"
    TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphSideNavigation" runat="server">
    <asp:HyperLink ID="HyperLink2" runat="server" Text="Help" NavigateUrl="~/Doc/ExpenditureAgainstJobType.doc.aspx" /><br />
    <uc2:PrinterFriendlyButton ID="PrinterFriendlyButton1" runat="server" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cph" runat="server">
    <div style="float: left">
        <asp:MultiView ID="MultiView1" runat="server">
            <asp:View ID="C" runat="server">
                Displays Expenditure for Contractual Jobs. These jobs are generally of large amount.
            </asp:View>
            <asp:View ID="W" runat="server">
                Displays Expenditure for work orders. These jobs are generally of lesser amount
                then the Contractual Jobs.<br />
            </asp:View>
            <asp:View ID="D" runat="server">
                Display Expenditure for Jobs in progress those are executed for a department.
            </asp:View>
        </asp:MultiView><br />
        <br />
        <eclipse:TwoColumnPanel runat="server">
            <eclipse:LeftLabel runat="server" Text="From Date / To Date" />
            <i:TextBoxEx ID="tbFromDate" runat="server" FriendlyName="From Date">
                <Validators>
                    <i:Date Max="0" />
                    <i:Required />
                </Validators>
            </i:TextBoxEx>
            <i:TextBoxEx ID="tbToDate" runat="server" FriendlyName="To Date">
                <Validators>
                    <i:Date DateType="ToDate" Max="0" />
                    <i:Required />
                </Validators>
            </i:TextBoxEx>
            <br />
            <br />
            <i:ButtonEx ID="btnShowReport" runat="server" Text="Show Report" Icon="Refresh" Action="Submit"
                CausesValidation="true" IsDefault="true" />
            <i:ValidationSummary ID="valSummary" runat="server" />
        </eclipse:TwoColumnPanel>
        <phpa:PhpaLinqDataSource ID="ds" runat="server" ContextTypeName="Eclipse.PhpaLibrary.Reporting.ReportingDataContext"
            TableName="RoJobs" RenderLogVisible="false" OnSelecting="ds_Selecting">
        </phpa:PhpaLinqDataSource>
        <asp:ListView ID="lvExpenditureAgainstContract" runat="server" OnItemCreated="lvExpenditureAgainstContract_ItemCreated"
            DataSourceID="ds">
            <LayoutTemplate>
                <asp:Label ID="lblCaption" runat="server" Visible="false" Font-Bold="true" OnPreRender="lblCaption_PreRender" />
                <div id="itemPlaceholderContainer" runat="server">
                    <div id="itemPlaceholder" runat="server" />
                </div>
            </LayoutTemplate>
            <ItemTemplate>
                <div>
                    <asp:Label ID="lblDivisionName" runat="server" Text='<%# Eval("Key.DivisionName")%>'
                        Font-Bold="true" Font-Size="Large" Font-Underline="true" />
                </div>
                <br />
                <div>
                    <jquery:GridViewEx ID="gvExpenditureAgainstContract" runat="server" AutoGenerateColumns="false"
                        GridLines="Both" ShowFooter="true" OnDataBinding="gvExpenditureAgainstContract_DataBinding"
                        OnRowCreated="gvExpenditureAgainstContract_RowCreated" OnRowDataBound="gvExpenditureAgainstContract_RowDataBound">
                        <Columns>
                            <eclipse:SequenceField FooterText="">
                            </eclipse:SequenceField>
                            <eclipse:MultiBoundField HeaderText="Job Code" HeaderStyle-HorizontalAlign="Center"
                                DataFields="JobCode">
                            </eclipse:MultiBoundField>
                            <eclipse:MultiBoundField HeaderText="Head of Account" DataFields="HeadofAccount"
                                AccessibleHeaderText="Head" />
                            <eclipse:MultiBoundField HeaderText="Job | Description" DataFields="Description">
                                <HeaderStyle Width="20em" />
                            </eclipse:MultiBoundField>
                            <eclipse:MultiBoundField HeaderText="Job | Contractor" ToolTipFields="ContractorCode"
                                ToolTipFormatString="Contractor Code:{0}" FooterText="Total" DataFields="ContractorName">
                                <HeaderStyle Width="20em" />
                            </eclipse:MultiBoundField>
                            <eclipse:MultiBoundField HeaderText="Award Amount (Nu.)" DataFields="AwardAmount"
                                DataSummaryCalculation="ValueSummation" DataFormatString="{0:C2}">
                                <ItemStyle HorizontalAlign="Right" />
                                <FooterStyle HorizontalAlign="Right" />
                            </eclipse:MultiBoundField>
                            <eclipse:MultiBoundField HeaderText="Expenditure (Nu.)|Current Month" DataFields="AmountMonth"
                                DataSummaryCalculation="ValueSummation" DataFormatString="{0:C2}" AccessibleHeaderText="AmountMonth">
                                <ItemStyle HorizontalAlign="Right" />
                                <FooterStyle HorizontalAlign="Right" />
                            </eclipse:MultiBoundField>
                            <eclipse:MultiBoundField HeaderText="Expenditure (Nu.)|Progressive" DataFields="AmountProgressive"
                                DataSummaryCalculation="ValueSummation" DataFormatString="{0:C2}" AccessibleHeaderText="AmountProgressive">
                                <ItemStyle HorizontalAlign="Right" />
                                <FooterStyle HorizontalAlign="Right" />
                            </eclipse:MultiBoundField>
                            <eclipse:MultiBoundField DataFields="AdvanceOutstanding" AccessibleHeaderText="AdvanceOutstanding"
                                DataSummaryCalculation="ValueSummation" DataFormatString="{0:C2}" HeaderText="Advance Outstanding(Nu.)">
                                <ItemStyle HorizontalAlign="Right" />
                                <FooterStyle HorizontalAlign="Right" />
                            </eclipse:MultiBoundField>
                            <eclipse:MultiBoundField DataFields="Total" AccessibleHeaderText="Total" HeaderText="Total(Nu.)"
                                DataSummaryCalculation="ValueSummation" DataFormatString="{0:C2}" HeaderToolTip="Summation of Progressive amount and Advance Outstanding.">
                                <ItemStyle HorizontalAlign="Right" />
                                <FooterStyle HorizontalAlign="Right" />
                            </eclipse:MultiBoundField>
                        </Columns>
                        <EmptyDataTemplate>
                            <asp:Label ID="lblEmpty" runat="server" Font-Bold="true" Font-Size="Larger">No Job exists in this Division.</asp:Label>
                        </EmptyDataTemplate>
                    </jquery:GridViewEx>
                </div>
            </ItemTemplate>
            <ItemSeparatorTemplate>
                <hr />
            </ItemSeparatorTemplate>
            <EmptyDataTemplate>
                <div>
                    No expenditures found for this period.
                </div>
            </EmptyDataTemplate>
        </asp:ListView>
    </div>
</asp:Content>
