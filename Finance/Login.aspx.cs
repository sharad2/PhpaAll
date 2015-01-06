/*
 *  Programming: Eclipse Systems (P) Ltd., NOIDA, INDIA
 *  E-mail: support@eclsys.com
 *
 *  $Workfile:   Login.aspx.cs  $
 *  $Revision: 30534 $
 *  $Author: ssinghal $
 *  $Date: 2010-01-29 14:06:59 +0530 (Fri, 29 Jan 2010) $
 *  $Modtime:   Jul 09 2008 17:52:14  $
 *
 *  $Log:   S:/Projects/PHPA2/archives/Finance/Login.aspx.cs-arc  $
 * 
 *    Rev 1.5   Jul 09 2008 17:54:10   vraturi
 * PVCS Template Added.
 */
using System;
using System.Web.Security;
using Eclipse.PhpaLibrary.Web;
using System.Web;
using Eclipse.PhpaLibrary.Reporting;
using System.Data.SqlClient;
using System.Data;

namespace Finance
{
    public partial class Login : PageBase
    {
        //protected override void OnLoad(EventArgs e)
        //{
        //    Control ctlUserName = Login1.FindControl("UserName");
        //    ctlUserName.Focus();
        //    // After loggin in, we want to navaigate to our referrer
        //    if (!IsPostBack && this.Request.UrlReferrer != null)
        //    {
        //        this.Login1.DestinationPageUrl = this.Request.UrlReferrer.ToString();
        //    }
        //    string btnLogin = Login1.FindControl("LoginButton").UniqueID;
        //    Page.Form.DefaultButton = btnLogin;
        //    base.OnLoad(e);
        //}

        protected override void OnLoad(EventArgs e)
        {
            string returnUrl = this.Request.QueryString["ReturnUrl"];
            if (!string.IsNullOrEmpty(returnUrl))
            {
                SiteMapNode node = SiteMap.Provider.FindSiteMapNode(returnUrl);
                string pageTitle;
                if (node == null)
                {
                    pageTitle = returnUrl;
                }
                else
                {
                    pageTitle = node.Title;
                }
                lblWelcome.Text = string.Format(lblWelcome.Text, pageTitle);
                lblWelcome.Visible = true;
            }
            base.OnLoad(e);
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {

            if (Membership.ValidateUser(tbUserName.Text, tbPassword.Text))
            {
               
                FormsAuthentication.RedirectFromLoginPage(tbUserName.Text, cbRememberMe.Checked);
            }
            else
            {
                valSummary.ErrorMessages.Add("Your login request was not successful. Please try again.");
            }
        }
        
        protected void btnSavePassword_Click(object sender, EventArgs e)
        {
            // The user must be logged in
            var user = Membership.GetUser(tbCPUserName.Text);
            var success = user.ChangePassword(tbCurrentpassword.Text, tbNewPassword.Text);
            if (!success)
            {
                throw new NotImplementedException("Password change failed. Please re-enter your values and try again.");
            }
            //SqlConnection connection = new SqlConnection(ReportingUtilities.DefaultConnectString);
            //string strUserName = null;
            //string strCurrentPassword = null;  
            //try
            //{
            //    connection.Open();
            //    SqlDataAdapter da = new SqlDataAdapter("select * from PhpaUser where username='" + tbCPUserName.Text + "'", connection);
            //    DataSet ds = new DataSet();
            //    da.Fill(ds);
            //    connection.Close();
            //    strUserName = ds.Tables[0].Rows[0][1].ToString();
            //    strCurrentPassword = ds.Tables[0].Rows[0][2].ToString();
               
            //    if (strCurrentPassword.CompareTo(tbCurrentpassword.Text) == 0)
            //    {
            //        if (tbNewPassword.Text.CompareTo(tbConfirmPassword.Text) == 0)
            //        {
            //            connection.Open();
            //            SqlCommand cmd = new SqlCommand("update PhpaUser set password='" + tbNewPassword.Text + "' where username='" + tbCPUserName.Text + "'");
            //            cmd.Connection = connection;
            //            int querystatus = cmd.ExecuteNonQuery();
            //            if (querystatus > 0)
            //            {
            //                lblMsg.ForeColor = System.Drawing.Color.Green;
            //                lblMsg.Text = "Password updated successfully";
            //            }
            //            else
            //            {
            //                lblMsg.ForeColor = System.Drawing.Color.Red;
            //                lblMsg.Text = "Password updation faliure. Please try again.";
            //            }
            //        }
            //        else
            //        {
            //            lblMsg.ForeColor = System.Drawing.Color.Red;
            //            lblMsg.Text = "Failed Password Confirmation";
            //        }
            //    }
            //    else
            //    {
            //        lblMsg.ForeColor = System.Drawing.Color.Red;
            //        lblMsg.Text = "Current password is incorrect";
            //        tbCurrentpassword.Focus();
            //    }
            //    connection.Close();
            //}
            //// TODO: 
            //catch {
            //    connection.Close();
            //    lblMsg.ForeColor = System.Drawing.Color.Red;
            //    if (string.IsNullOrEmpty(strUserName))
            //    {
            //        lblMsg.Text = "Please enter a valid user name.";
            //        tbCPUserName.Focus();
            //        return;
            //    }
            //    lblMsg.Text = "Some problem occur. Please start again.";
            //}
        }


    }
}
