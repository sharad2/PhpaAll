using System;
using System.Linq;
using System.Web.UI.WebControls;
using Eclipse.PhpaLibrary.Database.PIS;
using Eclipse.PhpaLibrary.Reporting;
using Eclipse.PhpaLibrary.Web;
using System.Web.Security;
using EclipseLibrary.Web.UI;
using System.Web.Services;

namespace Finance.PIS
{
    public partial class Default : PageBase
    {
        #region Load Events

        private bool _canEdit;

        protected override void OnLoad(EventArgs e)
        {
            _canEdit = UrlAuthorizationModule.CheckUrlAccessForPrincipal(this.ResolveUrl(dlgAddEmployee.Ajax.Url), this.User, "GET");
            base.OnLoad(e);
        }

        protected void rep_DataBound(object sender, RepeaterItemEventArgs e)
        {
            Repeater rep = (Repeater)sender;
            switch (e.Item.ItemType)
            {
                case ListItemType.Item:
                case ListItemType.AlternatingItem:
                    MultiView mv = (MultiView)e.Item.FindControl("mv");
                    mv.ActiveViewIndex = _canEdit ? 1 : 0;
                    break;

                case ListItemType.Footer:
                    if (rep.Items.Count == 0)
                    {
                        rep.Visible = false;
                    }
                    break;
            }
        }

        protected void rep1_DataBound(object sender, RepeaterItemEventArgs e)
        {
            Repeater rep = (Repeater)sender;
            switch (e.Item.ItemType)
            {
                case ListItemType.Footer:
                    if (rep.Items.Count == 0)
                    {
                        rep.Visible = false;
                    }
                    break;
            }
        }

        #endregion

        #region Selecting

        protected void dsGrades_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            PISDataContext db = (PISDataContext)dsGrades.Database;

            e.Result = from emp in db.Employees
                       where emp.EmployeeStatusId == null
                       && emp.EmployeeTypeId != null
                       group emp by new { Division = emp.Division, EmployeeType = emp.EmployeeType } into grp
                       orderby grp.Key.Division.DivisionName, grp.Key.EmployeeType.Description
                       select new
                       {
                           DivisionName = grp.Key.Division.DivisionName,
                           DivisionId = (int?)grp.Key.Division.DivisionId,
                           EmployeeType = grp.Key.EmployeeType.Description,
                           CountForeigners = grp.Count(p => !p.IsBhutanese),
                           CountBhutanese = grp.Count(p => p.IsBhutanese),
                           CountTotal = grp.Count(),
                           EmployeeTypeId = (int?)grp.Key.EmployeeType.EmployeeTypeId
                       };
        }

        protected void dsPromotionDue_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            PISDataContext db = (PISDataContext)dsPromotionDue.Database;

            var queryPromotion = db.Employees.Select(p => new
            {
                EmployeeId = p.EmployeeId,
                FullName = p.FullName,
                NextPromotionDate = p.ServicePeriods.Max(q => q.NextPromotionDate),
                DateOfRelieve = p.DateOfRelieve
            }).Where(p => p.NextPromotionDate > DateTime.Today && p.NextPromotionDate < DateTime.Today.AddMonths(2) && p.DateOfRelieve == null)
                        .OrderBy(p => p.NextPromotionDate);
            e.Result = queryPromotion;
        }

        protected void dsRecentlyPromoted_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            PISDataContext db = (PISDataContext)dsPromotionDue.Database;

            var queryPromotion = db.Employees.Select(p => new
            {
                EmployeeId = p.EmployeeId,
                FullName = p.FullName,
                PromotionDate = p.ServicePeriods.Max(q => q.PromotionDate)
            }).Where(p => p.PromotionDate > DateTime.Today.AddMonths(-1) && p.PromotionDate < DateTime.Today.AddMonths(1))
                        .OrderBy(p => p.PromotionDate);
            e.Result = queryPromotion;
        }

        protected void dsIncrementDue_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            PISDataContext db = (PISDataContext)dsIncrementDue.Database;

            var queryPromotion = db.Employees.Select(p => new
            {
                EmployeeId = p.EmployeeId,
                FirstName = p.FirstName,
                LastName = p.LastName,
                FullName = p.FullName,
                JoiningDate = p.JoiningDate,
                DateOfNextIncrement = p.ServicePeriods.Max(q => q.DateOfNextIncrement),
                DateOfRelieve = p.DateOfRelieve
            }).Where(p => p.DateOfRelieve == null && p.JoiningDate.Value.Month == DateTime.Today.Month && p.JoiningDate.Value.Year < DateTime.Today.Year)
            //}).Where(p => p.DateOfNextIncrement == null && p.DateOfRelieve == null ||( p.DateOfNextIncrement >= DateTime.Today.AddDays(-3) 
            //    && p.DateOfNextIncrement < DateTime.Today.AddMonths(2)))
                        .OrderBy(p => p.DateOfNextIncrement ?? p.JoiningDate)
                        .ThenBy(p=> p.FirstName).ThenBy(p=>p.LastName);
            e.Result = queryPromotion;
        }

        protected void dsUpcomingBirthdays_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            PISDataContext db = (PISDataContext)dsGrades.Database;
            var bithdayQuery = from emp in db.Employees
                         where emp.DateOfBirth != null
                         let birthDay = new DateTime(DateTime.Today.Year, emp.DateOfBirth.Value.Month, emp.DateOfBirth.Value.Day)
                         where birthDay >= DateTime.Today.AddDays(-2) && birthDay <= DateTime.Today.AddDays(30) && emp.DateOfRelieve == null
                               orderby birthDay ascending
                         select emp;
            e.Result = bithdayQuery;
        }
        #endregion

        #region Helpers

        protected Unit GetWidth(object count, object total)
        {
            return new Unit(Convert.ToDouble(count) * 6 / Convert.ToDouble(total), UnitType.Em);
        }

        #endregion

    }
}
