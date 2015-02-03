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

    }
}
