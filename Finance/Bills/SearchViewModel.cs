//using PhpaBills.Database;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PhpaAll.Bills
{
    public class SearchModel
    {
        //For internal use to retrive row for editing
        public int Id { get; set; }

        public string BillNumber { get; set; }

        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime? BillDate { get; set; }

    }

    public class SearchViewModel
    {
        public IList<SearchModel> Bills { get; set; }
    }

}