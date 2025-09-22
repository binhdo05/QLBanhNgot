using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;

namespace BLL
{
    public class BLL_purchase
    {
        public BLL_purchase() { }
        public DataTable GetDataPurchase(string keyword)
        {
            return SqlDataAccess.GetDataFromTable($"View_Purchase_Invoice_Details where name_supplier like N'%{keyword}%' OR name_staff like N'%{keyword}%'");
        }
        public DataTable GetSupplier()
        {
            return SqlDataAccess.GetDataFromTable("NhaCungCap");
        }
        public DataTable GetStaff()
        {
            return SqlDataAccess.GetDataFromTable("NhanVien");
        }
        public DataTable GetProduct()
        {
            return SqlDataAccess.GetDataFromTable("SanPham");
        }
        public void CreatePurchaseInvoice(DateTime dateCreate, int idSupplier, int idStaff, decimal totalAmount)
        {
            string command = $"exec Proc_AddHoaDonNhap '{idStaff}', '{idSupplier}', '{dateCreate.ToString("MM/dd/yyyy")}', '{totalAmount}'";
            SqlDataAccess.ExecuteNonQuery(command);
        }

        public void CreatePurchaseDetail(int idPurchase, string idBatch)
        {
            string sqlCommand = $"EXEC Insert_ChiTietDonNhap @MaHDN = {idPurchase}, @MaLo = {idBatch}";
            SqlDataAccess.ExecuteNonQuery(sqlCommand);
        }
        public void DeletePurchaseInvoice(int idPurchase)
        {
            string sqlCommand = $"EXEC Proc_DeleteHoaDonNhap @MaHDN = {idPurchase}";
            SqlDataAccess.ExecuteNonQuery(sqlCommand);
        }

        public DataTable GetDataBatchID(string id)
        {
            return SqlDataAccess.GetDataFromTable($"View_Batch_Purchase_Details where id_purchase = '{id}'");
        }
        public DataRow GetRow(string id)
        {
            return SqlDataAccess.GetDataFromTable($"View_Purchase_Invoice_Details where id = {id}").Rows[0];
        }
        public string GetIDHDN()
        {
            return SqlDataAccess.GetDataFromProcedure("SELECT TOP 1 MaHDN \r\nFROM HoaDonNhap \r\nORDER BY MaHDN DESC;\r\n").Rows[0][0].ToString();
        }
    }   
}
