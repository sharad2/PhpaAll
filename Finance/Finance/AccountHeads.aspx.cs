/*
 *  Programming: Eclipse Systems (P) Ltd., NOIDA, INDIA
 *  E-mail: support@eclsys.com
 *
 *  $Workfile:   AccountHeads.aspx.cs  $
 *  $Revision: 37664 $
 *  $Author: ssingh $
 *  $Date: 2010-11-23 14:58:19 +0530 (Tue, 23 Nov 2010) $
 *  $Modtime:   Jul 09 2008 17:26:52  $
 *
 *  $Log:   S:/Projects/PHPA2/archives/Finance/Finance/AccountHeads.aspx.cs-arc  $
 * 
 *    Rev 1.36   Jul 09 2008 17:26:54   vraturi
 * PVCS Template Added.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using Eclipse.PhpaLibrary.Reporting;
using Eclipse.PhpaLibrary.Web;

namespace Finance
{
    /// <summary>
    /// You can pass a comma seperated list of account types in the query string Types.
    /// e.g. Types=BANKNU,BANKFE.
    /// In this case, all the heads of the passed types will display as selected.
    /// </summary>
    public partial class AccountHeads : PageBase
    {
        private List<RoHeadHierarchy> m_accountsToSelect;

        protected override void OnLoad(EventArgs e)
        {
            ctlEditor.ItemChanged += new EventHandler<EventArgs>(ctlEditor_ItemChanged);

            if (!IsPostBack)
            {
                string typesValue = Request.QueryString["Types"];
                if (!string.IsNullOrEmpty(typesValue))
                {
                    string[] types = typesValue.Split(',');
                    CreateListOfHeadsToSelect(types);
                }
            }
            base.OnLoad(e);
        }

        /// <summary>
        /// Fill up m_accountsToSelect with all the heads of the passed types
        /// </summary>
        /// <param name="types"></param>
        private void CreateListOfHeadsToSelect(string[] types)
        {
            m_accountsToSelect = (from hoa in this.dsHeads.Database.RoHeadHierarchies
                        where types.Contains(hoa.HeadOfAccountType)
                        orderby hoa.SortableName
                        select hoa).ToList();
            tvHeads.ExpandDepth = -1;
            tvHeads.ShowCheckBoxes = TreeNodeTypes.All;
        }

        void ctlEditor_ItemChanged(object sender, EventArgs e)
        {
            if (ctlEditor.HeadOfAccountId.HasValue)
            {
                RoHeadHierarchy accToSelect = (from acc in this.dsHeads.Database.RoHeadHierarchies
                                               where acc.HeadOfAccountId == ctlEditor.HeadOfAccountId
                                               select acc).SingleOrDefault();
                if (accToSelect == null)
                {
                    // accToSelect is null when a head has been deleted
                    tvHeads.ExpandDepth = 1;
                }
                else
                {
                    m_accountsToSelect = new List<RoHeadHierarchy>();
                    m_accountsToSelect.Add(accToSelect);
                    tvHeads.ExpandDepth = -1;
                }
                tvHeads.ShowCheckBoxes = TreeNodeTypes.None;
                tvHeads.DataBind();
            }
        }
        /// <summary>
        /// This event is registered as an AsyncPostBackTrigger with the UpdatePanel.
        /// This means that when this event is raised, only the update panel will be refreshed
        /// and not the whole page. A bad side effect of this is that the clicked tree node
        /// does not display as selected.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void tvHeads_SelectedNodeChanged(object sender, EventArgs e)
        {
            int nId = int.Parse(this.tvHeads.SelectedNode.Value);
            ctlEditor.HeadOfAccountId = nId;
            //panelHybrid.MakeAlwaysVisible(true);
        }

        protected void tvHeads_TreeNodeDataBound(object sender, TreeNodeEventArgs e)
        {
            RoHeadHierarchy head = (RoHeadHierarchy)e.Node.DataItem;
            if (head.RoAccountType != null)
            {
                switch (head.RoAccountType.Category)
                {
                    case "A":
                        e.Node.ImageUrl = "~/Images/assets.jpg";
                        break;
                    case "L":
                        e.Node.ImageUrl = "~/Images/liabilities.jpg";
                        break;

                    case "E":
                        e.Node.ImageUrl = "~/Images/expenditure.jpg";
                        break;

                    case "R":
                        e.Node.ImageUrl = "~/Images/receipts.jpg";
                        break;

                    case "B":
                        e.Node.ImageUrl = "~/Images/bank.jpg";
                        break;

                    case "C":
                        e.Node.ImageUrl = "~/Images/cash.jpg";
                        break;
                }
                e.Node.ImageToolTip = head.RoAccountType.Description;
            }
            if (m_accountsToSelect == null)
            {
                return;
            }
            // Our goal is to expand all ancestors of the node we want to select so that the node
            // we want to select is guaranteed to be visible
            int nCurId = int.Parse(e.Node.Value);

            // Selected if the editor is showing this head
            e.Node.Selected = ctlEditor.HeadOfAccountId.HasValue && ctlEditor.HeadOfAccountId.Value == nCurId;

            // Checked if are in the list
            e.Node.Checked = m_accountsToSelect.Exists(acc => acc.HeadOfAccountId == nCurId);

            // If any of our descendants need to get get selected, we must expand
            e.Node.Expanded = m_accountsToSelect.Exists(acc => acc.IsDescendantOf(nCurId));
        }

        protected void btnMark_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(ddlAccountTypes.Value))
            {
                if (m_accountsToSelect != null)
                {
                    m_accountsToSelect.Clear();
                }
                tvHeads.ShowCheckBoxes = TreeNodeTypes.None;
                tvHeads.ExpandDepth = 1;
                tvHeads.DataBind();
            }
            else
            {
                string[] types = new string[1];
                types[0] = ddlAccountTypes.Value;
                CreateListOfHeadsToSelect(types);
                tvHeads.DataBind();
            }
        }

        protected void btnExpand_Click(object sender, EventArgs e)
        {
            tvHeads.ExpandAll();
        }

        protected void btnCollapse_Click(object sender, EventArgs e)
        {
            tvHeads.CollapseAll();
        }

        protected void btnNewTopHead_Click(object sender, EventArgs e)
        {
            //panelHybrid.MakeAlwaysVisible(true);
            this.ctlEditor.InsertHead(null);
        }
    }
}
