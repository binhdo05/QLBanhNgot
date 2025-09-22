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
    public partial class frm_NhanVien : Form
    {
        BLL_staff BLL;
        private DataGridViewToExcelExporter excelExporter;
        public frm_NhanVien()
        {
            InitializeComponent();
            BLL = new BLL_staff();
            excelExporter = new DataGridViewToExcelExporter();
            LoadData();
        }
        void LoadData()
        {
            dgv_data.Rows.Clear();
            DataTable dataTable = BLL.GetData(txtSearch.Text);
            foreach (DataRow row in dataTable.Rows)
            {
                dgv_data.Rows.Add(row[0], row[1], row[2], row[5], row[4], row[3], Convert.ToDateTime(row[6]).ToString("dd/MM/yyyy"), row[7], row[8], row[9], row[10].ToString() == "True" ? "Còn làm việc" : "Đã nghỉ việc");
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

        private void dgv_data_DoubleClick(object sender, EventArgs e)
        {
            if (dgv_data.Rows.Count > 0)
            {
                string selectedID = dgv_data.SelectedRows[0].Cells[0].Value.ToString();
                frm_ThemNV form = new frm_ThemNV(1, selectedID);
                form.ShowDialog();
                LoadData();
            }
        }

        private void btn_add_Click(object sender, EventArgs e)
        {
            frm_ThemNV frm = new frm_ThemNV(0);
            frm.ShowDialog();
            LoadData();
        }

        private void btn_excel_Click(object sender, EventArgs e)
        {
            if (dgv_data.Rows.Count == 0)
            {
                MessageBox.Show("No data to export!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "Excel files (*.xlsx)|*.xlsx",
                Title = "Select location to save Excel file",
                FileName = $"StaffList_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx"
            };

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                excelExporter.ExportToExcelWithClosedXML(dgv_data, saveFileDialog.FileName, "StaffList");
            }
        }
    }
}
