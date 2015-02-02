<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" Inherits="_Default" EnableViewState="false"
    CodeBehind="Default.aspx.cs" Title="PHPA Home Page" AutoEventWireup="True" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .ImageLink image
        {
            border-style: none;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cph" runat="Server">
    <font size="5" face="algerian" color="blue">
        <marquee behavior="ALTERNATE"><%= ConfigurationManager.AppSettings["PrintTitle"]%> </marquee>
        <br />
        <br />
        <asp:Table runat="server">
            <asp:TableRow>
                <asp:TableCell>
                    <asp:HyperLink ID="HyperLink6" runat="server" Text="Finance" NavigateUrl="~/Finance/Finance.aspx">Finance</asp:HyperLink>
                </asp:TableCell>
                <asp:TableCell Width="5em">
                    <asp:HyperLink ID="HyperLink2" runat="server" Text="Finance" CssClass="ImageLink"
                        ImageUrl="~/Images/Finance.png" NavigateUrl="~/Finance/Finance.aspx">Finance</asp:HyperLink>
                </asp:TableCell>
                <asp:TableCell>
                    <asp:HyperLink ID="HyperLink7" runat="server" NavigateUrl="~/Payroll/Payroll.aspx">Payroll</asp:HyperLink>
                </asp:TableCell>
                <asp:TableCell Width="5em">
                    <asp:HyperLink ID="HyperLink1" CssClass="ImageLink" ImageUrl="~/Images/Payroll.png"
                        runat="server" NavigateUrl="~/Payroll/Payroll.aspx">Payroll</asp:HyperLink>
                </asp:TableCell>
                <asp:TableCell>
                    <asp:HyperLink ID="HyperLink8" runat="server" NavigateUrl="~/Store/Store.aspx">Store</asp:HyperLink>
                </asp:TableCell>
                <asp:TableCell Width="5em">
                    <asp:HyperLink ID="HyperLink3" runat="server" CssClass="ImageLink" ImageUrl="~/Images/warehouse.png"
                        NavigateUrl="~/Store/Store.aspx">Store</asp:HyperLink>
                </asp:TableCell>
                <asp:TableCell>
                    <asp:HyperLink ID="HyperLink9" runat="server" NavigateUrl="~/PIS/Default.aspx">PIS</asp:HyperLink>
                </asp:TableCell>
                <asp:TableCell Width="5em">
                    <asp:HyperLink ID="HyperLink4" runat="server" CssClass="ImageLink" ImageUrl="~/Images/PIS.png"
                        NavigateUrl="~/PIS/Default.aspx">PIS</asp:HyperLink>
                </asp:TableCell>
                <asp:TableCell>
                    <asp:HyperLink ID="HyperLink10" runat="server" NavigateUrl="~/MIS/Default.aspx">MIS</asp:HyperLink>
                </asp:TableCell>
                <asp:TableCell Width="5em">
                    <asp:HyperLink ID="HyperLink5" runat="server" CssClass="ImageLink" ImageUrl="~/Images/MIS.png"
                        NavigateUrl="~/MIS/Default.aspx">MIS</asp:HyperLink>
                </asp:TableCell>
            </asp:TableRow>
        </asp:Table>
        </font>
</asp:Content>
