using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Eclipse.PhpaLibrary.Reporting;
using Eclipse.PhpaLibrary.Web;
using EclipseLibrary.Web.UI;

namespace Finance.Reports
{
    public partial class CED : PageBase
    {
        DateTime date = DateTime.Now.Date;
        string HeadTypeFlag = string.Empty;
        string ReportName = string.Empty;
       
        protected override void OnLoad(EventArgs e)
        {          
            ReportingDataContext db = (ReportingDataContext)dsCED.Database;

            ReportName = Request.QueryString["ReportName"];

            if (string.IsNullOrEmpty(ReportName))
            {
                ReportName = "ED";
            }

            switch (ReportName)
            {
                case "ED":
                    //HeadTypeFlag = "EDGOI,EDRGOB";
                    HeadTypeFlag = HeadOfAccountHelpers.DutySubType.ExciseDutiesGOI.FirstOrDefault().ToString() +","  + HeadOfAccountHelpers.DutySubType.ExciseDutiesRGOB.FirstOrDefault().ToString();
                    Page.Title=string.Format("Central Excise Duty Report as on {0:dd/MM/yyyy}", tbMonth.ValueAsDate);
                    break;

                case "BST":
                    //HeadTypeFlag = "BST";
                    HeadTypeFlag =  HeadOfAccountHelpers.TaxSubTypes.BhutanSalesTax;
                    Page.Title = string.Format("Bhutan Sales Tax Report as on {0:dd/MM/yyyy}", tbMonth.ValueAsDate);
                    break;
            }

            hlHelp.NavigateUrl += string.Format("?ReportName={0}", ReportName);

            if (btnGo.IsPageValid())
            {
                var query = (from job in db.RoJobs
                             where job.PackageName != string.Empty
                             select new
                             {
                                 PackageName = job.PackageName
                             }).Distinct();

                lvCED.DataSource = query;
                lvCED.DataBind();
            }
            base.OnLoad(e);
        }

        protected void lvCED_ItemCreated(object sender, ListViewItemEventArgs e)
        {
            //DateTime date = (DateTime)tbMonth.Date;
            DateTime dateMonthStart = date.MonthStartDate();
            DateTime dateMonthEnd = date.MonthEndDate();
            DateTime dateFinancialYearStart = date.FinancialYearStartDate();
            switch (e.Item.ItemType)
            {
                case ListViewItemType.DataItem:
                    ReportingDataContext db = (ReportingDataContext)dsCED.Database;
                    ListViewDataItem lvdi = (ListViewDataItem)e.Item;
                    GridView gvCED = (GridView)lvdi.FindControl("gvCED");
                    string[] HeadTypeList=HeadTypeFlag.Split(',');
                    var query = from vd in db.RoVoucherDetails
                                where HeadTypeList.Contains(vd.RoHeadHierarchy.HeadOfAccountType) 
                                && vd.RoJob.RoContractor.Nationality == ddlNationality.Value
                                && vd.RoVoucher.VoucherDate <= tbMonth.ValueAsDate
                                && vd.RoJob.PackageName == (DataBinder.Eval(lvdi.DataItem, "PackageName")).ToString()
                                orderby vd.RoHeadHierarchy.DisplayName
                                group vd by vd.RoHeadHierarchy into grp
                                select new
                                {
                                    HeadOfAccountId = grp.Key.HeadOfAccountId,

                                    HeadOfAccount = grp.Key.DisplayName,

                                    DuringMonth = grp.Sum(
                                    vd => vd.RoVoucher.VoucherDate >= dateMonthStart && vd.RoVoucher.VoucherDate <= date ?
                                        vd.DebitAmount ?? 0 - vd.CreditAmount ?? 0 : 0),

                                    DuringYear = grp.Sum(
                                    vd => vd.RoVoucher.VoucherDate >= dateFinancialYearStart && vd.RoVoucher.VoucherDate <= date ?
                                        vd.DebitAmount ?? 0 - vd.CreditAmount ?? 0 : 0),

                                    Cumulative = grp.Sum(
                                    vd => vd.RoVoucher.VoucherDate <= date ?
                                        vd.DebitAmount ?? 0 - vd.CreditAmount ?? 0 : 0)
                                };
                    gvCED.DataSource = query;
                    break;
            }
        }

        protected void gvCED_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            GridView gvCED = (GridView)sender;
            switch (e.Row.RowType)
            {
                case DataControlRowType.Header:
                    DateTime date = tbMonth.ValueAsDate.Value;
                    HyperLinkFieldEx hlDM = (HyperLinkFieldEx)(from DataControlField col in gvCED.Columns
                                           where col.AccessibleHeaderText == "DuringMonth"
                                           select col).Single();
                    hlDM.HeaderToolTip = string.Format("Central Excise Duty from the period from {0:dd MMMM yyyy} to {1:dd MMMM yyyy}", date.MonthStartDate(), date);
                    hlDM.DataNavigateUrlFormatString += string.Format("&DateFrom={0:d}&DateTo={1:d}&AccountTypes={2}", date.MonthStartDate(), date, HeadTypeFlag);
                    HyperLinkFieldEx hlDY = (HyperLinkFieldEx)(from DataControlField col in gvCED.Columns
                          where col.AccessibleHeaderText == "DuringYear"
                                     select col).Single();
                    hlDY.HeaderToolTip = string.Format("Central Excise Duty from the period from {0:dd MMMM yyyy} to {1:dd MMMM yyyy}", date.FinancialYearStartDate(), date);
                    hlDY.DataNavigateUrlFormatString += string.Format(
                        "&DateFrom={0:d}&DateTo={1:d}&AccountTypes={2}",
                        date.FinancialYearStartDate(), date, HeadTypeFlag);
                    break;
            }
        }
    }
}

