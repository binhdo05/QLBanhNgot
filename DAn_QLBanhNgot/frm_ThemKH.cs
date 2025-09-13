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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace DAn_QLBanhNgot
{
    public partial class frm_ThemKH : Form
    {
        BLL_KhachHang BLL;
        string ID;
        int mode;
        public frm_ThemKH(int mode, string selectedID = "")
        {
            InitializeComponent();
            BLL = new BLL_KhachHang();
            this.mode = mode;
            ID = selectedID;
            if (mode == 1)
            {
                btnThem.Text = "Sửa";
                btnThem.BackColor = Color.DeepSkyBlue;
                ID = selectedID;

                DataRow selectedRow = BLL.GetRecord(ID);
                txtTen.Text = selectedRow[1].ToString();
                txtDiaChi.Text = selectedRow[2].ToString();
                txtSDT.Text = selectedRow[3].ToString();
            }
            else {
                label1.Text = "Thêm mới khách hàng";
                ID = BLL.GetNewID();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        bool Check()
        {
            if (string.IsNullOrEmpty(txtTen.Text))
            {
                MessageBox.Show("Tên không được để rỗng!!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning); return false;
            }
            if (!Utility.IsPhoneNumber(txtSDT.Text))
            {
                MessageBox.Show("Số điện thoại không hợp lệ!", "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            return true;
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            if (Check())
            {
                BLL.AddRecord(ID, txtTen.Text, txtDiaChi.Text, txtSDT.Text);
                MessageBox.Show("Cập nhật thông tin thành công!!!");
                Close();
            }
        }
    }
}
