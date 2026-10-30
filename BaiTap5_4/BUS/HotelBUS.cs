using System.Data;
using BaiTap5_4.DAL;
using BaiTap5_4.DTO;

namespace BaiTap5_4.BUS
{
    public class HotelBUS
    {
        private HotelDAL dal = new HotelDAL();

        public DataTable LayDanhSachPhong()
        {
            return dal.LayDanhSachPhong();
        }

        public KhachHangDTO TimKhachHangByCCCD(string cccd)
        {
            return dal.TimKhachHangByCCCD(cccd);
        }

        public int ThemKhachHang(KhachHangDTO kh)
        {
            return dal.ThemKhachHang(kh);
        }

        public bool CheckIn(string soPhong, int maKH)
        {
            return dal.CheckIn(soPhong, maKH);
        }

        public bool CheckOut(string soPhong, out decimal tongTien)
        {
            return dal.CheckOut(soPhong, out tongTien);
        }
    }
}