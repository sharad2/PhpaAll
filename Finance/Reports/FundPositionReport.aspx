﻿<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" CodeBehind="FundPositionReport.aspx.cs"
    Inherits="PhpaAll.Reports.FundPositionReport" Title="Fund Position Report" %>

<%@ Register Src="../Controls/PrinterFriendlyButton.ascx" TagName="PrinterFriendlyButton"
    TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server" EnableViewState="false">
    <style type="text/css">
        .MainTable {
            border-collapse: collapse;
            table-layout: auto;
            border: thin black solid;
        }

            .MainTable TD {
                border: 0.25mm black solid;
                padding: 2mm;
                font-size: 1.1em;
                font-family: Arial;
            }

            .MainTable TH {
                border: thin 0.5mm black solid;
            }

        .TableHeader {
            font-family: Arial;
            font-weight: bolder;
            padding-top: 3mm;
        }

        .RowHeader {
            font-size: 1.1em;
            font-family: Arial;
            /*text-indent: 5mm;*/
            font-weight: bolder;
            padding-top: 3mm;
            margin-top: 10mm;
            font-style: oblique;
            letter-spacing: 0.1mm;
        }

            .RowHeader td {
                border: solid 0.5mm black;
                border-collapse: collapse;
            }

            .RowHeader th {
                border: solid 0.5mm black;
                border-collapse: collapse;
            }

        .AlternatingRow {
            background-color: #F7F7F7;
            border: thin 0.25mm black solid;
        }

        .GridRow{
            border: thin 0.25mm black solid;
            height: 9mm;
            font-size: 1.1em;
        }
        .GridAlternatingRow {
            background-color: #F7F7F7;
            border: thin 0.25mm black solid;
            height: 9mm;
            font-size: 1.1em;
        }
        .lblExpenditureCurrentMonth {
            padding-left: 50mm;
        }
    </style>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphSideNavigation" runat="server">
    <asp:HyperLink ID="HyperLink2" runat="server" Text="Help" NavigateUrl="~/Doc/ReceiptandPayment.doc.aspx" /><br />
    <uc2:PrinterFriendlyButton ID="PrinterFriendlyButton1" runat="server" />
</asp:Content>
<asp:Content ID="c3" ContentPlaceHolderID="cph" runat="server">
    <phpa:PhpaLinqDataSource ID="dsQueries" runat="server" ContextTypeName="Eclipse.PhpaLibrary.Reporting.ReportingDataContext"
        TableName="ReportConfigurations" RenderLogVisible="false">
    </phpa:PhpaLinqDataSource>
    <eclipse:TwoColumnPanel ID="TwoColumnPanel1" runat="server">
        <eclipse:LeftLabel ID="LeftLabel1" runat="server" Text="Include transactions until" />
        <i:TextBoxEx ID="dttbreceiptpayment" runat="server" FriendlyName="Date">
            <Validators>
                <i:Required />
                <i:Date />
            </Validators>
        </i:TextBoxEx>
        <br />
        <i:ButtonEx runat="server" ID="btnGo" Text="Recalculate" CausesValidation="true"
            Action="Submit" IsDefault="true" />
    </eclipse:TwoColumnPanel>
    <i:ValidationSummary ID="ValidationSummary1" runat="server" />

    <asp:FormView runat="server" ItemType="PhpaAll.Reports.FundPositionReportData" SelectMethod="GetFundData">
        <ItemTemplate>
            <table rules="all" style="padding: 4mm; width: 1000px" class="MainTable">
                <caption style="text-align: right; font-weight: bold; font-size: 1.2em">All Figures in Million Rs/Nu</caption>
                <thead class="ui-state-default">
                    <tr class="RowHeader">
                        <th style="text-align: center; width: 50px" rowspan="3"><strong>I</strong></th>
                        <th style="text-align: center; width: 450px" rowspan="3" class="RowHeader">Receipts(1)
                        </th>
                        <th style="text-align: center; width: 150px" rowspan="3">Upto Previous Year(2)
                        </th>
                        <th style="text-align: center; width: 150px" rowspan="3">Current Year(3)
                        </th>
                        <th style="text-align: center; width: 200px" rowspan="3">Total Cumulative(4)
                        </th>
                    </tr>
                </thead>
                <tbody>
                    <tr>
                        <td></td>
                        <td>Funds Received From GOI (Grant) Rs./NU.
                        </td>
                        <td class="vd-amountcol">
                            <asp:HyperLink runat="server"
                                ToolTip="Click to list voucher entries for Funds Received from Government of India in local Currency prior to Financial Year"
                                NavigateUrl='<%# string.Format("~/Finance/VoucherSearch.aspx?AccountTypes={0}&DateTo={1:d}", Item.FundsReceivedGOIGrantNuHeads, Item.PreviousYearEndDate) %>'
                                EnableViewState="false" Text='<%# string.Format("{0:#,###,,.000;(#,###,,.000)}", Item.FundsReceivedGOIGrantNuUpToPrev) %>' />
                        </td>

                        <td class="vd-amountcol">
                            <asp:HyperLink runat="server" ToolTip="Click to list voucher entries for Funds Received from Government of India in local Currency from financial year start till input month"
                                EnableViewState="false"
                                NavigateUrl='<%# string.Format("~/Finance/VoucherSearch.aspx?AccountTypes={0}&DateFrom={1:d}&DateTo={2:d}", Item.FundsReceivedGOIGrantNuHeads, Item.DateFrom, Item.DateTo) %>'
                                Text='<%# string.Format("{0:#,###,,.000;(#,###,,.000)}", Item.FundsReceivedGOIGrantNuCurr) %>' />
                        </td>
                        <td class="vd-amountcol">
                            <asp:Literal runat="server" EnableViewState="false"
                                Text='<%# string.Format("{0:#,###,,.000;(#,###,,.000)}", Item.FundsReceivedGOIGrantNuCum) %>' />
                        </td>
                    </tr>
                    <tr class="AlternatingRow">
                        <td></td>
                        <td>Funds Received From GOI (Grant) F.E.
                        </td>
                        <td class="vd-amountcol">
                            <asp:HyperLink ID="hplnkGOIAidFEPreviousYear" runat="server" ToolTip="Click to list voucher entries for Funds Received from Government of India in local Currency prior to Financial Year"
                                NavigateUrl='<%# string.Format("~/Finance/VoucherSearch.aspx?AccountTypes={0}&DateTo={1:d}", Item.FundsReceivedGOIGrantFeHeads, Item.PreviousYearEndDate) %>'
                                EnableViewState="false" Text='<%# string.Format("{0:#,###,,.000;(#,###,,.000)}", Item.FundsReceivedGOIGrantFeUpToPrev) %>' />
                        </td>

                        <td class="vd-amountcol">
                            <asp:HyperLink ID="hplnkGOIAidFEUptoMonth" runat="server" ToolTip="Click to list voucher entries for Funds Received from Government of India in local Currency from financial year start till input month"
                                EnableViewState="false"
                                NavigateUrl='<%# string.Format("~/Finance/VoucherSearch.aspx?AccountTypes={0}&DateFrom={1:d}&DateTo={2:d}", Item.FundsReceivedGOIGrantFeHeads, Item.DateFrom, Item.DateTo) %>'
                                Text='<%# string.Format("{0:#,###,,.000;(#,###,,.000)}", Item.FundsReceivedGOIGrantFeCurr) %>' />
                        </td>
                        <td class="vd-amountcol">
                            <asp:Label ID="lblGOIAidFEsum" runat="server" ToolTip="Funds Received from Government of India in local currency till date" EnableViewState="false"
                                Text='<%# string.Format("{0:#,###,,.000;(#,###,,.000)}", Item.FundsReceivedGOIGrantFeCum) %>' />
                        </td>
                    </tr>
                    <tr>
                        <td></td>
                        <td>Funds Received From GOI (Loan) Rs. / Nu.
                        </td>
                        <td class="vd-amountcol">
                            <asp:HyperLink ID="hplnkGOILoanPreviousYear" runat="server" ToolTip="Click to list voucher entries for Funds Received from Government of India in local Currency prior to Financial Year"
                                NavigateUrl='<%# string.Format("~/Finance/VoucherSearch.aspx?AccountTypes={0}&DateTo={1:d}", Item.FundsReceivedGOILoanNuHeads, Item.PreviousYearEndDate) %>'
                                EnableViewState="false" Text='<%# string.Format("{0:#,###,,.000;(#,###,,.000)}", Item.FundsReceivedGOILoanNuUpToPrev) %>' />
                        </td>

                        <td class="vd-amountcol">
                            <asp:HyperLink ID="hplnkGOILoanUptoMonth" runat="server" ToolTip="Click to list voucher entries for Funds Received from Government of India in local Currency from financial year start till input month"
                                EnableViewState="false" NavigateUrl='<%# string.Format("~/Finance/VoucherSearch.aspx?AccountTypes={0}&DateFrom={1:d}&DateTo={2:d}", Item.FundsReceivedGOILoanNuHeads, Item.DateFrom, Item.DateTo) %>'
                                Text='<%# string.Format("{0:#,###,,.000;(#,###,,.000)}", Item.FundsReceivedGOILoanNuCurr) %>' />
                        </td>
                        <td class="vd-amountcol">
                            <asp:Label ID="lblGOILoansum" runat="server" ToolTip="Funds Received from Government of India in local currency till date" EnableViewState="false"
                                Text='<%# string.Format("{0:#,###,,.000;(#,###,,.000)}", Item.FundsReceivedGOILoanNuCum) %>' />
                        </td>
                    </tr>
                    <tr class="AlternatingRow">
                        <td></td>
                        <td>Funds Received From GOI (Loan) F.E.
                        </td>
                        <td class="vd-amountcol">

                            <asp:HyperLink ID="hplnkGOILoanFEPreviousYear" runat="server" ToolTip="Click to list voucher entries for Funds Received from Government of India in Foreign Exchange prior to Financial Year"
                                EnableViewState="false"
                                NavigateUrl='<%# string.Format("~/Finance/VoucherSearch.aspx?AccountTypes={0}&DateTo={1:d}", Item.FundsReceivedGOILoanFeHeads, Item.PreviousYearEndDate) %>'
                                Text='<%# string.Format("{0:#,###,,.000;(#,###,,.000)}", Item.FundsReceivedGOILoanFeUpToPrev) %>' />
                        </td>

                        <td class="vd-amountcol">

                            <asp:HyperLink ID="hplnkGOILoanFEUptoMonth" runat="server" ToolTip="Click to list voucher entries for Funds Received from Government of India in Foreign Exchange from financial year start till input month"
                                EnableViewState="false" NavigateUrl='<%# string.Format("~/Finance/VoucherSearch.aspx?AccountTypes={0}&DateFrom={1:d}&DateTo={2:d}", Item.FundsReceivedGOILoanFeHeads, Item.DateFrom, Item.DateTo) %>'
                                Text='<%# string.Format("{0:#,###,,.000;(#,###,,.000)}", Item.FundsReceivedGOILoanFeCurr) %>' />
                        </td>
                        <td class="vd-amountcol">
                            <asp:Label ID="lblGOILoanFEsum" runat="server" ToolTip="Funds received from Government of India in Foreign Exchange till date"
                                EnableViewState="false"
                                Text='<%# string.Format("{0:#,###,,.000;(#,###,,.000)}", Item.FundsReceivedGOILoanFeCum) %>' />
                        </td>
                    </tr>
                    <tr class="RowHeader">
                        <td style="text-align: center"><strong>II</strong></td>
                        <td style="text-align: right">Total Fund GOI
                        </td>
                        <td class="vd-amountcol">
                            <asp:Label ID="lblFundReceivedPreviousYear" runat="server" EnableViewState="false"
                                Text='<%# string.Format("{0:#,###,,.000;(#,###,,.000)}", Item.FundReceivedGOITotalUpToPrev) %>' />
                        </td>

                        <td class="vd-amountcol">
                            <asp:Label ID="lblFundRecievedUpToMonth" runat="server" EnableViewState="false"
                                Text='<%# string.Format("{0:#,###,,.000;(#,###,,.000)}", Item.FundReceivedGOITotalCurr) %>' />
                        </td>
                        <td class="vd-amountcol">
                            <asp:Label ID="lblFundRecievedSum" runat="server" EnableViewState="false"
                                Text='<%# string.Format("{0:#,###,,.000;(#,###,,.000)}", Item.FundReceivedGOITotalCum) %>' />
                        </td>
                    </tr>
                    <tr class="AlternatingRow">
                        <td style="text-align: center; font-style: oblique"><strong>III</strong></td>
                        <td title="TOTAL FUND GOI">Other Receipts:Ineterst,Sale of Tender,EMD,SD,other receipts etc.
                        </td>
                        <td class="vd-amountcol">

                            <asp:HyperLink ID="hplnkReceiptsPreviousYear" runat="server" ToolTip="Click to list voucher entries for Interest received prior to Financial Year"
                                EnableViewState="false"
                                NavigateUrl='<%# string.Format("~/Finance/VoucherSearch.aspx?AccountTypes={0}&DateTo={1:d}", Item.FundReceivedOtherHeads, Item.PreviousYearEndDate) %>'
                                Text='<%# string.Format("{0:#,###,,.000;(#,###,,.000)}", Item.FundReceivedOtherUpToPrev) %>' />
                        </td>

                        <td class="vd-amountcol">

                            <asp:HyperLink ID="hplnkReceiptsUptoMonth" runat="server" ToolTip="Click to list voucher entries for Interest received from financial year start till input month"
                                EnableViewState="false" NavigateUrl='<%# string.Format("~/Finance/VoucherSearch.aspx?AccountTypes={0}&DateFrom={1:d}&DateTo={2:d}", Item.FundReceivedOtherHeads, Item.DateFrom, Item.DateTo) %>'
                                Text='<%# string.Format("{0:#,###,,.000;(#,###,,.000)}", Item.FundReceivedOtherCurr) %>' />
                        </td>
                        <td class="vd-amountcol">
                            <asp:Label ID="lblReceiptssum" runat="server" ToolTip="Interest received till date"
                                EnableViewState="false"
                                Text='<%# string.Format("{0:#,###,,.000;(#,###,,.000)}", Item.FundReceivedOtherCum) %>' />
                        </td>
                    </tr>
                    <tr class="RowHeader">
                        <td style="text-align: center"><strong>IV</strong></td>
                        <td style="text-align: right">Total(II+III)
                        </td>
                        <td class="vd-amountcol">
                            <asp:Label ID="lblReceiptsPreviousYear" runat="server" EnableViewState="false"
                                Text='<%# string.Format("{0:#,###,,.000;(#,###,,.000)}", Item.FundReceivedOtherAndGOITotalUpToPrev) %>' />
                        </td>

                        <td class="vd-amountcol">
                            <asp:Label ID="lblReceiptsUptoMonth" runat="server" EnableViewState="false"
                                Text='<%# string.Format("{0:#,###,,.000;(#,###,,.000)}", Item.FundReceivedOtherAndGOITotalCurr) %>' />
                        </td>
                        <td class="vd-amountcol">
                            <asp:Label ID="Label1" runat="server" EnableViewState="false"
                                Text='<%# string.Format("{0:#,###,,.000;(#,###,,.000)}", Item.FundReceivedOtherAndGOITotalCum) %>' />
                        </td>
                    </tr>
                    <tr class="AlternatingRow">
                        <td style="text-align: center"><strong>V</strong></td>
                        <td style="text-align: right">Expenditure
                        </td>
                        <td class="vd-amountcol">
                            <asp:Label ID="lblExpenditurePreviousYear" runat="server" EnableViewState="false"
                                Text='<%# string.Format("{0:#,###,,.000;(#,###,,.000)}", Item.ExpendituresUpToPrev) %>' />
                        </td>
                        <td class="vd-amountcol">
                            <asp:Label ID="lblExpenditureUpToMonth" runat="server" EnableViewState="false"
                                Text='<%# string.Format("{0:#,###,,.000;(#,###,,.000)}", Item.ExpendituresCurr) %>' />
                        </td>
                        <td class="vd-amountcol">
                            <asp:Label ID="lblTotalExpenditure" runat="server" EnableViewState="false"
                                Text='<%# string.Format("{0:#,###,,.000;(#,###,,.000)}", Item.ExpendituresCum) %>' />
                        </td>
                    </tr>
                    <tr class="RowHeader ui-state-active ui-widget-header">
                        <td style="text-align: center"><strong>VI</strong></td>
                        <td style="text-align: right"><b>BALANCE FUND(IV-V)</b></td>
                        <td class="vd-amountcol">
                            <asp:Label ID="lblbalancefundprevious" runat="server" EnableViewState="False" Text='<%# string.Format("{0:#,###,,.000;(#,###,,.000)}", Item.BalanceFundUpToPrev) %>' />
                        </td>
                        <td></td>
                        <td class="vd-amountcol">
                            <asp:Label ID="lblbalancefundcumulative" runat="server" EnableViewState="False"
                                Text='<%# string.Format("{0:#,###,,.000;(#,###,,.000)}", Item.BalanceFundCum) %>' /></td>
                    </tr>
                </tbody>
            </table>

            <br />
            <br />
            <br />
            <span style="font-weight: bold;">VII Detail of Expenditure</span>
            <table rules="all" style="padding: 4mm; width: 1000px" class="MainTable">
                <thead class="ui-state-default">
                    <tr class="RowHeader">
                        <th style="text-align: center; width: 50px" rowspan="2"><strong></strong></th>
                        <th style="text-align: center; width: 450px" rowspan="2" class="RowHeader"></th>
                        <th style="text-align: center; width: 150px" rowspan="2">Upto Previous Year(2)</th>
                        <th style="text-align: center; width: 150px" rowspan="2">Current Year(3)</th>
                        <th style="text-align: center; width: 200px" rowspan="2">Total Cumulative(4)</th>
                    </tr>
                </thead>
                <tbody>
                    <tr>
                        <td style="text-align: right"><strong>a)</strong></td>
                        <td>Establishment
                        </td>

                        <td class="vd-amountcol">

                            <asp:HyperLink ID="hplnkEstablishExpenditurePreviousYear" runat="server" ToolTip="Click to list voucher entries for expenditure for input month"
                                EnableViewState="false" NavigateUrl='<%# string.Format("~/Finance/VoucherSearch.aspx?AccountTypes={0}&DateTo={1:d}", Item.EstablishmentExpendituresHead, Item.PreviousYearEndDate) %>'
                                Text='<%# string.Format("{0:#,###,,.000;(#,###,,.000)}", Item.EstablishmentExpendituresUpToPrev) %>' />
                        </td>
                        <td class="vd-amountcol">

                            <asp:HyperLink ID="hplnkEstablishExpenditureUptoMonth" runat="server" ToolTip="Click to list voucher entries for expenditure from financial year start till input month"
                                EnableViewState="false" NavigateUrl='<%# string.Format("~/Finance/VoucherSearch.aspx?AccountTypes={0}&DateFrom={1:d}&DateTo={2:d}", Item.EstablishmentExpendituresHead, Item.DateFrom, Item.DateTo) %>'
                                Text='<%# string.Format("{0:#,###,,.000;(#,###,,.000)}", Item.EstablishmentExpendituresCurr) %>' />
                        </td>
                        <td class="vd-amountcol">
                            <asp:Label ID="lblEstablishExpendituresum" runat="server" ToolTip="Expenditure incured till date"
                                EnableViewState="false" Text='<%# string.Format("{0:#,###,,.000;(#,###,,.000)}", Item.EstablishmentExpendituresCum) %>' />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right"><strong>a)</strong></td>
                        <td>WAPCOS
                        </td>

                        <td class="vd-amountcol">

                            <asp:HyperLink ID="HyperLink1" runat="server" ToolTip="Click to list voucher entries for expenditure for input month"
                                EnableViewState="false" NavigateUrl='<%# string.Format("~/Finance/VoucherSearch.aspx?AccountTypes={0}&DateTo={1:d}", Item.WapcosExpendituresHead, Item.PreviousYearEndDate) %>'
                                Text='<%# string.Format("{0:#,###,,.000;(#,###,,.000)}", Item.WapcosExpendituresUpToPrev) %>' />
                        </td>
                        <td class="vd-amountcol">

                            <asp:HyperLink ID="HyperLink3" runat="server" ToolTip="Click to list voucher entries for expenditure from financial year start till input month"
                                EnableViewState="false" NavigateUrl='<%# string.Format("~/Finance/VoucherSearch.aspx?AccountTypes={0}&DateFrom={1:d}&DateTo={2:d}", Item.WapcosExpendituresHead, Item.DateFrom, Item.DateTo) %>'
                                Text='<%# string.Format("{0:#,###,,.000;(#,###,,.000)}", Item.WapcosExpendituresCurr) %>' />
                        </td>
                        <td class="vd-amountcol">
                            <asp:Label ID="Label5" runat="server" ToolTip="Expenditure incured till date"
                                EnableViewState="false" Text='<%# string.Format("{0:#,###,,.000;(#,###,,.000)}", Item.WapcosExpendituresCum) %>' />
                        </td>
                    </tr>
                    <tr class="AlternatingRow">
                        <td style="text-align: right"><strong>b)</strong></td>
                        <td>Civil Works
                        </td>
                        <td class="vd-amountcol">

                            <asp:HyperLink ID="hplnkCivilExpenditurePreviousYear" runat="server" ToolTip="Click to list voucher entries for expenditure for input month"
                                EnableViewState="false" NavigateUrl='<%# string.Format("~/Finance/VoucherSearch.aspx?AccountTypes={0}&DateTo={1:d}", Item.CivilWorkExpendituresHead, Item.PreviousYearEndDate) %>'
                                Text='<%# string.Format("{0:#,###,,.000;(#,###,,.000)}", Item.CivilWorkExpendituresUpToPrev) %>' />
                        </td>
                        <td class="vd-amountcol">

                            <asp:HyperLink ID="hplnkCivilExpenditureUpToMonth" runat="server" ToolTip="Click to list voucher entries for expenditure from financial year start till input month"
                                EnableViewState="false" NavigateUrl='<%# string.Format("~/Finance/VoucherSearch.aspx?AccountTypes={0}&DateFrom={1:d}&DateTo={2:d}", Item.CivilWorkExpendituresHead, Item.DateFrom, Item.DateTo) %>'
                                Text='<%# string.Format("{0:#,###,,.000;(#,###,,.000)}", Item.CivilWorkExpendituresCurr) %>' />
                        </td>
                        <td class="vd-amountcol">
                            <asp:Label ID="lblCivilExpendituresum" runat="server" ToolTip="Expenditure incured till date"
                                EnableViewState="false"
                                Text='<%# string.Format("{0:#,###,,.000;(#,###,,.000)}", Item.CivilWorkExpendituresCum) %>' />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right"><strong>c)</strong></td>
                        <td>Electrical
                        </td>
                        <td class="vd-amountcol">

                            <asp:HyperLink ID="hplnkElectricalExpenditurePreviousYear" runat="server" ToolTip="Click to list voucher entries for employee advances prior to Financial Year"
                                EnableViewState="false" NavigateUrl='<%# string.Format("~/Finance/VoucherSearch.aspx?AccountTypes={0}&DateTo={1:d}", Item.ElectricalExpendituresHead, Item.PreviousYearEndDate) %>'
                                Text='<%# string.Format("{0:#,###,,.000;(#,###,,.000)}", Item.ElectricalExpendituresUpToPrev) %>' />
                        </td>
                        <td class="vd-amountcol">

                            <asp:HyperLink ID="hplnkElectricalExpenditureUpToMonth" runat="server" ToolTip="Click to list voucher entries for employee advances from financial year start till input month"
                                EnableViewState="false" NavigateUrl='<%# string.Format("~/Finance/VoucherSearch.aspx?AccountTypes={0}&DateFrom={1:d}&DateTo={2:d}", Item.ElectricalExpendituresHead, Item.DateFrom, Item.DateTo) %>'
                                Text='<%# string.Format("{0:#,###,,.000;(#,###,,.000)}", Item.ElectricalExpendituresCurr) %>' />
                        </td>
                        <td class="vd-amountcol">
                            <asp:Label ID="lblElectricalExpendituresum" runat="server" ToolTip="Advances to Employees till date"
                                EnableViewState="false"
                                Text='<%# string.Format("{0:#,###,,.000;(#,###,,.000)}", Item.ElectricalExpendituresCum) %>' />
                        </td>
                    </tr>
                    <tr class="AlternatingRow">
                        <td style="text-align: right"><strong>d)</strong></td>
                        <td>Transmission
                        </td>
                        <td class="vd-amountcol">

                            <asp:HyperLink ID="hplnkTransmissionExpenditurePreviousYear" runat="server" ToolTip="Click to list voucher entries for party and material advances prior to Financial Year"
                                NavigateUrl='<%# string.Format("~/Finance/VoucherSearch.aspx?AccountTypes={0}&DateTo={1:d}", Item.TransmissionExpendituresHead, Item.PreviousYearEndDate) %>'
                                EnableViewState="false"
                                Text='<%# string.Format("{0:#,###,,.000;(#,###,,.000)}", Item.TransmissionExpendituresUpToPrev) %>' />
                        </td>

                        <td class="vd-amountcol">

                            <asp:HyperLink ID="hplnkTransmissionExpenditureUpToMonth" runat="server" ToolTip="Click to list voucher entries for party and material advances from financial year start till input month"
                                NavigateUrl='<%# string.Format("~/Finance/VoucherSearch.aspx?AccountTypes={0}&DateFrom={1:d}&DateTo={2:d}", Item.ElectricalExpendituresHead, Item.DateFrom, Item.DateTo) %>'
                                EnableViewState="false"
                                Text='<%# string.Format("{0:#,###,,.000;(#,###,,.000)}", Item.TransmissionExpendituresCurr) %>' />
                        </td>
                        <td class="vd-amountcol" colspan="2">
                            <asp:Label ID="lblTransmissionExpendituresum" runat="server" ToolTip="Advances to parties for material till date"
                                EnableViewState="false"
                                Text='<%# string.Format("{0:#,###,,.000;(#,###,,.000)}", Item.TransmissionExpendituresCum) %>' />
                        </td>
                    </tr>

                    <tr class="RowHeader ui-state-active ui-widget-header">
                        <td style="text-align: center"></td>
                        <td style="text-align: right"><b>TOTAL PAYMENTS</b></td>
                        <td class="vd-amountcol">
                            <asp:Label ID="lblPaymentsPreviousYear" runat="server" EnableViewState="false" Text='<%# string.Format("{0:#,###,,.000;(#,###,,.000)}", Item.ExpendituresUpToPrev) %>' />
                        </td>

                        <td class="vd-amountcol">
                            <asp:Label ID="lblPaymentsumUptoMonth" runat="server" EnableViewState="false" Text='<%# string.Format("{0:#,###,,.000;(#,###,,.000)}", Item.ExpendituresCurr) %>' />
                        </td>
                        <td class="vd-amountcol">
                            <asp:Label ID="lblPaymentsum" runat="server" EnableViewState="false" />
                            <asp:Label ID="lblPaymentForMonthSum" runat="server" EnableViewState="false" Text='<%# string.Format("{0:#,###,,.000;(#,###,,.000)}", Item.ExpendituresCum) %>' />
                        </td>
                    </tr>
                </tbody>
            </table>
            <br />
            <br />
            <h2 runat="server">Expenditure for the month of <%# string.Format("{0:MMMMM, yyyy}", Item.DateTo) %> (till date):
                <%# string.Format("{0:#,###,,.000;(#,###,,.000)}", Item.CurrentMonthExpenditure) %>
            </h2>
            <%--  <br />
            <br />
            <br />
            <span style="font-weight: bolder; font-size: 1.2em; width: 1000px">
        <asp:Literal ID="ltrlCurrentMonth" runat="server"></asp:Literal>
        <asp:Label ID="lblExpenditureCurrentmonth" runat="server" SumType="PaymentsForMonth" CssClass="lblExpenditureCurrentMonth"
            OnPreRender="lblExpenditureCurrentmonth_PreRenderShowSum" /></span>--%>
            <br />
            <br />
            <span style="font-weight: bold;">VIII Detail of Balance Fund(Including Other receipts fund)</span>

            <jquery:GridViewEx ID="grvBankAccount" runat="server" AutoGenerateColumns="false" ShowFooter="true" Width="1000px" DataSource='<%# Item.AllBanks %>'>
                <Columns>
                    <eclipse:MultiBoundField DataFields="BankHead" HeaderText="Bank Head" AccessibleHeaderText="AccountHead" Visible="false"></eclipse:MultiBoundField>
                    <eclipse:MultiBoundField DataFields="BankName" HeaderText="Name Of The Account" AccessibleHeaderText="AccountName" ItemStyle-Width="800px">
                    </eclipse:MultiBoundField>
                    <eclipse:MultiBoundField DataFields="Balance" HeaderText="Balance As On Date" AccessibleHeaderText="AccountBalance" DataFormatString="{0:#,###,,.000;(#,###,,.000)}" DataSummaryCalculation="ValueSummation">
                        <ItemStyle HorizontalAlign="Right" Width="200px" />
                    </eclipse:MultiBoundField>
                </Columns>
                <HeaderStyle Height="9mm" CssClass="RowHeader ui-state-default" />
                <RowStyle CssClass="GridRow" />
                <AlternatingRowStyle CssClass="GridAlternatingRow" />
                <FooterStyle HorizontalAlign="Right" Height="9mm" CssClass="RowHeader ui-state-active ui-widget-header" />
            </jquery:GridViewEx>

            <br />
            <br />
            <br />
            <table rules="all" style="padding: 4mm; width: 1000px" class="MainTable">
                <thead class="ui-state-default">
                    <tr class="RowHeader">
                        <th style="text-align: center" class="RowHeader"><strong>Name of the receipts</strong></th>
                        <th style="text-align: center">Figures(in millions)</th>
                    </tr>
                </thead>
                <tbody>
                    <tr>
                        <td>
                            <asp:Label ID="lbltotfund" runat="server" Text="Total Fund Received from GOI"></asp:Label></td>
                        <td class="vd-amountcol">
                            <asp:Label ID="Label2" runat="server"
                                EnableViewState="false" Text='<%# string.Format("{0:#,###,,.000;(#,###,,.000)}", Item.FundReceivedGOITotalCum) %>' /></td>
                    </tr>
                    <tr class="AlternatingRow">
                        <td>
                            <asp:Label ID="lblexp" runat="server"
                                Text="Total Expenditure"></asp:Label></td>
                        <td class="vd-amountcol">
                            <asp:Label ID="Label3" runat="server"
                                EnableViewState="false"
                                SumType="PaymentsSum"
                                Text='<%# string.Format("{0:#,###,,.000;(#,###,,.000)}", Item.ExpendituresCum) %>' /></td>
                    </tr>
                </tbody>
                <tfoot>
                    <tr class="RowHeader ui-state-active ui-widget-header">
                        <td style="text-align: right"><b>
                            <asp:Label ID="lbldiff" runat="server"
                                Text="Balance Fund"></asp:Label></b></td>
                        <td class="vd-amountcol">
                            <asp:Label ID="lblDifference" runat="server"
                                EnableViewState="false" Text='<%# string.Format("{0:#,###,,.000;(#,###,,.000)}", Item.BalanceFund) %>' /></td>
                    </tr>
                </tfoot>
            </table>
            <br />
            <br />
            <br />
            <table rules="all" style="padding: 4mm; width: 1000px" class="MainTable">
                <tbody>
            <tr class="AlternatingRow">
                <td style="text-align: Left">
                    <asp:Label ID="Label4" runat="server" Text="Total amount of Bills outstanding"></asp:Label></td>
                    <%--<asp:Label ID="Label6" runat="server"
                        Text="Total amount of Bills outstanding"></asp:Label></b></td>--%>
                <td class="vd-amountcol">
                    <asp:Label ID="Label7" runat="server"
                        EnableViewState="false" Text='<%# string.Format("{0:#,###,,.000;(#,###,,.000)}", Item.OutstandingBillsAmount) %>' /></td>
            </tr>
                    </tbody>
                </table>
         
           <table rules="all" style="padding: 4mm; width: 1000px" class="MainTable"
                runat="server" visible='<%#Item.BalanceFund < Item.OutstandingBillsAmount%>'>
            <tbody>
                <tr class="AlternativeRow" style="font-size:large">
                    <td style="text-align:left">
                        <asp:Label ID="lblShortFall" runat="server" Text="Short falls (Balance funds - Total Outstanding) "></asp:Label>
                    </td>
                    <td class="vd-amountcol">
                        <asp:Label runat="server" ID="lblShortAmount" EnableViewState="false" Text='<%# string.Format("{0:#,###,,.000;(#,###,,.000)}", Item.BalanceFund-Item.OutstandingBillsAmount) %>'></asp:Label>
                            
                    </td>
                </tr>
            </tbody>
        </table>
            </ItemTemplate>
    </asp:FormView>

    <%-- <table rules="all" cellpadding="4mm" class="MainTable" width="1000px">
        <caption style="text-align: right; font-weight: bold; font-size: 1.2em">All Figures in Million Rs/Nu</caption>
        <thead class="ui-state-default">
            <tr class="RowHeader">
                <th align="center" rowspan="3" width="50px"><strong>I</strong></th>
                <th align="center" rowspan="3" class="RowHeader" width="450px">Receipts(1)
                </th>
                <th align="center" rowspan="3" width="150px">Upto Previous Year(2)
                </th>
                <th align="center" rowspan="3" width="150px">Current Year(3)
                </th>
                <th align="center" rowspan="3" width="200px">Total Cumulative(4)
                </th>
            </tr>
        </thead>
        <tbody>
            <tr class="AlternatingRow">
                <td></td>
                <td>Funds Received From GOI (Grant) Rs./NU.
                </td>
                <td class="vd-amountcol">
                    <asp:HyperLink ID="hplnkGOIAidPreviousYear" runat="server" ToolTip="Click to list voucher entries for Funds Received from Government of India in local Currency prior to Financial Year"
                        NavigateUrl="~/Finance/VoucherSearch.aspx?AccountTypes=GRANT_RECEIVED_GOINU&DateTo={3:d}"
                        EnableViewState="false" />
                </td>

                <td class="vd-amountcol">
                    <asp:HyperLink ID="hplnkGOIAidUptoMonth" runat="server" ToolTip="Click to list voucher entries for Funds Received from Government of India in local Currency from financial year start till input month"
                        EnableViewState="false" NavigateUrl="~/Finance/VoucherSearch.aspx?AccountTypes=GRANT_RECEIVED_GOINU&DateFrom={0:d}&DateTo={2:d}" />
                </td>
                <td class="vd-amountcol">
                    <asp:Label ID="lblGOIAidsum" runat="server" ToolTip="Funds Received from Government of India in local currency till date" EnableViewState="false" />
                </td>
            </tr>
            <tr class="AlternatingRow">
                <td></td>
                <td>Funds Received From GOI (Grant) F.E.
                </td>
                <td class="vd-amountcol">
                    <asp:HyperLink ID="hplnkGOIAidFEPreviousYear" runat="server" ToolTip="Click to list voucher entries for Funds Received from Government of India in local Currency prior to Financial Year"
                        NavigateUrl="~/Finance/VoucherSearch.aspx?AccountTypes=GRANT_RECEIVED_GOIFE&DateTo={3:d}"
                        EnableViewState="false" />
                </td>

                <td class="vd-amountcol">
                    <asp:HyperLink ID="hplnkGOIAidFEUptoMonth" runat="server" ToolTip="Click to list voucher entries for Funds Received from Government of India in local Currency from financial year start till input month"
                        EnableViewState="false" NavigateUrl="~/Finance/VoucherSearch.aspx?AccountTypes=GRANT_RECEIVED_GOIFE&DateFrom={0:d}&DateTo={2:d}" />
                </td>
                <td class="vd-amountcol">
                    <asp:Label ID="lblGOIAidFEsum" runat="server" ToolTip="Funds Received from Government of India in local currency till date" EnableViewState="false" />
                </td>
            </tr>
            <tr class="AlternatingRow">
                <td></td>
                <td>Funds Received From GOI (Loan) Rs. / Nu.
                </td>
                <td class="vd-amountcol">
                    <asp:HyperLink ID="hplnkGOILoanPreviousYear" runat="server" ToolTip="Click to list voucher entries for Funds Received from Government of India in local Currency prior to Financial Year"
                        NavigateUrl="~/Finance/VoucherSearch.aspx?AccountTypes=LOAN_RECEIVED_GOINU&DateTo={3:d}"
                        EnableViewState="false" />
                </td>

                <td class="vd-amountcol">
                    <asp:HyperLink ID="hplnkGOILoanUptoMonth" runat="server" ToolTip="Click to list voucher entries for Funds Received from Government of India in local Currency from financial year start till input month"
                        EnableViewState="false" NavigateUrl="~/Finance/VoucherSearch.aspx?AccountTypes=LOAN_RECEIVED_GOINU&DateFrom={0:d}&DateTo={2:d}" />
                </td>
                <td class="vd-amountcol">
                    <asp:Label ID="lblGOILoansum" runat="server" ToolTip="Funds Received from Government of India in local currency till date" EnableViewState="false" />
                </td>
            </tr>
            <tr>
                <td></td>
                <td>Funds Received From GOI (Loan) F.E.
                </td>
                <td class="vd-amountcol">

                    <asp:HyperLink ID="hplnkGOILoanFEPreviousYear" runat="server" ToolTip="Click to list voucher entries for Funds Received from Government of India in Foreign Exchange prior to Financial Year"
                        EnableViewState="false" NavigateUrl="~/Finance/VoucherSearch.aspx?AccountTypes=LOAN_RECEIVED_GOIFE&DateTo={3:d}" />
                </td>

                <td class="vd-amountcol">

                    <asp:HyperLink ID="hplnkGOILoanFEUptoMonth" runat="server" ToolTip="Click to list voucher entries for Funds Received from Government of India in Foreign Exchange from financial year start till input month"
                        EnableViewState="false" NavigateUrl="~/Finance/VoucherSearch.aspx?AccountTypes=LOAN_RECEIVED_GOIFE&DateFrom={0:d}&DateTo={2:d}" />
                </td>
                <td class="vd-amountcol">
                    <asp:Label ID="lblGOILoanFEsum" runat="server" ToolTip="Funds received from Government of India in Foreign Exchange till date"
                        EnableViewState="false" />
                </td>
            </tr>
            <tr class="RowHeader">
                <td align="center"><strong>II</strong></td>
                <td align="right">Total Fund GOI
                </td>
                <td class="vd-amountcol">
                    <asp:Label ID="lblFundReceivedPreviousYear" runat="server" EnableViewState="false"
                        OnPreRender="lbl_PreRenderShowSum" SumType="FundReceivedPreviousYear" />
                </td>

                <td class="vd-amountcol">
                    <asp:Label ID="lblFundRecievedUpToMonth" runat="server" EnableViewState="false" SumType="FundReceivedUptoMonth"
                        OnPreRender="lbl_PreRenderShowSum" />
                </td>
                <td class="vd-amountcol">
                    <asp:Label ID="lblFundRecievedSum" runat="server" EnableViewState="false" SumType="FundSum"
                        OnPreRender="lbl_PreRenderShowSum" />
                </td>
            </tr>
            <tr class="RowHeader">
                <td align="center"><strong>III</strong></td>
                <td title="TOTAL FUND GOI">
                    <span>Other Receipts:Ineterst,Sale of Tender,EMD,SD,other receipts etc</span>
                </td>
                <td class="vd-amountcol">

                    <asp:HyperLink ID="hplnkReceiptsPreviousYear" runat="server" ToolTip="Click to list voucher entries for Interest received prior to Financial Year"
                        EnableViewState="false" NavigateUrl="~/Finance/VoucherSearch.aspx?AccountTypes=INTEREST&DateTo={3:d}" />
                </td>

                <td class="vd-amountcol">

                    <asp:HyperLink ID="hplnkReceiptsUptoMonth" runat="server" ToolTip="Click to list voucher entries for Interest received from financial year start till input month"
                        EnableViewState="false" NavigateUrl="~/Finance/VoucherSearch.aspx?AccountTypes=INTEREST&DateFrom={0:d}&DateTo={2:d}" />
                </td>
                <td class="vd-amountcol">
                    <asp:Label ID="lblReceiptssum" runat="server" ToolTip="Interest received till date"
                        EnableViewState="false" />
                </td>
            </tr>
            <tr class="RowHeader">
                <td align="center"><strong>IV</strong></td>
                <td align="right">Total(II+III)
                </td>
                <td class="vd-amountcol">
                    <asp:Label ID="lblReceiptsPreviousYear" runat="server" EnableViewState="false" OnPreRender="lbl_PreRenderShowSum"
                        SumType="ReceiptsPreviousYear" />
                </td>

                <td class="vd-amountcol">
                    <asp:Label ID="lblReceiptsUptoMonth" runat="server" EnableViewState="false" OnPreRender="lbl_PreRenderShowSum"
                        SumType="ReceiptsUptoMonth" />
                </td>
                <td class="vd-amountcol">
                    <asp:Label ID="Label1" runat="server" EnableViewState="false" OnPreRender="lbl_PreRenderShowSum"
                        SumType="ReceiptsSum" />
                </td>
            </tr>
            <tr class="RowHeader">
                <td align="center"><strong>V</strong></td>
                <td align="right">Expenditure
                </td>
                <td class="vd-amountcol">
                    <asp:Label ID="lblExpenditurePreviousYear" runat="server" EnableViewState="false"
                        OnPreRender="lbl_PreRenderShowSum" SumType="PaymentsPreviousYear" />
                </td>
                <td class="vd-amountcol">
                    <asp:Label ID="lblExpenditureUpToMonth" runat="server" EnableViewState="false"
                        OnPreRender="lbl_PreRenderShowSum" SumType="PaymentsUptoMonth" />
                </td>
                <td class="vd-amountcol">
                    <asp:Label ID="lblTotalExpenditure" runat="server" EnableViewState="false"
                        OnPreRender="lbl_PreRenderShowSum" SumType="PaymentsSum" />
                </td>
            </tr>
            <tr class="RowHeader ui-state-active ui-widget-header">
                <td align="center"><strong>VI</strong></td>
                <td align="right"><b>BALANCE FUND(IV-V)</b></td>
                <td class="vd-amountcol">
                    <asp:Label ID="lblbalancefundprevious" runat="server" EnableViewState="False" OnPreRender="lblbalance_prerender" />
                </td>
                <td></td>
                <td class="vd-amountcol">
                    <asp:Label ID="lblbalancefundcumulative" runat="server" EnableViewState="False" OnPreRender="lblbalancefundcumulative_prerender" /></td>
            </tr>
        </tbody>
    </table>--%>

    <%-- <span style="font-weight: bold;">VII Detail of Expenditure</span>--%>
    <%-- <table rules="all" cellpadding="4mm" class="MainTable" width="1000px">
        <thead class="ui-state-default">
            <tr class="RowHeader">
                <th align="center" rowspan="2" width="50px"><strong></strong></th>
                <th align="center" rowspan="2" class="RowHeader" width="450px"></th>
                <th align="center" rowspan="2" width="150px">Upto Previous Year(2)<br />
                </th>
                <th align="center" rowspan="2" width="150px">Current Year(3)
                </th>
                <th align="center" rowspan="2" width="200px">Total Cumulative(4)<br />
                </th>
            </tr>
        </thead>
        <tbody>
            <tr>
                <td align="right"><strong>a)</strong></td>
                <td>Establishment (including WAPCOS)
                </td>

                <td class="vd-amountcol">

                    <asp:HyperLink ID="hplnkEstablishExpenditurePreviousYear" runat="server" ToolTip="Click to list voucher entries for expenditure for input month"
                        EnableViewState="false" NavigateUrl="~/Finance/VoucherSearch.aspx?AccountTypes=EXPENDITURE,TOUR_EXPENSES,MR&DateFrom={1:d}&DateTo={2:d}" />
                </td>
                <td class="vd-amountcol">

                    <asp:HyperLink ID="hplnkEstablishExpenditureUptoMonth" runat="server" ToolTip="Click to list voucher entries for expenditure from financial year start till input month"
                        EnableViewState="false" NavigateUrl="~/Finance/VoucherSearch.aspx?AccountTypes=EXPENDITURE,TOUR_EXPENSES,MR&DateFrom={0:d}&DateTo={2:d}" />
                </td>
                <td class="vd-amountcol">
                    <asp:Label ID="lblEstablishExpendituresum" runat="server" ToolTip="Expenditure incured till date"
                        EnableViewState="false" />
                </td>
            </tr>
            <tr class="AlternatingRow">
                <td align="right"><strong>b)</strong></td>
                <td>Civil Works
                </td>
                <td class="vd-amountcol">

                    <asp:HyperLink ID="hplnkCivilExpenditurePreviousYear" runat="server" ToolTip="Click to list voucher entries for expenditure for input month"
                        EnableViewState="false" NavigateUrl="~/Finance/VoucherSearch.aspx?AccountTypes=EXPENDITURE,TOUR_EXPENSES,MR&DateFrom={1:d}&DateTo={2:d}" />
                </td>
                <td class="vd-amountcol">

                    <asp:HyperLink ID="hplnkCivilExpenditureUpToMonth" runat="server" ToolTip="Click to list voucher entries for expenditure from financial year start till input month"
                        EnableViewState="false" NavigateUrl="~/Finance/VoucherSearch.aspx?AccountTypes=EXPENDITURE,TOUR_EXPENSES,MR&DateFrom={0:d}&DateTo={2:d}" />
                </td>
                <td class="vd-amountcol">
                    <asp:Label ID="lblCivilExpendituresum" runat="server" ToolTip="Expenditure incured till date"
                        EnableViewState="false" />
                </td>
            </tr>
            <tr>
                <td align="right"><strong>c)</strong></td>
                <td>Electrical
                </td>
                <td class="vd-amountcol">

                    <asp:HyperLink ID="hplnkElectricalExpenditurePreviousYear" runat="server" ToolTip="Click to list voucher entries for employee advances prior to Financial Year"
                        EnableViewState="false" NavigateUrl="~/Finance/VoucherSearch.aspx?AccountTypes=EMPLOYEE_ADVANCE&DateTo={3:d}" />
                </td>
                <td class="vd-amountcol">

                    <asp:HyperLink ID="hplnkElectricalExpenditureUpToMonth" runat="server" ToolTip="Click to list voucher entries for employee advances from financial year start till input month"
                        EnableViewState="false" NavigateUrl="~/Finance/VoucherSearch.aspx?AccountTypes=EMPLOYEE_ADVANCE&DateFrom={0:d}&DateTo={2:d}" />
                </td>
                <td class="vd-amountcol">
                    <asp:Label ID="lblElectricalExpendituresum" runat="server" ToolTip="Advances to Employees till date"
                        EnableViewState="false" />
                </td>
            </tr>
            <tr>
                <td align="right"><strong>d)</strong></td>
                <td>Transmission
                </td>
                <td class="vd-amountcol">

                    <asp:HyperLink ID="hplnkTransmissionExpenditurePreviousYear" runat="server" ToolTip="Click to list voucher entries for party and material advances prior to Financial Year"
                        NavigateUrl="~/Finance/VoucherSearch.aspx?AccountTypes=PARTY_ADVANCE,MATERIAL_ADVANCE&DateTo={3:d}"
                        EnableViewState="false" />
                </td>

                <td class="vd-amountcol">

                    <asp:HyperLink ID="hplnkTransmissionExpenditureUpToMonth" runat="server" ToolTip="Click to list voucher entries for party and material advances from financial year start till input month"
                        NavigateUrl="~/Finance/VoucherSearch.aspx?AccountTypes=PARTY_ADVANCE,MATERIAL_ADVANCE&DateFrom={0:d}&DateTo={2:d}"
                        EnableViewState="false" />
                </td>
                <td class="vd-amountcol" colspan="2">
                    <asp:Label ID="lblTransmissionExpendituresum" runat="server" ToolTip="Advances to parties for material till date"
                        EnableViewState="false" />
                </td>
            </tr>

            <tr class="RowHeader ui-state-active ui-widget-header">
                <td class="center"></td>
                <td align="right"><b>TOTAL PAYMENTS</b></td>
                <td class="vd-amountcol">
                    <asp:Label ID="lblPaymentsPreviousYear" runat="server" EnableViewState="false" OnPreRender="lbl_PreRenderShowSum"
                        SumType="PaymentsPreviousYear" />
                </td>

                <td class="vd-amountcol">
                    <asp:Label ID="lblPaymentsumUptoMonth" runat="server" EnableViewState="false" SumType="PaymentsUptoMonth"
                        OnPreRender="lbl_PreRenderShowSum" />
                </td>
                <td class="vd-amountcol">
                    <asp:Label ID="lblPaymentsum" runat="server" EnableViewState="false" SumType="PaymentsSum"
                        OnPreRender="lbl_PreRenderShowSum" />
                    <asp:Label ID="lblPaymentForMonthSum" runat="server" EnableViewState="false" Visible="false" SumType="PaymentsForMonth"
                        OnPreRender="lbl_PreRenderShowSum" />
                </td>
            </tr>
        </tbody>
    </table>--%>
    <%--   <br />
    <br />
    <br />
  <span style="font-weight: bolder; font-size: 1.2em; width: 1000px">
        <asp:Literal ID="ltrlCurrentMonth" runat="server"></asp:Literal>
        <asp:Label ID="lblExpenditureCurrentmonth" runat="server" EnableViewState="false" SumType="PaymentsForMonth" CssClass="lblExpenditureCurrentMonth"
            OnPreRender="lblExpenditureCurrentmonth_PreRenderShowSum" /></span>
    <br />
    <br />
    <br />
    <span style="font-weight: bold;">VIII Detail of Balance Fund(Including Other receipts fund)</span>
    <div id="divBankAccount" runat="server">
        <jquery:GridViewEx ID="grvBankAccount" runat="server" AutoGenerateColumns="false" OnRowDataBound="grvBankAccount_RowDataBound" ShowFooter="true" Width="1000px">
            <Columns>
                <eclipse:MultiBoundField DataFields="BankHead" HeaderText="Bank Head" AccessibleHeaderText="AccountHead" Visible="false"></eclipse:MultiBoundField>
                <eclipse:MultiBoundField DataFields="BankName" HeaderText="Name Of The Account" AccessibleHeaderText="AccountName" ItemStyle-Width="800px">
                </eclipse:MultiBoundField>
                <eclipse:MultiBoundField DataFields="Balance" HeaderText="Balance As On Date" AccessibleHeaderText="AccountBalance">
                    <ItemStyle HorizontalAlign="Right" Width="200px" />
                </eclipse:MultiBoundField>
            </Columns>
            <FooterStyle HorizontalAlign="Right" Font-Bold="true" CssClass="RowHeader ui-state-active ui-widget-header" />
        </jquery:GridViewEx>
    </div>
    <br />
    <br />
    <br />--%>
    <%-- <div id="divdiff" runat="server">
        <table rules="all" cellpadding="4mm" class="MainTable" width="1000px">
            <thead class="ui-state-default">
                <tr class="RowHeader">
                    <th align="center"><strong>Name of the receipts</strong></th>
                    <th align="center">Figures(in millions)</th>
                </tr>
            </thead>
            <tbody>
                <tr class="AlternatingRow">
                    <td>
                        <asp:Label ID="lbltotfund" runat="server" Text="Total Fund Received from GOI"></asp:Label></td>
                    <td class="vd-amountcol">
                        <asp:Label ID="Label2" runat="server"
                            EnableViewState="false" SumType="FundSum"
                            OnPreRender="lbl_PreRenderShowSum" /></td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblexp" runat="server"
                            Text="Total Expenditure"></asp:Label></td>
                    <td class="vd-amountcol">
                        <asp:Label ID="Label3" runat="server"
                            EnableViewState="false"
                            OnPreRender="lbl_PreRenderShowSum"
                            SumType="PaymentsSum" /></td>
                </tr>
            </tbody>
            <tfoot>
                <tr class="RowHeader ui-state-active ui-widget-header">
                    <td align="right"><b>
                        <asp:Label ID="lbldiff" runat="server"
                            Text="Balance Fund"></asp:Label></b></td>
                    <td class="vd-amountcol">
                        <asp:Label ID="lblDifference" runat="server"
                            EnableViewState="false"
                            OnPreRender="lblDifference_PreRender" /></td>
                </tr>
            </tfoot>
        </table>
    </div>--%>
</asp:Content>
