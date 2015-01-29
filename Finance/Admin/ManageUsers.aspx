<%@ Page Language="C#" CodeBehind="ManageUsers.aspx.cs" Inherits="PhpaAll.ManageUsers"
    MasterPageFile="~/MasterPage.master" Title="Manage Users" EnableViewState="true" %>

<asp:Content ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        $.validator.addMethod('issame', function (value, element, params) {
            return value == $(params).val();
        });
    </script>
    <script type="text/javascript">
        function DeleteConfirmation(e) {
            return confirm("If you delete this user, he will no longer be able to log in. Are you sure?");
        }
    </script>
</asp:Content>
<asp:Content ID="c2" ContentPlaceHolderID="cph" runat="server">
    <eclipse:TwoColumnPanel ID="TwoColumnPanel1" runat="server">
        <eclipse:LeftLabel ID="LeftLabel1" runat="server" Text="Login ID" />
        <i:TextBoxEx ID="tbSearchUser" runat="server" MaxLength="10">
        </i:TextBoxEx>
        <eclipse:LeftLabel ID="LeftLabel2" runat="server" Text="User Name Contains" />
        <i:TextBoxEx ID="tbFullName" runat="server" MaxLength="10" />
    </eclipse:TwoColumnPanel>
    <i:ButtonEx ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click"
        Action="Submit" Icon="Search" />
    <i:ButtonEx ID="btnClearSearch" runat="server" Text="Clear Search" Action="Reset"
        Icon="Refresh" />
    <phpa:PhpaLinqDataSource ID="dsUsers" runat="server" ContextTypeName="Eclipse.PhpaLibrary.Database.AuthenticationDataContext"
        TableName="PhpaUsers" RenderLogVisible="False" OnSelecting="dsUsers_Selecting"
        OrderBy="UserName">
        <WhereParameters>
            <asp:ControlParameter ControlID="tbFullName" Name="FullName" PropertyName="Text" />
            <asp:ControlParameter ControlID="tbSearchUser" Name="UserName" PropertyName="Text" />
        </WhereParameters>
    </phpa:PhpaLinqDataSource>
    <p>
        <i:ButtonEx ID="btnNew" runat="server" CausesValidation="False" OnClick="btnNew_Click"
            Action="Submit" Icon="PlusThick" Text="Create New User" RolesRequired="*" />
    </p>
    <jquery:Dialog ID="dlgEditor" runat="server" Title="User Editor" EnablePostBack="true"
        EnableViewState="true" Position="RightTop" Width="400" Visible="false">
        <ContentTemplate>
            <phpa:PhpaLinqDataSource ID="dsSpecificUser" runat="server" ContextTypeName="Eclipse.PhpaLibrary.Database.AuthenticationDataContext"
                EnableDelete="True" EnableInsert="True" EnableUpdate="True" TableName="PhpaUsers"
                AutoGenerateWhereClause="True" RenderLogVisible="False" OnSelecting="dsSpecificUser_Selecting">
                <WhereParameters>
                    <asp:ControlParameter ControlID="gvUser" Name="UserId" Type="Int32" />
                </WhereParameters>
            </phpa:PhpaLinqDataSource>
            <asp:FormView ID="frmViewSpecificUser" runat="server" DataSourceID="dsSpecificUser"
                DataKeyNames="UserId" OnItemDeleted="frmViewSpecificUser_ItemDeleted" OnItemInserted="frmViewSpecificUser_ItemInserted"
                OnDataBound="frmViewSpecificUser_DataBound" OnItemInserting="frmViewSpecificUser_ItemInserting"
                OnItemUpdating="frmViewSpecificUser_ItemUpdating">
                <EmptyDataTemplate>
                    <asp:Label ID="lblEmptyMsg" runat="server" Text="Select the users to view details. You can also edit them. Click on the link below to Create New User" /><br />
                    <i:LinkButtonEx ID="btnNewUser" runat="server" CausesValidation="False" OnClick="btnNew_Click"
                        Text="New User" />
                </EmptyDataTemplate>
                <EditItemTemplate>
                    <jquery:JPanel runat="server" IsValidationContainer="true">
                        <eclipse:TwoColumnPanel ID="TwoColumnPanel2" runat="server" WidthLeft="125">
                            <eclipse:LeftLabel runat="server" Text="Login ID" />
                            <i:TextBoxEx runat="server" ID="tbUserName1" MaxLength="10" Text='<%# Bind("UserName") %>'>
                                <Validators>
                                    <i:Required />
                                </Validators>
                            </i:TextBoxEx>
                            <eclipse:LeftLabel runat="server" Text="Password" />
                            <i:TextBoxEx runat="server" ID="tbPassword" MaxLength="15" Text='<%# Bind("Password") %>'
                                CaseConversion="Password" ClientIDMode="Static">
                                <Validators>
                                    <i:Required />
                                </Validators>
                            </i:TextBoxEx>
                            <eclipse:LeftLabel ID="LeftLabel5" runat="server" Text="Confirm Password" />
                            <i:TextBoxEx runat="server" ID="tbConfirmPassword" MaxLength="15" Text='<%# Bind("Password") %>'
                                CaseConversion="Password">
                                <Validators>
                                    <i:Required />
                                    <i:Custom Rule="issame" StringParams="#tbPassword" ClientMessage="'The two passwords you entered do not match'"
                                        OnServerValidate="tbConfirmPassword_ServerValidate" />
                                </Validators>
                            </i:TextBoxEx>
                            <eclipse:LeftLabel ID="LeftLabel1" runat="server" Text="Name" />
                            <i:TextBoxEx runat="server" ID="Name" MaxLength="50" Text='<%# Bind("FullName") %>'
                                Size="25">
                                <Validators>
                                    <i:Required />
                                </Validators>
                            </i:TextBoxEx>
                            <eclipse:LeftPanel runat="server">
                                <fieldset>
                                    <legend class="ui-widget-header">Roles</legend>
                                    <i:RadioButtonListEx ID="rblRoles" runat="server" Value='<%# Bind("Roles") %>' OnInit="rblRoles_Init"
                                        FriendlyName="Roles" Orientation="Vertical">
                                    </i:RadioButtonListEx>
                                </fieldset>
                            </eclipse:LeftPanel>
                            <i:CheckBoxListEx runat="server" ID="cblModules" OnLoad="cblModules_Load" FriendlyName="Packages"
                                QueryStringValue='<%# Bind("Modules") %>'>
                                <Validators>
                                    <i:Filter DependsOn="rblRoles" DependsOnState="AnyValue" DependsOnValue="Operator,Manager,Executive," />
                                    <i:Required />
                                    <i:Value MinLength="1" />
                                </Validators>
                            </i:CheckBoxListEx>
                            <eclipse:LeftLabel ID="LeftLabel6" runat="server" Text="Comments" />
                            <i:TextArea runat="server" ID="tbComment" Text='<%# Bind("AdminComment") %>' Cols="25"
                                Rows="8" />
                        </eclipse:TwoColumnPanel>
                        <eclipse:LeftPanel ID="LeftPanel1" runat="server">
                            <i:CheckBoxListEx ID="cblStation" runat="server" Value='<%# Bind("Station") %>' OnLoad="cblStation_Load"
                                FriendlyName="Authorized Stations">
                            </i:CheckBoxListEx>
                         Banks, Head of Accounts and Employees of stations on which user is authorized to work. 
                         In case no station is selected user will have access on all stations.  
                        </eclipse:LeftPanel>
                        <i:ButtonEx runat="server" ID="btnUpdate" Text="Update" OnClick="btnUpdate_Click"
                            CausesValidation="true" Action="Submit" OnPreRender="btnUpdate_PreRender" />
                        <i:ButtonEx runat="server" ID="btnCancel" Text="Cancel" OnClick="btnCancel_Click"
                            CausesValidation="false" Action="Submit" />
                        <i:ValidationSummary runat="server" />
                    </jquery:JPanel>
                </EditItemTemplate>
                <ItemTemplate>
                    <jquery:Tabs ID="tabs" runat="server" EnableViewState="true" Collapsible="false">
                        <jquery:JPanel runat="server" HeaderText="Details" ID="panelDetails" EnableViewState="true">
                            <eclipse:TwoColumnPanel ID="TwoColumnPanel3" runat="server">
                                <eclipse:LeftLabel ID="LeftLabel7" runat="server" Text="Login ID" />
                                <asp:Label ID="Label1" runat="server" Text='<%# Eval("UserName") %>' />
                                <eclipse:LeftLabel ID="LeftLabel8" runat="server" Text="Role" />
                                <asp:Label ID="Label2" runat="server" Text='<%# Eval("Roles") %>' />
                                <eclipse:LeftLabel ID="LeftLabel4" runat="server" Text="Packages" />
                                <asp:Label ID="Label5" runat="server" Text='<%# Eval("Modules") %>' />
                                <eclipse:LeftLabel ID="LeftLabel9" runat="server" Text="Full Name" />
                                <asp:Label ID="Label3" runat="server" Text='<%# Eval("FullName") %>' />
                                <eclipse:LeftLabel ID="LeftLabel3" runat="server" Text="Comments" />
                                <asp:Label ID="Label4" runat="server" Text='<%# Eval("AdminComment") %>' />
                            </eclipse:TwoColumnPanel>
                        </jquery:JPanel>
                        <phpa:AuditTabPanel ID="panelAudit" runat="server" />
                    </jquery:Tabs>
                    <i:LinkButtonEx ID="btnEdit" runat="server" Text="Edit" CausesValidation="false"
                        OnClick="btnEdit_Click" />
                    <i:LinkButtonEx ID="btnDelete" runat="server" Text="Delete" OnClick="btnDelete_Click"
                        Action="Submit" CausesValidation="false" OnClientClick="DeleteConfirmation" />
                    <i:LinkButtonEx ID="NewButton" runat="server" CausesValidation="False" OnClick="btnNew_Click"
                        Text="New User" />
                </ItemTemplate>
                <FooterTemplate>
                    <phpa:FormViewStatusMessage ID="FormViewStatusMessage2" runat="server">
                    </phpa:FormViewStatusMessage>
                    <asp:ValidationSummary ID="ValidationSummary2" runat="server" />
                </FooterTemplate>
            </asp:FormView>
        </ContentTemplate>
    </jquery:Dialog>
    <jquery:GridViewEx ID="gvUser" runat="server" AutoGenerateColumns="False" DataSourceID="dsUsers"
        PageSize="25" DataKeyNames="UserId" AllowPaging="True" EnableViewState="true"
        AllowMultipleBindings="true" OnSelectedIndexChanged="gvUser_SelectedIndexChanged">
        <Columns>
            <asp:CommandField ShowSelectButton="True" />
            <asp:BoundField DataField="UserName" HeaderText="Login ID" SortExpression="UserName" />
            <asp:BoundField DataField="FullName" HeaderText="Name" SortExpression="FullName" />
            <asp:BoundField DataField="Roles" HeaderText="Role" SortExpression="Roles" />
            <asp:BoundField DataField="Modules" HeaderText="Packages" SortExpression="Modules" />
        </Columns>
    </jquery:GridViewEx>
</asp:Content>
