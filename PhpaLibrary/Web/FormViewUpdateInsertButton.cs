using System;
using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Eclipse.PhpaLibrary.Web
{
    /// <summary>
    /// If you are sharing the edit template and the insert template, then you can place this
    /// button in your edit template. It becomes Update if form view is in edit mode. If form view
    /// is in insert mode, it becomes Insert. Exception is thrown if this button is not within a form view
    /// or the form view mode is not update or insert.
    /// </summary>
    /// 
    [ToolboxData("<{0}:FormViewUpdateInsertButton runat=\"server\" />")]
    [PersistChildren(false)]
    [ParseChildren(true)]
    public class FormViewUpdateInsertButton:Button
    {
        public FormViewUpdateInsertButton()
        {
            this.CausesValidation = true;
            this.UpdateText = "Update";
            this.InsertText = "Insert";
        }

        [Browsable(true)]
        [DefaultValue("Update")]
        [Description("Text to display when updating")]
        public string UpdateText { get; set; }

        [Browsable(true)]
        [DefaultValue("Insert")]
        [Description("Text to display when inserting")]
        public string InsertText { get; set; }
       

        protected override void OnPreRender(System.EventArgs e)
        {
            // An exception generates here if we are not within form view
            FormView formview = (FormView)this.NamingContainer;
            switch (formview.CurrentMode)
            {
                case FormViewMode.Edit:
                    this.CommandName = "Update";
                    this.Text = this.UpdateText;
                    this.AccessKey = "U";
                    this.ToolTip = "Click or press Alt+U to Update";
                    break;

                case FormViewMode.Insert:
                    this.CommandName = "Insert";
                    this.Text = this.InsertText;
                    this.AccessKey = "I";
                    this.ToolTip = "Click or press Alt+I to Insert";
                    break;

                default:
                    throw new NotSupportedException("Form view mode must be edit or insert");

            }
            if (string.IsNullOrEmpty(this.Page.Form.DefaultButton))
            {
                // Do not overwrite if it has already been set
                this.Page.Form.DefaultButton = this.UniqueID;
            }
            base.OnPreRender(e);
        }
    }
}
