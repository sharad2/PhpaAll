using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PhpaAll.Bills
{
    public class BillHomeStationAmountModel
    {
        public int DueInMonth { get; set; }
        public decimal? Amount { get; set; }
    }

    public class BillHomeIndexStationModel
    {
        [Key]
        public string StationName { get; set; }

        public decimal? FundsAvailable { get; set; }

        public IList<BillHomeStationAmountModel> Amounts { get; set; }
    }




    public class BillHomeIndexViewModel
    {

        public IList<BillHomeIndexStationModel> Stations { get; set; }
    }
}