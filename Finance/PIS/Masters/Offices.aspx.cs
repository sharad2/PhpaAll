using System;
using System.Data.Linq;
using System.Linq;
using System.Web.Services;
using System.Web.UI.WebControls;
using Eclipse.PhpaLibrary.Database.PIS;
using Eclipse.PhpaLibrary.Reporting;
using Eclipse.PhpaLibrary.Web;
using EclipseLibrary.Web.JQuery;
using EclipseLibrary.Web.JQuery.Input;
using EclipseLibrary.Web.UI;

namespace PhpaAll.PIS.Masters
{
    public partial class Offices : PageBase
    {
        #region Context

        protected void dsOffices_ContextCreated(object sender, LinqDataSourceStatusEventArgs e)
        {
            DataContext db = (DataContext)e.Result;
            DataLoadOptions dlo = new DataLoadOptions();
            dlo.LoadWith<Office>(p => p.SubDivision);
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
            gvOffices.InsertRowsCount = 1;
            btnInsert.Visible = false;
        }

        #endregion

        #region Insertion

        /// <summary>
        /// Find ddlSubDivision for inserting
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvOffices_RowInserting(object sender, GridViewInsertingEventArgs e)
        {
            GridViewRow row = gvOffices.Rows[e.RowIndex];

            using (PISDataContext db = new PISDataContext(ReportingUtilities.DefaultConnectString))
            {
                DropDownListEx ddlSubDivision = (DropDownListEx)row.FindControl("ddlSubDivision");
                e.Values["SubDivisionId"] = ddlSubDivision.Value;
            }
        }

        /// <summary>
        /// This method use for following purpose
        /// 1.Set status message.       
        /// </summary>
        int _officeId = -1;
        protected void dsOffices_Inserted(object sender, LinqDataSourceStatusEventArgs e)
        {
            if (e.Exception == null)
            {
                btnInsert.Visible = true;
                _officeId = GetOfficeId(e.Result);
                Offices_sp.AddStatusText("Office inserted successfully");
            }

        }

        /// <summary>
        /// Catch exception from database and show exception message.
        /// Aso keep grid in insert mode in case of exception
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvOffices_RowInserted(object sender, GridViewInsertedEventArgs e)
        {
            if (e.Exception != null)
            {
                Offices_sp.AddErrorText(e.Exception.Message);
                e.ExceptionHandled = true;
                e.KeepInInsertMode = true;
            }
        }

        #endregion

        #region Updation

        /// <summary>
        /// Find SubDivision for updation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvOffices_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            GridViewRow row = gvOffices.Rows[e.RowIndex];

            using (PISDataContext db = new PISDataContext(ReportingUtilities.DefaultConnectString))
            {
                DropDownListEx ddlSubDivision = (DropDownListEx)row.FindControl("ddlSubDivision");
                e.NewValues["SubDivisionId"] = ddlSubDivision.Value;
            }
        }

        /// <summary>
        /// This method use for following purpose
        /// 1.Set status message
        /// 2.Catch exception from database and show exception message.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dsOffices_Updated(object sender, LinqDataSourceStatusEventArgs e)
        {
            if (e.Exception == null)
            {
                _officeId = GetOfficeId(e.Result);
                Offices_sp.AddStatusText("Office updated successfully");
            }
            else
            {
                Offices_sp.AddErrorText(e.Exception.Message);
                e.ExceptionHandled = true;
            }
        }

        #endregion

        #region Deletion

        /// <summary>
        /// This method use for following purpose
        /// 1.Set status message
        /// 2.Catch exception from database and show exception message. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dsOffices_Deleted(object sender, LinqDataSourceStatusEventArgs e)
        {
            if (e.Exception == null)
            {
                Offices_sp.AddStatusText("Office deleted successfully");
            }
            else
            {
                Offices_sp.AddErrorText(e.Exception.Message);
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
        protected void gvOffices_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            switch (e.Row.RowType)
            {
                case DataControlRowType.DataRow:

                    //If the row is in edit state find the SubDivision and Division and 
                    //set the their Value
                    if ((e.Row.RowState & DataControlRowState.Edit) == DataControlRowState.Edit)
                    {
                        DropDownListEx ddlDivision = (DropDownListEx)e.Row.FindControl("ddlDivision");
                        DropDownListEx ddlSubDivision = (DropDownListEx)e.Row.FindControl("ddlSubDivision");

                        using (PISDataContext db = new PISDataContext(ReportingUtilities.DefaultConnectString))
                        {
                            Office office = db.Offices
                                .Where(p => p.OfficeId == Convert.ToInt32(gvOffices.DataKeys[e.Row.RowIndex].Value))
                                .Single();

                            ddlSubDivision.Value = office.SubDivisionId.ToString();

                            SubDivision subDivision = db.SubDivisions
                                .Where(p => p.SubDivisionId == Convert.ToInt32(ddlSubDivision.Value))
                                .Single();

                            ddlDivision.Value = subDivision.DivisionId.ToString();

                            ddlSubDivision.Items.Add(new DropDownItem()
                            {
                                Text = office.SubDivision.SubDivisionName.ToString(),
                                Value = office.SubDivisionId.ToString(),
                                Persistent = DropDownPersistenceType.WhenEmpty
                            });
                        }
                    }

                    if (_officeId != -1)
                    {
                        int id = GetOfficeId(e.Row.DataItem);
                        if (id == _officeId)
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

        private int GetOfficeId(object dataItem)
        {
            Office officeResult = (Office)dataItem;
            return officeResult.OfficeId;
        }

        #endregion

        #region WebMethods

        [WebMethod]
        public static DropDownItem[] GetSubDivisions(string[] parentKeys)
        {
            using (PISDataContext db = new PISDataContext(ReportingUtilities.DefaultConnectString))
            {
                var query = (from subDivision in db.SubDivisions
                             where subDivision.DivisionId == int.Parse(parentKeys[0])
                             select new DropDownItem()
                             {
                                 Text = subDivision.SubDivisionName,
                                 Value = subDivision.SubDivisionId.ToString()
                             }).ToArray();


                return query;
            }
        }

        #endregion

    }
}
