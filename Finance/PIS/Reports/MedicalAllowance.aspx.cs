using System;
using System.Linq;
using System.Web.UI.WebControls;
using Eclipse.PhpaLibrary.Database.PIS;
using Eclipse.PhpaLibrary.Web;

namespace PIS.Reports
{
    public partial class MedicalAllowance : PageBase
    {              
        protected void dsMedAllowance_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {            
            PISDataContext db = (PISDataContext)dsMedAllowance.Database;
            IQueryable<MedicalRecord> query = db.MedicalRecords;
            if (dtFrom.ValueAsDate != null)
            {              
                query = query.Where(p => p.OrderDate >= dtFrom.ValueAsDate);
            }

            if (dtTo.ValueAsDate != null)
            {                
                query = query.Where(p => p.OrderDate <= dtTo.ValueAsDate);
            }

            if (!string.IsNullOrEmpty(ddlCountry.Value))
            {
                query = query.Where(p => p.CountryId == Convert.ToInt32(ddlCountry.Value));
            }
          
            e.Result = from medRec in query
                       orderby medRec.ServicePeriod.Employee.FirstName
                       select new
                       {
                           FirstName = medRec.ServicePeriod.Employee.FirstName,
                           FullName = medRec.ServicePeriod.Employee.FullName,
                           Designation = medRec.ServicePeriod.Designation,
                           Grade = medRec.ServicePeriod.Grade,
                           EmpStatus = medRec.ServicePeriod.Employee.EmployeeStatus.EmployeeStatusType,
                           RefNo = medRec.ReferalNo,
                           HospitalName = medRec.HospitalName,
                           HospitalAddr = medRec.HospitalAddress,
                           HospitalIn = medRec.Country,
                           Amount = medRec.GrantedAmount,
                           AdvanceAdjusted = medRec.AdvanceAdjusted,
                           PendingAdjustment = medRec.AdvanceAdjusted > medRec.GrantedAmount ? medRec.AdvanceAdjusted - medRec.GrantedAmount : 0,
                           Balance = medRec.GrantedAmount > medRec.AdvanceAdjusted ? medRec.GrantedAmount - medRec.AdvanceAdjusted : 0,
                           OffOrderNo = medRec.OfficeOrderNo,
                           OrderDate = medRec.OrderDate,
                           Remarks = medRec.Remarks,
                           PatientName = medRec.FamilyMember.FullName ?? "Self",
                           Relationship = medRec.FamilyMember.Relationship,
                           EmployeeId = medRec.ServicePeriod.EmployeeId,
                           CountryId = medRec.CountryId,
                           ServicePeriodId = medRec.ServicePeriodId,
                           MedicalRecordId = medRec.MedicalRecordId
                       };            
        }       

    }
}
