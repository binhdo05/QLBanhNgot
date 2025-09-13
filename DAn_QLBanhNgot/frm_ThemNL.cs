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

namespace DAn_QLBanhNgot
{
    public partial class frm_ThemNL : Form
    {
        BLL_NhienLieu BLL;
        string ID;
        int mode;
        public frm_ThemNL(int mode, string selectedID = "")
        {
            InitializeComponent();
            BLL = new BLL_NhienLieu();
            this.mode = mode;
            ID = selectedID;
            if (mode == 1)
            {
                btnThem.Text = "Sửa";
                btnThem.BackColor = Color.DeepSkyBlue;
                ID = selectedID;

                DataRow selectedRow = BLL.GetRecord(ID);
                txtTen.Text = selectedRow[1].ToString();
                txtMota.Text = selectedRow[2].ToString();
            }
            else
            {
                label1.Text = "Thêm mới nhiên liệu";
                ID = BLL.GetNewID();
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtTen.Text))
            {
                MessageBox.Show("Tên không được để rỗng!!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            BLL.AddRecord(ID, txtTen.Text, txtMota.Text);
            MessageBox.Show("Cập nhật thông tin thành công!!!");
            Close();
        }
    }
}
