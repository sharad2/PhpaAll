using System;
using System.Web.Security;
using Eclipse.PhpaLibrary.Web;
using System.Web.UI.WebControls;

namespace PhpaAll.Admin
{
    public partial class Security : PageBase
    {

        protected override void OnLoad(EventArgs e)
        {
            if (!this.IsPostBack)
            {
                tbUser.Text = this.Page.User.Identity.Name;
                ShowRoles();
            }
            base.OnLoad(e);
        }
        protected void btnGo_Click(object sender, EventArgs e)
        {
            ShowRoles();
        }
        protected void btnShowAll_Click(object sender, EventArgs e)
        {
            tbUser.Text = string.Empty;
            ShowRoles();
        }
        private void ShowRoles()
        {
            if (string.IsNullOrEmpty(tbUser.Text))
            {
                foreach (string role in Roles.GetAllRoles())
                {
                    blRoles.Items.Add(role);
                }
            }
            else
            {
                foreach (string role in Roles.GetRolesForUser(tbUser.Text))
                {
                    blRoles.Items.Add(role);
                }
            }
        }

    }
}
