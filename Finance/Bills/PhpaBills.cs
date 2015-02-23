using Eclipse.PhpaLibrary.Web.Providers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Linq;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Security;

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
            var user = Membership.GetUser(_context.User.Identity.Name) as PhpaMembershipUser;
            if (user == null)
            {
                instance.CreatedBy = _context.User.Identity.Name;
            }
            else
            {
                instance.CreatedBy = user.FullName;
            }
           // instance.CreatedBy = _context.User.Identity.Name;
            instance.Created = DateTime.Now;
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

                var user = Membership.GetUser(_context.User.Identity.Name) as PhpaMembershipUser;
                audit.BillId = billEntity.Id;
                if (user == null)
                {
                    audit.CreatedBy = _context.User.Identity.Name;
                }
                else
                {
                    audit.CreatedBy = user.FullName;
                }
                audit.CreatedBy = _context.User.Identity.Name;
                audit.Created = DateTime.Now;

                this.BillAudit2s.InsertOnSubmit(audit);

                foreach (var prop in modifiedMembers)
                {
                    // For each property updated
                    var auditDetail = new BillAuditDetail();

                    var propInfo = (PropertyInfo)prop.Member;

                    var propType = Nullable.GetUnderlyingType(propInfo.PropertyType) ?? propInfo.PropertyType;

                    int id;
                    if (billEntity.Changes.ContainsKey(propInfo.Name))
                    {
                        var info = billEntity.Changes[propInfo.Name];

                        
                        switch (info.IdKind)
                        {
                            
                            //Value of non id columns are store as is.
                            case IdKindType.None:
                                auditDetail.OldValue = info.OldValue;
                                auditDetail.NewValue = info.NewValue;
                                auditDetail.FieldName = info.FieldDisplayName;
                                break;
                            //for all Id columns retreinve the value against the Id from their master table, before inserting into audit detail table.
                            case IdKindType.Division:
                            case IdKindType.AtDivision:
                                if (!string.IsNullOrWhiteSpace(info.OldValue))
                                {
                                    id = int.Parse(info.OldValue);
                                    auditDetail.OldValue = Divisions.Where(p => p.DivisionId == id).Select(p => p.DivisionName).FirstOrDefault();
                                    auditDetail.FieldName = info.FieldDisplayName;
                                }
                                if (!string.IsNullOrWhiteSpace(info.NewValue))
                                {
                                    id = int.Parse(info.NewValue);
                                    auditDetail.NewValue = Divisions.Where(p => p.DivisionId == id).Select(p => p.DivisionName).FirstOrDefault();
                                    auditDetail.FieldName = info.FieldDisplayName;
                                }

                                break;

                            case IdKindType.Contractor:
                                if (!string.IsNullOrWhiteSpace(info.OldValue))
                                {
                                    id = int.Parse(info.OldValue);
                                    auditDetail.OldValue = Contractors.Where(p => p.ContractorId == id).Select(p => p.ContractorName).FirstOrDefault();
                                    auditDetail.FieldName = info.FieldDisplayName;
                                }
                                if (!string.IsNullOrWhiteSpace(info.NewValue))
                                {
                                    id = int.Parse(info.NewValue);
                                    auditDetail.NewValue = Contractors.Where(p => p.ContractorId == id).Select(p => p.ContractorName).FirstOrDefault();
                                    auditDetail.FieldName = info.FieldDisplayName;
                                }
                                break;
                            case IdKindType.Station:
                                if (!string.IsNullOrWhiteSpace(info.OldValue))
                                {
                                    id = int.Parse(info.OldValue);
                                    auditDetail.OldValue = Stations.Where(p => p.StationId == id).Select(p => p.StationName).FirstOrDefault();
                                    auditDetail.FieldName = info.FieldDisplayName;
                                }
                                if (!string.IsNullOrWhiteSpace(info.NewValue))
                                {
                                    id = int.Parse(info.NewValue);
                                    auditDetail.NewValue = Stations.Where(p => p.StationId == id).Select(p => p.StationName).FirstOrDefault();
                                    auditDetail.FieldName = info.FieldDisplayName;
                                }
                                break;

                            default:
                                break;
                        }
                        //auditDetail.FieldType = (int)TypeCode.String;
                        //auditDetail.FieldName = propInfo.Name;
                        auditDetail.CreatedBy = _context.User.Identity.Name;
                        auditDetail.Created = DateTime.Now;

                        audit.BillAuditDetails.Add(auditDetail);
                    }
                    //else if (propType == typeof(DateTime))
                    //{
                    //    // Round trip date time pattern
                    //    if (prop.OriginalValue != null)
                    //    {
                    //        auditDetail.OldValue = string.Format("{0:r}", prop.OriginalValue);
                    //    }
                    //    if (prop.CurrentValue != null)
                    //    {
                    //        auditDetail.NewValue = string.Format("{0:r}", prop.CurrentValue);
                    //    }
                    //    //auditDetail.FieldType = (int)TypeCode.DateTime;  
                    //}
                    //else
                    //{
                    //    // Store as simple string
                    //    if (prop.OriginalValue != null)
                    //    {
                    //        auditDetail.OldValue = prop.OriginalValue.ToString();
                    //    }
                    //    if (prop.CurrentValue != null)
                    //    {
                    //        auditDetail.NewValue = prop.CurrentValue.ToString();
                    //    }
                    //    //auditDetail.FieldType = (int)Type.GetTypeCode(propType);  
                    //}





                }

            }
            base.SubmitChanges(failureMode);
        }
    }

    
    internal enum IdKindType
    {
        None,
        Division,
        AtDivision,
        Contractor,
        Station
    };



    internal partial class Bill
    {
        internal class BillFieldChanges
        {
            public IdKindType IdKind { get; set; }

            /// <summary>
            /// If IdKind is not None, then this is an id
            /// </summary>
            public string OldValue { get; set; }
            public string NewValue { get; set; }
            public string FieldDisplayName { get; set; }
        }

        private Dictionary<string, BillFieldChanges> _dict;

        public Dictionary<string, BillFieldChanges> Changes
        {
            get
            {
                if (_dict == null)
                {
                    _dict = new Dictionary<string, BillFieldChanges>();
                }
                return _dict;
            }
        }

       
        partial void OnContractorIdChanging(int? value)
        {
            Changes["ContractorId"] = new BillFieldChanges
            {
                IdKind = IdKindType.Contractor,
                OldValue = this.ContractorId.HasValue ? this.ContractorId.Value.ToString() : null,
                NewValue = value.HasValue ? value.Value.ToString() : null,
                FieldDisplayName = "Contractor",
            };
        }

        partial void OnDivisionIdChanging(int? value)
        {
            Changes["DivisionId"] = new BillFieldChanges
            {
                IdKind = IdKindType.Division,
                OldValue = this.DivisionId.HasValue ? this.DivisionId.Value.ToString() : null,
                NewValue = value.HasValue ? value.Value.ToString() : null,
                FieldDisplayName = "Division",
            };
        }

        partial void OnStationIdChanging(int value)
        {
            Changes["StationId"] = new BillFieldChanges
            {
                IdKind = IdKindType.Station,
                OldValue = this.StationId.ToString(),
                NewValue = value.ToString(),
                FieldDisplayName = "Station",
            };
        }

        partial void OnAtDivisionIdChanging(int? value)
        {
            Changes["AtDivisionId"] = new BillFieldChanges
            {
                IdKind = IdKindType.AtDivision,
                OldValue = this.AtDivisionId.HasValue ? this.AtDivisionId.Value.ToString() : null,
                NewValue = value.HasValue ? value.Value.ToString() : null,
                FieldDisplayName = "Current Division",
            };
        }
        /// <summary>
        /// Stroing formated value 
        /// </summary>
        /// <param name="value"></param>
        partial void OnAmountChanging(decimal? value)
        {
            Changes["Amount"] = new BillFieldChanges
            {
                IdKind = IdKindType.None,
                OldValue = string.Format("{0:C}", this.Amount),
                NewValue = string.Format("{0:C}", value),
                FieldDisplayName = "Amount",
            };
        }
        partial void OnApprovedOnChanging(DateTime? value)
        {
            Changes["ApprovedOn"] = new BillFieldChanges
            {
                IdKind = IdKindType.None,
                OldValue = string.Format("{0:g}", this.ApprovedOn),
                NewValue = string.Format("{0:g}", value),
                FieldDisplayName = "Approved On",
            };
        }

        partial void OnApprovedByChanging(string value)
        {
            var changes = new BillFieldChanges
            {
                IdKind = IdKindType.None,
                OldValue = this.ApprovedBy,
                NewValue = !string.IsNullOrEmpty(value) ? value.ToString() : null,
                FieldDisplayName = "Approved By",
            };

            if (!string.IsNullOrWhiteSpace(this.ApprovedBy))
            {
                var user = Membership.GetUser(this.ApprovedBy) as PhpaMembershipUser;
                if (user == null) {
                    changes.OldValue = this.ApprovedBy;
                }
                else
                {
                    changes.OldValue = user.FullName;
                }
            }

            if (!string.IsNullOrWhiteSpace(value))
            {
                var user = Membership.GetUser(value) as PhpaMembershipUser;
                if (user == null)
                {
                    changes.NewValue = value;
                }
                else
                {
                    changes.NewValue = user.FullName;
                }
            }


            Changes["ApprovedBy"] = changes;
        }

        partial void OnBillDateChanging(DateTime? value)
        {
            Changes["BillDate"] = new BillFieldChanges
            {
                IdKind = IdKindType.None,
                OldValue = string.Format("{0:d}", this.BillDate),
                NewValue = string.Format("{0:d}", value),
                FieldDisplayName = "Bill Date",
            };
        }

        partial void OnBillNumberChanging(string value)
        {
            Changes["BillNumber"] = new BillFieldChanges
            {
                IdKind = IdKindType.None,
                OldValue = this.BillNumber,
                NewValue = !string.IsNullOrEmpty(value) ? value : null,
                FieldDisplayName = "Bill Number",
            };
        }

        partial void OnDueDateChanging(DateTime? value)
        {
            Changes["DueDate"] = new BillFieldChanges
            {
                IdKind = IdKindType.None,
                OldValue = string.Format("{0:d}", this.DueDate),
                NewValue = string.Format("{0:d}", value),
                FieldDisplayName = "Due Date",
            };
        }

        partial void OnParticularsChanging(string value)
        {
            Changes["Particulars"] = new BillFieldChanges
            {
                IdKind = IdKindType.None,
                OldValue = this.Particulars,
                NewValue = !string.IsNullOrEmpty(value) ? value : null,
                FieldDisplayName = "Particulars"
            };
        }

        partial void OnReceivedDateChanging(DateTime? value)
        {
            Changes["ReceivedDate"] = new BillFieldChanges
            {
                IdKind = IdKindType.None,
                OldValue = string.Format("{0:d}", this.ReceivedDate),
                NewValue = string.Format("{0:d}", value),
                FieldDisplayName = "Received Date"
            };
        }
        partial void OnRemarksChanging(string value)
        {
            Changes["Remarks"] = new BillFieldChanges
            {
                IdKind = IdKindType.None,
                OldValue = this.Remarks,
                NewValue = !string.IsNullOrEmpty(value) ? value : null,
                FieldDisplayName = "Remarks",
            };
        }

        public byte[] BillImage { get; set; }
    }

    //internal partial class BillAuditDetail
    //{
    //    /// <summary>
    //    /// Set the display values after an audit detail is loaded
    //    /// </summary>
    //    partial void OnLoaded()
    //    {
    //        var tc = FieldType.HasValue ? (TypeCode)FieldType : TypeCode.String;

    //        switch (tc)
    //        {
    //            case TypeCode.DateTime:
    //                // Round trip date time pattern
    //                if (OldValue != null)
    //                {
    //                    // Here we might need to decide whether to show time or not
    //                    try
    //                    {
    //                        OldValueDisplay = string.Format("{0:g}", DateTime.Parse(OldValue));
    //                    }
    //                    catch (FormatException)
    //                    {
    //                        // Use the value as is
    //                        OldValueDisplay = "Date " + OldValue;
    //                    }
    //                }
    //                if (NewValue != null)
    //                {
    //                    try
    //                    {
    //                        NewValueDisplay = string.Format("{0:g}", DateTime.Parse(NewValue));
    //                    }
    //                    catch (FormatException)
    //                    {
    //                        // Use the value as is
    //                        NewValueDisplay = "Date " + NewValue;
    //                    }
    //                }
    //                break;
    //            case TypeCode.Decimal:
    //                // This must be amount so display with commas
    //                if (OldValue != null)
    //                {
    //                    OldValueDisplay = string.Format("{0:C}", decimal.Parse(OldValue));
    //                }
    //                if (NewValue != null)
    //                {
    //                    NewValueDisplay = string.Format("{0:C}", decimal.Parse(NewValue));
    //                }
    //                break;
    //            default:
    //                // Display as is
    //                OldValueDisplay = OldValue;
    //                NewValueDisplay = NewValue;
    //                break;
    //        }
    //    }

    //    /// <summary>
    //    /// Returns the new value in a displayable format
    //    /// </summary>
    //    public string NewValueDisplay
    //    {
    //        get;
    //        private set;
    //    }

    //    public string OldValueDisplay
    //    {
    //        get;
    //        private set;
    //    }
    //}


}
