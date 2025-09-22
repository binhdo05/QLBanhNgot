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

namespace DAn_QLCuaHangBanthuoc
{
    public partial class frm_SuaLo : Form
    {
        private readonly frm_Main _mainForm;
        private readonly string _idPurchase;
        BLL_batch bll;
        public frm_SuaLo(frm_Main mainForm, string id)
        {
            InitializeComponent();
            _mainForm = mainForm;
            _idPurchase = id;
            bll = new BLL_batch();

            DataRow row = bll.GetRow(id);
            txt_quantity.Text = row[3].ToString();
            lbl_id.Text = row[0].ToString();
            lbl_medicine.Text = row[2].ToString();
            txt_price.Text = row[4].ToString();
            if (row[5] != DBNull.Value)
            {
                DateTime manuDate = Convert.ToDateTime(row[5]);
                txt_manu.Text = manuDate.ToString("dd/MM/yyyy");
            }
            else
            {
                txt_manu.Text = string.Empty;
            }

            if (row[6] != DBNull.Value)
            {
                DateTime expiryDate = Convert.ToDateTime(row[6]);
                txt_expiry.Text = expiryDate.ToString("dd/MM/yyyy");
            }
            else
            {
                txt_expiry.Text = string.Empty;
            }
            string status = row[7].ToString();
            if (status == "Active")
            {
                cbo_status.SelectedItem = "Active";
            }
            else { cbo_status.SelectedItem = "Inactive"; };
            txt_shortage.Text = row[8].ToString();
            txt_note.Text = row[9].ToString();
        }

        private void btn_close_Click(object sender, EventArgs e)
        {
            _mainForm.container(new frm_DonNhap(_mainForm));
        }

        private void btn_delete_Click(object sender, EventArgs e)
        {
            string idBatch = lbl_id.Text;
            DialogResult result = MessageBox.Show(
                $"Bạn có chắc muốn xoá lô có mã {idBatch}?",
                "Confirmation",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );
            if (result == DialogResult.No)
            {
                return;
            }
            bll.DeleteBatch(idBatch);
            MessageBox.Show("Xoá thành công!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            _mainForm.container(new frm_DonNhap(_mainForm));
        }

        private void btn_edit_Click(object sender, EventArgs e)
        {
            int idBatch;
            if (!int.TryParse(lbl_id.Text, out idBatch))
            {
                MessageBox.Show("Mã không hợp lệ!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            int quantityInBatch;
            if (!int.TryParse(txt_quantity.Text, out quantityInBatch) || quantityInBatch <= 0)
            {
                MessageBox.Show("Số lượng phải là số nguyên dương!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            decimal entryPrice;
            if (!decimal.TryParse(txt_price.Text, out entryPrice) || entryPrice < 0)
            {
                MessageBox.Show("Giá nhập không được âm!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            DateTime manufacturingDate;
            if (!DateTime.TryParseExact(txt_manu.Text, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out manufacturingDate))
            {
                MessageBox.Show("Nhập ngày sản xuất có dạng dd/MM/yyyy!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            DateTime expiryDate;
            if (!DateTime.TryParseExact(txt_expiry.Text, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out expiryDate))
            {
                MessageBox.Show("Nhập hạn sử dụng có dạng dd/MM/yyyy!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            string status = cbo_status.SelectedItem?.ToString();
            if (string.IsNullOrEmpty(status) || (status != "Active" && status != "Inactive"))
            {
                MessageBox.Show("Chọn trạng thái!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            int quantityShortage;
            if (!int.TryParse(txt_shortage.Text, out quantityShortage) || quantityShortage < 0)
            {
                MessageBox.Show("Số lượng lỗi không được âm!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (quantityShortage > quantityInBatch)
            {
                MessageBox.Show("Số lượng lỗi không được lớn hơn số lượng tồn!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            string note = txt_note.Text?.Trim();
            DialogResult result = MessageBox.Show(
                "Bạn có chắc muốn cập nhật?",
                "Confirmation",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );
            if (result == DialogResult.No)
            {
                return;
            }
            try
            {
                bll.UpdateBatch(idBatch, quantityInBatch, entryPrice, manufacturingDate, expiryDate, status, quantityShortage, note);
                MessageBox.Show("Cập nhật thành công!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                _mainForm.container(new frm_DonNhap(_mainForm));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating batch: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}