using System;
using System.Data.Linq;
using System.Linq;
using Eclipse.PhpaLibrary.Reporting;

namespace Eclipse.PhpaLibrary.Database.Store
{
    partial class StoreDataContext
    {
        #region Item
        partial void InsertItem(Item instance)
        {
            SetAuditFields(instance, ChangeAction.Insert);
            this.ExecuteDynamicInsert(instance);
        }

        partial void UpdateItem(Item instance)
        {
            SetAuditFields(instance, ChangeAction.Update);
            this.ExecuteDynamicUpdate(instance);
        }
        #endregion

        #region UOM
        partial void InsertUOM(UOM instance)
        {
            SetAuditFields(instance, ChangeAction.Insert);
            this.ExecuteDynamicInsert(instance);
        }

        partial void UpdateUOM(UOM instance)
        {
            SetAuditFields(instance, ChangeAction.Update);
            this.ExecuteDynamicUpdate(instance);
        }
        #endregion

        #region ItemCategory
        partial void InsertItemCategory(ItemCategory instance)
        {
            SetAuditFields(instance, ChangeAction.Insert);
            this.ExecuteDynamicInsert(instance);
        }

        partial void UpdateItemCategory(ItemCategory instance)
        {
            SetAuditFields(instance, ChangeAction.Update);
            this.ExecuteDynamicUpdate(instance);
        }
        #endregion

        #region GRN
        partial void InsertGRN(GRN instance)
        {
            SetAuditFields(instance, ChangeAction.Insert);
            this.ExecuteDynamicInsert(instance);
        }

        partial void UpdateGRN(GRN instance)
        {
            SetAuditFields(instance, ChangeAction.Update);
            this.ExecuteDynamicUpdate(instance);
        }
        #endregion

        #region SRS
        partial void InsertSRS(SRS instance)
        {
            SetAuditFields(instance, ChangeAction.Insert);
            this.ExecuteDynamicInsert(instance);
        }

        partial void UpdateSRS(SRS instance)
        {
            SetAuditFields(instance, ChangeAction.Update);
            this.ExecuteDynamicUpdate(instance);
        }

        #endregion

        partial void UpdateGRNItem(GRNItem instance)
        {
            SetAuditFields(instance, ChangeAction.Update);
            this.ExecuteDynamicUpdate(instance);
        }

        partial void InsertGRNItem(GRNItem instance)
        {
            SetAuditFields(instance, ChangeAction.Insert);
            this.ExecuteDynamicInsert(instance);
        }

    }


    partial class RoEmployee : IEmployee
    {
        public string FullName
        {
            get
            {
                return string.Format("{0} {1}", this.FirstName, this.LastName);
            }
        }

    }


    partial class Item : IAuditable
    {
    }

    partial class ItemCategory : IAuditable
    {
    }

    partial class GRN : IAuditable
    {
    }

    partial class GRNItem : IAuditable
    {
    }

    partial class SRS : IAuditable
    {
        /// <summary>
        /// Issue few pieces against the passed srs item
        /// </summary>
        /// <param name="db"></param>
        /// <param name="nQuantityToIssue"></param>
        /// <param name="srsItemId"></param>
        /// <param name="itemId"></param>
        /// <param name="remarks"></param>
        public static void IssueItem(StoreDataContext db, int nQuantityToIssue, int srsItemId, string remarks)
        {
            var query = from grnItem in db.GRNItems
                        let availableQty = (grnItem.AcceptedQty ?? 0) - (grnItem.SRSIssueItems.Sum(p => p.QtyIssued) ?? 0)
                        where grnItem.ItemId == (from srsItem in db.SRSItems
                                                 where srsItem.SRSItemId == srsItemId
                                                 select srsItem.ItemId).FirstOrDefault() && availableQty > 0
                        orderby grnItem.GRN.GRNReceiveDate ascending
                        select new
                        {
                            GRNId = grnItem.GRNId,
                            GrnItemId = grnItem.GRNItemId,
                            AvailableQuantity = availableQty
                        };

            foreach (var row in query)
            {
                if (nQuantityToIssue <= 0)
                {
                    break;
                }
                SRSIssueItem item = new SRSIssueItem();
                item.GRNItemId = row.GrnItemId;
                item.IssueDate = DateTime.Today;
                if (nQuantityToIssue <= row.AvailableQuantity)
                {
                    // We have issued enough
                    item.QtyIssued = nQuantityToIssue;
                    nQuantityToIssue = 0;
                }
                else
                {
                    // Issue as much as we can
                    item.QtyIssued = row.AvailableQuantity;
                    nQuantityToIssue -= row.AvailableQuantity;
                }
                item.SRSItemId = srsItemId;
                item.Remarks = remarks;
                db.SRSIssueItems.InsertOnSubmit(item);
            }
            return;
        }

        public static void UnissueItem(StoreDataContext db, int nQuantityToDelete, int srsItemId)
        {
            var query = from issueItem in db.SRSIssueItems
                        where issueItem.SRSItemId == srsItemId
                        orderby issueItem.Created descending
                        select issueItem;
            foreach (var row in query)
            {
                if (nQuantityToDelete <= 0)
                {
                    break;
                }
                if (row.QtyIssued == null || row.QtyIssued.Value <= 0)
                {
                    // This should not happen
                    db.SRSIssueItems.DeleteOnSubmit(row);
                }
                else if (row.QtyIssued.Value <= nQuantityToDelete)
                {
                    db.SRSIssueItems.DeleteOnSubmit(row);
                    nQuantityToDelete -= row.QtyIssued.Value;
                }
                else
                {
                    row.QtyIssued -= nQuantityToDelete;
                    nQuantityToDelete = 0;
                }
            }
        }

        /// <summary>
        /// Unassings all GRNS and then assigns them back. This may result in different rates being used for issuing.
        /// </summary>
        public static void RecalculateRates(int srsId)
        {
            using (StoreDataContext db = new StoreDataContext(ReportingUtilities.DefaultConnectString))
            {
                var query = from srs in db.SRSIssueItems
                            where srs.QtyIssued != null &&
                            srs.SRSItem.SRSId == srsId
                            select new
                            {
                                QtyIssued = (int)srs.QtyIssued,
                                SRSItemId = srs.SRSItemId,
                                Remarks = srs.Remarks
                            };
                foreach (var row in query)
                {
                    UnissueItem(db, -row.QtyIssued, row.SRSItemId);
                }
                foreach (var row in query)
                {
                    IssueItem(db, row.QtyIssued, row.SRSItemId, row.Remarks);
                }
                db.SubmitChanges();
            }
        }
    }

    partial class SRSItem : IAuditable
    {
    }

    partial class UOM : IAuditable
    {

    }

    partial class SRSIssueItem : IAuditable
    {
    }
}