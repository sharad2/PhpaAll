using Eclipse.PhpaLibrary.Database;
using Eclipse.PhpaLibrary.Reporting;
using Eclipse.PhpaLibrary.Web;
using EclipseLibrary.Web.JQuery;
using EclipseLibrary.Web.JQuery.Input;
using System;
using System.Linq;
using System.Web.UI.WebControls;

namespace Finance.Finance
{
    public partial class CloseFinancialYear : PageBase
    {
        protected void dsFiscalYear_Selecting(object sender, System.Web.UI.WebControls.LinqDataSourceSelectEventArgs e)
        {
            using (FiscalDataContext db = new FiscalDataContext(ReportingUtilities.DefaultConnectString))
            {
                var query = (from fy in db.FinancialYears
                             orderby fy.StartDate descending
                             select new
                             {
                                 YearId = fy.YearId,
                                 Name = fy.Name,
                                 StartDate = fy.StartDate,
                                 EndDate = fy.EndDate,
                                 Freeze = fy.Freeze
                             }).ToList();

                e.Result = query;
            }

        }

    }
}