using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web.UI;
using EclipseLibrary.Utilities;
using EclipseLibrary.Formatting;

namespace EclipseLibrary.Web.JQuery
{
    /// <summary>
    /// Orders matrix columns according to sort values
    /// </summary>
    [Obsolete("Use MatrixField in namespace EclipseLibrary.Web.JQuery.Matrix")]
    internal class MatrixColumnComparer : IComparer<MatrixColumn>
    {
        private readonly string[] _fieldNames;
        private readonly string[] _descendingFields;

        public MatrixColumnComparer(IEnumerable<string> fieldNames)
        {
            if (fieldNames != null)
            {
                _descendingFields = fieldNames.Where(p => p.EndsWith("$")).Select(p => p.TrimEnd('$')).ToArray();
                _fieldNames = fieldNames.Select(p => p.TrimEnd('$')).ToArray();
            }
        }

        #region IComparer<MatrixColumn> Members

        /// <summary>
        /// nulls compare equal. Provides NULLS FIRST behavior.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public int Compare(MatrixColumn x, MatrixColumn y)
        {
            if (_fieldNames == null)
            {
                // All columns are equal
                return 0;
            }

            int ret = CompareNulls(x.ColumnValues, y.ColumnValues);
            if (ret != 2)
            {
                return ret;
            }

            foreach (string field in _fieldNames)
            {
                IComparable val1 = (IComparable)x[field];
                IComparable val2 = (IComparable)y[field];
                ret = CompareNulls(val1, val2);
                if (ret == 2)
                {
                    // Both val1 and val2 are non null
                    ret = val1.CompareTo(val2);
                }
                if (ret != 0)
                {
                    if (_descendingFields.Contains(field))
                    {
                        // Toggle return value
                        return ret == -1 ? 1 : -1;
                    }
                    else
                    {
                        return ret;
                    }
                }
            }

            // All common values are equal.
            return 0;
        }

        #endregion

        /// <summary>
        /// Returns 2 if both o1 and o2 are non null. Otherwise provides NULLS FIRST behavior
        /// </summary>
        /// <param name="o1"></param>
        /// <param name="o2"></param>
        /// <returns></returns>
        internal static int CompareNulls(object o1, object o2)
        {
            if (o1 == null && o2 == null)
            {
                return 0;
            }
            else if (o1 == null)
            {
                // NULLS FIRST
                return -1;
            }
            else if (o2 == null)
            {
                return 1;
            }
            return 2;
        }
    }

    /// <summary>
    /// Column specific values
    /// </summary>
    /// <include file='MatrixColumn.xml' path='MatrixColumn/doc[@name="class"]/*'/>
    [ParseChildren(true)]
    [PersistChildren(false)]
    [Browsable(true)]
    [Obsolete("Use MatrixField in namespace EclipseLibrary.Web.JQuery.Matrix")]
    public class MatrixColumn
    {
        /// <summary>
        /// Cannot be null
        /// </summary>
        private readonly IDictionary<string, object> _columnValues;

        /// <summary>
        /// Constructor used when columns are manually constructed from user code
        /// </summary>
        public MatrixColumn()
        {
            this.Visible = true;
            _columnValues = new Dictionary<string, object>();
        }

        /// <summary>
        /// Constructor used when <see cref="MatrixField"/> constructs the column from data
        /// </summary>
        /// <remarks>
        /// It is very important that the concatenation of fields is done in the order 
        /// <see cref="MatrixField.DataHeaderFields" /> + <see cref="MatrixField.DataHeaderCustomFields" />
        /// + <see cref="MatrixField.DataHeaderSortFields"/> as documented in <see cref="MatrixField.DataHeaderFormatString"/>.
        /// </remarks>
        internal MatrixColumn(MatrixField mf, IDictionary<string, object> columnValues)
        {
            if (columnValues == null)
            {
                throw new ArgumentNullException("columnValues");
            }
            this.Visible = true;
            _columnValues = columnValues;
            ConditionalFormatter formatter = new ConditionalFormatter(p => _columnValues.ContainsKey(p) ? _columnValues[p] : string.Empty);
            object[] values = mf.DataHeaderFields.Select(p => this[p])
                .Concat(mf.DataHeaderCustomFields.Select(p => this[p]))
                .Concat(mf.DataHeaderSortFields.Select(p => this[p]))
                .ToArray();
            this.HeaderText = string.Format(formatter, mf.DataHeaderFormatString, values);
        }

        /// <summary>
        /// Column specific data values
        /// </summary>
        /// <remarks>
        /// It contains values of the fields referenced in <see cref="MatrixField.DataHeaderSortFields"/>,
        /// <see cref="MatrixField.DataHeaderFields"/> and <see cref="MatrixField.DataHeaderFields"/>.
        /// </remarks>
        [Browsable(false)]
        public IDictionary<string, object> ColumnValues
        {
            get
            {
                return _columnValues;
            }
        }

        /// <summary>
        /// Returns column value corresponding to the passed field
        /// </summary>
        /// <param name="field">The field whose value should be looked up</param>
        /// <returns>The value corresponding to the field, or null if the value does not exist</returns>
        /// <remarks>
        /// By contrast, accessing the value via <see cref="ColumnValues"/>[field] will raise an exception
        /// if the value does not exist.
        /// </remarks>
        public object this[string field]
        {
            get
            {
                object obj;
                bool b = _columnValues.TryGetValue(field, out obj);
                if (b)
                {
                    // Found
                    return obj;
                }
                else
                {
                    return null;
                }
            }

        }

        /// <summary>
        /// Returns true if the column constructed based on the data item would have been equivalent to this column
        /// </summary>
        /// <remarks>
        /// It evaluates each header field in the data item and compares it to the values stored in <see cref="ColumnValues"/>.
        /// Compatibility case is also handled
        /// </remarks>
        internal bool IsEquivalentTo(string[] headerFields, IComparable[] headerValues)
        {
            // Normal case
            var seq1 = headerFields.Select(field => this[field] as IComparable);
            //var seq2 = mf.DataHeaderFields.Select(field => DataBinder.Eval(dataItem, field) as IComparable);
            return seq1.SequenceEqual(headerValues);
        }

        private string _headerText = string.Empty;
        private string _firstRowText = string.Empty;
        private string _secondRowText = string.Empty;

        ///<summary>
        ///  If you are manually adding columns, you can specify the header text as well.
        ///</summary>
        /// <include file='MatrixColumn.xml' path='MatrixColumn/doc[@name="HeaderText"]/*'/>
        [Browsable(true)]
        [PersistenceMode(PersistenceMode.Attribute)]
        public string HeaderText
        {
            get
            {
                return _headerText;
            }
            set
            {
                _headerText = value;
                int pipeIndex = _headerText.IndexOf('|');
                if (pipeIndex < 0)
                {
                    _firstRowText = string.Empty;
                    _secondRowText = value;
                }
                else
                {
                    _firstRowText = value.Substring(0, pipeIndex);
                    _secondRowText = value.Substring(pipeIndex + 1);
                }
            }
        }

        /// <summary>
        /// Returns the text before the pipe. If no pipe, returns empty string
        /// </summary>
        internal string FirstRowText
        {
            get
            {
                return _firstRowText;
            }
        }

        /// <summary>
        /// Returns the text after the pipe. If no pipe, returns the text itself.
        /// </summary>
        internal string SecondRowText
        {
            get
            {
                return _secondRowText;
            }
        }

        /// <summary>
        /// To hide display of particular values, set this property to false.
        /// </summary>
        /// <remarks>
        /// Invisible columns do not participate in row and column totals.
        /// </remarks>
        [Browsable(true)]
        [DefaultValue(true)]
        public bool Visible { get; set; }

        public override string ToString()
        {
            return _headerText;
        }

    }
}
