<%@ Control Language="C#" CodeBehind="VoucherDetailControl.ascx.cs" Inherits="PhpaAll.Controls.VoucherDetailControl" %>
<phpa:PhpaLinqDataSource ID="dsVouchers" runat="server" ContextTypeName="Eclipse.PhpaLibrary.Reporting.ReportingDataContext"
    OnSelecting="dsVouchers_Selecting" RenderLogVisible="False" OrderBy="VoucherDate, VoucherCode"
    TableName="RoVouchers" Visible="True">
    <WhereParameters>
        <asp:Parameter Name="FromDate" Type="DateTime" />
        <asp:Parameter Name="DefaultFromDate" Type="DateTime" />
        <asp:Parameter Name="ToDate" Type="DateTime" />
        <asp:Parameter Name="VoucherType" Type="Char" />
         <asp:Parameter Name="Station" Type="String" />
    </WhereParameters>
</phpa:PhpaLinqDataSource>
<asp:DataPager runat="server" ID="ctlPager" PageSize="2000" PagedControlID="lvDayBook" />
<asp:ListView ID="lvDayBook" runat="server" EnableViewState="False" OnItemCreated="lvDayBook_ItemCreated"
    OnDataBound="lvDayBook_DataBound" OnLayoutCreated="lvDayBook_LayoutCreated" OnItemDataBound="lvDayBook_ItemDataBound">
    <EmptyDataTemplate>
        No vouchers found
    </EmptyDataTemplate>
    <LayoutTemplate>
        <table class="vd-table">
            <caption>
                <asp:Label ID="lblCount" runat="server" Text="Dynamic Count" OnPreRender="lblCount_PreRender"
                    Font-Size="Large" Font-Underline="true" />
            </caption>
            <thead>
                <tr>
                    <th class="vd-datecol">
                        Date&nbsp;&darr;
                    </th>
                    <th runat="server" onload="colName_Load" class="vd-namecol">
                        Name
                    </th>
                    <th>
                        Code|Type&nbsp;&uarr;
                    </th>
                    <th>
                        Particulars
                    </th>
                    <th runat="server" onload="colHeadOfAccount_Load" class="vd-headcol">
                        Head Of Account
                    </th>
                    <th class="vd-empjobcol" onload="colEmpContractor_Load" runat="server">
                        Employee / Job
                    </th>
                    <th id="tdDebitHeader" runat="server" onprerender="tdDebitHeader_prerender">
                        Debit
                    </th>
                    <th id="tdCreditHeader" runat="server" onprerender="tdCreditHeader_prerender">
                        Credit
                    </th>
                </tr>
            </thead>
            <tbody>
                <tr id="rowOpeningBalance" runat="server">
                    <th class="vd-datecol" />
                    <th runat="server" onload="colName_Load" />
                    <th onload="colEmpContractor_Load" runat="server" />
                    <th onload="colHeadOfAccount_Load" runat="server" />
                    <th colspan="2">
                        Opening Balance
                    </th>
                    <th id="cellOpeningBalance" runat="server" colspan="2" align="right" />
                </tr>
                <tr id="itemPlaceholder" runat="server" />
                <tr id="rowSubTotal" runat="server">
                    <th class="vd-datecol" />
                    <th runat="server" onload="colName_Load" />
                    <th onload="colEmpContractor_Load" runat="server" />
                    <th onload="colHeadOfAccount_Load" runat="server" />
                    <th colspan="2" id="tdSubTotal">
                        SubTotal
                    </th>
                    <th id="tdSubTotalDebit" runat="server" onprerender="tdSubTotalDebit_prerender" class="vd-amountcol" />
                    <th id="tdSubTotalCredit" runat="server" onprerender="tdSubTotalCredit_prerender"
                        class="vd-amountcol" />
                </tr>
                <tr id="rowClosingBalance" runat="server">
                    <th class="vd-datecol" />
                    <th runat="server" onload="colName_Load" />
                    <th onload="colEmpContractor_Load" runat="server" />
                    <th onload="colHeadOfAccount_Load" runat="server" />
                    <th colspan="2">
                        Closing Balance
                    </th>
                    <th id="ctlClosingBalance" runat="server" class="vd-amountcol" />
                </tr>
                <tr id="rowTotal" runat="server">
                    <th class="vd-datecol" />
                    <th runat="server" onload="colName_Load" />
                    <th onload="colEmpContractor_Load" runat="server" />
                    <th onload="colHeadOfAccount_Load" runat="server" />
                    <th colspan="2">
                        Total
                    </th>
                    <th id="tdDebitFooter" runat="server" onprerender="tdDebitFooter_prerender" class="vd-amountcol" />
                    <th id="tdCreditFooter" runat="server" onprerender="tdCreditFooter_prerender" class="vd-amountcol" />
                </tr>
                <tr id="netBalance" runat="server">
                    <th class="vd-datecol" />
                    <th runat="server" onload="colName_Load" />
                    <th onload="colEmpContractor_Load" runat="server" />
                    <th onload="colHeadOfAccount_Load" runat="server" />
                    <th colspan="2">
                        Net Balance
                    </th>
                    <th id="tdNetBalanceDebit" runat="server" class="vd-amountcol" onprerender="tdNetBalanceDebit_prerender">
                    </th>
                    <th id="tdNetBalanceCredit" runat="server" class="vd-amountcol" onprerender="tdNetBalanceCredit_prerender" />
                </tr>
            </tbody>
        </table>
    </LayoutTemplate>
    <ItemTemplate>
        <tr>
            <td id="tdDate" runat="server" class="vd-datecol">
                <asp:Label ID="lblSequence" runat="server" Text="Dynamic Sequence" />)&nbsp;<asp:Label
                    runat="server" Text='<%# Eval("VoucherDate", "{0:d}") %>' ToolTip="Voucher date" />
                <asp:HyperLink runat="server" Text='<%# Eval("VoucherReference") %>' NavigateUrl='<%# Eval("VoucherId", "~/Finance/InsertVoucher.aspx?VoucherId={0}") %>'
                    ToolTip="Voucher Reference Number. Click to see more details and edit" CssClass="vd-voucher-ref" />
            </td>
            <td id="tdPayee" runat="server" class="vd-namecol" onload="colName_Load">
                <asp:Label runat="server" Text='<%# Eval("PayeeName") %>' />
            </td>
            <td id="tdVoucherDetails" runat="server" class="vd-vouchercol">
                <asp:Label runat="server" Text='<%# Eval("VoucherCode") %>' />|<asp:Label ID="lblVoucherType"
                    runat="server" Text='<%# Eval("VoucherType") %>' />
                <asp:Label ID="lblCheckNo" runat="server" Text='<%# Eval("CheckNumber", "<br/><em>Check #:</em> {0}") %>'
                    Visible="false" />
            </td>
            <td id="tdParticulars" runat="server" class="vd-particularscol">
                <asp:Label ID="lblParticulars" runat="server" Text='<%# Eval("Particulars") %>' ToolTip="Particulars" />
            </td>
            <asp:Repeater ID="lvVoucherDetails" runat="server">
                <ItemTemplate>
                    <td class="vd-headcol" runat="server" onload="colHeadOfAccount_Load">
                        <asp:Label runat="server" Text='<%# Eval("HeadOfAccount.DisplayName") %>'></asp:Label>:
                        <asp:Label runat="server" Text='<%# Eval("HeadOfAccount.Description") %>'></asp:Label>
                    </td>
                    <td runat="server" class="vd-empjobcol" onload="colEmpContractor_Load">
                        <asp:Label runat="server" Text='<%# string.Format("{0}: {1}", Eval("RoEmployee.EmployeeCode"), Eval("RoEmployee.FullName")) %>'
                            Visible='<%# Eval("EmployeeId") != null  %>' />
                        <asp:Label runat="server" Text='<%# string.Format("{0}: {1} ({2})", Eval("RoJob.JobCode"),
                         Eval("RoJob.Description"), Eval("RoJob.RoContractor.ContractorName")) %>' Visible='<%# Eval("JobId") != null  %>' />
                    </td>
                    <td class="vd-amountcol">
                        <asp:Label runat="server" Text='<%# Eval("DebitAmount", "{0:C}") %>' />
                    </td>
                    <td class="vd-amountcol">
                        <asp:Label runat="server" Text='<%# Eval("CreditAmount", "{0:C}") %>' />
                    </td>
                </ItemTemplate>
                <SeparatorTemplate>
                    </tr><tr>
                </SeparatorTemplate>
            </asp:Repeater>
            <asp:MultiView ID="mvEmpty" runat="server">
                <asp:View ID="View1" runat="server">
                    <td colspan="4">
                        This voucher has no details
                    </td>
                </asp:View>
            </asp:MultiView>
        </tr>
    </ItemTemplate>
    <EmptyDataTemplate>
        No vouchers found
    </EmptyDataTemplate>
</asp:ListView>
