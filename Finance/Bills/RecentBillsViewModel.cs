//using PhpaBills.Database;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PhpaAll.Bills
{
    public class BillModel
    {
        //public BillModel()
        //{

        //}
   
        //[Obsolete]
        //internal BillModel(Bill bill)
        //{
        //    Amount = bill.Amount;
        //    Particulars = bill.Particulars;
        //    BillNumber = bill.BillNumber;
        //    BillDate = bill.BillDate;
        //    //BillImage = model.BillImage,          
        //    ContractorId = bill.ContractorId;
        //    SubmittedToDivisionId = bill.SubmitedToDivisionId;
        //    DueDate = bill.DueDate;
        //    PaidDate = bill.PaidDate;
        //    Remarks = bill.Remarks;
        //    SubmittedOnDate = bill.SubmittedOnDate;
        //    Id = bill.Id;
        //}
        //For internal use to retrive row for editing
        public int Id { get; set; }

        public int StationId { get; set; }

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

    public class RecentBillsFilterModel
    {
        public string Id { get; set; }

        [DisplayFormat(NullDisplayText="(Not Set)")]
        public string Name { get; set; }

        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int Count { get; set; }

        /// <summary>
        /// Whether this option should be initially selected
        /// </summary>
        public bool Selected { get; set; }
    }


    public class RecentBillsViewModel
    {
        public IList<RecentBillsFilterModel> Divisions { get; set; }

        public IList<RecentBillsFilterModel> Contractors { get; set; }

        public IList<RecentBillsFilterModel> Approvers { get; set; }

        public IList<BillModel> Bills { get; set; }

        public bool IsFiltered { get; set; }
    }
}