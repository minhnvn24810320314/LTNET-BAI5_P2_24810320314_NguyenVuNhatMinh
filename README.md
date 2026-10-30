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

#### 1. Gọi món & Cập nhật trạng thái bàn (Visual Table Layout)
Hiển thị danh sách bàn động dạng sơ đồ. Bàn "Trống" hiển thị màu xanh, khi gọi món tự động chuyển sang màu đỏ ("Có khách") và nạp danh sách món vào DataGridView.

![Gọi món và Đổi trạng thái bàn](images/bai5_3_01_goi_mon_va_doi_trang_thai.png)

#### 2. Thao tác Đổi bàn (Exchange Table)
Chuyển toàn bộ món ăn từ bàn hiện tại sang bàn mới. Bàn cũ quay về trạng thái màu xanh ("Trống"), bàn mới nhận hóa đơn chuyển sang màu đỏ ("Có khách").

![Thao tác Đổi bàn](images/bai5_3_02_thao_tac_doi_ban.png)

#### 3. Thực thi Nghiệp vụ Thanh toán (SqlTransaction)
Sử dụng `SqlTransaction` để đồng bộ xử lý: tính tổng tiền hóa đơn, cập nhật giờ ra/trạng thái hóa đơn thành công và giải phóng bàn về trạng thái "Trống".

![Thanh toán Hóa đơn Cafe](images/bai5_3_03_thanh_toan_hoa_don.png)

#### 4. Minh họa Bắt lỗi Validation
Bắt lỗi Validation nghiệp vụ khi chưa chọn bàn hoặc nhập số lượng/đơn giá không hợp lệ.

![Bắt lỗi Validation Bài 5.3](images/bai5_3_04_quan_ly_thuc_don_validation.png)

---

### BÀI 5.4: HỆ THỐNG QUẢN LÝ KHÁCH SẠN (HOTEL MANAGEMENT SYSTEM)

#### 1. Màn hình Chính & Quản lý danh sách Phòng (Check-in)
Hiển thị danh sách phòng, hỗ trợ tìm kiếm/lọc phòng trống và thực hiện thủ tục Check-in cho khách hàng.

![Check-in Khách sạn](images/bai5_4_01_checkin_phong.png)

#### 2. Quản lý Dịch vụ & Đặt món/Sử dụng dịch vụ
Thêm dịch vụ (nước uống, giặt ủi...) vào phòng đang có khách ở, tự động tính tổng tiền dịch vụ theo từng phòng.

![Sử dụng Dịch vụ Khách sạn](images/bai5_4_02_su_dung_dich_vu.png)

#### 3. Thanh toán Check-out (SqlTransaction)
Xử lý giao dịch Check-out bọc trong `SqlTransaction`: tính tiền phòng theo thời gian lưu trú + tiền dịch vụ, cập nhật trạng thái phòng về "Trống".

![Thanh toán Check-out](images/bai5_4_03_checkout_thanh_toan.png)

#### 4. Báo lỗi Giao dịch & Validation
Thông báo lỗi khi phòng chưa có khách Check-in hoặc nhập sai dữ liệu khách hàng/dịch vụ.

![Báo lỗi Validation Khách sạn](images/bai5_4_04_validation_loi.png)

---

### BÀI 5.5: HỆ THỐNG QUẢN LÝ KHO / BÁN HÀNG (INVENTORY MANAGEMENT SYSTEM)

#### 1. Giao diện Quản lý Danh mục & Sản phẩm (CRUD 3-Tier)
Giao diện quản lý danh sách hàng hóa/sản phẩm với đầy đủ chức năng Thêm, Sửa, Xóa và Tìm kiếm sản phẩm theo mã/tên.

![Quản lý Sản phẩm](images/bai5_5_01_quan_ly_san_pham.png)

#### 2. Lập Phiếu Nhập / Phiếu Xuất kho
Màn hình thực thi nghiệp vụ lập hóa đơn/phiếu xuất nhập kho, cập nhật số lượng tồn kho tự động trong CSDL.

![Lập Phiếu Xuất Nhập Kho](images/bai5_5_02_lap_phieu_xuat_nhap.png)

#### 3. Báo cáo - Thống kê Doanh thu / Tồn kho
Màn hình xuất báo cáo thống kê số lượng tồn kho và tổng doanh thu bán hàng theo khoảng thời gian.

![Báo cáo Thống kê Kho](images/bai5_5_03_bao_cao_thong_ke.png)

#### 4. Bắt lỗi SQL Injection & Ràng buộc Dữ liệu
Sử dụng `SqlParameter` chống SQL Injection và kiểm tra ràng buộc không cho nhập số lượng tồn âm hoặc giá tiền nhỏ hơn 0.

![Xử lý lỗi SQL Injection và Validation](images/bai5_5_04_sql_injection_validation.png)