using System;
using System.Data.Linq;

namespace Eclipse.PhpaLibrary.Database.MIS
{
    partial class MISDataContext
    {
        partial void UpdatePackage(Package instance)
        {
            SetAuditFields(instance, ChangeAction.Update);
            this.ExecuteDynamicUpdate(instance);
        }

        partial void InsertPackage(Package instance)
        {
            SetAuditFields(instance, ChangeAction.Insert);
            this.ExecuteDynamicInsert(instance);
        }

        partial void UpdateActivity(Activity instance)
        {
            instance.ActivityCode = Activity.PadActivityCode(instance.ActivityCode);
            SetAuditFields(instance, ChangeAction.Update);
            this.ExecuteDynamicUpdate(instance);
        }

        partial void InsertActivity(Activity instance)
        {
            instance.ActivityCode = Activity.PadActivityCode(instance.ActivityCode);
            SetAuditFields(instance, ChangeAction.Insert);
            this.ExecuteDynamicInsert(instance);
        }

        /// <summary>
        /// Activity code is expected to be dot seperated. We pad each component to two characters.
        /// </summary>
        /// <param name="instance"></param>
        //private static void FixActivityCode(Activity instance)
        //{
        //    string[] tokens = instance.ActivityCode.Split('.');
        //    int result;
        //    for (int i = 0; i < tokens.Length; ++i)
        //    {
        //        bool b = int.TryParse(tokens[i], out result);
        //        if (b)
        //        {
        //            // Numeric
        //            tokens[i] = tokens[i].PadLeft(2, '0');
        //        }
        //        else
        //        {
        //            // Non numeric
        //            //tokens[i] = tokens[i].PadLeft(2, ' ');
        //        }
        //    }
        //    instance.ActivityCode = string.Join(".", tokens);
        //}

        partial void InsertProgressFormat(ProgressFormat instance)
        {
            SetAuditFields(instance, ChangeAction.Insert);
            this.ExecuteDynamicInsert(instance);
        }
        partial void InsertFormatActivityDetail(FormatActivityDetail instance)
        {
            SetAuditFields(instance, ChangeAction.Insert);
            this.ExecuteDynamicInsert(instance);
        }

        partial void UpdateFormatActivityDetail(FormatActivityDetail instance)
        {
            SetAuditFields(instance, ChangeAction.Update);
            this.ExecuteDynamicUpdate(instance);
        }
        partial void InsertFormatDetail(FormatDetail instance)
        {
            SetAuditFields(instance, ChangeAction.Insert);
            this.ExecuteDynamicInsert(instance);
        }
        partial void UpdateFormatDetail(FormatDetail instance)
        {
            SetAuditFields(instance, ChangeAction.Update);
            this.ExecuteDynamicUpdate(instance);
        }

        

    }

    partial class Package : IAuditable
    {
        public string DisplayName
        {
            get
            {
                return string.Format("{0}: {1}",
                    this.PackageName, this.Description);
            }
        }
    }


    partial class Activity : IAuditable
    {
        /// <summary>
        /// Remove leading 0 from each component of the ID
        /// </summary>
        public string DisplayActivityCode
        {
            get
            {
                string[] tokens = this.ActivityCode.Split('.');
                for (int i = 0; i < tokens.Length; ++i)
                {
                    tokens[i] = tokens[i].TrimStart('0');
                }
                return string.Join(".", tokens);
            }
        }

        /// <summary>
        /// Splits with . and pads 0 for all single digit numerics. nNon numeric tokens are not padded
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// 1.1 becomes 01.01;
        /// 10.1 becomes 10.01
        /// 100.01 stays the same
        /// A.1 becomes A.01
        /// </remarks>
        public static string PadActivityCode(string activityCode)
        {
            string[] tokens = activityCode.Split('.');
            int result;
            for (int i = 0; i < tokens.Length; ++i)
            {
                bool b = int.TryParse(tokens[i], out result);
                if (b)
                {
                    // Numeric
                    tokens[i] = tokens[i].PadLeft(2, '0');
                }
            }
            return string.Join(".", tokens);
        }

        public decimal? FinancialTarget
        {
            get
            {
                return this.BoqRate * this.PhysicalTarget;
            }
        }

        public bool IsTopLevel
        {
            get
            {
                string[] tokens = this.ActivityCode.Split(new char[] {'.'}, StringSplitOptions.RemoveEmptyEntries);
                return tokens.Length == 1;
            }
        }
    }

    partial class ProgressFormat : IAuditable
    {
        /// <summary>
        /// The numbers assigned to each enum cannot change. These are the IDs which are stored in the database.
        /// </summary>
        public enum FormatType
        {
            MonthlyPhysicalFinancial = 1,
            DailyPhysical = 3,
            MonthlyConstruction = 4,
        }

        /// <summary>
        /// This is a never changing code which can be used to name pages
        /// </summary>
        public string FormatCode
        {
            get
            {
                switch ((FormatType)this.ProgressFormatType)
                {
                    case FormatType.MonthlyPhysicalFinancial:
                        return "MonthlyPhysical";

                    case FormatType.DailyPhysical:
                        return "DailyPhysical";

                    case FormatType.MonthlyConstruction:
                        return "MonthlyConstruction";

                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        /// <summary>
        /// Used to display in navigation grids
        /// </summary>
        public string FormatTypeDisplayName
        {
            get
            {
                switch ((FormatType)this.ProgressFormatType)
                {
                    case FormatType.MonthlyPhysicalFinancial:
                        return "Monthly Physical";

                    case FormatType.DailyPhysical:
                        return "Daily Physical";

                    case FormatType.MonthlyConstruction:
                        return "Monthly Construction";

                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        public string FormatDisplayName
        {
            get
            {
                return string.Format("{0} - {1}",
                    this.ProgressFormatName, this.Description);
            }
        }

    }

    partial class FormatActivityDetail : IAuditable
    {

    }
    partial class FormatDetail : IAuditable
    {

    }

}

