using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;

namespace BLL
{
    public class BLL_staff
    {
        public BLL_staff() { }
        public DataTable GetData(string keyword)
        {
            return SqlDataAccess.GetDataFromTable(
                $"NhanVien WHERE TenNV LIKE N'%{keyword}%' " +
                $"OR DiaChi LIKE N'%{keyword}%' " +
                $"OR SDT LIKE '%{keyword}%' " +
                $"OR TenDangNhap LIKE '%{keyword}%' " +
                $"OR GioiTinh LIKE N'%{keyword}%'"
            );
        }
        public void AddRecord(string name, string gender, string address, string email, string phone, DateTime startDate, string username, string password, string role, string isActive)
        {
            string formattedDate = startDate.ToString("yyyy-MM-dd");
            string command = $"EXEC Proc_AddStaff N'{name}', N'{gender}', N'{address}', '{email}', '{phone}', '{formattedDate}', '{username}', '{password}', N'{role}', '{isActive}'";
            SqlDataAccess.ExecuteNonQuery(command);
        }
        public void UpdateRecord(string id, string name, string gender, string address, string email, string phone, DateTime startDate, string username, string password, string role, string isActive)
        {
            string formattedDate = startDate.ToString("yyyy-MM-dd");
            string command = $"EXEC Proc_UpdateStaff '{id}', N'{name}', N'{gender}', N'{address}', '{email}', '{phone}', '{formattedDate}', '{username}', '{password}', N'{role}', '{isActive}'";
            SqlDataAccess.ExecuteNonQuery(command);
        }

        public void DeleteRecord(string id)
        {
            string command = $"EXEC Proc_DeleteStaff '{id}'";
            SqlDataAccess.ExecuteNonQuery(command);
        }

        public DataRow GetRecord(string id)
        {
            return SqlDataAccess.GetDataFromTable($"NhanVien WHERE MaNV = '{id}'").Rows[0];
        }
    }
}
