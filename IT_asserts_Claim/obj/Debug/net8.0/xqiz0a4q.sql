IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
CREATE TABLE [Employees] (
    [Id] int NOT NULL IDENTITY,
    [EmpCode] nvarchar(max) NOT NULL,
    [EmpName] nvarchar(max) NOT NULL,
    [EmpMail] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_Employees] PRIMARY KEY ([Id])
);

CREATE TABLE [Accessories] (
    [Id] int NOT NULL IDENTITY,
    [AccessoryType] nvarchar(max) NOT NULL,
    [AccessoryName] nvarchar(max) NOT NULL,
    [SerialNo] nvarchar(max) NOT NULL,
    [IssueDate] datetime2 NOT NULL,
    [EmpId] int NOT NULL,
    CONSTRAINT [PK_Accessories] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Accessories_Employees_EmpId] FOREIGN KEY ([EmpId]) REFERENCES [Employees] ([Id]) ON DELETE CASCADE
);

CREATE INDEX [IX_Accessories_EmpId] ON [Accessories] ([EmpId]);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20251106172411_InitialCreate', N'9.0.10');

DECLARE @var sysname;
SELECT @var = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Employees]') AND [c].[name] = N'EmpCode');
IF @var IS NOT NULL EXEC(N'ALTER TABLE [Employees] DROP CONSTRAINT [' + @var + '];');
ALTER TABLE [Employees] ALTER COLUMN [EmpCode] nvarchar(450) NOT NULL;

CREATE TABLE [AssetCategories] (
    [Id] uniqueidentifier NOT NULL,
    [CategoryName] nvarchar(450) NOT NULL,
    [IsActive] bit NOT NULL,
    [CreatedBy] nvarchar(max) NOT NULL,
    [CreatedAt] datetime2 NOT NULL,
    [ModifiedBy] nvarchar(max) NULL,
    [ModifiedAt] datetime2 NULL,
    [DeletedBy] nvarchar(max) NULL,
    [DeletedAt] datetime2 NULL,
    [IsDeleted] bit NOT NULL,
    CONSTRAINT [PK_AssetCategories] PRIMARY KEY ([Id])
);

CREATE TABLE [AssetBrands] (
    [Id] uniqueidentifier NOT NULL,
    [AssetCategoryId] uniqueidentifier NOT NULL,
    [BrandName] nvarchar(450) NOT NULL,
    [IsActive] bit NOT NULL,
    [CreatedBy] nvarchar(max) NOT NULL,
    [CreatedAt] datetime2 NOT NULL,
    [ModifiedBy] nvarchar(max) NULL,
    [ModifiedAt] datetime2 NULL,
    [DeletedBy] nvarchar(max) NULL,
    [DeletedAt] datetime2 NULL,
    [IsDeleted] bit NOT NULL,
    CONSTRAINT [PK_AssetBrands] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_AssetBrands_AssetCategories_AssetCategoryId] FOREIGN KEY ([AssetCategoryId]) REFERENCES [AssetCategories] ([Id]) ON DELETE NO ACTION
);

CREATE TABLE [AssetModels] (
    [Id] uniqueidentifier NOT NULL,
    [ModelName] nvarchar(450) NOT NULL,
    [IsActive] bit NOT NULL,
    [AssetCategoryId] uniqueidentifier NOT NULL,
    [AssetBrandId] uniqueidentifier NOT NULL,
    [CreatedBy] nvarchar(max) NOT NULL,
    [CreatedAt] datetime2 NOT NULL,
    [ModifiedBy] nvarchar(max) NULL,
    [ModifiedAt] datetime2 NULL,
    [DeletedBy] nvarchar(max) NULL,
    [DeletedAt] datetime2 NULL,
    [IsDeleted] bit NOT NULL,
    CONSTRAINT [PK_AssetModels] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_AssetModels_AssetBrands_AssetBrandId] FOREIGN KEY ([AssetBrandId]) REFERENCES [AssetBrands] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_AssetModels_AssetCategories_AssetCategoryId] FOREIGN KEY ([AssetCategoryId]) REFERENCES [AssetCategories] ([Id]) ON DELETE NO ACTION
);

CREATE UNIQUE INDEX [IX_Employees_EmpCode] ON [Employees] ([EmpCode]);

CREATE UNIQUE INDEX [IX_AssetBrands_AssetCategoryId_BrandName] ON [AssetBrands] ([AssetCategoryId], [BrandName]);

CREATE UNIQUE INDEX [IX_AssetCategories_CategoryName] ON [AssetCategories] ([CategoryName]);

CREATE UNIQUE INDEX [IX_AssetModels_AssetBrandId_ModelName] ON [AssetModels] ([AssetBrandId], [ModelName]);

CREATE INDEX [IX_AssetModels_AssetCategoryId] ON [AssetModels] ([AssetCategoryId]);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260815112642_Category', N'9.0.10');

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260815113727_Category1', N'9.0.10');

CREATE TABLE [Vendors] (
    [Id] uniqueidentifier NOT NULL,
    [VendorName] nvarchar(200) NOT NULL,
    [ContactPerson] nvarchar(100) NOT NULL,
    [Email] nvarchar(150) NOT NULL,
    [Phone] nvarchar(20) NOT NULL,
    [GSTNumber] nvarchar(20) NULL,
    [Address] nvarchar(500) NULL,
    [IsActive] bit NOT NULL,
    [CreatedBy] nvarchar(max) NOT NULL,
    [CreatedAt] datetime2 NOT NULL,
    [ModifiedBy] nvarchar(max) NULL,
    [ModifiedAt] datetime2 NULL,
    [DeletedBy] nvarchar(max) NULL,
    [DeletedAt] datetime2 NULL,
    [IsDeleted] bit NOT NULL,
    CONSTRAINT [PK_Vendors] PRIMARY KEY ([Id])
);

CREATE UNIQUE INDEX [IX_Vendors_Email] ON [Vendors] ([Email]);

CREATE UNIQUE INDEX [IX_Vendors_GSTNumber] ON [Vendors] ([GSTNumber]) WHERE [GSTNumber] IS NOT NULL;

CREATE UNIQUE INDEX [IX_Vendors_VendorName] ON [Vendors] ([VendorName]);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260816164025_AddVendor', N'9.0.10');

CREATE TABLE [Purchases] (
    [Id] uniqueidentifier NOT NULL,
    [PurchaseNumber] nvarchar(50) NOT NULL,
    [VendorId] uniqueidentifier NOT NULL,
    [PurchaseDate] date NOT NULL,
    [InvoiceNumber] nvarchar(100) NOT NULL,
    [InvoiceDate] date NOT NULL,
    [ExpectedDeliveryDate] date NOT NULL,
    [OwnershipType] nvarchar(20) NOT NULL,
    [TotalAmount] decimal(18,2) NOT NULL,
    [Remarks] nvarchar(500) NULL,
    [Status] nvarchar(20) NOT NULL,
    [CreatedBy] nvarchar(max) NOT NULL,
    [CreatedAt] datetime2 NOT NULL,
    [ModifiedBy] nvarchar(max) NULL,
    [ModifiedAt] datetime2 NULL,
    [DeletedBy] nvarchar(max) NULL,
    [DeletedAt] datetime2 NULL,
    [IsDeleted] bit NOT NULL,
    [IsActive] bit NOT NULL,
    CONSTRAINT [PK_Purchases] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Purchases_Vendors_VendorId] FOREIGN KEY ([VendorId]) REFERENCES [Vendors] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [PurchaseAttachments] (
    [Id] uniqueidentifier NOT NULL,
    [FileName] nvarchar(255) NOT NULL,
    [FilePath] nvarchar(500) NOT NULL,
    [ContentType] nvarchar(100) NOT NULL,
    [FileSize] bigint NOT NULL,
    [PurchaseId] uniqueidentifier NOT NULL,
    [CreatedBy] nvarchar(max) NOT NULL,
    [CreatedAt] datetime2 NOT NULL,
    [ModifiedBy] nvarchar(max) NULL,
    [ModifiedAt] datetime2 NULL,
    [DeletedBy] nvarchar(max) NULL,
    [DeletedAt] datetime2 NULL,
    [IsDeleted] bit NOT NULL,
    [IsActive] bit NOT NULL,
    CONSTRAINT [PK_PurchaseAttachments] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_PurchaseAttachments_Purchases_PurchaseId] FOREIGN KEY ([PurchaseId]) REFERENCES [Purchases] ([Id]) ON DELETE NO ACTION
);

CREATE TABLE [PurchasedItems] (
    [Id] uniqueidentifier NOT NULL,
    [Category] nvarchar(100) NOT NULL,
    [Brand] nvarchar(100) NOT NULL,
    [Model] nvarchar(100) NOT NULL,
    [Configuration] nvarchar(300) NOT NULL,
    [Quantity] int NOT NULL,
    [UnitPrice] decimal(18,2) NOT NULL,
    [WarrantyPeriod] nvarchar(50) NOT NULL,
    [SubTotal] decimal(18,2) NOT NULL,
    [PurchaseId] uniqueidentifier NOT NULL,
    [CreatedBy] nvarchar(max) NOT NULL,
    [CreatedAt] datetime2 NOT NULL,
    [ModifiedBy] nvarchar(max) NULL,
    [ModifiedAt] datetime2 NULL,
    [DeletedBy] nvarchar(max) NULL,
    [DeletedAt] datetime2 NULL,
    [IsDeleted] bit NOT NULL,
    [IsActive] bit NOT NULL,
    CONSTRAINT [PK_PurchasedItems] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_PurchasedItems_Purchases_PurchaseId] FOREIGN KEY ([PurchaseId]) REFERENCES [Purchases] ([Id]) ON DELETE NO ACTION
);

CREATE INDEX [IX_PurchaseAttachments_PurchaseId] ON [PurchaseAttachments] ([PurchaseId]);

CREATE INDEX [IX_PurchasedItems_PurchaseId] ON [PurchasedItems] ([PurchaseId]);

CREATE INDEX [IX_Purchases_InvoiceNumber] ON [Purchases] ([InvoiceNumber]);

CREATE UNIQUE INDEX [IX_Purchases_PurchaseNumber] ON [Purchases] ([PurchaseNumber]);

CREATE INDEX [IX_Purchases_Status] ON [Purchases] ([Status]);

CREATE INDEX [IX_Purchases_VendorId] ON [Purchases] ([VendorId]);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260822074241_Purchase', N'9.0.10');

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260822184350_FixAssetCategoryBrandModelRelationship', N'9.0.10');

CREATE TABLE [Assets] (
    [Id] uniqueidentifier NOT NULL,
    [AssetTag] nvarchar(50) NOT NULL,
    [SerialNumber] nvarchar(100) NOT NULL,
    [PurchaseId] uniqueidentifier NOT NULL,
    [Category] nvarchar(100) NOT NULL,
    [Brand] nvarchar(100) NOT NULL,
    [Model] nvarchar(100) NOT NULL,
    [Configuration] nvarchar(250) NULL,
    [Status] int NOT NULL,
    [Condition] int NOT NULL,
    [OwnershipType] int NOT NULL,
    [WarrantyStartDate] date NOT NULL,
    [WarrantyEndDate] date NOT NULL,
    [WarrantyMonths] int NOT NULL,
    [LastServiceDate] date NULL,
    [Remarks] nvarchar(500) NULL,
    [CurrentEmployeeId] uniqueidentifier NULL,
    [AssignedDate] date NULL,
    [CreatedBy] nvarchar(max) NOT NULL,
    [CreatedAt] datetime2 NOT NULL,
    [ModifiedBy] nvarchar(max) NULL,
    [ModifiedAt] datetime2 NULL,
    [DeletedBy] nvarchar(max) NULL,
    [DeletedAt] datetime2 NULL,
    [IsDeleted] bit NOT NULL,
    [IsActive] bit NOT NULL,
    CONSTRAINT [PK_Assets] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Assets_Purchases_PurchaseId] FOREIGN KEY ([PurchaseId]) REFERENCES [Purchases] ([Id]) ON DELETE NO ACTION
);

CREATE TABLE [AssetLifecycleHistories] (
    [Id] uniqueidentifier NOT NULL,
    [AssetId] uniqueidentifier NOT NULL,
    [Action] nvarchar(100) NOT NULL,
    [OldStatus] int NULL,
    [NewStatus] int NOT NULL,
    [Remarks] nvarchar(500) NULL,
    [PerformedBy] nvarchar(200) NOT NULL,
    [PerformedDate] datetime2 NOT NULL,
    [EmployeeId] uniqueidentifier NULL,
    [ReferenceId] uniqueidentifier NULL,
    [ReferenceType] nvarchar(50) NULL,
    [CreatedBy] nvarchar(max) NOT NULL,
    [CreatedAt] datetime2 NOT NULL,
    [ModifiedBy] nvarchar(max) NULL,
    [ModifiedAt] datetime2 NULL,
    [DeletedBy] nvarchar(max) NULL,
    [DeletedAt] datetime2 NULL,
    [IsDeleted] bit NOT NULL,
    [IsActive] bit NOT NULL,
    CONSTRAINT [PK_AssetLifecycleHistories] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_AssetLifecycleHistories_Assets_AssetId] FOREIGN KEY ([AssetId]) REFERENCES [Assets] ([Id]) ON DELETE NO ACTION
);

CREATE INDEX [IX_AssetLifecycleHistories_AssetId] ON [AssetLifecycleHistories] ([AssetId]);

CREATE INDEX [IX_AssetLifecycleHistories_AssetId_PerformedDate] ON [AssetLifecycleHistories] ([AssetId], [PerformedDate]);

CREATE UNIQUE INDEX [IX_Assets_AssetTag] ON [Assets] ([AssetTag]);

CREATE INDEX [IX_Assets_PurchaseId] ON [Assets] ([PurchaseId]);

CREATE UNIQUE INDEX [IX_Assets_SerialNumber] ON [Assets] ([SerialNumber]);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260825182747_Asset', N'9.0.10');

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260828160717_Asset', N'9.0.10');

DECLARE @var1 sysname;
SELECT @var1 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[PurchasedItems]') AND [c].[name] = N'WarrantyPeriod');
IF @var1 IS NOT NULL EXEC(N'ALTER TABLE [PurchasedItems] DROP CONSTRAINT [' + @var1 + '];');
ALTER TABLE [PurchasedItems] DROP COLUMN [WarrantyPeriod];

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260829045857_AssetOwershipremove', N'9.0.10');

COMMIT;
GO

