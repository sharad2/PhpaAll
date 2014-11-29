using System;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Eclipse.PhpaLibrary.Database;

namespace Eclipse.PhpaLibrary.Web
{
    /// <summary>
    /// This control can be placed anywhere within a form view, typeically within the footer template.
    /// It displays errors which occur due to editing or updating. If no error occurs, it displays
    /// a status message such as "User inserted" which provides positive feedback to the end user that the
    /// DML operation succeeded.
    /// </summary>
    /// <remarks>
    /// It stores its state information in HttpContext.Current.Items so that it can survice multiple
    /// instantiations.
    /// 
    /// It is a good idea to create this control in your empty template. This allows the item deleted message
    /// to get displayed.
    /// </remarks>
    public class FormViewStatusMessage:Control
    {
        /// <summary>
        /// We do not need view state because status messages are meant to be transient
        /// </summary>
        [Browsable(true)]
        [ReadOnly(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override bool EnableViewState
        {
            get
            {
                return false;
            }
            set
            {
                
            }
        }

        /// <summary>
        /// If this control is kept multiple times within the same form view, it will share the status message.
        /// </summary>
        private string ContextKey
        {
            get
            {
                return string.Format("{0}_{1}", this.NamingContainer.UniqueID, typeof(FormViewStatusMessage).ToString());
            }
        }

        public string Text {
            get
            {
                Pair pair = (Pair)HttpContext.Current.Items[this.ContextKey];
                if (pair == null)
                {
                    return string.Empty;
                }
                else
                {
                    return (string)pair.First;
                }
            }
            set
            {
                Pair pair = (Pair)HttpContext.Current.Items[this.ContextKey];
                if (pair == null)
                {
                    pair = new Pair(string.Empty, false);
                    HttpContext.Current.Items[this.ContextKey] = pair;
                }
                pair.First = value;
            }
        }

        public bool IsError
        {
            get
            {
                Pair pair = (Pair)HttpContext.Current.Items[this.ContextKey];
                if (pair == null)
                {
                    return false;
                }
                else
                {
                    return (bool)pair.Second;
                }
            }
            set
            {
                Pair pair = (Pair)HttpContext.Current.Items[this.ContextKey];
                if (pair == null)
                {
                    pair = new Pair(string.Empty, false);
                    HttpContext.Current.Items[this.ContextKey] = pair;
                }
                pair.Second = value;
            }
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            FormView fv = (FormView)this.NamingContainer;
            fv.ItemInserted += new FormViewInsertedEventHandler(fv_ItemInserted);
            fv.ItemUpdated += new FormViewUpdatedEventHandler(fv_ItemUpdated);
            fv.ItemDeleted += new FormViewDeletedEventHandler(fv_ItemDeleted);
        }

        void fv_ItemDeleted(object sender, FormViewDeletedEventArgs e)
        {
            this.IsError = e.Exception != null;
            if (e.Exception == null)
            {
                this.Text = "Item deleted successfully";
            }
            else
            {
                this.Text = DataContextBase.GetExceptionMessage(e.Exception);
                e.ExceptionHandled = true;
            }
        }

        void fv_ItemUpdated(object sender, FormViewUpdatedEventArgs e)
        {
            this.IsError = e.Exception != null;
            if (e.Exception == null)
            {
                this.Text = "Item updated successfully";
            }
            else
            {
                this.Text = DataContextBase.GetExceptionMessage(e.Exception);
                e.ExceptionHandled = true;
                e.KeepInEditMode = true;
            }
        }

        void fv_ItemInserted(object sender, FormViewInsertedEventArgs e)
        {
            this.IsError = e.Exception != null;
            if (e.Exception == null)
            {
                this.Text = "Item inserted successfully";
            }
            else
            {
                this.Text = DataContextBase.GetExceptionMessage(e.Exception);
                e.ExceptionHandled = true;
                e.KeepInInsertMode = true;
            }
        }

        protected override void Render(HtmlTextWriter writer)
        {
            if (string.IsNullOrEmpty(this.Text))
            {
                return;
            }
            if (IsError)
            {
                writer.AddStyleAttribute(HtmlTextWriterStyle.Color, "red");
                writer.RenderBeginTag(HtmlTextWriterTag.Span);
            }
            writer.Write(this.Text);

            if (IsError)
            {
                writer.RenderEndTag();
            }
            writer.RenderBeginTag(HtmlTextWriterTag.Br);
            writer.RenderEndTag();          // br
        }
    }
}
