/*
 *  Programming: Eclipse Systems (P) Ltd., NOIDA, INDIA
 *  E-mail: support@eclsys.com
 *
 *  $Workfile:   PrinterFriendlyButton.ascx.cs  $
 *  $Revision: 95 $
 *  $Author: ssinghal $
 *  $Date: 2008-07-31 14:56:11 +0530 (Thu, 31 Jul 2008) $
 *  $Modtime:   Jul 09 2008 17:56:22  $
 *
 *  $Log:   S:/Projects/PHPA2/archives/Finance/Controls/PrinterFriendlyButton.ascx.cs-arc  $
 * 
 *    Rev 1.5   Jul 09 2008 17:58:38   vraturi
 * PVCS Template Added.
 */
using System.Web.UI;
using System.Web.UI.WebControls;
namespace PhpaAll.Controls
{
    public partial class PrinterFriendlyButton : UserControl
    {
        protected void btlPrinterFriendly_PreRender(object sender, System.EventArgs e)
        {
            LinkButton btnPrinterFriendly = (LinkButton)sender;
            if (this.Page.Request.QueryString.Count == 0)
            {
                btnPrinterFriendly.PostBackUrl = this.Page.Request.RawUrl + "?Theme=Printing";
            }
            else
            {
                btnPrinterFriendly.PostBackUrl = this.Page.Request.RawUrl + "&Theme=Printing";
            }
        }
    }
}