using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using System.Security.Cryptography.X509Certificates;

namespace WebUIDesign.Pages
{
    public class IndexModel : PageModel
    {   

        

        private readonly ILogger<IndexModel> _logger;

        
        

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }


        public class UserInfo
        {
            public string Id;
            public string username;
            public string displayname;
            public string phone;
            public string email;
            public string userole;
            public string enabled = "false";


        }

        public List<UserInfo> users = new List<UserInfo>();



        public void OnGet()
        {
            try {

                String connectionString = "Data Source=DESKTOP-JUSA930\\SQLEXPRESS;Initial Catalog=User;Integrated Security=True";
                using(SqlConnection connection = new SqlConnection(connectionString)) { 
                    connection.Open();
                    String sql = "Select * from Users";
                    using(SqlCommand command = new SqlCommand(sql,connection)) { 
                       
                        using(SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read()) { 
                              
                                UserInfo userInfo = new UserInfo();
                                userInfo.Id=""+reader.GetInt32(0);
                                userInfo.username=reader.GetString(1);
                                userInfo.displayname=reader.GetString(2);  
                                userInfo.phone=reader.GetString(3);
                                userInfo.email=reader.GetString(4);
                                userInfo.userole=reader.GetString(5);
                                userInfo.enabled=reader.GetString(6);
                                users.Add(userInfo);    
                            }
                        }
                    }
                    connection.Close();
                }
            }
            catch(Exception ex) { 
            
              Console.WriteLine("Exception: "+ex.Message);    
            }   


        }

        public void OnPost() {



           


             UserInfo userInfo1 = new UserInfo();
             userInfo1.username = Request.Form["username"];
             userInfo1.displayname = Request.Form["displayname"];
             userInfo1.phone = Request.Form["phone"];
             userInfo1.email = Request.Form["email"];
             userInfo1.userole = Request.Form["userole"];
            if (Request.Form["enabled"].Equals("true") ) {
                userInfo1.enabled = Request.Form["enabled"];
            }
            else
            {
                userInfo1.enabled = "false";
            }
             

             try {
                 String connectionString = "Data Source=DESKTOP-JUSA930\\SQLEXPRESS;Initial Catalog=User;Integrated Security=True";
                 using(SqlConnection connection = new SqlConnection(connectionString))
                 {
                     connection.Open();
                     String sql="INSERT INTO Users (Username,Displayname,Phone,Email,Userole,Enabling) values (@username,@displayname,@phone,@email,@userole,@enabling)";
                     using(SqlCommand command = new SqlCommand(sql, connection))
                     {
                         command.Parameters.AddWithValue("@username", userInfo1.username);
                         command.Parameters.AddWithValue("@displayname", userInfo1.displayname);
                         command.Parameters.AddWithValue("@phone",userInfo1.phone);
                         command.Parameters.AddWithValue("@email", userInfo1.email);
                         command.Parameters.AddWithValue("@userole", userInfo1.userole);
                         command.Parameters.AddWithValue("@enabling", userInfo1.enabled);
                         command.ExecuteNonQuery();

                     }

                 }

             }catch(Exception ex) {

                 Console.WriteLine("Message: "+ex.Message);
             }

             userInfo1.username = "";
             userInfo1.email = "";
             userInfo1.displayname = "";
             userInfo1.phone = "";
             userInfo1.enabled = "false";
             userInfo1.userole = "";
             Response.Redirect("/Index");

            /*HasData = true;
            //var FormData = Request.Form;
            username = Request.Form["username"];
            displayname = Request.Form["displayname"];
            email = Request.Form["email"];
            phone = Request.Form["phone"];
            userole = Request.Form["userole"];
            enabled = Request.Form.ContainsKey("enabled");
            //IndexTo indeks = new IndexTo(username, displayname, email, phone, userole, enabled);
            //Console.WriteLine(username);*/

        }
    }
}