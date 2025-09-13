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
    public partial class frm_HDB : Form
    {
        BLL_HoaDonBan BLL;
        public frm_HDB()
        {
            InitializeComponent();
            BLL = new BLL_HoaDonBan();
            LoadData();
        }
        void LoadData()
        {
            dgvHD.Rows.Clear();
            DataTable dataTable = BLL.GetData();
            foreach (DataRow row in dataTable.Rows)
            {
                dgvHD.Rows.Add(row[0], row[1], row[2], row[3], row[4]);
            }
        }
        void LoadData2(string key)
        {
            dgvHD.Rows.Clear();
            DataTable dataTable = BLL.GetDataSearch(key); 

            foreach (DataRow row in dataTable.Rows)
            {
                dgvHD.Rows.Add(row["MaHDB"], row["NgayTao"], row["TenKhachHang"], row["TongTien"], row["TrangThai"]);
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (dgvHD.Rows.Count > 0)
            {
                string selectedID = dgvHD.SelectedRows[0].Cells[0].Value.ToString();
                if (MessageBox.Show($"Bạn có chắc muốn xóa hóa đơn mã '{selectedID}' không?", "Cảnh báo!", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                {
                    BLL.DeleteRecord(selectedID);
                    LoadData();
                }
            }
        }

        private void btnXem_Click(object sender, EventArgs e)
        {
            if (dgvHD.Rows.Count > 0)
            {
                frm_ShowCTHDB frm = new frm_ShowCTHDB(dgvHD.SelectedRows[0].Cells[0].Value.ToString());
                frm.ShowDialog();
                LoadData();
            }
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            frm_ThemHDB form = new frm_ThemHDB();
            form.ShowDialog();
            LoadData();
        }

        private void btnTim_Click(object sender, EventArgs e)
        {
            string key = txtSearch.Text.Trim(); 
            LoadData2(key);
        }
        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtSearch.Text))
            {
                LoadData();
            }
        }
    }
}
