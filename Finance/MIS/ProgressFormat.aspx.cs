using System;
using System.Data.Linq;
using System.Linq;
using System.Web.Script.Serialization;
using System.Web.UI.WebControls;
using Eclipse.PhpaLibrary.Database.MIS;
using Eclipse.PhpaLibrary.Reporting;
using Eclipse.PhpaLibrary.Web;
using EclipseLibrary.Web.JQuery.Input;
using EclipseLibrary.Web.UI;
namespace PhpaAll.MIS
{
    /// <summary>
    /// Query String :ProgressFormatId.
    /// Button with ID btnFormat can be clicked to submit the form for update action
    /// </summary>
    public partial class AddNewFormat : PageBase
    {
        /// <summary>
        /// This method use for following purpose
        /// 1.Open formview in edit or insert depaned on query string. 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnLoad(EventArgs e)
        {
            if (!string.IsNullOrEmpty(this.Request.QueryString["ProgressFormatId"]))
            {
                fv.DefaultMode = FormViewMode.Edit;
            }
            else
            {
                fv.DefaultMode = FormViewMode.Insert;
            }

            base.OnLoad(e);
        }

        protected void ddlFormatCategory_Load(object sender, EventArgs e)
        {
            RadioButtonListEx rbl = (RadioButtonListEx)sender;
            using (MISDataContext db = new MISDataContext(ReportingUtilities.DefaultConnectString))
            {
                var query = from cat in db.FormatCategories
                            select cat;
                foreach (var item in query)
                {

                    rbl.Items.Add(new RadioItem() { Text = item.Description, Value = item.FormatCategoryId.ToString() });
                }
            }
        }

        /// <summary>
        /// This method use for following purpose
        /// 1.select subpackage in dropdownlist. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void fv_Created(object sender, EventArgs e)
        {

            switch (fv.CurrentMode)
            {
                case FormViewMode.Edit:
                    DropDownListEx ddlPackages = (DropDownListEx)fv.FindControl("ddlPackages");
                    using (MISDataContext db = new MISDataContext(ReportingUtilities.DefaultConnectString))
                    {
                        DataLoadOptions dlo = new DataLoadOptions();                      
                        dlo.LoadWith<ProgressFormat>(p => p.Package);
                        ProgressFormat pf = db.ProgressFormats.Where(p => p.ProgressFormatId == Convert.ToInt32(fv.DataKey.Value))
                              .Single();
                        ddlPackages.Value = pf.PackageId.ToString();                     
                    }
                    break;
                case FormViewMode.Insert:                 
                    break;

                case FormViewMode.ReadOnly:
                    break;
                default:
                    break;
            }
        }
        /// <summary>
        /// This method use for following purpose
        /// 1.Bind progressformat dropdownlist.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlProgressFormatType_Load(object sender, EventArgs e)
        {
            DropDownListEx ddlProgressFormatType = (DropDownListEx)sender;
            foreach (int i in Enum.GetValues(typeof(ProgressFormat.FormatType)))
            {
                DropDownItem item = new DropDownItem()
                {
                    Text = Enum.GetName(typeof(ProgressFormat.FormatType), i),
                    Value = i.ToString(),
                    Persistent = DropDownPersistenceType.Always
                };
                ddlProgressFormatType.Items.Add(item);
            }

            base.OnLoad(e);
        }

        /// <summary>
        /// This method use for following purpose
        /// 1.Insert or update progress format.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 
        bool _Isexpception = true;
        protected void btn_Click(object sender, EventArgs e)
        {
            ButtonEx btn = (ButtonEx)sender;
            if (!btn.IsPageValid())
            {
                return;
            }
            switch (fv.CurrentMode)
            {
                case FormViewMode.Edit:
                    fv.UpdateItem(true);
                    break;
                case FormViewMode.Insert:
                    fv.InsertItem(true);
                    break;
                case FormViewMode.ReadOnly:
                    break;
                default:
                    break;
            }
            if (_Isexpception)
            {
                this.Response.ContentType = "application/json";
                JavaScriptSerializer ser = new JavaScriptSerializer();
                this.Response.StatusCode = 205;
                this.Response.End();
            }
        }


        /// <summary>
        /// This method use for following purpose.
        /// 1.show status message.
        /// 2.catch exception from database and show exception message. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ds_Inserted(object sender, LinqDataSourceStatusEventArgs e)
        {
            if (e.Exception == null)
            {
                _Isexpception = true;
                // Do nothing.
            }

            else
            {
                _Isexpception = false;
                sp_status.AddErrorText(e.Exception.Message);
                e.ExceptionHandled = true;
            }
        }
        /// <summary>
        /// this method use for following purpose
        /// 1.show status message.
        /// 2.catch exception from database and show exception message.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ds_Updated(object sender, LinqDataSourceStatusEventArgs e)
        {
            if (e.Exception == null)
            {
                //Do nothing.
            }
            else
            {
                sp_status.AddErrorText(e.Exception.Message);
                e.ExceptionHandled = true;
            }
        }      

    }
}
