
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PhpaAll.Bills
{
   public class OutstandingBillModel {

       public int BillId { get; set; }
       
       public String BillNumber { get; set; }

       public int? SubmittedToDivisionId { get; set; }

       public string SubmittedToDivisionName { get; set; }

       public int? ContractorId { get; set; }

       public string ContractorName { get; set; }

       [DisplayFormat(DataFormatString = "{0:d}")]
       public DateTime? BillDate { get; set; }

       [DisplayFormat(DataFormatString = "{0:d}")]
       public DateTime? DueDate { get; set; }

       [DisplayFormat(DataFormatString = "{0:N2}")]
       public Decimal? Amount { get; set; }
   }

    public class OutstandingBillsViewModel
    {
        public IList<OutstandingBillModel> Bills { get; set; }
    }
}
