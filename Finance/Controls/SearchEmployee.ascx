<%@ Control Language="C#" CodeBehind="SearchEmployee.ascx.cs" Inherits="PhpaAll.Controls.SearchEmployee"
    EnableViewState="false" %>
<div style="padding-top: 0.5em; padding-bottom: 0.5em;">
    <eclipse:LeftLabel runat="server" Text="Employee" />
    <i:TextBoxEx runat="server" ID="tbEmployee" ClientIDMode="Static"
        FriendlyName="Employee" />
    <i:ButtonEx runat="server" ID="btnSearch" Text="Search" OnClientClick="function(e){
var emp = $('#tbEmployee').val();
if(emp !=''){
window.location='Employees.aspx?Emp='+emp;
}
return false;
}" />
</div>
