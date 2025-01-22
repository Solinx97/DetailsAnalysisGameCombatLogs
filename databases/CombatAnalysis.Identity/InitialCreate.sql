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
CREATE TABLE [AuthorizationCodeChallenge] (
    [Id] nvarchar(450) NOT NULL,
    [CodeChallenge] nvarchar(max) NOT NULL,
    [CodeChallengeMethod] nvarchar(max) NOT NULL,
    [RedirectUrl] nvarchar(max) NOT NULL,
    [CreatedAt] datetimeoffset NOT NULL,
    [ExpiryTime] datetimeoffset NOT NULL,
    [ClientId] nvarchar(max) NOT NULL,
    [IsUsed] bit NOT NULL,
    CONSTRAINT [PK_AuthorizationCodeChallenge] PRIMARY KEY ([Id])
);

CREATE TABLE [Client] (
    [Id] nvarchar(450) NOT NULL,
    [RedirectUrl] nvarchar(max) NOT NULL,
    [Scope] nvarchar(max) NOT NULL,
    [ClientName] nvarchar(max) NOT NULL,
    [ClientType] nvarchar(max) NOT NULL,
    [CreatedAt] datetimeoffset NOT NULL,
    [UpdatedAt] datetimeoffset NOT NULL,
    CONSTRAINT [PK_Client] PRIMARY KEY ([Id])
);

CREATE TABLE [IdentityUser] (
    [Id] nvarchar(450) NOT NULL,
    [Email] nvarchar(max) NOT NULL,
    [PasswordHash] nvarchar(max) NOT NULL,
    [Salt] nvarchar(max) NOT NULL,
    [EmailVerified] bit NOT NULL,
    CONSTRAINT [PK_IdentityUser] PRIMARY KEY ([Id])
);

CREATE TABLE [RefreshToken] (
    [Id] nvarchar(450) NOT NULL,
    [Token] nvarchar(max) NOT NULL,
    [ExpiryTime] datetimeoffset NOT NULL,
    [ClientId] nvarchar(max) NOT NULL,
    [UserId] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_RefreshToken] PRIMARY KEY ([Id])
);

CREATE TABLE [ResetToken] (
    [Id] int NOT NULL IDENTITY,
    [Email] nvarchar(max) NOT NULL,
    [Token] nvarchar(max) NOT NULL,
    [ExpirationTime] datetime2 NOT NULL,
    [IsUsed] bit NOT NULL,
    CONSTRAINT [PK_ResetToken] PRIMARY KEY ([Id])
);

CREATE TABLE [VerifyEmailToken] (
    [Id] int NOT NULL IDENTITY,
    [Email] nvarchar(max) NOT NULL,
    [Token] nvarchar(max) NOT NULL,
    [ExpirationTime] datetime2 NOT NULL,
    [IsUsed] bit NOT NULL,
    CONSTRAINT [PK_VerifyEmailToken] PRIMARY KEY ([Id])
);

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'ClientName', N'ClientType', N'CreatedAt', N'RedirectUrl', N'Scope', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[Client]'))
    SET IDENTITY_INSERT [Client] ON;
INSERT INTO [Client] ([Id], [ClientName], [ClientType], [CreatedAt], [RedirectUrl], [Scope], [UpdatedAt])
VALUES (N'client1', N'web', N'public', '2025-01-22T10:56:33.3026555+01:00', N'encounters.analysis.com/callback', N'client1scope', '2025-01-22T10:56:33.3043452+01:00'),
(N'client2', N'desktop', N'public', '2025-01-22T10:56:33.3043652+01:00', N'localhost:45571/callback', N'client2scope', '2025-01-22T10:56:33.3043657+01:00'),
(N'client3', N'devWeb', N'public', '2025-01-22T10:56:33.3043659+01:00', N'localhost:44479/callback', N'client3scope', '2025-01-22T10:56:33.3043661+01:00');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'ClientName', N'ClientType', N'CreatedAt', N'RedirectUrl', N'Scope', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[Client]'))
    SET IDENTITY_INSERT [Client] OFF;

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250122095633_InitialCreate', N'9.0.1');

COMMIT;
GO

