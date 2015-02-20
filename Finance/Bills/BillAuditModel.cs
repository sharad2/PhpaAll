using Eclipse.PhpaLibrary.Web.Providers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Security;

namespace PhpaAll.Bills
{
    public class BillAuditFieldChangeModel
    {
        public string FieldName { get; set; }

        [DisplayFormat(NullDisplayText = "(Not Set)")]
        public string OldValue { get; set; }

        [DisplayFormat(NullDisplayText = "(Not Set)")]
        public string NewValue { get; set; }
    }

    public class BillAuditModel
    {
        private readonly IList<BillAuditFieldChangeModel> _fieldChanges;
        internal BillAuditModel(BillAudit2 entity)
        {
            DateCreated = entity.Created;

            var user = Membership.GetUser(entity.CreatedBy) as PhpaMembershipUser;
            CreatedBy = user == null ? entity.CreatedBy : user.FullName;

            _fieldChanges = new List<BillAuditFieldChangeModel>();

            if (entity.BillAuditDetails != null)
            {
                _fieldChanges = (from change in entity.BillAuditDetails
                                 select new BillAuditFieldChangeModel
                                 {
                                     FieldName = change.FieldName,
                                     NewValue = change.NewValue,
                                     OldValue = change.OldValue
                                 }).ToList();
            }
            else
            {
                _fieldChanges = new BillAuditFieldChangeModel[0];       // Empty array
            }
            //if (entity.RemarksOld != entity.RemarksNew)
            //{
            //    _fieldChanges.Add(new BillAuditFieldChangeModel
            //    {
            //        FieldName = "Remarks",
            //        OldValue = entity.RemarksOld,
            //        NewValue = entity.RemarksNew
            //    });
            //}

            //if (entity.SubmittedOnDateOld != entity.SubmittedOnDateNew)
            //{
            //    _fieldChanges.Add(new BillAuditFieldChangeModel
            //    {
            //        FieldName = "Submit Date",
            //        OldValue = string.Format("{0:d}", entity.SubmittedOnDateOld),
            //        NewValue = string.Format("{0:d}", entity.SubmittedOnDateNew)
            //    });
            //}

            //if (entity.SubmittedToDivisionNameNew != entity.SubmittedToDivisionNameOld)
            //{
            //    _fieldChanges.Add(new BillAuditFieldChangeModel
            //    {
            //        FieldName = "Division",
            //        OldValue = entity.SubmittedToDivisionNameOld,
            //        NewValue = entity.SubmittedToDivisionNameNew
            //    });
            //}
            //if (entity.PaidDateNew != entity.PaidDateOld)
            //{
            //    _fieldChanges.Add(new BillAuditFieldChangeModel
            //    {
            //        FieldName = "Due Date",
            //        OldValue = string.Format("{0:d}", entity.PaidDateOld),
            //        NewValue = string.Format("{0:d}", entity.PaidDateNew)
            //    });
            //}
            //if (entity.DueDateNew != entity.DueDateNew)
            //{
            //    _fieldChanges.Add(new BillAuditFieldChangeModel
            //    {
            //        FieldName = "Due Date",
            //        OldValue = string.Format("{0:d}", entity.DueDateOld),
            //        NewValue = string.Format("{0:d}", entity.DueDateOld)
            //    });
            //}
            //if (entity.ContractorNameNew != entity.ContractorNameOld)
            //{
            //    _fieldChanges.Add(new BillAuditFieldChangeModel
            //    {
            //        FieldName = "Contractor Name",
            //        OldValue = entity.ContractorNameOld,
            //        NewValue = entity.ContractorNameNew
            //    });
            //}
            //if (entity.BillNumberNew != entity.BillNumberOld)
            //{
            //    _fieldChanges.Add(new BillAuditFieldChangeModel
            //    {
            //        FieldName = "Bill Number",
            //        OldValue = entity.BillNumberOld,
            //        NewValue = entity.BillNumberNew
            //    });
            //}
            //if (entity.BillDateNew != entity.BillDateOld)
            //{
            //    _fieldChanges.Add(new BillAuditFieldChangeModel
            //    {
            //        FieldName = "Bill Date",
            //        OldValue = string.Format("{0:d}", entity.BillDateOld),
            //        NewValue = string.Format("{0:d}", entity.BillDateNew)
            //    });
            //}
            //if (entity.ApprovedOnNew != entity.ApprovedOnOld)
            //{
            //    _fieldChanges.Add(new BillAuditFieldChangeModel
            //    {
            //        FieldName = "Approved On",
            //        OldValue = string.Format("{0:g}", entity.ApprovedOnOld),
            //        NewValue = string.Format("{0:g}", entity.ApprovedOnNew)
            //    });
            //}
            //if (entity.AmountNew != entity.AmountOld)
            //{
            //    _fieldChanges.Add(new BillAuditFieldChangeModel
            //    {
            //        FieldName = "Amount",
            //        OldValue = string.Format("{0:N2}", entity.AmountOld),
            //        NewValue = string.Format("{0:N2}", entity.AmountNew)
            //    });
            //}


        }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:g}")]
        public DateTime? DateCreated { get; set; }

        public string CreatedBy { get; set; }

        public IList<BillAuditFieldChangeModel> FieldChanges
        {
            get
            {
                return _fieldChanges;
            }
        }

        //public string BillNumberOld { get; set; }

        //public string BillNumberNew { get; set; }

        //[DataType(DataType.Date)]
        //public DateTime? DueDateNew { get; set; }

        //[DataType(DataType.Date)]
        //public DateTime? DueDateOld { get; set; }

        //public decimal? AmountNew { get; set; }

        //public decimal? AmountOld { get; set; }

        //public string ApprovedByNew { get; set; }

        //public string ApprovedByOld { get; set; }

        //[DataType(DataType.Date)]
        //public DateTime? ApprovedOnNew { get; set; }

        //[DataType(DataType.Date)]
        //public DateTime? ApprovedOnOld { get; set; }

        //[DataType(DataType.Date)]
        //public DateTime? BillDateNew { get; set; }

        //[DataType(DataType.Date)]
        //public DateTime? BillDateOld { get; set; }

        //public string ContractorNameNew { get; set; }

        //public string ContractorNameOld { get; set; }

        //[DataType(DataType.Date)]
        //public DateTime? PaidDateNew { get; set; }

        //[DataType(DataType.Date)]
        //public DateTime? PaidDateOld { get; set; }

        //public string ParticularsNew { get; set; }

        //public string ParticularsOld { get; set; }

        //public string RemarksNew { get; set; }

        //public string RemarksOld { get; set; }

        //public string StationNameNew { get; set; }

        //public string StationNameOld { get; set; }

        //[DataType(DataType.Date)]
        //public DateTime? SubmittedOnDateNew { get; set; }

        //[DataType(DataType.Date)]
        //public DateTime? SubmittedOnDateOld { get; set; }

        //public string SubmittedToDivisionNameNew { get; set; }

        //public string SubmittedToDivisionNameOld { get; set; }


    }
}