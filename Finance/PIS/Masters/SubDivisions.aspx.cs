using System;
using System.Data.Linq;
using System.Web.UI.WebControls;
using Eclipse.PhpaLibrary.Database.PIS;
using Eclipse.PhpaLibrary.Web;

namespace Finance.PIS.Masters
{
    public partial class SubDivisions : PageBase
    {
        #region Context Creation

        protected void dsSubDivisions_ContextCreated(object sender, LinqDataSourceStatusEventArgs e)
        {
            DataContext db = (DataContext)e.Result;
            DataLoadOptions dlo = new DataLoadOptions();
            dlo.LoadWith<SubDivision>(p => p.Division);
            db.LoadOptions = dlo;
        }

        #endregion

        #region Insert Button

        /// <summary>
        /// This method use for following purpose
        /// 1.Set insert row count.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnInsert_Click(object sender, EventArgs e)
        {
            gvSubDivisions.InsertRowsCount = 1;
            btnInsert.Visible = false;
        }

        #endregion

        #region Inserted,Updated,Deleted

        /// <summary>
        /// This method use for following purpose
        /// 1.set status message.
        /// 2.catch exception from database and show exception message.
        /// </summary>
        int _subDivisionId = -1;
        protected void dsSubDivisions_Inserted(object sender, LinqDataSourceStatusEventArgs e)
        {
            if (e.Exception == null)
            {
                btnInsert.Visible = true;
                _subDivisionId = GetSubDivisionId(e.Result);
                SubDivisions_sp.AddStatusText("Sub Division inserted successfully");
            }
            else
            {
                SubDivisions_sp.AddErrorText(e.Exception.Message);
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
        protected void dsSubDivisions_Updated(object sender, LinqDataSourceStatusEventArgs e)
        {
            if (e.Exception == null)
            {
                _subDivisionId = GetSubDivisionId(e.Result);
                SubDivisions_sp.AddStatusText("Sub Division updated successfully");
            }
            else
            {
                SubDivisions_sp.AddErrorText(e.Exception.Message);
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
        protected void dsSubDivisions_Deleted(object sender, LinqDataSourceStatusEventArgs e)
        {
            if (e.Exception == null)
            {
                SubDivisions_sp.AddStatusText("Sub Division deleted successfully");
            }
            else
            {
                SubDivisions_sp.AddErrorText(e.Exception.Message);
                e.ExceptionHandled = true;
            }
        }

        #endregion

        #region RowDataBound

        /// <summary>
        /// This method use for following purpose.
        /// 1.In insert case inserted row have been selected.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvSubDivisions_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            switch (e.Row.RowType)
            {
                case DataControlRowType.DataRow:
                    if (_subDivisionId != -1)
                    {
                        int id = GetSubDivisionId(e.Row.DataItem);
                        if (id == _subDivisionId)
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

        private int GetSubDivisionId(object dataItem)
        {
            SubDivision subDivisionResult = (SubDivision)dataItem;
            return subDivisionResult.SubDivisionId;
        }

        #endregion
    }
}
