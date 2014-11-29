using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace EclipseLibrary.Web.JQuery
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// Sharad 21 Sep 2009: Displaying subtotals of row totals
    /// </remarks>
        [Obsolete("Use MatrixField in namespace EclipseLibrary.Web.JQuery.Matrix")]
    internal class MatrixFooterCell : DataControlFieldCell
    {
        private readonly int _firstRowIndex;
        private readonly int _lastRowIndex;
        private readonly GridViewRow _parentRow;

        public MatrixFooterCell(MatrixField field)
            : base(field)
        {
            _firstRowIndex = 0;
            _lastRowIndex = -1;
        }

        /// <summary>
        /// Sub total cells are not added to the row so we must explicitly pass the parent row
        /// </summary>
        /// <param name="parentRow"></param>
        /// <param name="field"></param>
        /// <param name="firstRowIndex"></param>
        /// <param name="lastRowIndex"></param>
        public MatrixFooterCell(GridViewRow parentRow, MatrixField field, int firstRowIndex, int lastRowIndex)
            : base(field)
        {
            _firstRowIndex = firstRowIndex;
            _lastRowIndex = lastRowIndex;
            _parentRow = parentRow;
        }

        protected override void Render(HtmlTextWriter writer)
        {
            MatrixField mf = (MatrixField)this.ContainingField;

            if (!mf.ColumnGroups.Any())
            {
                // We have no columns to show. Do nothing
                return;
            }

            string formatString;
            if (string.IsNullOrEmpty(mf.DataTotalFormatString))
            {
                formatString = mf.DataValueFormatString;
            }
            else
            {
                formatString = mf.DataTotalFormatString;
            }
            string footerText;

            // NamingContainer is null when we are computing subtotals
            GridViewRow row = ((GridViewRow)this.NamingContainer) ?? _parentRow;
            int index = row.Cells.Cast<DataControlFieldCell>()
                .Select((p, i) => p.ContainingField == mf ? i : -1)
                .First(p => p >= 0);
            GridViewEx gv = (GridViewEx)row.NamingContainer;

            // Matrix data cells which need to be summed up
            IEnumerable<MatrixDataCell> dataCells = gv.Rows.Cast<GridViewRow>()
                .Where(p => p.Visible)
                .Skip(_firstRowIndex)
                .Take(_lastRowIndex < 0 ? int.MaxValue : _lastRowIndex - _firstRowIndex + 1)
                .Select(p => (MatrixDataCell)p.Cells[index]);

            // Selects only those indexes which have been specified in format string.
            int[] indexes = mf.DataValueFields.Where((p, i) => formatString.Contains("{" + i.ToString()))
                .Select((p, i) => i).ToArray();

            foreach (var group in mf.ColumnGroups)
            {
                decimal[] rowTotalSubtotals = null;
                foreach (var column in group)
                {
                    this.ContainingField.FooterStyle.AddAttributesToRender(writer);
                    writer.RenderBeginTag(HtmlTextWriterTag.Td);
                    if (mf.DisplayColumnTotals || mf.DisplayRowTotals)
                    {
                        decimal?[] subtotals = new decimal?[mf.DataValueFields.Length];

                        foreach (int i in indexes)
                        {
                            foreach (var values in dataCells
                                .Where(p => p.CellValues.ContainsKey(column))
                                .Select(p => p.CellValues[column]).Where(p => p[i] != null && p[i] != DBNull.Value)
                                .Select(p => Convert.ToDecimal(p[i])))
                            {
                                subtotals[i] = (subtotals[i] ?? 0) + values;
                            }
                        }
                        //decimal?[] subtotals = mf.GetColumnSubtotal(column, _firstRowIndex, _lastRowIndex);
                        footerText = string.Format(formatString, subtotals.Cast<object>().ToArray());
                        if (rowTotalSubtotals == null)
                        {
                            rowTotalSubtotals = new decimal[subtotals.Length];
                        }
                        rowTotalSubtotals = rowTotalSubtotals.Select((p, i) => p + (subtotals[i] ?? 0)).ToArray();
                    }
                    else
                    {
                        footerText = string.Empty;
                    }
                    if (string.IsNullOrEmpty(footerText))
                    {
                        footerText = "&nbsp;";
                    }
                    writer.Write(footerText);
                    writer.RenderEndTag();
                }
                if (mf.DisplayRowTotals)
                {
                    // Showing row totals
                    this.ContainingField.ItemStyle.AddAttributesToRender(writer);
                    writer.RenderBeginTag(HtmlTextWriterTag.Td);

                    writer.Write(formatString, rowTotalSubtotals.Cast<object>().ToArray());

                    writer.RenderEndTag();
                }
            }
        }

    }
}
