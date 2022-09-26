CREATE TABLE [dbo].[PersonalChatMessage] (
    [Id]             INT            IDENTITY (1, 1) NOT NULL,
    [Username]       NVARCHAR (MAX) NULL,
    [Message]        NVARCHAR (MAX) NULL,
    [Time]           TIME (7)       NOT NULL,
    [PersonalChatId] INT            NOT NULL,
    CONSTRAINT [PK_PersonalChatMessage] PRIMARY KEY CLUSTERED ([Id] ASC)
);

