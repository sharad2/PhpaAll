/*
 *  Programming: Eclipse Systems (P) Ltd., NOIDA, INDIA
 *  E-mail: support@eclsys.com
 *
 *  $Workfile:   Jobs.aspx.cs  $
 *  $Revision: 38559 $
 *  $Author: ssingh $
 *  $Date: 2010-12-06 13:05:10 +0530 (Mon, 06 Dec 2010) $
 *  $Modtime:   Jul 24 2008 14:15:30  $
 *
 *  $Log:   S:/Projects/PHPA2/archives/Finance/Finance/Jobs.aspx.cs-arc  $
 * 
 *    Rev 1.94   Jul 24 2008 14:30:20   pshishodia
 * WIP* 
 *    Rev 1.93   Jul 24 2008 14:26:56   pshishodia
 * Contractor Code replaced.
 */

using System;
using System.Linq;
using System.Web.UI.WebControls;
using Eclipse.PhpaLibrary.Database;
using Eclipse.PhpaLibrary.Web;
using EclipseLibrary.Web.JQuery;
using Eclipse.PhpaLibrary.Reporting;

namespace Finance.Finance
{
    public partial class Jobs : PageBase
    {

        public class JobTotal
        {
            public decimal? Total { get; set; }
            public int JobId { get; set; }
            public string JobCode { get; set; }
            public string Description { get; set; }
            public string DivisionName { get; set; }
            public string ContractorName { get; set; }
            public string ContractorCode { get; set; }
            public decimal? SanctionedAmount { get; set; }
            public DateTime? CommencementDate { get; set; }
            public DateTime? CompletionDate { get; set; }
            public string Nationality { get; set; }
            public decimal? RevisedContract { get; set; }
        }

        /// <summary>
        /// Make where clause for search criteria.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        protected void dsJobs_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            FinanceDataContext db = (FinanceDataContext)dsJobs.Database;
            IQueryable<Job> query = db.Jobs;

            if (!string.IsNullOrEmpty(lbltbDivision.Value))
            {
                query = query.Where(p => p.DivisionId == Convert.ToInt32(lbltbDivision.Value));
            }

            if (!string.IsNullOrEmpty(lbltbJobCode.Value))
            {
                query = query.Where(p => p.JobCode.Contains(lbltbJobCode.Value));
            }

            if (!string.IsNullOrEmpty(lbltbDate.Value))
            {
                DateTime date = DateTime.Now.AddDays(Convert.ToInt32(lbltbDate.Value)).Date;
                query = query.Where(p => p.CompletionDate < date &&  p.CompletionDate >= DateTime.Now);
            }

            DateTime _date = DateTime.Now;
            e.Result = from job in query
                       select new JobTotal
                       {
                           JobId = job.JobId,
                           JobCode = job.JobCode,
                           Description = job.Description,
                           DivisionName = job.Division.DivisionName,
                           ContractorName = job.Contractor.ContractorName,
                           ContractorCode = job.Contractor.ContractorCode,
                           SanctionedAmount = job.SanctionedAmount,
                           CommencementDate = job.CommencementDate,
                           Nationality = job.Contractor.Nationality,
                           RevisedContract = (decimal?)(job.RevisedContract ?? job.SanctionedAmount),
                           CompletionDate = job.CompletionDate,
                           //Total = job.VoucherDetails.Sum(p => ((p.HeadOfAccount.HeadOfAccountType == "EXPENDITURE" ||
                           //    p.HeadOfAccount.HeadOfAccountType == "TOUR_EXPENSES") && p.Voucher.VoucherDate <= _date
                           //    ? p.DebitAmount ?? 0 - p.CreditAmount ?? 0 : 0))
                           //    + job.VoucherDetails.Sum(p => ((p.HeadOfAccount.HeadOfAccountType == "PARTY_ADVANCE" ||
                           //        p.HeadOfAccount.HeadOfAccountType == "MATERIAL_ADVANCE") && p.Voucher.VoucherDate <= _date ?
                           //        p.DebitAmount ?? 0 : 0))
                           //    - job.VoucherDetails.Sum(p => (p.HeadOfAccount.HeadOfAccountType == "PARTY_ADVANCE" ||
                           //        p.HeadOfAccount.HeadOfAccountType == "MATERIAL_ADVANCE")
                           //        && p.Voucher.VoucherDate <= _date ? p.CreditAmount ?? 0 : 0)
                           Total = job.VoucherDetails.Where(p => p.Voucher.VoucherDate <= _date && 
                               HeadOfAccountHelpers.JobExpenses.Concat(HeadOfAccountHelpers.JobAdvances).Contains(p.HeadOfAccount.HeadOfAccountType))
                                    .Sum(p => p.DebitAmount ?? 0 - p.CreditAmount ?? 0)
                              
                       };
        }


        /// <summary>
        /// Do not query when job is null.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dsEditJobs_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            if (e.WhereParameters["JobId"] == null)
            {
                e.Cancel = true;
            }
            if (m_jobid != null)
            {
                e.WhereParameters["JobId"] = m_jobid;
                m_jobid = null;
            }
        }

        /// <summary>
        /// Make sure the grid displays the updated jobs.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        protected void fvEdit_ItemUpdated(object sender, FormViewUpdatedEventArgs e)
        {
            if (e.Exception == null)
            {
                gvJobs.DataBind();
            }
            else
            {
                FormView fv = (FormView)sender;
                EclipseLibrary.Web.JQuery.Input.ValidationSummary valSummary = (EclipseLibrary.Web.JQuery.Input.ValidationSummary)fv.FindControl("valSummary");

                valSummary.ErrorMessages.Add(e.Exception.Message);
                e.ExceptionHandled = true;
            }
        }

        /// <summary>
        /// Make sure the grid displays the inserted jobs.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        protected void fvEdit_ItemInserted(object sender, FormViewInsertedEventArgs e)
        {
            if (e.Exception == null)
            {
                gvJobs.DataBind();
            }
            else
            {
                FormView fv = (FormView)sender;
                EclipseLibrary.Web.JQuery.Input.ValidationSummary valSummary = (EclipseLibrary.Web.JQuery.Input.ValidationSummary)fv.FindControl("valSummary");

                valSummary.ErrorMessages.Add(e.Exception.Message);
                e.ExceptionHandled = true;
            }
        }

        /// <summary>
        /// This method use for following purpose.
        /// 1.In insert case inserted row have been selected.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvJobs_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            switch (e.Row.RowType)
            {
                case DataControlRowType.DataRow:
                    if (m_jobid != -1)
                    {
                        JobTotal job = (JobTotal)e.Row.DataItem;
                        if (job.JobId == m_jobid)
                        {
                            e.Row.RowState |= DataControlRowState.Selected;
                        }
                    }
                    break;
            }
        }


        /// <summary>
        /// Event will execute when user will select any job
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvJobs_SelectedIndexChanged(object sender, EventArgs e)
        {
            dsEditJobs.WhereParameters["JobId"].DefaultValue = gvJobs.SelectedDataKey["JobId"].ToString();
            if (fvEdit .CurrentMode == FormViewMode.Insert)
                fvEdit.ChangeMode(FormViewMode.ReadOnly);
            dlgJobEditor.Visible = true;
        }


        /// <summary>
        /// Making inserted-data/update-data FormView invisible when no insertion or updation is performed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        protected void dsInsertedJobs_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            if (e.WhereParameters["JobId"] == null)
            {
                e.Cancel = true;
            }
        }


       /// <summary>
        /// Binding Grid and setting page index to first page on Search button click.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            gvJobs.PageIndex = 0;
            gvJobs.DataBind();
        }

        int? m_jobid;
        /// <summary>
        /// Setting JobId to the where clause of data-source of Inserted-data/Updataed data FormView on insertion.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dsEditJobs_Inserted(object sender, LinqDataSourceStatusEventArgs e)
        {
            if (e.Exception == null)
            {
                Job job = (Job)e.Result;
                m_jobid = job.JobId;
                dsEditJobs.WhereParameters["JobId"].DefaultValue = m_jobid.ToString();
            }
        }

        /// <summary>
        /// Setting JobId to the where clause of data-source of Inserted-data/Updataed data FormView on updation.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        protected void dsEditJobs_Updated(object sender, LinqDataSourceStatusEventArgs e)
        {
            if (e.Exception == null)
            {
                gvJobs.DataBind();
            }
            
        }

        /// <summary>
        /// Refreshing GridView, when selecting item is deleted.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void fvEdit_ItemDeleted(object sender, FormViewDeletedEventArgs e)
        {
            if (e.Exception == null)
            {
                gvJobs.DataBind();
            }
            else
            {
                FormView fv = (FormView)sender;
                EclipseLibrary.Web.JQuery.Input.ValidationSummary valSummary = (EclipseLibrary.Web.JQuery.Input.ValidationSummary)fv.FindControl("valSummary");

                valSummary.ErrorMessages.Add(e.Exception.Message);
                e.ExceptionHandled = true;
            }
        }


        protected void btnDelete_Click(object sender, EventArgs e)
        {
            fvEdit.DeleteItem();
        }

        protected void btnEdit_Click(object sender, EventArgs e)
        {
            fvEdit.ChangeMode(FormViewMode.Edit);
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            switch (fvEdit.CurrentMode)
            {
                case FormViewMode.Insert:
                    fvEdit.ChangeMode(FormViewMode.ReadOnly);
                    dsEditJobs.WhereParameters["JobId"].DefaultValue = Convert.ToString(-1);
                    break;
                case FormViewMode.Edit:
                    fvEdit.ChangeMode(FormViewMode.ReadOnly);
                    break;
            }


        }

        /// <summary>
        /// Change the mode of the form view from default mode to Insert mode.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnNew_Click(object sender, EventArgs e)
        {
            fvEdit.ChangeMode(FormViewMode.Insert);
            dlgJobEditor.Visible = true;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            switch (fvEdit.CurrentMode)
            {
                case FormViewMode.Insert:
                    fvEdit.InsertItem(false);
                    break;
                case FormViewMode.Edit:
                    fvEdit.UpdateItem(false);
                    break;
            }
        }


    }
}
