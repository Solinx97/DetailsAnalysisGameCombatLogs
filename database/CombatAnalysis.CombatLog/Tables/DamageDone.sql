CREATE TABLE [dbo].[DamageDone] (
    [Id]               INT            IDENTITY (1, 1) NOT NULL,
    [Value]            INT            NOT NULL,
    [Time]             NVARCHAR (MAX) NOT NULL,
    [FromPlayer]       NVARCHAR (MAX) NOT NULL,
    [ToEnemy]          NVARCHAR (MAX) NOT NULL,
    [SpellOrItem]      NVARCHAR (MAX) NOT NULL,
    [IsPeriodicDamage] BIT            NOT NULL,
    [IsDodge]          BIT            NOT NULL,
    [IsParry]          BIT            NOT NULL,
    [IsMiss]           BIT            NOT NULL,
    [IsResist]         BIT            NOT NULL,
    [IsImmune]         BIT            NOT NULL,
    [IsCrit]           BIT            NOT NULL,
    [IsPet]            BIT            NOT NULL,
    [CombatPlayerId]   INT            NOT NULL,
    CONSTRAINT [PK_DamageDone] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
CREATE NONCLUSTERED INDEX [IX_DamageDone_CombatPlayerId]
    ON [dbo].[DamageDone]([CombatPlayerId] ASC);

