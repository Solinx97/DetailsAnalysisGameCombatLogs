CREATE TABLE [dbo].[PlayerParseInfo] (
    [Id]               INT IDENTITY (1, 1) NOT NULL,
    [SpecId]           INT NOT NULL,
    [ClassId]          INT NOT NULL,
    [BossId]           INT NOT NULL,
    [Difficult]        INT NOT NULL,
    [DamageEfficiency] INT NOT NULL,
    [HealEfficiency]   INT NOT NULL,
    [CombatPlayerId]   INT NOT NULL,
    CONSTRAINT [PK_PlayerParseInfo] PRIMARY KEY CLUSTERED ([Id] ASC)
);

