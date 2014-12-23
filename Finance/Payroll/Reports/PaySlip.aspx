<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" CodeBehind="PaySlip.aspx.cs"
    Inherits="Finance.Payroll.Reports.PaySlip" Title="Pay Slip" EnableViewState="false" %>

<%@ Register Src="~/Controls/PrinterFriendlyButton.ascx" TagName="PrinterFriendlyButton"
    TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphSideNavigation" runat="server">
    <uc2:PrinterFriendlyButton ID="PrinterFriendlyButton1" runat="server" EnableTheming="true" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cph" runat="server">
    <eclipse:TwoColumnPanel ID="TwoColumnPanel1" runat="server" IsValidationContainer="true">
        <eclipse:LeftLabel ID="LeftLabel1" runat="server" Text="Pay Slip for the month of" />
        <i:TextBoxEx ID="tbDate" runat="server" FriendlyName="Date">
            <Validators>
                <i:Date />
                <i:Required />
            </Validators>
        </i:TextBoxEx>
        <eclipse:LeftLabel ID="LeftLabel2" runat="server" Text="Employee" />
        <i:AutoComplete ID="tbEmployee" runat="server" ClientIDMode="Static" Width="25em"
            WebMethod="GetEmployees" WebServicePath="~/Services/Employees.asmx" AutoValidate="true"
            ValidateWebMethodName="ValidateEmployee">
            <%--  <Validators>
                <i:Required />
            </Validators>--%>
        </i:AutoComplete>
        <br />
        <phpa:PhpaLinqDataSource ID="dsBankName" runat="server" ContextTypeName="Eclipse.PhpaLibrary.Database.Payroll.PayrollDataContext"
             RenderLogVisible="false" TableName="Banks" Select="new (BankName,BankId)">
        </phpa:PhpaLinqDataSource>
        <eclipse:LeftLabel ID="LeftLabel11" runat="server" Text="Bank Name" />
        <i:DropDownListEx ID="ddlBankName" runat="server" EnableViewState="false" DataSourceID="dsBankName"
            DataTextField="BankName" DataValueField="BankId" >
            <Items>
                <eclipse:DropDownItem Text="(All)" Persistent="Always" />
            </Items>
        </i:DropDownListEx>
        <br />
        <eclipse:LeftLabel ID="LeftLabel12" runat="server" Text="Division" />
        <phpa:PhpaLinqDataSource runat="server" ID="dsDivision" ContextTypeName="Eclipse.PhpaLibrary.Database.PIS.PISDataContext"
            Select="new (DivisionId, DivisionName, DivisionGroup)" TableName="Divisions"
            RenderLogVisible="false">
        </phpa:PhpaLinqDataSource>
        <i:DropDownListEx runat="server" ID="ddlDivision" DataSourceID="dsDivision" DataTextField="DivisionName"
            DataValueField="DivisionId" Value='<%# Bind("DivisionId") %>' FriendlyName="Division"
            DataOptionGroupField="DivisionGroup">
            <Items>
                <eclipse:DropDownItem Text="(All Divisions)" Value="" Persistent="Always" />
            </Items>
        </i:DropDownListEx>
        <br />
        <i:ButtonEx ID="btnShowPaySlip" runat="server" Text="Go" CausesValidation="true"
            Action="Submit" OnClick="btnShowPaySlip_Click" Icon="Refresh" />
        <i:ValidationSummary ID="ValidationSummary1" runat="server" />
    </eclipse:TwoColumnPanel>
    <br />
    <div style="border: 1mm; color: Gray; font-family: Times New Roman; font-size: large;
        font-style: italic; font-weight: bold; text-align: center">
        <asp:Label runat="server" ID="lblCaption" /></div>
    <asp:Repeater ID="rpt1" runat="server" OnItemDataBound="rpt1_ItemDataBound">
        <ItemTemplate>
            <b>
                <asp:FormView ID="frmView" runat="server">
                    <ItemTemplate>
                        <div style="float: left">
                            <asp:ImageMap ID="ImageMap1" runat="server" ImageUrl='<%$ AppSettings:logo %>'>
                                <asp:CircleHotSpot HotSpotMode="NotSet" X="20" Y="20" Radius="75" />
                            </asp:ImageMap>
                        </div>
                        <div style="float: left">
                            <eclipse:TwoColumnPanel ID="TwoColumnPanel1" runat="server" SkinID="PrintVisible">
                                <eclipse:LeftLabel ID="LeftLabel3" runat="server" Text="<b>Name" />
                                <asp:Label ID="Label2" runat="server" Text='<%# Eval("FullName") %>' />
                                <eclipse:LeftLabel ID="LeftLabel2" runat="server" Text="<b>Designation" />
                                <asp:Label ID="Label1" runat="server" Text='<%# Eval("Designation") %>' />
                                <eclipse:LeftLabel ID="LeftLabel4" runat="server" Text="<b>Emp. Code" />
                                <asp:Label ID="Label3" runat="server" Text='<%# Eval("EmployeeCode") %>' />
                                <eclipse:LeftLabel ID="LeftLabel5" runat="server" Text="<b>Parent Organization" />
                                <asp:Label ID="Label4" runat="server" Text='<%# Eval("ParentOrganization") %>' />
                                <eclipse:LeftLabel ID="LeftLabel6" runat="server" Text="<b>Identity Card No." />
                                <asp:Label ID="Label5" runat="server" Text='<%# Eval("CitizenCardNo") %>' />
                            </eclipse:TwoColumnPanel>
                        </div>
                        <div style="float: right">
                            <eclipse:TwoColumnPanel ID="TwoColumnPanel2" runat="server" SkinID="PrintVisible">
                                <eclipse:LeftLabel ID="LeftLabel1" runat="server" Text="<b>Bank Account No." />
                                <asp:Label ID="Label6" runat="server" Text='<%# Eval("BankAccountNo") %>' />
                                <eclipse:LeftLabel ID="LeftLabel7" runat="server" Text="<b>Bank Name" />
                                <asp:Label ID="Label7" runat="server" Text='<%# Eval("BankName") %>' />
                                <eclipse:LeftLabel ID="LeftLabel8" runat="server" Text="<b>Bank Place" />
                                <asp:Label ID="Label8" runat="server" Text='<%# Eval("BankPlace") %>' />
                                <eclipse:LeftLabel ID="LeftLabel9" runat="server" Text="<b>GPF Account Number" />
                                <asp:Label ID="Label9" runat="server" Text='<%# Eval("GPFAccountNumber") %>' />
                                <eclipse:LeftLabel ID="LeftLabel10" runat="server" Text="<b>NPPF Number" />
                                <asp:Label ID="Label10" runat="server" Text='<%# Eval("NPPFNumber") %>' />
                            </eclipse:TwoColumnPanel>
                        </div>
                    </ItemTemplate>
                </asp:FormView>
                <%--  <phpa:PhpaLinqDataSource ID="ds" runat="server" ContextTypeName="Eclipse.PhpaLibrary.Database.Payroll.PayrollDataContext"
                RenderLogVisible="false" OnSelecting="ds_Selecting">
            </phpa:PhpaLinqDataSource>--%>
                
                <jquery:GridViewEx ID="gv" runat="server" AutoGenerateColumns="false" ShowExpandCollapseButtons="false">
                    <Columns>
                        <eclipse:MultiBoundField DataFields="BasicSalary" HeaderText="Basic" DataFormatString="{0:C2}">
                            <ItemStyle HorizontalAlign="Right" />
                        </eclipse:MultiBoundField>
                        <jquery:MatrixField DataMergeFields="EmployeeId" DataHeaderFields="IsDeduction,AdjustmentCode"
                            DataValueFields="Amount" DataHeaderFormatString="{0::$IsDeduction:Deductions:Earnings}|{1}"
                            OnMatrixRowDataBound="gv_MatrixRowDataBound" DisplayRowTotals="true" RowTotalHeaderText="Total Sanction|Total Deduction"
                            DataValueFormatString="{0:C0}">
                            <ItemStyle HorizontalAlign="Right" />
                        </jquery:MatrixField>
                        <eclipse:MultiBoundField HeaderText="Net Pay" AccessibleHeaderText="NetPay">
                            <ItemStyle HorizontalAlign="Right" />
                        </eclipse:MultiBoundField>
                    </Columns>
                </jquery:GridViewEx>
            </b>
            <div id="divMargin">
            </div>
         <br />
        <i><b>This is computer generated document no signature is required.</i></b>
        <br />  
            <br />  
        </ItemTemplate> 
    </asp:Repeater>
</asp:Content>
