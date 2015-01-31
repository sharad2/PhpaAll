<%@ Page Title="Package Activity" Language="C#" MasterPageFile="~/MasterPage.master"
    AutoEventWireup="true" CodeBehind="PackageActivityData.aspx.cs" EnableViewState="true"
    Inherits="PhpaAll.MIS.FinancialActivityData" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            $('#gv input:text').bind('change', function (e) {
                $('#gv').gridViewEx('selectRows', e, $(this).closest('tr'));
            })
        });

        function btn_ClientClick(e) {
            var $gv = $('#gv');
            var $selectedRows = $gv.gridViewEx('selectedRows');
            if ($selectedRows.length == 0) { alert('Please select at least one row to Insert.'); return false; }
        }
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphSideNavigation" runat="server">
    <asp:HyperLink ID="HyperLink2" runat="server" NavigateUrl="~/MIS/Default.aspx">MIS Home</asp:HyperLink>
    <br />
    <asp:HyperLink ID="HyperLink3" runat="server" NavigateUrl="~/MIS/SummaryofFinacialProgress/PackageActivityList.aspx">Package activity list</asp:HyperLink>
    <br />
    <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="~/MIS/SummaryofFinacialProgress/PackageActivitySummary.aspx">Package activity summary</asp:HyperLink>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cph" runat="server">
    <eclipse:TwoColumnPanel runat="server" ID="tcp" IsValidationContainer="true">
        <eclipse:LeftLabel runat="server" Text="Package Report" />
        <phpa:PhpaLinqDataSource ID="dsPackagesReport" runat="server" RenderLogVisible="False"
            ContextTypeName="Eclipse.PhpaLibrary.Database.PackageActivityDataContext" TableName="PackageReports">
        </phpa:PhpaLinqDataSource>
        <i:DropDownListEx runat="server" ID="ddlPackagesReport" DataSourceID="dsPackagesReport"
            DataTextField="Description" DataValueField="PackageReportId" FriendlyName="Package Report">
        </i:DropDownListEx>
        <eclipse:LeftLabel runat="server" Text="Package" />
        <phpa:PhpaLinqDataSource ID="dsPackages" runat="server" RenderLogVisible="false"
            ContextTypeName="Eclipse.PhpaLibrary.Database.PackageActivityDataContext" TableName="Packages"
            OrderBy="PackageName">
        </phpa:PhpaLinqDataSource>
        <i:DropDownListEx runat="server" ID="ddlPackages" DataSourceID="dsPackages" DataTextField="PackageName"
            DataValueField="PackageId" FriendlyName="Package">
        </i:DropDownListEx>
        <eclipse:LeftLabel runat="server" Text="Activity Progress Date last one month" />
        <phpa:PhpaLinqDataSource ID="dsProgressDate" runat="server" RenderLogVisible="False"
            ContextTypeName="Eclipse.PhpaLibrary.Database.PackageActivityDataContext" TableName="PackageActivityTransactions"
            OnSelecting="dsProgressDate_Selecting">
        </phpa:PhpaLinqDataSource>
        <i:DropDownSuggest runat="server" ID="ddlProgressDate" DataSourceID="dsProgressDate"
            FriendlyName="Activity Progress Date" DataTextField="PackageActivityDate" DataValueField="PackageActivityDate"
            DataTextFormatString="{0:d}">
            <Items>
                <eclipse:DropDownItem Text="(Select Date)" Persistent="Always" />
            </Items>
            <TextBox ID="tbProgressDate" runat="server" FriendlyName="Activity Progress Date">
                <Validators>
                    <i:Date />
                </Validators>
            </TextBox>
            <Validators>
                <i:Required />
            </Validators>
        </i:DropDownSuggest>
        <i:ButtonEx ID="btnApplyFilter" runat="server" Action="Submit" Icon="Refresh" CausesValidation="true"
            Text="Go" OnClick="btnApplyFilter_Click" IsDefault="true"/>
        <br />
        Select a recent date or enter any date.
        <i:ValidationSummary runat="server" />
    </eclipse:TwoColumnPanel>
    <jquery:StatusPanel ID="sp_status" runat="server">
        <Ajax UseDialog="false" />
    </jquery:StatusPanel>
    <jquery:JPanel ID="JPanel1" runat="server" IsValidationContainer="true">
        <asp:PlaceHolder runat="server" ID="ph">
            <i:ButtonEx runat="server" Text="Save" ID="btnSaveProgress" Icon="Refresh" CausesValidation="true"
                OnClick="btnSaveProgress_Click" Action="Submit" OnClientClick="btn_ClientClick" />
            <br />
            Remarks/Last ra bill no:
            <i:TextArea runat="server" ID="tbRemarks">
                <Validators>
                    <i:Value ValueType="String" MaxLength="256" />
                </Validators>
            </i:TextArea>
        </asp:PlaceHolder>
        <i:ValidationSummary ID="ValidationSummary1" runat="server" />
        <phpa:PhpaLinqDataSource ID="ds" runat="server" RenderLogVisible="false" ContextTypeName="Eclipse.PhpaLibrary.Database.PackageActivityDataContext"
            TableName="FinancialProgresses" OnSelecting="ds_Selecting" EnableViewState="false">
        </phpa:PhpaLinqDataSource>
        <jquery:GridViewEx ID="gv" runat="server" AutoGenerateColumns="false" DataSourceID="ds"
            DataKeyNames="PackageActivityId,ActivityTransactionDetailId" ShowExpandCollapseButtons="false"
            ClientIDMode="Static" EnableViewState="true" OnDataBound="gv_DataBound" DefaultSortExpression="GroupDescription;$;">
            <Columns>
                <eclipse:SequenceField />
                <eclipse:MultiBoundField DataFields="GroupDescription" HeaderText="Group" SortExpression="GroupDescription" />
                <jquery:SelectCheckBoxField />
                <eclipse:MultiBoundField DataFields="Description" HeaderText="Description" />
                <asp:TemplateField HeaderText="Progress Data" AccessibleHeaderText="ProgressData"
                    ItemStyle-HorizontalAlign="Right">
                    <ItemTemplate>
                        <i:TextBoxEx ID="tbPackageActivityData" runat="server" Text='<%#Bind("PackageActivtiyData","{0:0.00}") %>'
                            FriendlyName='<%#  Eval("Description", "{0}") %>'>
                            <Validators>
                                <i:Value ValueType="Decimal" MaxLength="15" />
                            </Validators>
                        </i:TextBoxEx>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </jquery:GridViewEx>
    </jquery:JPanel>
    <asp:HiddenField ID="hfPackagesReport" runat="server" />
    <asp:HiddenField ID="hfPackages" runat="server" />
    <asp:HiddenField ID="hfDate" runat="server" />
    <asp:HiddenField ID="hfActivityTransactionId" runat="server" />
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cphNoForm" runat="server">
</asp:Content>
