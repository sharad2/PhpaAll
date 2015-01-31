<%@ Page Language="C#" CodeBehind="Termination.aspx.cs" Inherits="PhpaAll.PIS.Termination" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <jquery:JQueryScriptManager runat="server" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <phpa:PhpaLinqDataSource ID="dsTermination" runat="server" ContextTypeName="Eclipse.PhpaLibrary.Database.PIS.PISDataContext"
            TableName="Employees" RenderLogVisible="false" EnableUpdate="true" OnUpdating="dsTermination_Updating"
            OnUpdated="dsTermination_Updated" Where="EmployeeId == @EmployeeId" OnSelecting="dsTermination_Selecting">
            <WhereParameters>
                <asp:QueryStringParameter QueryStringField="EmployeeId" Name="EmployeeId" Type="Int32" />
            </WhereParameters>
            <UpdateParameters>
                <asp:Parameter Name="RelieveOrderNo" Type="String" />
                <asp:Parameter Name="RelieveOrderDate" Type="DateTime" />
                <asp:Parameter Name="LeavingReason" Type="String" />
                <asp:Parameter Name="DateOfRelieve" Type="DateTime" />
                <asp:Parameter Name="EmployeeStatusId" Type="Int32" />
                <asp:QueryStringParameter QueryStringField="EmployeeId" Name="EmployeeId" Type="Int32" />
            </UpdateParameters>
        </phpa:PhpaLinqDataSource>
        <asp:FormView ID="fvTermination" runat="server" DefaultMode="Edit" DataSourceID="dsTermination"
            DataKeyNames="EmployeeId">
            <EditItemTemplate>
                <eclipse:TwoColumnPanel runat="server">
                    <eclipse:LeftLabel runat="server" Text="Relieve Order No" />
                    <i:TextBoxEx runat="server" ID="tbRelieveOrderNo" Text='<%# Bind("RelieveOrderNo") %>'
                        Size="10" FriendlyName="Relieve Order No" MaxLength="30">
                        <Validators>
                            <i:Required />
                        </Validators>
                    </i:TextBoxEx>
                    <eclipse:LeftLabel runat="server" Text="Relieve Order Date" />
                    <i:TextBoxEx runat="server" ID="tbRelieveOrderDate" Text='<%# Bind("RelieveOrderDate","{0:d}") %>'
                        FriendlyName="Relieve Order Date">
                        <Validators>
                            <i:Date />
                            <i:Required />
                        </Validators>
                    </i:TextBoxEx>
                    <eclipse:LeftLabel runat="server" Text="Relieve Date" />
                    <i:TextBoxEx runat="server" ID="tbDateOfRelieve" Text='<%# Bind("DateOfRelieve","{0:d}") %>'
                        FriendlyName="Relieve Date">
                        <Validators>
                            <i:Date DateType="ToDate" />
                            <i:Required />
                        </Validators>
                    </i:TextBoxEx>
                    <eclipse:LeftLabel runat="server" Text="Reason for Leaving" />
                    <i:TextBoxEx runat="server" ID="tbLeavingReason" Text='<%# Bind("LeavingReason") %>'
                        Size="20" FriendlyName="Reason for Leaving" MaxLength="50" />
                    <eclipse:LeftLabel runat="server" Text="Termination Status" />
                    <phpa:PhpaLinqDataSource runat="server" ID="dsTerminationStatus" ContextTypeName="Eclipse.PhpaLibrary.Database.PIS.PISDataContext"
                        TableName="EmployeeStatus" OrderBy="EmployeeStatusType" RenderLogVisible="false" />
                    <i:DropDownListEx runat="server" ID="ddlTerminationType" DataSourceID="dsTerminationStatus"
                        DataTextField="EmployeeStatusType" DataValueField="EmployeeStatusId" FriendlyName="Termination Status"
                        Value='<%# Bind("EmployeeStatusId") %>'>
                        <Items>
                            <eclipse:DropDownItem Text="(Not Set)" Value="" Persistent="Always" />
                        </Items>
                        <Validators>
                            <i:Required />
                        </Validators>
                    </i:DropDownListEx>
                </eclipse:TwoColumnPanel>
                <i:ValidationSummary ID="valTermination" runat="server" />
            </EditItemTemplate>
        </asp:FormView>
        <i:ButtonEx runat="server" ID="btnTerminate" Action="Submit" Icon="Refresh" CausesValidation="true"
            OnClick="btnTerminate_Click" Text="Terminate" ClientIDMode="Static" />
        <jquery:StatusPanel runat="server" ID="Termination_sp">
            <Ajax UseDialog="false" />
        </jquery:StatusPanel>
    </div>
    </form>
</body>
</html>
