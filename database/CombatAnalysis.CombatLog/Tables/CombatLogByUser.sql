CREATE TABLE [dbo].[CombatLogByUser] (
    [Id]              INT            IDENTITY (1, 1) NOT NULL,
    [CombatLogId]     INT            NOT NULL,
    [UserId]          NVARCHAR (MAX) NOT NULL,
    [PersonalLogType] INT            NOT NULL,
    CONSTRAINT [PK_CombatLogByUser] PRIMARY KEY CLUSTERED ([Id] ASC)
);
