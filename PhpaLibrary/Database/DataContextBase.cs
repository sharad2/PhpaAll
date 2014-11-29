using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Data.SqlClient;
using System.IO;
using System.Reflection;
using System.Security;
using System.Text;
using System.Web;
using System.Web.UI.WebControls;

namespace Eclipse.PhpaLibrary.Database
{
    /// <summary>
    /// Encapsulates all the audit columns which we have added to each table
    /// </summary>
    public interface IAuditable
    {
        DateTime? Created { get; set; }
        string CreatedBy { get; set; }
        string CreatedWorkstation { get; set; }
        DateTime? Modified { get; set; }
        string ModifiedBy { get; set; }
        string ModifiedWorkstation { get; set; }
    }

    public abstract class DataContextBase : DataContext
    {
        internal const string CONTEXT_KEY = "DataContextBase";
        /// <summary>
        /// For debugging purposes counts the number of instances which have been created
        /// </summary>
        private void AddRef()
        {
            int? count = (int?)HttpContext.Current.Items[CONTEXT_KEY];
            if (count.HasValue)
            {
                ++count;
            }
            else
            {
                count = 1;
            }
            HttpContext.Current.Items[CONTEXT_KEY] = count;
        }

        protected override void Dispose(bool disposing)
        {
            if (HttpContext.Current != null && HttpContext.Current.Items[CONTEXT_KEY] != null)
            {
                // Current is null when created within a service
                int count = (int)HttpContext.Current.Items[CONTEXT_KEY];
                --count;
                if (count == 0)
                {
                    HttpContext.Current.Items[CONTEXT_KEY] = null;
                }
                else
                {
                    HttpContext.Current.Items[CONTEXT_KEY] = count;
                }
            }
            base.Dispose(disposing);
        }
        /// <summary>
        /// Anonymous user should never be able submit any changes
        /// </summary>
        /// <param name="failureMode"></param>
        public override void SubmitChanges(ConflictMode failureMode)
        {
            if (!HttpContext.Current.User.Identity.IsAuthenticated)
            {
                throw new SecurityException("Anonymous users are not allowed to make any modificataions");
            }
            base.SubmitChanges(failureMode);
        }

        protected DataContextBase(string connectString, MappingSource mappingSource)
            : base(connectString)
        {
            m_sbLog = new StringBuilder();
            this.Log = new StringWriter(m_sbLog);
            AddRef();
        }

        protected DataContextBase(System.Data.IDbConnection connection, MappingSource mappingSource)
            : base(connection)
        {
            m_sbLog = new StringBuilder();
            this.Log = new StringWriter(m_sbLog);
            AddRef();
        }

        private readonly StringBuilder m_sbLog;

        public StringBuilder LogOutput
        {
            get
            {
                return m_sbLog;
            }
        }

        /// <summary>
        /// Executes a query to return the value returned by user_name function
        /// </summary>
        /// <returns></returns>
        [Function(Name = "user_name", IsComposable = true)]
        protected string DbExecuteUserName()
        {
            return (string)(this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod()))).ReturnValue);
        }

        /// <summary>
        /// Returns a readable string depending on the exception type
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public static string GetExceptionMessage(Exception e)
        {
            StringBuilder sb = new StringBuilder();
            if (e is LinqDataSourceValidationException)
            {
                LinqDataSourceValidationException le = e as LinqDataSourceValidationException;
                // This is a data validation exception
                foreach (KeyValuePair<string, Exception> kvp in le.InnerExceptions)
                {
                    sb.AppendFormat("{0} : {1}", kvp.Key, kvp.Value.Message);
                }
            }
            else if (e is SqlException)
            {
                sb.Append(e.Message);
            }
            else
            {
                sb.Append(e.ToString());
            }
            return sb.ToString();
        }

        /// <summary>
        /// Sets reasonable values for all audit columns.
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="action"></param>
        protected void SetAuditFields(IAuditable instance, ChangeAction action)
        {
            if (action == ChangeAction.None || action == ChangeAction.Delete)
            {
                throw new ArgumentOutOfRangeException();
            }
            string userName = HttpContext.Current.User.Identity.Name;
            if (string.IsNullOrEmpty(userName))
            {
                // Anonymous user
                // TODO: Raise exception here. Anonymous users should not be able to modify anything
                userName = "Anonymous";
            }
            if (action == ChangeAction.Insert)
            {
                instance.CreatedWorkstation = HttpContext.Current.Request.UserHostName;

                instance.Created = DateTime.Now;
                instance.CreatedBy = userName;
            }
            instance.ModifiedWorkstation = HttpContext.Current.Request.UserHostName;
            instance.Modified = DateTime.Now;
            instance.ModifiedBy = userName;

        }

    }
}
