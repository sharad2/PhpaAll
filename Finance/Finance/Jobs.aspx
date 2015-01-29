<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" CodeBehind="Jobs.aspx.cs"
    Inherits="PhpaAll.Finance.Jobs" Title="Manage Jobs" %>

<%@ Register Src="~/Controls/PrinterFriendlyButton.ascx" TagName="PrinterFriendlyButton"
    TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function DeleteConfirmation(e) {
            return confirm("Deletion will only succeed if this Job is not associated anywhere else.Are you sure you want to delete the Job?");
        }
    </script>
</asp:Content>
<asp:Content ID="c4" ContentPlaceHolderID="cphSideNavigation" runat="server">
    <asp:HyperLink ID="HyperLink2" runat="server" Text="Help" NavigateUrl="~/Doc/Jobs.doc.aspx" />
    <uc2:PrinterFriendlyButton ID="PrinterFriendlyButton1" runat="server" />
</asp:Content>
<asp:Content ID="c3" ContentPlaceHolderID="cph" runat="server">
    <p>
        Manage Jobs will help you to create new job and edit existing jobs.</p>
    <jquery:Accordion runat="server" Collapsible="true" SelectedIndex="-1">
        <jquery:JPanel runat="server" HeaderText="Search">
            <p>
                To search any Job, specifying any of the search criteria in the space provided below:<br />
                (Please specify the text you remember and is contained there in the respective fields.)
            </p>
            <eclipse:TwoColumnPanel runat="server">
                <eclipse:LeftLabel runat="server" Text="Job Code" />
                <i:TextBoxEx ID="lbltbJobCode" runat="server" MaxLength="10">
                </i:TextBoxEx>
                <eclipse:LeftLabel runat="server" Text="Division" />
                <phpa:PhpaLinqDataSource runat="server" ID="dsDivisionSearch" ContextTypeName="Eclipse.PhpaLibrary.Reporting.ReportingDataContext"
                    OrderBy="DivisionName" Select="new (DivisionId, DivisionName, DivisionGroup)"
                    TableName="RoDivisions" Visible="True" RenderLogVisible="false">
                </phpa:PhpaLinqDataSource>
                <i:DropDownListEx runat="server" ID="lbltbDivision" DataSourceID="dsDivisionSearch"
                    FriendlyName="Division Code" DataTextField="DivisionName" DataValueField="DivisionId"
                    DataOptionGroupField="DivisionGroup" Width="25em">
                    <Items>
                        <eclipse:DropDownItem Text="(All)" Value="" Persistent="Always" />
                    </Items>
                </i:DropDownListEx>
                <eclipse:LeftLabel runat="server" Text="Jobs whose completion Date comes in(Days):" />
                <i:TextBoxEx ID="lbltbDate" runat="server" MaxLength="3" ToolTip="Enter no of day's">
                    <Validators>
                        <i:Value ValueType="Integer" />
                    </Validators>
                </i:TextBoxEx>
            </eclipse:TwoColumnPanel>
            <i:ButtonEx ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click"
                Action="Submit" Icon="Search" />
            <i:ButtonEx ID="btnClearSearch" runat="server" Text="Clear Search" Action="Reset"
                Icon="Refresh" />
        </jquery:JPanel>
    </jquery:Accordion>
    <br />
    <i:ButtonEx ID="btnNewJob" runat="server" Text="Create New Job" OnClick="btnNew_Click"
        Action="Submit" CausesValidation="false" Icon="PlusThick" RolesRequired="FinanceManager" />
    <phpa:PhpaLinqDataSource ID="dsJobs" runat="server" ContextTypeName="Eclipse.PhpaLibrary.Database.FinanceDataContext"
        TableName="Jobs" RenderLogVisible="False" OnSelecting="dsJobs_Selecting" OrderBy="JobId">
    </phpa:PhpaLinqDataSource>
    <jquery:GridViewEx ID="gvJobs" runat="server" DataSourceID="dsJobs" DataKeyNames="JobId"
        AutoGenerateColumns="False" Width="100%" AllowPaging="true" AllowSorting="True"
        OnSelectedIndexChanged="gvJobs_SelectedIndexChanged" PageSize="200" Caption="Display Jobs in each Division."
        OnRowDataBound="gvJobs_RowDataBound" ShowFooter="true" EnableViewState="false">
        <EmptyDataTemplate>
            <b>No Job found.</b>
        </EmptyDataTemplate>
        <Columns>
            <asp:TemplateField ItemStyle-CssClass="noprint" HeaderStyle-CssClass="noprint" FooterStyle-CssClass="noprint">
                <ItemTemplate>
                    <asp:LinkButton ID="lnkSelect" runat="server" CommandName="Select" Text="Select">
                    </asp:LinkButton>
                </ItemTemplate>
            </asp:TemplateField>
            <eclipse:SequenceField />
            <eclipse:MultiBoundField DataFields="JobCode" HeaderText="Code" SortExpression="JobCode">
                <ItemStyle HorizontalAlign="Left" />
            </eclipse:MultiBoundField>
            <eclipse:MultiBoundField DataFields="ContractorName" HeaderText="Contractor" SortExpression="ContractorName" />
            <eclipse:MultiBoundField DataFields="Nationality" HeaderText="Nationality" SortExpression="Nationality" />
            <eclipse:MultiBoundField DataFields="DivisionName" HeaderText="Division" SortExpression="DivisionName" />
            <eclipse:MultiBoundField DataFields="Description" HeaderText="Description" SortExpression="Description" />
            <eclipse:MultiBoundField DataFields="CommencementDate" HeaderText="Commenced" SortExpression="CommencementDate"
                DataFormatString="{0:d}" HeaderToolTip="Commencement Date">
                <ItemStyle HorizontalAlign="Right" />
            </eclipse:MultiBoundField>
            <eclipse:MultiBoundField DataFields="CompletionDate" HeaderText="Completion Date"
                SortExpression="CompletionDate" DataFormatString="{0:d}" FooterText="Total">
            </eclipse:MultiBoundField>
            <eclipse:MultiBoundField DataFields="SanctionedAmount" HeaderText="Sanction" HeaderToolTip="Sanctioned amount"
                SortExpression="SanctionedAmount" DataSummaryCalculation="ValueSummation" DataFormatString="{0:N2}">
                <ItemStyle HorizontalAlign="Right" />
                <HeaderStyle HorizontalAlign="Center" />
                <FooterStyle HorizontalAlign="Right" />
            </eclipse:MultiBoundField>
            <eclipse:MultiBoundField DataFields="RevisedContract" HeaderText="Revised Contract"
                HeaderToolTip="Revised Contract" SortExpression="RevisedContract" DataSummaryCalculation="ValueSummation"
                DataFormatString="{0:N2}">
                <ItemStyle HorizontalAlign="Right" />
                <HeaderStyle HorizontalAlign="Center" />
                <FooterStyle HorizontalAlign="Right" />
            </eclipse:MultiBoundField>
            <eclipse:MultiBoundField DataFields="Total" HeaderText="Till Date Expenditure" SortExpression="Total"
                DataSummaryCalculation="ValueSummation" DataFormatString="{0:N2}">
                <ItemStyle HorizontalAlign="Right" />
                <HeaderStyle HorizontalAlign="Center" />
                <FooterStyle HorizontalAlign="Right" />
            </eclipse:MultiBoundField>
        </Columns>
    </jquery:GridViewEx>
    <jquery:Dialog ID="dlgJobEditor" runat="server" Title="Job Editor" Position="RightTop"
        Width="450" EnableViewState="true" EnablePostBack="true" Visible="false">
        <ContentTemplate>
            <phpa:PhpaLinqDataSource ID="dsEditJobs" runat="server" ContextTypeName="Eclipse.PhpaLibrary.Database.FinanceDataContext"
                EnableDelete="True" EnableInsert="True" EnableUpdate="True" TableName="Jobs"
                AutoGenerateWhereClause="true" RenderLogVisible="false" OnSelecting="dsEditJobs_Selecting"
                OnInserted="dsEditJobs_Inserted" OnUpdated="dsEditJobs_Updated">
                <WhereParameters>
                    <asp:Parameter Name="JobId" Type="Int32" />
                </WhereParameters>
                <UpdateParameters>
                    <asp:Parameter Name="HeadOfAccountId" Type="Int32" ConvertEmptyStringToNull="true" />
                    <asp:Parameter Name="JobTypeCode" Type="Char" />
                    <asp:Parameter Name="SanctionNumber" Type="String" />
                    <asp:Parameter Name="SanctionDate" Type="DateTime" />
                    <asp:Parameter Name="SanctionedAmount" Type="Decimal" />
                    <asp:Parameter Name="WorkOrderNumber" Type="String" />
                    <asp:Parameter Name="WorkOrderDate" Type="DateTime" />
                    <asp:Parameter Name="CommencementDate" Type="DateTime" />
                    <asp:Parameter Name="CompletionDate" Type="DateTime" />
                    <asp:Parameter Name="DivisionId" Type="Int32" />
                    <asp:Parameter Name="Description" Type="String" />
                    <asp:Parameter Name="AwardDate" Type="DateTime" />
                    <asp:Parameter Name="ContractAmount" Type="Decimal" />
                    <asp:Parameter Name="ContractorId" Type="Int32" />
                    <asp:Parameter Name="RevisedSanction" Type="Decimal" />
                    <asp:Parameter Name="RevisedContract" Type="Decimal" />
                    <asp:Parameter Name="JobCode" Type="String" />
                    <asp:Parameter Name="PackageName" Type="String" />
                </UpdateParameters>
                <InsertParameters>
                    <asp:Parameter Name="JobId" Type="Int32" />
                    <asp:Parameter Name="JobTypeCode" Type="Char" />
                    <asp:Parameter Name="SanctionNumber" Type="String" />
                    <asp:Parameter Name="SanctionDate" Type="DateTime" />
                    <asp:Parameter Name="SanctionedAmount" Type="Decimal" />
                    <asp:Parameter Name="WorkOrderNumber" Type="String" />
                    <asp:Parameter Name="WorkOrderDate" Type="DateTime" />
                    <asp:Parameter Name="CommencementDate" Type="DateTime" />
                    <asp:Parameter Name="CompletionDate" Type="DateTime" />
                    <asp:Parameter Name="HeadOfAccountId" Type="Int32" />
                    <asp:Parameter Name="DivisionId" Type="Int32" />
                    <asp:Parameter Name="Description" Type="String" />
                    <asp:Parameter Name="AwardDate" Type="DateTime" />
                    <asp:Parameter Name="ContractAmount" Type="Decimal" />
                    <asp:Parameter Name="ContractorId" Type="Int32" />
                    <asp:Parameter Name="RevisedSanction" Type="Decimal" />
                    <asp:Parameter Name="RevisedContract" Type="Decimal" />
                    <asp:Parameter Name="JobCode" Type="String" />
                    <asp:Parameter Name="PackageName" Type="String" />
                </InsertParameters>
            </phpa:PhpaLinqDataSource>
            <asp:FormView ID="fvEdit" runat="server" DataKeyNames="JobId" DataSourceID="dsEditJobs"
                OnItemInserted="fvEdit_ItemInserted" OnItemUpdated="fvEdit_ItemUpdated" OnItemDeleted="fvEdit_ItemDeleted">
                <HeaderTemplate>
                    <phpa:FormViewContextHeader ID="FormViewContextHeader1" runat="server" EntityName="Job"
                        CurrentEntity='<%# Eval("Description") %>'></phpa:FormViewContextHeader>
                </HeaderTemplate>
                <EmptyDataTemplate>
                    <asp:Label ID="lblEmptyMsg" runat="server" Text="Select the Job to view details." />
                    <br />
                    <asp:LoginView ID="restricUser1" runat="server">
                        <RoleGroups>
                            <asp:RoleGroup Roles="FinanceManager">
                                <ContentTemplate>
                                    <asp:Label ID="lblEmptyMsg" runat="server" Text="Click on the link below to Create New Job" />
                                    <br />
                                    <i:LinkButtonEx ID="btnInsertNewJob" runat="server" Text="Insert New Job" OnClick="btnNew_Click"
                                        Action="Submit" CausesValidation="false" />
                                </ContentTemplate>
                            </asp:RoleGroup>
                        </RoleGroups>
                        <LoggedInTemplate>
                            <br />
                            Only Finance Managers are allowed to create divisions
                        </LoggedInTemplate>
                    </asp:LoginView>
                </EmptyDataTemplate>
                <EditItemTemplate>
                    <eclipse:TwoColumnPanel runat="server" WidthLeft="25%" WidthRight="75%">
                        <eclipse:LeftLabel runat="server" Text="Job Type" />
                        <asp:ObjectDataSource ID="odsJobTypes" runat="server" SelectMethod="GetJobTypes"
                            TypeName="Eclipse.PhpaLibrary.Database.Job"></asp:ObjectDataSource>
                        <i:DropDownListEx runat="server" ID="DropDownListEx1" DataSourceID="odsJobTypes"
                            FriendlyName="Job Type" DataTextField="DisplayJobType" DataValueField="JobTypeCode"
                            Value='<%# Bind("JobTypeCode") %>'>
                        </i:DropDownListEx>
                        <br />
                        <eclipse:LeftLabel runat="server" Text="Package" />
                        <i:AutoComplete ID="tbPackage" runat="server" FriendlyName="Pakage" Value='<%# Bind("PackageName") %>'
                            Text='<%# Bind("PackageName") %>' WebMethod="GetPackageNames" ValidateWebMethodName="ValidatePackage"
                            WebServicePath="~/Services/Packages.asmx" AutoValidate="true" Width="20em">
                        </i:AutoComplete>
                        <br />
                        Select Package in order to view the entry in Central Excise Duty Report.
                        <eclipse:LeftLabel runat="server" Text="Account Head" />
                        <i:AutoComplete ID="tbHeadofAccount" runat="server" FriendlyName="Head of Account"
                            Width="20em" Value='<%# Bind("HeadOfAccountId") %>' Text='<%# Eval("HeadOfAccount.DisplayDescription") %>'
                            WebMethod="GetHeadOfAccount" ValidateWebMethodName="ValidateHeadOfAccount" WebServicePath="~/Services/HeadOfAccounts.asmx">
                        </i:AutoComplete>
                        <eclipse:LeftLabel ID="LeftLabel1" runat="server" Text="Division Code" />
                        <phpa:PhpaLinqDataSource runat="server" ID="dsDivisions" ContextTypeName="Eclipse.PhpaLibrary.Reporting.ReportingDataContext"
                            OrderBy="DivisionName" Select="new (DivisionId, DivisionName, DivisionGroup)"
                            TableName="RoDivisions" Visible="True" RenderLogVisible="false">
                        </phpa:PhpaLinqDataSource>
                        <i:DropDownListEx runat="server" ID="tbDivisionCode" DataSourceID="dsDivisions" FriendlyName="Division Code"
                            DataTextField="DivisionName" DataValueField="DivisionId" DataOptionGroupField="DivisionGroup"
                            Value='<%# Bind("DivisionId") %>' Width="25em">
                            <Items>
                                <eclipse:DropDownItem Text="(Please Select)" Value="" Persistent="Always" />
                            </Items>
                            <Validators>
                                <i:Required />
                            </Validators>
                        </i:DropDownListEx>
                        <eclipse:LeftLabel runat="server" Text="Sanction Number" />
                        <i:TextBoxEx ID="tbSanctionNumber" runat="server" Value='<%# Bind("SanctionNumber") %>'
                            MaxLength="50" Size="25" FriendlyName="Sanction Number">
                        </i:TextBoxEx>
                        <eclipse:LeftLabel runat="server" Text="Sanction Date" />
                        <i:TextBoxEx ID="dtb" runat="server" Value='<%# Bind("SanctionDate", "{0:d}") %>'
                            FriendlyName="Sanction Date">
                            <Validators>
                                <i:Date />
                            </Validators>
                        </i:TextBoxEx>
                        <eclipse:LeftLabel runat="server" Text="Sanctioned Amount" />
                        <i:TextBoxEx ID="tbSanctionAmount" runat="server" Value='<%# Bind("SanctionedAmount", "{0:C2}") %>'
                            FriendlyName="Sanctioned Amount">
                            <Validators>
                                <i:Required />
                            </Validators>
                        </i:TextBoxEx>
                        <eclipse:LeftLabel runat="server" Text="Revised Sanction Amount" />
                        <i:TextBoxEx ID="tbRevisedSantion" runat="server" Value='<%# Bind("RevisedSanction", "{0:C2}") %>'
                            FriendlyName="Revised Sanction Amount">
                        </i:TextBoxEx>
                        <eclipse:LeftLabel runat="server" Text="Work Order Number" />
                        <i:TextBoxEx ID="tbWorkOrderNumber" runat="server" Value='<%# Bind("WorkOrderNumber") %>'
                            MaxLength="50" Size="20" FriendlyName="Work Order Number">
                        </i:TextBoxEx>
                        <eclipse:LeftLabel runat="server" Text="Work Order Date" />
                        <i:TextBoxEx ID="tbWorkOrderDate" runat="server" FriendlyName="Work Order Date" Value='<%# Bind("WorkOrderDate", "{0:d}") %>'>
                            <Validators>
                                <i:Date />
                            </Validators>
                        </i:TextBoxEx>
                        <eclipse:LeftLabel runat="server" Text="Commenced Date" />
                        <i:TextBoxEx ID="tbCommencementDate" runat="server" FriendlyName="Commencement Date"
                            Value='<%# Bind("CommencementDate", "{0:d}") %>'>
                            <Validators>
                                <i:Date />
                            </Validators>
                        </i:TextBoxEx>
                        <eclipse:LeftLabel runat="server" Text="Completion Date" />
                        <i:TextBoxEx ID="tbCompletionDate" runat="server" FriendlyName="Completion Date"
                            Value='<%# Bind("CompletionDate", "{0:d}") %>'>
                            <Validators>
                                <i:Date />
                            </Validators>
                        </i:TextBoxEx>
                        <eclipse:LeftLabel runat="server" Text="Description" />
                        <i:TextArea ID="tbDescription" runat="server" Value='<%# Bind("Description") %>'
                            Cols="40" Rows="3">
                            <Validators>
                                <i:Value MaxLength="150" />
                            </Validators>
                        </i:TextArea>
                        <br />
                        Job will be identified with this name. Its should be explanatory.
                        <eclipse:LeftLabel runat="server" Text="Contractor" />
                        <i:AutoComplete ID="txtContractorCode" runat="server" Text='<%# Eval("Contractor.ContractorCode") %>'
                            Value='<%# Bind("ContractorId") %>' WebMethod="GetContractors" ValidateWebMethodName="ValidateContractor"
                            WebServicePath="~/Services/Contractors.asmx" Width="20em">
                        </i:AutoComplete>
                        <eclipse:LeftLabel runat="server" Text="Award Date" />
                        <i:TextBoxEx ID="tbAwardDate" runat="server" FriendlyName="Award Date" Value='<%# Bind("AwardDate" , "{0:d}") %>'>
                            <Validators>
                                <i:Date />
                            </Validators>
                        </i:TextBoxEx>
                        <eclipse:LeftLabel runat="server" Text="Contract Amount" />
                        <i:TextBoxEx ID="tbContractAmount" runat="server" Value='<%# Bind("ContractAmount", "{0:C2}") %>'
                            FriendlyName="Contract Amount">
                        </i:TextBoxEx>
                        <eclipse:LeftLabel runat="server" Text="Revised Contract Amount" />
                        <i:TextBoxEx ID="tbRevisedContract" runat="server" Value='<%# Bind("RevisedContract", "{0:C2}") %>'
                            FriendlyName="Revised Contract Amount">
                        </i:TextBoxEx>
                    </eclipse:TwoColumnPanel>
                    <br />
                    <i:ButtonEx runat="server" ID="btnSave" Text="Save" CausesValidation="true" Action="Submit"
                        Icon="Disk" OnClick="btnSave_Click" />
                    <i:LinkButtonEx ID="btnCancel" runat="server" Text="Cancel" CausesValidation="false"
                        OnClick="btnCancel_Click" />
                    <i:ValidationSummary ID="valSummary" runat="server" />
                </EditItemTemplate>
                <ItemTemplate>
                    <jquery:Tabs ID="TabContainer1" runat="server">
                        <jquery:JPanel ID="panelBasic" runat="server" HeaderText="Details">
                            <eclipse:TwoColumnPanel ID="TwoColumnPanel1" runat="server">
                                <eclipse:LeftLabel runat="server" Text="Job Code" />
                                <asp:Label runat="server" Text='<%# Eval("JobCode") %>' />
                                <eclipse:LeftLabel runat="server" Text="Job Type" />
                                <asp:Label runat="server" Text='<%# Eval("DisplayJobType") %>' />
                                <eclipse:LeftLabel runat="server" Text="Package" />
                                <asp:Label runat="server" Text='<%# Eval("PackageName") %>' />
                                <eclipse:LeftLabel runat="server" Text="Description" />
                                <asp:Label runat="server" Text='<%# Eval("Description") %>' />
                                <eclipse:LeftLabel runat="server" Text="Head Of Account" />
                                <asp:Label runat="server" Text='<%# Eval("HeadOfAccount.DisplayName") %>' />
                                :
                                <asp:Label runat="server" Text='<%# Eval("HeadOfAccount.Description") %>' />
                                <eclipse:LeftLabel runat="server" Text="Division" />
                                <asp:Label runat="server" Text='<%# Eval("Division.DivisionName") %>' ToolTip='<%# Eval("Division.DivisionGroup", "Division Group: {0}") %>' />
                                <eclipse:LeftLabel runat="server" Text="Sanction Number" />
                                <asp:Label runat="server" Text='<%# Eval("SanctionNumber") %>' />
                                <eclipse:LeftLabel runat="server" Text="Sanction Date" />
                                <asp:Label ID="Label1" runat="server" Text='<%# Eval("SanctionDate", "{0:d}") %>' />
                                <eclipse:LeftLabel runat="server" Text="Sanction Amount" />
                                <asp:Label runat="server" Text='<%# Eval("SanctionedAmount","{0:N2}") %>' />
                                <eclipse:LeftLabel runat="server" Text="Revised Sanction" />
                                <asp:Label runat="server" Text='<%# Eval("RevisedSanction","{0:N2}") %>' />
                                <eclipse:LeftLabel runat="server" Text="Work Order Number" />
                                <asp:Label runat="server" Text='<%# Eval("WorkOrderNumber","{0:d}") %>' />
                                <eclipse:LeftLabel runat="server" Text="Work Order Date" />
                                <asp:Label runat="server" Text='<%# Eval("WorkOrderDate","{0:d}") %>' />
                                <eclipse:LeftLabel runat="server" Text="Completion Date" />
                                <asp:Label runat="server" Text='<%# Eval("CompletionDate","{0:d}") %>' />
                                <eclipse:LeftLabel runat="server" Text="Contractor" />
                                <asp:Label runat="server" Text='<%# Eval("Contractor.ContractorName") %>' />
                                <eclipse:LeftLabel runat="server" Text="Contract Amount" />
                                <asp:Label runat="server" Text='<%# Eval("ContractAmount","{0:N2}") %>' />
                                <eclipse:LeftLabel runat="server" Text="Revised Contract" />
                                <asp:Label runat="server" Text='<%# Eval("RevisedContract","{0:N2}") %>' />
                                <eclipse:LeftLabel runat="server" Text="Commencement Date" />
                                <asp:Label runat="server" Text='<%# Eval("CommencementDate","{0:d}") %>' />
                                <eclipse:LeftLabel runat="server" Text="Award Date" />
                                <asp:Label runat="server" Text='<%# Eval("AwardDate","{0:d}") %>' />
                            </eclipse:TwoColumnPanel>
                        </jquery:JPanel>
                        <phpa:AuditTabPanel ID="tabpanelAudit" runat="server" />
                    </jquery:Tabs>
                    <br />
                    <asp:LoginView ID="userRestriction" runat="server">
                        <RoleGroups>
                            <asp:RoleGroup Roles="FinanceManager">
                                <ContentTemplate>
                                    <i:LinkButtonEx ID="btnEdit" runat="server" Text="Edit" CausesValidation="false"
                                        OnClick="btnEdit_Click" />
                                    <i:LinkButtonEx ID="LinkButtonEx1" runat="server" Text="Delete" OnClick="btnDelete_Click"
                                        Action="Submit" CausesValidation="false" OnClientClick="DeleteConfirmation" />
                                    <i:LinkButtonEx ID="btnNew" runat="server" Text="New" OnClick="btnNew_Click" Action="Submit"
                                        CausesValidation="false" />
                                </ContentTemplate>
                            </asp:RoleGroup>
                        </RoleGroups>
                        <LoggedInTemplate>
                            Only Finance Manager can edit Jobs.
                        </LoggedInTemplate>
                    </asp:LoginView>
                </ItemTemplate>
                <FooterTemplate>
                    <phpa:FormViewStatusMessage ID="fvEditStatusMessage" runat="server" />
                </FooterTemplate>
            </asp:FormView>
        </ContentTemplate>
    </jquery:Dialog>
</asp:Content>
