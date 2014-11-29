using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;
using EclipseLibrary.Formatting;
using EclipseLibrary.Web.UI;

namespace EclipseLibrary.Web.JQuery.Input
{
    /// <summary>
    /// Similar to ASP DropDownList. Adds support for option groups and JQuery validation
    /// </summary>
    /// <remarks>
    /// <para>
    /// You can add items statically using the <see cref="Items"/> property. You can also add items
    /// via data binding. Specify <see cref="DataSourceID"/> as well as <see cref="DataTextField"/>
    /// and <see cref="DataValueField"/>. Data binding will happen automatically. Alternatively you can
    /// specify <see cref="DataSource"/> and call <see cref="DataBind"/> manually.
    /// </para>
    /// <para>
    /// You specify the initial value to select using the <see cref="Value"/> property. This can be
    /// done at any time, even before items have been added to the list. If the 
    /// <c>SelectedValue</c> does not exist in list of items, the first item will be selected.
    /// </para>
    /// <para>
    /// If you would like to derive a class which is responsible for filling its own items, then the recommended pattern
    /// is to override PerformDataBinding() and add the items you need. Do NOT call the base PerformDataBinding().
    /// In your OnPreRender() override, call EnsureDataBound().
    /// </para>
    /// <para>
    /// Normally, you will let the framework update the cookie value by setting <see cref="InputControlBase.UseCookie"/> to <c>WriteOnChange</c>.
    /// To manually set the value of the cookie, call <c>$('#ddl').dropdownListEx('setCookie')</c>.
    /// </para>
    /// <para>
    /// Sharad 5 Jan 2010: If there is only a single entry in the list, it displays as a label.
    /// </para>
    /// <para>
    /// Sharad 17 Aug 2010: Not generating hidden field if web method not specified
    /// </para>
    /// <para>
    /// Sharad 13 Sep 2010: When <see cref="CascadableHelper.WebMethod"/> specified, both text and value are stored in the cookie.
    /// </para>
    /// <para>
    /// Sharad 22 Sep 2010: Added property CssClass to <see cref="DropDownItem"/>.
    /// </para>
    /// <para>
    /// Sharad 14 Oct 2010: <see cref="DataCustomFields"/> gives you rich control over sorting items. This is also one way
    /// you can set the <see cref="Value"/> based on some field in the database. <c>DataOptionSortField</c> is now obsolete.
    /// </para>
    /// </remarks>
    /// <example>
    /// <code>
    /// <![CDATA[
    ///<o:OracleDataSource runat="server" ID="dsLabel" ConnectionString="<%$ ConnectionStrings:dcms8 %>"
    ///    ProviderName="<%$ ConnectionStrings:dcms8.ProviderName %>"
    ///    SelectCommand="Select tabLabel.label_id, tabLabel.description from tab_style_label tabLabel order by tabLabel.description" />
    ///<i:DropDownListEx ID="ddlLabel" ToolTip="Label" runat="server" DataTextField="description"
    ///    DataValueField="label_id" DataSourceID="dsLabel" AppendDataBoundItems="True">
    ///    <Items>
    ///        <eclipse:DropDownItem Value="" Text="(All Labels)" />
    ///    </Items>
    ///</i:DropDownListEx>
    /// ]]>
    /// </code>
    /// </example>
    [Obsolete("Use DropDownListEx2 or AjaxDropDown")]
    public class DropDownListEx : InputControlBase
    {
        private readonly Collection<DropDownItem> _items;

        /// <summary>
        /// 
        /// </summary>
        public DropDownListEx()
            : base("dropDownListEx")
        {
            _items = new Collection<DropDownItem>();
            this.DataTextFormatString = "{0}";
            this.Value = string.Empty;
            this.DataOptionGroupFormatString = "{0}";
        }

        /// <summary>
        /// The <see cref="DropDownItem"/> which corresponds to the <see cref="Value"/>.
        /// </summary>
        [Browsable(false)]
        public virtual DropDownItem SelectedItem
        {
            get
            {
                return _items.FirstOrDefault(p => p.Value == Value);
            }
        }
        /// <summary>
        /// Formats the value of the field specified in the <see cref="DataOptionGroupField"/>
        /// </summary>
        /// <remarks>
        /// <para>
        /// You can specify format string for the field specified in the <see cref="DataOptionGroupField"/>,
        /// and eventually it get shown under HTML OPTGROUP tag.
        /// </para>
        /// </remarks>
        /// <example>
        ///     <para>
        ///     Following example checks the value <c>"Y"</c> in <c>Is_Pallet_Required</c> field, (which 
        ///     is specified in <see cref="DataOptionGroupField"/>) and shows <c>"Pallet Areas"</c> or else
        ///     for the values other than <c>"Y"</c>, <c>"Non Pallet Areas"</c> will be shown.
        ///     </para>
        ///     <code>
        ///         <![CDATA[
        ///         <i:DropDownListEx ID="ddlDestinationArea" runat="server" DataSourceID="dsDestinationArea"
        ///             FriendlyName="Destination Area" ClientIDMode="Static" DataTextField="description"
        ///             DataValueField="Inventory_Storage_Area" DataTextFormatString="{1}: {0}" DataOptionGroupField="Is_Pallet_Required"
        ///             DataOptionGroupFormatString="{0::$Is_Pallet_Required = 'Y':Pallet Areas:Non Pallet Areas}">
        ///         </i:DropDownListEx>
        ///         ]]>
        ///     </code>
        /// </example>
        [Browsable(true)]
        [DefaultValue("{0}")]
        public string DataOptionGroupFormatString { get; set; }
        #region Cascadable

        private CascadableHelper _cascadable;

        /// <summary>
        /// If this control should update its value whenever the value of some other control changes, then you would
        /// need to specify some Cascadable properties.
        /// </summary>
        /// <remarks>
        /// It is virtual so that derived classes can make it Browsable(false) if they do not support cascading
        /// </remarks>
        [Browsable(true)]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        public CascadableHelper Cascadable
        {
            get
            {
                if (_cascadable == null)
                {
                    _cascadable = new CascadableHelper();
                }
                return _cascadable;
            }
        }
        #endregion
        #region DataBinding

        /// <summary>
        /// The field from which item text will be read.
        /// </summary>
        /// <remarks>
        /// <para>
        /// It is permissible to leave this blank. If this has not been specified, then the
        /// <c>ToString()</c> of the data item is used as <see cref="DropDownItem.Text"/>.
        /// This is useful if you are binding to
        /// an array of strings.
        /// </para>
        /// </remarks>
        [Browsable(true)]
        public string DataTextField { get; set; }

        /// <summary>
        /// {0} refers to DataTextField, {1} refers to DataValueField, {2} refers to DataOptionGroupField
        /// </summary>
        /// <remarks>
        /// <para>
        /// This is useful when you want to include the value as part of the text which displays.
        /// </para>
        /// </remarks>
        [Browsable(true)]
        [DefaultValue("{0}")]
        public string DataTextFormatString { get; set; }

        /// <summary>
        /// The field from which item value will be read.
        /// </summary>
        /// <remarks>
        /// <para>
        /// It is permissible to leave this blank. If this has not been specified, then the
        /// <c>ToString()</c> of the data item is used as <see cref="DropDownItem.Value"/>. This is useful if you are binding to
        /// an array of strings.
        /// </para>
        /// </remarks>
        [Browsable(true)]
        public string DataValueField { get; set; }

        /// <summary>
        /// The field from which <see cref="DropDownItem.OptionGroup"/> property will be populated.
        /// </summary>
        /// <remarks>
        /// This property provides support for the HTML OPTGROUP tag. If the list ends up having exactly one non empty option group,
        /// the option group is not displayed.
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        ///<oracle:OracleDataSource ID="dsDestArea" runat="server" ConnectionString="<%$ ConnectionStrings:DCMSLIVE %>"
        ///    ProviderName="<%$ ConnectionStrings:DCMSLIVE.ProviderName %>" SelectCommand="
        ///        select inventory_storage_area as inventory_storage_area,
        ///               description as description,
        ///               t.stores_what AS stores_what,
        ///               CASE
        ///                 WHEN t.stores_what = 'CTN' THEN
        ///                  'Carton Areas'
        ///                 ELSE
        ///                  'SKU Area'
        ///               END as area_type,
        ///               1 AS option_sort_sequence
        ///          from tab_inventory_area t
        ///         WHERE unusable_inventory IS NULL
        ///        union
        ///        select ia_id, short_description, 'Shipping Areas', 2
        ///          from ia
        ///         where ia.shipping_area_flag = 'Y'
        ///         order by 1
        ///" />
        ///<i:DropDownListEx ID="ctlDestCtnArea" runat="server" DataSourceID="dsDestArea" AppendDataBoundItems="true"
        ///    ToolTip="Destination Area" DataTextField="description" DataValueField="inventory_storage_area"
        ///    DataTextFormatString="{1}: {0}" DataOptionSortField="option_sort_sequence"
        ///    DataOptionGroupField="stores_what">
        ///    <Items>
        ///        <eclipse:DropDownItem Text="(All)" Value="" OptionGroupSortSequence="0" />
        ///    </Items>
        ///</i:DropDownListEx>
        /// ]]>
        /// </code>
        /// </example>
        [Browsable(true)]
        public string DataOptionGroupField { get; set; }

        /// <summary>
        /// The ID of the data source to use for data binding.
        /// </summary>
        /// <remarks>
        /// <para>
        /// You do not need to manually call <see cref="DataBind"/>. It will be called automatically.
        /// </para>
        /// <para>
        /// The data source is first searched for within the same naming container as this drop down list. If it is not
        /// found there, then each parent naming container is searched until the source is found.
        /// </para>
        /// </remarks>
        [Browsable(true)]
        [IDReferenceProperty(typeof(DataSourceControl))]
        public string DataSourceID { get; set; }

        /// <summary>
        /// Use this if you want to call DataBind yourself.
        /// </summary>
        [Browsable(false)]
        public object DataSource { get; set; }

        /// <summary>
        /// These fields are not used by this control. They are provided as a convenience to populate 
        /// <see cref="DropDownItem.CustomData"/>
        /// </summary>
        /// <remarks>
        /// <para>
        /// This is useful when you want to custom sort the items or if you want to set the <see cref="Value"/> based on some
        /// field in the database. The values of each field in <see cref="DataCustomFields"/> can be accessed using the
        /// <see cref="DropDownItem.CustomData"/> property. You will typically do this in the <see cref="DataBound"/> event.
        /// </para>
        /// </remarks>
        /// <example>
        /// <para>
        /// We want to set the default selection to the inventory area where <c>default_receiving_quality = 'Y'</c>. If multiple
        /// areas have this flag set, we want to use the one with the lowest <c>quality_rank</c>
        /// </para>
        /// <code lang="XML">
        /// <![CDATA[
        ///<i:DropDownListEx runat="server" ID="ddlQualityCode" 
        ///    DataTextField="description" DataValueField="Quality_Code"
        ///    DataSourceID="dsQualityCode" DataCustomFields="default_receiving_quality,quality_rank"
        ///    OnDataBound="ddlQualityCode_DataBound" />
        /// ]]>
        /// </code>
        /// <para>
        /// In the <see cref="DataBound"/> event handler, find those items whose <c>default_receiving_quality = 'Y'</c>
        /// and then select the one with the minimum rank.
        /// </para>
        /// <code lang="c#">
        /// <![CDATA[
        /// protected void ddlQualityCode_DataBound(object sender, EventArgs e)
        /// {
        ///    DropDownItem item = ddlQualityCode.Items.Where(p => p.CustomData["default_receiving_quality"].ToString() == "Y")
        ///        .Aggregate((p, next) => Convert.ToInt32(p.CustomData["quality_rank"]) < Convert.ToInt32(next.CustomData["quality_rank"]) ? p : next);
        ///    ddlQualityCode.SelectedValue = item.Value;
        /// }
        /// ]]>
        /// </code>
        /// </example>
        [Browsable(true)]
        [Category("Data")]
        [Description("Comma seperated list of data fields")]
        [TypeConverterAttribute(typeof(StringArrayConverter))]
        [Themeable(false)]
        public string[] DataCustomFields { get; set; }

        private bool _requiresDataBinding = true;

        /// <summary>
        /// Set to true just before DataBind() is called from within PreRender. Then set to false.
        /// </summary>
        private bool _inPreRender;

        /// <summary>
        /// Calls <see cref="DataBind"/> if it has not been called already.
        /// </summary>
        /// <param name="inPreRender">true if you are calling this function from within OnPrender</param>
        protected void EnsureDataBound(bool inPreRender)
        {
            if (_requiresDataBinding)
            {
                if (inPreRender)
                {
                    _inPreRender = true;
                }
                DataBind();
                if (inPreRender)
                {
                    _inPreRender = false;
                }

            }
        }

        public event EventHandler DataBound;

        /// <summary>
        /// When resolving the data source, the data source identified by the DataSourceID property takes precedence.
        /// If DataSourceID is not set, the object identified by the DataSource property is used.
        /// </summary>
        public override void DataBind()
        {
            var query = _items.Where(p => p.Persistent != DropDownPersistenceType.Never).ToList();
            _items.Clear();
            query.ForEach(delegate(DropDownItem item) { _items.Add(item); });

            if (!_inPreRender)
            {
                OnDataBinding(EventArgs.Empty);
            }

            int preCount = _items.Count;
            PerformDataBinding();

            // Sharad 2 Aug 2010: Added code to handle the WhenEmpty case
            if (_items.Count > preCount)
            {
                // We added some items. Remove the WhenEmpty items
                var toRemove = _items.Where(p => p.Persistent == DropDownPersistenceType.WhenEmpty).ToArray();
                foreach (var item in toRemove)
                {
                    _items.Remove(item);
                }
            }

            if (this.Items.Count > 0 && this.SelectedItem == null)
            {
                // Select the first item
                this.Value = this.Items[0].Value;
            }
            _requiresDataBinding = false;

            OnDataBound(EventArgs.Empty);
        }

        protected virtual void OnDataBound(EventArgs e)
        {
            if (this.DataBound != null)
            {
                this.DataBound(this, e);
            }
        }

        private IEnumerable PerformSelect()
        {
            IEnumerable data;
            if (!string.IsNullOrEmpty(this.DataSourceID))
            {
                IDataSource ds = (IDataSource)this.NamingContainer.FindControl(this.DataSourceID);
                if (ds == null)
                {
                    // For now, just look at one naming container above.
                    // TODO: Look at all parent naming containers
                    ds = (IDataSource)this.NamingContainer.NamingContainer.FindControl(this.DataSourceID);
                }
                data = null;
                ds.GetView(string.Empty).Select(DataSourceSelectArguments.Empty, delegate(IEnumerable data1)
                {
                    data = data1;
                });
            }
            else if (this.DataSource == null)
            {
                data = null;
            }
            else if (this.DataSource is IEnumerable)
            {
                data = (IEnumerable)this.DataSource;
            }
            else if (this.DataSource is IDataSource)
            {
                IDataSource ds = (IDataSource)this.DataSource;
                data = null;
                ds.GetView(string.Empty).Select(DataSourceSelectArguments.Empty, delegate(IEnumerable data1)
                {
                    data = data1;
                });
            }
            else
            {
                throw new NotSupportedException();
            }
            return data;
        }

        /// <summary>
        /// Add <see cref="Items"/> from the data source. Derived classes should override this to add items
        /// from their custom data source.
        /// </summary>
        protected virtual void PerformDataBinding()
        {
            IEnumerable data = PerformSelect();
            if (data == null)
            {
                // Do nothing. This happens when no data source has been assigned to us
                return;
            }

            Dictionary<string, object> dict = new Dictionary<string, object>();
            ConditionalFormatter formatter = new ConditionalFormatter(p => dict[p]);
            foreach (object dataItem in data)
            {
                dict.Clear();
                DropDownItem item = new DropDownItem();
                object obj;
                if (string.IsNullOrEmpty(this.DataValueField))
                {
                    item.Value = dataItem.ToString();
                }
                else
                {
                    obj = DataBinder.Eval(dataItem, this.DataValueField);
                    item.Value = string.Format("{0}", obj);
                    dict[this.DataValueField] = obj;
                }
                if (string.IsNullOrEmpty(this.DataTextField))
                {
                    obj = dataItem.ToString();
                }
                else
                {
                    obj = DataBinder.Eval(dataItem, this.DataTextField);
                    dict[this.DataTextField] = obj;
                }
                item.Text = string.Format(this.DataTextFormatString, obj, item.Value);
                //if (!string.IsNullOrEmpty(this.DataOptionSortField))
                //{
                //    item.OptionGroupSortSequence = Convert.ToInt32(
                //      DataBinder.Eval(dataItem, this.DataOptionSortField));
                //    dict[this.DataOptionSortField] = item.OptionGroupSortSequence;
                //}
                if (!string.IsNullOrEmpty(this.DataOptionGroupField))
                {
                    obj = DataBinder.Eval(dataItem, this.DataOptionGroupField);
                    dict[this.DataOptionGroupField] = obj;
                    item.OptionGroup = string.Format(formatter, this.DataOptionGroupFormatString, obj);
                }

                if (this.DataCustomFields != null && this.DataCustomFields.Length > 0)
                {
                    item.CustomData = new Dictionary<string, object>();
                    foreach (string field in this.DataCustomFields)
                    {
                        obj = DataBinder.Eval(dataItem, field);
                        dict[field] = obj;
                        item.CustomData.Add(field, obj);
                    }
                }

                _items.Add(item);
            }

        }


        #endregion

        /// <summary>
        /// A collection of items in this drop down
        /// </summary>
        [PersistenceMode(PersistenceMode.InnerProperty)]
        [NotifyParentProperty(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public Collection<DropDownItem> Items
        {
            get
            {
                return _items;
            }
        }

        [Obsolete("Use Value")]
        [Browsable(true)]
        public string SelectedValue 
        {
            get 
            {
                return this.Value;
            }
            set 
            {
                this.Value = value;
            }
        }
        /// <summary>
        /// TODO: Get and set value;text
        /// </summary>
        [Browsable(true)]
        public override string Value
        {
            get;
            set;
        }

        #region Rendering

        /// <summary>
        /// Data bind if necessary
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPreRender(EventArgs e)
        {
            if (!string.IsNullOrEmpty(this.DataSourceID))
            {
                EnsureDataBound(true);
            }

            base.OnPreRender(e);
        }

        protected override HtmlTextWriterTag TagKey
        {
            get
            {
                return HtmlTextWriterTag.Select;
            }
        }

        /// <summary>
        /// If there is a single value in the list, and we are not cascadable, displays as label
        /// </summary>
        /// <param name="writer"></param>
        protected override void Render(HtmlTextWriter writer)
        {
            if (this.Validators.OfType<Required>().Any(p => p.DependsOnState == DependsOnState.NotSet) && this.IsEnabled)
            {
                writer.AddStyleAttribute(HtmlTextWriterStyle.WhiteSpace, "nowrap");
                writer.RenderBeginTag(HtmlTextWriterTag.Span);
            }
            base.Render(writer);
            if (this.Validators.OfType<Required>().Any(p => p.DependsOnState == DependsOnState.NotSet) && this.IsEnabled)
            {
                writer.RenderBeginTag(HtmlTextWriterTag.Sup);
                writer.Write("*");
                writer.RenderEndTag();  // sup
                writer.RenderEndTag();  // span
            }
            if (!string.IsNullOrEmpty(this.Cascadable.WebMethod))
            {
                // Hidden Field to postback selected text
                writer.AddAttribute(HtmlTextWriterAttribute.Type, "hidden");
                writer.AddAttribute(HtmlTextWriterAttribute.Name, this.HiddenFieldName);
                if (this.SelectedItem != null)
                {
                    writer.AddAttribute(HtmlTextWriterAttribute.Value, this.SelectedItem.Text);
                }
                writer.RenderBeginTag(HtmlTextWriterTag.Input);
                writer.RenderEndTag();
            }
        }

        /// <summary>
        /// Render each item as an OPTION tag
        /// </summary>
        /// <param name="writer"></param>
        protected override void RenderContents(HtmlTextWriter writer)
        {
            var groups = _items.ToLookup(p => p.OptionGroup);
            string selectedValue = this.Value;      // for efficiency
            var groupCount = groups.Count(p => !string.IsNullOrEmpty(p.Key));
            foreach (var group in groups)
            {
                if (groupCount > 1 && !string.IsNullOrEmpty(group.Key))
                {
                    writer.AddAttribute("label", group.Key);
                    writer.RenderBeginTag("optgroup");
                }
                foreach (var li in group)
                {
                    writer.AddAttribute(HtmlTextWriterAttribute.Value, li.Value);
                    if (li.Value == selectedValue)
                    {
                        writer.AddAttribute(HtmlTextWriterAttribute.Selected, "selected");
                    }
                    if (!string.IsNullOrEmpty(li.CssClass))
                    {
                        writer.AddAttribute(HtmlTextWriterAttribute.Class, li.CssClass);
                    }
                    writer.RenderBeginTag(HtmlTextWriterTag.Option);
                    writer.Write(li.Text);
                    writer.RenderEndTag();      // option
                }
                if (groupCount > 1 && !string.IsNullOrEmpty(group.Key))
                {
                    writer.RenderEndTag();      // optgroup
                }
            }
        }

        #endregion

        internal override string GetClientCode(ClientCodeType codeType)
        {
            switch (codeType)
            {
                case ClientCodeType.InterestEvent:
                    return "click";

                case ClientCodeType.InputSelector:
                    return this.ClientSelector;

                case ClientCodeType.GetValue:
                    return "function(e) { return $(this).val(); }";
                case ClientCodeType.LoadData:
                    return "function(data) { $(this).dropDownListEx('fill', data); }";

                case ClientCodeType.PreLoadData:
                    return "function(data) { $(this).dropDownListEx('preFill'); }";

                case ClientCodeType.SetCookie:
                    if (string.IsNullOrEmpty(this.Cascadable.WebMethod))
                    {
                        // Base class will do fine
                        return base.GetClientCode(codeType);
                    }
                    else
                    {
                        // Store both text and value in cookie
                        string func = string.Format(@"function(e) {{
$(this).{0}('setCookie');
}}", this.WidgetName);
                        return func;
                    }

                default:
                    return base.GetClientCode(codeType);
            }

        }

        /// <summary>
        /// Returns text corresponding to the <see cref="SelectedItem"/>.
        /// </summary>
        public override string DisplayValue
        {
            get
            {
                var item = this.SelectedItem;
                if (item == null)
                {
                    return string.Empty;
                }
                else
                {
                    return this.SelectedItem.Text;
                }
            }
        }

        /// <summary>
        /// Name of the hidden field which will post back the selected text
        /// </summary>
        private string HiddenFieldName
        {
            get
            {
                return this.ClientID + "_hf";
            }
        }
        /// <summary>
        /// Accesses the posted value from <paramref name="postCollection"/> using <paramref name="postDataKey"/>
        /// and sets it as the <see cref="Value"/>
        /// </summary>
        /// <param name="postDataKey"></param>
        /// <param name="postCollection"></param>
        /// <returns></returns>
        public override bool LoadPostData(string postDataKey, NameValueCollection postCollection)
        {
            string str = postCollection[postDataKey];
            this.Value = str;
            if (!string.IsNullOrEmpty(this.Cascadable.WebMethod))
            {
                string text = postCollection[HiddenFieldName];
                this.Items.Add(new DropDownItem() { Value = this.Value, Text = text, Persistent = DropDownPersistenceType.Never });
            }
            return false;
        }

        protected override void PreCreateScripts()
        {
            if (_cascadable != null)
            {
                _cascadable.CreateCascadeScripts(this);
            }
            if (string.IsNullOrEmpty(this.Cascadable.WebMethod))
            {
                // Scripts not needed
                this.WidgetName = string.Empty;
            }
            else
            {
                StringBuilder sb = new StringBuilder("[");
                bool needComma = false;
                foreach (var item in this.Items.Where(p => p.Persistent != DropDownPersistenceType.Never))
                {
                    if (needComma)
                    {
                        sb.Append(",");
                    }
                    else
                    {
                        needComma = true;
                    }
                    sb.AppendFormat("{{ Text:'{0}', Value:'{1}', Persistent:'{2}' }}",
                        item.Text, item.Value, item.Persistent);
                }
                if (needComma)
                {
                    // There is at least one persistent item
                    sb.Append("]");
                    this.ClientOptions.AddRaw("persistentItems", sb.ToString());
                }
                this.ClientOptions.Add("clientPopulate", true);
                if ((this.UseCookie & CookieUsageType.Write) == CookieUsageType.Write)
                {
                    // When filled via AJAX, we store both text and value in cookie
                    this.ClientOptions.Add("cookieName", this.QueryString);
                    this.ClientOptions.Add("cookieExpiryDays", this.CookieExpiryDays);
                }
            }

            base.PreCreateScripts();
        }

        protected override void SetValueFromCookie(string cookieValue)
        {
            if (string.IsNullOrEmpty(this.Cascadable.WebMethod))
            {
                // Normal drop down. Cookie contains just the value
                this.Value = cookieValue;
            }
            else
            {
                // Filled via AJAX. Cookie contains both text and value
                JavaScriptSerializer ser = new JavaScriptSerializer();
                string str = HttpUtility.UrlDecode(cookieValue);
                try
                {
                    DropDownItem item = ser.Deserialize<DropDownItem>(str);
                    this.Items.Add(item);
                    this.Value = item.Value;
                }
                catch (ArgumentException)
                {
                    // Garbage cookie. Ignore.
                }
            }
        }
    }
}
