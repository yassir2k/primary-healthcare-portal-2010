using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Data.SqlClient;

namespace Primary_Healthcare
{
    public partial class useraddedPage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Object theDate = DateTime.Now.ToShortDateString();
            phcDate.Text = theDate.ToString();
            if (Session["Who_Is_it"] == null)
            {
                Response.Redirect("index.aspx");
            }
            else
            {
                string ID = Request["ID"];
                string siteName = Request["site"];
                newUserLabel.Text = "You Have Successfully Added A New User With ID "+ID+" & Site ID "+siteName;
                string get_who = Session["Who_Is_It"].ToString();
                temp.Text = get_who;
                string DbConnectionStr = "Data Source=.\\SQLEXPRESS;Initial Catalog=NPHCDA;Integrated Security=SSPI";
                SqlConnection Connect = new SqlConnection(DbConnectionStr);
                try
                {
                    Connect.Open();
                }
                catch (System.Exception)
                {

                }
                finally
                {
                    SqlCommand facility = new SqlCommand("Select dbo.check_ID(@user)", Connect);
                    facility.Parameters.Add("@user", SqlDbType.NVarChar);
                    facility.Parameters["@user"].Value = get_who;
                    Object which_Facility = facility.ExecuteScalar();
                    phcLabel.Text = which_Facility.ToString();
                    SqlCommand user = new SqlCommand("Select dbo.whichUser(@user)", Connect);
                    user.Parameters.Add("@user", SqlDbType.NVarChar);
                    user.Parameters["@user"].Value = get_who;
                    Object which_user = user.ExecuteScalar();
                    theUser.Text = "Welcome, " + which_user.ToString();
                }
            }
        }

        protected void logOut_Click(object sender, EventArgs e)
        {
            Session.Abandon();
            Session.Contents.RemoveAll();
            FormsAuthentication.RedirectToLoginPage("index.aspx");
        }

        protected void gobackMenuButton_Click(object sender, EventArgs e)
        {
            Response.Redirect("optionspage.aspx");
        }
    }
}
