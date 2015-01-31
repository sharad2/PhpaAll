<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" CodeBehind="Finance.aspx.cs"
    Inherits="PhpaAll.Finance.Finance" Title="Finance Home -- PHPA" EnableViewState="false" %>
<asp:Content ID="c1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="c3" ContentPlaceHolderID="cphSideNavigation" runat="server">
</asp:Content>
<asp:Content ID="c2" ContentPlaceHolderID="cph" runat="server">
    <p>
        First time user?
        <asp:HyperLink runat="server" NavigateUrl="~/Finance/LearnMore.aspx">Learn more...</asp:HyperLink>
        <br />
        Select one of the common tasks from the left or review the list of everything &nbsp;you
        can do below.
    </p>
    <phpa:PhpaLinqDataSource ID="dsVouchers" runat="server" ContextTypeName="Eclipse.PhpaLibrary.Database.FinanceDataContext"
        OnSelecting="dsVouchers_Selecting" RenderLogVisible="false">
    </phpa:PhpaLinqDataSource>
    <jquery:GridViewEx runat="server" DataSourceID="dsVouchers" AutoGenerateColumns="false"
        Caption="Recently Created Vouchers. List updated every 3 hours." ID="gvRecentVouchers">
        <Columns>
            <asp:BoundField DataField="Created" DataFormatString="{0:d}" HeaderText="Created On" />
            <asp:HyperLinkField DataTextField="CountVouchers" DataTextFormatString="{0:N0}" HeaderText="Vouchers|#"
                ItemStyle-HorizontalAlign="Right" DataNavigateUrlFields="Created" DataNavigateUrlFormatString="~/Finance/VoucherSearch.aspx?Created={0:d}" />
            <asp:TemplateField HeaderText="Vouchers|Dated">
                <ItemTemplate>
                    <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl='<%# string.Format("~/Finance/VoucherSearch.aspx?DateFrom={0:d}&DateTo={0:d}&Created={1:d}", Eval("MinVoucherDate"), Eval("Created")) %>'><%# Eval("MinVoucherDate", "{0:d}")%></asp:HyperLink>
                    to
                    <asp:HyperLink ID="HyperLink2" runat="server" NavigateUrl='<%# string.Format("~/Finance/VoucherSearch.aspx?DateFrom={0:d}&DateTo={0:d}&Created={1:d}", Eval("MaxVoucherDate"), Eval("Created")) %>'><%# Eval("MaxVoucherDate", "{0:d}")%></asp:HyperLink>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="By">
                <ItemTemplate>
                    <eclipse:MultiValueLabel ID="MultiValueLabel1" runat="server" DataFields="CountCreatedBy,MinCreatedBy,MaxCreatedBy" />
                </ItemTemplate>
            </asp:TemplateField>
            <eclipse:MultiBoundField DataFields="Debits" HeaderText="Voucher Amount|Total" DataFormatString="{0:C0}">
                <ItemStyle HorizontalAlign="Right" />
            </eclipse:MultiBoundField>
            <asp:TemplateField HeaderText="Voucher Amount|Largest">
                <ItemStyle HorizontalAlign="Right" />
                <ItemTemplate>
                    <asp:HyperLink runat="server" NavigateUrl='<%# Eval("MaxDebitInfo.VoucherId", "~/Finance/InsertVoucher.aspx?VoucherId={0}") %>'>
                    <%# Eval("MaxDebitInfo.Amount", "{0:C0}")%>
                    </asp:HyperLink>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </jquery:GridViewEx>
</asp:Content>
