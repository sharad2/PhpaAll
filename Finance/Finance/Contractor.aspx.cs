/*
 *  Programming: Eclipse Systems (P) Ltd., NOIDA, INDIA
 *  E-mail: support@eclsys.com
 *
 *  $Workfile:   Contractor.aspx.cs  $
 *  $Revision: 38561 $
 *  $Author: ssingh $
 *  $Date: 2010-12-06 13:28:28 +0530 (Mon, 06 Dec 2010) $
 *  $Modtime:   Jul 21 2008 11:26:24  $
 *
 *  $Log:   S:/Projects/PHPA2/archives/Finance/Finance/Contractor.aspx.cs-arc  $
 * 
 *    Rev 1.72   Jul 21 2008 11:26:24   yjuneja
 * WIP
 */
using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using Eclipse.PhpaLibrary.Database;
using Eclipse.PhpaLibrary.Web;
using EclipseLibrary.Web.JQuery;

namespace PhpaAll.Finance
{
    public partial class ManageContractor : PageBase
    {
        /// <summary>
        /// If nothing is selected, then do not query
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dsSpecificContratctor_Selecting(object sender, System.Web.UI.WebControls.LinqDataSourceSelectEventArgs e)
        {
            if (e.WhereParameters["ContractorId"] == null)
            {
                e.Cancel = true;
            }
            if (m_ContractorId != null)
            {
                e.WhereParameters["ContractorId"] = m_ContractorId;
                m_ContractorId = null;
            }
        }

        /// <summary>
        /// After inserting data from update panel FormView -
        /// binding data grid, changing PageIndex of grid to last page, keeping formview in insert mode,
        /// changing text of cancel button, reseting controls of formview .        
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void frmContractor_ItemInserted(object sender, FormViewInsertedEventArgs e)
        {
            if (e.Exception == null)
            {
                gvContractors.DataBind();
            }
        }

        /// <summary>
        /// Binding grid after the selected data is updated.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void frmContractor_ItemUpdated(object sender, FormViewUpdatedEventArgs e)
        {
            if (e.Exception == null)
            {
                gvContractors.DataBind();
            }
        }

        /// <summary>
        /// Creating where clause list for the data-source of GridView.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dsContractors_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            List<string> whereClauses = new List<string>();
            string strParam = (string)e.WhereParameters["ContractorCode"];
            if (!string.IsNullOrEmpty(strParam))
            {
                whereClauses.Add(string.Format("ContractorCode.Contains(@ContractorCode)"));
            }
            strParam = (string)e.WhereParameters["ContractorName"];
            if (!string.IsNullOrEmpty(strParam))
            {
                whereClauses.Add(string.Format("ContractorName.Contains(@ContractorName)"));
            }
            strParam = (string)e.WhereParameters["Contact_Person"];
            if(!string.IsNullOrEmpty(strParam))
            {
                whereClauses.Add(string.Format("Contact_Person.Contains(@Contact_Person)"));
            }
            strParam = (string)e.WhereParameters["Address"];
            if (!string.IsNullOrEmpty(strParam))
            {
                whereClauses.Add(string.Format("Address.Contains(@Address)"));
            }
            strParam =(string) e.WhereParameters["City"];
            if (!string.IsNullOrEmpty(strParam))
            {
                whereClauses.Add(string.Format("City.Contains(@City)"));
            }
            strParam = (string)e.WhereParameters["Country"];
            if(!string.IsNullOrEmpty(strParam))
            {
                whereClauses.Add(string.Format("Country.Contains(@Country)"));
            }
            int i = whereClauses.Count;
            dsContractors.Where = string.Join("&&", whereClauses.ToArray());
        }

        /// <summary>
        /// Handling search button click. Binding grid and setting page index.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            this.gvContractors.PageIndex = 0;
            gvContractors.DataBind();
        }


        /// <summary>
        /// Bidnding grid to refreshed data after the employee is deleted from the table.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void frmContractor_ItemDeleted(object sender, FormViewDeletedEventArgs e)
        {
            if (e.Exception == null)
            {
                gvContractors.DataBind();
            }
        }


        /// <summary>
        /// If nothing inserted or updated, do nothing.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dsInsertedContractors_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            if (e.WhereParameters["ContractorId"] == null)
            {
                e.Cancel = true;
            }
        }

        /// <summary>
        /// Setting currently inserted EmployeeId as a where parameter for inserted-data / updated-data 
        /// FormView's data source.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        int? m_ContractorId;
        protected void dsSpecificContratctor_Inserted(object sender, LinqDataSourceStatusEventArgs e)
        {
            if (e.Exception == null)
            {
                Contractor contractor = (Contractor)e.Result;
                //this.dsSpecificContratctor.WhereParameters["ContractorId"].DefaultValue = contractor.ContractorId.ToString();
                m_ContractorId = contractor.ContractorId;
            }
        }

   

        protected void gvContractors_SelectedIndexChanged(object sender, EventArgs e)
        {
            dsSpecificContratctor.WhereParameters["ContractorId"].DefaultValue = gvContractors.SelectedDataKey["ContractorId"].ToString();
            if (frmContractor.CurrentMode == FormViewMode.Insert)
                frmContractor.ChangeMode(FormViewMode.ReadOnly);
            dlgContractorEditor.Visible = true;
        }

        protected void gvContractors_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            switch (e.Row.RowType)
            {
                case DataControlRowType.DataRow:
                    if (m_ContractorId != -1)
                    {
                        Contractor con = (Contractor)e.Row.DataItem;
                        if (con.ContractorId == m_ContractorId)
                        {
                            e.Row.RowState |= DataControlRowState.Selected;
                        }
                    }
                    break;
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            frmContractor.DeleteItem();
        }

        protected void btnEdit_Click(object sender, EventArgs e)
        {
            frmContractor.ChangeMode(FormViewMode.Edit);
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            switch (frmContractor.CurrentMode)
            {
                case FormViewMode.Insert:
                    frmContractor.ChangeMode(FormViewMode.ReadOnly);
                    dsSpecificContratctor.WhereParameters["ContractorId"].DefaultValue = Convert.ToString(-1);
                    break;
                case FormViewMode.Edit:
                    frmContractor.ChangeMode(FormViewMode.ReadOnly);
                    break;
            }
            

        }

        protected void btnNew_Click(object sender, EventArgs e)
        {
            frmContractor.ChangeMode(FormViewMode.Insert);
            dlgContractorEditor.Visible = true;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            switch (frmContractor.CurrentMode)
            {
                case FormViewMode.Insert:
                    frmContractor.InsertItem(false);
                    break;
                case FormViewMode.Edit:
                    frmContractor.UpdateItem(false);
                    break;
            }
        }
    }
}