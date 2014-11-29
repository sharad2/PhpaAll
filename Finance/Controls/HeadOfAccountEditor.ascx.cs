/*
 *  Programming: Eclipse Systems (P) Ltd., NOIDA, INDIA
 *  E-mail: support@eclsys.com
 *
 *  $Workfile:   HeadOfAccountEditor.ascx.cs  $
 *  $Revision: 37681 $
 *  $Author: ssingh $
 *  $Date: 2010-11-23 16:14:31 +0530 (Tue, 23 Nov 2010) $
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Eclipse.PhpaLibrary.Database;
using EclipseLibrary.Web.JQuery.Input;
using EclipseLibrary.Web.UI;



namespace Finance
{
    public partial class HeadOfAccountEditor : UserControl
    {
        # region   Variables
        public event EventHandler<EventArgs> ItemChanged;
        # endregion


        # region Selecting
        /// <summary>
        /// Don't perform a query when the id is 0
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dsEditAccounts_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            if (this.HeadOfAccountId == null)
            {
                e.Cancel = true;
            }
        }

        protected void dsAccountTypes_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            Eclipse.PhpaLibrary.Reporting.ReportingDataContext db = (Eclipse.PhpaLibrary.Reporting.ReportingDataContext)dsAccountTypes.Database;
            /*
            <eclipse:DropDownItem Text="(Not set)" Persistent="Always" />
                                <eclipse:DropDownItem Text="Asset" Value="A" Persistent="Always" />
                                <eclipse:DropDownItem Text="Liability" Value="L" Persistent="Always" />
                                <eclipse:DropDownItem Text="Expenditure" Value="E" Persistent="Always" />
                                <eclipse:DropDownItem Text="Receipts" Value="R" Persistent="Always" />
                                <eclipse:DropDownItem Text="Cash Account" Value="C" Persistent="Always" />
                                <eclipse:DropDownItem Text="Bank Account" Value="B" Persistent="Always" />
            */
            var dict = new Dictionary<string, string>
            {
                {"A", "Asset"},
                {"L", "Liability"},
                {"E", "Expenditure"},
                {"R", "Receipts"},
                {"C", "Cash Account"},
                {"B", "Bank Account"}
            };
            e.Result = from acctype in db.RoAccountTypes
                       orderby acctype.Category, acctype.Description, acctype.HeadOfAccountType
                       select new DropDownItem()
                       {
                           Value = acctype.HeadOfAccountType,
                           Text = acctype.Description,
                           OptionGroup = dict[acctype.Category]
                       };
        }


        /// <summary>
        /// Raise the item changed event so that the tree gets refreshed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnRefreshTree_Click(object sender, EventArgs e)
        {
            this.HeadOfAccountId = (int?)fvEdit.DataKey.Value;
            hdNewHead.Value = null;
            hdNewSubHead.Value = null;
            OnItemChanged();
        }

        private void OnItemChanged()
        {
            if (ItemChanged != null)
            {
                ItemChanged(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Set id to zero so that head of account editor form closes.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnClose_Click(object sender, EventArgs e)
        {
            hdNewHead.Value = null;
            hdNewSubHead.Value = null;
            this.HeadOfAccountId = null;
        }
        # endregion

        # region Inserting

        /// <summary>
        /// Used during insertion. Contains the parent of the head being inserted.
        /// </summary>
        private int? ParentHeadOfAccountId
        {
            get
            {
                object obj = ViewState["ParentHeadOfAccountId"];
                if (obj == null)
                {
                    return null;
                }
                else
                {
                    return (int?)obj;
                }
            }
            set
            {
                ViewState["ParentHeadOfAccountId"] = value;
            }
        }




        /// <summary>
        /// The form is displaying the controls for entering insert information.
        /// Specify the parent information of the account being inserted
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void fvEdit_DataBound(object sender, EventArgs e)
        {
            FinanceDataContext db = (FinanceDataContext)this.dsEditAccounts.Database;
            Label lblHeader = (Label)fvEdit.FindControl("lblHeader");
            switch (fvEdit.CurrentMode)
            {
                case FormViewMode.Edit:
                case FormViewMode.ReadOnly:
                    HeadOfAccount head = (HeadOfAccount)fvEdit.DataItem;
                    if (head != null)
                    {
                        lblHeader.Text = head.DisplayDescription;
                        lblHeader.ToolTip = string.Format("Internal Id: {0}", head.HeadOfAccountId);
                    }
                    break;

                case FormViewMode.Insert:
                    short maxName;
                    LeftLabel lblParentName = (LeftLabel)fvEdit.FindControl("lblParentName");
                    if (ParentHeadOfAccountId.HasValue)
                    {
                        HeadOfAccount parent = (from acc in db.HeadOfAccounts
                                                where acc.HeadOfAccountId == ParentHeadOfAccountId
                                                select acc).Single();
                        lblHeader.Text = string.Format("Creating subhead of {0}", parent.DisplayDescription);
                        lblHeader.ToolTip = string.Format("Parent Internal Id: {0}", parent.HeadOfAccountId);
                        if (parent.ChildHeads.Count() > 0)
                        {
                            maxName = parent.ChildHeads.Max(p => p.NameWithinParent);
                        }
                        else
                        {
                            maxName = 0;
                        }
                        lblParentName.Text = string.Format("Number: {0}.", parent.DisplayName);

                        // Type defaults to parent type
                        //if (parent.AccountType != null)
                        //{
                        //    DropDownListEx ddlCategory = (DropDownListEx)fvEdit.FindControl("ddlCategory");
                        //    ddlCategory.Value = parent.AccountType.Category;
                        //    DropDownListEx ddlAccountTypes = (DropDownListEx)fvEdit.FindControl("ddlAccountTypes");
                        //    ddlAccountTypes.Value = parent.HeadOfAccountType;
                        //}
                        var ddlAccountTypes = (DropDownListEx2)fvEdit.FindControl("ddlAccountTypes");
                        ddlAccountTypes.Value = parent.HeadOfAccountType;
                    }
                    else
                    {
                        lblHeader.Text = "Creating top level head";
                        lblParentName.Text = "Head Number";
                        try
                        {
                            maxName = (from acc in db.HeadOfAccounts
                                       where !acc.ParentHeadOfAccountId.HasValue
                                       select acc.NameWithinParent).Max();
                        }
                        catch (InvalidOperationException)
                        {
                            // This is the first top level head
                            maxName = 0;
                        }
                    }

                    TextBoxEx tbNameWithinParent = (TextBoxEx)fvEdit.FindControl("tbNameWithinParent");
                    tbNameWithinParent.Value = (maxName + 1).ToString();
                    break;
            }

        }

        /// <summary>
        /// If the insert was successful, retrieve the inserted row
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dsEditAccounts_Inserted(object sender, LinqDataSourceStatusEventArgs e)
        {
            if (e.Exception == null)
            {
                HeadOfAccount acc = (HeadOfAccount)e.Result;
                this.dsEditAccounts.WhereParameters["HeadOfAccountId"].DefaultValue = acc.HeadOfAccountId.ToString();
            }
        }
        # endregion

        # region Functions
        /// <summary>
        /// This property must be set by the containing page to tell us which head to display
        /// </summary>
        /// 
        public int? HeadOfAccountId
        {
            get
            {
                string str = this.dsEditAccounts.WhereParameters["HeadOfAccountId"].DefaultValue;
                if (string.IsNullOrEmpty(str))
                {
                    return null;
                }
                return int.Parse(str);
            }
            set
            {
                string str = this.dsEditAccounts.WhereParameters["HeadOfAccountId"].DefaultValue;
                int? curValue;
                if (string.IsNullOrEmpty(str))
                {
                    curValue = null;
                }
                else
                {
                    curValue = int.Parse(str);
                }

                if (curValue != value)
                {
                    this.dsEditAccounts.WhereParameters["HeadOfAccountId"].DefaultValue = value.ToString();
                    // User has clicked a new node while we were in insert mode. Sswitch to readonly
                    if (ParentHeadOfAccountId == null && fvEdit.CurrentMode == FormViewMode.Insert)
                    {
                        fvEdit.ChangeMode(FormViewMode.ReadOnly);
                    }
                }
            }
        }

        # endregion


        protected void fvEdit_ItemInserting(object sender, FormViewInsertEventArgs e)
        {
            if (ParentHeadOfAccountId != null)
            {
                e.Values["ParentHeadOfAccountId"] = ParentHeadOfAccountId.Value;
            }
            //Control ctlAccountType = fvEdit.FindControl("ctlAccountType");
            var ddlAccountTypes = (DropDownListEx2)fvEdit.FindControl("ddlAccountTypes");
            e.Values["HeadOfAccountType"] = ddlAccountTypes.Value;
        }

        protected void fvEdit_ItemInserted(object sender, FormViewInsertedEventArgs e)
        {
            if (e.Exception == null)
            {
                OnItemChanged();
            }
        }

        protected void fvEdit_ItemDeleted(object sender, FormViewDeletedEventArgs e)
        {
            if (e.Exception == null)
            {
                OnItemChanged();
            }
        }

        protected void fvEdit_ItemUpdated(object sender, FormViewUpdatedEventArgs e)
        {
            if (e.Exception == null)
            {
                OnItemChanged();
            }
        }

        protected void hlSeeLedger_PreRender(object sender, EventArgs e)
        {
            HyperLink hlSeeLedger = (HyperLink)sender;
            if (fvEdit.DataKey["HeadOfAccountId"] == null)
            {
                hlSeeLedger.Visible = false;
            }
            else
            {
                int id = (int)fvEdit.DataKey["HeadOfAccountId"];
                hlSeeLedger.NavigateUrl += "?HeadOfAccount=" + id.ToString();
            }
        }

        protected void fvEdit_ItemUpdating(object sender, FormViewUpdateEventArgs e)
        {
            var ddlAccountTypes = (DropDownListEx2)fvEdit.FindControl("ddlAccountTypes");
            //DropDownList ddlAccountTypes = (DropDownList)ctlAccountType.FindControl("ddlAccountTypes");
            if (string.IsNullOrEmpty(ddlAccountTypes.Value))
            {
                // To force an update
                e.OldValues["HeadOfAccountType"] = "Sharad";
            }
            e.NewValues["HeadOfAccountType"] = ddlAccountTypes.Value;
        }

        protected void btnNewSubhead_PreRender(object sender, EventArgs e)
        {
            LinkButtonEx btnNewSubhead = (LinkButtonEx)sender;
            HeadOfAccount head = (HeadOfAccount)fvEdit.DataItem;

            if (head != null)
            {
                Eclipse.PhpaLibrary.Reporting.ReportingDataContext db = (Eclipse.PhpaLibrary.Reporting.ReportingDataContext)dsHead.Database;
                int query = (from hoa in db.RoHeadHierarchies
                             where hoa.HeadOfAccountId == head.HeadOfAccountId
                             select hoa.Level).Single();

                if (query < 4)
                {
                    btnNewSubhead.Action = ButtonAction.Submit;
                    int id = (int)fvEdit.DataKey["HeadOfAccountId"];
                    hdNewSubHead.Value = id.ToString();
                }
                else
                {
                    btnNewSubhead.Action = ButtonAction.None;
                    btnNewSubhead.ToolTip = string.Format("Cannot Insert SubHead below this Level");
                }
            }
        }

        protected void btnNewHead_PreRender(object sender, EventArgs e)
        {
            if (fvEdit.DataKey["ParentHeadOfAccountId"] != null)
            {
                LinkButtonEx btnNewHead = (LinkButtonEx)sender;
                int parentId = (int)fvEdit.DataKey["ParentHeadOfAccountId"];
                hdNewHead.Value = parentId.ToString();
            }
        }

        protected void btnDelete_PreRender(object sender, EventArgs e)
        {
            LinkButtonEx btnDelete = (LinkButtonEx)sender;
            HeadOfAccount head = (HeadOfAccount)fvEdit.DataItem;
            if (head != null && head.ChildHeads.Count > 0)
            {
                btnDelete.Action = ButtonAction.None;
                btnDelete.OnClientClick = "NoMessage";
                btnDelete.ToolTip = string.Format("Disabled because {0} subheads exist. Delete the subheads first",
                    head.ChildHeads.Count);
            }
            else
            {
                btnDelete.Action = ButtonAction.Submit;
                btnDelete.ToolTip = "Permanently delete this head. Deletion will fail if vouchers exist for this head.";
            }
        }

        protected void btnDeleteNew_Click(object sender, EventArgs e)
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
                    dsEditAccounts.WhereParameters["HeadOfAccountId"].DefaultValue = null;
                    break;
                case FormViewMode.Edit:
                    fvEdit.ChangeMode(FormViewMode.ReadOnly);
                    break;
            }
        }

        /// <summary>
        /// Public function which allows our containing page to request insertion of a head
        /// </summary>
        /// <param name="parentId"></param>
        public void InsertHead(string parentId)
        {
            if (string.IsNullOrEmpty(parentId))
            {
                ParentHeadOfAccountId = null;
            }
            else
            {
                ParentHeadOfAccountId = Convert.ToInt32(parentId);
            }

            if (fvEdit.CurrentMode == FormViewMode.Insert)
            {
                fvEdit.DataBind();
            }
            fvEdit.ChangeMode(FormViewMode.Insert);
        }

        protected void btnNewTopHead_Click(object sender, EventArgs e)
        {
            InsertHead(null);
        }

        protected void btnNewHead_Click(object sender, EventArgs e)
        {
            InsertHead(hdNewHead.Value.ToString());
        }

        protected void btnNewSubHead_Click(object sender, EventArgs e)
        {
            InsertHead(hdNewSubHead.Value.ToString());
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
        /// <summary>
        /// Ritesh 13 Jan 2012
        /// User can now set station against head of account
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dsStations_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            using (Eclipse.PhpaLibrary.Database.PIS.PISDataContext db = new Eclipse.PhpaLibrary.Database.PIS.PISDataContext(Eclipse.PhpaLibrary.Reporting.ReportingUtilities.DefaultConnectString))
            {
                var query = (from station in db.Stations
                             where station.StationName != null && station.StationName != string.Empty
                             orderby station.StationName
                             select station).ToList();
                e.Result = query;
            }
          
        }
    
    }
}

