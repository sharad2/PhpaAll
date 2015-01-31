using System;
using System.Web.UI.WebControls;
using Eclipse.PhpaLibrary.Database.MIS;
using Eclipse.PhpaLibrary.Web;

namespace PhpaAll.MIS
{
    public partial class Packages : PageBase
    {
        /// <summary>
        /// This method use for following purpose
        /// 1.Set insert row count.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnInsert_Click(object sender, EventArgs e)
        {
            gvPackage.InsertRowsCount = 1;
            btnInsert.Visible = false;
        }
        /// <summary>
        /// This method use for following purpose
        /// 1.set status message.
        /// 2.catch exception from database and show exception message.
        /// </summary>
        int _packageId = -1;
        protected void dsPackage_Inserted(object sender, LinqDataSourceStatusEventArgs e)
        {
            if (e.Exception == null)
            {
                btnInsert.Visible = true;
                _packageId = GetPackageId(e.Result);
                Package_status.AddStatusText("Package inserted successfully");
            }
            else
            {
                Package_status.AddErrorText(e.Exception.Message);
                e.ExceptionHandled = true;
            }
        }
        /// <summary>
        /// This method use for following purpose.
        /// 1.In insert case inserted row have been selected.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvPackage_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            switch (e.Row.RowType)
            {
                case DataControlRowType.DataRow:
                    if (_packageId != -1)
                    {
                        int id = GetPackageId(e.Row.DataItem);
                        if (id == _packageId)
                        {
                            e.Row.RowState |= DataControlRowState.Selected;
                        }
                    }
                    
                    break;
                default:
                    break;
            }

        }
        private int GetPackageId(object dataItem)
        {
            Package packageresult = (Package)dataItem;
            return packageresult.PackageId;
        }
        /// <summary>
        /// This method use for following purpose
        /// 1.Set status message
        /// 2.Catch exception from database and show exception message.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dsPackage_Updated(object sender, LinqDataSourceStatusEventArgs e)
        {
            if (e.Exception == null)
            {
                _packageId = GetPackageId(e.Result);
                Package_status.AddStatusText("package updated successfully");
            }
            else
            {
                Package_status.AddErrorText(e.Exception.Message);
                e.ExceptionHandled = true;
            }
        }
        /// <summary>
        /// This method use for following purpose
        /// 1.Set status message
        /// 2.Catch exception from database and show exception message. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dsPackage_Deleted(object sender, LinqDataSourceStatusEventArgs e)
        {
            if (e.Exception == null)
            {
                Package_status.AddStatusText("Package deleted successfully");
            }
            else
            {
                Package_status.AddErrorText(e.Exception.Message);
                e.ExceptionHandled = true;
            }
        }
    }
}





