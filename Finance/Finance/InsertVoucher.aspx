<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" CodeBehind="InsertVoucher.aspx.cs"
    Inherits="PhpaAll.Finance.InsertVoucher" Title="Create Voucher" EnableViewState="true" %>

<%@ Register Src="../Controls/VoucherDetailControl.ascx" TagName="VoucherDetailControl"
    TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="InsertVoucher.js" type="text/javascript"></script>

     <script type="text/javascript">
        function tbBills_Search(event, ui) {
            var divisionId = $('#tbDivisionCode').autocompleteEx('selectedValue');
            $(this).autocompleteEx('option', 'parameters', { divisionId: divisionId, term: $(this).val() });
            return true;
        }
    </script>


</asp:Content>
<asp:Content ID="c4" ContentPlaceHolderID="cphSideNavigation" runat="server">
    <asp:HyperLink ID="HyperLink1" runat="server" Text="Help" NavigateUrl="~/Doc/InsertVoucher.doc.aspx" />
    <br />
    <asp:HyperLink runat="server" NavigateUrl="~/Finance/InsertVoucher.aspx">Create New Voucher</asp:HyperLink>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cph" runat="server">

    <phpa:PhpaLinqDataSource ID="dsFiscalYear" runat="server" ContextTypeName="Eclipse.PhpaLibrary.Database.FiscalDataContext"
        TableName="FinancialYears" RenderLogVisible="False" OrderBy="Name desc" Where='Freeze == "N"' OnSelected="dsFiscalYear_Selected">
    </phpa:PhpaLinqDataSource>
    <br/>
    <asp:ListView runat="server" ID="lvEditableDates" DataSourceID="dsFiscalYear" ItemType="Eclipse.PhpaLibrary.Database.FinancialYear">
        <LayoutTemplate>
            <fieldset>
                <legend>
                    Vouchers can be created or edited for
                </legend>
                <ul>
                    <li runat="server" id="itemPlaceholder"></li>
                </ul>
                <asp:HyperLink runat="server" Text="Manage" NavigateUrl="~/Finance/ManageFinancialYears.aspx"></asp:HyperLink>
            </fieldset>
        </LayoutTemplate>
        <ItemTemplate>
            <li>Year <%# Item.Name %>: Voucher Dates from <%# Item.StartDate.ToShortDateString() %> to <%# Item.EndDate.ToShortDateString() %>
            </li>
        </ItemTemplate>
        <EmptyDataTemplate>
            <asp:Label runat="server" Text="All financial years have been closed. Vouchers cannot be created or edited." ForeColor="Red"></asp:Label>    
            <asp:HyperLink runat="server" Text="Manage" NavigateUrl="~/Finance/ManageFinancialYears.aspx"></asp:HyperLink>
        </EmptyDataTemplate>
    </asp:ListView>
        <br />
    <phpa:PhpaLinqDataSource ID="dsEditVouchers" runat="server" ContextTypeName="Eclipse.PhpaLibrary.Database.FinanceDataContext"
        TableName="Vouchers" AutoGenerateWhereClause="false" Where="VoucherId == @VoucherId"
        EnableInsert="true" EnableUpdate="true" OnContextCreating="ds_ContextCreating"
        OnContextCreated="dsEditVoucher_ContextCreated" RenderLogVisible="false" OnSelecting="dsEditVouchers_Selecting"
        OnInserted="dsEditVouchers_Inserted" OnUpdated="dsEditVouchers_Updated" OnDeleting="dsEditVouchers_Deleting"
        EnableDelete="true">
        <WhereParameters>
            <asp:QueryStringParameter QueryStringField="VoucherId" Name="VoucherId" Type="Int32" />
        </WhereParameters>
        <UpdateParameters>
            <asp:Parameter Name="VoucherDate" Type="DateTime" />
            <asp:Parameter Name="CheckNumber" Type="Int32" />
            <asp:Parameter Name="DivisionId" Type="Int32" />
            <asp:Parameter Name="VoucherCode" Type="String" />
            <asp:Parameter Name="Particulars" Type="String" />
            <asp:Parameter Name="PayeeName" Type="String" />
            <asp:Parameter Name="StationId" Type="String" ConvertEmptyStringToNull="true" />
        </UpdateParameters>
        <InsertParameters>
            <asp:Parameter Name="VoucherDate" Type="DateTime" />
            <asp:Parameter Name="CheckNumber" Type="Int32" />
            <asp:Parameter Name="DivisionId" Type="Int32" />
            <asp:Parameter Name="VoucherCode" Type="String" />
            <asp:Parameter Name="Particulars" Type="String" />
            <asp:Parameter Name="PayeeName" Type="String" />
            <asp:Parameter Name="StationId" Type="String" ConvertEmptyStringToNull="true" />
        </InsertParameters>
    </phpa:PhpaLinqDataSource>
    <phpa:PhpaLinqDataSource ID="dsEditVoucherDetail" runat="server" ContextTypeName="Eclipse.PhpaLibrary.Database.FinanceDataContext"
        TableName="VoucherDetails" AutoGenerateWhereClause="false" Where="VoucherId == @VoucherId"
        EnableInsert="true" EnableUpdate="true" OnContextCreating="ds_ContextCreating"
        EnableDelete="true" RenderLogVisible="false" OnSelecting="dsEditVouchers_Selecting"
        OnContextCreated="dsEditVoucherDetail_ContextCreated" OnInserting="dsEditVoucherDetail_Inserting"
        OnUpdating="dsEditVoucherDetail_Updating">
        <WhereParameters>
            <asp:QueryStringParameter QueryStringField="VoucherId" Name="VoucherId" Type="Int32" />
        </WhereParameters>
        <UpdateParameters>
            <asp:Parameter Name="VoucherId" Type="Int32" />
            <asp:Parameter Name="DebitAmount" DbType="Decimal" />
            <asp:Parameter Name="CreditAmount" DbType="Decimal" />
            <asp:Parameter Name="HeadOfAccountId" DbType="Int32" />
            <asp:Parameter Name="EmployeeId" DbType="Int32" />
            <asp:Parameter Name="JobId" DbType="Int32" />
        </UpdateParameters>
        <InsertParameters>
            <asp:Parameter Name="DebitAmount" DbType="Decimal" />
            <asp:Parameter Name="CreditAmount" DbType="Decimal" />
            <asp:Parameter Name="HeadOfAccountId" DbType="Int32" />
            <asp:Parameter Name="EmployeeId" DbType="Int32" />
            <asp:Parameter Name="JobId" DbType="Int32" />
            <asp:Parameter Name="VoucherId" Type="Int32" />
        </InsertParameters>
    </phpa:PhpaLinqDataSource>
    <asp:FormView ID="fvEdit" runat="server" DataKeyNames="VoucherId" DataSourceID="dsEditVouchers">
        <HeaderTemplate>
            Voucher:
           
            <%# Eval("Particulars") ?? "New"%>
        </HeaderTemplate>
        <EditItemTemplate>
            <div id="fvEditTemplate">
                <eclipse:TwoColumnPanel runat="server">
                    <eclipse:LeftLabel runat="server" Text="Voucher Type" />
                    <i:RadioButtonListEx ID="rblVoucherTypes" runat="server" Value='<%# Bind("VoucherTypeCode") %>'
                        OnClientChange="rblVoucherTypes_Change" ClientIDMode="Static">
                        <Items>
                            <i:RadioItem Text="Bank" Value="B" />
                            <i:RadioItem Text="Cash" Value="C" />
                            <i:RadioItem Text="Journal" Value="J" />
                        </Items>
                    </i:RadioButtonListEx>
                    <eclipse:LeftLabel ID="LeftLabel1" runat="server" Text="Voucher Station" />
                    <phpa:PhpaLinqDataSource runat="server" ID="dsStations" ContextTypeName="Eclipse.PhpaLibrary.Database.PIS.PISDataContext"
                        OnSelecting="dsStations_Selecting" RenderLogVisible="false">
                    </phpa:PhpaLinqDataSource>
                    <i:DropDownListEx ID="ddlStation" DataSourceID="dsStations" DataTextField="StationName" DataValueField="StationId" runat="server" FriendlyName="Station"
                        Value='<%#Bind("StationId")%>' UseCookie="ReadWrite" QueryString="StationId" CookieExpiryDays="21">
                        <Items>
                            <eclipse:DropDownItem Text="(Not Set)" Persistent="Always" />
                        </Items>
                        <Validators>
                            <i:Required />
                        </Validators>
                    </i:DropDownListEx>
                    List displays only those stations for which you are authorized
                   
                    <eclipse:LeftLabel runat="server" Text="Voucher Date" />
                    <i:TextBoxEx ID="tbVoucherDate" runat="server" Text='<%# Bind("VoucherDate", "{0:d}") %>'
                        FriendlyName="Voucher Date" QueryString="VoucherDate"
                        OnPreRender="tbVoucherDate_PreRender">
                        <Validators>
                            <i:Required OnServerValidate="tbVD_ServerValidate" ClientMessage="Not Valid date" />
                            <i:Date />
                        </Validators>
                    </i:TextBoxEx>
                    <eclipse:LeftLabel runat="server" Text="Voucher No" />
                    <i:TextBoxEx ID="tbVoucherCode" runat="server" Text='<%# Bind("VoucherCode") %>'
                        CaseConversion="UpperCase" FocusPriority="High">
                        <Validators>
                            <i:Required />
                        </Validators>
                    </i:TextBoxEx>
                    <br />
                    First time in the session, you must enter a code here. then it will keep incrementing.
                   
                    <eclipse:LeftLabel runat="server" Text="Division" />
                    <%--<i:AutoComplete ID="tbDivisionCode" runat="server" FriendlyName="Division" WebMethod="GetDivisions"
                        WebServicePath="~/Services/Divisions.asmx" Value='<%# Bind("DivisionId") %>'
                        Text='<%# Eval("Division.DivisionCode", "{0}: ") +  Eval("Division.DivisionName", "{0}") %>'
                        ValidateWebMethodName="ValidateDivision" AutoValidate="true" Delay="1000" Width="25em">
                        <Validators>
                            <i:Required DependsOn="ddlMoreColumns" DependsOnState="Value" DependsOnValue="J" />
                        </Validators>
                    </i:AutoComplete>
                    <i:AutoComplete ID="AutoComplete1" runat="server" FriendlyName="Division" WebMethod="SearchAutoComplete2"
                        WebServicePath="~/BillsHome" Value='<%# Bind("DivisionId") %>'
                        Text='<%# Eval("Division.DivisionCode", "{0}: ") +  Eval("Division.DivisionName", "{0}") %>'
                        ValidateWebMethodName="ValidateDivision" AutoValidate="false" Delay="1000" Width="25em">
                        <Validators>
                            <i:Required DependsOn="ddlMoreColumns" DependsOnState="Value" DependsOnValue="J" />
                        </Validators>
                    </i:AutoComplete>--%>
                    <i:AutoComplete ID="tbDivisionCode" runat="server" ClientIDMode="Static" Width="25em"
            WebMethod="GetDivisions" WebServicePath="~/Services/Divisions.asmx">
        </i:AutoComplete>
       
        <i:AutoComplete ID="tbBills" runat="server" ClientIDMode="Static" Width="25em" WebMethod="GetBillsForDivision"
            WebServicePath="~/Services/Divisions.asmx" OnClientSearch="tbBills_Search" AutoValidate="false" Delay="1000">
        </i:AutoComplete>
                    <eclipse:LeftLabel ID="lblCheckNumber" runat="server" Text="Cheque #" />
                    <i:TextBoxEx ID="tbCheckNumber" runat="server" QueryStringValue='<%# Bind("CheckNumber") %>'
                        MaxLength="9" ClientIDMode="Static" QueryString="CheckNumber">
                        <Validators>
                            <i:Filter DependsOn="rblVoucherTypes" DependsOnState="Value" DependsOnValue="B" OnServerDependencyCheck="tbCheckNumber_ServerDependencyCheck" />
                            <i:Value ValueType="Integer" Min="1" />
                        </Validators>
                    </i:TextBoxEx>
                    <br />
                    First time in the session, you must enter a cheque number here. then it will keep
                    incrementing.
                   
                    <eclipse:LeftLabel ID="lblPayee" runat="server" Text="Payee" />
                    <i:AutoComplete ID="tbPayee" runat="server" FriendlyName="Payee" WebMethod="GetVoucherPayeeList"
                        WebServicePath="~/Services/Divisions.asmx" Text='<%# Bind("PayeeName") %>' ShowUiHint="false"
                        Width="25em" AutoValidate="false">
                        <Validators>
                            <i:Value MaxLength="50" />
                            <i:Custom Rule="ValidatePayee" ClientMessage="Payee is required." OnServerValidate="tb_ServerValidate" />
                        </Validators>
                    </i:AutoComplete>
                    <span>*</span>
                    <eclipse:LeftLabel runat="server" Text="Particulars" />
                    <i:TextArea ID="tbParticulars" runat="server" Text='<%# Bind("Particulars") %>' Rows="5"
                        Cols="50">
                        <Validators>
                            <i:Required />
                            <i:Value MaxLength="250" />
                        </Validators>
                    </i:TextArea>
                </eclipse:TwoColumnPanel>
            </div>
            <i:ButtonEx ID="btnSave2" runat="server" Text="Save" Action="Submit" CausesValidation="true"
                Icon="Disk" Enabled="true" OnClick="btnSave_Click" OnClientClick="btnSave_Click"
                OnPreRender="btnSave_PreRender" />
            <i:DropDownListEx ID="ddlMoreColumns" runat="server" OnClientChange="ddlMoreColumns_Change"
                ClientIDMode="Static">
                <Items>
                    <eclipse:DropDownItem Text="(More Columns)" Value="" Persistent="Always" />
                    <eclipse:DropDownItem Text="Show Employee" Value="E" Persistent="Always" />
                    <eclipse:DropDownItem Text="Show Job" Value="J" Persistent="Always" />
                </Items>
            </i:DropDownListEx>
            <i:LinkButtonEx ID="btnCancel1" runat="server" Text="Cancel" CausesValidation="false"
                OnClick="btnCancel_Click" />
            <i:ValidationSummary ID="valSummary" runat="server" />
            <jquery:GridViewExInsert ID="gvEditVoucherDetails" runat="server" AutoGenerateColumns="False"
                DataSourceID="dsEditVoucherDetail" DataKeyNames="VoucherDetailId" ShowFooter="true"
                ClientIDMode="Static" InsertRowsCount="6" InsertRowsAtBottom="true" OnDataBound="gvEditVoucherDetails_DataBound"
                OnRowDataBound="gvEditVoucherDetails_RowDataBound">
                <Columns>
                    <eclipse:SequenceField />
                    <asp:TemplateField HeaderText="Status" AccessibleHeaderText="Status">
                        <ItemTemplate>
                            <i:DropDownListEx ID="ddlStatus" runat="server" TabIndex="-1">
                                <Items>
                                    <eclipse:DropDownItem Text="Unchanged" Value="U" Persistent="Always" />
                                    <eclipse:DropDownItem Text="Modified" Value="S" Persistent="Always" />
                                    <eclipse:DropDownItem Text="Delete" Value="D" Persistent="Always" />
                                </Items>
                            </i:DropDownListEx>
                        </ItemTemplate>
                        <InsertItemTemplate>
                            <i:DropDownListEx ID="ddlStatus" runat="server" TabIndex="-1">
                                <Items>
                                    <eclipse:DropDownItem Text="New" Value="N" />
                                    <eclipse:DropDownItem Text="Insert" Value="S" />
                                </Items>
                            </i:DropDownListEx>
                        </InsertItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Employee" AccessibleHeaderText="Employee">
                        <ItemTemplate>
                            <i:AutoComplete ID="tbEmployee" runat="server" FriendlyName="Employee" Value='<%# Bind("EmployeeId") %>'
                                Text='<%# Eval("Employee.FullName") %>' WebMethod="GetEmployees" WebServicePath="~/Services/Employees.asmx"
                                ValidateWebMethodName="ValidateEmployee" AutoValidate="true">
                            </i:AutoComplete>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Job" AccessibleHeaderText="Job">
                        <ItemTemplate>
                            <i:AutoComplete ID="tbJob" runat="server" FriendlyName="Job" Value='<%# Bind("JobId") %>'
                                Text='<%# Eval("Job.JobCode", "{0}: ") +  Eval("Job.Description", "{0}") %>'
                                OnClientSearch="tbJob_Search" WebMethod="GetJobsForDivision" WebServicePath="~/Services/Contractors.asmx"
                                ValidateWebMethodName="ValidateJobForDivision" AutoValidate="false" OnClientSelect="tbJob_Select"
                                OnClientKeyPress="tbJob_KeyPress" OnClientChange="tbJob_Change" Delay="1000">
                            </i:AutoComplete>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Contractor" AccessibleHeaderText="Contractor">
                        <ItemTemplate>
                            <i:AutoComplete ID="tbContractor" FriendlyName="Contractor" runat="server" Text='<%# Eval("Contractor.ContractorCode", "{0}: ") +  Eval("Contractor.ContractorName", "{0}") %>'
                                Value='<%# Bind("ContractorId") %>' WebMethod="GetContractors" ValidateWebMethodName="ValidateContractor"
                                WebServicePath="~/Services/Contractors.asmx" AutoValidate="true" Delay="1000">
                            </i:AutoComplete>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Head" AccessibleHeaderText="Head">
                        <FooterStyle HorizontalAlign="Right" Font-Size="Large" />
                        <ItemTemplate>
                            <i:AutoComplete ID="tbHead" runat="server" FriendlyName="Head of Account" Value='<%# Bind("HeadOfAccountId") %>'
                                Text='<%# Eval("HeadOfAccount.DisplayDescription") %>' WebMethod="GetHeadOfAccountForStation"
                                ValidateWebMethodName="ValidateHeadOfAccount" WebServicePath="~/Services/HeadOfAccounts.asmx"
                                AutoValidate="true" Delay="1000" OnClientSearch="tbHead_Search">
                                <Validators>
                                    <i:Required DependsOn="IsRowSelected" DependsOnState="Custom" OnServerDependencyCheck="val_IsRowSelected"
                                        ClientMessageFunction="msg_HeadRequired" OnServerValidate="val_HeadRequired" />
                                </Validators>
                            </i:AutoComplete>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Debit" AccessibleHeaderText="Debit">
                        <FooterStyle HorizontalAlign="Right" Font-Size="Large" />
                        <ItemTemplate>
                            <i:TextBoxEx ID="tbDebit" runat="server" Text='<%# Bind("DebitAmount", "{0:C2}") %>'
                                FriendlyName="Debit Amount" ToolTip="Debit Amount">
                                <Validators>
                                    <i:Value DependsOn="IsRowSelected" DependsOnState="Custom" ValueType="Decimal" Min="0"
                                        OnServerDependencyCheck="val_IsRowSelected" ClientMessageFunction="msg_NumericAmount" />
                                </Validators>
                            </i:TextBoxEx>
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:Literal runat="server" ID="litDebitSum"></asp:Literal>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Credit" AccessibleHeaderText="Credit">
                        <FooterStyle HorizontalAlign="Right" Font-Size="Large" />
                        <ItemTemplate>
                            <i:TextBoxEx ID="tbCredit" runat="server" Text='<%# Bind("CreditAmount", "{0:C2}") %>'
                                FriendlyName="Credit Amount" ToolTip="Credit Amount">
                                <Validators>
                                    <i:Custom DependsOn="IsRowSelected" DependsOnState="Custom" Rule="DebitOrCredit"
                                        OnServerDependencyCheck="val_IsRowSelected" OnServerValidate="val_DebitOrCredit" />
                                    <i:Value DependsOn="IsRowSelected" DependsOnState="Custom" ValueType="Decimal" Min="0"
                                        OnServerDependencyCheck="val_IsRowSelected" ClientMessageFunction="msg_NumericAmount" />
                                </Validators>
                            </i:TextBoxEx>
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:Literal runat="server" ID="litCreditSum"></asp:Literal>
                        </FooterTemplate>
                    </asp:TemplateField>
                </Columns>
            </jquery:GridViewExInsert>
            <i:ButtonEx ID="btnSave" runat="server" Text="Save" Action="Submit" CausesValidation="true"
                Icon="Disk" Enabled="true" OnClick="btnSave_Click" OnClientClick="btnSave_Click"
                OnPreRender="btnSave_PreRender" />
            <i:LinkButtonEx runat="server" ID="btnAddRow" Text="Show more rows" CausesValidation="true"
                ToolTip="If you need more detail lines for the voucher, click here" OnClick="btnAddRow_Click" />
            <i:LinkButtonEx ID="btnCancel2" runat="server" Text="Cancel" CausesValidation="false"
                OnClick="btnCancel_Click" />
        </EditItemTemplate>
        <ItemTemplate>
            <jquery:Dialog runat="server" ID="dlgVoucher" AutoOpen="true" OnPreRender="dlgVoucher_PreRender"
                ClientIDMode="Static">
                <Ajax Url="VoucherDetails.aspx" UseDialog="false" />
            </jquery:Dialog>
            <i:LinkButtonEx ID="btnEdit" runat="server" Text="Edit" CausesValidation="false" Enabled="false"
                Action="Submit" OnClick="btnEdit_Click" RolesRequired="FinanceManager"
                OnPreRender="DisableLinkButtonEx_PreRender" />
            <i:LinkButtonEx ID="btnDelete" runat="server" Text="Delete" OnClick="btnDelete_Click" Enabled="false" OnPreRender="DisableLinkButtonEx_PreRender"
                RolesRequired="FinanceManager" Action="Submit" CausesValidation="false" OnClientClick="
function(e) {
    return confirm('Are you sure you want to delete the Voucher?');
}" />
            <asp:HyperLink ID="HyperLink4" runat="server" NavigateUrl="~/Finance/InsertVoucher.aspx"
                Text="New" />
        </ItemTemplate>
        <EmptyDataTemplate>
            <phpa:FormViewStatusMessage ID="fvDeleteStatusMessage" runat="server" />
            What would you like to do now ?
           
            <ul>
                <asp:LoginView ID="LoginView1" runat="server">
                    <AnonymousTemplate>
                        <li>To create vouchers, you must
                           
                            <asp:HyperLink ID="HyperLink3" runat="server" NavigateUrl="~/Login.aspx">login</asp:HyperLink>.
                        </li>
                    </AnonymousTemplate>
                    <RoleGroups>
                        <asp:RoleGroup Roles="Operator">
                            <ContentTemplate>
                                <li>You can
                                   
                                    <asp:HyperLink ID="HyperLink4" runat="server" NavigateUrl="~/Finance/InsertVoucher.aspx"
                                        Text="Create New Voucher" />
                                    . </li>
                                <li>To edit a voucher,
                                   
                                    <asp:HyperLink ID="HyperLink3" runat="server" NavigateUrl="~/Finance/DayBook.aspx">Go to Day Book</asp:HyperLink>
                                    and select the voucher to edit. </li>
                                <li>
                            </ContentTemplate>
                        </asp:RoleGroup>
                    </RoleGroups>
                    <LoggedInTemplate>
                        <li>You need to be an operator to create. You can view vouchers for a specific date
                            using the
                           
                            <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="~/Finance/DayBook.aspx">Day Book</asp:HyperLink>
                        </li>
                    </LoggedInTemplate>
                </asp:LoginView>
                <li>
                    <asp:HyperLink ID="HyperLink2" runat="server" NavigateUrl="~/Finance/VoucherDiscrepancies.aspx?ReportName=DiscrepentVouchers">Correct voucher discrepancies</asp:HyperLink>,
                    i.e. vouchers whose total debits do not equal total credits. </li>
            </ul>
        </EmptyDataTemplate>
    </asp:FormView>
    <uc1:VoucherDetailControl ID="ctlVoucherDetail" runat="server" OrderBy="(Modified == null ? Created : Modified) desc"
        EnableViewState="false" />
</asp:Content>
