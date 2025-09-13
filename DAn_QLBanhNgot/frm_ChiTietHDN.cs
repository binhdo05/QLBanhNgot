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
    public partial class frm_ChiTietHDN : Form
    {
        BLL_HoaDonNhap BLL;
        string id;
        public frm_ChiTietHDN(string idHD)
        {
            InitializeComponent();
            BLL = new BLL_HoaDonNhap();
            if (!string.IsNullOrEmpty(idHD))
            {
                id = idHD;
                DataTable tb = BLL.GetCTHDN(id);
                DataRow row = BLL.GetRow(id);
                txtNgay.Text = row[1].ToString();
                txtMaHD.Text = idHD.ToString();
                decimal tongThanhTien = 0;
                if (tb.Rows.Count > 0)
                {
                    foreach (DataRow r in tb.Rows)
                    {
                        dgvCT.Rows.Add(r["Mã NL"], r["Tên NL"], r["soLuong"], r["Gia"], r["TongTien"]);
                        tongThanhTien += Convert.ToDecimal(r["TongTien"]);
                    }
                    txtTongTien.Text = tongThanhTien + " VNĐ";
                }
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
