using System;
using System.Web.UI.WebControls;
using Eclipse.PhpaLibrary.Database.PIS;
using Eclipse.PhpaLibrary.Web;


namespace PhpaAll.PIS.Masters
{
    public partial class Banks : PageBase
    {
        #region Insert Button

        /// <summary>
        /// This method use for following purpose
        /// 1.Set insert row count.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnInsert_Click(object sender, EventArgs e)
        {
            
            gvBanks.InsertRowsCount = 1;
            btnInsert.Visible = false;
        }

        #endregion

        #region Inserted,Updated,Deleted

        /// <summary>
        /// This method use for following purpose
        /// 1.set status message.
        /// 2.catch exception from database and show exception message.
        /// </summary>
        int _bankId = -1;

        protected void dsBanks_Inserted(object sender, LinqDataSourceStatusEventArgs e)
        {
            
            if (e.Exception == null)
            {
                btnInsert.Visible = true;
                _bankId = GetBankId(e.Result);
                Banks_sp.AddStatusText("Bank inserted successfully");
            }
            else
            {
                Banks_sp.AddErrorText(e.Exception.Message);
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
        protected void dsBanks_Updated(object sender, LinqDataSourceStatusEventArgs e)
        {
            if (e.Exception == null)
            {
                _bankId = GetBankId(e.Result);
                Banks_sp.AddStatusText("Bank updated successfully");
            }
            else
            {
                Banks_sp.AddErrorText(e.Exception.Message);
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
        protected void dsBanks_Deleted(object sender, LinqDataSourceStatusEventArgs e)
        {
            if (e.Exception == null)
            {
                Banks_sp.AddStatusText("Bank deleted successfully");
            }
            else
            {
                Banks_sp.AddErrorText(e.Exception.Message);
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
        protected void gvBanks_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            switch (e.Row.RowType)
            {
                case DataControlRowType.DataRow:
                    if (_bankId != -1)
                    {
                        int id = GetBankId(e.Row.DataItem);
                        if (id == _bankId)
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

        private int GetBankId(object dataItem)
        {
            Bank bankResult = (Bank)dataItem;
            return bankResult.BankId;
        }

        #endregion
    }
}