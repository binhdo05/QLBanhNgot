using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using BLL;
using Microsoft.SqlServer.Server;

namespace DAn_QLBanhNgot
{
    public partial class frm_ThongKe : Form
    {
        BLL_ThongKe BLL_TK;
        public frm_ThongKe()
        {
            InitializeComponent();
            BLL_TK = new BLL_ThongKe();
            CapNhatDoanhThu(DateTime.Now.Month, DateTime.Now.Year);
            SoLuongKhachHangHD();
            CapNhatSanPhamBanChay();
            TKSHDNow();
        }
        void CapNhatDoanhThu(int? month, int year)
        {
            string procedureCall = month.HasValue
                ? $"exec ThongKeDoanhThuTheoThangNam {month}, {year}"
                : $"exec ThongKeDoanhThuTheoThangNam NULL, {year}";

            DataTable dataTable = BLL_TK.TKDoanhThu(month, year);

            chart1.Series["Series1"].Points.Clear();

            if (month.HasValue)
            {
                // Tìm kiếm theo tháng cụ thể, chỉ hiển thị tháng đó
                DataRow row = dataTable.Rows.Count > 0 ? dataTable.Rows[0] : null;
                int revenue = row != null && row["Revenue"] != DBNull.Value
                    ? Convert.ToInt32(row["Revenue"])
                    : 0;

                var point = new DataPoint(month.Value, revenue)
                {
                    LegendText = $"Tháng {month.Value}: {revenue:N0} VNĐ",
                    Label = $"{revenue:N0} VNĐ"
                };
                chart1.Series["Series1"].Points.Add(point);
            }
            else
            {
                // Tìm kiếm theo năm, hiển thị tất cả 12 tháng
                int[] revenues = new int[12];
                foreach (DataRow row in dataTable.Rows)
                {
                    int monthNumber = Convert.ToInt32(row["MonthNumber"]);
                    int revenue = row["Revenue"] != DBNull.Value ? Convert.ToInt32(row["Revenue"]) : 0;
                    revenues[monthNumber - 1] = revenue;
                }

                for (int i = 1; i <= 12; i++)
                {
                    int revenue = revenues[i - 1];

                    var point = new DataPoint(i, revenue)
                    {
                        LegendText = $"Tháng {i}: {revenue:N0} VNĐ",
                        Label = $"{revenue:N0} VNĐ"
                    };
                    chart1.Series["Series1"].Points.Add(point);
                }
            }

            // Cấu hình biểu đồ
            chart1.Series["Series1"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Column;
            chart1.Series["Series1"].IsValueShownAsLabel = true;
            chart1.Legends[0].Enabled = true;
            chart1.Legends[0].Docking = System.Windows.Forms.DataVisualization.Charting.Docking.Top;
        }

        private void btnTK_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNam.Text))
            {
                MessageBox.Show("Vui lòng nhập năm.");
                return;
            }
            if (!int.TryParse(txtNam.Text, out int year) || year < 1)
            {
                MessageBox.Show("Năm không hợp lệ. Vui lòng nhập giá trị năm dương.");
                return;
            }
            int? month = null;
            if (!string.IsNullOrWhiteSpace(txtThang.Text))
            {
                if (!int.TryParse(txtThang.Text, out int monthValue) || monthValue < 1 || monthValue > 12)
                {
                    MessageBox.Show("Tháng không hợp lệ. Vui lòng nhập giá trị từ 1 đến 12.");
                    return;
                }
                month = monthValue;
            }
            CapNhatDoanhThu(month, year);
        }
        private void btnHuy_Click(object sender, EventArgs e)
        {
            CapNhatDoanhThu(DateTime.Now.Month, DateTime.Now.Year);
            txtNam.Clear();
            txtThang.Clear();
        }
        private void SoLuongKhachHangHD()
        {
            DataTable dataTable = BLL_TK.TKSoLuong(); 
            if (dataTable.Rows.Count > 0)
            {
                int soLuongKhachHang = Convert.ToInt32(dataTable.Rows[0]["SoLuongKhachHang"]);
                int soSp = Convert.ToInt32(dataTable.Rows[0]["SoSanPham"]);
                int soLuongHDB = Convert.ToInt32(dataTable.Rows[0]["SoHDB"]);
                int soLuongHDN = Convert.ToInt32(dataTable.Rows[0]["SoHDN"]);
                lblKH.Text = $"{soLuongKhachHang:N0}";
                lblHDB.Text = $"{soLuongHDB:N0}";
                lblSP.Text = $"{soSp:N0}";
                lblHDN.Text = $"{soLuongHDN:N0}";
            }
            else
            {
                lblKH.Text = "0";
                lblHDB.Text = "0";
                lblSP.Text = "0";
                lblHDN.Text = "0";
            }
        }
        private void CapNhatSanPhamBanChay()
        {
            DataTable dataTable = BLL_TK.TK_SPBanChay();
            if (dataTable.Rows.Count > 0)
            {
                chart2.Series["Series1"].Points.Clear();
                foreach (DataRow row in dataTable.Rows)
                {
                    string tenSanPham = row["TenSanPham"].ToString();
                    int tongSoLuongBan = Convert.ToInt32(row["TongSoLuongBan"]);
                    decimal tongDoanhThu = Convert.ToDecimal(row["TongDoanhThu"]);

                    var point = new DataPoint(0, tongSoLuongBan)
                    {
                        LegendText = $"{tenSanPham}: {tongSoLuongBan:N0} sản phẩm",
                        AxisLabel = tenSanPham,
                        Label = $"{tongSoLuongBan:N0} ({tongDoanhThu:N0} VND)"
                    };
                    chart2.Series["Series1"].Points.Add(point);
                }

                chart2.Series["Series1"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Column;
                chart2.Series["Series1"].IsValueShownAsLabel = true;
                chart2.Legends[0].Enabled = true;
                chart2.Legends[0].Docking = System.Windows.Forms.DataVisualization.Charting.Docking.Top;
            }
        }
        void TKSHDNow()
        {
            dgvHDNow.Rows.Clear();
            DataTable dataTable = BLL_TK.TKHD_Now();
            foreach (DataRow row in dataTable.Rows)
            {
                dgvHDNow.Rows.Add(row["MaHoaDon"], row["NgayTao"], row["TongTien"]);
            }
        }

        private void dgvHDNow_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvHDNow.Rows.Count > 0)
            {
                frm_ShowCTHDB frm = new frm_ShowCTHDB(dgvHDNow.SelectedRows[0].Cells[0].Value.ToString());
                frm.ShowDialog();
            }
        }
    }
}
