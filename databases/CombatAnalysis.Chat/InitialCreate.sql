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
CREATE TABLE [GroupChat] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(max) NOT NULL,
    [AppUserId] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_GroupChat] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [GroupChatMessage] (
    [Id] int NOT NULL IDENTITY,
    [Username] nvarchar(max) NOT NULL,
    [Message] nvarchar(max) NOT NULL,
    [Time] datetimeoffset NOT NULL,
    [Status] int NOT NULL,
    [Type] int NOT NULL,
    [ChatId] int NOT NULL,
    [GroupChatUserId] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_GroupChatMessage] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [GroupChatMessageCount] (
    [Id] int NOT NULL IDENTITY,
    [Count] int NOT NULL,
    [ChatId] int NOT NULL,
    [GroupChatUserId] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_GroupChatMessageCount] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [GroupChatRules] (
    [Id] int NOT NULL IDENTITY,
    [InvitePeople] int NOT NULL,
    [RemovePeople] int NOT NULL,
    [PinMessage] int NOT NULL,
    [Announcements] int NOT NULL,
    [ChatId] int NOT NULL,
    CONSTRAINT [PK_GroupChatRules] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [GroupChatUser] (
    [Id] nvarchar(450) NOT NULL,
    [Username] nvarchar(max) NOT NULL,
    [ChatId] int NOT NULL,
    [AppUserId] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_GroupChatUser] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [PersonalChat] (
    [Id] int NOT NULL IDENTITY,
    [InitiatorId] nvarchar(max) NOT NULL,
    [CompanionId] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_PersonalChat] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [PersonalChatMessage] (
    [Id] int NOT NULL IDENTITY,
    [Username] nvarchar(max) NOT NULL,
    [Message] nvarchar(max) NOT NULL,
    [Time] datetimeoffset NOT NULL,
    [Status] int NOT NULL,
    [Type] int NOT NULL,
    [ChatId] int NOT NULL,
    [AppUserId] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_PersonalChatMessage] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [PersonalChatMessageCount] (
    [Id] int NOT NULL IDENTITY,
    [Count] int NOT NULL,
    [ChatId] int NOT NULL,
    [AppUserId] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_PersonalChatMessageCount] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [UnreadGroupChatMessage] (
    [Id] int NOT NULL IDENTITY,
    [GroupChatUserId] nvarchar(max) NOT NULL,
    [GroupChatMessageId] int NOT NULL,
    CONSTRAINT [PK_UnreadGroupChatMessage] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [VoiceChat] (
    [Id] nvarchar(450) NOT NULL,
    [AppUserId] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_VoiceChat] PRIMARY KEY ([Id])
);
GO

CREATE PROCEDURE GetPersonalChatMessageByChatIdPagination (@chatId INT, @pageSize INT)
AS
BEGIN
	SELECT TOP (@pageSize) * 
	FROM PersonalChatMessage
	WHERE ChatId = @chatId
	ORDER BY Id DESC
END
GO

CREATE PROCEDURE GetPersonalChatMessageByChatIdMore (@chatId INT, @offset INT, @pageSize INT)
AS
BEGIN
	SELECT * 
	FROM PersonalChatMessage
	WHERE ChatId = @chatId
	ORDER BY Id DESC
	OFFSET @offset ROWS
	FETCH NEXT @pageSize ROWS ONLY
END
GO

CREATE PROCEDURE GetGroupChatMessageByChatIdPagination (@chatId INT, @pageSize INT)
AS
BEGIN
	SELECT TOP (@pageSize) * 
	FROM GroupChatMessage
	WHERE ChatId = @chatId
	ORDER BY Id DESC
END
GO

CREATE PROCEDURE GetGroupChatMessageByChatIdMore (@chatId INT, @offset INT, @pageSize INT)
AS
BEGIN
	SELECT * 
	FROM GroupChatMessage
	WHERE ChatId = @chatId
	ORDER BY Id DESC
	OFFSET @offset ROWS
	FETCH NEXT @pageSize ROWS ONLY
END
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250122135024_InitialCreate', N'9.0.1');

COMMIT;
GO

