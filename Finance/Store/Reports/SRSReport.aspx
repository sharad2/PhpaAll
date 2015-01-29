<%@ Page Title="GIN Report" Language="C#" MasterPageFile="~/MasterPage.master" CodeBehind="SRSReport.aspx.cs"
    Inherits="PhpaAll.Store.Reports.SRSReport" EnableViewState="false" %>

<%@ Register Src="~/Controls/PrinterFriendlyButton.ascx" TagName="PrinterFriendlyButton"
    TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        #gvIssueItems td
        {
            border: 1px solid;
            border-collapse: collapse;
        }
    </style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphSideNavigation" runat="server">
    <uc2:PrinterFriendlyButton ID="PrinterFriendlyButton1" runat="server" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cph" runat="server">
    <eclipse:TwoColumnPanel ID="TwoColumnPanel1" runat="server" WidthLeft="20%" IsValidationContainer="true">
        <eclipse:LeftLabel runat="server" Text="GIN No" />
        <i:TextBoxEx ID="tbSRSNo" runat="server" CaseConversion="UpperCase" QueryString="SrsId"
            TextAlign="Left">
            <Validators>
                <i:Required />
                <i:Value ValueType="Integer" MaxLength="10" Min="0" />
            </Validators>
        </i:TextBoxEx>
        <br />
        <a href="SRSList.aspx">Select from list</a>.
        <br />
        <i:ButtonEx ID="btnShowReport" runat="server" Text="Go" Icon="Refresh" Action="Submit"
            CausesValidation="true" IsDefault="true" />
        <i:ValidationSummary ID="valSummary1" runat="server" />
    </eclipse:TwoColumnPanel>
    <phpa:PhpaLinqDataSource ID="dsSRS" runat="server" ContextTypeName="Eclipse.PhpaLibrary.Database.Store.StoreDataContext"
        TableName="SRS" RenderLogVisible="False" OnSelecting="dsSRS_Selecting" AutoGenerateWhereClause="false">
    </phpa:PhpaLinqDataSource>
    <asp:FormView ID="fvSRS" runat="server" DataSourceID="dsSRS" OnItemCreated="fvSRS_ItemCreated"
        RenderOuterTable="false">
        <EmptyDataTemplate>
            No Data found.
        </EmptyDataTemplate>
        <ItemTemplate>
            <div style="float: left; width: 50%">
                <asp:LoginView ID="LoginView1" runat="server">
                    <AnonymousTemplate>
                        <asp:Label ID="lblError" runat="server" Text="Login to Issue or Edit this GIN." ForeColor="Red" />
                    </AnonymousTemplate>
                    <LoggedInTemplate>
                        <asp:HyperLink ID="btnIssue" Text="Issue" runat="server" CssClass="noprint" ToolTip='<%# Eval("SRSId", "Issue items  of GIN no {0}") %>'
                            NavigateUrl='<%# Eval("SRSId", "~/Store/IssueSRS.aspx?SRSId={0}") %>' />&nbsp;&nbsp;
                        <asp:HyperLink ID="btnEdit" runat="server" Text="Edit" CssClass="noprint" ToolTip='<%# Eval("SRSId","Edit GIN no {0}") %>'
                            NavigateUrl='<%# Eval("SRSId", "~/Store/CreateSRS.aspx?SRSId={0}") %>' />&nbsp;&nbsp;
                        <i:LinkButtonEx ID="btnRecalculate" runat="server" CssClasses="noprint" Text="Recalculate"
                            OnClick="btnRecalculate_Click" CausesValidation="false" />
                    </LoggedInTemplate>
                </asp:LoginView>
                <eclipse:TwoColumnPanel ID="panelDetails" runat="server" SkinID="PrintVisible">
                    <eclipse:LeftLabel runat="server" Text="GIN No." />
                    <asp:Label runat="server" Text='<%# Eval("SRSId") %>' />
                    <eclipse:LeftLabel runat="server" Text="SRS No." />
                    <asp:Label runat="server" Text='<%# Eval("SRSCode") %>' />
                    <eclipse:LeftLabel runat="server" Text="SRS Receive Date" />
                    <asp:Label runat="server" Text='<%# Eval("SRSCreateDate", "{0:d}") %>' />
                    <eclipse:LeftLabel runat="server" Text="GIN Create Date" />
                    <asp:Label runat="server" Text='<%#Eval("Created","{0:d}") %>' />
                    <eclipse:LeftLabel runat="server" Text="SRS From" />
                    <asp:Label runat="server" Text='<%# Eval("RoDivision1.DivisionName") %>' />
                    <eclipse:LeftLabel runat="server" Text="SRS To" />
                    <asp:Label runat="server" Text='<%# Eval("RoDivision2.DivisionName") %>' />
                    <eclipse:LeftLabel runat="server" Text="Vehicle No" />
                    <asp:Label runat="server" Text='<%# Eval("VehicleNumber") %>' />
                </eclipse:TwoColumnPanel>
            </div>
            <div style="float: left; width: 50%">
                <br />
                <eclipse:TwoColumnPanel ID="panelDetails1" runat="server" SkinID="PrintVisible" WidthRight="50%">
                    <eclipse:LeftLabel runat="server" Text="Material Issued To" />
                    <asp:Label runat="server" Text='<%# Eval("IssuedTo") %>' />
                    <eclipse:LeftLabel runat="server" Text="Chargeable To" />
                    <asp:Label runat="server" Text='<%# Eval("HeadOfAccount.DisplayName") %>' />
                    <asp:Label runat="server" Text='<%# Eval("HeadOfAccount.Description") %>' />
                    <eclipse:LeftLabel runat="server" Text="Approving Officer" />
                    <asp:Label runat="server" Text='<%# Eval("RoEmployee.FullName", "{0},")  %>' />
                    <asp:Label runat="server" Text='<%# Eval("RoEmployee.Designation") %>' />
                    <eclipse:LeftLabel runat="server" Text="Indenting Officer" />
                    <asp:Label runat="server" Text='<%# Eval("RoEmployee1.FullName", "{0},") %>' />
                    <asp:Label runat="server" Text='<%# Eval("RoEmployee1.Designation") %>' />
                    <eclipse:LeftLabel runat="server" Text="Issuing Officer" />
                    <asp:Label runat="server" Text='<%# Eval("RoEmployee3.FullName", "{0},") %>' />
                    <asp:Label runat="server" Text='<%# Eval("RoEmployee3.Designation") %>' />
                    <eclipse:LeftLabel runat="server" Text="Receiving Officer" />
                    <asp:Label runat="server" Text='<%# Eval("RoEmployee4.FullName", "{0},") %>' />
                    <asp:Label runat="server" Text='<%# Eval("RoEmployee4.Designation") %>' />
                    <br />
                </eclipse:TwoColumnPanel>
                <br />
            </div>
            <div style="clear: both">
                <br />
            </div>
            <phpa:PhpaLinqDataSource ID="dsIssueItems" runat="server" ContextTypeName="Eclipse.PhpaLibrary.Database.Store.StoreDataContext"
                TableName="SRS" RenderLogVisible="False" OnSelecting="dsIssueItems_Selecting"
                AutoGenerateWhereClause="false">
            </phpa:PhpaLinqDataSource>
            <jquery:GridViewEx ID="gvIssueItems" runat="server" AutoGenerateColumns="false" ShowFooter="true"
                DataSourceID="dsIssueItems" GridLines="Both" ClientIDMode="Static" CellPadding="2">
                <Columns>
                    <eclipse:MultiBoundField DataFields="HeadOfAccount,HOADescription" HeaderText="Head"
                        DataFormatString="{0}: {1}" EnableRowSpan="true" FooterText="Total" />
                    <eclipse:MultiBoundField DataFields="ItemCode,Description,Brand,Color,Identifier,Size,QtyRequired,ItemUnit"
                        HeaderText="Item" DataFormatString="{0}: {1} {2} {3} {4} {5} <strong>{6}&nbsp;{7}</strong>"
                        EnableRowSpan="true" />
                    <eclipse:MultiBoundField DataFields="QtyIssued" HeaderText="Issued|Quantity" DataFormatString="{0}">
                        <ItemStyle HorizontalAlign="Right" />
                    </eclipse:MultiBoundField>
                    <eclipse:MultiBoundField DataFields="Rate" HeaderText="Issued|Rate" DataFormatString="{0:N2}">
                        <ItemStyle HorizontalAlign="Right" />
                    </eclipse:MultiBoundField>
                    <eclipse:MultiBoundField DataFields="Amount" HeaderText="Issued|Amount" DataFormatString="{0:N2}"
                        DataSummaryCalculation="ValueSummation">
                        <ItemStyle HorizontalAlign="Right" />
                    </eclipse:MultiBoundField>
                    <eclipse:MultiBoundField DataFields="IssueDate" HeaderText="Issued|On" AccessibleHeaderText="IssueDate"
                        DataFormatString="{0:d}" EnableRowSpan="true" />
                    <eclipse:MultiBoundField DataFields="GRNId,GRNCode" HeaderText="Issued|GRN" EnableRowSpan="true"
                        DataFormatString="<a href='GRNReport.aspx?GrnId={0}'>{1}</a>" />
                    <eclipse:MultiBoundField DataFields="Remarks" HeaderText="Issued|Remarks" ItemStyle-Width="1in" />
                </Columns>
            </jquery:GridViewEx>
        </ItemTemplate>
    </asp:FormView>
    <br />
    <br />
    <br />
    <br />
    <div id="Div1" runat="server" style="width: 100%; text-align: center; font-size: large">
        <div id="Div2" runat="server" style="width: 33%; float: left">
            <asp:Label ID="lblIssuingOff" runat="server" Text="<b>Issuing Officer</b>" Visible="true" />
        </div>
        <div id="Div3" runat="server" style="width: 33%; float: left">
            <asp:Label ID="lblReceivingOff" runat="server" Text="<b>Receiving Officer</b>" Visible="true" />
        </div>
        <div id="Div4" runat="server" style="width: 33%; float: left">
            <asp:Label ID="lblApprovingOff" runat="server" Text="<b>Approved by Store Incharge</b>"
                Visible="true" />
        </div>
    </div>
</asp:Content>
