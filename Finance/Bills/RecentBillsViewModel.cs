//using PhpaBills.Database;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Linq;
using System.Web;

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


    public class RecentBillsViewModel
    {
        public IList<BillModel> Bills { get; set; }
    }
}