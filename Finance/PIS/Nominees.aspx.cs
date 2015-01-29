using System;
using System.Data.Linq;
using System.Web.UI.WebControls;
using Eclipse.PhpaLibrary.Database.PIS;
using Eclipse.PhpaLibrary.Reporting;
using Eclipse.PhpaLibrary.Web;
using EclipseLibrary.Web.JQuery;
using EclipseLibrary.Web.JQuery.Input;

namespace PhpaAll.PIS
{
    public partial class Nominees : PageBase
    {
       
        protected void dsNominees_ContextCreated(object sender, LinqDataSourceStatusEventArgs e)
        {
            DataContext db = (DataContext)e.Result;
            DataLoadOptions dlo = new DataLoadOptions();
            dlo.LoadWith<Nominee>(p => p.FamilyMember);
            dlo.LoadWith<Nominee>(p => p.EntitlementType);
            db.LoadOptions = dlo;
        }

        protected void dsNominees_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            if (e.WhereParameters["EmployeeId"] == null)
            {
                e.Cancel = true;
            }
        }       

        int _nomineeId = -1;
        protected void btnInsert_Click(object sender, EventArgs e)
        {
            gvNominees.InsertRowsCount = 1;
            formNominees_btnInsert.Visible = false;                     
        }

        private int GetNomineeId(object dataItem)
        {
            Nominee nomineeResult = (Nominee)dataItem;
            return nomineeResult.NomineeId;
        }
       

        protected void gvNominees_RowInserting(object sender, GridViewInsertingEventArgs e)
        {
            GridViewRow row = gvNominees.Rows[e.RowIndex];
            string entitlementTypeId = (string)e.Values["EntitlementTypeId"];
            if (string.IsNullOrEmpty(entitlementTypeId))
            {
                using (PISDataContext db = new PISDataContext(ReportingUtilities.DefaultConnectString))
                {
                    DropDownSuggest ddlEntitlementType = (DropDownSuggest)row.FindControl("ddlEntitlementType");
                    EntitlementType entType = new EntitlementType()
                    {
                        EntitlementDescription = ddlEntitlementType.TextBox.Text
                    };

                    db.EntitlementTypes.InsertOnSubmit(entType);
                    db.SubmitChanges();
                    e.Values["EntitlementTypeId"] = entType.EntitlementTypeId;
                }
            }           
        }

        protected void dsNominees_Inserted(object sender, LinqDataSourceStatusEventArgs e)
        {
            if (e.Exception == null)
            {
                formNominees_btnInsert.Visible = true;
                _nomineeId = GetNomineeId(e.Result);
                Nominees_sp.AddStatusText("Item inserted successfully");
            }
            else
            {
                Nominees_sp.AddErrorText(e.Exception.Message);
                e.ExceptionHandled = true;
            }
        }

        protected void dsNominees_Updated(object sender, LinqDataSourceStatusEventArgs e)
        {
            if (e.Exception == null)
            {
                _nomineeId = GetNomineeId(e.Result);
                Nominees_sp.AddStatusText("Item updated successfully");
            }
            else
            {
                Nominees_sp.AddErrorText(e.Exception.Message);
                e.ExceptionHandled = true;
            }
        }

        protected void dsNominees_Deleted(object sender, LinqDataSourceStatusEventArgs e)
        {
            if (e.Exception == null)
            {
                Nominees_sp.AddStatusText("Item deleted successfully");
            }
            else
            {
                Nominees_sp.AddErrorText(e.Exception.Message);
                e.ExceptionHandled = true;
            }
        }

        protected void gvNominees_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            switch (e.Row.RowType)
            {
                case DataControlRowType.DataRow:
                    if (_nomineeId != -1)
                    {
                       int id = GetNomineeId(e.Row.DataItem);
                       if (id == _nomineeId)
                       {
                           e.Row.RowState |= DataControlRowState.Selected;
                       }
                    }
                    break;
                
                default:
                    break;
            }
        }


    }
}
