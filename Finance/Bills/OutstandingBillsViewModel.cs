
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace PhpaAll.Bills
{
    public enum OrderByField
    {
        Division,
        Station,
        Contractor
    }

    public class OutstandingBillGroupModel
    {
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal? GroupTotal { get; set; }

        [DisplayFormat(NullDisplayText = "(Not Set)")]
        public String OrderByValue { get; set; }

        public IList<OutstandingBillModel> Bills { get; set; }

        public int DatabaseCount { get; set; }
    }
   
    public class OutstandingBillModel
    {

        public int BillId { get; set; }

        public String BillNumber { get; set; }

        public int? SubmittedToDivisionId { get; set; }

        public string SubmittedToDivisionName { get; set; }

        public int? ContractorId { get; set; }

        public string ContractorName { get; set; }

        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime? BillDate { get; set; }

        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime? DueDate { get; set; }

        [DisplayFormat(DataFormatString = "{0:N2}")]
        public Decimal? Amount { get; set; }

        [Obsolete]
        [DisplayFormat(NullDisplayText="(Not Set)")]
        public String OrderByValue { get; set; }

        public string StationName { get; set; }

        [Obsolete]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal? GroupTotal { get; set; }
    }

    public class OutstandingBillsViewModel
    {
        public OrderByField OrderByField { get; set; }
        public string OrderByDisplayName
        {
            get
            {
                switch (OrderByField)
                {
                    case OrderByField.Division:
                        return "Division";
                    case OrderByField.Station:
                        return "Station";
                    case OrderByField.Contractor:
                        return "Contractor";
                    default:
                        return "Unknown";
                }
            }

        }
        public bool? OverDueOnly { get; set; }

        [Obsolete]
        public IList<OutstandingBillModel> Bills { get; set; }
        public IList<OutstandingBillGroupModel> BillGroups { get; set; }


        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal? TotalAmount
        {

            get
            {
                return Bills.Sum(p => p.Amount);
            }
        }
    }
}
