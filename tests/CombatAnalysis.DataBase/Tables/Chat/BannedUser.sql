CREATE TABLE [dbo].[BannedUser] (
    [Id]           INT            IDENTITY (1, 1) NOT NULL,
    [WhomBannedId] NVARCHAR (MAX) NULL,
    [BannedUserId] NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_BannedUser] PRIMARY KEY CLUSTERED ([Id] ASC)
);