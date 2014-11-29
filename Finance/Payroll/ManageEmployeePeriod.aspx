<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" CodeBehind="ManageEmployeePeriod.aspx.cs"
    Inherits="Finance.Payroll.ManageEmployeePeriod" Title="Salary Remittances to Bank" %>

<%@ Register Src="~/Controls/PrinterFriendlyButton.ascx" TagName="PrinterFriendlyButton"
    TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        // it passes EmployeePeriodID as a parameter to the web method.
        function PassEmployeePeriod(event, ui) {
            var employeePeriodId = $('#hdEmployeePeriodId').val();
            $(this).autocompleteEx('option', 'parameters', { employeePeriodId: employeePeriodId, term: $(this).val() });
            return true;
        }

        function cbFractionBasicOverrriden_Click(e) {
            var $tb = $(this).closest('td').find('input:text');
            if ($(this).is(':checked')) {
                $tb.removeAttr('readOnly');
            }
            else {
                $tb.attr('readOnly', 'true');
            }
        }
     
    </script>
    <style type="text/css">
        input[type=text][readonly]
        {
            background-color: Silver;
        }
    </style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphSideNavigation" runat="server">
    <asp:HyperLink ID="HyperLink2" runat="server" Text="Help" NavigateUrl="~/Doc/ManageEmployeePeriod.doc.aspx" />
    <uc2:PrinterFriendlyButton ID="PrinterFriendlyButton1" runat="server" EnableTheming="true" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cph" runat="server">
    <asp:HiddenField runat="server" ID="hdEmployeePeriodId" ClientIDMode="Static" />
    <div class="noprint">
        Screen helps you to Manage Employee Adjustments and allows you to create new Adjustments
        for Employees for specified period.</div>
    <jquery:JPanel runat="server" IsValidationContainer="true">
        <eclipse:TwoColumnPanel ID="TwoColumnPanel1" runat="server" WidthLeft="50">
            <eclipse:LeftLabel runat="server" Text="Period" />
            <i:AutoComplete ID="tbPeriod" runat="server" FriendlyName="Period" WebMethod="GetPeriod"
                QueryString="PeriodId" ValidateWebMethodName="ValidatePeriod" WebServicePath="~/Services/PONumbers.asmx"
                Delay="2000" Width="20em">
                <Validators>
                    <i:Required />
                </Validators>
            </i:AutoComplete>
            <br />
            <i:ButtonEx runat="server" ID="btnPeriod" Text="Show Employees" OnClick="btnPeriod_Click"
                Action="Submit" Icon="Search" CausesValidation="true" />
            <i:ValidationSummary runat="server" />
        </eclipse:TwoColumnPanel>
    </jquery:JPanel>
    <jquery:JPanel runat="server" IsValidationContainer="true" CssClasses="noprint">
        <eclipse:TwoColumnPanel ID="TwoColumnPanel2" runat="server" WidthLeft="50">
            <eclipse:LeftLabel ID="LeftLabel1" Text="Employee" runat="server" />
            <i:AutoComplete ID="tbEmployee" runat="server" FriendlyName="Employee" Width="20em"
                WebMethod="GetEmployees" WebServicePath="~/Services/Employees.asmx" ValidateWebMethodName="ValidateEmployee"
                AutoValidate="true">
                <Validators>
                    <i:Required />
                </Validators>
            </i:AutoComplete>
            <br />
            <i:ButtonEx ID="btnLookUp" runat="server" Text="Lookup Employee" CausesValidation="true"
                OnClick="btnLookUp_Click" Action="Submit" Icon="Search" />
            <i:ButtonEx ID="btnShowAllemployee" runat="server" Action="Submit" Text="Show all employee"
                OnClick="btnShowAllemployee_Click" EnableViewState="false" Icon="Refresh" />
            <i:ValidationSummary runat="server" />
            <br />
            You can look up any employee by entering his code above. If the employee you are
            looking for is not in the list, then you can add that employee.
        </eclipse:TwoColumnPanel>
    </jquery:JPanel>
    <asp:FormView ID="fvNewEmplyeeforperiod" runat="server" DataSourceID="dsNewEmployeeForPeriod"
        DataKeyNames="EmployeePeriodId" OnItemInserting="fvNewEmplyeeforperiod_ItemInserting"
        OnItemInserted="fvNewEmplyeeforperiod_ItemInserted" DefaultMode="Insert">
        <InsertItemTemplate>
        </InsertItemTemplate>
    </asp:FormView>
    <phpa:PhpaLinqDataSource ID="dsNewEmployeeForPeriod" runat="server" ContextTypeName="Eclipse.PhpaLibrary.Database.Payroll.PayrollDataContext"
        EnableInsert="True" OnSelecting="dsNewEmployeeForPeriod_Selecting" TableName="EmployeePeriods"
        RenderLogVisible="False" OnInserting="dsNewEmployeeForPeriod_Inserting">
    </phpa:PhpaLinqDataSource>
    <phpa:PhpaLinqDataSource ID="dsEmployeePeriod" runat="server" ContextTypeName="Eclipse.PhpaLibrary.Database.Payroll.PayrollDataContext"
        TableName="EmployeePeriods" EnableUpdate="True" RenderLogVisible="False" EnableInsert="True"
        EnableDelete="True" OrderBy="Employee.FirstName" OnSelecting="dsEmployeePeriod_Selecting"
        OnDeleting="dsEmployeePeriod_Deleting">
        <WhereParameters>
            <asp:Parameter Name="EmployeeId" Type="Int32" />
            <asp:Parameter Name="SalaryPeriodId" Type="Int32" />
            <asp:QueryStringParameter Name="BankName" Type="String" />
        </WhereParameters>
        <DeleteParameters>
            <asp:Parameter Name="EmployeePeriodId" Type="Int32" />
        </DeleteParameters>
    </phpa:PhpaLinqDataSource>
    <asp:FormView ID="FormView1" runat="server" DataSourceID="dsEmployeePeriod">
        <ItemTemplate>
            <div style="float: left">
                <asp:ImageMap ID="ImageMap1" runat="server" ImageUrl='<%$ AppSettings:logo %>'>
                    <asp:CircleHotSpot HotSpotMode="NotSet" X="20" Y="20" Radius="75" />
                </asp:ImageMap>
            </div>
        </ItemTemplate>
    </asp:FormView>
    <jquery:GridViewEx ID="gvEmployeesForperiod" runat="server" AutoGenerateColumns="False"
        OnSelectedIndexChanged="gvEmployeesForperiod_SelectedIndexChanged" DataKeyNames="EmployeePeriodId"
        DataSourceID="dsEmployeePeriod" OnRowUpdating="gvEmployeesForperiod_RowUpdating"
        OnRowDataBound="gvEmployeesForperiod_RowDataBound" AllowSorting="true" OnDataBound="gvEmployeesForperiod_DataBound"
        OnRowUpdated="gvEmployeesForperiod_RowUpdated" ShowFooter="true" EnableViewState="true">
        <EmptyDataTemplate>
            <asp:LoginView runat="server">
                <AnonymousTemplate>
                    Please
                    <asp:HyperLink ID="hlLogin" runat="server" Text="login" NavigateUrl="~/Login.aspx" />
                    to edit and add employee for period.
                </AnonymousTemplate>
                <LoggedInTemplate>
                    Click to add <b>
                        <%= this.tbEmployee.Text %></b> for the given period.
                    <jquery:JPanel runat="server" IsValidationContainer="true" CssClasses="noprint">
                        <i:LinkButtonEx ID="btnAddNewEmployeeForPeriod" runat="server" Text="Add" CausesValidation="true"
                            OnClick="btnAddNewEmployeeForPeriod_Click" Action="Submit" />
                        <i:ValidationSummary ID="valSummary" runat="server" />
                    </jquery:JPanel>
                </LoggedInTemplate>
            </asp:LoginView>
        </EmptyDataTemplate>
        <Columns>
            <jquery:CommandFieldEx ControlStyle-CssClass="noprint" HeaderStyle-CssClass="noprint"
                ItemStyle-CssClass="noprint" FooterStyle-CssClass="noprint" ShowDeleteButton="true"
                ShowEditButton="false" DeleteConfirmationText="Employee will be deprived from salary if deleted.Do you want to continue?">
            </jquery:CommandFieldEx>
            <eclipse:SequenceField ItemStyle-HorizontalAlign="Left" />
            <eclipse:MultiBoundField DataFields="Employee.EmployeeCode" HeaderText="Employee|Code"
                SortExpression="Employee.EmployeeCode" />
            <eclipse:MultiBoundField DataFields="Employee.FullName" HeaderText="Employee|Name"
                AccessibleHeaderText="Employee" SortExpression="Employee.FirstName" />
            <eclipse:MultiBoundField DataFields="Employee.Designation" HeaderText="Designation" />
            <eclipse:MultiBoundField DataFields="Employee.CitizenCardNo" HeaderText="Citizen Card No." />
            <eclipse:MultiBoundField DataFields="Employee.Bank.BankName" HeaderText="Bank" AccessibleHeaderText="Bank"
                FooterText="Total" />
            <eclipse:MultiBoundField SortExpression="BasicPay" AccessibleHeaderText="BasicPay"
                DataFields="BasicPay" HeaderText="Basic Salary" DataFormatString="{0:N0}">
                <FooterStyle HorizontalAlign="Right" CssClass="noprint"></FooterStyle>
                <HeaderStyle HorizontalAlign="Center" CssClass="noprint"></HeaderStyle>
                <ItemStyle HorizontalAlign="Right" CssClass="noprint"></ItemStyle>
            </eclipse:MultiBoundField>
            <asp:TemplateField HeaderText="Total Allowances" ItemStyle-HorizontalAlign="Right"
                FooterStyle-HorizontalAlign="Right">
                <ItemTemplate>
                    <asp:LinkButton runat="server" ID="lnkAllowance" ToolTip="Click to see the all adjustment of this employee"
                        CommandName="Select"></asp:LinkButton>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Right" CssClass="noprint"></ItemStyle>
                <HeaderStyle CssClass="noprint" />
                <FooterTemplate>
                    <asp:Label ID="lblAllowance" runat="server" />
                </FooterTemplate>
                <FooterStyle CssClass="noprint" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Total Deductions" ItemStyle-HorizontalAlign="Right"
                FooterStyle-HorizontalAlign="Right">
                <ItemTemplate>
                    <asp:LinkButton runat="server" ID="lnkDeduction" ToolTip="Click to see the all adjustment of this employee"
                        CommandName="Select"></asp:LinkButton>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Right" CssClass="noprint"></ItemStyle>
                <HeaderStyle CssClass="noprint" />
                <FooterTemplate>
                    <asp:Label ID="lblDeduction" runat="server" />
                </FooterTemplate>
                <FooterStyle CssClass="noprint" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Net Pay" ItemStyle-HorizontalAlign="Right">
                <ItemTemplate>
                    <asp:Label runat="server" ID="lblNetPay" ToolTip="Click to see the paybill of this employee"></asp:Label>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                <FooterTemplate>
                    <asp:Label ID="lblNetPay" runat="server" />
                </FooterTemplate>
                <FooterStyle HorizontalAlign="Right" />
            </asp:TemplateField>
            <eclipse:MultiBoundField DataFields="Employee.BankAccountNo" HeaderText="Bank A/C No."
                SortExpression="Employee.BankAccountNo" ItemStyle-HorizontalAlign="Center" />
        </Columns>
    </jquery:GridViewEx>
    <br /><br />
    <asp:Panel ID="pnlMessage" runat="server" Visible="false">
        <b>Please find enclosed a cheque No.______________Dt.___________for Nu <asp:Label ID="lblamount"
            runat="server"></asp:Label>(<asp:Label ID="lblCurrencyInWords" runat="server"></asp:Label>)<br />
        drawn in your favour, for crediting the above mentioned A/c maintained with your
        Bank towards salary for the month_________</b>
    </asp:Panel>
    <jquery:Dialog ID="dlgEditor" runat="server" Width="550" Title="Employee Adjustment Editor"
        Position="RightTop" EnableViewState="true" EnablePostBack="true" Visible="false">
        <ContentTemplate>
            <phpa:PhpaLinqDataSource ID="dsEditPeriodEmpAdjustments" runat="server" ContextTypeName="Eclipse.PhpaLibrary.Database.Payroll.PayrollDataContext"
                TableName="PeriodEmployeeAdjustments" AutoGenerateWhereClause="True" OrderBy="Adjustment.AdjustmentCode"
                OnSelecting="dsEditPeriodEmpAdjustments_Selecting" RenderLogVisible="False" EnableUpdate="True"
                EnableDelete="True" EnableInsert="True" OnContextCreated="dsEditPeriodEmpAdjustments_ContextCreated">
                <UpdateParameters>
                    <asp:Parameter Type="Boolean" Name="IsAmountOverridden" />
                    <asp:Parameter Type="Decimal" Name="Amount" />
                </UpdateParameters>
                <WhereParameters>
                    <asp:Parameter Name="EmployeePeriodId" Type="Int32" />
                </WhereParameters>
                <InsertParameters>
                    <asp:Parameter Name="EmployeeId" Type="Int32" />
                </InsertParameters>
            </phpa:PhpaLinqDataSource>
            <i:LinkButtonEx ID="btnNew" runat="server" OnClick="btnNew_Click" Text="Add New Adjustment"
                Action="Submit" RolesRequired="PayrollOperator" OnPreRender="btnEdit_PreRender"
                EnableViewState="true" />
            <i:ValidationSummary ID="valSummaryEditor" runat="server" />
            <jquery:GridViewExInsert ID="gvEditPeriodEmpAdjustments" runat="server" AutoGenerateColumns="False"
                DataKeyNames="PeriodEmployeeAdjustmentId" DataSourceID="dsEditPeriodEmpAdjustments"
                OnRowDataBound="gvEditPeriodEmpAdjustments_RowDataBound" AllowSorting="true"
                OnRowUpdating="gvEditPeriodEmpAdjustments_RowUpdating" OnRowInserted="gvEditPeriodEmpAdjustments_RowInserted"
                OnRowInserting="gvEditPeriodEmpAdjustments_RowInserting" OnRowDeleted="gvEditPeriodEmpAdjustments_RowDeleted"
                InsertRowsAtBottom="false" InsertRowsCount="0">
                <Columns>
                    <jquery:CommandFieldEx ShowDeleteButton="true" RolesRequired="PayrollOperator" DeleteConfirmationText="Adjustment will be deleted. Are you sure you want to delete Adjustment?">
                    </jquery:CommandFieldEx>
                    <eclipse:MultiBoundField DataFields="Adjustment.AdjustmentCode, Adjustment.Description"
                        HeaderText="Adjustment" SortExpression="Adjustment.AdjustmentCode" DataFormatString="{0}:{1}">
                        <EditItemTemplate>
                            <i:AutoComplete ID="tbAdjustmentCodeNew" runat="server" Value='<%# Bind("AdjustmentId") %>'
                                FriendlyName="Adjustment Code" WebMethod="GetAdjustmentWithoutPeriodID" Text='<%# string.Format("{0}:{1}", Eval("Adjustment.AdjustmentCode"),Eval("Adjustment.Description")) %>'
                                WebServicePath="~/Services/Adjustments.asmx" ValidateWebMethodName="ValidateAdjustmentCode"
                                OnClientSearch="PassEmployeePeriod">
                                <Validators>
                                    <i:Required />
                                </Validators>
                            </i:AutoComplete>
                        </EditItemTemplate>
                    </eclipse:MultiBoundField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <span title="Display amount which is defined explicitly or default for Adjustment.">
                                Amount </span>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <div style="text-align: right">
                                <phpa:InfoImage runat="server" ID="infoImg2" Visible='<%# Eval("IsAmountOverridden") %>'
                                    ToolTip="You have explicitly changed the amount of this adjustment for this employee"
                                    EnableViewState="true" />
                                <asp:Label ID="Label4" runat="server" Text='<%# Eval("Amount","{0:C2}") %>' />
                            </div>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <i:CheckBoxEx ID="cbFlatAmountOverridden" runat="server" Text="Override Default"
                                Checked='<%# Bind("IsAmountOverridden") %>' OnClientClick="cbFractionBasicOverrriden_Click" />
                            <div style="text-align: left; white-space: nowrap">
                                <i:TextBoxEx ID="tbFlatAmount" runat="server" QueryStringValue='<%# Bind("Amount", "{0:C2}") %>'
                                    FriendlyName="Flat Amount" ReadOnly='<%# !((bool)Eval("IsAmountOverridden")) %>'>
                                    <Validators>
                                        <i:Filter DependsOn="cbFlatAmountOverridden" DependsOnState="Checked" />
                                        <i:Required />
                                        <i:Value ValueType="Decimal" Min="0" />
                                    </Validators>
                                </i:TextBoxEx>
                            </div>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <phpa:BoolField HeaderText="Type" SortExpression="Adjustment.IsDeduction" DataField="Adjustment.IsDeduction"
                        TrueValue="Deduction" FalseValue="Allowance" TrueToolTip="Red color shows Deduction type.">
                    </phpa:BoolField>
                    <asp:TemplateField HeaderText="Remarks">
                        <ItemStyle HorizontalAlign="Left" />
                        <ItemTemplate>
                            <asp:Label runat="server" Text='<%# Eval("Comment") %>' />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <i:TextBoxEx ID="tbComment" runat="server" Text='<%# Bind("Comment") %>' MaxLength="20"
                                Size="10" />
                        </EditItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </jquery:GridViewExInsert>
        </ContentTemplate>
    </jquery:Dialog>
</asp:Content>
