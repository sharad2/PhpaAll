<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" CodeBehind="TdsCertificate.aspx.cs"
    Inherits="Finance.Payroll.Reports.TdsCertificate" Title="TDS Certificate" EnableViewState="false" %>

<%@ Register Src="~/Controls/PrinterFriendlyButton.ascx" TagName="PrinterFriendlyButton"
    TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphSideNavigation" runat="server">
    <asp:HyperLink ID="HyperLink2" runat="server" Text="Help" NavigateUrl="~/Doc/TdsCertificate.doc.aspx" />
    <uc2:PrinterFriendlyButton ID="PrinterFriendlyButton1" runat="server" EnableTheming="true" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cph" runat="server">
    <jquery:JPanel runat="server" CssClasses="ParamInstructions">
        <eclipse:TwoColumnPanel ID="TwoColumnPanel1" runat="server" IsValidationContainer="true">
            <eclipse:LeftLabel ID="LeftLabel1" runat="server" Text="Employee" />
            <i:AutoComplete ID="tbEmployee" runat="server" ClientIDMode="Static" Width="25em"
                WebMethod="GetEmployees" WebServicePath="~/Services/Employees.asmx" AutoValidate="true"
                ValidateWebMethodName="ValidateEmployee" Delay="500">
                <Validators>
                    <i:Required />
                </Validators>
            </i:AutoComplete>
            <eclipse:LeftLabel ID="LeftLabel2" runat="server" Text="Salary Period Start Date From / To" />
            <i:TextBoxEx ID="tbFromDate" runat="server" FriendlyName="From Date">
                <Validators>
                    <i:Required />
                    <i:Date />
                </Validators>
            </i:TextBoxEx>
            <i:TextBoxEx ID="tbToDate" runat="server" FriendlyName="To Date">
                <Validators>
                    <i:Required />
                    <i:Date DateType="ToDate" />
                </Validators>
            </i:TextBoxEx>
            <br />
            <i:ButtonEx ID="btnGo" runat="server" Text="Go" Action="Submit" Icon="Refresh" CausesValidation="true" />
            <i:ValidationSummary ID="valSelector" runat="server" />
        </eclipse:TwoColumnPanel>
    </jquery:JPanel>
    <br />
    <div style="margin-bottom: 4mm">
        <phpa:PhpaLinqDataSource ID="dsEmployee" runat="server" ContextTypeName="Eclipse.PhpaLibrary.Database.Payroll.PayrollDataContext"
            RenderLogVisible="false" OnSelecting="dsEmployee_Selecting">
        </phpa:PhpaLinqDataSource>
        <asp:FormView ID="fv" runat="server" DataSourceID="dsEmployee">
            <ItemTemplate>
                <eclipse:TwoColumnPanel runat="server" SkinID="PrintVisible">
                    <eclipse:LeftLabel runat="server" Text="Employee" />
                    <asp:Label runat="server" Text='<%# Eval("FullName") %>' />
                    <eclipse:LeftLabel runat="server" Text="Employee Code" />
                    <asp:Label runat="server" Text='<%# Eval("EmployeeCode") %>' />
                    <eclipse:LeftLabel runat="server" Text="Citizen I.D./Resident Permit No" />
                    <asp:Label runat="server" Text='<%# Eval("CitizenCardNo") %>' />
                    <eclipse:LeftLabel runat="server" Text="Designation" />
                    <asp:Label runat="server" Text='<%# Eval("Designation") %>' />
                    <eclipse:LeftLabel runat="server" Text="TPN" />
                    <asp:Label runat="server" Text='<%# Eval("Tpn") %>' />
                    <eclipse:LeftLabel ID="LeftLabel8" runat="server" Text="Name of the Employer" />
                    <asp:Label runat="server" Text="PHPA" />
                </eclipse:TwoColumnPanel>
            </ItemTemplate>
        </asp:FormView>
    </div>
    <phpa:PhpaLinqDataSource ID="ds" runat="server" ContextTypeName="Eclipse.PhpaLibrary.Database.Payroll.PayrollDataContext"
        RenderLogVisible="false" OnSelecting="ds_Selecting">
    </phpa:PhpaLinqDataSource>
    <jquery:GridViewEx ID="gv" runat="server" DataSourceID="ds" AutoGenerateColumns="false"
        ShowFooter="true" OnRowDataBound="gv_RowDataBound">
        <Columns>
            <eclipse:MultiBoundField DataFields="Month" HeaderText="Month" />
            <eclipse:MultiBoundField DataFields="BasicPay" HeaderText="Basic Salary" DataFormatString="{0:N0}"
                DataSummaryCalculation="ValueSummation">
                <ItemStyle HorizontalAlign="Right" />
                <FooterStyle HorizontalAlign="Right" />
            </eclipse:MultiBoundField>
            <%--<jquery:MatrixField DataMergeFields="EmployeePeriodId" DataHeaderFields="CategoryId,IsDeduction"
                DataHeaderSortFields="IsDeduction,HeaderSortExpression" DataHeaderCustomFields="CategoryShortDescription"
                DataValueFields="Amount" DataHeaderFormatString="{0::$IsDeduction:Deduction:Allowance}s|{2::$CategoryId = 0:Others:~}"
                DisplayRowTotals="true" DataValueFormatString="{0:C}" OnMatrixRowDataBound="gv_MatrixRowDataBound"
                RowTotalHeaderText="Gross Salary|Total Deductions" DisplayColumnTotals="true">
                <ItemStyle HorizontalAlign="Right" />
                <FooterStyle HorizontalAlign="Right" />
            </jquery:MatrixField>--%>
              <m:MatrixField DataMergeFields="EmployeePeriodId" DataCellFields="Amount" DataHeaderFields="IsDeduction,CategoryShortDescription"  
               HeaderText="{0::$IsDeduction:Deductions:Allowances} " >
                <MatrixColumns>
                    <m:MatrixColumn DataCellFormatString="{0:N0}" DataHeaderFormatString="{1}" ColumnType="CellValue"
                        DisplayColumnTotal="true"  />
                </MatrixColumns>
            </m:MatrixField>
            <eclipse:MultiBoundField HeaderText="Net Pay" AccessibleHeaderText="NetPay" DataFormatString={0:N0}>
                <ItemStyle HorizontalAlign="Right" />
                <FooterStyle HorizontalAlign="Right" />
            </eclipse:MultiBoundField>
            <eclipse:MultiBoundField DataFields="MRNumber , MRNumberDate" HeaderText="M.R. Number / Date" DataFormatString="{0}  {1:d}"/>
        </Columns>
    </jquery:GridViewEx>
</asp:Content>
