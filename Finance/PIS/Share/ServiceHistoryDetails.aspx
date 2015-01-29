<%@ Page Language="C#" CodeBehind="ServiceHistoryDetails.aspx.cs" Inherits="PhpaAll.PIS.Share.ServiceHistoryDetails"
    EnableViewState="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <jquery:JQueryScriptManager runat="server" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <phpa:PhpaLinqDataSource runat="server" ID="dsServiceHistoryDetails" TableName="ServicePeriods"
            ContextTypeName="Eclipse.PhpaLibrary.Database.PIS.PISDataContext" RenderLogVisible="false"
            Where="ServicePeriodId == @ServicePeriodId" OnSelecting="dsServiceHistoryDetails_Selecting"
            OrderBy="PeriodStartDate desc">
            <WhereParameters>
                <asp:QueryStringParameter Name="ServicePeriodId" Type="Int32" QueryStringField="ServicePeriodId" />
            </WhereParameters>
        </phpa:PhpaLinqDataSource>
        <asp:FormView runat="server" ID="fvServiceHistoryDetails" DataSourceID="dsServiceHistoryDetails"
            DataKeyNames="ServicePeriodId" DefaultMode="ReadOnly">
            <EmptyDataTemplate>
                Service period not defined.
            </EmptyDataTemplate>
            <ItemTemplate>
                <jquery:Tabs runat="server" Selected="0" Collapsible="false">
                    <jquery:JPanel runat="server" HeaderText="Primary">
                        <eclipse:TwoColumnPanel runat="server">
                            <eclipse:LeftLabel runat="server" Text="Designation" />
                            <asp:Label runat="server" Text='<%# Eval("Designation") %>' />
                            <eclipse:LeftLabel runat="server" Text="Grade" />
                            <asp:Label runat="server" Text='<%# Eval("Grade") %>' />
                            <eclipse:LeftLabel runat="server" Text="Posted At" />
                            <asp:Label runat="server" Text='<%# Eval("PostedAt") %>' />
                            <eclipse:LeftLabel runat="server" Text="Start Date" />
                            <asp:Label runat="server" Text='<%# Eval("PeriodStartDate","{0:d}") %>' />
                            <eclipse:LeftLabel runat="server" Text="End Date" />
                            <asp:Label runat="server" Text='<%# Eval("PeriodEndDate","{0:d}") %>' />
                            <eclipse:LeftLabel runat="server" Text="Order No" />
                            <asp:Label runat="server" Text='<%# Eval("GovtOrderNo") %>' />
                            <eclipse:LeftLabel runat="server" Text="Order Date" />
                            <asp:Label runat="server" Text='<%# Eval("GovtOrderDate","{0:d}") %>' />
                            <eclipse:LeftLabel runat="server" Text="Initial Term" />
                            <asp:Label runat="server" Text='<%# Eval("InitialTerm") %>' />
                            <eclipse:LeftLabel runat="server" Text="Remarks" />
                            <asp:Label runat="server" Text='<%# Eval("Remarks") %>' />
                        </eclipse:TwoColumnPanel>
                    </jquery:JPanel>
                    <jquery:JPanel runat="server" HeaderText="Financial">
                        <legend>Salary</legend>
                        <%# Eval("PayScale") %>
                        <eclipse:TwoColumnPanel runat="server">
                            <eclipse:LeftLabel runat="server" Text="Proposed Basic Salary" />
                            <asp:Label runat="server" Text='<%# Eval("BasicSalary","{0:N2}") %>' ToolTip="Salary authorized from the first day of the service period" />
                            <eclipse:LeftLabel runat="server" Text="Consolidated" />
                            <asp:Label runat="server" Text='<%# Eval("IsConsolidated") %>' />
                            <eclipse:LeftLabel runat="server" Text="Min PayScale" />
                            <asp:Label runat="server" Text='<%# Eval("MinPayScaleAmount","{0:N2}") %>' />
                            <eclipse:LeftLabel runat="server" Text="Max PayScale" />
                            <asp:Label runat="server" Text='<%# Eval("MaxPayScaleAmount","{0:N2}") %>' />
                            <eclipse:LeftLabel runat="server" Text="Increment Amount" />
                            <asp:Label runat="server" Text='<%# Eval("IncrementAmount","{0:N2}") %>' />
                            <eclipse:LeftLabel runat="server" Text="Increment Date" />
                            <asp:Label runat="server" Text='<%# Eval("DateOfIncrement","{0:d}") %>' />
                            <eclipse:LeftLabel runat="server" Text="Next Increment Date" />
                            <asp:Label runat="server" Text='<%# Eval("DateOfNextIncrement","{0:d}") %>' />
                        </eclipse:TwoColumnPanel>
                    </jquery:JPanel>
                    <jquery:JPanel runat="server" HeaderText="Promotion">
                        <eclipse:TwoColumnPanel runat="server">
                            <eclipse:LeftLabel runat="server" Text="Promotion Date" />
                            <asp:Label runat="server" Text='<%# Eval("PromotionDate","{0:d}") %>' />
                            <eclipse:LeftLabel runat="server" Text="Promotion Type" />
                            <asp:Label runat="server" Text='<%# Eval("PromotionType.PromotionDescription") %>' />
                            <eclipse:LeftLabel runat="server" Text="Extension Upto" />
                            <asp:Label runat="server" Text='<%# Eval("ExtensionUpto","{0:d}") %>' />
                            <eclipse:LeftLabel runat="server" Text="Next Promotion Date" />
                            <asp:Label runat="server" Text='<%# Eval("NextPromotionDate","{0:d}") %>' />
                        </eclipse:TwoColumnPanel>
                    </jquery:JPanel>
                </jquery:Tabs>
            </ItemTemplate>
        </asp:FormView>
    </div>
    </form>
</body>
</html>
