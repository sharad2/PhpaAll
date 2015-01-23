<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" CodeBehind="PaybillRegister.aspx.cs"
    Inherits="Finance.Payroll.Reports.PaybillRegister" Title="Paybill Register" EnableViewState="false" %>

<%@ Register Src="~/Controls/PrinterFriendlyButton.ascx" TagName="PrinterFriendlyButton"
    TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphSideNavigation" runat="server">
    <asp:HyperLink ID="HyperLink2" runat="server" Text="Help" NavigateUrl="~/Doc/PaybillRegister.doc.aspx" />
    <uc2:PrinterFriendlyButton ID="PrinterFriendlyButton1" runat="server" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cph" runat="server">
    <div class="ParamInstructions" style="border: solid 1px Black">
        <eclipse:TwoColumnPanel ID="TwoColumnPanel1" runat="server" IsValidationContainer="true">
            <eclipse:LeftLabel ID="LeftLabel1" runat="server" Text="Employee" />
            <i:AutoComplete ID="tbEmployee" runat="server" ClientIDMode="Static" Width="25em"
                ValidateWebMethodName="ValidateEmployee" WebMethod="GetEmployees" WebServicePath="~/Services/Employees.asmx"
                AutoValidate="true" Delay="500">
                <Validators>
                    <i:Required />
                </Validators>
            </i:AutoComplete>
            <eclipse:LeftLabel ID="LeftLabel2" runat="server" Text="From Date/To Date" />
            <i:TextBoxEx ID="tbFromDate" runat="server" FriendlyName="From Date">
                <Validators>
                    <i:Date />
                    <i:Required />
                </Validators>
            </i:TextBoxEx>
            <i:TextBoxEx ID="tbToDate" runat="server" FriendlyName="To Date">
                <Validators>
                    <i:Date DateType="ToDate" />
                    <i:Required />
                </Validators>
            </i:TextBoxEx>
            <p>
                You can select the Range Maximum of 365 days. Select From Date first then Select
                To Date. Default dates are previous month Start Date and End Date. In order to select
                different Date Range first clear the To Date then select From Date.
            </p>
            <i:ButtonEx ID="btnGo" runat="server" Text="Go" Action="Submit" CausesValidation="true"
                Icon="Refresh" />
            <i:ValidationSummary ID="ValSummary" runat="server" />
        </eclipse:TwoColumnPanel>
    </div>
    <phpa:PhpaLinqDataSource ID="dsEmployee" runat="server" ContextTypeName="Eclipse.PhpaLibrary.Database.Payroll.PayrollDataContext"
        TableName="Employees" RenderLogVisible="false" OnSelecting="dsEmployee_Selecting">
    </phpa:PhpaLinqDataSource>
    <asp:ListView ID="ListView1" runat="server" DataSourceID="dsEmployee">
        <LayoutTemplate>
            <asp:PlaceHolder ID="itemPlaceholder" runat="server" />
        </LayoutTemplate>
        <ItemTemplate>
            <div style="width: 50%; float: left">
                <eclipse:TwoColumnPanel ID="TwoColumnPanel2" WidthLeft="30%" WidthRight="70%" runat="server"
                    SkinID="PrintVisible">
                    <eclipse:LeftLabel runat="server" Text="<b>Name" />
                    <asp:Label runat="server" Text='<%# Eval("FullName") %>' />
                    <eclipse:LeftLabel runat="server" Text="<b>Emp. Code" />
                    <asp:Label runat="server" Text='<%# Eval("EmployeeCode") %>' />
                    <eclipse:LeftLabel runat="server" Text="<b>Designation" />
                    <asp:Label runat="server" Text='<%# Eval("Designation") %>' />
                    <eclipse:LeftLabel runat="server" Text="<b>Grade" />
                    <asp:Label runat="server" Text='<%# Eval("Grade") %>' />
                    <eclipse:LeftLabel runat="server" Text="<b>Service Status" />
                    <asp:Label runat="server" Text='<%# Eval("EmpTypeDescription") %>' />
                    <eclipse:LeftLabel runat="server" Text="<b>Scale of Pay" />
                    <asp:Label runat="server" Text='<%# Eval("PayScale") %>' />
                    <eclipse:LeftLabel runat="server" Text="<b>Date of Joining" />
                    <asp:Label runat="server" Text='<%# Eval("JoiningDate", "{0:d}") %>' />
                    <eclipse:LeftLabel runat="server" Text="<b>Date of Increment" />
                    <asp:Label runat="server" Text='<%# Eval("DateOfIncrement","{0:d}") %>' />
                    <eclipse:LeftLabel runat="server" Text="<b>Increment Amount" />
                    <asp:Label runat="server" Text='<%# Eval("IncrementAmount") %>' />
                    <eclipse:LeftLabel runat="server" Text="<b>Sex" />
                    <asp:Label runat="server" Text='<%# Eval("Gender") %>' />
                    <eclipse:LeftLabel runat="server" Text="<b>Date of Birth" />
                    <asp:Label runat="server" Text='<%# Eval("DateOfBirth","{0:d}") %>' />
                    <eclipse:LeftLabel runat="server" Text="<b>Marital Status" />
                    <asp:Label runat="server" Text='<%# Eval("MaritalStatusType") %>' />
                    <eclipse:LeftLabel runat="server" Text="<b>Posted at" />
                    <asp:Label runat="server" Text='<%# Eval("PostedAt") %>' />
                </eclipse:TwoColumnPanel>

            </div>
            <div style="float: left; width: 50%">
                <eclipse:TwoColumnPanel ID="TwoColumnPanel1" WidthLeft="30%" WidthRight="70%" runat="server"
                    SkinID="PrintVisible">
                    <eclipse:LeftLabel runat="server" Text="<b>Division" />
                    <asp:Label runat="server" Text='<%# Eval("DivisionName") %>' />
                    <eclipse:LeftLabel runat="server" Text="<b>Bank Account No." />
                    <asp:Label runat="server" Text='<%# Eval("BankAccountNo") %>' />
                    <eclipse:LeftLabel runat="server" Text="<b>Bank Name/Place" />
                    <asp:Label runat="server" Text='<%# Eval("BankName") %>' />
                    <eclipse:LeftLabel runat="server" Text="<b>GPF A/c No." />
                    <asp:Label runat="server" Text='<%# Eval("GPFAccountNo") %>' />
                    <eclipse:LeftLabel runat="server" Text="<b>GIS A/c No." />
                    <asp:Label runat="server" Text='<%# Eval("GISAccountNumber") %>' />
                    <eclipse:LeftLabel runat="server" Text="<b>NPPF Number" />
                    <asp:Label runat="server" Text='<%# Eval("NPPFNumber") %>' />
                    <eclipse:LeftLabel runat="server" Text="<b>Citizen I/D No." />
                    <asp:Label runat="server" Text='<%# Eval("CitizenCardNo") %>' />
                    <eclipse:LeftLabel runat="server" Text="<b>TPN No." />
                    <asp:Label runat="server" Text='<%# Eval("Tpn") %>' />
                    <eclipse:LeftLabel runat="server" Text="<b>Gross BCA" />
                    <asp:Label ID="lblGrossBCA" runat="server" OnPreRender="lblGrossBCA_PreRender" />
                    <eclipse:LeftLabel ID="LeftLabel24" runat="server" Text="<b>Slab Deduction" />
                    <asp:Label ID="lblSlabDeduction" runat="server" OnPreRender="lblSlabDeduction_PreRender" />
                    <eclipse:LeftLabel ID="LeftLabel25" runat="server" Text="<b>Net BCA" />
                    <asp:Label ID="lblNetBCA" runat="server" OnPreRender="lblNetBCA_PreRender" />
                    <eclipse:LeftLabel ID="LeftLabel26" runat="server" Text="<b>Nationality" />
                    <asp:Label runat="server" Text='<%# Eval("IsBhutanese") %>' />
                    <eclipse:LeftLabel runat="server" Text="<b>Parent Organization" />
                    <asp:Label runat="server" Text='<%# Eval("ParentOrganization") %>' />
                </eclipse:TwoColumnPanel>
            </div>
    
        </ItemTemplate>

    </asp:ListView>
    <phpa:PhpaLinqDataSource ID="dsPayBillDtl" runat="server" ContextTypeName="Eclipse.PhpaLibrary.Database.Payroll.PayrollDataContext"
        TableName="PeriodEmployeeAdjustments" RenderLogVisible="false">
    </phpa:PhpaLinqDataSource>
    <jquery:GridViewEx ID="gvPayBillDtl" runat="server" AutoGenerateColumns="false" OnRowDataBound="gvPayBillDtl_RowDataBound"
        ShowFooter="true" EnableViewState="false">
        <Columns>
            <eclipse:SequenceField />
            <eclipse:MultiBoundField DataFields="EmployeePeriod.SalaryPeriod.SalaryPeriodStart.Month,EmployeePeriod.SalaryPeriod.SalaryPeriodEnd.Year"
                DataFormatString="{0}/{1}" HeaderText="Month/Year" FooterText="Total" />
            <eclipse:MultiBoundField DataFields="EmployeePeriod.BasicPay" DataFormatString="{0:N0}"
                AccessibleHeaderText="BasicPay" HeaderText="Basic Pay" DataSummaryCalculation="ValueSummation"
                DataFooterFormatString="{0:N0}">
                <FooterStyle HorizontalAlign="Right"></FooterStyle>
                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                <ItemStyle HorizontalAlign="Right"></ItemStyle>
            </eclipse:MultiBoundField>
        </Columns>
        <EmptyDataTemplate>
            No salary provided to the employee during the given period.
        </EmptyDataTemplate>
    </jquery:GridViewEx>

             <br />
        <asp:Label id="lblSignatureMessage" runat="server" Text="This is computer generated document no signature is required." Font-Italic="true" Font-Bold="true" visible="false"></asp:Label>
        <br />  
            <br /> 
    <phpa:PhpaLinqDataSource ID="dsRepCat" runat="server" ContextTypeName="Eclipse.PhpaLibrary.Database.Payroll.PayrollDataContext"
        TableName="ReportCategories" RenderLogVisible="false" />
</asp:Content>
