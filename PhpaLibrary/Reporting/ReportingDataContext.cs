using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using Eclipse.PhpaLibrary.Database.Payroll;
using Eclipse.PhpaLibrary.Database.Store;

namespace Eclipse.PhpaLibrary.Reporting
{
    /*
     * This is the script for the view used to display the heads hierarchy
     * Updated by SS Jun 19, 2008

ALTER VIEW [dbo].[HeadHierarchy] AS
-- Sortable name is a 64 bit number which gives us 63 bits to use.
    -- Leading 10 bits contain the top parent and every subsequent 8 bits contain a child.
    -- This allows for a maximum of 7 levels including the top level. Each level can have a name within parent
    -- as high as 255. The top parent can have a name within parent up to 1000.
    WITH Descendants (
        HeadOfAccountId,
        StoredDisplayName,
        NameWithinParent,
        TopParentName,
		TopParentId,
        SortableName,
        Description,
        ProjectCost,
        RevisedProjectCost,
        HierarchyPath,
        ParentHeadOfAccountId,
        HeadOfAccountType,
        Level
    )
    AS
    (
    SELECT
        e.HeadOfAccountId
        ,e.DisplayName
        ,e.NameWithinParent
        ,e.NameWithinParent
		,e.HeadOfAccountId
        ,e.NameWithinParent * power(CAST(2 as bigint), 48)
        ,e.Description
        ,e.ProjectCost
        ,e.RevisedProjectCost
        ,CAST(e.HeadOfAccountId AS VARCHAR(256))
        ,e.ParentHeadOfAccountId
        ,e.HeadOfAccountType
        , 0 AS Level
    FROM HeadOfAccount AS e
    WHERE ParentHeadOfAccountId is null

    UNION ALL

    SELECT
        e.HeadOfAccountId
        ,e.DisplayName
        ,e.NameWithinParent
        ,d.TopParentName
		,d.TopParentId
        ,d.SortableName + e.NameWithinParent * power(CAST(2 AS bigint), 48 - (Level + 1) * 8)
        ,e.Description
        ,e.ProjectCost
        ,e.RevisedProjectCost
        ,CAST(d.HierarchyPath + '|' + CAST(e.HeadOfAccountId AS VARCHAR) AS VARCHAR(256))
        ,e.ParentHeadOfAccountId
        ,e.HeadOfAccountType
        ,Level + 1
    FROM HeadOfAccount AS e
    INNER JOIN
        Descendants AS d ON e.ParentHeadOfAccountId = d.HeadOfAccountId
    )
    SELECT
        HeadOfAccountId,
        StoredDisplayName,
        NameWithinParent,
        TopParentName,
		TopParentId,
        SortableName,
        Description,
        ProjectCost,
        RevisedProjectCost,
        HierarchyPath,
        ParentHeadOfAccountId,
        HeadOfAccountType,
        Level,
        (SELECT COUNT(*) FROM HeadOfAccount WHERE ParentHeadOfAccountId = h.HeadOfAccountId) AS CountChildren
    FROM Descendants AS h
    */
    partial class ReportingDataContext
    {
        /// <summary>
        /// Whenever voucher details are retrieved, retrieve voucher info and head info as well
        /// </summary>
        partial void OnCreated()
        {
            this.ObjectTrackingEnabled = false;     // we are readonly
            DataLoadOptions dlo = new DataLoadOptions();
            dlo.LoadWith<RoVoucherDetail>(vd => vd.RoVoucher);
            //dlo.LoadWith<RoVoucherDetail>(vd => vd.HeadOfAccount);
            dlo.LoadWith<RoVoucherDetail>(vd => vd.RoEmployee);
            dlo.LoadWith<RoVoucherDetail>(vd => vd.RoJob);
            //dlo.LoadWith<RoVoucherDetail>(vd => vd.RoContractor);
            dlo.LoadWith<RoVoucher>(v => v.RoDivision);
            this.LoadOptions = dlo;
        }

        /// <summary>
        /// Returs Sum(debit - credit)
        /// </summary>
        /// <param name="headOfAccountId"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public decimal? GetOpeningBalance(int headOfAccountId, DateTime date)
        {
            var openingBalance = (from vd in this.RoVoucherDetails
                                  where vd.RoVoucher.VoucherDate < date &&
                                          vd.HeadOfAccountId == headOfAccountId
                                  group vd by vd.HeadOfAccountId into grouping
                                  select new
                                  {
                                      grouping.Key,
                                      SumDebit = grouping.Sum(p => p.DebitAmount),
                                      SumCredit = grouping.Sum(p => p.CreditAmount)
                                  }).FirstOrDefault();
            if (openingBalance == null)
            {
                return null;
            }
            return (openingBalance.SumDebit ?? 0) - (openingBalance.SumCredit ?? 0);
        }

        public decimal? GetContractorOpeningBalance(string headOfAccountType, DateTime date, int ContractorId)
        {
            var openingBalance = (from vd in this.RoVoucherDetails
                                  where vd.RoVoucher.VoucherDate < date &&
                                          vd.HeadOfAccount.HeadOfAccountType == headOfAccountType &&
                                          (vd.RoJob.ContractorId == ContractorId || vd.ContractorId == ContractorId)
                                  group vd by vd.HeadOfAccount.HeadOfAccountType into grouping
                                  select new
                                  {
                                      grouping.Key,
                                      SumDebit = grouping.Sum(p => p.DebitAmount),
                                      SumCredit = grouping.Sum(p => p.CreditAmount)
                                  }).FirstOrDefault();
            if (openingBalance == null)
            {
                return null;
            }
            return (openingBalance.SumDebit ?? 0) - (openingBalance.SumCredit ?? 0);
        }

        public decimal? GetEmployeeOpeningBalance(string headOfAccountType, DateTime date, int EmployeeId)
        {
            var openingBalance = (from vd in this.RoVoucherDetails
                                  where vd.RoVoucher.VoucherDate < date &&
                                          vd.HeadOfAccount.HeadOfAccountType == headOfAccountType &&
                                          vd.EmployeeId == EmployeeId
                                  group vd by vd.HeadOfAccount.HeadOfAccountType into grouping
                                  select new
                                  {
                                      grouping.Key,
                                      SumDebit = grouping.Sum(p => p.DebitAmount),
                                      SumCredit = grouping.Sum(p => p.CreditAmount)
                                  }).FirstOrDefault();
            if (openingBalance == null)
            {
                return null;
            }
            return (openingBalance.SumDebit ?? 0) - (openingBalance.SumCredit ?? 0);
        }        
    }


    public partial class RoVoucher
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

                return m_voucherTypes[this.VoucherType];
            }
        }

        public string VoucherReference
        {
            
            get
            {
                return ReportingUtilities.VoucherIdToVoucherReferenceNumber(this._VoucherId);
            }
        }
    }

    public partial class RoVoucherDetail
    {
        /// <summary>
        /// Since all properties are readonly, this constructor allows you to set the debit and credit sum
        /// while creating an instance. This is useful in aggregation scenarios.
        /// </summary>
        /// <param name="debitAmount"></param>
        /// <param name="creditAmount"></param>
        public RoVoucherDetail(decimal? debitAmount, decimal? creditAmount)
        {
            _DebitAmount = debitAmount;
            _CreditAmount = creditAmount;

        }
    }
  
   public partial class RoHeadHierarchy
    {
        public bool IsDescendantOf(int headOfAccountId)
        {
            return this.HierarchyPath.Contains(string.Format("{0}|", headOfAccountId));
        }

        public string DisplayDescription
        {
            get
            {
                return string.Format("{0}: {1}", this.DisplayName, this.Description);
            }
        }

        public override string ToString()
        {
            return this.DisplayDescription;
        }
    }


   public partial class RoEmployee : IEmployee

   {

       public string FullName

       {
           get
           {
               return string.Format("{0} {1}", this.FirstName, this.LastName);
           
           }
       }
     
   }
}
