<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" CodeBehind="Ledger.aspx.cs"
    Inherits="PhpaAll.Reports.Ledger" Title="Ledger" EnableViewState="false" %>

<%@ Register Src="../Controls/PrinterFriendlyButton.ascx" TagName="PrinterFriendlyButton"
    TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphSideNavigation" runat="server">
    <asp:HyperLink ID="HyperLink2" runat="server" Text="Help" NavigateUrl="~/Doc/Ledger.doc.aspx" /><br />
    <uc2:PrinterFriendlyButton ID="PrinterFriendlyButton1" runat="server" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cph" runat="server">
    <div class="PanelContainer">
        <phpa:PhpaLinqDataSource ID="dsVouchers" runat="server" ContextTypeName="Eclipse.PhpaLibrary.Reporting.ReportingDataContext"
            TableName="RoVoucherDetails" OnSelecting="dsVouchers_Selecting" RenderLogVisible="False" />
        <eclipse:TwoColumnPanel ID="TwoColumnPanel1" runat="server" IsValidationContainer="true">
            <eclipse:LeftLabel runat="server" Text="From Date/TO Date" />
            <i:TextBoxEx ID="tbFromDate" runat="server" FriendlyName="from Date" QueryString="FromDate">
                <Validators>
                    <i:Date />
                    <i:Required />
                </Validators>
            </i:TextBoxEx>
            <i:TextBoxEx ID="tbToDate" runat="server" FriendlyName="to Date" QueryString="ToDate">
                <Validators>
                    <i:Date DateType="ToDate" />
                    <i:Required />
                </Validators>
            </i:TextBoxEx>
            <eclipse:LeftLabel runat="server" Text="Head of Account" />
            <i:AutoComplete ID="tbHeadOfAccount" runat="server" FriendlyName="HeadOfAccount"
                QueryString="HeadOfAccount" Width="20em" WebMethod="GetHeadOfAccount" WebServicePath="~/Services/HeadOfAccounts.asmx"
                ValidateWebMethodName="ValidateHeadOfAccount">
                <Validators>
                    <i:Required />
                </Validators>
            </i:AutoComplete>
            <eclipse:LeftLabel runat="server" Text="Division" />
            <phpa:PhpaLinqDataSource runat="server" ID="dsDivisions" ContextTypeName="Eclipse.PhpaLibrary.Reporting.ReportingDataContext"
                OrderBy="DivisionName" Select="new (DivisionId, DivisionName, DivisionGroup)"
                TableName="RoDivisions" Visible="True" RenderLogVisible="false">
            </phpa:PhpaLinqDataSource>
            <i:DropDownListEx runat="server" ID="ddlDivision" DataSourceID="dsDivisions" DataTextField="DivisionName"
                DataValueField="DivisionId" DataOptionGroupField="DivisionGroup" ClientIDMode="Static">
                <Items>
                    <eclipse:DropDownItem Text="(Any Division)" Value="" Persistent="Always" />
                </Items>
            </i:DropDownListEx>
            <br />
            <eclipse:LeftLabel ID="LeftLabel1" runat="server" Text="Station" />
            <phpa:PhpaLinqDataSource runat="server" ID="dsStations" ContextTypeName="Eclipse.PhpaLibrary.Database.PIS.PISDataContext"
                OnSelecting="dsStations_Selecting" RenderLogVisible="false">
            </phpa:PhpaLinqDataSource>
            <i:DropDownListEx ID="ddlStation" DataSourceID="dsStations" DataTextField="StationName"
                DataValueField="StationId" runat="server" FriendlyName="Station" Value='<%#Bind("StationId")%>'>
                <Items>
                    <eclipse:DropDownItem Text="(Not Set)" Persistent="Always" />
                </Items>                
            </i:DropDownListEx>
            <br />
            <i:ButtonEx ID="btnGo" runat="server" Text="Show Ledger Book" CausesValidation="true"
                Action="Submit" Icon="Refresh" />
            <i:ValidationSummary runat="server" />
        </eclipse:TwoColumnPanel>
        <div style="text-align: center">
            <asp:Label ID="lblHeadOfAccount" CssClass="headerStyle" runat="server" ForeColor="DarkGray"
                EnableViewState="true" />
        </div>
        <br />
        <div style="font-size: large; margin-bottom: 1mm">
            Opening Balance
            <asp:Label ID="lblOpeningBalance" ToolTip="Opening balance of head of account" runat="server"
                Width="40%" />
            Closing Balance
            <asp:Label ID="lblClosingBalance" ToolTip="Closing balance of head of account" runat="server" />
        </div>
        <jquery:GridViewEx ID="gvVouchers" runat="server" AllowPaging="false" AutoGenerateColumns="False"
            ShowFooter="True" DataKeyNames="VoucherId" DataSourceID="dsVouchers" AllowSorting="false"
            OnRowDataBound="gvVouchers_RowDataBound" OnDataBound="gvVouchers_DataBound">
            <Columns>
                <eclipse:SequenceField Visible="false" />
                <asp:HyperLinkField DataNavigateUrlFields="VoucherId" SortExpression="VoucherRefrance"
                    HeaderText="VR" DataTextField="VoucherRefrance" DataNavigateUrlFormatString="~/Finance/InsertVoucher.aspx?VoucherId={0}"
                    ItemStyle-Width="100" />
                <eclipse:MultiBoundField HeaderText="Date" SortExpression="VoucherDate" DataFields="VoucherDate"
                    DataFormatString="{0:d}" ToolTipFields="VoucherCode" ToolTipFormatString="Voucher Code: {0}" />
                <%--         <eclipse:MultiBoundField HeaderText="Voucher|Refrance" SortExpression="VoucherRefrance" DataFields="VoucherRefrance"
                     ToolTipFields="VoucherId"  ToolTipFormatString="Internal Voucher Id: {0}"
                     />--%>
                <eclipse:MultiBoundField HeaderText="Name" SortExpression="Name" DataFields="Name"
                    ItemStyle-Width="16em" DataFormatString="{0:d}" />
                <eclipse:MultiBoundField DataFields="Particulars" HeaderText="Particulars" SortExpression="Particulars"
                    ItemStyle-Width="25em" />
                <eclipse:MultiBoundField DataFields="Cheque" HeaderText="Cheque No" SortExpression="Cheque" />
                <eclipse:MultiBoundField DataFields="VoucherType" HeaderText="Voucher|Type" ToolTipFields="VoucherTypeCode"
                    ToolTipFormatString="Internal Voucher Type: {0}" />
                <eclipse:MultiBoundField DataFields="VoucherCode" HeaderText="Voucher|No" SortExpression="VoucherCode" />
                <eclipse:MultiBoundField DataFields="EmployeeCode" HeaderText="Code|Employee" SortExpression="EmployeeCode"
                    HideEmptyColumn="true" />
                <eclipse:MultiBoundField DataFields="JobCode" HeaderText="Code|Job" SortExpression="JobCode"
                    HideEmptyColumn="true" />
                <eclipse:MultiBoundField DataFields="Division" HeaderText="Division" SortExpression="Division"
                    FooterText="Total">
                    <ItemStyle Width="1.5in" />
                </eclipse:MultiBoundField>
                <eclipse:MultiBoundField DataFields="DebitAmount" HeaderText="Amount|Debit" SortExpression="DebitAmount"
                    AccessibleHeaderText="Debit" DataFormatString="{0:N2}" DataSummaryCalculation="ValueSummation">
                    <FooterStyle HorizontalAlign="Right"></FooterStyle>
                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                </eclipse:MultiBoundField>
                <eclipse:MultiBoundField DataFields="CreditAmount" HeaderText="Amount|Credit" SortExpression="CreditAmount"
                    AccessibleHeaderText="Credit" DataFormatString="{0:N2}" DataSummaryCalculation="ValueSummation">
                    <FooterStyle HorizontalAlign="Right"></FooterStyle>
                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                </eclipse:MultiBoundField>
                <eclipse:MultiBoundField DataFields="Balance" HeaderText="Amount|Balance(Dr-Cr)"
                    AccessibleHeaderText="Balance" DataFormatString="{0:N2}" DataSummaryCalculation="ValueSummation">
                    <FooterStyle HorizontalAlign="Right"></FooterStyle>
                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                </eclipse:MultiBoundField>
            </Columns>
        </jquery:GridViewEx>
    </div>
</asp:Content>
