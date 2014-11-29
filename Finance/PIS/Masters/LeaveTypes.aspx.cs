using System;
using System.Web.UI.WebControls;
using Eclipse.PhpaLibrary.Database.PIS;
using Eclipse.PhpaLibrary.Web;

namespace PIS.Masters
{
    public partial class LeaveTypes : PageBase
    {
        #region Button Insert

        /// <summary>
        /// This method use for following purpose
        /// 1.Set insert row count.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnInsert_Click(object sender, EventArgs e)
        {
          
            gvLeaveTypes.InsertRowsCount = 1;
            btnInsert.Visible = false;
        }

        #endregion

        #region Inserted, Updated, Deleted

        /// <summary>
        /// This method use for following purpose
        /// 1.set status message.
        /// 2.catch exception from database and show exception message.
        /// </summary>
        int _leaveTypeId = -1;
        protected void dsLeaveTypes_Inserted(object sender, LinqDataSourceStatusEventArgs e)
        {
            if (e.Exception == null)
            {
                btnInsert.Visible = true;
                _leaveTypeId = GetLeaveTypeId(e.Result);
                LeaveTypes_sp.AddStatusText("Leave Type inserted successfully");
            }
            else
            {
                LeaveTypes_sp.AddErrorText(e.Exception.Message);
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
        protected void dsLeaveTypes_Updated(object sender, LinqDataSourceStatusEventArgs e)
        {
            if (e.Exception == null)
            {
                _leaveTypeId = GetLeaveTypeId(e.Result);
                LeaveTypes_sp.AddStatusText("Leave Type updated successfully");
            }
            else
            {
                LeaveTypes_sp.AddErrorText(e.Exception.Message);
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
        protected void dsLeaveTypes_Deleted(object sender, LinqDataSourceStatusEventArgs e)
        {
            if (e.Exception == null)
            {
                LeaveTypes_sp.AddStatusText("Leave Type deleted successfully");
            }
            else
            {
                LeaveTypes_sp.AddErrorText(e.Exception.Message);
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
        protected void gvLeaveTypes_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            switch (e.Row.RowType)
            {
                case DataControlRowType.DataRow:
                    if (_leaveTypeId != -1)
                    {
                        int id = GetLeaveTypeId(e.Row.DataItem);
                        if (id == _leaveTypeId)
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

        private int GetLeaveTypeId(object dataItem)
        {
            LeaveType _leaveTypeResult = (LeaveType)dataItem;
            return _leaveTypeResult.LeaveTypeId;
        }

        #endregion

    }
}






