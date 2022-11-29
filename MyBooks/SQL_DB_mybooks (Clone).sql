use master;
go
Drop database DB_MYBOOKS;
GO
CREATE DATABASE DB_MYBOOKS;
go
use DB_MYBOOKS;
go
-- Bang AdminUser
CREATE TABLE [dbo].[AdminUser] (
    [ID]           INT    primary key        NOT NULL,
    [NameUser]     NVARCHAR (MAX) NULL,
    [PasswordUser] NCHAR (50)     NULL,
);
insert into AdminUser values 
('1412','admin', 'admin')

--Bang Customer 
CREATE TABLE [dbo].[Customer] (
    [IDCus]		INT PRIMARY KEY NOT NULL,
    [NameCus]	NVARCHAR (MAX) NULL,
	[PassCus]	NVARCHAR (MAX) NULL,
    [PhoneCus]	NVARCHAR (15)  NULL,
    [EmailCus]	NVARCHAR (MAX) NULL,
)
-- tại customer mặc định để test
insert into Customer values
(123, 'Phuoc', '123', '123123123', 'dfzdfh@gmail.com');
--BAng Category
create table Category(
	[MaCate] char(20) primary key not null,
	[MoTa] nvarchar(max) not null,
)
insert into Category values 
('VANHOC', N'Văn Học'),
('KINHTE',N'Kinh Tế'),
('TAMLI',N'Tâm Lí'),
('GIAODUC',N'Giáo Dục'),
('THIEUNHI',N'Thiếu Nhi');


--Bang book
CREATE TABLE [dbo].[Book] (
    [BookID]		nvarchar(10) PRIMARY KEY NOT NULL,
    [BookName]		NVARCHAR (MAX)  NULL,
	[NXB]			nvarchar(max),
	[NCC]			nvarchar(max),
    [Category]		char(20) null,
	[Unit]			nvarchar(max) default N'Cuốn',
    [Price]			DECIMAL (18, 2) NULL,
	[SLuong] int null,
	[DateAdd] date,
    [Imagebook]	NVARCHAR (MAX)  NULL,
    FOREIGN KEY ([Category]) REFERENCES [dbo].[Category]([MaCate])
)

INSERT INTO [dbo].[Book]  VALUES 
(N'001',N'PATHWAYS',N'Kim Đồng', N'Mạc Văn Khoa', N'GIAODUC', N'Cuốn', 341000,10, '2022-11-25', N'test_product_anh1.jpg'),
(N'002',N'CHIÊM TINH HỌC', N'Hồ Hạc', N'Hồng Hướng','VANHOC', N'Cuốn', 341000,10,'2022-11-23', N'test_product_anh2.jpg'),
(N'003',N'KINH DOANH',N'Văn Lang', N'Tiểu Vân','GIAODUC', N'Cuốn', 341000,10,'2022-11-25', N'test_product_anh3.jpg'),
(N'004',N'LỊCH SỬ TƯƠNG LAI', N'Trùng Hiếu',N'Lê Khoa','GIAODUC', N'Cuốn', 114000,10,'2022-11-25', N'test_product_anh4.jpg'),
(N'005',N'TOKYO REVENGER TẬP 6', N'Trương Tú',N'Hoàng Hoa Thám','THIEUNHI', N'Cuốn', 115000,10,'2022-11-25', N'test_product_anh5.jpg');

-- BẢNG VOUCHER
CREATE TABLE VOUCHER(
	MAVOUCHER char(10) primary key,
	GIAM decimal(10,2),
	MoTa Nvarchar(max) null	
)
insert into VOUCHER values
( 'SALE10', 5.00/100, N'Siêu sale 10% giá'),
('BIGSALE', 20.0/100, N'Sale 20% giá sản phẩm'),
('SPECIAL', 40.0/100, N'Sale 40% giá sản phẩm');

--Bang OrderPro Hóa đơn
CREATE TABLE [dbo].[OrderPro] (
    [ID]               INT       PRIMARY KEY     IDENTITY (1, 1) NOT NULL,
    [DateOrder]        DATE           NULL,
    [IDCus]            INT            NULL,
    [AddressDeliverry] NVARCHAR (MAX) NULL,
    FOREIGN KEY ([IDCus]) REFERENCES [dbo].[Customer]([IDCus])
);
--Bang OrderDetail Chi tiết hóa đơn
CREATE TABLE [dbo].[OrderDetail] (
    [ID]        INT        IDENTITY (1, 1) PRIMARY KEY NOT NULL,
    [IDProduct] NVArCHAR(10)        NULL,
    [IDOrder]   INT        NULL,
    [Quantity]  INT        NULL,
    [PriceOld] FLOAT (53) NULL,
	[PriceNew] FLOAT (53) NULL,
	[IDVoucher] char(10),
	FOREIGN KEY ([IDProduct]) REFERENCES [dbo].[book] ([BookID]),
    FOREIGN KEY ([IDOrder]) REFERENCES [dbo].[OrderPro] ([ID]),
    FOREIGN KEY ([IDVoucher]) REFERENCES [dbo].[VOUCHER] ([MAVOUCHER]),

);

-- BẢNG LƯU TRỮ GIỎ HÀNG CỦA NGƯỜI DÙNG
CREATE TABLE [dbo].[CartList] (
	[IDCartList] int identity(1,1) primary key not null,
	[IDCus]		int null,
	[BookID]	NVArCHAR(10) null,
	[IDVoucher] char(10),
	FOREIGN KEY ([IDCus]) references [dbo].[Customer]([IDCus]),
	FOREIGN KEY ([BookID]) references [dbo].[book]([BookID]),
	FOREIGN KEY ([IDVoucher]) references [dbo].[VOUCHER]([MAVOUCHER])
)


create table voucher_pro (
	[ID] int identity(1,1) primary key not null,
	[IDVoucher] char(10) not null,
	[IDBook] NVArCHAR(10) NOT NULL,
	[Script] NVARCHAR(MAX) NULL,
	FOREIGN KEY ([IDVoucher]) references [dbo].[VOUCHER]([MAVOUCHER]),
	FOREIGN KEY ([IDBook]) references [dbo].[Book]([BookID]),

)
insert into voucher_pro (IDVoucher, IDBook) values
('SALE10', N'001'),
('SALE10', N'002'),
('SALE10', N'003'),
('BIGSALE', N'001'),
('BIGSALE', N'002');

select*
from Category
select*
from Customer
select*
from OrderPro
select*
from OrderDetail
select*
from VOUCHER
select*
from voucher_pro
select*
from AdminUser
select*
from book
select*
from CartList
