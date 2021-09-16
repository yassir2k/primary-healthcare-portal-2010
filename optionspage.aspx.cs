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
    public partial class optionsPage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Object theDate = DateTime.Now.ToShortDateString();
            phcDate.Text = theDate.ToString();
            if (Session["Who_Is_it"] == null || Session["Role"] == null)
            {
                Response.Redirect("index.aspx");
            }
            else
            {
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
                    //Authenticity
                    SqlCommand userRoleSite = new SqlCommand("Select dbo.userRoleSite(@ID,@Site)", Connect);
                    userRoleSite.Parameters.Add("@ID", SqlDbType.NVarChar);
                    userRoleSite.Parameters["@ID"].Value = get_who;
                    //Lets Get Site ID
                    SqlCommand getSiteID = new SqlCommand("Select dbo.getSiteID(@user)", Connect);
                    getSiteID.Parameters.Add("@user", SqlDbType.NVarChar);
                    getSiteID.Parameters["@user"].Value = get_who;
                    which_Facility = getSiteID.ExecuteScalar();
                    //Site ID
                    userRoleSite.Parameters.Add("@Site", SqlDbType.NVarChar);
                    userRoleSite.Parameters["@Site"].Value = which_Facility.ToString();
                    which_Facility = userRoleSite.ExecuteScalar();
                    int what = Convert.ToInt16(which_Facility.ToString());
                    if (what == 2 && Session["Role"].ToString() == "Administrator" && Session["Who_Is_It"].ToString() == get_who)
                    {
                        SqlCommand user = new SqlCommand("Select dbo.whichUser(@user)", Connect);
                        user.Parameters.Add("@user", SqlDbType.NVarChar);
                        user.Parameters["@user"].Value = get_who;
                        Object which_user = user.ExecuteScalar();
                        theUser.Text = "Welcome, " + which_user.ToString();
                    }
                    else
                    {
                        Session.Abandon();
                        Session.Contents.RemoveAll();
                        FormsAuthentication.RedirectToLoginPage("index.aspx");
                    }
                }
            }
        }

        protected void logOut_Click(object sender, EventArgs e)
        {
            Session.Abandon();
            Session.Contents.RemoveAll();
            FormsAuthentication.RedirectToLoginPage("index.aspx");
        }
    }
}
