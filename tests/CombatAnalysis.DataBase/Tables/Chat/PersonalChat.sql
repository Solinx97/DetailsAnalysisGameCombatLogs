CREATE TABLE [dbo].[PersonalChat] (
    [Id]                INT            IDENTITY (1, 1) NOT NULL,
    [InitiatorUsername] NVARCHAR (MAX) NULL,
    [CompanionUsername] NVARCHAR (MAX) NULL,
    [LastMessage]       NVARCHAR (MAX) NULL,
    [InitiatorId]       NVARCHAR (MAX) NULL,
    [CompanionId]       NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_PersonalChat] PRIMARY KEY CLUSTERED ([Id] ASC)
);

