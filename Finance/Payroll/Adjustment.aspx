﻿<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" CodeBehind="Adjustment.aspx.cs"
    Inherits="Finance.Payroll.ManageAdjustments" Title="Adjustment Manager" %>

<%@ Register Src="~/Controls/PrinterFriendlyButton.ascx" TagName="PrinterFriendlyButton"
    TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function DeleteConfirmation(e) {
            return confirm("Are you sure you want to Delete the Adjustment?");
        }
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphSideNavigation" runat="server">
    <asp:HyperLink ID="HyperLink2" runat="server" Text="Help" NavigateUrl="~/Doc/Adjustment.doc.aspx" /><br />
    <uc2:PrinterFriendlyButton ID="PrinterFriendlyButton1" runat="server" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cph" runat="server">
    <br />
    <i:ButtonEx ID="btnNew" runat="server" Text="Create New Adjustment" ToolTip="Click to enter new ajustment's details."
        OnClick="btnNew_Click" Action="Submit" CausesValidation="false" RolesRequired="*"
        Icon="PlusThick" />
    <p>
        This is a list of all adjustments, i.e. allowances and deductions, which can be
        applied to the basic pay of any employee. Select an adjustment to view more details
        and to edit it.
    </p>
    <phpa:PhpaLinqDataSource ID="dsAdjustment" runat="server" ContextTypeName="Eclipse.PhpaLibrary.Database.Payroll.PayrollDataContext"
        TableName="Adjustments" RenderLogVisible="False" OrderBy="AdjustmentCode">
    </phpa:PhpaLinqDataSource>
    <jquery:GridViewEx ID="gvAdjustments" runat="server" AutoGenerateColumns="false"
        DataKeyNames="AdjustmentId" DataSourceID="dsAdjustment" Width="50em" OnSelectedIndexChanged="gvAdjustments_SelectedIndexChanged"
        EnableViewState="False" AllowSorting="True" OnRowDataBound="gvAdjustments_RowDataBound">
        <Columns>
            <eclipse:SequenceField />
            <asp:TemplateField ItemStyle-CssClass="noprint" HeaderStyle-CssClass="noprint" FooterStyle-CssClass="noprint">
                <ItemTemplate>
                    <asp:LinkButton ID="lnkSelect" runat="server" CommandName="Select" Text="Select">
                    </asp:LinkButton>
                </ItemTemplate>
            </asp:TemplateField>
            <phpa:BoolField DataField="IsDeduction" HeaderText="Type" TrueValue="&darr;" FalseValue="&uarr;"
                TrueToolTip="Deduction" FalseToolTip="Allowance">
                <ItemStyle ForeColor="OrangeRed" HorizontalAlign="Center" Font-Bold="true" VerticalAlign="Middle" />
                <FalseItemStyle ForeColor="Black" />
            </phpa:BoolField>
            <eclipse:MultiBoundField DataFields="IsDefault,EmployeeType.Description" HeaderText="Auto?"
                DataFormatString="{0::$IsDefault:Yes:} {1::$EmployeeType.Description and $IsDefault:for ~:}"
                HeaderToolTip="Whether the adjustment applies automatically to some types of employees">
            </eclipse:MultiBoundField>
            <%--            <asp:CheckBoxField DataField="IsDefault" HeaderText="Auto?" AccessibleHeaderText="IsDefault">
                <ItemStyle Width="15em" />
            </asp:CheckBoxField>--%>
            <eclipse:MultiBoundField DataFields="AdjustmentCode,Description" HeaderText="Adjustment"
                SortExpression="AdjustmentCode" DataFormatString="{0}:{1}" HeaderToolTip="Showing Adjustment Code and its Description." />
            <eclipse:MultiBoundField DataFields="FractionOfBasic" HeaderText="% Basic" SortExpression="FractionOfBasic"
                DataFormatString="{0:#0.##%}">
                <ItemStyle HorizontalAlign="Right" Wrap="false" />
            </eclipse:MultiBoundField>
            <eclipse:MultiBoundField DataFields="FlatAmount" HeaderText="Flat Amount" SortExpression="FlatAmount"
                DataFormatString="{0:N0}">
                <FooterStyle HorizontalAlign="Right"></FooterStyle>
                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                <ItemStyle HorizontalAlign="Right"></ItemStyle>
            </eclipse:MultiBoundField>
            <eclipse:MultiBoundField DataFields="AdjustmentCategory.ShortDescription" HeaderText="Category"
                ToolTipFields="AdjustmentCategory.Description">
            </eclipse:MultiBoundField>
        </Columns>
        <EmptyDataTemplate>
            No Adjustment exists.
        </EmptyDataTemplate>
    </jquery:GridViewEx>
    <jquery:Dialog ID="dlgEditor" runat="server" Title="Adjustment Editor" Position="RightTop"
        Width="400" EnableViewState="true" EnablePostBack="true" Visible="false">
        <ContentTemplate>
            <phpa:PhpaLinqDataSource ID="dsSpecificAdjustment" runat="server" ContextTypeName="Eclipse.PhpaLibrary.Database.Payroll.PayrollDataContext"
                EnableDelete="True" EnableInsert="True" EnableUpdate="True" TableName="Adjustments"
                RenderLogVisible="false" Where="AdjustmentId==@AdjustmentId" OnSelecting="dsSpecificAdjustment_Selecting"
                OnInserted="dsSpecificAdjustment_Inserted" OnUpdating="dsSpecificAdjustment_Updating"
                OnInserting="dsSpecificAdjustment_Inserting">
                <WhereParameters>
                    <asp:Parameter Name="AdjustmentId" Type="Int32" />
                </WhereParameters>
                <UpdateParameters>
                    <asp:Parameter Name="AdjustmentCode" Type="String" />
                    <asp:Parameter Name="AdjustmentCategoryId" Type="Int32" />
                    <asp:Parameter Name="IsDefault" Type="Boolean" />
                    <asp:Parameter Name="EmployeeTypeId" Type="Int32" />
                    <asp:Parameter Name="FractionOfBasic" Type="Double" />
                    <asp:Parameter Name="HeadOfAccountId" Type="Int32" />
                    <asp:Parameter Name="Description" Type="String" />
                    <asp:Parameter Name="FlatAmount" Type="Decimal" />
                    <asp:Parameter Name="IsDeduction" Type="Boolean" />
                </UpdateParameters>
                <InsertParameters>
                    <asp:Parameter Name="AdjustmentId" Type="Int32" />
                    <asp:Parameter Name="AdjustmentCode" Type="String" />
                    <asp:Parameter Name="AdjustmentCategoryId" Type="Int32" />
                    <asp:Parameter Name="IsDefault" Type="Boolean" />
                    <asp:Parameter Name="EmployeeTypeId" Type="Int32" />
                    <asp:Parameter Name="FractionOfBasic" Type="Double" />
                    <asp:Parameter Name="HeadOfAccountId" Type="Int32" />
                    <asp:Parameter Name="Description" Type="String" />
                    <asp:Parameter Name="FlatAmount" Type="Decimal" />
                    <asp:Parameter Name="IsDeduction" Type="Boolean" />
                </InsertParameters>
            </phpa:PhpaLinqDataSource>
            <asp:FormView ID="frmAdjustment" runat="server" DataKeyNames="AdjustmentId" DataSourceID="dsSpecificAdjustment"
                OnItemDeleted="frmAdjustment_ItemDeleted" OnItemInserted="frmAdjustment_ItemInserted"
                OnItemUpdated="frmAdjustment_ItemUpdated">
                <EmptyDataTemplate>
                    Please select any adjustment from the list to view and edit its details.<br />
                    <asp:LoginView ID="restrictUser" runat="server">
                        <LoggedInTemplate>
                            Click on the following link to enter new adjustment details.<br />
                            <i:LinkButtonEx ID="btnNewAdjustment" runat="server" Text="New Adjustment" ToolTip="Click to enter new ajustment's details."
                                OnClick="btnNew_Click" Action="Submit" CausesValidation="false" />
                        </LoggedInTemplate>
                        <AnonymousTemplate>
                            <b>Please login, if you want to create or manage employee details.</b>
                        </AnonymousTemplate>
                    </asp:LoginView>
                </EmptyDataTemplate>
                <HeaderTemplate>
                    <phpa:FormViewContextHeader ID="FormViewContextHeader1" runat="server" CurrentEntity='<%# Eval("AdjustmentCode") %>'
                        EntityName="Adjustment" />
                </HeaderTemplate>
                <ItemTemplate>
                    <jquery:Tabs ID="tabAdjustment" runat="server">
                        <jquery:JPanel runat="server" HeaderText="Adjustment Details" ID="panelDetails">
                            <eclipse:TwoColumnPanel runat="server">
                                <eclipse:LeftLabel runat="server" Text="Adjustment Code" />
                                <asp:Label runat="server" Text='<%# Eval("AdjustmentCode") %>' /><br />
                                <asp:Label runat="server" Text='<%# (bool)Eval("IsDeduction")? "&darr;" : "&uarr;"%>'
                                    ForeColor='<%# (bool)Eval("IsDeduction") ? System.Drawing.Color.Red : System.Drawing.Color.Empty %>' />
                                <asp:Label runat="server" Text='<%# (bool)Eval("IsDeduction")? "Deduction" :"Allowance"%>'
                                    ForeColor='<%# (bool)Eval("IsDeduction") ? System.Drawing.Color.Red : System.Drawing.Color.Empty %>' />
                                <eclipse:LeftLabel runat="server" Text="Adjustment Category" />
                                <asp:Label runat="server" Text='<%# Eval("AdjustmentCategory.AdjustmentCategoryCode") %>' />
                                <eclipse:LeftLabel runat="server" Text="Description" />
                                <asp:Label runat="server" Text='<%# Eval("Description") %>' />
                                <eclipse:LeftLabel runat="server" Text="Short Description" />
                                <asp:Label runat="server" Text='<%# Eval("ShortDescription") %>' />
                                <eclipse:LeftLabel runat="server" Text="Applicable to" />
                                <asp:Label runat="server" Text='<%# Eval("EmployeeType.Description") ?? "All Employees" %>' /><br />
                                <asp:CheckBox runat="server" Checked='<%# Eval("IsDefault")  %>' Text="Applies Automatically"
                                    Enabled="false"></asp:CheckBox>
                                <eclipse:LeftLabel runat="server" Text="Account Head" />
                                <asp:Label runat="server" Text='<%# string.Format("{0}: {1}", Eval("HeadOfAccount.DisplayName"), Eval("HeadOfAccount.Description")) %>'
                                    ToolTip='<%# Eval("HeadOfAccountId", "Internal Id: {0}") %>' />
                                <eclipse:LeftLabel runat="server" Text="Amount" />
                                <asp:Label runat="server" />
                                <%# Eval("FractionOfBasic", "{0:p} of basic salary")%>
                                <%# Eval("FractionOfBasic") != null && Eval("FlatAmount") != null ? "plus" : ""%>
                                <%# Eval("FlatAmount","Nu {0:N2}") %>
                            </eclipse:TwoColumnPanel>
                        </jquery:JPanel>
                        <phpa:AuditTabPanel ID="panelAudit" runat="server" CssClasses="PanelContainer" LeftCssClass="PanelLeftField"
                            RightCssClass="PanelRightField">
                        </phpa:AuditTabPanel>
                    </jquery:Tabs>
                    <asp:LoginView ID="AdjustmentLoginView" runat="server">
                        <LoggedInTemplate>
                            <i:LinkButtonEx ID="btnEdit" runat="server" Text="Edit" CausesValidation="false"
                                OnClick="btnEdit_Click" />
                            <i:LinkButtonEx ID="LinkButtonEx1" runat="server" Text="Delete" OnClick="btnDelete_Click"
                                Action="Submit" CausesValidation="false" OnClientClick="DeleteConfirmation" />
                            <i:LinkButtonEx ID="btnNewadj" runat="server" Text="New" OnClick="btnNew_Click" Action="Submit"
                                CausesValidation="false" />
                        </LoggedInTemplate>
                        <AnonymousTemplate>
                            <b>Please login, if you want to create or manage adjustment details.</b>
                        </AnonymousTemplate>
                    </asp:LoginView>
                </ItemTemplate>
                <EditItemTemplate>
                    <eclipse:TwoColumnPanel runat="server">
                        <eclipse:LeftLabel ID="ctlAdjustmentType" runat="server" Text="Adjustment Type" />
                        <i:RadioButtonListEx ID="rblIsDeduction" runat="server" Value='<%# Bind("IsDeduction") %>'
                            Orientation="Horizontal">
                            <Items>
                                <i:RadioItem Text="&darr; Deduction" Value="True" Enabled="true" />
                                <i:RadioItem Text="&uarr; Allowance" Value="False" />
                            </Items>
                        </i:RadioButtonListEx>
                        <eclipse:LeftLabel runat="server" Text="Adjustment Code" />
                        <i:TextBoxEx ID="tbAdjustmentCode" runat="server" MaxLength="15" Text='<%# Bind("AdjustmentCode") %>'
                            CaseConversion="UpperCase">
                            <Validators>
                                <i:Required />
                            </Validators>
                        </i:TextBoxEx>
                        An easy to remember short code which you will use to refer to this adjustment.
                        <eclipse:LeftLabel runat="server" Text="Adjustment Category" />
                        <phpa:PhpaLinqDataSource ID="dsAdjustmentCategory" runat="server" ContextTypeName="Eclipse.PhpaLibrary.Database.Payroll.PayrollDataContext"
                            TableName="AdjustmentCategories" RenderLogVisible="false" OrderBy="ShortDescription" />
                        <i:DropDownListEx runat="server" ID="ddlCategories" Value='<%# Bind("AdjustmentCategoryId") %>'
                            DataSourceID="dsAdjustmentCategory" DataTextField="ShortDescription" DataValueField="AdjustmentCategoryId"
                            DataOptionGroupField="CategoryTypeDescription">
                            <Items>
                                <eclipse:DropDownItem Text="(Not set)" Persistent="Always" />
                            </Items>
                        </i:DropDownListEx>
                        <eclipse:LeftLabel runat="server" Text="Description" />
                        <i:TextBoxEx ID="tbDescription" runat="server" Text='<%# Bind("Description") %>'>
                        </i:TextBoxEx>
                        <eclipse:LeftLabel runat="server" Text="Short Description" />
                        <i:TextBoxEx ID="tbShortDescription" runat="server" Text='<%# Bind("ShortDescription") %>'>
                            <Validators>
                                <i:Value MaxLength="20" />
                            </Validators>
                        </i:TextBoxEx>
                        <br />
                        Short Desription will reflect in Pay Bill header text.
                        <eclipse:LeftLabel ID="lblEmpType" runat="server" Text="Applicable to" />
                        <phpa:PhpaLinqDataSource ID="dsEmpType" runat="server" ContextTypeName="Eclipse.PhpaLibrary.Database.Payroll.PayrollDataContext"
                            Select="new (EmployeeTypeId, Description)" TableName="EmployeeTypes" RenderLogVisible="false">
                        </phpa:PhpaLinqDataSource>
                        <i:DropDownListEx runat="server" ID="ddlEmpTypeList" Value='<%# Bind("EmployeeTypeId") %>'
                            DataSourceID="dsEmpType" DataTextField="Description" DataValueField="EmployeeTypeId"
                            ToolTip="Choose All Employee to apply to all.">
                            <Items>
                                <eclipse:DropDownItem Text="(All Employees)" Persistent="Always" />
                            </Items>
                        </i:DropDownListEx>
                        <i:CheckBoxEx ID="chkEmpTypeFlag" runat="server" Text="Apply Automatically" Checked='<%# Bind("IsDefault") %>' />
                        <eclipse:LeftLabel runat="server" Text="Head of Account" />
                        <i:AutoComplete ID="tbHeadOdAccountSelector" runat="server" FriendlyName="Head of Account"
                            Width="20em" Value='<%# Bind("HeadOfAccountId") %>' Text='<%# Eval("HeadOfAccount.DisplayName") %>'
                            WebMethod="GetLeafHeadOfAccountsForTypes" ValidateWebMethodName="ValidateHeadOfAccount"
                            WebServicePath="~/Services/HeadOfAccounts.asmx" AutoValidate="true">
                        </i:AutoComplete>
                        <br />
                        When a voucher is created, the amount will be debited/credited against this head.
                        It must be entered before the salary is actually paid.
                        <eclipse:LeftLabel runat="server" Text="% of Basic" />
                        <i:TextBoxEx ID="tbPercentageBasic" runat="server" Value='<%# Bind("FractionOfBasic") %>'
                            MaxLength="5" OnDataBinding="tbPercentageBasic_DataBinding" FriendlyName="Percentage Basic">
                            <Validators>
                                <i:Value ValueType="Decimal" Max="100" Min="0" />
                            </Validators>
                        </i:TextBoxEx>
                        <br />
                        % of basic salary.
                        <eclipse:LeftLabel runat="server" Text="Flat Amount" />
                        <i:TextBoxEx ID="tbFlatAmount" runat="server" Text='<%# Bind("FlatAmount", "{0:C2}") %>'
                            FriendlyName="Flat Amount">
                            <Validators>
                                <i:Value ValueType="Decimal" />
                            </Validators>
                        </i:TextBoxEx>
                        <br />
                        Its an amount which is fixed and not based on % of basic salary.
                    </eclipse:TwoColumnPanel>
                    <br />
                    <i:ButtonEx runat="server" ID="btnSave" Text="Save" CausesValidation="true" Action="Submit"
                        Icon="Disk" OnClick="btnSave_Click" />
                    <i:LinkButtonEx ID="btnCancel" runat="server" Text="Cancel" CausesValidation="false"
                        OnClick="btnCancel_Click" />
                    <i:ValidationSummary ID="valSummary" runat="server" />
                </EditItemTemplate>
                <FooterTemplate>
                    <phpa:FormViewStatusMessage ID="FormViewStatusMessage1" runat="server">
                    </phpa:FormViewStatusMessage>
                </FooterTemplate>
            </asp:FormView>
        </ContentTemplate>
    </jquery:Dialog>
</asp:Content>
