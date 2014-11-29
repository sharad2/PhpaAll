<%@ Page Language="C#" CodeBehind="GrantDetails.aspx.cs" Inherits="Finance.PIS.Share.GrantDetails" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <jquery:JQueryScriptManager runat="server" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <phpa:PhpaLinqDataSource runat="server" ID="dsGrants" ContextTypeName="Eclipse.PhpaLibrary.Database.PIS.PISDataContext"
            TableName="EmployeeGrants" RenderLogVisible="false" Where="EmployeeGrantId=@EmployeeGrantId"
            OrderBy="GrantReceivedDate descending" OnSelecting="dsGrants_Selecting" EnableInsert="true"
            OnInserting="dsGrants_Inserting" EnableUpdate="true" OnUpdating="dsGrants_Updating">
            <WhereParameters>
                <asp:QueryStringParameter Name="EmployeeGrantId" Type="Int32" QueryStringField="EmployeeGrantId" />
            </WhereParameters>
            <InsertParameters>
                <asp:Parameter Name="GrantName" Type="String" />
                <asp:Parameter Name="GrantType" Type="String" />
                <asp:Parameter Name="GrantReceivedDate" Type="DateTime" />
                <asp:Parameter Name="GrantingAuthority" Type="String" />
                <asp:Parameter Name="MeritoriousService" Type="String" />
                <asp:Parameter Name="PenalityImposed" Type="String" />
                <asp:Parameter Name="EnquiryDetails" Type="String" />
                <asp:Parameter Name="Remarks" Type="String" />
                <asp:QueryStringParameter QueryStringField="EmployeeId" Name="EmployeeId" Type="Int32" />
            </InsertParameters>
            <UpdateParameters>
                <asp:Parameter Type="Int32" Name="EmployeeGrantId" />
                <asp:Parameter Name="GrantReceivedDate" Type="DateTime" />
                <asp:Parameter Name="GrantName" Type="String" />
                <asp:Parameter Name="GrantType" Type="String" />
                <asp:Parameter Name="GrantingAuthority" Type="String" />
                <asp:Parameter Name="MeritoriousService" Type="String" />
                <asp:Parameter Name="PenalityImposed" Type="String" />
                <asp:Parameter Name="EnquiryDetails" Type="String" />
                <asp:Parameter Name="Remarks" Type="String" />
                <asp:QueryStringParameter QueryStringField="EmployeeId" Name="EmployeeId" Type="Int32" />
            </UpdateParameters>
        </phpa:PhpaLinqDataSource>
        <asp:FormView runat="server" ID="fvGrantDetails" DataSourceID="dsGrants" DataKeyNames="EmployeeGrantId,EmployeeId"
            OnItemUpdated="fvGrantDetails_ItemUpdated" OnItemInserted="fvGrantDetails_ItemInserted">
            <EditItemTemplate>
                <eclipse:TwoColumnPanel runat="server">
                    <eclipse:LeftLabel runat="server" Text="Grant Type" />
                    <i:RadioButtonListEx runat="server" ID="rbGrantType" Value='<%# Bind("GrantType") %>'
                        FriendlyName="Grant Type">
                        <Items>
                            <i:RadioItem Text="Award" Value="Award" />
                            <i:RadioItem Text="Penalty" Value="Penalty" />
                        </Items>
                        <Validators>
                            <i:Required />
                        </Validators>
                    </i:RadioButtonListEx>
                    <eclipse:LeftLabel runat="server" Text="Grant Name" />
                    <i:TextBoxEx runat="server" ID="tbGrantName" Text='<%# Bind("GrantName") %>' FriendlyName="Grant Name"
                        Size="20">
                        <Validators>
                            <i:Value ValueType="String" MaxLength="50" />
                            <i:Required />
                        </Validators>
                    </i:TextBoxEx>
                    <eclipse:LeftLabel runat="server" Text="Grant Received Date" />
                    <i:TextBoxEx runat="server" ID="tbGrantReceivedDt" Text='<%# Bind("GrantReceivedDate","{0:d}") %>'
                        FriendlyName="Grant Received Date">
                        <Validators>
                            <i:Date Min="-1460" Max="365" />
                        </Validators>
                    </i:TextBoxEx>
                    <eclipse:LeftLabel runat="server" Text="Granting Authority" />
                    <i:TextBoxEx runat="server" ID="tbGrantingAuthority" Text='<%# Bind("GrantingAuthority") %>'
                        FriendlyName="Granting Authority" Size="20">
                        <Validators>
                            <i:Value MaxLength="50" ValueType="String" />
                        </Validators>
                    </i:TextBoxEx>
                    <eclipse:LeftLabel runat="server" Text="Meritorious Service" />
                    <i:TextBoxEx runat="server" ID="tbMeritoriousService" Text='<%# Bind("MeritoriousService") %>'
                        FriendlyName="Meritorious Service" Size="20">
                        <Validators>
                            <i:Value MaxLength="50" ValueType="String" />
                        </Validators>
                    </i:TextBoxEx>
                    <eclipse:LeftLabel runat="server" Text="Penalty Imposed" />
                    <i:TextBoxEx runat="server" ID="tbPenalityImposed" Text='<%# Bind("PenalityImposed") %>'
                        FriendlyName="Penalty Imposed" Size="20">
                        <Validators>
                            <i:Value MaxLength="50" ValueType="String" />
                        </Validators>
                    </i:TextBoxEx>
                    <eclipse:LeftLabel runat="server" Text="Enquiry Details" />
                    <i:TextBoxEx runat="server" ID="tbEnquiryDetails" Text='<%# Bind("EnquiryDetails") %>'
                        FriendlyName="Enquiry Details" Size="20">
                        <Validators>
                            <i:Value MaxLength="50" ValueType="String" />
                        </Validators>
                    </i:TextBoxEx>
                    <eclipse:LeftLabel runat="server" Text="Remarks" />
                    <i:TextBoxEx runat="server" ID="tbRemarks" Text='<%# Bind("Remarks") %>' FriendlyName="Remarks"
                        Size="20">
                        <Validators>
                            <i:Value MaxLength="50" ValueType="String" />
                        </Validators>
                    </i:TextBoxEx>
                </eclipse:TwoColumnPanel>
                <i:ValidationSummary runat="server" />
            </EditItemTemplate>
        </asp:FormView>
        <i:ButtonEx runat="server" ID="btnGrantDetails" ClientIDMode="Static" Action="Submit"
            OnClick="btnGrantDetails_Click" Icon="Refresh" CausesValidation="true" />
        <jquery:StatusPanel runat="server" ID="GrantDetails_sp">
            <Ajax UseDialog="false" />
        </jquery:StatusPanel>
    </div>
    </form>
</body>
</html>
