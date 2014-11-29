using System;
using System.Data.Linq;
using System.Linq;
using System.Web.UI.WebControls;
using Eclipse.PhpaLibrary.Database.PIS;
using Eclipse.PhpaLibrary.Reporting;
using Eclipse.PhpaLibrary.Web;

namespace Finance.PIS
{
    public partial class FamilyMembers : PageBase
    {
        /// <summary>
        /// Set the _familyMemberId on successful insertion and updation
        /// </summary>
        int _familyMemberId = -1;

        #region Selecting

        /// <summary>
        /// Cancel query if EmployeeId is null
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dsFamilyMembers_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            if (e.WhereParameters["EmployeeId"] == null)
            {
                e.Cancel = true;
            }
        }

        /// <summary>
        /// Show distinct Relationship
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dsRelationship_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            using (PISDataContext db = new PISDataContext(ReportingUtilities.DefaultConnectString))
            {
                e.Result = (from fm in db.FamilyMembers
                            where fm.Relationship != null
                            orderby fm.Relationship
                            select fm.Relationship).Distinct().ToArray();
            }
        }

        #endregion

        #region Context Creation

        protected void dsFamilyMembers_ContextCreated(object sender, LinqDataSourceStatusEventArgs e)
        {
            DataContext db = (DataContext)e.Result;
            DataLoadOptions dlo = new DataLoadOptions();
            dlo.LoadWith<FamilyMember>(p => p.Country);
            db.LoadOptions = dlo;
        }


        #endregion

        #region Insertion

        /// <summary>
        /// Show row to insert in the GridView
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnInsert_Click(object sender, EventArgs e)
        {
            gvFamilyMembers.InsertRowsCount = 1;
        }

      
        /// <summary>
        /// Show Status on Insertion completion
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dsFamilyMembers_Inserted(object sender, LinqDataSourceStatusEventArgs e)
        {
            if (e.Exception == null)
            {
                _familyMemberId = GetFamilyMemberId(e.Result);
                FamilyMembers_sp.AddStatusText("Item inserted successfully");
            }
            else
            {
                FamilyMembers_sp.AddErrorText(e.Exception.Message);
                e.ExceptionHandled = true;
            }
        }

        #endregion

        #region Updation

        /// <summary>
        /// Show status of updation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dsFamilyMembers_Updated(object sender, LinqDataSourceStatusEventArgs e)
        {
            if (e.Exception == null)
            {
                _familyMemberId = GetFamilyMemberId(e.Result);
                FamilyMembers_sp.AddStatusText("Item updated successfully");
            }
            else
            {
                FamilyMembers_sp.AddErrorText(e.Exception.Message);
                e.ExceptionHandled = true;
            }
        }

        #endregion

        #region Deletion

        /// <summary>
        /// Show status on deletion
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dsFamilyMembers_Deleted(object sender, LinqDataSourceStatusEventArgs e)
        {
            if (e.Exception == null)
            {
                FamilyMembers_sp.AddStatusText("Item deleted successfully");
            }
            else
            {
                FamilyMembers_sp.AddErrorText(e.Exception.Message);
                e.ExceptionHandled = true;
            }
        }

        #endregion

        #region GridViewRowDataBound

        /// <summary>
        /// Hide Insert button while inserting or updating.
        /// Also select the row newly inserted or updated
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvFamilyMembers_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            switch (e.Row.RowType)
            {
                case DataControlRowType.DataRow:
                    if (_familyMemberId != -1)
                    {
                        int id = GetFamilyMemberId(e.Row.DataItem);
                        if (id == _familyMemberId)
                        {
                            e.Row.RowState |= DataControlRowState.Selected;
                        }
                    }
                    // Checks whether any of Insert or Edit flag is on
                    if ((e.Row.RowState & (DataControlRowState.Insert | DataControlRowState.Edit)) !=
                        DataControlRowState.Normal)
                    {
                        formFamilyMembers_btnInsert.Visible = false;
                    }
                    break;

                default:
                    break;
            }

        }

        #endregion

        #region Helpers

        private int GetFamilyMemberId(object dataItem)
        {
            FamilyMember familyResult = (FamilyMember)dataItem;
            return familyResult.FamilyMemberId;
        }

        #endregion

    }
}
