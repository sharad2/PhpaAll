using System.Collections.Generic;
using System.Linq;
using System;
using Eclipse.PhpaLibrary.Database;

namespace Eclipse.PhpaLibrary.Reporting
{
    public abstract class SubTotalItem
    {
        private readonly bool _bIsSubTotal;
        private readonly RoHeadHierarchy _acc;

        protected SubTotalItem(RoHeadHierarchy acc, bool isSubtotal)
        {
            if (acc == null)
            {
                throw new ArgumentNullException("acc");
            }
            _acc = acc;
            _bIsSubTotal = isSubtotal;
            if (isSubtotal)
            {
                this.DisplayName = string.Format(
                    "<div style='margin-left:{0}mm;font-size:{4}em;font-weight:700;margin-top:1mm;margin-bottom:1mm'>{2:Subtotal;Negative;Total} of {1}: {3}</div>",
                    (acc.Level + 1) * 0, acc.DisplayName, acc.Level, acc.Description, 0.6 + (5 - acc.Level) * 0.1);
            }
            else
            {
                this.DisplayName = string.Format("<div style='margin-left:{0}mm'>{1}: {3}</div>",
                    acc.Level * 0, acc.DisplayName, acc.Level, acc.Description, 0.4 + (5 - acc.Level) * 0.1);
                //this.DisplayName = string.Format("<div style='margin-left:{0}mm'>{1}: {2}</div>",
                //    acc.Level * 3, acc.DisplayName, acc.Description);
            }
            this.ParentHeadOfAccountId = acc.ParentHeadOfAccountId;
            this.Level = acc.Level;
            this.HeadOfAccountId = acc.HeadOfAccountId;
            this.TopParentId = acc.TopParentId;
        }

        public RoHeadHierarchy HeadHierarchy
        {
            get
            {
                return _acc;
            }
        }

        public int TopParentId { get; set; }

        public bool IsSubTotal
        {
            get
            {
                return _bIsSubTotal;
            }
        }

        /// <summary>
        /// Count of the items which were totaled to create this subtotal
        /// </summary>
        private int _countItems;
        public void UpdateSubtotals(SubTotalItem item)
        {
            if (!_bIsSubTotal)
            {
                throw new InvalidOperationException("Subtotals can only be computed for subtotal items");
            }
            ++_countItems;
            ExecuteUpdateSubtotals(item);
        }

        public int CountItems
        {
            get
            {
                return _countItems;
            }
        }

        /// <summary>
        /// This function returns an item which will keep totals for the siblings of the item
        /// which is creating the new item.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public abstract SubTotalItem CreateNew(RoHeadHierarchy head);

        /// <summary>
        /// This function must add the values of the passed item to the values in the current item.
        /// </summary>
        /// <param name="item"></param>
        protected abstract void ExecuteUpdateSubtotals(SubTotalItem item);

        public int HeadOfAccountId
        {
            get;
            set;
        }

        public int Level
        {
            get;
            set;
        }
        public int? ParentHeadOfAccountId
        {
            get;
            set;
        }

        /// <summary>
        /// LevelSubtotals class uses this property to set a string like Subtotal for 100.1.* 
        /// </summary>
        public string DisplayName
        {
            get;
            set;
        }

        /// <summary>
        /// For debugging
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            if (_bIsSubTotal)
            {
                return string.Format("Subtotal of {0}", _acc.DisplayName);
            }
            else
            {
                return _acc.DisplayName;
            }
        }

    }

    public class LevelSubtotals
    {
        private readonly Stack<SubTotalItem> m_levelSubtotals;

        private readonly ReportingDataContext m_db;
        public LevelSubtotals(ReportingDataContext db)
        {
            m_levelSubtotals = new Stack<SubTotalItem>();
            m_db = db;
        }

        /// <summary>
        /// For each item retrieved from the query, call this function.
        /// If this function returns a non null, display the returned items. They contain the
        /// subtotals which need to be displayed.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public IEnumerable<SubTotalItem> ProcessItem(SubTotalItem queryItem)
        {
            List<SubTotalItem> subtotals = new List<SubTotalItem>();
            EnsureSubtotalLevels(queryItem, subtotals);

            foreach (SubTotalItem item in m_levelSubtotals)
            {
                item.UpdateSubtotals(queryItem);
            }

            return subtotals.SkipWhile(item => item.CountItems <= 1);
        }

        /// <summary>
        /// Ensures that the subtotal stack contains complete ancestry of passed item with the item at the end of the stack.
        /// </summary>
        /// <param name="item"></param>
        private void EnsureSubtotalLevels(SubTotalItem item, List<SubTotalItem> poppedItems)
        {
            // Search for sibling of passed item
            SubTotalItem siblingSubtotalItem = m_levelSubtotals.FirstOrDefault(
                subtotalItem => subtotalItem.Level == item.Level);
            if (siblingSubtotalItem == null)
            {
                EnsureAncestry(item, poppedItems);
            }
            else
            {
                // Pop sibling and nephews
                PopItemAndDescendants(siblingSubtotalItem, poppedItems);
                EnsureAncestry(item, poppedItems);
            }

            return;
        }

        /// <summary>
        /// Removes the item and its descendants from the stack. If item is null, then the stack is emptied.
        /// </summary>
        /// <param name="siblingSubtotalItem"></param>
        private void PopItemAndDescendants(SubTotalItem item, List<SubTotalItem> poppedItems)
        {
            while (m_levelSubtotals.Count > 0)
            {
                SubTotalItem curItem = m_levelSubtotals.Pop();
                poppedItems.Add(curItem);
                if (item != null && curItem.HeadOfAccountId == item.HeadOfAccountId)
                {
                    break;
                }
            }

            if (item != null)
            {
                //throw new AssertFailedException("If an item was passed, it must be found");
            }
        }

        /// <summary>
        /// Removes all items whose level is greater than the passed level
        /// </summary>
        /// <param name="item"></param>
        /// <param name="poppedItems"></param>
        private void PopDescendants(int level, List<SubTotalItem> poppedItems)
        {
            while (m_levelSubtotals.Count > 0)
            {
                SubTotalItem curItem = m_levelSubtotals.Peek();
                if (curItem.Level > level)
                {
                    poppedItems.Add(m_levelSubtotals.Pop());
                }
                else
                {
                    break;
                }
                
            }

        }

        /// <summary>
        /// This function assumes that item does not already exist in m_levelSubtotals
        /// and therefore always adds it.
        /// </summary>
        /// <param name="siblingSubtotalItem"></param>
        private void EnsureAncestry(SubTotalItem item, List<SubTotalItem> poppedItems)
        {
            SubTotalItem ancestorItem = FindAncestorItem(item);
            PopDescendants(ancestorItem == null ? -1 : ancestorItem.Level, poppedItems);

            List<RoHeadHierarchy> list = new List<RoHeadHierarchy>();
            list.Add(item.HeadHierarchy);
            int? nParentId = item.ParentHeadOfAccountId;
            while (nParentId != null)
            {
                SubTotalItem parentItem = m_levelSubtotals.FirstOrDefault(
                    subtotalItem => subtotalItem.HeadOfAccountId == nParentId);
                if (parentItem == null)
                {
                    RoHeadHierarchy acc = (from head in m_db.RoHeadHierarchies
                                           where head.HeadOfAccountId == nParentId
                                           select head).Single();
                    list.Add(acc);
                    nParentId = acc.ParentHeadOfAccountId;
                }
                else
                {
                    nParentId = null;   // force loop exit
                }
            }
            list.Reverse();
            foreach (RoHeadHierarchy head in list)
            {
                SubTotalItem newItem = item.CreateNew(head);
                m_levelSubtotals.Push(newItem);
            }

        }

        /// <summary>
        /// Searches for the nearest ancestor of passed item in m_levelSubtotals and returns it
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        private SubTotalItem FindAncestorItem(SubTotalItem item)
        {
            string[] ancestors = item.HeadHierarchy.HierarchyPath.Split('|');

            for (int i = ancestors.Length - 1; i >= 0; --i)
            {
                int ancestorId = int.Parse(ancestors[i]);
                SubTotalItem ancestorItem = m_levelSubtotals.FirstOrDefault(
                    subtotalItem => subtotalItem.HeadOfAccountId == ancestorId);
                if (ancestorItem != null)
                {
                    return ancestorItem;
                }
            }
            // No ancestor found in the subtotals
            return null;
        }

        public IEnumerable<SubTotalItem> GetFinalSubTotals()
        {
           return m_levelSubtotals;
            //return m_levelSubtotals.SkipWhile(item => item.CountItems <= 1);
        }
    }
}
