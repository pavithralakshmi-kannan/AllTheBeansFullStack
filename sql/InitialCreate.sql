-- =========================================
-- SQL Server schema for AllTheBeans
-- Generated from EF Core migration InitialCreate
-- =========================================

-- Drop tables if they exist (for reset)
IF OBJECT_ID('dbo.DailySelections', 'U') IS NOT NULL DROP TABLE dbo.DailySelections;
IF OBJECT_ID('dbo.Orders', 'U') IS NOT NULL DROP TABLE dbo.Orders;
IF OBJECT_ID('dbo.Beans', 'U') IS NOT NULL DROP TABLE dbo.Beans;

-- =========================
-- Table: Beans
-- =========================
CREATE TABLE [dbo].[Beans] (
    [Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [Name] NVARCHAR(120) NOT NULL,
    [Description] NVARCHAR(MAX) NULL,
    [Country] NVARCHAR(80) NULL,
    [Colour] NVARCHAR(80) NULL,
    [Image] NVARCHAR(MAX) NULL,
    [Cost] DECIMAL(18,2) NOT NULL
);

-- Indexes for Beans
CREATE INDEX [IX_Beans_Name] ON [dbo].[Beans] ([Name]);
CREATE INDEX [IX_Beans_Country] ON [dbo].[Beans] ([Country]);
CREATE INDEX [IX_Beans_Cost] ON [dbo].[Beans] ([Cost]);
CREATE INDEX [IX_Beans_Country_Name] ON [dbo].[Beans] ([Country], [Name]);

-- =========================
-- Table: Orders
-- =========================
CREATE TABLE [dbo].[Orders] (
    [Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [CustomerName] NVARCHAR(MAX) NOT NULL,
    [Address] NVARCHAR(MAX) NOT NULL,
    [BeanId] INT NOT NULL,
    [Quantity] INT NOT NULL,
    [OrderDate] DATETIME2 NOT NULL,
    CONSTRAINT [FK_Orders_Beans_BeanId] FOREIGN KEY ([BeanId]) REFERENCES [dbo].[Beans] ([Id]) ON DELETE CASCADE
);

-- Indexes for Orders
CREATE INDEX [IX_Orders_OrderDate] ON [dbo].[Orders] ([OrderDate]);
CREATE INDEX [IX_Orders_BeanId] ON [dbo].[Orders] ([BeanId]);

-- =========================
-- Table: DailySelections
-- =========================
CREATE TABLE [dbo].[DailySelections] (
    [Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [Date] DATETIME2 NOT NULL,
    [BeanId] INT NOT NULL,
    CONSTRAINT [FK_DailySelections_Beans_BeanId] FOREIGN KEY ([BeanId]) REFERENCES [dbo].[Beans] ([Id]) ON DELETE CASCADE
);

-- Indexes for DailySelections
CREATE UNIQUE INDEX [IX_DailySelections_Date] ON [dbo].[DailySelections] ([Date]);
CREATE INDEX [IX_DailySelections_BeanId] ON [dbo].[DailySelections] ([BeanId]);

-- =========================================
-- End of schema
-- =========================================
