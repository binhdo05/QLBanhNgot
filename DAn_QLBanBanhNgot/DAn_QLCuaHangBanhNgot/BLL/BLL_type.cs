using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;

namespace BLL
{
    public class BLL_type
    {
        public BLL_type() { }
        public DataTable GetData(string keyword)
        {
            return SqlDataAccess.GetDataFromTable(
                $"DanhMuc WHERE TenDM LIKE N'%{keyword}%' OR Note LIKE N'%{keyword}%'"
            );
        }
        public void AddRecord(string tenDm, string note)
        {
            string command = $"EXEC Proc_AddDanhMuc N'{tenDm}', N'{note}'";
            SqlDataAccess.ExecuteNonQuery(command);
        }
        public void UpdateRecord(string id, string tenDm, string note)
        {
            string command = $"EXEC Proc_UpdateDanhMuc '{id}', N'{tenDm}', N'{note}'";
            SqlDataAccess.ExecuteNonQuery(command);
        }
        public void DeleteRecord(string id)
        {
            string command = $"EXEC Proc_DeleteDanhMuc '{id}'";
            SqlDataAccess.ExecuteNonQuery(command);
        }
        public DataRow GetRecord(string id)
        {
            return SqlDataAccess
                .GetDataFromTable($"DanhMuc WHERE MaDM = '{id}'")
                .Rows[0];
        }
    }
}
