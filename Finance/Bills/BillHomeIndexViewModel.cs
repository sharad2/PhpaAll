using Eclipse.PhpaLibrary.Reporting;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace PhpaAll.Bills
{
    /// <summary>
    /// Contains all the information needed to display funds available at a station
    /// </summary>
    public class BillHomeIndexStationModel
    {
        [Key]
        public int StationId { get; set; }

        public string StationName { get; set; }

        [DisplayFormat(DataFormatString = "{0:C0}")]
        public decimal? FundsAvailable { get; set; }

        /// <summary>
        /// Key is month in which the amount is due. Value is the bill amount
        /// </summary>
        public IDictionary<int, decimal?> AmountsByMonth
        {
            get;
            set;
        }
    }


    /// <summary>
    /// IComparable interface allows us to sort the list based on month key
    /// </summary>
    public class BillHomeIndexMonthModel : IComparable<BillHomeIndexMonthModel>
    {
        public DateTime? MonthStartDate { get; set; }

        public DateTime? MonthEndDate { get; set; }

        /// <summary>
        /// The amounts are stored in a dictionary against this key.
        /// </summary>
        public int MonthKey { get; set; }

        [DisplayFormat(HtmlEncode = false)]
        public string DisplayName
        {
            get
            {
                if (MonthStartDate == null && MonthEndDate == null)
                {
                    return "No Due Date";
                }
                if (MonthStartDate == null)
                {
                    return string.Format("Upto {0:MMM yyyy}", MonthEndDate);
                }
                if (MonthEndDate == null)
                {
                    return string.Format("{0:MMM yyyy} and beyond", MonthStartDate);
                }
                return string.Format("{0:MMM yyyy}", MonthEndDate);
            }
        }

        /// <summary>
        /// The key is the same for all dates which belong to the same month and year
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        internal static int GetMonthKeyFromDate(DateTime? date)
        {
            if (date == null)
            {
                return 0;
            }
            return date.Value.Year * 100 + date.Value.Month;
        }

        internal static DateTime? GetMonthStartDateFromKey(int key)
        {
            if (key == 0)
            {
                return null;
            }
            return new DateTime(key / 100, key % 100, 1);
        }


        public int CompareTo(BillHomeIndexMonthModel other)
        {
            return MonthKey.CompareTo(other.MonthKey);
        }
    }




    public class BillHomeIndexViewModel
    {
        private IList<BillHomeIndexMonthModel> _allMonths;

        /// <summary>
        /// List of all possible months between min and max due dates
        /// </summary>
        public IList<BillHomeIndexMonthModel> AllMonths
        {
            get
            {
                if (_allMonths == null)
                {
                    if (Stations.Count == 0)
                    {
                        // Handle special case. No bills yet
                        _allMonths = new BillHomeIndexMonthModel[0];
                        return _allMonths;
                    }
                    var minMonthKey = Stations.SelectMany(p => p.AmountsByMonth.Keys).Where(p => p > 0).Min();  // Exclude null dates
                    var maxMonthKey = Stations.SelectMany(p => p.AmountsByMonth.Keys).Max();
                    var minMonthStartDate = BillHomeIndexMonthModel.GetMonthStartDateFromKey(minMonthKey).Value;
                    var list = new List<BillHomeIndexMonthModel>
                    {
                        new BillHomeIndexMonthModel
                        {
                            MonthStartDate = null,
                            MonthEndDate = minMonthStartDate.MonthEndDate(),
                            MonthKey = minMonthKey
                        },
                        new BillHomeIndexMonthModel
                        {
                            MonthStartDate = BillHomeIndexMonthModel.GetMonthStartDateFromKey(maxMonthKey),
                            MonthEndDate = null,
                            MonthKey = maxMonthKey
                        }
                    };

                    if (Stations.SelectMany(p => p.AmountsByMonth.Keys).Any(p => p == 0))
                    {
                        // Some bills have null due date
                        list.Add(new BillHomeIndexMonthModel
                        {
                            MonthStartDate = null,
                            MonthEndDate = null,
                            MonthKey = 0
                        });
                    }

                    for (var month = minMonthStartDate.AddMonths(1);
                        month < BillHomeIndexMonthModel.GetMonthStartDateFromKey(maxMonthKey); month = month.AddMonths(1))
                    {
                        list.Add(new BillHomeIndexMonthModel
                        {
                            MonthStartDate = month.MonthStartDate(),
                            MonthEndDate = month.MonthEndDate(),
                            MonthKey = BillHomeIndexMonthModel.GetMonthKeyFromDate(month)
                        });
                    }
                    list.Sort();
                    _allMonths = list;
                }
                return _allMonths;

            }
        }

        public IList<BillHomeIndexStationModel> Stations { get; set; }
    }
}