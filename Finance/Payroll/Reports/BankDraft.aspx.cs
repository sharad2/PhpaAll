using System.Linq;
using System.Web.UI.WebControls;
using Eclipse.PhpaLibrary.Database.Payroll;
using Eclipse.PhpaLibrary.Web;


namespace Finance.Payroll.Reports
{
    public partial class BankDraft : PageBase
    {
        protected void ds_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            int selectedType;
            if (string.IsNullOrEmpty(ddlEmpType.Value))
            {
                selectedType = 0;
            }
            else
            {
                selectedType = int.Parse(ddlEmpType.Value);
            }
            PayrollDataContext db = (PayrollDataContext)ds.Database;
            var query = from pea in db.PeriodEmployeeAdjustments
                        where (selectedType == 0 || pea.EmployeePeriod.Employee.EmployeeType.EmployeeTypeId == selectedType) &&
                              pea.EmployeePeriod.SalaryPeriod.SalaryPeriodStart >= tbFromDate.ValueAsDate &&
                              pea.EmployeePeriod.SalaryPeriod.SalaryPeriodEnd <= tbToDate.ValueAsDate &&
                              pea.EmployeePeriod.Employee.ParentOrganization != null && pea.EmployeePeriod.Employee.ParentOrganization != "" &&
                              (from rptCat in db.ReportCategories
                              where rptCat.ReportId == 103
                              orderby rptCat.AdjustmentCategory.IsDeduction
                              select rptCat.AdjustmentCategory).Any(p => p.AdjustmentCategoryId == pea.Adjustment.AdjustmentCategoryId)
                        group pea by new
                        {
                            ParentOrganization = pea.EmployeePeriod.Employee.ParentOrganization,
                            AdjustmentCategory = pea.Adjustment.AdjustmentCategory
                        } into grp
                        orderby grp.Key.ParentOrganization, grp.Key.AdjustmentCategory.AdjustmentCategoryCode
                        select new 
                        {
                            ParentOrganization = grp.Key.ParentOrganization,
                            AdjustmentCategoryCode = grp.Key.AdjustmentCategory.ShortDescription,
                            Amount = grp.Sum(p => p.Amount ?? 0)
                        };
            e.Result = query;

        }
    }
}


