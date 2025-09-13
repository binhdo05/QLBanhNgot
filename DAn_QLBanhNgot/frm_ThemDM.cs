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
    public partial class frm_ThemDM : Form
    {
        BLL_DanhMuc BLL;
        string ID;
        int mode;
        public frm_ThemDM(int mode, string selectedID = "")
        {
            InitializeComponent();
            BLL = new BLL_DanhMuc();
            this.mode = mode;
            ID = selectedID;
            if (mode == 1)
            {
                btnThem.Text = "Sửa";
                btnThem.BackColor = Color.DeepSkyBlue;
                ID = selectedID;

                DataRow selectedRow = BLL.GetRecord(ID);
                txtTen.Text = selectedRow[1].ToString();
                txtSL.Text = selectedRow[2].ToString();
                txtGiaBan.Text = selectedRow[3].ToString();
                txtMota.Text = selectedRow[4].ToString();
            }
            else
            {
                label1.Text = "Thêm mới sản phẩm";
                ID = BLL.GetNewID();
            }
        }
        bool Check()
        {
            if (string.IsNullOrEmpty(txtTen.Text))
            {
                MessageBox.Show("Tên không được để rỗng!!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning); return false;
            }
            if (!Utility.IsDigit(txtSL.Text) || Convert.ToInt32(txtSL.Text) < 0)
            {
                MessageBox.Show("Số lượng không hợp lệ!", "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (!Utility.IsDigit(txtGiaBan.Text) || Convert.ToDecimal(txtGiaBan.Text) < 0)
            {
                MessageBox.Show("Giá bán phải không hợp lệ!", "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }
        private void button1_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (Check())
            {
                BLL.AddRecord(ID, txtTen.Text, txtMota.Text, txtSL.Text, txtGiaBan.Text);
                MessageBox.Show("Cập nhật thông tin thành công!!!");
                Close();
            }
        }
    }
}
