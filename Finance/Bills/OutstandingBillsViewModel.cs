
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

        [Display(Order=10, ShortName="Bill Number")]
        public String BillNumber { get; set; }

        [ScaffoldColumn(false)]
        public int? DivisionId { get; set; }

        [Display(Order = 20, ShortName="Division")]
        public string DivisionName { get; set; }

         [ScaffoldColumn(false)]
        public int? ContractorId { get; set; }

        [Display(Order = 30, ShortName="Contractor")]
        public string ContractorName { get; set; }

        [DisplayFormat(DataFormatString = "{0:d}")]
        [Display(Order = 5, ShortName="Bill Date")]
        public DateTime? BillDate { get; set; }

        [DisplayFormat(DataFormatString = "{0:d}")]
        [Display(Order=40, ShortName="Due Date")]
        public DateTime? DueDate { get; set; }

        [DisplayFormat(DataFormatString = "{0:C2}")]
        [DataType(DataType.Currency)]
        [Display(Order=50)]
        public Decimal? Amount { get; set; }

        [Obsolete]
        [ScaffoldColumn(false)]
        [DisplayFormat(NullDisplayText="(Not Set)")]
        public String OrderByValue { get; set; }

        [Display(Order=60, ShortName="Station")]
        public string StationName { get; set; }

        [Obsolete]
        [ScaffoldColumn(false)]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal? GroupTotal { get; set; }


        [Display(ShortName="Approved By")]
        public string ApprovedBy { get; set; }

        [DisplayFormat(DataFormatString = "{0:d}")]
        [Display(ShortName="Approved On")]
        public DateTime? ApprovedDate { get; set; }

        [Display(ShortName="Current Division")]
        public string CurrentDivision { get; set; }

        [DisplayFormat(DataFormatString = "{0:d}")]
        [Display(ShortName = "Received On")]
        public DateTime? ReceivedDate { get; set; }

        [Display(ShortName="Bill Created By")]
        public string CreatedBy { get; set; }

        [DisplayFormat(DataFormatString = "{0:d}")]
        [Display(ShortName = "Bill Created On")]
        public DateTime? CreatedDate { get; set; }



        public string Particulars { get; set; }

        public string Remarks { get; set; }

        
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
