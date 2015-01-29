using Eclipse.PhpaLibrary.Database;
using Eclipse.PhpaLibrary.Reporting;
using Eclipse.PhpaLibrary.Web;
using EclipseLibrary.Web.JQuery;
using EclipseLibrary.Web.JQuery.Input;
using System;
using System.Linq;
using System.Web.UI.WebControls;

namespace PhpaAll.Finance
{
    public partial class ManageFinancialYears : PageBase
    {
        //protected void dsFiscalYear_Selecting(object sender, System.Web.UI.WebControls.LinqDataSourceSelectEventArgs e)
        //{
        //    //using (FiscalDataContext db = new FiscalDataContext(ReportingUtilities.DefaultConnectString))
        //    //{
        //    //    var query = (from fy in db.FinancialYears
        //    //                 select new
        //    //                 {
        //    //                     YearId = fy.YearId,
        //    //                     Name = fy.Name,
        //    //                     StartDate = fy.StartDate,
        //    //                     EndDate = fy.EndDate,
        //    //                     Freeze = fy.Freeze
        //    //                 }).ToList();

        //    //    e.Result = query;
        //    //}

        //}

        protected void btnNewFiscalYear_Click(object sender, EventArgs e)
        {
            gvFiscalYear.InsertRowsCount = 1;
        }

        protected void gvFiscalYear_RowInserting(object sender, GridViewInsertingEventArgs e)
        {
            GridViewExInsert gv = (GridViewExInsert)sender;
            GridViewRow row = gv.Rows[e.RowIndex];
            TextBoxEx tbFYStartDate = (TextBoxEx)row.FindControl("tbFYStartDate");
            //TextBoxEx tbFreeze = (TextBoxEx)row.FindControl("tbFreeze");
            DateTime fiscalYear = tbFYStartDate.ValueAsDate.Value;
            e.Values["Name"] = fiscalYear.Year.ToString(); 
        }

    }
}