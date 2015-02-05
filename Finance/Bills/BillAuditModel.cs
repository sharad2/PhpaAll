using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PhpaAll.Bills
{
    public class BillAuditFieldChangeModel
    {
        public string FieldName { get; set; }

        public string OldValue { get; set; }

        public string NewValue { get; set; }
    }

    public class BillAuditModel
    {
        private readonly IList<BillAuditFieldChangeModel> _fieldChanges;
        internal BillAuditModel(BillAudit entity)
        {
            _fieldChanges = new List<BillAuditFieldChangeModel>();
            if (entity.RemarksOld != entity.RemarksNew)
            {
                _fieldChanges.Add(new BillAuditFieldChangeModel
                {
                    FieldName = "Remarks",
                    OldValue = entity.RemarksOld,
                    NewValue = entity.RemarksNew
                });
            }

            if (entity.SubmittedOnDateOld != entity.SubmittedOnDateNew)
            {
                _fieldChanges.Add(new BillAuditFieldChangeModel
                {
                    FieldName = "Submit Date",
                    OldValue = string.Format("{0:d}", entity.SubmittedOnDateOld),
                    NewValue = string.Format("{0:d}", entity.SubmittedOnDateNew)
                });
            }
        }

        [DataType(DataType.Date)]
        public DateTime? DateCreated { get; set; }

        public IList<BillAuditFieldChangeModel> FieldChanges
        {
            get
            {
                return _fieldChanges;
            }
        }

        public string BillNumberOld { get; set; }

        public string BillNumberNew { get; set; }

        [DataType(DataType.Date)]
        public DateTime? DueDateNew { get; set; }

        [DataType(DataType.Date)]
        public DateTime? DueDateOld { get; set; }

        public decimal? AmountNew { get; set; }

        public decimal? AmountOld { get; set; }

        public string ApprovedByNew { get; set; }

        public string ApprovedByOld { get; set; }

        [DataType(DataType.Date)]
        public DateTime? ApprovedOnNew { get; set; }

        [DataType(DataType.Date)]
        public DateTime? ApprovedOnOld { get; set; }

        [DataType(DataType.Date)]
        public DateTime? BillDateNew { get; set; }

        [DataType(DataType.Date)]
        public DateTime? BillDateOld { get; set; }

        public string ContractorNameNew { get; set; }

        public string ContractorNameOld { get; set; }

        [DataType(DataType.Date)]
        public DateTime? PaidDateNew { get; set; }

        [DataType(DataType.Date)]
        public DateTime? PaidDateOld { get; set; }

        public string ParticularsNew { get; set; }

        public string ParticularsOld { get; set; }

        public string RemarksNew { get; set; }

        public string RemarksOld { get; set; }

        [DataType(DataType.Date)]
        public DateTime? SubmittedOnDateNew { get; set; }

        [DataType(DataType.Date)]
        public DateTime? SubmittedOnDateOld { get; set; }

        public string SubmittedToDivisionNameNew { get; set; }

        public string SubmittedToDivisionNameOld { get; set; }
    }
}