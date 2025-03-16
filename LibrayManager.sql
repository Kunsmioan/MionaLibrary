

create database LibraryManager
-- Bảng Users (Người dùng)
use LibraryManager

CREATE TABLE [dbo].[Users] (
    [Id]       INT            IDENTITY (1, 1) NOT NULL,
    [Username] NVARCHAR (50)  NOT NULL UNIQUE,
    [Password] NVARCHAR (255) NOT NULL,
    [FirstName] NVARCHAR (100) NOT NULL,
	[LastName] NVARCHAR (100) NOT NULL,
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

INSERT INTO [dbo].[Users] ([Username], [Password], [FirstName], [LastName], [Birthday], [Role], [Gender], [Phone])
VALUES 
    ('john_doe', 'password123', 'John', 'Doe', '1990-05-15', 'User', 'Male', '123-456-7890'),
    ('jane_smith', 'securepass', 'Jane', 'Smith', '1985-08-22', 'User', 'Female', '987-654-3210'),
    ('alice_jones', 'alicepass', 'Alice', 'Jones', '1995-03-10', 'User', 'Female', NULL),
    ('bob_brown', 'bobspass', 'Bob', 'Brown', '1980-11-25', 'User', 'Male', '555-555-5555'),
    ('charlie_wilson', 'charlie123', 'Charlie', 'Wilson', '1975-07-04', 'User', 'Male', '444-444-4444'),
    ('emily_davis', 'emilypass', 'Emily', 'Davis', '1992-09-18', 'User', 'Female', NULL),
    ('michael_miller', 'mikepass', 'Michael', 'Miller', '1988-12-30', 'User', 'Male', '333-333-3333'),
    ('sarah_johnson', 'sarahpass', 'Sarah', 'Johnson', '1998-02-14', 'User', 'Female', '222-222-2222'),
    ('david_wilson', 'davidpass', 'David', 'Wilson', '1970-06-01', 'User', 'Male', NULL),
    ('laura_taylor', 'laurapass', 'Laura', 'Taylor', '1982-04-12', 'Manager', 'Female', '111-111-1111'),
    ('steven_anderson', 'stevenpass', 'Steven', 'Anderson', '1993-07-20', 'User', 'Male', '666-666-6666'),
    ('olivia_thomas', 'oliviapass', 'Olivia', 'Thomas', '1987-01-05', 'User', 'Female', NULL),
    ('james_jackson', 'jamespass', 'James', 'Jackson', '1977-03-17', 'User', 'Male', '777-777-7777'),
    ('ava_white', 'avapass', 'Ava', 'White', '1996-11-28', 'User', 'Female', '888-888-8888'),
    ('noah_harris', 'noahpass', 'Noah', 'Harris', '1984-05-09', 'User', 'Male', NULL),
    ('mia_clark', 'miapass', 'Mia', 'Clark', '1991-08-23', 'User', 'Female', '999-999-9999'),
    ('liam_lewis', 'liampass', 'Liam', 'Lewis', '1979-10-14', 'User', 'Male', '123-123-1234'),
    ('amelia_young', 'ameliapass', 'Amelia', 'Young', '1994-02-02', 'User', 'Female', NULL),
    ('ethan_king', 'ethanpass', 'Ethan', 'King', '1986-06-19', 'User', 'Male', '432-432-4321'),
    ('isabella_wright', 'isabellapass', 'Isabella', 'Wright', '1997-04-11', 'User', 'Female', '543-543-5432');
