using System.Data.SqlClient;
using System.Web;

namespace refactor_me.Models
{
    public class Helpers
    {
        private const string ConnectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename={DataDirectory}\Database.mdf;Integrated Security=True";

        public static SqlDataReader ExecuteSQL(string cmdText)
        {
            var conn = NewConnection();
            var cmd = new SqlCommand(cmdText, conn);
            conn.Open();

            return cmd.ExecuteReader();
        }
        private static SqlConnection NewConnection()
        {
            var connStr = ConnectionString.Replace("{DataDirectory}", HttpContext.Current.Server.MapPath("~/App_Data"));
            return new SqlConnection(connStr);
        }
    }
}