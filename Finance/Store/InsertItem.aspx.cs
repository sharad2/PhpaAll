using System;
using System.Data.Linq;
using System.Linq;
using System.Web.UI.WebControls;
using Eclipse.PhpaLibrary.Database.Store;
using Eclipse.PhpaLibrary.Web;
using EclipseLibrary.Web.JQuery.Input;

namespace Finance.Store
{
    public partial class InsertItem : PageBase
    {

        protected void dsItem_ContextCreated(object sender, LinqDataSourceStatusEventArgs e)
        {
            StoreDataContext db = (StoreDataContext)e.Result;
            DataLoadOptions dlo = new DataLoadOptions();
            dlo.LoadWith<Item>(item => item.ItemCategory);
            dlo.LoadWith<Item>(item => item.UOM);
            dlo.LoadWith<Item>(item => item.HeadOfAccount);
            db.LoadOptions = dlo;
        }

        /// <summary>
        /// Binding grid after the selected data is Deleted..
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        protected void fvEditItem_ItemDeleted(object sender, FormViewDeletedEventArgs e)
        {
            if (e.Exception == null)
            {
                gvItem.DataBind();
            }
        }

        /// <summary>
        /// Creating Dynamic where clause.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dsItem_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            StoreDataContext db = (StoreDataContext)dsItem.Database;

            var items = from item in db.Items
                        select item;

            string strItem = (string)e.WhereParameters["ItemCode"];
            if (!string.IsNullOrEmpty(strItem))
            {
                items = items.Where(p => p.ItemCode == strItem);
            }
            int? categoryId = null;
            if (!string.IsNullOrEmpty(ddlCategory.Value))
            {
                categoryId = Convert.ToInt32(ddlCategory.Value);
                items = items.Where(p => p.ItemCategoryId == categoryId);
            }

            if (!string.IsNullOrEmpty(tbHeadOfAccount.Value))
            {
                items = items.Where(p => p.HeadOfAccountId == Convert.ToInt32(tbHeadOfAccount.Value));
            }

            if (!string.IsNullOrEmpty(tbBrand.Value))
            {
                items = items.Where(p => p.Brand.Contains(tbBrand.Text));
            }

            if (!string.IsNullOrEmpty(tbColor.Value))
            {
                items = items.Where(p => p.Color.Contains(tbColor.Text));
            }

            if (!string.IsNullOrEmpty(tbDimension.Value))
            {
                items = items.Where(p => p.Dimension.Contains(tbDimension.Text));
            }

            if (!string.IsNullOrEmpty(tbIdentifier.Value))
            {
                items = items.Where(p => p.Identifier.Contains(tbIdentifier.Text));
            }

            if (!string.IsNullOrEmpty(tbSize.Value))
            {
                items = items.Where(p => p.Size.Contains(tbSize.Text));
            }

            if (!string.IsNullOrEmpty(tbDescription.Value))
            {
                items = items.Where(p => p.Description.Contains(tbDescription.Text));
            }
            if (!string.IsNullOrEmpty(tbKeyword.Text))
            {
                string strKeyword = tbKeyword.Text;
                items = items.Where(p => p.Brand.Contains(strKeyword) || p.Color.Contains(strKeyword) || p.Description.Contains(strKeyword) ||
                    p.Dimension.Contains(strKeyword) || p.Identifier.Contains(strKeyword) || p.Size.Contains(strKeyword) ||
                    p.ItemCode == strKeyword);
            }
            e.Result = from item in items
                       orderby item.ItemCategory.Description, item.ItemCategoryId, item.Description, item.Brand, item.Color, item.Size, item.Identifier
                       select new
                       {
                           Description = item.Description,
                           ItemCode = item.ItemCode,
                           ItemId = item.ItemId,
                           Brand = item.Brand,
                           Dimension = item.Dimension,
                           Size = item.Size,
                           Color = item.Color,
                           Identifier = item.Identifier,
                           ItemCategory = item.ItemCategory,
                           UOM = item.UOM,
                           HeadOfAccount = item.HeadOfAccount,
                           ReorderingLevel = item.ReorderingLevel,
                           Remark = item.Remark,
                           CountGrn = item.GRNItems.Count,
                           CountSrs = item.SRSItems.Count
                       };
        }

        protected void fvEditItem_ItemCreated(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Set eh inserted Item in Red only mode ater inserted.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dsEditItem_Inserted(object sender, LinqDataSourceStatusEventArgs e)
        {
            if (e.Exception == null)
            {
                Item item = (Item)e.Result;
                gvItem.SelectedIndex = -1;
                this.dsEditItem.WhereParameters["ItemId"].DefaultValue = item.ItemId.ToString();
            }
        }


        /// <summary>
        /// Force the grid to Query.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            gvItem.PageIndex = 0;
            gvItem.DataBind();
        }

        /// <summary>
        /// Binding grid after the selected data is inserted.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        protected void fvEditItem_ItemInserted(object sender, FormViewInsertedEventArgs e)
        {
            if (e.Exception == null)
            {
                gvItem.DataBind();
            }
        }

        protected void dsEditItem_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            if (e.WhereParameters["ItemId"] == null)
            {
                e.Cancel = true;
            }
        }

        /// <summary>
        /// Binding grid after the selected data is updated.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        protected void fvEditItem_ItemUpdated(object sender, FormViewUpdatedEventArgs e)
        {
            if (e.Exception == null)
            {
                gvItem.DataBind();
            }
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

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            fvEditItem .ChangeMode(FormViewMode.ReadOnly);
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            ButtonEx btn = (ButtonEx)sender;
            if (!btn.IsPageValid())
            {
                return;
            }

            try
            {
                switch (fvEditItem.CurrentMode)
                {
                    case FormViewMode.Edit:
                        fvEditItem.UpdateItem(false);
                        break;

                    case FormViewMode.Insert:
                        fvEditItem.InsertItem(false);
                        break;
                }
                fvEditItem.ChangeMode(FormViewMode.ReadOnly);
            }
            catch (Exception ex)
            {
                EclipseLibrary.Web.JQuery.Input.ValidationSummary val = (EclipseLibrary.Web.JQuery.Input.ValidationSummary)btn.NamingContainer.FindControl("ValidationSummary2");
                val.ErrorMessages.Add(ex.Message);
            }
            
        }

        protected void btnEdit_Click(object sender, EventArgs e)
        {
            fvEditItem.ChangeMode(FormViewMode.Edit);
        }

        protected void btnNew_Click(object sender, EventArgs e)
        {
            fvEditItem.ChangeMode(FormViewMode.Insert);
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            fvEditItem.DeleteItem();
        }

        protected void btnRowDetails_Click(object sender, EventArgs e)
        {
            fvEditItem.ChangeMode(FormViewMode.ReadOnly);
        }
    }
}
