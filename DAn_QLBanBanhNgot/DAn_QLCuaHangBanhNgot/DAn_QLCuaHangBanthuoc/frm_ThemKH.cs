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
using iTextSharp.text.pdf.qrcode;

namespace DAn_QLCuaHangBanthuoc
{
    public partial class frm_ThemKH : Form
    {
        BLL_customer BLL;
        string ID;
        int mode;
        public frm_ThemKH(int mode, string selectedID="")
        {
            InitializeComponent();
            BLL = new BLL_customer();
            this.mode = mode;
            ID = selectedID;
            if (mode == 1)
            {
                ID = selectedID;
                btn_add.Enabled = false;
                DataRow selectedRow = BLL.GetRecord(ID);
                txt_name.Text = selectedRow[1].ToString();
                string gender = selectedRow[2].ToString().Trim();
                txt_phone.Text = selectedRow[3].ToString();
                txt_address.Text = selectedRow[4].ToString();
                if (gender == "Name")
                {
                    cbo_gender.SelectedItem = "Nam";
                }
                else { cbo_gender.SelectedItem = "Nữ"; };
            }
            else
            {
                btn_edit.Enabled = false;
                btn_delete.Enabled = false;
            }
        }
        private bool Check()
        {
            if (string.IsNullOrWhiteSpace(txt_name.Text))
            {
                MessageBox.Show("Vui lòng nhập tên khách hàng!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txt_name.Focus();
                return false;
            }
            if (string.IsNullOrWhiteSpace(cbo_gender.Text))
            {
                MessageBox.Show("Vui lòng chọn giới tính!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                cbo_gender.Focus();
                return false;
            }
            string phonePattern = @"^0(3[2-9]|5[6-9]|7[0-9]|8[1-9]|9[0-4|6-9])[0-9]{7}$";
            if (string.IsNullOrWhiteSpace(txt_phone.Text) || !System.Text.RegularExpressions.Regex.IsMatch(txt_phone.Text, phonePattern))
            {
                MessageBox.Show("Vui lòng nhập số điện thoại hợp lệ (10 chữ số)!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txt_phone.Focus();
                return false;
            }
            if (string.IsNullOrWhiteSpace(txt_address.Text))
            {
                MessageBox.Show("Vui lòng nhập địa chỉ!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txt_address.Focus();
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
                    BLL.AddRecord(txt_name.Text, cbo_gender.Text, txt_phone.Text, txt_address.Text);
                    MessageBox.Show("Thêm khách hàng thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Close();
                }
                catch (Exception ex)
                {
                    if (ex.Message.Contains("Phone number already exists"))
                    {
                        MessageBox.Show("Số điện thoại này đã tồn tại! Vui lòng nhập số khác.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        txt_phone.Clear();
                        txt_phone.Focus();
                    }
                    else
                    {
                        MessageBox.Show($"Lỗi khi thêm khách hàng: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                    BLL.UpdateRecord(ID, txt_name.Text, cbo_gender.Text, txt_phone.Text, txt_address.Text);
                    MessageBox.Show("Cập nhật thông tin khách hàng thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Close();
                }
                catch (Exception ex)
                {
                    if (ex.Message.Contains("Phone number already exists"))
                    {
                        MessageBox.Show("Số điện thoại này đã tồn tại! Vui lòng nhập số khác.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        txt_phone.Clear();
                        txt_phone.Focus();
                    }
                    else
                    {
                        MessageBox.Show($"Lỗi khi cập nhật khách hàng: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void btn_delete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show($"Bạn có chắc chắn muốn xóa khách hàng với mã '{ID}' không?", "Cảnh báo", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                BLL.DeleteRecord(ID);
                Close();
            }
        }
    }
}
