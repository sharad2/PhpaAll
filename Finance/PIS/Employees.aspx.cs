/*
 *  Programming: Eclipse Systems (P) Ltd., NOIDA, INDIA
 *  E-mail: support@eclsys.com
 *
 *  $Workfile:   Employees.aspx.cs  $
 *  $Revision: 36560 $
 *  $Author: ssingh $
 *  $Date: 2010-10-25 13:50:25 +0530 (Mon, 25 Oct 2010) $
 *  $Modtime:   Jul 29 2008 11:44:52  $
 *
 *  $Log:   S:/Projects/PHPA2/archives/Finance/Payroll/Employees.aspx.cs-arc  $
 */
using System;
using System.Data.Linq;
using System.Linq;
using System.Web.UI.WebControls;
using Eclipse.PhpaLibrary.Web;
using Eclipse.PhpaLibrary.Database.PIS;
using System.Web.Security;

namespace PhpaAll.Finance
{
    /// <summary>
    /// After a new employee is inserted, the page is posted back with employee id 
    /// in a hidden field. If this hidden field contains a value, all filters are
    /// cleared and just this id is used to select the employee.
    /// If the user searches an employee from the SideNavigationPane the querystring Emp
    /// is passed
    /// </summary>
    /// <remarks>
    /// Quer strings: IsBhutanese, ActiveOnDate,Emp
    /// </remarks>   
    public partial class ManageEmployee : PageBase
    {

        protected void dsDesignation_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            PISDataContext db = (PISDataContext)dsDesignation.Database;
            e.Result = (from emp in db.Employees
                        where emp.Designation != null
                        orderby emp.Designation
                        select emp.Designation).Distinct();
        }

        /// <summary>
        /// Creating where clause list for the data-source of GridView.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dsEmployee_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            PISDataContext db = (PISDataContext)dsEmployee.Database;
            DataLoadOptions dlo = new DataLoadOptions();
            dlo.LoadWith<Employee>(p => p.Division);
            dlo.LoadWith<Employee>(p => p.EmployeeType);
            db.LoadOptions = dlo;

            IQueryable<Employee> query = db.Employees;

            if (!string.IsNullOrEmpty(tbEmployee.Text))
            {
                if (tbEmployee.Text.Split(' ').ToArray().Length > 1)
                {
                    query = query.Where(p => p.EmployeeCode.Contains(tbEmployee.Text) ||
                   p.FirstName.Contains(tbEmployee.Text.Split(' ').First()) &&
                   p.LastName.Contains(tbEmployee.Text.Split(' ').Last()));
                }
                else
                {
                    query = query.Where(p => p.EmployeeCode.Contains(tbEmployee.Text) ||
                        p.FirstName.Contains(tbEmployee.Text) ||
                        p.LastName.Contains(tbEmployee.Text));
                }
            }
            if (!string.IsNullOrEmpty(tbFileindexno.Text))
            {
                query = query.Where(p => p.FileindexNo == tbFileindexno.Text);
            }

            if (!string.IsNullOrEmpty(ddlDesignation.Value))
            {
                query = query.Where(p => p.Designation.Contains(ddlDesignation.Value));
            }

            if (!string.IsNullOrEmpty(ddlEmployeeType.Value))
            {
                query = query.Where(p => p.EmployeeTypeId == Convert.ToInt32(ddlEmployeeType.Value));
            }

            if (!string.IsNullOrEmpty(ddlDivision.Value))
            {
                query = query.Where(p => p.DivisionId == Convert.ToInt32(ddlDivision.Value));
            }

            if (!string.IsNullOrEmpty(ddlEmployeeStatus.Value))
            {
                query = query.Where(p => p.EmployeeStatusId == Convert.ToInt32(ddlEmployeeStatus.Value));
            }
            else
            {
                query = query.Where(p => p.EmployeeStatusId == null);
            }

            DateTime? activeOnDate = dtActiveOn.ValueAsDate;
            //Not considering Service period
            //if (activeOnDate != null)
            //{
            //    query = query.Where(emp => !emp.ServicePeriods.Any() ||
            //            (emp.ServicePeriods.FirstOrDefault(p => p.PeriodStartDate.Date <= activeOnDate &&
            //                (p.PeriodEndDate == null || p.PeriodEndDate.Value.Date >= activeOnDate)) != null));
            //}

            if (!string.IsNullOrEmpty(rblNationality.Value))
            {
                bool? b;
                if (rblNationality.Value == "0")
                {
                    b = false;
                }
                else if (rblNationality.Value == "1")
                {
                    b = true;
                }
                else
                {
                    // Garbage value
                    b = null;
                }
                if (b.HasValue)
                {
                    query = query.Where(p => p.IsBhutanese == b);
                }
            }

            if (cbNoServicePeriod.Checked)
            {
                query = query.Where(p => !p.ServicePeriods.Any());
            }

            if (cbToJoinEmployees.Checked)
            {
                query = query.Where(p => (p.JoiningDate > DateTime.Now
                    && p.JoiningDate < DateTime.Now.AddDays(30)));
            }

            if (cbToTerminate.Checked)
            {
                query = query.Where(p => (p.DateOfRelieve > DateTime.Now
                    && p.DateOfRelieve < DateTime.Now.AddDays(30)));
            }

            if (cbNewlyJoined.Checked)
            {
                query = query.Where(p => (p.JoiningDate > DateTime.Now.AddDays(-30)
                    && p.JoiningDate < DateTime.Now));
            }

            if (cbNewlyTerminated.Checked)
            {
                query = query.Where(p => (p.DateOfRelieve > DateTime.Now.AddDays(-30)
                    && p.DateOfRelieve < DateTime.Now));
            }

            if (cbNewlyPromoted.Checked)
            {
                // Find Employees whose ServicePeriod exists and whose PromotionDate in ServicePeriod
                // is not null and PromotionDate is in between last 30 days
                //PromotionDate is in between last 30 days

                query = (from emp in db.Employees
                         join sp in db.ServicePeriods
                           on emp.EmployeeId equals sp.EmployeeId
                         where sp.PromotionDate != null
                         && (sp.PromotionDate > DateTime.Now.AddDays(-30)
                         && sp.PromotionDate < DateTime.Now)
                         select emp).Distinct();

            }
            if (cbCompleteprobation.Checked)
            {
                query = query.Where(p => p.ProbationEndDate != null &&
                      p.ProbationEndDate > DateTime.Now && p.ProbationEndDate < DateTime.Now.AddDays(30));

            }
            if (cbPromotiondue.Checked)
            {
                query = (from emp in db.Employees
                         join sp in db.ServicePeriods
                         on emp.EmployeeId equals sp.EmployeeId
                         where sp.NextPromotionDate != null
                         && (sp.NextPromotionDate > DateTime.Now
                         && sp.NextPromotionDate < DateTime.Now.AddDays(30))
                         select emp).Distinct();
            }
            //if (cbIncrementdue.Checked)
            //{
            //    query = (from emp in db.Employees
            //             join sp in db.ServicePeriods
            //             on emp.EmployeeId equals sp.EmployeeId
            //             where sp.DateOfNextIncrement != null
            //             && (sp.DateOfNextIncrement > DateTime.Now
            //             && sp.DateOfNextIncrement < DateTime.Now.AddDays(30))
            //             select emp).Distinct();
            //}
            e.Result = query;


        }

        private bool _canEdit;
        protected void gvEmployee_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            switch (e.Row.RowType)
            {
                case DataControlRowType.Header:
                    _canEdit = UrlAuthorizationModule.CheckUrlAccessForPrincipal(this.ResolveUrl("EmployeeDetails.aspx"),
                        this.User, "GET");
                    break;

                case DataControlRowType.DataRow:
                    MultiView mv = (MultiView)e.Row.FindControl("mv");
                    mv.ActiveViewIndex = _canEdit ? 1 : 0;
                    break;
            }
        }
    }
}
