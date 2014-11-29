using System.Web.UI;
using Eclipse.PhpaLibrary.Web;
using System.Web.UI.WebControls;
using System.Web;


namespace Finance
{
    public partial class SiteMenu : PageBase
    {
        protected override void OnLoad(System.EventArgs e)
        {
            string url = Request.QueryString["StartingNodeUrl"];
            var node = SiteMap.Provider.FindSiteMapNode(url);
            if (node == null)
            {
            }
            else
            {
                smds.StartingNodeUrl = url;
                this.Title += string.Format(" - {0}", node.Title);
            }
            base.OnLoad(e);
        }

        protected void menuStore_MenuItemDataBound(object sender, MenuEventArgs e)
        {
            if (e.Item.NavigateUrl.EndsWith("#"))
            {
                e.Item.Selectable = false;
            }
        }
    }
}
