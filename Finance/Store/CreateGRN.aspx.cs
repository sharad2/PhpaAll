using System;
using System.Data.Linq;
using System.Data.SqlClient;
using System.Drawing;
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
    public partial class CreateGRN : PageBase
    {


        #region Load

        // If no query string parameter passed, Form view mode changed to insert.
        protected override void OnLoad(EventArgs e)
        {

            // Query string passed from material reciept to make the
            // form view in Read Only mode.
            if (!IsPostBack)
            {
                if (this.Request.UrlReferrer != null)
                {
                    this.hfReferrer.Value = this.Request.UrlReferrer.ToString();
                }
                string gRNId = Request.QueryString["GRNId"];
                if (!string.IsNullOrEmpty(gRNId))
                {
                    this.fvEdit.ChangeMode(FormViewMode.ReadOnly);
                    this.Title = "Edit GRN";
                }
                else
                {
                    this.fvEdit.ChangeMode(FormViewMode.Insert);
                    this.Title = "Create GRN";
                }
            }
            base.OnLoad(e);
        }
        #endregion


        /// <summary>
        /// Created during insert/update operations only
        /// </summary>
        private StoreDataContext _db;

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

        #region Selecting
        protected void dsGRN_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            if (e.WhereParameters["GRNId"] == null)
            {
                e.Cancel = true;
            }
        }

        protected void dsGRN_ContextCreated(object sender, LinqDataSourceStatusEventArgs e)
        {
            StoreDataContext db = (StoreDataContext)e.Result;
            DataLoadOptions dlo = new DataLoadOptions();
            dlo.LoadWith<GRN>(grn => grn.RoContractor);
            db.LoadOptions = dlo;
        }

        protected void dsGRNItems_ContextCreated(object sender, LinqDataSourceStatusEventArgs e)
        {
            StoreDataContext db = (StoreDataContext)e.Result;
            DataLoadOptions dlo = new DataLoadOptions();
            dlo.LoadWith<GRNItem>(gd => gd.Item);
            dlo.LoadWith<Item>(item => item.ItemCategory);
            dlo.LoadWith<Item>(item => item.UOM);
            db.LoadOptions = dlo;
        }
        #endregion

        #region Inserting

        /// <summary>
        /// Set the Inserted GRNID as default id for form view.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dsGRN_Inserted(object sender, LinqDataSourceStatusEventArgs e)
        {
            if (e.Exception == null)
            {
                GRN grn = (GRN)e.Result;
                this.dsGRN.WhereParameters["GRNId"].DefaultValue = grn.GRNId.ToString();
                this.dsGRNItems.WhereParameters["GRNId"].DefaultValue = grn.GRNId.ToString();
                this.dsGRNItems.InsertParameters["GRNId"].DefaultValue = grn.GRNId.ToString();
                this.dsGRNItems.UpdateParameters["GRNId"].DefaultValue = grn.GRNId.ToString();
            }
        }

        /// <summary>
        /// Set the Updated GRNID as default id for form view.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dsGRN_Updated(object sender, LinqDataSourceStatusEventArgs e)
        {
            if (e.Exception == null)
            {
                GRN grn = (GRN)e.Result;
                this.dsGRN.WhereParameters["GRNId"].DefaultValue = grn.GRNId.ToString();
                this.dsGRNItems.WhereParameters["GRNId"].DefaultValue = grn.GRNId.ToString();
                this.dsGRNItems.InsertParameters["GRNId"].DefaultValue = grn.GRNId.ToString();
                this.dsGRNItems.UpdateParameters["GRNId"].DefaultValue = grn.GRNId.ToString();
            }
        }


        /// <summary>
        /// When a GRN is inserted, the next GRN shows the previous GRN date as default
        /// </summary>
        private DateTime? m_defaultDateForInsert;

        protected void fvEdit_ItemInserted(object sender, FormViewInsertedEventArgs e)
        {
            if (e.Exception == null)
            {
                e.KeepInInsertMode = true;
                m_defaultDateForInsert = Convert.ToDateTime(e.Values["GRNCreateDate"]);
            }
        }

        protected void fvEdit_ItemUpdated(object sender, FormViewUpdatedEventArgs e)
        {
            if (e.Exception == null)
            {
                e.KeepInEditMode = true;
            }
        }

        /// <summary>
        /// Set default values for inserting.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void fvEdit_ItemCreated(object sender, EventArgs e)
        {
            switch (fvEdit.CurrentMode)
            {
                case FormViewMode.Insert:
                    TextBoxEx tbGRNCreateDate = (TextBoxEx)fvEdit.FindControl("tbGRNCreateDate");
                    if (m_defaultDateForInsert == null)
                    {
                        tbGRNCreateDate.Text = DateTime.Today.ToShortDateString();
                    }
                    else
                    {
                        tbGRNCreateDate.Text = m_defaultDateForInsert.Value.ToShortDateString();
                    }
                    break;
            }
        }

        #endregion

        /// <summary>
        /// TODO: Transactions
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSave_Click(object sender, EventArgs e)
        {
            ButtonEx btn = (ButtonEx)sender;
            if (!btn.IsPageValid())
            {
                return;
            }
            
            GridViewExInsert gvEditGRNItems = (GridViewExInsert)btn.NamingContainer.FindControl("gvEditGRNItems");
            bool bSuccess= SaveGRNItems(gvEditGRNItems);

            if (bSuccess)
            {
                fvEdit.ChangeMode(FormViewMode.ReadOnly);
            }
        }

        /// <summary>
        /// Keep the Edit mode and insert the previous entered items on Add row click.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAddrow_Click(object sender, EventArgs e)
        {
            LinkButtonEx btn = (LinkButtonEx)sender;
            if (!btn.IsPageValid())
            {
                return;
            }

            GridViewExInsert gvEditGRNItems = (GridViewExInsert)btn.NamingContainer.FindControl("gvEditGRNItems");
            bool bSuccess = SaveGRNItems(gvEditGRNItems);

            if (bSuccess)
            {
                fvEdit.ChangeMode(FormViewMode.Edit);
            }
            
        }
        
        /// <summary>
        /// Its common code used in btnAddrow_Click and btnSave_Click
        /// </summary>
        /// <param name="gvEditGRNItems"></param>
        private bool SaveGRNItems(GridViewExInsert gvEditGRNItems)
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

                switch (fvEdit.CurrentMode)
                {
                    case FormViewMode.Edit:
                        fvEdit.UpdateItem(false);
                        break;

                    case FormViewMode.Insert:
                        fvEdit.InsertItem(false);
                        break;
                }

                foreach (GridViewRow row in gvEditGRNItems.Rows)
                {
                    DropDownListEx ddlStatus = (DropDownListEx)row.FindControl("ddlStatus");
                    switch (ddlStatus.Value)
                    {
                        case "I":
                            gvEditGRNItems.InsertRow(row.RowIndex);
                            break;

                        case "G":
                            break;

                        case "U":
                            gvEditGRNItems.UpdateRow(row.RowIndex, false);
                            break;

                        case "D":
                            gvEditGRNItems.DeleteRow(row.RowIndex);
                            break;

                        default:
                            throw new NotImplementedException();
                    }
                }

                trans.Commit();
                bSuccess = true;
            }
            catch(Exception ex)
            {
                if (trans != null)
                {
                    trans.Rollback();
                }
                EclipseLibrary.Web.JQuery.Input.ValidationSummary val = (EclipseLibrary.Web.JQuery.Input.ValidationSummary)gvEditGRNItems.NamingContainer.FindControl("ValidationSummary2");
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

        #region Updating
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            fvEdit.DeleteItem();
        }

        protected void btnEdit_Click(object sender, EventArgs e)
        {
            fvEdit.ChangeMode(FormViewMode.Edit);
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            fvEdit.ChangeMode(FormViewMode.ReadOnly);
        }

        protected void btnNew_Click(object sender, EventArgs e)
        {
            Response.Redirect(Request.Url.AbsolutePath);
        }


      
        private decimal? m_sumTotal = 0;

        /// <summary>
        /// Calculation of Total column i.e Multiplication of Accpeted pieces and Price.
        /// Highlight the passed Item through query string.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvGRNItems_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            GridView gv = (GridView)sender;
            switch (e.Row.RowType)
            {
                case DataControlRowType.DataRow:

                    GRNItem grnDet = (GRNItem)e.Row.DataItem;
                    DataControlFieldCell cell = (from DataControlFieldCell c in e.Row.Cells
                                                 where c.ContainingField.AccessibleHeaderText == "ItemCode"
                                                 select c).Single();
                    if (cell.Text == this.Request.QueryString["ItemCode"])
                    {
                        e.Row.BackColor = Color.Plum;
                    }
                    if (grnDet.ReceivedQty != null)
                    {
                        int? delivered = grnDet.ReceivedQty;
                        decimal? Price = grnDet.Price;
                        decimal? LandedPrice = grnDet.LandedPrice;
                        decimal? TotalPrice = ReportingUtilities.AddNullable(Price, LandedPrice);
                        DataControlFieldCell k = (from DataControlFieldCell c in e.Row.Cells
                                                  where c.ContainingField.AccessibleHeaderText == "Total"
                                                  select c).Single();
                        k.Text = MultiplyValue(delivered, TotalPrice).ToString("N2");
                        m_sumTotal += Convert.ToDecimal(k.Text);
                    }
                    break;

                case DataControlRowType.Footer:
                    DataControlFieldCell cellF = (from DataControlFieldCell c in e.Row.Cells
                                                  where c.ContainingField.AccessibleHeaderText == "Total"
                                                  select c).Single();
                    cellF.Text = string.Format("{0:N2}", m_sumTotal.ToString());
                    break;
            }
        }

        /// <summary>
        /// Multiply two decimal values.
        /// </summary>
        /// <param name="value1"></param>
        /// <param name="value2"></param>
        /// <returns></returns>

        private decimal MultiplyValue(decimal? value1, decimal? value2)
        {
            if (value1 == null || value2 == null)
            {
                return 0;
            }
            return value1.Value * value2.Value;
        }

        #endregion

        #region Deleting

        /// <summary>
        /// Delete the selected GRN.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dsGRN_Deleting(object sender, LinqDataSourceDeleteEventArgs e)
        {
            StoreDataContext db = (StoreDataContext)dsGRN.Database;
            GRN grn = (GRN)e.OriginalObject;
            var query = from grnItem in db.GRNItems
                        where grnItem.GRNId == grn.GRNId
                        && grnItem.ItemId != null
                        select grnItem;

            foreach (GRNItem grnItem in query)
            {
                db.GRNItems.DeleteOnSubmit(grnItem);
            }
        }
        #endregion

        #region Web Methods
       

        /// <summary>
        /// Get the item unit based on the item selected.
        /// </summary>
        /// <param name="contextKey"></param>
        /// <returns></returns>
        [WebMethod]
        public static string GetItemUnit(string contextKey)
        {
            // The code selector sends description after : so get rid of it
            string itemCode = contextKey.Split(':')[0].Trim();
            using (StoreDataContext db = new StoreDataContext(ReportingUtilities.DefaultConnectString))
            {
                string unit = (from item in db.Items
                               where item.ItemCode == itemCode
                               select item.UOM.UOMCode).SingleOrDefault();
                return unit ?? "??";
            }
        }
        #endregion

        protected void tbOrdered_ServerValidate(object sender, EclipseLibrary.Web.JQuery.Input.ServerValidateEventArgs e)
        {
            TextBoxEx tbReceived = (TextBoxEx)e.ControlToValidate;
            int nReceivedQty = Convert.ToInt32(tbReceived.Text);

            TextBoxEx tbOrdered = (TextBoxEx)tbReceived.NamingContainer.FindControl("tbOrdered");
            int nOrderedQty = Convert.ToInt32(tbOrdered.Text);

            e.ControlToValidate.IsValid = nOrderedQty >= nReceivedQty;
            e.ControlToValidate.ErrorMessage = "Delivered qty cannot be greater than Ordered qty";
        }

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
    }
}
