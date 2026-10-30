namespace BaiTap5_5.GUI
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            dgvVatTu = new DataGridView();
            txtTimKiem = new TextBox();
            cboLoaiPhieu = new ComboBox();
            btnLuuPhieu = new Button();
            btnTongTien = new Button();
            lblKetQua = new Label();
            lblTimKiem = new Label();
            lblLoaiPhieu = new Label();
            grpPhieuKho = new GroupBox();
            ((System.ComponentModel.ISupportInitialize)dgvVatTu).BeginInit();
            grpPhieuKho.SuspendLayout();
            SuspendLayout();
            // 
            // dgvVatTu
            // 
            dgvVatTu.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvVatTu.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvVatTu.Location = new Point(12, 180);
            dgvVatTu.Name = "dgvVatTu";
            dgvVatTu.RowHeadersWidth = 51;
            dgvVatTu.Size = new Size(760, 240);
            dgvVatTu.TabIndex = 0;
            // 
            // lblTimKiem
            // 
            lblTimKiem.AutoSize = true;
            lblTimKiem.Location = new Point(12, 22);
            lblTimKiem.Name = "lblTimKiem";
            lblTimKiem.Size = new Size(73, 20);
            lblTimKiem.TabIndex = 6;
            lblTimKiem.Text = "Tìm kiếm:";
            // 
            // txtTimKiem
            // 
            txtTimKiem.Location = new Point(90, 19);
            txtTimKiem.Name = "txtTimKiem";
            txtTimKiem.Size = new Size(200, 27);
            txtTimKiem.TabIndex = 1;
            txtTimKiem.TextChanged += txtTimKiem_TextChanged;
            // 
            // btnTongTien
            // 
            btnTongTien.BackColor = Color.LightSkyBlue;
            btnTongTien.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnTongTien.Location = new Point(310, 16);
            btnTongTien.Name = "btnTongTien";
            btnTongTien.Size = new Size(180, 32);
            btnTongTien.TabIndex = 4;
            btnTongTien.Text = "Tính Tổng Giá Trị Tồn";
            btnTongTien.UseVisualStyleBackColor = false;
            btnTongTien.Click += btnTongTien_Click;
            // 
            // lblKetQua
            // 
            lblKetQua.AutoSize = true;
            lblKetQua.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblKetQua.ForeColor = Color.DarkBlue;
            lblKetQua.Location = new Point(500, 20);
            lblKetQua.Name = "lblKetQua";
            lblKetQua.Size = new Size(205, 23);
            lblKetQua.TabIndex = 5;
            lblKetQua.Text = "Tổng giá trị: 0 VNĐ";
            // 
            // grpPhieuKho
            // 
            grpPhieuKho.Controls.Add(cboLoaiPhieu);
            grpPhieuKho.Controls.Add(lblLoaiPhieu);
            grpPhieuKho.Controls.Add(btnLuuPhieu);
            grpPhieuKho.Location = new Point(12, 65);
            grpPhieuKho.Name = "grpPhieuKho";
            grpPhieuKho.Size = new Size(760, 95);
            grpPhieuKho.TabIndex = 8;
            grpPhieuKho.TabStop = false;
            grpPhieuKho.Text = "Tạo Giao Dịch Nhập / Xuất Kho (SqlTransaction)";
            // 
            // lblLoaiPhieu
            // 
            lblLoaiPhieu.AutoSize = true;
            lblLoaiPhieu.Location = new Point(15, 40);
            lblLoaiPhieu.Name = "lblLoaiPhieu";
            lblLoaiPhieu.Size = new Size(81, 20);
            lblLoaiPhieu.TabIndex = 7;
            lblLoaiPhieu.Text = "Loại phiếu:";
            // 
            // cboLoaiPhieu
            // 
            cboLoaiPhieu.DropDownStyle = ComboBoxStyle.DropDownList;
            cboLoaiPhieu.FormattingEnabled = true;
            cboLoaiPhieu.Items.AddRange(new object[] { "NHAP", "XUAT" });
            cboLoaiPhieu.Location = new Point(102, 37);
            cboLoaiPhieu.Name = "cboLoaiPhieu";
            cboLoaiPhieu.Size = new Size(150, 28);
            cboLoaiPhieu.TabIndex = 2;
            // 
            // btnLuuPhieu
            // 
            btnLuuPhieu.BackColor = Color.LightGreen;
            btnLuuPhieu.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnLuuPhieu.Location = new Point(270, 35);
            btnLuuPhieu.Name = "btnLuuPhieu";
            btnLuuPhieu.Size = new Size(160, 32);
            btnLuuPhieu.TabIndex = 3;
            btnLuuPhieu.Text = "Lưu Phiếu Kho";
            btnLuuPhieu.UseVisualStyleBackColor = false;
            btnLuuPhieu.Click += btnLuuPhieu_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(784, 433);
            Controls.Add(grpPhieuKho);
            Controls.Add(lblTimKiem);
            Controls.Add(lblKetQua);
            Controls.Add(btnTongTien);
            Controls.Add(txtTimKiem);
            Controls.Add(dgvVatTu);
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Bài 5.5 - Quản Lý Xuất Nhập Kho Nguyên Vật Liệu";
            Load += Form1_Load;
            ((System.ComponentModel.ISupportInitialize)dgvVatTu).EndInit();
            grpPhieuKho.ResumeLayout(false);
            grpPhieuKho.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private DataGridView dgvVatTu;
        private TextBox txtTimKiem;
        private ComboBox cboLoaiPhieu;
        private Button btnLuuPhieu;
        private Button btnTongTien;
        private Label lblKetQua;
        private Label lblTimKiem;
        private Label lblLoaiPhieu;
        private GroupBox grpPhieuKho;
    }
}