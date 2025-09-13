namespace DAn_QLBanhNgot
{
    partial class frm_ShowCTHDB
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.btnTT = new System.Windows.Forms.Button();
            this.panel4 = new System.Windows.Forms.Panel();
            this.cboTT = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.dgvCT = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnIN = new System.Windows.Forms.Button();
            this.txtMaHD = new System.Windows.Forms.Label();
            this.hd = new System.Windows.Forms.Label();
            this.txtNgay = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txtTenKH = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.txtTongTien = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.pnPrint = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCT)).BeginInit();
            this.pnPrint.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // btnTT
            // 
            this.btnTT.BackColor = System.Drawing.Color.MediumTurquoise;
            this.btnTT.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.125F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTT.Location = new System.Drawing.Point(776, 524);
            this.btnTT.Margin = new System.Windows.Forms.Padding(2);
            this.btnTT.Name = "btnTT";
            this.btnTT.Size = new System.Drawing.Size(103, 49);
            this.btnTT.TabIndex = 85;
            this.btnTT.Text = "Cập nhật TT";
            this.btnTT.UseVisualStyleBackColor = false;
            this.btnTT.Click += new System.EventHandler(this.btnTT_Click);
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.panel4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel4.Controls.Add(this.cboTT);
            this.panel4.Location = new System.Drawing.Point(663, 149);
            this.panel4.Margin = new System.Windows.Forms.Padding(2, 1, 2, 1);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(238, 36);
            this.panel4.TabIndex = 84;
            // 
            // cboTT
            // 
            this.cboTT.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cboTT.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cboTT.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.125F);
            this.cboTT.FormattingEnabled = true;
            this.cboTT.Items.AddRange(new object[] {
            "Chờ TT",
            "Hoàn tất"});
            this.cboTT.Location = new System.Drawing.Point(2, 4);
            this.cboTT.Margin = new System.Windows.Forms.Padding(2);
            this.cboTT.MinimumSize = new System.Drawing.Size(211, 0);
            this.cboTT.Name = "cboTT";
            this.cboTT.Size = new System.Drawing.Size(233, 28);
            this.cboTT.TabIndex = 49;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(651, 124);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(135, 18);
            this.label5.TabIndex = 83;
            this.label5.Text = "Trạng thái hóa đơn:";
            // 
            // dgvCT
            // 
            this.dgvCT.AllowUserToAddRows = false;
            this.dgvCT.AllowUserToDeleteRows = false;
            this.dgvCT.AllowUserToResizeColumns = false;
            this.dgvCT.AllowUserToResizeRows = false;
            this.dgvCT.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvCT.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvCT.BackgroundColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.ActiveCaption;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.125F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.GradientActiveCaption;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvCT.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvCT.ColumnHeadersHeight = 60;
            this.dgvCT.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvCT.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column3,
            this.Column4,
            this.Column5,
            this.Column2});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.125F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.Desktop;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.ActiveCaption;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvCT.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgvCT.GridColor = System.Drawing.SystemColors.HighlightText;
            this.dgvCT.Location = new System.Drawing.Point(40, 187);
            this.dgvCT.Margin = new System.Windows.Forms.Padding(2, 1, 2, 1);
            this.dgvCT.MultiSelect = false;
            this.dgvCT.Name = "dgvCT";
            this.dgvCT.ReadOnly = true;
            this.dgvCT.RowHeadersVisible = false;
            this.dgvCT.RowHeadersWidth = 82;
            this.dgvCT.RowTemplate.Height = 41;
            this.dgvCT.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvCT.Size = new System.Drawing.Size(860, 329);
            this.dgvCT.TabIndex = 80;
            // 
            // Column1
            // 
            this.Column1.HeaderText = "Mã danh mục";
            this.Column1.MinimumWidth = 10;
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            // 
            // Column3
            // 
            this.Column3.HeaderText = "Tên danh mục";
            this.Column3.MinimumWidth = 10;
            this.Column3.Name = "Column3";
            this.Column3.ReadOnly = true;
            // 
            // Column4
            // 
            this.Column4.HeaderText = "Số lượng";
            this.Column4.MinimumWidth = 10;
            this.Column4.Name = "Column4";
            this.Column4.ReadOnly = true;
            // 
            // Column5
            // 
            this.Column5.HeaderText = "Đơn giá";
            this.Column5.MinimumWidth = 10;
            this.Column5.Name = "Column5";
            this.Column5.ReadOnly = true;
            // 
            // Column2
            // 
            this.Column2.HeaderText = "Thành tiền";
            this.Column2.MinimumWidth = 6;
            this.Column2.Name = "Column2";
            this.Column2.ReadOnly = true;
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(900, 2);
            this.btnClose.Margin = new System.Windows.Forms.Padding(2);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(42, 31);
            this.btnClose.TabIndex = 78;
            this.btnClose.Text = "X";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnIN
            // 
            this.btnIN.BackColor = System.Drawing.Color.Gainsboro;
            this.btnIN.Location = new System.Drawing.Point(853, 2);
            this.btnIN.Margin = new System.Windows.Forms.Padding(2);
            this.btnIN.Name = "btnIN";
            this.btnIN.Size = new System.Drawing.Size(43, 31);
            this.btnIN.TabIndex = 77;
            this.btnIN.Text = "IN";
            this.btnIN.UseVisualStyleBackColor = false;
            this.btnIN.Click += new System.EventHandler(this.btnIN_Click);
            // 
            // txtMaHD
            // 
            this.txtMaHD.AutoSize = true;
            this.txtMaHD.Font = new System.Drawing.Font("Times New Roman", 12F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMaHD.ForeColor = System.Drawing.SystemColors.ControlText;
            this.txtMaHD.Location = new System.Drawing.Point(150, 103);
            this.txtMaHD.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.txtMaHD.Name = "txtMaHD";
            this.txtMaHD.Size = new System.Drawing.Size(70, 23);
            this.txtMaHD.TabIndex = 75;
            this.txtMaHD.Text = "HĐ001";
            // 
            // hd
            // 
            this.hd.AutoSize = true;
            this.hd.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.hd.ForeColor = System.Drawing.SystemColors.ControlText;
            this.hd.Location = new System.Drawing.Point(36, 104);
            this.hd.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.hd.Name = "hd";
            this.hd.Size = new System.Drawing.Size(110, 22);
            this.hd.TabIndex = 74;
            this.hd.Text = "Mã hóa đơn:";
            // 
            // txtNgay
            // 
            this.txtNgay.AutoSize = true;
            this.txtNgay.Font = new System.Drawing.Font("Times New Roman", 12F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtNgay.ForeColor = System.Drawing.SystemColors.ControlText;
            this.txtNgay.Location = new System.Drawing.Point(107, 72);
            this.txtNgay.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.txtNgay.Name = "txtNgay";
            this.txtNgay.Size = new System.Drawing.Size(102, 23);
            this.txtNgay.TabIndex = 72;
            this.txtNgay.Text = "20/05/2024";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label4.Location = new System.Drawing.Point(36, 73);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(57, 22);
            this.label4.TabIndex = 71;
            this.label4.Text = "Ngày:";
            // 
            // txtTenKH
            // 
            this.txtTenKH.AutoSize = true;
            this.txtTenKH.Font = new System.Drawing.Font("Times New Roman", 13.125F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTenKH.Location = new System.Drawing.Point(205, 137);
            this.txtTenKH.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.txtTenKH.Name = "txtTenKH";
            this.txtTenKH.Size = new System.Drawing.Size(342, 25);
            this.txtTenKH.TabIndex = 64;
            this.txtTenKH.Text = ".......................................................";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label9.Location = new System.Drawing.Point(60, 140);
            this.label9.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(137, 22);
            this.label9.TabIndex = 61;
            this.label9.Text = "Tên khách hàng:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Times New Roman", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label8.Location = new System.Drawing.Point(356, 61);
            this.label8.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(228, 35);
            this.label8.TabIndex = 60;
            this.label8.Text = "HÓA ĐƠN BÁN";
            // 
            // txtTongTien
            // 
            this.txtTongTien.AutoSize = true;
            this.txtTongTien.Font = new System.Drawing.Font("Times New Roman", 13.875F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTongTien.ForeColor = System.Drawing.SystemColors.ControlText;
            this.txtTongTien.Location = new System.Drawing.Point(172, 542);
            this.txtTongTien.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.txtTongTien.Name = "txtTongTien";
            this.txtTongTien.Size = new System.Drawing.Size(129, 26);
            this.txtTongTien.TabIndex = 50;
            this.txtTongTien.Text = "99999 VNĐ";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Times New Roman", 13.875F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label2.Location = new System.Drawing.Point(59, 541);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(109, 27);
            this.label2.TabIndex = 49;
            this.label2.Text = "Tổng tiền:";
            // 
            // pnPrint
            // 
            this.pnPrint.BackColor = System.Drawing.Color.White;
            this.pnPrint.Controls.Add(this.pictureBox1);
            this.pnPrint.Controls.Add(this.label1);
            this.pnPrint.Controls.Add(this.btnTT);
            this.pnPrint.Controls.Add(this.panel4);
            this.pnPrint.Controls.Add(this.label5);
            this.pnPrint.Controls.Add(this.dgvCT);
            this.pnPrint.Controls.Add(this.btnClose);
            this.pnPrint.Controls.Add(this.btnIN);
            this.pnPrint.Controls.Add(this.txtMaHD);
            this.pnPrint.Controls.Add(this.hd);
            this.pnPrint.Controls.Add(this.txtNgay);
            this.pnPrint.Controls.Add(this.label4);
            this.pnPrint.Controls.Add(this.txtTenKH);
            this.pnPrint.Controls.Add(this.label9);
            this.pnPrint.Controls.Add(this.label8);
            this.pnPrint.Controls.Add(this.txtTongTien);
            this.pnPrint.Controls.Add(this.label2);
            this.pnPrint.Location = new System.Drawing.Point(11, 11);
            this.pnPrint.Margin = new System.Windows.Forms.Padding(2);
            this.pnPrint.Name = "pnPrint";
            this.pnPrint.Size = new System.Drawing.Size(943, 584);
            this.pnPrint.TabIndex = 53;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(71, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(298, 20);
            this.label1.TabIndex = 86;
            this.label1.Text = "Cửa hàng: Bánh ngọt Quỳnh Trang";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::DAn_QLBanhNgot.Properties.Resources.birthday_cake;
            this.pictureBox1.Location = new System.Drawing.Point(12, 13);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(53, 48);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 87;
            this.pictureBox1.TabStop = false;
            // 
            // frm_ShowCTHDB
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Honeydew;
            this.ClientSize = new System.Drawing.Size(962, 606);
            this.Controls.Add(this.pnPrint);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frm_ShowCTHDB";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frm_ShowCTHDB";
            this.panel4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvCT)).EndInit();
            this.pnPrint.ResumeLayout(false);
            this.pnPrint.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnTT;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.ComboBox cboTT;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.DataGridView dgvCT;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnIN;
        private System.Windows.Forms.Label txtMaHD;
        private System.Windows.Forms.Label hd;
        private System.Windows.Forms.Label txtNgay;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label txtTenKH;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label txtTongTien;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel pnPrint;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column4;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column5;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}