<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" Inherits="Finance.AccountHeads"
    Title="Manage Head of Accounts" CodeBehind="AccountHeads.aspx.cs" EnableEventValidation="false" %>

<%--Event validation is being turned off because otherwise we are having problems expanding a tree noder
after a click of the node has updated the UpdatePanel--%>
<%@ Register Src="~/Controls/HeadOfAccountEditor.ascx" TagName="HeadOfAccountEditor"
    TagPrefix="uc1" %>
<asp:Content ID="c1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .AspNet-Menu ul li
        {
            height: 35px;
            padding: 2px;
            vertical-align: middle;
        }
    </style>
    <script type="text/javascript">
        function DeleteConfirmation(e) {
            return confirm("Deletion will succeed only if no vouchers exist for this head. Are you sure you want to delete this head?");
        }

        function NoMessage(e) {
            return false;
        }
    </script>
</asp:Content>
<asp:Content ID="c4" ContentPlaceHolderID="cphSideNavigation" runat="Server">
    <jquery:Accordion runat="server" ID="ctlHelpfulInfo" Collapsible="true" SelectedIndex="-1">
        <jquery:JPanel ID="JPanel1" runat="server" HeaderText="Legends">
            <asp:Menu ID="Menu2" runat="server" Orientation="Vertical" CssClass="AspNet-Menu"
                RenderingMode="List">
                <Items>
                    <asp:MenuItem ImageUrl="~/Images/assets.jpg" Text=" Assets" Selectable="false" />
                    <asp:MenuItem ImageUrl="~/Images/liabilities.jpg" Text=" Liabilities" Selectable="false" />
                    <asp:MenuItem ImageUrl="~/Images/expenditure.jpg" Text=" Expenditure" Selectable="false" />
                    <asp:MenuItem ImageUrl="~/Images/receipts.jpg" Text=" Receipts" Selectable="false" />
                    <asp:MenuItem ImageUrl="~/Images/cash.jpg" Text=" Cash Account" Selectable="false" />
                    <asp:MenuItem ImageUrl="~/Images/Bank.jpg" Text=" Bank Account" Selectable="false" />
                </Items>
            </asp:Menu>
        </jquery:JPanel>
    </jquery:Accordion>
</asp:Content>
<asp:Content ID="c3" ContentPlaceHolderID="cph" runat="Server">
    <p>
        You can review all heads of accounts on this page. Click on a specific account to
        review and edit its details and to insert new heads under it.
    </p>
    <phpa:PhpaLinqDataSource ID="dsAccountTypes" runat="server" ContextTypeName="Eclipse.PhpaLibrary.Reporting.ReportingDataContext"
        OrderBy="Category,Description" Select="new (HeadOfAccountType, Description, Category)"
        TableName="RoAccountTypes" RenderLogVisible="false">
    </phpa:PhpaLinqDataSource>
    <fieldset>
        <legend>Common tasks</legend>Mark nodes of type
        <i:DropDownListEx ID="ddlAccountTypes" runat="server" DataSourceID="dsAccountTypes"
            DataTextField="Description" DataValueField="HeadOfAccountType">
            <Items>
                <eclipse:DropDownItem Text="(Hide Marks)" Persistent="Always" />
            </Items>
        </i:DropDownListEx>
        <i:ButtonEx ID="btnMark" runat="server" Text="Mark" OnClick="btnMark_Click" Action="Submit"
            Icon="Check" />
        <i:ButtonEx ID="btnClearMark" runat="server" Text="Cancel" Action="Reset" Icon="Refresh" />
        <br />
        <i:ButtonEx ID="btnExpand" runat="server" Text="Expand All" OnClick="btnExpand_Click"
            Action="Submit" ToolTip="Click to expand account heads" Icon="PlusThick" />
        <i:ButtonEx ID="btnCollapse" runat="server" Text="Collapse All" OnClick="btnCollapse_Click"
            Action="Submit" ToolTip="Click to collapse account heads" Icon="MinusThick" />
        <hr />
        <i:LinkButtonEx ID="btnNewTop" runat="server" Action="Submit" CausesValidation="False"
            RolesRequired="*" Text="New Top Level Head" OnClick="btnNewTopHead_Click" ToolTip="Create a new top level head" />
    </fieldset>
    <phpa:HeadsHierarchicalDataSourceControl ID="dsHeads" runat="server" />
    <jquery:Dialog runat="server" Title="Head of Account Editor" Position="RightTop"
        EnableViewState="true" EnablePostBack="true" Width="400">
        <ContentTemplate>
            <uc1:HeadOfAccountEditor ID="ctlEditor" runat="server" />
        </ContentTemplate>
    </jquery:Dialog>
    <div>
        <asp:TreeView ID="tvHeads" runat="server" DataSourceID="dsHeads" ExpandDepth="1"
            OnSelectedNodeChanged="tvHeads_SelectedNodeChanged" NodeWrap="True" OnTreeNodeDataBound="tvHeads_TreeNodeDataBound"
            SkinID="Heads">
            <DataBindings>
                <asp:TreeNodeBinding TextField="DisplayDescription" ValueField="HeadOfAccountId"
                    PopulateOnDemand="true" ToolTipField="HeadOfAccountId" SelectAction="Select" />
            </DataBindings>
        </asp:TreeView>
    </div>
</asp:Content>
