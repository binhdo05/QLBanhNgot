using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;

namespace BLL
{
    public class BLL_customer
    {
        public BLL_customer() { }
        public DataTable GetData(string keyword)
        {
            return SqlDataAccess.GetDataFromTable($"KhachHang WHERE TenKH LIKE N'%{keyword}%' OR DiaChi LIKE N'%{keyword}%' OR SDT LIKE '{keyword}'");
        }

        public DataRow GetRecord(string ID)
        {
            return SqlDataAccess.GetDataFromTable($"KhachHang WHERE MaKH = '{ID}'").Rows[0];
        }

        public void AddRecord(string name, string gender, string phone, string address)
        {
            string command = $"EXEC Proc_AddCustomer N'{name}', N'{gender}', '{phone}', N'{address}'";
            SqlDataAccess.ExecuteNonQuery(command);
        }

        public void UpdateRecord(string id, string name, string gender, string phone, string address)
        {
            string command = $"EXEC Proc_UpdateCustomer '{id}', N'{name}', N'{gender}', '{phone}', N'{address}'";
            SqlDataAccess.ExecuteNonQuery(command);
        }

        public void DeleteRecord(string id)
        {
            SqlDataAccess.ExecuteNonQuery($"EXEC Proc_rmCustomer '{id}'");
        }

    }
}
