using System.Linq;
using System.Web.UI.WebControls;
using Eclipse.PhpaLibrary.Database.Store;
using Eclipse.PhpaLibrary.Web;

namespace Finance.Store
{
    public partial class Store : PageBase
    {

        protected void dsGrnBrief_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            StoreDataContext db = (StoreDataContext)dsGrnBrief.Database;

            e.Result = (from grn in db.GRNs
                        orderby grn.Modified ?? grn.GRNCreateDate descending
                        select new
                        {
                            GRNId = grn.GRNId,
                            GRNCode = grn.GRNCode,
                            LastUpdateDate = grn.Modified ?? grn.GRNCreateDate,
                            GRNReceiveDate = grn.GRNReceiveDate,
                            CountItems = grn.GRNItems.Count,
                            UpdatedBy = grn.ModifiedBy ?? grn.CreatedBy
                        }).Take(20);

        }

        protected void dsGRNReceipt_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            StoreDataContext db = (StoreDataContext)dsGRNReceipt.Database;
            e.Result = (from gd in db.GRNItems
                        where gd.AcceptedQty != null
                        orderby gd.Modified ?? gd.Created descending
                        select new
                        {
                            ItemId = gd.ItemId,
                            GRNId = gd.GRN.GRNId,
                            ItemCode = gd.Item.ItemCode,
                            Brand = gd.Item.Brand,
                            Identifier = gd.Item.Identifier,
                            Size = gd.Item.Size,
                            ItemDescription = gd.Item.Description,
                            GRNCode = gd.GRN.GRNCode,
                            AcceptedQty = gd.AcceptedQty,
                            UpdatedDate = gd.Modified ?? gd.Created,
                            UpdatedBy = gd.ModifiedBy ?? gd.CreatedBy
                        }).Take(20);
        }
    }
}
