using System;
using System.Data.Linq;
using System.Data.SqlClient;
using System.Linq;
using System.Web.UI.WebControls;
using Eclipse.PhpaLibrary.Database;
using Eclipse.PhpaLibrary.Reporting;
using Eclipse.PhpaLibrary.Web;
using EclipseLibrary.Web.JQuery;
using EclipseLibrary.Web.JQuery.Input;
using System.Collections.Generic;
using System.Web.Security;

namespace Finance.Finance
{
    /// <summary>
    /// If voucherId is passed in the query string, then details of that voucher are shown.
    /// Otherwise the screen opens up in insert mode.
    /// </summary>
    public partial class InsertVoucher : PageBase
    {
        /// <summary>
        /// Created during insert/update operations only
        /// </summary>
        private FinanceDataContext _db;

        /// <summary>
        /// While updating we use a transaction so at that time _db is not null
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ds_ContextCreating(object sender, LinqDataSourceContextEventArgs e)
        {
            if (_db != null)
            {
                e.ObjectInstance = _db;
            }
        }

        /// <summary>
        /// If VoucherID passed then edit else insert
        /// </summary>
        /// <param name="e"></param>
        protected override void OnLoad(EventArgs e)
        {
            if (!IsPostBack)
            {
                if (string.IsNullOrEmpty(this.Request.QueryString["VoucherId"]))
                {
                    fvEdit.DefaultMode = FormViewMode.Insert;
                    //fvEdit.ChangeMode(FormViewMode.Insert);

                    if (string.IsNullOrEmpty(this.Request.QueryString["VoucherDate"]))
                    {
                        TextBoxEx tbVoucherDate = (TextBoxEx)fvEdit.FindControl("tbVoucherDate");
                        tbVoucherDate.Text = DateTime.Now.ToShortDateString();
                    }
                }
                else
                {
                    fvEdit.DefaultMode = FormViewMode.ReadOnly;
                    //fvEdit.ChangeMode(FormViewMode.ReadOnly);
                }
            }
            ctlVoucherDetail.Station = this.GetUserStations();
            ctlVoucherDetail.DataBind();
            base.OnLoad(e);
        }

        /// <summary>
        /// Cancel query if no voucher id
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dsEditVouchers_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            if (e.WhereParameters["VoucherId"] == null)
            {
                e.Cancel = true;
            }
        }

        protected void dsEditVouchers_Deleting(object sender, LinqDataSourceDeleteEventArgs e)
        {
            FinanceDataContext db = (FinanceDataContext)dsEditVouchers.Database;
            Voucher voucher = (Voucher)e.OriginalObject;
            var query = from voucherItem in db.VoucherDetails
                        where voucherItem.VoucherId == voucher.VoucherId
                        && voucherItem.VoucherId != null
                        select voucherItem;

            foreach (VoucherDetail voucherItem in query)
            {
                db.VoucherDetails.DeleteOnSubmit(voucherItem);
            }
        }

        private bool _anyEmployee;
        private bool _anyJob;

        /// <summary>
        /// Set the _anyEmployee and _anyJob flags if data contains any employee/job
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvEditVoucherDetails_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            switch (e.Row.RowType)
            {
                case DataControlRowType.DataRow:
                    if ((e.Row.RowState & DataControlRowState.Insert) != DataControlRowState.Insert)
                    {
                        VoucherDetail vd = (VoucherDetail)e.Row.DataItem;
                        if (vd.EmployeeId != null)
                        {
                            _anyEmployee = true;
                        }
                        if (vd.JobId != null)
                        {
                            _anyJob = true;
                        }
                    }
                    break;
            }
        }

        /// <summary>
        /// Make the employee/job column visible if it contains data
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvEditVoucherDetails_DataBound(object sender, EventArgs e)
        {
            GridViewExInsert gvEditVoucherDetails = (GridViewExInsert)fvEdit.FindControl("gvEditVoucherDetails");
            DropDownListEx ddlMoreColumns = (DropDownListEx)fvEdit.FindControl("ddlMoreColumns");
            if (_anyEmployee)
            {
                ddlMoreColumns.Value = "E";
            }
            else
            {
                DataControlField mbf = gvEditVoucherDetails.Columns.Cast<DataControlField>()
                    .Single(p => p.AccessibleHeaderText == "Employee");
                mbf.FooterStyle.CssClass =
                    mbf.HeaderStyle.CssClass =
                    mbf.ItemStyle.CssClass = "ui-helper-hidden";
            }
            if (_anyJob)
            {
                ddlMoreColumns.Value = "J";
            }
            else
            {
                DataControlField mbf = gvEditVoucherDetails.Columns.Cast<DataControlField>()
                    .Single(p => p.AccessibleHeaderText == "Job");
                mbf.FooterStyle.CssClass =
                    mbf.HeaderStyle.CssClass =
                    mbf.ItemStyle.CssClass = "ui-helper-hidden";
                DataControlField mbfCon = gvEditVoucherDetails.Columns.Cast<DataControlField>()
                   .Single(p => p.AccessibleHeaderText == "Contractor");
                mbfCon.FooterStyle.CssClass =
                    mbfCon.HeaderStyle.CssClass =
                    mbfCon.ItemStyle.CssClass = "ui-helper-hidden";
            }
        }

        #region Updating/Inserting
        protected void btnSave_Click(object sender, EventArgs e)
        {
            ButtonEx btn = (ButtonEx)sender;
            if (!btn.IsPageValid())
            {
                return;
            }
            //voucher.Station = Session["station"].ToString();
            bool bSuccess = SaveVoucher();

            if (bSuccess)
            {
                //fvEdit.ChangeMode(FormViewMode.Edit);
                switch (fvEdit.CurrentMode)
                {
                    /*case FormViewMode.Edit:
                        fvEdit.ChangeMode(FormViewMode.ReadOnly);
                        ctlVoucherDetail.DataBind();
                        break;*/

                    case FormViewMode.Insert:
                        //string url = Request.Url.AbsoluteUri;
                        string [] url = Request.Url.AbsoluteUri.Split('?');

                        if(!string.IsNullOrEmpty(voucher.CheckNumber.ToString()))
                        {
                            int? CheckNumber = voucher.CheckNumber + 1;
                            url[0] = string.Format("{0}?CheckNumber={1}&VoucherDate={2}", url[0], CheckNumber.ToString(), voucher.VoucherDate.ToShortDateString());
                        }
                        Response.Redirect(url[0]);
                        break;
                    default:
                        // Hemant K. Singh 25 Apr 2014: Redirect so that the updated values show properly on the screen
                        Response.Redirect(Request.RawUrl, true);
                        break;
                }
            }
        }

        /// <summary>
        /// 1. Show entered data in edit mode if add new rows is clicked.
        /// 2. Show 6 more rows for entering details
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAddRow_Click(object sender, EventArgs e)
        {
            ButtonEx btnAddRow = (ButtonEx)sender;

            if (!btnAddRow.IsPageValid())
            {
                return;
            }
            bool bSuccess = SaveVoucher();

            if (bSuccess)
            {
               fvEdit.ChangeMode(FormViewMode.Edit);
            }
        }

        /// <summary>
        /// Returns true/false if voucher is created/edited/deleted 
        /// </summary>
        /// <returns></returns>
        private bool SaveVoucher()
        {
           
            SqlConnection conn = null;
            SqlTransaction trans = null;
            bool bSuccess = false;
            try
            {                
                conn = new SqlConnection(ReportingUtilities.DefaultConnectString);
                conn.Open();
                trans = conn.BeginTransaction();
                _db = new FinanceDataContext(conn);
                _db.Transaction = trans;
                PerformUpdates();
                trans.Commit();
                bSuccess = true;
            }
            catch (Exception ex)
            {
                if (trans != null)
                {
                    trans.Rollback();
                }
                
                EclipseLibrary.Web.JQuery.Input.ValidationSummary valSummary = (EclipseLibrary.Web.JQuery.Input.ValidationSummary)fvEdit.FindControl("valSummary");
                valSummary.ErrorMessages.Add(ex.Message);
            }
            finally
            {
                if (_db != null)
                {
                    _db.Dispose();
                    _db = null;
                }
                if (conn != null)
                {
                    conn.Dispose();
                }
            }
            return bSuccess;
        }
        /// <summary>
        /// Ritesh:18th Jan 2012
        /// Introduced new column Station in voucher table
        /// Storing station in vouvher table while inserting or updation voucher
        /// </summary>
        private void PerformUpdates()
        {
            DropDownListEx ddlStation = (DropDownListEx)fvEdit.FindControl("ddlStation");
            switch (fvEdit.CurrentMode)
            {
                case FormViewMode.Edit:
                    
                    //dsEditVouchers.UpdateParameters["Station"].DefaultValue = Session["station"].ToString();
                    dsEditVouchers.UpdateParameters["StationId"].DefaultValue = ddlStation.Value;
                    fvEdit.UpdateItem(false);
                    break;

                case FormViewMode.Insert:
                    
                    //dsEditVouchers.InsertParameters["Station"].DefaultValue = Session["station"].ToString();
                    dsEditVouchers.InsertParameters["StationId"].DefaultValue =ddlStation.Value;
                    fvEdit.InsertItem(false);
                    break;

                default:
                    throw new NotImplementedException();
            }
            GridViewExInsert gvEditVoucherDetails = (GridViewExInsert)fvEdit.FindControl("gvEditVoucherDetails");
            foreach (var index in gvEditVoucherDetails.SelectedIndexes)
            {
                GridViewRow row = gvEditVoucherDetails.Rows[index];
                DropDownListEx ddlStatus = (DropDownListEx)row.FindControl("ddlStatus");

                if ((row.RowState & DataControlRowState.Insert) == DataControlRowState.Insert)
                {
                    // Insert
                    gvEditVoucherDetails.InsertRow(index);
                }
                else if (ddlStatus.Value.Equals("D"))
                {
                    //Delete VoucherDetail
                    gvEditVoucherDetails.DeleteRow(index);
                }
                else
                {
                    // Update
                    gvEditVoucherDetails.UpdateRow(index, false);
                }

            }
        }       

        /// <summary>
        /// If form view insert fails, cancel the grid view insert
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dsEditVoucherDetail_Inserting(object sender, LinqDataSourceInsertEventArgs e)
        {
            if (string.IsNullOrEmpty(dsEditVoucherDetail.InsertParameters["VoucherId"].DefaultValue))
            {
                e.Cancel = true;
            }
        }

        /// <summary>
        /// If FormView update fails cancel the GridView update
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dsEditVoucherDetail_Updating(object sender, LinqDataSourceUpdateEventArgs e)
        {
            if (string.IsNullOrEmpty(dsEditVoucherDetail.UpdateParameters["VoucherId"].DefaultValue))
            {
                e.Cancel = true;
            }
        }

        Voucher voucher = new Voucher();
        /// <summary>
        /// Set the voucher id of GridView data source after a new voucher is inserted
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dsEditVouchers_Inserted(object sender, LinqDataSourceStatusEventArgs e)
        {
            if (e.Exception == null)
            {
                voucher = (Voucher)e.Result;

                dsEditVoucherDetail.InsertParameters["VoucherId"].DefaultValue = voucher.VoucherId.ToString();               
                dsEditVouchers.WhereParameters["VoucherId"].DefaultValue = voucher.VoucherId.ToString();
                dsEditVoucherDetail.WhereParameters["VoucherId"].DefaultValue = voucher.VoucherId.ToString();
            }
        }

        /// <summary>
        /// Set the voucher id in GridView data source
        /// 1) Pass VoucherId while inserting new VoucherDetail record at updating
        /// 2) Pass VoucherId while updating VoucherDetail record
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dsEditVouchers_Updated(object sender, LinqDataSourceStatusEventArgs e)
        {
            if (e.Exception == null)
            {
                voucher = (Voucher)e.Result;
                dsEditVoucherDetail.InsertParameters["VoucherId"].DefaultValue = voucher.VoucherId.ToString();
                dsEditVoucherDetail.UpdateParameters["VoucherId"].DefaultValue = voucher.VoucherId.ToString();
                

            }
        }

        #endregion

        #region Validation
        protected void val_IsRowSelected(object sender, DependencyCheckEventArgs e)
        {
            GridViewRow row = (GridViewRow)e.ControlToValidate.NamingContainer;
            GridViewEx gv = (GridViewEx)row.NamingContainer;
            e.NeedsToBeValdiated = gv.SelectedIndexes.Contains(row.RowIndex);
        }

        protected void tbCheckNumber_ServerDependencyCheck(object sender, DependencyCheckEventArgs e)
        {
            RadioButtonListEx rblVoucherTypes = (RadioButtonListEx)fvEdit.FindControl("rblVoucherTypes");
            if (!rblVoucherTypes.Value.Equals("B"))
            {
                e.NeedsToBeValdiated = false;
            }
        }       
     
        protected void val_DebitOrCredit(object sender, EclipseLibrary.Web.JQuery.Input.ServerValidateEventArgs e)
        {
            TextBoxEx tbCredit = (TextBoxEx)e.ControlToValidate;
            GridViewRow row = (GridViewRow)e.ControlToValidate.NamingContainer;
            TextBoxEx tbDebit = (TextBoxEx)row.FindControl("tbDebit");
            if (string.IsNullOrEmpty(tbCredit.Text) && string.IsNullOrEmpty(tbDebit.Text) ||
                (!string.IsNullOrEmpty(tbCredit.Text) && !string.IsNullOrEmpty(tbDebit.Text)))
            {
                tbCredit.ErrorMessage = string.Format("Row {0}: Debit or credit, but not both, must be specified", row.RowIndex + 1);
                tbCredit.IsValid = false;
            }
        }

        protected void val_HeadRequired(object sender, EclipseLibrary.Web.JQuery.Input.ServerValidateEventArgs e)
        {
            if (!e.ControlToValidate.IsValid)
            {
                GridViewRow row = (GridViewRow)e.ControlToValidate.NamingContainer;
                e.ControlToValidate.ErrorMessage = string.Format("Row {0}: ", row.RowIndex + 1) + e.ControlToValidate.ErrorMessage;
            }
        }

        protected void tb_ServerValidate(object sender, EclipseLibrary.Web.JQuery.Input.ServerValidateEventArgs e)
        {
            AutoComplete tbPayee = (AutoComplete)e.ControlToValidate;
            e.ControlToValidate.IsValid = false;
            RadioButtonListEx rblVoucherTypes = (RadioButtonListEx)fvEdit.FindControl("rblVoucherTypes");

            if (string.IsNullOrEmpty(tbPayee.Text))
            {
                if (rblVoucherTypes.Value.Equals("B") || rblVoucherTypes.Value.Equals("C"))
                {
                    e.ControlToValidate.ErrorMessage = "Payee is required";
                    return;
                }
                else
                {
                    e.ControlToValidate.IsValid = true;
                }
            }
            else
            {
                tbPayee.Value = tbPayee.Text;
                e.ControlToValidate.IsValid = true;
            }
        }
       
        #endregion

        protected void dlgVoucher_PreRender(object sender, EventArgs e)
        {
            Dialog dlgVoucher = (Dialog)fvEdit.FindControl("dlgVoucher");

            if (!string.IsNullOrEmpty(this.Request.QueryString["VoucherId"]))
            {
                dlgVoucher.Ajax.Url += string.Format("?VoucherId={0}", this.Request.QueryString["VoucherId"].ToString());
            }else if (voucher.VoucherId != 0)
            {
                dlgVoucher.Ajax.Url += string.Format("?VoucherId={0}", voucher.VoucherId.ToString());
            }
        }

        #region Selecting

        protected void dsEditVoucher_ContextCreated(object sender, LinqDataSourceStatusEventArgs e)
        {
            FinanceDataContext db = (FinanceDataContext)e.Result;
            DataLoadOptions dlo = new DataLoadOptions();
            dlo.LoadWith<Voucher>(p => p.Division);
            db.LoadOptions = dlo;
        }

        protected void dsEditVoucherDetail_ContextCreated(object sender, LinqDataSourceStatusEventArgs e)
        {
            FinanceDataContext db = (FinanceDataContext)e.Result;
            DataLoadOptions dlo = new DataLoadOptions();
            dlo.LoadWith<VoucherDetail>(p => p.HeadOfAccount);
            dlo.LoadWith<VoucherDetail>(p => p.Employee);
            dlo.LoadWith<VoucherDetail>(p => p.Job);
            db.LoadOptions = dlo;
        }

        #endregion

        #region ButtonEvent

        protected void btnEdit_Click(object sender, EventArgs e)
        {
            fvEdit.ChangeMode(FormViewMode.Edit);
        }
        
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            fvEdit.ChangeMode(FormViewMode.ReadOnly);
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            fvEdit.DeleteItem();
            Response.Redirect(Request.Url.AbsoluteUri);
        }

        protected void btnSave_PreRender(object sender, EventArgs e)
        {
            ButtonEx btn = (ButtonEx)sender;
            switch (fvEdit.CurrentMode)
            {
                case FormViewMode.Edit:
                    btn.Text = "Update";
                    break;
                //case FormViewMode.Insert:
                //    break;

            }
        }
        /// <summary>
        /// Ritesh 6th Feb 2012
        /// Drop down list for station provided which is now manadatory.
        /// Drop down shows stations on which user has right
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dsStations_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            //if (Session["Station"] == null)
            //{
            //    FormsAuthentication.RedirectToLoginPage();
            //    return;
            //}
            //List<string> stations = new List<string>(Session["Station"].ToString().Split(','));
            using (Eclipse.PhpaLibrary.Database.PIS.PISDataContext db = new Eclipse.PhpaLibrary.Database.PIS.PISDataContext(Eclipse.PhpaLibrary.Reporting.ReportingUtilities.DefaultConnectString))
            {
                var stations = this.GetUserStations();
                var query = (from station in db.Stations
                             where station.StationName != null && station.StationName != string.Empty
                             // && ((Session["Roles"].ToString() != "Administrator") ? stations.Contains(station.StationName) : stations.Any())
                             orderby station.StationName
                             select station).Distinct().ToList();
                if (stations != null)
                {
                    query = query.Where(p => p.StationId == null || stations.Contains(p.StationId)).ToList();
                }
                e.Result = query;
            }

        }

        #endregion
  
       
    }
}
