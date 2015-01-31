<%@ Page Title="Package Activity List" Language="C#" MasterPageFile="~/MasterPage.master"
    CodeBehind="PackageActivityList.aspx.cs" Inherits="PhpaAll.MIS.FinancialActivity" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="cphSideNavigation" runat="server">
    <asp:HyperLink ID="HyperLink2" runat="server" NavigateUrl="~/MIS/Default.aspx">MIS Home</asp:HyperLink>
    <br />
    <asp:HyperLink ID="HyperLink4" runat="server" NavigateUrl="~/MIS/SummaryofFinacialProgress/PackageActivityData.aspx">Package activity data entries</asp:HyperLink>
    <br />
    <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="~/MIS/SummaryofFinacialProgress/PackageActivitySummary.aspx">Package activity summary</asp:HyperLink>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cph" runat="server">
    Show Package Activities for
    <phpa:PhpaLinqDataSource ID="dsPackagesReport" runat="server" RenderLogVisible="False"
        ContextTypeName="Eclipse.PhpaLibrary.Database.PackageActivityDataContext" TableName="PackageReports">
    </phpa:PhpaLinqDataSource>
    <i:DropDownListEx runat="server" ID="ddlPackagesReport" DataSourceID="dsPackagesReport"
        DataTextField="Description" DataValueField="PackageReportId" FriendlyName="Package Report"
        OnClientChange="function(e) {
            $(this).closest('form').submit();
            }">
    </i:DropDownListEx>
    <br />
    <i:ButtonEx ID="btnInsert" runat="server" Text="Add New" OnClick="btnInsert_Click"
        Action="Submit" Icon="PlusThick" />
    <phpa:PhpaLinqDataSource ID="ds" runat="server" RenderLogVisible="False" ContextTypeName="Eclipse.PhpaLibrary.Database.PackageActivityDataContext"
        TableName="PackageActivities" EnableInsert="True" OnInserted="ds_Inserted" EnableUpdate="True"
        OnUpdated="ds_Updated" EnableDelete="True" OnDeleted="ds_Deleted" Where="PackageActivityGroup.PackageReportId=@PackageReportId"
        OrderBy="PackageActivityGroupId, ColumnNumber" Visible="True">
        <WhereParameters>
            <asp:ControlParameter ControlID="ddlPackagesReport" Name="PackageReportId" Type="Int32" />
        </WhereParameters>
        <InsertParameters>
            <asp:Parameter Name="Description" Type="String" />
            <asp:Parameter Name="PackageActivityGroupId" Type="Int32" />
            <asp:Parameter Name="ColumnNumber" Type="Int32" />
            <asp:Parameter Name="CalculatedFormula" Type="String" />
        </InsertParameters>
        <UpdateParameters>
            <asp:Parameter Name="Description" Type="String" />
            <asp:Parameter Name="ColumnNumber" Type="Int32" />
            <asp:Parameter Name="CalculatedFormula" Type="String" />
        </UpdateParameters>
    </phpa:PhpaLinqDataSource>
    <jquery:GridViewExInsert ID="gv" runat="server" AutoGenerateColumns="False" DataKeyNames="PackageActivityId"
        DataSourceID="ds" DefaultSortExpression="PackageActivityGroup.Description {0};$"
        ShowExpandCollapseButtons="False" OnRowDeleting="gv_RowDeleting" EnableModelValidation="True"
        InsertRowsCount="0">
        <RowMenuItems>
            <jquery:RowMenuPostBack PostBackType="Edit" Text="Edit" />
            <jquery:RowMenuPostBack PostBackType="Delete" Text="Delete" />
        </RowMenuItems>
        <Columns>
            <eclipse:MultiBoundField DataFields="PackageActivityGroup.Description" HeaderText="Group"
                SortExpression="PackageActivityGroup.Description" />
            <jquery:CommandFieldEx ShowDeleteButton="false" ShowEditButton="false" DeleteConfirmationText="Package activity {0} will be deleted."
                DataFields="Description" />
            <eclipse:MultiBoundField DataFields="Description" HeaderText="Description">
                <EditItemTemplate>
                    <i:TextBoxEx ID="tbDescription" runat="server" Text='<%#Bind("Description") %>' FriendlyName="Description"
                        Size="50">
                        <Validators>
                            <i:Value ValueType="String" />
                            <i:Required />
                        </Validators>
                    </i:TextBoxEx>
                </EditItemTemplate>
                <InsertItemTemplate>
                    <i:TextBoxEx ID="tbDescription" runat="server" Text='<%#Bind("Description") %>' FriendlyName="Description"
                        Size="50">
                        <Validators>
                            <i:Value ValueType="String" />
                            <i:Required />
                        </Validators>
                    </i:TextBoxEx>
                    <i:DropDownListEx runat="server" ID="ddlActivityGroup" DataSourceID="dsActivityGroup"
                        DataTextField="Description" DataValueField="PackageActivityGroupId" FriendlyName="Activity Group"
                        QueryString="PackageActivityGroupId" Value='<%#Bind("PackageActivityGroupId")%>'>
                        <Items>
                            <eclipse:DropDownItem Text="(No Group)" Persistent="Always" />
                        </Items>
                    </i:DropDownListEx>
                    <phpa:PhpaLinqDataSource ID="dsActivityGroup" runat="server" RenderLogVisible="False"
                        ContextTypeName="Eclipse.PhpaLibrary.Database.PackageActivityDataContext" Where="PackageReportId=@PackageReportId"
                        TableName="PackageActivityGroups">
                        <WhereParameters>
                            <asp:ControlParameter ControlID="ddlPackagesReport" Name="PackageReportId" Type="Int32" />
                        </WhereParameters>
                    </phpa:PhpaLinqDataSource>
                </InsertItemTemplate>
            </eclipse:MultiBoundField>
            <eclipse:MultiBoundField DataFields="ColumnNumber" HeaderText="Column Number" ItemStyle-HorizontalAlign="Right">
                <InsertItemTemplate>
                    <i:TextBoxEx ID="tbColumnNumber" runat="server" Text='<%#Bind("ColumnNumber") %>'
                        FriendlyName="Column Number" Size="4">
                        <Validators>
                            <i:Value ValueType="Integer" />
                        </Validators>
                    </i:TextBoxEx>
                </InsertItemTemplate>
                <EditItemTemplate>
                    <i:TextBoxEx ID="tbColumnNumber" runat="server" Text='<%#Bind("ColumnNumber") %>'
                        FriendlyName="Column Number" Size="4">
                        <Validators>
                            <i:Value ValueType="Integer" />
                        </Validators>
                    </i:TextBoxEx>
                </EditItemTemplate>
            </eclipse:MultiBoundField>
            <eclipse:MultiBoundField DataFields="CalculatedFormula" HeaderText="Calculated Formula">
                <InsertItemTemplate>
                    <i:TextBoxEx ID="tbCalculatedFormula" runat="server" Text='<%#Bind("CalculatedFormula") %>'
                        FriendlyName="CalculatedFormula">
                        <Validators>
                            <i:Value ValueType="String" />
                        </Validators>
                    </i:TextBoxEx>
                </InsertItemTemplate>
                <EditItemTemplate>
                    <i:TextBoxEx ID="tbCalculatedFormula" runat="server" Text='<%#Bind("CalculatedFormula") %>'
                        FriendlyName="CalculatedFormula">
                        <Validators>
                            <i:Value ValueType="String" />
                        </Validators>
                    </i:TextBoxEx>
                </EditItemTemplate>
            </eclipse:MultiBoundField>
        </Columns>
    </jquery:GridViewExInsert>
    <i:ValidationSummary runat="server" />
    <jquery:StatusPanel ID="Activity_status" runat="server">
        <Ajax UseDialog="false" />
    </jquery:StatusPanel>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cphNoForm" runat="server">
</asp:Content>
