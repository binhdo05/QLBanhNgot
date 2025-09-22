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
    public partial class frm_ThemNCC : Form
    {
        BLL_supplier BLL;
        string ID;
        int mode;
        public frm_ThemNCC(int mode, string selectedID = "")
        {
            InitializeComponent();
            BLL = new BLL_supplier();
            this.mode = mode;
            ID = selectedID;
            if (mode == 1)
            {
                ID = selectedID;
                btn_add.Enabled = false;
                DataRow selectedRow = BLL.GetRecord(ID); 
                txt_name.Text = selectedRow["TenNCC"].ToString();
                txt_phone.Text = selectedRow["SDT"].ToString();
                txt_gmail.Text = selectedRow["Email"].ToString();
                txt_address.Text = selectedRow["DiaChi"].ToString();
            }
            else
            {
                btn_edit.Enabled = false;
                btn_delete.Enabled = false;
            }
        }
        bool Check()
        {
            if(string.IsNullOrWhiteSpace(txt_name.Text))
            {
                MessageBox.Show("Vui lòng nhập tên nhà cung cấp!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txt_name.Focus();
                return false;
            }
            string phonePattern = @"^0(3[2-9]|5[6-9]|7[0-9]|8[1-9]|9[0-4|6-9])[0-9]{7}$";
            if (string.IsNullOrWhiteSpace(txt_phone.Text) || !System.Text.RegularExpressions.Regex.IsMatch(txt_phone.Text, phonePattern))
            {
                MessageBox.Show("Vui lòng nhập số điện thoại hợp lệ của Việt Nam (10 chữ số)!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txt_phone.Focus();
                return false;
            }
            string emailPattern = @"^[\w\.]+@gmail(\.[\w-]{2,5})+$";
            if (string.IsNullOrWhiteSpace(txt_gmail.Text) || !System.Text.RegularExpressions.Regex.IsMatch(txt_gmail.Text, emailPattern))
            {
                MessageBox.Show("Vui lòng nhập địa chỉ email hợp lệ (ví dụ: user@example.com)!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txt_gmail.Focus();
                txt_gmail.Clear();
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
                    BLL.AddRecord(txt_name.Text, txt_phone.Text, txt_gmail.Text, txt_address.Text);
                    MessageBox.Show("Thêm nhà cung cấp thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Close();
                }
                catch (Exception ex)
                {
                    if (ex.Message.Contains("Số điện thoại đã tồn tại."))
                    {
                        MessageBox.Show("Số điện thoại này đã tồn tại! Vui lòng sử dụng số điện thoại khác.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        txt_phone.Clear();
                        txt_phone.Focus();
                    }
                    else if (ex.Message.Contains("Email đã tồn tại."))
                    {
                        MessageBox.Show("Email này đã tồn tại! Vui lòng sử dụng email khác.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        txt_gmail.Clear();
                        txt_gmail.Focus();
                    }
                    else
                    {
                        MessageBox.Show($"Lỗi khi thêm nhà cung cấp: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                    BLL.UpdateRecord(ID, txt_name.Text, txt_phone.Text, txt_gmail.Text, txt_address.Text);
                    MessageBox.Show("Cập nhật nhà cung cấp thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Close();
                }
                catch (Exception ex)
                {
                    if (ex.Message.Contains("Phone number already exists"))
                    {
                        MessageBox.Show("Số điện thoại này đã tồn tại! Vui lòng sử dụng số điện thoại khác.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        txt_phone.Focus();
                    }
                    else if (ex.Message.Contains("Email already exists"))
                    {
                        MessageBox.Show("Email này đã tồn tại! Vui lòng sử dụng email khác.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        txt_gmail.Focus();
                    }
                    else
                    {
                        MessageBox.Show($"Lỗi khi thêm nhà cung cấp: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void btn_delete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show($"Bạn có chắc chắn muốn xóa nhà cung cấp với ID '{ID}' không?", "Cảnh báo!", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                BLL.DeleteRecord(ID);
                MessageBox.Show("Xóa nhà cung cấp thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Close();
            }
        }
    }
}
