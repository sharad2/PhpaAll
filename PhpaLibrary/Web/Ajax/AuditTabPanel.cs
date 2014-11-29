using System;
using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.WebControls;
using Eclipse.PhpaLibrary.Database;
using EclipseLibrary.Web.JQuery;

namespace Eclipse.PhpaLibrary.Web.Ajax
{
    /// <summary>
    /// Displays audit information within an AJAX audit tab
    /// </summary>
    public class AuditTabPanel:JPanel
    {
        [Serializable]
        private class AuditableCopy : IAuditable
        {
            public AuditableCopy(IAuditable auditable)
            {
                _created = auditable.Created;
                _createdBy = auditable.CreatedBy;
                _createdWorkstation = auditable.CreatedWorkstation;
                _modified = auditable.Modified;
                _modifiedBy = auditable.ModifiedBy;
                _modifiedWorkstation = auditable.ModifiedWorkstation;

            }
            #region IAuditable Members

            private DateTime? _created;
            public DateTime? Created
            {
                get
                {
                    return _created;
                }
                set
                {
                    throw new System.NotImplementedException();
                }
            }

            private string _createdBy;
            public string CreatedBy
            {
                get
                {
                    return _createdBy;
                }
                set
                {
                    throw new System.NotImplementedException();
                }
            }

            private string _createdWorkstation;
            public string CreatedWorkstation
            {
                get
                {
                    return _createdWorkstation;
                }
                set
                {
                    throw new System.NotImplementedException();
                }
            }

            private DateTime? _modified;
            public DateTime? Modified
            {
                get
                {
                    return _modified;
                }
                set
                {
                    throw new System.NotImplementedException();
                }
            }

            private string _modifiedBy;
            public string ModifiedBy
            {
                get
                {
                    return _modifiedBy;
                }
                set
                {
                    throw new System.NotImplementedException();
                }
            }

            private string _modifiedWorkstation;
            public string ModifiedWorkstation
            {
                get
                {
                    return _modifiedWorkstation;
                }
                set
                {
                    throw new System.NotImplementedException();
                }
            }

            #endregion
        }
        internal IAuditable Auditable
        {
            get
            {
                IAuditable auditable = (IAuditable) ViewState["Auditable"];
                return auditable;
            }
            set
            {
                ViewState["Auditable"] = new AuditableCopy(value);
            }
        }

        public AuditTabPanel()
        {
            this.HeaderText = "Audit";
            this.Height = new Unit(180, UnitType.Pixel);
        }

        protected override void OnInit(System.EventArgs e)
        {
            AuditTemplate template = new AuditTemplate(this);
            template.InstantiateIn(this);
            base.OnInit(e);
        }

        private string _leftCssClass = string.Empty;

        /// <summary>
        /// Css class to apply to the Left column
        /// </summary>
        /// 
        [Browsable(true)]
        [Category("Styles")]
        [Themeable(true)]
        [Description("Css class to apply to the left column")]
        [DefaultValue("")]
        public string LeftCssClass
        {
            get { return _leftCssClass; }
            set { _leftCssClass = value; }
        }
        private string _rightCssClass = string.Empty;

        /// <summary>
        /// Css class to apply to the right column
        /// </summary>
        /// 
        [Browsable(true)]
        [Category("Styles")]
        [Themeable(true)]
        [Description("Css class to apply to the right column")]
        [DefaultValue("")]
        public string RightCssClass
        {
            get { return _rightCssClass; }
            set { _rightCssClass = value; }
        }

    }
}
