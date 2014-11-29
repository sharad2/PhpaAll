using System;
using System.ComponentModel;
using System.Data.Common;
using System.Data.Linq;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Eclipse.PhpaLibrary.Database;
using Eclipse.PhpaLibrary.Reporting;
using Eclipse.PhpaLibrary.Database.Payroll;
using Eclipse.PhpaLibrary.Database.Store;
using Eclipse.PhpaLibrary.Database.PIS;
using Eclipse.PhpaLibrary.Database.MIS;

namespace Eclipse.PhpaLibrary.Web
{
    /// <summary>
    /// Provides query logging within HTML comments
    /// </summary>
    /// <remarks>
    /// Transaction Support
    /// This data source supports the scenario where you want multiple PhpaLinqDataSource instances
    /// to perform DML in the context of the same transaction.
    /// 1. Specify the same transaction id for all the data sources which need to participate in the transaction
    /// using the property TransactionId.
    /// 2. Perform DML through the associated data bound controls.
    /// 3. Commit or rollback the transaction using the static function CommitTransaction which takes the
    /// transaction id as the parameter.
    /// 
    /// Committing or rolling back is always your responsibility.
    /// BUG: Dispose connection. Each time a context uses the shared connection, a reference count is kept.
    /// </remarks>
    public class PhpaLinqDataSource : LinqDataSource
    {
        public PhpaLinqDataSource()
        {
            this.RenderLogVisible = true;
            //this.ConnectStringId = string.Empty;
        }

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

        protected override void OnInit(System.EventArgs e)
        {
            base.OnInit(e);
            this.ContextCreating += new EventHandler<LinqDataSourceContextEventArgs>(PhpaLinqDataSource_ContextCreating);
            this.ContextDisposing += new EventHandler<LinqDataSourceDisposeEventArgs>(PhpaLinqDataSource_ContextDisposing);
            //this.Updating += new EventHandler<LinqDataSourceUpdateEventArgs>(PhpaLinqDataSource_Updating);
            this.Deleted += new EventHandler<LinqDataSourceStatusEventArgs>(PhpaLinqDataSource_Deleted);
            this.Deleting += new EventHandler<LinqDataSourceDeleteEventArgs>(PhpaLinqDataSource_Deleting);
        }


        private object m_entityBeingDeleted;
        /// <summary>
        /// In case deleting fails, we want to requery the object being deleted.
        /// Here we save it, and in deleted we refresh it when an error occurs
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void PhpaLinqDataSource_Deleting(object sender, LinqDataSourceDeleteEventArgs e)
        {
            m_entityBeingDeleted = e.OriginalObject;
        }

        void PhpaLinqDataSource_Deleted(object sender, LinqDataSourceStatusEventArgs e)
        {
            if (e.Exception != null)
            {
                m_db.Refresh(RefreshMode.OverwriteCurrentValues, m_entityBeingDeleted);
            }
            m_entityBeingDeleted = null;
        }

        //void PhpaLinqDataSource_Updating(object sender, LinqDataSourceUpdateEventArgs e)
        //{
        //    if (!string.IsNullOrEmpty(this.TransactionId))
        //    {
        //        TransactionInfo info = GetTransactionInfo(this.TransactionId);
        //        this.Database.Transaction = info.Transaction;
        //    }
        //}


        /// <summary>
        /// Do not dish out this database to anyone in the future.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void PhpaLinqDataSource_ContextDisposing(object sender, LinqDataSourceDisposeEventArgs e)
        {
            //if (!string.IsNullOrEmpty(this.TransactionId))
            //{
            //    TransactionInfo info = GetTransactionInfo(this.TransactionId);
            //    info.Release();
            //}
            m_db = null;
        }

        /// <summary>
        /// If the context was never created, we must dispose off the db now
        /// </summary>
        public override void Dispose()
        {
            if (m_db != null)
            {
                m_db.Dispose();
                m_db = null;
            }
            base.Dispose();
        }

        private DataContext m_db;

        //[Browsable(true)]
        //[DefaultValue("default")]
        //[Category("Phpa")]
        //[Description("Connect string id to use for database connection string")]
        //[Editor(typeof(System.Web.UI.Design.ConnectionStringEditor),
        //             typeof(System.Drawing.Design.UITypeEditor))]
        //public string ConnectStringId
        //{
        //    get;
        //    set;
        //}

        /// <summary>
        /// This class is reference counted
        /// </summary>
        //private class TransactionInfo
        //{
        //    private int m_referenceCount;

        //    public void AddRef()
        //    {
        //        m_referenceCount++;
        //    }

        //    public void Release()
        //    {
        //        if (m_referenceCount == 0)
        //        {
        //            throw new AssertFailedException("Releasing too many times");
        //        }
        //        --m_referenceCount;
        //        if (m_referenceCount == 0 && m_conn != null)
        //        {
        //            m_conn.Dispose();
        //        }
        //    }
        //    public string TransactionId { get; set; }

        //    private SqlConnection m_conn;

        //    private string _connecString;
        //    public SqlConnection GetConnection(string connecString)
        //    {
        //        if (string.IsNullOrEmpty(connecString))
        //        {
        //            throw new ArgumentNullException("connecString");
        //        }
        //        if (m_conn == null)
        //        {
        //            _connecString = connecString;
        //            m_conn = new SqlConnection(_connecString);
        //            m_conn.Open();
        //        }
        //        else
        //        {
        //            if (connecString != _connecString)
        //            {
        //                throw new InvalidOperationException("The connect string for all data sources must be same");
        //            }
        //        }
        //        return m_conn;
        //    }

        //    private DbTransaction m_transaction;

        //    public DbTransaction Transaction
        //    {
        //        get
        //        {
        //            if (m_transaction == null)
        //            {
        //                m_transaction = m_conn.BeginTransaction();
        //            }
        //            return m_transaction;
        //        }
        //    }

        //    public bool IsInTransaction
        //    {
        //        get
        //        {
        //            return m_transaction != null;
        //        }
        //    }

        //    /// <summary>
        //    /// Currently we dispose the connection after commit/rollback. If this connection is used
        //    /// after that, ConnectionDisposed exception will get generated.
        //    /// </summary>
        //    public void DisposeConnection()
        //    {
        //        if (m_conn != null)
        //        {
        //            m_conn.Dispose();
        //            m_conn = null;
        //        }
        //    }
        //}

        //private static TransactionInfo GetTransactionInfo(string transactionId)
        //{
        //    if (string.IsNullOrEmpty(transactionId))
        //    {
        //        throw new AssertFailedException("Details available only if transaction id is set");
        //    }
        //    string key = string.Format("{0}_{1}", typeof(PhpaLinqDataSource), transactionId);
        //    TransactionInfo info = (TransactionInfo)HttpContext.Current.Items[key];
        //    if (info == null)
        //    {
        //        info = new TransactionInfo();
        //        HttpContext.Current.Items[key] = info;
        //    }
        //    return info;
        //}

        //private string _transactionId = string.Empty;

        //[Browsable(false)]
        //[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        //public string TransactionId
        //{
        //    get
        //    {
        //        return _transactionId;
        //    }
        //    set
        //    {
        //        if (m_db != null)
        //        {
        //            // If a context exists, dispose it off
        //            m_db.Dispose();
        //            m_db = null;
        //        }
        //        _transactionId = value;
        //    }
        //}

        //public static void CommitTransaction(string transactionId, bool bCommit)
        //{
        //    TransactionInfo info = GetTransactionInfo(transactionId);
        //    if (bCommit)
        //    {
        //        info.Transaction.Commit();
        //    }
        //    else
        //    {
        //        info.Transaction.Rollback();
        //    }
        //}

        /// <summary>
        /// If you want to execute your own queries against the data context, use this property
        /// You can create the data context yourself and set it using this property. This is useful in
        /// transaction scenarios. You must create the context in PreContextCreating event.
        /// </summary>
        /// 
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public DataContext Database
        {
            get
            {
                if (m_db == null)
                {
                    m_db = CreateContext(this.ContextTypeName, Reporting.ReportingUtilities.DefaultConnectString);
                }
                return m_db;
            }
            set
            {
                if (m_db != null)
                {
                    throw new InvalidOperationException("Data context has already been created");
                }
                m_db = value;
            }
        }

        private DataContext CreateContext(string contextTypeName, SqlConnection sqlConnection)
        {
            DataContextBase db;
            if (contextTypeName == typeof(FinanceDataContext).ToString())
            {
                db = new FinanceDataContext(sqlConnection);
            }
            else if (contextTypeName == typeof(AuthenticationDataContext).ToString())
            {
                db = new AuthenticationDataContext(sqlConnection);
            }
            else if (contextTypeName == typeof(ReportingDataContext).ToString())
            {
                db = new ReportingDataContext(sqlConnection);
            }
            //else if (contextTypeName == typeof(PayrollDataContext).ToString())
            else if (contextTypeName == typeof(PayrollDataContext).ToString())
            {
                db = new PayrollDataContext(sqlConnection);
            }
            else if (contextTypeName == typeof(StoreDataContext).ToString())
            {
                db = new StoreDataContext(sqlConnection);
            }
            else if (contextTypeName == typeof(PISDataContext).ToString())
            {
                db = new PISDataContext(sqlConnection);
            }
            else if (contextTypeName == typeof(MISDataContext).ToString())
            {
                db = new MISDataContext(sqlConnection);
            }
            else
            {
                throw new NotSupportedException();
            }
            return db;

        }

        private static DataContextBase CreateContext(string contextTypeName, string strConnectString)
        {
            DataContextBase db;
            if (contextTypeName == typeof(FinanceDataContext).ToString())
            {
                db = new FinanceDataContext(strConnectString);
            }
            else if (contextTypeName == typeof(AuthenticationDataContext).ToString())
            {
                db = new AuthenticationDataContext(strConnectString);
            }
            else if (contextTypeName == typeof(ReportingDataContext).ToString())
            {
                db = new ReportingDataContext(strConnectString);
            }
            else if (contextTypeName == typeof(PayrollDataContext).ToString())
            {
                db = new PayrollDataContext(strConnectString);

            }
            else if (contextTypeName == typeof(StoreDataContext).ToString())
            {
                db = new StoreDataContext(strConnectString);
            }
            else if (contextTypeName == typeof(PISDataContext).ToString())
            {
                db = new PISDataContext(strConnectString);
            }
            else if (contextTypeName == typeof(MISDataContext).ToString())
            {
                db = new MISDataContext(strConnectString);
            }          
            else if (contextTypeName == typeof(PackageActivityDataContext).ToString())
            {
                db = new PackageActivityDataContext(strConnectString);
            }
            else
            {
                throw new NotSupportedException();
            }
            return db;
        }

        private void PhpaLinqDataSource_ContextCreating(object sender, LinqDataSourceContextEventArgs e)
        {
            //if (!string.IsNullOrEmpty(this.TransactionId))
            //{
            //    TransactionInfo info = GetTransactionInfo(this.TransactionId);
            //    info.AddRef();
            //}
            e.ObjectInstance = this.Database;
        }

        protected override void Render(System.Web.UI.HtmlTextWriter writer)
        {
            if (m_db == null)
            {
                // Happens when we are not asked to execute a query
                return;
            }
            DataContextBase db = (DataContextBase)m_db;
            if (this.RenderLogVisible)
            {
                writer.RenderBeginTag(HtmlTextWriterTag.Pre);
            }
            else
            {
                writer.WriteLineNoTabs("<!--****");
            }
            writer.WriteLineNoTabs("SQL log for " + this.ID);
            writer.WriteLineNoTabs(db.LogOutput.ToString());

            if (this.RenderLogVisible)
            {
                writer.RenderEndTag();
            }
            else
            {
                writer.WriteLineNoTabs("-->");
            }
            //base.Render(writer);
        }

        [Browsable(true)]
        [Description("Whether log should be rendered visible instead of within HTML comment")]
        [DefaultValue(true)]
        [Category("Phpa")]
        public bool RenderLogVisible { get; set; }
    }
}
