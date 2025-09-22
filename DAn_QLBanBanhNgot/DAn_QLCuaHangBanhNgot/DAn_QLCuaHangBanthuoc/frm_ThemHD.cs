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
    public partial class frm_ThemHD : Form
    {
        private readonly frm_Main _mainForm;
        BLL_sale bll = new BLL_sale();
        List<string> ID_medicine = new List<string>();
        List<int> stock_quantity = new List<int>();
        DataTable TableMedicine;

        public frm_ThemHD(frm_Main mainForm)
        {
            InitializeComponent();
            _mainForm = mainForm;
            bll = new BLL_sale();
            load();
        }
        void load()
        {
            DataTable staff = bll.GetStaff();
            DataRow row = staff.NewRow();
            row["MaNV"] = -1;
            row["TenNV"] = "-- Chọn nhân viên --";
            staff.Rows.InsertAt(row, 0);
            cbo_staff.DataSource = staff;
            cbo_staff.DisplayMember = "TenNV";
            cbo_staff.ValueMember = "MaNV";
            cbo_staff.SelectedValue = -1;

            DataTable c = bll.GetCustomer();
            DataRow row2 = c.NewRow();
            row2["MaKH"] = -1;
            row2["TenKH"] = "-- Chọn khách hàng --";
            c.Rows.InsertAt(row2, 0);
            cbo_customer.DataSource = c;
            cbo_customer.DisplayMember = "TenKH";
            cbo_customer.ValueMember = "MaKH";
            cbo_customer.SelectedValue = -1;

            TableMedicine = bll.GetProduct();
            DataRow row3 = TableMedicine.NewRow();
            row3["MaSP"] = -1;
            row3["TenSP"] = "-- Chọn sản phẩm --";
            TableMedicine.Rows.InsertAt(row3, 0);
            cbo_medicine.DataSource = TableMedicine;
            cbo_medicine.DisplayMember = "TenSP";
            cbo_medicine.ValueMember = "MaSP";
            cbo_medicine.SelectedValue = -1;
        }

        private void btn_close_Click(object sender, EventArgs e)
        {
            _mainForm.container(new frm_HoaDon(_mainForm));
        }

        private void UpdateTotalAmount()
        {
            decimal totalAmount = 0;
            foreach (DataGridViewRow row in dgv_data.Rows)
            {
                if (!row.IsNewRow)
                {
                    int quantity = Convert.ToInt32(row.Cells[1].Value); 
                    decimal price = Convert.ToDecimal(row.Cells[2].Value); 
                    totalAmount += quantity * price;
                }
            }
            lbl_price.Text = totalAmount.ToString("N0") + " VND";
        }
        private void btn_add_Click(object sender, EventArgs e)
        {
            if (cbo_medicine.SelectedValue == null || cbo_medicine.SelectedValue.ToString() == "-1")
            {
                MessageBox.Show("Vui lòng chọn một sản phẩm từ danh sách!", "Cảnh báo!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            string selectedMedicineId = cbo_medicine.SelectedValue.ToString();
            if (ID_medicine.Contains(selectedMedicineId))
            {
                MessageBox.Show("Sản phẩm đã được thêm vào hóa đơn!", "Cảnh báo!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            foreach (DataRow row in TableMedicine.Rows)
            {
                if (selectedMedicineId == row["MaSP"].ToString())
                {
                    dgv_data.Rows.Add(row["TenSP"], 1, row["Gia"]);
                    ID_medicine.Add(selectedMedicineId);
                    stock_quantity.Add(Convert.ToInt32(row["SoLuong"]));
                    UpdateTotalAmount();
                    return; 
                }
            }
        }

        private void btn_delete_Click(object sender, EventArgs e)
        {
            if (dgv_data.SelectedRows.Count > 0)
            {
                int selectedIndex = dgv_data.SelectedRows[0].Index;
                dgv_data.Rows.RemoveAt(selectedIndex);
                if (selectedIndex >= 0 && selectedIndex < ID_medicine.Count)
                {
                    ID_medicine.RemoveAt(selectedIndex);
                    stock_quantity.RemoveAt(selectedIndex);
                }
                UpdateTotalAmount();
                btn_delete.Visible = false;
                btn_add.Visible = true;
            }
        }

        private void btn_new_sale_Click(object sender, EventArgs e)
        {
            if (dgv_data.Rows.Count == 0 || (dgv_data.Rows.Count == 1 && dgv_data.Rows[0].IsNewRow))
            {
                MessageBox.Show("Vui lòng thêm ít nhất một sản phẩm vào hóa đơn!", "Cảnh báo!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (cbo_staff.SelectedValue == null || cbo_staff.SelectedValue.ToString() == "-1")
            {
                MessageBox.Show("Vui lòng chọn nhân viên!", "Cảnh báo!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (cbo_customer.SelectedValue == null || cbo_customer.SelectedValue.ToString() == "-1")
            {
                MessageBox.Show("Vui lòng chọn một khách hàng!", "Cảnh báo!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            
            for (int i = 0; i < dgv_data.Rows.Count - 1; i++)
            {
                if (!int.TryParse(dgv_data.Rows[i].Cells[1].Value?.ToString(), out int quantity) || quantity < 1)
                {
                    MessageBox.Show("Số lượng sản phẩm phải là một số lớn hơn 0!", "Cảnh báo!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (quantity > stock_quantity[i])
                {
                    string id = dgv_data.Rows[i].Cells[0].Value.ToString();
                    MessageBox.Show($"Số lượng sản phẩm có ID {id} vượt quá số lượng có sẵn trong kho: {stock_quantity[i]}!", "Cảnh báo!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }
            if (MessageBox.Show("Bạn có chắc chắn muốn tạo hóa đơn này không?", "Câu hỏi", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    bll.AddRecord(cbo_staff.SelectedValue.ToString(), cbo_customer.SelectedValue.ToString());
                    string idHDBnew = bll.GetIDHDB();

                    for (int i = 0; i < dgv_data.Rows.Count - 1; i++) 
                    {
                        string idMedicine = ID_medicine[i]; 
                        string quantity = dgv_data.Rows[i].Cells[1].Value.ToString(); 
                        string price = dgv_data.Rows[i].Cells[2].Value.ToString(); 
                        bll.AddDetails(idHDBnew, idMedicine, quantity, price);
                    }
                    MessageBox.Show("Tạo đơn bán thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    dgv_data.Rows.Clear();
                    ID_medicine.Clear();
                    stock_quantity.Clear();
                    _mainForm.container(new frm_HoaDon(_mainForm));
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi tạo đơn: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void dgv_data_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            btn_delete.Visible = true;
            btn_add.Visible = false;
        }

        private void guna2ShadowPanel1_Click(object sender, EventArgs e)
        {
            btn_delete.Visible = false;
            btn_add.Visible = true;
        }

        private void cbo_medicine_SelectedIndexChanged(object sender, EventArgs e)
        {
            btn_delete.Visible = false;
            btn_add.Visible = true;
        }
    }
}
