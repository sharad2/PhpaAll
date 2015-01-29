/*
 *  Programming: Eclipse Systems (P) Ltd., NOIDA, INDIA
 *  E-mail: support@eclsys.com
 *
 *  $Workfile:   ManageUsers.aspx.cs  $
 *  $Revision: 39807 $
 *  $Author: ssingh $
 *  $Date: 2011-04-26 11:51:19 +0530 (Tue, 26 Apr 2011) $
 *  $Modtime:   Jul 21 2008 19:11:00  $
 *
 *  $Log:   S:/Projects/PHPA2/archives/Finance/Admin/ManageUsers.aspx.cs-arc  $
 * 
 *    Rev 1.17   Jul 22 2008 13:07:28   dbhatt
 * wip
 * 
 *    Rev 1.16   Jul 21 2008 21:11:30   dbhatt
 * wip
 * 
 *    Rev 1.15   Jul 21 2008 20:21:52   dbhatt
 * wip
 * 
 *    Rev 1.14   Jul 21 2008 20:16:14   dbhatt
 * wip
 * 
 *    Rev 1.13   Jul 21 2008 20:07:44   dbhatt
 * wip
 * 
 *    Rev 1.12   Jul 21 2008 18:32:50   dbhatt
 * wip
 * 
 *    Rev 1.11   Jul 21 2008 17:53:10   dbhatt
 * wip
 * 
 *    Rev 1.10   Jul 21 2008 16:36:38   dbhatt
 * wip
 * 
 *    Rev 1.9   Jul 21 2008 15:35:16   dbhatt
 * wip
 * 
 *    Rev 1.8   Jul 09 2008 17:58:38   vraturi
 * PVCS Template Added.
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Security;
using System.Web.UI.WebControls;
using Eclipse.PhpaLibrary.Database;
using Eclipse.PhpaLibrary.Web;
using Eclipse.PhpaLibrary.Web.Providers;
using EclipseLibrary.Web.JQuery.Input;
using Eclipse.PhpaLibrary.Database.PIS;
using Eclipse.PhpaLibrary.Reporting;



namespace PhpaAll
{
    public partial class ManageUsers : PageBase
    {
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            this.gvUser.PageIndex = 0;
        }

        #region Selecting
        /// <summary>
        /// Build the where clause
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dsUsers_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            List<string> whereClauses = new List<string>();
            string str = (string)e.WhereParameters["UserName"];
            if (!string.IsNullOrEmpty(str))
            {
                whereClauses.Add(string.Format("UserName.Contains(@UserName)"));
            }
            str = (string)e.WhereParameters["FullName"];
            if (!string.IsNullOrEmpty(str))
            {
                whereClauses.Add(string.Format("FullName.Contains(@FullName)"));
            }
            dsUsers.Where = string.Join(" && ", whereClauses.ToArray());
        }


        /// <summary>
        /// If nothing is selected, then do not query
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dsSpecificUser_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            if (e.WhereParameters["UserId"] == null)
            {
                e.Cancel = true;
            }
        }

        #endregion

        protected void frmViewSpecificUser_ItemInserting(object sender, FormViewInsertEventArgs e)
        {
            CheckBoxListEx cblModules = (CheckBoxListEx)frmViewSpecificUser.FindControl("cblModules");
            e.Values["Modules"] = string.Join(",", cblModules.SelectedValues);
            CheckBoxListEx cblStation = (CheckBoxListEx)frmViewSpecificUser.FindControl("cblStation");
            if (cblStation.Value == string.Empty)
            {
                e.Values["Station"] = null;
            }
        }

        protected void frmViewSpecificUser_ItemUpdating(object sender, FormViewUpdateEventArgs e)
        {
            CheckBoxListEx cblModules = (CheckBoxListEx)frmViewSpecificUser.FindControl("cblModules");
            e.NewValues["Modules"] = string.Join(",", cblModules.SelectedValues);
            CheckBoxListEx cblStation = (CheckBoxListEx)frmViewSpecificUser.FindControl("cblStation");
            if (cblStation.Value == string.Empty)
            {
                e.NewValues["Station"] = null;
            }
        }

        protected void frmViewSpecificUser_ItemDeleted(object sender, FormViewDeletedEventArgs e)
        {
            if (e.Exception == null)
            {
                gvUser.DataBind();
            }
        }

        protected void frmViewSpecificUser_ItemInserted(object sender, FormViewInsertedEventArgs e)
        {
            if (e.Exception == null)
            {
                tbSearchUser.Text = e.Values["UserName"].ToString();
                gvUser.DataBind();
            }
        }

        protected void btnNew_Click(object sender, EventArgs e)
        {
            frmViewSpecificUser.ChangeMode(FormViewMode.Insert);
            dlgEditor.Visible = true;
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            frmViewSpecificUser.ChangeMode(FormViewMode.ReadOnly);
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            frmViewSpecificUser.DeleteItem();
        }

        protected void btnEdit_Click(object sender, EventArgs e)
        {
            frmViewSpecificUser.ChangeMode(FormViewMode.Edit);
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            ButtonEx btnUpdate = (ButtonEx)sender;
            if (!btnUpdate.IsPageValid())
            {
                return;
            }

            switch (frmViewSpecificUser.CurrentMode)
            {
                case FormViewMode.Edit:
                    frmViewSpecificUser.UpdateItem(false);
                    break;
                case FormViewMode.Insert:
                    frmViewSpecificUser.InsertItem(false);
                    break;
                default:
                    throw new InvalidOperationException();
            }
        }

        protected void btnUpdate_PreRender(object sender, EventArgs e)
        {
            ButtonEx btn = (ButtonEx)sender;
            switch (frmViewSpecificUser.CurrentMode)
            {
                case FormViewMode.Edit:
                    btn.Text = "Update";
                    break;
                case FormViewMode.Insert:
                    btn.Text = "Create";
                    break;
                default:
                    throw new InvalidOperationException();
            }
        }

        protected void tbConfirmPassword_ServerValidate(object sender, EclipseLibrary.Web.JQuery.Input.ServerValidateEventArgs e)
        {
            TextBoxEx tbPassword = (TextBoxEx)e.ControlToValidate.NamingContainer.FindControl("tbPassword");
            TextBoxEx tbConfirmPassword = (TextBoxEx)e.ControlToValidate;
            e.ControlToValidate.IsValid = tbConfirmPassword.Text == tbPassword.Text;
        }

        protected void rblRoles_Init(object sender, EventArgs e)
        {
            RadioButtonListEx rblRoles = (RadioButtonListEx)sender;
            PhpaRoleProvider provider = (PhpaRoleProvider)Roles.Provider;
            foreach (Duty duty in provider.Duties.OrderBy(p => p.Seniority))
            {
                rblRoles.Items.Add(new RadioItem()
                {
                    Text = duty.Name,
                    Value = duty.Seniority == 0 ? string.Empty : duty.Name
                });
            }
        }

        protected void frmViewSpecificUser_DataBound(object sender, EventArgs e)
        {
            if (frmViewSpecificUser.DataItem != null)
            {
                PhpaUser user = (PhpaUser)frmViewSpecificUser.DataItem;
                dlgEditor.Title = string.Format("{0}: {1}", user.UserName, user.FullName);
            }
        }

        protected void cblModules_Load(object sender, EventArgs e)
        {
            PhpaRoleProvider provider = (PhpaRoleProvider)Roles.Provider;
            CheckBoxListEx cblModules = (CheckBoxListEx)sender;
            foreach (string module in provider.ModuleNames)
            {
                cblModules.Items.Add(new CheckBoxItem() { Text = module, Value = module });
            }
        }

        protected void gvUser_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (frmViewSpecificUser.CurrentMode == FormViewMode.Insert)
                frmViewSpecificUser.ChangeMode(FormViewMode.ReadOnly);
            dlgEditor.Visible = true;
        }
        protected void cblStation_Load(object sender, EventArgs e)
        {
            PhpaRoleProvider provider = (PhpaRoleProvider)Roles.Provider;
            CheckBoxListEx cblStation = (CheckBoxListEx)sender;
            var stats = new List<Eclipse.PhpaLibrary.Database.PIS.Station>();
            using (PISDataContext db = new PISDataContext(ReportingUtilities.DefaultConnectString))
            {
                var query = from station in db.Stations
                             where station.StationName != null && station.StationName != string.Empty
                             orderby station.StationName
                             select station;
                stats = query.ToList();
            }
            foreach (var station in stats)
            {
                cblStation.Items.Add(new CheckBoxItem() { Text = station.StationName, Value = station.StationId.ToString() });
               
            }
            
        }
    }
}
