<%@ Page Title="Summary of Financial Progress in respect of Major Civil Packages"
    Language="C#" MasterPageFile="~/MasterPage.master" CodeBehind="PackageActivitySummary.aspx.cs"
    Inherits="Finance.MIS.SummaryofFinacialProgress.PackageActivitySummary" %>

<%@ Register Src="~/Controls/PrinterFriendlyButton.ascx" TagName="PrinterFriendlyButton"
    TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphSideNavigation" runat="server">
    <uc2:PrinterFriendlyButton ID="pfb" runat="server" />
    <asp:HyperLink ID="HyperLink2" runat="server" NavigateUrl="~/MIS/Default.aspx">MIS Home</asp:HyperLink>
    <br />
    <asp:HyperLink ID="HyperLink3" runat="server" NavigateUrl="~/MIS/SummaryofFinacialProgress/PackageActivityList.aspx">Package activity list</asp:HyperLink>
    <br />
    <asp:HyperLink ID="HyperLink4" runat="server" NavigateUrl="~/MIS/SummaryofFinacialProgress/PackageActivityData.aspx">Package activity data entries</asp:HyperLink>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cph" runat="server">
    <eclipse:TwoColumnPanel runat="server" ID="tcp">
        <eclipse:LeftLabel runat="server" Text="Package Report" />
        <phpa:PhpaLinqDataSource ID="dsPackagesReport" runat="server" RenderLogVisible="False"
            ContextTypeName="Eclipse.PhpaLibrary.Database.PackageActivityDataContext" TableName="PackageReports">
        </phpa:PhpaLinqDataSource>
        <i:DropDownListEx runat="server" ID="ddlPackagesReport" DataSourceID="dsPackagesReport"
            DataTextField="Description" DataValueField="PackageReportId" FriendlyName="Package Report">
        </i:DropDownListEx>
        <eclipse:LeftLabel runat="server" Text="Package Progress Between" />
        <i:TextBoxEx ID="tbFromDate" runat="server" FriendlyName="Package Progress From">
            <Validators>
                <i:Date />
            </Validators>
        </i:TextBoxEx>
        to
        <i:TextBoxEx ID="tbToDate" runat="server" FriendlyName="Package Progress Untill">
            <Validators>
                <i:Date DateType="ToDate" />
            </Validators>
        </i:TextBoxEx>
        <i:ButtonEx ID="ButtonEx1" runat="server" Action="Submit" Icon="Refresh" CausesValidation="true"
            Text="Go" IsDefault="true" />
        <br />
        Status as on this date.
    </eclipse:TwoColumnPanel>
    <i:ValidationSummary runat="server" />
    <phpa:PhpaLinqDataSource ID="ds" runat="server" ContextTypeName="Eclipse.PhpaLibrary.Database.PackageActivityDataContext"
        TableName="PackageActivityTransactionDetails" OnSelecting="ds_Selecting" RenderLogVisible="false">
    </phpa:PhpaLinqDataSource>
    <jquery:GridViewEx ID="gv" runat="server" AutoGenerateColumns="false" DataSourceID="ds"
        AllowSorting="true" ShowFooter="true">
        <Columns>
            <asp:BoundField HeaderText="Contract Package" DataField="PackageName" FooterText="Total" />
            <jquery:MatrixField DataHeaderFields="GroupDescription,ActivityDescription,ActivityColumnNumber"
                DataHeaderSortFields="GroupColumnNumber,ActivityColumnNumber" DataHeaderFormatString="{0}|{1}<br/>{2:'('#')'}"
                DataHeaderCustomFields="CalculatedFormula" DataValueFields="PackageActivtiyData"
                DataMergeFields="PackageId" DataValueFormatString="{0:N2}" OnMatrixRowDataBound="gv_MatrixRowDataBound"
                DisplayColumnTotals="true">
            </jquery:MatrixField>
            <eclipse:MultiBoundField DataFields="Remarks" HeaderText="Last ra bill no./Remarks"
                HideEmptyColumn="true" />
        </Columns>
    </jquery:GridViewEx>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cphNoForm" runat="server">
</asp:Content>
