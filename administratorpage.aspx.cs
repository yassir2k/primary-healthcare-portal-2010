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
    public partial class administratorPage : System.Web.UI.Page
    {
        private void MessageBox(string msg)
        {
            Page.Controls.Add(new LiteralControl(
                           "<script language='javascript'> window.alert('" + msg + "')</script>"));
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            
            userPassword.Text = "Default Password Is 123456";
            userPassword.Enabled = false;
            if(Session["Who_Is_It"]==null)
            {
                Response.Redirect("index.aspx");
            }
            else
            {
                Object theDate = DateTime.Now.ToShortDateString();
                phcDate.Text = theDate.ToString();
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
                    Connect.Close();
                }
            }
        }

        protected void logOut_Click(object sender, EventArgs e)
        {
            Session.Abandon();
            Session.Contents.RemoveAll();
            FormsAuthentication.RedirectToLoginPage("index.aspx");
        }

        protected void createUser_Click(object sender, EventArgs e)
        {
            if(userRole.Text=="Choose One...")
            {
                MessageBox("Select an option For User role");
            }
            else
            {
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
                    SqlCommand getSiteID = new SqlCommand("Select dbo.getSiteID(@ID)", Connect);
                    getSiteID.Parameters.Add("@ID", SqlDbType.NVarChar);
                    getSiteID.Parameters["@ID"].Value = Session["Who_Is_It"].ToString();
                    Object getSite = getSiteID.ExecuteScalar();
                    string site = getSite.ToString();
                    //Check Whether The User ID Exist In Database
                    SqlCommand getUser1 = new SqlCommand("Select dbo.getUser(@ID)", Connect);
                    getUser1.Parameters.Add("@ID", SqlDbType.NVarChar);
                    getUser1.Parameters["@ID"].Value = userID.Text;
                    Object getUserAll1 = getUser1.ExecuteScalar();
                    string checkIt1 = getUserAll1.ToString();
                    if(checkIt1 == "true")//Meaning There is already a user with this ID
                    {
                        SqlCommand alreadyAUser = new SqlCommand("Select dbo.alreadyAUser(@ID)", Connect);
                        alreadyAUser.Parameters.Add("@ID", SqlDbType.NVarChar);
                        alreadyAUser.Parameters["@ID"].Value = userID.Text;
                        Object collectName = alreadyAUser.ExecuteScalar();
                        string temp = collectName.ToString();
                        Connect.Close();
                        MessageBox("The User " + temp + " Is Already Assigned The User ID No " + userID.Text);
                    }
                    else//Meaning This ID Is Available For Use
                    {
                        SqlCommand newUser = new SqlCommand("newUser", Connect);
                        newUser.CommandType = CommandType.StoredProcedure;
                        newUser.Parameters.Add("@ID", SqlDbType.NVarChar);
                        newUser.Parameters["@ID"].Value = userID.Text;
                        newUser.Parameters.Add("@name", SqlDbType.NVarChar);
                        newUser.Parameters["@name"].Value = userName.Text;
                        newUser.Parameters.Add("@DOB", SqlDbType.DateTime);
                        newUser.Parameters["@DOB"].Value = userDOB.Text;
                        newUser.Parameters.Add("@Password", SqlDbType.NVarChar);
                        newUser.Parameters["@Password"].Value = "123456";
                        newUser.Parameters.Add("@Role", SqlDbType.NVarChar);
                        newUser.Parameters["@Role"].Value = userRole.Text;
                        newUser.Parameters.Add("@Telephone", SqlDbType.NVarChar);
                        newUser.Parameters["@Telephone"].Value = userTelephone.Text;
                        newUser.Parameters.Add("@siteID", SqlDbType.NVarChar);
                        newUser.Parameters["@siteID"].Value = site;
                        newUser.ExecuteReader();
                        Connect.Close();
                        Connect.Open();
                        SqlCommand getUser = new SqlCommand("Select dbo.getUser(@ID)", Connect);
                        getUser.Parameters.Add("@ID", SqlDbType.NVarChar);
                        getUser.Parameters["@ID"].Value = userID.Text;
                        Object getUserAll = getUser.ExecuteScalar();
                        string checkIt = getUserAll.ToString();
                        if (checkIt == "true")
                        {
                            string link = "useraddedpage.aspx?ID=" + userID.Text + "&site=" + site;
                            Response.Redirect(link);
                            Connect.Close();
                        }
                        else
                        {
                            Connect.Close();
                        }
                    }
                }
            }
        }
    }
}
