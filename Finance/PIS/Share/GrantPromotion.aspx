<%@ Page Language="C#" CodeBehind="GrantPromotion.aspx.cs" Inherits="PhpaAll.PIS.GrantPromotion" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <jquery:JQueryScriptManager runat="server" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <phpa:PhpaLinqDataSource ID="dsPromotion" runat="server" ContextTypeName="Eclipse.PhpaLibrary.Database.PIS.PISDataContext"
            TableName="ServicePeriods" RenderLogVisible="false" EnableInsert="true" OnInserting="dsPromotion_Inserting"
            EnableUpdate="true" AutoGenerateWhereClause="true" OnUpdating="dsPromotion_Updating">
            <WhereParameters>
                <asp:QueryStringParameter Name="ServicePeriodId" QueryStringField="ServicePeriodId"
                    Type="Int32" />
            </WhereParameters>
            <InsertParameters>
                <asp:Parameter Name="PeriodStartDate" Type="DateTime" />
                <asp:Parameter Name="PeriodEndDate" Type="DateTime" />
                <asp:Parameter Name="PromotionDate" Type="DateTime" />
                <asp:Parameter Name="ExtensionUpto" Type="DateTime" />
                <asp:Parameter Name="PromotionTypeId" Type="Int32" />
                <asp:Parameter Name="EmployeeId" Type="Int32" />
                <asp:Parameter Name="NextPromotionDate" Type="DateTime" />
            </InsertParameters>
            <UpdateParameters>
                <asp:Parameter Name="PeriodStartDate" Type="DateTime" />
                <asp:Parameter Name="PeriodEndDate" Type="DateTime" />
                <asp:Parameter Name="PromotionDate" Type="DateTime" />
                <asp:Parameter Name="ExtensionUpto" Type="DateTime" />
                <asp:Parameter Name="PromotionTypeId" Type="Int32" />
                <asp:Parameter Name="EmployeeId" Type="Int32" />
                <asp:Parameter Name="NextPromotionDate" Type="DateTime" />
            </UpdateParameters>
        </phpa:PhpaLinqDataSource>
        <asp:FormView ID="fvPromotion" runat="server" DataSourceID="dsPromotion" DataKeyNames="ServicePeriodId"
            OnItemInserted="fvPromotion_ItemInserted" OnItemUpdated="fvPromotion_ItemUpdated">
            <EditItemTemplate>
                <eclipse:TwoColumnPanel runat="server">
                    <eclipse:LeftLabel runat="server" Text="Promotion Date" />
                    <i:TextBoxEx runat="server" ID="tbPromotionDate" Text='<%# Bind("PromotionDate","{0:d}") %>'
                        FriendlyName="Promotion Date">
                        <Validators>
                            <i:Date />
                            <i:Required />
                        </Validators>
                    </i:TextBoxEx>
                    <eclipse:LeftLabel runat="server" Text="Promotion Type" />
                    <phpa:PhpaLinqDataSource runat="server" ID="dsPromotionType" ContextTypeName="Eclipse.PhpaLibrary.Database.PIS.PISDataContext"
                        TableName="PromotionTypes" RenderLogVisible="false">
                    </phpa:PhpaLinqDataSource>
                    <i:DropDownListEx runat="server" ID="ddlPromotionType" DataSourceID="dsPromotionType"
                        DataTextField="PromotionDescription" DataValueField="PromotionTypeId" FriendlyName="Promotion Type"
                        Value='<%# Bind("PromotionTypeId") %>'>
                        <Items>
                            <eclipse:DropDownItem Text="(Not Set)" Value="" Persistent="Always" />
                        </Items>
                    </i:DropDownListEx>
                    <eclipse:LeftLabel runat="server" Text="Extension Upto" />
                    <i:TextBoxEx runat="server" ID="tbExtensionUpto" Text='<%# Bind("ExtensionUpto","{0:d}") %>'
                        FriendlyName="Extension Upto">
                        <Validators>
                            <i:Date DateType="ToDate" />
                        </Validators>
                    </i:TextBoxEx>
                    <eclipse:LeftLabel runat="server" Text="Next Promotion Date" />
                    <i:TextBoxEx runat="server" ID="tbNextPromotionDate" Text='<%# Bind("NextPromotionDate","{0:d}") %>'
                        FriendlyName="Next Promotion Date">
                        <Validators>
                            <i:Date />
                            <i:Custom ID="custValNextPromotionDate" OnServerValidate="custValPromotion_ServerValidate" />
                        </Validators>
                    </i:TextBoxEx>
                </eclipse:TwoColumnPanel>
                <i:ButtonEx runat="server" ID="btnPromote" Action="Submit" Icon="Refresh" CausesValidation="true"
                    OnClick="btnPromote_click" Text="Promote" ClientIDMode="Static" />
                <i:ValidationSummary ID="valPromotion" runat="server" />
            </EditItemTemplate>
        </asp:FormView>
        <jquery:StatusPanel runat="server" ID="Promotion_sp">
            <Ajax UseDialog="false" />
        </jquery:StatusPanel>
    </div>
    </form>
</body>
</html>
