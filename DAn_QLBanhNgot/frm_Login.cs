using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.SqlServer.Server;

namespace DAn_QLBanhNgot
{
    public partial class frm_Login : Form
    {
        public frm_Login()
        {
            InitializeComponent();
        }
        string username = "admin";
        string password = "1";
        Dictionary<string, string> accounts = new Dictionary<string, string>();
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (!ValidateLoginInput())
            {
                return;
            }

            // Đăng nhập trực tiếp
            if (ValidateDirectLogin(txtTK.Text, txtMK.Text))
            {
                OpenMainForm();
            }
            else
            {
                DisplayLoginError("Tên đăng nhập hoặc mật khẩu không chính xác!");
            }
        }
        private bool ValidateLoginInput()
        {
            if (string.IsNullOrWhiteSpace(txtTK.Text))
            {
                MessageBox.Show("Tên đăng nhập không được bỏ trống!", "Cảnh Báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtMK.Text))
            {
                MessageBox.Show("Mật khẩu không được bỏ trống!", "Cảnh Báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        private bool ValidateDirectLogin(string username, string password)
        {
            return username == this.username && password == this.password;
        }

        private void OpenMainForm()
        {
            this.Hide();
            using (frm_Main mainForm = new frm_Main())
            {
                mainForm.ShowDialog();
            }
        }

        private void DisplayLoginError(string message)
        {
            txtTK.Clear();
            txtMK.Clear();
            MessageBox.Show(message, "Cảnh Báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }
}
