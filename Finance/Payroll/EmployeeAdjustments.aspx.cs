/*
 *  Programming: Eclipse Systems (P) Ltd., NOIDA, INDIA
 *  E-mail: support@eclsys.com
 *
 *  $Workfile:   EmployeeAdjustments.aspx.cs  $
 *  $Revision: 38609 $
 *  $Author: skumar $
 *  $Date: 2010-12-07 10:38:52 +0530 (Tue, 07 Dec 2010) $
 *  $Id: EmployeeAdjustments.aspx.cs 38609 2010-12-07 05:08:52Z skumar $
 * 
 */

using Eclipse.PhpaLibrary.Database.Payroll;
using Eclipse.PhpaLibrary.Web;
using EclipseLibrary.Web.JQuery.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Finance.Payroll
{
    public partial class EmployeeAdjustments : PageBase
    {

        protected void dsDivision_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            PayrollDataContext db = (PayrollDataContext)dsDivision.Database;
            var query = (from d in db.Divisions
                         orderby d.DivisionName
                         select new
                         {
                             Division = d,
                             EmployeeCount = d.Employees.Count(emp => emp.BasicSalary != null && !emp.EmployeeAdjustments.Any() &&
                             db.Adjustments.Any(
                                 adj => adj.IsDefault && (adj.EmployeeTypeId == null ||
                                     adj.EmployeeTypeId == emp.EmployeeTypeId)))
                         }
                            ).Where(q => q.EmployeeCount > 0).Take(20);

            e.Result = (from q in query
                        select new
                        {
                            Description = string.Format("{0} ({1} employees)", q.Division.DivisionName, q.EmployeeCount),
                            //Description = string.Format("{0}: {1} ({2} employees)", q.Division.DivisionId, q.Division.DivisionName, q.EmployeeCount),
                            DivisionId = q.Division.DivisionId.ToString()
                        }).Take(20).ToArray();
        }


        protected override void OnInit(EventArgs e)
        {
            ctlEditor.ItemChanged += new EventHandler<EventArgs>(ctlEditor_ItemChanged);
            base.OnInit(e);
        }

        void ctlEditor_ItemChanged(object sender, EventArgs e)
        {
            gvEmployeeAdjustments.DataBind();
        }

        /// <summary>
        /// Display employees whose adjustments have been created.
        /// Ritesh 13 Jan 2012
        /// Station  property of employee profile is matched against the station belonging to logged in user.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dsEmployeeAdjustments_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            PayrollDataContext db = (PayrollDataContext)this.dsEmployeeAdjustments.Database;
            var stations = this.GetUserStations();
            //if (Session["station"] == null)
            //{
            //    e.Cancel = true;
            //    FormsAuthentication.RedirectToLoginPage();
            //    return;
            //}
            //List<string> stations = new List<string>(Session["station"].ToString().Split(','));
            if (!string.IsNullOrEmpty(tbEmployee.Value))
            {
                e.WhereParameters["EmployeeId"] = Convert.ToInt32(tbEmployee.Value);
                dsEmployeeAdjustments.Where = "EmployeeId == @EmployeeId";
            }

            var query = from empad in db.EmployeeAdjustments
                        //where ((Session["Roles"].ToString() != "Administrator") ? stations.Contains(empad.Employee.Station.StationName) : stations.Any())
                        group empad by empad.Employee into grouping
                        orderby grouping.Key.FirstName ascending
                        let allowance = grouping.Sum(p => !p.Adjustment.IsDeduction ? (p.FlatAmount ?? 0) + (Convert.ToDecimal(p.FractionOfBasic ?? 0) * (grouping.Key.BasicSalary ?? 0)) : 0)
                        let basic = grouping.Key.BasicSalary
                        let gross = allowance + basic
                        let deduction = grouping.Sum(p => p.Adjustment.IsDeduction ?
                                (p.FlatAmount ?? 0) +
                                (Convert.ToDecimal(p.FractionOfBasic ?? 0) * (grouping.Key.BasicSalary ?? 0)) +
                                (Convert.ToDecimal(p.FractionOfGross ?? 0)) * gross
                                : 0)
                        select new
                        {
                            DivisionName = grouping.Key.Division.DivisionName,
                            DivisionCode = grouping.Key.Division.DivisionCode,
                            EmployeeAdjustmentId = grouping.Min(p => p.EmployeeAdjustmentId),
                            EmployeeId = grouping.Key.EmployeeId,
                            EmployeeCode = grouping.Key.EmployeeCode,
                            EmployeeName = grouping.Key.FullName,
                            Designation = grouping.Key.Designation,
                            Grade = grouping.Key.ServicePeriods.OrderByDescending(p => p.PeriodStartDate).Take(1).Max(p => p.Grade),
                            Basic = basic,
                            Deduction = deduction,
                            Allowance = allowance,
                            GrossSalary = gross,
                            NetPay = gross - deduction,
                            StationId = grouping.Key.StationId,
                            StationName = grouping.Key.Station.StationName
                        };

            /*
             
             select new
                       {
                           DivisionName = grouping.Key.Division.DivisionName,
                           DivisionCode = grouping.Key.Division.DivisionCode,
                           EmployeeAdjustmentId = grouping.Min(p => p.EmployeeAdjustmentId),
                           EmployeeId = grouping.Key.EmployeeId,
                           EmployeeCode = grouping.Key.EmployeeCode,
                           EmployeeName = grouping.Key.FullName,
                           Designation = grouping.Key.Designation,
                           Grade = grouping.Key.ServicePeriods.OrderByDescending(p => p.PeriodStartDate).Take(1).Max(p => p.Grade), 
                           Basic = grouping.Key.BasicSalary,
                           Deduction = grouping.Sum(p => p.Adjustment.IsDeduction ? (p.FlatAmount ?? 0) + (Convert.ToDecimal(p.FractionOfBasic ?? 0) * (grouping.Key.BasicSalary ?? 0)) : 0),
                           Allowance = grouping.Sum(p => !p.Adjustment.IsDeduction ? (p.FlatAmount ?? 0) + (Convert.ToDecimal(p.FractionOfBasic ?? 0) * (grouping.Key.BasicSalary ?? 0)) : 0),
                           GrossSalary = (grouping.Key.BasicSalary ?? 0) + (grouping.Sum(p => !p.Adjustment.IsDeduction ? (p.FlatAmount ?? 0) + (Convert.ToDecimal(p.FractionOfBasic ?? 0) * (grouping.Key.BasicSalary ?? 0)) : 0)),
                           NetPay = ((grouping.Key.BasicSalary ?? 0) + (grouping.Sum(p => !p.Adjustment.IsDeduction ? (p.FlatAmount ?? 0) + (Convert.ToDecimal(p.FractionOfBasic ?? 0) * (grouping.Key.BasicSalary ?? 0)) : 0)))
                           - (grouping.Sum(p => p.Adjustment.IsDeduction ? (p.FlatAmount ?? 0) + (Convert.ToDecimal(p.FractionOfBasic ?? 0) * (grouping.Key.BasicSalary ?? 0)) : 0)),
                           StationId=grouping.Key.StationId,
                           StationName=grouping.Key.Station.StationName
                       };
             */

            if (stations != null)
            {
                query = query.Where(p => stations.Contains(p.StationId));
            }
            e.Result = query;
        }

        protected void gvEmployeeAdjustments_SelectedIndexChanged(object sender, EventArgs e)
        {
            ctlEditor.SetCurrentEmployee((int)gvEmployeeAdjustments.SelectedDataKey["EmployeeId"],
             (string)gvEmployeeAdjustments.SelectedDataKey["EmployeeName"]);
            dlgEditor.Title = string.Format("Adjustments for {0}", gvEmployeeAdjustments.SelectedDataKey["EmployeeName"]);
            dlgEditor.Visible = true;
        }

        /// <summary>
        /// Make the Hybrid Drag Panel always visible.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            ButtonEx btn = (ButtonEx)sender;
            AutoComplete tbEmployee = (AutoComplete)btn.NamingContainer.FindControl("tbEmployee");
            if (!tbEmployee.IsValid)
            {
                return;
            }
            gvEmployeeAdjustments.PageIndex = 0;
            dlgEditor.Visible = false;
        }

        /// <summary>
        /// Insert Employee when we are looking for an employee through Employee selector.
        /// Create Default payroll template for an employee.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        protected void btnNewEmployee_Click(object sender, EventArgs e)
        {
            PayrollDataContext dbPayroll = (PayrollDataContext)dsEmployeeAdjustments.Database;
            int nId = System.Convert.ToInt32(tbEmployee.Value);
            // All employees of selected division whose basic pay is defined will be selected
            // Employees for whom adjustments already exist are excluded
            var employee = (from emp in dbPayroll.Employees
                            where emp.EmployeeId == nId
                            select new
                            {
                                Employee = emp,
                                EmployeeTypeDescription = emp.EmployeeType.Description,
                                Adjustments = from adj in dbPayroll.Adjustments
                                              where adj.IsDefault &&
                                              (adj.EmployeeTypeId == null || adj.EmployeeTypeId == emp.EmployeeTypeId)
                                              select adj
                            }).Single();

            m_addedEmployees = new List<int>();
            m_addedEmployees.Add(employee.Employee.EmployeeId);
            int nCountAdjustments = 0;
            foreach (Adjustment adj in employee.Adjustments)
            {
                ++nCountAdjustments;
                EmployeeAdjustment empAdj = new EmployeeAdjustment(adj);
                employee.Employee.EmployeeAdjustments.Add(empAdj);
            }


            dbPayroll.SubmitChanges();
            if (nCountAdjustments == 0)
            {
                if (string.IsNullOrEmpty(employee.EmployeeTypeDescription))
                {
                    this.lblCountAdded.Text = string.Format(@"Since employee type has not been specified for {0}, you must add adjustments manually.",
                        employee.Employee.FullName);
                }
                else
                {
                    this.lblCountAdded.Text = string.Format(@"No default adjustments have been defined for employees of type {0}. You can add adjustments manually.",
                        employee.EmployeeTypeDescription);
                }
            }
            else
            {
                this.lblCountAdded.Text = string.Format("{0} adjustements added for {1}", nCountAdjustments,
                    employee.Employee.FullName);
                gvEmployeeAdjustments.DataBind();
            }
            this.lblCountAdded.Visible = true;
            //HybridDragVisiblePanel1.MakeAlwaysVisible(true);
            ctlEditor.SetCurrentEmployee(nId, employee.Employee.FullName);
        }

        /// <summary>
        /// Contains the id of the employees which have just been added.
        /// Insert employee when we are looking employees deivision wise.
        /// Do bulk insert.
        /// </summary>
        private List<int> m_addedEmployees;
        protected void btnBulk_Click(object sender, EventArgs e)
        {
            ButtonEx btn = (ButtonEx)sender;
            DropDownListEx ddlDivision = (DropDownListEx)btn.NamingContainer.Parent.FindControl("ddlDivision");
            if (string.IsNullOrEmpty(ddlDivision.Value))
            {
                return;
            }

            PayrollDataContext dbPayroll = (PayrollDataContext)dsEmployeeAdjustments.Database;

            // All employees of selected division whose basic pay is defined will be selected
            // Employees for whom adjustments already exist are excluded
            var employeeInfo = (from emp in dbPayroll.Employees
                                where emp.BasicSalary != null &&
                                emp.DivisionId == Convert.ToInt32(ddlDivision.Value) &&
                                !emp.EmployeeAdjustments.Any()
                                select new
                                {
                                    Employee = emp,
                                    Adjustments = from adj in dbPayroll.Adjustments
                                                  where adj.IsDefault &&
                                                  (adj.EmployeeTypeId == null || adj.EmployeeTypeId == emp.EmployeeTypeId)
                                                  select adj
                                }).Where(info => info.Adjustments.Any());

            m_addedEmployees = new List<int>();
            foreach (var employee in employeeInfo)
            {
                m_addedEmployees.Add(employee.Employee.EmployeeId);
                foreach (Adjustment adj in employee.Adjustments)
                {
                    EmployeeAdjustment empAdj = new EmployeeAdjustment(adj);
                    employee.Employee.EmployeeAdjustments.Add(empAdj);
                }
            }

            dbPayroll.SubmitChanges();
            this.lblCountAdded.Text = string.Format("{0} employees added", m_addedEmployees.Count);
            this.lblCountAdded.Visible = true;
        }

        /// <summary>
        /// Display New! image for newly added employees.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvEmployeeAdjustments_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            switch (e.Row.RowType)
            {
                case DataControlRowType.DataRow:
                    Control imgNew = e.Row.FindControl("imgNew");
                    if (m_addedEmployees == null)
                    {
                        imgNew.Visible = false;
                    }
                    else
                    {
                        imgNew.Visible = m_addedEmployees.Contains((int)gvEmployeeAdjustments.DataKeys[e.Row.RowIndex].Value);
                    }
                    break;
            }
        }


        /// <summary>
        /// Delete Employee from Employee Adjustment along with all its adjustments.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvEmployeeAdjustments_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            PayrollDataContext db = (PayrollDataContext)this.dsEmployeeAdjustments.Database;
            var empId = Convert.ToInt32(e.Keys["EmployeeId"]);
            var query = from empadj in db.EmployeeAdjustments
                        where empadj.EmployeeId == empId
                        select empadj;


            foreach (EmployeeAdjustment empadj in query)
            {
                db.EmployeeAdjustments.DeleteOnSubmit(empadj);
            }
            db.SubmitChanges();
            gvEmployeeAdjustments.DataBind();
            ctlEditor.SetCurrentEmployee(null, string.Empty);
            e.Cancel = true;
        }
    }
}
