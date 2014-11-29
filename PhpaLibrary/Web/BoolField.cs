using System;
using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Specialized;
using System.Drawing;

namespace Eclipse.PhpaLibrary.Web
{
    /// <summary>
    /// This class intends to provide user defined values to the boolean fields.
    /// User will specify the "TrueValue" property during design time and the value will be shown for true values.
    /// Simailarly on specifying "FalseValue" during design time the said value will be shown for false value.
    /// </summary>
    /// <remarks>
    /// ItemStyle is applied to all true items. FalseItemStyle is additionally applied to false items.
    /// 
    /// SS 18 Jul 200: Fixed bug which caused us to forget our value after a row was updated
    /// </remarks>
    public class BoolField:DataControlField
    {
        public BoolField()
        {
            this.TrueValue = "True Value";
            this.FalseValue = "False Value";
            //this.ReadOnly = true;
        }

        protected override DataControlField CreateField()
        {
            return new BoolField();
        }

        public override void InitializeCell(DataControlFieldCell cell, DataControlCellType cellType, DataControlRowState rowState, int rowIndex)
        {
            base.InitializeCell(cell, cellType, rowState, rowIndex);
            switch (cellType)
            {
                case DataControlCellType.DataCell:
                    if ((rowState & DataControlRowState.Edit) == DataControlRowState.Edit)
                    {
                        TextBox tb = new TextBox();
                        tb.ReadOnly = true;
                        tb.BorderStyle = BorderStyle.None;
                        tb.BackColor = Color.Transparent;
                        tb.DataBinding += new EventHandler(tb_DataBinding);
                        cell.Controls.Add(tb);
                    }
                    else
                    {
                        cell.DataBinding += new EventHandler(cell_DataBinding);
                    }
                    break;
            }
        }

        void tb_DataBinding(object sender, EventArgs e)
        {
            TextBox tb = (TextBox)sender;
            GridViewRow row = (GridViewRow)tb.NamingContainer;
            bool b = (bool)DataBinder.Eval(row.DataItem, this.DataField);
            if (b)
            {
                tb.Text = this.TrueValue;
                tb.ToolTip = this.TrueToolTip;
            }
            else
            {
                tb.Text = this.FalseValue;
                tb.ToolTip = this.FalseToolTip;
            }            
        }

        void cell_DataBinding(object sender, EventArgs e)
        {
            DataControlFieldCell cell = (DataControlFieldCell)sender;
            GridViewRow row = (GridViewRow)cell.NamingContainer;
            bool b;
            if (_extractedValue == null || ((int)_extractedValue.First) != row.RowIndex)
            {
                b = (bool)DataBinder.Eval(row.DataItem, this.DataField);
            }
            else
            {
                // After a row is updated we get here. At this time we may not have the data item available.
                // So we use the value which we remembered during the extraction process.
                b = (bool)_extractedValue.Second;
            }
            if (b)
            {
                cell.Text = this.TrueValue;
                cell.ToolTip = this.TrueToolTip;
            }
            else
            {
                cell.Text = this.FalseValue;
                cell.ApplyStyle(FalseItemStyle);
                cell.ToolTip = this.FalseToolTip;
            }
        }

        /// <summary>
        /// First is row index, Second is the bool value
        /// </summary>
        private Pair _extractedValue;
        public override void ExtractValuesFromCell(IOrderedDictionary dictionary, DataControlFieldCell cell, DataControlRowState rowState, bool includeReadOnly)
        {
            if (cell.HasControls())
            {
                // We will not have controls during deleting
                GridViewRow row = (GridViewRow)cell.NamingContainer;
                TextBox tb = (TextBox)cell.Controls[0];
                _extractedValue = new Pair();
                _extractedValue.First = row.RowIndex;
                _extractedValue.Second = (tb.Text == this.TrueValue);
            }
            base.ExtractValuesFromCell(dictionary, cell, rowState, includeReadOnly);
        }

        /// <summary>
        /// Property set by the user for "True" as value.
        /// </summary>
        [Browsable(true)]
        [Category("Phpa")]
        [Description("The value to show when the expression evaluates to true")]
        [Themeable(true)]
        [DefaultValue("True Value")]
        public string TrueValue { get; set; }


        /// <summary>
        /// Property set by the user for "False" as value.
        /// </summary>
        [Browsable(true)]
        [Category("Phpa")]
        [Description("The value to show when the expression evaluates to false")]
        [Themeable(true)]
        [DefaultValue("False Value")]
        public string FalseValue { get; set; }

        [Browsable(true)]
        [Category("Phpa")]
        [Description("The additional style to apply to false items")]
        [Themeable(true)]
        [DefaultValue(null)]
        [PersistenceModeAttribute(PersistenceMode.InnerProperty)]
        public TableItemStyle FalseItemStyle
        {
            get;
            set;
        }

        [Browsable(true)]
        [Category("Phpa")]
        [Description("Tooltip to display for true values")]
        [Themeable(true)]
        [DefaultValue("")]
        public string TrueToolTip { get; set; }

        [Browsable(true)]
        [Category("Phpa")]
        [Description("Tooltip to display for false values")]
        [Themeable(true)]
        [DefaultValue("")]
        public string FalseToolTip { get; set; }

        [Browsable(true)]
        [Category("Phpa")]
        [Description("Data field to evaluate")]
        [Themeable(true)]
        [DefaultValue("")]
        public string DataField { get; set; }

        protected override void OnFieldChanged()
        {
            //base.OnFieldChanged();
        }
    }
}
