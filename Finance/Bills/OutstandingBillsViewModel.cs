
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
        public String GroupValue { get; set; }
        
        public string GroupDisplayName { get; set; }

        public IList<OutstandingBillModel> Bills { get; set; }

        public int DatabaseCount { get; set; }

    }
   
    public class OutstandingBillModel
    {
        [ScaffoldColumn(false)]
        public int BillId { get; set; }

        [Display(Order=10)]
        public String BillNumber { get; set; }

        [ScaffoldColumn(false)]
        public int? DivisionId { get; set; }

        [Display(Order = 20, ShortName="Division")]
        public string DivisionName { get; set; }

         [ScaffoldColumn(false)]
        public int? ContractorId { get; set; }

        [Display(Order = 30)]
        public string ContractorName { get; set; }

        [DisplayFormat(DataFormatString = "{0:d}")]
        [Display(Order = 5)]
        public DateTime? BillDate { get; set; }

        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime? DueDate { get; set; }

        [DisplayFormat(DataFormatString = "{0:C}")]
        [DataType(DataType.Currency)]
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
        //[Obsolete]
        //public string OrderByDisplayName
        //{
        //    get
        //    {
        //        switch (OrderByField)
        //        {
        //            case OrderByField.Division:
        //                return "Division";
        //            case OrderByField.Station:
        //                return "Station";
        //            case OrderByField.Contractor:
        //                return "Contractor";
        //            default:
        //                return "Unknown";
        //        }
        //    }

        //}
        /// <summary>
        /// URL which will cause the current data to be displayed in Excel
        /// </summary>
        public string UrlExcel { get; set; }

        public bool? OverDueOnly { get; set; }

        public IList<OutstandingBillGroupModel> BillGroups { get; set; }

        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal? TotalAmount
        {

            get
            {
                return BillGroups.SelectMany(p => p.Bills).Sum(p => p.Amount);
            }
        }
    }
}
