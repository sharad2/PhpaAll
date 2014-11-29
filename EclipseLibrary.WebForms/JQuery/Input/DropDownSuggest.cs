using System;
using System.ComponentModel;
using System.Linq;
using System.Web.UI;

namespace EclipseLibrary.Web.JQuery.Input
{
    /// <summary>
    /// This is a drop down list which allows users to slect a value which
    /// may not exist in the list.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Very often, you would like to suggest values to the user but at the same
    /// time give him the flexibility of entering any value he chooses. This control
    /// provides a reasonable UI for this task. The <see cref="Value"/>
    /// property returns the value in the text box if no value is chosen in the drop down.
    /// Use the <see cref="TextBox"/> property to specify constraints for text input.
    /// </para>
    /// <para>
    /// The scenario where you want to auto add the value entered in a master table
    /// is also supported. For this scenario, you set the <see cref="AutoSelectedValue"/>
    /// property to false. See the documentation of <c>AutoSelectedValue</c> for an
    /// example.
    /// </para>
    /// <para>
    /// Follows the pattern specified for <c>CompositeControl</c> in MSDN.
    /// Thus it implements <c>INamingContainer</c>
    /// </para>
    /// </remarks>
    /// <example>
    /// <para>
    /// This example allows the user to choose from one of the existing relationships
    /// or enter a new relationship.
    /// </para>
    /// <code>
    /// <![CDATA[
    /// <i:DropDownSuggest ID="ddlRelationship" runat="server" AppendDataBoundItems="true"
    ///     DataSourceID="dsRelationship" FriendlyName="Relationship" SelectedValue='<%# Bind("Relationship") %>'>
    ///     <Items>
    ///         <eclipse:DropDownItem Text="(New Relationship)" Value="" />
    ///     </Items>
    ///     <TextBox runat="server" MaxLength="50" Size="20" />
    ///     <Validators>
    ///         <i:Required />
    ///     </Validators>
    /// </i:DropDownSuggest>
    /// protected void dsRelationship_Selecting(object sender, LinqDataSourceSelectEventArgs e)
    /// {
    ///      using (PISDataContext db = new PISDataContext(ReportingUtilities.DefaultConnectString))
    ///      {
    ///          e.Result = (from fm in db.FamilyMembers
    ///                      where fm.Relationship != null
    ///                      orderby fm.Relationship
    ///                      select fm.Relationship).Distinct().ToArray();
    ///      }                      
    /// }
    /// ]]>
    /// </code>
    /// </example>
    [Obsolete("Use AutoComplete")]
    public class DropDownSuggest : DropDownListEx, INamingContainer
    {
        private readonly TextBoxEx _tb;

        public DropDownSuggest()
        {
            _tb = new TextBoxEx();
            _tb.FocusPriority = FocusPriority.NotAllowed;
            this.AutoSelectedValue = true;
        }

        protected override void CreateChildControls()
        {
            this.Controls.Add(_tb);
            base.CreateChildControls();
        }

        /// <summary>
        /// Use this to set validations needed for the text box.
        /// </summary>
        [PersistenceMode(PersistenceMode.InnerProperty)]
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public TextBoxEx TextBox
        {
            get
            {
                return _tb;
            }
        }

        /// <summary>
        /// If a value is chosen in drop down, empty the text box
        /// </summary>
        protected override void PreCreateScripts()
        {
            string script = string.Format(@"function(e) {{
if ($(this).val() != '') {{
    $('{0}').textBoxEx('setval', '');
}}
}}", _tb.ClientSelector);
            this.ClientEvents.Add(this.ClientChangeEventName, script);
            base.PreCreateScripts();
        }

        protected override void OnPreRender(EventArgs e)
        {
            Required valRequired = this.Validators.OfType<Required>().FirstOrDefault();
            if (valRequired != null)
            {
                // Value in combo is required only if text box is empty
                valRequired.SetDependsOnControl(_tb);
                valRequired.DependsOnState = DependsOnState.Unchecked;
            }
            _tb.ClientEvents.Add("keypress", @"function(e) {
if ($(this).val() != '') {
    $(this).prevAll('select:first').val('');
}
}");
            // If the value chosen is found in the drop down, select it and clear the text box, otherwise clear the ddl
            _tb.ClientEvents.Add(_tb.ClientChangeEventName, @"function(e) {
var tbValue = $(this).val();
var $ddl = $(this).prevAll('select:first');
if (tbValue) {
  if ($('option[value=""' + tbValue + '""]', $ddl).length > 0) {
    $ddl.val(tbValue);
    $(this).val('');
  } else {
    $ddl.val('');
  }
}
}");
            //_tb.WaterMark = this.Items[0].Text;
            if (string.IsNullOrEmpty(_tb.FriendlyName))
            {
                _tb.FriendlyName = this.FriendlyName;
            }

            // If drop down is empty, do not clear value of text box
            if (!string.IsNullOrEmpty(base.Value))
            {
                if (this.SelectedItem == null)
                {
                    // Sharad 8 Aug 2010: This happens when value is passed via query string.
                    // If the query string value is not found in drop down list, set it in the text box
                    _tb.Text = base.Value;
                }
                else
                {
                    _tb.Text = string.Empty;
                }
            }
            base.OnPreRender(e);
        }

        /// <summary>
        /// If selected value is empty, return text box value
        /// </summary>
        public override string Value
        {
            get
            {
                if (string.IsNullOrEmpty(base.Value) && this.AutoSelectedValue)
                {
                    return _tb.Text;
                }
                else
                {
                    return base.Value;
                }
            }
            set
            {
                base.Value = value;
            }
        }

        /// <summary>
        /// Whether the <see cref="Value"/> property should return the
        /// text box value when necessary
        /// </summary>
        /// <remarks>
        /// <para>
        /// This property controls what happens if the user chooses the empty value
        /// in the drop down list.
        /// If this property is true (the default), then
        /// <see cref="Value"/> returns the value of the text box. Otherwise
        /// <c>SelectedValue</c> returns an empty string.
        /// </para>
        /// </remarks>
        /// <example>
        /// <para>
        /// This example allows the user to choose from one of the existing blood groups
        /// or enter a new blood group. Here,BloodGroup is a master table.
        /// </para>
        /// <code>
        /// <![CDATA[
        ///<phpa:PhpaLinqDataSource runat="server" ID="dsEmployee" ContextTypeName="Eclipse.PhpaLibrary.Database.PIS.PISDataContext"
        ///    TableName="Employees" RenderLogVisible="False" Where="EmployeeId == @EmployeeId"
        ///    EnableUpdate="True" OnUpdating="dsEmployee_Updating">
        /// 
        /// 
        ///<asp:FormView runat="server" ID="fvEmployeeDetailsEdit" DataKeyNames="EmployeeId"
        ///    DataSourceID="dsEmployee" DefaultMode="Edit">
        ///<EditItemTemplate>
        ///        <jquery:Tabs runat="server" Selected="0" Collapsible="false" OnLoad="tabs_Load">
        ///            <jquery:JPanel runat="server" HeaderText="Personal">
        ///              <eclipse:LeftLabel runat="server" Text="Blood Group" />
        ///            <eclipse:TwoColumnPanel runat="server">
        ///<phpa:PhpaLinqDataSource runat="server" ID="dsBloodGroup" ContextTypeName="Eclipse.PhpaLibrary.Database.PIS.PISDataContext"
        ///                    Select="new (BloodGroupId, BloodGroupType)" TableName="BloodGroups" RenderLogVisible="false"
        ///                    EnableInsert="true">
        ///                    <InsertParameters>
        ///                        <asp:Parameter Name="BloodGroupId" Type="Int32" />
        ///                        <asp:Parameter Name="BloodGroupType" Type="String" />
        ///                    </InsertParameters>
        ///                </phpa:PhpaLinqDataSource>
        ///                
        ///<i:DropDownSuggest runat="server" ID="ddlBloodGroup" DataSourceID="dsBloodGroup"
        ///                       AppendDataBoundItems="true" DataTextField="BloodGroupType" DataValueField="BloodGroupId"
        ///                       SelectedValue='<%# Bind("BloodGroupId") %>' FriendlyName="Blood Group" AutoSelectedValue="false"
        ///                       EnableViewState="true">
        ///                       <Items>
        ///                           <eclipse:DropDownItem Text="(New Blood Group)" Value="" />
        ///                       </Items>
        ///                       <TextBox runat="server" Size="5" MaxLength="10" />
        ///                   </i:DropDownSuggest>
        ///                   .........
        ///              </eclipse:TwoColumnPanel>
        ///     </jquery:JPanel>
        ///     ....
        ///        </jquery:Tabs>
        ///    </EditItemTemplate>
        ///</asp:FormView>  
        ///
        ///protected void dsEmployee_Updating(object sender, LinqDataSourceUpdateEventArgs e)
        ///{
        ///    Employee emp = (Employee)e.NewObject;
        ///    DropDownSuggest ddlBloodGroup = (DropDownSuggest)fvEmployeeDetailsEdit.FindControl("ddlBloodGroup");
        ///    PISDataContext db = (PISDataContext)dsEmployee.Database;
        ///    if (string.IsNullOrEmpty(ddlBloodGroup.SelectedValue) && !string.IsNullOrEmpty(ddlBloodGroup.TextBox.Text))
        ///    {
        ///        // Need to insert
        ///        BloodGroup bg = new BloodGroup()
        ///        {
        ///            BloodGroupType = ddlBloodGroup.TextBox.Text
        ///        };
        ///        db.BloodGroups.InsertOnSubmit(bg);            
        ///        db.SubmitChanges();
        ///        emp.BloodGroupId = bg.BloodGroupId;
        ///    }
        ///}
        /// 
        /// ]]>
        /// </code>
        /// </example>
        [Browsable(true)]
        [DefaultValue(true)]
        public bool AutoSelectedValue { get; set; }

        /// <summary>
        /// Do nothing. We will render our textbox later
        /// </summary>
        /// <param name="writer"></param>
        protected override void RenderChildren(HtmlTextWriter writer)
        {
            //base.RenderChildren(writer);
        }

        /// <summary>
        /// Render the textbox
        /// </summary>
        /// <param name="writer"></param>
        //protected override void RenderAfterTag(HtmlTextWriter writer)
        //{
        //    base.RenderAfterTag(writer);


        //}

        protected override void Render(HtmlTextWriter writer)
        {
            base.Render(writer);
            _tb.RenderControl(writer);
        }

        //protected override void SetValueFromQueryString(string queryStringValue)
        //{
        //    base.SetValueFromQueryString(queryStringValue);
        //}
        /// <summary>
        /// Allow controls
        /// </summary>
        /// <returns></returns>
        protected override ControlCollection CreateControlCollection()
        {
            return new ControlCollection(this);
        }

        /// <summary>
        /// We always have controls
        /// </summary>
        /// <returns></returns>
        public override bool HasControls()
        {
            return true;
        }

        public override ControlCollection Controls
        {
            get
            {
                EnsureChildControls();
                return base.Controls;
            }
        }

        public override void DataBind()
        {
            EnsureChildControls();
            base.DataBind();
        }

    }
}
