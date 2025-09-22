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
using System.IO;

namespace DAn_QLCuaHangBanthuoc
{
    public partial class frm_SanPham : Form
    {
        private readonly frm_Main _mainForm;
        private DataGridViewToExcelExporter excelExporter;
        BLL_Product BLL;

        int mode;

        public frm_SanPham(frm_Main mainForm, int mode)
        {
            InitializeComponent();
            _mainForm = mainForm;
            BLL = new BLL_Product();
            excelExporter = new DataGridViewToExcelExporter();
            SetupDataGridView();
            LoadData();

            this.mode = mode;
            if (mode == 1)
            {
                btn_excel.Visible = false;
                btn_type.Visible = false;
                btn_add.Visible = false;
            }
        }

        void LoadData()
        {
            try
            {
                dgv_data.Rows.Clear();
                DataTable dataTable = BLL.GetData(txt_search.Text);
                if (dataTable == null || dataTable.Rows.Count == 0)
                {
                    System.Diagnostics.Debug.WriteLine("LoadData: No data returned.");
                    return;
                }

                foreach (DataRow row in dataTable.Rows)
                {
                    Image image = null;
                    string imagePath = row["image_path"]?.ToString();
                    if (!string.IsNullOrEmpty(imagePath) && File.Exists(imagePath))
                    {
                        try
                        {
                            image = Image.FromFile(imagePath);
                        }
                        catch
                        {
                            image = null;
                        }
                    }

                    dgv_data.Rows.Add(
                        row["id"],
                        row["name"],
                        row["category_name"],
                        row["manu"] != DBNull.Value ? Convert.ToDateTime(row["manu"]).ToString("dd/MM/yyyy") : "",
                        row["ex"] != DBNull.Value ? Convert.ToDateTime(row["ex"]).ToString("dd/MM/yyyy") : "",
                        row["quantity"],
                        row["price"],
                        row["dv"],
                        row["mt"],
                        row["is_active"].ToString() == "True" ? "Còn kinh doanh" : "Ngừng kinh doanh",
                        image
                    );
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void SetupDataGridView()
        {
            dgv_data.Columns.Clear();
            dgv_data.Columns.Add(new DataGridViewTextBoxColumn { Name = "id", HeaderText = "ID" });
            dgv_data.Columns.Add(new DataGridViewTextBoxColumn { Name = "name", HeaderText = "Tên sản phẩm" });
            dgv_data.Columns.Add(new DataGridViewTextBoxColumn { Name = "category_name", HeaderText = "Danh mục" });
            dgv_data.Columns.Add(new DataGridViewTextBoxColumn { Name = "manu", HeaderText = "NSX" });
            dgv_data.Columns.Add(new DataGridViewTextBoxColumn { Name = "ex", HeaderText = "HSD" });
            dgv_data.Columns.Add(new DataGridViewTextBoxColumn { Name = "quantity", HeaderText = "Số lượng" });
            dgv_data.Columns.Add(new DataGridViewTextBoxColumn { Name = "price", HeaderText = "Giá bán" });
            dgv_data.Columns.Add(new DataGridViewTextBoxColumn { Name = "dv", HeaderText = "Đơn vị" });
            dgv_data.Columns.Add(new DataGridViewTextBoxColumn { Name = "mt", HeaderText = "Mô tả" });
            dgv_data.Columns.Add(new DataGridViewTextBoxColumn { Name = "is_active", HeaderText = "Trạng thái" });
            var imageColumn = new DataGridViewImageColumn
            {
                Name = "image_path",
                HeaderText = "Ảnh",
                ImageLayout = DataGridViewImageCellLayout.Zoom
            };
            dgv_data.Columns.Add(imageColumn);
            dgv_data.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }
        private void btn_add_Click(object sender, EventArgs e)
        {
            frm_ThemBanh frm = new frm_ThemBanh(0);
            frm.ShowDialog();
            LoadData();
        }

        private void btn_type_Click(object sender, EventArgs e)
        {
            _mainForm.container(new frm_LoaiSP());
        }

        private void txt_search_TextChanged(object sender, EventArgs e)
        {
            LoadData();
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
                FileName = $"ProductList_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx"
            };

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                excelExporter.ExportToExcelWithClosedXML(dgv_data, saveFileDialog.FileName, "ProductList");
            }
        }

        private void dgv_data_CellClick_1(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgv_data.Rows[e.RowIndex];

                lbl_ten.Text = row.Cells["name"].Value?.ToString() ?? "";
                lbl_dm.Text = row.Cells["category_name"].Value?.ToString() ?? "";
                string manuStr = row.Cells["manu"].Value?.ToString();
                string exStr = row.Cells["ex"].Value?.ToString();
                /*
                lbl_nsx.Text = DateTime.TryParse(manuStr, out DateTime manuDate)
                    ? manuDate.ToString("dd/MM/yyyy")  
                    : "";

                lbl_hsd.Text = DateTime.TryParse(exStr, out DateTime exDate)
                    ? exDate.ToString("dd/MM/yyyy") 
                    : "";
                */
                lbl_nsx.Text = manuStr;
                lbl_hsd.Text = exStr;

                lbl_soluong.Text = row.Cells["quantity"].Value?.ToString() ?? "";
                lbl_gia.Text = row.Cells["price"].Value?.ToString() ?? "";
                lbl_tt.Text = row.Cells["is_active"].Value?.ToString() ?? "";

                if (row.Cells["image_path"].Value is Image img)
                {
                    pic_image.Image = img;
                }
                else
                {
                    pic_image.Image = null;
                }
            }
        }

        private void dgv_data_DoubleClick_1(object sender, EventArgs e)
        {
            if (mode == 1) return;

            if (dgv_data.SelectedRows.Count > 0)
            {
                string id = dgv_data.SelectedRows[0].Cells["id"].Value?.ToString();
                if (!string.IsNullOrEmpty(id))
                {
                    frm_ThemBanh frm = new frm_ThemBanh(1, id);
                    frm.FormClosed += (s, args) => LoadData();
                    frm.ShowDialog();
                }
            }
        }
    }
}