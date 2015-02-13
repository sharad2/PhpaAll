//using PhpaBills.Database;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace PhpaAll.Bills
{

    public class RecentBillsFilterModel
    {
        public string Id { get; set; }

        [DisplayFormat(NullDisplayText = "(Not Set)")]
        public string Name { get; set; }

        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int Count { get; set; }

        /// <summary>
        /// Whether this option should be initially selected
        /// </summary>
        public bool Selected { get; set; }
    }


    public class RecentBillsViewModel
    {
        public IList<RecentBillsFilterModel> Divisions { get; set; }

        /// <summary>
        /// Returns null if all divisions are selected. Else retuns the count of selected divisions
        /// </summary>
        [DisplayFormat(DataFormatString="{0:N0}")]
        public int? SelectedDivisionsCount
        {
            get
            {
                if (Divisions.All(p => p.Selected))
                {
                    return null;
                }
                return Divisions.Count(p => p.Selected);
            }
        }

        public IList<RecentBillsFilterModel> ProcessingDivisions { get; set; }

        /// <summary>
        /// Returns null if all contractors are selected. Else retuns the count of selected contractors.
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? SelectedProcessingDivisionsCount
        {
            get
            {
                if (ProcessingDivisions.All(p => p.Selected))
                {
                    return null;
                }
                return ProcessingDivisions.Count(p => p.Selected);
            }
        }

        public IList<RecentBillsFilterModel> Contractors { get; set; }

        /// <summary>
        /// Returns null if all contractors are selected. Else retuns the count of selected contractors.
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? SelectedContracatorsCount
        {
            get
            {
                if (Contractors.All(p => p.Selected))
                {
                    return null;
                }
                return Contractors.Count(p => p.Selected);
            }
        }

        public IList<RecentBillsFilterModel> Approvers { get; set; }

        /// <summary>
        /// Returns null if all approvers are selected. Else retuns the count of selected approvers.
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? SelectedApproversCount
        {
            get
            {
                if (Approvers.All(p => p.Selected))
                {
                    return null;
                }
                return Approvers.Count(p => p.Selected);
            }
        }

        public IList<RecentBillsFilterModel> Stations { get; set; }

        /// <summary>
        /// Returns null if all stations are selected. Else retuns the count of selected stations.
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? SelectedStationsCount
        {
            get
            {
                if (Stations.All(p => p.Selected))
                {
                    return null;
                }
                return Stations.Count(p => p.Selected);
            }
        }

        public IList<BillModel> Bills { get; set; }

        public bool IsFiltered { get; set; }

        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime? DateFrom { get; set; }

        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime? DateTo { get; set; }

        [DisplayFormat(DataFormatString = "{0:N2}")]
        public Decimal? FilterMinAmount { get; set; }

        [DisplayFormat(DataFormatString = "{0:N2}")]
        public Decimal? FilterMaxAmount { get; set; }

        /// <summary>
        /// True means approved bills, false means unapproved bills, null means all
        /// </summary>
        public bool? FilterApprovedBills;

        /// <summary>
        /// True means paid bills, false means unpaid bills, null means all
        /// </summary>
        public bool? FilterPaidBills;

        /// <summary>
        /// URL which will cause the current data to be displayed in Excel
        /// </summary>
        public string UrlExcel { get; set; }
    }
}