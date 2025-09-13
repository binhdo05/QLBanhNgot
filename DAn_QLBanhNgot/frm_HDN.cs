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
    public partial class frm_HDN : Form
    {
        BLL_NhienLieu BLL_NL;
        BLL_HoaDonNhap BLL_HDN;
        List<string> maSPMua = new List<string>();
        DataTable bangMon;
        public frm_HDN()
        {
            InitializeComponent();
            BLL_NL = new BLL_NhienLieu();
            BLL_HDN = new BLL_HoaDonNhap();

            LoadDataNL();
            LoadHDN();
            Load_cbo();
        }
        //Nhien lieu
        void LoadDataNL()
        {
            dgvNL.Rows.Clear();
            DataTable dataTable = BLL_NL.GetData();
            foreach (DataRow row in dataTable.Rows)
            {
                dgvNL.Rows.Add(row[0], row[1], row[2]);
            }
        }
        void Load_cbo()
        {
            bangMon = BLL_NL.GetData();
            cboMon.DataSource = bangMon;
            cboMon.DisplayMember = "TenNL";
            cboMon.ValueMember = "MaNL";
        }
        private void btnThem_Click(object sender, EventArgs e)
        {
            frm_ThemNL form = new frm_ThemNL(0);
            form.ShowDialog();
            LoadDataNL();
            Load_cbo();
        }

        private void btnXem_Click(object sender, EventArgs e)
        {
            if (dgvNL.Rows.Count > 0)
            {
                string selectedID = dgvNL.SelectedRows[0].Cells[0].Value.ToString();
                frm_ThemNL form = new frm_ThemNL(1, selectedID);
                form.ShowDialog();
                LoadDataNL();
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (dgvNL.Rows.Count > 0)
            {
                string selectedID = dgvNL.SelectedRows[0].Cells[0].Value.ToString();
                if (MessageBox.Show($"Bạn có chắc muốn xóa nhiên liệu mã '{selectedID}' không?", "Cảnh báo!", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                {
                    BLL_NL.DeleteRecord(selectedID);
                    LoadDataNL();
                }
            }
        }
        //HDN
        void LoadHDN()
        {
            dgvIn4DN.Rows.Clear();
            DataTable dataTable = BLL_HDN.GetData();
            foreach (DataRow row in dataTable.Rows)
            {
                dgvIn4DN.Rows.Add(row[0], row[1], row[2]);
            }
        }
        private void btnTaoDN_Click(object sender, EventArgs e)
        {
            if (dgvThemDN.Rows.Count - 1 > 0)
            {
                for (int i = 0; i < dgvThemDN.Rows.Count - 1; i++)
                {
                    if (!Utility.IsDigit(dgvThemDN.Rows[i].Cells[2].Value.ToString()) || int.Parse(dgvThemDN.Rows[i].Cells[2].Value.ToString()) < 0)
                    {
                        MessageBox.Show("Số lượng sản phẩm phải lớn hơn 0", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    if (!Utility.IsDigit(dgvThemDN.Rows[i].Cells[3].Value.ToString()) || int.Parse(dgvThemDN.Rows[i].Cells[3].Value.ToString()) < 0)
                    {
                        MessageBox.Show("Giá bán sản phẩm phải lớn hơn 0", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }
                if (MessageBox.Show("Bạn chắc chắn muốn tạo hóa đơn nhập này chứ?", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    BLL_HDN.AddRecord();
                    string idHDBnew = BLL_HDN.GetIDHDB();
                    for (int i = 0; i < dgvThemDN.Rows.Count -1; i++)
                    {
                        BLL_HDN.AddDetails(
                                    idHDBnew,
                                    dgvThemDN.Rows[i].Cells[0].Value.ToString(),
                                    dgvThemDN.Rows[i].Cells[2].Value.ToString(),
                                    dgvThemDN.Rows[i].Cells[3].Value.ToString()
                                );
                    }
                    MessageBox.Show("Tạo hóa đơn thành công!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    dgvThemDN.Rows.Clear();
                    LoadHDN();
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (dgvIn4DN.Rows.Count > 0)
            {
                string selectedID = dgvIn4DN.SelectedRows[0].Cells[0].Value.ToString();
                if (MessageBox.Show($"Bạn có chắc muốn xóa hóa đơn mã '{selectedID}' không?", "Cảnh báo!", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                {
                    BLL_HDN.DeleteRecord(selectedID);
                    LoadHDN();
                }
            }
        }
        private void btnThemMon_Click(object sender, EventArgs e)
        {
            foreach (string maMenu in maSPMua)
            {
                if (maMenu == cboMon.SelectedValue.ToString())
                {
                    MessageBox.Show("Sản phẩm đã được thêm vào hóa đơn!!!", "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }
            foreach (DataRow row in bangMon.Rows)
            {
                if (cboMon.SelectedValue.ToString() == row[0].ToString())
                {
                    dgvThemDN.Rows.Add(row[0], row[1], 1, 1);
                    maSPMua.Add(row[0].ToString());
                }
            }
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            dgvThemDN.Rows.Clear();
        }

        private void btnXemIn4DN_Click(object sender, EventArgs e)
        {
            if (dgvIn4DN.Rows.Count > 0)
            {
                frm_ChiTietHDN frm = new frm_ChiTietHDN(dgvIn4DN.SelectedRows[0].Cells[0].Value.ToString());
                frm.ShowDialog();
            }
        }
    }
}
