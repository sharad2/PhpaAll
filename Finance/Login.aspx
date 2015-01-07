<%--$Id: Login.aspx 37050 2010-11-08 06:33:09Z bkumar $--%>

<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" CodeBehind="Login.aspx.cs"
    Inherits="Finance.Login" Title="PHPA Login" EnableViewState="false" %>

<asp:Content ID="Content3" ContentPlaceHolderID="cphSideNavigation" runat="server">
    <br />
    <asp:HyperLink ID="HyperLink2" runat="server" Text="Help" NavigateUrl="~/Doc/Login.doc.aspx" /><br />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cph" runat="server">
    <asp:Label runat="server" ID="lblWelcome" CssClass="ui-state-highlight" Visible="false">
You are not authorized to view the page <em>{0}</em>. 
Please login with valid credentials.    
    </asp:Label>
    <eclipse:TwoColumnPanel runat="server">
        <eclipse:LeftLabel runat="server" Text="User Name" />
        <i:TextBoxEx ID="tbUserName" runat="server" MaxLength="15" FocusPriority="High" FriendlyName="User name">
            <Validators>
                <i:Required />
            </Validators>
        </i:TextBoxEx>
        <eclipse:LeftLabel runat="server" Text="Password" />
        <i:TextBoxEx ID="tbPassword" runat="server"  CaseConversion="Password" MaxLength="15"
            FriendlyName="Password">
            <Validators>
                <i:Required />
            </Validators>
        </i:TextBoxEx>
        <br />
        <eclipse:LeftPanel runat="server">
            <i:CheckBoxEx runat="server" ID="cbRememberMe" Text="Remember me on this computer" Visible="true" />
        </eclipse:LeftPanel>
        <i:ButtonEx ID="btnLogin" runat="server" Text="Log In" Action="Submit" CausesValidation="true"
            OnClick="btnLogin_Click" />
    </eclipse:TwoColumnPanel>
    <i:ValidationSummary runat="server" ID="valSummary" />
    <br />
    If you are a new visitor and do not have a login Id contact one of the administrators
    to create one. You can make changes or modify data only if you are authorized to
    do so by the administrator. Following is the list of Site Administrators.
    <br />
    <br />
    <jquery:GridViewEx runat="server" AutoGenerateColumns="False" DataSourceID="dsAdmins"
        DataKeyNames="UserId" Caption="Administrators who can grant access to this site">
        <Columns>
            <asp:BoundField DataField="FullName" HeaderText="Name" SortExpression="FullName" />
            <asp:BoundField DataField="AdminComment" HeaderText="Comments" />
        </Columns>
    </jquery:GridViewEx>
    <phpa:PhpaLinqDataSource ID="dsAdmins" runat="server" ContextTypeName="Eclipse.PhpaLibrary.Database.AuthenticationDataContext"
        TableName="PhpaUsers" Where="Roles == @Roles" RenderLogVisible="false">
        <WhereParameters>
            <asp:Parameter DefaultValue="Administrator" Name="Roles" Type="String" />
        </WhereParameters>
    </phpa:PhpaLinqDataSource>
</asp:Content>
