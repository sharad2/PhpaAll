/*
 *  Programming: Eclipse Systems (P) Ltd., NOIDA, INDIA
 *  E-mail: support@eclsys.com
 *
 *  $Workfile:   YearlyExpenditure.aspx.cs  $
 *  $Revision: 7029 $
 *  $Author: pshishodia $
 *  $Date: 2008-12-30 16:08:30 +0530 (Tue, 30 Dec 2008) $
 *  $Modtime:   Jul 09 2008 17:40:12  $
 *
 *  $Log:   S:/Projects/PHPA2/archives/Finance/Reports/YearlyExpenditure.aspx.cs-arc  $
 * 
 *    Rev 1.14   Jul 09 2008 17:41:00   vraturi
 * PVCS Template Added.
 */
using System;
using System.Linq;
using Eclipse.PhpaLibrary.Reporting;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using System.Data;
using Eclipse.PhpaLibrary.Web;

namespace Finance.Reports
{
    public partial class YearlyExpenditure : PageBase
    {
        DateTime? dt = DateTime.Today; 

        protected override void OnLoad(EventArgs e)
        {
            ReportingDataContext db = (ReportingDataContext)dsExpenditure.Database;

            DateTime curDate = DateTime.Today;


            List<MonthYear> l = (from vd in db.RoVoucherDetails
                                 where (HeadOfAccountHelpers.JobExpenses.Contains(vd.RoHeadHierarchy.HeadOfAccountType))
                                 //where vd.RoHeadHierarchy.HeadOfAccountType == "Expenditure"
                                 //         || vd.RoHeadHierarchy.HeadOfAccountType == "TOUR_EXPENSES"
                                 group vd by new
                                 {
                                     vd.RoVoucher.VoucherDate.Year,
                                     vd.RoVoucher.VoucherDate.Month
                                 }
                                     into groouping
                                     orderby groouping.Key.Year
                                     select new MonthYear()
                                     {
                                         Month = groouping.Key.Month,
                                         Year = groouping.Key.Year,
                                         Amount = groouping.Sum(p => p.DebitAmount ?? 0 - p.CreditAmount ?? 0)
                                     }).ToList();

            gvExpenditure.DataSource = from g in l
                                       group g by g.Year into grp
                                       select new
                                       {
                                           Year = grp.Key,
                                           Amount = grp.Sum(p => p.Amount)
                                       };
            gvExpenditure.DataBind();

            base.OnLoad(e);
        }

        protected void gvExpenditure_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            switch (e.Row.RowType)
            {
                case DataControlRowType.Header:
                    gvExpenditure.Caption = string.Format("<b>Yearly expenditure as of {0:yyyy}</b>", dt.Value);
                    break;
            }
        }


       public class MonthYear
        {
            int _month;
            public int Month 
            {
                get
                {
                    return _month;
                }
                set
                {
                    _month = value;
                }
            }

            int _year;
            public int Year 
            {
                get
                {
                    if (_month < 4)
                    {
                        _year--;
                    }
                    return _year;
                }
                set
                {
                    _year = value;
                }
            }

            decimal? _amount;
            public decimal? Amount 
            { 
                get
                {
                    return _amount;
                } 
                set
                {
                    if (value.HasValue)
                    {
                        if (_amount.HasValue)
                        {
                            _amount += value.Value;
                        }
                        else
                        {
                            _amount = value;
                        }
                    }
                } 
            }
        }
    }
}
