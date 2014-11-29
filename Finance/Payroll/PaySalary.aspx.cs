using System;
using System.Linq;
using System.Web.UI.WebControls;
using Eclipse.PhpaLibrary.Database.Payroll;
using Eclipse.PhpaLibrary.Web;
using EclipseLibrary.Web.JQuery.Input;

namespace Finance.Payroll
{
    public partial class PaySalary :PageBase
    {
        private int m_PeriodId;
        private string m_UpdateMassasge;
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (Request.QueryString["Periodid"] != null)
                {
                    m_PeriodId = Convert.ToInt32(Request.QueryString["Periodid"]);
                    sdPayBill.SalaryPeriodId = m_PeriodId;
                }
      
            sdPayBill.DataBind();
        }        

       
        protected void frmSalaryPeriod_ItemUpdated(object sender, FormViewUpdatedEventArgs e)
        {
            if (e.Exception == null)
            {
                e.KeepInEditMode = false;               
                frmSalaryPeriod.DefaultMode = FormViewMode.ReadOnly;
                m_UpdateMassasge = "Period Paid";
            }
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            LinkButtonEx btn = (LinkButtonEx)sender;
            if (btn.IsPageValid())
            {
                frmSalaryPeriod.UpdateItem(false);
            }
        }

        protected void dsSalaryPeriod_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            if (e.WhereParameters["SalaryPeriodId"] == null)
            {
                e.Cancel = true;
            }
        }

        protected void CustomValidPaidDate_ServerValidate(object source, EclipseLibrary.Web.JQuery.Input.ServerValidateEventArgs e)
        {
            TextBoxEx tbPaidDate = (TextBoxEx)source;
            int spId = (int)frmSalaryPeriod.SelectedValue;
            PayrollDataContext db = (PayrollDataContext)dsSalaryPeriod.Database;
          
            SalaryPeriod salPeriod = (from sp in db.SalaryPeriods
                               where sp.SalaryPeriodId==spId
                               select sp).SingleOrDefault();

            if (salPeriod.PayableDate.HasValue)
            {
                e.ControlToValidate.IsValid = Convert.ToDateTime(tbPaidDate.ValueAsDate) >= salPeriod.PayableDate;
                e.ControlToValidate.ErrorMessage = "Paid Date must be greater than or equal to Payable Date.";
                
            }
            else
            {
                e.ControlToValidate.IsValid = Convert.ToDateTime(tbPaidDate.ValueAsDate) >= salPeriod.SalaryPeriodEnd;
                e.ControlToValidate.ErrorMessage = "Paid Date must be greater than or equal to End To date.";
            }
           
        }

        protected void frmSalaryPeriod_ItemCreated(object sender, EventArgs e)
        {
            Label lbl = (Label)frmSalaryPeriod.FindControl("lblUpdateMsg");
            if (m_UpdateMassasge != null)
                lbl.Text = m_UpdateMassasge;
        }
    }
}
