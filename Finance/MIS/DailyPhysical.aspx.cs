using System;
using System.Linq;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;
using Eclipse.PhpaLibrary.Database.MIS;
using Eclipse.PhpaLibrary.Reporting;
using Eclipse.PhpaLibrary.Web;
using EclipseLibrary.Web.Extensions;
using EclipseLibrary.Web.JQuery;
using EclipseLibrary.Web.JQuery.Input;
using System.Web.Security;

namespace PhpaAll.MIS
{
    /// <summary>
    /// Query String: ProgressFormatId, IsEditMode. 
    /// </summary>
    public partial class DailyPhysical : PageBase
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
           

            if (!this.Master.IsEditMode)
            {
                using (MISDataContext db = new MISDataContext(ReportingUtilities.DefaultConnectString))
                {

                    DateTime[] dates = (from formatdetail in db.FormatDetails
                                        where formatdetail.ProgressDate <= DateTime.Today &&
                                        formatdetail.ProgressDate >= (DateTime.Today - TimeSpan.FromDays(365))
                                        //where formatdetail.ProgressFormatId == Convert.ToInt32(ddlProgressformat.Value)
                                        orderby formatdetail.ProgressDate
                                        select formatdetail.ProgressDate).Distinct().ToArray();


                    var stringDates = dates.Select(p => p.ToString("yyyy/M/d")).ToArray();
                    JavaScriptSerializer ser = new JavaScriptSerializer();
                    string script = ser.Serialize(stringDates);
                    script = string.Format("var _availableDates = {0};", script);
                    JQueryScriptManager.Current.RegisterScriptBlock(script);

                }
                tbProgressDate.DatePickerOptions.AddRaw("beforeShowDay", @"function(date) {
var x = date.getFullYear() + '/' + (date.getMonth() + 1) + '/' + date.getDate();
var index = $.inArray(x, _availableDates);
var ret = [];
if (index >= 0) {
// Data available
ret[0] = true;
ret[2] = 'Data Available';
} else {
ret[0] = false;
ret[2] = 'Data not entered for this date';
}
return ret;
}");

            }

            if (this.Master.IsEditMode)
            {
                Page.Title += " - Data Entry";
                pfb.Visible = false;
            }
            else
            {
                Page.EnableViewState = false ;
            }
            if (!string.IsNullOrEmpty( tbProgressDate.Text))
            {
                Page.Title += string.Format(" for {0:MMMM yyyy}", DateTime.Parse(tbProgressDate.Text));
            }

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
                       where fmt.ProgressFormatType == Convert.ToInt32(ProgressFormat.FormatType.DailyPhysical) &&
                       fmt.PackageId == this.Master.PackageId
                       select new
                       {
                           ProgressFormatId = fmt.ProgressFormatId,
                           DisplayName = fmt.ProgressFormatName + " (" + fmt.Description + ")"
                       };
        }

        protected void btn_Click(object sender, EventArgs e)
        {
            gv.DataBind();
        }

        protected void dsPhysicalReport_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            if ((string.IsNullOrEmpty(tbProgressDate.Text) && tbProgressDate.IsValid) || string.IsNullOrEmpty(ddlProgressformat.Value))
            {
                e.Cancel = true;
                return;
            }
            MISDataContext db = (MISDataContext)dsDPR.Database;

            DateTime selectedDate = tbProgressDate.ValueAsDate.Value;
            hfDate.Value = tbProgressDate.Text;

            var allActivites = (from pf in db.ProgressFormats
                                where pf.ProgressFormatId == Convert.ToInt32(ddlProgressformat.Value)
                                select pf.Activities).SelectMany(p => p);                       
            var dataActivities = from fad in db.FormatActivityDetails
                                 where fad.FormatDetail.ProgressFormatId == Convert.ToInt32(ddlProgressformat.Value)
                                 && fad.FormatDetail.ProgressDate == selectedDate
                                 select new
                                 {
                                     PhysicalProgress = fad.PhysicalProgressData,
                                     ActivityId = fad.ActivityId,
                                     FormatActivityDetailId = (int?)fad.FormatActivityDetailId,
                                     Remarks = fad.Remarks
                                 };



            var query2 = from act1 in allActivites
                         let uptoPreMonth = (from fad2 in db.FormatActivityDetails
                                             where fad2.ActivityId == act1.ActivityId &&
                                             fad2.FormatDetail.ProgressDate < selectedDate.MonthStartDate()
                                             && fad2.FormatDetail.ProgressFormatId == Convert.ToInt32(ddlProgressformat.Value)
                                             select fad2.PhysicalProgressData).Sum()
                         let uptoPreday = (from fad2 in db.FormatActivityDetails
                                           where fad2.ActivityId == act1.ActivityId &&
                                           fad2.FormatDetail.ProgressDate >= selectedDate.MonthStartDate()
                                           && fad2.FormatDetail.ProgressDate < selectedDate
                                           && fad2.FormatDetail.ProgressFormatId == Convert.ToInt32(ddlProgressformat.Value)
                                           select fad2.PhysicalProgressData).Sum()

                         join act2 in dataActivities on act1.ActivityId equals act2.ActivityId into g
                         from act2 in g.DefaultIfEmpty()

                         orderby act1.ActivityCode
                         let today = act2.PhysicalProgress
                         let totalQuantity = (uptoPreMonth ?? 0) + (uptoPreday ?? 0) + (today ?? 0)
                         select new

                         {
                             ActivityId = act1.ActivityId,
                             FormatActivityDetailId = (int?)act2.FormatActivityDetailId,
                             ItemNumber = act1.ActivityCode,
                             Description = act1.Description,
                             Unit = act1.UOM,
                             AsPerBOQ = act1.PhysicalTarget,
                             PreviousMonth = uptoPreMonth,
                             UptoPreviousDay = uptoPreday,
                             Today = today,
                             ThisMonth = (uptoPreday ?? 0) + (today ?? 0),
                             TotalQuantity = totalQuantity,
                             Balance = (act1.PhysicalTarget - totalQuantity) > 0 ? (act1.PhysicalTarget - totalQuantity) : 0,
                             Remarks = act2.Remarks

                         };

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
                    break;
            }
        }

        protected void gv_DataBound(object sender, EventArgs e)
        {
            //if (gv.Rows.Count > 0 && this.Master.IsEditMode)
            //{
            //    btnSaveProgress.Visible = true;
            //    btnSaveProgress1.Visible = true;
            //}
            //else
            //{
            //    btnSaveProgress.Visible = false;
            //    btnSaveProgress1.Visible = false;
            //}
        }

        /// <summary>
        /// Pass query string to data entry link
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPreRenderComplete(EventArgs e)
        {
            base.OnPreRenderComplete(e);
            hlDataEntry.NavigateUrl += string.Format("?ProgressFormatId={0}&ProgressDate={1:d}",
                ddlProgressformat.Value, tbProgressDate.Text);
            if (this.Master.IsEditMode)
            {
                hlDataEntry.Text = "Daily Progress Report";
            }
            else
            {
                hlDataEntry.Text = "Daily Progress Data Entry";
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
           
            try
            {
                using (MISDataContext db = new MISDataContext(ReportingUtilities.DefaultConnectString))
                {

                    FormatDetail formatDetail = (from fd in db.FormatDetails
                                                 where fd.ProgressFormatId == Convert.ToInt32(ddlProgressformat.Value) &&
                                                 fd.ProgressDate == Convert.ToDateTime(hfDate.Value)
                                                 select fd).SingleOrDefault();
                    if (formatDetail == null)
                    {
                        formatDetail = new FormatDetail()
                        {
                            ProgressFormatId = Convert.ToInt32(ddlProgressformat.Value),
                            ProgressDate = Convert.ToDateTime(hfDate.Value)
                        };
                        db.FormatDetails.InsertOnSubmit(formatDetail);
                    }
                    for(int i=0;i<= gv.Rows.Count-1;i++)
                    {
                        FormatActivityDetail fad;
                        int? formatActivityDetailid = (int?)gv.DataKeys[i]["FormatActivityDetailId"];
                        int activityId = (int)gv.DataKeys[i]["ActivityId"];
                        decimal? progressData = null;
                        TextBoxEx tbProgressData = (TextBoxEx)gv.Rows[i].FindControl("tbProgressData");
                        if (!string.IsNullOrEmpty(tbProgressData.Text))
                        {
                            progressData = Convert.ToDecimal(tbProgressData.Text);
                        }
                        TextArea taRemarks = (TextArea)gv.Rows[i].FindControl("taRemarks");
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
                            // Update
                            fad = formatDetail.FormatActivityDetails
                                .Where(p => p.FormatActivityDetailId == formatActivityDetailid)
                                .Single();
                            fad.PhysicalProgressData = progressData;
                            fad.Remarks = remarks;
                        }
                    }
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


    }

}
