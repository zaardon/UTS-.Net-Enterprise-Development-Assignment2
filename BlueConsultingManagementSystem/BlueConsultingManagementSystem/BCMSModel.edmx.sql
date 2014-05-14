
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 05/14/2014 22:03:58
-- Generated from EDMX file: c:\users\alex\documents\visual studio 2013\Projects\BlueConsultingManagementSystem\BlueConsultingManagementSystem\BCMSModel.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [BCMSDB];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------


-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------


-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Expenses'
CREATE TABLE [dbo].[Expenses] (
    [ExpensePK] int IDENTITY(1,1) NOT NULL,
    [Description] nvarchar(max)  NOT NULL,
    [Amount] nvarchar(max)  NOT NULL,
    [Currency] nvarchar(max)  NOT NULL,
    [Location] nvarchar(max)  NOT NULL,
    [Date] nvarchar(max)  NOT NULL,
    [PDF] nvarchar(max)  NOT NULL,
    [Report_ReportPK] int  NOT NULL
);
GO

-- Creating table 'Reports'
CREATE TABLE [dbo].[Reports] (
    [ReportPK] int IDENTITY(1,1) NOT NULL,
    [ReportName] nvarchar(max)  NOT NULL,
    [ConsultantName] nvarchar(max)  NOT NULL,
    [SuppervisorApproval] nvarchar(max)  NOT NULL,
    [StaffApproval] nvarchar(max)  NOT NULL,
    [DateOfApproval] nvarchar(max)  NOT NULL,
    [Department] nvarchar(max)  NOT NULL,
    [SupervisorName] nvarchar(max)  NOT NULL
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