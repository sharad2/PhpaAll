using System.ComponentModel;
using System.Data.Linq;
using System.Web.UI;
using Eclipse.PhpaLibrary.Reporting;

namespace Eclipse.PhpaLibrary.Web.Specialized
{
    /// <summary>
    /// Returns the full hierarchy of account heads
    /// </summary>
    public class HeadsHierarchicalDataSourceControl : HierarchicalDataSourceControl
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="viewPath">id of the parent whose children must be returned.
        /// If empty, then top level heads are returned.
        /// </param>
        /// <returns></returns>
        protected override HierarchicalDataSourceView GetHierarchicalView(string viewPath)
        {
            return new HeadsHierarchicalDataSourceView(this, viewPath);
        }

        private ReportingDataContext m_db;

        /// <summary>
        /// If you want to execute your own queries against the data context, use this property
        /// </summary>
        /// 
        [Browsable(false)]
        public ReportingDataContext Database
        {
            get
            {
                if (m_db == null)
                {
                    m_db = new ReportingDataContext(Reporting.ReportingUtilities.DefaultConnectString);
                    DataLoadOptions dlo = new DataLoadOptions();
                    dlo.LoadWith<RoHeadHierarchy>(h => h.RoAccountType);
                    m_db.LoadOptions = dlo;
                }
                return m_db;
            }
        }

        public override void Dispose()
        {
            if (m_db != null)
            {
                m_db.Dispose();
                m_db = null;
            }
            base.Dispose();
        }

        #region Log rendering

        [Browsable(true)]
        [ReadOnly(true)]
        [Category("Phpa")]
        public override bool Visible
        {
            get
            {
                return true;
            }
            set
            {
                // Ignore
            }
        }

        protected override void Render(System.Web.UI.HtmlTextWriter writer)
        {
            if (m_db == null)
            {
                // Happens when we are not asked to execute a query
                return;
            }
                writer.WriteLineNoTabs("<!-- ****");
            writer.WriteLineNoTabs("SQL log for " + this.ID);
            writer.WriteLineNoTabs(m_db.LogOutput.ToString());

            writer.WriteLineNoTabs("-->");
            //base.Render(writer);
        }


        #endregion
    }
}
