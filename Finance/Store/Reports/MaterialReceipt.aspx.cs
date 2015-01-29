using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI.WebControls;
using Eclipse.PhpaLibrary.Database.Store;
using Eclipse.PhpaLibrary.Reporting;
using Eclipse.PhpaLibrary.Web;
using EclipseLibrary.Web.JQuery.Input;


namespace PhpaAll.Store.Reports
{
    public partial class MaterialReceipt : PageBase
    {
        private string m_reportName;

        /// <summary>
        /// Set the title for Material rejected report.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            m_reportName = Request.QueryString["ReportName"];
            switch (m_reportName)
            {
                case "Rejected":
                    Page.Title = "Material Rejected per GRN";
                    break;
            }
        }


        /// <summary>
        /// Creating dynamic where clause.
        /// Pass the Query String values.
        /// Querying when Report name passed and not passed.
        /// Show default list when no where parameter paased.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dsGRNReceipt_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            if(!btnGo.IsPageValid())
            {
                e.Cancel = true;
                return;
            }
            StoreDataContext db = (StoreDataContext)dsGRNReceipt.Database;
            var query = (from gd in db.GRNItems
                         where gd.AcceptedQty != null
                         && gd.GRN.GRNReceiveDate >= tbFromDate.ValueAsDate
                         && gd.GRN.GRNReceiveDate <= tbToDate.ValueAsDate
                         
                         select new
                         {
                             ItemId = gd.ItemId,
                             grnId = gd.GRN.GRNId,
                             grn = gd.GRN,
                             ItemCode = gd.Item.ItemCode,
                             Brand = gd.Item.Brand,
                             Identifier = gd.Item.Identifier,
                             Size = gd.Item.Size,
                             ItemDescription = gd.Item.Description,
                             ItemCategoryId = gd.Item.ItemCategoryId,
                             ItemCategoryCode = gd.Item.ItemCategory.ItemCategoryCode,
                             CatDescription = gd.Item.ItemCategory.Description,
                             GRNNo = gd.GRN.GRNCode,
                             PONo = gd.GRN.PONumber,
                             RecieveDate = gd.GRN.GRNReceiveDate,
                             AcceptedQty = gd.AcceptedQty,
                             ReceivedQty = gd.ReceivedQty,
                             TotalPrice = (gd.Price ?? 0) + (gd.LandedPrice ?? 0),
                             RejectedQty = gd.ReceivedQty - gd.AcceptedQty,
                             TotalAccepted = gd.AcceptedQty * ((gd.Price ?? 0) + (gd.LandedPrice ?? 0)),
                             TotalRejected = (gd.ReceivedQty - gd.AcceptedQty) * ((gd.Price ?? 0) + (gd.LandedPrice ?? 0))
                         });//.OrderByDescending(p => p.RecieveDate).ThenBy(p => p.ItemDescription);

            if (!string.IsNullOrEmpty(m_reportName) && m_reportName.Equals("Rejected"))
            {
                query = query.Where(p => p.RejectedQty > 0);
            }
            if (!string.IsNullOrEmpty(tbItem.Value))
            {
                query = query.Where(p => p.ItemId == Convert.ToInt32(tbItem.Value))
                                .OrderByDescending(p => p.RecieveDate).ThenBy(p => p.ItemDescription);
            }
            if (!string.IsNullOrEmpty(ddlCategory.Value))
            {
                query = query.Where(p => p.ItemCategoryId == Convert.ToInt32(ddlCategory.Value))
                                .OrderByDescending(p => p.RecieveDate).ThenBy(p => p.ItemDescription);
            }
            

            e.Result = query;
        }


        protected void gvReceipt_DataBinding(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(m_reportName) && m_reportName.Equals("Rejected"))
            {
                DataControlField columnAQ = (from DataControlField col in gvReceipt.Columns
                                             where col.AccessibleHeaderText == "AcceptedQty"
                                             select col).Single();
                DataControlField columnTotAccepted = (from DataControlField col in gvReceipt.Columns
                                                      where col.AccessibleHeaderText == "TotalAccepted"
                                                      select col).Single();
                columnAQ.Visible = false;
                columnTotAccepted.Visible = false;
            }
            else
            {
                DataControlField columnRQ = (from DataControlField col in gvReceipt.Columns
                                             where col.AccessibleHeaderText == "RejectedQty"
                                             select col).Single();
                DataControlField columnTotRejected = (from DataControlField col in gvReceipt.Columns
                                                      where col.AccessibleHeaderText == "TotalRejected"
                                                      select col).Single();
                columnRQ.Visible = false;
                columnTotRejected.Visible = false;
            }
        }

        /// <summary>
        /// Set the caption of the grid according to the Parameters passed or seleced.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvReceipt_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            switch (e.Row.RowType)
            {
                case DataControlRowType.Header:
                    if (!string.IsNullOrEmpty(this.Request.QueryString["ReportName"]))
                    {
                        gvReceipt.Caption = string.Format("<b>Material rejected summary from {0: dd MMMM yyyy} to {1:dd MMMM yyyy}</b>", tbFromDate.ValueAsDate, tbToDate.ValueAsDate);
                    }
                    else if (tbFromDate.ValueAsDate != null)
                    {
                        gvReceipt.Caption = string.Format("<b>Material receipt summary from {0: dd MMMM yyyy} to {1:dd MMMM yyyy}</b>", tbFromDate.ValueAsDate, tbToDate.ValueAsDate);
                    }
                    else
                    {
                        gvReceipt.Caption = string.Format("<b>Material receipt summary upto {0:dd MMMM yyyy}</b>", tbToDate.ValueAsDate);
                    }
                    break;
                case DataControlRowType.Footer:
                    // Prevents printing of the footer on each page
                    e.Row.TableSection = TableRowSection.TableBody;
                    break;
            }
        }
    }
}
