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
    public partial class frm_KhachHang : Form
    {
        BLL_KhachHang BLL;
        public frm_KhachHang()
        {
            InitializeComponent();
            BLL = new BLL_KhachHang();

            LoadData();
        }
        void LoadData()
        {
            dgvKhachHang.Rows.Clear();
            DataTable dataTable = BLL.GetData(txtSearch.Text);
            foreach (DataRow row in dataTable.Rows)
            {
                dgvKhachHang.Rows.Add(row[0], row[1], row[2], row[3]);
            }
        }
        private void btnTim_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            frm_ThemKH form = new frm_ThemKH(0);
            form.ShowDialog();
            LoadData();
        }

        private void btnXem_Click(object sender, EventArgs e)
        {
            if (dgvKhachHang.Rows.Count > 0)
            {
                string selectedID = dgvKhachHang.SelectedRows[0].Cells[0].Value.ToString();
                frm_ThemKH form = new frm_ThemKH(1, selectedID);
                form.ShowDialog();
                LoadData();
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (dgvKhachHang.Rows.Count > 0)
            {
                string selectedID = dgvKhachHang.SelectedRows[0].Cells[0].Value.ToString();
                if (MessageBox.Show($"Bạn có chắc muốn xóa khách hàng mã '{selectedID}' không?", "Cảnh báo!", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                {
                    BLL.DeleteRecord(selectedID);
                    LoadData();
                }
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            if (txtSearch.Text.Length < 1)
            {
                LoadData();
            }
        }
    }
}
