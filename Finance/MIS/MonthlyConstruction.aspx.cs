using System;
using System.Globalization;
using System.Linq;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using Eclipse.PhpaLibrary.Database.MIS;
using Eclipse.PhpaLibrary.Reporting;
using Eclipse.PhpaLibrary.Web;
using EclipseLibrary.Web.JQuery.Input;
using EclipseLibrary.Web.UI;

namespace PhpaAll.MIS
{
    /// <summary>
    /// Query String :ProgressFormatId, IsEditMode.
    /// </summary>
    public partial class MonthlyConstructionProgress : PageBase
    {
        protected new NestedMIS Master
        {
            get
            {
                return (NestedMIS)base.Master;
            }
        }
        /// <summary>
        /// this property  use for following purpose
        /// 1.this month and Remarks coulumn from gridviewex enable and disable
        /// depaned on query string value. 
        /// </summary>
        //public bool IsEditMode
        //{
        //    get
        //    {
        //        return !string.IsNullOrEmpty(this.Request.QueryString["IsEditMode"]);
        //    }
        //}
        /// <summary>
        /// This method use for following purpose
        /// 1.Register web method.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnLoad(EventArgs e)
        {
            //ddlProgressDate.TextBox.DatePickerOptions.Add("dateFormat", "m/yy");
            if (this.Master.IsEditMode)
            {
                Page.Title += " - Data Entry";
                pfb.Visible = false;
            }
            else
            {
                Page.EnableViewState = false ;
                lblStartsWith.RowVisible = false;
            }

            if (!string.IsNullOrEmpty(ddlProgressDate.Value))
            {
                Page.Title += string.Format(" for {0:MMMM yyyy}", DateTime.Parse(ddlProgressDate.Value));
            }
            int Today = DateTime.Now.Day;
            int ComingMonthStartDate = DateTime.Now.MonthEndDate().Day;
            Date dt = (Date)ddlProgressDate.TextBox.Validators[0];
            dt.Max = ComingMonthStartDate - Today;

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
      
        protected void dsProgressFormat_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            MISDataContext db = (MISDataContext)dsProgressFormat.Database;
            e.Result = from fmt in db.ProgressFormats
                       where fmt.ProgressFormatType == Convert.ToInt32(ProgressFormat.FormatType.MonthlyConstruction) &&
                       fmt.PackageId == this.Master.PackageId
                       select new
                       {
                           ProgressFormatId = fmt.ProgressFormatId,
                           DisplayName = fmt.ProgressFormatName + " (" + fmt.Description + ")"
                       };
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
                        orderby ac1.ActivityCode
                        let monthStartToDate = db.FormatActivityDetails
                                                 .Where(p => p.FormatDetail.ProgressDate < monthStartDate
                                                         && p.ActivityId == ac1.ActivityId
                                                         && p.FormatDetail.ProgressFormatId == progressFormatId)
                                                         .Sum(p => p.PhysicalProgressData)
                        select new
                        {
                            ActivityId = ac1.ActivityId,
                            FormatActivityDetailId = (int?)ac2.FormatActivityDetailId,
                            ItemNumber = ac1.DisplayActivityCode,
                            Description = ac1.Description,
                            UOM = ac1.UOM,
                            PhysicalTarget = ac1.PhysicalTarget,
                            PreMonth = monthStartToDate,
                            ThisMonth = ac2.PhysicalProgressData,
                            TotalExpenditure = (monthStartToDate ?? 0) + (ac2.PhysicalProgressData ?? 0),
                            Remarks = ac2.Remarks,
                            IsTopLevel = ac1.IsTopLevel
                        }).Take(maxRows);
            e.Result = query;
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
                        PhysicalProgressData = progressData,
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
                    fad.Remarks = remarks;
                }
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
                btnSave1.Visible = true;
                btnSave.Visible = true;
            }
            else
            {
                btnSave1.Visible = false;
                btnSave.Visible = false;
            }
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
                hlDataEntry.Text = "Monthly Construction Report";
            }
            else
            {
                hlDataEntry.Text = "Monthly Construction Data Entry";
                hlDataEntry.NavigateUrl += "&IsEditMode=true";
            }
            gv.Caption = string.Format("{0}<br/>{1}",
                this.Master.PackageName, ddlProgressformat.SelectedItem.Text);

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
                                Count = formatdetail.FormatActivityDetails.Count()
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
