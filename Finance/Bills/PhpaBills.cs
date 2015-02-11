using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Linq;
using System.IO;
using System.Web;
using System.Linq;

namespace PhpaAll.Bills
{
    /// <summary>
    /// Adds capability to trace queries. Always uses the default connection string
    /// </summary>
    partial class PhpaBillsDataContext
    {
        private readonly StringWriter _sw;

        //[Obsolete]
        //private readonly TraceContext _trace;

        private HttpContextBase _context;

        public PhpaBillsDataContext(HttpContextBase context)
            : base(ConfigurationManager.ConnectionStrings["default"].ConnectionString)
        {
            _context = context;

            _sw = new StringWriter();
            this.Log = _sw;
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (_sw != null)
            {
                _context.Trace.Write(_sw.ToString());
            }
        }

        partial void UpdateBill(Bill instance)
        {
            instance.ModifiedBy = _context.User.Identity.Name;
            instance.Modified = DateTime.Now;
            ExecuteDynamicUpdate(instance);
        }

        partial void InsertBill(PhpaAll.Bills.Bill instance)
        {
            instance.CreatedBy = _context.User.Identity.Name;
            ExecuteDynamicInsert(instance);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="failureMode"></param>
        /// <remarks>
        /// http://stackoverflow.com/questions/9158237/linq-to-sql-audit-trail-find-old-values-in-submitchanges
        /// </remarks>
        public override void SubmitChanges(System.Data.Linq.ConflictMode failureMode)
        {
            ChangeSet changeSet = this.GetChangeSet();

            // Put the updated objects into a IEnumerable
            IEnumerable<object> updatedEntities = changeSet.Updates;

            foreach (var bill in updatedEntities.OfType<Bill>())
            {
                // For each bill updated
                var properties = this.GetTable(bill.GetType()).GetModifiedMembers(bill);
                // Do something with the old values
                var audit = new BillAudit2();

                audit.BillId = bill.Id;
                audit.CreatedBy = _context.User.Identity.Name;
                audit.Created = DateTime.Now;

                this.BillAudit2s.InsertOnSubmit(audit);

                foreach (var prop in properties)
                {
                    // For each property updated
                    var auditDetail = new BillAuditDetail();

                    auditDetail.OldValue = string.Format("{0}", prop.OriginalValue);
                    auditDetail.NewValue = string.Format("{0}", prop.CurrentValue);
                    auditDetail.FieldName = prop.Member.Name;
                    auditDetail.CreatedBy = _context.User.Identity.Name;
                    auditDetail.Created = DateTime.Now;

                    audit.BillAuditDetails.Add(auditDetail);
                }

            }
            base.SubmitChanges(failureMode);
        }
    }
    

}
