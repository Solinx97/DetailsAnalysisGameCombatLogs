CREATE TABLE [dbo].[HealDone] (
    [Id]                INT            IDENTITY (1, 1) NOT NULL,
    [ValueWithOverheal] INT            NOT NULL,
    [Time]              NVARCHAR (MAX) NULL,
    [Overheal]          INT            NOT NULL,
    [Value]             INT            NOT NULL,
    [FromPlayer]        NVARCHAR (MAX) NULL,
    [ToPlayer]          NVARCHAR (MAX) NULL,
    [SpellOrItem]       NVARCHAR (MAX) NULL,
    [IsCrit]            BIT            NOT NULL,
    [IsFullOverheal]    BIT            NOT NULL,
    [CombatPlayerId]    INT            NOT NULL,
    CONSTRAINT [PK_HealDone] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
CREATE NONCLUSTERED INDEX [IX_HealDone_CombatPlayerId]
    ON [dbo].[HealDone]([CombatPlayerId] ASC);

