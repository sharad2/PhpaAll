using System;
using System.Web.UI.WebControls;
using Eclipse.PhpaLibrary.Database.PIS;
using Eclipse.PhpaLibrary.Web;


namespace PhpaAll.PIS.Masters
{
    public partial class Stations : PageBase
    {
        #region Button Insert
        /// <summary>
        /// This method is used to set insert row count.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnInsert_Click(object sender, EventArgs e)
        {
            gvStations.InsertRowsCount = 1;
            btnInsert.Visible = false;
        }
        #endregion
        #region Inserted, Updated, Deleted Events

        /// <summary>
        /// This method is used for following purpose
        /// 1.Set status message.
        /// 2.catch exception from database and show exception message.
        /// </summary>
        int _stationId = -1;
        protected void dsStations_Inserted(object sender, LinqDataSourceStatusEventArgs e)
        {
            if (e.Exception == null)
            {
                btnInsert.Visible = true;
                _stationId = GetStationId(e.Result);
                Stations_sp.AddStatusText("Station inserted successfully");
            }
            else
            {
                Stations_sp.AddErrorText(e.Exception.Message);
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
        protected void dsStations_Updated(object sender, LinqDataSourceStatusEventArgs e)
        {
            if (e.Exception == null)
            {
                _stationId = GetStationId(e.Result);
                Stations_sp.AddStatusText("Station updated successfully");
            }
            else
            {
                Stations_sp.AddErrorText(e.Exception.Message);
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
        protected void dsStations_Deleted(object sender, LinqDataSourceStatusEventArgs e)
        {
            if (e.Exception == null)
            {
                Stations_sp.AddStatusText("Station deleted successfully");
            }
            else
            {
                Stations_sp.AddErrorText(e.Exception.Message);
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
        protected void gvStations_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            switch (e.Row.RowType)
            {
                case DataControlRowType.DataRow:
                    if (_stationId != -1)
                    {
                        int id = GetStationId(e.Row.DataItem);
                        if (id == _stationId)
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

        private int GetStationId(object dataItem)
        {
            Station stationResult = (Station)dataItem;
            return stationResult.StationId;
        }

        #endregion
    }
}