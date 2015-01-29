using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Drawing;

namespace PhpaAll.Bills
{

    public class CreateViewModel
    {

        [Display(Name = "Bill")]
        [StringLength(60, ErrorMessage = "The Bill number cannot exceed 60 characters.")]
        [Required (ErrorMessage="Bill number is required.")]
        public string BillNumber { get; set; }

        [Display(Name = "Bill Date")]
        [DataType(DataType.Date)]
        [Required(ErrorMessage="Bill date is required.")]
        public DateTime? BillDate { get; set; }

        [Display(Name = "Due Date")]
        [DataType(DataType.Date)]
        public DateTime? DueDate { get; set; }

        [Display(Name = "Submitted to division")]
        [DataType(DataType.Date)]
        public DateTime? DivisionSubmittedDate { get; set; }

        [Display(Name = "Submitted to finance")]
        [DataType(DataType.Date)]
        public DateTime? FinanceSubmittedDate { get; set; }

        [Display(Name = "Pro Date")]
        [DataType(DataType.Date)]
        public DateTime? PaidDate { get; set; }

        [Display(Name = "Bill Type")]
        [StringLength(10, ErrorMessage = "The Bill Type value cannot exceed 10 characters. ")]
        public string BillType { get; set; }

        [Display(Name = "Division Id")]
        [Required (ErrorMessage="Division Id is required.")]
        public int? DivisionId { get; set; }

        [Display(Name = "Contractor Id")]
        public int? ContractorId { get; set; }

        [Display(Name = "Amount")]
        [Required (ErrorMessage="Amount is required.")]
        public Decimal? Amount { get; set; }

        [Display(Name = "Approved by")]
        [StringLength(50, ErrorMessage = "You can insert only 50 characters ")]
        public string ApprovedBy { get; set; }

        [Display(Name = "Approved by")]
        [DataType(DataType.Date)]
        public DateTime? ApprovedDate { get; set; }

        [Display(Name = "Remarks")]
        [StringLength(255, ErrorMessage = "Max limit is 255 characters")]
        public string Remarks { get; set; }

        [Display(Name = "Image")]
        public Image BillImage { get; set; }
    }
}