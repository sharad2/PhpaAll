using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Linq;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;

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

            // Always load audit details with audit
            var dlo = new DataLoadOptions();
            dlo.LoadWith<BillAudit2>(p => p.BillAuditDetails);
            this.LoadOptions = dlo;
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
        /// Add rows to bill audit tables
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

            var table = this.GetTable(typeof(Bill));

            foreach (var billEntity in updatedEntities.OfType<Bill>())
            {
                // For each bill updated
                var modifiedMembers = table.GetModifiedMembers(billEntity);
                // Do something with the old values
                var audit = new BillAudit2();

                audit.BillId = billEntity.Id;
                audit.CreatedBy = _context.User.Identity.Name;
                audit.Created = DateTime.Now;

                this.BillAudit2s.InsertOnSubmit(audit);

                foreach (var prop in modifiedMembers)
                {
                    // For each property updated
                    var auditDetail = new BillAuditDetail();

                    var propInfo = (PropertyInfo)prop.Member;

                    var propType = Nullable.GetUnderlyingType(propInfo.PropertyType) ?? propInfo.PropertyType;

                    if (billEntity._list.ContainsKey(propInfo.Name))
                    {
                        var info = billEntity._list[propInfo.Name];

                        switch (info.IdKind)
                        {
                            case IdKindType.Division:
                                if (info.OldId.HasValue) { 
                                auditDetail.OldValue = Divisions.Where(p => p.DivisionId == (info.OldId ?? 0)).Select(p => p.DivisionName).FirstOrDefault();
                                }
                                if (info.NewId.HasValue)
                                {
                                    auditDetail.NewValue = Divisions.Where(p => p.DivisionId == (info.NewId ?? 0)).Select(p => p.DivisionName).FirstOrDefault();
                                }

                                break;

                            case IdKindType.Contractor:
                                break;

                            default:
                                break;
                        }
                        auditDetail.FieldType = (int)TypeCode.String;
                    }
                    else if (propType == typeof(DateTime))
                    {
                        // Round trip date time pattern
                        if (prop.OriginalValue != null)
                        {
                            auditDetail.OldValue = string.Format("{0:r}", prop.OriginalValue);
                        }
                        if (prop.CurrentValue != null)
                        {
                            auditDetail.NewValue = string.Format("{0:r}", prop.CurrentValue);
                        }
                        auditDetail.FieldType = (int)TypeCode.DateTime;  
                    }
                    else
                    {
                        // Store as simple string
                        if (prop.OriginalValue != null)
                        {
                            auditDetail.OldValue = prop.OriginalValue.ToString();
                        }
                        if (prop.CurrentValue != null)
                        {
                            auditDetail.NewValue = prop.CurrentValue.ToString();
                        }
                        auditDetail.FieldType = (int)Type.GetTypeCode(propType);  
                    }

                    auditDetail.FieldName = propInfo.Name;


                    auditDetail.CreatedBy = _context.User.Identity.Name;
                    auditDetail.Created = DateTime.Now;

                    audit.BillAuditDetails.Add(auditDetail);
                }

            }
            base.SubmitChanges(failureMode);
        }
    }


    internal enum IdKindType {
        Division,
        Contractor
    };

    internal class MyChanges
    {
        public IdKindType IdKind { get; set; }
        public int? OldId { get; set; }
        public int? NewId { get; set; }
    }

    internal partial class Bill
    {
        public Dictionary<string, MyChanges> _list = new Dictionary<string, MyChanges>();

        partial void OnContractorIdChanging(int? value)
        {
            _list["ContractorId"] = new MyChanges
            {
                IdKind = IdKindType.Contractor,
                OldId = this.ContractorId,
                NewId = value
            };
        }

        partial void OnDivisionIdChanging(int? value)
        {
            _list["DivisionId"] = new MyChanges
            {
                IdKind = IdKindType.Division,
                OldId = this.DivisionId,
                NewId = value
            };
        }

        partial void OnStationIdChanging(int value)
        {
            throw new NotImplementedException();
        }

        partial void OnAtDivisionIdChanging(int? value)
        {
            throw new NotImplementedException();
        }

    }

    internal partial class BillAuditDetail
    {
        /// <summary>
        /// Set the display values after an audit detail is loaded
        /// </summary>
        partial void OnLoaded()
        {
            var tc = FieldType.HasValue ? (TypeCode)FieldType : TypeCode.String;

            switch (tc)
            {
                case TypeCode.DateTime:
                    // Round trip date time pattern
                    if (OldValue != null)
                    {
                        // Here we might need to decide whether to show time or not
                        try
                        {
                            OldValueDisplay = string.Format("{0:g}", DateTime.Parse(OldValue));
                        }
                        catch (FormatException)
                        {
                            // Use the value as is
                            OldValueDisplay = "Date " + OldValue;
                        }
                    }
                    if (NewValue != null)
                    {
                        try
                        {
                            NewValueDisplay = string.Format("{0:g}", DateTime.Parse(NewValue));
                        }
                        catch (FormatException)
                        {
                            // Use the value as is
                            NewValueDisplay = "Date " + NewValue;
                        }
                    }
                    break;
                case TypeCode.Decimal:
                    // This must be amount so display with commas
                    if (OldValue != null)
                    {
                        OldValueDisplay = string.Format("{0:C}", decimal.Parse(OldValue));
                    }
                    if (NewValue != null)
                    {
                        NewValueDisplay = string.Format("{0:C}", decimal.Parse(NewValue));
                    }
                    break;
                default:
                    // Display as is
                    OldValueDisplay = OldValue;
                    NewValueDisplay = NewValue;
                    break;
            }
        }

        /// <summary>
        /// Returns the new value in a displayable format
        /// </summary>
        public string NewValueDisplay
        {
            get;
            private set;
        }

        public string OldValueDisplay
        {
            get;
            private set;
        }
    }


}
