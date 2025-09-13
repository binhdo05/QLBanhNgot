using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;

namespace BLL
{
    public class BLL_HoaDonNhap
    {
        public BLL_HoaDonNhap() { }
        public DataTable GetData()
        {
            return SqlDataAccess.GetDataFromTable("HoaDonNhap");
        }
        public DataRow GetRecord(string ID)
        {
            return SqlDataAccess.GetDataFromTable($"HoaDonNhap where MaHDN = '{ID}'").Rows[0];
        }
        public void AddRecord()
        {
            string command = $"exec Proc_ThemHDN";
            SqlDataAccess.ExecuteNonQuery(command);
        }
        public void AddDetails(string id, string maSan, string soluong, string gia)
        {
            string command = $"exec Proc_ThemCTDN '{id}', '{maSan}', '{soluong}', '{gia}'";
            SqlDataAccess.ExecuteNonQuery(command);
        }
        public void DeleteRecord(string id)
        {
            SqlDataAccess.ExecuteNonQuery($"exec Proc_XoaHDN '{id}'");
        }
        public string GetIDHDB()
        {
            return SqlDataAccess.GetDataFromProcedure("SELECT TOP 1 MaHDN \r\nFROM HoaDonNhap \r\nORDER BY MaHDN DESC;\r\n").Rows[0][0].ToString();
        }
        public DataTable GetCTHDN(string id)
        {
            return SqlDataAccess.GetDataFromTable($"v_HoaDonNhap where MaHDN = {id}");
        }
        public DataRow GetRow(string id)
        {
            return SqlDataAccess.GetDataFromTable($"v_HoaDonNhap WHERE MaHDN = {id}").Rows[0];
        }
    }
}
