
using System.Collections.Generic;
using System.Linq;

namespace Eclipse.PhpaLibrary.Reporting
{
    public static class HeadOfAccountHelpers
    {

        public static class ExpenditureSubTypes
        {
            public readonly static IEnumerable<string> CivilExpenditure = new string[] { "CIVIL_EXPENDITURE" };
        }

        public readonly static IEnumerable<string> JobExpenses = (new string[] { "EXPENDITURE", "TOUR_EXPENSES" }).Concat(ExpenditureSubTypes.CivilExpenditure);

        public static class AdvanceSubTypes
        {
            public readonly static IEnumerable<string> PartyAdvance = new string[] { "PARTY_ADVANCE" };
            public readonly static IEnumerable<string> MaterialAdvance = new string[] { "MATERIAL_ADVANCE" };
            public readonly static IEnumerable<string> EmployeeAdvance = new string[] { "EMPLOYEE_ADVANCE" };
        }

        public static class TaxSubTypes
        {
            public readonly static IEnumerable<string> GreenTax = new string[] { "GREEN_TAX" };
            public readonly static IEnumerable<string> BhutanTax = new string[] { "BIT" };
            public readonly static IEnumerable<string> ServiceTax = new string[] { "SVCTAX" };
            public readonly static IEnumerable<string> BhutanSalesTax = new string[] { "BST" };
        }


        public readonly static IEnumerable<string> SalaryRemitances = new string[] { "SALARY_REMITANCES" };
        public readonly static IEnumerable<string> JobAdvances = AdvanceSubTypes.PartyAdvance.Concat(AdvanceSubTypes.MaterialAdvance);
        public readonly static IEnumerable<string> Grants = new string[] { "GRANT_RECEIVED_GOINU", "GRANT_RECEIVED_GOIFE" };
        public readonly static IEnumerable<string> Loans = new string[] { "LOAN_RECEIVED_GOINU", "LOAN_RECEIVED_GOIFE" };

        public readonly static IEnumerable<string> AccumulatedReceipts = new string[] { "ACCUMULATED_RECEIPTS", "TENDER_SALE" };

        public readonly static IEnumerable<string> InterestReceipts = new string[] { "INTEREST" };

        public static class CashSubType
        {
            public readonly static IEnumerable<string> CashInBank = new string[] { "BANKNU", "BANKFE" };
            public readonly static IEnumerable<string> BankNu = new string[] { "BANKNU" };
            public readonly static IEnumerable<string> BankFE = new string[] { "BANKFE" };
            public readonly static IEnumerable<string> Cash = new string[] { "CASH" };
            public readonly static IEnumerable<string> Investment = new string[] { "INVESTMENT" };
        }
        

        public readonly static IEnumerable<string> ExciseDuties = new string[] { "EDGOI", "EDRGOB" };

        public static class DepositSubTypes
        {
            public readonly static IEnumerable<string> EarnestMoneyDeposit = new string[] { "EMD" };
            public readonly static IEnumerable<string> SecurityDeposits = new string[] { "SD" };
        }

    }
}
