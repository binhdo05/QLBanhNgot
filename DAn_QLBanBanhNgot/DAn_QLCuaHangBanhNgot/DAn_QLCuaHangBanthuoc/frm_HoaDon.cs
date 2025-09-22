using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BLL;

namespace DAn_QLCuaHangBanthuoc
{
    public partial class frm_HoaDon : Form
    {
        private readonly frm_Main _mainForm;
        private DataGridViewToExcelExporter excelExporter;
        BLL_sale bll;
        public frm_HoaDon(frm_Main mainForm)
        {
            InitializeComponent();
            _mainForm = mainForm;
            bll = new BLL_sale();
            excelExporter = new DataGridViewToExcelExporter();
            loadData();
            btn_delete.Visible = false;
        }
        void loadData()
        {
            dgv_data.Rows.Clear();
            DataTable dataTable = bll.GetDataSale(txtSearch.Text);
            foreach (DataRow row in dataTable.Rows)
            {
                dgv_data.Rows.Add(row[0], row[1], row[2], Convert.ToDateTime(row[3]).ToString("dd/MM/yyyy"), row[5], row[4]);
            }
        }
        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            loadData();
            if (txtSearch.Text.Length < 1)
            {
                loadData();
            }
        }

        private void btn_add_Click(object sender, EventArgs e)
        {
            _mainForm.container(new frm_ThemHD(_mainForm));
        }

        private void btn_delete_Click(object sender, EventArgs e)
        {
            if (dgv_data.SelectedRows.Count > 0)
            {
                string idSale = dgv_data.SelectedRows[0].Cells[0].Value.ToString();
                if (MessageBox.Show($"Bạn có chắc chắn muốn xóa hóa đơn với mã {idSale}?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    try
                    {
                        bll.DeleteSale(idSale);
                        MessageBox.Show("Xóa đơn bán thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        loadData();
                    }
                    catch (SqlException sqlEx)
                    {
                        MessageBox.Show($"Lỗi: {sqlEx.Message}", "Lỗi SQL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Có lỗi xảy ra: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một hóa đơn để xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void dgv_data_Click(object sender, EventArgs e)
        {
            btn_delete.Visible = true;
            btn_add.Visible = false;
        }

        private void dgv_data_DoubleClick(object sender, EventArgs e)
        {
            if (dgv_data.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = dgv_data.SelectedRows[0];
                string id = selectedRow.Cells[0].Value.ToString();
                _mainForm.container(new frm_InHD(_mainForm, id));
            }
        }

        private void frm_sale_Click(object sender, EventArgs e)
        {
            btn_delete.Visible = false;
            btn_add.Visible = true;
        }

        private void btn_excel_Click(object sender, EventArgs e)
        {
            if (dgv_data.Rows.Count == 0)
            {
                MessageBox.Show("Không có dữ liệu để xuất!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "Excel files (*.xlsx)|*.xlsx",
                Title = "Select location to save Excel file",
                FileName = $"SaleList_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx"
            };

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                excelExporter.ExportToExcelWithClosedXML(dgv_data, saveFileDialog.FileName, "SaleList");
            }
        }
    }
}