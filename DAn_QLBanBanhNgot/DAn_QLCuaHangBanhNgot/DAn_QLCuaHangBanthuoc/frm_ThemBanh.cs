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
    public partial class frm_ThemBanh : Form
    {
        BLL_Product BLL;
        string ID;
        int mode;
        string selectedImagePath;
        public frm_ThemBanh(int mode, string selectedID = "")
        {
            InitializeComponent();
            BLL = new BLL_Product();
            this.mode = mode;
            ID = selectedID;
            LoadCategories();
            LoadStatusComboBox(); 

            if (mode == 1)
            {
                btn_add.Enabled = false;
                DataRow selectedRow = BLL.GetRecord(ID);
                if (selectedRow != null)
                {
                    cbo_category.SelectedValue = selectedRow["MaDM"] != DBNull.Value ? selectedRow["MaDM"] : -1;
                    txt_name.Text = selectedRow["TenSP"].ToString();
                    txt_price.Text = selectedRow["Gia"].ToString();
                    txt_description.Text = selectedRow["MoTa"].ToString();
                    cbo_unit.Text = selectedRow["DonVi"].ToString();

                    bool trangThai = Convert.ToBoolean(selectedRow["TrangThai"]);
                    cbo_status.SelectedValue = trangThai ? 1 : 0;

                    selectedImagePath = selectedRow["HinhAnh"].ToString();
                    if (!string.IsNullOrWhiteSpace(selectedImagePath) && File.Exists(selectedImagePath))
                        pic_image.Image = Image.FromFile(selectedImagePath);
                }
                else
                {
                    MessageBox.Show("Không tìm thấy sản phẩm!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Close();
                }
            }
            else
            {
                btn_edit.Enabled = false;
                btn_delete.Enabled = false;
                txt_price.Enabled = false;
            }
        }

        private void LoadCategories()
        {
            DataTable dt = BLL.GetCategories();
            DataRow row = dt.NewRow();
            row["MaDM"] = -1;
            row["TenDM"] = "-- Chọn danh mục --";
            dt.Rows.InsertAt(row, 0);
            cbo_category.DataSource = dt;
            cbo_category.DisplayMember = "TenDM";
            cbo_category.ValueMember = "MaDM";
            cbo_category.SelectedValue = -1;
        }
        private void LoadStatusComboBox()
        {
            DataTable statusTable = new DataTable();
            statusTable.Columns.Add("Value", typeof(int));
            statusTable.Columns.Add("Text", typeof(string));

            statusTable.Rows.Add(1, "Hoạt động");
            statusTable.Rows.Add(0, "Không hoạt động");

            cbo_status.DataSource = statusTable;
            cbo_status.DisplayMember = "Text";
            cbo_status.ValueMember = "Value";
            cbo_status.SelectedValue = 1; 
        }

        bool Check()
        {
            if (string.IsNullOrWhiteSpace(txt_name.Text))
            {
                MessageBox.Show("Vui lòng nhập tên sản phẩm!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txt_name.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(cbo_unit.Text))
            {
                MessageBox.Show("Vui lòng chọn đơn vị!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                cbo_unit.Focus();
                return false;
            }

            if (cbo_status.SelectedValue == null)
            {
                MessageBox.Show("Vui lòng chọn trạng thái!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                cbo_status.Focus();
                return false;
            }

            return true;
        }

        private void btn_add_Click(object sender, EventArgs e)
        {
            if (Check())
            {
                try
                {
                    int? maDM = cbo_category.SelectedValue != null && (int)cbo_category.SelectedValue != -1 ? (int?)cbo_category.SelectedValue : null;

                    if (!maDM.HasValue)
                    {
                        MessageBox.Show("Vui lòng chọn danh mục sản phẩm!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    int status = Convert.ToInt32(cbo_status.SelectedValue);

                    BLL.AddRecord(maDM.Value,
                          txt_name.Text.Trim(),
                          txt_description.Text.Trim(),
                          cbo_unit.Text.Trim(),
                          selectedImagePath ?? "",
                          status);

                    MessageBox.Show("Thêm sản phẩm thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Close();
                }
                catch (Exception ex)
                {
                    if (ex.Message.Contains("Tên sản phẩm đã tồn tại"))
                    {
                        MessageBox.Show("Tên sản phẩm này đã tồn tại! Vui lòng chọn tên khác.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        txt_name.Focus();
                    }
                    else
                    {
                        MessageBox.Show("Lỗi khi thêm: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void btn_edit_Click(object sender, EventArgs e)
        {
            if (Check())
            {
                if (!decimal.TryParse(txt_price.Text, out decimal price) || price < 0)
                {
                    MessageBox.Show("Vui lòng nhập giá hợp lệ!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txt_price.Focus();
                    return;
                }

                try
                {
                    int? idType = cbo_category.SelectedValue != null && (int)cbo_category.SelectedValue != -1 ? (int?)cbo_category.SelectedValue : null;
                    int status = Convert.ToInt32(cbo_status.SelectedValue); // Sử dụng SelectedValue thay vì Text

                    BLL.UpdateRecord(
                            ID,
                            idType,
                            txt_name.Text,
                            price,
                            txt_description.Text,
                            cbo_unit.Text,
                            selectedImagePath ?? "",
                            status.ToString() // Convert về string nếu UpdateRecord cần string
                        );
                    MessageBox.Show("Cập nhật thông tin thành công!!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Close();
                }
                catch (Exception ex)
                {
                    if (ex.Message.Contains("Tên sản phẩm đã tồn tại"))
                    {
                        MessageBox.Show("Tên sản phẩm này đã tồn tại! Vui lòng chọn tên khác.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        txt_name.Focus();
                    }
                    else
                    {
                        MessageBox.Show($"Lỗi khi cập nhật sản phẩm: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void btn_delete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show($"Bạn có chắc chắn muốn xóa bánh có mã '{ID}' không?", "Cảnh báo!", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                BLL.DeleteRecord(ID);
                MessageBox.Show("Xóa bánh thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Close();
            }
        }

        private void pic_image_DoubleClick(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    selectedImagePath = ofd.FileName;
                    pic_image.Image = Image.FromFile(selectedImagePath);
                }
            }
        }
    }
}