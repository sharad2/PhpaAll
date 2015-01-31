<%@ Page Title="Personnel Home Page" Language="C#" MasterPageFile="~/MasterPage.master"
    CodeBehind="Default.aspx.cs" Inherits="PhpaAll.PIS.Default" EnableViewState="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .def_StaffCount
        {
            overflow: visible;
            min-width: 1.2em;
        }
        a.def_StaffCount
        {
            display: inline-block;
            min-width: 2.5em;
        }
        .container
        {
            width: 20em;
            padding-right: 1em;
            float: left;
            margin-bottom: 2mm;
            margin-top: 1.5mm;
            margin-right: 5mm;
        }
        .containerLegend
        {
            margin-bottom: 1.5mm;
        }
    </style>
    <script type="text/javascript">
        var $_target;
        $(document).ready(function () {
            $('#fsNoFileIndex').click(function (e) {
                if ($(e.target).is('a')) {
                    $_target = $(e.target);
                    $('#dlgAddEmployee').dialog('option', 'title', $_target.html())
                        .ajaxDialog('option', 'data', { EmployeeId: $_target.attr('href') })
                        .ajaxDialog('load');
                }
                return false;
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphSideNavigation" runat="server">
    <br />
    <asp:HyperLink ID="HyperLink2" runat="server" Text="Help" NavigateUrl="~/Doc/PISDefault.doc.aspx" />
    <br />
    <div>
        Search:
        <i:AutoComplete runat="server" ID="tbEmployee" ClientIDMode="Static" FriendlyName="Employee"
            WebMethod="GetEmployees" WebServicePath="~/Services/Employees.asmx" Width="20em"
            AutoValidate="true" ValidateWebMethodName="ValidateEmployee">
        </i:AutoComplete>
        <i:ButtonEx runat="server" ID="btnSearch" Text="Search" OnClientClick="function(e){
var empId = $('#tbEmployee').autocompleteEx('selectedValue');
if (empId == '') {
    window.location = 'Employees.aspx?Emp=' + $('#tbEmployee').val();
} else {
    window.location = 'EmployeeDetails.aspx?EmployeeId=' + empId
}
return false;
}" />
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cph" runat="server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cphNoForm" runat="server">
    <br />
    <jquery:Dialog runat="server" ID="dlgAddEmployee" Title="New Employee" ClientIDMode="Static"
        AutoOpen="false" Position="Top" Width="600">
        <Ajax Url="EmployeeDetailsEdit.aspx" OnAjaxDialogClosing="function(event, ui) {
        $_target.hide();
        }" />
        <Buttons>
            <jquery:RemoteSubmitButton RemoteButtonSelector="#btnUpdate" IsDefault="true" Text="Ok" />
            <jquery:CloseButton />
        </Buttons>
    </jquery:Dialog>
    <div class="ui-widget-header">
        PIS News</div>
    <phpa:PhpaLinqDataSource runat="server" ContextTypeName="Eclipse.PhpaLibrary.Database.PIS.PISDataContext"
        ID="dsNoFileIndex" Select="new (FullName, EmployeeId)" TableName="Employees"
        Visible="True" Where="FileindexNo == null && EmployeeTypeId != null && DateOfRelieve == null"
        RenderLogVisible="false" OrderBy="FirstName, LastName" />
    <asp:Repeater ID="repNoFileIndex" runat="server" DataSourceID="dsNoFileIndex" OnItemDataBound="rep_DataBound">
        <HeaderTemplate>
            <fieldset class="container" id="fsNoFileIndex">
                <legend class="containerLegend ui-state-error">File Index not defined for</legend>
        </HeaderTemplate>
        <ItemTemplate>
            <asp:MultiView runat="server" ID="mv">
                <asp:View runat="server">
                    <%# Eval("FullName") %>
                </asp:View>
                <asp:View ID="View1" runat="server">
                    <a href='<%# Eval("EmployeeId")%>'>
                        <%# Eval("FullName") %></a>
                </asp:View>
            </asp:MultiView>
        </ItemTemplate>
        <SeparatorTemplate>
            <br />
        </SeparatorTemplate>
        <FooterTemplate>
            </fieldset>
        </FooterTemplate>
    </asp:Repeater>
    <phpa:PhpaLinqDataSource runat="server" ContextTypeName="Eclipse.PhpaLibrary.Database.PIS.PISDataContext"
        ID="dsToJoin" Select="new (FullName, EmployeeId, JoiningDate)" TableName="Employees"
        Visible="True" Where="JoiningDate > DateTime.Today.AddMonths(-1) && DateOfRelieve == null"
        RenderLogVisible="false" OrderBy="JoiningDate" />
    <asp:Repeater ID="Repeater1" runat="server" DataSourceID="dsToJoin" OnItemDataBound="rep_DataBound">
        <HeaderTemplate>
            <fieldset class="container">
                <legend class="containerLegend ui-state-default">New Employees</legend>
        </HeaderTemplate>
        <ItemTemplate>
            <asp:MultiView runat="server" ID="mv">
                <asp:View ID="View2" runat="server">
                    <em>
                        <%# Eval("FullName") %>
                    </em>
                </asp:View>
                <asp:View ID="View1" runat="server">
                    <a href='<%# Eval("EmployeeId", "EmployeeDetails.aspx?EmployeeId={0}")%>'>
                        <%# Eval("FullName") %></a>
                </asp:View>
            </asp:MultiView>
            joining on
            <%# Eval("JoiningDate", "{0:d}") %>
        </ItemTemplate>
        <SeparatorTemplate>
            <br />
        </SeparatorTemplate>
        <FooterTemplate>
            </fieldset>
        </FooterTemplate>
    </asp:Repeater>
    <phpa:PhpaLinqDataSource runat="server" ContextTypeName="Eclipse.PhpaLibrary.Database.PIS.PISDataContext"
        ID="dsToRetire" Select="new (FullName, EmployeeId, DateOfRelieve)" TableName="Employees"
        Visible="True" Where="DateOfRelieve >= DateTime.Today.AddDays(-7) && DateOfRelieve < DateTime.Today.AddMonths(3)"
        RenderLogVisible="false" OrderBy="DateOfRelieve" />
    <asp:Repeater ID="Repeater2" runat="server" DataSourceID="dsToRetire" OnItemDataBound="rep_DataBound">
        <HeaderTemplate>
            <fieldset class="container">
                <legend class="containerLegend ui-state-default">Leaving Project</legend>
        </HeaderTemplate>
        <ItemTemplate>
            <asp:MultiView runat="server" ID="mv">
                <asp:View ID="View2" runat="server">
                    <em>
                        <%# Eval("FullName") %>
                    </em>
                </asp:View>
                <asp:View ID="View1" runat="server">
                    <a href='<%# Eval("EmployeeId", "EmployeeDetails.aspx?EmployeeId={0}")%>'>
                        <%# Eval("FullName") %></a>
                </asp:View>
            </asp:MultiView>
            on
            <%# Eval("DateOfRelieve", "{0:d}")%>
        </ItemTemplate>
        <SeparatorTemplate>
            <br />
        </SeparatorTemplate>
        <FooterTemplate>
            </fieldset>
        </FooterTemplate>
    </asp:Repeater>
    <phpa:PhpaLinqDataSource runat="server" ContextTypeName="Eclipse.PhpaLibrary.Database.PIS.PISDataContext"
        ID="dsCompletingProbation" Select="new (FullName, EmployeeId, ProbationEndDate)"
        TableName="Employees" Visible="True" Where="ProbationEndDate >= DateTime.Today.AddDays(-3) && ProbationEndDate < DateTime.Today.AddMonths(20)"
        RenderLogVisible="false" OrderBy="ProbationEndDate" />
    <asp:Repeater ID="Repeater3" runat="server" DataSourceID="dsCompletingProbation"
        OnItemDataBound="rep_DataBound">
        <HeaderTemplate>
            <fieldset class="container">
                <legend class="containerLegend ui-state-default">Completing Probation</legend>
        </HeaderTemplate>
        <ItemTemplate>
            <asp:MultiView runat="server" ID="mv">
                <asp:View ID="View2" runat="server">
                    <em>
                        <%# Eval("FullName") %>
                    </em>
                </asp:View>
                <asp:View ID="View1" runat="server">
                    <a href='<%# Eval("EmployeeId", "EmployeeDetails.aspx?EmployeeId={0}")%>'>
                        <%# Eval("FullName") %></a>
                </asp:View>
            </asp:MultiView>
            on
            <%# Eval("ProbationEndDate", "{0:d}")%>
        </ItemTemplate>
        <SeparatorTemplate>
            <br />
        </SeparatorTemplate>
        <FooterTemplate>
            </fieldset>
        </FooterTemplate>
    </asp:Repeater>
    <phpa:PhpaLinqDataSource runat="server" ContextTypeName="Eclipse.PhpaLibrary.Database.PIS.PISDataContext"
        ID="dsPromotionDue" OnSelecting="dsPromotionDue_Selecting" RenderLogVisible="false" />
    <asp:Repeater ID="Repeater4" runat="server" DataSourceID="dsPromotionDue" OnItemDataBound="rep_DataBound">
        <HeaderTemplate>
            <fieldset class="container">
                <legend class="containerLegend ui-state-default">Upcoming Promotions</legend>
        </HeaderTemplate>
        <ItemTemplate>
            <asp:MultiView runat="server" ID="mv">
                <asp:View ID="View2" runat="server">
                    <em>
                        <%# Eval("FullName") %>
                    </em>
                </asp:View>
                <asp:View ID="View1" runat="server">
                    <a href='<%# Eval("EmployeeId", "EmployeeDetails.aspx?EmployeeId={0}")%>'>
                        <%# Eval("FullName") %></a>
                </asp:View>
            </asp:MultiView>
            on
            <%# Eval("NextPromotionDate", "{0:d}")%>
        </ItemTemplate>
        <SeparatorTemplate>
            <br />
        </SeparatorTemplate>
        <FooterTemplate>
            </fieldset>
        </FooterTemplate>
    </asp:Repeater>
    <phpa:PhpaLinqDataSource runat="server" ContextTypeName="Eclipse.PhpaLibrary.Database.PIS.PISDataContext"
        ID="dsIncrementDue" OnSelecting="dsIncrementDue_Selecting" RenderLogVisible="false" />
    <asp:Repeater ID="Repeater5" runat="server" DataSourceID="dsIncrementDue" OnItemDataBound="rep_DataBound">
        <HeaderTemplate>
            <fieldset class="container">
                <legend class="containerLegend ui-state-default">Increment Due This Month</legend>
        </HeaderTemplate>
        <ItemTemplate>
            <asp:MultiView runat="server" ID="mv">
                <asp:View ID="View2" runat="server">
                    <em>
                        <%# Eval("FirstName") %>
                        <%# Eval("LastName") %>
                    </em>
                </asp:View>
                <asp:View ID="View1" runat="server">
                    <a href='<%# Eval("EmployeeId", "EmployeeDetails.aspx?EmployeeId={0}")%>'>
                        <%# Eval("FirstName") %>
                        <%# Eval("LastName") %></a>
                </asp:View>
            </asp:MultiView>
            on
            <%# Eval("DateOfNextIncrement", "{0:d}")%>
        </ItemTemplate>
        <SeparatorTemplate>
            <br />
        </SeparatorTemplate>
        <FooterTemplate>
            </fieldset>
        </FooterTemplate>
    </asp:Repeater>
    <phpa:PhpaLinqDataSource runat="server" ContextTypeName="Eclipse.PhpaLibrary.Database.PIS.PISDataContext"
        ID="dsRecentlyPromoted" OnSelecting="dsRecentlyPromoted_Selecting" RenderLogVisible="false" />
    <asp:Repeater ID="Repeater7" runat="server" DataSourceID="dsRecentlyPromoted" OnItemDataBound="rep_DataBound">
        <HeaderTemplate>
            <fieldset class="container">
                <legend class="containerLegend ui-state-default">Recently Promoted</legend>
        </HeaderTemplate>
        <ItemTemplate>
            <asp:MultiView runat="server" ID="mv">
                <asp:View ID="View2" runat="server">
                    <em>
                        <%# Eval("FullName") %>
                    </em>
                </asp:View>
                <asp:View ID="View1" runat="server">
                    <a href='<%# Eval("EmployeeId", "EmployeeDetails.aspx?EmployeeId={0}")%>'>
                        <%# Eval("FullName") %></a>
                </asp:View>
            </asp:MultiView>
            on
            <%# Eval("PromotionDate", "{0:d}")%>
        </ItemTemplate>
        <SeparatorTemplate>
            <br />
        </SeparatorTemplate>
        <FooterTemplate>
            </fieldset>
        </FooterTemplate>
    </asp:Repeater>
    <phpa:PhpaLinqDataSource runat="server" ContextTypeName="Eclipse.PhpaLibrary.Database.PIS.PISDataContext"
        ID="dsUpcomingBirthdays" OnSelecting="dsUpcomingBirthdays_Selecting" />
    <asp:Repeater ID="Repeater9" runat="server" DataSourceID="dsUpcomingBirthdays" OnItemDataBound="rep_DataBound">
        <HeaderTemplate>
            <fieldset class="container">
                <legend class="containerLegend ui-state-default">Upcoming Birthdays</legend>
        </HeaderTemplate>
        <ItemTemplate>
            <asp:MultiView runat="server" ID="mv">
                <asp:View ID="View2" runat="server">
                    <em>
                        <%# Eval("FullName") %>
                    </em>
                </asp:View>
                <asp:View ID="View1" runat="server">
                    <a href='<%# Eval("EmployeeId", "EmployeeDetails.aspx?EmployeeId={0}")%>'>
                        <%# Eval("FullName") %></a>
                </asp:View>
            </asp:MultiView>
            on
            <%# Eval("DateOfBirth", "{0:dd MMMM}")%>
        </ItemTemplate>
        <SeparatorTemplate>
            <br />
        </SeparatorTemplate>
        <FooterTemplate>
            </fieldset>
        </FooterTemplate>
    </asp:Repeater>
    <div class="ui-helper-clearfix">
    </div>
    <phpa:PhpaLinqDataSource ID="dsGrades" runat="server" ContextTypeName="Eclipse.PhpaLibrary.Database.PIS.PISDataContext"
        OnSelecting="dsGrades_Selecting" RenderLogVisible="false">
    </phpa:PhpaLinqDataSource>
    <jquery:GridViewEx ID="GridViewEx1" runat="server" AutoGenerateColumns="false" DataSourceID="dsGrades"
        EnableViewState="False" ShowFooter="true" Caption="Staff Strength by Division and Type">
        <Columns>
            <eclipse:MultiBoundField DataFields="DivisionName" HeaderText="Division" DataFormatString="{0:N0}"
                NullDisplayText="N/A">
            </eclipse:MultiBoundField>
            <jquery:MatrixField DataHeaderFields="EmployeeType" DataMergeFields="DivisionName,DivisionId"
                DataValueFields="CountForeigners,CountBhutanese,CountTotal,EmployeeTypeId" DisplayColumnTotals="true"
                DisplayRowTotals="true" HeaderText="Type (<span class='ui-state-active'>Foreigners</span> + <span class='ui-state-highlight'>Bhutanese</span> = Total)"
                DataTotalFormatString="<span class='ui-state-active'>{0:N0}</span> + <span class='ui-state-highlight'>{1:N0}</span> = {2:N0}">
                <ItemStyle HorizontalAlign="Right" Width="8em" Wrap="false" />
                <FooterStyle Wrap="false" />
                <ItemTemplate>
                    <asp:Label runat="server" Width='<%# GetWidth(MatrixBinder.Eval("CountForeigners"), MatrixBinder.Eval("CountTotal")) %>'
                        CssClass="ui-state-active def_StaffCount" ToolTip="Foreigners" Visible='<%# (int)MatrixBinder.Eval("CountForeigners") > 0 %>'>
<%# MatrixBinder.Eval("CountForeigners", "{0:N0}")%>                
                    </asp:Label>
                    <asp:Label ID="Label1" runat="server" Width='<%# GetWidth(MatrixBinder.Eval("CountBhutanese"), MatrixBinder.Eval("CountTotal")) %>'
                        CssClass="ui-state-highlight def_StaffCount" ToolTip="Bhutanese" Visible='<%# (int)MatrixBinder.Eval("CountBhutanese") > 0 %>'>
<%# MatrixBinder.Eval("CountBhutanese", "{0:N0}")%>                
                    </asp:Label>
                    <asp:HyperLink runat="server" ToolTip="See Personnel Info" CssClass="def_StaffCount"
                        NavigateUrl='<%# string.Format("~/PIS/Reports/PersonnelInfo.aspx?EmployeeTypeId={0}&DivisionId={1}", ((int?)MatrixBinder.Eval("EmployeeTypeId")) ?? -1, ((int?)MatrixBinder.Eval("DivisionId")) ?? -1) %>'>
<%# MatrixBinder.Eval("CountTotal", "{0:N0}")%>                  
                    </asp:HyperLink>
                </ItemTemplate>
            </jquery:MatrixField>
        </Columns>
    </jquery:GridViewEx>
</asp:Content>
