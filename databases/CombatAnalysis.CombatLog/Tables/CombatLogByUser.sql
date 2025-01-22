CREATE TABLE [dbo].[CombatLogByUser] (
    [Id]              INT            IDENTITY (1, 1) NOT NULL,
    [PersonalLogType]     INT            NOT NULL,
    [NumberReadyCombats]          INT NOT NULL,
    [CombatsInQueue] INT            NOT NULL,
    [IsReady] BIT NOT NULL, 
    [CombatLogId] INT NOT NULL, 
    [AppUserId] NVARCHAR(50) NOT NULL, 
    CONSTRAINT [PK_CombatLogByUser] PRIMARY KEY CLUSTERED ([Id] ASC)
);
