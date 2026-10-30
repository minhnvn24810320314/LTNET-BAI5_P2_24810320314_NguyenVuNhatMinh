using System.Collections.Generic;
using BaiTap5_5.DAL;
using BaiTap5_5.DTO;

namespace BaiTap5_5.BUS
{
    public class InventoryBUS
    {
        private InventoryDAL dal = new InventoryDAL();

        public List<VatTuDTO> LayDanhSachVatTu() => dal.GetAllVatTu();

        public List<VatTuDTO> TimKiemVatTu(string kw) => dal.SearchVatTu(kw);

        public bool LuuPhieuKho(PhieuKhoDTO phieu, out string msg)
        {
            if (phieu.ChiTiet == null || phieu.ChiTiet.Count == 0)
            {
                msg = "Danh sách chi tiết phiếu kho không được để trống!";
                return false;
            }
            return dal.LuuPhieuKhoTransaction(phieu, out msg);
        }

        public decimal LayTongGiaTriTon() => dal.TinhTongGiaTriTonKho();
    }
}