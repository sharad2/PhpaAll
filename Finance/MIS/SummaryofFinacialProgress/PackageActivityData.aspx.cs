using System;
using System.Globalization;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Eclipse.PhpaLibrary.Database;
using Eclipse.PhpaLibrary.Reporting;
using Eclipse.PhpaLibrary.Web;
using EclipseLibrary.Web.JQuery.Input;

namespace PhpaAll.MIS
{
    public partial class FinancialActivityData : PageBase
    {
        protected override void OnInit(EventArgs e)
        {
            gv.RowDataBound += new GridViewRowEventHandler(gv_RowDataBound);
            //btnApplyFilter.Click += new EventHandler(btnApplyFilter_Click);
            base.OnInit(e);
        }

        void gv_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            switch (e.Row.RowType)
            {
                case DataControlRowType.DataRow:
                    object obj = DataBinder.Eval(e.Row.DataItem, "ActivityTransactionId");
                    if (obj != null) {
                        hfActivityTransactionId.Value = obj.ToString();
                    }
                    break;
            }
        }
        /// <summary>
        /// This method use for following purpose
        /// 1.Bind ProgressDate in  dropdownlist.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dsProgressDate_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            PackageActivityDataContext db = (PackageActivityDataContext)ds.Database;
            var query = (from pat in db.PackageActivityTransactions
                         orderby pat.PackageActivityDate descending
                         select new
                         {
                             PackageActivityDate = pat.PackageActivityDate.ToShortDateString()
                         }).Take(30).Distinct();
            e.Result = query;

        }
        /// <summary>
        /// Build the query for grid view. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>       
        protected void ds_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {

            if (string.IsNullOrEmpty(ddlProgressDate.Value))
            {
                // This happens when the page is initially loaded.
                e.Cancel = true;
                return;
            }

            gv.Caption = string.Format("Enter data for {0:d}", ddlProgressDate.Value);

            PackageActivityDataContext db = (PackageActivityDataContext)ds.Database;
            DateTime selectedDate = DateTime.Parse(ddlProgressDate.Value, CultureInfo.CurrentUICulture);

            hfPackagesReport.Value = ddlPackagesReport.Value;
            hfPackages.Value = ddlPackages.Value;
            hfDate.Value = ddlProgressDate.Value;
            hfActivityTransactionId.Value = string.Empty;   // This will be set in RowDataBound

            var details = from det in db.PackageActivityTransactionDetails
                          where det.PackageActivityTransaction.PackageActivityDate == selectedDate
                          && det.PackageActivityTransaction.PackageReportId == Convert.ToInt32(ddlPackagesReport.Value)
                          && det.PackageActivityTransaction.PackageId == Convert.ToInt32(ddlPackages.Value)
                          select det;

            e.Result = from act in db.PackageActivities
                       join det in details on act.PackageActivityId equals det.PackageActivityId into g
                       from det in g.DefaultIfEmpty()
                       where act.PackageActivityGroup.PackageReportId == Convert.ToInt32(ddlPackagesReport.Value)
                       && (!act.Description.Contains("Total"))
                       && (!act.Description.Contains("Advance Outstanding"))
                       orderby act.PackageActivityGroup.Description
                       select new
                       {
                           GroupDescription = act.PackageActivityGroup.Description,
                           ActivityTransactionId = (int?)det.ActivityTransactionId,
                           PackageActivityId = act.PackageActivityId,
                           ActivityTransactionDetailId = (int?)det.ActivityTransactionDetailId,
                           Description = act.Description,
                           PackageActivtiyData = det.PackageActivtiyData
                       };

        }

        protected void gv_DataBound(object sender, EventArgs e)
        {
            ph.Visible = gv.Rows.Count > 0;
        }


        protected void btnApplyFilter_Click(object sender, EventArgs e)
        {
            gv.DataBind();
        }

        protected void btnSaveProgress_Click(object sender, EventArgs e)
        {
            if (gv.SelectedIndexes.Count < 0)
            {
                return;
            }
            try
            {
                int? activityTransactionId = string.IsNullOrEmpty(hfActivityTransactionId.Value) ? (int?)null :
                    Convert.ToInt32(hfActivityTransactionId.Value);

                using (PackageActivityDataContext db = new PackageActivityDataContext(ReportingUtilities.DefaultConnectString))
                {
                    PackageActivityTransaction pat;
                    //Insert Case
                    if (activityTransactionId == null)
                    {
                        pat = new PackageActivityTransaction()
                        {
                            PackageActivityDate = Convert.ToDateTime(hfDate.Value),
                            PackageId = Convert.ToInt32(hfPackages.Value),
                            PackageReportId = Convert.ToInt32(hfPackagesReport.Value),
                            //PackageActivityId = (int)gv.DataKeys[r].Value,
                            Remarks = tbRemarks.Text
                        };
                        db.PackageActivityTransactions.InsertOnSubmit(pat);
                    }
                    else
                    {
                        //Update Case
                        pat = db.PackageActivityTransactions
                            .Where(p => p.ActivityTransactionId == activityTransactionId).Single();
                        pat.Remarks = tbRemarks.Text;
                    }

                    foreach (int r in gv.SelectedIndexes)
                    {
                        int packageActivityId = (int)gv.DataKeys[r]["PackageActivityId"];
                        int? activityTransactionDetailId = (int?)gv.DataKeys[r]["ActivityTransactionDetailId"];
                        //int? activityTransactionId = (int?)gv.DataKeys[r]["ActivityTransactionId"];
                        decimal? packageActivtiyData = null;
                        TextBoxEx tbPackageActivityData = (TextBoxEx)gv.Rows[r].FindControl("tbPackageActivityData");
                        if (!string.IsNullOrEmpty(tbPackageActivityData.Text))
                        {
                            packageActivtiyData = Convert.ToDecimal(tbPackageActivityData.Text);
                        }

                        UpdateActivityDetail(pat, activityTransactionDetailId, packageActivtiyData, packageActivityId);
                    }
                    db.SubmitChanges();
                }
                gv.DataBind();
            }
            catch (Exception ex)
            {
                sp_status.AddErrorText(ex.Message);

            }
            
        }

        protected void UpdateActivityDetail(PackageActivityTransaction pat, int? activityTransactionDetailId,
            decimal? packageActivtiyData, int packageActivityId)
        {
            PackageActivityTransactionDetail patd;
            if (activityTransactionDetailId == null)
            {
                patd = new PackageActivityTransactionDetail()
                {
                    PackageActivtiyData = packageActivtiyData,
                    PackageActivityId = packageActivityId
                };
                pat.PackageActivityTransactionDetails.Add(patd);
            }
            else
            {
                patd = pat.PackageActivityTransactionDetails
                    .Single(p => p.ActivityTransactionDetailId == activityTransactionDetailId);
                patd.PackageActivtiyData = packageActivtiyData;

            }

        }

    }

}
