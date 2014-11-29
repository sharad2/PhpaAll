using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using Eclipse.PhpaLibrary.Web;
using Eclipse.PhpaLibrary.Database.Store;
using System.Collections.Generic;

namespace Finance.Store
{
    public partial class ItemsCategory : PageBase
    {
        /// <summary>
        /// Creating dynamic where clause.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        protected void dsItemCategory_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            List<string> whereClause = new List<string>();

            string strParameter = (string)e.WhereParameters["ItemCategory"];

            if (!string.IsNullOrEmpty(strParameter))
            {
                whereClause.Add(string.Format("ItemCategoryCode.Contains(@ItemCategory) || Description.Contains(@ItemCategory)"));
            }
            dsItemCategory.Where = string.Join("&&", whereClause.ToArray());
        }

        protected void gvItemCategory_RowDeleted(object sender, GridViewDeletedEventArgs e)
        {
            if (e.Exception != null)
            {
                valSummary.ErrorMessages.Add(string.Format("{0} cannot be deleted because it is in use. Error is: {1}", e.Keys["ItemCategoryCode"], e.Exception.Message));
                e.ExceptionHandled = true;
            }
        }
        
        /// <summary>
        /// Force the Grid to query.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            gvItemCategory.PageIndex = 0;
            gvItemCategory.DataBind();
        }


        protected void btnNew_Click(object sender, EventArgs e)
        {
            gvItemCategory.InsertRowsCount = 1;
        }
    }
}
