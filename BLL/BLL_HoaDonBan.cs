using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;

namespace BLL
{
    public class BLL_HoaDonBan
    {
        public BLL_HoaDonBan() { }
        public DataTable GetData()
        {
            return SqlDataAccess.GetDataFromTable($"v_HoaDonBan");
        }
        public DataTable GetDataSearch(string key)
        {
            return SqlDataAccess.GetDataFromTableSearch("v_HoaDonBan", "*", key);
        }

        public DataRow GetRow(string id)
        {
            return SqlDataAccess.GetDataFromTable($"v_HoaDonBan WHERE MaHDB = {id}").Rows[0];
        }
        public DataTable GetCTHDB(string id)
        {
            return SqlDataAccess.GetDataFromTable($"v_CTHDB WHERE [Mã HDB] = {id}");
        }
        public void AddRecord(string idKH, string tt)
        {
            string command = $"exec Proc_ThemHDB '{idKH}', N'{tt}'";
            SqlDataAccess.ExecuteNonQuery(command);
        }
        public void AddDetails(string id, string maSan, string soluong)
        {
            string command = $"exec Proc_ThemCTHDB '{id}', '{maSan}', '{soluong}'";
            SqlDataAccess.ExecuteNonQuery(command);
        }
        public void Confirm(string id, string status)
        {
            SqlDataAccess.ExecuteNonQuery($"exec prc_CapNhatTrangThaiHoaDon '{id}', N'{status}'");
        }
        public void DeleteRecord(string id)
        {
            SqlDataAccess.ExecuteNonQuery($"delete from HoaDonBan where MaHDB = '{id}'");
        }
        public DataTable GetKhachHang()
        {
            return SqlDataAccess.GetDataFromTable("KhachHang");
        }

        public DataTable GetDanhMuc()
        {
            return SqlDataAccess.GetDataFromTable("DanhMuc where SoLuong > 0 and GiaBan > 0");
        }

        public string GetIDHDB()
        {
            return SqlDataAccess.GetDataFromProcedure("SELECT TOP 1 MaHDB \r\nFROM HoaDonBan \r\nORDER BY MaHDB DESC;\r\n").Rows[0][0].ToString();
        }
    }
}
