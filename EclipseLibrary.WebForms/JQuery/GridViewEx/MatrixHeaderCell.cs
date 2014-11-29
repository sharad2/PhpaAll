using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace EclipseLibrary.Web.JQuery
{
        [Obsolete("Use MatrixField in namespace EclipseLibrary.Web.JQuery.Matrix")]
    internal class MatrixHeaderCell : DataControlFieldHeaderCell
    {
        public MatrixHeaderCell(MatrixField field)
            : base(field)
        {
        }

        /// <summary>
        /// Prevents this column from spanning with previous or next columns even if the first row
        /// header text matches
        /// </summary>
        /// <returns></returns>
        public override bool HasControls()
        {
            return true;
        }

        /// <summary>
        /// The call to this function is expected only from the GridViewExMasterRow class.
        /// It returns the number of columns we are actually showing. The value of this property cannot
        /// be set.
        /// </summary>
        /// <remarks>
        /// GL: Dated - 22 July, 2010.
        /// </remarks>
        public override int ColumnSpan
        {
            get
            {
                MatrixField mf = (MatrixField)this.ContainingField;
                int count = mf.MatrixColumns.Where(p => p.Visible).Count();
                if (mf.DisplayRowTotals)
                {
                    // One column for row total of each column group
                    count += mf.ColumnGroups.Count;
                }
                return count;
            }
            set
            {
                // Ignore the span we are being asked to set
            }
        }

        protected override void Render(HtmlTextWriter writer)
        {
            MatrixField mf = (MatrixField)this.ContainingField;
            GridViewRow gridRow = (GridViewRow)this.NamingContainer;
            if (!mf.ColumnGroups.Any())
            {
                // We have no columns to show. Do nothing
                return;
            }

            switch (gridRow.RowIndex)
            {
                case GridViewExHeaderRow.ROW_FIRST_LINE_HEADER:
                    // This is the first row of multi row header. Write the header text
                    // If the header text of the matrix column is pipe seperated, then the text before the pipe
                    // is the first row text. Otherwise the MatrixField HeaderText is the first row text
                    //string prevText = null;
                    //int colspan = 0;
                    foreach (var item in mf.ColumnGroups)
                    {
                        RenderFirstRow(writer, mf, item);
                        if (mf.DisplayRowTotals)
                        {
                            RenderRowTotalHeader(writer, mf, item);
                        }
                    }
                    break;

                case GridViewExHeaderRow.ROW_SECOND_LINE_HEADER:
                    // This is the second header row
                    foreach (var item in mf.ColumnGroups)
                    {
                        RenderSecondRow(writer, mf, item);
                    }
                    break;

                case GridViewExHeaderRow.ROW_SINGLE_LINE_HEADER:
                    foreach (var item in mf.ColumnGroups)
                    {
                        RenderSecondRow(writer, mf, item);
                        if (mf.DisplayRowTotals)
                        {
                            RenderRowTotalHeader(writer, mf, item);
                        }
                    }
                    break;

                default:
                    throw new NotImplementedException("Will this happen ?");
            }
        }

        /// <summary>
        /// If header text is empty, renders the matrix field header text
        /// </summary>
        private void RenderFirstRow(HtmlTextWriter writer, MatrixField mf, IGrouping<int, MatrixColumn> colGroup)
        {
            int colSpan = colGroup.Count();
            string headerText = colGroup.First().FirstRowText;
            //int groupIndex = colGroup.Key;

            mf.HeaderStyle.AddAttributesToRender(writer);

            if (colSpan > 1)
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Colspan, colSpan.ToString());
            }

            // If there is no second row taxt, row span this column
            if (colGroup.All(p => string.IsNullOrEmpty(p.SecondRowText)))
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Rowspan, "2");
            }

            writer.RenderBeginTag(HtmlTextWriterTag.Th);
            if (string.IsNullOrEmpty(headerText))
            {
                string[] tokens = this.ContainingField.HeaderText.Split('|');
                // Sharad 21 Jul 2010: Cannot use format string with matrix field header text
                //headerText = string.Format(mf.DataHeaderFormatString, tokens[0]);
                headerText = tokens[0];
            }
            if (string.IsNullOrEmpty(headerText))
            {
                writer.Write("&nbsp;");
            }
            else
            {
                writer.Write(headerText);
            }
            writer.RenderEndTag();
        }

        private static void RenderRowTotalHeader(HtmlTextWriter writer, MatrixField mf, IGrouping<int, MatrixColumn> colGroup)
        {
            writer.AddAttribute(HtmlTextWriterAttribute.Rowspan, "2");
            writer.RenderBeginTag(HtmlTextWriterTag.Th);
            string[] tokens = mf.RowTotalHeaderText.Split('|');
            switch (tokens.Length)
            {
                case 0:
                    // Empty Text
                    writer.Write("Total {0}", mf.HeaderText);
                    break;

                case 1:
                    // No pipe. Same for all
                    writer.Write(tokens[0]);
                    break;

                default:
                    // Pipe encountered.
                    if (colGroup.Key < tokens.Length)
                    {
                        writer.Write(tokens[colGroup.Key]);
                    }
                    else
                    {
                        // Use the first one
                        writer.Write(tokens[0]);
                    }
                    break;
            }
            writer.RenderEndTag();
        }

        private void RenderSecondRow(HtmlTextWriter writer, MatrixField mf, IGrouping<int, MatrixColumn> colGroup)
        {
            if (colGroup.All(p => string.IsNullOrEmpty(p.SecondRowText)))
            {
                // Render nothing. We have already row spanned the first header row.
                return;
            }
            foreach (MatrixColumn column in colGroup)
            {
                mf.HeaderStyle.AddAttributesToRender(writer);

                //if (string.IsNullOrEmpty(column.HeaderText))
                //{
                //    throw new NotSupportedException("Let Sharad know if this happens");
                //    //writer.RenderBeginTag(HtmlTextWriterTag.Th);
                //    //var query = (from field in mf.DataHeaderFields select column.ColumnValues[field]).ToArray();
                //    //writer.Write(mf.DataHeaderFormatString, query);
                //    //writer.RenderEndTag();
                //}
                //else
                //{
                    writer.RenderBeginTag(HtmlTextWriterTag.Th);
                    writer.Write(column.SecondRowText);
                    writer.RenderEndTag();
                //}
            }
        }
    }
}
