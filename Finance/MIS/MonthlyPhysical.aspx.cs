using System;
using System.Globalization;
using System.Linq;
using System.Web.Services;
using System.Web.UI.WebControls;
using Eclipse.PhpaLibrary.Database.MIS;
using Eclipse.PhpaLibrary.Reporting;
using Eclipse.PhpaLibrary.Web;
using EclipseLibrary.Web.JQuery.Input;
using EclipseLibrary.Web.UI;
using System.Web.UI;

namespace Finance.MIS
{
    /// <summary>
    /// Query String: ProgressFormatId,IsEditMode. 
    /// </summary>
    /// <remarks>
    /// Allows you to enter Physical Progress of a month or update it.
    /// Shows you Target Progress set at the begining of the project, quantity executed this
    /// previous month, progress during the month, total quantity executed till now, deviation if it is
    /// more then 30% of target, reason for deviation and remarks.   
    /// </remarks>
    public partial class MonthlyPhysicalFinancialProgress : PageBase
    {
        protected new NestedMIS Master
        {
            get
            {
                return (NestedMIS)base.Master;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="e"></param>
        protected override void OnLoad(EventArgs e)
        {
            //JQueryScriptManager.Current.RegisterScripts(ScriptTypes.WebMethods | ScriptTypes.Json);
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
            foreach (DataControlField col in gv.Columns)
            {
                switch (col.AccessibleHeaderText)
                {
                    case "ReadOnly":
                        col.Visible = !this.Master.IsEditMode;
                        break;

                    case "EditOnly":
                        col.Visible = this.Master.IsEditMode;
                        break;

                    default:
                        break;
                }
            }
            // Explicit data binding to ensure that printer friendly page works.
            ddlProgressformat.DataBind();
            base.OnLoad(e);
        }


        /// <summary>
        /// This method use for following purpose
        /// 1.set where parameters.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dsProgressFormat_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            MISDataContext db = (MISDataContext)dsProgressFormat.Database;
            e.Result = from fmt in db.ProgressFormats
                       where fmt.ProgressFormatType == Convert.ToInt32(ProgressFormat.FormatType.MonthlyPhysicalFinancial) &&
                       fmt.PackageId == this.Master.PackageId
                       select new
                       {
                           ProgressFormatId = fmt.ProgressFormatId,
                           DisplayName = fmt.ProgressFormatName + " (" + fmt.Description + ")"
                       };
        }

        /// <summary>
        /// Refresh data
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnGo_Click(object sender, EventArgs e)
        {
            gv.DataBind();
        }

        /// <summary>
        /// This method use for following purpose
        /// 1.Bind the gridviewex. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dsPhysicalReport_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            if (string.IsNullOrEmpty(ddlProgressDate.Value))
            {
                e.Cancel = true;
                return;
            }

            MISDataContext db = (MISDataContext)dsPhysicalReport.Database;
            int progressFormatId = Convert.ToInt32(ddlProgressformat.Value);
            IQueryable<Activity> allActivites = from act in db.Activities
                               where act.ProgressFormatId == progressFormatId
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

            // ddlProgressDate.Value = string.Format("{0:M/yyyy}", selectedDate);
            // No data exists yet for the selected date
            hfDate.Value = ddlProgressDate.Value;
            hfProgressformat.Value = ddlProgressformat.Value;

            DateTime monthStartDate = selectedDate.MonthStartDate();
            var dataThisMonth = from fad in db.FormatActivityDetails
                                where fad.FormatDetail.ProgressFormatId == progressFormatId
                                && fad.FormatDetail.ProgressDate == selectedDate
                                select fad;

            var query2 = (from actAll in allActivites
                         let monthStartToDate = db.FormatActivityDetails
                            .Where(p => p.FormatDetail.ProgressDate < monthStartDate
                                   && p.FormatDetail.ProgressFormatId == progressFormatId
                                   && p.ActivityId == actAll.ActivityId)
                            .Sum(p => p.PhysicalProgressData)
                         join actData in dataThisMonth on actAll.ActivityId equals actData.ActivityId into g
                         from actData in g.DefaultIfEmpty()
                         orderby actAll.ActivityCode
                         let totalQuantity = (actData.PhysicalProgressData ?? 0) + (monthStartToDate ?? 0)
                         //let boqPlus30 = actAll.PhysicalTarget * 1.3M
                         select new
                         {
                             ActivityId = actAll.ActivityId,
                             FormatActivityDetailId = (int?)actData.FormatActivityDetailId,
                             ItemNumber = actAll.DisplayActivityCode,
                             Description = actAll.Description,
                             Unit = actAll.UOM,
                             AsPerBOQ = actAll.PhysicalTarget,
                             ThisMonth = actData.PhysicalProgressData,
                             PreviousMonth = monthStartToDate,
                             TotalQuantity = totalQuantity,
                             BoQ30 = actAll.PhysicalTarget * 1.3M > totalQuantity ? totalQuantity : actAll.PhysicalTarget * 1.3M,
                             Deviation = totalQuantity - actAll.PhysicalTarget * 1.3M,
                             ReasonForDeviation = actData.ReasonForDeviation,
                             Remarks = actData.Remarks,
                             IsTopLevel = actAll.IsTopLevel
                         }).Take(maxRows);
            e.Result = query2;

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
                    bool b = (bool)DataBinder.Eval(e.Row.DataItem, "IsTopLevel");
                    if (b)
                    {
                        e.Row.CssClass += " ui-priority-primary";
                    }
                    break;
            }
        }

        /// <summary>
        /// This method use for following purpose
        /// 1.enable and disable save button. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gv_DataBound(object sender, EventArgs e)
        {
            if (gv.Rows.Count > 0 && this.Master.IsEditMode)
            {
                btnSaveProgress.Visible = true;
                btnSaveProgress1.Visible = true;
            }
            else
            {
                btnSaveProgress.Visible = false;
                btnSaveProgress1.Visible = false;
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
                hlDataEntry.Text = "Monthly Physical Report";
            }
            else
            {
                hlDataEntry.Text = "Monthly Physical Data Entry";
                hlDataEntry.NavigateUrl += "&IsEditMode=true";
            }
            gv.Caption = string.Format("{0}<br/>{1}",
                this.Master.PackageName, ddlProgressformat.SelectedItem.Text);

        }

        protected void btnSaveProgress_Click(object sender, EventArgs e)
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
                                                 //&& fd.ProgressDate.Year == Convert.ToDateTime(hfDate.Value).Year
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
            for (int r=0; r<=gv.Rows.Count-1; r++)
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
                TextArea taReasonForDeviation = (TextArea)gv.Rows[r].FindControl("taReasonForDeviation");
                string reasonForDeviation = taReasonForDeviation.Text;
                if (formatActivityDetailid == null)
                {
                    // Insert
                    fad = new FormatActivityDetail()
                    {
                        ActivityId = activityId,
                        PhysicalProgressData = progressData,
                        ReasonForDeviation = reasonForDeviation,
                        Remarks = remarks

                    };
                    formatDetail.FormatActivityDetails.Add(fad);
                }
                else
                {
                    fad = formatDetail.FormatActivityDetails
                        .Where(p => p.FormatActivityDetailId == formatActivityDetailid.Value)
                        .Single();
                    fad.PhysicalProgressData = progressData;
                    fad.ReasonForDeviation = reasonForDeviation;
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
                                Count = formatdetail.FormatActivityDetails.Where(p => p.PhysicalProgressData != null).Count()
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
