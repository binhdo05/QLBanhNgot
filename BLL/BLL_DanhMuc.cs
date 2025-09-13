using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;

namespace BLL
{
    public class BLL_DanhMuc
    {
        public BLL_DanhMuc() { }
        public string GetNewID()
        {
            string header = "DM";
            var dataTable = SqlDataAccess.GetDataFromTable("DanhMuc");
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
            return SqlDataAccess.GetDataFromTable($"DanhMuc where TenDM like '%{keyword}%' OR MoTa like '%{keyword}%'");
        }
        public DataRow GetRecord(string ID)
        {
            return SqlDataAccess.GetDataFromTable($"DanhMuc where MaDM = '{ID}'").Rows[0];
        }
        public void AddRecord(string id, string ten, string mota, string soluong, string giaban)
        {
            string command = $"exec Proc_ThemDM '{id}', N'{ten}', N'{mota}', '{soluong}', '{giaban}'";
            SqlDataAccess.ExecuteNonQuery(command);
        }
        public void DeleteRecord(string id)
        {
            SqlDataAccess.ExecuteNonQuery($"exec Proc_XoaDM '{id}'");
        }
    }
}
