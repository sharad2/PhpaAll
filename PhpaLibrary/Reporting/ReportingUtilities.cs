using System;
using System.Linq;
using System.Web.UI.WebControls;
using Eclipse.PhpaLibrary.Database.Payroll;
using System.Collections.Generic;


namespace Eclipse.PhpaLibrary.Reporting
{
    /// <summary>
    /// All start dates are inclusive. All end dates are exclusive.
    /// </summary>
    public static class ReportingUtilities
    {
        /// <summary>
        /// All services and data sources use this connect string
        /// </summary>
        public static string DefaultConnectString
        {
            get
            {
                return System.Configuration.ConfigurationManager.ConnectionStrings["default"].ConnectionString;
            }
        }

        /// <summary>
        /// Returns a comma seperated list of asset and liability head types
        /// </summary>
        //public static string AllHeadTypes
        //{
        //    get
        //    {
        //        return string.Join(",", m_assetheadTypes) + "," + string.Join(",", m_liabilityheadTypes);
        //    }
        //}

        //public static string LiabilityHeadTypesCommaSeperated
        //{
        //    get
        //    {
        //        return string.Join(",", m_liabilityheadTypes);
        //    }
        //}


        public const string MONEY_FORMAT_SPECIFIER = "###,###,###,###,###.##";
        /// <summary>
        /// For the passed date, returns the date wqhich corresponds to the first of the month.
        /// This is an inclusive date and should be included in your date range.
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DateTime MonthStartDate(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, 1);
        }

        /// <summary>
        /// Returns the date on which this month ends.
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DateTime MonthEndDate(this DateTime date)
        {
            // date = 13 Jun 2008
            DateTime dateTemp = date.AddMonths(1);  // 13 Jul 2008
            dateTemp = new DateTime(dateTemp.Year, dateTemp.Month, 1);  // 1 Jul 2008
            dateTemp = dateTemp.AddDays(-1);        // 30 Jun 2008
            return dateTemp;
        }

        /// <summary>
        /// Returns the starting date of the financial year to which the passed date belongs.
        /// It reurns 1st April of the financial year.
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DateTime FinancialYearStartDate(this DateTime date)
        {
            int intyear;

            if (date.Month < 4)
                intyear = date.Year - 1;
            else
                intyear = date.Year;

            return new DateTime(intyear, 4, 1);
        }

        /// <summary>
        /// Returns the ending date of the financial year to which the passed date belongs.
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DateTime FinancialYearEndDate(this DateTime date)
        {
            int intyear;

            if (date.Month > 3)
                intyear = date.Year + 1;
            else
                intyear = date.Year;

            return new DateTime(intyear, 3, 31);
        }

        //public static int FindByAccessibleHeaderText(this DataControlFieldCollection dcfColl, string accessibleHeaderText)
        //{
        //    int i = 0;
        //    foreach (DataControlField dcf in dcfColl)
        //    {

        //        if (dcf.AccessibleHeaderText == accessibleHeaderText)
        //        {
        //            return i;
        //        }
        //        i++;

        //    }
        //    throw new ArgumentOutOfRangeException(string.Format("No {0} accessibleHeaderText found", accessibleHeaderText));

        //}

        /// <summary>
        /// Returns -1 if not found
        /// </summary>
        /// <param name="dcfColl"></param>
        /// <param name="accessibleHeaderText"></param>
        /// <returns></returns>
        //public static int TryFindByAccessibleHeaderText(this DataControlFieldCollection dcfColl, string accessibleHeaderText)
        //{
        //    int i = 0;
        //    foreach (DataControlField dcf in dcfColl)
        //    {
        //        if (dcf.AccessibleHeaderText == accessibleHeaderText)
        //        {
        //            return i;
        //        }
        //        i++;

        //    }
        //    return -1;
        //}

        /// <summary>
        /// Get 12345. Return VR000-012-345X where X is modulo 10 checkdigit as defined in
        /// http://en.wikipedia.org/wiki/Luhn_algorithm
        /// </summary>
        /// <param name="voucherId"></param>
        /// <returns></returns>
        public static string VoucherIdToVoucherReferenceNumber(int voucherId)
        {
            //int l = voucherId.ToString().Length;
           int checkDigit= CalculateCheckSumDigit(voucherId);
           string str = string.Format("{0:'VR'000'-'000'-'000'}{1}", voucherId, checkDigit);
           return str;
        }

        
        
        private static int CalculateCheckSumDigit(int voucherId)
        {
            int sum = 0;
            char[] strVoucherId = voucherId.ToString().ToCharArray();
            bool evenPosition = false;
            int temp;
            for (int i = strVoucherId.Length-1 ; i >= 0; i--)
            {
                temp = int.Parse(strVoucherId[i].ToString());

                if (evenPosition)
                {

                    temp = temp * 2;
                    if (temp > 9)
                    {
                        int rem = temp % 10;
                        temp = 1 + rem;
                    }
                }
                sum += temp;

                evenPosition = (!evenPosition);
            }
            return (sum % 10);
        }

        /// <summary>
        /// Returns -1 if check digit is invalid
        /// </summary>
        /// <param name="voucherReference"></param>
        /// <returns></returns>
        public static int VoucherReferenceNumberToVoucherId(string voucherReference)
        {
            // Input: VR123-456-7891; strId becomes 123456789
            string strId = voucherReference.Substring(2, 11).Replace("-", "");
            int voucherId = int.Parse(strId);
            string strCheckDigitPassed = voucherReference.Substring(voucherReference.Length-1, 1);
            int checkDigitPassed = int.Parse(strCheckDigitPassed);
            int checkDigitCalculated = CalculateCheckSumDigit(voucherId);
            if (checkDigitCalculated != checkDigitPassed)
            {
                // Data entry error
                return -1;
            }

            return voucherId;
        }

        public static decimal? AddNullable(decimal? value1, decimal? value2)
        {
            if (value1 == null)
            {
                return value2;
            }
            if (value2 == null)
            {
                return value1;
            }
            // Both are not null
            return value1.Value + value2.Value;
        }

        public static decimal? SubtractNullable(decimal? value1, decimal? value2)
        {
            if (value1 == null)
            {
                if (value2.HasValue)
                {
                    return -value2.Value;
                }
                else
                {
                    return null;
                }
            }
            if (value2 == null)
            {
                return value1;
            }
            // Both are not null
            return value1.Value - value2.Value;
        }

        //private static Dictionary<AdjustmentCategory, decimal?> dictAdjCatsum = new Dictionary<AdjustmentCategory,decimal?>();
        /// <summary>
        /// Evaluates the sum against all the adjustments and saves the same in dictionary m_dictAdjCatsum
        /// </summary>
        /// <param name="amount"></param>
        /// <param name="adj"></param>
        public static void UpdateSum(Dictionary<AdjustmentCategory, decimal?> dictAdjCatsum,
            decimal? amount, AdjustmentCategory adj)
        {
            decimal? curValue;
            bool bFound = dictAdjCatsum.TryGetValue(adj, out curValue);
            if (bFound)
            {
                dictAdjCatsum[adj] = ReportingUtilities.AddNullable(amount, curValue);
            }
            else
            {
                dictAdjCatsum.Add(adj, amount);
            }
            return;
        }

    }
}
