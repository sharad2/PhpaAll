<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" CodeBehind="ContractorPaymentReport.aspx.cs"
    Inherits="Finance.Reports.ContractorPaymentReport" Title="Contractor Payment Register" %>

<%@ Register Src="../Controls/PrinterFriendlyButton.ascx" TagName="PrinterFriendlyButton"
    TagPrefix="uc2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">      
        function tbJobs_Search(event, ui) {
            var contractorId = $('#tbContractors').autocompleteEx('selectedValue');
            if (!contractorId) {
                alert('Please select contractor first');
                return false;
            }
            $(this).autocompleteEx('option', 'parameters', { contractorId: contractorId, term: $(this).val() });
            return true;
        }
        function tbJobs_KeyPress() {
            var contractorId = $('#tbContractors').autocompleteEx('selectedValue');
            if (!divisionId) {
                alert('Please select contractor first');
                return false;
            }
        }

        function tbJobs_Change() {
            var contractorId = $('#tbContractors').autocompleteEx('selectedValue');
            if (!contractorId) {
                alert('Please select contractor first');
                return false;
            }
            $(this).autocompleteEx('option', 'parameters', { contractorId: contractorId, term: $(this).val() });
            var badJob = $(this).autocompleteEx('validate');
            var job = $(this).autocompleteEx('selectedValue');
            if (!job) {
                alert('Job is invalid or it does not belongs contractor. Please re-enter valid job');
                $(this).val('');
                $(this).focus();
            }
        }


    </script>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphSideNavigation" runat="server">
    <asp:HyperLink ID="HyperLink2" runat="server" Text="Help" NavigateUrl="~/Doc/ContractorPayment.doc.aspx" /><br />
    <uc2:PrinterFriendlyButton ID="PrinterFriendlyButton1" runat="server" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cph" runat="server">
    <b>
        <eclipse:LeftLabel ID="lblMessage" runat="server" Visible="false" />
    </b>
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
        <eclipse:LeftLabel ID="LeftLabel18" runat="server" Text="Contractor" />
        <i:AutoComplete ID="tbContractors" runat="server" ClientIDMode="Static" Width="25em"
            WebMethod="GetContractors" WebServicePath="~/Services/Contractors.asmx">
        </i:AutoComplete>
        <br />
        <eclipse:LeftLabel ID="lblJob" runat="server" Text="Jobs" />
        <i:AutoComplete ID="tbJobs" runat="server" ClientIDMode="Static" Width="25em" WebMethod="GetJobsForContractor"
            WebServicePath="~/Services/Contractors.asmx" OnClientSearch="tbJobs_Search" OnClientKeyPress="tbJobs_KeyPress" OnClientChange="tbJobs_Change" ValidateWebMethodName="ValidateJobForContractor" AutoValidate="false" Delay="1000">
        </i:AutoComplete>
        <br />
        <i:ButtonEx ID="btnGo" runat="server" Text="Go" CausesValidation="true" Action="Submit"
            Icon="Refresh" />
        <br />
        <i:ValidationSummary ID="ValidationSummary1" runat="server" />
    </eclipse:TwoColumnPanel>
    <hr />
    <asp:Repeater ID="rptContarctorPayment" runat="server" OnItemDataBound="rptContarctorPayment_ItemDataBound">
        <ItemTemplate>
            <asp:FormView ID="fvContractorJob" runat="server">
                <ItemTemplate>
                    <div style="float: left; width: 60%">
                        <table border="1">
                            <tr>
                                <td>
                                    <asp:Label ID="Label1" runat="server" Text="<strong>Job Code</strong>" />
                                </td>
                                <td>
                                    <asp:Label ID="Label2" runat="server" Text='<%#Eval("JobCode") %>' />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label3" runat="server" Text="<strong>Name Of The Work</strong>" />
                                </td>
                                <td>
                                    <asp:Label ID="Label4" runat="server" Text='<%#Eval("Description") %>' />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label5" runat="server" Text="<strong>Administrative/ Tech Sanction Number</strong>" />
                                </td>
                                <td>
                                    <asp:Label ID="Label6" runat="server" Text='<%#Eval("SanctionNumber") %>' />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label7" runat="server" Text="<strong>Tender Work/Work Order Number</strong>" />
                                </td>
                                <td>
                                    <asp:Label ID="Label8" runat="server" Text='<%#Eval("WorkOrderNumber") %>' />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label9" runat="server" Text="<strong>Date Of Commencement</strong>" />
                                </td>
                                <td>
                                    <asp:Label ID="Label10" runat="server" Text='<%#Eval("CommencementDate", "{0:d}") %>' />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label11" runat="server" Text="<strong>Contractor Code</strong>" />
                                </td>
                                <td>
                                    <asp:Label ID="Label12" runat="server" Text='<%#Eval("ContractorCode") %>' />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label13" runat="server" Text="<strong>Contractor Name</strong>" />
                                </td>
                                <td>
                                    <asp:Label ID="Label14" runat="server" Text='<%#Eval("ContractorName") %>' />
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div style="float: left; margin-left: 1mm">
                        <table border="1">
                            <tr>
                                <td>
                                    <asp:Label ID="Label15" runat="server" Text="<strong>Award Amount</strong>" />
                                </td>
                                <td>
                                    <asp:Label ID="lblContractAmount" Text='<%#Eval("ContractAmount","{0:C2}") %>' runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label16" runat="server" Text="<strong>Sanction Order Date</strong>" />
                                </td>
                                <td>
                                    <asp:Label ID="lblSanctionDate" runat="server" Text='<%#Eval("SanctionDate", "{0:d}") %>' />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label17" runat="server" Text="<strong>Sanctioned Amount</strong>" />
                                </td>
                                <td>
                                    <asp:Label ID="lblSanctionAmount" Text='<%#Eval("SanctionedAmount","{0:C2}") %>'
                                        runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label18" runat="server" Text="<strong>Revised Sanction Amount</strong>" />
                                </td>
                                <td>
                                    <asp:Label ID="lblRevisedSanctionAmount" Text='<%#Eval("RevisedSanction") %>' runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label19" runat="server" Text="<strong>Work Order Date</strong>" />
                                </td>
                                <td>
                                    <asp:Label ID="lblWorkOrderDate" Text='<%#Eval("WorkOrderDate","{0:d}") %>' runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label20" runat="server" Text="<strong>Period/Date Of Completion</strong>" />
                                </td>
                                <td>
                                    <asp:Label ID="lblCompletionDate" Text='<%#Eval("CompletionDate","{0:d}") %>' runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label21" runat="server" Text="<strong>Contract Award Date</strong>" />
                                </td>
                                <td>
                                    <asp:Label ID="lblContractAwardDate" Text='<%#Eval("AwardDate","{0:d}") %>' runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label22" runat="server" Text="<strong>Revised Award Amount</strong>" />
                                </td>
                                <td>
                                    <asp:Label ID="lblRevisedAwardAmount" Text='<%#Eval("RevisedContract") %>' runat="server" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </ItemTemplate>
            </asp:FormView>
            <div style="font-size: large; margin-bottom: 1mm">
                <asp:Label ID="lblOpenBal" runat="server" Text="Opening Balance:" Visible="false" />
                <asp:Label ID="lblOpeningBalance" ToolTip="Opening balance of Job" runat="server"
                    Width="40%" />
            </div>
            <jquery:GridViewEx ID="gvContractorPayment" runat="server" GridLines="Both" AutoGenerateColumns="false"
                ShowFooter="true" AllowPaging="false">
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
            <div style="font-size: 12pt; margin-bottom: 1mm; margin-left: 85mm">
                <asp:Label ID="lbldiffrence" runat="server" Font-Bold="true" Text="Balance: " ToolTip="Difference in Contractor's Advance Paid(5) and Advance Adjusted(6) (Advance Paid - Advance Adjusted)"
                    Visible="false" />
            </div>
        </ItemTemplate>
        <SeparatorTemplate>
            <hr />
            <hr />
        </SeparatorTemplate>
    </asp:Repeater>
    <div id="dvGrand" runat="server" visible="false">
        <asp:Label ID="lblGrandTotal" runat="server" Font-Bold="true">Grand Total</asp:Label>
        <table border="1" width="100%" id="tbgrand" visible="false">
            <thead>
                <tr class="ui-state-default">
                    <th>
                        Amount
                    </th>
                    <th>
                        Advance
                    </th>
                    <th>
                        Adjustment
                    </th>
                    <th>
                        Contract Tax
                    </th>
                    <th>
                        Security Deposit
                    </th>
                    <th>
                        Interest
                    </th>
                    <th>
                        Others
                    </th>
                    <th>
                        Total Recovery
                    </th>
                    <th>
                        Net Payment
                    </th>
                </tr>
            </thead>
            <tbody>
                <tr class="ui-state-active ui-widget-header">
                    <td align="right">
                        <asp:Label ID="lblAmount" runat="server"></asp:Label>
                    </td>
                    <td align="right">
                        <asp:Label ID="lblAdvance" runat="server"></asp:Label>
                    </td>
                    <td align="right">
                        <asp:Label ID="lblAdjustmenet" runat="server"></asp:Label>
                    </td>
                    <td align="right">
                        <asp:Label ID="lblConTax" runat="server"></asp:Label>
                    </td>
                    <td align="right">
                        <asp:Label ID="lblSD" runat="server"></asp:Label>
                    </td>
                    <td align="right">
                        <asp:Label ID="lblInterest" runat="server"></asp:Label>
                    </td>
                    <td align="right">
                        <asp:Label ID="lblOthers" runat="server"></asp:Label>
                    </td>
                    <td align="right">
                        <asp:Label ID="lblTotalRecovery" runat="server"></asp:Label>
                    </td>
                    <td align="right">
                        <asp:Label ID="lblNetPayment" runat="server"></asp:Label>
                    </td>
                </tr>
            </tbody>
        </table>
    </div>
</asp:Content>
