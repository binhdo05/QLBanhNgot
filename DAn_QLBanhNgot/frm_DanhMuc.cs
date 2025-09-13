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
    public partial class frm_DanhMuc : Form
    {
        BLL_DanhMuc BLL;
        public frm_DanhMuc()
        {
            InitializeComponent();
            BLL = new BLL_DanhMuc();

            LoadData();
        }
        void LoadData()
        {
            dgvDanhMuc.Rows.Clear();
            DataTable dataTable = BLL.GetData(txtSearch.Text);
            foreach (DataRow row in dataTable.Rows)
            {
                dgvDanhMuc.Rows.Add(row[0], row[1], row[2], row[3], row[4]);
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            if (txtSearch.Text.Length < 1)
            {
                LoadData();
            }
        }

        private void btnTim_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            frm_ThemDM form = new frm_ThemDM(0);
            form.ShowDialog();
            LoadData();
        }

        private void btnXem_Click(object sender, EventArgs e)
        {
            if (dgvDanhMuc.Rows.Count > 0)
            {
                string selectedID = dgvDanhMuc.SelectedRows[0].Cells[0].Value.ToString();
                frm_ThemDM form = new frm_ThemDM(1, selectedID);
                form.ShowDialog();
                LoadData();
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (dgvDanhMuc.Rows.Count > 0)
            {
                string selectedID = dgvDanhMuc.SelectedRows[0].Cells[0].Value.ToString();
                if (MessageBox.Show($"Bạn có chắc muốn xóa sản phẩm mã '{selectedID}' không?", "Cảnh báo!", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                {
                    BLL.DeleteRecord(selectedID);
                    LoadData();
                }
            }
        }
    }
}
