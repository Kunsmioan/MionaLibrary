

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


CREATE TRIGGER trg_UpdateIsAvailable
ON [dbo].[Books]
AFTER INSERT, UPDATE
AS
BEGIN
    -- Cập nhật IsAvailable dựa trên Quantity
    UPDATE b
    SET b.IsAvailable = CASE 
                            WHEN i.Quantity >= 1 THEN 1 
                            ELSE 0 
                        END
    FROM [dbo].[Books] b
    INNER JOIN inserted i ON b.Id = i.Id;
END;

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


INSERT INTO [dbo].[Books] ([Title], [Author], [PublishYear], [ISBN], [Genre], [Language], [Description], [ImagePath], [Page], [Quantity], [IsAvailable])
VALUES 
-- Book 1
('The Night Circus', 'Erin Morgenstern', 2011, '978-0307744166', 'Fantasy', 'English', 'A magical competition between two young illusionists.', 'images/night-circus.jpg', 387, 5, 1),
-- Book 2
('The Shadow of the Wind', 'Carlos Ruiz Zafón', 2001, '978-0143117355', 'Mystery', 'French', 'A boy discovers a mysterious book that changes his life.', 'images/shadow-wind.jpg', 487, 3, 0),
-- Book 3
('The Little Prince', 'Antoine de Saint-Exupéry', 1943, '978-0156012195', 'Fiction', 'Vietnamese', 'A philosophical tale about friendship and love.', 'images/little-prince.jpg', 96, 8, 1),
-- Book 4
('The Alchemist', 'Paulo Coelho', 1988, '978-0061122415', 'Fiction', 'English', 'A shepherd embarks on a journey of self-discovery.', 'images/alchemist.jpg', 163, 7, 1),
-- Book 5
('The Kite Runner', 'Khaled Hosseini', 2003, '978-1594631931', 'Historical Fiction', 'French', 'A story of friendship, betrayal, and redemption.', 'images/kite-runner.jpg', 371, 2, 0),
-- Book 6
('Life of Pi', 'Yann Martel', 2001, '978-0151008112', 'Adventure', 'Vietnamese', 'A boy survives 227 days at sea with a Bengal tiger.', 'images/life-pi.jpg', 319, 10, 1),
-- Book 7
('The Book Thief', 'Markus Zusak', 2005, '978-0375842207', 'Historical Fiction', 'English', 'A girl steals books during World War II.', 'images/book-thief.jpg', 552, 4, 1),
-- Book 8
('The Road', 'Cormac McCarthy', 2006, '978-0307387899', 'Post-Apocalyptic', 'French', 'A father and son struggle to survive in a desolate world.', 'images/road.jpg', 287, 6, 0),
-- Book 9
('Sapiens: A Brief History of Humankind', 'Yuval Noah Harari', 2011, '978-0062316097', 'Non-Fiction', 'Vietnamese', 'An exploration of human history and evolution.', 'images/sapiens.jpg', 443, 9, 1),
-- Book 10
('The Hunger Games', 'Suzanne Collins', 2008, '978-0439023481', 'Science Fiction', 'English', 'A dystopian society forces children to fight to the death.', 'images/hunger-games.jpg', 374, 5, 1),
-- Book 11
('The Da Vinci Code', 'Dan Brown', 2003, '978-0385504201', 'Mystery', 'French', 'A symbologist uncovers a centuries-old secret.', 'images/davinci-code.jpg', 454, 3, 0),
-- Book 12
('The Help', 'Kathryn Stockett', 2009, '978-0399155345', 'Historical Fiction', 'Vietnamese', 'The lives of African-American maids in the 1960s South.', 'images/help.jpg', 451, 7, 1),
-- Book 13
('The Girl with the Dragon Tattoo', 'Stieg Larsson', 2005, '978-0307949486', 'Mystery', 'English', 'A journalist and hacker investigate a decades-old disappearance.', 'images/dragon-tattoo.jpg', 465, 6, 1),
-- Book 14
('The Secret Garden', 'Frances Hodgson Burnett', 1911, '978-0064401889', 'Fiction', 'French', 'A young girl discovers a hidden garden.', 'images/secret-garden.jpg', 368, 4, 0),
-- Book 15
('The Chronicles of Narnia', 'C.S. Lewis', 1950, '978-0064471046', 'Fantasy', 'Vietnamese', 'Children enter a magical land through a wardrobe.', 'images/narnia.jpg', 767, 8, 1),
-- Book 16
('The Catcher in the Rye', 'J.D. Salinger', 1951, '978-0316769488', 'Fiction', 'English', 'A teenager struggles with growing up.', 'images/catcher-rye.jpg', 277, 5, 1),
-- Book 17
('The Old Man and the Sea', 'Ernest Hemingway', 1952, '978-0684801223', 'Fiction', 'French', 'An old fisherman battles a giant marlin.', 'images/old-man-sea.jpg', 127, 2, 0),
-- Book 18
('The Hobbit', 'J.R.R. Tolkien', 1937, '978-0547928227', 'Fantasy', 'Vietnamese', 'A hobbit embarks on an epic quest.', 'images/hobbit.jpg', 310, 9, 1),
-- Book 19
('The Adventures of Sherlock Holmes', 'Arthur Conan Doyle', 1892, '978-0486223013', 'Mystery', 'English', 'Detective Sherlock Holmes solves crimes.', 'images/sherlock-holmes.jpg', 307, 6, 1),
-- Book 20
('The Picture of Dorian Gray', 'Oscar Wilde', 1890, '978-0141439563', 'Fiction', 'French', 'A man sells his soul for eternal youth.', 'images/dorian-gray.jpg', 254, 3, 0),
-- Book 21
('The Grapes of Wrath', 'John Steinbeck', 1939, '978-0143039433', 'Historical Fiction', 'Vietnamese', 'A family flees the Dust Bowl during the Great Depression.', 'images/grapes-wrath.jpg', 464, 7, 1),
-- Book 22
('The Fellowship of the Ring', 'J.R.R. Tolkien', 1954, '978-0618346257', 'Fantasy', 'English', 'A fellowship sets out to destroy a powerful ring.', 'images/fellowship-ring.jpg', 423, 8, 1),
-- Book 23
('The Count of Monte Cristo', 'Alexandre Dumas', 1844, '978-0140439308', 'Adventure', 'French', 'A man seeks revenge after being wrongfully imprisoned.', 'images/monte-cristo.jpg', 1276, 4, 0),
-- Book 24
('The Bell Jar', 'Sylvia Plath', 1963, '978-0061148507', 'Fiction', 'Vietnamese', 'A woman struggles with mental illness.', 'images/bell-jar.jpg', 288, 5, 1),
-- Book 25
('The Martian', 'Andy Weir', 2011, '978-0804139021', 'Science Fiction', 'English', 'An astronaut fights to survive on Mars.', 'images/martian.jpg', 369, 6, 1),
-- Book 26
('The Handmaid''s Tale', 'Margaret Atwood', 1985, '978-0385490818', 'Science Fiction', 'French', 'A woman lives in a totalitarian society.', 'images/handmaids-tale.jpg', 311, 3, 0),
-- Book 27
('The Stand', 'Stephen King', 1978, '978-0307947307', 'Post-Apocalyptic', 'Vietnamese', 'A deadly virus wipes out most of humanity.', 'images/stand.jpg', 1153, 10, 1),
-- Book 28
('The Shining', 'Stephen King', 1977, '978-0385121675', 'Horror', 'English', 'A family is haunted by supernatural forces.', 'images/shining.jpg', 447, 4, 1),
-- Book 29
('The Metamorphosis', 'Franz Kafka', 1915, '978-0486299964', 'Fiction', 'French', 'A man wakes up transformed into a giant insect.', 'images/metamorphosis.jpg', 82, 2, 0),
-- Book 30
('The Time Machine', 'H.G. Wells', 1895, '978-0486284726', 'Science Fiction', 'Vietnamese', 'A scientist travels through time to the distant future.', 'images/time-machine.jpg', 118, 7, 1);
