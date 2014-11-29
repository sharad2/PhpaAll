<%@ Page Language="C#" CodeBehind="ProgressFormat.aspx.cs" Inherits="Finance.MIS.AddNewFormat" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <jquery:JQueryScriptManager runat="server" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <phpa:PhpaLinqDataSource ID="ds" runat="server" RenderLogVisible="False" ContextTypeName="Eclipse.PhpaLibrary.Database.MIS.MISDataContext"
            TableName="ProgressFormats" EnableInsert="true" OnInserted="ds_Inserted" Where="ProgressFormatId == @ProgressFormatId"
            EnableUpdate="true" OnUpdated="ds_Updated">
            <WhereParameters>
                <asp:QueryStringParameter Name="ProgressFormatId" QueryStringField="ProgressFormatId"
                    Type="Int32" />
            </WhereParameters>
            <InsertParameters>
                <asp:Parameter Name="ProgressFormatName" Type="String" />
                <asp:Parameter Name="FinancialFormatName" Type="String" />
                <asp:Parameter Name="PackageId" Type="Int32" />
                <asp:Parameter Name="ProgressFormatType" Type="Int32" />
                <asp:Parameter Name="FormatCategoryId" Type="Int32" />
                <asp:Parameter Name="Description" Type="String" />
            </InsertParameters>
            <UpdateParameters>
                <asp:QueryStringParameter Name="ProgressFormatId" QueryStringField="ProgressFormatId"
                    Type="Int32" />
                <asp:Parameter Name="ProgressFormatName" Type="String" />
                <asp:Parameter Name="FinancialFormatName" Type="String" />
                <asp:Parameter Name="PackageId" Type="Int32" />
                <asp:Parameter Name="ProgressFormatType" Type="Int32" />
                <asp:Parameter Name="FormatCategoryId" Type="Int32" />
                <asp:Parameter Name="Description" Type="String" />
            </UpdateParameters>
        </phpa:PhpaLinqDataSource>
        <asp:FormView ID="fv" runat="server" DataSourceID="ds" DataKeyNames="ProgressFormatId"
            OnItemCreated="fv_Created">
            <EditItemTemplate>
                <eclipse:TwoColumnPanel ID="TwoColumnPanel1" runat="server">
                    <eclipse:LeftLabel runat="server" Text="Format Name" />
                    <i:TextBoxEx ID="tbFormatName" runat="server" Text='<%#Bind("ProgressFormatName") %>'
                        FriendlyName="Format Name" CaseConversion="UpperCase">
                        <Validators>
                            <i:Required />
                            <i:Value MaxLength="10" />
                        </Validators>
                    </i:TextBoxEx>
                    Example:A-1,C-1
                    <eclipse:LeftLabel runat="server" Text="Financial Format Name" />
                    <i:TextBoxEx ID="tbFinancialFormatName" runat="server" Text='<%#Bind("FinancialFormatName") %>'
                        FriendlyName="Financial Format Name" CaseConversion="UpperCase">
                        <Validators>
                            <i:Value MaxLength="10" />
                        </Validators>
                    </i:TextBoxEx>
                    Example:B-4,B-5
                    <eclipse:LeftLabel ID="LeftLabel3" runat="server" Text="Package" />
                    <phpa:PhpaLinqDataSource ID="dsPackages" runat="server" RenderLogVisible="false"
                        ContextTypeName="Eclipse.PhpaLibrary.Database.MIS.MISDataContext" TableName="Packages"
                        OrderBy="PackageName">
                    </phpa:PhpaLinqDataSource>
                    <i:DropDownListEx runat="server" ID="ddlPackages" DataSourceID="dsPackages" DataTextField="DisplayName"
                        DataValueField="PackageId" FriendlyName="Package" Value='<%#Bind("PackageId") %>'>
                        <Items>
                            <eclipse:DropDownItem Text="(No available package)" Persistent="WhenEmpty" />
                        </Items>
                    </i:DropDownListEx>
                    <br />
                    <asp:HyperLink runat="server" NavigateUrl="~/MIS/Packages.aspx" Text="Add new package" />
                    <eclipse:LeftLabel runat="server" Text="Progress Category" />
                    <i:DropDownListEx ID="ddlProgressFormatType" runat="server" FriendlyName="Progress Format Type"
                        OnLoad="ddlProgressFormatType_Load" QueryString="ProgressFormatType" Value='<%#Bind("ProgressFormatType") %>'>
                    </i:DropDownListEx>
                    <eclipse:LeftLabel runat="server" Text="Format Category" />
                    <i:RadioButtonListEx ID="ddlFormatCategory" runat="server" FriendlyName="Format Category"
                        Value='<%#Bind("FormatCategoryId") %>' OnLoad="ddlFormatCategory_Load"
                        Orientation="Horizontal">
                        <Items>
                            <i:RadioItem Text="None" Value="" />
                        </Items>
                    </i:RadioButtonListEx>
                    <eclipse:LeftLabel runat="server" Text="Format Description" />
                    <i:TextBoxEx ID="tbFromatDescription" runat="server" Text='<%#Bind("Description") %>'
                        FriendlyName="Financial Format Name" Size="30">
                        <Validators>
                            <i:Value MaxLength="256" />
                        </Validators>
                    </i:TextBoxEx>
                    <br />
                    E.g. - Concrete Dam, Diversion tunnel, etc.
                    <i:ButtonEx ID="btnFormat" runat="server" Text="Update Now" Action="Submit" CausesValidation="true"
                        OnClick="btn_Click" ClientIDMode="Static" />
                </eclipse:TwoColumnPanel>
            </EditItemTemplate>
        </asp:FormView>
        <i:ValidationSummary runat="server" />
        <jquery:StatusPanel ID="sp_status" runat="server">
            <Ajax UseDialog="false" />
        </jquery:StatusPanel>
    </div>
    </form>
</body>
</html>
