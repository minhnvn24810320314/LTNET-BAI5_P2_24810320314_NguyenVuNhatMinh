using System;
using System.Data;
using BaiTap5_3.DAL;

namespace BaiTap5_3.BUS
{
    public class CafeBUS
    {
        private CafeDAL dal = new CafeDAL();

        public DataTable LayDanhSachBan() => dal.LayDanhSachBan();
        public DataTable LayDanhSachMon() => dal.LayDanhSachMon();
        public DataTable LayChiTietHoaDonByBan(int maBan) => dal.LayChiTietHoaDonByBan(maBan);

        public void GoiMon(int maBan, int maMon, int soLuong, decimal donGia)
        {
            if (soLuong <= 0) throw new Exception("Số lượng món phải lớn hơn 0!");
            dal.GoiMon(maBan, maMon, soLuong, donGia);
        }

        public bool ThanhToan(int maBan, out decimal tongTien)
        {
            return dal.ThanhToan(maBan, out tongTien);
        }
    }
}