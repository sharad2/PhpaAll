using System;
using System.Linq;
using System.Web.UI.WebControls;
using Eclipse.PhpaLibrary.Database;
using Eclipse.PhpaLibrary.Web;

namespace Finance.MIS
{
    public partial class FinancialActivity : PageBase
    {
        /// <summary>
        /// This method use for following purpose
        /// 1.Set insert row count.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnInsert_Click(object sender, EventArgs e)
        {
            gv.InsertRowsCount = 1;
            btnInsert.Visible = false;
        }

        /// <summary>
        /// This method use for following purpose
        /// 1.set status message.
        /// 2.Catch exception from database and show exception message.
        /// </summary>
        protected void ds_Inserted(object sender, LinqDataSourceStatusEventArgs e)
        {
            if (e.Exception == null)
            {
                btnInsert.Visible = true;
                Activity_status.AddStatusText("Package activity inserted successfully.");
            }
            else
            {
                Activity_status.AddErrorText(e.Exception.Message);
                e.ExceptionHandled = true;
            }

        }

        /// <summary>
        /// This method use for following purpose.
        /// 1.set status message.
        /// 2.catch exception message from database and show exception message.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ds_Updated(object sender, LinqDataSourceStatusEventArgs e)
        {
            if (e.Exception == null)
            {

                Activity_status.AddStatusText("Package activity update successfully.");
            }
            else
            {
                Activity_status.AddErrorText(e.Exception.Message);
                e.ExceptionHandled = true;
            }
        }

        protected void gv_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int packageActivityId = (int)e.Keys["PackageActivityId"];
            DeleteActivity(packageActivityId);
        }

        private void DeleteActivity(int PackageActivityId)
        {
            PackageActivityDataContext db = (PackageActivityDataContext)ds.Database;
            var activitytransactionId = (from pat in db.PackageActivityTransactionDetails
                                         where pat.PackageActivityId == PackageActivityId
                                         select pat.ActivityTransactionId).ToList();
            foreach (var item in activitytransactionId)
            {
                PackageActivityTransactionDetail patd = db.PackageActivityTransactionDetails.Single(p => p.ActivityTransactionId == item);
                db.PackageActivityTransactionDetails.DeleteOnSubmit(patd);
                PackageActivityTransaction patransactionId = db.PackageActivityTransactions.Single(p => p.ActivityTransactionId == item);
                db.PackageActivityTransactions.DeleteOnSubmit(patransactionId);
                db.SubmitChanges();
            }


        }

        /// <summary>
        /// This method use for following purpose
        /// 1.set status message
        /// 2.catch exception message from database and show exception message.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ds_Deleted(object sender, LinqDataSourceStatusEventArgs e)
        {
            if (e.Exception == null)
            {
                Activity_status.AddStatusText("Package activity deleted successfully.");
            }
            else
            {
                Activity_status.AddErrorText(e.Exception.Message);
                e.ExceptionHandled = true;
            }
        }


    }
}
