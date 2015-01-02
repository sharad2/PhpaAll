/*
 *  Programming: Eclipse Systems (P) Ltd., NOIDA, INDIA
 *  E-mail: support@eclsys.com
 *
 *  $Workfile:   EmployeeAdjustmentEditor.ascx.cs  $
 *  $Revision: 37671 $
 *  $Author: ssingh $
 *  $Date: 2010-11-23 15:14:17 +0530 (Tue, 23 Nov 2010) $
 *
 */
using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Eclipse.PhpaLibrary.Database.Payroll;
using EclipseLibrary.Web.JQuery;
using EclipseLibrary.Web.JQuery.Input;
using System.Data.Linq;
using Finance.Payroll;

namespace Finance.Controls
{
    /// <summary>
    /// Set the EmployeeId to enable updating/deleting of that employee's adjustments.
    /// When the user successfully changes to the database, ItemChanged event is raised. You can handle this
    /// event to update your UI.
    /// </summary>
    public partial class EmployeeAdjustmentEditor : UserControl
    {

        /// <summary>
        /// Raised when user makes any update the to employee being displayed
        /// </summary>
        public event EventHandler<EventArgs> ItemChanged;

        private void OnItemChanged()
        {
            if (ItemChanged != null)
            {
                ItemChanged(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// The employee for whom information is being displayed. If this is null, we become almost invisible.
        /// </summary>
        public int? EmployeeId
        {
            get
            {
                string str = this.dsEditEmpAdjustments.WhereParameters["EmployeeId"].DefaultValue;
                if (string.IsNullOrEmpty(str))
                {
                    return null;
                }
                else
                {
                    return int.Parse(str);
                }
            }
        }

        public void SetCurrentEmployee(int? employeeId, string employeeName)
        {
            if (employeeId == null)
            {
                this.dsEditEmpAdjustments.WhereParameters["EmployeeId"].DefaultValue = string.Empty;
                hdemployeeid.Value = string.Empty;
            }
            else
            {
                this.dsEditEmpAdjustments.WhereParameters["EmployeeId"].DefaultValue = employeeId.ToString();
                hdemployeeid.Value = employeeId.ToString();
            }
            this.EmployeeName = employeeName;
            gvEditEmpAdjustments.InsertRowsCount = 0;
        }
 

        protected override void OnPreRender(EventArgs e)
        {
            btnNew.Visible = this.EmployeeId != null && gvEditEmpAdjustments.InsertRowsCount == 0;
            base.OnPreRender(e);
        }

        /// <summary>
        /// Do not query when EmployeeId is null.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dsEditEmpAdjustments_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            if (e.WhereParameters["EmployeeId"] == null)
            {
                e.Cancel = true;
            }
        }


        protected void tbFractionOfBasic_DataBinding(object sender, EventArgs e)
        {
            TextBoxEx tb = (TextBoxEx)sender;
            if (!string.IsNullOrEmpty(tb.Text))
            {
                decimal? val = Convert.ToDecimal(tb.Text);
                tb.Text = string.Format("{0}", val * 100);
            }
        }

        protected void tbFractionOfGross_DataBinding(object sender, EventArgs e)
        {
            TextBoxEx tb = (TextBoxEx)sender;
            if (!string.IsNullOrEmpty(tb.Text))
            {
                decimal? val = Convert.ToDecimal(tb.Text);
                tb.Text = string.Format("{0}", val * 100);
            }
        }
        
        /// <summary>
        /// Make sure the Grid displays the updated Employee Adjustments.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvEditEmpAdjustments_RowUpdated(object sender, GridViewUpdatedEventArgs e)
        {
            if (e.Exception == null)
            {
                OnItemChanged();
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvEditEmpAdjustments_RowInserted(object sender, GridViewInsertedEventArgs e)
        {
            if (e.Exception == null)
            {
                OnItemChanged();
            }
        }

        /// <summary>
        /// Specify Data while Inserting, if user wants to use default value
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvEditEmpAdjustments_RowInserting(object sender, GridViewInsertingEventArgs e)
        {
            try
            {
                if (!btnNew.IsPageValid())
                {
                    return;
                }

                PayrollDataContext db = (PayrollDataContext)this.dsEditEmpAdjustments.Database;

                int adjustmentId = Convert.ToInt32(e.Values["AdjustmentId"]);
                e.Values["EmployeeId"] = this.EmployeeId;

                var x = e.Values["FractionOfBasic"];
                if (x == null)
                {
                    x = (from empadj in db.EmployeeAdjustments
                                                   where empadj.EmployeeAdjustmentId == adjustmentId
                                                   select empadj.Adjustment.FractionOfBasic).FirstOrDefault();
                    if (x != null)
                    {
                        e.Values["FractionOfBasic"] = x;
                    }
                }
                else
                {
                    decimal pct = Convert.ToDecimal(x) / 100;
                    e.Values["FractionOfBasic"] = pct;
                }

                var flatAmt = e.Values["FlatAmount"];
                if (flatAmt == null)
                {
                    flatAmt = (from empadj in db.EmployeeAdjustments
                                              where empadj.EmployeeAdjustmentId == adjustmentId
                                              select empadj.Adjustment.FlatAmount).FirstOrDefault();
                    if(flatAmt!= null)
                    {
                        e.Values["FlatAmount"] = flatAmt;
                    }
                }

            }
            catch(Exception ex)
            {
                GridViewExInsert gv = (GridViewExInsert)sender;
                EclipseLibrary.Web.JQuery.Input.ValidationSummary valSummary = (EclipseLibrary.Web.JQuery.Input.ValidationSummary)gv.NamingContainer.FindControl("valSummary");
                valSummary.ErrorMessages.Add(ex.Message);
            }
        }
       
        /// <summary>
        /// Hadle the override flags. If the revert to default flag has been checkd, recalculate the flat amount or % basic
        /// and store. If user modifies flat amout of %, set the corresponding override flag.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvEditEmpAdjustments_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            var x = e.NewValues["FractionOfBasic"];
            if (x == null)
            {
                // User wants default
                PayrollDataContext dbFractionOfBasic = (PayrollDataContext)this.dsEditEmpAdjustments.Database;
                e.NewValues["FractionOfBasic"] = (from empadj in dbFractionOfBasic.EmployeeAdjustments
                            where empadj.EmployeeAdjustmentId == Convert.ToInt32(e.Keys["EmployeeAdjustmentId"])
                            select empadj.Adjustment.FractionOfBasic).Single();
            }
            else
            {
                // User has specified a value
                decimal pct = Convert.ToDecimal(x) / 100;
                e.NewValues["FractionOfBasic"] = pct;
            }


            var y = e.NewValues["FractionOfGross"];
            if (y == null)
            {
                // User wants default
                PayrollDataContext dbFractionOfGross = (PayrollDataContext)this.dsEditEmpAdjustments.Database;
                e.NewValues["FractionOfGross"] = (from empadj in dbFractionOfGross.EmployeeAdjustments
                                                  where empadj.EmployeeAdjustmentId == Convert.ToInt32(e.Keys["EmployeeAdjustmentId"])
                                                  select empadj.Adjustment.FractionOfGross).Single();
            }
            else
            {
                // User has specified a value
                decimal pct = Convert.ToDecimal(y) / 100;
                e.NewValues["FractionOfGross"] = pct;
            }

            var flatAmt = e.NewValues["FlatAmount"];
            if (flatAmt == null)
            {
                // User wants default other wise specified value will be stored
                PayrollDataContext db = (PayrollDataContext)this.dsEditEmpAdjustments.Database;
                e.NewValues["FlatAmount"] = (from empadj in db.EmployeeAdjustments
                                             where empadj.EmployeeAdjustmentId == Convert.ToInt32(e.Keys["EmployeeAdjustmentId"])
                                             select empadj.Adjustment.FlatAmount).Single();

            }
        }

        /// <summary>
        /// Raise ItemChanged
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvEditEmpAdjustments_RowDeleted(object sender, GridViewDeletedEventArgs e)
        {
            if (e.Exception == null)
            {
                OnItemChanged();
            }
        }

        /// <summary>
        /// Event will make the textbox readonly as per requirement
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvEditEmpAdjustments_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            switch (e.Row.RowState)
            {
                case DataControlRowState.Insert:
                    TextBoxEx tbFlatAmount = (TextBoxEx)e.Row.FindControl("tbFlatAmount");
                    TextBoxEx tbFractionOfBasic = (TextBoxEx)e.Row.FindControl("tbFractionOfBasic");
                    tbFlatAmount.ReadOnly = true;
                    tbFractionOfBasic.ReadOnly = true;
                    break;
                
            }
        }
       
        /// <summary>
        /// Name of the employee for whom we are inserting the first adjustment
        /// </summary>
        private string EmployeeName
        {
            get
            {
                return (string)ViewState["EmployeeName"];
            }
            set
            {
                ViewState["EmployeeName"] = value;
            }
        }

        /// <summary>
        /// Prepares the form view to insert adjustments for the passed employee
        /// </summary>
        /// <param name="employeeId"></param>
        //public void InsertAdjustments(int employeeId, string name)
        //{
        //    SetCurrentEmployee(employeeId, name);
        //    //fvInsertEmpAdj.ChangeMode(FormViewMode.Insert);
        //}


        protected void tb_ServerValidate(object sender, EclipseLibrary.Web.JQuery.Input.ServerValidateEventArgs e)
        {
            AutoComplete tb = (AutoComplete)e.ControlToValidate;
            e.ControlToValidate.IsValid = true;
            if ((tb.Value == "" && tb.DisplayValue == "") || (tb.Value != "" && tb.DisplayValue != ""))
            {
                return;
            }

            e.ControlToValidate.IsValid = false;
            e.ControlToValidate.ErrorMessage = "Invalid Data in " + tb.FriendlyName + "  Field";
        }


        protected void btnNew_Click(object sender, EventArgs e)
        {
            gvEditEmpAdjustments.InsertRowsCount = 1;
        }

        /// <summary>
        /// Optimization. Load adjustment along with each employee adjustment
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dsEditEmpAdjustments_ContextCreated(object sender, LinqDataSourceStatusEventArgs e)
        {
            var db = (PayrollDataContext)e.Result;
            DataLoadOptions lo = new DataLoadOptions();
            lo.AssociateWith<EmployeePeriod>(ep => ep.PeriodEmployeeAdjustments.Where(pea => pea.Amount != null));
            lo.LoadWith<EmployeeAdjustment>(ep => ep.Adjustment);
            db.LoadOptions = lo;
        }
    }
}