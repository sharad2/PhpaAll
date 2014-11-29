using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace EclipseLibrary.Web.JQuery
{
    /// <summary>
    /// Default template displayed in a matrix cell
    /// </summary>
        [Obsolete("Use MatrixField in namespace EclipseLibrary.Web.JQuery.Matrix")]
    public class MatrixItemTemplate:ITemplate
    {
        private readonly string _formatString;
        private readonly object[] _values;
        public MatrixItemTemplate(string formatString, object[] values)
        {
            _formatString = formatString;
            _values = values;
        }

        #region ITemplate Members

        public void InstantiateIn(Control container)
        {
            container.DataBinding += new EventHandler(container_DataBinding);
        }

        void container_DataBinding(object sender, EventArgs e)
        {
            TableCell cell = (TableCell)sender;
            cell.Text = string.Format(_formatString, _values);
        }

        #endregion
    }
}
