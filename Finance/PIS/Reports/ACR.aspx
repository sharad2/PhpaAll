<%@ Page Title="Annual Confidential Review Report" Language="C#" MasterPageFile="~/MasterPage.master"
    CodeBehind="ACR.aspx.cs" Inherits="PhpaAll.PIS.Reports.ACR" %>

<%@ Register Src="~/Controls/PrinterFriendlyButton.ascx" TagName="PrinterFriendlyButton"
    TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript">
        function btnACR_ClientClick(e) {
            var $gvACR = $('#gvACR');
            var $selectedRows = $gvACR.gridViewEx('selectedRows');
            if ($selectedRows.length == 0) { alert('Please select at least one row to update.'); return false; }
        }
    </script>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphSideNavigation" runat="server">
    <uc1:PrinterFriendlyButton runat="server" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cph" runat="server">
    <eclipse:TwoColumnPanel runat="server" ID="tcp">
        <eclipse:LeftLabel ID="LeftLabel1" runat="server" Text="Employee" />
        <i:TextBoxEx ID="tbEmployee" runat="server" QueryString="Emp"  />
        <br />
        Will search within employee code, first name and last name
        <eclipse:LeftLabel runat="server" />
        <i:RadioButtonListEx runat="server" ID="rbACRStatus" FriendlyName="ACR Status" Value="False"
            OnLoad="rbACRStatus_Load">
            <Items>
                <i:RadioItem Text="ACRComplete" Value="False" />
                <i:RadioItem Text="ACRPending" Value="True" />
            </Items>
        </i:RadioButtonListEx>
        <i:ButtonEx ID="btnApplyFilters" runat="server" Text="Apply Filters" Action="Submit"
            CausesValidation="true" Icon="Refresh" OnClick="btnApplyFilters_Click" IsDefault="true" />
    </eclipse:TwoColumnPanel>
    <eclipse:AppliedFilters runat="server" ID="af" ContainerId="tcp" />
    <i:ButtonEx ID="btnACRdone" runat="server" Text="ACR complete" Action="Submit" Icon="Refresh"
        OnClick="btnACRdone_Click" OnClientClick="btnACR_ClientClick" />
    <i:ButtonEx ID="btnAcrUndo" runat="server" Text="Undo ACR" Action="Submit" Icon="Refresh"
        OnClick="btnAcrUndo_Click" OnClientClick="btnACR_ClientClick" />
    <i:ValidationSummary ID="ValidationSummary1" runat="server" />
    <phpa:PhpaLinqDataSource runat="server" ID="dsACR" ContextTypeName="Eclipse.PhpaLibrary.Database.PIS.PISDataContext"
        TableName="Employees" OnSelecting="dsACR_Selecting" RenderLogVisible="false"
        EnableUpdate="true">
    </phpa:PhpaLinqDataSource>
    <jquery:GridViewEx ID="gvACR" runat="server" AutoGenerateColumns="false" DataSourceID="dsACR"
        DataKeyNames="EmployeeId" AllowPaging="true" PageSize="50" EnableViewState="true"
        ClientIDMode="Static" Caption="Annual Confidential Review completed/pending"
        OnLoad="gvACR_Load">
        <Columns>
            <eclipse:SequenceField />
            <jquery:SelectCheckBoxField />
            <asp:HyperLinkField DataTextField="FullName" HeaderText="Employee Name" DataNavigateUrlFields="EmployeeId"
                DataNavigateUrlFormatString="~/PIS/EmployeeDetails.aspx?EmployeeId={0}" SortExpression="FullName" />
            <eclipse:MultiBoundField DataFields="ACRDate" HeaderText="ACR Year" DataFormatString="{0:y}" />
            <eclipse:MultiBoundField DataFields="EmployeeType.Description" HeaderText="Type" />
            <eclipse:MultiBoundField DataFields="Designation" HeaderText="Designation" />
            <eclipse:MultiBoundField DataFields="Office.OfficeName" HeaderText="Office" />
        </Columns>
    </jquery:GridViewEx>
    <i:ButtonEx ID="btnACRdone1" runat="server" Text="ACR complete" Action="Submit" Icon="Refresh"
        OnClick="btnACRdone_Click" OnClientClick="btnACR_ClientClick" />
    <jquery:StatusPanel ID="sp" runat="server" Title="Status Message">
    </jquery:StatusPanel>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cphNoForm" runat="server">
</asp:Content>
