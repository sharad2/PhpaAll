<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" Title="Employee Details"
    EnableViewState="false" CodeBehind="EmployeeDetails.aspx.cs" Inherits="Finance.PIS.EmployeeDetails" %>

<%@ Register Src="~/Controls/SearchEmployee.ascx" TagPrefix="uc1" TagName="SearchEmployee" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        fieldset legend
        {
            font-size: 1.5em;
        }
    </style>

    <script type="text/javascript">

        $(document).ready(function () {
            var dateOfRelieve = $('#lblDateOfRelieve').html();
            if (dateOfRelieve != null) {
                dateOfRelieve = $.datepicker.parseDate('d/m/yy', $('#lblDateOfRelieve').html());
                var d = new Date();
                var curr_date = d.getDate();
                var curr_month = d.getMonth() + 1;
                var curr_year = d.getFullYear();
                var cuurent_date = curr_date + "/" + curr_month + "/" + curr_year;
                cuurent_date = $.datepicker.parseDate('d/m/yy', cuurent_date);

                var empStatus = $('#lblEmployeeStatus').html();
                // If DateOfRelieve exists and is less than current date
                // then make employee terminated           
                if (cuurent_date >= dateOfRelieve && dateOfRelieve != null) {
                    // Disable buttons
                    $('.ui-button', '#ctnDiv').attr('disabled', 'disabled');
                    // Show header in red.
                    $('.ui-helper-clearfix.headerStyle').css('color', 'Red');
                    $('#btnTerminate1').attr('disabled', false);
                    $('#btnUndoTermination').attr('disabled', false);
                }

                //If DateOfRelieve does not exists hide termination section
                if (dateOfRelieve == null) {
                    //Hide Termination section when not filled
                    $('#fldTermination').hide();
                    $('#btnUndoTermination').hide();
                }
            }           
        });
        // Delete recent employee service period.
        function btnDeleteServicePeriod_Click(e) {
            var b = confirm('This will delete service information for ' + $('#lblFullName').html() + '. You can add it again later. Click Ok to confirm');
            if (b) {
                CallPageMethod('DeleteServicePeriod', { servicePeriodId: $(this).prev().html() }, function() {
                    $('form:first').submit();
                });
            }
        }
        // Deletes an employee
        function btnDeleteEmployee_Click(e) {
            var b = confirm('This will delete all information for ' +
                     $('#lblFullName').html() + '. Press Ok to confirm.');
            if (b) {
                CallPageMethod('DeleteEmployee', { employeeId: $(this).prev().html() }, function() {
                    window.location = 'Employees.aspx';
                });
            }
        }

        // Undo Termination
        function btnUndoTermination_Click(e) {
            var b = confirm('Are you sure you want to undo termination of ' + $('#lblFullName').html() + '? Press ok to confirm.');
            if (b) {
                CallPageMethod('UndoTermination', { employeeId: $('#spEmployeeId').html() }, function() {
                    window.location = 'EmployeeDetails.aspx?EmployeeId=' + $('#spEmployeeId').html();
                });
            }
        }
        


       
    </script>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphSideNavigation" runat="server">
    <br />
    <asp:HyperLink ID="HyperLink2" runat="server" Text="Help" NavigateUrl="~/Doc/EmployeeDetails.doc.aspx" /><br/>
    <uc1:SearchEmployee runat="server" />
    <asp:LoginView runat="server">
        <RoleGroups>
            <asp:RoleGroup Roles="PayrollManager">
                <ContentTemplate>
                    <i:ButtonEx runat="server" Text="Add New Employee..." OnClientClick="function(e) {
$('#dlgAddEmployee')
.ajaxDialog('option','data',{ActiveTab: 'TE'})
.ajaxDialog('load');}" />
                    <br />
                    <jquery:Dialog runat="server" ID="dlgAddEmployee" Title="New Employee" ClientIDMode="Static"
                        AutoOpen="false" Position="Top" Width="600">
                        <Ajax Url="EmployeeDetailsEdit.aspx" OnAjaxDialogClosing="function(event, ui) {                                                                                       
                       window.location='EmployeeDetails.aspx?EmployeeId=' + ui.data;
                        }" />
                        <Buttons>
                            <jquery:RemoteSubmitButton RemoteButtonSelector="#btnUpdate" IsDefault="true" Text="Insert" />
                            <jquery:CloseButton />
                        </Buttons>
                    </jquery:Dialog>
                </ContentTemplate>
            </asp:RoleGroup>
        </RoleGroups>
    </asp:LoginView>
    <asp:HyperLink runat="server" NavigateUrl="~/PIS/Employees.aspx" Text="Employee List" />
    <br />
    <fieldset style="padding-left: 1em">
        <legend>Jump to</legend><a href="#Professional">Professional Details</a>
        <br />
        <a href="#Financial">Financial Details</a>
        <br />
        <a href="#Personal">Personal Details</a>
        <br />
    </fieldset>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cph" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphNoForm" runat="server">
    <div id="ctnDiv">
        <div style="padding-top: 0.5em; padding-bottom: 0.5em; font-size: medium;">
            <asp:Label runat="server" ID="lblSubTitle" Font-Italic="true" />
        </div>
        <phpa:PhpaLinqDataSource runat="server" ID="dsServiceHistory" ContextTypeName="Eclipse.PhpaLibrary.Database.PIS.PISDataContext"
            TableName="ServicePeriods" Where="EmployeeId == @EmployeeId" OnSelecting="dsServiceHistory_Selecting"
            RenderLogVisible="False" OrderBy="PeriodStartDate desc">
            <WhereParameters>
                <asp:QueryStringParameter Name="EmployeeId" QueryStringField="EmployeeId" Type="Int32" />
            </WhereParameters>
        </phpa:PhpaLinqDataSource>
        <asp:FormView runat="server" ID="fvServiceHistory" DataSourceID="dsServiceHistory"
            DataKeyNames="ServicePeriodId,EmployeeId">
            <HeaderTemplate>
                Service
            </HeaderTemplate>
            <EmptyDataTemplate>
                Service period not defined.
                <i:ButtonEx runat="server" Icon="Refresh" Text="Add..." OnClientClick="function(e){
                $('#dlgEmployeeDetailsEdit')
                    .ajaxDialog('option', 'data', {ActiveTab: 'SP'})
                    .ajaxDialog('load');
                }" />
            </EmptyDataTemplate>
            <ItemTemplate>
                <fieldset style="width: 20em; float: left">
                    <legend>Primary</legend>
                    <eclipse:TwoColumnPanel runat="server">
                        <eclipse:LeftLabel runat="server" Text="Designation" />
                        <asp:Label runat="server" Text='<%# Eval("Designation") %>' />
                        <eclipse:LeftLabel runat="server" Text="Grade" />
                        <asp:Label runat="server" Text='<%# Eval("Grade") %>' />
                       <%-- <eclipse:LeftLabel runat="server" Text="Posted At" />
                        <asp:Label runat="server" Text='<%# Eval("PostedAt") %>' />--%>
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
                    <div class="ui-helper-clearfix">
                        <i:ButtonEx runat="server" Icon="Refresh" Text="Edit..." OnClientClick="function(e){
                $('#dlgEmployeeDetailsEdit')
                    .ajaxDialog('option', 'data', {ActiveTab: 'SP'})
                    .ajaxDialog('load');
                }" />
                        <span class="ui-helper-hidden">
                            <%# Eval("ServicePeriodId")%></span>
                        <i:ButtonEx runat="server" ID="btnDeleteServicePeriod" Text="Delete Service..." OnClientClick="btnDeleteServicePeriod_Click" />
                    </div>
                </fieldset>
                <fieldset style="width: 20em; float: left">
                    <legend>Salary</legend>PayScale
                    <br />
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
                    </eclipse:TwoColumnPanel>
                </fieldset>
                <fieldset>
                    <legend>Increment</legend>
                    <eclipse:TwoColumnPanel runat="server">
                        <eclipse:LeftLabel runat="server" Text="Increment Date" />
                        <asp:Label runat="server" Text='<%# Eval("DateOfIncrement","{0:d}") %>' />
                        <eclipse:LeftLabel runat="server" Text="Next Increment Date" />
                        <asp:Label runat="server" Text='<%# Eval("DateOfNextIncrement","{0:d}") %>' />
                    </eclipse:TwoColumnPanel>
                    <div class="ui-helper-clearfix">
                       <%-- <i:ButtonEx runat="server" ID="btnIncrement" Text="Increment..." OnClientClick="function(e){
$('#dlgIncrement')
.ajaxDialog('option','data',{Type:'I'})
.ajaxDialog('load');
                }" />--%>
                        <i:ButtonEx runat="server" ID="btnEditIncrement" Text="Edit Increment..." OnClientClick="function(e){
$('#dlgIncrement')
.ajaxDialog('option','data',{Type:'E'})
.ajaxDialog('load');
                }" />
                        <jquery:Dialog ID="dlgIncrement" runat="server" ClientIDMode="Static" AutoOpen="false"
                            Title="Increment" OnPreRender="dlgIncrementPromotion_PreRender">
                            <Ajax Url="Share/GrantIncrement.aspx" OnAjaxDialogClosing="function(event, ui) {
$('form:first').submit();                
                }" />
                            <Buttons>
                                <jquery:RemoteSubmitButton Text="Increment" RemoteButtonSelector="#btnIncrement"
                                    IsDefault="true" />
                                <jquery:CloseButton />
                            </Buttons>
                        </jquery:Dialog>
                    </div>
                </fieldset>
                <fieldset>
                    <legend>Promotion</legend>
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
                    <div class="ui-helper-clearfix">
                       <%-- <i:ButtonEx runat="server" ID="btnPromote" Text="Promote..." OnClientClick="function(e){
$('#dlgPromotion')
.ajaxDialog('option','data',{Type:'I'})
.ajaxDialog('load');
                }" />--%>
                        <i:ButtonEx runat="server" ID="btnUnPromote" Text="Edit Promotion..." OnClientClick="function(e){
$('#dlgPromotion')
.ajaxDialog('option','data',{Type:'E'})
.ajaxDialog('load');
                }" />
                        <jquery:Dialog ID="dlgPromotion" runat="server" ClientIDMode="Static" AutoOpen="false"
                            Title="Promotion" OnPreRender="dlgIncrementPromotion_PreRender">
                            <Ajax Url="Share/GrantPromotion.aspx" OnAjaxDialogClosing="function(event, ui) {
$('form:first').submit();                
                }" />
                            <Buttons>
                                <jquery:RemoteSubmitButton Text="Promote" RemoteButtonSelector="#btnPromote" IsDefault="true" />
                                <jquery:CloseButton />
                            </Buttons>
                        </jquery:Dialog>
                    </div>
                </fieldset>
            </ItemTemplate>
        </asp:FormView>
        <phpa:PhpaLinqDataSource runat="server" ID="dsEmployeeDetails" ContextTypeName="Eclipse.PhpaLibrary.Database.PIS.PISDataContext"
            TableName="Employees" RenderLogVisible="false" Where="EmployeeId==@EmployeeId"
            OnContextCreated="dsEmployeeDetails_ContextCreated" OnSelecting="dsEmployeeDetails_Selecting"
            OnSelected="dsEmployeeDetails_Selected">
            <WhereParameters>
                <asp:QueryStringParameter Name="EmployeeId" Type="Int32" QueryStringField="EmployeeId" />
            </WhereParameters>
        </phpa:PhpaLinqDataSource>
        <asp:FormView runat="server" ID="fvEmployeeDetails" DataSourceID="dsEmployeeDetails"
            DataKeyNames="EmployeeId" OnDataBound="fvEmployeeDetails_DataBound">
            <ItemTemplate>
                <fieldset id="fldTermination" style="width: 100%">
                    <legend>Termination</legend>
                    <eclipse:TwoColumnPanel runat="server" ID="tcpTermination">
                        <eclipse:LeftLabel runat="server" Text="Relieve Order No" />
                        <asp:Label runat="server" Text='<%# Eval("RelieveOrderNo") %>' />
                        <eclipse:LeftLabel runat="server" Text="Relieve Order Date" />
                        <asp:Label runat="server" Text='<%# Eval("RelieveOrderDate","{0:d}") %>' />
                        <eclipse:LeftLabel runat="server" Text="Reason for Leaving" />
                        <asp:Label runat="server" Text='<%# Eval("LeavingReason") %>' />
                        <eclipse:LeftLabel runat="server" Text="Relieve Date" />
                        <span id="lblDateOfRelieve">
                            <%# Eval("DateOfRelieve","{0:d}")%></span>
                        <eclipse:LeftLabel runat="server" Text="Termination Status" />
                        <span id="lblEmployeeStatus">
                            <%# Eval("EmployeeStatus.EmployeeStatusType")%></span>
                    </eclipse:TwoColumnPanel>
                </fieldset>
                <i:ButtonEx runat="server" ID="btnTerminate1" Text='<%# Eval("FullName","Terminate {0}...") %>'
                    ClientIDMode="Static" OnClientClick="function(e){
$('#dlgTerminate').ajaxDialog('load');
                }" />
                <jquery:Dialog ID="dlgTerminate" runat="server" ClientIDMode="Static" AutoOpen="false"
                    Title="Terminate" OnPreRender="dlg_PreRender">
                    <Ajax Url="Termination.aspx" OnAjaxDialogClosing="function(event, ui) {
$('form:first').submit();                
                }" />
                    <Buttons>
                        <jquery:RemoteSubmitButton Text="Terminate" RemoteButtonSelector="#btnTerminate"
                            IsDefault="true" />
                        <jquery:CloseButton />
                    </Buttons>
                </jquery:Dialog>
                <i:ButtonEx runat="server" ID="btnUndoTermination" Text="Undo Termination..." OnClientClick="btnUndoTermination_Click"
                    ClientIDMode="Static" />
                <fieldset id="Professional" style="width: 100%">
                    <legend>Professional</legend>
                    <div style="width: 100%">
                        <div style="width: 30em; float: left; overflow: auto">
                            <eclipse:TwoColumnPanel runat="server">
                                <eclipse:LeftLabel runat="server" Text="Joining Date" />
                                <asp:Label runat="server" Text='<%# Eval("JoiningDate","{0:d}") %>' />
                                <eclipse:LeftLabel runat="server" Text="Code" />
                                <asp:Label runat="server" Text='<%# Eval("EmployeeCode") %>' />
                                <eclipse:LeftLabel ID="LeftLabel1" runat="server" Text="Employee Number" />
                                <asp:Label ID="Label1" runat="server" Text='<%# Eval("EmployeeNumber") %>' />
                                <eclipse:LeftLabel runat="server" Text="Employee Type" />
                                <asp:Label runat="server" Text='<%# Eval("EmployeeType.Description") %>' />
                                <eclipse:LeftLabel runat="server" Text="Division" />
                                <asp:Label runat="server" Text='<%# Eval("Division.DivisionName") %>' />
                                <eclipse:LeftLabel runat="server" Text="Sub Division" />
                                <asp:Label runat="server" Text='<%# Eval("SubDivision.SubDivisionName") %>' />
                                <eclipse:LeftLabel runat="server" Text="Office" />
                                <asp:Label runat="server" Text='<%# Eval("Office.OfficeName") %>' />
                                 <eclipse:LeftLabel ID="LeftLabel2" runat="server" Text="Posted At" />
                                 <asp:Label ID="Label2" runat="server" Text='<%# Eval("Station.StationName") %>' />
                                <eclipse:LeftLabel runat="server" Text="Probation End Date" />
                                <asp:Label runat="server" Text='<%# Eval("ProbationEndDate","{0:d}") %>' />
                                <eclipse:LeftLabel runat="server" Text="ACR Date" />
                                <asp:Label runat="server" Text='<%# Eval("ACRDate","{0:d}") %>' />
                                <eclipse:LeftLabel runat="server" Text="Fileindex No" />
                                <asp:Label runat="server" Text='<%# Eval("FileindexNo") %>' />
                                <eclipse:LeftLabel runat="server" Text="Citizen Card /Work Permit#" />
                                <asp:Label runat="server" Text='<%# Eval("CitizenCardNo") %>' />
                                <eclipse:LeftLabel runat="server" Text="Parent Organization" />
                                <asp:Label runat="server" Text='<%# Eval("ParentOrganization") %>' />
                            </eclipse:TwoColumnPanel>
                            <i:ButtonEx runat="server" ID="btnProfessional" Icon="Refresh" Text="Edit..." OnClientClick="function(e){
                $('#dlgEmployeeDetailsEdit')
                    .ajaxDialog('option', 'data', {ActiveTab: 'PR'})
                    .ajaxDialog('load');
                }" />
                            <span id="spEmployeeId" class="ui-helper-hidden">
                                <%# Eval("EmployeeId")%></span>
                            <i:ButtonEx ID="btnDeleteEmployee" runat="server" OnClientClick="btnDeleteEmployee_Click"
                                Text='<%# Eval("FullName", "Delete {0}...") %>' />
                        </div>
                        <div style="float: left; overflow: auto; margin-left: 2em;">
                            <fieldset>
                                <legend>Audit</legend>
                                <phpa:AuditTabPanel runat="server" />
                            </fieldset>
                        </div>
                    </div>
                    <div style="width: 100%; float: left;">
                        <jquery:Dialog runat="server" ID="dlgQualifications" AutoOpen="true" ClientIDMode="Static"
                            OnPreRender="dlg_PreRender">
                            <Ajax Url="Qualifications.aspx" UseDialog="false" />
                        </jquery:Dialog>
                        <jquery:Dialog runat="server" ID="dlgGrants" AutoOpen="true" ClientIDMode="Static"
                            OnPreRender="dlg_PreRender">
                            <Ajax Url="Grants.aspx" UseDialog="false" />
                        </jquery:Dialog>
                        <jquery:Dialog runat="server" ID="dlgTrainings" AutoOpen="true" ClientIDMode="Static"
                            OnPreRender="dlg_PreRender">
                            <Ajax Url="Trainings.aspx" UseDialog="false" />
                        </jquery:Dialog>
                        <jquery:Dialog runat="server" ID="dlgLeaveInfo" AutoOpen="true" ClientIDMode="Static"
                            OnPreRender="dlg_PreRender">
                            <Ajax Url="Leaves.aspx" UseDialog="false" />
                        </jquery:Dialog>
                    </div>
                </fieldset>
                <fieldset id="Financial" style="width: 100%">
                    <legend>Financial</legend>
                    <div style="width: 20em; float: left">
                        <eclipse:TwoColumnPanel runat="server">
                            <eclipse:LeftLabel runat="server" Text="Basic Salary" />
                            <asp:Label runat="server" Text='<%# Eval("BasicSalary","{0:N2}") %>' />
                            <eclipse:LeftLabel runat="server" Text="Bank Account Number" />
                            <asp:Label runat="server" Text='<%# Eval("BankAccountNo") %>' />
                            <eclipse:LeftLabel runat="server" Text="Bank Name" />
                            <asp:Label runat="server" Text='<%# Eval("Bank.BankName") %>' />
                            <eclipse:LeftLabel runat="server" Text="Bank Address" />
                            <asp:Label runat="server" Text='<%# Eval("BankPlace") %>' />
                            <eclipse:LeftLabel runat="server" Text="Bank Loan Account Number" />
                            <asp:Label runat="server" Text='<%# Eval("BankLoanAccountNo") %>' />
                            <eclipse:LeftLabel runat="server" Text="GIS Account Number" />
                            <asp:Label runat="server" Text='<%# Eval("GISAccountNumber") %>' />
                            <eclipse:LeftLabel runat="server" Text="GPF Account Number" />
                            <asp:Label runat="server" Text='<%# Eval("GPFAccountNo") %>' />
                            <eclipse:LeftLabel runat="server" Text="BDFC Account Number" />
                            <asp:Label runat="server" Text='<%# Eval("BDFCAccountNo") %>' />
                            <eclipse:LeftLabel runat="server" Text="NPPFP Number" />
                            <asp:Label runat="server" Text='<%# Eval("NPPFPNo") %>' />
                            <eclipse:LeftLabel runat="server" Text="NPPFP Type" />
                            <asp:Label runat="server" Text='<%# Eval("NPPFType") %>' />
                            <eclipse:LeftLabel runat="server" Text="TPN" />
                            <asp:Label runat="server" Text='<%# Eval("Tpn") %>' />
                            <eclipse:LeftLabel runat="server" Text="GIS Group" />
                            <asp:Label runat="server" Text='<%# Eval("GISGroup") %>' />
                        </eclipse:TwoColumnPanel>
                        <asp:LoginView runat="server">
                            <RoleGroups>
                                <asp:RoleGroup Roles="PayrollManager">
                                    <ContentTemplate>
                                        <i:ButtonEx runat="server" ID="btnFinancial" Icon="Refresh" Text="Edit..." OnClientClick="function(e){
                $('#dlgEmployeeDetailsEdit')
                    .ajaxDialog('option', 'data', {ActiveTab: 'FI'})
                    .ajaxDialog('load');
                }" />
                                    </ContentTemplate>
                                </asp:RoleGroup>
                            </RoleGroups>
                            <LoggedInTemplate>
                                <h5>
                                    Only Payroll Manager can edit financial details</h5>
                                .
                            </LoggedInTemplate>
                        </asp:LoginView>
                    </div>
                    <div style="float: left">
                        <jquery:Dialog runat="server" ID="dlgMedicalAllowance" AutoOpen="true" ClientIDMode="Static"
                            OnPreRender="dlg_PreRender">
                            <Ajax Url="MedicalAllowance.aspx" UseDialog="false" />
                        </jquery:Dialog>
                    </div>
                </fieldset>
                <fieldset id="Personal" style="width: 100%">
                    <legend>Personal</legend>
                    <div style="width: 20em; float: left; overflow: auto">
                        <eclipse:TwoColumnPanel runat="server">
                            <eclipse:LeftLabel runat="server" Text="Name" />
                            <span id="lblFullName">
                                <%# Eval("FullName") %></span>
                            <eclipse:LeftLabel runat="server" Text="Gender" />
                            <asp:Label runat="server" Text='<%# Eval("Gender") %>' />
                            <eclipse:LeftLabel runat="server" Text="Permanent Address" />
                            <pre><%# Eval("HomeTown", "{0}") %></pre>
                            <eclipse:LeftLabel runat="server" Text="Nationality" />
                            <asp:Label runat="server" Text='<%# (bool)Eval("IsBhutanese") ? "Bhutanese" : "Foreigner" %>' />
                            <eclipse:LeftLabel runat="server" Text="Birth Date" />
                            <asp:Label runat="server" Text='<%# Eval("DateOfBirth","{0:d}") %>' />
                            <eclipse:LeftLabel runat="server" Text="Blood Group" />
                            <asp:Label runat="server" Text='<%# Eval("BloodGroup.BloodGroupType") %>' />
                            <eclipse:LeftLabel runat="server" Text="Marital Status" />
                            <asp:Label runat="server" Text='<%# Eval("MaritalStatus.MaritalStatusType") %>' />
                            <eclipse:LeftLabel runat="server" Text="Religion" />
                            <asp:Label runat="server" Text='<%# Eval("Religion") %>' />
                            <eclipse:LeftLabel runat="server" Text="Identification Mark" />
                            <asp:Label runat="server" Text='<%# Eval("IdentificationMark") %>' />
                            <eclipse:LeftLabel runat="server" Text="Height(in cms)" />
                            <asp:Label runat="server" Text='<%# Eval("Height") %>' />
                        </eclipse:TwoColumnPanel>
                        <i:ButtonEx runat="server" Icon="Refresh" Text="Edit..." OnClientClick="function(e){
                $('#dlgEmployeeDetailsEdit')
                    .ajaxDialog('option', 'data', {ActiveTab: 'PE'})
                    .ajaxDialog('load');
                }" />
                    </div>
                    <jquery:Dialog ID="dlgFamilyMembers" runat="server" ClientIDMode="Static" AutoOpen="true"
                        OnPreRender="dlg_PreRender">
                        <Ajax Url="FamilyMembers.aspx" UseDialog="false" />
                    </jquery:Dialog>
                    <jquery:Dialog ID="dlgNominees" runat="server" ClientIDMode="Static" AutoOpen="true"
                        OnPreRender="dlg_PreRender">
                        <Ajax Url="Nominees.aspx" UseDialog="false" />
                    </jquery:Dialog>
                    <div class="ui-helper-clearfix">
                    </div>
                </fieldset>
                <jquery:Dialog ID="dlgEmployeeDetailsEdit" runat="server" AutoOpen="false" Width="600"
                    ClientIDMode="Static" Title='<%# Eval("FullName") %>' Position="Top" OnPreRender="dlg_PreRender">
                    <Ajax Url="EmployeeDetailsEdit.aspx" OnAjaxDialogClosing="function(event, ui) {            
window.location='EmployeeDetails.aspx?EmployeeId=' + ui.data;
                }" />
                    <Buttons>
                        <jquery:RemoteSubmitButton Text="Update" RemoteButtonSelector="#btnUpdate" IsDefault="true" />
                        <jquery:CloseButton />
                    </Buttons>
                </jquery:Dialog>
            </ItemTemplate>
        </asp:FormView>
        <jquery:Dialog ID="dlgServiceHistory" runat="server" ClientIDMode="Static" AutoOpen="true"
            OnPreRender="dlg_PreRender">
            <Ajax Url="ServiceHistory.aspx" UseDialog="false" />
        </jquery:Dialog>
    </div>
</asp:Content>
