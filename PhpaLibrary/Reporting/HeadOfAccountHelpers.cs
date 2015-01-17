
using System.Collections.Generic;
using System.Linq;

namespace Eclipse.PhpaLibrary.Reporting
{
    public static class HeadOfAccountHelpers
    {

        public static class ExpenditureSubTypes
        {
            internal const string EstablishmentExpenditure = "ESTABL_EXPENDITURE";
            internal const string EstablishmentTourExpenditure = "ESTABLISHMENT_TOUR";

            internal const string CivilExpenditure = "CIVIL_EXPENDITURE";
            internal const string CivilTourExpenditure = "CIVIL_TOUR";

            // Used by Job Payment Register
            public const string MainCivilExpenditure = "CIVIL_MAIN_EXPENSES";

            internal const string ElectricalExpenditure = "ELECTR_EXPENDITURE";
            internal const string ElectricalTourExpenditure = "ELECTRICAL_TOUR";

            internal const string TransmissionExpenditure = "TRANS_EXPENDITURE";
            internal const string TransmissionTourExpenditure = "TRANS_TOUR";

        }

        public readonly static IEnumerable<string> TourExpenditures = new[] { ExpenditureSubTypes.CivilTourExpenditure, ExpenditureSubTypes.ElectricalTourExpenditure,
                ExpenditureSubTypes.EstablishmentTourExpenditure, ExpenditureSubTypes.TransmissionTourExpenditure
            };

        public readonly static IEnumerable<string> ElectricalExpenditures = new[] { ExpenditureSubTypes.ElectricalExpenditure, ExpenditureSubTypes.ElectricalTourExpenditure };

        public readonly static IEnumerable<string> TransmissionExpenditures = new[] { ExpenditureSubTypes.TransmissionExpenditure, ExpenditureSubTypes.TransmissionTourExpenditure };

        private readonly static IEnumerable<string> ProjectExpenditures = new[] {
                ExpenditureSubTypes.EstablishmentExpenditure, ExpenditureSubTypes.CivilExpenditure,ExpenditureSubTypes.MainCivilExpenditure, ExpenditureSubTypes.ElectricalExpenditure,
                ExpenditureSubTypes.TransmissionExpenditure
            };

        public readonly static IEnumerable<string> AllExpenditures = TourExpenditures.Concat(ProjectExpenditures);


        public static class AdvanceSubTypes
        {
            public const string MaterialAdvance = "MATERIAL_ADVANCE";
            public const string EmployeeAdvance = "EMPLOYEE_ADVANCE";
            public const string EstablishmentPartyAdvance = "ESTB_PARTY_ADVANCE";
            public const string CivilPartyAdance = "CIVIL_PARTY_ADVANCE";
            public const string ElectricalPartyAdvance = "ELEC_PARTY_ADVANCE";
            public const string TransmissionPartyAdance = "TRAN_PARTY_ADVANCE";

        }
        public readonly static IEnumerable<string> PartyAdvances = new[] { 
            AdvanceSubTypes.EstablishmentPartyAdvance, AdvanceSubTypes.CivilPartyAdance, AdvanceSubTypes.ElectricalPartyAdvance, AdvanceSubTypes.TransmissionPartyAdance };

        public readonly static IEnumerable<string> JobAdvances = new[] { AdvanceSubTypes.MaterialAdvance }.Concat(PartyAdvances);

        public readonly static IEnumerable<string> AllAdvances = new[] { AdvanceSubTypes.EmployeeAdvance }.Concat(JobAdvances);


        public static class TaxSubTypes
        {
            public const string GreenTax = "GREEN_TAX";
            public const string BhutanIncomeTax = "BIT";
            public const string ServiceTax = "SVCTAX";
            public const string BhutanSalesTax = "BST";
        }

        /// <summary>
        /// Government taxes levied on contractor activities
        /// </summary>
        public readonly static IEnumerable<string> ContractorTaxes = new[] { TaxSubTypes.GreenTax, TaxSubTypes.ServiceTax, TaxSubTypes.BhutanSalesTax };

        public readonly static IEnumerable<string> SalaryRemittances = new string[] { "SALARY_REMITANCES" };

        public static class GrantSubType
        {
            public const string GrantNu = "GRANT_RECEIVED_GOINU";
            public const string GrantFe = "GRANT_RECEIVED_GOIFE";
        }

        public readonly static IEnumerable<string> Grants = new[] { GrantSubType.GrantNu, GrantSubType.GrantFe };

        public static class LoanSubType
        {
            public const string LoanNu = "LOAN_RECEIVED_GOINU";
            public const string LoanFe = "LOAN_RECEIVED_GOIFE";
        }

        public readonly static IEnumerable<string> AllLoans = new[] { LoanSubType.LoanNu, LoanSubType.LoanFe };


        public static class ReceiptSubType
        {
            public const string InterestReceipt = "INTEREST";
            public const string AccumulatedReceipt = "ACCUMULATED_RECEIPTS";
            public const string TenderSale = "TENDER_SALE";
        }

        public static class CashSubTypes
        {
            public const string CashInBankNu = "BANKNU";
            public const string CashInBankFe = "BANKFE";
            public const string CashInHand = "CASH";
            public const string Investment = "INVESTMENT";
        }

        public readonly static IEnumerable<string> AllBanks = new[] { CashSubTypes.CashInBankNu, CashSubTypes.CashInBankFe };

        public static class ExciseDutySubTypes
        {
            public const string ExciseDutyGOI = "EDGOI";
            public const string ExciseDutyRGOB = "EDRGOB";
        }

        public readonly static IEnumerable<string> AllExciseDuties = new[] { ExciseDutySubTypes.ExciseDutyGOI, ExciseDutySubTypes.ExciseDutyRGOB };

        public readonly static IEnumerable<string> Assets = new string[] { "ASSETS" };

        public readonly static IEnumerable<string> Liabilities = new string[] { "LIABILITY" };

        public readonly static IEnumerable<string> StockSuspense = new string[] { "STOCK_SUSPENSE" };

        public readonly static IEnumerable<string> FundTransit = new string[] { "FUNDS_TRANSIT" };

        public static class DepositSubTypes
        {
            public const string EarnestMoneyDeposit = "EMD";
            public const string SecurityDeposit = "SD";
        }

        /// <summary>
        /// Used by Fund position report
        /// </summary>
        public readonly static IEnumerable<string> CivilExpenditures = new[] { 
                ExpenditureSubTypes.CivilTourExpenditure, ExpenditureSubTypes.CivilExpenditure, ExpenditureSubTypes.MainCivilExpenditure,                   
                ExciseDutySubTypes.ExciseDutyGOI, AdvanceSubTypes.MaterialAdvance, AdvanceSubTypes.CivilPartyAdance, TaxSubTypes.GreenTax,
                TaxSubTypes.BhutanSalesTax
            }.Concat(StockSuspense);

        public readonly static IEnumerable<string> EstablishmentExpenditures = new[] {
            ExpenditureSubTypes.EstablishmentExpenditure, ExpenditureSubTypes.EstablishmentTourExpenditure, AdvanceSubTypes.EmployeeAdvance,
            AdvanceSubTypes.EstablishmentPartyAdvance, TaxSubTypes.ServiceTax
        };

    }
}
