using System.Collections.Generic;
using System.Data.Linq;
using System.Web;
using System;
using System.Linq;
using System.Data.SqlClient;

namespace Eclipse.PhpaLibrary.Database
{
    public sealed partial class FinanceDataContext
    {
        #region HeadOfAccount

        partial void InsertHeadOfAccount(HeadOfAccount instance)
        {
            if (instance.ParentHead == null)
            {
                instance.DisplayName = instance.NameWithinParent.ToString("00");
            }
            else
            {
                instance.DisplayName = string.Format("{0}.{1:00}", instance.ParentHead.DisplayName, instance.NameWithinParent);
            }
            SetAuditFields(instance, ChangeAction.Insert);
            this.ExecuteDynamicInsert(instance);
        }
        /// <summary>
        /// If NameWithinParent has changed, recursively change the display names of all descendants
        /// </summary>
        /// <param name="instance"></param>
        partial void UpdateHeadOfAccount(HeadOfAccount instance)
        {
            HeadOfAccount accOld = this.HeadOfAccounts.GetOriginalEntityState(instance);
            if (accOld.NameWithinParent != instance.NameWithinParent)
            {
                if (instance.ParentHeadOfAccountId.HasValue)
                {
                    instance.DisplayName = string.Format("{0}.{1:00}",
                        instance.ParentHead.DisplayName, instance.NameWithinParent);
                }
                else
                {
                    instance.DisplayName = instance.NameWithinParent.ToString();
                }
                RecurseUpdateDisplayName(instance);
                //throw new NotImplementedException();
            }
            SetAuditFields(instance, ChangeAction.Update);
            this.ExecuteDynamicUpdate(instance);
        }

        /// <summary>
        /// Recursively updates the display name of all child heads based on the display name of
        /// the passed acc.
        /// </summary>
        /// <param name="acc"></param>
        private void RecurseUpdateDisplayName(HeadOfAccount accParent)
        {
            foreach (HeadOfAccount accChild in accParent.ChildHeads)
            {
                accChild.DisplayName = string.Format("{0}.{1:00}",
                        accParent.DisplayName, accChild.NameWithinParent);
                SetAuditFields(accChild, ChangeAction.Update);
                this.ExecuteDynamicUpdate(accChild);
                RecurseUpdateDisplayName(accChild);
            }
        }
        #endregion

        #region Voucher
        partial void UpdateVoucher(Voucher instance)
        {
            SetAuditFields(instance, ChangeAction.Update);
            this.ExecuteDynamicUpdate(instance);
        }


        partial void InsertVoucher(Voucher instance)
        {
            //string stationName = HttpContext.Current.Server.MachineName;
            // instance.StationName = HttpContext.Current.Server.MachineName;
            SetAuditFields(instance, ChangeAction.Insert);
            this.ExecuteDynamicInsert(instance);
        }

        partial void DeleteVoucher(Voucher instance)
        {
            this.ExecuteDynamicDelete(instance);
        }
        #endregion

        #region Budget
        partial void UpdateBudget(Budget instance)
        {
            SetAuditFields(instance, ChangeAction.Update);
            this.ExecuteDynamicUpdate(instance);
        }


        partial void InsertBudget(Budget instance)
        {
            //string stationName = HttpContext.Current.Server.MachineName;
            //instance.StationName = HttpContext.Current.Server.MachineName;
            SetAuditFields(instance, ChangeAction.Insert);
            this.ExecuteDynamicInsert(instance);
        }

        partial void DeleteBudget(Budget instance)
        {
            this.ExecuteDynamicDelete(instance);
        }
        #endregion

        #region VoucherDetail
        partial void UpdateVoucherDetail(VoucherDetail instance)
        {
            SetAuditFields(instance, ChangeAction.Update);
            this.ExecuteDynamicUpdate(instance);
        }

        partial void InsertVoucherDetail(VoucherDetail instance)
        {
            SetAuditFields(instance, ChangeAction.Insert);
            this.ExecuteDynamicInsert(instance);
        }
        #endregion

        #region Contractor
        partial void UpdateContractor(Contractor instance)
        {
            SetAuditFields(instance, ChangeAction.Update);
            this.ExecuteDynamicUpdate(instance);
        }

        partial void InsertContractor(Contractor instance)
        {


            if (string.IsNullOrEmpty(instance.ContractorCode))
            {
                try
                {
                    var q = this.Contractors.Max(p => Convert.ToInt32(p.ContractorCode));
                    instance.ContractorCode = (Convert.ToInt32(q) + 1).ToString();
                }
                catch (SqlException ex)
                {
                    if (ex.ErrorCode == -2146232060)
                    {
                        throw new ValidationException("When non numeric contractor codes are used, contractor code must be explicitly specified");
                    }
                }

            }

            SetAuditFields(instance, ChangeAction.Insert);
            this.ExecuteDynamicInsert(instance);

        }
        #endregion

        #region Division
        partial void UpdateDivision(Division instance)
        {
            SetAuditFields(instance, ChangeAction.Update);
            this.ExecuteDynamicUpdate(instance);
        }

        partial void InsertDivision(Division instance)
        {
            //if (string.IsNullOrEmpty(instance.DivisionCode))
            //{
            //    try
            //    {
            //        var q = this.Divisions.Max(p => Convert.ToInt32(p.DivisionCode));
            //        instance.DivisionCode = (Convert.ToInt32(q) + 1).ToString();
            //    }
            //    catch (SqlException ex)
            //    {
            //        if (ex.ErrorCode == -2146232060)
            //        {
            //            throw new ValidationException("When non numeric division codes are used, division code must be explicitly specified");
            //        }
            //    }
            //}
            
            SetAuditFields(instance, ChangeAction.Insert);
            this.ExecuteDynamicInsert(instance);
        }
        #endregion

        #region Job
        partial void UpdateJob(Job instance)
        {
            SetAuditFields(instance, ChangeAction.Update);
            this.ExecuteDynamicUpdate(instance);
        }

        partial void InsertJob(Job instance)
        {
            if (string.IsNullOrEmpty(instance.JobCode))
            {
                try
                {
                    var q = this.Jobs.Max(p => Convert.ToInt32(p.JobCode));
                    instance.JobCode = (Convert.ToInt32(q) + 1).ToString();
                }
                catch (SqlException ex)
                {
                    if (ex.ErrorCode == -2146232060)
                    {
                        throw new ValidationException("When non numeric job codes are used, job code must be explicitly specified");
                    }
                }

            }
            SetAuditFields(instance, ChangeAction.Insert);
            this.ExecuteDynamicInsert(instance);
        }

        #endregion

    }




    public partial class Employee
    {
        public string FullName
        {
            get
            {
                return string.Format("{0} {1}", this.FirstName, this.LastName);
            }
        }
    }

    public partial class HeadOfAccount : IAuditable
    {
        public string DisplayDescription
        {
            get
            {
                return string.Format("{0}: {1}", this.DisplayName, this.Description);
            }
        }


    }

    public partial class Voucher : IAuditable
    {
        private static Dictionary<char, string> m_voucherTypes;

        public string DisplayVoucherType
        {
            get
            {
                if (m_voucherTypes == null)
                {
                    m_voucherTypes = new Dictionary<char, string>(3);
                    m_voucherTypes.Add('B', "Bank");
                    m_voucherTypes.Add('J', "Journal");
                    m_voucherTypes.Add('C', "Cash");
                }

                return m_voucherTypes[this.VoucherTypeCode];
            }
        }
    }

    public partial class VoucherDetail : IAuditable
    {
    }

    public partial class Contractor : IAuditable
    {
        partial void OnValidate(ChangeAction action)
        {
            switch (action)
            {
                case ChangeAction.Delete:
                    break;

                case ChangeAction.Update:
                case ChangeAction.Insert:

                    try
                    {

                        if (!string.IsNullOrEmpty(this.ContractorCode))
                        {

                            int.Parse(this.ContractorCode);

                        }

                    }
                    catch (FormatException)
                    {
                        throw new ValidationException("You can type only integer values in Contractor Code");


                    }



                    ;
                    break;
                case ChangeAction.None:
                    break;

                default:
                    break;
            }
        }


        private static Dictionary<string, string> m_Nationality;

        public string DisplayNationality
        {
            get
            {
                if (m_Nationality == null)
                {
                    m_Nationality = new Dictionary<string, string>(3);
                    m_Nationality.Add("BN", "Bhutan National");
                    m_Nationality.Add("IN", "Indian National");
                    m_Nationality.Add("TC", "Third Country");
                }
                return m_Nationality[this.Nationality];
            }

        }
    }

    public partial class Division : IAuditable
    {
    }

    public partial class Job : IAuditable
    {

        partial void OnValidate(ChangeAction action)
        {
            switch (action)
            {
                case ChangeAction.Delete:
                    break;

                case ChangeAction.Update:
                case ChangeAction.Insert:

                    try
                    {

                        if (!string.IsNullOrEmpty(this.JobCode))
                        {

                            int.Parse(this.JobCode);

                        }

                    }
                    catch (FormatException)
                    {
                        throw new ValidationException("You can type only integer values in Job Code");


                    }



                    ;
                    break;
                case ChangeAction.None:
                    break;

                default:
                    break;
            }
        }

        /// <summary>
        /// This class is designed for datgabinding to dropdownlist where you want to show the\
        /// possible voucher types.
        /// </summary>
        public class JobType
        {
            public JobType(char jobTypeCode, string displayJobType)
            {
                JobTypeCode = jobTypeCode;
                DisplayJobType = displayJobType;
            }

            public char JobTypeCode { get; set; }

            public string DisplayJobType { get; set; }
        }

        private static List<JobType> m_jobTypes;

        public static List<JobType> GetJobTypes()
        {
            if (m_jobTypes == null)
            {
                m_jobTypes = new List<JobType>(3);
                m_jobTypes.Add(new JobType('D', "Department"));
                m_jobTypes.Add(new JobType('W', "Work Order"));
                m_jobTypes.Add(new JobType('C', "Contract"));
                m_jobTypes.Add(new JobType('S', "Special"));
            }
            return m_jobTypes;
        }

        public string DisplayJobType
        {
            get
            {
                JobType jobType = Job.GetJobTypes().Find(p => p.JobTypeCode == this.JobTypeCode);
                if (jobType == null)
                {
                    return string.Format("Unknown job type: {0}", this.JobTypeCode);
                }
                return jobType.DisplayJobType;
            }
        }

    }


    public partial class Budget : IAuditable
    {

    }
    public partial class BankGuarantee : IAuditable
    {

    }


}
