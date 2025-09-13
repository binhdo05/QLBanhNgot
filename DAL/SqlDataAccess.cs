using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class SqlDataAccess
    {
        private const string connectionString = @"Data Source=ANDUONG;Initial Catalog=DAn_1_QLBanhNgot;Integrated Security=True;TrustServerCertificate=True";
        public static DataTable GetDataFromTable(string tableName, string columns = "*") //hàm trả về bảng dữ liệu từ view hoặc table
        {
            string query = $"SELECT {columns} FROM {tableName}";

            DataTable dataTable = new DataTable();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    try
                    {
                        connection.Open();

                        SqlDataAdapter adapter = new SqlDataAdapter(command);
                        adapter.Fill(dataTable);
                    }
                    catch { }
                }
            }

            return dataTable;
        }

        public static void ExecuteNonQuery(string sqlCommand) // hàm thực thi câu lệnh SQL không trả về clg
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(sqlCommand, connection))
                {

                    connection.Open();
                    command.ExecuteNonQuery();

                }
            }
        }
        public static DataTable GetDataFromProcedure(string procCommand)
        {
            DataTable dataTable = new DataTable();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(procCommand, connection))
                {

                    connection.Open();

                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    adapter.Fill(dataTable);
                }
            }
            return dataTable;
        }
        public static DataTable GetDataFromTableSearch(string tableName, string columns = "*", string key = "")
        {
            string query = $"SELECT {columns} FROM {tableName}";

            if (!string.IsNullOrEmpty(key))
            {
                query += " WHERE TenKhachHang LIKE @key OR NgayTao LIKE @key OR trangthai LIKE @key";
            }

            DataTable dataTable = new DataTable();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@key", "%" + key + "%");

                    try
                    {
                        connection.Open();

                        SqlDataAdapter adapter = new SqlDataAdapter(command);
                        adapter.Fill(dataTable);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error: " + ex.Message);
                    }
                }
            }

            return dataTable;
        }

    }
}
