<%@ Page Language="C#" CodeBehind="ServiceHistory.aspx.cs" EnableViewState="true"
    Inherits="PhpaAll.PIS.ServiceHistory" Title="Service History" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <jquery:JQueryScriptManager runat="server" />
    <script type="text/javascript" src="TerminatedEmployee.js">
    </script>
</head>
<body>
    <form id="formServiceHistory" runat="server">
    <div>
        <phpa:PhpaLinqDataSource runat="server" ID="dsServiceHistory" TableName="ServicePeriods"
            ContextTypeName="Eclipse.PhpaLibrary.Database.PIS.PISDataContext" RenderLogVisible="false"
            Where="EmployeeId == @EmployeeId" OnSelecting="dsServiceHistory_Selecting" OrderBy="PeriodEndDate desc"
            EnableUpdate="true" EnableDelete="true" OnUpdated="dsServiceHistory_Updated"
            OnDeleted="dsServiceHistory_Deleted">
            <WhereParameters>
                <asp:QueryStringParameter Name="EmployeeId" Type="Int32" QueryStringField="EmployeeId" />
            </WhereParameters>
            <UpdateParameters>
                <asp:Parameter Name="ServicePeriodId" Type="Int32" />
                <asp:Parameter Name="PeriodStartDate" Type="DateTime" />
                <asp:Parameter Name="PeriodEndDate" Type="DateTime" />
                <asp:Parameter Name="Designation" Type="String" />
                <asp:Parameter Name="Grade" Type="Int32" />
                <asp:Parameter Name="PostedAt" Type="String" />
                <asp:Parameter Name="GovtOrderNo" Type="String" />
                <asp:Parameter Name="GovtOrderDate" Type="DateTime" />
                <asp:Parameter Name="InitialTerm" Type="String" />
                <asp:Parameter Name="Remarks" Type="String" />
            </UpdateParameters>
            <DeleteParameters>
                <asp:Parameter Name="ServicePeriodId" Type="Int32" />
            </DeleteParameters>
        </phpa:PhpaLinqDataSource>
        <jquery:GridViewExInsert runat="server" ID="gvServiceHistory" DataSourceID="dsServiceHistory"
            AutoGenerateColumns="false" Caption="Service History" DataKeyNames="ServicePeriodId"
            EmptyDataText="No Service History information found" OnRowDataBound="gvServiceHistory_RowDataBound">
            <RowMenuItems>
                <jquery:RowMenuPostBack PostBackType="Edit" Text="Edit" />
                <jquery:RowMenuPostBack PostBackType="Delete" Text="Delete" />
                <jquery:RowMenuItem Text="Details" OnClientClick="function(keys){
    $('#_dlgServiceHistoryDetails') 
    .ajaxDialog('option','data',{ServicePeriodId:keys[0]})  
    .ajaxDialog('load');
            }" />
            </RowMenuItems>
            <Columns>
                <jquery:CommandFieldEx ShowEditButton="false" />
                <eclipse:MultiBoundField DataFields="PeriodStartDate,PeriodEndDate" HeaderText="Period"
                    DataFormatString="{0:d}-{1:d}" ItemStyle-HorizontalAlign="Right">
                    <EditItemTemplate>
                        <i:TextBoxEx runat="server" ID="tbPeriodStartDate" Text='<%# Bind("PeriodStartDate","{0:d}") %>'
                            FriendlyName="PeriodStartDate">
                            <Validators>
                                <i:Required />
                                <i:Date />
                            </Validators>
                        </i:TextBoxEx>
                        <i:TextBoxEx runat="server" ID="tbPeriodEndDate" Text='<%# Bind("PeriodEndDate","{0:d}") %>'
                            FriendlyName="PeriodEndDate">
                            <Validators>
                                <i:Date DateType="ToDate" />
                            </Validators>
                        </i:TextBoxEx>
                    </EditItemTemplate>
                </eclipse:MultiBoundField>
                <eclipse:MultiBoundField DataFields="Designation" HeaderText="Designation">
                    <EditItemTemplate>
                        <phpa:PhpaLinqDataSource runat="server" ID="dsDesignation" OnSelecting="dsDesignation_Selecting"
                            RenderLogVisible="false">
                        </phpa:PhpaLinqDataSource>
                        <i:DropDownSuggest runat="server" ID="ddlDesignation" Value='<%# Bind("Designation") %>'
                            FriendlyName="Designation" DataSourceID="dsDesignation">
                            <Items>
                                <eclipse:DropDownItem Text="(New Designation)" Value="" Persistent="Always" />
                            </Items>
                            <TextBox runat="server" Size="25" MaxLength="50" />
                        </i:DropDownSuggest>
                    </EditItemTemplate>
                </eclipse:MultiBoundField>
                <eclipse:MultiBoundField DataFields="Grade" HeaderText="Grade" ItemStyle-HorizontalAlign="Right">
                    <EditItemTemplate>
                        <phpa:PhpaLinqDataSource runat="server" ID="dsGrade" OnSelecting="dsGrade_Selecting"
                            RenderLogVisible="false">
                        </phpa:PhpaLinqDataSource>
                        <i:DropDownSuggest runat="server" ID="ddlGrade" Value='<%# Bind("Grade") %>'
                            FriendlyName="Grade" DataSourceID="dsGrade">
                            <Items>
                                <eclipse:DropDownItem Text="(New Grade)" Value="" Persistent="Always" />
                            </Items>
                            <TextBox runat="server" MaxLength="4" />
                        </i:DropDownSuggest>
                    </EditItemTemplate>
                </eclipse:MultiBoundField>
                <eclipse:MultiBoundField DataFields="PostedAt" HeaderText="Posted At" ItemStyle-HorizontalAlign="Right">
                    <EditItemTemplate>
                        <phpa:PhpaLinqDataSource runat="server" ID="dsPostedAt" OnSelecting="dsPostedAt_Selecting"
                            RenderLogVisible="false">
                        </phpa:PhpaLinqDataSource>
                        <i:DropDownSuggest ID="ddlPostedAt" runat="server" DataSourceID="dsPostedAt" FriendlyName="Posted At"
                            Value='<%# Bind("PostedAt") %>'>
                            <Items>
                                <eclipse:DropDownItem Value="" Text="(New Posted At)" Persistent="Always" />
                            </Items>
                            <TextBox runat="server" Size="25" MaxLength="50" />
                        </i:DropDownSuggest>
                    </EditItemTemplate>
                </eclipse:MultiBoundField>
                <eclipse:MultiBoundField DataFields="GovtOrderNo" HeaderText="Order|No">
                    <EditItemTemplate>
                        <i:TextBoxEx runat="server" ID="tbGovtOrderNo" Text='<%# Bind("GovtOrderNo") %>'
                            Size="10">
                            <Validators>
                                <i:Value ValueType="String" MaxLength="30" />
                            </Validators>
                        </i:TextBoxEx>
                    </EditItemTemplate>
                </eclipse:MultiBoundField>
                <eclipse:MultiBoundField DataFields="GovtOrderDate" HeaderText="Order|Date" DataFormatString="{0:d}"
                    ItemStyle-HorizontalAlign="Right">
                    <EditItemTemplate>
                        <i:TextBoxEx runat="server" ID="tbGovtOrderDate" Text='<%# Bind("GovtOrderDate","{0:d}") %>'>
                            <Validators>
                                <i:Date />
                            </Validators>
                        </i:TextBoxEx>
                    </EditItemTemplate>
                </eclipse:MultiBoundField>
                <eclipse:MultiBoundField DataFields="InitialTerm" HeaderText="Initial Term" Visible="false">
                    <EditItemTemplate>
                        <i:TextBoxEx runat="server" ID="tbInitialTerm" Text='<%# Bind("InitialTerm") %>'
                            Size="10">
                            <Validators>
                                <i:Value ValueType="String" MaxLength="20" />
                            </Validators>
                        </i:TextBoxEx>
                    </EditItemTemplate>
                </eclipse:MultiBoundField>
                <eclipse:MultiBoundField DataFields="Remarks" HeaderText="Remarks" Visible="false">
                    <EditItemTemplate>
                        <i:TextBoxEx runat="server" ID="tbRemarks" Text='<%# Bind("Remarks") %>' Size="20">
                            <Validators>
                                <i:Value ValueType="String" MaxLength="50" />
                            </Validators>
                        </i:TextBoxEx>
                    </EditItemTemplate>
                </eclipse:MultiBoundField>
            </Columns>
        </jquery:GridViewExInsert>
        <i:ValidationSummary runat="server" />
        <span id="spTerminated" class="ui-helper-hidden">
            <%= this.Request.QueryString["Terminated"] %></span>
        <jquery:StatusPanel runat="server" ID="ServiceHistory_sp">
            <Ajax UseDialog="false" />
        </jquery:StatusPanel>
        <jquery:Dialog ID="_dlgServiceHistoryDetails" runat="server" ClientIDMode="Static"
            AutoOpen="false" Title="Service History Details">
            <Ajax Url="Share/ServiceHistoryDetails.aspx" />
            <Buttons>
                <jquery:CloseButton />
            </Buttons>
        </jquery:Dialog>
        <%--These two breaks are necessary so that the 'Details' can be seen in the HoverMenu
        of the ServiceHistoryDetails remote page --%>
        <br />
        <br />
    </div>
    </form>
</body>
</html>
