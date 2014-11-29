using System.Web.UI.WebControls;
using Eclipse.PhpaLibrary.Web;

namespace Finance.PIS.Share
{
    public partial class ServiceHistoryDetails : PageBase
    {
        /// <summary>
        /// Cancel the query if ServicePeriodId not found
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dsServiceHistoryDetails_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            if (e.WhereParameters["ServicePeriodId"] == null)
            {
                e.Cancel = true;
            }
        }

    }
}
