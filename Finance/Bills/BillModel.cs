using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace PhpaAll.Bills
{

    public class BillModel
    {
        internal static IQueryable<T> FromQuery<T>(IQueryable<Bill> query) where T : BillModel, new()
        {
            var results = from bill in query
                          select new T
                          {
                              Amount = bill.Amount,
                              Particulars = bill.Particulars,
                              BillNumber = bill.BillNumber,
                              BillDate = bill.BillDate,
                              ContractorName = bill.Contractor.ContractorName,
                              DivisionName = bill.Division.DivisionName,
                              DueDate = bill.DueDate,
                              ApprovedDate = bill.ApprovedOn,
                              ApprovedBy = bill.ApprovedBy,
                              BillId = bill.Id,
                              StationName = bill.Station.StationName,
                              AtDivisionName = bill.AtDivision.DivisionName,
                              VoucherDate = bill.Voucher.VoucherDate,
                              VoucherId = bill.Voucher.VoucherId,
                              Remarks = bill.Remarks,
                              ReceivedDate = bill.ReceivedDate,
                              CreatedBy = bill.CreatedBy,
                              CreatedDate = bill.Created,
                              VoucherCode = bill.Voucher.VoucherCode
                          };
            return results;
        }

        //For internal use to retrive row for editing
        [ScaffoldColumn(false)]
        public int BillId { get; set; }

        /// <summary>
        /// If this is non null, a check box is displayed with this name and the value will be bill id.
        /// Additionally, a button is displayed for approving the bill
        /// </summary>
        [ScaffoldColumn(false)]
        public string CheckBoxName { get; set; }

        [DisplayFormat(DataFormatString = "{0:d}")]
        [Display(Order=5, ShortName = "Bill Date")]
        public DateTime? BillDate { get; set; }

        [Display(Order=10, ShortName="Bill Number")]
        public string BillNumber { get; set; }

        [Display(Order=20, ShortName = "Division")]
        public string DivisionName { get; set; }

        [Display(Order=30, ShortName = "Contractor")]
        public string ContractorName { get; set; }

        [DisplayFormat(DataFormatString = "{0:d}")]
        [Display(Order=40, ShortName = "Due Date")]
        public DateTime? DueDate { get; set; }

        [DisplayFormat(DataFormatString = "{0:C2}")]
        [DataType(DataType.Currency)]
        [Display(Order=50)]
        public Decimal? Amount { get; set; }

        [Display(Order=60, ShortName = "Station")]
        public string StationName { get; set; }

        [Display(ShortName = "Approved By")]
        public string ApprovedBy { get; set; }

        /// <summary>
        /// Display time as well
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:d}")]
        [Display(ShortName = "Approved On")]
        public DateTime? ApprovedDate { get; set; }

        [DisplayFormat(NullDisplayText="(Unknown)")]
        [Display(ShortName = "Current Division")]
        public string AtDivisionName { get; set; }

        [DisplayFormat(DataFormatString = "{0:d}")]
        [Display(ShortName = "Received On")]
        public DateTime? ReceivedDate { get; set; }

        [Display(ShortName = "Bill Created By")]
        public string CreatedBy { get; set; }

        [DisplayFormat(DataFormatString = "{0:d}")]
        [Display(ShortName = "Bill Created On")]
        public DateTime? CreatedDate { get; set; }

        public string Particulars { get; set; }

        public string Remarks { get; set; }   
        
        [ScaffoldColumn(false)]
        [DisplayFormat(DataFormatString = "{0:d}")]
        [Display(ShortName = "Voucher Date")]
        public DateTime? VoucherDate { get; set; }

        [Display(ShortName="Voucher Number")]
        public string VoucherCode { get; set; } 
        [ScaffoldColumn(false)]
        public int? VoucherId { get; set; }
    }

}