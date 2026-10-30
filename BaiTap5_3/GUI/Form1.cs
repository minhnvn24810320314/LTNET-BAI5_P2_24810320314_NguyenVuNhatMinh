using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using BaiTap5_3.BUS;

namespace BaiTap5_3.GUI
{
    public partial class Form1 : Form
    {
        private CafeBUS bus = new CafeBUS();
        private int currentMaBan = -1;

        public Form1()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            LoadSoDoBan();
            LoadDanhSachMon();
        }

        // Tạo sơ đồ bàn động dạng Button
        private void LoadSoDoBan()
        {
            flpBanAn.Controls.Clear();
            DataTable dtBan = bus.LayDanhSachBan();

            foreach (DataRow row in dtBan.Rows)
            {
                Button btn = new Button();
                btn.Width = 90;
                btn.Height = 80;
                int maBan = Convert.ToInt32(row["MaBan"]);
                string trangThai = row["TrangThai"].ToString();

                btn.Text = $"{row["TenBan"]}\n({trangThai})";
                btn.Tag = maBan;

                // Quy định màu sắc bàn
                if (trangThai == "Trống")
                    btn.BackColor = Color.LightGreen;
                else
                    btn.BackColor = Color.Salmon;

                btn.Click += BtnBan_Click;
                flpBanAn.Controls.Add(btn);
            }
        }

        private void LoadDanhSachMon()
        {
            cboMonAn.DataSource = bus.LayDanhSachMon();
            cboMonAn.DisplayMember = "TenMon";
            cboMonAn.ValueMember = "MaMon";
        }

        private void BtnBan_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            currentMaBan = Convert.ToInt32(btn.Tag);
            HienThiHoaDonBan(currentMaBan);
        }

        private void HienThiHoaDonBan(int maBan)
        {
            dgvChiTietHD.DataSource = bus.LayChiTietHoaDonByBan(maBan);
        }

        private void btnGoiMon_Click(object sender, EventArgs e)
        {
            // Bắt lỗi Validation 1: Chưa chọn bàn
            if (currentMaBan == -1)
            {
                MessageBox.Show("Vui lòng chọn bàn trước khi gọi món!", "Cảnh báo Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Bắt lỗi Validation 2: Số lượng nhỏ hơn hoặc bằng 0
            if (nudSoLuong.Value <= 0)
            {
                MessageBox.Show("Số lượng món ăn phải lớn hơn 0 và không được để trống!", "Lỗi Validation", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                DataRowView drv = cboMonAn.SelectedItem as DataRowView;
                int maMon = Convert.ToInt32(drv["MaMon"]);
                decimal donGia = Convert.ToDecimal(drv["DonGia"]);
                int soLuong = (int)nudSoLuong.Value;

                bus.GoiMon(currentMaBan, maMon, soLuong, donGia);
                HienThiHoaDonBan(currentMaBan);
                LoadSoDoBan();
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Thông báo lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnThanhToan_Click(object sender, EventArgs e)
        {
            if (currentMaBan == -1)
            {
                MessageBox.Show("Vui lòng chọn bàn cần thanh toán!");
                return;
            }

            if (bus.ThanhToan(currentMaBan, out decimal tongTien))
            {
                MessageBox.Show($"Thanh toán thành công!\nTổng tiền: {tongTien:N0} VNĐ");
                HienThiHoaDonBan(currentMaBan);
                LoadSoDoBan(); // Làm mới sơ đồ bàn
            }
            else
            {
                MessageBox.Show("Bàn này hiện chưa có hóa đơn cần thanh toán.");
            }
        }
    }
}