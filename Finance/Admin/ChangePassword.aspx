<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ChangePassword.aspx.cs" Title="Change Password" Inherits="Finance.Admin.ChangePassword" MasterPageFile="~/MasterPage.master" %>

<asp:Content ID="Content4" ContentPlaceHolderID="cph" runat="server">
 
    <asp:ChangePassword 
        runat="server" 
        BackColor="WhiteSmoke"
        ConfirmPasswordCompareErrorMessage="Password Confirmation Failed" 
        CancelButtonStyle-CssClass="ui-state-default" 
        ChangePasswordButtonStyle-CssClass="ui-state-default"
        CancelDestinationPageUrl="~/Default.aspx"
        BorderColor="Gray" TitleTextStyle-Font-Italic="true" 
        PasswordLabelText="Old Password:" BorderStyle="Solid"  
        ContinueDestinationPageUrl="~/Default.aspx" 
        ChangePasswordButtonText="Change" 
        SuccessTextStyle-ForeColor="Green" 
        SuccessTextStyle-Font-Bold="true" 
        SuccessTextStyle-Font-Italic="true" 
        FailureTextStyle-ForeColor="Red" 
        FailureTextStyle-Font-Bold="true" 
        FailureTextStyle-Font-Italic="true" 
        BorderPadding="10" >
    </asp:ChangePassword>
</asp:Content>
