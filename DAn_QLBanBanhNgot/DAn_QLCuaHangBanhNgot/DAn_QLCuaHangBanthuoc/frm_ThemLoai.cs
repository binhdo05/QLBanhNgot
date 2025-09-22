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
    public partial class frm_ThemLoai : Form
    {
        BLL_type BLL;
        string ID;
        int mode;
        public frm_ThemLoai(int mode = 0, string selectedID = "")
        {
            InitializeComponent();
            BLL = new BLL_type();
            this.mode = mode;
            ID = selectedID;
            if (mode == 1)
            {
                ID = selectedID;
                btn_add.Enabled = false;
                DataRow selectedRow = BLL.GetRecord(ID);
                txt_name.Text = selectedRow[1].ToString();
                txt_des.Text = selectedRow[2].ToString();
            }
            else
            {
                btn_edit.Enabled = false;
                btn_delete.Enabled = false;
            }
        }
        bool Check()
        {
            if (string.IsNullOrWhiteSpace(txt_name.Text))
            {
                MessageBox.Show("Nhập tên loại!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txt_name.Focus();
                return false;
            }
            if (!string.IsNullOrWhiteSpace(txt_des.Text) && txt_des.Text.Length > 100)
            {
                MessageBox.Show("Mô tả không quá 100 ký tự!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txt_des.Focus();
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
                    BLL.AddRecord(txt_name.Text, txt_des.Text);
                    MessageBox.Show("Thêm loại bánh thành công!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Close();
                }
                catch (Exception ex)
                {
                    if (ex.Message.Contains("Medicine type name already exists"))
                    {
                        MessageBox.Show("Loại thuốc đã tồn tại, nhập tên khác!.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        txt_name.Clear();
                        txt_name.Focus();
                    }
                    else
                    {
                        MessageBox.Show($"Lỗi khi thêm loại sản phẩm: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void btn_edit_Click(object sender, EventArgs e)
        {
            if (Check())
            {
                try
                {
                    BLL.UpdateRecord(ID, txt_name.Text, txt_des.Text);
                    MessageBox.Show("Cập nhật thành công!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Close();
                }
                catch (Exception ex)
                {
                    if (ex.Message.Contains("Medicine type name already exists"))
                    {
                        MessageBox.Show("Tên loại đã tồn tại, đặt tên khác!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        txt_name.Focus();
                    }
                    else
                    {
                        MessageBox.Show($"Lỗi cập nhật: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void btn_delete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show($"Bạn có chắc muốn xoá bản ghi có mã '{ID}'?", "Warning!", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                BLL.DeleteRecord(ID);
                MessageBox.Show("Xoá thành công!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Close();
            }
        }
    }
}
