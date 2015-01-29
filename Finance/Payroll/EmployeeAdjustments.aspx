<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" CodeBehind="EmployeeAdjustments.aspx.cs"
    Inherits="PhpaAll.Payroll.EmployeeAdjustments" Title="Default Employee Adjustments" %>

<%@ Register Src="../Controls/EmployeeAdjustmentEditor.ascx" TagName="EmployeeAdjustmentEditor"
    TagPrefix="uc1" %>
<asp:Content ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        input[type=text][readonly]
        {
            background-color: Silver;
        }
    </style>
    <script type="text/javascript">
        // it passes EmployeeID as a parameter to the web method.
        function EmployeeSearch(event, ui) {
            var employeeId = $('#hdemployeeid').val();
            $(this).autocompleteEx('option', 'parameters', { employeeId: employeeId, term: $(this).val() });
            return true;
        }

    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphSideNavigation" runat="server">
    <asp:HyperLink ID="HyperLink2" runat="server" Text="Help" NavigateUrl="~/Doc/EmployeeAdjustments.doc.aspx" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cph" runat="server">
    <p>
        Employee adjustments serve as template for creating the payroll of an employee.
        A template must exist for each employee before he can be paid a salary. The template
        defines the allowances and deductions to be used while calculating the net pay.
    </p>
    <p>
        The list below shows all employees for whom payroll template exists. You can edit
        the template for any employee by clicking on the employee name. To add a template
        for a new employee, search for him first. If the template does not already exist,
        you will be provided with the option to create it.
    </p>
    <phpa:PhpaLinqDataSource ID="dsEmployeeAdjustments" runat="server" ContextTypeName="Eclipse.PhpaLibrary.Database.Payroll.PayrollDataContext"
        RenderLogVisible="False" EnableInsert="true" OnSelecting="dsEmployeeAdjustments_Selecting"
        TableName="EmployeeAdjustment">
        <WhereParameters>
            <asp:Parameter Name="EmployeeId" Type="Int32" />
        </WhereParameters>
    </phpa:PhpaLinqDataSource>
    <jquery:JPanel ID="JPanel1" runat="server" IsValidationContainer="true" Width="100%">
        <eclipse:TwoColumnPanel ID="TwoColumnPanel2" runat="server" WidthLeft="100">
            <eclipse:LeftLabel runat="server" Text="Specific Employee" />
            <i:AutoComplete ID="tbEmployee" runat="server" FriendlyName="Employee" Value='<%# Bind("EmployeeId") %>'
                Text='<%# Eval("Employee.FullName") %>' WebMethod="GetEmployees" WebServicePath="~/Services/Employees.asmx"
                Width="20em" ValidateWebMethodName="ValidateEmployee" AutoValidate="true">
                <Validators>
                    <i:Required />
                </Validators>
            </i:AutoComplete>
            <i:ValidationSummary runat="server" />
            <br />
            <i:ButtonEx ID="btnSearch" runat="server" Action="Submit" CausesValidation="true"
                Text="Search" OnClick="btnSearch_Click" Icon="Search" IsDefault="true" />
            <i:ButtonEx ID="btnShowAll" runat="server" Action="Reset" Text="Show all Employees"
                Icon="Refresh" CausesValidation="false" />
        </eclipse:TwoColumnPanel>
    </jquery:JPanel>
    <jquery:JPanel ID="JPanel2" runat="server" IsValidationContainer="true" Width="100%">
        <eclipse:TwoColumnPanel ID="TwoColumnPanel1" runat="server" WidthLeft="100">
            <eclipse:LeftLabel runat="server" Text="Division" />
            <phpa:PhpaLinqDataSource runat="server" ID="dsDivision" ContextTypeName="Eclipse.PhpaLibrary.Database.Payroll.PayrollDataContext"
                Visible="True" RenderLogVisible="False" OnSelecting="dsDivision_Selecting">
            </phpa:PhpaLinqDataSource>
            <i:DropDownListEx runat="server" ID="ddlDivision" DataSourceID="dsDivision" ClientIDMode="Static"
                FriendlyName="Division" DataTextField="Description" DataValueField="DivisionId"
                Value='<%# Bind("DivisionId") %>'>
                <Items>
                    <eclipse:DropDownItem Persistent="Always" Text="(Please Select)" />
                </Items>
                <Validators>
                    <i:Required />
                </Validators>
            </i:DropDownListEx>
            <i:ValidationSummary runat="server" />
            <br />
            <asp:LoginView ID="LoginView1" runat="server">
                <LoggedInTemplate>
                    To create a default template for all employees of a particular division, select
                    the division and press
                    <i:ButtonEx ID="btnBulk" runat="server" Text="Create Default Templates" OnClick="btnBulk_Click"
                        Action="Submit" Icon="None" CausesValidation="true" />
                </LoggedInTemplate>
                <AnonymousTemplate>
                    Login in order to create default template.</AnonymousTemplate>
            </asp:LoginView>
        </eclipse:TwoColumnPanel>
    </jquery:JPanel>
    <asp:Label ID="lblCountAdded" runat="server" Font-Bold="true" Font-Size="Larger"
        Visible="false"></asp:Label><br />
    <jquery:GridViewEx ID="gvEmployeeAdjustments" runat="server" AutoGenerateColumns="False"
        Caption="Employee Adjustments" DataKeyNames="EmployeeId,EmployeeName" DataSourceID="dsEmployeeAdjustments"
        OnSelectedIndexChanged="gvEmployeeAdjustments_SelectedIndexChanged" AllowSorting="true"
        OnRowDeleting="gvEmployeeAdjustments_RowDeleting" AllowPaging="true" PageSize="100"
        EnableViewState="false" OnRowDataBound="gvEmployeeAdjustments_RowDataBound">
        <EmptyDataTemplate>
            No Payroll template has been defined for <em>
                <%= this.tbEmployee.Text %>.</em>
            <br />
            <asp:LoginView runat="server">
                <LoggedInTemplate>
                    <i:LinkButtonEx ID="btnNewEmployee" runat="server" Text="Create Now." Action="Submit"
                        OnClick="btnNewEmployee_Click" />
                </LoggedInTemplate>
                <AnonymousTemplate>
                    You are not logged in. Login, in order to create default template for <b>
                        <%= this.tbEmployee.Text %>.</b>
                </AnonymousTemplate>
            </asp:LoginView>
        </EmptyDataTemplate>
        <PagerSettings FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast"
            NextPageText="Next" Position="TopAndBottom" PreviousPageText="Previous" />
        <Columns>
            <jquery:CommandFieldEx ShowDeleteButton="true" ShowEditButton="false" RolesRequired="Operator"
                DeleteConfirmationText="Are you sure you want to delete the Employee along with its Adjustments?">
            </jquery:CommandFieldEx>
            <eclipse:SequenceField>
            </eclipse:SequenceField>
            <asp:TemplateField SortExpression="EmployeeCode" HeaderText="Employee">
                <ItemTemplate>
                    <asp:LinkButton runat="server" CommandName="Select" ToolTip="Click to see Adjustments Details"
                        Text='<%# Eval("EmployeeName") %>'>
                    </asp:LinkButton>
                    <asp:Image ID="imgNew" runat="server" ImageUrl="~/Images/new.gif" ForeColor="DarkBlue"
                        ImageAlign="Middle" />
                </ItemTemplate>
            </asp:TemplateField>
            <eclipse:MultiBoundField DataFields="DivisionName" HeaderText="Division" SortExpression="DivisionName"
                ToolTipFields="DivisionCode" ToolTipFormatString="Division Code:{0}" />
            <eclipse:MultiBoundField DataFields="Designation" HeaderText="Designation" SortExpression="Designation" />
            <eclipse:MultiBoundField DataFields="Grade" HeaderText="Grade" SortExpression="Grade" />
            <eclipse:MultiBoundField DataFields="Basic" HeaderText="Basic Salary" SortExpression="Basic"
                DataFormatString="{0:N2}">
                <FooterStyle HorizontalAlign="Right"></FooterStyle>
                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                <ItemStyle HorizontalAlign="Right"></ItemStyle>
            </eclipse:MultiBoundField>
            <eclipse:MultiBoundField DataFields="Allowance" HeaderText="Allowances" DataFormatString="{0:N2}">
                <FooterStyle HorizontalAlign="Right"></FooterStyle>
                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                <ItemStyle HorizontalAlign="Right"></ItemStyle>
            </eclipse:MultiBoundField>
            <eclipse:MultiBoundField DataFields="Deduction" HeaderText="Deductions" DataFormatString="{0:N2}">
                <FooterStyle HorizontalAlign="Right"></FooterStyle>
                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                <ItemStyle HorizontalAlign="Right"></ItemStyle>
            </eclipse:MultiBoundField>
            <eclipse:MultiBoundField DataFields="GrossSalary" HeaderText="Gross Salary" SortExpression="GrossSalary"
                DataFormatString="{0:N2}">
                <FooterStyle HorizontalAlign="Right"></FooterStyle>
                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                <ItemStyle HorizontalAlign="Right"></ItemStyle>
            </eclipse:MultiBoundField>
            <eclipse:MultiBoundField DataFields="NetPay" HeaderText="Net Pay" DataFormatString="{0:N2}">
                <FooterStyle HorizontalAlign="Right"></FooterStyle>
                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                <ItemStyle HorizontalAlign="Right"></ItemStyle>
            </eclipse:MultiBoundField>
            <eclipse:MultiBoundField DataFields="StationName" HeaderText="Station of Employee" >
                <FooterStyle HorizontalAlign="Right"></FooterStyle>
                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                <ItemStyle HorizontalAlign="Right"></ItemStyle>
            </eclipse:MultiBoundField>
        </Columns>
    </jquery:GridViewEx>
    <jquery:Dialog runat="server" Width="700" Title="Employee Adjustment Editor" Position="RightTop"
        EnablePostBack="true" EnableViewState="true" ID="dlgEditor" Visible="false">
        <ContentTemplate>
            <uc1:EmployeeAdjustmentEditor ID="ctlEditor" runat="server" />
        </ContentTemplate>
    </jquery:Dialog>
</asp:Content>
