CREATE TABLE [dbo].[GroupChatUser] (
    [Id]          INT            IDENTITY (1, 1) NOT NULL,
    [UserId]      NVARCHAR (MAX) NULL,
    [GroupChatId] INT            NOT NULL,
    CONSTRAINT [PK_GroupChatUser] PRIMARY KEY CLUSTERED ([Id] ASC)
);