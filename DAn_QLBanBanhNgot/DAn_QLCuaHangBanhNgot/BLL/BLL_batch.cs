using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;

namespace BLL
{
    public class BLL_batch
    {
        public BLL_batch() { }
        public DataTable GetDataBatch(string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
            {
                return SqlDataAccess.GetDataFromTable("View_Batch_Details");
            }
            else
            {
                return SqlDataAccess.GetDataFromTable($"View_LoSanPham_Details where name_product like N'%{keyword}%' OR status like N'%{keyword}%'");
            }
        }
        public void CreateBatch(int idsp, int quantityInBatch, decimal entryPrice, DateTime manufacturingDate, DateTime expiryDate, string status)
        {
            SqlDataAccess.ExecuteNonQuery($"EXEC Proc_AddBatch '{idsp}', '{quantityInBatch}', '{entryPrice}', '{manufacturingDate.ToString("MM/dd/yyyy")}', '{expiryDate.ToString("MM/dd/yyyy")}', N'{status}'");
        }
        public string getIDbathNew()
        {
            return SqlDataAccess.GetDataFromProcedure("SELECT TOP 1 MaLo \r\nFROM LoSanPham \r\nORDER BY MaLo DESC;\r\n").Rows[0][0].ToString();
        }
        public DataRow GetRow(string id)
        {
            return SqlDataAccess.GetDataFromTable($"View_Batch_With_Medicine where id_batch = {id}").Rows[0];
        }
        public void UpdateBatch(int idBatch, int quantityInBatch, decimal entryPrice, DateTime manufacturingDate, DateTime expiryDate, string status, int quantityShortage, string note)
        {
            string command = $@"
            EXEC Proc_UpdateBatch 
                @MaLo = {idBatch}, 
                @SoLuongNhap = {quantityInBatch}, 
                @GiaNhap = {entryPrice}, 
                @NSX = '{manufacturingDate}', 
                @HSD = '{expiryDate}', 
                @TrangThai = N'{status}', 
                @SoLuongLoi = {quantityShortage}, 
                @GhiChu = N'{note}'";
            SqlDataAccess.ExecuteNonQuery(command);
        }
        public void DeleteBatch(string id)
        {
            SqlDataAccess.ExecuteNonQuery($"Exec Proc_DeleteBatch '{id}'");
        }
    }
}