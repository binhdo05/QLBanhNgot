using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BLL;

namespace DAn_QLCuaHangBanthuoc
{
    public partial class frm_LoaiSP : Form
    {
        BLL_type BLL;
        private DataGridViewToExcelExporter excelExporter;
        public frm_LoaiSP()
        {
            InitializeComponent();
            excelExporter = new DataGridViewToExcelExporter();
            BLL = new BLL_type();
            LoadData();
        }
        void LoadData()
        {
            dgv_data.Rows.Clear();
            DataTable dataTable = BLL.GetData(txtSearch.Text);
            foreach (DataRow row in dataTable.Rows)
            {
                dgv_data.Rows.Add(row[0], row[1], row[2]);
            }
        }
        private void btn_add_Click(object sender, EventArgs e)
        {
            frm_ThemLoai frm = new frm_ThemLoai(0);
            frm.ShowDialog();
            LoadData();
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            LoadData();
            if (txtSearch.Text.Length < 1)
            {
                LoadData();
            }
        }

        private void dgv_data_DoubleClick(object sender, EventArgs e)
        {
            if (dgv_data.Rows.Count > 0)
            {
                string selectedID = dgv_data.SelectedRows[0].Cells[0].Value.ToString();
                frm_ThemLoai frm = new frm_ThemLoai(1, selectedID);
                frm.ShowDialog();
                LoadData();
            }
        }

        private void btn_excel_Click(object sender, EventArgs e)
        {
            if (dgv_data.Rows.Count == 0)
            {
                MessageBox.Show("Không có dữ liệu xuất!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "Excel files (*.xlsx)|*.xlsx",
                Title = "Select location to save Excel file",
                FileName = $"DanhMucSanPham_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx"
            };

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                excelExporter.ExportToExcelWithClosedXML(dgv_data, saveFileDialog.FileName, "DanhMucSanPham");
            }
        }
    }
}
