using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Drawing;

namespace PhpaAll.Bills
{
    public class BillAuditModel
    {
        //public BillAuditViewModel()
        //{

        //}

        public string BillCreatedBy { get; set; }

        [DataType(DataType.Date)]
        public DateTime? DateCreated { get; set; }

        public string BillNumberOld { get; set; }

        public string BillNumberNew { get; set; }

        [DataType(DataType.Date)]
        public DateTime? DueDateNew { get; set; }

        [DataType(DataType.Date)]
        public DateTime? DueDateOld { get; set; }

        public decimal? AmountNew { get; set; }

        public decimal? AmountOld { get; set; }

        public string ApprovedByNew { get; set; }

        public string ApprovedByOld { get; set; }

        [DataType(DataType.Date)]
        public DateTime? ApprovedOnNew { get; set; }

        [DataType(DataType.Date)]
        public DateTime? ApprovedOnOld { get; set; }

        [DataType(DataType.Date)]
        public DateTime? BillDateNew { get; set; }

        [DataType(DataType.Date)]
        public DateTime? BillDateOld { get; set; }

        public string ContractorNameNew { get; set; }

        public string ContractorNameOld { get; set; }

        [DataType(DataType.Date)]
        public DateTime? PaidDateNew { get; set; }

        [DataType(DataType.Date)]
        public DateTime? PaidDateOld { get; set; }

        public string ParticularsNew { get; set; }

        public string ParticularsOld { get; set; }

        public string RemarksNew { get; set; }

        public string RemarksOld { get; set; }

        [Obsolete]
        public int? StationIdNew { get; set; }

        [Obsolete]
        public int? StationIdOld { get; set; }

        [DataType(DataType.Date)]
        public DateTime? SubmittedOnDateNew { get; set; }

        [DataType(DataType.Date)]
        public DateTime? SubmittedOnDateOld { get; set; }

        public string SubmittedToDivisionNameNew { get; set; }

        public string SubmittedToDivisionNameOld { get; set; }
    }
}