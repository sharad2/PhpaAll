using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Eclipse.PhpaLibrary.Database;
using Eclipse.PhpaLibrary.Web;
using EclipseLibrary.Formatting;
using EclipseLibrary.Web.JQuery;

namespace PhpaAll.MIS.SummaryofFinacialProgress
{
    public partial class PackageActivitySummary : PageBase
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ds_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            PackageActivityDataContext db = (PackageActivityDataContext)ds.Database;

            if (!string.IsNullOrEmpty(ddlPackagesReport.Value))
            {
                var captionText = (from q in db.PackageReports
                                   where q.PackageReportId == Convert.ToInt32(ddlPackagesReport.Value)
                                   select q.Description).FirstOrDefault();

                gv.Caption = string.Format("{0}", captionText);
            }

            var allact = from act in db.PackageActivities
                         where act.PackageActivityGroup.PackageReportId == Convert.ToInt32(ddlPackagesReport.Value)
                         select act;

            IQueryable<PackageActivityTransactionDetail> details =
                from patd in db.PackageActivityTransactionDetails
                where patd.PackageActivityTransaction.PackageReportId == Convert.ToInt32(ddlPackagesReport.Value)
                select patd;

            if (!string.IsNullOrEmpty(tbFromDate.Text))
            {
                details = details.Where(p => p.PackageActivityTransaction.PackageActivityDate >= Convert.ToDateTime(tbFromDate.Text));
            }

            if (!string.IsNullOrEmpty(tbToDate.Text))
            {
                details = details.Where(p => p.PackageActivityTransaction.PackageActivityDate <= Convert.ToDateTime(tbToDate.Text));
            }
            

            // Remarks returned correspond to the latest transaction
            var alldata = from patd in details
                          group patd by new
                          {
                              PackageId = patd.PackageActivityTransaction.PackageId,
                              PackageActivityId = patd.PackageActivityId
                          } into g
                          let latestDate = g.Max(p => p.PackageActivityTransaction.PackageActivityDate)
                          select new
                          {
                              PackageId = g.Key.PackageId,
                              PackageName = g.Max(p => p.PackageActivityTransaction.Package.PackageName),
                              PackageActivityId = g.Key.PackageActivityId,
                              PackageActivtiyData = g.Sum(p => p.PackageActivtiyData),
                              Remarks = g.Where(p => p.PackageActivityTransaction.PackageActivityDate == latestDate)
                                .Select(p => p.PackageActivityTransaction.Remarks).FirstOrDefault()
                          };

            e.Result = from act in allact
                       join data in alldata on act.PackageActivityId equals data.PackageActivityId into g
                       from data in g.DefaultIfEmpty()
                       orderby
                          data.PackageId,
                          act.PackageActivityGroup.Description,
                          act.Description
                       select new
                       {
                           PackageId = (int?)data.PackageId,
                           PackageName = data.PackageName,
                           PackageActivityGroupId = act.PackageActivityGroupId,
                           //GroupDescription = (act.PackageActivityGroup.HeaderTextVisible ?? true) ? act.PackageActivityGroup.Description : string.Empty,
                           GroupDescription = act.PackageActivityGroup.HeaderTextVisible ? act.PackageActivityGroup.Description : string.Empty,
                           ActivityDescription = act.Description,
                           PackageActivityId = act.PackageActivityId,
                           PackageActivtiyData = data.PackageActivtiyData,
                           Remarks = data.Remarks,
                           ActivityColumnNumber = act.ColumnNumber,
                           GroupColumnNumber = act.PackageActivityGroup.ColumnNumber,
                           CalculatedFormula = act.CalculatedFormula,
                           GroupVisible = act.PackageActivityGroup.HeaderTextVisible
                       };

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gv_MatrixRowDataBound(object sender, MatrixRowEventArgs e)
        {
            MatrixField mf = (MatrixField)sender;
            int? packageId = (int?)DataBinder.Eval(e.Row.DataItem, "PackageId");
            if (packageId == null)
            {
                e.Row.Visible = false;
            }
            else
            {
                //Here we  select all activitycolumn number and there corresponding value.
                var allValues = mf.MatrixColumns.Where(p => p.ColumnValues.ContainsKey("ActivityColumnNumber"))
                    .Select(p => new
                    {
                        ColumnNumber = string.Format("col{0}", p.ColumnValues["ActivityColumnNumber"]),
                        ColumnValue = e.MatrixRow.GetCellValue(p) ?? 0
                    }).OrderBy(p => p.ColumnNumber).ToDictionary(p => p.ColumnNumber, p => p.ColumnValue);
                //select all columns formula.
                var formulaColumns = mf.MatrixColumns.Where(p => p.ColumnValues.ContainsKey("CalculatedFormula"))
                    .OrderBy(p => p.ColumnValues["ActivityColumnNumber"]);
                XPathEvaluator nav = new XPathEvaluator(p => allValues[p]);
                foreach (MatrixColumn formulaColumn in formulaColumns)
                {
                    string formula = (string)formulaColumn.ColumnValues["CalculatedFormula"];
                    var obj = nav.Evaluate(formula);
                    int columnNumber = (int)formulaColumn.ColumnValues["ActivityColumnNumber"];
                    allValues[string.Format("col{0}", columnNumber)] = obj;
                    //e.MatrixRow.SetCellText(formulaColumn, string.Format("{0:N2}", obj));
                    e.MatrixRow.SetCellValue(formulaColumn, "PackageActivtiyData", obj);
                }
            }
        }
    }
}
