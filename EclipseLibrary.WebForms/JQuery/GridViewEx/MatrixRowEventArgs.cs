using System;
using System.Web.UI.WebControls;

namespace EclipseLibrary.Web.JQuery
{
    /// <summary>
    /// Arguments passed with <see cref="MatrixField.MatrixRowDataBound"/> event.
    /// </summary>
    // [Obsolete("Use MatrixField in namespace EclipseLibrary.Web.JQuery.Matrix")]
    public class MatrixRowEventArgs : EventArgs
    {
        private readonly MatrixDataCell _cell;
        internal MatrixRowEventArgs(MatrixDataCell cell)
        {
            _cell = cell;
        }

        public IMatrixRow MatrixRow
        {
            get
            {
                return _cell;
            }
        }
        //public object DataItem
        //{
        //    get
        //    {
        //        return _cell.DataItem;
        //    }
        //}

        //public decimal? GetRowTotal(IEnumerable<MatrixColumn> columns)
        //{
        //    decimal?[] totals = _cell.GetRowTotals(columns);
        //    if (totals == null)
        //    {
        //        return null;
        //    }
        //    return totals[0];
        //}

        //public void SetCellText(MatrixColumn col, string text)
        //{
        //    _cell.SetCellText(col, text);
        //}

        /// <summary>
        /// Returns the row which has just been data bound
        /// </summary>
        public GridViewRow Row
        {
            get
            {
                GridViewRow row = (GridViewRow)_cell.NamingContainer;
                row.DataItem = _cell.DataItem;
                return row;
            }

        }

    }
}
