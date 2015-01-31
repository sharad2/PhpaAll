using System;
using System.Linq;
using System.Web.UI.WebControls;
using Eclipse.PhpaLibrary.Database.PIS;
using Eclipse.PhpaLibrary.Web;

namespace PhpaAll.PIS.Reports
{
    public partial class ProbationEnding : PageBase
    {
        protected void dsProbation_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            PISDataContext db = (PISDataContext)dsProbation.Database;
            IQueryable<Employee> query = db.Employees
                .Where(p => p.ProbationEndDate != null);

            if (dtFrom.ValueAsDate.HasValue)
            {
                query = query.Where(p => p.ProbationEndDate >= Convert.ToDateTime(dtFrom.Text));
            }

            if (dtTo.ValueAsDate.HasValue)
            {
                query = query.Where(p => p.ProbationEndDate <= Convert.ToDateTime(dtTo.Text));
            }

            e.Result = query;
        }
    }
}
