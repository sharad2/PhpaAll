using System;
using System.Web.Script.Serialization;
using System.Web.UI.WebControls;
using Eclipse.PhpaLibrary.Web;
using EclipseLibrary.Web.JQuery.Input;
namespace PhpaAll.MIS
{
    /// <summary>
    /// To edit an activity, pass ActivityID.   
    /// Button with ID btn can be clicked to submit the form for Insert or update action
    /// </summary>
    public partial class AddNewActivity : PageBase
    {
        protected override void OnLoad(EventArgs e)
        {
            if (!string.IsNullOrEmpty(this.Request.QueryString["ActivityID"]))
            {
                fv.DefaultMode = FormViewMode.Edit;
            }
            else
            {
                fv.DefaultMode = FormViewMode.Insert;
            }

            base.OnLoad(e);
        }

        

        /// <summary>
        /// This method use for following purpose
        /// 1.Insert a new activity.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                default:
                    break;
            }
            this.Response.ContentType = "application/json";
            JavaScriptSerializer ser = new JavaScriptSerializer();
            this.Response.StatusCode = 205;
            this.Response.End();
        }


        /// <summary>
        /// This method use for following purpose
        /// 1.set status message
        /// 2.Catch exception from database and show exception message. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>     
        protected void ds_Inserted(object sender, LinqDataSourceStatusEventArgs e)
        {
            if (e.Exception == null)
            {
                //do Nothing.
            }
            else
            {
                sp_Status.AddErrorText(e.Exception.Message);
                e.ExceptionHandled = true;
            }
        }

        protected void ds_Updated(object senser, LinqDataSourceStatusEventArgs e)
        {
            if (e.Exception == null)
            {
                //do Nothing.
            }
            else
            {
                sp_Status.AddErrorText(e.Exception.Message);
                e.ExceptionHandled = true;
            }
        }


    }
}
