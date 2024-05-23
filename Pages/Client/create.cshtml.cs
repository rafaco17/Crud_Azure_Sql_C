using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using static test.Pages.Client.IndexModel;

namespace test.Pages.Client
{
    public class createModel : PageModel
    {
        public ClientInfo clientInfo = new ClientInfo();
        public String errorMessage = "";
        public String succesMessage = "";
        public void OnGet()
        {
        }

        public void OnPost()
        {
            clientInfo.Name = Request.Form["name"];
            clientInfo.Email = Request.Form["email"];
            clientInfo.Phone = Request.Form["phone"];
            clientInfo.Address = Request.Form["address"];

            if (clientInfo.Name.Length == 0 || clientInfo.Email.Length == 0 || clientInfo.Phone.Length == 0 || clientInfo.Address.Length == 0)
            {
                errorMessage = "All the fields are required";
                return;
            }

            try
            {
                String connectionString = "URL-CONECTION-DATABASE;";
                using SqlConnection connection = new(connectionString);
                connection.Open();
                String sql = "INSERT INTO clients (name, email, phone, address) VALUES (@name, @email, @phone, @address);";

                using (SqlCommand command = new(sql, connection))
                {
                    command.Parameters.AddWithValue("@name", clientInfo.Name);
                    command.Parameters.AddWithValue("@email", clientInfo.Email);
                    command.Parameters.AddWithValue("@phone", clientInfo.Phone);
                    command.Parameters.AddWithValue("@address", clientInfo.Address);
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
                errorMessage = "Exception: " + ex.ToString();
            }
        }
    }
}
