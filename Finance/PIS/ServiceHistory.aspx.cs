using System;
using System.Linq;
using System.Web.UI.WebControls;
using Eclipse.PhpaLibrary.Database.PIS;
using Eclipse.PhpaLibrary.Reporting;
using Eclipse.PhpaLibrary.Web;

namespace PhpaAll.PIS
{
    public partial class ServiceHistory : PageBase
    {
        /// <summary>
        /// Cancel query when no Employee is found
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dsServiceHistory_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            if (e.WhereParameters["EmployeeId"] == null)
            {
                e.Cancel = true;
                return;
            }
            // Do not show the most recent record
            PISDataContext db = (PISDataContext)dsServiceHistory.Database;
            var query = (from sp in db.ServicePeriods
                         where sp.EmployeeId == Convert.ToInt32(e.WhereParameters["EmployeeId"])
                         orderby sp.PeriodStartDate descending
                         select sp).Skip(1);
            e.Result = query;
        }

        protected void dsServiceHistoryDetails_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            int i = 0;
            string[] str = (string[])e.WhereParameters["ServicePeriodId"];

            if (!string.IsNullOrEmpty(str.FirstOrDefault()))
            {
                i = Convert.ToInt32(str);
            }

            if (e.WhereParameters["ServicePeriodId"] == null)
            {
                e.Cancel = true;
            }
            else
            {
                e.WhereParameters["ServicePeriodId"] = i;
            }
        }

        #region DropDownSuggest Distinct queries
        protected void dsDesignation_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            using (PISDataContext db = new PISDataContext(ReportingUtilities.DefaultConnectString))
            {
                e.Result = (from emp in db.Employees
                            where emp.Designation != null
                            orderby emp.Designation
                            select emp.Designation).Distinct().ToArray();
            }
        }

        protected void dsGrade_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            using (PISDataContext db = new PISDataContext(ReportingUtilities.DefaultConnectString))
            {
                e.Result = (from grd in db.ServicePeriods
                            where grd.Grade != null
                            orderby grd.Grade
                            select grd.Grade).Distinct().ToArray();
            }
        }

        protected void dsPostedAt_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            using (PISDataContext db = new PISDataContext(ReportingUtilities.DefaultConnectString))
            {
                e.Result = (from sp in db.ServicePeriods
                            where sp.PostedAt != null
                            orderby sp.PostedAt
                            select sp.PostedAt).Distinct().ToArray();
            }
        }
        #endregion

        int _servicePeriodId = -1;
        protected void dsServiceHistory_Updated(object sender, LinqDataSourceStatusEventArgs e)
        {
            if (e.Exception == null)
            {
                _servicePeriodId = GetServicePeriodId(e.Result);
                ServiceHistory_sp.AddStatusText("Item updated successfully");
            }
            else
            {
                ServiceHistory_sp.AddErrorText(e.Exception.Message);
                e.ExceptionHandled = true;
            }
        }

        protected void dsServiceHistory_Deleted(object sender, LinqDataSourceStatusEventArgs e)
        {
            if (e.Exception == null)
            {
                ServiceHistory_sp.AddStatusText("Item deleted successfully");
            }
            else
            {
                ServiceHistory_sp.AddErrorText(e.Exception.Message);
                e.ExceptionHandled = true;
            }


        }

        protected void gvServiceHistory_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            switch (e.Row.RowType)
            {
                case DataControlRowType.DataRow:
                    if (_servicePeriodId != -1)
                    {
                        int id = GetServicePeriodId(e.Row.DataItem);
                        if (id == _servicePeriodId)
                        {
                            e.Row.RowState |= DataControlRowState.Selected;
                        }
                    }
                    break;

                default:
                    break;
            }
        }

        #region Helpers
        private int GetServicePeriodId(object dataItem)
        {
            ServicePeriod sp = (ServicePeriod)dataItem;
            return sp.ServicePeriodId;
        }
        #endregion

    }
}
