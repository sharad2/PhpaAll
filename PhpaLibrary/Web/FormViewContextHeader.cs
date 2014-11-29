using System;
using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Eclipse.PhpaLibrary.Web
{
    /// <summary>
    /// This can be placed anywhere within a form view. Typically you will place it in the header template.
    /// It asks you for an EntityName. As an example, if the entity name is User, it will display
    /// one of:
    ///   Edit User
    ///   Insert New User
    ///   View User Details
    ///   
    /// depending on the mode of the containing form view.
    /// </summary>
    /// 
    [ToolboxData(@"
<{0}:FormViewContextHeader runat=""server"" EntityName=""Voucher"" CurrentEntity='<%# Eval(""VoucherCode"") %>' />
")]
    public class FormViewContextHeader:Label
    {
        public FormViewContextHeader()
        {
            this.EntityName = string.Empty;
        }

        [Browsable(true)]
        public string EntityName
        {
            get;
            set;
        }

        [Browsable(true)]
        [Description("Any string which identifies the current entity, such as user name")]
        public string CurrentEntity 
        {
            get
            {
                object obj = ViewState["CurrentEntity"];
                if (obj == null)
                {
                    return string.Empty;
                }
                return (string)obj;
            }
            set
            {
                ViewState["CurrentEntity"] = value;
            }
        }

        protected override void OnPreRender(System.EventArgs e)
        {
            FormView formview = (FormView)this.NamingContainer;
            switch (formview.CurrentMode)
            {
                case FormViewMode.ReadOnly:
                case FormViewMode.Edit:
                    if (string.IsNullOrEmpty(this.EntityName))
                    {
                        this.Text = this.CurrentEntity;
                    }
                    else
                    {
                        this.Text = string.Format("{0}: {1}", this.EntityName, this.CurrentEntity);
                    }
                    break;

                case FormViewMode.Insert:
                    this.Text = string.Format("Create New {0}", this.EntityName);
                    break;

                default:
                    throw new NotSupportedException("Form view mode must be in edit, insert or read only");

            }
            base.OnPreRender(e);
        }

    }
}
