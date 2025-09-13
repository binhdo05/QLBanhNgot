using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using Microsoft.SqlServer.Server;

namespace BLL
{
    public class BLL_ThongKe
    {
        public BLL_ThongKe() { }
        public DataTable TKDoanhThu(int? month, int year)
        {
            string procedureCall = month.HasValue
                ? $"exec ThongKeDoanhThuTheoThangNam {month}, {year}"
                : $"exec ThongKeDoanhThuTheoThangNam NULL, {year}";
            return SqlDataAccess.GetDataFromProcedure(procedureCall);
        }
        public DataTable TKHD_Now()
        {
            return SqlDataAccess.GetDataFromTable("V_HoaDon_HienTai");
        }
        public DataTable TK_SPBanChay()
        {
            return SqlDataAccess.GetDataFromProcedure("SELECT * FROM V_SanPhamBanChay ORDER BY TongSoLuongBan DESC"); 
        }
        public DataTable TKSoLuong()
        {
            return SqlDataAccess.GetDataFromProcedure("exec ThongKeSoLuongKhachHangHD");
        }
    }
}
