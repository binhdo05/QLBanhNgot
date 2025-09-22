using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;

namespace BLL
{
    public class BLL_supplier
    {
        public BLL_supplier() { }

        public void AddRecord(string name, string phone, string gmail, string address)
        {
            string command = $"EXEC Proc_AddSupplier N'{name}', '{phone}', '{gmail}', N'{address}'";
            SqlDataAccess.ExecuteNonQuery(command);
        }

        public void UpdateRecord(string id, string name, string phone, string gmail, string address)
        {
            string command = $"EXEC Proc_UpdateSupplier '{id}', N'{name}', '{phone}', '{gmail}', N'{address}'";
            SqlDataAccess.ExecuteNonQuery(command);
        }

        public void DeleteRecord(string id)
        {
            string command = $"EXEC Proc_DeleteSupplier '{id}'";
            SqlDataAccess.ExecuteNonQuery(command);
        }

        public DataTable GetData(string keyword)
        {
            return SqlDataAccess.GetDataFromTable(
                $"NhaCungCap WHERE TenNCC LIKE N'%{keyword}%' OR DiaChi LIKE N'%{keyword}%' OR SDT LIKE '{keyword}'"
            );
        }

        public DataRow GetRecord(string ID)
        {
            return SqlDataAccess.GetDataFromTable($"NhaCungCap WHERE MaNCC = '{ID}'").Rows[0];
        }

    }
}
