<%@ Page Title="MIS Home" Language="C#" MasterPageFile="~/MIS/NestedMIS.master" EnableViewState="false"
    CodeBehind="Default.aspx.cs" Inherits="Finance.MIS.Default2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cphNoForm" runat="server">
    <br />
    <jquery:Tabs runat="server" Collapsible="false">
        <jquery:JPanel runat="server" HeaderText="Recent">
        <p>
        Summarizes all recent data entry activities which have occured. Click on one of the formats to continue data entry.
        </p>
            <phpa:PhpaLinqDataSource runat="server" ID="dsBuzz" ContextTypeName="Eclipse.PhpaLibrary.Database.MIS.MISDataContext"
                OnSelecting="dsBuzz_Selecting" RenderLogVisible="false" />
            <jquery:GridViewEx ID="gvBuzz" runat="server" AutoGenerateColumns="false" DataSourceID="dsBuzz"
                Caption="Recent Data Entry Activities" DefaultSortExpression="PackageDescription;$;"
                PreSorted="true" ShowExpandCollapseButtons="false" OnRowDataBound="gvBuzz_RowDataBound" CellPadding="1">
                <Columns>
                    <eclipse:MultiBoundField HeaderText="Data Entry Date" DataFields="Date" DataFormatString="{0:d}">
                    </eclipse:MultiBoundField>
                    <eclipse:MultiBoundField HeaderText="Package" DataFields="PackageDescription" SortExpression="PackageDescription">
                    </eclipse:MultiBoundField>
                    <asp:TemplateField HeaderText="Physical|Format">
                        <ItemTemplate>
                            <asp:HyperLink runat="server" ID="hlFormat" Text='<%# Eval("PhysicalFormatName") %>'
                                NavigateUrl="{0}.aspx?ProgressFormatId={1}&ProgressDate={2:d}" ToolTip='<%# Eval("FormatDescription") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Physical|# Activities">
                        <ItemStyle HorizontalAlign="Right" />
                        <ItemTemplate>
                            <em>
                                <%# Eval("PhysicalActivityCount", "{0:N0}")%>
                            </em>
                            <eclipse:MultiValueLabel runat="server" DataFields="CountPhysicalProgressDate,MinPhysicalProgressDate,MaxPhysicalProgressDate"
                                DataTwoValueFormatString="for {1:MMM\'y} and {2:MMM\'y}" DataMultiValueFormatString="for {1:MMM\'y} to {2:MMM\'y}"
                                DataSingleValueFormatString="for {1:MMM\'y}" DataZeroValueFormatString="&nbsp;" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Financial|Format">
                        <ItemTemplate>
                            <asp:HyperLink runat="server" ID="hlFinancialFormat" Text='<%# Eval("FinancialFormatName") %>'
                                NavigateUrl="MonthlyFinancial.aspx?ProgressFormatId={0}&ProgressDate={1:d}" ToolTip='<%# Eval("FormatDescription") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Financial|# Activities">
                        <ItemStyle HorizontalAlign="Right" />
                        <ItemTemplate>
                            <em>
                                <%# Eval("FinancialActivityCount", "{0:N0}")%>
                            </em>
                            <eclipse:MultiValueLabel runat="server" DataFields="CountFinancialProgressDate,MinFinancialProgressDate,MaxFinancialProgressDate"
                                DataTwoValueFormatString="for {1:MMM\'y} and {2:MMM\'y}" DataMultiValueFormatString="for {1:MMM\'y} to {2:MMM\'y}"
                                DataSingleValueFormatString="for {1:MMM\'y}" DataZeroValueFormatString="&nbsp;" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <EmptyDataTemplate>
                No data has been entered so far
                </EmptyDataTemplate>
            </jquery:GridViewEx>
        </jquery:JPanel>
        <jquery:JPanel ID="JPanel1" runat="server" HeaderText="All">
            <phpa:PhpaLinqDataSource runat="server" ID="dsFormats" ContextTypeName="Eclipse.PhpaLibrary.Database.MIS.MISDataContext"
                TableName="ProgressFormats" OnSelecting="dsFormats_Selecting" RenderLogVisible="false">
            </phpa:PhpaLinqDataSource>
            <jquery:GridViewEx ID="gv" runat="server" AutoGenerateColumns="false" DataKeyNames="ProgressFormatId"
                DataSourceID="dsFormats" DefaultSortExpression="Dummy;$;" PreSorted="true" ShowExpandCollapseButtons="false"
                Caption="All Report Formats">
                <Columns>
                    <eclipse:MultiBoundField SortExpression="Dummy" DataFields="FormatDisplayName" />
                    <asp:HyperLinkField DataTextField="ProgressFormatName" HeaderText="Format" DataNavigateUrlFields="FormatCode,ProgressFormatId"
                        DataNavigateUrlFormatString="{0}.aspx?ProgressFormatId={1}" />
                    <eclipse:MultiBoundField DataFields="Description" HeaderText="Description" DataFormatString="{0}" />
                    <eclipse:MultiBoundField DataFields="FinancialTarget" HeaderText="Rs / Nu|Target"
                        DataFormatString="{0:N2}" />
                    <eclipse:MultiBoundField DataFields="FinancialActual,CompletionRatio" HeaderText="Rs / Nu|Actual"
                        DataFormatString="{0:N2} ({1:p0})" />
                    <asp:TemplateField HeaderText="Data Entry">
                        <ItemStyle HorizontalAlign="Center" />
                        <ItemTemplate>
                            <a href='<%# string.Format("{0}.aspx?ProgressFormatId={1}&IsEditMode=EM", Eval("FormatCode"), Eval("ProgressFormatId"))%>'
                                title="Click to enter data for this format"><span class="ui-icon ui-icon-pencil">
                                </span></a><span title='<%# Eval("LastDataEntryDate", "Data was last entered on {0:d}") %>'>
                                    <%# Eval("DaysAgo", "{0:N0} days ago")%>
                                </span>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </jquery:GridViewEx>
        </jquery:JPanel>
    </jquery:Tabs>
</asp:Content>
