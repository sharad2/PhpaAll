
using System.Collections.Generic;
using System.Linq;

namespace Eclipse.PhpaLibrary.Reporting
{
    public static class HeadOfAccountHelpers
    {

        public static class ExpenditureSubTypes
        {
            public readonly static IEnumerable<string> EstablishmentExpenditure = new string[] { "ESTABL_EXPENDITURE" };
            public readonly static IEnumerable<string> EstablishmentTourExpenditure = new string[] { "ESTABLISHMENT_TOUR" };

            public readonly static IEnumerable<string> CivilExpenditure = new string[] { "CIVIL_EXPENDITURE" };
            public readonly static IEnumerable<string> CivilTourExpenditure = new string[] { "CIVIL_TOUR" };
            public readonly static IEnumerable<string> MainCivilExpenditure = new string[] { "CIVIL_MAIN_EXPENSES" };

            public readonly static IEnumerable<string> ElectricalExpenditure = new string[] { "ELECTR_EXPENDITURE" };
            public readonly static IEnumerable<string> ElectricalTourExpenditure = new string[] { "ELECTRICAL_TOUR" };

            public readonly static IEnumerable<string> TransmissionExpenditure = new string[] { "TRANS_EXPENDITURE" };
            public readonly static IEnumerable<string> TransmissionTourExpenditure = new string[] { "TRANS_TOUR" };
                 
            public readonly static IEnumerable<string> TourExpenditure = new string[] { "TOUR_EXPENSES" };
        }

        public readonly static IEnumerable<string> TourExpenditure = ExpenditureSubTypes.CivilTourExpenditure
                                                                        .Concat(ExpenditureSubTypes.ElectricalTourExpenditure)
                                                                        .Concat(ExpenditureSubTypes.EstablishmentTourExpenditure)
                                                                        .Concat(ExpenditureSubTypes.TransmissionTourExpenditure);
        public readonly static IEnumerable<string> Expenditure = ExpenditureSubTypes.EstablishmentExpenditure
                                                                    .Concat(ExpenditureSubTypes.CivilExpenditure)
                                                                    .Concat(ExpenditureSubTypes.MainCivilExpenditure)
                                                                    .Concat(ExpenditureSubTypes.ElectricalExpenditure)
                                                                    .Concat(ExpenditureSubTypes.TransmissionExpenditure);
        
        public readonly static IEnumerable<string> AllExpenditures = TourExpenditure.Concat(Expenditure);
        

        public static class AdvanceSubTypes
        {
            public readonly static IEnumerable<string> PartyAdvance = new string[] { "PARTY_ADVANCE" };
            public readonly static IEnumerable<string> MaterialAdvance = new string[] { "MATERIAL_ADVANCE" };
            public readonly static IEnumerable<string> EmployeeAdvance = new string[] { "EMPLOYEE_ADVANCE" };
            public readonly static IEnumerable<string> EstablishmentPartyAdvance = new string[] { "ESTB_PARTY_ADVANCE" };
            public readonly static IEnumerable<string> CivilPartyAdance = new string[] { "CIVIL_PARTY_ADVANCE" };
            public readonly static IEnumerable<string> ElectricalPartyAdvance = new string[] { "ELEC_PARTY_ADVANCE" };
            public readonly static IEnumerable<string> TransmissionPartyAdance = new string[] { "TRAN_PARTY_ADVANCE" };

        }
        public readonly static IEnumerable<string> PartyAdances = AdvanceSubTypes.EstablishmentPartyAdvance.Concat(AdvanceSubTypes.CivilPartyAdance).Concat(AdvanceSubTypes.ElectricalPartyAdvance).Concat(AdvanceSubTypes.TransmissionPartyAdance);
        public readonly static IEnumerable<string> JobAdvances = AdvanceSubTypes.PartyAdvance.Concat(AdvanceSubTypes.MaterialAdvance);
        public readonly static IEnumerable<string> AllAdvances = JobAdvances.Concat(AdvanceSubTypes.EmployeeAdvance);


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
            public readonly static IEnumerable<string> GrantNu = new string[] {"GRANT_RECEIVED_GOINU"};
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
            public readonly static IEnumerable<string> CashInBankNu = new string[] { "BANKNU"};
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
