using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

/*
 *  Programming: Eclipse Systems (P) Ltd., NOIDA, INDIA
 *  E-mail: support@eclsys.com
 *
 *  $Workfile:   MasterPage.master.cs  $
 *  $Revision: 37084 $
 *  $Author: bkumar $
 *  $Date: 2010-11-09 10:36:28 +0530 (Tue, 09 Nov 2010) $
 *  $Modtime:   Jul 18 2008 15:56:58  $
 *
 *  $Log:   S:/Projects/PHPA2/archives/Finance/MasterPage.master.cs-arc  $
 * 
 */



public partial class MasterPage : System.Web.UI.MasterPage
{
    /// <summary>
    /// If we can identify the referrer, show to Back to link
    /// </summary>
    /// <param name="e"></param>
    protected override void OnLoad(System.EventArgs e)
    {
        if (!IsPostBack)
        {
            if (this.Request.UrlReferrer != null)
            {
                SiteMapNode node = SiteMap.Provider.FindSiteMapNode(this.Request.UrlReferrer.AbsolutePath);
                if (node != null)
                {
                    HyperLink hlTrackBack = (HyperLink)this.FindControl("hlTrackBack");
                    hlTrackBack.NavigateUrl = this.Request.UrlReferrer.PathAndQuery;
                    hlTrackBack.Text = string.Format("Back to {0}", node.Title);
                    hlTrackBack.Visible = true;
                }
            }
        }

        if (this.Page.Request.QueryString["Theme"] == "Printing")
        {
            this.toprow.Visible = false;
            
            this.SideNavigation.Visible = false;
            this.footer.Visible = false;
        }

        // Site map link shows up on home page only
        var curNode = SiteMap.CurrentNode;
        if (curNode == null)
        {
            menuContext.Visible = false;
            hlSiteMap.Visible = false;
        }
        else
        {
            if (curNode.ParentNode == null || curNode.ParentNode.Key == curNode.RootNode.Key)
            {
                // Do not show context menu on Package Home Page
                menuContext.Visible = false;
                hlSiteMap.NavigateUrl += curNode.Url;
            }
            else
            {
                // Show my children if possible, otherwise show siblings
                //dsContext.StartingNodeOffset = -2;// curNode.ChildNodes.Count > 0 ? 0 : -1;
                hlSiteMap.Visible = false;
            }
        }

        base.OnLoad(e);
    }
    
}
