using System;
using System.Data;
using Microsoft.Data.SqlClient;

namespace BaiTap5_3.DAL
{
    public class CafeDAL
    {
        private string connString = @"Server=(localdb)\MSSQLLocalDB;Database=CafePOSDB;Trusted_Connection=True;";

        // Lấy danh sách toàn bộ bàn ăn
        public DataTable LayDanhSachBan()
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(connString))
            {
                SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM BanAn", conn);
                da.Fill(dt);
            }
            return dt;
        }

        // Lấy danh sách thực đơn món ăn
        public DataTable LayDanhSachMon()
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(connString))
            {
                SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM MonAn", conn);
                da.Fill(dt);
            }
            return dt;
        }

        // Lấy mã hóa đơn chưa thanh toán của bàn (TrangThai = 0)
        public int LayMaHoaDonChuaThanhToan(int maBan)
        {
            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT TOP 1 MaHoaDon FROM HoaDon WHERE MaBan = @MaBan AND TrangThai = 0", conn);
                cmd.Parameters.AddWithValue("@MaBan", maBan);
                object res = cmd.ExecuteScalar();
                return res != null ? Convert.ToInt32(res) : -1;
            }
        }

        // Xử lý gọi món / đặt món
        public void GoiMon(int maBan, int maMon, int soLuong, decimal donGia)
        {
            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();
                int maHD = LayMaHoaDonChuaThanhToan(maBan);

                // Nếu chưa có hóa đơn tạm -> Tạo mới hóa đơn & Đổi bàn sang 'Có khách'
                if (maHD == -1)
                {
                    SqlCommand cmdHD = new SqlCommand("INSERT INTO HoaDon (MaBan, GioVao, TrangThai) VALUES (@MaBan, GETDATE(), 0); SELECT SCOPE_IDENTITY();", conn);
                    cmdHD.Parameters.AddWithValue("@MaBan", maBan);
                    maHD = Convert.ToInt32(cmdHD.ExecuteScalar());

                    SqlCommand cmdBan = new SqlCommand("UPDATE BanAn SET TrangThai = N'Có khách' WHERE MaBan = @MaBan", conn);
                    cmdBan.Parameters.AddWithValue("@MaBan", maBan);
                    cmdBan.ExecuteNonQuery();
                }

                // Cập nhật hoặc thêm món mới vào chi tiết hóa đơn
                string sqlCT = @"IF EXISTS (SELECT 1 FROM ChiTietHoaDon WHERE MaHoaDon = @MaHD AND MaMon = @MaMon)
                                    UPDATE ChiTietHoaDon SET SoLuong = SoLuong + @SoLuong WHERE MaHoaDon = @MaHD AND MaMon = @MaMon
                                 ELSE
                                    INSERT INTO ChiTietHoaDon (MaHoaDon, MaMon, SoLuong, DonGia) VALUES (@MaHD, @MaMon, @SoLuong, @DonGia)";
                SqlCommand cmdCT = new SqlCommand(sqlCT, conn);
                cmdCT.Parameters.AddWithValue("@MaHD", maHD);
                cmdCT.Parameters.AddWithValue("@MaMon", maMon);
                cmdCT.Parameters.AddWithValue("@SoLuong", soLuong);
                cmdCT.Parameters.AddWithValue("@DonGia", donGia);
                cmdCT.ExecuteNonQuery();
            }
        }

        // Lấy danh sách món đã gọi của bàn để hiện trên DataGridView
        public DataTable LayChiTietHoaDonByBan(int maBan)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(connString))
            {
                string sql = @"SELECT m.TenMon, c.SoLuong, c.DonGia, (c.SoLuong * c.DonGia) AS ThanhTien
                               FROM ChiTietHoaDon c
                               JOIN MonAn m ON c.MaMon = m.MaMon
                               JOIN HoaDon h ON c.MaHoaDon = h.MaHoaDon
                               WHERE h.MaBan = @MaBan AND h.TrangThai = 0";
                SqlDataAdapter da = new SqlDataAdapter(sql, conn);
                da.SelectCommand.Parameters.AddWithValue("@MaBan", maBan);
                da.Fill(dt);
            }
            return dt;
        }

        // Thực thi Transaction cho chức năng Thanh Toán
        public bool ThanhToan(int maBan, out decimal tongTien)
        {
            tongTien = 0;
            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();
                SqlTransaction trans = conn.BeginTransaction();
                try
                {
                    int maHD = LayMaHoaDonChuaThanhToan(maBan);
                    if (maHD == -1) return false;

                    // 1. Tính tổng tiền hóa đơn
                    SqlCommand cmdSum = new SqlCommand("SELECT ISNULL(SUM(SoLuong * DonGia), 0) FROM ChiTietHoaDon WHERE MaHoaDon = @MaHD", conn, trans);
                    cmdSum.Parameters.AddWithValue("@MaHD", maHD);
                    tongTien = Convert.ToDecimal(cmdSum.ExecuteScalar());

                    // 2. Cập nhật hóa đơn (GioRa, TongTien, TrangThai = 1)
                    SqlCommand cmdHD = new SqlCommand("UPDATE HoaDon SET GioRa = GETDATE(), TongTien = @TongTien, TrangThai = 1 WHERE MaHoaDon = @MaHD", conn, trans);
                    cmdHD.Parameters.AddWithValue("@TongTien", tongTien);
                    cmdHD.Parameters.AddWithValue("@MaHD", maHD);
                    cmdHD.ExecuteNonQuery();

                    // 3. Cập nhật bàn về 'Trống'
                    SqlCommand cmdBan = new SqlCommand("UPDATE BanAn SET TrangThai = N'Trống' WHERE MaBan = @MaBan", conn, trans);
                    cmdBan.Parameters.AddWithValue("@MaBan", maBan);
                    cmdBan.ExecuteNonQuery();

                    trans.Commit();
                    return true;
                }
                catch
                {
                    trans.Rollback();
                    return false;
                }
            }
        }
    }
}