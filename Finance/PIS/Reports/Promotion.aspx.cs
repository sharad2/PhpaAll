using System;
using System.Linq;
using System.Web.UI.WebControls;
using Eclipse.PhpaLibrary.Database.PIS;
using Eclipse.PhpaLibrary.Web;

namespace PIS.Reports
{
    public partial class Promotion : PageBase
    {
        protected void dsPromotion_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            PISDataContext db = (PISDataContext)dsPromotion.Database;
            IQueryable<Employee> query = db.Employees;

            if (dtPeriodFrom.ValueAsDate != null)
            {
                query = query.Where(p => p.ServicePeriods.Max(q => q.PromotionDate) >= dtPeriodFrom.ValueAsDate);
            }

            if (dtPeriodTo.ValueAsDate != null)
            {
                query = query.Where(p => p.ServicePeriods.Max(q => q.PromotionDate) <= dtPeriodTo.ValueAsDate);
            }

            e.Result = from emp in query
                       let contService = emp.JoiningDate.HasValue ? DateTime.Today.Subtract(emp.JoiningDate.Value).Days : 0
                       where emp.ServicePeriods.Max(p => p.PromotionDate.HasValue)
                       orderby emp.ServicePeriods.Max(p => p.PromotionDate.Value) descending
                       select new
                       {
                           FirstName = emp.FirstName,
                           FullName = emp.FullName,
                           Designation = emp.ServicePeriods.Max(p => p.Designation),
                           Grade = emp.ServicePeriods.Max(p => p.Grade),
                           JoiningDate = emp.JoiningDate,
                           PromotionDate = emp.ServicePeriods.Max(p => p.PromotionDate),
                           ServiceAfterPromotion = DateTime.Today.Subtract(emp.ServicePeriods.Max(p => p.PromotionDate.Value)).Days,
                           ContinuousService = contService,
                           Remarks = emp.ServicePeriods.Max(p => p.Remarks),
                           EmployeeId = emp.EmployeeId
                       };
        }

        int contServiceIndex = 0;
        int afterPromoIndex = 0;
        protected void gvPromotion_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            GridView gvPromotion = (GridView)sender;
            switch (e.Row.RowType)
            {
                case DataControlRowType.Header:

                    DataControlField dcfContService = (from DataControlField dcf in gvPromotion.Columns
                                                       where dcf.AccessibleHeaderText == "ContinuousService"
                                                       select dcf).SingleOrDefault();

                    contServiceIndex = gvPromotion.Columns.IndexOf(dcfContService);

                    DataControlField dcfServiceAfterPromo = (from DataControlField dcf in gvPromotion.Columns
                                                             where dcf.AccessibleHeaderText == "ServiceAfterPromotion"
                                                             select dcf).SingleOrDefault();

                    afterPromoIndex = gvPromotion.Columns.IndexOf(dcfServiceAfterPromo);
                    break;

                case DataControlRowType.DataRow:
                    float contServiceVal = 0;
                    float promoServiceVal = 0;
                    bool convert = false;

                    string contService = (from DataControlFieldCell c in e.Row.Cells
                                          where c.ContainingField.AccessibleHeaderText == "ContinuousService"
                                          select c).Single().Text;

                    convert = float.TryParse(contService, out contServiceVal);
                    if (convert)
                    {
                        contServiceVal = contServiceVal / 365;
                    }

                    e.Row.Cells[contServiceIndex].Text = string.Format("{0:N1}", contServiceVal);

                    string promoService = (from DataControlFieldCell c in e.Row.Cells
                                           where c.ContainingField.AccessibleHeaderText == "ServiceAfterPromotion"
                                           select c).Single().Text;

                    convert = float.TryParse(promoService, out promoServiceVal);
                    if (convert)
                    {
                        promoServiceVal = promoServiceVal / 365;
                    }
                    e.Row.Cells[afterPromoIndex].Text = string.Format("{0:N1}", promoServiceVal);
                    break;

                default:
                    break;
            }
        }

    }
}
