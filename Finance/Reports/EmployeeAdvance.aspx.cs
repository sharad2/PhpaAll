/*
 *  Programming: Eclipse Systems (P) Ltd., NOIDA, INDIA
 *  E-mail: support@eclsys.com
 *
 *  $Workfile:   EmployeeAdvance.aspx.cs  $
 *  $Revision: 38598 $
 *  $Author: ssingh $
 *  $Date: 2010-12-06 17:52:33 +0530 (Mon, 06 Dec 2010) $
 *  $Modtime:   Jul 22 2008 13:42:24  $
 *
 *  $Log:   S:/Projects/PHPA2/archives/Finance/Reports/EmployeeAdvance.aspx.cs-arc  $
 * 
 *    Rev 1.42   Jul 22 2008 13:45:16   yjuneja
 * WIP
 * 
 *    Rev 1.41   Jul 09 2008 17:40:56   vraturi
 * PVCS Template Added.
 */
using System;
using System.Linq;
using System.Web.UI.WebControls;
using Eclipse.PhpaLibrary.Reporting;
using Eclipse.PhpaLibrary.Web;


namespace Finance.Reports
{
    public partial class EmployeeAdvance : PageBase
    {
        /// <summary>
        /// Execute the query for the report
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dsEmpAdv_Selecting(object sender, System.Web.UI.WebControls.LinqDataSourceSelectEventArgs e)
        {
            DateTime? toDate = (DateTime?)tbDate.ValueAsDate;

            if (toDate == null)
            {
                e.Cancel = true;
                return;
            }
            ReportingDataContext db = (ReportingDataContext)dsEmpAdv.Database;
            this.Page.Title = string.Format("Employee Advance Outstanding Report as on {0:dd/MM/yyyy}", toDate);
            var voucherDetails = db.RoVoucherDetails.Where(p => p.RoVoucher.VoucherDate <= toDate &&
                //p.RoHeadHierarchy.HeadOfAccountType == "EMPLOYEE_ADVANCE" && p.EmployeeId != null
                HeadOfAccountHelpers.AdvanceSubTypes.EmployeeAdvance.Contains(p.RoHeadHierarchy.HeadOfAccountType) && p.EmployeeId != null
                );

            if (!string.IsNullOrEmpty(tbDivisionCode.Value))
            {
                voucherDetails = voucherDetails.Where(p => p.RoEmployee.DivisionId == Convert.ToInt32(tbDivisionCode.Value));
            }


            e.Result = (from vd in voucherDetails
                        group vd by new
                        {
                            RoEmployee = vd.RoEmployee,
                            HeadOfAccount = vd.HeadOfAccount
                        } into g
                        orderby g.Key.RoEmployee.FirstName, g.Key.RoEmployee.LastName, g.Key.HeadOfAccount.DisplayName
                        select new
                        {
                            //EmployeeCode = g.Key.RoEmployee.EmployeeCode,
                            Employee = g.Key.RoEmployee,
                            EmployeeID = g.Key.RoEmployee.EmployeeId,
                            Division = g.Key.RoEmployee.RoDivision.DivisionName,
                            DivisionId = g.Key.RoEmployee.DivisionId,
                            advanceAmount = (decimal?)g.Key.RoEmployee.RoVoucherDetails.Sum(
                                vd2 => vd2.DebitAmount ?? 0 - vd2.CreditAmount ?? 0),
                            HeadOfAccount = g.Key.HeadOfAccount,
                            HeadOfAccountId = g.Key.HeadOfAccount.HeadOfAccountId
                        }).Where(p => p.advanceAmount != 0);

        }

        protected void grdEmpAdv_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            switch (e.Row.RowType)
            {
                case DataControlRowType.Footer:
                    // Prevents footer from printing on each page
                    e.Row.TableSection = TableRowSection.TableBody;
                    break;
            }
        }
    }
}
