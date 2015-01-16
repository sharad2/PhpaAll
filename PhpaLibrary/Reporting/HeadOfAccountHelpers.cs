
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
            internal const string CivilTourExpenditure =  "CIVIL_TOUR";

            // Used by Job Payment Register
            public const string MainCivilExpenditure = "CIVIL_MAIN_EXPENSES";

            internal const string ElectricalExpenditure =  "ELECTR_EXPENDITURE";
            internal const string ElectricalTourExpenditure = "ELECTRICAL_TOUR";

            internal const string TransmissionExpenditure =  "TRANS_EXPENDITURE";
            internal const string TransmissionTourExpenditure =  "TRANS_TOUR";

        }

        public readonly static IEnumerable<string> TourExpenditures = new[] { ExpenditureSubTypes.CivilTourExpenditure, ExpenditureSubTypes.ElectricalTourExpenditure,
                ExpenditureSubTypes.EstablishmentTourExpenditure, ExpenditureSubTypes.TransmissionTourExpenditure
            };

        public readonly static IEnumerable<string> EstablishmentExpenditures = new[] { ExpenditureSubTypes.EstablishmentExpenditure, ExpenditureSubTypes.EstablishmentTourExpenditure };

        public readonly static IEnumerable<string> ElectricalExpenditures = new[] { ExpenditureSubTypes.ElectricalExpenditure, ExpenditureSubTypes.ElectricalTourExpenditure };

        public readonly static IEnumerable<string> TransmissionExpenditures = new[] { ExpenditureSubTypes.TransmissionExpenditure, ExpenditureSubTypes.TransmissionTourExpenditure };

        public readonly static IEnumerable<string> CivilExpenditures = new[] { 
            ExpenditureSubTypes.CivilTourExpenditure, ExpenditureSubTypes.CivilExpenditure, ExpenditureSubTypes.MainCivilExpenditure };

        private readonly static IEnumerable<string> ProjectExpenditures = new[] {
                ExpenditureSubTypes.EstablishmentExpenditure, ExpenditureSubTypes.CivilExpenditure,ExpenditureSubTypes.MainCivilExpenditure, ExpenditureSubTypes.ElectricalExpenditure,
                ExpenditureSubTypes.TransmissionExpenditure
            };

        public readonly static IEnumerable<string> AllExpenditures = TourExpenditures.Concat(ProjectExpenditures);


        public static class AdvanceSubTypes
        {
            public const string MaterialAdvance =  "MATERIAL_ADVANCE";
            public const string EmployeeAdvance =  "EMPLOYEE_ADVANCE";
            public const string EstablishmentPartyAdvance = "ESTB_PARTY_ADVANCE";
            public const string CivilPartyAdance =  "CIVIL_PARTY_ADVANCE";
            public const string ElectricalPartyAdvance =  "ELEC_PARTY_ADVANCE";
            public const string TransmissionPartyAdance = "TRAN_PARTY_ADVANCE";

        }
        public readonly static IEnumerable<string> PartyAdvances = new[] { 
            AdvanceSubTypes.EstablishmentPartyAdvance, AdvanceSubTypes.CivilPartyAdance, AdvanceSubTypes.ElectricalPartyAdvance, AdvanceSubTypes.TransmissionPartyAdance };

        public readonly static IEnumerable<string> JobAdvances = new[] {AdvanceSubTypes.MaterialAdvance}.Concat(PartyAdvances);

        public readonly static IEnumerable<string> AllAdvances = new[] {AdvanceSubTypes.EmployeeAdvance}.Concat(JobAdvances);


        public static class TaxSubTypes
        {
            public readonly static IEnumerable<string> GreenTax = new string[] { "GREEN_TAX" };
            public readonly static IEnumerable<string> BhutanIncomeTax = new string[] { "BIT" };
            public readonly static IEnumerable<string> ServiceTax = new string[] { "SVCTAX" };
            public readonly static IEnumerable<string> BhutanSalesTax = new string[] { "BST" };
        }

        /// <summary>
        /// Government taxes levied on contractor activities
        /// </summary>
        public readonly static IEnumerable<string> ContractorTaxes = TaxSubTypes.GreenTax.Concat(TaxSubTypes.ServiceTax).Concat(TaxSubTypes.BhutanSalesTax);

        public readonly static IEnumerable<string> SalaryRemittances = new string[] { "SALARY_REMITANCES" };

        public readonly static IEnumerable<string> Grants = GrantSubType.GrantNu.Concat(GrantSubType.GrantFe);

        public static class GrantSubType
        {
            public readonly static IEnumerable<string> GrantNu = new string[] { "GRANT_RECEIVED_GOINU" };
            public readonly static IEnumerable<string> GrantFe = new string[] { "GRANT_RECEIVED_GOIFE" };
        }

        public static class LoanSubType
        {
            public readonly static IEnumerable<string> LoanNu = new string[] { "LOAN_RECEIVED_GOINU" };
            public readonly static IEnumerable<string> LoanFe = new string[] { "LOAN_RECEIVED_GOIFE" };
        }

        public readonly static IEnumerable<string> AllLoans = LoanSubType.LoanNu.Concat(LoanSubType.LoanFe);


        public static class ReceiptSubType
        {
            public readonly static IEnumerable<string> InterestReceipts = new string[] { "INTEREST" };
            public readonly static IEnumerable<string> AccumulatedReceipts = new string[] { "ACCUMULATED_RECEIPTS" };
            public readonly static IEnumerable<string> TenderSale = new string[] { "TENDER_SALE" };
        }

        public static class CashSubType
        {
            public readonly static IEnumerable<string> CashInBankNu = new string[] { "BANKNU" };
            public readonly static IEnumerable<string> CashInBankFe = new string[] { "BANKFE" };
            public readonly static IEnumerable<string> CashInHand = new string[] { "CASH" };
            public readonly static IEnumerable<string> Investment = new string[] { "INVESTMENT" };
        }

        public readonly static IEnumerable<string> AllBanks = CashSubType.CashInBankNu.Concat(CashSubType.CashInBankFe);

        public static class DutySubType
        {
            public readonly static IEnumerable<string> ExciseDutiesGOI = new string[] { "EDGOI" };
            public readonly static IEnumerable<string> ExciseDutiesRGOB = new string[] { "EDRGOB" };
        }

        public readonly static IEnumerable<string> AllExciseDuties = DutySubType.ExciseDutiesGOI.Concat(DutySubType.ExciseDutiesRGOB);

        public readonly static IEnumerable<string> Assets = new string[] { "ASSETS" };

        public readonly static IEnumerable<string> Liabilities = new string[] { "LIABILITY" };

        public readonly static IEnumerable<string> StockSuspense = new string[] { "STOCK_SUSPENSE" };

        public readonly static IEnumerable<string> FundTransit = new string[] { "FUNDS_TRANSIT" };

        public static class DepositSubTypes
        {
            public readonly static IEnumerable<string> EarnestMoneyDeposit = new string[] { "EMD" };
            public readonly static IEnumerable<string> SecurityDeposits = new string[] { "SD" };
        }

    }
}
