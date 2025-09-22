using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BLL;

namespace DAn_QLCuaHangBanthuoc
{
    public partial class frm_DonNhap : Form
    {
        private readonly frm_Main _mainForm; // Save reference to frm_Main
        BLL_batch bll_2;
        BLL_purchase bll_1;
        private DataGridViewToExcelExporter excelExporter;
        public frm_DonNhap(frm_Main mainForm)
        {
            InitializeComponent();
            _mainForm = mainForm; // Save frm_Main
            bll_1 = new BLL_purchase();
            bll_2 = new BLL_batch();
            excelExporter = new DataGridViewToExcelExporter();
            loadData1();
            loadData2();
            btn_delete.Visible = false;
        }
        void loadData1()
        {
            dgv_data.Rows.Clear();
            DataTable dataTable = bll_1.GetDataPurchase(txtSearch.Text);
            foreach (DataRow row in dataTable.Rows)
            {
                dgv_data.Rows.Add(row[0], Convert.ToDateTime(row[1]).ToString("dd/MM/yyyy"), row[2], row[3], row[4]);
            }
        }
        void loadData2()
        {
            dgv_data2.Rows.Clear();
            DataTable dataTable2 = bll_2.GetDataBatch(txt_seach2.Text);
            if (txt_seach2.Text == "")
            {
                foreach (DataRow row in dataTable2.Rows)
                {
                    dgv_data2.Rows.Add(row[0], row[1], row[2], row[3], row[4], Convert.ToDateTime(row[5]).ToString("dd/MM/yyyy"), Convert.ToDateTime(row[6]).ToString("dd/MM/yyyy"), row["SlThuc"], row["Loi"]);
                }
            }
            else
            {
                foreach (DataRow row in dataTable2.Rows)
                {
                    dgv_data2.Rows.Add(row[0], row[1], row[2], row[3], row[4], Convert.ToDateTime(row[5]).ToString("dd/MM/yyyy"), Convert.ToDateTime(row[6]).ToString("dd/MM/yyyy"), row["SlThuc"], row["Loi"]);
                }
            }
        }
        private void btn_add_Click(object sender, EventArgs e)
        {
            _mainForm.container(new frm_ThemDN(_mainForm));
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            loadData1();
            if (txtSearch.Text.Length < 1)
            {
                loadData1();
            }
            btn_delete.Visible = true;
            btn_add.Visible = false;
        }

        private void txt_seach24_TextChanged(object sender, EventArgs e)
        {
            loadData2();
            if (txt_seach2.Text.Length < 1)
            {
                loadData2();
            }
            btn_delete.Visible = true;
            btn_add.Visible = false;
        }

        private void dgv_data_DoubleClick(object sender, EventArgs e)
        {
            if (dgv_data.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = dgv_data.SelectedRows[0];
                string idPurchase = selectedRow.Cells[0].Value.ToString();
                _mainForm.container(new frm_InDN(_mainForm, idPurchase));
            }
        }

        private void dgv_data2_DoubleClick(object sender, EventArgs e)
        {
            if (dgv_data.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = dgv_data2.SelectedRows[0];
                string idPurchase = selectedRow.Cells[0].Value.ToString();
                _mainForm.container(new frm_SuaLo(_mainForm, idPurchase));
            }
        }
        private void btn_delete_Click(object sender, EventArgs e)
        {
            if (dgv_data.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = dgv_data.SelectedRows[0];
                int idPurchase = Convert.ToInt32(selectedRow.Cells[0].Value);
                if (idPurchase > 0)
                {
                    DialogResult result = MessageBox.Show(
                    $"Bạn có chắc chắn muốn xóa đơn nhập với mã {idPurchase}?",
                    "Thông báo",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                     );
                    if (result == DialogResult.No)
                    {
                        return;
                    }
                    bll_1.DeletePurchaseInvoice(idPurchase);
                    MessageBox.Show("Xóa đơn nhập thành công!!!", "Thông tin", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    loadData1();
                    loadData2();
                    btn_delete.Visible = false;
                    btn_add.Visible = true;
                }
            }
        }
        private void frm_purchase_Click(object sender, EventArgs e)
        {
            btn_delete.Visible = false;
            btn_add.Visible = true;
        }
        private void dgv_data_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            btn_delete.Visible = true;
            btn_add.Visible = false;
        }


        private void btn_excel_Click(object sender, EventArgs e)
        {
            bool hasPurchaseData = dgv_data != null && dgv_data.Rows.Count > 0;
            bool hasBatchData = dgv_data2 != null && dgv_data2.Rows.Count > 0;

            if (!hasPurchaseData && !hasBatchData)
            {
                MessageBox.Show("Không có dữ liệu để xuất trong cả Hóa đơn mua hàng và Lô hàng!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "Excel files (*.xlsx)|*.xlsx",
                Title = "Select location to save Excel file"
            };

            if (hasPurchaseData)
            {
                saveFileDialog.FileName = $"PurchaseInvoices_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    excelExporter.ExportToExcelWithClosedXML(dgv_data, saveFileDialog.FileName, "PurchaseInvoices");
                }
                else
                {
                    return;
                }
            }
            if (hasBatchData)
            {
                saveFileDialog.FileName = $"Batches_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    excelExporter.ExportToExcelWithClosedXML(dgv_data2, saveFileDialog.FileName, "Batches");
                }
                else
                {
                    return;
                }
            }
        }
    }
}
