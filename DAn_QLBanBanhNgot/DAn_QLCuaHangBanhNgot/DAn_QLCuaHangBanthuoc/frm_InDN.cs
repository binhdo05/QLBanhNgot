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
    public partial class frm_InDN : Form
    {
        private readonly frm_Main _mainForm;
        private readonly string _idPurchase;
        BLL_purchase bll_purchase;
        
        public frm_InDN(frm_Main mainForm, string id)
        {
            InitializeComponent();
            _mainForm = mainForm;
            _idPurchase = id;
            bll_purchase = new BLL_purchase();

            DataTable tb = bll_purchase.GetDataBatchID(_idPurchase);
            DataRow row = bll_purchase.GetRow(id);
            lbl_supplier.Text = row[3].ToString();
            lbl_staff.Text = row[4].ToString();
            decimal total_price = 0;
            lbl_date.Text = Convert.ToDateTime(row[1]).ToString("dd/MM/yyyy");

            if (tb.Rows.Count > 0)
            {
                foreach (DataRow r in tb.Rows)
                {
                    
                    dgv_data.Rows.Add(
                                    r["id_batch"],           
                                    r["id_medicine"],        // r[1]
                                    r["name_medicine"],      // r[2]
                                    r["quantity_in_batch"],  // r[3]
                                    r["entry_price"],        // r[4]
                                    Convert.ToDateTime(r["manufacturing_date"]).ToString("MM/dd/yyyy"),
                                    Convert.ToDateTime(r["expiry_date"]).ToString("MM/dd/yyyy")
                                );
                    total_price = Convert.ToDecimal(r["total_amount"]);
                }
                lbl_total_price.Text = total_price + " VNĐ";
            }
        }
       
        private void btn_close_Click(object sender, EventArgs e)
        {
            _mainForm.container(new frm_DonNhap(_mainForm));
        }
    }
}
