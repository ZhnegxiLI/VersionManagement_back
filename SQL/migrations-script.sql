IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20211120002644_init')
BEGIN
    CREATE TABLE [Environment] (
        [Id] bigint NOT NULL IDENTITY,
        [CreatedBy] bigint NULL,
        [CreatedOn] datetime2 NULL,
        [UpdatedBy] bigint NULL,
        [UpdatedOn] datetime2 NULL,
        [Name] nvarchar(max) NULL,
        CONSTRAINT [PK_Environment] PRIMARY KEY ([Id])
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20211120002644_init')
BEGIN
    CREATE TABLE [Projet] (
        [Id] bigint NOT NULL IDENTITY,
        [CreatedBy] bigint NULL,
        [CreatedOn] datetime2 NULL,
        [UpdatedBy] bigint NULL,
        [UpdatedOn] datetime2 NULL,
        [Name] nvarchar(max) NULL,
        [Description] nvarchar(max) NULL,
        [ParentId] bigint NULL,
        CONSTRAINT [PK_Projet] PRIMARY KEY ([Id])
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20211120002644_init')
BEGIN
    CREATE TABLE [ProjetEnvironment] (
        [Id] bigint NOT NULL IDENTITY,
        [CreatedBy] bigint NULL,
        [CreatedOn] datetime2 NULL,
        [UpdatedBy] bigint NULL,
        [UpdatedOn] datetime2 NULL,
        [ProjetId] bigint NOT NULL,
        [EnvironmentId] bigint NOT NULL,
        [Description] nvarchar(max) NULL,
        CONSTRAINT [PK_ProjetEnvironment] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_ProjetEnvironment_Environment_EnvironmentId] FOREIGN KEY ([EnvironmentId]) REFERENCES [Environment] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_ProjetEnvironment_Projet_ProjetId] FOREIGN KEY ([ProjetId]) REFERENCES [Projet] ([Id]) ON DELETE CASCADE
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20211120002644_init')
BEGIN
    CREATE TABLE [Version] (
        [Id] bigint NOT NULL IDENTITY,
        [CreatedBy] bigint NULL,
        [CreatedOn] datetime2 NULL,
        [UpdatedBy] bigint NULL,
        [UpdatedOn] datetime2 NULL,
        [VersionNumber] nvarchar(max) NULL,
        [ProjetId] bigint NOT NULL,
        CONSTRAINT [PK_Version] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Version_Projet_ProjetId] FOREIGN KEY ([ProjetId]) REFERENCES [Projet] ([Id]) ON DELETE CASCADE
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20211120002644_init')
BEGIN
    CREATE TABLE [DeploimentHistory] (
        [Id] bigint NOT NULL IDENTITY,
        [CreatedBy] bigint NULL,
        [CreatedOn] datetime2 NULL,
        [UpdatedBy] bigint NULL,
        [UpdatedOn] datetime2 NULL,
        [VersionId] bigint NOT NULL,
        [ProjetEnvironmentId] bigint NOT NULL,
        CONSTRAINT [PK_DeploimentHistory] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_DeploimentHistory_ProjetEnvironment_ProjetEnvironmentId] FOREIGN KEY ([ProjetEnvironmentId]) REFERENCES [ProjetEnvironment] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_DeploimentHistory_Version_VersionId] FOREIGN KEY ([VersionId]) REFERENCES [Version] ([Id]) 
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20211120002644_init')
BEGIN
    CREATE INDEX [IX_DeploimentHistory_ProjetEnvironmentId] ON [DeploimentHistory] ([ProjetEnvironmentId]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20211120002644_init')
BEGIN
    CREATE INDEX [IX_DeploimentHistory_VersionId] ON [DeploimentHistory] ([VersionId]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20211120002644_init')
BEGIN
    CREATE INDEX [IX_ProjetEnvironment_EnvironmentId] ON [ProjetEnvironment] ([EnvironmentId]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20211120002644_init')
BEGIN
    CREATE INDEX [IX_ProjetEnvironment_ProjetId] ON [ProjetEnvironment] ([ProjetId]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20211120002644_init')
BEGIN
    CREATE INDEX [IX_Version_ProjetId] ON [Version] ([ProjetId]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20211120002644_init')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20211120002644_init', N'3.1.21');
END;

GO


IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20211208111950_addOrderInEnv')
BEGIN
    ALTER TABLE [Environment] ADD [Order] int NULL;
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20211208111950_addOrderInEnv')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20211208111950_addOrderInEnv', N'3.1.21');
END;

GO

