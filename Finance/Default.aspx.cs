/*
 *  Programming: Eclipse Systems (P) Ltd., NOIDA, INDIA
 *  E-mail: support@eclsys.com
 *
 *  $Workfile:   Default.aspx.cs  $
 *  $Revision: 30488 $
 *  $Author: ssinghal $
 *  $Date: 2010-01-28 09:49:12 +0530 (Thu, 28 Jan 2010) $
 *  $Modtime:   Jul 09 2008 17:52:14  $
 *
 *  $Log:   S:/Projects/PHPA2/archives/Finance/Default.aspx.cs-arc  $
 * 
 *    Rev 1.1   Jul 09 2008 17:54:10   vraturi
 * PVCS Template Added.
 */
using System.Web.UI;
using Eclipse.PhpaLibrary.Web;
public partial class _Default : PageBase
{
    protected override void OnLoad(System.EventArgs e)
    {
        this.ClearSession();
        base.OnLoad(e);
    }
}
