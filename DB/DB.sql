CREATE DATABASE DAn_1_QLBanhNgot
Go

USE DAn_1_QLBanhNgot
Go

-- Create table 
CREATE TABLE DanhMuc
(
	MaDM VARCHAR(10) PRIMARY KEY,
	TenDM NVARCHAR(50),
	SoLuong INT CHECK (SoLuong >= 0),
	GiaBan DECIMAL(18, 0) CHECK (GiaBan >= 0),
	MoTa NVARCHAR(255)
)

CREATE TABLE KhachHang
(
	MaKH VARCHAR(10) PRIMARY KEY,
	HoTen NVARCHAR(50),
	DiaChi NVARCHAR(255),
	SDT CHAR(10)
)

CREATE TABLE HoaDonBan 
(
	MaHDB INT IDENTITY(1,1) PRIMARY KEY,
	NgayTao DATETIME,
	MaKH VARCHAR(10),
	TrangThai NVARCHAR(20),
	FOREIGN KEY (MaKH) REFERENCES KhachHang(MaKH) ON DELETE SET NULL
)

CREATE TABLE ChiTietHDB 
(
	MaHDB INT,
	MaDM VARCHAR(10),
	SoLuongBan INT CHECK (SoLuongBan > 0),
	DonGia DECIMAL(18, 0) CHECK (DonGia > 0),
	PRIMARY KEY (MaHDB, MaDM),
	FOREIGN KEY (MaHDB) REFERENCES HoaDonBan(MaHDB),
	FOREIGN KEY (MaDM) REFERENCES DanhMuc(MaDM)
)

CREATE TABLE NhienLieu
(
	MaNL VARCHAR(10) PRIMARY KEY,
	TenNL NVARCHAR(50),
	MoTa NVARCHAR(100),
	soLuong INT
)

CREATE TABLE HoaDonNhap
(
    MaHDN INT IDENTITY(1,1) PRIMARY KEY,
    NgayTao DATETIME,
	TongTien DECIMAL(18, 0)
);

CREATE TABLE ChiTietHDN
(
    MaHDN INT,
    MaNL VARCHAR(10),
    DinhLuong INT CHECK (DinhLuong > 0),
    GiaNhap DECIMAL(18,0) CHECK (GiaNhap > 0),
    PRIMARY KEY (MaHDN, MaNL),
    FOREIGN KEY (MaHDN) REFERENCES HoaDonNhap(MaHDN),
    FOREIGN KEY (MaNL) REFERENCES NhienLieu(MaNL)
);
GO

-- Trigger
-- trg_XoaHDB
CREATE TRIGGER trg_XoaHDB
ON HoaDonBan
INSTEAD OF DELETE
AS 
BEGIN
	-- Khôi phục số lượng DM trước khi xóa CTHDB và HDB
	UPDATE DanhMuc
	SET SoLuong = SoLuong + CT.SoLuongBan
	FROM DanhMuc DM 
	INNER JOIN ChiTietHDB CT ON DM.MaDM = CT.MaDM
	INNER JOIN deleted D on CT.MaHDB = D.MaHDB;

	-- Xoa CTHDB
	DELETE FROM ChiTietHDB
	WHERE MaHDB IN (SELECT MaHDB FROM deleted)

	-- Xoa HDB	
	DELETE FROM HoaDonBan
	WHERE MaHDB IN (SELECT MaHDB from deleted)
END;
GO

--
CREATE TRIGGER trg_UpdateTongTienHDN
ON ChiTietHDN
AFTER INSERT, UPDATE
AS
BEGIN
    DECLARE @MaHDN INT;
    DECLARE @TongTien DECIMAL(18, 0);

    SELECT @MaHDN = MaHDN FROM inserted;

    -- Tính tổng tiền mới cho hóa đơn này
    SELECT @TongTien = SUM(c.DinhLuong * c.GiaNhap)
    FROM ChiTietHDN c
    WHERE c.MaHDN = @MaHDN
    GROUP BY c.MaHDN;

    UPDATE HoaDonNhap
    SET TongTien = @TongTien
    WHERE MaHDN = @MaHDN;
END;
GO

-- Views
-- View HDB
CREATE OR ALTER VIEW v_HoaDonBan AS
SELECT
	hd.MaHDB,
    CONVERT(VARCHAR(10), hd.NgayTao, 103) AS NgayTao, -- Định dạng 103
    kh.HoTen AS TenKhachHang,
    ISNULL(SUM(ct.SoLuongBan * ct.DonGia), 0) AS TongTien,  -- Tính tổng tiền
	hd.TrangThai AS trangthai
FROM HoaDonBan hd 
	LEFT JOIN KhachHang kh ON hd.MaKH = kh.MaKH
	LEFT JOIN ChiTietHDB ct ON hd.MaHDB = ct.MaHDB
GROUP BY 
	hd.MaHDB, hd.NgayTao, hd.MaKH, kh.HoTen, hd.trangthai
GO

-- View CTHDB
CREATE OR ALTER VIEW v_CTHDB AS
SELECT 
    ct.MaHDB [Mã HDB],
    ct.MaDM [Mã DM],
    b.TenDM [Tên DM],
    ct.SoLuongBan [SL],
    ct.DonGia [Gia],
    ct.SoLuongBan * ct.DonGia AS [ThanhTien]
FROM 
	ChiTietHDB ct
JOIN 
    DanhMuc b ON ct.MaDM = b.MaDM;
GO

-- view CTHDN
CREATE OR ALTER VIEW v_HoaDonNhap AS
SELECT 
    h.MaHDN,
    h.NgayTao,
	n.MaNL [Mã NL],
    n.TenNL [Tên NL],
    c.DinhLuong AS soLuong,
    c.GiaNhap [Gia],
    (c.DinhLuong * c.GiaNhap) AS TongTien
FROM 
    HoaDonNhap h
JOIN 
    ChiTietHDN c ON h.MaHDN = c.MaHDN
JOIN 
    NhienLieu n ON c.MaNL = n.MaNL;
GO

-- Procedures Thêm/ Sửa In4
-- Proc_ThemDM 
CREATE PROCEDURE Proc_ThemDM
    @MaDM VARCHAR(10),
    @TenDM NVARCHAR(50),
    @MoTa NVARCHAR(255) = NULL,
    @SoLuong INT = 0,
    @GiaBan DECIMAL(18, 0) = 0
AS
BEGIN
    IF EXISTS (SELECT 1 FROM DanhMuc WHERE MaDM = @MaDM)
    BEGIN
        UPDATE DanhMuc
        SET TenDM = @TenDM,
            SoLuong = @SoLuong,
            GiaBan = @GiaBan,
            MoTa = @MoTa
        WHERE MaDM = @MaDM;
    END
    ELSE
    BEGIN
        INSERT INTO DanhMuc (MaDM, TenDM, SoLuong, GiaBan, MoTa)
        VALUES (@MaDM, @TenDM, @SoLuong, @GiaBan, @MoTa);
    END
END;
go

-- Proc_ThemKH
CREATE PROCEDURE Proc_ThemKH (
    @MaKH VARCHAR(10),
    @HoTen NVARCHAR(30),
    @DiaChi NVARCHAR(50),
	@SDT VARCHAR(20)
)
AS
BEGIN
    IF EXISTS (SELECT 1 FROM KhachHang WHERE MaKH = @MaKH)
    BEGIN
        UPDATE KhachHang
        SET HoTen = @HoTen,
            SDT = @SDT,
            DiaChi = @DiaChi
        WHERE MaKH = @MaKH;
    END
    ELSE
    BEGIN
        INSERT INTO KhachHang (MaKH, HoTen, DiaChi, SDT)
        VALUES (@MaKH, @HoTen, @DiaChi, @SDT);
    END
END;
GO

-- Proc_ThemHDB
CREATE OR ALTER PROCEDURE Proc_ThemHDB
    @MaKhachHang VARCHAR(10) = NULL,
	@Trangthai NVARCHAR(20)
AS
BEGIN
    INSERT INTO HoaDonBan (NgayTao, MaKH, TrangThai)
    VALUES (GETDATE(), @MaKhachHang, @Trangthai);
END;
GO

-- Proc_ThemCTHDB
CREATE PROCEDURE Proc_ThemCTHDB
    @MaHoaDon VARCHAR(10),
    @MaBanh VARCHAR(10),
    @SoLuongBan INT
AS
BEGIN
    INSERT INTO ChiTietHDB(MaHDB, MaDM, SoLuongBan, DonGia)
    VALUES (@MaHoaDon, @MaBanh, @SoLuongBan, (SELECT GiaBan FROM DanhMuc WHERE MaDM = @MaBanh));

    UPDATE DanhMuc
    SET SoLuong = SoLuong - @SoLuongBan
    WHERE MaDM = @MaBanh;
END;
GO

-- Proc_ThemNL
CREATE OR ALTER PROCEDURE Proc_ThemNL (
    @MaNL VARCHAR(10),
    @TenNL NVARCHAR(50),
    @MoTa NVARCHAR(100)
)
AS
BEGIN
    IF EXISTS (SELECT 1 FROM NhienLieu WHERE MaNL = @MaNL)
    BEGIN
        UPDATE NhienLieu
        SET TenNL = @TenNL,
            MoTa = @MoTa
        WHERE MaNL = @MaNL;
    END
    ELSE
    BEGIN
        INSERT INTO NhienLieu (MaNL, TenNL, MoTa)
        VALUES (@MaNL, @TenNL, @MoTa);
    END
END;
GO

-- Proc_ThemHDN
CREATE OR ALTER PROC Proc_ThemHDN
AS
BEGIN
	INSERT INTO HoaDonNhap (NgayTao)
	VALUES (GETDATE())
END;
GO

-- Proc_ThemCTDN
CREATE OR ALTER PROC Proc_ThemCTDN
	@MaHDN INT, 
	@MaNL VARCHAR(10),
	@DinhLuong INT,
	@GiaNhap DECIMAL(18,0)
AS
BEGIN
	INSERT INTO ChiTietHDN (MaHDN, MaNL, DinhLuong, GiaNhap)
	VALUES (@MaHDN, @MaNL,  @DinhLuong, @GiaNhap)
END
GO

-- Proc Xóa In4
-- Proc_XoaHDB
CREATE OR ALTER PROCEDURE Proc_XoaHDB
    @MaHoaDon INT
AS
BEGIN
    SET NOCOUNT ON;

    DELETE FROM HoaDonBan WHERE MaHDB = @MaHoaDon;
END;
GO

-- Proc_XoaHDN
CREATE OR ALTER PROC Proc_XoaHDN
	@MaHDN INT
AS
BEGIN
	SET NOCOUNT ON;
	DELETE FROM ChiTietHDN WHERE MaHDN = @MaHDN
	DELETE FROM HoaDonNhap WHERE MaHDN = @MaHDN
END;
GO

-- Proc_XoaNL										á
CREATE PROC Proc_XoaNL
	@id VARCHAR(10)
AS 
BEGIN
	UPDATE ChiTietHDN
	SET MaNL = NULL
	WHERE MaNL = @id;

	DELETE FROM NhienLieu
	WHERE MaNL = @id;
END;
GO 

-- Proc_XoaDM
CREATE OR ALTER PROC Proc_XoaDM
    @id VARCHAR(10)
AS
BEGIN
    UPDATE ChiTietHDB
    SET MaDM = NULL
    WHERE MaDM = @id;

    DELETE FROM DanhMuc
    WHERE MaDM = @id;
END;
GO 

-- Proc_XoaKH
CREATE OR ALTER PROC Proc_XoaKH
    @id VARCHAR(10)
AS
BEGIN
    UPDATE HoaDonBan
    SET MaKH = NULL
    WHERE MaKH = @id;

    DELETE FROM KhachHang
    WHERE MaKH = @id;
END;
GO

-- Proc thống kê
--
CREATE OR ALTER PROCEDURE Proc_TK_SpBanChay
AS
BEGIN
    SELECT 
        TenDM,
        SoLuong
    FROM 
        DanhMuc
    WHERE 
        SoLuong > 0; 
END;
GO

--
CREATE OR ALTER PROCEDURE ThongKeDoanhThuTheoThangNam
    @Month INT = NULL,  
    @Year INT
AS
BEGIN
    DECLARE @StartDate DATE;
    DECLARE @EndDate DATE;

    IF @Month IS NULL
    BEGIN
        -- Trường hợp chỉ có @Year: Lấy doanh thu từng tháng của năm đó
        DECLARE @MonthCounter INT = 1;

        CREATE TABLE #MonthlyRevenue (
            MonthNumber INT,
            Revenue DECIMAL(12, 0)
        );

        WHILE @MonthCounter <= 12
        BEGIN
            SET @StartDate = DATEFROMPARTS(@Year, @MonthCounter, 1);
            SET @EndDate = EOMONTH(@StartDate);

            INSERT INTO #MonthlyRevenue (MonthNumber, Revenue)
            SELECT 
                @MonthCounter AS MonthNumber,
                ISNULL(SUM(CT.DonGia * CT.SoLuongBan), 0) AS Revenue
            FROM ChiTietHDB CT
            INNER JOIN HoaDonBan H ON CT.MaHDB = H.MaHDB
            WHERE H.NgayTao >= @StartDate AND H.NgayTao <= @EndDate
                AND H.TrangThai = N'Hoàn tất'  -- Trạng thái hóa đơn hoàn tất

            SET @MonthCounter = @MonthCounter + 1;
        END

        -- Trả về dữ liệu doanh thu cho đủ 12 tháng
        SELECT MonthNumber, Revenue FROM #MonthlyRevenue ORDER BY MonthNumber;
        DROP TABLE #MonthlyRevenue;
    END
    ELSE
    BEGIN
        -- Trường hợp có cả @Month và @Year: Lấy doanh thu cho tháng cụ thể
        SET @StartDate = DATEFROMPARTS(@Year, @Month, 1);
        SET @EndDate = EOMONTH(@StartDate);

        SELECT 
            @Month AS MonthNumber,
            ISNULL(SUM(CT.DonGia * CT.SoLuongBan), 0) AS Revenue
        FROM ChiTietHDB CT
        INNER JOIN HoaDonBan H ON CT.MaHDB = H.MaHDB
        WHERE H.NgayTao >= @StartDate AND H.NgayTao <= @EndDate
            AND H.TrangThai = N'Hoàn tất'  -- Trạng thái hóa đơn hoàn tất
    END
END;
GO

-- Thống kê số lượng
CREATE OR ALTER PROCEDURE ThongKeSoLuongKhachHangHD
AS
BEGIN
    SELECT 
        (SELECT COUNT(*) FROM KhachHang) AS SoLuongKhachHang,
		(SELECT COUNT(*) FROM DanhMuc) AS SoSanPham,
		(SELECT COUNT(*) FROM HoaDonBan) AS SoHDB,
		(SELECT COUNT(*) FROM HoaDonNhap) AS SoHDN
END;
Go

-- Thống kê hóa đơn ngày hiện tại
CREATE VIEW V_HoaDon_HienTai AS
SELECT 
    H.MaHDB AS MaHoaDon,
    CONVERT(VARCHAR(10), H.NgayTao, 103) AS NgayTao, 
    SUM(CT.DonGia * CT.SoLuongBan) AS TongTien
FROM 
    HoaDonBan H
JOIN 
    ChiTietHDB CT ON H.MaHDB = CT.MaHDB
WHERE 
    CAST(H.NgayTao AS DATE) = CAST(GETDATE() AS DATE)  -- Lọc theo ngày hiện tại
GROUP BY H.MaHDB, H.NgayTao;
GO

-- Thống kê sản phảm bán chạy nhất
CREATE VIEW V_SanPhamBanChay AS
SELECT 
    DM.TenDM AS TenSanPham,  
    SUM(CT.SoLuongBan) AS TongSoLuongBan, 
    SUM(CT.SoLuongBan * CT.DonGia) AS TongDoanhThu  
FROM 
    ChiTietHDB CT
JOIN 
    DanhMuc DM ON CT.MaDM = DM.MaDM 
GROUP BY 
    DM.TenDM;  
GO

-- update tt hdb
CREATE PROCEDURE prc_CapNhatTrangThaiHoaDon
    @idHD INT,
    @tThai NVARCHAR(20)
AS
BEGIN
    IF EXISTS (SELECT 1 FROM HoaDonBan WHERE MaHDB = @idHD)
    BEGIN
        UPDATE HoaDonBan
        SET TrangThai = @tThai
        WHERE MaHDB = @idHD;
        
        PRINT 'Cập nhật trạng thái hóa đơn thành công.';
    END
    ELSE
    BEGIN
        PRINT 'Hóa đơn không tồn tại.';
    END
END;

