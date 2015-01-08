
using System.Collections.Generic;
using System.Linq;

namespace Eclipse.PhpaLibrary.Reporting
{
    public static class HeadOfAccountHelpers
    {

        public readonly static IEnumerable<string> JobExpenses = new string[] { "EXPENDITURE", "TOUR_EXPENSES" };

        public static class JobAdvanceSubTypes
        {
            public readonly static IEnumerable<string> PartyAdvance = new string[] { "PARTY_ADVANCE" };
            public readonly static IEnumerable<string> MaterialAdvance = new string[] { "MATERIAL_ADVANCE" };
        }

        public readonly static IEnumerable<string> JobAdvances = JobAdvanceSubTypes.PartyAdvance.Concat(JobAdvanceSubTypes.MaterialAdvance); //new string[] { "PARTY_ADVANCE", "MATERIAL_ADVANCE" };
        public readonly static IEnumerable<string> Grants = new string[] { "GRANT_RECEIVED_GOINU", "GRANT_RECEIVED_GOIFE" };
        public readonly static IEnumerable<string> Loans = new string[] { "LOAN_RECEIVED_GOINU", "LOAN_RECEIVED_GOIFE" };
        public readonly static IEnumerable<string> GreenTaxes = new string[] { "GREEN_TAX" };
        public readonly static IEnumerable<string> AccumulatedReceipts = new string[] { "ACCUMULATED_RECEIPTS", "TENDER_SALE" };
    }
}
