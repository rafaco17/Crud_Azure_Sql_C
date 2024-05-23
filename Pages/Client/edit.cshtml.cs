using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using System.Reflection.PortableExecutable;
using static test.Pages.Client.IndexModel;

namespace test.Pages.Client
{
    public class editModel : PageModel
    {
        public ClientInfo clientInfo = new ClientInfo();
        public String errorMessage = "";
        public String succesMessage = "";
        public void OnGet()
        {
            String id = Request.Query["id"];

            try
            {
                String connectionString = "URL-CONECTION-DATABASE;";

                using SqlConnection connection = new(connectionString);
                connection.Open();
                String sql = "SELECT * FROM clients WHERE id=@id";
                using (SqlCommand command = new(sql, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    using SqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        clientInfo.Id = "" + reader.GetInt32(0).ToString();
                        clientInfo.Name = reader.GetString(1);
                        clientInfo.Email = reader.GetString(2);
                        clientInfo.Phone = reader.GetString(3);
                        clientInfo.Address = reader.GetString(4);
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = "Exception: " + ex.ToString();
            }
        }

        public void OnPost()
        {
            clientInfo.Id = Request.Form["id"];
            clientInfo.Name = Request.Form["name"];
            clientInfo.Email = Request.Form["email"];
            clientInfo.Phone = Request.Form["phone"];
            clientInfo.Address = Request.Form["address"];

            if (clientInfo.Name.Length == 0 || clientInfo.Email.Length == 0 || clientInfo.Phone.Length == 0 || clientInfo.Address.Length == 0 || clientInfo.Id.Length == 0)
            {
                errorMessage = "All the fields are required";
                return;
            }

            try
            {
                String connectionString = "URL-CONECTION-DATABASE;";
                using SqlConnection connection = new(connectionString);
                connection.Open();
                String sql = "UPDATE clients " + "SET name=@name, email=@email, phone=@phone, address=@address " + "WHERE id=@id";

                using (SqlCommand command = new(sql, connection))
                {
                    command.Parameters.AddWithValue("@name", clientInfo.Name);
                    command.Parameters.AddWithValue("@email", clientInfo.Email);
                    command.Parameters.AddWithValue("@phone", clientInfo.Phone);
                    command.Parameters.AddWithValue("@address", clientInfo.Address);
                    command.Parameters.AddWithValue("@id", clientInfo.Id);
                    command.ExecuteNonQuery();
                }

                clientInfo.Name = "";
                clientInfo.Email = "";
                clientInfo.Phone = "";
                clientInfo.Address = "";
                succesMessage = "New Client Added Correctly";

                Response.Redirect("/Client/Index");
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return;
            }

            Response.Redirect("/Client/Index");
        }
    }
}
