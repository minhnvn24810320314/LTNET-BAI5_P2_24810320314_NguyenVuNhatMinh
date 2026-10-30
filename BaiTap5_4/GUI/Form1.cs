using System;
using System.Data;
using System.Windows.Forms;
using BaiTap5_4.BUS;
using BaiTap5_4.DTO;

namespace BaiTap5_4.GUI
{
    public partial class Form1 : Form
    {
        private HotelBUS bus = new HotelBUS();
        private DataTable dtPhong;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            cboLocTrangThai.Items.Clear();
            cboLocTrangThai.Items.AddRange(new object[] { "Tất cả", "Trống", "Đang ở", "Đang dọn dẹp" });
            cboLocTrangThai.SelectedIndex = 0;
            LoadData();
        }

        private void LoadData()
        {
            dtPhong = bus.LayDanhSachPhong();
            dgvPhong.DataSource = dtPhong;
        }

        private void cboLocTrangThai_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (dtPhong != null)
            {
                DataView dv = dtPhong.DefaultView;
                string loc = cboLocTrangThai.SelectedItem?.ToString();

                if (string.IsNullOrEmpty(loc) || loc == "Tất cả")
                    dv.RowFilter = "";
                else
                    dv.RowFilter = $"TrangThai = '{loc}'";

                dgvPhong.DataSource = dv;
            }
        }

        private void btnCheckIn_Click(object sender, EventArgs e)
        {
            string cccd = txtCCCD.Text.Trim();
            string soPhong = txtSoPhong.Text.Trim();

            if (string.IsNullOrEmpty(cccd) || string.IsNullOrEmpty(soPhong))
            {
                MessageBox.Show("Vui lòng nhập CCCD và Số phòng!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            KhachHangDTO kh = bus.TimKhachHangByCCCD(cccd);
            int maKH;

            if (kh == null)
            {
                KhachHangDTO khMoi = new KhachHangDTO
                {
                    HoTen = txtHoTen.Text.Trim(),
                    CCCD = cccd,
                    SoDienThoai = txtSDT.Text.Trim()
                };
                maKH = bus.ThemKhachHang(khMoi);
            }
            else
            {
                maKH = kh.MaKH;
            }

            if (bus.CheckIn(soPhong, maKH))
            {
                MessageBox.Show($"Check-in phòng {soPhong} thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadData();
            }
            else
            {
                MessageBox.Show("Check-in thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCheckOut_Click(object sender, EventArgs e)
        {
            string soPhong = txtSoPhong.Text.Trim();
            if (bus.CheckOut(soPhong, out decimal tongTien))
            {
                MessageBox.Show($"Check-out phòng {soPhong} thành công!\nTổng tiền thanh toán: {tongTien:N0} VNĐ", "Hóa đơn", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadData();
            }
            else
            {
                MessageBox.Show("Check-out thất bại hoặc phòng chưa có khách!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}