using System;
using System.Collections.Generic;

namespace BaiTap5_5.DTO
{
    public class PhieuKhoDTO
    {
        public int MaPhieu { get; set; }
        public string LoaiPhieu { get; set; } = "NHAP";
        public DateTime NgayLap { get; set; } = DateTime.Now;
        public int? MaNCC { get; set; }
        public decimal TongTien { get; set; }
        public List<ChiTietPhieuKhoDTO> ChiTiet { get; set; } = new List<ChiTietPhieuKhoDTO>();
    }

    public class ChiTietPhieuKhoDTO
    {
        public int MaPhieu { get; set; }
        public string MaVatTu { get; set; } = string.Empty;
        public decimal SoLuong { get; set; }
        public decimal DonGia { get; set; }
        public decimal ThanhTien => SoLuong * DonGia;
    }
}