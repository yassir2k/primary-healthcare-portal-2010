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
    public partial class index : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!(Page.IsPostBack))
            {
                Session.Abandon();
            }
        }

        protected void LoginButton_Click(object sender, EventArgs e)
        {
            string DbConnectionStr = "Server = .\\SQLEXPRESS ;Database=NPHCDA; Integrated Security=SSPI";
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
                SqlCommand log_user = new SqlCommand("Select dbo.Access(@user,@Key)", Connect);
                log_user.Parameters.Add("@User", SqlDbType.NVarChar);
                log_user.Parameters["@User"].Value = Login1.UserName;
                log_user.Parameters.Add("@Key", SqlDbType.NVarChar);
                log_user.Parameters["@Key"].Value = Login1.Password;
                Object Do_Log_user = log_user.ExecuteScalar();
                bool Check_Value_For_user = Convert.ToBoolean(Convert.ToInt16(Do_Log_user.ToString()));
                if (Check_Value_For_user)
                {
                    string link = "basicconfirmpage.aspx?fname=" + Login1.UserName;
                    Session["Who_Is_It"] = Login1.UserName;
                    //Lets Check whether This user Is An Administrator or Not
                    SqlCommand checkUserRole = new SqlCommand("Select dbo.checkUserRole(@ID)",Connect);
                    checkUserRole.Parameters.Add("@ID", SqlDbType.NVarChar);
                    checkUserRole.Parameters["@ID"].Value = Login1.UserName;
                    Object getUserRole = checkUserRole.ExecuteScalar();
                    string role = getUserRole.ToString();
                    if(role == "Administrator")
                    {
                        link = "confirmpage.aspx?fname=" + Login1.UserName;
                        Session["Role"] = "Administrator";
                        Connect.Close();
                        Response.Redirect(link);
                    }
                    else
                    {
                        Session["Role"] = "Basic";
                        Connect.Close();
                        Response.Redirect(link);
                    }
                }
                else
                {
                    ;
                }
            }
        }
    }
}
