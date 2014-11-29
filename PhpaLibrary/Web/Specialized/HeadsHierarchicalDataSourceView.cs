using System.Web.UI;

namespace Eclipse.PhpaLibrary.Web.Specialized
{
    public class HeadsHierarchicalDataSourceView : HierarchicalDataSourceView
    {
        private readonly string _viewPath;
        private readonly HeadsHierarchicalDataSourceControl _owner;

        public HeadsHierarchicalDataSourceView(HeadsHierarchicalDataSourceControl owner, string viewPath)
        {
            _viewPath = viewPath;
            _owner = owner;
        }
        public override IHierarchicalEnumerable Select()
        {
            if (string.IsNullOrEmpty(_viewPath))
            {
                return new HeadsHierarchicalEnumerable(_owner.Database, null);
            }
            else
            {
                return new HeadsHierarchicalEnumerable(_owner.Database, int.Parse(_viewPath));
            }
        }
    }
}
