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
        [DisplayFormat (DataFormatString="{0:C}")]
        public decimal? Amount { get; set; }
    }

    public class BillHomeIndexStationModel
    {
        public BillHomeIndexStationModel()
        {
            Amounts = new List<BillHomeStationAmountModel>();

            for (var i = 0; i < 12; ++i)
            {
                Amounts.Add(null);
            }
        }

        [Key]
        public string StationName { get; set; }

        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal? FundsAvailable { get; set; }

        public IList<BillHomeStationAmountModel> Amounts
        {
            get;
            private set;
        }
    }




    public class BillHomeIndexViewModel
    {

        public IList<BillHomeIndexStationModel> Stations { get; set; }
    }
}