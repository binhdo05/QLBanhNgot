CREATE DATABASE DAn_1_QLBanBanhNgot
GO
USE DAn_1_QLBanBanhNgot
GO

-- (Bảng danh mục bánh)
CREATE TABLE DanhMuc
(
    MaDM INT IDENTITY(1,1) PRIMARY KEY,
    TenDM NVARCHAR(50) NOT NULL,
    Note NVARCHAR(100)
);

-- (Bảng sản phẩm bánh)
CREATE TABLE SanPham
(
    MaSP INT IDENTITY(1,1) PRIMARY KEY,
    MaDM INT,
    TenSP NVARCHAR(100) NOT NULL,
    SoLuong INT CHECK (SoLuong >= 0),
    Gia DECIMAL(18, 0) CHECK (Gia >= 0),
    MoTa NVARCHAR(255),
    DonVi NVARCHAR(30) NOT NULL, --(túi, hộp, ..)
    HinhAnh NVARCHAR(500),
    TrangThai BIT DEFAULT 1,
    FOREIGN KEY (MaDM) REFERENCES DanhMuc(MaDM) ON DELETE SET NULL
);

-- (Bảng nhà cung cấp)
CREATE TABLE NhaCungCap
(
    MaNCC INT IDENTITY(1,1) PRIMARY KEY,
    TenNCC NVARCHAR(50) NOT NULL,
    SDT VARCHAR(10) UNIQUE,
    Email VARCHAR(100) UNIQUE,
    DiaChi NVARCHAR(255)
);

-- Bảng NhanVien (Quản lý nhân viên)
CREATE TABLE NhanVien
(
    MaNV INT IDENTITY(1,1) PRIMARY KEY,
    TenNV NVARCHAR(100) NOT NULL,
    GioiTinh NVARCHAR(5),
    DiaChi NVARCHAR(255),
    Email VARCHAR(100) UNIQUE,
    SDT VARCHAR(10) UNIQUE,
    NgayVaoLam DATE,
    TenDangNhap VARCHAR(100) UNIQUE NOT NULL,
    MatKhau VARCHAR(255) NOT NULL,
    VaiTro NVARCHAR(30),
    TrangThai BIT DEFAULT 1
);

-- (Bảng khách hàng)
CREATE TABLE KhachHang
(
    MaKH INT IDENTITY(1,1) PRIMARY KEY,
    TenKH NVARCHAR(50) NOT NULL,
    GioiTinh NVARCHAR(5),
    SDT VARCHAR(20) UNIQUE,
    DiaChi NVARCHAR(255)
);

-- (Bảng lô bánh nhập)
CREATE TABLE LoSanPham
(
    MaLo INT IDENTITY(1,1) PRIMARY KEY,
    MaSP INT,
    SoLuongNhap INT CHECK (SoLuongNhap >= 0),
    GiaNhap DECIMAL(18, 0) CHECK (GiaNhap >= 0),
    NSX DATE,
    HSD DATE,
    SoLuongLoi INT DEFAULT 0,
    TrangThai NVARCHAR(20),
    GhiChu NVARCHAR(255),
	SoLuongThucNhap INT CHECK (SoLuongThucNhap >= 0) DEFAULT 0,
    FOREIGN KEY (MaSP) REFERENCES SanPham(MaSP) ON DELETE SET NULL
);

-- (Bảng hóa đơn nhập bánh)
CREATE TABLE HoaDonNhap
(
    MaHDN INT IDENTITY(1,1) PRIMARY KEY,
    NgayTao DATETIME NOT NULL DEFAULT GETDATE(),
    MaNCC INT,
    MaNV INT,
    TongTien DECIMAL(18, 0) DEFAULT 0 CHECK (TongTien >= 0),
    TrangThai NVARCHAR(20),
    FOREIGN KEY (MaNCC) REFERENCES NhaCungCap(MaNCC) ON DELETE SET NULL,
    FOREIGN KEY (MaNV) REFERENCES NhanVien(MaNV) ON DELETE SET NULL
);

-- (Bảng Chi tiết hóa đơn nhập)
CREATE TABLE ChiTietDonNhap
(
    MaHDN INT,
    MaLo INT,
    PRIMARY KEY (MaHDN, MaLo),
    FOREIGN KEY (MaHDN) REFERENCES HoaDonNhap(MaHDN) ON DELETE CASCADE,
    FOREIGN KEY (MaLo) REFERENCES LoSanPham(MaLo) ON DELETE CASCADE
);

-- (Bảng hóa đơn bán)
CREATE TABLE HoaDonBan
(
    MaHDB INT IDENTITY(1,1) PRIMARY KEY,
    MaNV INT,
    MaKH INT,
    NgayTao DATETIME NOT NULL DEFAULT GETDATE(),
    TrangThai NVARCHAR(20),
    TongTien DECIMAL(18, 0) DEFAULT 0 CHECK (TongTien >= 0),
    FOREIGN KEY (MaNV) REFERENCES NhanVien(MaNV) ON DELETE SET NULL,
    FOREIGN KEY (MaKH) REFERENCES KhachHang(MaKH) ON DELETE SET NULL
);

-- Bảng ChiTietBan (Chi tiết hóa đơn bán)
CREATE TABLE ChiTietDonBan
(
    MaHDB INT,
    MaCT INT IDENTITY(1,1),
    MaSP INT,
    SoLuongBan INT CHECK (SoLuongBan > 0),
    DonGia DECIMAL(18, 0) CHECK (DonGia >= 0),
    PRIMARY KEY (MaHDB, MaCT),
    FOREIGN KEY (MaHDB) REFERENCES HoaDonBan(MaHDB) ON DELETE CASCADE,
    FOREIGN KEY (MaSP) REFERENCES SanPham(MaSP) ON DELETE SET NULL
);


--- PROC ---
GO


-- Prc (Add-Edit/ Delete) --
																-- Khách Hàng --
CREATE OR ALTER PROCEDURE Proc_AddCustomer
(
    @Name NVARCHAR(50),
    @Gender NVARCHAR(5),
    @Phone VARCHAR(20),
    @Address NVARCHAR(255)
)
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS (SELECT 1 FROM KhachHang WHERE SDT = @Phone)
    BEGIN
        RAISERROR('Số điện thoại đã tồn tại.', 16, 1);
        RETURN;
    END

    INSERT INTO KhachHang (TenKH, GioiTinh, SDT, DiaChi)
    VALUES (@Name, @Gender, @Phone, @Address);
END;
GO

CREATE OR ALTER PROCEDURE Proc_UpdateCustomer
(
    @IdCustomer INT,
    @Name NVARCHAR(50),
    @Gender NVARCHAR(5),
    @Phone VARCHAR(20),
    @Address NVARCHAR(255)
)
AS
BEGIN
    SET NOCOUNT ON;

    IF NOT EXISTS (SELECT 1 FROM KhachHang WHERE MaKH = @IdCustomer)
    BEGIN
        RAISERROR('Khách hàng không tồn tại.', 16, 1);
        RETURN;
    END

    IF EXISTS (SELECT 1 FROM KhachHang WHERE SDT = @Phone AND MaKH != @IdCustomer)
    BEGIN
        RAISERROR('Số điện thoại đã tồn tại.', 16, 1);
        RETURN;
    END

    UPDATE KhachHang
    SET TenKH = @Name,
        GioiTinh = @Gender,
        SDT = @Phone,
        DiaChi = @Address
    WHERE MaKH = @IdCustomer;
END;
GO

CREATE OR ALTER PROCEDURE Proc_rmCustomer
    @MaKH INT
AS
BEGIN
    SET NOCOUNT ON;

	UPDATE HoaDonBan
    SET MaKH = NULL
    WHERE MaKH = @MaKH;

    DELETE FROM KhachHang
    WHERE MaKH = @MaKH;
END;
GO


																	-- Nhà cung cấp --
CREATE OR ALTER PROCEDURE Proc_AddSupplier
(
    @TenNCC NVARCHAR(50),
    @SDT VARCHAR(10),
    @Email VARCHAR(100),
    @DiaChi NVARCHAR(255)
)
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS (SELECT 1 FROM NhaCungCap WHERE SDT = @SDT)
    BEGIN
        RAISERROR(N'Số điện thoại đã tồn tại.', 16, 1);
        RETURN;
    END

    IF EXISTS (SELECT 1 FROM NhaCungCap WHERE Email = @Email)
    BEGIN
        RAISERROR(N'Email đã tồn tại.', 16, 1);
        RETURN;
    END

    INSERT INTO NhaCungCap (TenNCC, SDT, Email, DiaChi)
    VALUES (@TenNCC, @SDT, @Email, @DiaChi);
END;
GO

CREATE OR ALTER PROCEDURE Proc_UpdateSupplier
(
    @MaNCC INT,
    @TenNCC NVARCHAR(50),
    @SDT VARCHAR(10),
    @Email VARCHAR(100),
    @DiaChi NVARCHAR(255)
)
AS
BEGIN
    SET NOCOUNT ON;

    IF NOT EXISTS (SELECT 1 FROM NhaCungCap WHERE MaNCC = @MaNCC)
    BEGIN
        RAISERROR(N'Nhà cung cấp không tồn tại.', 16, 1);
        RETURN;
    END

    IF EXISTS (SELECT 1 FROM NhaCungCap WHERE SDT = @SDT AND MaNCC != @MaNCC)
    BEGIN
        RAISERROR(N'Số điện thoại đã tồn tại.', 16, 1);
        RETURN;
    END

    IF EXISTS (SELECT 1 FROM NhaCungCap WHERE Email = @Email AND MaNCC != @MaNCC)
    BEGIN
        RAISERROR(N'Email đã tồn tại.', 16, 1);
        RETURN;
    END

    UPDATE NhaCungCap
    SET TenNCC = @TenNCC,
        SDT = @SDT,
        Email = @Email,
        DiaChi = @DiaChi
    WHERE MaNCC = @MaNCC;
END;
GO

CREATE OR ALTER PROCEDURE Proc_DeleteSupplier
(
    @MaNCC INT
)
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE HoaDonNhap
    SET MaNCC = NULL
    WHERE MaNCC = @MaNCC;

    DELETE FROM NhaCungCap
    WHERE MaNCC = @MaNCC;
END;
GO


									-- Nhân viên --
CREATE OR ALTER PROCEDURE Proc_AddStaff
(
    @Name NVARCHAR(100),
    @Gender NVARCHAR(5),
    @Address NVARCHAR(255),
    @Gmail VARCHAR(255),
    @Phone VARCHAR(20),
    @StartDate DATE,
    @Username VARCHAR(100),
    @Password VARCHAR(255),
    @TypeStaff NVARCHAR(30),
    @IsActive BIT
)
AS
BEGIN
    SET NOCOUNT ON;
    IF EXISTS (SELECT 1 FROM NhanVien WHERE SDT = @Phone)
    BEGIN
        RAISERROR('Phone number already exists.', 16, 1);
        RETURN;
    END
    IF EXISTS (SELECT 1 FROM NhanVien WHERE Email = @Gmail)
    BEGIN
        RAISERROR('Email already exists.', 16, 1);
        RETURN;
    END
    IF EXISTS (SELECT 1 FROM NhanVien WHERE TenDangNhap = @Username)
    BEGIN
        RAISERROR('Username already exists.', 16, 1);
        RETURN;
    END

    INSERT INTO NhanVien (TenNV, GioiTinh, DiaChi, Email, SDT, NgayVaoLam, TenDangNhap, MatKhau, VaiTro, TrangThai)
    VALUES (@Name, @Gender, @Address, @Gmail, @Phone, @StartDate, @Username, @Password, @TypeStaff, @IsActive);
END;
GO

CREATE OR ALTER PROCEDURE Proc_UpdateStaff
(
    @IdStaff INT,
    @Name NVARCHAR(100),
    @Gender NVARCHAR(5),
    @Address NVARCHAR(255),
    @Gmail VARCHAR(255),
    @Phone VARCHAR(20),
    @StartDate DATE,
    @Username VARCHAR(100),
    @Password VARCHAR(255),
    @TypeStaff NVARCHAR(30),
    @IsActive BIT
)
AS
BEGIN
    SET NOCOUNT ON;

    IF NOT EXISTS (SELECT 1 FROM NhanVien WHERE MaNV = @IdStaff)
    BEGIN
        RAISERROR('Staff does not exist.', 16, 1);
        RETURN;
    END

    IF EXISTS (SELECT 1 FROM NhanVien WHERE SDT = @Phone AND MaNV != @IdStaff)
    BEGIN
        RAISERROR('Phone number already exists.', 16, 1);
        RETURN;
    END

    IF EXISTS (SELECT 1 FROM NhanVien WHERE Email = @Gmail AND MaNV != @IdStaff)
    BEGIN
        RAISERROR('Email already exists.', 16, 1);
        RETURN;
    END

    IF EXISTS (SELECT 1 FROM NhanVien WHERE TenDangNhap = @Username AND MaNV != @IdStaff)
    BEGIN
        RAISERROR('Username already exists.', 16, 1);
        RETURN;
    END

    UPDATE NhanVien
    SET TenNV = @Name,
        GioiTinh = @Gender,
        DiaChi = @Address,
        Email = @Gmail,
        SDT = @Phone,
        NgayVaoLam = @StartDate,
        TenDangNhap = @Username,
        MatKhau = @Password,
        VaiTro = @TypeStaff,
        TrangThai = @IsActive
    WHERE MaNV = @IdStaff;
END;
GO

CREATE OR ALTER PROCEDURE Proc_DeleteStaff
    @IdStaff INT
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE HoaDonBan
    SET MaNV = NULL
    WHERE MaNV = @IdStaff;

    UPDATE HoaDonNhap
    SET MaNV = NULL
    WHERE MaNV = @IdStaff;

    DELETE FROM NhanVien
    WHERE MaNV = @IdStaff;
END;
GO


								-- Danh mục/ Sản phẩm --
CREATE OR ALTER PROCEDURE Proc_AddDanhMuc
(
    @TenDM NVARCHAR(50),
    @Note NVARCHAR(100)
)
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS (SELECT 1 FROM DanhMuc WHERE TenDM = @TenDM)
    BEGIN
        RAISERROR(N'Tên danh mục đã tồn tại.', 16, 1);
        RETURN;
    END

    INSERT INTO DanhMuc (TenDM, Note)
    VALUES (@TenDM, @Note);
END;
GO

CREATE OR ALTER PROCEDURE Proc_UpdateDanhMuc
(
    @MaDM INT,
    @TenDM NVARCHAR(50),
    @Note NVARCHAR(100)
)
AS
BEGIN
    SET NOCOUNT ON;

    IF NOT EXISTS (SELECT 1 FROM DanhMuc WHERE MaDM = @MaDM)
    BEGIN
        RAISERROR(N'Danh mục không tồn tại.', 16, 1);
        RETURN;
    END

    IF EXISTS (SELECT 1 FROM DanhMuc WHERE TenDM = @TenDM AND MaDM != @MaDM)
    BEGIN
        RAISERROR(N'Tên danh mục đã tồn tại.', 16, 1);
        RETURN;
    END

    UPDATE DanhMuc
    SET TenDM = @TenDM,
        Note = @Note
    WHERE MaDM = @MaDM;
END;
GO

CREATE OR ALTER PROCEDURE Proc_DeleteDanhMuc
(
    @MaDM INT
)
AS
BEGIN
    SET NOCOUNT ON;


    DELETE FROM DanhMuc
    WHERE MaDM = @MaDM;
END;
GO

------
CREATE OR ALTER PROCEDURE Proc_AddSanPham
(
    @MaDM INT,
    @TenSP NVARCHAR(100),
    @MoTa NVARCHAR(255),
    @DonVi NVARCHAR(30),
    @HinhAnh NVARCHAR(500),
    @TrangThai BIT
)
AS
BEGIN
    SET NOCOUNT ON;
    
    IF EXISTS (SELECT 1 FROM SanPham WHERE TenSP = @TenSP)
    BEGIN
        RAISERROR('Tên sản phẩm đã tồn tại.', 16, 1);
        RETURN;
    END

    IF @MaDM IS NOT NULL AND NOT EXISTS (SELECT 1 FROM DanhMuc WHERE MaDM = @MaDM)
    BEGIN
        RAISERROR('Danh mục không tồn tại.', 16, 1);
        RETURN;
    END

    INSERT INTO SanPham (MaDM, TenSP, MoTa, DonVi, HinhAnh, TrangThai)
    VALUES (@MaDM, @TenSP, @MoTa, @DonVi, @HinhAnh, @TrangThai);
END;
GO

CREATE OR ALTER PROCEDURE Proc_UpdateSanPham
(
    @MaSP INT,
    @MaDM INT,
    @TenSP NVARCHAR(100),
    @Gia DECIMAL(18, 0),
    @MoTa NVARCHAR(255),
    @DonVi NVARCHAR(30),
    @HinhAnh NVARCHAR(500),
    @TrangThai BIT
)
AS
BEGIN
    SET NOCOUNT ON;

    IF NOT EXISTS (SELECT 1 FROM SanPham WHERE MaSP = @MaSP)
    BEGIN
        RAISERROR('Sản phẩm không tồn tại.', 16, 1);
        RETURN;
    END

    IF EXISTS (SELECT 1 FROM SanPham WHERE TenSP = @TenSP AND MaSP != @MaSP)
    BEGIN
        RAISERROR('Tên sản phẩm đã tồn tại.', 16, 1);
        RETURN;
    END

    IF @MaDM IS NOT NULL AND NOT EXISTS (SELECT 1 FROM DanhMuc WHERE MaDM = @MaDM)
    BEGIN
        RAISERROR('Danh mục không tồn tại.', 16, 1);
        RETURN;
    END

    UPDATE SanPham
    SET MaDM = @MaDM,
        TenSP = @TenSP,
        Gia = @Gia,
        MoTa = @MoTa,
        DonVi = @DonVi,
        HinhAnh = @HinhAnh,
        TrangThai = @TrangThai
    WHERE MaSP = @MaSP;
END;
GO

CREATE OR ALTER PROCEDURE Proc_DeleteSanPham
(
    @MaSP INT
)
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE LoSanPham
    SET MaSP = NULL
    WHERE MaSP = @MaSP;

    UPDATE ChiTietDonBan
    SET MaSP = NULL
    WHERE MaSP = @MaSP;

    DELETE FROM SanPham
    WHERE MaSP = @MaSP;
END;
GO


														-- Lô --
CREATE OR ALTER PROCEDURE Proc_AddBatch
(
    @IdSanPham INT,
    @SoLuongNhap INT,
    @GiaNhap DECIMAL(18, 0),
    @NSX DATE,
    @HSD DATE,
    @TrangThai NVARCHAR(20)
)
AS
BEGIN
    SET NOCOUNT ON;
    
    -- Sửa lỗi syntax từ WHERE-MaSP thành WHERE MaSP
    IF NOT EXISTS (SELECT 1 FROM SanPham WHERE MaSP = @IdSanPham)
    BEGIN
        RAISERROR('San pham voi ID %d khong ton tai trong he thong.', 16, 1, @IdSanPham);
        RETURN;
    END
    
    IF @HSD <= @NSX
    BEGIN
        RAISERROR('Expiry date must be greater than manufacturing date.', 16, 1);
        RETURN;
    END
    
    INSERT INTO LoSanPham (MaSP, SoLuongNhap, GiaNhap, NSX, HSD, TrangThai, SoLuongThucNhap)
    VALUES (@IdSanPham, @SoLuongNhap, @GiaNhap, @NSX, @HSD, @TrangThai, @SoLuongNhap);
END;
GO

CREATE OR ALTER PROCEDURE Proc_UpdateBatch
(
    @MaLo INT,
    @SoLuongNhap INT,
    @GiaNhap DECIMAL(18, 0),
    @NSX DATE,
    @HSD DATE,
    @TrangThai NVARCHAR(20),
    @SoLuongLoi INT,
    @GhiChu NVARCHAR(255)
)
AS
BEGIN
    SET NOCOUNT ON;
    IF @HSD <= @NSX
    BEGIN
        RAISERROR('Expiry date must be greater than manufacturing date.', 16, 1);
        RETURN;
    END
    IF @SoLuongLoi > @SoLuongNhap
    BEGIN
        RAISERROR('Quantity shortage must be less than or equal to quantity in batch.', 16, 1);
        RETURN;
    END
    UPDATE LoSanPham
    SET 
        SoLuongNhap = @SoLuongNhap,
        GiaNhap = @GiaNhap,
        NSX = @NSX,
        HSD = @HSD,
        TrangThai = @TrangThai,
        SoLuongLoi = @SoLuongLoi,
        GhiChu = @GhiChu,
		SoLuongThucNhap = @SoLuongNhap
    WHERE MaLo = @MaLo;
END;
GO

CREATE OR ALTER PROCEDURE Proc_DeleteBatch
    @MaLo INT
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM ChiTietDonNhap
    WHERE MaLo = @MaLo;
    DELETE FROM LoSanPham
    WHERE MaLo = @MaLo;
END;
GO

															-- Đơn nhập --
-- Thủ tục thêm hóa đơn nhập
CREATE OR ALTER PROCEDURE Proc_AddHoaDonNhap
    @MaNV INT,
    @MaNCC INT,
    @NgayTao DATETIME,
    @TongTien DECIMAL(18, 0)
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO HoaDonNhap (MaNV, MaNCC, NgayTao, TongTien)
    VALUES (@MaNV, @MaNCC, @NgayTao, @TongTien);
END;
GO

-- Thủ tục thêm chi tiết hóa đơn nhập
CREATE OR ALTER PROCEDURE Insert_ChiTietDonNhap
    @MaHDN INT,
    @MaLo INT
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO ChiTietDonNhap (MaHDN, MaLo)
    VALUES (@MaHDN, @MaLo);
END;
GO

-- Thủ tục xóa hóa đơn nhập
CREATE OR ALTER PROCEDURE Proc_DeleteHoaDonNhap
    @MaHDN INT
AS
BEGIN
    SET NOCOUNT ON;
    IF NOT EXISTS (SELECT 1 FROM HoaDonNhap WHERE MaHDN = @MaHDN)
    BEGIN
        RAISERROR('Hóa đơn nhập không tồn tại.', 16, 1);
        RETURN;
    END

    DELETE FROM LoSanPham
    WHERE MaLo IN (
        SELECT lsp.MaLo 
        FROM ChiTietDonNhap ctdn
        RIGHT JOIN LoSanPham lsp ON ctdn.MaLo = lsp.MaLo
        WHERE ctdn.MaHDN = @MaHDN OR ctdn.MaHDN IS NULL
    );

    DELETE FROM HoaDonNhap
    WHERE MaHDN = @MaHDN;
END;
GO


																-- Đơn bán --
CREATE OR ALTER PROCEDURE Proc_AddSaleInvoice
    @MaNV INT,
    @MaKH INT,
    @NgayTao DATETIME,
    @TrangThai NVARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON;
    IF @NgayTao > GETDATE()
    BEGIN
        RAISERROR('Ngày tạo không thể là ngày trong tương lai.', 16, 1);
        RETURN;
    END
    IF @TrangThai NOT IN ('Pending', 'Completed')
    BEGIN
        RAISERROR('Trạng thái phải là Đang chờ hoặc Đã hoàn thành.', 16, 1);
        RETURN;
    END
    INSERT INTO HoaDonBan (MaNV, MaKH, NgayTao, TrangThai)
    VALUES (@MaNV, @MaKH, @NgayTao, @TrangThai);
END;
GO

CREATE OR ALTER PROCEDURE Proc_AddSaleDetail
    @MaHDB INT,
    @MaSP INT,
    @SoLuongBan INT,
    @DonGia DECIMAL(18, 0)
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @SoLuongTon INT;
    SELECT @SoLuongTon = SoLuong
    FROM SanPham
    WHERE MaSP = @MaSP;
    IF @SoLuongBan > @SoLuongTon
    BEGIN
        RAISERROR('Không đủ hàng cho sản phẩm này.', 16, 1);
        RETURN;
    END
    INSERT INTO ChiTietDonBan (MaHDB, MaSP, SoLuongBan, DonGia)
    VALUES (@MaHDB, @MaSP, @SoLuongBan, @DonGia);
END;
GO

CREATE OR ALTER PROCEDURE Proc_DeleteSaleInvoice
    @MaHDB INT
AS
BEGIN
    SET NOCOUNT ON;
    IF (SELECT TrangThai FROM HoaDonBan WHERE MaHDB = @MaHDB) NOT IN ('Pending')
    BEGIN
        RAISERROR('Chỉ những hóa đơn có trạng thái "Pending" mới có thể bị xóa.', 16, 1);
        RETURN;
    END
    DELETE FROM HoaDonBan
    WHERE MaHDB = @MaHDB;
END;
GO

CREATE OR ALTER PROCEDURE Proc_UpdateSaleInvoiceStatus
    @MaHDB INT,
    @TrangThaiMoi NVARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE HoaDonBan
    SET TrangThai = @TrangThaiMoi
    WHERE MaHDB = @MaHDB;
END;
GO


										-- Thống kê --
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
            Revenue DECIMAL(18, 0)
        );
        WHILE @MonthCounter <= 12
        BEGIN
            SET @StartDate = DATEFROMPARTS(@Year, @MonthCounter, 1);
            SET @EndDate = EOMONTH(@StartDate);
            INSERT INTO #MonthlyRevenue (MonthNumber, Revenue)
            SELECT 
                @MonthCounter AS MonthNumber,
                ISNULL(SUM(CTDB.DonGia * CTDB.SoLuongBan), 0) AS Revenue
            FROM ChiTietDonBan CTDB
            INNER JOIN HoaDonBan HDB ON CTDB.MaHDB = HDB.MaHDB
            WHERE HDB.NgayTao >= @StartDate AND HDB.NgayTao <= @EndDate
                AND HDB.TrangThai = 'Completed'
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
            ISNULL(SUM(CTDB.DonGia * CTDB.SoLuongBan), 0) AS Revenue
        FROM ChiTietDonBan CTDB
        INNER JOIN HoaDonBan HDB ON CTDB.MaHDB = HDB.MaHDB
        WHERE HDB.NgayTao >= @StartDate AND HDB.NgayTao <= @EndDate
            AND HDB.TrangThai = 'Completed'
    END
END;
GO

CREATE OR ALTER PROCEDURE ThongKeDoanhThuTheoQuy
    @Quarter INT = NULL,
    @Year INT
AS
BEGIN
    CREATE TABLE #QuarterInfo (
        QuarterNumber INT,
        StartMonth INT,
        EndMonth INT
    );

    INSERT INTO #QuarterInfo (QuarterNumber, StartMonth, EndMonth)
    VALUES 
        (1, 1, 3),
        (2, 4, 6),
        (3, 7, 9),
        (4, 10, 12);

    IF @Quarter IS NULL
    BEGIN
        CREATE TABLE #QuarterlyRevenue (
            QuarterNumber INT,
            Revenue DECIMAL(18, 0)
        );
        
        INSERT INTO #QuarterlyRevenue (QuarterNumber, Revenue)
        SELECT 
            QI.QuarterNumber,
            ISNULL(
                (SELECT SUM(CTDB.DonGia * CTDB.SoLuongBan)
                 FROM ChiTietDonBan CTDB
                 INNER JOIN HoaDonBan HDB ON CTDB.MaHDB = HDB.MaHDB
                 WHERE HDB.NgayTao >= DATEFROMPARTS(@Year, QI.StartMonth, 1)
                   AND HDB.NgayTao <= EOMONTH(DATEFROMPARTS(@Year, QI.EndMonth, 1))
                   AND HDB.TrangThai = 'Completed'), 0) AS Revenue
        FROM #QuarterInfo QI;
        
        SELECT 
            QuarterNumber AS [Quý],
            Revenue AS [Doanh Thu]
        FROM #QuarterlyRevenue
        ORDER BY QuarterNumber;

        DROP TABLE #QuarterlyRevenue;
    END
    ELSE
    BEGIN
        DECLARE @StartMonth INT, @EndMonth INT;
        
        SELECT @StartMonth = StartMonth, @EndMonth = EndMonth
        FROM #QuarterInfo
        WHERE QuarterNumber = @Quarter;
        SELECT 
            @Quarter AS [Quý],
            ISNULL(SUM(CTDB.DonGia * CTDB.SoLuongBan), 0) AS [Doanh Thu]
        FROM ChiTietDonBan CTDB
        INNER JOIN HoaDonBan HDB ON CTDB.MaHDB = HDB.MaHDB
        WHERE HDB.NgayTao >= DATEFROMPARTS(@Year, @StartMonth, 1)
          AND HDB.NgayTao <= EOMONTH(DATEFROMPARTS(@Year, @EndMonth, 1))
          AND HDB.TrangThai = 'Completed';
    END
    DROP TABLE #QuarterInfo;
END;
GO


-- VIEW --
GO


CREATE OR ALTER VIEW View_Full_Product_Info AS
SELECT 
    sp.MaSP AS [id],
    sp.TenSP AS [name],
    ISNULL(dm.TenDM, N'') AS [category_name],
    ls.NSX AS [manu],
    ls.HSD AS [ex],
    sp.SoLuong AS [quantity],
    sp.Gia AS [price],
	sp.DonVi AS [dv],
	sp.MoTa AS [mt],
    sp.TrangThai AS [is_active],
    sp.HinhAnh AS [image_path]
FROM 
    SanPham sp
LEFT JOIN 
    DanhMuc dm ON sp.MaDM = dm.MaDM
LEFT JOIN 
    (
        SELECT MaSP, NSX, HSD,
               ROW_NUMBER() OVER (PARTITION BY MaSP ORDER BY NSX DESC) AS rn
        FROM LoSanPham
    ) ls ON sp.MaSP = ls.MaSP AND ls.rn = 1;

GO

------ HDN - BATCH -----
CREATE OR ALTER VIEW View_HoaDonNhap_Details
AS
SELECT 
    hdn.MaHDN AS id,
    hdn.NgayTao AS date,
    hdn.TongTien AS total,
    ncc.TenNCC AS name_supplier,
    nv.TenNV AS name_staff
FROM 
    HoaDonNhap hdn
LEFT JOIN 
    NhaCungCap ncc ON hdn.MaNCC = ncc.MaNCC
LEFT JOIN 
    NhanVien nv ON hdn.MaNV = nv.MaNV
GO

CREATE OR ALTER VIEW View_LoSanPham_Details
AS
SELECT 
    lsp.MaLo AS id_batch,
    lsp.MaSP AS id_product,
    sp.TenSP AS name_product,
    lsp.SoLuongNhap AS quantity_in_batch,
    lsp.GiaNhap AS entry_price,
    lsp.NSX AS manufacturing_date,
    lsp.HSD AS expiry_date,
    lsp.TrangThai AS status,
    ctdn.MaHDN AS id_purchase,
    hdn.TongTien AS total_amount,
	lsp.SoLuongThucNhap AS SlThuc,
	lsp.SoLuongLoi AS Loi
FROM 
    LoSanPham lsp
LEFT JOIN 
    SanPham sp ON lsp.MaSP = sp.MaSP
LEFT JOIN 
    ChiTietDonNhap ctdn ON lsp.MaLo = ctdn.MaLo
LEFT JOIN 
    HoaDonNhap hdn ON ctdn.MaHDN = hdn.MaHDN
GO

CREATE OR ALTER VIEW View_Batch_Details
AS
SELECT 
    lo.MaLo AS id,
    ct.MaHDN AS id_purchase,
    sp.TenSP AS name_medicine,
    lo.SoLuongNhap AS q_i_b,
    lo.GiaNhap AS price,
    lo.NSX AS manu,
    lo.HSD AS exp,
    lo.SoLuongLoi AS q_s,
    lo.GhiChu AS note,
    lo.TrangThai AS sta,
    sp.TrangThai AS medicine_is_active,
	lo.SoLuongThucNhap AS SlThuc, 
	lo.SoLuongLoi AS Loi
FROM 
    LoSanPham lo
LEFT JOIN 
    SanPham sp ON lo.MaSP = sp.MaSP
LEFT JOIN 
    ChiTietDonNhap ct ON lo.MaLo = ct.MaLo;
GO

CREATE OR ALTER VIEW View_Purchase_Invoice_Details
AS
SELECT 
    hdn.MaHDN AS id,
    hdn.NgayTao AS date,
    hdn.TongTien AS total,
    ncc.TenNCC AS name_supplier,
    nv.TenNV AS name_staff
FROM 
    HoaDonNhap hdn
LEFT JOIN 
    NhaCungCap ncc ON hdn.MaNCC = ncc.MaNCC
LEFT JOIN 
    NhanVien nv ON hdn.MaNV = nv.MaNV;
GO

CREATE OR ALTER VIEW View_Batch_With_Medicine
AS
SELECT 
    lo.MaLo AS id_batch,
    lo.MaSP AS id_medicine,
    sp.TenSP AS name_medicine,
    lo.SoLuongNhap AS quantity_in_batch,
    lo.GiaNhap AS entry_price,
    lo.NSX AS manufacturing_date,
    lo.HSD AS expiry_date,
    lo.TrangThai AS status,
    lo.SoLuongLoi AS quantity_shortage,
    lo.GhiChu AS note
FROM 
    LoSanPham lo
LEFT JOIN 
    SanPham sp ON lo.MaSP = sp.MaSP;
GO

CREATE OR ALTER VIEW View_Batch_Purchase_Details
AS
SELECT 
    lo.MaLo AS id_batch,
    lo.MaSP AS id_medicine,
    sp.TenSP AS name_medicine,
    lo.SoLuongNhap AS quantity_in_batch,
    lo.GiaNhap AS entry_price,
    lo.NSX AS manufacturing_date,
    lo.HSD AS expiry_date,
    lo.TrangThai AS status,
    ct.MaHDN AS id_purchase,
	hdn.TongTien AS total_amount

FROM 
    LoSanPham lo
LEFT JOIN 
    SanPham sp ON lo.MaSP = sp.MaSP
LEFT JOIN 
    ChiTietDonNhap ct ON lo.MaLo = ct.MaLo
LEFT JOIN 
    HoaDonNhap hdn ON ct.MaHDN = hdn.MaHDN;
GO

--- SALE --
CREATE OR ALTER VIEW View_Sale_Invoice_Details
AS
SELECT 
    hdb.MaHDB AS id_sale,
    nv.TenNV AS name_staff,
    kh.TenKH AS name_customer,
    hdb.NgayTao AS date_create,
    hdb.TrangThai AS status,
    ctdb.DonGia AS Price,
    ctdb.SoLuongBan AS quantity,
    sp.TenSP AS name_medicine,
    (ctdb.SoLuongBan * ctdb.DonGia) AS total_amount
FROM 
    HoaDonBan hdb
LEFT JOIN 
    NhanVien nv ON hdb.MaNV = nv.MaNV
LEFT JOIN 
    KhachHang kh ON hdb.MaKH = kh.MaKH
LEFT JOIN 
    ChiTietDonBan ctdb ON hdb.MaHDB = ctdb.MaHDB
LEFT JOIN 
    SanPham sp ON ctdb.MaSP = sp.MaSP
GO


CREATE OR ALTER VIEW View_Sale_Invoice_Details_2
AS
SELECT 
    hdb.MaHDB AS id_sale,
    nv.TenNV AS name_staff,
    kh.TenKH AS name_customer,
    hdb.NgayTao AS date_create,
    hdb.TrangThai AS status,
    SUM(ctdb.SoLuongBan * ctdb.DonGia) AS total_amount
FROM 
    HoaDonBan hdb
LEFT JOIN 
    NhanVien nv ON hdb.MaNV = nv.MaNV
LEFT JOIN 
    KhachHang kh ON hdb.MaKH = kh.MaKH
LEFT JOIN 
    ChiTietDonBan ctdb ON hdb.MaHDB = ctdb.MaHDB
LEFT JOIN 
    SanPham sp ON ctdb.MaSP = sp.MaSP
GROUP BY 
    hdb.MaHDB, nv.TenNV, kh.TenKH, hdb.NgayTao, hdb.TrangThai
GO

--- Thống kê ----
CREATE OR ALTER VIEW View_Counts
AS
SELECT 
    (SELECT COUNT(*) FROM KhachHang) AS TotalCustomers,
    (SELECT COUNT(*) FROM SanPham) AS TotalMedicines,
    (SELECT COUNT(*) FROM NhanVien) AS TotalStaff,
    (SELECT ISNULL(SUM(CTDB.SoLuongBan * CTDB.DonGia), 0) 
     FROM ChiTietDonBan CTDB
     INNER JOIN HoaDonBan HDB ON CTDB.MaHDB = HDB.MaHDB
     WHERE HDB.TrangThai = 'Completed') AS TotalRevenue,
    (SELECT ISNULL((
        (SELECT ISNULL(SUM(CTDB.SoLuongBan * CTDB.DonGia), 0) 
         FROM ChiTietDonBan CTDB
         INNER JOIN HoaDonBan HDB ON CTDB.MaHDB = HDB.MaHDB
         WHERE HDB.TrangThai = 'Completed') -- Total Revenue
        - 
        (SELECT ISNULL(SUM(LSP.SoLuongThucNhap * LSP.GiaNhap), 0) 
         FROM LoSanPham LSP) -- Total Cost using SoLuongThucNhap
    ), 0)) AS TotalProfit;
GO

CREATE OR ALTER VIEW View_TopSale
AS
SELECT 
    SP.TenSP AS ProductName,
    SUM(CTDB.SoLuongBan) AS TotalQuantitySold,
    SUM(CTDB.SoLuongBan * CTDB.DonGia) AS TotalRevenue
FROM 
    ChiTietDonBan CTDB
JOIN 
    SanPham SP ON CTDB.MaSP = SP.MaSP
GROUP BY 
    SP.TenSP;
GO

CREATE OR ALTER VIEW View_BatchExpiryStatus
AS
SELECT 
    LSP.MaLo AS id_batch,
    SP.TenSP AS name_medicine,
    LSP.SoLuongNhap AS quantity_in_batch,
    LSP.NSX AS manufacturing_date,
    LSP.HSD AS expiry_date,
    LSP.SoLuongLoi AS quantity_shortage,
    LSP.GhiChu AS note,
    LSP.TrangThai AS status
FROM 
    LoSanPham LSP
JOIN 
    SanPham SP ON LSP.MaSP = SP.MaSP
WHERE 
    LSP.SoLuongLoi > 0
    OR (
        LSP.HSD <= DATEADD(MONTH, 3, GETDATE()) -- Expiring within 3 months
        OR LSP.HSD < GETDATE() -- Already expired
    );
GO

CREATE OR ALTER VIEW vw_LoaiSanPhamBanChayTrongThang AS
SELECT 
    dm.MaDM,
    dm.TenDM,
    SUM(ctdb.SoLuongBan) AS TongSoLuongBan,
    SUM(ctdb.SoLuongBan * ctdb.DonGia) AS TongDoanhThu
FROM 
    HoaDonBan hdb
    INNER JOIN ChiTietDonBan ctdb ON hdb.MaHDB = ctdb.MaHDB
    INNER JOIN SanPham sp ON ctdb.MaSP = sp.MaSP
    INNER JOIN DanhMuc dm ON sp.MaDM = dm.MaDM
WHERE 
    YEAR(hdb.NgayTao) = YEAR(GETDATE())
    AND MONTH(hdb.NgayTao) = MONTH(GETDATE())
GROUP BY 
    dm.MaDM,
    dm.TenDM;


-- TRIGGER --
GO

-- Trigger cập nhật trạng thái lô sản phẩm
CREATE OR ALTER TRIGGER Trigger_UpdateBatchStatus
ON LoSanPham
AFTER INSERT, UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE lo
    SET TrangThai = 'Inactive'
    FROM LoSanPham lo
    INNER JOIN inserted i ON lo.MaLo = i.MaLo
    WHERE i.HSD <= CAST(GETDATE() AS DATE);
    
    UPDATE lo
    SET TrangThai = 'Inactive'
    FROM LoSanPham lo
    INNER JOIN inserted i ON lo.MaLo = i.MaLo
    WHERE (i.SoLuongLoi = i.SoLuongNhap AND i.SoLuongLoi > 0);
END;
GO


-- Trigger cập nhật thông tin sản phẩm từ lô sản phẩm
CREATE OR ALTER TRIGGER Trigger_UpdateProductFromBatch
ON LoSanPham
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @AffectedProducts TABLE (MaSP INT);
    
    INSERT INTO @AffectedProducts (MaSP)
    SELECT MaSP FROM inserted
    UNION
    SELECT MaSP FROM deleted;
    
    -- Cập nhật số lượng sản phẩm
    UPDATE sp
    SET SoLuong = ISNULL((
        SELECT SUM(lo.SoLuongNhap - lo.SoLuongLoi)
        FROM LoSanPham lo
        WHERE lo.MaSP = sp.MaSP
          AND (lo.TrangThai IS NULL OR lo.TrangThai = 'Active')
          AND lo.SoLuongNhap >= lo.SoLuongLoi
          AND lo.HSD > CAST(GETDATE() AS DATE)
    ), 0)
    FROM SanPham sp
    WHERE sp.MaSP IN (SELECT MaSP FROM @AffectedProducts);

    -- Cập nhật trạng thái sản phẩm
    UPDATE sp
    SET TrangThai = CASE
        WHEN sp.SoLuong = 0 THEN 0 
        WHEN EXISTS (
            SELECT 1
            FROM LoSanPham lo
            WHERE lo.MaSP = sp.MaSP
              AND lo.HSD <= CAST(GETDATE() AS DATE)
        ) THEN 0 
        ELSE 1
    END
    FROM SanPham sp
    WHERE sp.MaSP IN (SELECT MaSP FROM @AffectedProducts);
END;
GO

-- Trigger cập nhật kho sau khi bán
CREATE OR ALTER TRIGGER Trigger_UpdateStockAfterSale
ON ChiTietDonBan
AFTER INSERT, UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    
    -- Bảng tạm lưu thay đổi số lượng bán
    DECLARE @SaleChanges TABLE (
        MaSP INT,
        quantity_change INT
    );
    
    -- Tính toán sự thay đổi số lượng bán
    INSERT INTO @SaleChanges (MaSP, quantity_change)
    SELECT 
        i.MaSP,
        i.SoLuongBan - ISNULL((SELECT d.SoLuongBan FROM deleted d WHERE d.MaHDB = i.MaHDB AND d.MaSP = i.MaSP), 0) AS quantity_change
    FROM inserted i;
    
    -- Cập nhật số lượng tổng của sản phẩm
    UPDATE sp
    SET SoLuong = sp.SoLuong - sc.quantity_change
    FROM SanPham sp
    INNER JOIN @SaleChanges sc ON sp.MaSP = sc.MaSP
    WHERE sc.quantity_change > 0;
    
    -- Cập nhật từng lô sản phẩm (FIFO - First In First Out)
    DECLARE @MaSP INT, @quantity_to_deduct INT;
    DECLARE sale_cursor CURSOR FOR
    SELECT MaSP, quantity_change 
    FROM @SaleChanges 
    WHERE quantity_change > 0;
    
    OPEN sale_cursor;
    FETCH NEXT FROM sale_cursor INTO @MaSP, @quantity_to_deduct;
    
    WHILE @@FETCH_STATUS = 0
    BEGIN
        WHILE @quantity_to_deduct > 0
        BEGIN
            DECLARE @MaLo INT, @available_quantity INT;
            
            -- Lấy lô có HSD gần nhất và còn hàng
            SELECT TOP 1 
                @MaLo = MaLo,
                @available_quantity = SoLuongNhap
            FROM LoSanPham
            WHERE MaSP = @MaSP
              AND (TrangThai IS NULL OR TrangThai = 'Active')
              AND HSD > CAST(GETDATE() AS DATE)
              AND SoLuongNhap > 0
            ORDER BY HSD ASC;
            
            -- Nếu không còn lô nào khả dụng
            IF @MaLo IS NULL
            BEGIN
                -- Có thể thêm logic báo lỗi hoặc ghi log ở đây
                BREAK;
            END
            
            -- Tính số lượng cần trừ từ lô này
            DECLARE @deduct_from_batch INT = CASE 
                WHEN @available_quantity >= @quantity_to_deduct THEN @quantity_to_deduct
                ELSE @available_quantity
            END;
            
            -- Cập nhật số lượng trong lô (trừ vào SoLuongNhap)
            UPDATE LoSanPham
            SET SoLuongNhap = SoLuongNhap - @deduct_from_batch
            WHERE MaLo = @MaLo;
            
            -- Giảm số lượng còn phải trừ
            SET @quantity_to_deduct = @quantity_to_deduct - @deduct_from_batch;
        END
        
        FETCH NEXT FROM sale_cursor INTO @MaSP, @quantity_to_deduct;
    END
    
    CLOSE sale_cursor;
    DEALLOCATE sale_cursor;
END;
GO

-- Trigger khôi phục kho sau khi xóa đơn bán hàng
CREATE OR ALTER TRIGGER Trigger_RestoreStockAfterDeleteSale
ON ChiTietDonBan
AFTER DELETE
AS
BEGIN
    SET NOCOUNT ON;
    
    -- Khôi phục số lượng sản phẩm
    UPDATE SanPham
    SET SoLuong = SoLuong + d.SoLuongBan
    FROM SanPham sp
    INNER JOIN deleted d ON sp.MaSP = d.MaSP;
    
    -- Khôi phục số lượng trong lô (giảm số lượng lỗi)
    UPDATE LoSanPham
    SET SoLuongLoi = SoLuongLoi - d.SoLuongBan
    FROM LoSanPham lo
    INNER JOIN deleted d ON lo.MaSP = d.MaSP
    WHERE lo.MaLo IN (
        SELECT TOP 1 MaLo
        FROM LoSanPham lo2
        WHERE lo2.MaSP = d.MaSP
          AND lo2.HSD >= CAST(GETDATE() AS DATE) 
          AND (lo2.TrangThai IS NULL OR lo2.TrangThai = 'Active') 
        ORDER BY lo2.HSD ASC 
    );
END;
GO
