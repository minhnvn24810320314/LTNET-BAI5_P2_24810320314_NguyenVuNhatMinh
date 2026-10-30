using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using BaiTap5_5.DTO;

namespace BaiTap5_5.DAL
{
    public class InventoryDAL
    {
        public List<VatTuDTO> GetAllVatTu()
        {
            List<VatTuDTO> list = new List<VatTuDTO>();
            using (SqlConnection conn = DatabaseAccess.GetConnection())
            {
                string query = "SELECT MaVatTu, TenVatTu, DVT, SoLuongTon FROM VatTu";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new VatTuDTO
                        {
                            MaVatTu = reader["MaVatTu"].ToString()!,
                            TenVatTu = reader["TenVatTu"].ToString()!,
                            DVT = reader["DVT"]?.ToString() ?? "",
                            SoLuongTon = Convert.ToDecimal(reader["SoLuongTon"])
                        });
                    }
                }
            }
            return list;
        }

        public List<VatTuDTO> SearchVatTu(string keyword)
        {
            List<VatTuDTO> list = new List<VatTuDTO>();
            using (SqlConnection conn = DatabaseAccess.GetConnection())
            {
                string query = "SELECT MaVatTu, TenVatTu, DVT, SoLuongTon FROM VatTu WHERE MaVatTu LIKE @kw OR TenVatTu LIKE @kw";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@kw", "%" + keyword + "%");
                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new VatTuDTO
                        {
                            MaVatTu = reader["MaVatTu"].ToString()!,
                            TenVatTu = reader["TenVatTu"].ToString()!,
                            DVT = reader["DVT"]?.ToString() ?? "",
                            SoLuongTon = Convert.ToDecimal(reader["SoLuongTon"])
                        });
                    }
                }
            }
            return list;
        }

        public bool LuuPhieuKhoTransaction(PhieuKhoDTO phieu, out string errorMessage)
        {
            errorMessage = string.Empty;
            using (SqlConnection conn = DatabaseAccess.GetConnection())
            {
                conn.Open();
                SqlTransaction trans = conn.BeginTransaction();

                try
                {
                    string sqlPhieu = @"INSERT INTO PhieuKho (LoaiPhieu, NgayLap, MaNCC, TongTien) 
                                        VALUES (@LoaiPhieu, @NgayLap, @MaNCC, @TongTien);
                                        SELECT SCOPE_IDENTITY();";
                    SqlCommand cmdPhieu = new SqlCommand(sqlPhieu, conn, trans);
                    cmdPhieu.Parameters.AddWithValue("@LoaiPhieu", phieu.LoaiPhieu);
                    cmdPhieu.Parameters.AddWithValue("@NgayLap", phieu.NgayLap);
                    cmdPhieu.Parameters.AddWithValue("@MaNCC", (object?)phieu.MaNCC ?? DBNull.Value);
                    cmdPhieu.Parameters.AddWithValue("@TongTien", phieu.TongTien);

                    int maPhieuMoi = Convert.ToInt32(cmdPhieu.ExecuteScalar());

                    foreach (var item in phieu.ChiTiet)
                    {
                        string sqlCT = @"INSERT INTO ChiTietPhieuKho (MaPhieu, MaVatTu, SoLuong, DonGia) 
                                         VALUES (@MaPhieu, @MaVatTu, @SoLuong, @DonGia)";
                        SqlCommand cmdCT = new SqlCommand(sqlCT, conn, trans);
                        cmdCT.Parameters.AddWithValue("@MaPhieu", maPhieuMoi);
                        cmdCT.Parameters.AddWithValue("@MaVatTu", item.MaVatTu);
                        cmdCT.Parameters.AddWithValue("@SoLuong", item.SoLuong);
                        cmdCT.Parameters.AddWithValue("@DonGia", item.DonGia);
                        cmdCT.ExecuteNonQuery();

                        if (phieu.LoaiPhieu == "XUAT")
                        {
                            string sqlCheck = "SELECT SoLuongTon FROM VatTu WHERE MaVatTu = @MaVT";
                            SqlCommand cmdCheck = new SqlCommand(sqlCheck, conn, trans);
                            cmdCheck.Parameters.AddWithValue("@MaVT", item.MaVatTu);
                            decimal tonHienTai = Convert.ToDecimal(cmdCheck.ExecuteScalar());

                            if (tonHienTai < item.SoLuong)
                            {
                                trans.Rollback();
                                errorMessage = $"Không đủ hàng trong kho cho mã {item.MaVatTu}! Tồn: {tonHienTai}, Xuất: {item.SoLuong}";
                                return false;
                            }

                            string sqlUpdate = "UPDATE VatTu SET SoLuongTon = SoLuongTon - @SL WHERE MaVatTu = @MaVT";
                            SqlCommand cmdUpdate = new SqlCommand(sqlUpdate, conn, trans);
                            cmdUpdate.Parameters.AddWithValue("@SL", item.SoLuong);
                            cmdUpdate.Parameters.AddWithValue("@MaVT", item.MaVatTu);
                            cmdUpdate.ExecuteNonQuery();
                        }
                        else
                        {
                            string sqlUpdate = "UPDATE VatTu SET SoLuongTon = SoLuongTon + @SL WHERE MaVatTu = @MaVT";
                            SqlCommand cmdUpdate = new SqlCommand(sqlUpdate, conn, trans);
                            cmdUpdate.Parameters.AddWithValue("@SL", item.SoLuong);
                            cmdUpdate.Parameters.AddWithValue("@MaVT", item.MaVatTu);
                            cmdUpdate.ExecuteNonQuery();
                        }
                    }

                    trans.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    errorMessage = "Lỗi hệ thống: " + ex.Message;
                    return false;
                }
            }
        }

        public decimal TinhTongGiaTriTonKho()
        {
            using (SqlConnection conn = DatabaseAccess.GetConnection())
            {
                string query = "SELECT ISNULL(SUM(SoLuongTon * 100000), 0) FROM VatTu";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                return Convert.ToDecimal(cmd.ExecuteScalar());
            }
        }
    }
}