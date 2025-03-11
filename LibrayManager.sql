
use master
drop database LibraryManager

create database LibraryManager
-- Bảng Users (Người dùng)
use LibraryManager

CREATE TABLE [dbo].[Users] (
    [Id]       INT            IDENTITY (1, 1) NOT NULL,
    [Username] NVARCHAR (50)  NOT NULL UNIQUE,
    [Password] NVARCHAR (255) NOT NULL,
    [FullName] NVARCHAR (100) NOT NULL,
    [Birthday] DATE           NOT NULL,
    [Role]     NVARCHAR (20)  NOT NULL CHECK ([Role] IN ('Manager', 'User')),
    [Gender]   NVARCHAR (20)  NOT NULL,
    [Phone]    NCHAR (50)     NULL,
    PRIMARY KEY ([Id])
);
GO

-- Bảng Books (Sách)
CREATE TABLE [dbo].[Books] (
    [Id]          INT            IDENTITY (1, 1) NOT NULL,
    [Title]       NVARCHAR (255) NOT NULL,
    [Author]      NVARCHAR (100) NOT NULL,
    [PublishYear] INT            NOT NULL,
    [ISBN]        NVARCHAR (20)  NOT NULL UNIQUE,
    [Genre]       NVARCHAR (50)  NULL,
    [Quantity]    INT            DEFAULT (1) NOT NULL CHECK ([Quantity] >= 0),
    [Language]    NVARCHAR (50)  NOT NULL,
    [Description] TEXT           NULL,
    [ImagePath]   NVARCHAR (100) NULL,
    [IsAvailable] BIT            DEFAULT (1) NOT NULL,
    [Page]        INT            NULL,
    PRIMARY KEY ([Id])
);
GO

-- Bảng BookTransactions (Giao dịch mượn/trả sách)
CREATE TABLE [dbo].Loans (
    [Id]         INT           IDENTITY (1, 1) NOT NULL,
    [UserId]     INT           NOT NULL,
    [BookId]     INT           NOT NULL,
    [BorrowDate] DATE          DEFAULT (getdate()) NOT NULL,
    [ReturnDate] DATE          NULL,
    [Status]     NVARCHAR (20) NOT NULL CHECK ([Status] IN ('Overdue', 'Returned', 'Borrowed')),
    PRIMARY KEY ([Id]),
    FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users] ([Id]) ON DELETE CASCADE,
    FOREIGN KEY ([BookId]) REFERENCES [dbo].[Books] ([Id]) ON DELETE CASCADE
);
GO

-- Bảng BookReservations (Đặt trước sách)
CREATE TABLE [dbo].[BookReservations] (
    [Id]          INT           IDENTITY (1, 1) NOT NULL,
    [UserId]      INT           NOT NULL,
    [BookId]      INT           NOT NULL,
    [ReserveDate] DATE          DEFAULT (getdate()) NOT NULL,
    [Status]      NVARCHAR (20) NOT NULL CHECK ([Status] IN ('Cancelled', 'Approved', 'Pending')),
    PRIMARY KEY ([Id]),
    FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users] ([Id]) ON DELETE CASCADE,
    FOREIGN KEY ([BookId]) REFERENCES [dbo].[Books] ([Id]) ON DELETE CASCADE
);
GO

-- Bảng BookLogs (Lịch sử thao tác với sách) - Không cần khóa ngoại
CREATE TABLE [dbo].[BookLogs] (
    [Id]         INT           IDENTITY (1, 1) NOT NULL,
    [ManagerId]  INT           NOT NULL,
    [BookId]     INT           NULL,
    [Action]     NVARCHAR (50) NOT NULL CHECK ([Action] IN ('Deleted', 'Updated', 'Added')),
    [ActionDate] DATETIME      DEFAULT (getdate()) NOT NULL,
    PRIMARY KEY ([Id])
);
GO

CREATE TABLE LoanHistory (
    HistoryID INT IDENTITY(1,1) PRIMARY KEY,
    UserID INT NOT NULL,
    BookID INT NOT NULL,
    LoanDate DATETIME NOT NULL,
    DueDate DATETIME NOT NULL,
    ReturnDate DATETIME NOT NULL,
    OverdueDays INT DEFAULT 0,
    FOREIGN KEY (UserID) REFERENCES Users(Id),
    FOREIGN KEY (BookID) REFERENCES Books(Id)
);

DELETE FROM [dbo].[Users];

INSERT INTO [dbo].[Users]
           ([Username]
           ,[Password]
           ,[FullName]
           ,[Birthday]
           ,[Role]
           ,[Gender]
           ,[Phone])
     VALUES   ('Nguyen15', 'Nguyen2000', 'Nguyễn Hoa', '2000-05-15', 'User', 'male', '1234567890'),
           ('Tran20', 'Tran2001', 'Trần Mai', '2001-06-20', 'Manager', 'female', '0987654321'),
           ('Le12', 'Le2002', 'Lê Minh', '2002-04-12', 'User', 'male', '0912345678'),
           ('Nguyen07', 'Nguyen2003', 'Nguyễn Lan', '2003-07-22', 'User', 'female', '0922334455'),
           ('Pham15', 'Pham2000', 'Phạm Quyên', '2000-09-15', 'User', 'female', '0912345679'),
           ('Nguyen11', 'Nguyen2004', 'Nguyễn Tuấn', '2004-11-30', 'User', 'male', '0933344556'),
           ('Nguyen10', 'Nguyen2005', 'Nguyễn Sơn', '2005-12-10', 'User', 'male', '0944455667'),
           ('Hoang25', 'Hoang2006', 'Hoàng Ngọc', '2006-01-25', 'User', 'female', '0955566778'),
           ('Pham14', 'Pham2000', 'Phạm Trang', '2000-02-14', 'User', 'female', '0966677889'),
           ('Truong11', 'Truong2002', 'Trương Linh', '2002-03-11', 'Manager', 'female', '0977788990'),
           ('Nguyen18', 'Nguyen2000', 'Nguyễn Bình', '2000-04-18', 'User', 'male', '0988899001'),
           ('Le28', 'Le2001', 'Lê Chiến', '2001-05-28', 'User', 'male', '0999000112'),
           ('Nguyen30', 'Nguyen2003', 'Nguyễn Hà', '2003-06-30', 'User', 'female', '0911122334'),
           ('Tran14', 'Tran2002', 'Trần Thùy', '2002-07-14', 'User', 'female', '0922233445'),
           ('Le25', 'Le2004', 'Lê Hồng', '2004-08-25', 'User', 'male', '0933344556'),
           ('Nguyen20', 'Nguyen2005', 'Nguyễn Thanh', '2005-09-20', 'User', 'male', '0944455667'),
           ('Hoang15', 'Hoang2006', 'Hoàng Tuấn', '2006-10-15', 'User', 'female', '0955566778'),
           ('Tran18', 'Tran2002', 'Trần Linh', '2002-11-18', 'User', 'female', '0966677889'),
           ('Nguyen12', 'Nguyen2001', 'Nguyễn Mai', '2001-12-12', 'User', 'male', '0977788990'),
           ('Nguyen22', 'Nguyen2000', 'Nguyễn Vinh', '2000-01-22', 'User', 'male', '0988899001');