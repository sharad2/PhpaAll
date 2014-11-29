using System;
using System.Globalization;
using System.Threading;
using System.Web.UI;
using Eclipse.PhpaLibrary.Database;
using EclipseLibrary.Web.JQuery;
using Eclipse.PhpaLibrary.Reporting;
using System.Linq;
using System.Web;

namespace Eclipse.PhpaLibrary.Web
{
    /// <summary>
    /// Sets the printing theme depending on the value of the QueryString variable Theme
    /// </summary>
    public class PageBase : Page
    {

        public override string StyleSheetTheme
        {
            get
            {
                if (JQueryScriptManager.IsAjaxCall)
                {
                    return string.Empty;
                }
                string theme = this.Request.QueryString["Theme"];
                if (string.IsNullOrEmpty(theme))
                {
                    return base.StyleSheetTheme;
                }
                else
                {
                    return theme;
                }
            }
            set
            {
                base.StyleSheetTheme = value;
            }
        }

        public override string Theme
        {
            get
            {
                return string.Empty;
            }
            set
            {
                throw new NotSupportedException("Themes are not supported. Use StyleSheetTheme");
            }
        }

        private static CultureInfo m0_phpaCulture;

        /// <summary>
        /// For use by web methods
        /// </summary>
        public static new CultureInfo Culture
        {
            get
            {
                return m0_phpaCulture;
            }
        }

        static PageBase()
        {
            m0_phpaCulture = new CultureInfo("en-US");
            m0_phpaCulture.DateTimeFormat.ShortDatePattern = "d/M/yyyy";
            m0_phpaCulture.NumberFormat.CurrencySymbol = string.Empty;
            // Negative numbers within parenthesis
            m0_phpaCulture.NumberFormat.CurrencyNegativePattern = 14;
        }

        protected override void InitializeCulture()
        {
            Thread.CurrentThread.CurrentCulture = m0_phpaCulture;
            Thread.CurrentThread.CurrentUICulture = m0_phpaCulture;

            base.InitializeCulture();
        }

        protected override void OnUnload(EventArgs e)
        {
            base.OnUnload(e);
            if (this.Context.Items[DataContextBase.CONTEXT_KEY] != null)
            {
                // Spurious exceptions are being thrown sometimes so the exception has been commented out
                //throw new HttpException("Some data contexts have not been disposed off");
            }
        }

        public override void VerifyRenderingInServerForm(Control control)
        {
            //base.VerifyRenderingInServerForm(control);
        }

        /// <summary>
        /// Returns authorized stations for currently logged in user. Returns null if user is authorized for all stations.
        /// </summary>
        /// <returns></returns>
        protected int[] GetUserStations()
        {

            var stations = this.Context.Session["station"] as int[];
            if (stations == null)
            {
                using (AuthenticationDataContext ctx = new AuthenticationDataContext(ReportingUtilities.DefaultConnectString))
                {
                    var station = (from user in ctx.PhpaUsers
                                   where user.UserName == HttpContext.Current.User.Identity.Name
                                   select user.Station).Single();
                    if (string.IsNullOrEmpty(station))
                    {
                        stations = new int[0];
                    }
                    else
                    {
                        stations = station.Split(',').Select(p => int.Parse(p)).ToArray();
                    }
                    this.Context.Session["station"] = stations;
                }
            }
            return stations.Length == 0 ? null : stations;
        }
        protected void ClearSession()
        {
            this.Context.Session["station"] = null;
        }
       
    }
}
