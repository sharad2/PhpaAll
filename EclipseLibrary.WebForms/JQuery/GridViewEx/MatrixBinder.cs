using System;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace EclipseLibrary.Web.JQuery
{
    /// <summary>
    /// Used for evaluating data binding expressions in DefaultTemplate
    /// </summary>
    /// <remarks>
    /// This class is used to evaluate data binding expressions specified in <see cref="MatrixField.ItemTemplate"/>.
    /// It is modeled after the standard <c>DataBinder</c> class and provides a overloaded public static functions
    /// <see cref="Eval(string)"/> and <see cref="Eval(string, string)"/>.
    /// </remarks>
    //[Obsolete("Use MatrixField in namespace EclipseLibrary.Web.JQuery.Matrix")]
    public class MatrixBinder
    {
        /// <summary>
        /// The field whose binding expressions are being evaluated
        /// </summary>
        private readonly MatrixField _matrixField;

        private readonly MatrixDataCell _cell;
        internal MatrixBinder(MatrixDataCell cell)
        {
            _matrixField = (MatrixField)cell.ContainingField;
            _cell = cell;
        }

        private MatrixColumn _matrixColumn;

        /// <summary>
        /// The matrix column in which the default template will be displayed
        /// </summary>
        internal MatrixColumn MatrixColumn
        {
            get { return _matrixColumn; }
            set { _matrixColumn = value; }
        }

        private string DoEval(string field, string formatString)
        {
            string str = string.Format(formatString, DoEval(field));
            return str;
        }

        /// <summary>
        /// Special handles the evaluation of the DataHeaderField. It returns the header value of the column
        /// instead of evaluating the data item.
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        private object DoEval(string field)
        {
            MatrixDataCell mr = _cell;
            object obj;
            if (_matrixField.DataHeaderFields
                .Union(_matrixField.DataHeaderSortFields)
                .Union(_matrixField.DataHeaderCustomFields).Contains(field))
            {
                obj =_matrixColumn[field];
            }
            else if (_matrixField.DataValueFields.Contains(field))
            {
                obj = _matrixField.DataValueFields
                    .Select((p, index) => p == field ? mr.CellValues[_matrixColumn][index] : null)
                    .FirstOrDefault(p => p != null);
            }
            else if (_matrixField.DataMergeFields.Contains(field))
            {
                obj = _matrixField.DataMergeFields
                    .Select((p, index) => p == field ? mr.MergeValues[index] : null)
                    .FirstOrDefault(p => p != null);
            }
            else
            {
                throw new ArgumentOutOfRangeException("field", field,
                    "Must be one of DataHeaderFields, DataHeaderSortFields, DataHeaderCustomFields, " +
                    "DataValueFields, DataMergeFields"
                );
            }
            return obj;
        }

        /// <summary>
        /// Returns the value after applying a format string
        /// </summary>
        /// <param name="field">The name of the field to evaluate.</param>
        /// <param name="formatString">The format string to use to format the results.</param>
        /// <returns>The value of the field as a string</returns>
        /// <remarks>
        /// The passed field must be one of the fields specified in <see cref="MatrixField.DataHeaderFields"/>,
        /// <see cref="MatrixField.DataHeaderSortFields"/>, <see cref="MatrixField.DataHeaderCustomFields"/>
        /// <see cref="MatrixField.DataValueFields"/> or <see cref="MatrixField.DataMergeFields"/>.
        /// </remarks>
        public static string Eval(string field, string formatString)
        {
            MatrixBinder info = (MatrixBinder)HttpContext.Current.Items["MatrixBinder"];
            return info.DoEval(field, formatString);
        }

        /// <summary>
        /// Evaluates the value of the passed field
        /// </summary>
        /// <param name="field">The name of the field to evaluate</param>
        /// <returns>Value of the field</returns>
        /// <remarks>
        /// <see cref="Eval(string,string)"/> for more detailed information.
        /// </remarks>
        public static object Eval(string field)
        {
            MatrixBinder info = (MatrixBinder)HttpContext.Current.Items["MatrixBinder"];
            return info.DoEval(field);
        }

        internal static void SetAsCurrent(MatrixBinder matrixBinder)
        {
            HttpContext.Current.Items["MatrixBinder"] = matrixBinder;
        }
    }

}
