using System;
using System.Linq;
using System.Web.UI.WebControls;
using Eclipse.PhpaLibrary.Database.Store;
using Eclipse.PhpaLibrary.Web;
using EclipseLibrary.Web.JQuery.Input;

namespace PhpaAll.Store.Reports
{
    public partial class POGRNs : PageBase
    {
        //protected void Page_Load(object sender, EventArgs e)
        //{
        //    if (!Page.IsPostBack)
        //    {
        //        tbFromDate.Value = DateTime.Today.AddDays(-7).ToShortDateString();
        //        tbToDate.Value = DateTime.Today.ToShortDateString();
        //    }
        //}

        protected void dsPOGRNs_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            if (string.IsNullOrEmpty(tbPoNo.Value))
            {
                e.Cancel = true;
                return;
            }
            
            StoreDataContext db = (StoreDataContext)dsPOGRNs.Database;
            e.Result = from grn in db.GRNs
                       where  grn.PONumber == tbPoNo.Value
                       orderby grn.GRNCreateDate descending
                       select new
                       {
                           GRNId = grn.GRNId,
                           GRNNo = grn.GRNCode,
                           PONo = grn.PONumber,
                           GRNCreateDate = grn.GRNCreateDate,
                           GRNReceiveDate = grn.GRNReceiveDate,
                           Supplier = grn.RoContractor.ContractorName,
                           PoDate = grn.PODate,
                           OrderedItems = grn.GRNItems.Sum(p=>p.OrderedQty),
                           DeliveredItems = grn.GRNItems.Sum(p=>p.ReceivedQty)
                       };

        }

        protected void gvPoGrns_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            switch (e.Row.RowType)
            {
                case DataControlRowType.Header:
                    gvPoGrns.Caption = string.Format("GRN List for purchase Order No {0}", tbPoNo.Value);
                    break;
            }
        }

        protected void btnShow_Click(object sender, EventArgs e)
        {
            ButtonEx btn = (ButtonEx)sender;
            if (btn.IsPageValid())
            {
                gvPoGrns.DataBind();
            }
        }
    }
}
