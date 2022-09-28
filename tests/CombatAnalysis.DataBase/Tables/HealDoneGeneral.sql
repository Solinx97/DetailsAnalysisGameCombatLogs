CREATE TABLE [dbo].[HealDoneGeneral] (
    [Id]             INT            IDENTITY (1, 1) NOT NULL,
    [Value]          INT            NOT NULL,
    [HealPerSecond]  FLOAT (53)     NOT NULL,
    [SpellOrItem]    NVARCHAR (MAX) NULL,
    [CritNumber]     INT            NOT NULL,
    [CastNumber]     INT            NOT NULL,
    [MinValue]       INT            NOT NULL,
    [MaxValue]       INT            NOT NULL,
    [AverageValue]   FLOAT (53)     NOT NULL,
    [CombatPlayerId] INT            NOT NULL,
    CONSTRAINT [PK_HealDoneGeneral] PRIMARY KEY CLUSTERED ([Id] ASC)
);

