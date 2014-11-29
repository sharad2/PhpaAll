/*
 *  Programming: Eclipse Systems (P) Ltd., NOIDA, INDIA
 *  E-mail: support@eclsys.com
 *
 *  $Workfile:   TourExpenses.aspx.cs  $
 *  $Revision: 36679 $
 *  $Author: bkumar $
 *  $Date: 2010-10-26 17:25:51 +0530 (Tue, 26 Oct 2010) $
 *  $Modtime:   Jul 21 2008 15:09:40  $
 *
 *  $Log:   S:/Projects/PHPA2/archives/Finance/Reports/TourExpenses.aspx.cs-arc  $
 * 
 *    Rev 1.39   Jul 21 2008 15:10:44   yjuneja
 * WIP
 * 
 *    Rev 1.38   Jul 09 2008 17:41:00   vraturi
 * PVCS Template Added.
 */
using System;
using System.Linq;
using System.Web.UI;
using Eclipse.PhpaLibrary.Reporting;
using Eclipse.PhpaLibrary.Web;
using System.Web.UI.WebControls;
using EclipseLibrary.Web.Extensions;

namespace Finance.Reports
{
    public partial class TourExpenses : PageBase
    {
        //DateTime? dt = new DateTime();

        protected override void OnLoad(EventArgs e)
        {
            //if (!Page.IsPostBack)
            //{
            //    //  tbdate.Value = DateTime.Now;
            //    tbFromDate.Text = DateTime.Now.AddYears(-1).ToShortDateString();
            //    tbToDate.Text = DateTime.Now.ToShortDateString();
            //}
            //this.Page.Title = string.Format("Tour Expenses as on {0:dd/MM/yyyy}", tbdate.Date);
            base.OnLoad(e);
        }


        protected void dsTourExp_Selecting(object sender, System.Web.UI.WebControls.LinqDataSourceSelectEventArgs e)
        {
            ReportingDataContext db = (ReportingDataContext)dsTourExp.Database;
            var allDetails = from vd in db.RoVoucherDetails
                             where vd.RoHeadHierarchy.HeadOfAccountType == "TOUR_EXPENSES" &&
                                     vd.EmployeeId != null
                             select vd;

            DateTime? fromdate = tbFromDate.ValueAsDate;
            if (fromdate != null)
            {
                allDetails = allDetails.Where(p => p.RoVoucher.VoucherDate.Date >= fromdate);
                this.Title += string.Format(" From {0:d}", fromdate);
            }

            DateTime? todate = tbToDate.ValueAsDate;
            if (todate != null)
            {
                this.Title += string.Format(" To {0:d}", todate);
                allDetails = allDetails.Where(p => p.RoVoucher.VoucherDate.Date <= todate);
            }

            if (fromdate == null && todate == null)
            {
                this.Title += " For All Dates";
            }

            var tourExpenses = (from vd in allDetails
                                group vd by vd.RoEmployee into grouping
                                select new
                                {
                                    Employee = grouping.Key,
                                    EmpCode = grouping.Key.EmployeeCode,
                                    EmpFirstName = grouping.Key.FirstName,
                                    EmpLastName = grouping.Key.LastName,
                                    Designation = grouping.Key.Designation,
                                    division = grouping.Key.RoDivision.DivisionName,
                                    //TourExpense = grouping.Sum(p => p.RoVoucher.RoVoucherDetails.Sum(q => q.DebitAmount ?? 0))
                                    TourExpense = grouping.Key.RoVoucherDetails.Sum(q => q.DebitAmount ?? 0)
                                }).ToList();

            e.Result = tourExpenses;
            if (tourExpenses.Count != 0)
            {
                lblnodata.Visible = false;
                gvTourExpenses.Visible = true;
            }
            else
            {
                gvTourExpenses.Visible = false;
                lblnodata.Visible = true;
            }
        }

        decimal? m_sumExpense = 0.0M;
        protected void gvTourExpenses_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            switch (e.Row.RowType)
            {
                case DataControlRowType.Header:
                    //gvTourExpenses.Caption = string.Format("<b>Tour Expenses upto {0:dd MMMM yyyy}</b>", tbdate.Date);
                    break;

                case DataControlRowType.DataRow:
                    HyperLink hl = (HyperLink)e.Row.FindControl("hlTourExpense");
                    RoEmployee emp = (RoEmployee)DataBinder.Eval(e.Row.DataItem, "Employee");
                    if (emp == null)
                    {
                        hl.NavigateUrl = string.Format("~/Finance/VoucherSearch.aspx?AccountTypes=TOUR_EXPENSES&EmployeeId=0");
                    }
                    else
                    {
                        hl.NavigateUrl = string.Format("~/Finance/VoucherSearch.aspx?AccountTypes=TOUR_EXPENSES&EmployeeId={0}", emp.EmployeeId);
                    }
                    m_sumExpense += (decimal?)DataBinder.Eval(e.Row.DataItem, "TourExpense");
                    break;
                case DataControlRowType.Footer:
                    DataControlFieldCell columnSum = (from DataControlFieldCell cell in e.Row.Cells
                                                      where cell.ContainingField.AccessibleHeaderText == "TourExpense"
                                                      select cell).Single();
                    columnSum.Text = string.Format("{0:###,###,###,##0.00;(###,###,###,##0.00);#}", m_sumExpense);
                    break;
            }
        }

        protected void btnShowReport_Click(object sender, EventArgs e)
        {
            if (btnShowReport.IsPageValid())
            {
                gvTourExpenses.DataBind();
            }
        }
    }
}
