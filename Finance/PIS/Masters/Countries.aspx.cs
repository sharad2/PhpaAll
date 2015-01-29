using System;
using System.Web.UI.WebControls;
using Eclipse.PhpaLibrary.Database.PIS;
using Eclipse.PhpaLibrary.Web;

namespace PhpaAll.PIS.Masters
{
    public partial class Countries : PageBase
    {
        #region Button Insert

        /// <summary>
        /// This method is used to set insert row count.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnInsert_Click(object sender, EventArgs e)
        {
            gvCountries.InsertRowsCount = 1;
            btnInsert.Visible = false;
        }

        #endregion

        #region Inserted, Updated, Deleted Events

        /// <summary>
        /// This method is used for following purpose
        /// 1.Set status message.
        /// 2.catch exception from database and show exception message.
        /// </summary>
        int _countryId = -1;
        protected void dsCountries_Inserted(object sender, LinqDataSourceStatusEventArgs e)
        {
            if (e.Exception == null)
            {
                btnInsert.Visible = true;
                _countryId = GetCountryId(e.Result);
                Countries_sp.AddStatusText("Country inserted successfully");
            }
            else
            {
                Countries_sp.AddErrorText(e.Exception.Message);
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
        protected void dsCountries_Updated(object sender, LinqDataSourceStatusEventArgs e)
        {
            if (e.Exception == null)
            {
                _countryId = GetCountryId(e.Result);
                Countries_sp.AddStatusText("Country updated successfully");
            }
            else
            {
                Countries_sp.AddErrorText(e.Exception.Message);
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
        protected void dsCountries_Deleted(object sender, LinqDataSourceStatusEventArgs e)
        {
            if (e.Exception == null)
            {
                Countries_sp.AddStatusText("Country deleted successfully");
            }
            else
            {
                Countries_sp.AddErrorText(e.Exception.Message);
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
        protected void gvCountries_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            switch (e.Row.RowType)
            {
                case DataControlRowType.DataRow:
                    if (_countryId != -1)
                    {
                        int id = GetCountryId(e.Row.DataItem);
                        if (id == _countryId)
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

        private int GetCountryId(object dataItem)
        {
            Country countryResult = (Country)dataItem;
            return countryResult.CountryId;
        }

        #endregion

    }
}
