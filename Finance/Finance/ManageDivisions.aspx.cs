/*
 *  Programming: Eclipse Systems (P) Ltd., NOIDA, INDIA
 *  E-mail: support@eclsys.com
 *
 *  $Workfile:   ManageDivisions.aspx.cs  $
 *  $Revision: 37408 $
 *  $Author: ssinghal $
 *  $Date: 2010-11-18 11:35:22 +0530 (Thu, 18 Nov 2010) $
 *  $Modtime:   Jul 17 2008 12:15:42  $
 *
 *  $Log:   S:/Projects/PHPA2/archives/Finance/Finance/ManageDivisions.aspx.cs-arc  $
 * 
 *    Rev 1.56   Jul 17 2008 13:13:16   ssinghal
 * WIP
 * 
 *    Rev 1.55   Jul 17 2008 12:19:22   ssinghal
 * WIP
 * 
 *    Rev 1.54   Jul 17 2008 11:13:30   ssinghal
 * WIP
 * 
 *    Rev 1.53   Jul 17 2008 11:03:26   ssinghal
 * WIP
 * 
 *    Rev 1.52   Jul 17 2008 10:52:16   ssinghal
 * WIP
 * 
 *    Rev 1.51   Jul 16 2008 10:12:32   ssinghal
 * WIP
 * 
 *    Rev 1.50   Jul 16 2008 10:02:00   ssinghal
 * WIP
 * 
 *    Rev 1.49   Jul 09 2008 17:33:52   vraturi
 * PVSC Template Added.
 */
using System;
using System.Linq;
using System.Web.UI.WebControls;
using Eclipse.PhpaLibrary.Reporting;
using Eclipse.PhpaLibrary.Web;
using EclipseLibrary.Web.JQuery;
using EclipseLibrary.Web.JQuery.Input;

namespace PhpaAll.Finance
{
    public partial class ManageDivisions : PageBase
    {
        private ReportingDataContext _dbDivisionGroup;
        protected void dsDivisionGroup_Load(object sender, EventArgs e)
        {
            PhpaLinqDataSource ds = (PhpaLinqDataSource)sender;
            _dbDivisionGroup = (ReportingDataContext)ds.Database;
        }

        protected void dsDivisionGroup_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            e.Result = (from d in _dbDivisionGroup.RoDivisions
                        orderby d.DivisionGroup
                        select new
                        {
                            DivisionGroup = d.DivisionGroup
                        }).Distinct();
                       
        }

        protected void gvDivision_RowInserting(object sender, GridViewInsertingEventArgs e)
        {
            GridViewExInsert gv = (GridViewExInsert)sender;
            GridViewRow row = gv.Rows[e.RowIndex];
            DropDownListEx ddlDivision = (DropDownListEx)row.FindControl("ddlDivisionGroup");
            TextBoxEx tbDivisionGroup = (TextBoxEx)row.FindControl("tbDivisionGroup");
            if (string.IsNullOrEmpty(ddlDivision.Value))
            {
                e.Values["DivisionGroup"] = tbDivisionGroup.Value;
            }
        }
        
        protected void gvDivision_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            GridViewExInsert gv = (GridViewExInsert)sender;
            GridViewRow row = gv.Rows[e.RowIndex];
            DropDownListEx ddlDivision = (DropDownListEx)row.FindControl("ddlDivisionGroup");
            TextBoxEx tbDivisionGroup = (TextBoxEx)row.FindControl("tbDivisionGroup");
            if (string.IsNullOrEmpty(ddlDivision.Value))
            {
                e.NewValues["DivisionGroup"] = tbDivisionGroup.Value;
            }
        }

        protected void btnNew_Click(object sender, EventArgs e)
        {
            gvDivisions.InsertRowsCount = 1;
        }

        protected void gvDivisions_RowDeleted(object sender, GridViewDeletedEventArgs e)
        {
            if (e.Exception != null)
            {
                valSummary.ErrorMessages.Add(string.Format("{0} cannot be deleted because it is in use. Error is: {1}", e.Keys["DivisionName"], e.Exception.Message));
                e.ExceptionHandled = true;
            }
        }
    }
}
