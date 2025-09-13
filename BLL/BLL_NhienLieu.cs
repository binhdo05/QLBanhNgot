using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;

namespace BLL
{
    public class BLL_NhienLieu
    {
        public BLL_NhienLieu() { }
        public string GetNewID()
        {
            string header = "NL";
            var dataTable = SqlDataAccess.GetDataFromTable("NhienLieu");
            if (dataTable.Rows.Count == 0) return header + "001";
            List<int> IDlist = new List<int>();
            foreach (DataRow row in dataTable.Rows)
            {
                IDlist.Add(int.Parse(row[0].ToString().Substring(2)));
            }
            return header + (IDlist.Max() + 1001).ToString().Substring(1);
        }
        public DataTable GetData()
        {
            return SqlDataAccess.GetDataFromTable("NhienLieu");
        }
        public DataRow GetRecord(string ID)
        {
            return SqlDataAccess.GetDataFromTable($"NhienLieu where MaNL = '{ID}'").Rows[0];
        }
        public void AddRecord(string id, string hoten, string mota)
        {
            string command = $"exec Proc_ThemNL '{id}', N'{hoten}', N'{mota}'";
            SqlDataAccess.ExecuteNonQuery(command);
        }
        public void DeleteRecord(string id)
        {
            SqlDataAccess.ExecuteNonQuery($" exec Proc_XoaNL '{id}'");
        }
    }
}
