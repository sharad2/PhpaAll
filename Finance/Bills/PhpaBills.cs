using System;
using System.Configuration;
using System.IO;
using System.Web;

namespace PhpaAll.Bills
{
    /// <summary>
    /// Adds capability to trace queries. Always uses the default connection string
    /// </summary>
    partial class PhpaBillsDataContext
    {
        private readonly StringWriter _sw;

        private readonly TraceContext _trace;

        public PhpaBillsDataContext(TraceContext trace): base(ConfigurationManager.ConnectionStrings["default"].ConnectionString)
        {
            _trace = trace;

            _sw = new StringWriter();
            this.Log = _sw;
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (_sw != null)
            {
                _trace.Write(_sw.ToString());
            }
        }

        partial void UpdateBill(Bill instance)
        {
            var info = this.Bills.GetModifiedMembers(instance);

            foreach (var x in info)
            {
                var audit = new BillAudit();
                var y = x.OriginalValue;
                var y1 = x.CurrentValue;
                var y2 = x.Member.Name;
                this.BillAudits.InsertOnSubmit(audit);
            }
            throw new NotImplementedException("TODO: Add update audit");

            instance.ModifiedBy = HttpContext.Current.User.Identity.Name;
            instance.Modified = DateTime.Now;
            ExecuteDynamicUpdate(instance);
        }

        partial void InsertBill(PhpaAll.Bills.Bill instance)
        {
            instance.CreatedBy = HttpContext.Current.User.Identity.Name;
            ExecuteDynamicInsert(instance);
        }

    }
}
