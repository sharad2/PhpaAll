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

       public DateTime? DateCreated { get; set; }

       public string BillNumberOld { get; set; }

       public string BillNumberNew { get; set; }

       public DateTime? DueDateNew { get; set; }

       public DateTime? DueDateOld { get; set; }

       public decimal? AmountNew { get; set; }

       public decimal? AmountOld { get; set; }

       public string ApprovedByNew { get; set; }

       public string ApprovedByOld { get; set; }

       public DateTime? ApprovedOnNew { get; set; }

       public DateTime? ApprovedOnOld { get; set; }

       public DateTime? BillDateNew { get; set; }

       public DateTime? BillDateOld { get; set; }

       public string ContractorNameNew { get; set; }

       public string ContractorNameOld { get; set; }

       public DateTime? PaidDateNew { get; set; }

       public DateTime? PaidDateOld { get; set; }

       public string ParticularsNew { get; set; }

       public string ParticularsOld { get; set; }

       public string RemarksNew { get; set; }

       public string RemarksOld { get; set; }

       public int? StationIdNew { get; set; }

       public int? StationIdOld { get; set; }

       public DateTime? SubmittedOnDateNew { get; set; }

       public DateTime? SubmittedOnDateOld { get; set; }

       public string SubmittedToDivisionNameNew { get; set; }

       public string SubmittedToDivisionNameOld { get; set; }
    }
}