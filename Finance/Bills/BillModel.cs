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
                              CurrentDivisionName = bill.AtDivision.DivisionName,
                              VoucherDate = bill.Voucher.VoucherDate,
                              VoucherId = bill.Voucher.VoucherId
                          };
            return results;
        }

        //For internal use to retrive row for editing
        public int BillId { get; set; }

        /// <summary>
        /// If this is non null, a check box is displayed with this name and the value will be bill id.
        /// Additionally, a button is displayed for approving the bill
        /// </summary>
        [ScaffoldColumn(false)]
        public string CheckBoxName { get; set; }

        [Display(ShortName = "Station")]
        public string StationName { get; set; }

        public string BillNumber { get; set; }

        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime? BillDate { get; set; }

        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime? DueDate { get; set; }

        public string DivisionName { get; set; }

        [DisplayFormat(NullDisplayText="(Unknown)")]
        public string CurrentDivisionName { get; set; }

        public string ContractorName { get; set; }

        public string Particulars { get; set; }

        [DisplayFormat(DataFormatString = "{0:N2}")]
        public Decimal? Amount { get; set; }

        public string ApprovedBy { get; set; }

        /// <summary>
        /// Display time as well
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:g}")]
        public DateTime? ApprovedDate { get; set; }
        
        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime? VoucherDate { get; set; }

        public int? VoucherId { get; set; }
    }

}