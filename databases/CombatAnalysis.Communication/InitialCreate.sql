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
CREATE TABLE [Community] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(max) NOT NULL,
    [Description] nvarchar(max) NOT NULL,
    [PolicyType] int NOT NULL,
    [AppUserId] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_Community] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [CommunityDiscussion] (
    [Id] int NOT NULL IDENTITY,
    [Title] nvarchar(max) NOT NULL,
    [Content] nvarchar(max) NOT NULL,
    [When] datetimeoffset NOT NULL,
    [AppUserId] nvarchar(max) NOT NULL,
    [CommunityId] int NOT NULL,
    CONSTRAINT [PK_CommunityDiscussion] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [CommunityDiscussionComment] (
    [Id] int NOT NULL IDENTITY,
    [Content] nvarchar(max) NOT NULL,
    [When] datetimeoffset NOT NULL,
    [AppUserId] nvarchar(max) NOT NULL,
    [CommunityDiscussionId] int NOT NULL,
    CONSTRAINT [PK_CommunityDiscussionComment] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [CommunityPost] (
    [Id] int NOT NULL IDENTITY,
    [CommunityName] nvarchar(max) NOT NULL,
    [Owner] nvarchar(max) NOT NULL,
    [Content] nvarchar(max) NOT NULL,
    [PostType] int NOT NULL,
    [PublicType] int NOT NULL,
    [Restrictions] int NOT NULL,
    [Tags] nvarchar(max) NOT NULL,
    [CreatedAt] datetimeoffset NOT NULL,
    [LikeCount] int NOT NULL,
    [DislikeCount] int NOT NULL,
    [CommentCount] int NOT NULL,
    [CommunityId] int NOT NULL,
    [AppUserId] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_CommunityPost] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [CommunityPostComment] (
    [Id] int NOT NULL IDENTITY,
    [Content] nvarchar(max) NOT NULL,
    [CommentType] int NOT NULL,
    [CreatedAt] datetimeoffset NOT NULL,
    [CommunityPostId] int NOT NULL,
    [CommunityId] int NOT NULL,
    [AppUserId] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_CommunityPostComment] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [CommunityPostDislike] (
    [Id] int NOT NULL IDENTITY,
    [CreatedAt] datetimeoffset NOT NULL,
    [CommunityPostId] int NOT NULL,
    [CommunityId] int NOT NULL,
    [AppUserId] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_CommunityPostDislike] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [CommunityPostLike] (
    [Id] int NOT NULL IDENTITY,
    [CreatedAt] datetimeoffset NOT NULL,
    [CommunityPostId] int NOT NULL,
    [CommunityId] int NOT NULL,
    [AppUserId] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_CommunityPostLike] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [CommunityUser] (
    [Id] nvarchar(450) NOT NULL,
    [Username] nvarchar(max) NOT NULL,
    [AppUserId] nvarchar(max) NOT NULL,
    [CommunityId] int NOT NULL,
    CONSTRAINT [PK_CommunityUser] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [InviteToCommunity] (
    [Id] int NOT NULL IDENTITY,
    [CommunityId] int NOT NULL,
    [ToAppUserId] nvarchar(max) NOT NULL,
    [When] datetimeoffset NOT NULL,
    [AppUserId] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_InviteToCommunity] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [UserPost] (
    [Id] int NOT NULL IDENTITY,
    [Owner] nvarchar(max) NOT NULL,
    [Content] nvarchar(max) NOT NULL,
    [PublicType] int NOT NULL,
    [Tags] nvarchar(max) NOT NULL,
    [CreatedAt] datetimeoffset NOT NULL,
    [LikeCount] int NOT NULL,
    [DislikeCount] int NOT NULL,
    [CommentCount] int NOT NULL,
    [AppUserId] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_UserPost] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [UserPostComment] (
    [Id] int NOT NULL IDENTITY,
    [Content] nvarchar(max) NOT NULL,
    [CreatedAt] datetimeoffset NOT NULL,
    [UserPostId] int NOT NULL,
    [AppUserId] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_UserPostComment] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [UserPostDislike] (
    [Id] int NOT NULL IDENTITY,
    [UserPostId] int NOT NULL,
    [AppUserId] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_UserPostDislike] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [UserPostLike] (
    [Id] int NOT NULL IDENTITY,
    [UserPostId] int NOT NULL,
    [AppUserId] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_UserPostLike] PRIMARY KEY ([Id])
);
GO

CREATE PROCEDURE GetCommunityPostByCommunityIdPagination (@communityId INT, @pageSize INT)
AS
BEGIN
	SELECT TOP (@pageSize) * 
	FROM CommunityPost
	WHERE CommunityId = @communityId
	ORDER BY Id DESC
END
GO

CREATE PROCEDURE GetMoreCommunityPostByCommunityId (@communityId INT, @offset INT, @pageSize INT)
AS
BEGIN
	SELECT * 
	FROM CommunityPost
	WHERE CommunityId = @communityId
	ORDER BY Id DESC
	OFFSET @offset ROWS
	FETCH NEXT @pageSize ROWS ONLY
END
GO

CREATE PROCEDURE GetNewCommunityPostByCommunityId (@communityId INT, @checkFrom DATETIME)
AS
BEGIN
	SELECT * 
	FROM CommunityPost
	WHERE CommunityId = @communityId AND CreatedAt > @checkFrom
	ORDER BY CreatedAt DESC
END
GO

CREATE PROCEDURE GetCommunityPostByListOfCommunityIdPagination (@communityIds NVARCHAR(MAX), @pageSize INT)
AS
BEGIN
	DECLARE @communityIdTable TABLE (CommunityId INT);
	INSERT INTO @communityIdTable (CommunityId)
	SELECT value FROM STRING_SPLIT(@communityIds, ',');
	SELECT TOP (@pageSize) * 
	FROM CommunityPost
	WHERE CommunityId IN (SELECT CommunityId FROM @communityIdTable)
	ORDER BY Id DESC
END
GO

CREATE PROCEDURE GetMoreCommunityPostByListOfCommunityId (@communityIds NVARCHAR(MAX), @offset INT, @pageSize INT)
AS
BEGIN
	DECLARE @communityIdTable TABLE (CommunityId INT);
	INSERT INTO @communityIdTable (CommunityId)
	SELECT value FROM STRING_SPLIT(@communityIds, ',');
	SELECT * 
	FROM CommunityPost
	WHERE CommunityId IN (SELECT CommunityId FROM @communityIdTable)
	ORDER BY Id DESC
	OFFSET @offset ROWS
	FETCH NEXT @pageSize ROWS ONLY
END
GO

CREATE PROCEDURE GetNewCommunityPostByListOfCommunityId (@communityIds NVARCHAR(MAX), @checkFrom DATETIME)
AS
BEGIN
	DECLARE @communityIdTable TABLE (CommunityId INT);
	INSERT INTO @communityIdTable (CommunityId)
	SELECT value FROM STRING_SPLIT(@communityIds, ',');
	SELECT * 
	FROM CommunityPost
	WHERE CommunityId IN (SELECT CommunityId FROM @communityIdTable) AND CreatedAt > @checkFrom
	ORDER BY CreatedAt DESC
END
GO

CREATE PROCEDURE GetUserPostByAppUserIdPagination (@appUserId NVARCHAR (MAX), @pageSize INT)
AS
BEGIN
	SELECT TOP (@pageSize) * 
	FROM UserPost
	WHERE AppUserId = @appUserId
	ORDER BY Id DESC
END
GO

CREATE PROCEDURE GetMoreUserPostByAppUserId (@appUserId NVARCHAR (MAX), @offset INT, @pageSize INT)
AS
BEGIN
	SELECT * 
	FROM UserPost
	WHERE AppUserId = @appUserId
	ORDER BY Id DESC
	OFFSET @offset ROWS
	FETCH NEXT @pageSize ROWS ONLY
END
GO

CREATE PROCEDURE GetNewUserPostByAppUserId (@appUserId NVARCHAR (MAX), @checkFrom DATETIME)
AS
BEGIN
	SELECT * 
	FROM UserPost
	WHERE AppUserId = @appUserId AND CreatedAt > @checkFrom
	ORDER BY CreatedAt DESC
END
GO

CREATE PROCEDURE GetUserPostByListOfAppUserIdPagination (@appUserIds NVARCHAR (MAX), @pageSize INT)
AS
BEGIN
	DECLARE @appUserIdTable TABLE (AppUserId NVARCHAR (MAX));
	INSERT INTO @appUserIdTable (AppUserId)
	SELECT value FROM STRING_SPLIT(@appUserIds, ',');
	SELECT TOP (@pageSize) * 
	FROM UserPost
	WHERE AppUserId IN (SELECT AppUserId FROM @appUserIdTable)
	ORDER BY Id DESC
END
GO

CREATE PROCEDURE GetMoreUserPostByListOfAppUserId (@appUserIds NVARCHAR (MAX), @offset INT, @pageSize INT)
AS
BEGIN
	DECLARE @appUserIdTable TABLE (AppUserId NVARCHAR (MAX));
	INSERT INTO @appUserIdTable (AppUserId)
	SELECT value FROM STRING_SPLIT(@appUserIds, ',');
	SELECT * 
	FROM UserPost
	WHERE AppUserId IN (SELECT AppUserId FROM @appUserIdTable)
	ORDER BY Id DESC
	OFFSET @offset ROWS
	FETCH NEXT @pageSize ROWS ONLY
END
GO

CREATE PROCEDURE GetNewUserPostByListOfAppUserId (@appUserIds NVARCHAR (MAX), @checkFrom DATETIME)
AS
BEGIN
	DECLARE @appUserIdTable TABLE (AppUserId NVARCHAR (MAX));
	INSERT INTO @appUserIdTable (AppUserId)
	SELECT value FROM STRING_SPLIT(@appUserIds, ',');
	SELECT * 
	FROM UserPost
	WHERE AppUserId IN (SELECT AppUserId FROM @appUserIdTable) AND CreatedAt > @checkFrom
	ORDER BY CreatedAt DESC
END
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250122125652_InitialCreate', N'9.0.1');

COMMIT;
GO

