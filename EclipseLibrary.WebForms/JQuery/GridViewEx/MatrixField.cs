using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace EclipseLibrary.Web.JQuery
{
    /// <summary>
    /// Arguments passed with the <see cref="MatrixField.MatrixRowDataBound"/> event.
    /// </summary>
    /// <include file='MatrixField.xml' path='MatrixField/doc[@name="class"]/*'/>
    [ParseChildren(true)]
    [PersistChildren(false)]
    [Obsolete("Use MatrixField in namespace EclipseLibrary.Web.JQuery.Matrix")]
    public partial class MatrixField : DataControlField, IHasCustomCells
    {
        #region Row and column data
        /// <summary>
        /// Value to display in header and footer. Key displays in header, value displays in footer
        /// </summary>
        private readonly Collection<MatrixColumn> _matrixColumns;

        /// <summary>
        /// List of all matrix columns. GridViewExMatrixCell uses this list to display
        /// column headings.
        /// </summary>
        /// <include file='MatrixField.xml' path='MatrixField/doc[@name="MatrixColumns"]/*'/>
        [Browsable(true)]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        [NotifyParentProperty(true)]
        public Collection<MatrixColumn> MatrixColumns
        {
            get
            {
                return _matrixColumns;
            }
        }

        /// <summary>
        ///  Returns the column total for the passed column.
        /// </summary>
        /// <param name="col"></param>
        /// <returns></returns>
        /// <include file='MatrixField.xml' path='MatrixField/doc[@name="GetColumnTotal"]/*'/>
        public decimal?[] GetColumnTotal(MatrixColumn col)
        {
            // Matrix data cells which need to be summed up
            var columnValues = _gv.Rows.Cast<GridViewRow>()
                .Where(p => p.Visible)
                .SelectMany(p => p.Cells.OfType<MatrixDataCell>())
                .Where(p => p.ContainingField == this && p.CellValues.ContainsKey(col))
                .Select(p => p.CellValues[col]);

            // Selects only those indexes which have been specified in DataFooterFormatString.
            IEnumerable<int> indexes = DataValueFields.Where((p, i) =>
                string.IsNullOrEmpty(this.DataTotalFormatString) ||
                this.DataTotalFormatString.Contains("{" + i.ToString())
                ).Select((p, i) => i);
            decimal?[] totals = new decimal?[this.DataValueFields.Length];
            foreach (int i in indexes)
            {
                foreach (var row in columnValues.Where(p => p[i] != null && p[i] != DBNull.Value)
                    .Select(p => Convert.ToDecimal(p[i])))
                {
                    totals[i] = (totals[i] ?? 0) + row;
                }
            }
            return totals;
        }


        private ILookup<int, MatrixColumn> _columnGroups;

        /// <summary>
        /// The key is the index of each group. The value is the column.
        /// </summary>
        internal ILookup<int, MatrixColumn> ColumnGroups
        {
            get
            {
                if (_columnGroups == null)
                {
                    List<KeyValuePair<int, MatrixColumn>> list = new List<KeyValuePair<int, MatrixColumn>>();
                    MatrixColumn prevCol = null;
                    int groupIndex = 0;

                    // Retrieve columns sorted by sort fields
                    var sortedColumns = this.MatrixColumns.Where(p => p.Visible);

                    // If no sort fields, user header fields
                    if (this.DataHeaderSortFields == null || this.DataHeaderSortFields.Length == 0)
                    {
                        sortedColumns = sortedColumns.OrderBy(p => p, new MatrixColumnComparer(this.DataHeaderFields));
                    }
                    else
                    {
                        sortedColumns = sortedColumns.OrderBy(p => p, new MatrixColumnComparer(this.DataHeaderSortFields));
                    }

                    foreach (MatrixColumn col in sortedColumns)
                    {
                        if (prevCol == null)
                        {
                            prevCol = col;
                        }
                        else if (prevCol.FirstRowText != col.FirstRowText)
                        {
                            prevCol = col;
                            ++groupIndex;
                        }
                        list.Add(new KeyValuePair<int, MatrixColumn>(groupIndex, col));
                    }
                    _columnGroups = list.ToLookup(p => p.Key, p => p.Value);
                }
                return _columnGroups;
            }
        }
        #endregion

        #region Initialization
        public MatrixField()
        {
            //_matrixCells = new List<MatrixDataCell>();
            _matrixColumns = new Collection<MatrixColumn>();
            this.DataValueFormatString = "{0}";
            this.DataHeaderFormatString = "{0}";
            this.ItemStyle.HorizontalAlign = HorizontalAlign.Right;
            this.FooterStyle.HorizontalAlign = HorizontalAlign.Right;
            this.RowTotalHeaderText = "Total";
            this.HeaderText = "&nbsp;";
        }

        /// <exclude />
        /// <summary>
        /// Not implemented
        /// </summary>
        /// <returns></returns>
        protected override DataControlField CreateField()
        {
            throw new NotImplementedException();
        }

        private GridViewEx _gv;
        /// <summary>
        /// Hook up the row data bound and pre render events of the grid
        /// </summary>
        /// <param name="sortingEnabled">Not used</param>
        /// <param name="control">This must be of type <see cref="GridViewEx"/>.</param>
        /// <returns></returns>
        public override bool Initialize(bool sortingEnabled, Control control)
        {
            _gv = (GridViewEx)control;
            if (this.DataMergeFields == null)
            {
                // DataMergeFields are optional only in compatibility mode
                throw new Exception("DataMergeFields must be specified");
            }
            else
            {
                _gv.RowCreated += new GridViewRowEventHandler(gv_RowCreated);
                _gv.DataBound += new EventHandler(gv_DataBound);
            }

            return base.Initialize(sortingEnabled, control);
        }

        #endregion

        #region Grid event handlers
        private void gv_DataBound(object sender, EventArgs e)
        {
            MatrixDataCell cell = LastVisibleMatrixCell();

            if (cell != null && cell.DataItem != null)
            {
                // cell.DataItem is null, we have already raised this event when footer was created
                MatrixRowEventArgs args = new MatrixRowEventArgs(cell);
                //MatrixRowDataBound(this, args);
                //cell.DataItem = null;
                OnMatrixRowDataBound(args);
            }
        }

        /// <summary>
        /// Find out whether we have already added the master of this row to our list. If not, add it now.
        /// If yes, make the row invisible. Also update the dictionary of columns. When rendering begins, all rows
        /// will show all columns. Possibly, some columns will have blank values.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <remarks>
        /// Called only when DataMergeFields != null
        /// </remarks>
        private void gv_RowCreated(object sender, GridViewRowEventArgs e)
        {
            GridViewEx gv = (GridViewEx)sender;

            switch (e.Row.RowType)
            {
                case DataControlRowType.DataRow:
                    MatrixDataCell previousCell = LastVisibleMatrixCell();
                    MatrixColumn matrixColumn = MaybeCreateColumn(e.Row.DataItem);
                    IComparable[] masterValues = (
                        from masterValue in this.DataMergeFields
                        select DataBinder.Eval(e.Row.DataItem, masterValue) as IComparable
                        ).ToArray();
                    bool bMasterSame = previousCell != null && previousCell.MergeValues.SequenceEqual(masterValues);
                    if (bMasterSame)
                    {
                        e.Row.Visible = false;
                        previousCell.UpdateCellValues(matrixColumn, e.Row.DataItem);
                    }
                    else
                    {
                        if (previousCell != null)
                        {
                            GridViewRow previousRow = (GridViewRow)previousCell.NamingContainer;
                            if ((previousRow.RowState & DataControlRowState.Alternate) == DataControlRowState.Alternate)
                            {
                                e.Row.RowState &= ~DataControlRowState.Alternate;
                            }
                            else
                            {
                                e.Row.RowState |= DataControlRowState.Alternate;
                            }
                            MatrixRowEventArgs args = new MatrixRowEventArgs(previousCell);
                            OnMatrixRowDataBound(args);
                        }

                        // Save the data item for use in the event
                        MatrixDataCell currentMatrixCell = e.Row.Cells.OfType<MatrixDataCell>()
                            .Where(p => p.ContainingField == this).Single();
                        currentMatrixCell.MergeValues = masterValues;
                        currentMatrixCell.DataItem = e.Row.DataItem;

                        currentMatrixCell.UpdateCellValues(matrixColumn, e.Row.DataItem);
                    }
                    break;

                case DataControlRowType.Footer:
                    // Last cell
                    MatrixDataCell cell = LastVisibleMatrixCell();
                    if (cell != null)
                    {
                        MatrixRowEventArgs args = new MatrixRowEventArgs(cell);
                        OnMatrixRowDataBound(args);
                    }
                    break;
            }
        }

        /// <summary>
        /// Raised when all columns of a matrix row have been populated
        /// </summary>
        /// <include file='MatrixField.xml' path='MatrixField/doc[@name="MatrixRowDataBound"]/*'/>
        public event EventHandler<MatrixRowEventArgs> MatrixRowDataBound;

        /// <summary>
        /// Invokes all delegates attached to the <see cref="MatrixRowDataBound"/> event.
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnMatrixRowDataBound(MatrixRowEventArgs e)
        {
            MatrixDataCell cell = (MatrixDataCell)e.MatrixRow;
            if (MatrixRowDataBound != null)
            {
                MatrixRowDataBound(this, e);
            }
            cell.DataItem = null;
        }
        #endregion


        #region Public Properties

        private string[] _dataHeaderFields;
        /// <summary>
        /// The data field whose value will be displayed as the column header
        /// </summary>
        /// <remarks>
        /// <para>
        /// Each distinct value of the <c>DataHeaderField</c> becomes a column of the <see cref="MatrixField"/>.
        /// </para>
        /// <para>
        /// You can control the format in which the header field value displays by setting the
        /// <see cref="DataHeaderFormatString" /> property. When the value of the <c>DataHeaderField</c> is null,
        /// then <see cref="DefaultTemplate"/> is used instead.
        /// </para>
        /// <para>
        /// If there are no header fields, an empty string array is returned.
        /// </para>
        /// </remarks>
        /// <example>
        /// For a simple example, see <see cref="MatrixField"/>. The following markup demonstrates the use of multiple
        /// fields as DataHeaderFields.
        /// <code lang="XML">
        /// <![CDATA[
        ///<jquery:GridViewEx ID="gv" runat="server" AutoGenerateColumns="false" DataSourceID="ds"
        ///    AllowSorting="true" ShowFooter="true">
        ///    <Columns>
        ///        <asp:BoundField HeaderText="Package Name" DataField="PackageName" />
        ///        <jquery:MatrixField DataHeaderFields="GroupDescription,ActivityDescription,ActivityColumnNumber"
        ///            DataHeaderSortFields="GroupColumnNumber,ActivityColumnNumber" DataHeaderFormatString="{0}|{1}<br/>({2})" DataValueFields="PackageActivtiyData"
        ///            DataMergeFields="PackageId" DataValueFormatString="{0:N2}" DisplayRowTotals="true"
        ///            RowTotalHeaderText="Total Advances|Total Recoveries" OnMatrixRowDataBound="gv_MatrixRowDataBound">
        ///        </jquery:MatrixField>
        ///        <eclipse:MultiBoundField HeaderText="Advance Outstanding" AccessibleHeaderText="advOut"
        ///            DataFormatString="{0:N2}" ItemStyle-HorizontalAlign="Right" />
        ///    </Columns>
        ///</jquery:GridViewEx>
        /// ]]>
        /// </code>
        /// </example>
        [Browsable(true)]
        [TypeConverterAttribute(typeof(StringArrayConverter))]
        public string[] DataHeaderFields
        {
            get
            {
                // Never return null
                if (_dataHeaderFields == null)
                {
                    _dataHeaderFields = new string[0];
                }
                return _dataHeaderFields;
            }
            set
            {
                _dataHeaderFields = value;
            }
        }

        ///<summary>
        ///  Values from <see cref="DataValueFields" /> becomes the <c>HeaderText</c> of each matrix columns.
        ///  Use this property to format the header field values.
        ///</summary>
        /// <include file='MatrixField.xml' path='MatrixField/doc[@name="DataHeaderFormatString"]/*'/>
        [Browsable(true)]
        [DefaultValue("{0}")]
        public string DataHeaderFormatString { get; set; }

        private string[] _dataHeaderSortFields;
        /// <summary>
        /// In what order should the columns be displayed. The columns are displayed ascending in the order
        /// of this field value.
        /// </summary>
        /// <include file='MatrixField.xml' path='MatrixField/doc[@name="DataHeaderSortFields"]/*'/>
        [Browsable(true)]
        [TypeConverterAttribute(typeof(StringArrayConverter))]
        public string[] DataHeaderSortFields
        {
            get
            {
                // Never return null
                if (_dataHeaderSortFields == null)
                {
                    _dataHeaderSortFields = new string[0];
                }
                return _dataHeaderSortFields;
            }
            set
            {
                _dataHeaderSortFields = value;
            }
        }

        private string[] _dataHeaderCustomFields;

        /// <summary>
        /// If you are writing code and wish to distinguish between columns, you can store some custom field values.
        /// </summary>
        /// <include file='MatrixField.xml' path='MatrixField/doc[@name="DataHeaderCustomFields"]/*'/>
        [Browsable(true)]
        [TypeConverterAttribute(typeof(StringArrayConverter))]
        public string[] DataHeaderCustomFields
        {
            get
            {
                // Never return null
                if (_dataHeaderCustomFields == null)
                {
                    _dataHeaderCustomFields = new string[0];
                }
                return _dataHeaderCustomFields;
            }
            set
            {
                _dataHeaderCustomFields = value;
            }
        }

        /// <summary>
        /// If a template is specified, DataValueFields are used only for totalling.
        /// </summary>
        /// <include file='MatrixField.xml' path='MatrixField/doc[@name="ItemTemplate"]/*'/>
        [
            Browsable(false),
            PersistenceMode(PersistenceMode.InnerProperty),
            DefaultValue(typeof(ITemplate), ""),
            Description("Item template"),
            TemplateContainer(typeof(GridViewRow))
        ]
        public ITemplate ItemTemplate { get; set; }

        /// <summary>
        /// Default template to be used for cells which have no data. 
        /// </summary>
        /// <remarks>
        ///     <para>
        /// Normally cells which have no data are displayed as empty.
        /// But you can display the contents in your template here. Remember that Eval("") will not work in the
        /// DefaultTemplate. Instead you must use MatrixBinder.Eval()
        ///     </para>
        ///     <para>Refer <see cref="ItemTemplate"/> to see the example.
        ///     </para>
        /// </remarks>
        [
            Browsable(false),
            PersistenceMode(PersistenceMode.InnerProperty),
            DefaultValue(typeof(ITemplate), "")
        ]
        public ITemplate DefaultTemplate { get; set; }


        /// <summary>
        /// If the previous and current rows have the same values for all merge fields, the current row will be
        /// hidden, and the data for the second row will be shown in the previous row.
        /// </summary>
        /// <remarks>
        /// <para>
        /// It is very important that the rows within the data source are pre sorted by the fields specified here.
        /// As long as the rows have the same values for all <c>DataMergeFields</c>, they are collapsed into a single
        /// row. As soon as any of the <c>DataMergeFields</c> change, we begin a new row.
        /// </para>
        /// <note type="caution">
        /// You must ensure that all fields specified in <c>DataMergeFields</c> and <see cref="DataHeaderFields"/>
        /// should be part of the conceptual primary key of the query output. Otherwise you will encounter exceptions.
        /// </note>
        /// </remarks>
        /// <example>
        /// <para>
        /// Following is a basic markup which uses <c>DataMergeFields</c>.
        /// </para>
        /// <code>
        /// <![CDATA[
        ///<jquery:GridViewEx ID="gv" runat="server" DataSourceID="ds">
        ///    <Columns>
        ///        <asp:BoundField DataField="upc_code" HeaderText="UPC" />
        ///        <jquery:MatrixField DataHeaderField="carton_storage_area" DataValueFields="quantity"
        ///            DataMergeFields="upc_code" HeaderText="Pieces">
        ///        </jquery:MatrixField>
        ///    </Columns>
        ///</jquery:GridViewEx>        
        /// ]]>
        /// </code>
        /// </example>
        [Browsable(true)]
        [TypeConverterAttribute(typeof(StringArrayConverter))]
        public string[] DataMergeFields { get; set; }

        /// <summary>
        /// Value of this field will be displayed in the matrix column.
        /// </summary>
        /// <remarks>
        /// You can specify more than one field for this property, seperated by commas.
        /// <see cref="DataValueFormatString"/> can be used to determine exactly what is displayed in the column.
        /// </remarks>
        /// <example>
        /// <para>
        /// In this markup we are displaying two values in each matrix cell:
        /// pieces and percent damaged. We had to specify the <see cref="DataValueFormatString"/> to define how the
        /// two values should be formatted.
        /// </para>
        /// <code>
        /// <![CDATA[
        ///<jquery:GridViewEx ID="gv" runat="server" DataSourceID="ds">
        ///    <Columns>
        ///        <asp:BoundField DataField="upc_code" HeaderText="UPC" />
        ///        <jquery:MatrixField DataHeaderField="carton_storage_area" DataValueFields="quantity,percentage_damaged"
        ///            DataMergeFields="upc_code" HeaderText="Pieces" DataValueFormatString="{0:N0} (1:p2}">
        ///        </jquery:MatrixField>
        ///    </Columns>
        ///</jquery:GridViewEx> 
        /// ]]>
        /// </code>
        /// <para>
        /// For an example of displaying multiple values in each cell, <see cref="DisplayColumnTotals"/>.
        /// </para>
        /// </example>
        [Browsable(true)]
        [TypeConverterAttribute(typeof(StringArrayConverter))]
        public string[] DataValueFields { get; set; }

        /// <summary>
        /// Format of value fields to display in each matrix cell.
        /// </summary>
        /// <remarks>
        /// <para>
        /// If the query is retrieving numbers, then you can specify this as <c>{0:N0}</c> to ensure that the
        /// correct number of decimal places are output. If multiple fields have been specified for 
        /// <see cref="DataValueFields"/>, then you can reference them similarly. See how to specify
        /// <c>DataValueFormatString</c>in <see cref="DataValueFields"/>.
        /// </para>
        /// <para>
        /// If a simple format string is not sufficient for your markup requirements, you can use
        /// <see cref="ItemTemplate"/>.
        /// </para>
        /// <para>
        /// For an example of displaying multiple values in each cell, <see cref="DisplayColumnTotals"/>.
        /// </para>
        /// </remarks>
        [Browsable(true)]
        [DefaultValue("{0}")]
        public string DataValueFormatString { get; set; }

        /// <summary>
        /// Defaults to DataValueFormatString. Only those fields are summed for which there are place holders in this
        /// format string.
        /// </summary>
        /// <remarks>
        /// Follow the links to see, how the value of this property is used to display all totals including <see cref="DisplayRowTotals"/>,
        /// <see cref="DisplayColumnTotals"/> and subtotals.
        /// 
        /// </remarks>
        [Browsable(true)]
        [DefaultValue("{0}")]
        public string DataTotalFormatString { get; set; }

        ///<summary>
        ///  Whether row totals should be displayed.
        ///</summary>
        /// <include file='MatrixField.xml' path='MatrixField/doc[@name="DisplayRowTotals"]/*'/>
        [Browsable(true)]
        public bool DisplayRowTotals { get; set; }

        /// <summary>
        /// Whether column totals should be displayed
        /// </summary>
        /// <include file='MatrixField.xml' path='MatrixField/doc[@name="DisplayColumnTotals"]/*'/>
        [Browsable(true)]
        public bool DisplayColumnTotals { get; set; }

        /// <summary>
        /// Header text to be displayed against the DisplayRowTotals totals
        /// </summary>
        /// <include file='MatrixField.xml' path='MatrixField/doc[@name="RowTotalHeaderText"]/*'/>
        [Browsable(true)]
        public string RowTotalHeaderText { get; set; }

        /// <summary>
        /// Header text to be displayed against the field
        /// </summary>
        /// <remarks>
        /// <para>
        /// The header text you specify is displayed in the first row of the header. The second row of the header
        /// always contains the header text of each matrix column. The <see cref="RowTotalHeaderText"/> is displayed
        /// for the row total column if <see cref="DisplayRowTotals"/> is true. This header text does not span the
        /// row total column. In fact, this header text does not span previous and next cells either.
        /// </para>
        /// <para>
        /// Specifying the header text is highly recommended. Otherwise the first header row will be empty
        /// which looks very ugly. Whenever a <see cref="MatrixField"/> exists within a grid, there will always
        /// be two header rows.
        /// </para>
        /// </remarks>
        /// <exception cref="ArgumentException">The value cannot contain the <c>|</c>
        /// symbol since only one row of header text can be specified.
        /// The second row is reserved for <see cref="MatrixColumn.HeaderText"/>.
        /// </exception>
        [Browsable(true)]
        [DefaultValue("&nbsp;")]
        public override string HeaderText
        {
            get
            {
                return base.HeaderText;
            }
            set
            {
                if (value.Contains("|"))
                {
                    throw new ArgumentException("Header text cannot contain |", "value");
                }
                if (string.IsNullOrEmpty(value))
                {
                    base.HeaderText = "&nbsp;|xx";
                }
                else
                {
                    base.HeaderText = value + "|xx";
                }
            }
        }

        /// <exclude />
        /// <summary>
        /// SortExpression is ignored
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [ReadOnly(true)]
        public override string SortExpression
        {
            get
            {
                return string.Empty;
            }
            set
            {

            }
        }

        #endregion

        #region IHasCustomCells

        //private MatrixDataCell _currentMatrixCell;

        /// <exclude/>
        /// <summary>
        /// </summary>
        /// <param name="cellType"></param>
        public DataControlFieldCell CreateCell(DataControlCellType cellType)
        {
            switch (cellType)
            {
                case DataControlCellType.Header:
                    return new MatrixHeaderCell(this);

                case DataControlCellType.DataCell:
                    return new MatrixDataCell(this);
                //return _currentMatrixCell;

                case DataControlCellType.Footer:
                    return new MatrixFooterCell(this);

                default:
                    throw new NotImplementedException();
            }
        }

        /// <exclude/>
        /// <summary>
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="firstRowIndex"></param>
        /// <param name="lastRowIndex"></param>
        /// <param name="parentRow"></param>
        public void RenderSubtotals(GridViewRow parentRow, HtmlTextWriter writer, int firstRowIndex, int lastRowIndex)
        {
            MatrixFooterCell footerCell = new MatrixFooterCell(parentRow, this, firstRowIndex, lastRowIndex);
            footerCell.RenderControl(writer);
        }

        #endregion

        #region Private implementation
        /// <summary>
        /// Can return null if grid has no rows
        /// </summary>
        /// <returns></returns>
        private MatrixDataCell LastVisibleMatrixCell()
        {
            GridViewRow lastVisibleRow = null;
            for (int i = _gv.Rows.Count - 1; i >= 0; --i)
            {
                if (_gv.Rows[i].Visible)
                {
                    lastVisibleRow = _gv.Rows[i];
                    break;
                }
            }
            if (lastVisibleRow == null)
            {
                return null;
            }
            return lastVisibleRow.Cells.OfType<MatrixDataCell>().FirstOrDefault(p => p.ContainingField == this);
            //return _gv.Rows.Cast<GridViewRow>()
            //    .Where(p => p.Visible)
            //    .Reverse()
            //    .SelectMany(p => p.Cells.OfType<MatrixDataCell>())
            //    .FirstOrDefault(p => p.ContainingField == this);
        }

        /// <summary>
        /// The matrix column is created if it does not already exist.
        /// </summary>
        /// <param name="dataItem">The data item which will be evaluated</param>
        /// <returns>The column referenced by the <see cref="DataHeaderFields"/></returns>
        private MatrixColumn MaybeCreateColumn(object dataItem)
        {
            IComparable[] headerValues = this.DataHeaderFields
                .Select(field => DataBinder.Eval(dataItem, field) as IComparable)
                .ToArray();
            MatrixColumn matrixColumn = _matrixColumns.FirstOrDefault(col => col.IsEquivalentTo(this.DataHeaderFields, headerValues));
            if (matrixColumn == null)
            {
                // DataHeaderFields, DataHeaderSortFields, DataHeaderCustomFields. Don't store nulls.
                Dictionary<string, object> dict =
                    this.DataHeaderFields
                    .Union(this.DataHeaderSortFields.Select(p => p.TrimEnd('$')))
                    .Union(this.DataHeaderCustomFields)
                    .Select(p => new KeyValuePair<string, object>(p, DataBinder.Eval(dataItem, p)))
                    .Where(p => p.Value != null && p.Value != DBNull.Value)
                    .ToDictionary(p => p.Key, q => q.Value);
                matrixColumn = new MatrixColumn(this, dict);
                _matrixColumns.Add(matrixColumn);
            }

            return matrixColumn;
        }
        #endregion




        public bool FilterRow(object dataItem, object peek)
        {
            return true;
        }
    }
}
