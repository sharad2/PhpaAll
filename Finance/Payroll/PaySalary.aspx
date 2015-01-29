<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" CodeBehind="PaySalary.aspx.cs"
    Inherits="PhpaAll.Payroll.PaySalary" Title="Pay Salary" %>

<%@ Register Src="~/Controls/PrinterFriendlyButton.ascx" TagName="PrinterFriendlyButton"
    TagPrefix="uc2" %>
<%@ Register Src="~/Controls/SalaryDetail.ascx" TagName="SalaryDetail" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
        <script type="text/javascript">
            function DeleteConfirmation(e) {
                return confirm("If you pay this period, you can not make further changes to this Period.Pay Now..?");
            }
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphSideNavigation" runat="server">
    <uc2:PrinterFriendlyButton ID="PrinterFriendlyButton1" runat="server" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cph" runat="server">
    <p>
        <phpa:PhpaLinqDataSource ID="dsSalaryPeriod" runat="server" ContextTypeName="Eclipse.PhpaLibrary.Database.Payroll.PayrollDataContext"
            TableName="SalaryPeriods" EnableUpdate="True" AutoGenerateWhereClause="true" OnSelecting="dsSalaryPeriod_Selecting"
            RenderLogVisible="False">
            <WhereParameters>
                <asp:QueryStringParameter Name="SalaryPeriodId" QueryStringField="PeriodId" Type="Int32" />
            </WhereParameters>
        </phpa:PhpaLinqDataSource>
    </p>
    <div class="ParamInstructions">
        <asp:FormView ID="frmSalaryPeriod" runat="server" DataKeyNames="SalaryPeriodId" DataSourceID="dsSalaryPeriod"
            DefaultMode="Edit" OnItemUpdated="frmSalaryPeriod_ItemUpdated" OnItemCreated="frmSalaryPeriod_ItemCreated">
            <EditItemTemplate>
                <eclipse:TwoColumnPanel runat="server">
                    <eclipse:LeftLabel runat="server" Text="Period Code:" />
                    <asp:Label ID="lblSalPeriodCode" runat="server" Text='<%# Bind("SalaryPeriodCode") %>' />
                    <eclipse:LeftLabel runat="server" Text="Start From:" />
                    <asp:Label ID="ctlStart" runat="server" Text='<%# Bind("SalaryPeriodStart","{0:d}") %>' />
                    <eclipse:LeftLabel runat="server" Text="End To:" />
                    <asp:Label ID="lblEndTo" runat="server" Text='<%# Bind("SalaryPeriodEnd","{0:d}") %>' />
                    <eclipse:LeftLabel runat="server" Text="Description:" />
                    <asp:Label ID="lblDescription" runat="server" Text='<%# Bind("Description") %>' />
                    <eclipse:LeftLabel runat="server" Text="Payable Date:" />
                    <asp:Label ID="lblPayableDate" runat="server" Text='<%# Eval("PayableDate","{0:d}") %>' />
                    <eclipse:LeftLabel runat="server" Text="Paid Date:" />
                    <i:TextBoxEx ID="tbPaidDate" FriendlyName="Paid Date" runat="server" Text='<%# Bind("PaidDate", "{0:d}") %>'>
                        <Validators>
                            <i:Date OnServerValidate="CustomValidPaidDate_ServerValidate" />
                            <i:Required />
                        </Validators>
                    </i:TextBoxEx>
                    <br />
                    if specified period has a payable date, Paid date must be grater than or equal to
                    payable date. otherwise paid date must be greater than or equal to End To.
                </eclipse:TwoColumnPanel>
                <br />
                <i:LinkButtonEx ID="btnUpdate" runat="server" Action="Submit" Text="Paynow" OnClientClick="DeleteConfirmation"
                    CausesValidation="true" OnClick="btnUpdate_Click" />
                <i:LinkButtonEx ID="btnCancel" runat="server" Text="Cancel" CausesValidation="false"
                    Action="Reset" />
            </EditItemTemplate>
            <ItemTemplate>
                <eclipse:TwoColumnPanel ID="TwoColumnPanel1" runat="server">
                    <eclipse:LeftLabel ID="LeftLabel1" runat="server" Text="Period Code:" />
                    <asp:Label ID="Label1" runat="server" Text='<%# Eval("SalaryPeriodCode") %>' />
                    <eclipse:LeftLabel ID="LeftLabel2" runat="server" Text="Start From:" />
                    <asp:Label ID="Label2" runat="server" Text='<%# Eval("SalaryPeriodStart","{0:d}") %>' />
                    <eclipse:LeftLabel ID="LeftLabel3" runat="server" Text="End To:" />
                    <asp:Label ID="Label3" runat="server" Text='<%# Eval("SalaryPeriodEnd","{0:d}") %>' />
                    <eclipse:LeftLabel ID="LeftLabel4" runat="server" Text="Description:" />
                    <asp:Label ID="Label4" runat="server" Text='<%# Eval("Description") %>' />
                    <eclipse:LeftLabel ID="LeftLabel5" runat="server" Text="Payable Date:" />
                    <asp:Label ID="Label5" runat="server" Text='<%# Eval("PayableDate","{0:d}") %>' />
                    <eclipse:LeftLabel ID="LeftLabel6" runat="server" Text="Paid" />
                    <asp:Label ID="Label6" runat="server" Text='<%# Eval("PaidDate","{0:d}") %>' />
                </eclipse:TwoColumnPanel>
            </ItemTemplate>
            <FooterTemplate>
                <asp:Label ID="lblUpdateMsg" EnableViewState="true" runat="server"></asp:Label>
                <i:ValidationSummary runat="server" />
            </FooterTemplate>
        </asp:FormView>
    </div>
    <div>
        <br />
        <uc1:SalaryDetail ID="sdPayBill" runat="server"></uc1:SalaryDetail>
    </div>
</asp:Content>
