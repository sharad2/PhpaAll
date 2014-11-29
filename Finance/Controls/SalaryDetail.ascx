<%@ Control Language="C#" CodeBehind="SalaryDetail.ascx.cs" Inherits="Finance.Controls.SalaryDetail" %>
<phpa:PhpaLinqDataSource ID="dsEmployeePeriod" RenderLogVisible="False" runat="server"
    ContextTypeName="Eclipse.PhpaLibrary.Database.Payroll.PayrollDataContext" TableName="EmployeePeriods"
    OnSelecting="dsEmployeePeriod_Selecting" OrderBy="Employee.Division.DivisionName, Employee.FirstName">
    <WhereParameters>
        <asp:Parameter Name="SalaryPeriodId" Type="Int32" />
        <asp:Parameter Name="EmployeeId" Type="Int32" />
        <asp:Parameter Name="DivisionId" Type="Int32" />
        <asp:Parameter Name="FromDate" Type="DateTime" />
        <asp:Parameter Name="ToDate" Type="DateTime" />
    </WhereParameters>
</phpa:PhpaLinqDataSource>
<div style="float: left">
    <hr style="clear: both" />
    <asp:ListView ID="lvDPayBill" EnableViewState="false" runat="server" OnItemCreated="lvDPayBill_ItemCreated"
        OnItemDataBound="lvDPayBill_ItemDataBound">
        <LayoutTemplate>
            <div id="itemPlaceholderContainer" runat="server">
                <div id="itemPlaceholder" runat="server" />
            </div>
        </LayoutTemplate>
        <ItemTemplate>
            <eclipse:TwoColumnPanel runat="server" SkinID="PrintVisible">
                <eclipse:LeftLabel runat="server" Text="<b>Division" />
                <asp:Label runat="server" Text='<%#Eval ("Employee.Division.DivisionName") %>' ToolTip='<%#Eval("Employee.Division.DivisionCode","DivisionCode : {0}") %>' />
                <eclipse:LeftLabel runat="server" Text="<b>Employee" />
                <asp:Label runat="server" Text='<%#Eval("Employee.FullName")%>' ToolTip='<%#Eval("Employee.EmployeeCode","Employee Code :{0}") %>' />
                <eclipse:LeftLabel runat="server" Text="<b>Designation" />
                <asp:Label Text='<%#Eval("Employee.Designation")%>' runat="server" />
                <eclipse:LeftLabel runat="server" Text="<b>Citizen Id Card No." />
                <asp:Label Text='<%#Eval("Employee.CitizenCardNo")%>' runat="server" />
                <eclipse:LeftLabel runat="server" Text="<b>Employee Type" />
                <asp:Label Text='<%#Eval("Employee.EmployeeType.Description")%>' runat="server" />
                <eclipse:LeftLabel runat="server" Text="<b>Grade" />
                <asp:Label Text='<%#Eval("Employee.Grade")%>' runat="server" />
                <eclipse:LeftLabel runat="server" Text="<b>Nationality</b>" />
                <asp:Label runat="server" Text='<%#((bool)Eval("Employee.IsBhutanese"))?"Bhutan National":"Non Bhutan National" %>' />
                <eclipse:LeftLabel runat="server" Text="<b>Basic Pay" />
                <asp:Label Text='<%#Eval("BasicPay", "{0:C}") %>' runat="server" />
                <eclipse:LeftLabel ID="LeftLabel8" runat="server" Text="<b>Net Payment" />
                <asp:Label runat="server" ID="lblNetPayment" ToolTip="Net Payment=(Basic Pay + Earnings - Deductions)" />
            </eclipse:TwoColumnPanel>
            <jquery:GridViewEx ID="gvSalaryDetails" runat="server" OnRowDataBound="gvSalaryDetails_RowDataBound"
                AutoGenerateColumns="false" ShowFooter="true" >
                <EmptyDataTemplate>
                    No adjustments are defined.
                </EmptyDataTemplate>
                <EmptyDataRowStyle BackColor="AliceBlue" />
                <Columns>
                    <eclipse:SequenceField />
                    <eclipse:MultiBoundField HeaderText="Adjustment" DataFields="Adjustment" FooterText="Total">
                    </eclipse:MultiBoundField>
                    <eclipse:MultiBoundField HeaderText="Earning" DataFields="Earning" DataSummaryCalculation="ValueSummation"
                        DataFormatString="{0:C2}">
                        <ItemStyle HorizontalAlign="Right" />
                        <HeaderStyle HorizontalAlign="Center" />
                        <FooterStyle HorizontalAlign="Right" />
                    </eclipse:MultiBoundField>
                    <eclipse:MultiBoundField HeaderText="Deduction" DataFields="Deduction" DataSummaryCalculation="ValueSummation"
                        DataFormatString="{0:C2}">
                        <ItemStyle HorizontalAlign="Right" />
                        <HeaderStyle HorizontalAlign="Center" />
                        <FooterStyle HorizontalAlign="Right" />
                    </eclipse:MultiBoundField>
                </Columns>
            </jquery:GridViewEx>
        </ItemTemplate>
        <ItemSeparatorTemplate>
            <hr />
        </ItemSeparatorTemplate>
    </asp:ListView>
</div>
