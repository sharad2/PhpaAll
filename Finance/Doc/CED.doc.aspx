<%@ Page Title="CED Doc" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeBehind="CED.doc.aspx.cs" Inherits="PhpaAll.Doc.CED" %>

<asp:Content ID="Content6" ContentPlaceHolderID="cph" runat="server">
<asp:MultiView ID="mvHelp" runat="server">
        <asp:View ID="ED" runat="server">
            <br />
            <p>Display Central Excise Duty amount incurred on the material imported.</p>
            <ol>
                <li>By Default Report shows the Central Excise Duty reimbursed to Bhutanese Contractors.
                    If you want to see the Central Excise Duty for other Contractors, Select Nationality
                    from drop down list. </li>
                <li>By default, the date is set to the current date. This makes it convenient for you
                    to see all the Central Excise Duty up to current date. </li>
                <li>You will be given with the Nationality filter which allows you to filter the data
                    according to the Nationality of the Contractor. </li>
                <li>Reports shows all the packages by default, if any package does not contain any entry,
                    it shows the message <b>"No Data Found in this Package".</b> </li>
                <li>Each package contain a table having Head of Account, During the Month, During the
                    Year and Total Cumulative columns. </li>
                <li>In order to verify the entry of the vouchers, During the Month and During the Year
                    Columns contain links which will redirect you to the voucher search form. </li>
                <li><b>Note:</b> While creating voucher, please mention the division which contains
                    the Job against which you are going to the reimburse the Central Excise Duty. When
                    you select the Job during Voucher entry, Head of Account associated with that Job
                    selected automatically. Replace that Head of Account and Debit Excise Duty Head
                    Of Account. </li>
            </ol>
        </asp:View>
        <asp:View ID="BST" runat="server">
            <br />
            <p>Display Bhutan Sales Tax applied on Material.</p>
            <ol>
                <li>By Default Report shows the Bhutan Sales Tax reimbursed to Bhutnese Contractors.
                    If you want to see the Bhutan Sales Tax for other Contractors, Select Nationality
                    from drop down list. </li>
                <li>By default, the date is set to the current date. This makes it convenient for you
                    to see all the Central Excise Duty upto current date. </li>
                <li>You will be given with the Nationality filter which allows you to filter the data
                    according to the Nationality of the Contractor. </li>
                <li>Reports shows all the packages by default, if any package does not contain any entry,
                    it shows the message <b>"No Data Found in this Package".</b> </li>
                <li>Each package contain a table having Head of Account, During the Month, During the
                    Year and Total Cumulative columns. </li>
                <li>In order to verify the entry of the vouhcers, During the Month and During the Year
                    Columns contain links which will redirect you to the voucher search form. </li>
                <li><b>Note:</b> While creating voucher, please mention the division which contains
                    the Job against which you are going to the reimburse the Bhutan Sales Tax. When
                    you select the Job during Voucher entry, Head of Account associated with that Job
                    selected automatically. Replace that Head of Account and Debit Bhutan Sales Tax
                    Head of Account. </li>
            </ol>
        </asp:View>
    </asp:MultiView>
</asp:Content>
