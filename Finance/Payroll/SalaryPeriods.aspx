<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" CodeBehind="SalaryPeriods.aspx.cs"
    Inherits="Finance.Payroll.SalaryPeriods" Title="Salary Period" %>

<%@ Register Src="~/Controls/PrinterFriendlyButton.ascx" TagName="PrinterFriendlyButton"
    TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function OnSalaryPeriodUpdate(e) {
            var paidDate = $('#tbPaidDate').val();
            if (paidDate != '') {
                return confirm("Since Paid Date has been entered, you will not be able to delete this Salary Period");
            }
            return true;
        }

        function DeleteConfirmation(e) {
            var msg = $('#hfDeleteConfirmMessage').val();
            if (msg != '') {
                return confirm(msg);
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphSideNavigation" runat="server">
    <uc2:PrinterFriendlyButton ID="PrinterFriendlyButton1" runat="server" />
    <br />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cph" runat="server">
    <p>
        Through this page you can add new salary period and edit details of existing salary
        periods. This will help you to create payroll for Periods that are created from
        this page.
    </p>
    <asp:HiddenField ID="hfDeleteConfirmMessage" runat="server" ClientIDMode="Static" />
    <jquery:Tabs ID="tabSalaryPeriodSearch" runat="server" Selected="0" Collapsible="false">
        <jquery:JPanel ID="JPanel1" runat="server" HeaderText="Search" IsValidationContainer="true">
            <eclipse:TwoColumnPanel ID="TwoColumnPanel1" runat="server">
                <eclipse:LeftLabel ID="LeftLabel1" runat="server" Text="Salary Period" />
                <i:TextBoxEx ID="tbFromDate" runat="server" FriendlyName="From Date">
                    <Validators>
                        <i:Date />
                    </Validators>
                </i:TextBoxEx>
                <i:TextBoxEx ID="tbToDate" runat="server" FriendlyName="To Date">
                    <Validators>
                        <i:Date DateType="ToDate" />
                    </Validators>
                </i:TextBoxEx>
                <br />
                By default, salary period from last six months to upcoming three months is shown.
                <br />
                <i:ButtonEx ID="btnLookUp" runat="server" Text="Look Up" OnClick="btnLookUp_Click"
                    Action="Submit" Icon="Search" CausesValidation="true" IsDefault="true" />
                <i:ButtonEx ID="btnShowAllPeriods" runat="server" Text="Show All Periods" CausesValidation="false"
                    Action="Reset" Icon="Refresh" />
                <i:ValidationSummary ID="vsSalaryPeriod" runat="server" />
            </eclipse:TwoColumnPanel>
        </jquery:JPanel>
    </jquery:Tabs>
    <br />
    <i:ButtonEx ID="btnNew" runat="server" Text="New Salary Period" OnClick="btnNew_Click"
        RolesRequired="*" Action="Submit" ToolTip="Click to create new salary period ."
        CausesValidation="false" Icon="PlusThick" />
    <phpa:PhpaLinqDataSource ID="dsSalaryPeriod" runat="server" ContextTypeName="Eclipse.PhpaLibrary.Database.Payroll.PayrollDataContext"
        TableName="SalaryPeriods" RenderLogVisible="False" OnSelecting="dsSalaryPeriod_Selecting">
    </phpa:PhpaLinqDataSource>
    <br />
    <b>*List displays only those salary periods which belong to station of user, in case
        of administrator showing all</b>
    <jquery:GridViewEx ID="gvSalaryPeriod" runat="server" AutoGenerateColumns="False"
        EnableViewState="false" DataKeyNames="SalaryPeriodId" DataSourceID="dsSalaryPeriod"
        OnSelectedIndexChanged="gvSalaryPeriod_SelectedIndexChanged" PageSize="12" AllowSorting="True"
        Caption="Salary Period" AllowPaging="True" OnRowDataBound="gvSalaryPeriod_RowDataBound">
        <Columns>
            <eclipse:SequenceField>
            </eclipse:SequenceField>
            <asp:TemplateField ItemStyle-CssClass="noprint" HeaderStyle-CssClass="noprint">
                <ItemTemplate>
                    <asp:LinkButton ID="LinkButton1" runat="server" CommandName="Select" ToolTip="Click to see Salary Period Details."
                        Text="Select">
                    </asp:LinkButton>
                </ItemTemplate>
            </asp:TemplateField>
            <eclipse:MultiBoundField DataFields="SalaryPeriodStart,SalaryPeriodEnd" HeaderToolTip="Start date of period"
                DataFormatString="{0:d MMM} - {1:d MMM yyyy}" HeaderText="Salary Period" SortExpression="SalaryPeriodStart" />
            <eclipse:MultiBoundField DataFields="SalaryPeriodCode,Description" DataFormatString="{0}: {1}"
                HeaderText="Description" SortExpression="SalaryPeriodCode" HeaderToolTip="Salary period code" />
            <eclipse:MultiBoundField DataFields="PayableDate" DataFormatString="{0:d}" HeaderToolTip="Salary Payable Date"
                HeaderText="Payable On" SortExpression="PayableDate" />
            <eclipse:MultiBoundField DataFields="PaidDate" DataFormatString="{0:d}" HeaderToolTip="Salary Payment Date"
                HeaderText="Paid On" SortExpression="PaidDate" />
            <eclipse:MultiBoundField DataFields="NetPay" HeaderText="Net Pay" SortExpression="NetPay"
                DataFormatString="{0:N0}" ItemStyle-HorizontalAlign="Right" />
            <asp:TemplateField AccessibleHeaderText="Employees" HeaderText="# Employees" ItemStyle-CssClass="noprint"
                HeaderStyle-CssClass="noprint">
                <ItemStyle HorizontalAlign="Right" />
                <HeaderTemplate>
                    <asp:Label ID="lblHeaderToolTip" runat="server" ToolTip="Number of employees for given salary period."
                        Text="# Employees" />
                </HeaderTemplate>
                <ItemTemplate>
                    <asp:HyperLink NavigateUrl='<%# Eval("SalaryPeriodId", "~/Payroll/ManageEmployeePeriod.aspx?PeriodId={0}") %>'
                        runat="server" ID="hlManageEmployees" ToolTip="Click here to manage employees for this period"
                        Text='<%#Eval("TotalEmployees") %>'></asp:HyperLink>
                </ItemTemplate>
            </asp:TemplateField>
            <eclipse:MultiBoundField DataFields="StationName" HeaderText="Station" SortExpression="StationName"
                ItemStyle-CssClass="noprint" HeaderStyle-CssClass="noprint" />
        </Columns>
        <EmptyDataTemplate>
            <b>No Salary Period exist.</b>
        </EmptyDataTemplate>
    </jquery:GridViewEx>
    <jquery:Dialog ID="dlgEditor" runat="server" Title="Salary Period Editor" Width="500"
        Position="RightTop" EnableViewState="true" EnablePostBack="true" Visible="false">
        <ContentTemplate>
            <phpa:PhpaLinqDataSource ID="dsSpecificSalaryPeriod" runat="server" ContextTypeName="Eclipse.PhpaLibrary.Database.Payroll.PayrollDataContext"
                EnableInsert="True" EnableUpdate="True" TableName="SalaryPeriods" RenderLogVisible="False"
                OnSelecting="dsSpecificSalaryPeriod_Selecting" Where="SalaryPeriodId == @SalaryPeriodId"
                EnableDelete="True" OnInserted="dsSpecificSalaryPeriod_Inserted" OnInserting="dsSpecificSalaryPeriod_Inserting"
                OnDeleting="dsSpecificSalaryPeriod_Deleting">
                <InsertParameters>
                    <asp:Parameter Name="SalaryPeriodCode" Type="String" />
                    <asp:Parameter Name="SalaryPeriodStart" Type="DateTime" />
                    <asp:Parameter Name="SalaryPeriodEnd" Type="DateTime" />
                    <asp:Parameter Name="PayableDate" Type="DateTime" />
                    <asp:Parameter Name="PaidDate" Type="DateTime" />
                    <asp:Parameter Name="StationId" Type="Int32" ConvertEmptyStringToNull="true" />
                </InsertParameters>
                <WhereParameters>
                    <asp:Parameter Name="SalaryPeriodId" Type="Int32" />
                </WhereParameters>
                <UpdateParameters>
                    <asp:Parameter Name="SalaryPeriodCode" Type="String" />
                    <asp:Parameter Name="SalaryPeriodStart" Type="DateTime" />
                    <asp:Parameter Name="SalaryPeriodEnd" Type="DateTime" />
                    <asp:Parameter Name="PayableDate" Type="DateTime" />
                    <asp:Parameter Name="StationId" Type="Int32" ConvertEmptyStringToNull="true" />
                    <asp:Parameter Name="MRNumber" Type="String" ConvertEmptyStringToNull="true" />
                </UpdateParameters>
            </phpa:PhpaLinqDataSource>
            <asp:FormView ID="frmSalaryPeriod" runat="server" DataKeyNames="SalaryPeriodId" DataSourceID="dsSpecificSalaryPeriod"
                OnItemDeleted="frmSalaryPeriod_ItemDeleted" OnItemInserted="frmSalaryPeriod_ItemInserted"
                OnItemUpdated="frmSalaryPeriod_ItemUpdated" OnItemCreated="frmSalaryPeriod_ItemCreated">
                <EmptyDataTemplate>
                    Please select any salary period from the list to view and edit its details.<br />
                    <asp:LoginView ID="restrictUser" runat="server">
                        <RoleGroups>
                            <asp:RoleGroup Roles="PayrollOperator">
                                <ContentTemplate>
                                    Click on the following link to create new salary period.<br />
                                    <i:LinkButtonEx ID="btnNewSalaryPeriod" runat="server" Text="New Salary Period" OnClick="btnNew_Click"
                                        Action="Submit" ToolTip="Click to create new salary period ." CausesValidation="false" />
                                </ContentTemplate>
                            </asp:RoleGroup>
                        </RoleGroups>
                        <LoggedInTemplate>
                            <b>You must be at least a Payroll Operator to create or manage salary periods.</b>
                        </LoggedInTemplate>
                    </asp:LoginView>
                </EmptyDataTemplate>
                <HeaderTemplate>
                    Salary Period <em>
                        <%# Eval("SalaryPeriodCode")%></em>:
                    <%# Eval("EmployeePeriods.Count") %>
                    employees
                </HeaderTemplate>
                <ItemTemplate>
                    <jquery:Tabs ID="tabSalaryPeriod" runat="server" Selected="0">
                        <jquery:JPanel runat="server" HeaderText="Details" ID="panelDetails">
                            <eclipse:TwoColumnPanel ID="TwoColumnPanel2" runat="server">
                                <eclipse:LeftLabel ID="LeftLabel2" runat="server" Text="Period Code:" />
                                <asp:Label ID="Label1" runat="server" Text='<%# Eval("SalaryPeriodCode") %>' />
                                <eclipse:LeftLabel ID="LeftLabel3" runat="server" Text="Start From:" />
                                <asp:Label ID="Label2" runat="server" Text='<%# Eval("SalaryPeriodStart","{0:d}") %>' />
                                <eclipse:LeftLabel ID="LeftLabel4" runat="server" Text="End To:" />
                                <asp:Label ID="Label3" runat="server" Text='<%# Eval("SalaryPeriodEnd","{0:d}") %>' />
                                <eclipse:LeftLabel ID="LeftLabel5" runat="server" Text="Description:" />
                                <asp:Label ID="Label4" runat="server" Text='<%# Eval("Description") %>' />
                                <%--<eclipse:LeftLabel ID="LeftLabel6" runat="server" Text="Bank Name:" />
                                <asp:Label ID="Label5" runat="server" Text='<%# Eval("BankName")%>' />--%>
                                <eclipse:LeftLabel ID="LeftLabel14" runat="server" Text="Station Name:" />
                                <asp:Label ID="Label7" runat="server" Text='<%# Eval("Station.StationName")%>' />
                                <eclipse:LeftLabel ID="LeftLabel7" runat="server" Text="Payable Date:" />
                                <asp:Label ID="Label6" runat="server" Text='<%# Eval("PayableDate","{0:d}") %>' />
                                <eclipse:LeftLabel ID="LeftLabel8" runat="server" Text="Paid Date:" />
                                <asp:Label runat="server" ID="lblPaidDate" Text='<%# Eval("PaidDate","{0:d}") %>' />
                                <eclipse:LeftLabel ID="LeftLabel19" runat="server" Text="M.R. Number:" />
                                <asp:Label runat="server" ID="tbMRNumber" Text='<%# Eval("MRNumber") %>' />
                            </eclipse:TwoColumnPanel>
                        </jquery:JPanel>
                        <phpa:AuditTabPanel ID="panelAudit" runat="server" />
                    </jquery:Tabs>
                    <asp:LoginView ID="SalaryPeriodLoginView" runat="server">
                        <RoleGroups>
                            <asp:RoleGroup Roles="PayrollManager">
                                <ContentTemplate>
                                    <i:LinkButtonEx ID="btnEdit" runat="server" Text="Edit" CausesValidation="false"
                                        OnClick="btnEdit_Click" />
                                    <i:LinkButtonEx ID="btnDelete" runat="server" Text="Delete" OnClick="btnDelete_Click"
                                        Action="Submit" CausesValidation="false" OnPreRender="btnDelete_PreRender" OnClientClick="DeleteConfirmation" />
                                    <i:LinkButtonEx ID="btnNew" runat="server" Text="New" OnClick="btnNew_Click" Action="Submit"
                                        CausesValidation="false" />
                                    <br />
                                </ContentTemplate>
                            </asp:RoleGroup>
                        </RoleGroups>
                        <LoggedInTemplate>
                            You must be a Payroll Manager to create or manage adjustment details.
                        </LoggedInTemplate>
                    </asp:LoginView>
                </ItemTemplate>
                <EditItemTemplate>
                    <eclipse:TwoColumnPanel ID="X1" runat="server">
                        <eclipse:LeftLabel ID="LeftLabel9" runat="server" Text="Period Code" />
                        <i:TextBoxEx ID="tbsalaryPeriodCode" runat="server" Text='<%# Bind("SalaryPeriodCode") %>'
                            CaseConversion="UpperCase" MaxLength="30">
                            <Validators>
                                <i:Required />
                            </Validators>
                        </i:TextBoxEx>
                        <eclipse:LeftLabel ID="LeftLabel10" runat="server" Text="From / To" />
                        <i:TextBoxEx ID="tbFromDateEdit" runat="server" FriendlyName="From Date" Value='<%# Bind("SalaryPeriodStart", "{0:d}") %>'>
                            <Validators>
                                <i:Required />
                                <i:Date />
                            </Validators>
                        </i:TextBoxEx>
                        <i:TextBoxEx ID="tbToDateEdit" runat="server" FriendlyName="To Date" Value='<%# Bind("SalaryPeriodEnd", "{0:d}" ) %>'>
                            <Validators>
                                <i:Required />
                                <i:Date DateType="ToDate" />
                            </Validators>
                        </i:TextBoxEx>
                        <eclipse:LeftLabel ID="LeftLabel11" runat="server" Text="Description" />
                        <i:TextBoxEx ID="tbPeriodDescription" runat="server" Text='<%# Bind("Description") %>'
                            CaseConversion="UpperCase" MaxLength="50" Size="25">
                            <Validators>
                                <i:Required />
                            </Validators>
                        </i:TextBoxEx>
                        <eclipse:LeftLabel ID="LeftLabel12" runat="server" Text="Payable Date" />
                        <i:TextBoxEx ID="dtbSalaryPeriodPayable" runat="server" FriendlyName="Payable Date"
                            Value='<%# Bind("PayableDate", "{0:d}") %>'>
                            <Validators>
                                <%-- // <i:Required />--%>
                                <i:Date />
                            </Validators>
                        </i:TextBoxEx>
                        <eclipse:LeftPanel ID="xx" runat="server" Span="true">
                            <i:CheckBoxEx ID="chkAllEmp" runat="server" Text="Add employees whose bank is" ToolTip="Check if you want to create salary for the employees whose bank account is in the selected Bank" />
                            <phpa:PhpaLinqDataSource ID="dsBankName" runat="server" OnSelecting="dsBankName_Selecting"
                                ContextTypeName="Eclipse.PhpaLibrary.Database.Payroll.PayrollDataContext">
                            </phpa:PhpaLinqDataSource>
                            <i:DropDownListEx ID="ddlBankName" DataSourceID="dsBankName" DataTextField="BankName"
                                DataValueField="BankId" runat="server" Value='<%# Bind("BankName") %>'>
                                <Items>
                                    <eclipse:DropDownItem Text="(Not Set)" Value="" Persistent="Always" />
                                </Items>
                            </i:DropDownListEx>
                        </eclipse:LeftPanel>
                        <eclipse:LeftLabel ID="lblEmpType" runat="server" Text="Employee Type" />
                        <i:DropDownListEx ID="ddlEmpType" runat="server" EnableViewState="false">
                            <Items>
                                <eclipse:DropDownItem Text="(All types)" Value="" Persistent="Always" />
                                <eclipse:DropDownItem Text="Not Set" Value="Not" Persistent="Always" />
                                <eclipse:DropDownItem Text="Deputation" Value="D" Persistent="Always" />
                                <eclipse:DropDownItem Text="Regular" Value="R" Persistent="Always" />
                                <eclipse:DropDownItem Text="Contract" Value="C" Persistent="Always" />
                                <eclipse:DropDownItem Text="Work Charged" Value="WC" Persistent="Always" />
                                <eclipse:DropDownItem Text="Secondment" Value="S" Persistent="Always" />
                            </Items>
                        </i:DropDownListEx>
                        <eclipse:LeftLabel runat="server" Text="Paid Date" />
                        <i:TextBoxEx ID="tbPaidDate" runat="server" FriendlyName="Paid Date" QueryStringValue='<%# Bind("PaidDate", "{0:d}") %>'
                            ClientIDMode="Static">
                            <Validators>
                                <i:Date DateType="ToDate" AssociatedControlID="tbFromDateEdit" />
                            </Validators>
                        </i:TextBoxEx>
                        <br />
                        Enter Paid date only after you have actually paid the bill.
                        <eclipse:LeftLabel ID="LeftLabel13" runat="server" Text="Station" />
                        <phpa:PhpaLinqDataSource ID="dsStation" runat="server" OnSelecting="dsStation_Selecting"
                            ContextTypeName="Eclipse.PhpaLibrary.Database.Payroll.PayrollDataContext">
                        </phpa:PhpaLinqDataSource>
                        <i:DropDownListEx ID="ddlStation" DataSourceID="dsStation" DataTextField="StationName"
                            DataValueField="StationId" runat="server" Value='<%# Bind("StationId") %>'>
                            <Items>
                                <eclipse:DropDownItem Text="(Not Set)" Value="" Persistent="Always" />
                            </Items>
                            <Validators>
                                <i:Required />
                            </Validators>
                        </i:DropDownListEx>
                        <br />
                        List displays only those stations for which you are authorized.
                        <br />
                        <eclipse:LeftLabel ID="LeftLabel6" runat="server" Text="M.R. Number" />
                         <i:TextBoxEx ID="tbMRNumber" runat="server" Text='<%# Bind("MRNumber") %>'
                            MaxLength="50" Size="25">
                        </i:TextBoxEx>
                    </eclipse:TwoColumnPanel>
                    <i:ButtonEx runat="server" ID="btnSave" Text="Save" CausesValidation="true" Action="Submit"
                        Icon="Disk" OnClick="btnSave_Click" OnClientClick="OnSalaryPeriodUpdate" />
                    <i:LinkButtonEx ID="btnCan" runat="server" Text="Cancel" CausesValidation="false"
                        OnClick="btnCancel_Click" />
                    <i:ValidationSummary runat="server" />
                </EditItemTemplate>
                <FooterTemplate>
                    <phpa:FormViewStatusMessage ID="FormViewStatusMessage1" runat="server">
                    </phpa:FormViewStatusMessage>
                </FooterTemplate>
            </asp:FormView>
        </ContentTemplate>
    </jquery:Dialog>
</asp:Content>
