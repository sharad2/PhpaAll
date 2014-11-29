using System;
using System.Web.UI.WebControls;
using Eclipse.PhpaLibrary.Database.PIS;
using Eclipse.PhpaLibrary.Web;

namespace Finance.PIS.Masters
{
    public partial class TrainingTypes : PageBase
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
            gvTrainingType.InsertRowsCount = 1;
            btnInsert.Visible = false;
        }

        #endregion

        #region Inserted, Updated, Deleted

        /// <summary>
        /// This method use for following purpose
        /// 1.set status message.
        /// 2.catch exception from database and show exception message.
        /// </summary>
        int _trainingTypeId = -1;
        protected void dsTrainingType_Inserted(object sender, LinqDataSourceStatusEventArgs e)
        {
            if (e.Exception == null)
            {
                btnInsert.Visible = true;
                _trainingTypeId = GetTrainingTypeId(e.Result);
                TrainingType_sp.AddStatusText("Training Type inserted successfully");
            }
            else
            {
                TrainingType_sp.AddErrorText(e.Exception.Message);
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
        protected void dsTrainingType_Updated(object sender, LinqDataSourceStatusEventArgs e)
        {
            if (e.Exception == null)
            {
                _trainingTypeId = GetTrainingTypeId(e.Result);
                TrainingType_sp.AddStatusText("Training Type updated successfully");
            }
            else
            {
                TrainingType_sp.AddErrorText(e.Exception.Message);
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
        protected void dsTrainingType_Deleted(object sender, LinqDataSourceStatusEventArgs e)
        {
            if (e.Exception == null)
            {
                TrainingType_sp.AddStatusText("Training Type deleted successfully");
            }
            else
            {
                TrainingType_sp.AddErrorText(e.Exception.Message);
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
        protected void gvTrainingType_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            switch (e.Row.RowType)
            {
                case DataControlRowType.DataRow:
                    if (_trainingTypeId != -1)
                    {
                        int id = GetTrainingTypeId(e.Row.DataItem);
                        if (id == _trainingTypeId)
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

        private int GetTrainingTypeId(object dataItem)
        {
            TrainingType trainingTypeResult = (TrainingType)dataItem;
            return trainingTypeResult.TrainingTypeId;
        }

        #endregion

    }
}
