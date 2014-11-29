using System;
using System.Linq;
using System.Web.UI.WebControls;
using Eclipse.PhpaLibrary.Database.PIS;
using Eclipse.PhpaLibrary.Web;

namespace PIS.Reports
{
    public partial class Trainings : PageBase
    {
        protected void dsTraining_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {            
            DateTime? dt = null;

            PISDataContext db = (PISDataContext)dsTraining.Database;
            IQueryable<Training> query = db.Trainings;

            if (dtPeriodFrom.ValueAsDate != null)
            {              
                query = query.Where(p => p.TrainingStartFrom >= dtPeriodFrom.ValueAsDate);
            }

            if (dtPeriodTo.ValueAsDate != null)
            {             
                query = query.Where(p => p.TrainingEndTo <= dtPeriodTo.ValueAsDate);
            }

            if (!string.IsNullOrEmpty(ddlTraining.Value))
            {
                query = query.Where(p => p.TrainingTypeId == Convert.ToInt32(ddlTraining.Value));
            }

            if (!string.IsNullOrEmpty(ddlCountry.Value))
            {
                query = query.Where(p => p.CountryId == Convert.ToInt32(ddlCountry.Value));
            }


            e.Result = from trn in query
                       orderby trn.ServicePeriod.Employee.FirstName
                       select new
                       {
                           TrainingId = trn.TrainingId,
                           ServicePeriodId = trn.ServicePeriodId,
                           FirstName = trn.ServicePeriod.Employee.FirstName,
                           FullName = trn.ServicePeriod.Employee.FullName,
                           Designation = trn.ServicePeriod.Designation,
                           InstituteName = trn.InstituteName,
                           InstituteAddress = trn.InstituteAddress,
                           CourseLevel = trn.CourseLevel,
                           Subject = trn.Subject,
                           CountryId = trn.CountryId,
                           CountryName = trn.Country.CountryName,
                           TrainingTypeId = trn.TrainingTypeId,
                           TrainingDescription = trn.TrainingType.TrainingDescription,
                           TrainingStartFrom = trn.TrainingStartFrom,
                           TrainingEndTo = trn.TrainingEndTo.HasValue ? trn.TrainingEndTo.Value : dt,
                           GovtApprovalNo = trn.GovtApprovalNo,
                           Remarks = trn.Remarks,
                           EmployeeId = trn.ServicePeriod.EmployeeId
                       };          
        }
    }
}
