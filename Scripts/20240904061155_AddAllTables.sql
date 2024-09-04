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
GO

CREATE TABLE [Category] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(50) NOT NULL,
    CONSTRAINT [PK_Category] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [User] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(20) NOT NULL,
    [Phone] nvarchar(max) NULL,
    [Email] nvarchar(max) NULL,
    [Password] nvarchar(max) NULL,
    [IsAdmin] bit NOT NULL DEFAULT (('0')),
    CONSTRAINT [PK_User] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [SubCategory] (
    [Id] int NOT NULL IDENTITY,
    [CategoryId] int NOT NULL,
    [Name] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_SubCategory] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_SubCategory_Category] FOREIGN KEY ([CategoryId]) REFERENCES [Category] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [UserCategory] (
    [Id] int NOT NULL IDENTITY,
    [CategoryId] int NOT NULL,
    [UserId] int NOT NULL,
    CONSTRAINT [PK_UserCategory] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_UserCategory_Category] FOREIGN KEY ([CategoryId]) REFERENCES [Category] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_UserCategory_User] FOREIGN KEY ([UserId]) REFERENCES [User] ([Id]) ON DELETE CASCADE
);
GO

CREATE INDEX [IX_SubCategory_CategoryId] ON [SubCategory] ([CategoryId]);
GO

CREATE INDEX [IX_UserCategory_CategoryId] ON [UserCategory] ([CategoryId]);
GO

CREATE INDEX [IX_UserCategory_UserId] ON [UserCategory] ([UserId]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20240904061155_AddAllTables', N'6.0.33');
GO

COMMIT;
GO