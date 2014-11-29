using System;
using System.Web.UI;
using System.Collections;
using Eclipse.PhpaLibrary.Database;
using System.Linq;
using Eclipse.PhpaLibrary.Reporting;
using System.Collections.Generic;


namespace Eclipse.PhpaLibrary.Web.Specialized
{
    public class HeadsHierarchicalEnumerable : IHierarchicalEnumerable
    {
        private readonly int? _nParentId;
        private readonly ReportingDataContext _db;
        public HeadsHierarchicalEnumerable(ReportingDataContext db, int? nParentId)
        {
            _nParentId = nParentId;
            _db = db;
        }

        #region IHierarchicalEnumerable Members

        public IHierarchyData GetHierarchyData(object enumeratedItem)
        {
            return new HeadsHierarchyData(_db, (RoHeadHierarchy) enumeratedItem);
        }

        #endregion

        #region IEnumerable Members

        public IEnumerator GetEnumerator()
        {
                return Heads;
        }

        #endregion

        IEnumerator<RoHeadHierarchy> Heads
        {
            get
            {
                IQueryable<RoHeadHierarchy> query;
                if (_nParentId == null)
                {
                    query = from acc in _db.RoHeadHierarchies
                             where !acc.ParentHeadOfAccountId.HasValue
                            orderby acc.SortableName
                             select acc;
                }
                else
                {
                    query = from acc in _db.RoHeadHierarchies
                             where acc.ParentHeadOfAccountId == _nParentId
                            orderby acc.SortableName ascending
                             select acc;
                }

                //foreach (RoHeadHierarchy acc in query)
                //{
                //    yield return acc;
                //}
                //yield break;
                return query.GetEnumerator();
            }
        }
    }
}
