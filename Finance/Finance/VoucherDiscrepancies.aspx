<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" CodeBehind="VoucherDiscrepancies.aspx.cs"
    Inherits="PhpaAll.Vouchers" Title="Vouchers" %>

<%@ Register Src="../Controls/PrinterFriendlyButton.ascx" TagName="PrinterFriendlyButton"
    TagPrefix="uc2" %>
<%@ Register Src="../Controls/VoucherDetailControl.ascx" TagName="VoucherDetailControl"
    TagPrefix="uc1" %>
<asp:Content ID="c1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphSideNavigation" runat="server">
    <uc2:PrinterFriendlyButton ID="PrinterFriendlyButton1" runat="server" />
</asp:Content>
<asp:Content ID="c3" ContentPlaceHolderID="cph" runat="server">
    <div class="PanelContainer" style="width: 32%; min-width: 60em">
        <div class="ParamInstructions">
            Select a voucher to view its details or edit it.
        </div>
        <br />
        <br />
        <uc1:VoucherDetailControl ID="ctlVoucherDetail" runat="server" InformationLevel="Summary"
            OnVoucherDetailSelecting="ctlVoucherDetail_Selecting">
            <EmptyTemplate>
                <asp:Label ID="lblMessage" runat="server" OnPreRender="lblMessage_PreRender" />
            </EmptyTemplate>
        </uc1:VoucherDetailControl>
    </div>
</asp:Content>
