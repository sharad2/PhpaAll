using Eclipse.PhpaLibrary.Reporting;
using Eclipse.PhpaLibrary.Web;
using EclipseLibrary.Web.JQuery.Input;
/*
 *  Programming: Eclipse Systems (P) Ltd., NOIDA, INDIA
 *  E-mail: support@eclsys.com
 *
 *  $Workfile:   PartyAdvances.aspx.cs  $
 *  $Revision: 37236 $
 *  $Author: ssingh $
 *  $Date: 2010-11-13 14:08:30 +0530 (Sat, 13 Nov 2010) $
 *  $Modtime:   Jul 22 2008 13:07:28  $
 *
 *  $Log:   S:/Projects/PHPA2/archives/Finance/Reports/PartyAdvances.aspx.cs-arc  $
 * 
 *    Rev 1.33   Jul 22 2008 13:08:54   yjuneja
 * WIP
 * 
 *    Rev 1.32   Jul 16 2008 12:41:34   msharma
 * WIP
 * 
 *    Rev 1.31   Jul 16 2008 12:23:32   msharma
 * WIP
 * 
 *    Rev 1.30   Jul 09 2008 17:40:58   vraturi
 * PVCS Template Added.
 */
using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

//Code reviewed by Mayank Sharma on 7th April 2008 
namespace Finance.Reports
{
    public partial class PartyAdvances : PageBase
    {
        //const string partyAdvance = "PARTY_ADVANCE";
        //const string materialAdvance = "MATERIAL_ADVANCE";
        ///// <summary>
        ///// This class is created to calculate  TotalAdvaceAmout.
        ///// </summary>
        //class Party
        //{
        //    public RoJob Job { get; set; }
        //    public int? PartyId { get; set; }
        //    public string PartyCode { get; set; }
        //    public string PartyName { get; set; }
        //    public decimal? Advance { get; set; }
        //    public decimal? MaterialAdvance { get; set; }
        //    public decimal? TotalAdvance { get; set; }
        //    public DateTime? EarliestDate { get; set; }
        //}

       
        protected override void OnLoad(EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                tbDate.Value = DateTime.Now.ToShortDateString(); ;
            }
            this.Title = string.Format("List of outstanding Party Advance as on {0:dd/MM/yyyy}", tbDate.Value);
            base.OnLoad(e);
        }
        /// <summary>
        /// Execute the query for report
        /// </summary>
        /// <param name="e"></param>
        protected void dsPartyAdv_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            if (string.IsNullOrEmpty(tbDate.Text))
            {
                e.Cancel = true;
                return;
            }
                ReportingDataContext db = (ReportingDataContext)dsPartyAdv.Database;
                e.Result = (from vd in db.RoVoucherDetails
                            //where (vd.HeadOfAccount.HeadOfAccountType == partyAdvance ||
                            //       vd.HeadOfAccount.HeadOfAccountType == materialAdvance)
                            where (HeadOfAccountHelpers.JobAdvances.Contains(vd.HeadOfAccount.HeadOfAccountType)) &&
                            vd.RoVoucher.VoucherDate <= tbDate.ValueAsDate
                            group vd by vd.ContractorId ?? vd.RoJob.ContractorId into grp
                            orderby grp.Min(p=>p.RoJob.RoContractor.ContractorCode ?? p.RoContractor.ContractorCode)
                            let advance = (decimal?)grp.Where(p => HeadOfAccountHelpers.PartyAdvances.Contains(p.HeadOfAccount.HeadOfAccountType))
                                .Sum(p => (p.DebitAmount ?? 0) - (p.CreditAmount ?? 0))
                            let materialAdvance = (decimal?)grp.Where(p => p.HeadOfAccount.HeadOfAccountType == HeadOfAccountHelpers.AdvanceSubTypes.MaterialAdvance)
                                .Sum(p => (p.DebitAmount ?? 0) - (p.CreditAmount ?? 0))
                            select new 
                            {
                                PartyId = grp.Key,
                                PartyCode = grp.Min(p=>p.RoJob.RoContractor.ContractorCode ?? p.RoContractor.ContractorCode),
                                PartyName = grp.Min(p=>p.RoJob.RoContractor.ContractorName ?? p.RoContractor.ContractorName),
                                //Advance = (decimal?)grp.Sum(p => p.HeadOfAccount.HeadOfAccountType == partyAdvance ? (p.DebitAmount ?? 0 - p.CreditAmount ?? 0) : 0)
                                Advance = advance,
                                AdvanceAccountTypes = string.Join(",", HeadOfAccountHelpers.PartyAdvances),
                                //MaterialAdvance = (decimal?)grp.Sum(p => HeadOfAccountHelpers.AdvanceSubTypes.MaterialAdvance.Contains(p.HeadOfAccount.HeadOfAccountType) ? (p.DebitAmount ?? 0 - p.CreditAmount ?? 0) : 0),
                                MaterialAdvance = materialAdvance,
                                MaterialAdvanceAccountType = HeadOfAccountHelpers.AdvanceSubTypes.MaterialAdvance,
                                TotalAdvance = (advance ?? 0) + (materialAdvance ?? 0),
                                //EarliestDate = (DateTime?)grp.Min(p => p.RoVoucher.VoucherDate)
                            }).Where(p => p.TotalAdvance != 0);
        }

        //decimal? sumAdvance = 0.0M;
        //decimal? sumMaterialAdvance = 0.0M;
        //protected void grdPartyAdv_RowDataBound(object sender, GridViewRowEventArgs e)
        //{
        //    switch (e.Row.RowType)
        //    {
        //        case DataControlRowType.Header:
        //            //grdPartyAdv.Caption = string.Format("<b>List of outstanding Party Advance as on {0:dd/MM/yyyy}</b>", tbDate.Value);
        //            break;
        //        case DataControlRowType.DataRow:
        //            //Party pt = (Party)e.Row.DataItem;
        //            //HyperLink hlAdvanceAmount = (HyperLink)e.Row.FindControl("hlAdvanceAmount");
        //            //HyperLink hlMaterialAdvance = (HyperLink)e.Row.FindControl("hlMaterialAdvance");
        //            //if (!string.IsNullOrEmpty(HeadOfAccountHelpers.PartyAdvances.ToString()))
        //            //{
        //            //    if (pt.PartyId == null)
        //            //    {
        //            //        hlAdvanceAmount.NavigateUrl = string.Format("~/Finance/VoucherSearch.aspx?AccountTypes=PARTY_ADVANCE&ContractorId=0");

        //            //    }
        //            //    else
        //            //    {
        //            //        hlAdvanceAmount.NavigateUrl = string.Format("~/Finance/VoucherSearch.aspx?AccountTypes=PARTY_ADVANCE&ContractorId={0}", pt.PartyId);
        //            //    }
        //            //}


        //            //if (!string.IsNullOrEmpty(HeadOfAccountHelpers.AdvanceSubTypes.MaterialAdvance.ToString()))
        //            //{
        //            //    if (pt.PartyId == null)
        //            //    {
        //            //        hlMaterialAdvance.NavigateUrl = string.Format("~/Finance/VoucherSearch.aspx?AccountTypes=MATERIAL_ADVANCE&ContractorId=0");
        //            //    }
        //            //    else
        //            //    {
        //            //        hlMaterialAdvance.NavigateUrl = string.Format("~/Finance/VoucherSearch.aspx?AccountTypes=MATERIAL_ADVANCE&ContractorId={0}", pt.PartyId);
        //            //    }
        //            //}

        //            //sumAdvance += pt.Advance;
        //            //sumMaterialAdvance += pt.MaterialAdvance;
        //            break;

        //        case DataControlRowType.Footer:
        //            DataControlFieldCell cellAdv = (from DataControlFieldCell c in e.Row.Cells
        //                                            where c.ContainingField.AccessibleHeaderText == "advance"
        //                                            select c).Single();
        //            cellAdv.Text = string.Format("{0:C}", sumAdvance);
        //            DataControlFieldCell cellMatAdv = (from DataControlFieldCell c in e.Row.Cells
        //                                               where c.ContainingField.AccessibleHeaderText == "MaterialAdvance"
        //                                               select c).Single();
        //            cellMatAdv.Text = string.Format("{0:C}", sumMaterialAdvance);
        //            break;
        //    }
        //}

        protected void btnShowPartyAdvances_Click(object sender, EventArgs e)
        {
            ButtonEx btn = (ButtonEx)sender;
            if (btn.IsPageValid())
            {
                grdPartyAdv.DataBind();
            }
        }
    }
}
