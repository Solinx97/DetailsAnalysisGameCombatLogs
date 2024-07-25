CREATE TABLE [dbo].[CombatPlayer] (
    [Id]               INT            IDENTITY (1, 1) NOT NULL,
    [Username]         NVARCHAR (MAX) NOT NULL,
    [PlayerId]         NVARCHAR (MAX) NOT NULL,
    [AverageItemLevel] FLOAT (53)     NOT NULL,
    [EnergyRecovery]   INT            NOT NULL,
    [DamageDone]       INT            NOT NULL,
    [HealDone]         INT            NOT NULL,
    [DamageTaken]      INT            NOT NULL,
    [UsedBuffs]        INT            NOT NULL,
    [CombatId]         INT            NOT NULL,
    CONSTRAINT [PK_CombatPlayer] PRIMARY KEY CLUSTERED ([Id] ASC)
);

