CREATE TABLE [dbo].[PersonalChatMessage] (
    [Id]             INT            IDENTITY (1, 1) NOT NULL,
    [Message]        NVARCHAR (MAX) NULL,
    [Time]           TIME (7)       NOT NULL,
    [PersonalChatId] INT            NOT NULL,
    [OwnerId]        NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_PersonalChatMessage] PRIMARY KEY CLUSTERED ([Id] ASC)
);

