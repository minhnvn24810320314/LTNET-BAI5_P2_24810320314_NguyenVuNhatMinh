using System;
using System.Collections.Generic;
using System.Windows.Forms;
using BaiTap5_5.BUS;
using BaiTap5_5.DTO;

namespace BaiTap5_5.GUI
{
    public partial class Form1 : Form
    {
        private InventoryBUS bus = new InventoryBUS();

        public Form1()
        {
            InitializeComponent();
        }

        // Tải danh sách vật tư khi mở Form
        private void Form1_Load(object sender, EventArgs e)
        {
            cboLoaiPhieu.SelectedIndex = 0; // Chọn mặc định "NHAP"
            HienThiDanhSach();
        }

        private void HienThiDanhSach()
        {
            dgvVatTu.DataSource = bus.LayDanhSachVatTu();
        }

        // 1. Tìm kiếm vật tư bằng SqlParameter (Chống SQL Injection)
        private void txtTimKiem_TextChanged(object sender, EventArgs e)
        {
            dgvVatTu.DataSource = bus.TimKiemVatTu(txtTimKiem.Text.Trim());
        }

        // 2. Thực thi SqlTransaction Lập phiếu kho (ACID)
        private void btnLuuPhieu_Click(object sender, EventArgs e)
        {
            string loaiPhieuChon = cboLoaiPhieu.SelectedItem?.ToString() ?? "NHAP";
           
            // Tạo phiếu mẫu kiểm thử giao dịch cho vật tư VT01
            PhieuKhoDTO phieu = new PhieuKhoDTO
            {
                LoaiPhieu = loaiPhieuChon,
                NgayLap = DateTime.Now,
                TongTien = 1000000,
                ChiTiet = new List<ChiTietPhieuKhoDTO>
                {
                    new ChiTietPhieuKhoDTO { MaVatTu = "VT01", SoLuong = 5, DonGia = 200000 }
                }
            };

            if (bus.LuuPhieuKho(phieu, out string errorMsg))
            {
                MessageBox.Show($"Lập phiếu {loaiPhieuChon} thành công! Kho đã được cập nhật.", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                HienThiDanhSach(); // Tải lại bảng vật tư để thấy biến động tồn kho
            }
            else
            {
                MessageBox.Show("Giao dịch bị Rollback: " + errorMsg, "Lỗi Giao Dịch", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // 3. Thực thi ExecuteScalar tính tổng giá trị hàng tồn
        private void btnTongTien_Click(object sender, EventArgs e)
        {
            decimal tongGiaTri = bus.LayTongGiaTriTon();
            lblKetQua.Text = $"Tổng giá trị tồn kho: {tongGiaTri:N0} VNĐ";
        }
    }
}