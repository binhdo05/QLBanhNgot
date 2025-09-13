using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System.Diagnostics;
using System.IO;
using Microsoft.SqlServer.Server;
using BLL;

namespace DAn_QLBanhNgot
{
    public partial class frm_ShowCTHDB : Form
    {
        BLL_HoaDonBan BLL;
        string id;
        public frm_ShowCTHDB(string idHD)
        {
            InitializeComponent();
            BLL = new BLL_HoaDonBan();
            if (!string.IsNullOrEmpty(idHD))
            {
                id = idHD;
                DataTable tb = BLL.GetCTHDB(id);
                DataRow row = BLL.GetRow(id);
                txtNgay.Text = row[1].ToString();
                txtTenKH.Text = row[2].ToString();
                txtMaHD.Text = idHD.ToString();
                cboTT.Text = row[4].ToString();
                decimal tongThanhTien = 0;
                if (tb.Rows.Count > 0)
                {
                    foreach (DataRow r in tb.Rows)
                    {
                        dgvCT.Rows.Add(r["Mã DM"], r["Tên DM"], r["SL"], r["Gia"], r["ThanhTien"]);
                        tongThanhTien += Convert.ToDecimal(r["ThanhTien"]);
                    }
                    txtTongTien.Text = tongThanhTien  + " VNĐ";
                }
                checkTT();
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        bool checkTT()
        {
            if (cboTT.Text == "Chờ TT")
            {
                btnIN.Enabled = false;
                return false;
            }
            else
            {
                btnTT.Visible = false;
                label5.Visible = false;
                panel4.Visible = false;
                cboTT.Visible = false;
                btnIN.Enabled = true;
                return true;
            }
        }
        private void btnIN_Click(object sender, EventArgs e)
        {
            if (checkTT())
            {
                Document document = new Document(PageSize.A4);
                try
                {
                    PdfWriter writer = PdfWriter.GetInstance(document, new FileStream("Invoice.pdf", FileMode.Create));
                    document.SetMargins(0, document.TopMargin, document.LeftMargin, document.BottomMargin);
                    document.Open();
                    Bitmap bmp = new Bitmap(pnPrint.Width, pnPrint.Height);
                    btnIN.Visible = false;
                    btnClose.Visible = false;
                    pnPrint.DrawToBitmap(bmp, new System.Drawing.Rectangle(0, 0, pnPrint.Width, pnPrint.Height));
                    btnIN.Visible = true;
                    btnClose.Visible = true;
                    iTextSharp.text.Image pic = iTextSharp.text.Image.GetInstance(bmp, System.Drawing.Imaging.ImageFormat.Bmp);
                    pic.ScaleToFit(PageSize.A4.Width, PageSize.A4.Height);
                    pic.SetAbsolutePosition(document.Left, document.Top - pic.ScaledHeight);
                    document.Add(pic);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Đã xảy ra lỗi khi tạo PDF: " + ex.Message);
                }
                finally
                {
                    document.Close();
                    this.Close();
                }
                Process.Start("Invoice.pdf");
            }
            else
            {
                MessageBox.Show("Hóa đơn chưa hoàn tất thanh toán!!!");
                return;
            }
        }

        private void btnTT_Click(object sender, EventArgs e)
        {
            if (cboTT.SelectedItem == null || string.IsNullOrEmpty(cboTT.SelectedItem.ToString()))
            {
                MessageBox.Show("Vui lòng xác nhận trạng thái hóa đơn!!!", "Warning!!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            BLL.Confirm(id, cboTT.Text);
            MessageBox.Show("Cập nhật trạng thái hóa đơn thành công!!!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            checkTT();
        }
    }
}
