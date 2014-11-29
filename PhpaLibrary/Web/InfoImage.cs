using System;
using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.WebControls;

[assembly: WebResource("Eclipse.PhpaLibrary.Web.info.jpg", "image/png")]
namespace Eclipse.PhpaLibrary.Web
{
    /// <summary>
    /// A trivial control which displays the info image
    /// </summary>
    public class InfoImage:Image
    {
        public InfoImage()
        {
            this.EnableViewState = false;
        }

        protected override void OnInit(EventArgs e)
        {
            this.ImageUrl = this.Page.ClientScript.GetWebResourceUrl(typeof(InfoImage),
                "Eclipse.PhpaLibrary.Web.info.jpg");
            this.AlternateText = "Info image";
            this.Height = new Unit(14);
            this.Width = new Unit(14);
            
            this.ImageAlign = ImageAlign.Middle;
            base.OnInit(e);
        }
        [Browsable(false)]
        [ReadOnly(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override string ImageUrl
        {
            get
            {
                return base.ImageUrl;
            }
            set
            {
                base.ImageUrl = value;
            }
        }
    }
}
