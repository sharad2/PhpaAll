using System;
using System.Data.Linq;
using System.Web.UI.WebControls;
using Eclipse.PhpaLibrary.Database.PIS;
using Eclipse.PhpaLibrary.Web;
using EclipseLibrary.Web.JQuery.Input;

namespace PhpaAll.PIS
{
    public partial class Trainings : PageBase
    {

        protected void dsTraining_ContextCreated(object sender, LinqDataSourceStatusEventArgs e)
        {
            DataContext db = (DataContext)e.Result;
            DataLoadOptions dlo = new DataLoadOptions();
            dlo.LoadWith<Training>(p => p.TrainingType);
            dlo.LoadWith<Training>(p => p.Country);
            db.LoadOptions = dlo;

        }

        protected void dsTraining_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            if (e.WhereParameters["EmployeeId"] == null)
            {
                e.Cancel = true;
            }
        }

        protected void dsServicePeriod_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            if (e.WhereParameters["EmployeeId"] == null)
            {
                e.Cancel = true;
            }
        }

        protected void dsTraining_Deleted(object sender, LinqDataSourceStatusEventArgs e)
        {
            if (e.Exception == null)
            {
                Trainings_sp.AddStatusText("Item deleted successfully");
            }
            else
            {
                Trainings_sp.AddErrorText(e.Exception.Message);
                e.ExceptionHandled = true;
            }
        }

        protected void ddlServicePeriod_PreRender(object sender, EventArgs e)
        {
            DropDownListEx ddlServicePeriod = (DropDownListEx)sender;
        }

        protected void _dlgAddEditTraining_PreRender(object sender, EventArgs e)
        {
            _dlgAddEditTraining.Ajax.Url += string.Format("?EmployeeId={0}", this.Request.QueryString["EmployeeId"]);
        }


    }
}
