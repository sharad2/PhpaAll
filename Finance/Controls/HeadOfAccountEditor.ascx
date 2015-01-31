<%@ Control Language="C#" Inherits="PhpaAll.HeadOfAccountEditor" CodeBehind="HeadOfAccountEditor.ascx.cs" %>
<phpa:PhpaLinqDataSource ID="dsEditAccounts" runat="server" ContextTypeName="Eclipse.PhpaLibrary.Database.FinanceDataContext"
    EnableDelete="True" EnableInsert="True" EnableUpdate="True" TableName="HeadOfAccounts"
    Where="HeadOfAccountId == @HeadOfAccountId" RenderLogVisible="False" OnSelecting="dsEditAccounts_Selecting"
    OnInserted="dsEditAccounts_Inserted">
    <UpdateParameters>
        <asp:Parameter Name="HeadOfAccountId" Type="Int32" />
        <asp:Parameter Name="NameWithinParent" Type="Int16" />
        <asp:Parameter Name="RevisedProjectCost" Type="Decimal" />
        <asp:Parameter Name="ProjectCost" Type="Decimal" />
        <asp:Parameter Name="ParentHeadOfAccountId" Type="Int32" />
        <asp:Parameter Name="HeadOfAccountType" Type="String" ConvertEmptyStringToNull="true" />
        <asp:Parameter Name="StationId" Type="Int32" ConvertEmptyStringToNull="true" />
    </UpdateParameters>
    <InsertParameters>
        <asp:Parameter Name="HeadOfAccountId" Type="Int32" />
        <asp:Parameter Name="NameWithinParent" Type="Int16" />
        <asp:Parameter Name="ParentHeadOfAccountId" Type="Int32" />
        <asp:Parameter Name="RevisedProjectCost" Type="Decimal" />
        <asp:Parameter Name="ProjectCost" Type="Decimal" />
        <asp:Parameter Name="HeadOfAccountType" Type="String" ConvertEmptyStringToNull="true" />
        <asp:Parameter Name="StationId" Type="Int32" ConvertEmptyStringToNull="true" />
    </InsertParameters>
    <WhereParameters>
        <asp:Parameter Name="HeadOfAccountId" Type="Int32" DefaultValue="0" />
    </WhereParameters>
</phpa:PhpaLinqDataSource>
<phpa:PhpaLinqDataSource ID="dsHead" runat="server" RenderLogVisible="false" ContextTypeName="Eclipse.PhpaLibrary.Reporting.ReportingDataContext"
    TableName="RoHeadHierarchies">
</phpa:PhpaLinqDataSource>
<asp:HiddenField ID="hdNewHead" runat="server" />
<asp:HiddenField ID="hdNewSubHead" runat="server" />
<phpa:PhpaLinqDataSource ID="dsAccountTypes" runat="server" ContextTypeName="Eclipse.PhpaLibrary.Reporting.ReportingDataContext"
    TableName="HeadOfAccounts" RenderLogVisible="false" OnSelecting="dsAccountTypes_Selecting">
</phpa:PhpaLinqDataSource>
<asp:FormView ID="fvEdit" runat="server" DataKeyNames="HeadOfAccountId,ParentHeadOfAccountId"
    DataSourceID="dsEditAccounts" Width="100%" OnDataBound="fvEdit_DataBound" OnItemInserting="fvEdit_ItemInserting"
    OnItemInserted="fvEdit_ItemInserted" OnItemDeleted="fvEdit_ItemDeleted" OnItemUpdated="fvEdit_ItemUpdated"
    OnItemUpdating="fvEdit_ItemUpdating">
    <HeaderTemplate>
        <asp:Label ID="lblHeader" runat="server" />
    </HeaderTemplate>
    <FooterTemplate>
        <asp:HyperLink ID="hlSeeLedger" runat="server" NavigateUrl="~/Reports/Ledger.aspx"
            OnPreRender="hlSeeLedger_PreRender" EnableViewState="false">See the ledger for this head</asp:HyperLink>
        <phpa:FormViewStatusMessage ID="status" runat="server" />
    </FooterTemplate>
    <EmptyDataTemplate>
        <phpa:FormViewStatusMessage ID="status" runat="server" />
        Select an account head below to view its details here. You will also be able to
        edit it.
        <br />
        <i:LinkButtonEx ID="btnNewTopHead" runat="server" Action="Submit" CausesValidation="False"
            RolesRequired="*" Text="New Top Level Head" OnClick="btnNewTopHead_Click" ToolTip="Add a new top level head" />
        &nbsp;<i:LinkButtonEx ID="btnEmptyRefreshTree" runat="server" OnClick="btnRefreshTree_Click"
            Action="Submit" CausesValidation="False" ToolTip="If you have made changes to this head of account, clicking this will also update the tree"
            Text="Refresh Tree" />
    </EmptyDataTemplate>
    <EditItemTemplate>
        <eclipse:TwoColumnPanel runat="server">
            <%-- <eclipse:LeftLabel runat="server" Text="Current Number" />
            <asp:Label ID="lblDisplayName" runat="server" Text='<%# Bind("DisplayName") %>' Visible='<%# Eval("ParentHead") != null %>' />--%>
            <eclipse:LeftLabel ID="lblParentName" runat="server" Text='<%# string.Format("Change to {0}", Eval("ParentHead.DisplayName", "{0}.")) %>'
                EnableViewState="true" />
            <i:TextBoxEx ID="tbNameWithinParent" runat="server" Text='<%# Bind("NameWithinParent") %>'
                Size="5">
                <Validators>
                    <i:Required />
                    <i:Value ValueType="Integer" Min="0" Max="999" />
                </Validators>
            </i:TextBoxEx>
            <eclipse:LeftLabel runat="server" Text="Name" />
            <i:TextBoxEx ID="txtDescription" runat="server" Text='<%# Bind("Description") %>'
                MaxLength="200" Size="40">
                <Validators>
                    <i:Required />
                </Validators>
            </i:TextBoxEx>
            <br />
            Any text which will help you identify the purpose of this account head.
            <eclipse:LeftLabel runat="server" Text="Comment" />
            <i:TextBoxEx ID="txtGroup" runat="server" Text='<%# Bind("GroupDescription") %>'
                MaxLength="90" Size="20">
            </i:TextBoxEx>
            <br />
            Optional space for any comment pertaining to this head.
            <eclipse:LeftLabel runat="server" Text="Project Cost" />
            <i:TextBoxEx ID="mtbProjectCost" runat="server" Text='<%# Bind("ProjectCost", "{0:f2}") %>'>
            </i:TextBoxEx>
            <br />
            The total amount of expenditure you expect to incur under this head during the lifetime
            of the project.
            <eclipse:LeftLabel runat="server" Text="Revised Cost" />
            <i:TextBoxEx ID="tbRevisedCost" runat="server" Text='<%# Bind("RevisedProjectCost", "{0:f2}") %>'
                FriendlyName="Revised Project Cost">
            </i:TextBoxEx>
            <br />
            If your estimate changes, you can retain the old project cost and enter a new value
            here.
            <eclipse:LeftLabel runat="server" Text="Specify Balance Sheet Type" />
            <i:DropDownListEx2 ID="ddlAccountTypes" runat="server" Value='<%# Eval("HeadOfAccountType") %>'
                DataSourceID="dsAccountTypes" DataFields="Value,Text,OptionGroup" DataTextFormatString="{1}"
                DataValueFormatString="{0}" DataOptionGroupFormatString="{2}">
                <Items>
                    <eclipse:DropDownItem Persistent="Always" Value="" Text="(Not Specified)" />
                </Items>
            </i:DropDownListEx2>
             <eclipse:LeftLabel ID="LeftLabel4" runat="server" Text="Station" />
          <%--  <asp:TextBox ID="txtStation" runat="server" Text='<%# Bind("Station") %>' />--%>
            <phpa:PhpaLinqDataSource runat="server" ID="dsStations" ContextTypeName="Eclipse.PhpaLibrary.Database.PIS.PISDataContext"
                OnSelecting="dsStations_Selecting" RenderLogVisible="false">
            </phpa:PhpaLinqDataSource>
            <i:DropDownListEx ID="ddlStation" DataTextField="StationName" DataValueField="StationId"  DataSourceID="dsStations" runat="server" FriendlyName="Station" Value='<%#Bind("StationId")%>'>
            <Items>
            <eclipse:DropDownItem Persistent="Always" Value="" Text="(All)" />
            </Items>
            </i:DropDownListEx>
            <asp:Label ID="lblstation" runat="server" Text="Head of Account visible to users of which station.Station is use full in case of Head of Account is Bank Account  "></asp:Label>
        </eclipse:TwoColumnPanel>
        <i:ButtonEx runat="server" ID="btnSave" Text="Save" CausesValidation="true" Action="Submit"
            Icon="Disk" OnClick="btnSave_Click" />
        <i:LinkButtonEx ID="btnCan" runat="server" Text="Cancel" CausesValidation="false"
            OnClick="btnCancel_Click" />
        <i:ValidationSummary runat="server" />
    </EditItemTemplate>
    <ItemTemplate>
        <jquery:Tabs runat="server">
            <jquery:JPanel runat="server" ID="Panel2" HeaderText="Details">
                <asp:Label ID="Label2" runat="server" Text='<%# Eval("ChildHeads.Count", "Contains {0} subheads") %>'
                    Font-Italic="True"></asp:Label>
                <eclipse:TwoColumnPanel runat="server">
                    <eclipse:LeftLabel runat="server" Text="Comment" />
                    <asp:Label ID="Label1" runat="server" Text='<%# Eval("GroupDescription") %>' />
                    <eclipse:LeftLabel ID="LeftLabel1" runat="server" Text="Project Cost" />
                    <asp:Label ID="Label3" runat="server" Text='<%# Eval("ProjectCost", "{0:C}") %>' />
                    <eclipse:LeftLabel ID="LeftLabel2" runat="server" Text="Revised Cost" />
                    <asp:Label ID="Label4" runat="server" Text='<%# Eval("RevisedProjectCost", "{0:C}") %>' />
                    <eclipse:LeftLabel ID="LeftLabel3" runat="server" Text="Type" />
                    <asp:Label ID="Label5" runat="server" Text='<%# Eval("AccountType.Description") %>' />
                </eclipse:TwoColumnPanel>
            </jquery:JPanel>
            <phpa:AuditTabPanel ID="panelAudit" runat="server" />
        </jquery:Tabs>
        <i:LinkButtonEx ID="btnEdit" runat="server" Text="Edit" CausesValidation="false"
            OnClick="btnEdit_Click" RolesRequired="*" />
        <i:LinkButtonEx ID="btnDelete" runat="server" Text="Delete" OnClick="btnDeleteNew_Click"
            Action="Submit" CausesValidation="false" OnClientClick="DeleteConfirmation" ToolTip="Disabled if subheads exist. You must delete the subheads first."
            OnPreRender="btnDelete_PreRender" RolesRequired="*" />
        <i:LinkButtonEx ID="btnNewHead" runat="server" Text="New Head" OnClick="btnNewHead_Click"
            Action="Submit" CausesValidation="false" ToolTip="Add a new head which is a sibling of this head"
            OnPreRender="btnNewHead_PreRender" RolesRequired="*" />
        <i:LinkButtonEx ID="btnNewSubHead" runat="server" Text="New Subhead" OnClick="btnNewSubHead_Click"
            Action="Submit" CausesValidation="false" ToolTip="Add a new subhead below this head"
            OnPreRender="btnNewSubhead_PreRender" RolesRequired="*" />
        <i:LinkButtonEx ID="btnClose" runat="server" OnClick="btnClose_Click" Action="Submit"
            Text="Close" />
    </ItemTemplate>
</asp:FormView>
