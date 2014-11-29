using System.ComponentModel;
using System.Data.Linq;
using EclipseLibrary.Web.Extensions;
using System.Data.SqlClient;
using System;
using System.Linq;

namespace Eclipse.PhpaLibrary.Database.PIS
{
    public sealed partial class PISDataContext
    {
        partial void InsertEmployee(Employee instance)
        {

            if (string.IsNullOrEmpty(instance.EmployeeCode))
            {
                try
                {
                    var q = this.Employees.Max(p => Convert.ToInt32(p.EmployeeCode));
                    instance.EmployeeCode = (Convert.ToInt32(q) + 1).ToString();
                }
                catch (SqlException ex)
                {
                    if (ex.ErrorCode == -2146232060)
                    {
                        throw new ValidationException("When non numeric Employees codes are used, contractor code must be explicitly specified");
                    }
                }
            }

            SetAuditFields(instance, ChangeAction.Insert);
            this.ExecuteDynamicInsert(instance);
        }

        partial void UpdateEmployee(Employee instance)
        {
            SetAuditFields(instance, ChangeAction.Update);
            this.ExecuteDynamicUpdate(instance);
        }

        partial void InsertFamilyMember(FamilyMember instance)
        {
            SetAuditFields(instance, ChangeAction.Insert);
            this.ExecuteDynamicInsert(instance);
        }

        partial void UpdateFamilyMember(FamilyMember instance)
        {
            SetAuditFields(instance, ChangeAction.Update);
            this.ExecuteDynamicUpdate(instance);
        }

        partial void InsertNominee(Nominee instance)
        {
            SetAuditFields(instance, ChangeAction.Insert);
            this.ExecuteDynamicInsert(instance);
        }

        partial void UpdateNominee(Nominee instance)
        {
            SetAuditFields(instance, ChangeAction.Update);
            this.ExecuteDynamicUpdate(instance);
        }

        partial void InsertQualification(Qualification instance)
        {
            SetAuditFields(instance, ChangeAction.Insert);
            this.ExecuteDynamicInsert(instance);
        }

        partial void UpdateQualification(Qualification instance)
        {
            SetAuditFields(instance, ChangeAction.Update);
            this.ExecuteDynamicUpdate(instance);
        }

        partial void InsertServicePeriod(ServicePeriod instance)
        {
            SetAuditFields(instance, ChangeAction.Insert);
            this.ExecuteDynamicInsert(instance);
        }

        partial void UpdateServicePeriod(ServicePeriod instance)
        {
            SetAuditFields(instance, ChangeAction.Update);
            this.ExecuteDynamicUpdate(instance);
        }

        partial void InsertEmployeeGrant(EmployeeGrant instance)
        {
            SetAuditFields(instance, ChangeAction.Insert);
            this.ExecuteDynamicInsert(instance);
        }

        partial void UpdateEmployeeGrant(EmployeeGrant instance)
        {
            SetAuditFields(instance, ChangeAction.Update);
            this.ExecuteDynamicUpdate(instance);
        }

        partial void InsertMedicalRecord(MedicalRecord instance)
        {
            SetAuditFields(instance, ChangeAction.Insert);
            this.ExecuteDynamicInsert(instance);
        }

        partial void UpdateMedicalRecord(MedicalRecord instance)
        {
            SetAuditFields(instance, ChangeAction.Update);
            this.ExecuteDynamicUpdate(instance);
        }


        partial void InsertLeaveRecord(LeaveRecord instance)
        {
            SetAuditFields(instance, ChangeAction.Insert);
            this.ExecuteDynamicInsert(instance);
        }

        partial void UpdateLeaveRecord(LeaveRecord instance)
        {
            SetAuditFields(instance, ChangeAction.Update);
            this.ExecuteDynamicUpdate(instance);
        }

        partial void InsertTraining(Training instance)
        {
            SetAuditFields(instance, ChangeAction.Insert);
            this.ExecuteDynamicInsert(instance);
        }

        partial void UpdateTraining(Training instance)
        {
            SetAuditFields(instance, ChangeAction.Update);
            this.ExecuteDynamicUpdate(instance);
        }

        partial void InsertBloodGroup(BloodGroup instance)
        {
            SetAuditFields(instance, ChangeAction.Insert);
            this.ExecuteDynamicInsert(instance);
        }

        partial void UpdateBloodGroup(BloodGroup instance)
        {
            SetAuditFields(instance, ChangeAction.Update);
            this.ExecuteDynamicUpdate(instance);
        }

        partial void InsertEmployeeStatus(EmployeeStatus instance)
        {
            SetAuditFields(instance, ChangeAction.Insert);
            this.ExecuteDynamicInsert(instance);
        }

        partial void UpdateEmployeeStatus(EmployeeStatus instance)
        {
            SetAuditFields(instance, ChangeAction.Update);
            this.ExecuteDynamicUpdate(instance);
        }

        partial void InsertMaritalStatus(MaritalStatus instance)
        {
            SetAuditFields(instance, ChangeAction.Insert);
            this.ExecuteDynamicInsert(instance);
        }

        partial void UpdateMaritalStatus(MaritalStatus instance)
        {
            SetAuditFields(instance, ChangeAction.Update);
            this.ExecuteDynamicUpdate(instance);
        }

        partial void InsertPromotionType(PromotionType instance)
        {
            SetAuditFields(instance, ChangeAction.Insert);
            this.ExecuteDynamicInsert(instance);
        }

        partial void UpdatePromotionType(PromotionType instance)
        {
            SetAuditFields(instance, ChangeAction.Update);
            this.ExecuteDynamicUpdate(instance);
        }

        partial void InsertEntitlementType(EntitlementType instance)
        {
            SetAuditFields(instance, ChangeAction.Insert);
            this.ExecuteDynamicInsert(instance);
        }

        partial void UpdateEntitlementType(EntitlementType instance)
        {
            SetAuditFields(instance, ChangeAction.Update);
            this.ExecuteDynamicUpdate(instance);
        }

        partial void InsertTrainingType(TrainingType instance)
        {
            SetAuditFields(instance, ChangeAction.Insert);
            this.ExecuteDynamicInsert(instance);
        }

        partial void UpdateTrainingType(TrainingType instance)
        {
            SetAuditFields(instance, ChangeAction.Update);
            this.ExecuteDynamicUpdate(instance);
        }

        partial void InsertLeaveType(LeaveType instance)
        {
            SetAuditFields(instance, ChangeAction.Insert);
            this.ExecuteDynamicInsert(instance);
        }

        partial void UpdateLeaveType(LeaveType instance)
        {
            SetAuditFields(instance, ChangeAction.Update);
            this.ExecuteDynamicUpdate(instance);
        }

        partial void InsertCountry(Country instance)
        {
            SetAuditFields(instance, ChangeAction.Insert);
            this.ExecuteDynamicInsert(instance);
        }

        partial void UpdateCountry(Country instance)
        {
            SetAuditFields(instance, ChangeAction.Update);
            this.ExecuteDynamicUpdate(instance);
        }
        partial void InsertStation(Station instance)
        {
            SetAuditFields(instance, ChangeAction.Insert);
            this.ExecuteDynamicInsert(instance);
        }

        partial void UpdateStation(Station instance)
        {
            SetAuditFields(instance, ChangeAction.Update);
            this.ExecuteDynamicUpdate(instance);
        }
        partial void InsertBank(Bank instance)
        {
            SetAuditFields(instance, ChangeAction.Insert);
            this.ExecuteDynamicInsert(instance);
        }

        partial void UpdateBank(Bank instance)
        {
            SetAuditFields(instance, ChangeAction.Update);
            this.ExecuteDynamicUpdate(instance);
        }

        partial void InsertQualificationDivision(QualificationDivision instance)
        {
            SetAuditFields(instance, ChangeAction.Insert);
            this.ExecuteDynamicInsert(instance);
        }

        partial void UpdateQualificationDivision(QualificationDivision instance)
        {
            SetAuditFields(instance, ChangeAction.Update);
            this.ExecuteDynamicUpdate(instance);
        }

    }

    partial class Employee : IAuditable
    {
        public string FullName
        {
            get
            {
                return string.Format("{0} {1}", this.FirstName, this.LastName);
            }

        }
    }

    partial class FamilyMember : IAuditable
    {

    }

    partial class Nominee : IAuditable
    {

    }

    partial class Qualification : IAuditable
    {

    }

    partial class ServicePeriod : IAuditable
    {
        public string DateRange
        {
            get
            {
                return string.Format("{0:d}-{1:d}", this.PeriodStartDate, this.PeriodEndDate);
            }

        }

        public string PayScale
        {
            get
            {
                string str = string.Format("{0:N2}-{1:N2}-{2:N2}", this.MinPayScaleAmount,
                    this.IncrementAmount, this.MaxPayScaleAmount);
                return str;
            }

        }
    }

    partial class EmployeeGrant : IAuditable
    {

    }

    partial class MedicalRecord : IAuditable
    {

    }

    partial class LeaveRecord : IAuditable
    {

    }

    partial class Training : IAuditable
    {

    }

    partial class BloodGroup : IAuditable
    {

    }

    partial class EmployeeStatus : IAuditable
    {

    }

    partial class MaritalStatus : IAuditable
    {

    }

    partial class PromotionType : IAuditable
    {

    }

    partial class EntitlementType : IAuditable
    {

    }

    partial class TrainingType : IAuditable
    {

    }

    partial class LeaveType : IAuditable
    {

    }

    partial class Country : IAuditable
    {

    }

    partial class QualificationDivision : IAuditable
    {

    }
    partial class Station : IAuditable
    {

    }
    partial class Bank : IAuditable
    {

    }

}
