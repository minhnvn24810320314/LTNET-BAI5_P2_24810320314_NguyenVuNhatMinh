# BÁO CÁO BÀI TẬP 5.5: HỆ THỐNG QUẢN LÝ QUY TRÌNH XUẤT - NHẬP KHO NGUYÊN VẬT LIỆU (INVENTORY MANAGEMENT)

## MỤC 1: THÔNG TIN SINH VIÊN
- **Họ và tên sinh viên:** Nguyễn Vũ Nhật Minh
- **Mã sinh viên:** 24810320314
- **Tên Đề tài / Bài tập:** Tổng hợp Bài tập Thực hành (Bài 5.3, Bài 5.4, Bài 5.5)
---

## MỤC 2: HƯỚNG DẪN CÀI ĐẶT & CHẠY DỰ ÁN
1. Mở **SQL Server Object Explorer** trong Visual Studio (hoặc SSMS), chạy các script khởi tạo CSDL trong thư mục `Database/`:
   - `CafePOSDB` (cho Bài 5.3)
   - `HotelDB` (cho Bài 5.4)
   - `InventoryDB` / CSDL Bài 5.5 (cho Bài 5.5)
2. Mở file solution `BAI5_P2.sln` bằng **Visual Studio 2022**.
3. Kiểm tra file DAL/DatabaseHelper trong từng project, cập nhật chuỗi kết nối `connString` cho phù hợp với máy cá nhân nếu cần:
   `Server=(localdb)\MSSQLLocalDB;Database=<TenCSDL>;Trusted_Connection=True;`
4. Nhấn **Ctrl + Shift + B** để Build toàn bộ Solution.
5. Click chuột phải vào dự án muốn chạy (ví dụ `BaiTap5_3`, `BaiTap5_4`, `BaiTap5_5`) $\rightarrow$ Chọn **Set as Startup Project** và nhấn **F5** để bắt đầu.

---

## MỤC 3: HÌNH ẢNH KẾT QUẢ CÁC MÀN HÌNH CHỨC NĂNG

### BÀI 5.3: HỆ THỐNG QUẢN LÝ QUÁN CAFE (POS CAFE SYSTEM)

#### 1. Màn hình sơ đồ bàn ban đầu
![Giao diện sơ đồ bàn](docs/bai5_3_01_giao_dien_so_do_ban.png)

#### 2. Gọi món & Cập nhật trạng thái bàn
![Gọi món và đổi trạng thái](docs/bai5_3_02_goi_mon_va_doi_trang_thai.png)

#### 3. Thanh toán hóa đơn
![Thanh toán hóa đơn](docs/bai5_3_03_thanh_toan_hoa_don.png)

#### 4. Bắt lỗi Validation
![Quản lý thực đơn validation](docs/bai5_3_04_quan_ly_thuc_don_validation.png)

---

### BÀI 5.4: HỆ THỐNG QUẢN LÝ KHÁCH SẠN (HOTEL MANAGEMENT SYSTEM)

#### 1. Màn hình giao diện chính
![Giao diện chính khách sạn](docs/bai5_4_01_giao_dien_chinh.png)

#### 2. Lọc phòng & Xử lý ngắt kết nối
![Lọc phòng ngắt kết nối](docs/bai5_4_02_loc_phong_ngat_ket_noi.png)

#### 3. Check-in phòng thành công
![Check-in thành công](docs/bai5_4_03_check_in_thanh_cong.png)

#### 4. Check-out và xuất hóa đơn
![Check-out hóa đơn](docs/bai5_4_04_check_out_hoa_don.png)

#### 5. Trạng thái phòng sau Check-out
![Trạng thái sau Check-out](docs/bai5_4_05_trang_thai_sau_check_out.png)

---

### BÀI 5.5: HỆ THỐNG QUẢN LÝ KHO / BÁN HÀNG (INVENTORY MANAGEMENT SYSTEM)

#### 1. Màn hình giao diện chính
![Giao diện chính quản lý kho](docs/bai5_5_01_giao_dien_chinh.png)

#### 2. Tìm kiếm sử dụng SqlParameter
![Tìm kiếm SqlParameter](docs/bai5_5_02_tim_kiem_sqlparameter.png)

#### 3. Lập phiếu xuất/nhập thành công
![Lập phiếu thành công](docs/bai5_5_03_lap_phieu_thanh_cong.png)

#### 4. Thống kê tổng giá trị tồn kho
![Tổng giá trị tồn](docs/bai5_5_04_tong_gia_tri_ton.png)

#### 5. Cảnh báo lỗi & Rollback
![Cảnh báo Rollback](docs/bai5_5_05_canh_bao_rollback.png)
