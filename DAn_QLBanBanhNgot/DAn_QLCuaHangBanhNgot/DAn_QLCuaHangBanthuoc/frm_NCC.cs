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
    public partial class frm_NCC : Form
    {
        BLL_supplier BLL;
        private DataGridViewToExcelExporter excelExporter;
        public frm_NCC()
        {
            InitializeComponent();
            BLL = new BLL_supplier();
            excelExporter = new DataGridViewToExcelExporter();
            LoadData();
        }
        void LoadData()
        {
            dgv_data.Rows.Clear();
            DataTable dataTable = BLL.GetData(txtSearch.Text);
            foreach (DataRow row in dataTable.Rows)
            {
                dgv_data.Rows.Add(row[0], row[1], row[2], row[3], row[4]);
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            LoadData();
            if (txtSearch.Text.Length < 1)
            {
                LoadData();
            }
        }

        private void btn_add_Click(object sender, EventArgs e)
        {
            frm_ThemNCC frm = new frm_ThemNCC(0);
            frm.ShowDialog();
            LoadData();
        }

        private void dgv_data_DoubleClick(object sender, EventArgs e)
        {
            if (dgv_data.Rows.Count > 0)
            {
                string selectedID = dgv_data.SelectedRows[0].Cells[0].Value.ToString();
                frm_ThemNCC form = new frm_ThemNCC(1, selectedID);
                form.ShowDialog();
                LoadData();
            }
        }

        private void btn_excel_Click(object sender, EventArgs e)
        {
            if (dgv_data.Rows.Count == 0)
            {
                MessageBox.Show("Không có dữ liệu để xuất!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "Excel files (*.xlsx)|*.xlsx",
                Title = "Select location to save Excel file",
                FileName = $"SupplierList_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx"
            };

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                excelExporter.ExportToExcelWithClosedXML(dgv_data, saveFileDialog.FileName, "SuppierList");
            }
        }
    }
}
