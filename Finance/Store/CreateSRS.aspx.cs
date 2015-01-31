using System;
using System.Data.Linq;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Services;
using System.Web.UI.WebControls;
using Eclipse.PhpaLibrary.Database.Store;
using Eclipse.PhpaLibrary.Reporting;
using Eclipse.PhpaLibrary.Web;
using EclipseLibrary.Web.JQuery;
using EclipseLibrary.Web.JQuery.Input;

namespace PhpaAll.Store
{
    public partial class CreateSRS : PageBase
    {
        #region Load
        // If no query string parameter passed, Form view mode changed to insert.
        protected override void OnLoad(EventArgs e)
        {

            if (!IsPostBack)
            {
                if (this.Request.UrlReferrer != null)
                {
                    this.hfReferrer.Value = this.Request.UrlReferrer.ToString();
                }
                string gRNId = Request.QueryString["SRSId"];
                if (!string.IsNullOrEmpty(gRNId))
                {
                   // this.fvSrs.ChangeMode(FormViewMode.Edit);
                    this.fvSrs.ChangeMode(FormViewMode.ReadOnly);
                    this.Title = "Edit GIN";
                }
                else
                {
                    this.fvSrs.ChangeMode(FormViewMode.Insert);
                    this.Title = "Create GIN";
                }
            }
            base.OnLoad(e);
        }
        #endregion
       
        #region DataSource Methods
        private StoreDataContext _db;

        protected void dsSRS_ContextCreating(object sender, LinqDataSourceContextEventArgs e)
        {
            if (_db != null)
            {
                e.ObjectInstance = _db;
            }
        }

        protected void dsSRS_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            if (e.WhereParameters["SRSId"] == null)
            {
                e.Cancel = true;
            }
        }

        protected void dsSRS_ContextCreated(object sender, LinqDataSourceStatusEventArgs e)
        {
            StoreDataContext db = (StoreDataContext)e.Result;
            DataLoadOptions dlo = new DataLoadOptions();
            dlo.LoadWith<SRS>(srs => srs.SRSItems);
            dlo.LoadWith<SRS>(srs => srs.RoDivision1);
            dlo.LoadWith<SRS>(srs => srs.RoDivision2);
            dlo.LoadWith<SRS>(srs => srs.RoEmployee);
            dlo.LoadWith<SRS>(srs => srs.RoEmployee1);
            dlo.LoadWith<SRS>(srs => srs.RoEmployee3);
            dlo.LoadWith<SRS>(srs => srs.RoEmployee4);
            dlo.LoadWith<SRS>(srs => srs.HeadOfAccount);
            db.LoadOptions = dlo;
        }

        protected void dsSRSItems_ContextCreated(object sender, LinqDataSourceStatusEventArgs e)
        {
            StoreDataContext db = (StoreDataContext)e.Result;
            DataLoadOptions dlo = new DataLoadOptions();
            dlo.LoadWith<SRSItem>(srsIt => srsIt.Item);
            dlo.LoadWith<Item>(item => item.UOM);
            dlo.LoadWith<Item>(item => item.ItemCategory);
            db.LoadOptions = dlo;
        }

        protected void dsSRS_Inserted(object sender, LinqDataSourceStatusEventArgs e)
        {
            if (e.Exception == null)
            {
                SRS srs = (SRS)e.Result;
                this.dsSRS.WhereParameters["SRSId"].DefaultValue = srs.SRSId.ToString();
                this.dsSRSItems.WhereParameters["SRSId"].DefaultValue = srs.SRSId.ToString();
                this.dsSRSItems.InsertParameters["SRSId"].DefaultValue = srs.SRSId.ToString();
                this.dsSRSItems.UpdateParameters["SRSId"].DefaultValue = srs.SRSId.ToString();
            }
        }

        /// <summary>
        /// TO DO: Testing 
        /// 
        /// Testing - result
        /// 1. Deleteing when Only SRS item is present.
        /// 2. Deleteing with SRSitems present
        /// 3. The DELETE statement conflicted with the REFERENCE constraint "FK_SRSIssues_SRSItem". The conflict occurred in database "PhpaProd_082710", table "Store.SRSIssueItems", column 'SRSItemId'. The statement has been terminated.
        /// </summary>
        protected void dsSRS_Deleting(object sender, LinqDataSourceDeleteEventArgs e)
        {
            StoreDataContext db = (StoreDataContext)dsSRS.Database;
            SRS srs = (SRS)e.OriginalObject;
            var query = from srsItem in db.SRSItems
                        where srsItem.SRSId == srs.SRSId
                        && srsItem.SRSItemId != null
                        select srsItem;

            foreach (SRSItem srsItem in query)
            {
                db.SRSItems.DeleteOnSubmit(srsItem);
            }
        }

        protected void dsSRS_Updated(object sender, LinqDataSourceStatusEventArgs e)
        {
            if (e.Exception == null)
            {
                SRS srs = (SRS)e.Result;
                this.dsSRS.WhereParameters["SRSId"].DefaultValue = srs.SRSId.ToString();
                this.dsSRSItems.WhereParameters["SRSId"].DefaultValue = srs.SRSId.ToString();
                this.dsSRSItems.InsertParameters["SRSId"].DefaultValue = srs.SRSId.ToString();
                this.dsSRSItems.UpdateParameters["SRSId"].DefaultValue = srs.SRSId.ToString();
            }
        }

        #endregion

        #region Button Click Events
        protected void btnEdit_Click(object sender, EventArgs e)
        {
            this.fvSrs.ChangeMode(FormViewMode.Edit);
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            fvSrs.ChangeMode(FormViewMode.ReadOnly);
        }

        protected void btnNew_Click(object sender, EventArgs e)
        {
            Response.Redirect(Request.Url.AbsolutePath);
        }
     
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            fvSrs.DeleteItem();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            ButtonEx btn = (ButtonEx)sender;
            if (!btn.IsPageValid())
            {
                return;
            }

            GridViewExInsert gvEditSRSItems = (GridViewExInsert)btn.NamingContainer.FindControl("gvEditSRSItems");
            bool bSuccess = SaveSRSItems(gvEditSRSItems);

            if (bSuccess)
            {
                fvSrs.ChangeMode(FormViewMode.ReadOnly);
            }
        }

        protected void btnAddrow_Click(object sender, EventArgs e)
        {
            LinkButtonEx btn = (LinkButtonEx)sender;
            if (!btn.IsPageValid())
            {
                return;
            }

            GridViewExInsert gvEditSRSItems = (GridViewExInsert)btn.NamingContainer.FindControl("gvEditSRSItems");
            bool bSuccess = SaveSRSItems(gvEditSRSItems);

            if (bSuccess)
            {
                fvSrs.ChangeMode(FormViewMode.Edit);
            }
        }
        #endregion

        #region FormView Events

        private DateTime? m_defaultDateForInsert;
        
        protected void fvSrs_ItemCreated(object sender, EventArgs e)
        {
            switch (fvSrs.CurrentMode)
            {
                case FormViewMode.Insert:
                    TextBoxEx tbSrsDate = (TextBoxEx)fvSrs.FindControl("tbSrsDate");
                    if (m_defaultDateForInsert == null)
                    {
                        tbSrsDate.Text = DateTime.Today.ToShortDateString();
                    }
                    else
                    {
                        tbSrsDate.Text = m_defaultDateForInsert.Value.ToShortDateString();
                    }
                    break;
            }
        }

        protected void fvSrs_ItemInserted(object sender, FormViewInsertedEventArgs e)
        {
            if (e.Exception == null)
            {
                m_defaultDateForInsert = Convert.ToDateTime(e.Values["SRSCreateDate"]);
            }
        }
                      
        #endregion

        #region CommonMethod
       
        /// <summary>
        /// This function is where data is actually getting stored in DB
        /// Transaction are taken care of.
        /// Its common code used in btnAddrow_Click and btnSave_Click
        /// </summary>
        /// <param name="gvEditSRSItems"></param>
        private bool SaveSRSItems(GridViewExInsert gvEditSRSItems)
        {
            SqlConnection conn = null;
            SqlTransaction trans = null;
            bool bSuccess = false;

            try
            {
                conn = new SqlConnection(ReportingUtilities.DefaultConnectString);
                conn.Open();
                trans = conn.BeginTransaction();
                _db = new StoreDataContext(conn);
                _db.Transaction = trans;

                switch (fvSrs.CurrentMode)
                {
                    case FormViewMode.Edit:
                        fvSrs.UpdateItem(false);
                        break;

                    case FormViewMode.Insert:
                        fvSrs.InsertItem(false);
                        break;
                }

                foreach (GridViewRow row in gvEditSRSItems.Rows)
                {
                    DropDownListEx ddlStatus = (DropDownListEx)row.FindControl("ddlStatus");
                    switch (ddlStatus.Value)
                    {
                        case "I":
                            gvEditSRSItems.InsertRow(row.RowIndex);
                            break;

                        case "G":
                            break;

                        case "U":
                            gvEditSRSItems.UpdateRow(row.RowIndex, false);
                            break;

                        case "D":
                            gvEditSRSItems.DeleteRow(row.RowIndex);
                            break;

                        default:
                            throw new NotImplementedException();
                    }
                }

                trans.Commit();
                bSuccess = true;
            }
            catch (Exception ex)
            {
                if (trans != null)
                {
                    trans.Rollback();
                }
                EclipseLibrary.Web.JQuery.Input.ValidationSummary val = (EclipseLibrary.Web.JQuery.Input.ValidationSummary)gvEditSRSItems.NamingContainer.FindControl("ValidationSummary2");
                val.ErrorMessages.Add(ex.Message);
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

        #endregion

        protected void tb_ServerValidate(object sender, EclipseLibrary.Web.JQuery.Input.ServerValidateEventArgs e)
        {
            AutoComplete tb = (AutoComplete)e.ControlToValidate;
            e.ControlToValidate.IsValid = true;
            if ((tb.Value == "" && tb.DisplayValue == "") || (tb.Value != "" && tb.DisplayValue != ""))
            {
                return;
            }

            e.ControlToValidate.IsValid = false;
            e.ControlToValidate.ErrorMessage = "Invalid Data in "+ tb.FriendlyName +"  Field";
        }
    }
}