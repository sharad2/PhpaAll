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
        public BillModel()
        {

        }
   
        internal BillModel(Bill bill)
        {
            Amount = bill.Amount;
            ApprovedBy = bill.ApprovedBy;
            BillNumber = bill.BillNumber;
            ApprovedDate = bill.ApprovedOn;
            BillDate = bill.BillDate;
            //BillImage = model.BillImage,
            BillType = bill.BillType;
            ContractorId = bill.ContractorId;
            DivisionId = bill.DivisionId;
            DueDate = bill.DueDate;
            PaidDate = bill.PaidOn;
            Remarks = bill.Remarks;
            DivisionSubmittedDate = bill.SubmittedToDivision;
            FinanceSubmittedDate = bill.SubmittedToFinance;
            Id = bill.Id;
        }
        //For internal use to retrive row for editing
        public int Id { get; set; }

        public string BillNumber { get; set; }

        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime? BillDate { get; set; }

        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime? DueDate { get; set; }

        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime? DivisionSubmittedDate { get; set; }

         [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime? FinanceSubmittedDate { get; set; }

        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime? PaidDate { get; set; }

        public string BillType { get; set; }

        public int? DivisionId { get; set; }

        public int? ContractorId { get; set; }

        
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