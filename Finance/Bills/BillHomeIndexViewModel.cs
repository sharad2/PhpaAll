﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace PhpaAll.Bills
{
    public class BillHomeIndexStationModel
    {
        [Key]
        public string StationName { get; set; }

        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal? FundsAvailable { get; set; }

        /// <summary>
        /// Key is month in which the amount is due. Value is the bill amount
        /// </summary>
        public IDictionary<DateTime, decimal?> AmountsByMonth
        {
            get;
            set;
        }
    }




    public class BillHomeIndexViewModel
    {
        private IList<DateTime> _allMonths;

        /// <summary>
        /// List of all possible months between min and max due dates
        /// </summary>
        public IList<DateTime> AllMonths
        {
            get
            {
                if (_allMonths == null)
                {
                    var minMonth = Stations.SelectMany(p => p.AmountsByMonth.Keys).Min();
                    var maxMonth = Stations.SelectMany(p => p.AmountsByMonth.Keys).Max();
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