
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, and Azure
-- --------------------------------------------------
-- Date Created: 05/11/2014 15:34:33
-- Generated from EDMX file: M:\BlueConsultingManagementSystem\BlueConsultingManagementSystem\BCMSModel.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_ReportExpense]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Expenses] DROP CONSTRAINT [FK_ReportExpense];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Expenses]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Expenses];
GO
IF OBJECT_ID(N'[dbo].[Reports]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Reports];
GO
IF OBJECT_ID(N'[dbo].[Budgets]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Budgets];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Expenses'
CREATE TABLE [dbo].[Expenses] (
    [ExpensePK] int IDENTITY(1,1) NOT NULL,
    [Description] nvarchar(max)  NULL,
    [Amount] float  NULL,
    [Currency] nvarchar(max)  NULL,
    [Location] nvarchar(max)  NULL,
    [Date] datetime  NULL,
    [PDF] tinyint  NULL,
    [Report_ReportPK] int  NOT NULL
);
GO

-- Creating table 'Reports'
CREATE TABLE [dbo].[Reports] (
    [ReportPK] int IDENTITY(1,1) NOT NULL,
    [ReportName] nvarchar(max)  NULL,
    [SuppervisorApproval] nvarchar(max)  NULL,
    [StaffApproval] nvarchar(max)  NULL,
    [DateOfApproval] datetime  NULL,
    [Department] nvarchar(max)  NULL,
    [SupervisorName] nvarchar(max)  NULL
);
GO

-- Creating table 'Budgets'
CREATE TABLE [dbo].[Budgets] (
    [BudgetPK] int IDENTITY(1,1) NOT NULL,
    [DepartmentName] nvarchar(max)  NOT NULL,
    [DepartmentTotal] float  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [ExpensePK] in table 'Expenses'
ALTER TABLE [dbo].[Expenses]
ADD CONSTRAINT [PK_Expenses]
    PRIMARY KEY CLUSTERED ([ExpensePK] ASC);
GO

-- Creating primary key on [ReportPK] in table 'Reports'
ALTER TABLE [dbo].[Reports]
ADD CONSTRAINT [PK_Reports]
    PRIMARY KEY CLUSTERED ([ReportPK] ASC);
GO

-- Creating primary key on [BudgetPK] in table 'Budgets'
ALTER TABLE [dbo].[Budgets]
ADD CONSTRAINT [PK_Budgets]
    PRIMARY KEY CLUSTERED ([BudgetPK] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [Report_ReportPK] in table 'Expenses'
ALTER TABLE [dbo].[Expenses]
ADD CONSTRAINT [FK_ReportExpense]
    FOREIGN KEY ([Report_ReportPK])
    REFERENCES [dbo].[Reports]
        ([ReportPK])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_ReportExpense'
CREATE INDEX [IX_FK_ReportExpense]
ON [dbo].[Expenses]
    ([Report_ReportPK]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------