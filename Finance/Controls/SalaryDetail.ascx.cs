/*
 *  Programming: Eclipse Systems (P) Ltd., NOIDA, INDIA
 *  E-mail: support@eclsys.com
 *
 *  $Workfile:   SalaryDetail.ascx.cs  $
 *  $Revision: 37722 $
 *  $Author: ssinghal $
 *  $Date: 2010-11-23 20:00:52 +0530 (Tue, 23 Nov 2010) $
 *  $Modtime:   Jul 28 2008 13:14:34  $
 *
 *  $Log:   S:/Projects/PHPA2/archives/Finance/Controls/SalaryDetail.ascx.cs-arc  $
 * 
 *    Rev 1.46   Jul 28 2008 13:15:26   msharma
 * wip
 * 
 *    Rev 1.45   Jul 28 2008 12:08:06   msharma
 * WIP
 * 
 *    Rev 1.44   Jul 28 2008 11:26:06   msharma
 * WIP
 * 
 *    Rev 1.43   Jul 28 2008 10:18:28   msharma
 * WIP
 * 
 *    Rev 1.42   Jul 25 2008 17:22:52   msharma
 * WIP
 * 
 *    Rev 1.41   Jul 19 2008 17:09:08   msharma
 * WIP
 * 
 *    Rev 1.40   Jul 19 2008 15:49:28   msharma
 * WIP
 * 
 *    Rev 1.39   Jul 09 2008 17:58:38   vraturi
 * PVCS Template Added.
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Linq;
using System.Drawing;
using System.Web.UI;
using System.Web.UI.WebControls;
using Eclipse.PhpaLibrary.Database.Payroll;

namespace Finance.Controls
{
    [ParseChildren(true)]
    [PersistChildren(false)]
   public partial class SalaryDetail : System.Web.UI.UserControl
    {
       //public const string strFormat = "###,###,###,##0.00;(###,###,###,##0.00);#";  

       private IEnumerable<PeriodEmployeeAdjustment> m_periodEmployeeAdustment ;
       private decimal m_Deduction;
       private decimal m_Earning;
       private decimal m_TotalNetPayment;
       private bool bIsDataBind;
       public decimal TotalNetPayment
       {
           get
           {
               return m_TotalNetPayment;
           }
       }

       public int? SalaryPeriodId { get; set; }
       public int? EmployeeId { get; set; }
       public DateTime? FromDate { get; set; }
       public DateTime? ToDate { get; set; }
       public int? DivisiondId { get; set; }
      
      

       public override void DataBind()
       {
           lvDPayBill.DataSource = dsEmployeePeriod;
           bIsDataBind = true;
           lvDPayBill.DataBind();
       }


       protected override void OnPreRender(EventArgs e)
       {
           if (bIsDataBind == false)
           {
               DataBind();
           }
           base.OnPreRender(e);
       }
        class SalaryItem
        {
            public string Adjustment { get; set; }
            public decimal? Deduction { get; set; }
            public decimal? Earning { get; set; }
            public Boolean Isdeduction { get; set; }

        }


        /// <summary>
        /// Select salary details of particular Employee for particular period.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lvDPayBill_ItemCreated(object sender, ListViewItemEventArgs e)
        {
            switch (e.Item.ItemType)
            {
                case ListViewItemType.DataItem:
                    ListViewDataItem lvdi = (ListViewDataItem)e.Item;
                    GridView gvSalaryDetails = (GridView)lvdi.FindControl("gvSalaryDetails");
                    EmployeePeriod emp=(EmployeePeriod)lvdi.DataItem;
                    m_periodEmployeeAdustment = emp.PeriodEmployeeAdjustments;           
                    gvSalaryDetails.DataSource = QueryIterator();
                    break;
            }
        }

        

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private IEnumerable<SalaryItem> QueryIterator()
        {

            foreach (PeriodEmployeeAdjustment pea in m_periodEmployeeAdustment)
            {
                SalaryItem s = new SalaryItem();
                s.Adjustment = pea.Adjustment.AdjustmentCode + ":" + pea.Adjustment.Description;
                s.Deduction = pea.Adjustment.IsDeduction ? pea.Amount : null;
                s.Earning = (!pea.Adjustment.IsDeduction) ? pea.Amount : null;
                s.Isdeduction = pea.Adjustment.IsDeduction;
                //if (s.Deduction == null && s.Earning == null)
                //{
                //    continue;       // Ignore this adjustment
                //}
                yield return s;
            }

        }

       

        private static void SetDataLoadOptions(PayrollDataContext db)
        {
            DataLoadOptions lo = new DataLoadOptions();
            lo.AssociateWith<EmployeePeriod>(ep => ep.PeriodEmployeeAdjustments.Where(pea => pea.Amount != null));
            lo.LoadWith<EmployeePeriod>(ep => ep.SalaryPeriod);
            lo.LoadWith<EmployeePeriod>(ep => ep.Employee);
            lo.LoadWith<Employee>(ep => ep.EmployeeType);
            lo.LoadWith<Employee>(ep => ep.Division);
            lo.LoadWith<PeriodEmployeeAdjustment>(pea => pea.Adjustment);
            db.LoadOptions = lo;
        }




        protected void dsEmployeePeriod_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            if (!this.SalaryPeriodId.HasValue && !this.FromDate.HasValue && !this.FromDate.HasValue)
            {
                e.Cancel = true;
                return;
            }

            //if (ScriptManager.GetCurrent(this.Page).IsInAsyncPostBack)
            //{
            //    e.Cancel = true;
            //    return;
            //}

            SetDataLoadOptions((PayrollDataContext)dsEmployeePeriod.Database);

            List<string> whereClausesList = new List<string>();
            
            if (this.SalaryPeriodId != null)
            {
                e.WhereParameters["SalaryPeriodId"] = SalaryPeriodId;
                whereClausesList.Add("SalaryPeriodId=@SalaryPeriodId");
            }
            if (EmployeeId != null)
            {
                e.WhereParameters["EmployeeId"] = EmployeeId;
                whereClausesList.Add("EmployeeId=@EmployeeID");

            }
            if (DivisiondId != null)
            {
                e.WhereParameters["DivisionId"] = DivisiondId;
                whereClausesList.Add("Employee.DivisionId=@DivisionID");

            }

            if (FromDate.HasValue && ToDate.HasValue)
            {
                e.WhereParameters["FromDate"] = FromDate;
                whereClausesList.Add("SalaryPeriod.SalaryPeriodStart=@FromDate");
                e.WhereParameters["ToDate"] = ToDate;

                string s = @"((SalaryPeriod.SalaryPeriodStart <= @FromDate && SalaryPeriod.SalaryPeriodEnd >= @FromDate) ||
                                (SalaryPeriod.SalaryPeriodStart >= @FromDate && SalaryPeriod.SalaryPeriodEnd <= @ToDate) ||
                                (SalaryPeriod.SalaryPeriodStart >= @FromDate && SalaryPeriod.SalaryPeriodStart <= @ToDate))";
                whereClausesList.Add(s);
            }
            dsEmployeePeriod.Where = string.Join("&&", whereClausesList.ToArray());
        }

        protected void gvSalaryDetails_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            
           SalaryItem s =(SalaryItem) e.Row.DataItem;
            
           if (s != null)
           {
               m_Deduction +=Convert.ToDecimal(s.Deduction);
               m_Earning +=Convert.ToDecimal(s.Earning);

               if (s.Isdeduction == true)
               {
                   e.Row.BackColor = Color.Plum;
               }
           }
        }

        protected void lvDPayBill_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            ListViewDataItem lvdi = (ListViewDataItem)e.Item;
            Label lbl = (Label)lvdi.FindControl("lblNetPayment");
            EmployeePeriod emp=(EmployeePeriod)lvdi.DataItem;
            decimal  basicPay = 0;
            if (emp.BasicPay != null)
            {
                basicPay =(decimal)emp.BasicPay;
            }
            lbl.Text = (basicPay+ m_Earning - m_Deduction).ToString("C");
            m_TotalNetPayment += (basicPay + m_Earning - m_Deduction);
            m_Deduction = 0;
            m_Earning = 0;
        }
       

        
       

    

    }
}