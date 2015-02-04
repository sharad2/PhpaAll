using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Drawing;

namespace PhpaAll.Bills
{
    public class BillAuditViewModel
    {
        //public BillAuditViewModel()
        //{

        //}

       public string BillCreatedBy { get; set; }

       public DateTime? DateCreated { get; set; }

       public string BillNumberOld { get; set; }

       public string BillNumberNew { get; set; }
    }
}