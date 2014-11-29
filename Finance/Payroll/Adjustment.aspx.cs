﻿/*
 *  Programming: Eclipse Systems (P) Ltd., NOIDA, INDIA
 *  E-mail: support@eclsys.com
 *
 *  $Workfile:   Adjustment.aspx.cs  $
 *  $Revision: 38553 $
 *  $Author: ssingh $
 *  $Date: 2010-12-06 11:49:23 +0530 (Mon, 06 Dec 2010) $
 *  $Modtime:   Jul 24 2008 09:43:34  $
 *
 *  $Log:   S:/Projects/PHPA2/archives/Finance/Payroll/Adjustment.aspx.cs-arc  $
 *  
 */
using System;
using System.Data.Linq;
using System.Web.UI.WebControls;
using Eclipse.PhpaLibrary.Database.Payroll;
using Eclipse.PhpaLibrary.Web;
using EclipseLibrary.Web.JQuery.Input;

namespace Finance.Payroll
{
    public partial class ManageAdjustments : PageBase
    {
        private int _adjustmentID;

        protected override void OnInit(EventArgs e)
        {
            dsAdjustment.ContextCreated += new EventHandler<LinqDataSourceStatusEventArgs>(dsAdjustment_ContextCreated);
            base.OnInit(e);
        }

        private bool _loadOptionsSet;
        void dsAdjustment_ContextCreated(object sender, LinqDataSourceStatusEventArgs e)
        {
            if (!_loadOptionsSet)
            {
                // Sharad Lobeysa: Workaround because this event gets raised multiple times
                PayrollDataContext db = (PayrollDataContext)e.Result;
                DataLoadOptions dlo = new DataLoadOptions();
                dlo.LoadWith<Adjustment>(p => p.AdjustmentCategory);
                dlo.LoadWith<Adjustment>(p => p.EmployeeType);
                db.LoadOptions = dlo;
                _loadOptionsSet = true;
            }
        }
        /// <summary>
        /// Specifying formview to do nothing if, select parameter is null.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        protected void dsSpecificAdjustment_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            if (e.WhereParameters["AdjustmentId"] == null)
            {
                e.Cancel = true;
            }
        }


        # region InsertUpdate&Delete

        /// <summary>
        /// After insertion grid is getting refreshed and button caption of cancel is changed to close.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void frmAdjustment_ItemInserted(object sender, FormViewInsertedEventArgs e)
        {
            if (e.Exception == null)
            {
                gvAdjustments.DataBind();
                frmAdjustment.DataBind();
            }
            else
            {
                //FormView fv = (FormView)sender;
                //EclipseLibrary.Web.JQuery.Input.ValidationSummary valSummary = (EclipseLibrary.Web.JQuery.Input.ValidationSummary)fv.FindControl("valSummary");
                //valSummary.ErrorMessages.Add(e.Exception.Message);
                //e.ExceptionHandled = true;
            }
        }

        /// <summary>
        /// Selecting recently inserted data in grid.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dsSpecificAdjustment_Inserted(object sender, LinqDataSourceStatusEventArgs e)
        {
            if (e.Exception == null)
            {
                Adjustment adjObject = (Adjustment)e.Result;
                _adjustmentID = adjObject.AdjustmentId;
                dsSpecificAdjustment.WhereParameters["AdjustmentId"].DefaultValue = adjObject.AdjustmentId.ToString();
                gvAdjustments.DataBind();
            }
        }

        /// <summary>
        /// Binding GridView after updation to fill refreshed data.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void frmAdjustment_ItemUpdated(object sender, FormViewUpdatedEventArgs e)
        {
            if (e.Exception == null)
            {
                frmAdjustment.ChangeMode(FormViewMode.ReadOnly);
                gvAdjustments.DataBind();
            }
        }

        protected void dsSpecificAdjustment_Updating(object sender, LinqDataSourceUpdateEventArgs e)
        {
            if (e.Exception == null)
            {
                Adjustment adj = (Adjustment)e.NewObject;
                TextBoxEx tbFractionOfBasic = (TextBoxEx)frmAdjustment.FindControl("tbPercentageBasic");
                if (!string.IsNullOrEmpty(tbFractionOfBasic.Text))
                {
                    double? fractionBasic = Convert.ToDouble(tbFractionOfBasic.Value);
                    adj.FractionOfBasic = fractionBasic / 100;
                }
                else
                {
                    adj.FractionOfBasic = null;
                }
            }
        }

        protected void dsSpecificAdjustment_Inserting(object sender, LinqDataSourceInsertEventArgs e)
        {
            if (e.Exception == null)
            {
                Adjustment adj = (Adjustment)e.NewObject;
                TextBoxEx tbFractionOfBasic = (TextBoxEx)frmAdjustment.FindControl("tbPercentageBasic");
                if (!string.IsNullOrEmpty(tbFractionOfBasic.Text))
                {
                    double? fractionBasic = Convert.ToDouble(tbFractionOfBasic.Value);
                    adj.FractionOfBasic = fractionBasic / 100;
                }
                else
                {
                    adj.FractionOfBasic = null;
                }
            }
        }

        protected void tbPercentageBasic_DataBinding(object sender, EventArgs e)
        {
            TextBoxEx tb = (TextBoxEx)sender;
            if (!string.IsNullOrEmpty(tb.Text))
            {
                decimal? val = Convert.ToDecimal(tb.Value);
                tb.Value = Convert.ToString(val * 100);
            }
        }
        

        /// <summary>
        /// Binding Gridview when an item is deleted to refresh it.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void frmAdjustment_ItemDeleted(object sender, FormViewDeletedEventArgs e)
        {
            if (e.Exception == null)
            {
                gvAdjustments.DataBind();
                dsSpecificAdjustment.WhereParameters["AdjustmentId"].DefaultValue = "-1";
            }
        }
        # endregion

        /// <summary>
        /// This method use for following purpose.
        /// 1.In insert case inserted row have been selected.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvAdjustments_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            switch (e.Row.RowType)
            {
                case DataControlRowType.DataRow:
                    if (_adjustmentID != -1)
                    {
                        Adjustment adj=(Adjustment)e.Row.DataItem;

                        if (adj.AdjustmentId == _adjustmentID)
                        {
                            e.Row.RowState |= DataControlRowState.Selected;
                        }
                    }
                    break;
            }
        }


        protected void gvAdjustments_SelectedIndexChanged(object sender, EventArgs e)
        {
            dsSpecificAdjustment.WhereParameters["AdjustmentId"].DefaultValue = gvAdjustments.SelectedDataKey["AdjustmentId"].ToString();
            if (frmAdjustment.CurrentMode == FormViewMode.Insert)
                frmAdjustment.ChangeMode(FormViewMode.ReadOnly);
            dlgEditor.Visible = true;
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            frmAdjustment.DeleteItem();
            
        }

        protected void btnEdit_Click(object sender, EventArgs e)
        {
            frmAdjustment.ChangeMode(FormViewMode.Edit);
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            switch (frmAdjustment.CurrentMode)
            {
                case FormViewMode.Insert:
                    frmAdjustment.ChangeMode(FormViewMode.ReadOnly);
                    dsSpecificAdjustment.WhereParameters["AdjustmentId"].DefaultValue = Convert.ToString(-1);
                    break;
                case FormViewMode.Edit:
                    frmAdjustment.ChangeMode(FormViewMode.ReadOnly);
                    break;
            }
        }

        /// <summary>
        /// Change the mode of the form view from default mode to Insert mode.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnNew_Click(object sender, EventArgs e)
        {
            frmAdjustment.ChangeMode(FormViewMode.Insert);
            dlgEditor.Visible = true;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            switch (frmAdjustment.CurrentMode)
            {
                case FormViewMode.Insert:
                    frmAdjustment.InsertItem(false);
                    break;
                case FormViewMode.Edit:
                    frmAdjustment.UpdateItem(false);
                    break;
            }
        }
    }
}
