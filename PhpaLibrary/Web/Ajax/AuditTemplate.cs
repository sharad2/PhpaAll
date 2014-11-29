using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using Eclipse.PhpaLibrary.Database;

namespace Eclipse.PhpaLibrary.Web.Ajax
{
    /// <summary>
    /// This is a template which display audit columns in a tabular format. It detects the ancestor which
    /// implements the IDataItemContainer interface. It accesses the data item from this interface and
    /// expects it to be IAuditable. It displays the audit information based on the fiends of IAuditable.
    /// </summary>
    public class AuditTemplate:ITemplate
    {
        private readonly AuditTabPanel _auditTabPanel;
        public AuditTemplate(AuditTabPanel auditTabPanel)
        {
            _auditTabPanel = auditTabPanel;
        }

        #region ITemplate Members

        private Table m_table;
        public void InstantiateIn(Control container)
        {
            m_table = new Table();
            _auditTabPanel.PreRender += new EventHandler(_auditTabPanel_PreRender);
            m_table.DataBinding += new EventHandler(table_DataBinding);
            container.Controls.Add(m_table);
        }

        void table_DataBinding(object sender, EventArgs e)
        {
            m_table.CssClass = string.Join(" ", _auditTabPanel.CssClasses);
            IDataItemContainer dic = null;
            Control nc = _auditTabPanel;
            while (true)
            {
                nc = nc.NamingContainer;
                if (nc == null)
                {
                    break;
                }
                dic = nc as IDataItemContainer;
                if (dic != null)
                {
                    break;
                }
            }
            if (dic == null)
            {
                throw new InvalidOperationException("We must be contained in a container which implemnts IDataItemContainer such as FormView");
            }
            _auditTabPanel.Auditable = (IAuditable)dic.DataItem;
        }

        void _auditTabPanel_PreRender(object sender, EventArgs e)
        {
            CreateTableRows();
        }


        private void CreateTableRows()
        {
            if (_auditTabPanel.Auditable != null)
            {
                m_table.Rows.Add(GetRow("Created:", _auditTabPanel.Auditable.Created));
                m_table.Rows.Add(GetRow("Created By:", _auditTabPanel.Auditable.CreatedBy));
                m_table.Rows.Add(GetRow("Created At Station:", _auditTabPanel.Auditable.CreatedWorkstation));
                m_table.Rows.Add(GetRow("Modified:", _auditTabPanel.Auditable.Modified));
                m_table.Rows.Add(GetRow("Modified By:", _auditTabPanel.Auditable.ModifiedBy));
                m_table.Rows.Add(GetRow("Modified At Station:", _auditTabPanel.Auditable.ModifiedWorkstation));
            }
        }

        private TableRow GetRow(string name, object value)
        {
            TableRow row = new TableRow();
            TableCell cell = new TableCell();
            row.Cells.Add(cell);
            cell.Style.Add(HtmlTextWriterStyle.PaddingLeft, "12px");
            cell.CssClass = _auditTabPanel.LeftCssClass;
            cell.Text = name;
            cell = new TableCell();
            row.Cells.Add(cell);
            cell.CssClass = _auditTabPanel.RightCssClass;
            cell.Text = string.Empty;
            if (value != null)
            {
                cell.Text = value.ToString();
            }            
            return row;
        }

        #endregion
    }
}
