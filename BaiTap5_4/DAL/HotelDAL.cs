using System;
using System.Data;
using Microsoft.Data.SqlClient;
using BaiTap5_4.DTO;

namespace BaiTap5_4.DAL
{
    public class HotelDAL
    {
        private string connString = @"Server=(localdb)\MSSQLLocalDB;Database=HotelDB;Trusted_Connection=True;";

        // 1. Lấy toàn bộ danh sách phòng dạng DataTable
        public DataTable LayDanhSachPhong()
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(connString))
            {
                string query = "SELECT * FROM Phong";
                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                da.Fill(dt);
            }
            return dt;
        }

        // 2. Tìm khách hàng bằng CCCD
        public KhachHangDTO TimKhachHangByCCCD(string cccd)
        {
            using (SqlConnection conn = new SqlConnection(connString))
            {
                string query = "SELECT * FROM KhachHang WHERE CCCD = @CCCD";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@CCCD", cccd);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    return new KhachHangDTO
                    {
                        MaKH = Convert.ToInt32(reader["MaKH"]),
                        HoTen = reader["HoTen"].ToString(),
                        CCCD = reader["CCCD"].ToString(),
                        SoDienThoai = reader["SoDienThoai"].ToString()
                    };
                }
            }
            return null;
        }

        // 3. Thêm mới khách hàng
        public int ThemKhachHang(KhachHangDTO kh)
        {
            using (SqlConnection conn = new SqlConnection(connString))
            {
                string query = "INSERT INTO KhachHang (HoTen, CCCD, SoDienThoai) VALUES (@HoTen, @CCCD, @SDT); SELECT SCOPE_IDENTITY();";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@HoTen", kh.HoTen);
                cmd.Parameters.AddWithValue("@CCCD", kh.CCCD);
                cmd.Parameters.AddWithValue("@SDT", kh.SoDienThoai ?? (object)DBNull.Value);

                conn.Open();
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }

        // 4. Check-in phòng
        public bool CheckIn(string soPhong, int maKH)
        {
            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();
                SqlTransaction trans = conn.BeginTransaction();
                try
                {
                    string q1 = "INSERT INTO PhieuDatPhong (SoPhong, MaKH, NgayNhanPhong) VALUES (@SoPhong, @MaKH, GETDATE())";
                    SqlCommand cmd1 = new SqlCommand(q1, conn, trans);
                    cmd1.Parameters.AddWithValue("@SoPhong", soPhong);
                    cmd1.Parameters.AddWithValue("@MaKH", maKH);
                    cmd1.ExecuteNonQuery();

                    string q2 = "UPDATE Phong SET TrangThai = N'Đang ở' WHERE SoPhong = @SoPhong";
                    SqlCommand cmd2 = new SqlCommand(q2, conn, trans);
                    cmd2.Parameters.AddWithValue("@SoPhong", soPhong);
                    cmd2.ExecuteNonQuery();

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

        // 5. Check-out phòng
        public bool CheckOut(string soPhong, out decimal tongTien)
        {
            tongTien = 0;
            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();
                SqlTransaction trans = conn.BeginTransaction();
                try
                {
                    string q1 = @"SELECT TOP 1 p.MaPhieu, p.NgayNhanPhong, phong.GiaTheoGio 
                                  FROM PhieuDatPhong p 
                                  JOIN Phong phong ON p.SoPhong = phong.SoPhong 
                                  WHERE p.SoPhong = @SoPhong AND p.NgayTraPhong IS NULL 
                                  ORDER BY p.MaPhieu DESC";
                    SqlCommand cmd1 = new SqlCommand(q1, conn, trans);
                    cmd1.Parameters.AddWithValue("@SoPhong", soPhong);

                    int maPhieu = 0;
                    DateTime ngayNhan = DateTime.Now;
                    decimal giaTheoGio = 0;

                    using (SqlDataReader r = cmd1.ExecuteReader())
                    {
                        if (r.Read())
                        {
                            maPhieu = Convert.ToInt32(r["MaPhieu"]);
                            ngayNhan = Convert.ToDateTime(r["NgayNhanPhong"]);
                            giaTheoGio = Convert.ToDecimal(r["GiaTheoGio"]);
                        }
                    }

                    if (maPhieu == 0) return false;

                    double soGio = Math.Max(1, Math.Ceiling((DateTime.Now - ngayNhan).TotalHours));
                    decimal tienPhong = (decimal)soGio * giaTheoGio;

                    string q2 = @"SELECT ISNULL(SUM(sd.SoLuong * dv.DonGia), 0) 
                                  FROM SuDungDichVu sd 
                                  JOIN DichVu dv ON sd.MaDV = dv.MaDV 
                                  WHERE sd.MaPhieu = @MaPhieu";
                    SqlCommand cmd2 = new SqlCommand(q2, conn, trans);
                    cmd2.Parameters.AddWithValue("@MaPhieu", maPhieu);
                    decimal tienDichVu = Convert.ToDecimal(cmd2.ExecuteScalar());

                    tongTien = tienPhong + tienDichVu;

                    string q3 = "UPDATE PhieuDatPhong SET NgayTraPhong = GETDATE(), TongTienThanhToan = @TongTien WHERE MaPhieu = @MaPhieu";
                    SqlCommand cmd3 = new SqlCommand(q3, conn, trans);
                    cmd3.Parameters.AddWithValue("@TongTien", tongTien);
                    cmd3.Parameters.AddWithValue("@MaPhieu", maPhieu);
                    cmd3.ExecuteNonQuery();

                    string q4 = "UPDATE Phong SET TrangThai = N'Đang dọn dẹp' WHERE SoPhong = @SoPhong";
                    SqlCommand cmd4 = new SqlCommand(q4, conn, trans);
                    cmd4.Parameters.AddWithValue("@SoPhong", soPhong);
                    cmd4.ExecuteNonQuery();

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