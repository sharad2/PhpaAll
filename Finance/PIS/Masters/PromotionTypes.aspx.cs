using System;
using System.Web.UI.WebControls;
using Eclipse.PhpaLibrary.Database.PIS;
using Eclipse.PhpaLibrary.Web;

namespace Finance.PIS.Masters
{
    public partial class PromotionTypes : PageBase
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
            gvPromotionTypes.InsertRowsCount = 1;
            btnInsert.Visible = false;
        }

        #endregion

        #region Inserted, Updated,deleted

        /// <summary>
        /// This method use for following purpose
        /// 1.set status message.
        /// 2.catch exception from database and show exception message.
        /// </summary>
        int _promotionTypeId = -1;
        protected void dsPromotionTypes_Inserted(object sender, LinqDataSourceStatusEventArgs e)
        {
            if (e.Exception == null)
            {
                btnInsert.Visible = true;
                _promotionTypeId = GetPromotionTypeId(e.Result);
                PromotionTypes_sp.AddStatusText("Promotion Type inserted successfully");
            }
            else
            {
                PromotionTypes_sp.AddErrorText(e.Exception.Message);
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
        protected void dsPromotionTypes_Updated(object sender, LinqDataSourceStatusEventArgs e)
        {
            if (e.Exception == null)
            {
                _promotionTypeId = GetPromotionTypeId(e.Result);
                PromotionTypes_sp.AddStatusText("Promotion Type updated successfully");
            }
            else
            {
                PromotionTypes_sp.AddErrorText(e.Exception.Message);
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
        protected void dsPromotionTypes_Deleted(object sender, LinqDataSourceStatusEventArgs e)
        {
            if (e.Exception == null)
            {
                PromotionTypes_sp.AddStatusText("Promotion Type deleted successfully");
            }
            else
            {
                PromotionTypes_sp.AddErrorText(e.Exception.Message);
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
        protected void gvPromotionTypes_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            switch (e.Row.RowType)
            {
                case DataControlRowType.DataRow:
                    if (_promotionTypeId != -1)
                    {
                        int id = GetPromotionTypeId(e.Row.DataItem);
                        if (id == _promotionTypeId)
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

        private int GetPromotionTypeId(object dataItem)
        {
            PromotionType promotionTypeResult = (PromotionType)dataItem;
            return promotionTypeResult.PromotionTypeId;
        }

        #endregion

    }
}
