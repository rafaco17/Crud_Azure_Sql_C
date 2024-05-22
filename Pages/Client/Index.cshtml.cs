using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace test.Pages.Client
{
    public class IndexModel : PageModel
    {
        public List<ClientInfo> ListClients = [];

        public void OnGet()
        {
            try
            {
                String connectionString = "URL-CONECTION-DATABASE;";

                using SqlConnection connection = new(connectionString);
                connection.Open();
                String sql = "SELECT * FROM clients";

                using (SqlCommand command = new(sql, connection))
                {
                    using SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        ClientInfo clientInfo = new() 
                        {
                            Id = reader.GetInt32(0).ToString(),
                            Name = reader.GetString(1),
                            Email = reader.GetString(2),
                            Phone = reader.GetString(3),
                            Address = reader.GetString(4),
                            Create_at = reader.GetDateTime(5).ToString()
                        };

                        ListClients.Add(clientInfo);
                    }       
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.ToString());
            }
        }

        public class ClientInfo
        {
            public string Id { get; set; } = string.Empty;
            public string Name { get; set; } = string.Empty;
            public string Email { get; set; } = string.Empty;
            public string Phone { get; set; } = string.Empty;
            public string Address { get; set; } = string.Empty;
            public string Create_at { get; set; } = string.Empty;
        }
    }
}
