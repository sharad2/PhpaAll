using System.Collections.Generic;
using System;

namespace EclipseLibrary.Web.JQuery
{
    /// <summary>
    /// This interface is returned by <see cref="MatrixRowEventArgs.MatrixRow"/> property and provides
    /// functions to access values in the current row during the <see cref="MatrixField.MatrixRowDataBound"/>.
    /// </summary>
    [Obsolete("Use MatrixField in namespace EclipseLibrary.Web.JQuery.Matrix")]
    public interface IMatrixRow
    {
        /// <summary>
        /// Returns the row total of each field in <see cref="MatrixField.DataValueFields"/>.
        /// Pass null to get total of all columns.
        /// </summary>
        /// <param name="columns">List of columns whose total is needed or null for all columns</param>
        /// <returns>Columns totals of the row which has been data bound</returns>
        /// <remarks>
        /// <para>
        /// This function can be called during the <see cref="MatrixField.MatrixRowDataBound"/> event.
        /// </para>
        /// </remarks>
        /// <example>
        /// <para>
        /// This example accesses the total advances and total recoveries. We assume that all advances columns have
        /// <see cref="MatrixColumn.HeaderText"/> beginning with <c>A</c>. Similarly, all recovery columns
        /// have header text begining with <c>R</c>. It then displays advances minus recoveries in a different column
        /// of the grid.
        /// </para>
        /// <code lang="XML">
        /// <![CDATA[
        ///<jquery:GridViewEx ID="gv" runat="server" AutoGenerateColumns="false" DataSourceID="ds">
        ///    <Columns>
        ///        <asp:BoundField HeaderText="Package Name" DataField="PackageName" />
        ///        <jquery:MatrixField DataHeaderField="ActivityDescription" DataValueFields="PackageActivtiyData"
        ///            DataCustomFields="IsAdvance"
        ///            DataMergeFields="PackageId" DataValueFormatString="{0:N2}" DisplayRowTotals="true"
        ///            RowTotalHeaderText="Total Advances|Total Recoveries" OnMatrixRowDataBound="gv_MatrixRowDataBound">
        ///        </jquery:MatrixField>
        ///        <eclipse:MultiBoundField HeaderText="Advance Outstanding" AccessibleHeaderText="advOut"
        ///            DataFormatString="{0:N2}" ItemStyle-HorizontalAlign="Right" />
        ///    </Columns>
        ///</jquery:GridViewEx>
        /// ]]>
        /// </code>
        /// <code>
        /// <![CDATA[
        ///protected void gv_MatrixRowDataBound(object sender, MatrixRowEventArgs e)
        ///{
        ///    MatrixField mf = (MatrixField)sender;
        ///    var totalAdvances = e.GetRowTotal(mf.MatrixColumns.Where(p => (bool)p["IsAdvance"]));
        ///    var totalRecoveries = e.GetRowTotal(mf.MatrixColumns.Where(p => !(bool)p["IsAdvance"]));
        ///    // One way to figure out the index of the column in which we want to display a value
        ///    var index = gv.Columns.Cast<DataControlField>()
        ///        .Select((p, i) => new { Index = i, AccessibleHeaderText = p.AccessibleHeaderText })
        ///        .Where(p => p.AccessibleHeaderText == "advOut").Single().Index;
        ///    e.Row.Cells[index].Text = string.Format("{0:N2}", totalAdvances - totalRecoveries);
        ///}
        /// ]]>
        /// </code>
        /// </example>
        decimal?[] GetRowTotals(IEnumerable<MatrixColumn> columns);

        /// <summary>
        /// Sets a custom row total for a particular matrix row.
        /// </summary>
        /// <param name="groupIndex">The index of the column group for which row total text is being set, or 0 if there are no column groups.</param>
        /// <param name="text">The text to set</param>
        /// <remarks>
        /// <para>
        /// Custom row totals can be set during the <see cref="MatrixField.MatrixRowDataBound"/> event.
        /// </para>
        /// </remarks>
        /// <example>
        /// <para>
        /// For row totals, we display half the value of the total. This demonstrates that we have full power to decide
        /// what we want to show as row totals.
        /// </para>
        /// <code>
        /// <![CDATA[
        ///  protected void gvPaybill_MatrixRowDataBound(object sender, MatrixRowEventArgs e)
        ///  {
        ///     MatrixField mf = (MatrixField)sender;
        ///     decimal? earnings = e.MatrixRow.GetRowTotals(mf.MatrixColumns
        ///        .Where(p => !((bool)p.ColumnValues["Adjustment.IsDeduction"])))[0];
        ///     // Just for fun, we want the row total of all earnings to display half the earnings !
        ///     earnings = earnings / 2;
        ///     e.MatrixRow.SetRowTotalText(0, earnings.ToString("N0"));
        ///     ...
        ///  }
        /// ]]>
        /// </code>
        /// </example>
        void SetRowTotalText(int groupIndex, string text);

        void SetCellText(MatrixColumn col, string text);

        /// <summary>
        /// Returns the value of the first field specified in <see cref="MatrixField.DataValueFields"/> of the passed column.
        /// </summary>
        /// <param name="col">The column whose value is needed</param>
        /// <returns>The value of the first field</returns>
        /// <remarks>
        /// Never throws an exception. Returns null if no value for the field was retrieved from the data source.
        /// </remarks>
        object GetCellValue(MatrixColumn col);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="col">The column whose value needs to be set</param>
        /// <param name="fieldName">The value of which field. This field must be one of <see cref="MatrixField.DataValueFields"/></param>
        /// <param name="obj">The new value</param>
        /// <remarks>
        /// <para>
        /// During the <see cref="MatrixField.MatrixRowDataBound"/> event, you can change the value of any field.
        /// However this function is most useful for managing computed columns.
        /// </para>
        /// </remarks>
        /// <example>
        /// <para>
        /// </para>
        /// <code lang="XML">
        /// <![CDATA[
        ///<jquery:GridViewEx ID="gv" runat="server" AutoGenerateColumns="false" DataSourceID="ds">
        ///    <Columns>
        ///        ...
        ///        <jquery:MatrixField DataHeaderFields="GroupDescription,ActivityDescription,ActivityColumnNumber"
        ///            DataHeaderSortFields="GroupColumnNumber,ActivityColumnNumber" DataHeaderFormatString="{0}|{1}<br/>({2})"
        ///            DataHeaderCustomFields="CalculatedFormula" DataValueFields="PackageActivtiyData"
        ///            DataMergeFields="PackageId" DataValueFormatString="{0:N2}" OnMatrixRowDataBound="gv_MatrixRowDataBound"
        ///            DisplayColumnTotals="true">
        ///        </jquery:MatrixField>
        ///        <eclipse:MultiBoundField DataFields="Remarks" HeaderText="Last ra bill no." HideEmptyColumn="true" />
        ///    </Columns>
        ///</jquery:GridViewEx>
        /// ]]>
        /// </code>
        /// <code lang="C#">
        /// <![CDATA[
        /// protected void gv_MatrixRowDataBound(object sender, MatrixRowEventArgs e)
        /// {
        ///     MatrixField mf = (MatrixField)sender;
        ///     var formulaColumn = mf.MatrixColumns.Single(p => p.ColumnValues.ContainsKey("CalculatedFormula"));
        ///     decimal? computedResult;
        ///     // Write code to calculate the value to be shown in this column
        ///     e.MatrixRow.SetCellValue(formulaColumn, "PackageActivtiyData",  computedResult);
        /// }
        /// ]]>
        /// </code>
        /// </example>
        void SetCellValue(MatrixColumn col, string fieldName, object obj);
    }
}