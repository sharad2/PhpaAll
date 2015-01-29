<%@ Page Language="C#" CodeBehind="ActivityDetail.aspx.cs" Inherits="PhpaAll.MIS.AddNewActivity" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <jquery:JQueryScriptManager runat="server" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <phpa:PhpaLinqDataSource ID="ds" runat="server" ContextTypeName="Eclipse.PhpaLibrary.Database.MIS.MISDataContext"
            TableName="Activities" RenderLogVisible="false" EnableInsert="true" OnInserted="ds_Inserted"
            Where="ActivityId == @ActivityId" EnableUpdate="true" OnUpdated="ds_Updated">
            <WhereParameters>
                <asp:QueryStringParameter QueryStringField="ActivityId" Type="Int32" Name="ActivityId" />
            </WhereParameters>
            <UpdateParameters>
                <asp:QueryStringParameter QueryStringField="ActivityId" Type="Int32" Name="ActivityId" />
                <asp:Parameter Name="Activitycode" Type="String" />
                <asp:Parameter Name="Description" Type="String" />
                <asp:Parameter Name="uom" Type="String" />
                <asp:Parameter Name="physicaltarget" Type="Decimal" />
                <asp:Parameter Name="financialtarget" Type="Decimal" />
                <asp:Parameter Name="ProgressFrequencyId" Type="Int32" />
                <asp:Parameter Name="ParentActivityID" Type="Int32" />
                <asp:Parameter Name="BoqRate" Type="Decimal" />
                <asp:Parameter Name="ProvisionalRate" Type="Decimal" />
                <asp:Parameter Name="FinalRate" Type="Decimal" />
                <asp:Parameter Name="ProgressFormatId" Type="Int32" />
            </UpdateParameters>
            <InsertParameters>
                <asp:QueryStringParameter QueryStringField="SubPackageId" Type="Int32" Name="SubPackageId" />
                <asp:Parameter Name="Activitycode" Type="String" />
                <asp:Parameter Name="Description" Type="String" />
                <asp:Parameter Name="uom" Type="String" />
                <asp:Parameter Name="physicaltarget" Type="Decimal" />
                <asp:Parameter Name="financialtarget" Type="Decimal" />
                <asp:QueryStringParameter QueryStringField="ProgressFrequencyId" Type="Int32" Name="ProgressFrequencyId" />
                <asp:Parameter Name="ParentActivityID" Type="Int32" />
                <asp:Parameter Name="BoqRate" Type="Decimal" />
                <asp:Parameter Name="ProvisionalRate" Type="Decimal" />
                <asp:Parameter Name="FinalRate" Type="Decimal" />
                <asp:Parameter Name="ProgressFormatId" Type="Int32" />
            </InsertParameters>
        </phpa:PhpaLinqDataSource>
        <asp:FormView ID="fv" runat="server" DataSourceID="ds" DataKeyNames="ActivityId">
            <EditItemTemplate>
                <eclipse:TwoColumnPanel runat="server">
                    <eclipse:LeftLabel ID="LeftLabel1" runat="server" Text="Progress Format" />
                    <phpa:PhpaLinqDataSource ID="dsFormats" runat="server" ContextTypeName="Eclipse.PhpaLibrary.Database.MIS.MISDataContext"
                        TableName="ProgressFormats" RenderLogVisible="False" OrderBy="ProgressFormatName">
                    </phpa:PhpaLinqDataSource>
                    <i:DropDownListEx ID="ddlProgressformat" runat="server" DataSourceID="dsFormats"
                        DataTextField="ProgressFormatName" DataValueField="ProgressFormatId" FriendlyName="Progress Format"
                        Value='<%# Bind("ProgressFormatId") %>' QueryString="ProgressFormatId">
                        <Items>
                            <eclipse:DropDownItem Text="(No formats available)" Persistent="WhenEmpty" />
                        </Items>
                        <Validators>
                            <i:Required />
                        </Validators>
                    </i:DropDownListEx>
                    <eclipse:LeftLabel runat="server" Text="Item No." />
                    <i:TextBoxEx ID="tbItemno" runat="server" Text='<%#Bind("Activitycode") %>' FriendlyName="Item No"
                        Size="10">
                        <Validators>
                            <i:Required />
                            <i:Value ValueType="String" MaxLength="25" />
                        </Validators>
                    </i:TextBoxEx>
                    <br />
                    Example: 1,1.1,2.1,2.2
                    <eclipse:LeftLabel runat="server" Text="Description of work" />
                    <i:TextArea ID="tbDescription" runat="server" Text='<%#Bind("Description") %>' FriendlyName="Description"
                        Cols="30" Rows="4">
                        <Validators>
                            <i:Required />
                        </Validators>
                    </i:TextArea>
                    <eclipse:LeftLabel runat="server" Text="Unit" />
                    <i:TextBoxEx ID="tbUnit" runat="server" Text='<%#Bind("uom") %>' FriendlyName="Unit"
                        MaxLength="10">
                        <Validators>
                            <i:Value ValueType="String" MaxLength="20" />
                        </Validators>
                    </i:TextBoxEx>
                    e.g. MT, m3, RM, Per Hole
                    <eclipse:LeftLabel runat="server" Text="Quantity - BOQ" />
                    <i:TextBoxEx ID="tbBOQ" runat="server" Text='<%#Bind("physicaltarget") %>' FriendlyName="BOQ">
                        <Validators>
                            <i:Value ValueType="Decimal" />
                        </Validators>
                    </i:TextBoxEx>
                    <%--<eclipse:LeftLabel runat="server" Text="Quantity - Agreement" />
                    <i:TextBoxEx ID="tbAgreement" runat="server" Text='<%#Bind("financialtarget") %>'
                        FriendlyName="As per Agreement">
                        <Validators>
                            <i:Value ValueType="Decimal" />
                        </Validators>
                    </i:TextBoxEx>--%>
                    <%--                    <eclipse:LeftLabel runat="server" Text="Parent Activity" />
                    <phpa:PhpaLinqDataSource ID="dsActivity" runat="server" ContextTypeName="Eclipse.PhpaLibrary.Database.MIS.MISDataContext"
                        TableName="Activities" RenderLogVisible="False" AutoGenerateWhereClause="true">
                        <WhereParameters>
                            <asp:QueryStringParameter QueryStringField="ProgressFormatId" Name="ProgressFormatId"
                                Type="Int32" />
                        </WhereParameters>
                        <SelectParameters>
                            <asp:Parameter Name="ActivityID" Type="Int32" />
                        </SelectParameters>
                    </phpa:PhpaLinqDataSource>
                    <i:DropDownListEx ID="ddlActivity" runat="server" DataSourceID="dsActivity" DataTextField="DisplayName"
                        DataValueField="ActivityID" FriendlyName="Activity" Value='<%# Eval("ParentActivityId") %>'>
                        <Items>
                            <eclipse:DropDownItem Text="Not Set" Value="" Persistent="Always" />
                        </Items>
                    </i:DropDownListEx>--%>
                    <eclipse:LeftLabel runat="server" Text="BOQ Rate" />
                    <i:TextBoxEx ID="tbAsRate" runat="server" FriendlyName="As per BOQ Rate" Text='<%#Bind("BoqRate") %>'>
                        <Validators>
                            <i:Value ValueType="Decimal" MaxLength="15" />
                        </Validators>
                    </i:TextBoxEx>
                    <eclipse:LeftLabel runat="server" Text="Provisional Rate" />
                    <i:TextBoxEx ID="tbApprovedrate" runat="server" FriendlyName="Probation Rate" Text='<%#Bind("ProvisionalRate") %>'>
                        <Validators>
                            <i:Value ValueType="Decimal" MaxLength="15" />
                        </Validators>
                    </i:TextBoxEx>
                    <eclipse:LeftLabel runat="server" Text="Final Rate" />
                    <i:TextBoxEx ID="tbFinalrate" runat="server" FriendlyName="Final Rate" Text='<%#Bind("FinalRate") %>'>
                        <Validators>
                            <i:Value ValueType="Decimal" MaxLength="15" />
                        </Validators>
                    </i:TextBoxEx>
                    <br />
                    <i:ButtonEx ID="btn" runat="server" Action="Submit" CausesValidation="true" Text="Update Now"
                        OnClick="btn_Click" ClientIDMode="Static" />
                    <i:ValidationSummary runat="server" />
                </eclipse:TwoColumnPanel>
            </EditItemTemplate>
        </asp:FormView>
        <jquery:StatusPanel ID="sp_Status" runat="server">
            <Ajax UseDialog="false" />
        </jquery:StatusPanel>
    </div>
    </form>
</body>
</html>
