CREATE TABLE [dbo].[DamageTakenGeneral] (
    [Id]                   INT            IDENTITY (1, 1) NOT NULL,
    [Value]                INT            NOT NULL,
    [ActualValue]          INT            NOT NULL,
    [DamageTakenPerSecond] FLOAT (53)     NOT NULL,
    [SpellOrItem]          NVARCHAR (MAX) NOT NULL,
    [CritNumber]           INT            NOT NULL,
    [MissNumber]           INT            NOT NULL,
    [CastNumber]           INT            NOT NULL,
    [MinValue]             INT            NOT NULL,
    [MaxValue]             INT            NOT NULL,
    [AverageValue]         FLOAT (53)     NOT NULL,
    [CombatPlayerId]       INT            NOT NULL,
    CONSTRAINT [PK_DamageTakenGeneral] PRIMARY KEY CLUSTERED ([Id] ASC)
);

