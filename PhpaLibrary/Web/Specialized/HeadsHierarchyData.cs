using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using Eclipse.PhpaLibrary.Database;
using Eclipse.PhpaLibrary.Reporting;

namespace Eclipse.PhpaLibrary.Web.Specialized
{
    class HeadsHierarchyData:IHierarchyData
    {
        private readonly RoHeadHierarchy _acc;
        private readonly ReportingDataContext _db;
        public HeadsHierarchyData(ReportingDataContext db, RoHeadHierarchy acc)
        {
            _acc = acc;
            _db = db;
        }
        #region IHierarchyData Members

        public IHierarchicalEnumerable GetChildren()
        {
            return new HeadsHierarchicalEnumerable(_db, _acc.HeadOfAccountId);
        }

        public IHierarchyData GetParent()
        {
            throw new NotImplementedException();
        }

        public bool HasChildren
        {
            get
            {
                return _acc.CountChildren > 0;
            }
        }

        public object Item
        {
            get { return _acc; }
        }

        public string Path
        {
            get { return _acc.HeadOfAccountId.ToString(); }
        }

        public string Type
        {
            get { return "Sharad"; }
        }

        #endregion
    }
}
