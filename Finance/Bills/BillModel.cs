using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PhpaAll.Bills
{
    public class BillModel
    {
        internal static IList<BillModel> FromQuery(IQueryable<Bill> query)
        {
            var results = from bill in query
                          select new BillModel
                          {
                              Amount = bill.Amount,
                              Particulars = bill.Particulars,
                              BillNumber = bill.BillNumber,
                              BillDate = bill.BillDate,
                              ContractorId = bill.ContractorId,
                              ContractorName = bill.Contractor.ContractorName,
                              SubmittedToDivisionId = bill.SubmitedToDivisionId,
                              SubmittedToDivisionName = bill.SubmittedToDivision.DivisionName,
                              DueDate = bill.DueDate,
                              PaidDate = bill.PaidDate,
                              ApprovedDate = bill.ApprovedOn,
                              ApprovedBy = bill.ApprovedBy,
                              Remarks = bill.Remarks,
                              SubmittedOnDate = bill.SubmittedOnDate,
                              BillId = bill.Id,
                              StationId = bill.StationId,
                              StationName = bill.Station.StationName,
                              CurrentDivisionId = bill.CurrentDivisionId,
                              CurrentDivisionName = bill.CurrentDivision.DivisionName
                          };
            return results.ToList();
        }

        //For internal use to retrive row for editing
        public int BillId { get; set; }

        /// <summary>
        /// If this is non null, a check box is displayed with this name and the value will be bill id
        /// </summary>
        public string CheckBoxName { get; set; }

        /// <summary>
        /// ScaffoldColumn means do not display in Excel
        /// </summary>
        [ScaffoldColumn(false)]
        public int StationId { get; set; }

        [Display(ShortName = "Station")]
        public string StationName { get; set; }

        public string BillNumber { get; set; }

        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime? BillDate { get; set; }

        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime? DueDate { get; set; }

        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime? SubmittedOnDate { get; set; }

        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime? FinanceSubmittedDate { get; set; }

        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime? PaidDate { get; set; }


        public int? SubmittedToDivisionId { get; set; }

        public string SubmittedToDivisionName { get; set; }

        public int? CurrentDivisionId { get; set; }

        public string CurrentDivisionName { get; set; }

        public int? ContractorId { get; set; }

        public string ContractorName { get; set; }

        public string Particulars { get; set; }

        [DisplayFormat(DataFormatString = "{0:N2}")]
        public Decimal? Amount { get; set; }

        public string ApprovedBy { get; set; }

        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime? ApprovedDate { get; set; }

        public string Remarks { get; set; }

        //public Image BillImage { get; set; }
    }

}