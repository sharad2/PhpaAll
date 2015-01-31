<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" CodeBehind="LearnMore.aspx.cs"
    Inherits="PhpaAll.Finance.LearnMore" Title="Learn More" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphSideNavigation" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cph" runat="server">
    <p>
        If you are using this program for the first time, this checklist can help you in
        getting started.
    </p>
    <h3>
        Security</h3>
    <p>
        If you intend to modify any data, you must ask your administrator to create a login
        for you. For reviewing the information, no login is needed. If you are ready to
        login, click
        <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="~/Login.aspx">here</asp:HyperLink>.
        If you are an administrator, you can create a new login account by clicking
        <asp:HyperLink ID="HyperLink2" runat="server" NavigateUrl="~/Admin/ManageUsers.aspx">here</asp:HyperLink>.
        You may be asked to login as an administrator.
    </p>
    <h3>
        Basic Voucher Creation</h3>
    <p>
        View the currently existing
        <asp:HyperLink runat="server" NavigateUrl="~/Finance/AccountHeads.aspx">head of accounts</asp:HyperLink>.
        At least some head of accounts must be created before you are able to create vouchers.
    </p>
    <p>
        <asp:HyperLink runat="server" NavigateUrl="~/Finance/InsertVoucher.aspx">Create vouchers</asp:HyperLink>
        to record income and expenses incurred by your organization.
    </p>
    <p>
        Review the information you have entered by looking at the
        <asp:HyperLink runat="server" NavigateUrl="~/Finance/DayBook.aspx">Day Book</asp:HyperLink>,
        <asp:HyperLink runat="server" NavigateUrl="~/Reports/Ledger.aspx">Ledger</asp:HyperLink>
        and
        <asp:HyperLink runat="server" NavigateUrl="~/Reports/JournalBook.aspx">Journal</asp:HyperLink>.
    </p>
    <p>
        See your expenditures by viewing
        <asp:HyperLink ID="HyperLink6" runat="server" NavigateUrl="~/Reports/Expenditure.aspx">Expenditure Report</asp:HyperLink>.
    </p>
    <p>
        You can also view the
        <asp:HyperLink ID="HyperLink5" runat="server" NavigateUrl="~/Reports/BalanceSheet.aspx">Balance Sheet</asp:HyperLink>
        to view summary information.
    </p>
    <h3>
        Managing Cash and Bank Balances</h3>
    <p>
        Any head of account whose balance sheet type is specified as Cash Account/Cash in
        hand is considered to be a cash account.
    </p>
    <p>
        Similarly, any head of account whose balance sheet type is specified as Bank Account
        is considered to be a bank account. Bank accounts can be in local currency or in
        foreign currency. The balance sheet displays the two balances separately.
    </p>
    <p>
        You can now create head of accounts to represent a cash and bank accounts. When
        you create these head of accounts, be sure to specify the balance types as explained
        above. If you have already created these head of accounts, just change their balance
        sheet types to proper values.
    </p>
    <p>
        Now create some vouchers using these new head of accounts.
    </p>
    <p>
        View the
        <asp:HyperLink runat="server" NavigateUrl="~/Reports/CashBook.aspx">Cash Book</asp:HyperLink>
        to see the cash transactions. In this report, you can select any head of account
        which you have designated as a cash account.
    </p>
    <p>
        View the
        <asp:HyperLink runat="server" NavigateUrl="~/Reports/BankBook.aspx">Bank Book</asp:HyperLink>
        to see the bank transactions. In this report, you can select any head of account
        which you have designated as a bank account.
    </p>
    <h3>
        Managing Contractors</h3>
    <p>
        <em>Record EMD or Security Deposit</em>. To enter HeadOfAccount for EMD or Security
        deposit go to
        <asp:HyperLink ID="HyperLink4" runat="server" NavigateUrl="~/Finance/AccountHeads.aspx">Manage HeadOfAccount</asp:HyperLink>.
    </p>
    <p>
        <em>Create the Contractor</em>. To create a contractor or view information of existing
        contractors go to
        <asp:HyperLink ID="HyperLink3" runat="server" NavigateUrl="~/Finance/Contractor.aspx">Manage Contractors</asp:HyperLink>.
        Search for the contractor you are interested in. If this contractor has not yet
        been created, create him now. Once the contractor exists then you are ready to enter
        <asp:HyperLink ID="HyperLink7" runat="server" NavigateUrl="~/Finance/InsertVoucher.aspx">vouchers</asp:HyperLink>
        pertaining to them.
    </p>
    <h3>
        Managing Employee</h3>
    <p>
        To enter HeadOfAccount related to employees like tour expenses go to
        <asp:HyperLink ID="HyperLink10" runat="server" NavigateUrl="~/Finance/AccountHeads.aspx">Manage HeadOfAccount</asp:HyperLink>.
    </p>
    <p>
        <em>Create the employee</em>. To add new employee or view information of existing
        employees go to
        <asp:HyperLink ID="HyperLink8" runat="server" NavigateUrl="~/PIS/Employees.aspx">Manage Employees</asp:HyperLink>.
        You can also search for the employee you are interested in. Once the employee exists
        and his basic salary has been defined then you are ready to enter
        <asp:HyperLink ID="HyperLink9" runat="server" NavigateUrl="~/Finance/InsertVoucher.aspx">vouchers</asp:HyperLink>
        related to them.
    </p>
    <h3>
        Managing Jobs</h3>
    <p>
        <em>Create Divisions</em>. To enter a job you must create a division first, for
        creating division or view existing divisions click
        <asp:HyperLink runat="server" NavigateUrl="~/Finance/ManageDivisions.aspx">Manage Divisions</asp:HyperLink>.
        Now the jobs can be created by clicking
        <asp:HyperLink runat="server" NavigateUrl="~/Finance/Jobs.aspx">Create Jobs</asp:HyperLink>
    </p>
    <p>
        View the
        <asp:HyperLink runat="server" NavigateUrl="~/Reports/ExpenditureAgainstJobType.aspx"
            Text="ExpenditureAgainstJobType" />
        to see expenditures of a job.</p>
    <p>
        For viewing job expenditure for any division click
        <asp:HyperLink runat="server" NavigateUrl="~/Reports/Exp_VariousHeads.aspx" Text="Divisional Job Expenditure" /></p>
    <p>
        For getting details of payment and recoveries click
        <asp:HyperLink runat="server" NavigateUrl="~/Reports/ContractorPayment.aspx" Text="Contractor Payment" /></p>
    <p>
        Once the job are created you are ready to enter
        <asp:HyperLink ID="HyperLink11" runat="server" NavigateUrl="~/Finance/InsertVoucher.aspx">vouchers</asp:HyperLink>
        for a job.
    </p>
    <p>
        To enter HeadofAccount related to jobs expenses go to
        <asp:HyperLink ID="HyperLink12" runat="server" NavigateUrl="~/Finance/AccountHeads.aspx"
            Text="Manage HeadOfAccount" /></p>
</asp:Content>
