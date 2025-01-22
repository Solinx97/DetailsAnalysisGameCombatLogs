CREATE TABLE [dbo].[ResourceRecovery] (
    [Id]             INT            IDENTITY (1, 1) NOT NULL,
    [Value]          INT            NOT NULL,
    [Time]           NVARCHAR (MAX) NOT NULL,
    [SpellOrItem]    NVARCHAR (MAX) NOT NULL,
    [CombatPlayerId] INT            NOT NULL,
    CONSTRAINT [PK_ResourceRecovery] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
CREATE NONCLUSTERED INDEX [IX_ResourceRecovery_CombatPlayerId]
    ON [dbo].[ResourceRecovery]([CombatPlayerId] ASC);

