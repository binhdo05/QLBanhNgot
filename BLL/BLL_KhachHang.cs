using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using Microsoft.SqlServer.Server;

namespace BLL
{
    public class BLL_KhachHang
    {
        public BLL_KhachHang() { }
        public string GetNewID()
        {
            string header = "KH";
            var dataTable = SqlDataAccess.GetDataFromTable("KhachHang");
            if (dataTable.Rows.Count == 0) return header + "001";
            List<int> IDlist = new List<int>();
            foreach (DataRow row in dataTable.Rows)
            {
                IDlist.Add(int.Parse(row[0].ToString().Substring(2)));
            }
            return header + (IDlist.Max() + 1001).ToString().Substring(1);
        }
        public DataTable GetData(string keyword)
        {
            return SqlDataAccess.GetDataFromTable($"KhachHang where HoTen like '%{keyword}%' OR SDT like '%{keyword}%'");
        }
        public DataRow GetRecord(string ID)
        {
            return SqlDataAccess.GetDataFromTable($"KhachHang where MaKH = '{ID}'").Rows[0];
        }
        public void AddRecord(string id, string hoten, string sdt, string diachi)
        {
            string command = $"exec Proc_ThemKH '{id}', N'{hoten}', '{sdt}', N'{diachi}'";
            SqlDataAccess.ExecuteNonQuery(command);
        }
        public void DeleteRecord(string id)
        {
            SqlDataAccess.ExecuteNonQuery($" exec Proc_XoaKH '{id}'");
        }
    }
}
