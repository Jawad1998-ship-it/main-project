using Microsoft.Data.SqlClient;

namespace Donation_Website
{
    public class DBConnection
    {

        private const string connectionString = @"Server=TOKEY-ZAWAD\SQLEXPRESS;Database=DonationManagementDB;Integrated Security=True;Encrypt=False;TrustServerCertificate=True;";

        public SqlCommand GetQuery(string query)
            {
                var connection = new SqlConnection(connectionString);
                SqlCommand cmd = new SqlCommand(query, connection);

                return cmd;
            }

        

    }
}
