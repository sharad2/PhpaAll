/*
 *  Programming: Eclipse Systems (P) Ltd., NOIDA, INDIA
 *  E-mail: support@eclsys.com
 *
 *  $Workfile:   PendingAdvance.aspx.cs  $
 *  $Revision: 36979 $
 *  $Author: ssinghal $
 *  $Date: 2010-11-03 16:20:17 +0530 (Wed, 03 Nov 2010) $
 *  $Modtime:   Jul 22 2008 13:10:48  $
 *
 *  $Log:   S:/Projects/PHPA2/archives/Finance/Reports/PendingAdvance.aspx.cs-arc  $
 * 
 *    Rev 1.34   Jul 22 2008 13:10:50   pshishodia
 * WIP
 * 
 *    Rev 1.33   Jul 09 2008 17:40:58   vraturi
 * PVCS Template Added.
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Eclipse.PhpaLibrary.Reporting;
using Eclipse.PhpaLibrary.Web;


namespace Finance.Reports
{
    public partial class PendingAdvance : PageBase
    {

        /// <summary>
        /// select master(Division group) for report.
        /// </summary>
        /// <param name="e"></param>
        ///
        decimal m_TotalAdvance;
        protected override void OnLoad(EventArgs e)
        {

            ReportingDataContext db = (ReportingDataContext)dsPendingAdv.Database;

            var groupQuery = (from div in db.RoDivisions
                              orderby div.DivisionGroup
                              select new
                              {
                                  DivisionGroup = div.DivisionGroup
                              }).Distinct();


            lvPendingAdv.DataSource = groupQuery;
            lvPendingAdv.DataBind();

            base.OnLoad(e);
        }
        /// <summary>
        /// Select details for each master
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lvPendingAdv_ItemCreated(object sender, ListViewItemEventArgs e)
        {

            // List of Account head which are used as a filter in 'DivisionalExpences' report.
            //string[] divisionExpencesAccountHead = { "EXPENDITURE", "TOUR_EXPENSES" };
            // List of Account head which are used as a filter in 'PendingAdvance' report.
            //string[] divisionAdvanceAccountHead = { "EMPLOYEE_ADVANCE", "PARTY_ADVANCE", "MATERIAL_ADVANCE" };
            List<string> headOfAccountList = new List<string>();


            switch (e.Item.ItemType)
            {
                case ListViewItemType.DataItem:
                    ReportingDataContext db = (ReportingDataContext)dsPendingAdv.Database;
                    ListViewDataItem lvdi = (ListViewDataItem)e.Item;
                    GridView grdPendingAdv = (GridView)lvdi.FindControl("grdPendingAdv");
                    //int columnIndex = grdPendingAdv.Columns.FindByAccessibleHeaderText("AmountField");
                    DataControlField columnIndex = (from DataControlField col in grdPendingAdv.Columns
                                                    where col.AccessibleHeaderText == "AmountField"
                                                    select col).Single();
                    string reportName = Request.QueryString["ReportName"];
                    //Set the Header and head of Account list depending on Selected report
                    if (string.IsNullOrEmpty(reportName))
                    {
                        reportName = "PendingAdvances";
                    }
                    switch (reportName)
                    {
                        case "DivisionExpenditure":
                            //grdPendingAdv.Columns[columnIndex].HeaderText = "Expenditure (Nu.)";
                            columnIndex.HeaderText = "Expenditure (Nu.)";
                            headOfAccountList.AddRange(HeadOfAccountHelpers.AllExpenditures);
                            //lblHelp.Text = "Displays ependitures in each division.";
                            this.Title = "Divisional Expenses";
                            lblReportDescription.Text = "Displays ependitures in each division.";
                            break;

                        case "PendingAdvances":
                            //grdPendingAdv.Columns[columnIndex].HeaderText = "Outstanding Advances (Nu.)";
                            columnIndex.HeaderText = "Outstanding Advances (Nu.)";
                            headOfAccountList.AddRange(HeadOfAccountHelpers.AllAdvances);
                            //lblHelp.Text = "Displays outstanding Advance amount of All Employees and Contractors in each division.";
                            this.Title = "Pending Advances";
                            lblReportDescription.Text = "Displays outstanding Advance amount of All Employees and Contractors in each division.";
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }

                    var query = from division in db.RoDivisions
                                where division.DivisionGroup == DataBinder.Eval(lvdi.DataItem, "DivisionGroup").ToString()
                                orderby division.DivisionCode
                                select new
                                {
                                    DivisionId = division.DivisionId,
                                    DivisionCode = division.DivisionCode,
                                    DivisionName = division.DivisionName,
                                    AdvanceAmount = (decimal?)division.RoVouchers.Sum(
                                    p => p.RoVoucherDetails.Sum(q => headOfAccountList.Contains(q.RoHeadHierarchy.HeadOfAccountType)

                                        ? (q.DebitAmount ?? 0 - q.CreditAmount ?? 0) : 0))
                                };

                    grdPendingAdv.DataSource = query;
                    grdPendingAdv.DataBind();
                    break;
            }
        }


        /// <summary>
        /// Calculate grand total.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdPendingAdv_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            switch (e.Row.RowType)
            {
                case DataControlRowType.DataRow:
                    object amt = DataBinder.Eval(e.Row.DataItem, "AdvanceAmount");
                    if (amt != null)
                    {
                        m_TotalAdvance += (decimal)amt;
                    }

                    break;


            }
        }

        /// <summary>
        /// Displays  grand total in footer.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lvPendingAdv_DataBound(object sender, EventArgs e)
        {
            Label lbltotal = (Label)lvPendingAdv.FindControl("lblTotal");
            //LabelLabel lbltotal =(LabelLabel) lvPendingAdv.FindControl("lblTotal");
            if (lbltotal != null)
            {
                if (m_TotalAdvance < 0)
                {
                    lbltotal.Text = string.Format("({0})", Math.Abs(m_TotalAdvance));
                }
                else
                {
                    lbltotal.Text = m_TotalAdvance.ToString();
                }
            }
        }
    }
}

