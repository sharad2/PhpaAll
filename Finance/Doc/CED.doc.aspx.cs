using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Eclipse.PhpaLibrary.Web;

namespace PhpaAll.Doc
{
	public partial class CED : PageBase
	{
        protected override void OnLoad(EventArgs e)
        {
            string ReportName = Request.QueryString["ReportName"];

            if (string.IsNullOrEmpty(ReportName))
            {
                ReportName = "ED";
            }

            switch (ReportName)
            {
                case "ED":
                    Page.Title = string.Format("Central Excise Duty Report Documentation");
                    break;

                case "BST":
                    Page.Title = string.Format("Bhutan Sales Tax Report Documentation" );
                    break;
            }

            View view = (View)mvHelp.FindControl(ReportName);
            mvHelp.SetActiveView(view);

            base.OnLoad(e);
        }
	}
}