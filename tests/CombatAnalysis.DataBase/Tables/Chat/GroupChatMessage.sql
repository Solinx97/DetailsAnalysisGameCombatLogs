CREATE TABLE [dbo].[GroupChatMessage] (
    [Id]          INT            IDENTITY (1, 1) NOT NULL,
    [Username]    NVARCHAR (MAX) NULL,
    [Message]     NVARCHAR (MAX) NULL,
    [Time]        TIME (7)       NOT NULL,
    [GroupChatId] INT            NOT NULL,
    CONSTRAINT [PK_GroupChatMessage] PRIMARY KEY CLUSTERED ([Id] ASC)
);

