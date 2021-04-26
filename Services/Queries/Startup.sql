
/*** new concessions table -- Shelby **/
USE [BITS_DB]

CREATE TABLE [dbo].[ConcessionsTable] (
    [Id]          INT          IDENTITY (1, 1) NOT NULL,
    [Item_Name]   VARCHAR (50) NOT NULL,
    [Item_Amount] INT          NULL,
    [Item_Loc]    VARCHAR (50) NOT NULL, 
    [Item_Price] FLOAT NULL
);




SET IDENTITY_INSERT [dbo].[ConcessionsTable] ON
INSERT INTO [dbo].[ConcessionsTable] ([Id], [Item_Name], [Item_Amount], [Item_Loc], [Item_Price]) VALUES (3, N'Popcorn', 3, N'Back Dry Storage', 1)
INSERT INTO [dbo].[ConcessionsTable] ([Id], [Item_Name], [Item_Amount], [Item_Loc], [Item_Price]) VALUES (1006, N'Soda', 19, N'Back Cold Storage', 2.5)
INSERT INTO [dbo].[ConcessionsTable] ([Id], [Item_Name], [Item_Amount], [Item_Loc], [Item_Price]) VALUES (1008, N'Pretzels', 9, N'Front Dry Storage', 3.5)
INSERT INTO [dbo].[ConcessionsTable] ([Id], [Item_Name], [Item_Amount], [Item_Loc], [Item_Price]) VALUES (1009, N'Pretzels', 6, N'Back Dry Storage', 3.5)
INSERT INTO [dbo].[ConcessionsTable] ([Id], [Item_Name], [Item_Amount], [Item_Loc], [Item_Price]) VALUES (1010, N'Soda', 22, N'Front Cold Storage', 2.5)
SET IDENTITY_INSERT [dbo].[ConcessionsTable] OFF












CREATE TABLE [dbo].[Equipment] (
    [Id]       INT          IDENTITY (1, 1) NOT NULL,
    [Name]     VARCHAR (50) NULL,
    [Location] VARCHAR (50) NULL,
    [Amount]   INT          NULL
);



/******  funds table ********/
CREATE TABLE [dbo].[Funds]
(
  [Id] INT NOT NULL PRIMARY KEY, 
    [Concessions] FLOAT NOT NULL, 
    [Equipment] FLOAT NOT NULL, 
    [Space] FLOAT NOT NULL, 
    [BITS] FLOAT NOT NULL
)
INSERT INTO [dbo].[Funds] ([Id], [Concessions], [Equipment], [Space], [BITS]) VALUES (1, 700, 900, 1500, 10000)



/********** schedule table ********/
CREATE TABLE [dbo].[Schedule] (
    [Id]        INT          IDENTITY (1, 1) NOT NULL,
    [Monday]    VARCHAR (50) NULL,
    [Tuesday]   VARCHAR (50) NULL,
    [Wednesday] VARCHAR (50) NULL,
    [Thursday]  VARCHAR (50) NULL,
    [Friday]    VARCHAR (50) NULL,
    [Saturday]  VARCHAR (50) NULL,
    [Dates]     VARCHAR (50) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);




/*********** schedule data *********/
SET IDENTITY_INSERT [dbo].[Schedule] ON
INSERT INTO [dbo].[Schedule] ([Id], [Monday], [Tuesday], [Wednesday], [Thursday], [Friday], [Saturday], [Dates]) VALUES (1, N'Shelby', N'John', N'Nate', N'Yinuo', N'Alex', N'John&Alex', N'4/19-4/24')
INSERT INTO [dbo].[Schedule] ([Id], [Monday], [Tuesday], [Wednesday], [Thursday], [Friday], [Saturday], [Dates]) VALUES (2, N'John', N'Shelby&Nate', N'Yinuo&Alex', N'Yinuo', N'Nate', N'Alex', N'4/26-4/30')
SET IDENTITY_INSERT [dbo].[Schedule] OFF


/***********rent table***********/
CREATE TABLE [dbo].[RentTable] (
    [Id]         INT          IDENTITY (1, 1) NOT NULL,
    [Name]       VARCHAR (50) NULL,
    [itemrented] VARCHAR (50) NULL,
    [date]       VARCHAR (50) NULL,
    [hours]      INT          NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);


