using System;
using System.Globalization;
using System.Linq;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using Eclipse.PhpaLibrary.Database.MIS;
using Eclipse.PhpaLibrary.Reporting;
using Eclipse.PhpaLibrary.Web;
using EclipseLibrary.Web.JQuery;
using EclipseLibrary.Web.JQuery.Input;
using EclipseLibrary.Web.UI;
using System.Collections.Generic;

namespace Finance.MIS
{
    /// <summary>
    /// Query string : ProgressFormatId,IsEditMode.
    /// </summary>
    public partial class FinancialProgress : PageBase
    {
        protected new NestedMIS Master
        {
            get
            {
                return (NestedMIS)base.Master;
            }
        }


        protected override void OnLoad(EventArgs e)
        {
            // ddlProgressDate.TextBox.DatePickerOptions.Add("dateFormat", "m/yy");
            if (this.Master.IsEditMode)
            {
                Page.Title += " - Data Entry";
                pfb.Visible = false;
            }
            else
            {
                Page.EnableViewState = false;
                lblStartsWith.RowVisible = false;
            }
            if (!string.IsNullOrEmpty(ddlProgressDate.Value))
            {
                Page.Title += string.Format(" for {0:MMMM yyyy}", DateTime.Parse(ddlProgressDate.Value));
            }
            // Data entry for future months not allowed
            int monthStartDate = DateTime.Now.MonthEndDate().Day;
            Date dt = (Date)ddlProgressDate.TextBox.Validators[0];
            dt.Max = monthStartDate - DateTime.Now.Day;

            // Hide/Show grid columns
            List<DataControlField> colsToRemove = new List<DataControlField>();
            foreach (DataControlField col in gv.Columns)
            {
                switch (col.AccessibleHeaderText)
                {
                    case "ReadOnly":
                        //col.Visible = !this.Master.IsEditMode;
                        if (this.Master.IsEditMode)
                        {

                            colsToRemove.Add(col);
                        }
                        break;

                    case "EditOnly":
                        //col.Visible = this.Master.IsEditMode;
                        if (!this.Master.IsEditMode)
                        {

                            colsToRemove.Add(col);
                        }
                        break;

                    default:
                        break;
                }
            }

            foreach (DataControlField col in colsToRemove)
            {
                //gv.Columns.Remove(col);
                col.Visible = false;
            }
            // Explicit data binding to ensure that printer friendly page works.
            ddlProgressformat.DataBind();
            base.OnLoad(e);
        }

        protected void dsProgressFormat_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            MISDataContext db = (MISDataContext)dsProgressFormat.Database;
            e.Result = from fmt in db.ProgressFormats
                       where fmt.ProgressFormatType == Convert.ToInt32(ProgressFormat.FormatType.MonthlyPhysicalFinancial) &&
                       fmt.PackageId == this.Master.PackageId
                       select new
                       {
                           ProgressFormatId = fmt.ProgressFormatId,
                           DisplayName = fmt.FinancialFormatName + " (" + fmt.Description + ")"
                       };
        }

        /// <summary>
        /// Build the query for grid view
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ds_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            if (string.IsNullOrEmpty(ddlProgressDate.Value))
            {
                e.Cancel = true;
                return;
            }

            MISDataContext db = (MISDataContext)ds.Database;
            int progressFormatId = Convert.ToInt32(ddlProgressformat.Value);

            var allActivites = from act in db.Activities
                               where act.ProgressFormat.ProgressFormatId == progressFormatId
                               select act;

            int maxRows = int.MaxValue;

            if (string.IsNullOrEmpty(tbStartsWith.Text))
            {
                if (this.Master.IsEditMode)
                {
                    maxRows = 50;
                }
            }
            else
            {
                string str = Activity.PadActivityCode(tbStartsWith.Text);
                allActivites = allActivites.Where(p => p.ActivityCode.StartsWith(str));
            }
            DateTime selectedDate = DateTime.Parse(ddlProgressDate.Value, CultureInfo.CurrentUICulture).MonthEndDate();

            hfDate.Value = ddlProgressDate.Value;
            hfProgressformat.Value = ddlProgressformat.Value;

            DateTime monthStartDate = selectedDate.MonthStartDate();

            var dataThisMonth = from fad in db.FormatActivityDetails
                                where fad.FormatDetail.ProgressFormatId == progressFormatId
                                && fad.FormatDetail.ProgressDate == selectedDate
                                select fad;

            var query = (from ac1 in allActivites
                        join ac2 in dataThisMonth on ac1.ActivityId equals ac2.ActivityId into g
                        from ac2 in g.DefaultIfEmpty()
                        let Rate = ac1.FinalRate ?? ac1.ProvisionalRate ?? ac1.BoqRate
                        orderby ac1.ActivityCode
                        let previousMonth = db.FormatActivityDetails
                                               .Where(p => p.FormatDetail.ProgressDate < monthStartDate
                                                       && p.ActivityId == ac1.ActivityId
                                                       && p.FormatDetail.ProgressFormatId == progressFormatId)
                                                       .Sum(p => p.FinancialProgressData)
                        select new
                        {
                            ActivityId = ac1.ActivityId,
                            FormatActivityDetailId = (int?)ac2.FormatActivityDetailId,
                            ItemNumber = ac1.DisplayActivityCode,
                            Description = ac1.Description,
                            FinancialTarget = ac1.FinancialTarget,
                            FinancialPreMonth = previousMonth,
                            TotalExpenditure = (previousMonth ?? 0) + (ac2.FinancialProgressData ?? 0),
                            Remarks = ac2.Remarks,
                            ProgressData = ac2.FinancialProgressData,
                            Rate = ac1.FinalRate ?? ac1.ProvisionalRate ?? ac1.BoqRate,
                            BoqRate = ac1.BoqRate,
                            ProvisionalRate = ac1.ProvisionalRate,
                            FinalRate = ac1.FinalRate,
                            PhysicalProgressData = ac2.PhysicalProgressData,
                            PhysicalUnit = ac2.Activity.UOM,
                            PhysicalPreMonth = db.FormatActivityDetails
                                               .Where(p => p.FormatDetail.ProgressDate < monthStartDate
                                                       && p.ActivityId == ac1.ActivityId
                                                       && p.FormatDetail.ProgressFormatId == progressFormatId)
                                                       .Sum(p => p.PhysicalProgressData),
                            SavedByUser = ac2.ModifiedBy ?? ac2.CreatedBy,
                            SavedByDate = ac2.Modified ?? ac2.Created,
                            PhysicalTarget = ac1.PhysicalTarget
                        }).Take(maxRows);

            e.Result = query;

        }

        protected void gv_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            switch (e.Row.RowType)
            {
                case DataControlRowType.DataRow:
                    // This works around the problem of jquery script being generated for controls in invisible columns
                    for (int i = 0; i < gv.Columns.Count; ++i)
                    {
                        if (!gv.Columns[i].Visible)
                        {
                            e.Row.Cells[i].Visible = false;
                        }
                    }

                    if (!this.Master.IsEditMode)
                    {
                        // Nothing to propose in edit mode
                        break;
                    }
                    decimal? financialProgressData = Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "ProgressData"));
                    TextBoxEx tb = (TextBoxEx)e.Row.FindControl("tbProgressData");
                    if (financialProgressData != null && financialProgressData != 0)
                    {
                        // Data has already been saved
                        tb.ToolTip = string.Format("Saved by {0} on {1}",
                            DataBinder.Eval(e.Row.DataItem, "SavedByUser"),
                            DataBinder.Eval(e.Row.DataItem, "SavedByDate"));
                        break;
                    }

                    decimal? physicalThisMonth = (decimal?)DataBinder.Eval(e.Row.DataItem, "PhysicalProgressData");
                    if (physicalThisMonth == null)
                    {
                        // No proposal
                        break;
                    }

                    /// If no bo
                    decimal boqRate = ((decimal?)DataBinder.Eval(e.Row.DataItem, "BoqRate"))?? 0;
                    //decimal financialTarget = ((decimal?)DataBinder.Eval(e.Row.DataItem, "FinancialTarget"))??0;
                    //if (boqRate == null || financialTarget == null)
                    //{
                    //    // No proposal
                    //    break;
                    //}

                    decimal physicalTarget = ((decimal?)DataBinder.Eval(e.Row.DataItem, "PhysicalTarget")) ?? 0;

                    physicalTarget = physicalTarget * 1.3M;     // Add 30 %
                    decimal? provisionalRate = (decimal?)DataBinder.Eval(e.Row.DataItem, "ProvisionalRate");
                    decimal? finalRate = (decimal?)DataBinder.Eval(e.Row.DataItem, "FinalRate");

                    tb.AddCssClass("ui-selected");

                    decimal physicalPreMonth = ((decimal?)DataBinder.Eval(e.Row.DataItem, "PhysicalPreMonth")) ?? 0;
                    decimal deviationRate = finalRate ?? provisionalRate ?? boqRate;
                    decimal proposedValue;
                    if (physicalPreMonth > physicalTarget)
                    {
                        // Target already exceeded
                        proposedValue = deviationRate * physicalThisMonth.Value;
                        tb.ToolTip = string.Format("Exceeded (target + 30%): Latest Rate * Physical Progress this month " +
                        "{0:N2} * {1:N4} = {2:N2}",
                            deviationRate, physicalThisMonth, proposedValue);
                    }
                    else if (physicalPreMonth + physicalThisMonth > physicalTarget)
                    {
                        // Target exceeding this month
                        proposedValue = (physicalTarget - physicalPreMonth) * boqRate +
                            (physicalPreMonth + physicalThisMonth.Value - physicalTarget) * deviationRate;
                        tb.ToolTip = string.Format("Exceeding (target + 30%): BOQ Rate * ((physical target + 30%) - physical previous month) + Excess * Latest Rate = " +
                        "{0:N2} * ({1:N2} - {2:N4}) + ({2:N4} + {3:N4} - {1:N4}) * {4:N2} = {5:N2}",
                            boqRate, physicalTarget, physicalPreMonth, physicalThisMonth, deviationRate, proposedValue);
                    }
                    else
                    {
                        // Target will not exceed
                        proposedValue = boqRate * physicalThisMonth.Value;
                        tb.ToolTip = string.Format("Below (target + 30%): BOQ Rate * Physical Progress this month = {0:N2} * {1:N4} = {2:N2}",
                            boqRate, physicalThisMonth, proposedValue);
                    }
                    tb.Text = string.Format("{0:#.##}", proposedValue);

                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// If new progress date chosen, insert new format detail
        /// Allow only one entry for a month. Otherwise raise error.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnGo_Click(object sender, EventArgs e)
        {
            gv.DataBind();
        }

        /// <summary>
        /// This method use for following purpose
        /// 1.enable and disable save button. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gv_DataBound(object sender, EventArgs e)
        {

            // Disable if no rows or not in edit mode
            if (gv.Rows.Count > 0 && this.Master.IsEditMode)
            {
                btnSave1.Visible = true;
                btnSave.Visible = true;
            }
            else
            {
                btnSave1.Visible = false;
                btnSave.Visible = false;
            }
        }

        /// <summary>
        /// Pass query string to data entry link
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPreRenderComplete(EventArgs e)
        {
            base.OnPreRenderComplete(e);
            hlDataEntry.NavigateUrl += string.Format("?ProgressFormatId={0}&ProgressDate={1:d}",
                ddlProgressformat.Value, ddlProgressDate.Value);
            if (this.Master.IsEditMode)
            {
                hlDataEntry.Text = "Monthly Financial Report";
            }
            else
            {
                hlDataEntry.Text = "Monthly Financial Data Entry";
                hlDataEntry.NavigateUrl += "&IsEditMode=true";
            }
            gv.Caption = string.Format("{0}<br/>{1}",
                this.Master.PackageName, ddlProgressformat.SelectedItem.Text);
        }



        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (gv.SelectedIndexes.Count < 0)
            {
                return;
            }
            DateTime progressDate = Convert.ToDateTime(hfDate.Value).MonthEndDate();
            int progressFormatId = Convert.ToInt32(hfProgressformat.Value);
            try
            {
                using (MISDataContext db = new MISDataContext(ReportingUtilities.DefaultConnectString))
                {
                    FormatDetail formatDetail = (from fd in db.FormatDetails
                                                 where fd.ProgressFormatId == progressFormatId
                                                 && fd.ProgressDate == progressDate
                                                 select fd).SingleOrDefault();
                    if (formatDetail == null)
                    {
                        formatDetail = new FormatDetail()
                        {
                            ProgressFormatId = progressFormatId,
                            ProgressDate = progressDate
                        };
                        db.FormatDetails.InsertOnSubmit(formatDetail);
                    }
                    UpdateActivityDetail(formatDetail);
                    db.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                sp.AddErrorText(ex.Message);
            }
            finally
            {
                gv.DataBind();
            }

        }

        protected void UpdateActivityDetail(FormatDetail formatDetail)
        {
            for (int r=0;r<=gv.Rows.Count-1;r++)
            {
                FormatActivityDetail fad;
                int? formatActivityDetailid = (int?)gv.DataKeys[r]["FormatActivityDetailId"];
                int activityId = (int)gv.DataKeys[r]["ActivityId"];
                decimal? progressData = null;
                TextBoxEx tbProgressData = (TextBoxEx)gv.Rows[r].FindControl("tbProgressData");
                if (!string.IsNullOrEmpty(tbProgressData.Text))
                {
                    progressData = Convert.ToDecimal(tbProgressData.Text);
                }
                TextArea taRemarks = (TextArea)gv.Rows[r].FindControl("taRemarks");
                string remarks = taRemarks.Text;
                if (formatActivityDetailid == null)
                {
                    // Insert
                    fad = new FormatActivityDetail()
                    {
                        ActivityId = activityId,
                        FinancialProgressData = progressData,
                        Remarks = remarks

                    };
                    formatDetail.FormatActivityDetails.Add(fad);
                }
                else
                {
                    fad = formatDetail.FormatActivityDetails
                        .Where(p => p.FormatActivityDetailId == formatActivityDetailid.Value)
                        .Single();
                    fad.FinancialProgressData = progressData;
                    fad.Remarks = remarks;
                }
            }

        }

        /// <summary>
        /// This method use for follownig purpose
        ///1.Return all format details for the passed progress format
        /// </summary>
        /// <param name="parentKeys"></param>
        /// <returns></returns>
        ///       

        [WebMethod]
        public static DropDownItem[] GetProgressDate(string[] parentKeys)
        {
            using (MISDataContext db = new MISDataContext(ReportingUtilities.DefaultConnectString))
            {
                var query = from formatdetail in db.FormatDetails
                            where formatdetail.ProgressFormatId == int.Parse(parentKeys[0])
                            orderby formatdetail.ProgressDate descending
                            select new
                            {
                                ProgressDate = formatdetail.ProgressDate,
                                FormatDetailId = formatdetail.FormatDetailId,
                                Count = formatdetail.FormatActivityDetails.Where(p => p.FinancialProgressData != null).Count()
                            };

                var newResults = (from r in query
                                  select new DropDownItem()
                                  {
                                      Text = string.Format("{0:y} ({1:N0} entries)", r.ProgressDate, r.Count),
                                      Value = string.Format(PageBase.Culture, "{0:d}", r.ProgressDate)
                                  }).ToArray();


                return newResults;
            }

        }

    }
}
