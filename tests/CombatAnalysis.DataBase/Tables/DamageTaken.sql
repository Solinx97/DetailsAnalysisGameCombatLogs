CREATE TABLE [dbo].[DamageTaken] (
    [Id]             INT            IDENTITY (1, 1) NOT NULL,
    [Value]          INT            NOT NULL,
    [Time]           NVARCHAR (MAX) NULL,
    [FromEnemy]      NVARCHAR (MAX) NULL,
    [ToPlayer]       NVARCHAR (MAX) NULL,
    [SpellOrItem]    NVARCHAR (MAX) NULL,
    [Resist]         INT            NOT NULL,
    [Absorb]         INT            NOT NULL,
    [Blocked]        INT            NOT NULL,
    [RealDamage]     INT            NOT NULL,
    [Mitigated]      INT            NOT NULL,
    [IsDodge]        BIT            NOT NULL,
    [IsParry]        BIT            NOT NULL,
    [IsMiss]         BIT            NOT NULL,
    [IsResist]       BIT            NOT NULL,
    [IsImmune]       BIT            NOT NULL,
    [IsCrushing]     BIT            NOT NULL,
    [CombatPlayerId] INT            NOT NULL,
    CONSTRAINT [PK_DamageTaken] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
CREATE NONCLUSTERED INDEX [IX_DamageTaken_CombatPlayerId]
    ON [dbo].[DamageTaken]([CombatPlayerId] ASC);

