using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DAn_QLBanhNgot
{
    public partial class frm_Main : Form
    {
        public frm_Main()
        {
            InitializeComponent();
            container(new frm_ThongKe());
        }
        void container(Form form)
        {
            if (pn_frm.Controls.Count > 0)
            {
                pn_frm.Controls.Clear();
            }
            form.TopLevel = false;
            form.FormBorderStyle = FormBorderStyle.None;
            form.Dock = DockStyle.Fill;
            pn_frm.Controls.Add(form);
            form.Show();
        }

        private void btn_Thoat_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnTK_Click(object sender, EventArgs e)
        {
            container(new frm_ThongKe());
        }

        private void btnDM_Click(object sender, EventArgs e)
        {
            container(new frm_DanhMuc());
        }

        private void btnKH_Click(object sender, EventArgs e)
        {
            container(new frm_KhachHang());
        }

        private void btnHDB_Click(object sender, EventArgs e)
        {
            container(new frm_HDB());
        }

        private void btn_HDN_Click(object sender, EventArgs e)
        {
            container(new frm_HDN());
        }
    }
}