<%@ Page Language="C#" CodeBehind="VoucherDetails.aspx.cs" Inherits="PhpaAll.Finance.VoucherDetails"
    EnableViewState="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <jquery:JQueryScriptManager runat="server" />
</head>
<body>
    <div>
        <phpa:PhpaLinqDataSource runat="server" ID="dsVoucher" ContextTypeName="Eclipse.PhpaLibrary.Database.FinanceDataContext"
            AutoGenerateWhereClause="true" OnSelecting="dsVoucher_Selecting" TableName="Vouchers"
            OnContextCreated="dsVoucher_ContextCreated" RenderLogVisible="false">
            <WhereParameters>
                <asp:QueryStringParameter Name="VoucherId" QueryStringField="VoucherId" Type="Int32" />
            </WhereParameters>
        </phpa:PhpaLinqDataSource>
        <asp:FormView runat="server" ID="fvVoucher" DataSourceID="dsVoucher" DataKeyNames="VoucherId">
            <%--<HeaderTemplate>
                <h3> Voucher: <%# Eval("Particulars")%></h3>
            </HeaderTemplate>--%>
            <ItemTemplate>
                <jquery:Tabs ID="tab1" runat="server" Collapsible="false" Selected="0">
                    <jquery:JPanel ID="fvVoucher_PanelHeader" runat="server" HeaderText="Header">
                        <eclipse:TwoColumnPanel ID="TwoColumnPanel1" runat="server">
                            <eclipse:LeftLabel ID="LeftLabel1" runat="server" Text="Dated" />
                            <asp:Label ID="Label1" runat="server" Text='<%# Eval("VoucherDate", "{0:d}") %>'
                                ToolTip='<%# Eval("VoucherId", "Internal Voucher Id: {0}") %>' />
                            <eclipse:LeftLabel ID="LeftLabel2" runat="server" Text="Ref #" />
                            <asp:Label ID="Label2" runat="server" OnPreRender="lblVrRef_PreRender" />
                            <eclipse:LeftLabel ID="LeftLabel3" runat="server" Text="Type" />
                            <asp:Label ID="Label3" runat="server" Text='<%# Eval("DisplayVoucherType") %>' />
                            <eclipse:LeftLabel ID="LeftLabel4" runat="server" Text="Payee/Sender's Name" />
                            <asp:Label ID="Label4" runat="server" Text='<%# Eval("PayeeName") %>' />
                            <eclipse:LeftLabel ID="LeftLabel5" runat="server" Text="Cheque #" />
                            <asp:Label ID="Label5" runat="server" Text='<%# Eval("CheckNumber") %>' />
                            <eclipse:LeftLabel ID="LeftLabel6" runat="server" Text="Voucher No" />
                            <asp:Label ID="Label6" runat="server" Text='<%# Eval("VoucherCode") %>' />
                            <eclipse:LeftLabel ID="LeftLabel7" runat="server" Text="Division" />
                            <asp:Label ID="Label7" runat="server" Text='<%# Eval("Division.DivisionName") %>' />
                            <eclipse:LeftLabel ID="LeftLabel8" runat="server" Text="Particulars" />
                            <asp:Label ID="Label8" runat="server" Text='<%# Eval("Particulars") %>' />
                        </eclipse:TwoColumnPanel>
                    </jquery:JPanel>
                    <jquery:JPanel ID="fvVoucher_PanelDetails" runat="server" HeaderText="Details">
                        <phpa:PhpaLinqDataSource runat="server" ID="dsVoucherDetails" ContextTypeName="Eclipse.PhpaLibrary.Database.FinanceDataContext"
                            TableName="VoucherDetails" AutoGenerateWhereClause="true" OnSelecting="dsVoucher_Selecting"
                            RenderLogVisible="false">
                            <WhereParameters>
                                <asp:QueryStringParameter Name="VoucherId" QueryStringField="VoucherId" Type="Int32" />
                            </WhereParameters>
                        </phpa:PhpaLinqDataSource>
                        <jquery:GridViewEx ID="gvVoucherDetails" runat="server" AutoGenerateColumns="False"
                            ShowFooter="true" DataSourceID="dsVoucherDetails" DataKeyNames="VoucherDetailId"
                            Caption="Voucher Details">
                            <Columns>
                                <eclipse:MultiBoundField HeaderText="Head" SortExpression="HeadOfAccountId" DataFields="HeadOfAccount.DisplayDescription"
                                    ToolTipFields="HeadOfAccount.Description" />
                                <eclipse:MultiBoundField HeaderText="Job" SortExpression="Job.Description" DataFields="Job.Description"
                                    ToolTipFields="JobId" ToolTipFormatString="Inernal Job Id: {0}" DataFormatString="{0}"
                                    HideEmptyColumn="true" />
                                <eclipse:MultiBoundField HeaderText="Contractor" SortExpression="Contractor.ContractorName"
                                    DataFields="Contractor.ContractorName" ToolTipFields="ContractorId" ToolTipFormatString="Inernal Contractor Id: {0}"
                                    DataFormatString="{0}" HideEmptyColumn="true" />
                                <eclipse:MultiBoundField HeaderText="Employee" SortExpression="Employee.FirstName, Employee.LastName"
                                    DataFields="Employee.FirstName,Employee.LastName" ToolTipFields="EmployeeId"
                                    ToolTipFormatString="Internal Employee Id: {0}" DataFormatString="{0} {1}" HideEmptyColumn="true" />
                                <eclipse:MultiBoundField DataFields="DebitAmount" HeaderText="Debit" AccessibleHeaderText="DebitAmount"
                                    DataFormatString="{0:N2}" DataSummaryCalculation="ValueSummation">
                                    <ItemStyle HorizontalAlign="Right" />
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <FooterStyle HorizontalAlign="Right" />
                                </eclipse:MultiBoundField>
                                <eclipse:MultiBoundField DataFields="CreditAmount" HeaderText="Credit" AccessibleHeaderText="CreditAmount"
                                    DataFormatString="{0:N2}" DataSummaryCalculation="ValueSummation">
                                    <ItemStyle HorizontalAlign="Right" />
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <FooterStyle HorizontalAlign="Right" />
                                </eclipse:MultiBoundField>
                            </Columns>
                            <EmptyDataTemplate>
                                This voucher has no details
                            </EmptyDataTemplate>
                        </jquery:GridViewEx>
                    </jquery:JPanel>
                    <phpa:AuditTabPanel ID="fvVoucher_AuditTabPanel1" runat="server" HeaderText="Audit" />
                </jquery:Tabs>
            </ItemTemplate>
        </asp:FormView>
    </div>
</body>
</html>
