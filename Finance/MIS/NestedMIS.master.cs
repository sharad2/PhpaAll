using System;
using System.Linq;
using System.Web;
using System.Web.Security;
using Eclipse.PhpaLibrary.Database.MIS;
using Eclipse.PhpaLibrary.Reporting;
using Eclipse.PhpaLibrary.Web.Providers;

namespace PhpaAll.MIS
{
    public partial class NestedMIS : System.Web.UI.MasterPage
    {

        private bool? _isEditMode;

        /// <summary>
        /// True if we are here for data entry
        /// </summary>
        public bool IsEditMode
        {
            get
            {
                if (_isEditMode == null)
                {
                    _isEditMode = !string.IsNullOrEmpty(this.Request.QueryString["IsEditMode"]);
                }
                return _isEditMode.Value;
            }
            set
            {
                _isEditMode = value;
            }
        }

        public int PackageId
        {
            get
            {
                if (string.IsNullOrEmpty(ddl.Value))
                {
                    return 0;
                }
                return Convert.ToInt32(ddl.Value);
            }

        }

        public string PackageName
        {
            get
            {
                var item = ddl.SelectedItem;
                if (item == null)
                {
                    return string.Empty;
                }
                return item.Text;
            }

        }

        private class PackageInfo
        {
            public PackageInfo(Package pkg)
            {
                _packageName = pkg.PackageName;
                _packageId = pkg.PackageId;
                _displayName = string.Format("{0}: {1}", pkg.PackageName, pkg.Description);
            }

            private readonly string _packageName;
            public string PackageName
            {
                get
                {
                    return _packageName;
                }
            }

            private readonly int _packageId;
            public int PackageId
            {
                get
                {
                    return _packageId;
                }
            }

            private readonly string _displayName;
            public string DisplayName
            {
                get
                {
                    return _displayName;
                }
            }
        }

        protected override void OnInit(EventArgs e)
        {
            PhpaRoleProvider provider = (PhpaRoleProvider)Roles.Provider;
            string[] authorizedModules = provider.GetModulesForUser(this.Context.User.Identity.Name);
            if (authorizedModules != null)
            {
                PackageInfo[] misModules;
                using (MISDataContext db = new MISDataContext(ReportingUtilities.DefaultConnectString))
                {
                    misModules = (from package in db.Packages
                                  select new PackageInfo(package)
                                ).ToArray();
                }
                var query = from am in authorizedModules
                            join mm in misModules on am equals mm.PackageName
                            select mm;
                ddl.DataTextField = "DisplayName";
                ddl.DataValueField = "PackageId";
                ddl.DataSource = query;
                ddl.DataBind();
            }

            string str = this.Page.Request.Form[ddl.UniqueID];
            HttpCookie cookie;
            if (!string.IsNullOrEmpty(str))
            {
                // Go button was pressed. Updated the selected value of package drop down
                ddl.Value = str;
                // Save new value in cookie
                cookie = new HttpCookie("PackageId", ddl.Value);
                cookie.Expires = DateTime.Now.AddMonths(1);
                this.Page.Response.Cookies.Add(cookie);
            }
            else
            {
                // Use the cookie value
                cookie = this.Request.Cookies["PackageId"];
                if (cookie != null && !string.IsNullOrEmpty(cookie.Value))
                {
                    ddl.Value = cookie.Value;
                }
            }
            if (IsEditMode && !Roles.IsUserInRole("Operator"))
            {
                this.IsEditMode = false;
                this.lblAccess.Visible = true;
            }
            base.OnInit(e);
        }

    }
}