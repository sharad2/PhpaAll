using System;
using System.Linq;
using System.Web.UI.WebControls;
using Eclipse.PhpaLibrary.Database.PIS;
using Eclipse.PhpaLibrary.Web;
using Eclipse.PhpaLibrary.Reporting;

namespace PhpaAll.PIS.Reports
{
    public partial class ACR : PageBase
    {

        protected void btnApplyFilters_Click(object sender, EventArgs e)
        {
            gvACR.DataBind();
        }

        protected void rbACRStatus_Load(object sender, EventArgs e)
        {
            rbACRStatus.Items[0].Text = string.Format("ACR completed in {0}", DateTime.Now.Year);
            rbACRStatus.Items[1].Text = string.Format("ACR pending in {0} ", DateTime.Now.Year);
        }
        protected void gvACR_Load(object sender, EventArgs e)
        {
            gvACR.Caption = string.Format("Annual Confidential Review completed/pending {0}", DateTime.Now.Year);
        }

        protected override void OnPreRender(EventArgs e)
        {
            if (rbACRStatus.Value == "True")
            {
                btnAcrUndo.Visible = false;
            }
            else
            {
                btnACRdone.Visible = false;
            }
            base.OnPreRender(e);

        }
      

        protected void btnACRdone_Click(object sender, EventArgs e)
        {

            if (gvACR.SelectedIndexes.Count > 0)
            {
                using (PISDataContext db = new PISDataContext(ReportingUtilities.DefaultConnectString))
                {
                    Employee emp = new Employee();
                    foreach (int r in gvACR.SelectedIndexes)
                    {
                        gvACR.UpdateRow(r, false);
                        emp = db.Employees.Where(p => p.EmployeeId == Convert.ToInt32(gvACR.DataKeys[r]["EmployeeId"])).Single();
                        emp.ACRDate = DateTime.Now;
                        db.SubmitChanges();
                    }
                }
                sp.AddStatusText("ACR Date update successfully.");

            }
        }

        protected void btnAcrUndo_Click(object sender, EventArgs e)
        {
            using (PISDataContext db = new PISDataContext(ReportingUtilities.DefaultConnectString))
            {
                Employee emp = new Employee();
                foreach (int r in gvACR.SelectedIndexes)
                {
                    gvACR.UpdateRow(r, false);
                    emp = db.Employees.Where(p => p.EmployeeId == Convert.ToInt32(gvACR.DataKeys[r]["EmployeeId"])).Single();
                    emp.ACRDate = null;
                    db.SubmitChanges();
                }
            }
            sp.AddStatusText("ACR Date undo successfully.");

        }

        DateTime Yearstartdate;
        DateTime Yearenddate;
        protected void dsACR_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            PISDataContext db = (PISDataContext)dsACR.Database;

            Yearstartdate = new DateTime(DateTime.Now.Year, 1, 1);
            Yearenddate = new DateTime(DateTime.Now.Year, 12, 31);

            IQueryable<Employee> queryemp = db.Employees;
           
            if (!string.IsNullOrEmpty(tbEmployee.Text))
            {
                if (tbEmployee.Text.Split(' ').ToArray().Length > 1)
                {
                    queryemp = queryemp.Where(p => p.EmployeeCode.Contains(tbEmployee.Text) ||
                   p.FirstName.Contains(tbEmployee.Text.Split(' ').First()) &&
                   p.LastName.Contains(tbEmployee.Text.Split(' ').Last()));
                }
                else
                {
                    queryemp = queryemp.Where(p => p.EmployeeCode.Contains(tbEmployee.Text) ||
                        p.FirstName.Contains(tbEmployee.Text) ||
                        p.LastName.Contains(tbEmployee.Text));
                }
            }
            switch (rbACRStatus.Value)
            {
                case "False":
                    queryemp = queryemp.Where(p => p.ACRDate > Yearstartdate && p.ACRDate < Yearenddate);
                    break;
                case "True":
                    queryemp = queryemp.Where(p => p.ACRDate < Yearstartdate || p.ACRDate > Yearenddate || p.ACRDate.Value == null);
                    break;
                default:
                    break;
            }
            e.Result = queryemp;


        }


    }
}
