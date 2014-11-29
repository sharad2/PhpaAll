using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace EclipseLibrary.Web.JQuery
{
    /// <summary>
    /// Renders multiple TD elements, one per matrix column. Created by GridViewExMatrixField.
    /// During rendering, it asks its containing field about column headers and renders all columns.
    /// Then it renders values in the columns which contain a value for this row.
    /// </summary>
    /// <remarks>
    /// <para>
    /// A <c>MatrixDataCell</c> is passed to you in as <see cref="MatrixRowEventArgs.MatrixRow"/> during the
    /// <see cref="MatrixField.MatrixRowDataBound"/> event. During this event you can use
    /// <see cref="GetRowTotals"/> to access the totals for the row. You can also use
    /// <see cref="SetCellText"/> to set custom text for a particular column.
    /// </para>
    /// </remarks>
    [Obsolete("Use MatrixField in namespace EclipseLibrary.Web.JQuery.Matrix")]
    internal class MatrixDataCell : DataControlFieldCell, IMatrixRow
    {
        #region Initialization
        internal MatrixDataCell(MatrixField containingField)
            : base(containingField)
        {
            _dictCellValues = new Dictionary<MatrixColumn, object[]>();
        }

        /// <summary>
        /// A placeholder to store merge values for each row
        /// </summary>
        internal IComparable[] MergeValues
        {
            get;
            set;
        }

        /// <summary>
        /// The data item of the row to which this cell belongs. It is stored here so that it can
        /// be accessed during the <see cref="MatrixField.MatrixRowDataBound"/> event. It is null
        /// at all other times.
        /// </summary>
        internal object DataItem { get; set; }

        /// <summary>
        /// Key contains column, value contains the value to be displayed in that column
        /// </summary>
        private readonly Dictionary<MatrixColumn, object[]> _dictCellValues;

        /// <summary>
        /// Dictionary of matrix values which will be displayed in this row
        /// </summary>
        internal IDictionary<MatrixColumn, object[]> CellValues
        {
            get
            {
                return _dictCellValues;
            }
        }

        /// <summary>
        /// Adds the item template to addInCell. If ItemTemplate is null, adds the default template.
        /// </summary>
        /// <param name="matrixColumn">The column in which the values will be displayed</param>
        /// <param name="binder">The binder to use for data binding</param>
        private TableCell DataBindItemTemplate(MatrixColumn matrixColumn, MatrixBinder binder)
        {
            TableCell childCell = new TableCell();
            object[] values = _dictCellValues[matrixColumn];
            MatrixField mf = (MatrixField)this.ContainingField;
            if (mf.ItemTemplate == null)
            {
                MatrixItemTemplate templ = new MatrixItemTemplate(mf.DataValueFormatString, values);
                templ.InstantiateIn(childCell);
            }
            else
            {
                if (mf.DataMergeFields == null)
                {
                    throw new Exception("ItemTemplate works only when DataMergeFields have been specified");
                }
                mf.ItemTemplate.InstantiateIn(childCell);
            }
            binder.MatrixColumn = matrixColumn;
            MatrixBinder.SetAsCurrent(binder);
            childCell.DataBind();
            return childCell;
        }

        /// <summary>
        /// Does not do anything. Actual databinding occurs in <see cref="Render"/>.
        /// </summary>
        public sealed override void DataBind()
        {
            //base.DataBind();
        }

        /// <summary>
        /// Called just before rendering for visible rows after all values for the row are available.
        /// </summary>
        /// <remarks>
        /// This should only be called if DefaultTemplate has been specified
        /// </remarks>
        private TableCell InstantiateDefaultTemplate(MatrixColumn matrixColumn, MatrixBinder binder)
        {
            MatrixField mf = (MatrixField)this.ContainingField;
            binder.MatrixColumn = matrixColumn;
            MatrixBinder.SetAsCurrent(binder);
            TableCell childCell = new TableCell();
            mf.DefaultTemplate.InstantiateIn(childCell);
            childCell.DataBind();
            return childCell;
        }

        #endregion

        #region Rendering
        protected override void Render(HtmlTextWriter writer)
        {
            //this.PerformDataBinding();
            MatrixField mf = (MatrixField)this.ContainingField;
            GridViewRow gridRow = (GridViewRow)this.NamingContainer;

            if (!mf.ColumnGroups.Any())
            {
                // We have no columns to show. Do nothing
                return;
            }

            // For each group, show the cell values and row totals
            MatrixBinder binder = new MatrixBinder(this);
            foreach (var group in mf.ColumnGroups)
            {
                foreach (var column in group)
                {
                    string customText = string.Empty;
                    bool b = _dictCellText != null && _dictCellText.TryGetValue(column, out customText);
                    if (b)
                    {
                        // User specified custom text gets precedence
                        this.ContainingField.ItemStyle.AddAttributesToRender(writer);
                        writer.RenderBeginTag(HtmlTextWriterTag.Td);
                        writer.Write(customText);
                        writer.RenderEndTag();
                    }
                    else if (_dictCellValues.ContainsKey(column))
                    {
                        //TableCell cell = _dictColumnCells[column];
                        TableCell cell = DataBindItemTemplate(column, binder);
                        cell.ApplyStyle(this.ContainingField.ItemStyle);
                        cell.RenderControl(writer);
                    }
                    else
                    {
                        if (mf.DefaultTemplate == null)
                        {
                            this.ContainingField.ItemStyle.AddAttributesToRender(writer);
                            writer.RenderBeginTag(HtmlTextWriterTag.Td);
                            writer.Write("&nbsp;");
                            writer.RenderEndTag();
                        }
                        else
                        {
                            TableCell cell = InstantiateDefaultTemplate(column, binder);
                            cell.DataBind();
                            cell.ApplyStyle(this.ContainingField.ItemStyle);
                            cell.RenderControl(writer);
                        }
                    }
                }
                if (mf.DisplayRowTotals)
                {
                    // Showing row totals
                    this.ContainingField.ItemStyle.AddAttributesToRender(writer);

                    writer.RenderBeginTag(HtmlTextWriterTag.Td);

                    if (_dictRowTotalText != null && _dictRowTotalText.ContainsKey(group.Key))
                    {
                        // Just display the custom total
                        writer.Write(_dictRowTotalText[group.Key]);
                    }
                    else
                    {
                        decimal?[] totals = this.GetRowTotals(group);
                        if (totals == null)
                        {
                            writer.Write("&nbsp;");
                        }
                        else
                        {
                            writer.Write(string.IsNullOrEmpty(mf.DataTotalFormatString) ?
                                mf.DataValueFormatString : mf.DataTotalFormatString,
                                totals.Cast<object>().ToArray());
                        }
                    }

                    writer.RenderEndTag();
                }
            }
        }
        #endregion



        #region IMatrixRow Members


        /// <summary>
        /// Pass null to get total of all columns
        /// </summary>
        /// <param name="columns"></param>
        /// <returns></returns>
        public decimal?[] GetRowTotals(IEnumerable<MatrixColumn> columns)
        {
            MatrixField mf = (MatrixField)this.ContainingField;

            IEnumerable<int> indexes = mf.DataValueFields
                .Where((p, i) =>
                    string.IsNullOrEmpty(mf.DataTotalFormatString) ||
                    mf.DataTotalFormatString.Contains("{" + i.ToString())
                ).Select((p, i) => i);
            decimal?[] rowTotals = null;
            foreach (var item in _dictCellValues)
            {
                if (columns != null && !columns.Contains(item.Key))
                {
                    // Ignore the column
                    continue;
                }
                if (rowTotals == null)
                {
                    rowTotals = new decimal?[item.Value.Length];
                }
                decimal d;
                foreach (int i in indexes)
                {
                    if (item.Value[i] != DBNull.Value)
                    {
                        d = Convert.ToDecimal(item.Value[i]);
                        if (rowTotals[i] == null)
                        {
                            rowTotals[i] = d;
                        }
                        else
                        {
                            rowTotals[i] += d;
                        }
                    }
                }
            }
            return rowTotals;
        }

        /// <summary>
        /// Dictionary which stores custom text for each column.
        /// </summary>
        private Dictionary<MatrixColumn, string> _dictCellText;

        /// <summary>
        /// Displays the specified text in the passed <see cref="MatrixColumn"/>
        /// </summary>
        /// <param name="col"></param>
        /// <param name="text"></param>
        /// <remarks>
        /// <para>
        /// This function is designed to be used in conjunction with the
        /// <see cref="MatrixField.MatrixRowDataBound"/> event. Refer to it for a usage example.
        /// </para>
        /// </remarks>
        public void SetCellText(MatrixColumn col, string text)
        {
            if (_dictCellText == null)
            {
                _dictCellText = new Dictionary<MatrixColumn, string>();
            }
            string curValue;
            bool b = _dictCellText.TryGetValue(col, out curValue);
            if (_dictCellText.ContainsKey(col))
            {
                _dictCellText[col] = text;
            }
            else
            {
                _dictCellText.Add(col, text);
            }

        }

        /// <summary>
        /// If row total text for a group exists here, it is used instead of computing the actual row total
        /// </summary>
        private Dictionary<int, string> _dictRowTotalText;

        /// <summary>
        /// Implements <see cref="IMatrixRow.SetRowTotalText"/>
        /// </summary>
        /// <param name="groupIndex"></param>
        /// <param name="text"></param>
        public void SetRowTotalText(int groupIndex, string text)
        {
            if (_dictRowTotalText == null)
            {
                _dictRowTotalText = new Dictionary<int, string>();
            }
            _dictRowTotalText[groupIndex] = text;
        }

        /// <summary>
        /// Returns the value of the first field specified in <see cref="MatrixField.DataValueFields"/> of the passed column.
        /// </summary>
        /// <param name="col">The column whose value is needed</param>
        /// <returns>The value of the first field</returns>
        public object GetCellValue(MatrixColumn col)
        {
            object[] values;
            bool b = _dictCellValues.TryGetValue(col, out values);
            return b ? values[0] : null;
        }

        /// <summary>
        /// Implements <see cref="IMatrixRow.SetCellValue"/>
        /// </summary>
        /// <param name="col"></param>
        /// <param name="fieldName"></param>
        /// <param name="obj"></param>
        public void SetCellValue(MatrixColumn col, string fieldName, object obj)
        {
            MatrixField mf = (MatrixField)this.ContainingField;
            object[] values;
            bool b = _dictCellValues.TryGetValue(col, out values);
            if (!b)
            {
                values = new object[mf.DataValueFields.Length];
                _dictCellValues.Add(col, values);
            }
            var index = mf.DataValueFields.Select((p, i) => p == fieldName ? i : -1).First(p => p >= 0);
            values[index] = obj;
        }

        #endregion

        /// <summary>
        /// Not supported
        /// </summary>
        /// <exception cref="NotSupportedException">Always raised</exception>
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override string Text
        {
            get
            {
                throw new NotSupportedException();
            }
            set
            {
                throw new NotSupportedException();
            }
        }

        internal void UpdateCellValues(MatrixColumn matrixColumn, object dataItem)
        {
            MatrixField mf = (MatrixField)this.ContainingField;
            object[] cellValues = (from dataField in mf.DataValueFields
                                   select DataBinder.Eval(dataItem, dataField)).ToArray();
            try
            {
                this.CellValues.Add(matrixColumn, cellValues);
                //this.AddTemplateToCell(matrixColumn, cellValues);
            }
            catch (ArgumentException)
            {
                // This happens when the master columns have not been properly specified.
                // Yash TODO: Give example
                // Key already exists. Show all the values to help the programmer diagnose
                string str = matrixColumn.ToString();
                throw new Exception(str);
                //curMatrixRow.CellValues[matrixColumn][0] = string.Format("{0}; {1}", curMatrixRow.CellValues[matrixColumn][0], cellValues[0]);
            }
        }


    }
}
