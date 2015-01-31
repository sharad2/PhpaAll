<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" CodeBehind="Exp_VariousHeads.aspx.cs"
    Inherits="PhpaAll.Reports.Exp_VariousHeads" Title="Job Expenditure in Various Heads"
    EnableViewState="false" %>

<%@ Register Src="../Controls/PrinterFriendlyButton.ascx" TagName="PrinterFriendlyButton"
    TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphSideNavigation" runat="server">
    <asp:HyperLink ID="HyperLink2" runat="server" Text="Help" NavigateUrl="~/Doc/Exp_VariousHeads.doc.aspx" /><br />
    <uc2:PrinterFriendlyButton ID="PrinterFriendlyButton1" runat="server" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cph" runat="server">
    <br />
    <div style="float: left">
        <phpa:PhpaLinqDataSource ID="dsexp" runat="server" ContextTypeName="Eclipse.PhpaLibrary.Reporting.ReportingDataContext"
            TableName="ReportConfigurations" RenderLogVisible="false">
        </phpa:PhpaLinqDataSource>
        Displays expenditure against job for selected division upto current date.
        <br />
        <div class="ParamInstructions">
            Select the division for which Job Expenditure statement to be shown</div>
        <eclipse:TwoColumnPanel ID="Panel1" runat="server" IsValidationContainer="true">
            <eclipse:LeftLabel runat="server" Text="Division" />
            <phpa:PhpaLinqDataSource runat="server" ID="dsDivisions" ContextTypeName="Eclipse.PhpaLibrary.Reporting.ReportingDataContext"
                OrderBy="DivisionName" Select="new (DivisionId, DivisionName, DivisionGroup)"
                TableName="RoDivisions" Visible="True" RenderLogVisible="false">
            </phpa:PhpaLinqDataSource>
            <i:DropDownListEx runat="server" ID="ddlDivisionCode" DataSourceID="dsDivisions"
                DataTextField="DivisionName" DataValueField="DivisionId" DataOptionGroupField="DivisionGroup"
                ClientIDMode="Static">
                <Items>
                    <eclipse:DropDownItem Text="(Select Division)" Value="" Persistent="Always" />
                </Items>
                <Validators>
                    <i:Required />
                </Validators>
            </i:DropDownListEx>
            <br />
            <eclipse:LeftLabel ID="LeftLabel1" runat="server" Text="From Date" />
            <i:TextBoxEx ID="tbDate" runat="server" FriendlyName="From Date">   
                <Validators>
                    <i:Date />     
                    </Validators>   
        </i:TextBoxEx>
            To Date
             <i:TextBoxEx ID="txtToDate" runat="server" FriendlyName="To Date">   
                <Validators>
                    <i:Date />     
                    </Validators>   
        </i:TextBoxEx>
            <i:ButtonEx ID="btnGo" runat="server" Text="Go" Action="Submit" Icon="Refresh" CausesValidation="true"
                IsDefault="true" />
            <i:ValidationSummary ID="ValidationSummary1" runat="server" />
        </eclipse:TwoColumnPanel>
        <phpa:PhpaLinqDataSource ID="ds" runat="server" ContextTypeName="Eclipse.PhpaLibrary.Reporting.ReportingDataContext"
            RenderLogVisible="false" OnSelecting="ds_Selecting">
        </phpa:PhpaLinqDataSource>
        <jquery:GridViewEx ID="gv" runat="server" DataSourceID="ds" AutoGenerateColumns="false"
            DefaultSortExpression="JobCode,JobDescription;$;" PreSorted="true" ShowFooter="true"
            ShowExpandCollapseButtons="false">
            <Columns>
                <eclipse:SequenceField />
                <asp:TemplateField HeaderText="Jobs" SortExpression="JobCode,JobDescription">
                    <ItemTemplate>
                        <div style="margin-top: 3mm; margin-bottom: 3mm">
                            <strong>Job Code:</strong>
                            <%# Eval("JobCode")%><br />
                            <strong>Job Description:</strong>
                            <%# Eval("JobDescription")%>
                        </div>
                    </ItemTemplate>
                </asp:TemplateField>
                <%--<eclipse:MultiBoundField DataFields="JobCode" HeaderText="Job Code" SortExpression="JobCode" />
                <eclipse:MultiBoundField DataFields="JobDescription" HeaderText="Job Description"
                    SortExpression="JobDescription" />--%>
                <eclipse:MultiBoundField DataFields="DisplayName" HeaderText="Head Of Account|Display Name" />
                <eclipse:MultiBoundField DataFields="Description" HeaderText="Head Of Account|Description">
                    <ItemStyle Width="10em" />
                </eclipse:MultiBoundField>
                <eclipse:MultiBoundField DataFields="Expenditure" HeaderText="Financial Expenditure|Current "
                    DataSummaryCalculation="ValueSummation" DataFormatString="{0:C}" DataFooterFormatString="{0:C}">
                    <ItemStyle HorizontalAlign="Right" />
                </eclipse:MultiBoundField>
                <eclipse:MultiBoundField DataFields="ExpenditureOld" HeaderText="Financial Expenditure|Cummulative"
                    DataSummaryCalculation="ValueSummation" DataFormatString="{0:C}" DataFooterFormatString="{0:C}">
                    <ItemStyle HorizontalAlign="Right" />
                </eclipse:MultiBoundField>
            </Columns>
        </jquery:GridViewEx>
    </div>
</asp:Content>
