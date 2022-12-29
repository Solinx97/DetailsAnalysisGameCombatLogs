CREATE TABLE [dbo].[ResourceRecoveryGeneral] (
    [Id]                INT            IDENTITY (1, 1) NOT NULL,
    [Value]             INT            NOT NULL,
    [ResourcePerSecond] FLOAT (53)     NOT NULL,
    [SpellOrItem]       NVARCHAR (MAX) NULL,
    [CastNumber]        INT            NOT NULL,
    [MinValue]          INT            NOT NULL,
    [MaxValue]          INT            NOT NULL,
    [AverageValue]      FLOAT (53)     NOT NULL,
    [CombatPlayerId]    INT            NOT NULL,
    CONSTRAINT [PK_ResourceRecoveryGeneral] PRIMARY KEY CLUSTERED ([Id] ASC)
);

