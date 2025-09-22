using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;

namespace BLL
{
    public class BLL_Product
    {
        public DataTable GetData(string keyword)
        {
            string cleanedKeyword = keyword?.Trim().Replace("'", "''") ?? "";
            string query = $"View_Full_Product_Info WHERE [name] LIKE N'%{cleanedKeyword}%' OR [category_name] LIKE N'%{cleanedKeyword}%'";
            DataTable result = SqlDataAccess.GetDataFromTable(query);
            return result;
        }

        public DataRow GetRecord(string id)
        {
            DataTable dt = SqlDataAccess.GetDataFromTable($"SanPham WHERE MaSP = '{id}'");
            return dt.Rows.Count > 0 ? dt.Rows[0] : null;
        }

        public DataTable GetCategories()
        {
            return SqlDataAccess.GetDataFromTable("DanhMuc");
        }

        public void AddRecord(int? idCategory, string nameProduct, string description, string unit, string imagePath, int isActive)
        {
            string idCategoryValue = idCategory.HasValue ? $"{idCategory}" : "NULL";
            string command = $@"
                EXEC Proc_AddSanPham 
                    {idCategoryValue}, 
                    N'{nameProduct.Replace("'", "''")}', 
                    N'{description.Replace("'", "''")}', 
                    N'{unit.Replace("'", "''")}', 
                    N'{imagePath.Replace("'", "''")}', 
                    {isActive}";

            SqlDataAccess.ExecuteNonQuery(command);
        }


        public void UpdateRecord(string idProduct, int? idCategory, string nameProduct, decimal price, string description, string unit, string images, string isActive)
        {
            string idCategoryValue = idCategory.HasValue ? $"{idCategory}" : "NULL";

            string command = $@"
                EXEC Proc_UpdateSanPham 
                    {idProduct}, 
                    {idCategoryValue}, 
                    N'{nameProduct.Replace("'", "''")}', 
                    {price}, 
                    N'{description.Replace("'", "''")}', 
                    N'{unit.Replace("'", "''")}', 
                    N'{images.Replace("'", "''")}', 
                    {isActive}";

            SqlDataAccess.ExecuteNonQuery(command);
        }


        public void DeleteRecord(string id)
        {
            string command = $"EXEC Proc_DeleteSanPham {id}";
            SqlDataAccess.ExecuteNonQuery(command);
        }
    }

}
