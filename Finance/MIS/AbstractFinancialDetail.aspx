<%@ Page Title="Abstract Financial Details" Language="C#" MasterPageFile="~/MIS/NestedMIS.master"
    CodeBehind="AbstractFinancialDetail.aspx.cs" Inherits="PhpaAll.MIS.AbstractFinancialDetail" %>

<%@ Register Src="../Controls/PrinterFriendlyButton.ascx" TagName="PrinterFriendlyButton"
    TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphSideNavigation" runat="server">
    <uc2:PrinterFriendlyButton ID="pfb" runat="server" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cph" runat="server">
    <eclipse:TwoColumnPanel runat="server">
        <eclipse:LeftLabel runat="server" Text="Activity Category" />
        <phpa:PhpaLinqDataSource ID="dsActivityCategory" runat="server" ContextTypeName="Eclipse.PhpaLibrary.Database.MIS.MISDataContext"
            TableName="FormatCategories" RenderLogVisible="false">
        </phpa:PhpaLinqDataSource>
        <i:DropDownListEx ID="ddlActivityCategory" runat="server" DataSourceID="dsActivityCategory"
            DataTextField="Description" DataValueField="FormatCategoryId" FriendlyName="Format Category"
            QueryString="FormatCategoryId">
            <Validators>
                <i:Required />
            </Validators>
        </i:DropDownListEx>
        <eclipse:LeftLabel ID="LeftLabel1" runat="server" Text="Select Month" />
        <i:TextBoxEx ID="tbMonth" runat="server" FriendlyName="Month" Text="0" QueryString="ToDate">
            <Validators>
                <i:Date />
                <i:Required />
            </Validators>
        </i:TextBoxEx>
        <i:ButtonEx runat="server" Text="Go" Action="Submit" Icon="Refresh" CausesValidation="true" IsDefault="true"/>
        <i:ValidationSummary runat="server" />
    </eclipse:TwoColumnPanel>
    <br />
    <phpa:PhpaLinqDataSource ID="ds" runat="server" RenderLogVisible="false" ContextTypeName="Eclipse.PhpaLibrary.Database.MIS.MISDataContext"
        TableName="ActivityCategories" OnSelecting="ds_Selecting">
    </phpa:PhpaLinqDataSource>
    <jquery:GridViewEx ID="gv" runat="server" AutoGenerateColumns="false" DataSourceID="ds"
        ShowFooter="true">
        <Columns>
            <asp:BoundField HeaderText="Sl. no" DataField="SubPackageName" ItemStyle-HorizontalAlign="Center"
                ItemStyle-Wrap="false" />
            <asp:BoundField HeaderText="Description" DataField="Description" ItemStyle-Wrap="false" />
            <eclipse:MultiBoundField HeaderText="Payment made in Rs/Nu|As per agreement" DataFields="FinancialTarget"
                ItemStyle-HorizontalAlign="Right" DataFooterFormatString="{0:N2}" DataSummaryCalculation="ValueSummation"
                FooterStyle-HorizontalAlign="Right" DataFormatString="{0:N2}">
                <FooterStyle HorizontalAlign="Right" />
            </eclipse:MultiBoundField>
            <eclipse:MultiBoundField HeaderText="Payment made in Rs/Nu|Expenditure upto previous month"
                DataFields="preMonthData" DataFormatString="{0:N2}" ItemStyle-HorizontalAlign="Right"
                DataSummaryCalculation="ValueSummation">
                <FooterStyle HorizontalAlign="Right" />
            </eclipse:MultiBoundField>
            <eclipse:MultiBoundField HeaderText="Payment made in Rs/Nu|Expenditure during the month"
                DataFields="currentMonthData" DataFormatString="{0:N2}" ItemStyle-HorizontalAlign="Right"
                DataSummaryCalculation="ValueSummation">
                <FooterStyle HorizontalAlign="Right" />
            </eclipse:MultiBoundField>
            <eclipse:MultiBoundField HeaderText="Payment made in Rs/Nu|Total expenditure" DataFields="TotalExpenditure"
                DataFormatString="{0:N2}" ItemStyle-HorizontalAlign="Right" DataSummaryCalculation="ValueSummation">
                <FooterStyle HorizontalAlign="Right" />
            </eclipse:MultiBoundField>
            <asp:TemplateField HeaderText="Remarks">
                <ItemTemplate>
                    <i:TextArea runat="server">
                        <Validators>
                            <i:Value ValueType="String" />
                        </Validators>
                    </i:TextArea>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </jquery:GridViewEx>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cphNoForm" runat="server">
</asp:Content>
