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
    public partial class frm_ThemNV : Form
    {
        BLL_staff BLL;
        string ID;
        int mode;
        public frm_ThemNV(int mode, string selectedID = "")
        {
            InitializeComponent();
            BLL = new BLL_staff();
            this.mode = mode;
            ID = selectedID;
            if (mode == 1)
            {
                ID = selectedID;
                btn_add.Enabled = false;
                DataRow selectedRow = BLL.GetRecord(ID);
                txt_name.Text = selectedRow["TenNV"].ToString();
                string gender = selectedRow["GioiTinh"].ToString().Trim();
                if (gender == "Nam")
                {
                    cbo_gender.SelectedItem = "Nam";
                }
                else { cbo_gender.SelectedItem = "Nữ"; }
                txt_address.Text = selectedRow["DiaChi"].ToString();
                txt_gmail.Text = selectedRow["Email"].ToString();
                txt_phone.Text = selectedRow["SDT"].ToString();
                if (DateTime.TryParse(selectedRow["NgayVaoLam"].ToString(), out DateTime startDate))
                {
                    txt_date.Text = startDate.ToString("dd/MM/yyyy");
                }
                else
                {
                    txt_date.Text = string.Empty; 
                }
                txt_username.Text = selectedRow["TenDangNhap"].ToString();
                txt_password.Text = selectedRow["MatKhau"].ToString();
                string role = selectedRow["VaiTro"].ToString();
                if (role == "Nhân viên")
                {
                    cbo_role.SelectedItem = "Nhân viên";
                }
                else { cbo_role.SelectedItem = "Quản lý"; }
                string ac = selectedRow["TrangThai"].ToString();
                if (ac == "0" || ac =="False")
                {
                    cbo_active.SelectedItem = "0";
                }
                else if (ac == "1" || ac == "True")
                {
                    cbo_active.SelectedItem = "1";
                }
            }
            else
            {
                btn_edit.Enabled = false;
                btn_delete.Enabled = false;
                txt_date.Text = DateTime.Today.ToString("dd/MM/yyyy");
            }
        }
        bool Check()
        {
            if (string.IsNullOrWhiteSpace(txt_name.Text))
            {
                MessageBox.Show("Vui lòng nhập tên nhân viên!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txt_name.Focus();
                return false;
            }
            if (string.IsNullOrWhiteSpace(cbo_gender.Text) || (cbo_gender.Text != "Nam" && cbo_gender.Text != "Nữ"))
            {
                MessageBox.Show("Vui lòng chọn giới tính!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                cbo_gender.Focus();
                return false;
            }
            if (string.IsNullOrWhiteSpace(txt_address.Text))
            {
                MessageBox.Show("Vui lòng nhập địa chỉ!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txt_address.Focus();
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
            string phonePattern = @"^0(3[2-9]|5[6-9]|7[0-9]|8[1-9]|9[0-4|6-9])[0-9]{7}$";
            if (string.IsNullOrWhiteSpace(txt_phone.Text) || !System.Text.RegularExpressions.Regex.IsMatch(txt_phone.Text, phonePattern))
            {
                MessageBox.Show("Vui lòng nhập số điện thoại Việt Nam hợp lệ (10 chữ số)!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txt_phone.Focus();
                txt_phone.Clear();
                return false;
            }
            if (string.IsNullOrWhiteSpace(txt_date.Text) || !DateTime.TryParseExact(txt_date.Text, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out DateTime startDate))
            {
                MessageBox.Show("Vui lòng nhập ngày bắt đầu hợp lệ (định dạng dd/MM/yyyy)!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txt_date.Focus();
                txt_date.Clear();
                return false;
            }
            if (startDate > DateTime.Today)
            {
                MessageBox.Show("Ngày bắt đầu không được lớn hơn ngày hiện tại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txt_date.Focus();
                return false;
            }
            if (string.IsNullOrWhiteSpace(txt_username.Text))
            {
                MessageBox.Show("Vui lòng nhập tên đăng nhập!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txt_username.Focus();
                return false;
            }
            if (string.IsNullOrWhiteSpace(txt_password.Text) || txt_password.Text.Length < 6)
            {
                MessageBox.Show("Vui lòng nhập mật khẩu (tối thiểu 6 ký tự)!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txt_password.Focus();
                txt_password.Clear();
                return false;
            }
            if (string.IsNullOrWhiteSpace(cbo_active.Text))
            {
                MessageBox.Show("Vui lòng chọn trạng thái hoạt động!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                cbo_active.Focus();
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
                    DateTime startDate = DateTime.ParseExact(txt_date.Text, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    BLL.AddRecord(txt_name.Text, cbo_gender.Text, txt_address.Text, txt_gmail.Text, txt_phone.Text, startDate, txt_username.Text, txt_password.Text, cbo_role.Text, cbo_active.Text);
                    MessageBox.Show("Thêm nhân viên thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                    else if (ex.Message.Contains("Email already exists"))
                    {
                        MessageBox.Show("Email này đã tồn tại! Vui lòng nhập email khác.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        txt_gmail.Clear();
                        txt_gmail.Focus();
                    }
                    else if (ex.Message.Contains("Username already exists"))
                    {
                        MessageBox.Show("Tên đăng nhập này đã tồn tại! Vui lòng nhập tên khác.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        txt_username.Clear();
                        txt_username.Focus();
                    }
                    else
                    {
                        MessageBox.Show($"Lỗi khi thêm nhân viên: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void btn_edit_Click(object sender, EventArgs e)
        {
            if (Check())
            {
                if (DateTime.TryParseExact(txt_date.Text, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out DateTime startDate))
                {
                    try
                    {
                        BLL.UpdateRecord(ID, txt_name.Text, cbo_gender.Text, txt_address.Text, txt_gmail.Text, txt_phone.Text, startDate, txt_username.Text, txt_password.Text, cbo_role.Text, cbo_active.Text);
                        MessageBox.Show("Cập nhật thông tin nhân viên thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        Close();
                    }
                    catch (Exception ex)
                    {
                        if (ex.Message.Contains("Phone number already exists"))
                        {
                            MessageBox.Show("Số điện thoại này đã được sử dụng! Vui lòng nhập số khác.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            txt_phone.Focus();
                        }
                        else if (ex.Message.Contains("Email already exists"))
                        {
                            MessageBox.Show("Email này đã được sử dụng! Vui lòng nhập email khác.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            txt_gmail.Focus();
                        }
                        else if (ex.Message.Contains("Username already exists"))
                        {
                            MessageBox.Show("Tên đăng nhập này đã được sử dụng! Vui lòng nhập tên khác.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            txt_username.Focus();
                        }
                        else
                        {
                            MessageBox.Show($"Lỗi khi cập nhật thông tin: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Ngày bắt đầu không hợp lệ!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txt_date.Focus();
                }
            }
        }

        private void btn_delete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show($"Bạn có chắc chắn muốn xóa nhân viên với mã '{ID}' không?", "Cảnh báo!", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                BLL.DeleteRecord(ID);
                MessageBox.Show("Xóa nhân viên thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Close();
            }
        }

        private void guna2ShadowPanel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
