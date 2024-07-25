CREATE TABLE [dbo].[GroupChatMessage] (
    [Id]          INT            IDENTITY (1, 1) NOT NULL,
    [Message]     NVARCHAR (MAX) NULL,
    [Time]        TIME (7)       NOT NULL,
    [GroupChatId] INT            NOT NULL,
    [OwnerId]     NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_GroupChatMessage] PRIMARY KEY CLUSTERED ([Id] ASC)
);

