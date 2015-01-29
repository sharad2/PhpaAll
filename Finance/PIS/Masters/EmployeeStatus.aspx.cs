using System;
using System.Web.UI.WebControls;
using Eclipse.PhpaLibrary.Web;

namespace PhpaAll.PIS.Masters
{
    public partial class EmployeeStatus : PageBase
    {
        #region Button Insert

        /// <summary>
        /// This method is used for following purpose
        /// 1.Set insert row count.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnInsert_Click(object sender, EventArgs e)
        {
            gvEmployeeStatus.InsertRowsCount = 1;
            btnInsert.Visible = false;
        }

        #endregion

        #region Inserted,Updated,Deleted

        /// <summary>
        /// This method is used for following purpose
        /// 1.set status message.
        /// 2.catch exception from database and show exception message.
        /// </summary>
        int _employeeStatusId = -1;
        protected void dsEmployeeStatus_Inserted(object sender, LinqDataSourceStatusEventArgs e)
        {
            if (e.Exception == null)
            {
                btnInsert.Visible = true;
                _employeeStatusId = GetEmployeeStatusId(e.Result);
                EmployeeStatus_sp.AddStatusText("Employee Status inserted successfully");
            }
            else
            {
                EmployeeStatus_sp.AddErrorText(e.Exception.Message);
                e.ExceptionHandled = true;
            }
        }

        /// <summary>
        /// This method is used for following purpose
        /// 1.Set status message
        /// 2.Catch exception from database and show exception message.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dsEmployeeStatus_Updated(object sender, LinqDataSourceStatusEventArgs e)
        {
            if (e.Exception == null)
            {
                _employeeStatusId = GetEmployeeStatusId(e.Result);
                EmployeeStatus_sp.AddStatusText("Employee Status updated successfully");
            }
            else
            {
                EmployeeStatus_sp.AddErrorText(e.Exception.Message);
                e.ExceptionHandled = true;
            }
        }

        /// <summary>
        /// This method is used for following purpose
        /// 1.Set status message
        /// 2.Catch exception from database and show exception message. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dsEmployeeStatus_Deleted(object sender, LinqDataSourceStatusEventArgs e)
        {
            if (e.Exception == null)
            {
                EmployeeStatus_sp.AddStatusText("Employee Status deleted successfully");
            }
            else
            {
                EmployeeStatus_sp.AddErrorText(e.Exception.Message);
                e.ExceptionHandled = true;
            }
        }

        #endregion

        #region RowdataBound

        /// <summary>
        /// This method use for following purpose.
        /// 1.In insert case inserted row have been selected.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvEmployeeStatus_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            switch (e.Row.RowType)
            {
                case DataControlRowType.DataRow:
                    if (_employeeStatusId != -1)
                    {
                        int id = GetEmployeeStatusId(e.Row.DataItem);
                        if (id == _employeeStatusId)
                        {
                            e.Row.RowState |= DataControlRowState.Selected;
                        }
                    }

                    break;
                default:
                    break;
            }

        }

        #endregion

        #region Helpers

        private int GetEmployeeStatusId(object dataItem)
        {
            Eclipse.PhpaLibrary.Database.PIS.EmployeeStatus employeeStatusResult = (Eclipse.PhpaLibrary.Database.PIS.EmployeeStatus)dataItem;
            return employeeStatusResult.EmployeeStatusId;
        }

        #endregion

    }
}
