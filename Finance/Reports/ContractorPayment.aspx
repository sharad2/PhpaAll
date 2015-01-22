<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" CodeBehind="ContractorPayment.aspx.cs"
    Inherits="Finance.Reports.ContractorPayment" Title="Job Payment Register" EnableViewState="false" %>

<%@ Register Src="../Controls/PrinterFriendlyButton.ascx" TagName="PrinterFriendlyButton"
    TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphSideNavigation" runat="server">
    <asp:HyperLink ID="HyperLink2" runat="server" Text="Help" NavigateUrl="~/Doc/ContractorPayment.doc.aspx" /><br />
    <uc2:PrinterFriendlyButton ID="PrinterFriendlyButton1" runat="server" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cph" runat="server">
    <eclipse:TwoColumnPanel ID="TwoColumnPanel1" runat="server" IsValidationContainer="true">
        <eclipse:LeftLabel ID="LeftLabel1" runat="server" Text="Voucher Dates From" />
        <i:TextBoxEx ID="tbFromDate" runat="server" FriendlyName="From Date">
            <Validators>
                <i:Date />
                <i:Required />
            </Validators>
        </i:TextBoxEx>
        To
        <i:TextBoxEx ID="tbToDate" runat="server" FriendlyName="To Date">
            <Validators>
                <i:Date DateType="ToDate" />
            </Validators>
        </i:TextBoxEx>
        <br />
        Leave "To" date blank to see information for one month.
        <eclipse:LeftLabel ID="LeftLabel18" runat="server" Text="Job" />
        <i:AutoComplete ID="tbJob" runat="server" ClientIDMode="Static" Width="25em" WebMethod="GetJobList"
            WebServicePath="~/Services/Contractors.asmx" ValidateWebMethodName="ValidateJob" AutoValidate="true">
            <Validators>
                <i:Required />
            </Validators>
        </i:AutoComplete>
        <br />
        <i:ButtonEx ID="btnGo" runat="server" Text="Go" CausesValidation="true" Action="Submit"
            Icon="Refresh" />
        <br />
        <i:ValidationSummary ID="ValidationSummary1" runat="server" />
    </eclipse:TwoColumnPanel>
    <phpa:PhpaLinqDataSource ID="dsContractorPayment" runat="server" ContextTypeName="Eclipse.PhpaLibrary.Reporting.ReportingDataContext"
        TableName="RoJobs" RenderLogVisible="false" OnSelecting="dsContractorPayment_Selecting" />
    <phpa:PhpaLinqDataSource ID="dsSpecificJob" RenderLogVisible="false" runat="server"
        ContextTypeName="Eclipse.PhpaLibrary.Reporting.ReportingDataContext" TableName="RoJobs"
        AutoGenerateWhereClause="true" OnSelecting="dsSpecificJob_Selecting">
        <WhereParameters>
            <asp:Parameter Name="JobId" Type="Int32" />
        </WhereParameters>
    </phpa:PhpaLinqDataSource>
    <hr />
    <asp:FormView ID="fvJob" runat="server" DataSourceID="dsSpecificJob">
        <ItemTemplate>
            <div style="float: left; width: 60%">
                <table border="1">
                    <tr>
                        <td>
                            <asp:Label runat="server" Text="<strong>Job Code</strong>" />
                        </td>
                        <td>
                            <asp:Label runat="server" Text='<%#Eval("JobCode") %>' />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label runat="server" Text="<strong>Name Of The Work</strong>" />
                        </td>
                        <td>
                            <asp:Label runat="server" Text='<%#Eval("Description") %>' />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label runat="server" Text="<strong>Administrative/ Tech Sanction Number</strong>" />
                        </td>
                        <td>
                            <asp:Label runat="server" Text='<%#Eval("SanctionNumber") %>' />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label runat="server" Text="<strong>Tender Work/Work Order Number</strong>" />
                        </td>
                        <td>
                            <asp:Label runat="server" Text='<%#Eval("WorkOrderNumber") %>' />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label runat="server" Text="<strong>Date Of Commencement</strong>" />
                        </td>
                        <td>
                            <asp:Label runat="server" Text='<%#Eval("CommencementDate", "{0:d}") %>' />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label runat="server" Text="<strong>Contractor Code</strong>" />
                        </td>
                        <td>
                            <asp:Label runat="server" Text='<%#Eval("RoContractor.ContractorCode") %>' />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label runat="server" Text="<strong>Contractor Name</strong>" />
                        </td>
                        <td>
                            <asp:Label runat="server" Text='<%#Eval("RoContractor.ContractorName") %>' />
                        </td>
                    </tr>
                </table>
            </div>
            <div style="float: left; margin-left: 1mm">
                <table border="1">
                    <tr>
                        <td>
                            <asp:Label runat="server" Text="<strong>Award Amount</strong>" />
                        </td>
                        <td>
                            <asp:Label ID="lblContractAmount" Text='<%#Eval("ContractAmount","{0:C2}") %>' runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label runat="server" Text="<strong>Sanction Order Date</strong>" />
                        </td>
                        <td>
                            <asp:Label ID="lblSanctionDate" runat="server" Text='<%#Eval("SanctionDate", "{0:d}") %>' />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label runat="server" Text="<strong>Sanctioned Amount</strong>" />
                        </td>
                        <td>
                            <asp:Label ID="lblSanctionAmount" Text='<%#Eval("SanctionedAmount","{0:C2}") %>' runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label runat="server" Text="<strong>Revised Sanction Amount</strong>" />
                        </td>
                        <td>
                            <asp:Label ID="lblRevisedSanctionAmount" Text='<%#Eval("RevisedSanction") %>' runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label runat="server" Text="<strong>Work Order Date</strong>" />
                        </td>
                        <td>
                            <asp:Label ID="lblWorkOrderDate" Text='<%#Eval("WorkOrderDate","{0:d}") %>' runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label runat="server" Text="<strong>Period/Date Of Completion</strong>" />
                        </td>
                        <td>
                            <asp:Label ID="lblCompletionDate" Text='<%#Eval("CompletionDate","{0:d}") %>' runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label runat="server" Text="<strong>Contract Award Date</strong>" />
                        </td>
                        <td>
                            <asp:Label ID="lblContractAwardDate" Text='<%#Eval("AwardDate","{0:d}") %>' runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label runat="server" Text="<strong>Revised Award Amount</strong>" />
                        </td>
                        <td>
                            <asp:Label ID="lblRevisedAwardAmount" Text='<%#Eval("RevisedContract") %>' runat="server" />
                        </td>
                    </tr>
                </table>
            </div>
        </ItemTemplate>
    </asp:FormView>
    <br />
    <div style="font-size: large; margin-bottom: 1mm">
        <%--<asp:Label ID="lblOpenBal" runat="server" Text="Opening Balance:" Visible="false" />--%>
        <asp:Label ID="lblOpeningBalance" ToolTip="Opening balance of Job" runat="server"
            Width="40%" />
    </div>
    <jquery:GridViewEx ID="gvContractorPayment" runat="server" GridLines="Both" AutoGenerateColumns="false"
        ShowFooter="true" OnRowDataBound="gvContractorPayment_RowDataBound" AllowPaging="false"
        DataSourceID="dsContractorPayment">
        <Columns>
            <eclipse:SequenceField />
            <eclipse:MultiBoundField HeaderText="<br/><br/>1<br/> Date" HeaderStyle-HorizontalAlign="Center"
                DataFields="VoucherDate" DataFormatString="{0:d}" ItemStyle-HorizontalAlign="Right" />
            <eclipse:MultiBoundField HeaderText="<br/><br/>2<br > Particulars" HeaderStyle-HorizontalAlign="Center"
                DataFields="Particulars" FooterStyle-VerticalAlign="Top" />
            <eclipse:MultiBoundField HeaderText="<br/><br/>3<br> Amount" DataFields="AdmittedAmount"
                HeaderToolTip="Expenditure incurred against the job" DataSummaryCalculation="ValueSummation"
                DataFormatString="{0:N2}">
                <ItemStyle HorizontalAlign="Right" />
                <HeaderStyle HorizontalAlign="Center" />
                <FooterStyle HorizontalAlign="Right" VerticalAlign="Top" />
            </eclipse:MultiBoundField>
            <asp:HyperLinkField HeaderText="<br/><br/>4<br> Vr.No" AccessibleHeaderText="VoucherCode"
                DataTextField="VoucherCode" DataNavigateUrlFields="VoucherId" DataNavigateUrlFormatString="~/Finance/InsertVoucher.aspx?VoucherId={0}"
                HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Right" />
            <eclipse:MultiBoundField HeaderText="Contractor's Advance A/c|5<br> Advance" DataFields="AdvancePaid"
                DataFormatString="{0:C2}" DataSummaryCalculation="ValueSummation" AccessibleHeaderText="AdvancePaid">
                <HeaderStyle HorizontalAlign="Center" />
                <ItemStyle HorizontalAlign="Right" />
                <FooterStyle VerticalAlign="Top" HorizontalAlign="Right" />
            </eclipse:MultiBoundField>
            <eclipse:MultiBoundField HeaderText="Contractor's Advance A/c|6<br> Adjustment" DataFields="ContractorAdvanceAdjusted"
                DataFormatString="{0:C2}" DataSummaryCalculation="ValueSummation" AccessibleHeaderText="ContractorAdvanceAdjusted">
                <HeaderStyle HorizontalAlign="Center" />
                <ItemStyle HorizontalAlign="Right" />
                <FooterStyle VerticalAlign="Top" HorizontalAlign="Right" />
            </eclipse:MultiBoundField>
            <eclipse:MultiBoundField HeaderText="Detailed Recoveries |<br>7<br> Contract Tax"
                DataFields="ContractorTax" DataFormatString="{0:C2}" DataSummaryCalculation="ValueSummation">
                <HeaderStyle HorizontalAlign="Center" />
                <ItemStyle HorizontalAlign="Right" />
                <FooterStyle VerticalAlign="Top" HorizontalAlign="Right" />
            </eclipse:MultiBoundField>
            <eclipse:MultiBoundField HeaderText="Detailed Recoveries |<br>8<br> Security Deposit"
                DataFields="SecurityDeposit" DataFormatString="{0:C2}" DataSummaryCalculation="ValueSummation">
                <HeaderStyle HorizontalAlign="Center" />
                <ItemStyle HorizontalAlign="Right" />
                <FooterStyle VerticalAlign="Top" HorizontalAlign="Right" />
            </eclipse:MultiBoundField>
            <eclipse:MultiBoundField HeaderText="Detailed Recoveries |9<br> Interest" DataFields="InterestRecoverd"
                DataFormatString="{0:C2}" DataSummaryCalculation="ValueSummation">
                <HeaderStyle HorizontalAlign="Center" />
                <ItemStyle HorizontalAlign="Right" />
                <FooterStyle VerticalAlign="Top" HorizontalAlign="Right" />
            </eclipse:MultiBoundField>
            <eclipse:MultiBoundField HeaderText="Detailed Recoveries |10<br> Others" DataFields="OtherRecovery"
                DataFormatString="{0:C2}" DataSummaryCalculation="ValueSummation">
                <HeaderStyle HorizontalAlign="Center" />
                <ItemStyle HorizontalAlign="Right" />
                <FooterStyle VerticalAlign="Top" HorizontalAlign="Right" />
            </eclipse:MultiBoundField>
            <eclipse:MultiBoundField HeaderText="Detailed Recoveries |<br><br>11<br>(6 to 10)<br>Total Recovery"
                DataFields="TotalRecovery" DataFormatString="{0:C2}" DataSummaryCalculation="ValueSummation">
                <HeaderStyle HorizontalAlign="Center" />
                <ItemStyle HorizontalAlign="Right" />
                <FooterStyle VerticalAlign="Top" HorizontalAlign="Right" />
            </eclipse:MultiBoundField>
            <eclipse:MultiBoundField HeaderText="Detailed Recoveries |<br><br>12<br>(3+5-11)<br>Net Payment"
                DataFields="NetPayment" DataFormatString="{0:C2}" DataSummaryCalculation="ValueSummation">
                <HeaderStyle HorizontalAlign="Center" />
                <ItemStyle HorizontalAlign="Right" />
                <FooterStyle VerticalAlign="Top" HorizontalAlign="Right" />
            </eclipse:MultiBoundField>
        </Columns>
        <EmptyDataTemplate>
            No vouchers were found for the job specified.
        </EmptyDataTemplate>
        <HeaderStyle HorizontalAlign="Center" />
    </jquery:GridViewEx>
    <div style="font-size:12pt;margin-bottom: 1mm; margin-left:85mm">
        <asp:Label ID="lbldiffrence" runat="server" Font-Bold="true" Text="Balance: "
            ToolTip="Difference in Contractor's Advance Paid(5) and Advance Adjusted(6) (Advance Paid - Advance Adjusted)" Visible="false" />
    </div>
</asp:Content>
