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
using Microsoft.SqlServer.Server;

namespace DAn_QLBanhNgot
{
    public partial class frm_ThemHDB : Form
    {
        BLL_HoaDonBan BLL = new BLL_HoaDonBan();
        List<string> maSPMua = new List<string>();
        List<int> SoLuongTon = new List<int>();
        DataTable bangMon;
        public frm_ThemHDB()
        {
            InitializeComponent();
            BLL = new BLL_HoaDonBan();
            Load_cbo();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        void Load_cbo()
        {
            DataTable dtK = BLL.GetKhachHang();
            cboKhach.DataSource = dtK;
            cboKhach.DisplayMember = "HoTen";
            cboKhach.ValueMember = "MaKH";

            bangMon = BLL.GetDanhMuc();
            cboMon.DataSource = bangMon;
            cboMon.DisplayMember = "TenDM";
            cboMon.ValueMember = "MaDM";
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
                    dgvHD.Rows.Add(row[0], row[1], 1, row[3]);
                    maSPMua.Add(row[0].ToString());
                    SoLuongTon.Add(int.Parse(row[2].ToString()));
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (dgvHD.Rows.Count > 0)
            {
                if (string.IsNullOrEmpty(cboKhach.SelectedValue.ToString()))
                {
                    MessageBox.Show("Vui lòng chọn khách hàng!", "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (cboTT.SelectedItem == null || string.IsNullOrEmpty(cboTT.SelectedItem.ToString()))
                {
                    MessageBox.Show("Vui lòng xác định trạng thái hóa đơn!", "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                for (int i = 0; i < dgvHD.Rows.Count; i++)
                {
                    int soLuong = 0;
                    if (!int.TryParse(dgvHD.Rows[i].Cells[2].Value.ToString(), out soLuong) || soLuong < 1)
                    {
                        MessageBox.Show("Số lượng sản phẩm phải là số và lớn hơn 0", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    if (soLuong > SoLuongTon[i])
                    {
                        string id = dgvHD.Rows[i].Cells[0].Value.ToString();
                        MessageBox.Show($"Số lượng sản phẩm có mã {id} vượt quá số lượng trong kho: {SoLuongTon[i]}!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }
                if (MessageBox.Show("Bạn chắc chắn muốn tạo hóa đơn này chứ?", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    BLL.AddRecord(cboKhach.SelectedValue.ToString(), cboTT.SelectedItem.ToString());
                    string idHDBnew = BLL.GetIDHDB();
                    foreach (DataGridViewRow row in dgvHD.Rows)
                    {
                        BLL.AddDetails(idHDBnew, row.Cells[0].Value.ToString(), row.Cells[2].Value.ToString());
                    }
                    MessageBox.Show("Tạo hóa đơn thành công!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    dgvHD.Rows.Clear();
                    this.Close();
                }
            }
        }
    }
}
