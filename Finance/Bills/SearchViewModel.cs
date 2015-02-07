//using PhpaBills.Database;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PhpaAll.Bills
{

    public class SearchBillModel : BillModel
    {

    }

    public class SearchViewModel
    {
        public IList<SearchBillModel> Bills { get; set; }
    }

}