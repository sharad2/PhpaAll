using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using Eclipse.PhpaLibrary.Web;

namespace PhpaAll.Store
{
    public partial class InsertUOM : PageBase
    {
        /// <summary>
        /// Creating Dynamic where clause.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dsUOM_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {

            List<string> whereClause = new List<string>();

            string strParam = (string)e.WhereParameters["UOM"];

            if (!string.IsNullOrEmpty(strParam))
            {
                whereClause.Add(string.Format("UOMCode.Contains(@UOM) || Description.Contains(@UOM)"));
            }

            dsUOM.Where = string.Join("&&", whereClause.ToArray());
        }


        protected void gvUOM_RowDeleted(object sender, GridViewDeletedEventArgs e)
        {
            if (e.Exception != null)
            {
                valSummary.ErrorMessages.Add(string.Format("{0} cannot be deleted because it is in use. Error is: {1}", e.Keys["UOMCode"], e.Exception.Message));
                e.ExceptionHandled = true;
            }
        }
        
        /// <summary>
        /// Force th grid to query again.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            gvUOM.PageIndex = 0;
            gvUOM.DataBind();

        }

        protected void btnNew_Click(object sender, EventArgs e)
        {
            gvUOM.InsertRowsCount = 1;
        }
    }
}

