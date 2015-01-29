using System;
using System.Data.Linq;
using System.Linq;
using System.Web.UI.WebControls;
using Eclipse.PhpaLibrary.Database.Store;
using Eclipse.PhpaLibrary.Web;

namespace PhpaAll.Store
{
    public partial class GrnList : PageBase
    {


        protected void dsGrnBrief_ContextCreated(object sender, LinqDataSourceStatusEventArgs e)
        {
            StoreDataContext db = (StoreDataContext)e.Result;
            DataLoadOptions dlo = new DataLoadOptions();
            dlo.LoadWith<GRN>(grn => grn.RoContractor);
            db.LoadOptions = dlo;
        }

        protected void hlEdit_PreRender(object sender, EventArgs e)
        {
            HyperLink hl = (HyperLink)sender;
            if (!this.User.Identity.IsAuthenticated)
            {
                hl.Enabled = false;
                hl.ToolTip = "You must Login to Edit and Receive GRN";
            }
            else
            {
                hl.Enabled = true;
            }
        }

        //private bool _isValid = true;
        //protected void btnShowGRN_Click(object sender, EventArgs e)
        //{
        //    if (!btnShowGRN.IsPageValid())
        //    {
        //        _isValid = false;
        //    }
        //}

        /// <summary>
        /// Execute query for Summary grid according to the Radio button values chooses.
        /// i.e. For Unreceived, Received and All.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dsGrnBrief_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            if (!btnShowGRN.IsPageValid())
            {
                e.Cancel = true;
                return;
            }
            StoreDataContext db = (StoreDataContext)dsGrnBrief.Database;

            //List<string> whereClause = new List<string>();

            var grnList = db.GRNs.Where(p => p.GRNCreateDate >= tbFromDate.ValueAsDate
                                    && p.GRNCreateDate <= tbToDate.ValueAsDate);
            if(!string.IsNullOrEmpty(tbPo.Text))
            {
               grnList = grnList.Where(p=>p.PONumber.Contains(tbPo.Text));
            }

            if(!string.IsNullOrEmpty(tbSupplier.Text))
            {
                grnList = grnList.Where(p=> p.RoContractor.ContractorCode == tbSupplier.Text || p.RoContractor.ContractorName.Contains(tbSupplier.Text));
            }

            switch (rblShowGrn.Value)
            {
                case "UR":
                    grnList = grnList.Where(p => p.GRNReceiveDate == null);
                    break;

                case "R":
                    grnList = grnList.Where(p => p.GRNReceiveDate != null);
                    break;

                case "All":
                    break;

                default:
                    throw new NotImplementedException();
            }
            //dsGrnBrief.Where = string.Join("&&", whereClause.ToArray());
            e.Result = from grn in grnList
                       orderby grn.GRNCreateDate descending
                       select new
                       {
                           GRNId = grn.GRNId,
                           GRNCode = grn.GRNCode,
                           GRNCreateDate = grn.GRNCreateDate,
                           GRNReceiveDate = grn.GRNReceiveDate,
                           PONumber = grn.PONumber,
                           PODate = grn.PODate,
                           NameOfCarrier = grn.NameofCarrier,
                           Supplier = grn.RoContractor.ContractorName,
                           Created = grn.Created,
                           CountItems = grn.GRNItems.Count,
                       };

        }

    }
}
