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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolTip;
using System.Globalization;

namespace DAn_QLCuaHangBanthuoc
{
    public partial class frm_ThemDN : Form
    {
        private readonly frm_Main _mainForm;
        BLL_batch BLL_b;
        BLL_purchase BLL_p;
        List<string> MaLo_List = new List<string>();
        DataTable Table_SanPham;
        DataTable TempLoSanPhamTable;

        public frm_ThemDN(frm_Main mainForm)
        {
            InitializeComponent();
            _mainForm = mainForm;
            BLL_b = new BLL_batch();
            BLL_p = new BLL_purchase();
            btn_delete.Visible = false;
            Load_cbo();
            InitializeTempBatchTable();
            dgv_data2.Columns.Clear();
            dgv_data2.DataSource = TempLoSanPhamTable;
            dgv_data2.Columns["MaLo"].ReadOnly = true;
            dgv_data2.Columns["MaSP"].ReadOnly = true;
            dgv_data2.Columns["TrangThai"].ReadOnly = true;
            dgv_data2.Columns["TenSP"].ReadOnly = true;
        }

        private void InitializeTempBatchTable()
        {
            TempLoSanPhamTable = new DataTable();
            TempLoSanPhamTable.Columns.Add("MaLo", typeof(int));
            TempLoSanPhamTable.Columns.Add("MaSP", typeof(int));
            TempLoSanPhamTable.Columns.Add("TenSP", typeof(string));
            TempLoSanPhamTable.Columns.Add("SoLuongNhap", typeof(string));
            TempLoSanPhamTable.Columns.Add("GiaNhap", typeof(string));
            TempLoSanPhamTable.Columns.Add("NSX", typeof(string));
            TempLoSanPhamTable.Columns.Add("HSD", typeof(string));
            TempLoSanPhamTable.Columns.Add("TrangThai", typeof(string));
        }
        void Load_cbo()
        {
            DataTable staff = BLL_p.GetStaff();
            DataRow row = staff.NewRow();
            row["MaNV"] = -1;
            row["TenNV"] = "-- Chọn nhân viên --";
            staff.Rows.InsertAt(row, 0);
            cbo_staff.DataSource = staff;
            cbo_staff.DisplayMember = "TenNV";
            cbo_staff.ValueMember = "MaNV";
            cbo_staff.SelectedValue = -1;

            DataTable supp = BLL_p.GetSupplier();
            DataRow row2 = supp.NewRow();
            row2["MaNCC"] = -1;
            row2["TenNCC"] = "-- Chọn nhà cung cấp --";
            supp.Rows.InsertAt(row2, 0);
            cbo_supplier.DataSource = supp;
            cbo_supplier.DisplayMember = "TenNCC";
            cbo_supplier.ValueMember = "MaNCC";
            cbo_supplier.SelectedValue = -1;

            DataTable me = BLL_p.GetProduct();
            DataRow row3 = me.NewRow();
            row3["MaSP"] = -1;
            row3["TenSP"] = "-- Chọn sản phẩm --";
            me.Rows.InsertAt(row3, 0);
            cbo_medicine.DataSource = me;
            cbo_medicine.DisplayMember = "TenSP";
            cbo_medicine.ValueMember = "MaSP";
            cbo_medicine.SelectedValue = -1;

            Table_SanPham = BLL_p.GetProduct();
        }

        private void CalculateTotal()
        {
            decimal totalAmount = 0;
            foreach (DataGridViewRow row in dgv_data2.Rows)
            {
                if (row.IsNewRow) continue;
                decimal giaNhap = 0;
                int soLuongNhap = 0;
                if (decimal.TryParse(row.Cells["GiaNhap"].Value?.ToString(), out giaNhap) &&
                    int.TryParse(row.Cells["SoLuongNhap"].Value?.ToString(), out soLuongNhap))
                {
                    totalAmount += giaNhap * soLuongNhap;
                }
            }
            lbl_total_price.Text = totalAmount.ToString("N0") + " VND";
        }

        private void dgv_data2_DoubleClick(object sender, EventArgs e)
        {
            btn_add.Visible = false;
            btn_delete.Visible = true;
        }

        private void cbo_medicine_MouseClick(object sender, MouseEventArgs e)
        {
            btn_add.Visible = true;
            btn_delete.Visible = false;
        }

        private void btn_add_Click(object sender, EventArgs e)
        {
            if (cbo_medicine.SelectedValue.ToString() == "-1")
            {
                MessageBox.Show("Vui lòng chọn một sản phẩm!", "Cảnh báo!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            string selectedSanPhamId = cbo_medicine.SelectedValue.ToString();
            if (MaLo_List.Contains(selectedSanPhamId))
            {
                MessageBox.Show("Sản phẩm đã được thêm vào hóa đơn nhập!", "Cảnh báo!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            DataRow selectedRow = null;
            foreach (DataRow row in Table_SanPham.Rows)
            {
                if (row["MaSP"].ToString() == selectedSanPhamId)
                {
                    selectedRow = row;
                    break;
                }
            }
            if (selectedRow != null)
            {
                DataRow newRow = TempLoSanPhamTable.NewRow();
                newRow["MaLo"] = 1;
                newRow["MaSP"] = selectedRow["MaSP"];
                newRow["TenSP"] = selectedRow["TenSP"];
                newRow["SoLuongNhap"] = 1;
                newRow["GiaNhap"] = 1;
                newRow["NSX"] = DateTime.Today.ToString("dd/MM/yyyy");
                newRow["HSD"] = DateTime.Today.AddYears(1).ToString("dd/MM/yyyy");
                newRow["TrangThai"] = "Active";
                TempLoSanPhamTable.Rows.Add(newRow);
                MaLo_List.Add(selectedSanPhamId);
                CalculateTotal();
            }
        }

        private void btn_delete_Click(object sender, EventArgs e)
        {
            if (dgv_data2.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = dgv_data2.SelectedRows[0];
                string sanPhamId = selectedRow.Cells["MaSP"].Value.ToString();
                MaLo_List.Remove(sanPhamId);
                TempLoSanPhamTable.Rows.RemoveAt(selectedRow.Index);
                CalculateTotal();
                btn_delete.Visible = false;
                btn_add.Visible = true;
            }
        }

        private void btn_new_perchase_Click(object sender, EventArgs e)
        {
            if (cbo_staff.SelectedValue.ToString() == "-1" || cbo_supplier.SelectedValue.ToString() == "-1")
            {
                MessageBox.Show("Vui lòng chọn nhân viên và nhà cung cấp!", "Cảnh báo!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            BindingContext[TempLoSanPhamTable].EndCurrentEdit();

            if (TempLoSanPhamTable.Rows.Count == 0)
            {
                MessageBox.Show("Vui lòng thêm ít nhất một lô sản phẩm!", "Cảnh báo!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int idStaff = Convert.ToInt32(cbo_staff.SelectedValue);
            int idSupplier = Convert.ToInt32(cbo_supplier.SelectedValue);
            decimal totalAmount = 0;

            foreach (DataRow row in TempLoSanPhamTable.Rows)
            {
                int idSanPham = Convert.ToInt32(row["MaSP"]);
                int soLuongNhap;
                if (!int.TryParse(row["SoLuongNhap"].ToString(), out soLuongNhap) || soLuongNhap <= 0)
                {
                    MessageBox.Show($"Số lượng nhập phải là số nguyên dương! Giá trị hiện tại: {row["SoLuongNhap"]}", "Lỗi dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                decimal giaNhap;
                if (!decimal.TryParse(row["GiaNhap"].ToString(), out giaNhap) || giaNhap <= 0)
                {
                    MessageBox.Show($"Giá nhập phải là số dương! Giá trị hiện tại: {row["GiaNhap"]}", "Lỗi dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string nsx = row["NSX"].ToString();
                string hsd = row["HSD"].ToString();
                string trangThai = row["TrangThai"].ToString();
                DateTime nsxDate, hsdDate;

                if (!DateTime.TryParseExact(nsx, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out nsxDate))
                {
                    MessageBox.Show($"Lỗi định dạng ngày sản xuất: {nsx} - Định dạng hợp lệ (dd/MM/yyyy)!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (!DateTime.TryParseExact(hsd, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out hsdDate))
                {
                    MessageBox.Show($"Lỗi định dạng hạn sử dụng: {hsd} - Định dạng hợp lệ (dd/MM/yyyy)!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (hsdDate <= nsxDate)
                {
                    MessageBox.Show($"Hạn sử dụng ({hsd}) phải lớn hơn ngày sản xuất ({nsx})!", "Lỗi logic", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (nsxDate > DateTime.Today)
                {
                    MessageBox.Show($"Ngày sản xuất ({nsx}) không thể lớn hơn ngày hiện tại!", "Lỗi logic", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                
                if (hsdDate < DateTime.Today)
                {
                    DialogResult expiredResult = MessageBox.Show(
                        $"Sản phẩm đã hết hạn sử dụng ({hsd}). Bạn có chắc chắn muốn tiếp tục?",
                        "Cảnh báo",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Warning);

                    if (expiredResult == DialogResult.No)
                    {
                        return;
                    }
                }
                totalAmount += giaNhap * soLuongNhap;
            }
            CalculateTotal();
            DialogResult result = MessageBox.Show(
                $"Bạn có chắc chắn muốn tạo hóa đơn nhập này?\nTổng tiền: {totalAmount.ToString("N0")} VND",
                "Xác nhận",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );
            if (result == DialogResult.No)
            {
                return;
            }

            BLL_p.CreatePurchaseInvoice(DateTime.Today, idSupplier, idStaff, totalAmount);
            string purchaseId = BLL_p.GetIDHDN();
            int id_purchase = int.Parse(purchaseId);

            foreach (DataRow row in TempLoSanPhamTable.Rows)
            {
                int idSanPham = Convert.ToInt32(row["MaSP"]);
                int soLuongNhap = Convert.ToInt32(row["SoLuongNhap"]);
                decimal giaNhap = Convert.ToDecimal(row["GiaNhap"]);
                string nsx = row["NSX"].ToString();
                string hsd = row["HSD"].ToString();
                string trangThai = row["TrangThai"].ToString();

                DateTime nsxDate = DateTime.ParseExact(nsx, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                DateTime hsdDate = DateTime.ParseExact(hsd, "dd/MM/yyyy", CultureInfo.InvariantCulture);

                BLL_b.CreateBatch(idSanPham, soLuongNhap, giaNhap, nsxDate, hsdDate, trangThai);
                string lo = BLL_b.getIDbathNew();
                BLL_p.CreatePurchaseDetail(id_purchase, lo);
            }

            MessageBox.Show("Tạo hóa đơn nhập thành công!", "Thông tin", MessageBoxButtons.OK, MessageBoxIcon.Information);
            TempLoSanPhamTable.Clear();
            MaLo_List.Clear();
            cbo_staff.SelectedValue = -1;
            cbo_supplier.SelectedValue = -1;
            cbo_medicine.SelectedValue = -1;
            _mainForm.container(new frm_DonNhap(_mainForm));
        }

        private void btn_close_Click(object sender, EventArgs e)
        {
            _mainForm.container(new frm_DonNhap(_mainForm));
        }

        private void guna2ShadowPanel1_Click(object sender, EventArgs e)
        {
            btn_delete.Visible = false;
            btn_add.Visible = true;
        }

        private void dgv_data2_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            CalculateTotal();
        }

        private void label6_Click(object sender, EventArgs e)
        {

        }
    }
}