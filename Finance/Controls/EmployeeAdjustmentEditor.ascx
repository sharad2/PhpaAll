<%@ Control Language="C#" CodeBehind="EmployeeAdjustmentEditor.ascx.cs" Inherits="Finance.Controls.EmployeeAdjustmentEditor" %>
<%@ Import Namespace="System.Drawing" %>
<script type="text/javascript">
    function cbFractionBasicOverrriden_Click(e) {
        var $tb = $(this).closest('td').find('input:text');
        if ($(this).is(':checked')) {
            $tb.removeAttr('readOnly');
        }
        else {
            $tb.attr('readOnly', 'true');
        }
    }
    function cbFractionGrossOverrriden_Click(e) {
        var $tb = $(this).closest('td').find('input:text');
        if ($(this).is(':checked')) {
            $tb.removeAttr('readOnly');
        }
        else {
            $tb.attr('readOnly', 'true');
        }
    }
</script>
<phpa:PhpaLinqDataSource ID="dsEditEmpAdjustments" runat="server" ContextTypeName="Eclipse.PhpaLibrary.Database.Payroll.PayrollDataContext"
    TableName="EmployeeAdjustments" AutoGenerateWhereClause="True" OnSelecting="dsEditEmpAdjustments_Selecting"
    RenderLogVisible="False" EnableUpdate="True" EnableDelete="True" EnableInsert="True">
    <UpdateParameters>
        <asp:Parameter Type="Decimal" Name="Deductions" />
        <asp:Parameter Type="Boolean" Name="IsFlatAmountOverridden" />
        <asp:Parameter Type="Boolean" Name="IsFractionGrossOverridden" />
        <asp:Parameter Type="Boolean" Name="IsFractionBasicOverridden" />
        <asp:Parameter Type="Decimal" Name="FlatAmount" />
        <asp:Parameter Type="Double" Name="FractionOfBasic" />
        <asp:Parameter Type="Double" Name="FractionOfGross" />
    </UpdateParameters>
    <WhereParameters>
        <asp:Parameter Name="EmployeeId" Type="Int32" />
    </WhereParameters>
    <InsertParameters>
        <asp:Parameter Name="AdjustmentId" Type="Int32" />
        <asp:Parameter Name="EmployeeId" Type="Int32" />
        <asp:Parameter Type="Decimal" Name="Deductions" />
        <asp:Parameter Type="Boolean" Name="IsFlatAmountOverridden" />
        <asp:Parameter Type="Boolean" Name="IsFractionBasicOverridden" />
        <asp:Parameter Type="Boolean" Name="IsFractionGrossOverridden" />
        <asp:Parameter Type="Decimal" Name="FlatAmount" />
        <asp:Parameter Type="Double" Name="FractionOfBasic" />
        <asp:Parameter Type="Double" Name="FractionOfGross" />
    </InsertParameters>
</phpa:PhpaLinqDataSource>
<i:LinkButtonEx ID="btnNew" runat="server" OnClick="btnNew_Click" Text="Add New Adjustment"
    Action="Submit" RolesRequired="PayrollOperator" />
<i:ValidationSummary ID="valSummary" runat="server" />
<asp:HiddenField runat="server" ID="hdemployeeid" ClientIDMode="Static" />
<jquery:GridViewExInsert ID="gvEditEmpAdjustments" runat="server" AutoGenerateColumns="False"
    ClientIDMode="Static" EnableViewState="true" DataKeyNames="EmployeeAdjustmentId"
    OnRowDataBound="gvEditEmpAdjustments_RowDataBound" DataSourceID="dsEditEmpAdjustments"
    ShowFooter="false" OnRowUpdated="gvEditEmpAdjustments_RowUpdated" OnRowUpdating="gvEditEmpAdjustments_RowUpdating"
    Width="100%" OnRowDeleted="gvEditEmpAdjustments_RowDeleted" InsertRowsAtBottom="false"
    OnRowInserting="gvEditEmpAdjustments_RowInserting" OnRowInserted="gvEditEmpAdjustments_RowInserted">
    <Columns>
        <jquery:CommandFieldEx ShowDeleteButton="true" RolesRequired="PayrollOperator" DeleteConfirmationText="Adjustment will be deleted. Are you sure you want to delete Adjustment?">
        </jquery:CommandFieldEx>
        <eclipse:MultiBoundField DataFields="Adjustment.AdjustmentCode,Adjustment.Description"
            DataFormatString="{0}:{1}" HeaderText="Adjustment">
            <InsertItemTemplate>
                <i:AutoComplete ID="tbAdjustmentCodeNew" runat="server" Value='<%# Bind("AdjustmentId") %>'
                    FriendlyName="Adjustment Code" Text='<%# Eval("AdjustmentCode") %>' WebMethod="GetAdjustmentCode"
                    WebServicePath="~/Services/Adjustments.asmx" OnClientSearch="EmployeeSearch"
                    ValidateWebMethodName="ValidateAdjustmentCode">
                    <Validators>
                        <i:Required />
                        <i:Custom OnServerValidate="tb_ServerValidate" />
                    </Validators>
                </i:AutoComplete>
            </InsertItemTemplate>
        </eclipse:MultiBoundField>
        <asp:TemplateField>
            <HeaderTemplate>
                <span title="Display Flat amount which is defined explicitly or default for Adjustment.">
                    Amount </span>
            </HeaderTemplate>
            <ItemTemplate>
                <div style="text-align: right">
                    <phpa:InfoImage runat="server" ID="infoImg2" Visible='<%# Eval("IsFlatAmountOverridden") %>'
                        ToolTip="You have explicitly changed the amount of this adjustment for this employee"
                        EnableViewState="true" />
                    <asp:Label ID="Label4" runat="server" Text='<%# Eval("FlatAmount","{0:#0.##}") %>' />
                </div>
            </ItemTemplate>
            <EditItemTemplate>
                <i:CheckBoxEx ID="cbFlatAmountOverridden" runat="server" Text="Override Default"
                    Checked='<%# Bind("IsFlatAmountOverridden") %>' OnClientClick="cbFractionBasicOverrriden_Click" />
                <div style="text-align: left; white-space: nowrap">
                    <i:TextBoxEx ID="tbFlatAmount" runat="server" QueryStringValue='<%# Bind("FlatAmount", "{0}") %>'
                        FriendlyName="Flat Amount" ReadOnly='<%# !((bool)Eval("IsFlatAmountOverridden")) %>'>
                        <Validators>
                            <i:Filter DependsOn="cbFlatAmountOverridden" DependsOnState="Checked" />
                            <i:Required />
                            <i:Value ValueType="Decimal" Min="0" />
                        </Validators>
                    </i:TextBoxEx>
                </div>
            </EditItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField>
            <HeaderTemplate>
                <span title="Display percentage of basic which is defined explicitly or default for Adjustment">
                    % Basic</span>
            </HeaderTemplate>
            <ItemTemplate>
                <div style="text-align: right">
                    <phpa:InfoImage runat="server" ID="infoImg" Visible='<%# Eval("IsFractionBasicOverridden") %>'
                        ToolTip="You have explicitly changed the amount of this adjustment for this employee"
                        EnableViewState="true" />
                    <asp:Label runat="server" Text='<%# Eval("FractionOfBasic", "{0:#0.##%}") %>' />
                </div>
            </ItemTemplate>
            <EditItemTemplate>
                <i:CheckBoxEx ID="cbFractionBasicOverrriden" runat="server" Text="Override Default"
                    Checked='<%# Bind("IsFractionBasicOverridden") %>' OnClientClick="cbFractionBasicOverrriden_Click" />
                <div style="text-align: left; white-space: nowrap">
                    <i:TextBoxEx ID="tbFractionOfBasic" runat="server" QueryStringValue='<%# Bind("FractionOfBasic", "{0}") %>'
                        FriendlyName="Fraction Of Basic" MaxLength="6" ReadOnly='<%# !((bool)Eval("IsFractionBasicOverridden")) %>'
                        OnDataBinding="tbFractionOfBasic_DataBinding">
                        <Validators>
                            <i:Filter DependsOn="cbFractionBasicOverrriden" DependsOnState="Checked" />
                            <i:Required />
                            <i:Value ValueType="Decimal" Max="100" Min="0" />
                        </Validators>
                    </i:TextBoxEx>
                    %
                </div>
            </EditItemTemplate>
        </asp:TemplateField>
              <asp:TemplateField>
            <HeaderTemplate>
                <span title="Display percentage of basic which is defined explicitly or default for Adjustment">
                    % Gross</span>
            </HeaderTemplate>
            <ItemTemplate>
                <div style="text-align: right">
                    <phpa:InfoImage runat="server" ID="infoImg1" Visible='<%# Eval("IsFractionGrossOverridden") %>'
                        ToolTip="You have explicitly changed the amount of this adjustment for this employee"
                        EnableViewState="true" />
                    <asp:Label runat="server" Text='<%# Eval("FractionOfGross", "{0:#0.##%}") %>' />
                </div>
            </ItemTemplate>
            <EditItemTemplate>
                <i:CheckBoxEx ID="cbFractionGrossOverrriden" runat="server" Text="Override Default"
                    Checked='<%# Bind("IsFractionBasicOverridden") %>' OnClientClick="cbFractionGrossOverrriden_Click" />
                <div style="text-align: left; white-space: nowrap">
                    <i:TextBoxEx ID="tbFractionOfGross" runat="server" QueryStringValue='<%# Bind("FractionOfGross", "{0}") %>'
                        FriendlyName="Fraction Of Basic" MaxLength="6" ReadOnly='<%# !((bool)Eval("IsFractionGrossOverridden")) %>'
                        OnDataBinding="tbFractionOfGross_DataBinding">
                        <Validators>
                            <i:Filter DependsOn="cbFractionGrossOverrriden" DependsOnState="Checked" />
                            <i:Required />
                            <i:Value ValueType="Decimal" Max="100" Min="0" />
                        </Validators>
                    </i:TextBoxEx>
                    %
                </div>
            </EditItemTemplate>
        </asp:TemplateField>
    
        <phpa:BoolField HeaderText="Type" DataField="Adjustment.IsDeduction" TrueValue="Deduction"
            FalseValue="Allowance" />
        <asp:TemplateField HeaderText="Remarks">
            <ItemStyle HorizontalAlign="Left" />
            <ItemTemplate>
                <asp:Label runat="server" Text='<%# Eval("Comment")%>' />
            </ItemTemplate>
            <EditItemTemplate>
                <i:TextBoxEx ID="tbComment" runat="server" Text='<%# Bind("Comment")%>' MaxLength="20"
                    Size="10" />
            </EditItemTemplate>
        </asp:TemplateField>
    </Columns>
    <EmptyDataTemplate>
        Select Employee from the Grid.
    </EmptyDataTemplate>
</jquery:GridViewExInsert>

