CREATE TABLE [dbo].[HealDone] (
    [Id]                INT            IDENTITY (1, 1) NOT NULL,
    [ValueWithOverheal] INT            NOT NULL,
    [Time]              NVARCHAR (MAX) NOT NULL,
    [Overheal]          INT            NOT NULL,
    [Value]             INT            NOT NULL,
    [FromPlayer]        NVARCHAR (MAX) NOT NULL,
    [ToPlayer]          NVARCHAR (MAX) NOT NULL,
    [SpellOrItem]       NVARCHAR (MAX) NOT NULL,
    [DamageAbsorbed]    NVARCHAR (MAX) NOT NULL,
    [IsCrit]            BIT            NOT NULL,
    [IsFullOverheal]    BIT            NOT NULL,
    [IsAbsorbed]        BIT            NOT NULL,
    [CombatPlayerId]    INT            NOT NULL,
    CONSTRAINT [PK_HealDone] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
CREATE NONCLUSTERED INDEX [IX_HealDone_CombatPlayerId]
    ON [dbo].[HealDone]([CombatPlayerId] ASC);

