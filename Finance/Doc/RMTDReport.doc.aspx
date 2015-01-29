<%@ Page Title="RMTD Report Doc" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="RMTDReport.doc.aspx.cs" Inherits="PhpaAll.Doc.RMTDReport" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphSideNavigation" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="contentRight" runat="server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cph" runat="server">
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="cphNoForm" runat="server">
 <ol>
        <li>Displays details of remittances recovered from the salary of deputationists</li>
        <li>All the adjustment received by deputationist during the input month will be displayed</li>
        <li>Default Month is current month</li>
        <li>Details of any month can be known by specifying the month</li>
        <li>Data will only be displayed if for the given period and given employee adjustment
            amounts are deduced from the salary and ParentOrganization for those exist. Debut
            (If data expected and not found):
            <ul>
                <li>Check whether expected employee's parent organization exist, this can be verified
                    from employee screen</li>
                <li>Verify whether amount for the given employees have been deduced during the given
                    period,this can be verified from Paybill or paybillRegister</li>
            </ul>
        </li>
    </ol>
</asp:Content>
