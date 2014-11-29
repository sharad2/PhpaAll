<%@ Page Language="C#" CodeBehind="GrantIncrement.aspx.cs" Inherits="Finance.PIS.Share.GrantIncrement" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <jquery:JQueryScriptManager runat="server" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <phpa:PhpaLinqDataSource ID="dsGrantIncrement" runat="server" ContextTypeName="Eclipse.PhpaLibrary.Database.PIS.PISDataContext"
            TableName="ServicePeriods" RenderLogVisible="false" EnableInsert="true" OnInserting="dsGrantIncrement_Inserting"
            EnableUpdate="true" AutoGenerateWhereClause="true" OnUpdating="dsGrantIncrement_Updating">
            <WhereParameters>
                <asp:QueryStringParameter Name="ServicePeriodId" QueryStringField="ServicePeriodId"
                    Type="Int32" />
            </WhereParameters>
            <InsertParameters>
                <asp:Parameter Name="PeriodStartDate" Type="DateTime" />
                <asp:Parameter Name="PeriodEndDate" Type="DateTime" />
                <asp:Parameter Name="DateOfIncrement" Type="DateTime" />
                <asp:Parameter Name="DateOfNextIncrement" Type="DateTime" />
                <asp:Parameter Name="EmployeeId" Type="Int32" />
            </InsertParameters>
            <UpdateParameters>
                <asp:Parameter Name="PeriodStartDate" Type="DateTime" />
                <asp:Parameter Name="PeriodEndDate" Type="DateTime" />
                <asp:Parameter Name="DateOfIncrement" Type="DateTime" />
                <asp:Parameter Name="DateOfNextIncrement" Type="DateTime" />
                <asp:Parameter Name="EmployeeId" Type="Int32" />
            </UpdateParameters>
        </phpa:PhpaLinqDataSource>
        <asp:FormView ID="fvGrantIncrement" runat="server" DataSourceID="dsGrantIncrement"
            DataKeyNames="ServicePeriodId" OnItemInserted="fvGrantIncrement_ItemInserted"
            OnItemUpdated="fvGrantIncrement_ItemUpdated">
            <EditItemTemplate>
                <eclipse:TwoColumnPanel runat="server">
                    <eclipse:LeftLabel runat="server" Text="Increment Date" />
                    <i:TextBoxEx runat="server" ID="tbIncrementDate" Text='<%# Bind("DateOfIncrement","{0:d}") %>'
                        FriendlyName="Increment Date">
                        <Validators>
                            <i:Date />
                            <i:Required />
                        </Validators>
                    </i:TextBoxEx>
                    <eclipse:LeftLabel runat="server" Text="Next Increment Date" />
                    <i:TextBoxEx runat="server" ID="tbNextIncrementDate" Text='<%# Bind("DateOfNextIncrement","{0:d}") %>'
                        FriendlyName="Next Increment Date">
                        <Validators>
                            <i:Date DateType="ToDate" />
                        </Validators>
                    </i:TextBoxEx>
                </eclipse:TwoColumnPanel>
                <i:ButtonEx runat="server" ID="btnIncrement" Action="Submit" Icon="Refresh" CausesValidation="true"
                    OnClick="btnIncrement_Click" Text="Increment" ClientIDMode="Static" />
                <i:ValidationSummary ID="ValIncrement" runat="server" />
            </EditItemTemplate>
        </asp:FormView>
        <jquery:StatusPanel runat="server" ID="GrantIncrement_sp">
            <Ajax UseDialog="false" />
        </jquery:StatusPanel>
    </div>
    </form>
</body>
</html>
