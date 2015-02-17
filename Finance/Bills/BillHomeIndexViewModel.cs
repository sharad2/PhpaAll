using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PhpaAll.Bills
{
    public class BillHomeIndexStationModel
    {
        [Key]
        public string StationName { get; set; }

        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal? FundsAvailable { get; set; }

        public IDictionary<DateTime, decimal?> AmountDictionary
        {
            get;
            set;
        }
    }




    public class BillHomeIndexViewModel
    {
        private IList<DateTime> _allMonths;

        public IList<DateTime> AllMonths
        {
            get
            {
                if (_allMonths == null)
                {
                    var minMonth = Stations.SelectMany(p => p.AmountDictionary.Keys).Min();
                    var maxMonth = Stations.SelectMany(p => p.AmountDictionary.Keys).Max();
                    _allMonths = new List<DateTime>();

                    for (var month = minMonth; month <= maxMonth; month = month.AddMonths(1))
                    {
                        _allMonths.Add(month);
                    }
                }
                return _allMonths;

            }
        }

        public IList<BillHomeIndexStationModel> Stations { get; set; }
    }
}