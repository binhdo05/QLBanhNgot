using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;

namespace BLL
{
    public class BLL_sale
    {
        public BLL_sale() { }
        public DataTable GetDataSale(string keyword)
        {
            return SqlDataAccess.GetDataFromTable($"View_Sale_Invoice_Details_2 where name_staff like N'%{keyword}%' OR name_customer like N'%{keyword}%' OR status like N'%{keyword}%'");
        }
        public DataTable GetCustomer()
        {
            return SqlDataAccess.GetDataFromTable("KhachHang");
        }

        public DataTable GetStaff()
        {
            return SqlDataAccess.GetDataFromTable("NhanVien");
        }
        public DataTable GetProduct()
        {
            return SqlDataAccess.GetDataFromTable("SanPham WHERE (SoLuong > 0 AND TrangThai = 1 AND Gia > 0) AND SoLuong IS NOT NULL AND Gia IS NOT NULL");
        }

        public string GetIDHDB()
        {
            return SqlDataAccess.GetDataFromProcedure("SELECT TOP 1 MaHDB \r\nFROM HoaDonBan \r\nORDER BY MaHDB DESC;\r\n").Rows[0][0].ToString();
        }
        public void AddRecord(string idStaff, string idCus)
        {
            string command = $"exec Proc_AddSaleInvoice '{idStaff}', '{idCus}', '{DateTime.Now.ToString("MM/dd/yyyy")}', '{"Pending"}'";
            SqlDataAccess.ExecuteNonQuery(command);
        }
        public void AddDetails(string id, string id_medicine, string quan, string price)
        {
            string command = $"exec Proc_AddSaleDetail '{id}', '{id_medicine}', '{quan}', '{price}'";
            SqlDataAccess.ExecuteNonQuery(command);
        }
        public void DeleteSale(string id)
        {
            string sqlCommand = $"EXEC Proc_DeleteSaleInvoice @MaHDB = {id}";
            SqlDataAccess.ExecuteNonQuery(sqlCommand);
        }
        public DataTable GetSaleDetail(string id)
        {
            return SqlDataAccess.GetDataFromTable($"View_Sale_Invoice_Details WHERE id_sale = {id}");
        }
        public DataRow GetRow(string id)
        {
            return SqlDataAccess.GetDataFromTable($"View_Sale_Invoice_Details WHERE id_sale = {id}").Rows[0];
        }
        public void Confirm(string id, string status)
        {
            SqlDataAccess.ExecuteNonQuery($"exec Proc_UpdateSaleInvoiceStatus '{id}', N'{status}'");
        }
    }
}
