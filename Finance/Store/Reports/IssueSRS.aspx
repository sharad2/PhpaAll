<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" CodeBehind="IssueSRS.aspx.cs"
    Inherits="PhpaAll.Store.Reports.IssueSRS" Title="Issue Items" EnableViewState="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphSideNavigation" runat="server">
    <asp:HyperLink ID="HyperLink2" runat="server" Text="Help" NavigateUrl="~/Doc/IssueSRS.doc.aspx" />
    <fieldset>
        <legend>Quick Links</legend>
        <ul>
            <li>
                <asp:HyperLink runat="server" NavigateUrl="~/Store/Reports/SRSReport.aspx" Text="GIN Report" />
            </li>
            <li>
                <asp:HyperLink runat="server" NavigateUrl="~/Store/CreateSRS.aspx" Text="Create New SRS" />
            </li>
            <li>
                <asp:HyperLink runat="server" NavigateUrl="~/Store/Reports/StockBalance.aspx" Text="View Stock Balance" />
            </li>
            <li>
                <asp:HyperLink runat="server" NavigateUrl="~/Store/Reports/ItemLedger.aspx" Text="Item Ledger" />
            </li>
        </ul>
    </fieldset>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cph" runat="server">
    <phpa:PhpaLinqDataSource ID="dsSRSIssue" runat="server" ContextTypeName="Eclipse.PhpaLibrary.Database.Store.StoreDataContext"
        TableName="SRS" RenderLogVisible="False" OnSelecting="dsSRSIssue_Selecting" AutoGenerateWhereClause="false">
    </phpa:PhpaLinqDataSource>
    <eclipse:TwoColumnPanel ID="panelContainer" runat="server" IsValidationContainer="true">
        <eclipse:LeftLabel runat="server" Text="GIN No" />
        <i:TextBoxEx ID="tbSRSNo" runat="server" CaseConversion="UpperCase" TextAlign="Left"
            Size="10">
            <Validators>
                <i:Required />
                <i:Value ValueType="Integer" Min="0" MaxLength="10" />
            </Validators>
        </i:TextBoxEx>
        <br />
        Enter GIN No to issue or <a href="SRSList.aspx">select from list</a>.<br />
        <i:ButtonEx ID="btnShowSRS" runat="server" Text="Go" CausesValidation="true" Icon="Refresh"
            Action="Submit" OnClick="btnShowSRS_Click" IsDefault="true" />
        <i:ValidationSummary ID="valSum" runat="server" />
    </eclipse:TwoColumnPanel>
    <asp:FormView ID="fvSRSIssue" runat="server" OnItemCreated="fvSRSIssue_ItemCreated"
        DataSourceID="dsSRSIssue">
        <ItemTemplate>
            <div style="float: left; width: 50%">
                <eclipse:TwoColumnPanel ID="panelDetails" runat="server" SkinID="PrintVisible" WidthLeft="50%"
                    WidthRight="50%">
                    <eclipse:LeftLabel runat="server" Text="GIN No." />
                    <asp:Label runat="server" Text='<%# Eval("SRSId") %>' />
                    <eclipse:LeftLabel runat="server" Text="SRS No." />
                    <asp:Label runat="server" Text='<%# Eval("SRSCode") %>' />
                    <eclipse:LeftLabel runat="server" Text="SRS Create Date" />
                    <asp:Label runat="server" Text='<%# Eval("SRSCreateDate", "{0:d}") %>' />
                    <eclipse:LeftLabel runat="server" Text="SRS From" />
                    <asp:Label runat="server" Text='<%# Eval("RoDivision1.DivisionName") %>' />
                    <eclipse:LeftLabel runat="server" Text="SRS To" />
                    <asp:Label runat="server" Text='<%# Eval("RoDivision2.DivisionName") %>' />
                    <eclipse:LeftLabel ID="LeftLabel5" runat="server" Text="Vehicle No" />
                    <asp:Label runat="server" Text='<%# Eval("VehicleNumber") %>' />
                </eclipse:TwoColumnPanel>
            </div>
            <div style="float: left; width: 45%; margin-left: 2mm">
                <eclipse:TwoColumnPanel ID="panelDetails1" runat="server" SkinID="PrintVisible" WidthLeft="50%"
                    WidthRight="50%">
                    <eclipse:LeftLabel runat="server" Text="Material Issued To" />
                    <asp:Label runat="server" Text='<%# Eval("IssuedTo") %>' />
                    <eclipse:LeftLabel runat="server" Text="Chargeable To" />
                    <asp:Label runat="server" Text='<%# Eval("HeadOfAccount.DisplayName") %>' />
                    <asp:Label runat="server" Text='<%# Eval("HeadOfAccount.Description") %>' />
                    <eclipse:LeftLabel runat="server" Text="Approving Officer" />
                    <asp:Label runat="server" Text='<%# Eval("RoEmployee.FullName") %>' />
                    <eclipse:LeftLabel runat="server" Text="Indenting Officer" />
                    <asp:Label runat="server" Text='<%# Eval("RoEmployee1.FullName") %>' />
                    <eclipse:LeftLabel runat="server" Text="Issuing Officer" />
                    <asp:Label runat="server" Text='<%# Eval("RoEmployee3.FullName") %>' />
                    <eclipse:LeftLabel runat="server" Text="Receiving Officer" />
                    <asp:Label runat="server" Text='<%# Eval("RoEmployee4.FullName") %>' />
                </eclipse:TwoColumnPanel>
            </div>
            <div class="ui-helper-clearfix">
            </div>
            <jquery:JPanel runat="server" IsValidationContainer="true" EnableViewState="true">
                <jquery:GridViewEx ID="gvSRSItems" runat="server" AutoGenerateColumns="false" Caption="List of SRS Items"
                    DataKeyNames="SRSItemId,ItemId" ShowFooter="true" OnRowDataBound="gvSRSItems_RowDataBound"
                    EnableViewState="true">
                    <Columns>
                        <eclipse:SequenceField>
                        </eclipse:SequenceField>
                        <eclipse:MultiBoundField DataFields="ItemCode" HeaderText="Item|Code" ToolTipFields="SRSItemId"
                            ToolTipFormatString="{0}" AccessibleHeaderText="ItemCode" />
                        <eclipse:MultiBoundField DataFields="ItemDescription" HeaderText="Item|Description" />
                        <eclipse:MultiBoundField DataFields="QtyReq,ItemUnit" HeaderText="Quantity|Required"
                            DataFormatString="{0} {1}">
                            <ItemStyle HorizontalAlign="Right" />
                        </eclipse:MultiBoundField>
                        <asp:HyperLinkField DataTextField="QtyIssued" HeaderText="Quantity|Issued" DataNavigateUrlFormatString="~/Store/Reports/DivisionSRS.aspx?ItemId={0}&SRSId={1}"
                            DataNavigateUrlFields="ItemId,SRSId" DataTextFormatString="{0:#,###;(#,###);''}">
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:HyperLinkField>
                        <asp:HyperLinkField HeaderText="Quantity|In Stock" DataTextField="QtyAvailable" DataNavigateUrlFields="ItemId"
                            DataNavigateUrlFormatString="~/Store/Reports/ItemLedger.aspx?ItemId={0}">
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:HyperLinkField>
                        <asp:TemplateField HeaderText="Quantity|Issue">
                            <ItemStyle HorizontalAlign="Right" />
                            <ItemTemplate>
                                <i:TextBoxEx runat="server" ID="tbIssue" EnableViewState="true" MaxLength="6">
                                    <Validators>
                                        <i:Value ValueType="Integer" Max="0" Min="0" />
                                    </Validators>
                                </i:TextBoxEx>
                                <br />
                                <asp:Literal runat="server" ID="litUnissue">
                                Negative to unissue
                                </asp:Literal>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Remarks">
                            <ItemTemplate>
                                <i:TextArea ID="tbRemarks" runat="server" Text='<%# Bind("Remarks") %>' FriendlyName="Remarks"
                                    Cols="30" Rows="3" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <EmptyDataTemplate>
                        No Data found for the given GIN.
                    </EmptyDataTemplate>
                </jquery:GridViewEx>
                <i:ButtonEx ID="btnIssuesSRS" runat="server" Text="Issue Items" OnClick="btnIssuesSRS_Click"
                    ToolTip="Click to Issue Items for the Selected GIN" CausesValidation="true" Action="Submit" />
                <asp:HyperLink ID="HyperLink1" runat="server" Text="Cancel" NavigateUrl='<%# Eval("SRSId", "~/Store/CreateSRS.aspx?SRSId={0}") %>' />
                <i:ValidationSummary runat="server" />
            </jquery:JPanel>
        </ItemTemplate>
        <EmptyDataTemplate>
            No Data found.
        </EmptyDataTemplate>
    </asp:FormView>
</asp:Content>
