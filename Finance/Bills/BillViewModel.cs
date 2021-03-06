﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Drawing;
using System.Data.Linq;

namespace PhpaAll.Bills
{
    public class BillViewModel
    {

        public int BillId { get; set; }

        [Display(Name = "Bill")]
        [StringLength(60, ErrorMessage = "The Bill number cannot exceed 60 characters.")]
        [Required(ErrorMessage = "Bill number is required.")]
        public string BillNumber { get; set; }

        [Display(Name = "Bill Date")]
        [DataType(DataType.Date)]
        [Required(ErrorMessage = "Bill date is required.")]
        [DisplayFormat(DataFormatString="{0:d}")]
        public DateTime? BillDate { get; set; }

        [Display(Name = "Due Date")]
        [DataType(DataType.Date)]
        public DateTime? DueDate { get; set; }

        [Display(Name = "Submitted to division")]
        [DataType(DataType.Date)]
        public DateTime? ReceivedDate { get; set; }

        //[Display(Name = "Bill Type")]
        //[StringLength(10, ErrorMessage = "The Bill Type value cannot exceed 10 characters. ")]
        //public string BillType { get; set; }

        [Display(Name = "Division Id")]
        [Required(ErrorMessage = "Division Id is required.")]
        
        public int? DivisionId { get; set; }

      
        public string DivisionName { get; set; }

        [Display(Name = "Contractor Id")]
        public int? ContractorId { get; set; }

        public string ContractorName { get; set; }


        [Display(Name = "Amount")]
        [DisplayFormat(DataFormatString="{0:N2}")]
        public Decimal? Amount { get; set; }

        [Display(Name = "Bill For")]
        [StringLength(255, ErrorMessage = "You can insert only 255 characters ")]
        public string Particulars { get; set; }


        [Display(Name = "Remarks")]
        public string Remarks { get; set; }



        //[Display(Name = "Image")]
        //public Binary BillImage { get; set; }

        //Weather it is edit mode or not
        public bool isEditMode { get; set; }

        public IList<BillAuditModel> BillHistory { get; set; }

        [Display(Name = "Current Division")]
        public string AtDivision { get; set; }

        [Display(Name = "Paid On")]
        [DataType(DataType.Date)]
        public DateTime? VoucherDate { get; set; }

        public int? VoucherId { get; set; } 

        [DataType(DataType.Date)]
        public DateTime? ApprovedDate { get; set; }

        public string ApprovedBy { get; set; }

        public string StationName { get; set; }

        public int AttachedImageCount { get; set; }



        public string VoucherCode { get; set; }

        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime? BillCreateDate { get; set; }

        public string BillCreatedby { get; set; }
    }

}