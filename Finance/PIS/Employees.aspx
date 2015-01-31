<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" CodeBehind="Employees.aspx.cs"
    Inherits="PhpaAll.Finance.ManageEmployee" ValidateRequest="false" Title="Manage Employees"
    EnableViewState="false" %>

<%@ Register Src="~/Controls/PrinterFriendlyButton.ascx" TagName="PrinterFriendlyButton"
    TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphSideNavigation" runat="server">
    <br />
    <asp:HyperLink ID="HyperLink2" runat="server" Text="Help" NavigateUrl="~/Doc/Employees.doc.aspx" /><br/>
    <uc2:PrinterFriendlyButton runat="server" />
    <br />
    <asp:LoginView ID="loginRestriction1" runat="server">
        <LoggedInTemplate>
            <i:ButtonEx runat="server" Text="Add New Employee..." Enabled="false" ToolTip="Only Payroll Managers are allowed to create employees" />
        </LoggedInTemplate>
        <RoleGroups>
            <asp:RoleGroup Roles="PayrollManager,Personnel">
                <ContentTemplate>
                    <i:ButtonEx runat="server" Text="Add New Employee..." OnClientClick="function(e) {
$('#dlgAddEmployee').ajaxDialog('load');
                    }" />
                    <jquery:Dialog runat="server" ID="dlgAddEmployee" Title="New Employee" ClientIDMode="Static"
                        AutoOpen="false" Position="Top" Width="600">
                        <Ajax Url="../PIS/EmployeeDetailsEdit.aspx" OnAjaxDialogClosing="function(event, ui) {                                                                                       
                       window.location='../PIS/EmployeeDetails.aspx?EmployeeId=' + ui.data;
                        }" />
                        <Buttons>
                            <jquery:RemoteSubmitButton RemoteButtonSelector="#btnUpdate" IsDefault="true" Text="Insert" />
                            <jquery:CloseButton />
                        </Buttons>
                    </jquery:Dialog>
                </ContentTemplate>
            </asp:RoleGroup>
        </RoleGroups>
    </asp:LoginView>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cph" runat="server">
    <p>
        Manage Employee will help you to keep track of your employees. Through this page
        you can add new employee and edit details of existing employees.
    </p>
    <jquery:Tabs runat="server" ID="tabEmployeeSearch" Collapsible="true" Selected="-1">
        <jquery:JPanel runat="server" HeaderText="Search">
            <eclipse:TwoColumnPanel runat="server">
                <eclipse:LeftLabel runat="server" Text="Employee" />
                <i:TextBoxEx ID="tbEmployee" runat="server" QueryString="Emp"  />
                <br />
                Will search within employee code, first name and last name
                <eclipse:LeftLabel runat="server" Text="File Index No" />
                <i:TextBoxEx ID="tbFileindexno" runat="server" FriendlyName="File Index No" />
                <eclipse:LeftLabel runat="server" Text="Division" />
                <phpa:PhpaLinqDataSource runat="server" ID="dsDivision" ContextTypeName="Eclipse.PhpaLibrary.Database.PIS.PISDataContext"
                    Select="new (DivisionId, DivisionName, DivisionGroup)" TableName="Divisions" RenderLogVisible="false" />
                <i:DropDownListEx runat="server" ID="ddlDivision" DataSourceID="dsDivision" DataTextField="DivisionName"
                    DataValueField="DivisionId" FriendlyName="Division" DataOptionGroupField="DivisionGroup">
                    <Items>
                        <eclipse:DropDownItem Text="(All)" Value="" Persistent="Always" />
                    </Items>
                </i:DropDownListEx>
                <eclipse:LeftLabel runat="server" Text="Designation" />
                <phpa:PhpaLinqDataSource runat="server" ID="dsDesignation" ContextTypeName="Eclipse.PhpaLibrary.Database.PIS.PISDataContext"
                    OnSelecting="dsDesignation_Selecting" RenderLogVisible="false">
                </phpa:PhpaLinqDataSource>
                <i:DropDownListEx runat="server" ID="ddlDesignation" FriendlyName="Designation" DataSourceID="dsDesignation">
                    <Items>
                        <eclipse:DropDownItem Text="(All)" Value="" Persistent="Always" />
                    </Items>
                </i:DropDownListEx>
                <eclipse:LeftLabel runat="server" Text="Employee Type" />
                <phpa:PhpaLinqDataSource runat="server" ID="dsEmployeeType" ContextTypeName="Eclipse.PhpaLibrary.Database.PIS.PISDataContext"
                    Select="new (EmployeeTypeId, Description)" TableName="EmployeeTypes" RenderLogVisible="false">
                </phpa:PhpaLinqDataSource>
                <i:DropDownListEx ID="ddlEmployeeType" runat="server" DataSourceID="dsEmployeeType"
                    QueryString="EmployeeTypeId" DataTextField="Description" DataValueField="EmployeeTypeId"
                    FriendlyName="Employee Type">
                    <Items>
                        <eclipse:DropDownItem Text="(All)" Value="" Persistent="Always" />
                    </Items>
                </i:DropDownListEx>
                <eclipse:LeftLabel runat="server" Text="Termination Reason" />
                <phpa:PhpaLinqDataSource runat="server" ID="dsEmployeeSatus" ContextTypeName="Eclipse.PhpaLibrary.Database.PIS.PISDataContext"
                    OrderBy="EmployeeStatusType" TableName="EmployeeStatus" RenderLogVisible="false" />
                <i:DropDownListEx runat="server" ID="ddlEmployeeStatus" DataSourceID="dsEmployeeSatus"
                    DataTextField="EmployeeStatusType" DataValueField="EmployeeStatusId">
                    <Items>
                        <eclipse:DropDownItem Text="(Not terminated)" Value="" Persistent="Always" />
                    </Items>
                </i:DropDownListEx>
                <eclipse:LeftLabel runat="server" Text="Nationality" />
                <i:RadioButtonListEx runat="server" ID="rblNationality" QueryString="IsBhutanese">
                    <Items>
                        <i:RadioItem Text="All" />
                        <i:RadioItem Text="Bhutanese" Value="1" />
                        <i:RadioItem Text="Foreigners" Value="0" />
                    </Items>
                </i:RadioButtonListEx>
            </eclipse:TwoColumnPanel>
            <i:ButtonEx runat="server" ID="btnSearch" AccessKey="S" Action="Submit" Text="Search"
                CausesValidation="true" Icon="Refresh" />
            <i:ButtonEx runat="server" ID="btnClearSearch" Action="Reset" AccessKey="L" Text="Clear Search"
                Icon="Cancel" CausesValidation="false" />
        </jquery:JPanel>
        <jquery:JPanel runat="server" HeaderText="More">
            <eclipse:TwoColumnPanel runat="server">
                <eclipse:LeftLabel runat="server" Text="Active On" />
                <i:TextBoxEx runat="server" ID="dtActiveOn" FriendlyName="Active On" QueryString="ActiveOnDate"
                    >
                    <Validators>
                        <i:Date />
                    </Validators>
                </i:TextBoxEx>
                <br />
                To see employees who were employed by the project as of a specific date, enter that
                date here.
                <eclipse:LeftPanel runat="server" Span="true">
                    <i:CheckBoxEx runat="server" ID="cbNoServicePeriod" Text="Service Period Not Defined"
                        CheckedValue="1" QueryString="NoServicePeriod" />
                    <br />
                    Check this to see a list of all active employees for whom service information has
                    not yet been entered.
                </eclipse:LeftPanel>
                <eclipse:LeftPanel runat="server" Span="true">
                    <i:CheckBoxEx runat="server" ID="cbToJoinEmployees" Text="New employees to join"
                        CheckedValue="1" QueryString="ToJoinEmployees" />
                    <br />
                    Check this to see a list of new employees joining in the next 30 days.
                </eclipse:LeftPanel>
                <eclipse:LeftPanel runat="server" Span="true">
                    <i:CheckBoxEx runat="server" ID="cbToTerminate" Text="New employees to retire" CheckedValue="1"
                        QueryString="ToTerminate" />
                    <br />
                    Check this to see a list of new employees to retire in the next 30 days.
                </eclipse:LeftPanel>
                <eclipse:LeftPanel runat="server" Span="true">
                    <i:CheckBoxEx runat="server" ID="cbNewlyJoined" Text="Newly joined employees" CheckedValue="1"
                        QueryString="NewlyJoined" />
                    <br />
                    Check this to see a list of newly joined employees in the last 30 days.
                </eclipse:LeftPanel>
                <eclipse:LeftPanel runat="server" Span="true">
                    <i:CheckBoxEx runat="server" ID="cbNewlyTerminated" Text="Newly retired employees"
                        CheckedValue="1" QueryString="NewlyTerminated" />
                    <br />
                    Check this to see a list of newly retired employees in the last 30 days.
                </eclipse:LeftPanel>
                <eclipse:LeftPanel runat="server" Span="true">
                    <i:CheckBoxEx runat="server" ID="cbNewlyPromoted" Text="Newly promoted employees"
                        CheckedValue="1" QueryString="NewlyPromoted" />
                    <br />
                    Check this to see a list of newly retired employees in the last 30 days.
                </eclipse:LeftPanel>
                <eclipse:LeftPanel runat="server" Span="true">
                    <i:CheckBoxEx runat="server" ID="cbCompleteprobation" Text="Complete probation" CheckedValue="1"
                        QueryString="ToCompleteprobation" />
                    <br />
                    Check this to see a list of complete their probation employees in the next 30 days.
                </eclipse:LeftPanel>
                <eclipse:LeftPanel runat="server" Span="true">
                    <i:CheckBoxEx runat="server" ID="cbPromotiondue" Text="Promotion due" CheckedValue="1"
                        QueryString="Promotiondue" />
                    <br />
                    Check this to see a list of employees their promotion due in the next 30 days.
                </eclipse:LeftPanel>
<%--                <eclipse:LeftPanel runat="server" Span="true">
                    <i:CheckBoxEx runat="server" ID="cbIncrementdue" Text="Increment due" CheckedValue="1"
                        QueryString="Incrementdue" />
                    <br />
                    Check this to see a list of employees their increment due in the next 30 days.
                </eclipse:LeftPanel>--%>
            </eclipse:TwoColumnPanel>
        </jquery:JPanel>
    </jquery:Tabs>
    <eclipse:AppliedFilters runat="server" ID="af" ContainerId="tabEmployeeSearch" ClientIDMode="Static" />
    <i:ValidationSummary runat="server" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphNoForm" runat="server">
    <phpa:PhpaLinqDataSource ID="dsEmployee" runat="server" ContextTypeName="Eclipse.PhpaLibrary.Database.PIS.PISDataContext"
        TableName="Employees" OrderBy="FirstName,EmployeeCode" RenderLogVisible="False"
        OnSelecting="dsEmployee_Selecting">
    </phpa:PhpaLinqDataSource>
    <jquery:GridViewEx ID="gvEmployee" runat="server" AutoGenerateColumns="False" DataKeyNames="EmployeeId"
        DataSourceID="dsEmployee" AllowPaging="true" PageSize="100" AllowSorting="True"
        Caption="List of Employees" EnableViewState="false" OnRowDataBound="gvEmployee_RowDataBound"
        ShowExpandCollapseButtons="false">
        <PagerSettings Position="TopAndBottom" Mode="NumericFirstLast"></PagerSettings>
        <Columns>
            <eclipse:SequenceField />
            <asp:TemplateField HeaderText="Name">
                <ItemTemplate>
                    <asp:MultiView runat="server" ID="mv">
                        <asp:View ID="View2" runat="server">
                            <%# Eval("FullName") %>
                        </asp:View>
                        <asp:View ID="View1" runat="server">
                            <a href='<%# Eval("EmployeeId", "EmployeeDetails.aspx?EmployeeId={0}")%>'>
                                <%# Eval("FullName") %></a>
                        </asp:View>
                    </asp:MultiView>
                </ItemTemplate>
            </asp:TemplateField>
            <eclipse:MultiBoundField DataFields="EmployeeCode" HeaderText="Employee Code" ShowHeader="true"
                SortExpression="EmployeeCode" />
            <eclipse:MultiBoundField DataFields="Designation" HeaderText="Designation" ShowHeader="true"
                SortExpression="Designation" />
            <eclipse:MultiBoundField DataFields="JoiningDate" HeaderText="Joined" SortExpression="JoiningDate"
                ItemStyle-Wrap="true" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:d}" />
            <eclipse:MultiBoundField DataFields="EmployeeType.Description" HeaderText="Type"
                SortExpression="EmployeeType.Description" ItemStyle-Wrap="true" ItemStyle-HorizontalAlign="Left" />
            <%--<eclipse:MultiBoundField DataFields="Office.OfficeName" HeaderText="Office" SortExpression="Office.OfficeName"
                ItemStyle-Wrap="true" ItemStyle-HorizontalAlign="Left" />--%>
            <eclipse:MultiBoundField DataFields="Gender" HeaderText="Gender" SortExpression="Gender" />
            <phpa:BoolField DataField="IsBhutanese" HeaderText="Nationality" TrueValue="Bhutanese"
                FalseValue="Foreigner" SortExpression="IsBhutanese" />
        </Columns>
        <EmptyDataTemplate>
            <b>No Employee exists.</b>
        </EmptyDataTemplate>
    </jquery:GridViewEx>
</asp:Content>
