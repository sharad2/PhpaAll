<%@ Page Title="Abstract Financial Progress" Language="C#" MasterPageFile="~/MIS/NestedMIS.master"
    CodeBehind="AbstractFinancialProgress.aspx.cs" Inherits="Finance.MIS.AbstractFinancialProgress"
    EnableViewState="false" %>

<%@ Register Src="../Controls/PrinterFriendlyButton.ascx" TagName="PrinterFriendlyButton"
    TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphSideNavigation" runat="server">
    <uc2:PrinterFriendlyButton ID="pfb" runat="server" />
</asp:Content>
<asp:Content ID="c4" ContentPlaceHolderID="cph" runat="server">
    <eclipse:TwoColumnPanel runat="server">
        <eclipse:LeftLabel runat="server" Text="Select Month" />
        <i:TextBoxEx ID="tbMonth" runat="server" FriendlyName="Month" Text="0">
            <Validators>
                <i:Date />
            </Validators>
        </i:TextBoxEx>
        <i:ButtonEx ID="btnSubmit" runat="server" Text="Go" Action="Submit" Icon="Refresh" CausesValidation="true" IsDefault="true"/>
        <i:ValidationSummary runat="server" />
    </eclipse:TwoColumnPanel>
    <br />
    <phpa:PhpaLinqDataSource ID="ds" runat="server" RenderLogVisible="false" ContextTypeName="Eclipse.PhpaLibrary.Database.MIS.MISDataContext"
        TableName="FormatCategories" OnSelecting="ds_Selecting">
    </phpa:PhpaLinqDataSource>
    <jquery:GridViewEx ID="gv" runat="server" DataSourceID="ds" AutoGenerateColumns="False"
        ShowFooter="true">
        <Columns>
            <eclipse:SequenceField HeaderText="Sl.no" />
            <asp:HyperLinkField HeaderText="Component" DataTextField="Description" DataNavigateUrlFields="FormatCategoryId,ToDate"
                DataNavigateUrlFormatString="AbstractFinancialDetail.aspx?FormatCategoryId={0}&ToDate={1:d}" />
            <eclipse:MultiBoundField HeaderText="Payment made in Rs/Nu|As per agreement" DataFields="FinancialTarget"
                DataSummaryCalculation="ValueSummation" DataFormatString="{0:N2}">
                    <FooterStyle HorizontalAlign="Right" />
                    <ItemStyle HorizontalAlign = "Right" />
                </eclipse:MultiBoundField>
            <eclipse:MultiBoundField HeaderText="Payment made in Rs/Nu|Expenditure upto previous month"
                DataFields="PreMonthData" DataFormatString="{0:N2}" DataSummaryCalculation="ValueSummation" >
                    <FooterStyle HorizontalAlign="Right" />
                    <ItemStyle HorizontalAlign = "Right" />
                </eclipse:MultiBoundField>
            <eclipse:MultiBoundField HeaderText="Payment made in Rs/Nu|Expenditure during the month"
                DataFields="currentMonthData" DataFormatString="{0:N2}" DataSummaryCalculation="ValueSummation" >
                    <FooterStyle HorizontalAlign="Right" />
                    <ItemStyle HorizontalAlign = "Right" />
                </eclipse:MultiBoundField>
            <eclipse:MultiBoundField HeaderText="Payment made in Rs/Nu|Total expenditure" DataFields="TotalExpenditure"
                DataFormatString="{0:N2}" DataSummaryCalculation="ValueSummation" >
                    <FooterStyle HorizontalAlign="Right" />
                    <ItemStyle HorizontalAlign = "Right" />
                </eclipse:MultiBoundField>
            <asp:TemplateField HeaderText="Remarks">
                <ItemTemplate>
                    <i:TextArea runat="server">
                    </i:TextArea>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </jquery:GridViewEx>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cphNoForm" runat="server">
</asp:Content>
