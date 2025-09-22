using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DAn_QLCuaHangBanthuoc
{
    public partial class frm_Main : Form
    {
        private frm_Login loginForm;
        private int currentMode;
        private string userRole;
        private string username;
        public frm_Main(int mode, frm_Login login, string role, string username)
        {
            InitializeComponent();
            this.FormClosing += frm_Main_FormClosing;
            this.loginForm = login;
            this.currentMode = mode;
            this.userRole = role;
            this.username = username;
            SetupPermissions();
            container(new frm_dashboard());
            this.Text = $"Hệ thống quản lý bán cửa hàng bán bánh ngọt - Người dùng: {username} ({userRole})";
        }

        private void SetupPermissions()
        {
            // Only Management role has full access
            if (userRole == "Quản lý")
            {
                btn_dashboard.Enabled = true;
                btn_product.Enabled = true;
                btn_Customer.Enabled = true;
                btn_sale.Enabled = true;
                btn_purchase.Enabled = true;
                btn_supplier.Enabled = true;
                btn_Staff.Enabled = true;
                btn_Staff.Visible = true;
            }
            // Staff role has limited access
            else if (userRole == "Nhân viên")
            {
                btn_dashboard.Enabled = false;
                btn_product.Enabled = true;
                btn_Customer.Enabled = true;
                btn_sale.Enabled = true;
                btn_purchase.Enabled = false;
                btn_supplier.Enabled = false;
                btn_Staff.Enabled = false;
                btn_Staff.Visible = false;
            }
        }

        public void container(Form form)
        {
            if (pn_main.Controls.Count > 0)
            {
                foreach (Control control in pn_main.Controls)
                {
                    if (control is Form oldForm)
                    {
                        oldForm.Dispose();
                    }
                }
                pn_main.Controls.Clear();
            }
            form.TopLevel = false;
            form.FormBorderStyle = FormBorderStyle.None;
            form.Dock = DockStyle.Fill;
            pn_main.Controls.Add(form);
            form.Show();
        }

        private void btn_dashboard_Click(object sender, EventArgs e)
        {
            container(new frm_dashboard());
            if (userRole == "Quản lý")
            {
                container(new frm_dashboard());
            }
            else
            {
                MessageBox.Show("You don't have permission to access this feature.",
                    "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btn_product_Click(object sender, EventArgs e)
        {
            
            container(new frm_SanPham(this, currentMode));
        }

        private void btn_Customer_Click(object sender, EventArgs e)
        {
            container(new frm_KhachHang());
        }

        private void btn_sale_Click(object sender, EventArgs e)
        {
            container(new frm_HoaDon(this));
        }

        private void btn_purchase_Click(object sender, EventArgs e)
        {
            if (userRole == "Quản lý")
            {
                container(new frm_DonNhap(this));
            }
            else
            {
                MessageBox.Show("You don't have permission to access this feature.",
                    "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            container(new frm_DonNhap(this));
        }

        private void btn_supplier_Click(object sender, EventArgs e)
        {
            if (userRole == "Quản lý")
            {
                container(new frm_NCC());
            }
            else
            {
                MessageBox.Show("You don't have permission to access this feature.",
                    "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            container(new frm_NCC());
        }

        private void btn_Staff_Click(object sender, EventArgs e)
        {
            if (userRole == "Quản lý")
            {
                container(new frm_NhanVien());
            }
            else
            {
                MessageBox.Show("You don't have permission to access this feature.",
                    "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            container(new frm_NhanVien());
        }

        private void btn_logout_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(
                "Bạn có chắc chắn muốn đăng xuất không?",
                "Confirmation",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );
            if (result == DialogResult.Yes)
            {
                this.Hide();
                if (loginForm != null && !loginForm.IsDisposed)
                {
                    loginForm.Show();
                }
                else
                {
                    frm_Login frm = new frm_Login(1);
                    frm.ShowDialog();
                }
                this.Dispose();
            }
        }

        private void frm_Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.ApplicationExitCall)
            {
                e.Cancel = false;
                return;
            }
            DialogResult result = MessageBox.Show(
                "Bạn có chắc chắn muốn đăng xuất không?",
                "Confirmation",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (result == DialogResult.Yes)
            {
                e.Cancel = false;
                Application.Exit();
            }
            else
            {
                e.Cancel = true;
            }
        }

        private void pn_main_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}