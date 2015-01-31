using System;
using System.Data.Linq;
using System.Web.UI.WebControls;
using Eclipse.PhpaLibrary.Database.PIS;
using Eclipse.PhpaLibrary.Web;

namespace PhpaAll.PIS
{
    public partial class Qualifications : PageBase
    {
        protected void dsQualifications_ContextCreated(object sender, LinqDataSourceStatusEventArgs e)
        {
            DataContext db = (DataContext)e.Result;
            DataLoadOptions dlo = new DataLoadOptions();
            dlo.LoadWith<Qualification>(p => p.QualificationDivision);
            dlo.LoadWith<Qualification>(p => p.Country);
            db.LoadOptions = dlo;
        }

        protected void dsQualifications_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            if (e.WhereParameters["EmployeeId"] == null)
            {
                e.Cancel = true;
            }
        }

        protected void dsQualifications_Deleted(object sender, LinqDataSourceStatusEventArgs e)
        {
            if (e.Exception == null)
            {
                Qualifications_sp.AddStatusText("Item deleted successfully");
            }
            else
            {
                Qualifications_sp.AddErrorText(e.Exception.Message);
                e.ExceptionHandled = true;
            }
        }

        protected void _dlgAddEditQualification_PreRender(object sender, EventArgs e)
        {
            _dlgAddEditQualification.Ajax.Url += string.Format("?EmployeeId={0}", this.Request.QueryString["EmployeeId"]);
        }

    }
}
